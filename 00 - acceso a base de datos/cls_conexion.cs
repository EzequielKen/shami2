using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;


namespace acceso_a_base_de_datos
{
    public class cls_conexion
    {
        #region constructor
        public cls_conexion(string servidor, string puerto,string usuario,string password,string base_de_datos)
        {
         
            cadena_conexion = "server="+servidor+";"+"port="+puerto+";"+"user id="+usuario+";"+"password="+password+";"+"database="+base_de_datos+ ";CharSet=utf8;";

            conexion = new MySqlConnection(cadena_conexion);
        }
        #endregion 

        #region atributos
        MySqlConnection conexion;
        MySqlDataAdapter adapter;
        MySqlCommand comando;
        MySqlDataReader lector;

        private string servidor;
        private string base_de_datos;
        private string usuario;
        private string password;
        private string puerto;
        private string cadena_conexion;
        #endregion

        #region conexion
            private void abrir_conexion()
            {
                try
                {
                    conexion.Open();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            private void cerrar_conexion()
            {
                try
                {
                    conexion.Close();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        #endregion

        #region CRUD
            public bool CREATE(string query)
            {
                bool retorno = false;
                try
                {
                abrir_conexion();
                    comando = new MySqlCommand(query,conexion);
                    lector = comando.ExecuteReader();
                    retorno = true;
                }
                catch (Exception ex)
                {
                    retorno = false;
                    throw ex;
                }
                finally 
                { 
                     cerrar_conexion();
                }
                return retorno;
            }
            public DataTable READ(string query)
            {
                DataTable dataTable = new DataTable();
            
                try
                {
                abrir_conexion();
                    adapter = new MySqlDataAdapter(query, conexion);
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                { 
                    cerrar_conexion();
                
                }
                return dataTable;
            }
        
            public bool UPDATE(string query)
            {
                bool retorno = false;
                try
                {
                abrir_conexion();
                    comando = new MySqlCommand(query, conexion);
                    lector = comando.ExecuteReader();
                    retorno = true;
                }
                catch (Exception ex)
                {
                    retorno=false;
                    throw ex;
                }
                finally 
                {
                    cerrar_conexion();
                }
                return retorno;
            }


        #endregion

        #region execute
        public bool EXECUTE(string query)
        {
            bool retorno = false;
            try
            {
                abrir_conexion(); // Abre la conexión a la base de datos
                comando = new MySqlCommand(query, conexion);
                comando.ExecuteNonQuery(); // Ejecuta la consulta sin esperar un resultado
                retorno = true; // Indica que la ejecución fue exitosa
            }
            catch (Exception ex)
            {
                retorno = false; // Indica que ocurrió un error
                throw new Exception($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
            finally
            {
                cerrar_conexion(); // Cierra la conexión en cualquier caso
            }
            return retorno;
        }

        #endregion
    }
}
