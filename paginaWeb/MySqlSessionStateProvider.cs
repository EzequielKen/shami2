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
            try
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SetAndReleaseItemExclusive: {ex.Message}");
                throw;
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
                return new SessionStateItemCollection();
            }
            catch (SerializationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Serialization Exception: {ex.Message}");
                return new SessionStateItemCollection();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return new SessionStateItemCollection();
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
            try
            {
                var bytes = Convert.FromBase64String(dataTableString);
                using (var memoryStream = new MemoryStream(bytes))
                {
                    var formatter = new BinaryFormatter();
                    return (DataTable)formatter.Deserialize(memoryStream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deserializing DataTable: {ex.Message}");
                throw;
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
            try
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ReleaseItemExclusive: {ex.Message}");
                throw;
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

            try
            {
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
                            DateTime expires = reader.GetDateTime("Expires");
                            if (expires < DateTime.Now)
                            {
                                locked = false;
                                actions = SessionStateActions.InitializeItem;
                                RemoveItem(context, id, lockId, sessionData);
                            }
                            else
                            {
                                string sessionItems = reader["SessionItems"] as string;
                                sessionData = new SessionStateStoreData(DeserializeSessionItems(sessionItems), SessionStateUtility.GetSessionStaticObjects(context), (int)reader["Timeout"]);
                                lockId = Guid.NewGuid(); // Genera un ID de bloqueo único para la sesión
                                locked = true; // Marca la sesión como bloqueada
                            }
                        }
                        else
                        {
                            actions = SessionStateActions.InitializeItem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetItemExclusive: {ex.Message}");
                throw;
            }

            return sessionData;
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM Sessions WHERE SessionId = @SessionId";
                    cmd.Parameters.AddWithValue("@SessionId", id);

                    int sessionCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (sessionCount == 0)
                    {
                        cmd.CommandText = @"
                            INSERT INTO Sessions (SessionId, Created, Expires, LockDate, LockDateLocal, LockOwner, Timeout, SessionItems, Flags)
                            VALUES (@SessionId, @Created, @Expires, @LockDate, @LockDateLocal, @LockOwner, @Timeout, @SessionItems, @Flags)";
                        DateTime fecha = DateTime.Now;
                        cmd.Parameters.AddWithValue("@Created", fecha);
                        cmd.Parameters.AddWithValue("@Expires", fecha.AddMinutes(timeout));
                        cmd.Parameters.AddWithValue("@LockDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@LockDateLocal", DBNull.Value);
                        cmd.Parameters.AddWithValue("@LockOwner", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Timeout", timeout);
                        cmd.Parameters.AddWithValue("@SessionItems", "{}"); // Representa un JSON vacío
                        cmd.Parameters.AddWithValue("@Flags", 1);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("Session already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateUninitializedItem: {ex.Message}");
                throw;
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
            try
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RemoveItem: {ex.Message}");
                throw;
            }
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            try
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ResetItemTimeout: {ex.Message}");
                throw;
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
