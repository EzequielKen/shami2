using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace paginaWeb
{
    public class proveedor_de_sesiones : SessionStateStoreProviderBase
    {
        private string connectionString;

        public proveedor_de_sesiones(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            throw new NotImplementedException();
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void EndRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            throw new NotImplementedException();
        }

        public void GuardarSesion(string sessionId, string sessionData)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO sesiones (session_id, session_data) VALUES (@sessionId, @sessionData)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sessionId", sessionId);
                    command.Parameters.AddWithValue("@sessionData", sessionData);

                    command.ExecuteNonQuery();
                }
            }
        }

        public override void InitializeRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public string RecuperarSesion(string sessionId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT session_data FROM sesiones WHERE session_id = @sessionId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sessionId", sessionId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString("session_data");
                        }
                    }
                }
            }

            return null;
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            throw new NotImplementedException();
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            throw new NotImplementedException();
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            throw new NotImplementedException();
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            throw new NotImplementedException();
        }
    }
}