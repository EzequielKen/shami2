using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.SessionState;
using MySql.Data.MySqlClient;

namespace paginaWeb
{
    [Serializable]
    public class MySqlSessionStateProvider : SessionStateStoreProviderBase
    {
        private string connectionString;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            base.Initialize(name, config);

            connectionString = ConfigurationManager.ConnectionStrings["MySqlSessionState"].ConnectionString;
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO sessions (SessionId, ApplicationName, Created, Expires, LockDate, LockId, Timeout, Locked, SessionItems, Flags) VALUES (@SessionId, @ApplicationName, @Created, @Expires, @LockDate, @LockId, @Timeout, @Locked, @SessionItems, @Flags)", connection);
                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Expires", DateTime.Now.AddMinutes(timeout));
                command.Parameters.AddWithValue("@LockDate", DateTime.Now);
                command.Parameters.AddWithValue("@LockId", 0); // Asegurando que LockId tenga un valor inicial
                command.Parameters.AddWithValue("@Timeout", timeout);
                command.Parameters.AddWithValue("@Locked", false);
                command.Parameters.AddWithValue("@SessionItems", DBNull.Value);
                command.Parameters.AddWithValue("@Flags", 0);
                command.ExecuteNonQuery();
            }
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionStoreItem(false, context, id, out locked, out lockAge, out lockId, out actions);
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetSessionStoreItem(true, context, id, out locked, out lockAge, out lockId, out actions);
        }

        private SessionStateStoreData GetSessionStoreItem(bool lockRecord, HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            locked = false;
            lockAge = TimeSpan.Zero;
            lockId = null;
            actions = SessionStateActions.None;
            SessionStateStoreData item = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT Expires, SessionItems, LockId, Locked, Flags, Timeout, LockDate FROM sessions WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName", connection);
                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime expires = reader.GetDateTime(0);

                        if (expires < DateTime.Now)
                        {
                            locked = false;
                            return null;
                        }

                        string sessionItems = reader.IsDBNull(1) ? null : reader.GetString(1);
                        lockId = reader.GetInt32(2);
                        locked = reader.GetBoolean(3);
                        actions = (SessionStateActions)reader.GetInt32(4);
                        int timeout = reader.GetInt32(5);
                        DateTime lockDate = reader.GetDateTime(6);

                        lockAge = DateTime.Now.Subtract(lockDate);

                        if (locked && !lockRecord)
                        {
                            return null;
                        }

                        if (lockRecord)
                        {
                            locked = true;
                            lockId = (int)lockId + 1; // Incrementando LockId

                            reader.Close(); // Cerrando el DataReader antes de ejecutar la actualización

                            var updateCommand = new MySqlCommand("UPDATE sessions SET Locked = @Locked, LockId = @LockId, LockDate = @LockDate WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName", connection);
                            updateCommand.Parameters.AddWithValue("@Locked", true);
                            updateCommand.Parameters.AddWithValue("@LockId", lockId);
                            updateCommand.Parameters.AddWithValue("@LockDate", DateTime.Now);
                            updateCommand.Parameters.AddWithValue("@SessionId", id);
                            updateCommand.Parameters.AddWithValue("@ApplicationName", "/");
                            updateCommand.ExecuteNonQuery();
                        }

                        if (!string.IsNullOrEmpty(sessionItems))
                        {
                            item = new SessionStateStoreData(Deserialize(sessionItems),
                                SessionStateUtility.GetSessionStaticObjects(context), timeout);
                        }
                        else
                        {
                            item = CreateNewStoreData(context, timeout);
                        }
                    }
                    reader.Close(); // Cerrando el DataReader al final de su uso
                }
            }

            return item;
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE sessions SET Locked = @Locked WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName AND LockId = @LockId", connection);
                command.Parameters.AddWithValue("@Locked", false);
                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");
                command.Parameters.AddWithValue("@LockId", lockId);
                command.ExecuteNonQuery();
            }
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand(newItem ?
                    "INSERT INTO sessions (SessionId, ApplicationName, Created, Expires, LockDate, LockId, Timeout, Locked, SessionItems, Flags) VALUES (@SessionId, @ApplicationName, @Created, @Expires, @LockDate, @LockId, @Timeout, @Locked, @SessionItems, @Flags)" :
                    "UPDATE sessions SET Expires = @Expires, SessionItems = @SessionItems, Locked = @Locked, LockId = @LockId WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName AND LockId = @LockId", connection);

                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");
                command.Parameters.AddWithValue("@Created", DateTime.Now); // Añadido @Created
                command.Parameters.AddWithValue("@Expires", DateTime.Now.AddMinutes((double)item.Timeout));
                command.Parameters.AddWithValue("@LockDate", DateTime.Now);
                command.Parameters.AddWithValue("@LockId", lockId ?? 0); // Asegurando que LockId no sea nulo
                command.Parameters.AddWithValue("@Timeout", item.Timeout);
                command.Parameters.AddWithValue("@Locked", false);
                command.Parameters.AddWithValue("@SessionItems", Serialize((SessionStateItemCollection)item.Items));
                command.Parameters.AddWithValue("@Flags", 0);
                command.ExecuteNonQuery();
            }
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM sessions WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName AND LockId = @LockId", connection);
                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");
                command.Parameters.AddWithValue("@LockId", lockId);
                command.ExecuteNonQuery();
            }
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE sessions SET Expires = @Expires WHERE SessionId = @SessionId AND ApplicationName = @ApplicationName", connection);
                command.Parameters.AddWithValue("@Expires", DateTime.Now.AddMinutes(20));
                command.Parameters.AddWithValue("@SessionId", id);
                command.Parameters.AddWithValue("@ApplicationName", "/");
                command.ExecuteNonQuery();
            }
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);
        }

        public override void EndRequest(HttpContext context)
        {
            // No implementation required
        }

        public override void Dispose()
        {
            // No resources to dispose
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            // Expiration callbacks are not supported
            return false;
        }

        public override void InitializeRequest(HttpContext context)
        {
            // No implementation required for this example
        }

        private string Serialize(SessionStateItemCollection items)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                if (items != null)
                    items.Serialize(writer);

                writer.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private SessionStateItemCollection Deserialize(string serializedItems)
        {
            using (var ms = new MemoryStream(Convert.FromBase64String(serializedItems)))
            using (var reader = new BinaryReader(ms))
            {
                return SessionStateItemCollection.Deserialize(reader);
            }
        }
    }
}
