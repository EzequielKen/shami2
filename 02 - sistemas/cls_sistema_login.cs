using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using modulos;
namespace _02___sistemas
{
    public class cls_sistema_login
    {
        public cls_sistema_login()
        {
            ip = ConfigurationManager.AppSettings["ip"];
            puerto = ConfigurationManager.AppSettings["puerto"];
            user = ConfigurationManager.AppSettings["user"];
            userPasword = ConfigurationManager.AppSettings["userPasword"];

            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }

            consultas_Mysql = new cls_consultas_Mysql(ip, puerto, user, userPasword, base_de_datos);

        }
        #region atributos
        cls_consultas_Mysql consultas_Mysql;
        string ip, puerto, user, userPasword, base_de_datos;
        DataTable usuario;
        DataTable empleado;
        DataTable sucursal;
        DataTable tipo_usuario;
        DataTable proveedor;
        #endregion

        #region metodos
        public bool login(string usuario, string contraseña)
        {
            bool retorno = false;

            try
            {
                this.usuario = consultas_Mysql.login(usuario, contraseña);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            if (this.usuario.Rows.Count > 0)
            {
                if ((this.usuario.Rows[0]["usuario"].ToString() == usuario && this.usuario.Rows[0]["contraseña"].ToString() == contraseña))
                {
                    retorno = true;
                    int id = int.Parse(this.usuario.Rows[0]["sucursal"].ToString());
                    consultar_sucursal(id);
                    id = int.Parse(this.usuario.Rows[0]["tipo_usuario"].ToString());
                    consultar_tipo_usuario(id);
                }

            }
            return retorno;
        }
        public bool login_empleado(string usuario, string contraseña)
        {
            bool retorno = false;

            try
            {
                this.empleado = consultas_Mysql.login_empleado(usuario, contraseña);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            if (this.empleado.Rows.Count > 0)
            {

                retorno = true;
                int id_sucursal = int.Parse(this.empleado.Rows[0]["id_sucursal"].ToString());
                consultar_sucursal(id_sucursal);
                consultar_usuario_segun_sucursal(id_sucursal.ToString());
                string id_usuario = this.usuario.Rows[0]["id"].ToString();

            }
            return retorno;
        }
        #endregion

        #region metodos privados
        private void consultar_proveedor_id(string id)
        {
            proveedor = consultas_Mysql.consultar_proveeedor_id(id);
        }
        private void consultar_sucursal(int id)
        {
            sucursal = consultas_Mysql.consultar_sucursal(id);
        }
        private void consultar_tipo_usuario(int id)
        {
            tipo_usuario = consultas_Mysql.consultar_tipo_usuario(id);
        }
        private void consultar_usuario_segun_sucursal(string id_sucursal)
        {
            usuario = consultas_Mysql.consultar_usuario_segun_sucural(id_sucursal);
        }
        #endregion

        #region metodos get
        public DataTable get_proveedor_seleccionado(string id_proveedor)
        {
            consultar_proveedor_id(id_proveedor);
            return proveedor;
        }
        public DataTable get_usuarios()
        {
            return usuario;
        }
        public DataTable get_sucursal()
        {
            return sucursal;
        }
        public DataTable get_tipo_usuario()
        {
            return tipo_usuario;
        }
        public DataTable get_empleado()
        {
            return empleado;
        }
        #endregion
    }
}
