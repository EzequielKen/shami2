using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace paginaWeb
{
    [Serializable]
    public class SessionStateItemCollectionWrapper
    {
        public Dictionary<string, object> Items { get; set; }

        public SessionStateItemCollectionWrapper()
        {
            Items = new Dictionary<string, object>();
        }

        public SessionStateItemCollectionWrapper(SessionStateItemCollection sessionItems)
        {
            Items = new Dictionary<string, object>();
            foreach (string key in sessionItems.Keys)
            {
                Items[key] = sessionItems[key];
            }
        }

        public SessionStateItemCollection ToSessionStateItemCollection()
        {
            var sessionItems = new SessionStateItemCollection();
            foreach (var key in Items.Keys)
            {
                sessionItems[key] = Items[key];
            }
            return sessionItems;
        }
    }

    public class MySqlSessionStateProvider : SessionStateStoreProviderBase
    {
        private string connectionString;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            connectionString = config["connectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ConfigurationErrorsException("Connection string not provided.");
            }
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Sessions (SessionId, Created, Expires, LockDate, LockDateLocal, LockOwner, Timeout, SessionItems, Flags)
                    VALUES (@SessionId, @Created, @Expires, @LockDate, @LockDateLocal, @LockOwner, @Timeout, @SessionItems, @Flags)
                    ON DUPLICATE KEY UPDATE 
                    Created = VALUES(Created),
                    Expires = VALUES(Expires),
                    LockDate = VALUES(LockDate),
                    LockDateLocal = VALUES(LockDateLocal),
                    LockOwner = VALUES(LockOwner),
                    Timeout = VALUES(Timeout),
                    SessionItems = VALUES(SessionItems),
                    Flags = VALUES(Flags)";
                DateTime fecha = DateTime.Now;
                cmd.Parameters.AddWithValue("@SessionId", id);
                cmd.Parameters.AddWithValue("@Created", fecha);
                cmd.Parameters.AddWithValue("@Expires", fecha.AddMinutes(item.Timeout));
                cmd.Parameters.AddWithValue("@LockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@LockDateLocal", DBNull.Value);
                cmd.Parameters.AddWithValue("@LockOwner", DBNull.Value);
                cmd.Parameters.AddWithValue("@Timeout", item.Timeout);
                string serializedItems = SerializeSessionItems((SessionStateItemCollection)item.Items);
                System.Diagnostics.Debug.WriteLine($"Serialized items for session {id}: {serializedItems}");
                cmd.Parameters.AddWithValue("@SessionItems", serializedItems);
                cmd.Parameters.AddWithValue("@Flags", 0);

                cmd.ExecuteNonQuery();
            }
        }

        private string SerializeSessionItems(SessionStateItemCollection items)
        {
            var wrapper = new SessionStateItemCollectionWrapper(items);
            foreach (var key in wrapper.Items.Keys.ToList())
            {
                if (wrapper.Items[key] is DataTable dataTable)
                {
                    wrapper.Items[key] = SerializeDataTable(dataTable);
                }
            }
            return JsonConvert.SerializeObject(wrapper);
        }

        private SessionStateItemCollection DeserializeSessionItems(string sessionItems)
        {
            if (string.IsNullOrWhiteSpace(sessionItems))
            {
                return new SessionStateItemCollection();
            }

            System.Diagnostics.Debug.WriteLine($"Deserializing session items: {sessionItems}");
            try
            {
                var wrapper = JsonConvert.DeserializeObject<SessionStateItemCollectionWrapper>(sessionItems);
                foreach (var key in wrapper.Items.Keys.ToList())
                {
                    if (wrapper.Items[key] is string dataTableString && IsBase64String(dataTableString))
                    {
                        wrapper.Items[key] = DeserializeDataTable(dataTableString);
                    }
                }
                return wrapper.ToSessionStateItemCollection();
            }
            catch (JsonSerializationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Serialization Exception: {ex.Message}");
                var itemList = JsonConvert.DeserializeObject<List<object>>(sessionItems);
                var sessionItemsCollection = new SessionStateItemCollection();
                for (int i = 0; i < itemList.Count; i++)
                {
                    sessionItemsCollection[i.ToString()] = itemList[i];
                }
                return sessionItemsCollection;
            }
            catch (Exception ex)
            {
                // Log the exception and handle it appropriately
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                throw new SerializationException("Error deserializing session items", ex);
            }
        }

        private string SerializeDataTable(DataTable dataTable)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, dataTable);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private DataTable DeserializeDataTable(string dataTableString)
        {
            var bytes = Convert.FromBase64String(dataTableString);
            using (var memoryStream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                return (DataTable)formatter.Deserialize(memoryStream);
            }
        }

        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return false;
            base64 = base64.Trim();
            return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Sessions SET LockOwner = NULL, LockDate = NULL WHERE SessionId = @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", id);
                cmd.ExecuteNonQuery();
            }
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetItemExclusive(context, id, out locked, out lockAge, out lockId, out actions);
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            SessionStateStoreData sessionData = null;
            locked = false;
            lockAge = TimeSpan.Zero;
            lockId = null;
            actions = SessionStateActions.None;

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Sessions WHERE SessionId = @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var timeout = (int)reader["Timeout"];
                        var sessionItems = reader["SessionItems"] as string;

                        System.Diagnostics.Debug.WriteLine($"Session items from DB for session {id}: {sessionItems}");

                        var items = sessionItems != null ? DeserializeSessionItems(sessionItems) : new SessionStateItemCollection();

                        sessionData = new SessionStateStoreData(items, SessionStateUtility.GetSessionStaticObjects(context), timeout);
                        lockId = Guid.NewGuid(); // Genera un ID de bloqueo único para la sesión
                        locked = true; // Marca la sesión como bloqueada
                    }
                }
            }

            return sessionData;
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Sessions (SessionId, Created, Expires, LockDate, LockDateLocal, LockOwner, Timeout, SessionItems, Flags)
                    VALUES (@SessionId, @Created, @Expires, @LockDate, @LockDateLocal, @LockOwner, @Timeout, @SessionItems, @Flags)";

                cmd.Parameters.AddWithValue("@SessionId", id);
                cmd.Parameters.AddWithValue("@Created", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@Expires", DateTime.UtcNow.AddMinutes(timeout));
                cmd.Parameters.AddWithValue("@LockDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@LockDateLocal", DBNull.Value);
                cmd.Parameters.AddWithValue("@LockOwner", DBNull.Value);
                cmd.Parameters.AddWithValue("@Timeout", timeout);
                cmd.Parameters.AddWithValue("@SessionItems", "");
                cmd.Parameters.AddWithValue("@Flags", 1);

                cmd.ExecuteNonQuery();
            }
        }

        public override void EndRequest(HttpContext context)
        {
        }

        public override void InitializeRequest(HttpContext context)
        {
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Sessions WHERE SessionId = @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", id);
                cmd.ExecuteNonQuery();
            }
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Sessions SET Expires = @Expires WHERE SessionId = @SessionId";
                cmd.Parameters.AddWithValue("@SessionId", id);
                cmd.Parameters.AddWithValue("@Expires", DateTime.UtcNow.AddMinutes(20)); // Tiempo de expiración
                cmd.ExecuteNonQuery();
            }
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);
        }

        public override void Dispose()
        {
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }
    }
}