using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_crear_usuarios_sucursal
    {
        public cls_crear_usuarios_sucursal(DataTable usuario_BD)
        {
            usuarioBD = usuario_BD;
            servidor = usuarioBD.Rows[0]["servidor"].ToString();
            puerto = usuarioBD.Rows[0]["puerto"].ToString();
            usuario_dato = usuarioBD.Rows[0]["usuario_BD"].ToString();
            usuario_dato = usuarioBD.Rows[0]["usuario_BD"].ToString();
            contraseña_BD = usuarioBD.Rows[0]["contraseña_BD"].ToString();
            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }
            consultas = new cls_consultas_Mysql(servidor, puerto, usuario_dato, contraseña_BD, base_de_datos);
        }
        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable usuarios;
        #endregion

        #region carga a base de datos
        public void crear_usuario(DataTable usuario)
        {
            string columna = string.Empty;
            string valores = string.Empty;

            //usuario
            columna = funciones.armar_query_columna(columna, "usuario", false);
            valores = funciones.armar_query_valores(valores, usuario.Rows[0]["usuario"].ToString(), false);
            //contraseña
            columna = funciones.armar_query_columna(columna, "contraseña", false);
            valores = funciones.armar_query_valores(valores, usuario.Rows[0]["contraseña"].ToString(), false);
            //sucursal
            columna = funciones.armar_query_columna(columna, "sucursal", true);
            valores = funciones.armar_query_valores(valores, usuario.Rows[0]["id_sucursal_seleccionada"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos,"usuario",columna, valores);

        }
        public void actualizar_dato(string id, string columna, string dato)
        {
            string actualizar = "`" + columna + "` = '" + dato + "'";
            consultas.actualizar_tabla(base_de_datos, "usuario", actualizar, id);
        }
        #endregion

        #region metodos consultas
        private void consultar_usuarios(string id_sucursal)
        {
            usuarios = consultas.consultar_todos_usuario_segun_sucural(id_sucursal);
        }
        #endregion

        #region metodos get/set
        public DataTable get_usuarios(string id_sucursal)
        {
            consultar_usuarios(id_sucursal);
            return usuarios;
        }
        #endregion
    }
}
