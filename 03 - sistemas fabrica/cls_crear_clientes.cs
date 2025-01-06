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
    public class cls_crear_clientes
    {
        public cls_crear_clientes(DataTable usuario_BD)
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

        DataTable sucursales;
        #endregion

        #region carga a base de datos
        public void crear_sucursal(DataTable sucursal)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal",false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(),false);
            //provincia
            columnas = funciones.armar_query_columna(columnas, "provincia", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["provincia"].ToString(), false);
            //localidad
            columnas = funciones.armar_query_columna(columnas, "localidad", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["localidad"].ToString(), false);
            //direccion
            columnas = funciones.armar_query_columna(columnas, "direccion", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["direccion"].ToString(), false);
            //telefono
            columnas = funciones.armar_query_columna(columnas, "telefono", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["telefono"].ToString(), false);
            //franquicia
            columnas = funciones.armar_query_columna(columnas, "franquicia", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["franquicia"].ToString(), false);
            //ultimo_pedido_enviado
            columnas = funciones.armar_query_columna(columnas, "ultimo_pedido_enviado", true);
            valores = funciones.armar_query_valores(valores, "0", true);

            consultas.insertar_en_tabla(base_de_datos,"sucursal",columnas, valores);

            crear_tipo_acuerdo(sucursal.Rows[0]["sucursal"].ToString());

            consultas.agregar_columna_tipo_acuerdo_fabrica(base_de_datos, "tipo_acuerdo_fabrica_a_marca", sucursal.Rows[0]["sucursal"].ToString());
        }
        private void crear_tipo_acuerdo(string sucursal)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", true);
            valores = funciones.armar_query_valores(valores, sucursal, true);

            consultas.insertar_en_tabla(base_de_datos, "tipo_acuerdo", columnas, valores);

        }
        public void actualizar_dato(string id, string columna, string dato)
        {
            string actualizar = "`" + columna + "` = '" + dato + "'";
            consultas.actualizar_tabla(base_de_datos, "sucursal", actualizar, id);
        }
        #endregion

        #region metodos consultas
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla_completa(base_de_datos,"sucursal");
        }
        #endregion

        #region metodos get/set
        public  DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        #endregion
    }
}
