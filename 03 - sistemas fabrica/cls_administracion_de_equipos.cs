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
    public class cls_administracion_de_equipos
    {
        public cls_administracion_de_equipos(DataTable usuario_BD)
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
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;
        
        DataTable ubicaciones;
        DataTable equipos;
        #endregion
        #region carga a base de datos
        public void modificar_equipo_observaciones(string id, string observaciones)
        {
            string actualizar = "";
            //nombre
            actualizar = "`observaciones` = '" + observaciones + "'";
            consultas.actualizar_tabla(base_de_datos, "equipos", actualizar, id);
        }
        public void modificar_equipo_temperatura(string id, string temperatura)
        {
            string actualizar = "";
            //nombre
            actualizar = "`temperatura` = '" + temperatura + "'";
            consultas.actualizar_tabla(base_de_datos, "equipos", actualizar, id);
        }
        public void modificar_equipo_nombre(string id, string nombre)
        {
            string actualizar="";
            //nombre
            actualizar = "`nombre` = '"+ nombre + "'";
            consultas.actualizar_tabla(base_de_datos, "equipos",actualizar,id);
        }
        public void modificar_equipo_categoria(string id, string categoria)
        {
            string actualizar = "";
            //categoria
            actualizar = "`categoria` = '" + categoria + "'";
            consultas.actualizar_tabla(base_de_datos, "equipos", actualizar, id);
        }
        public void modificar_equipo_ubicacion(string id, string ubicacion)
        {
            string actualizar = "";
            //ubicacion
            actualizar = "`ubicacion` = '" + ubicacion + "'";
            consultas.actualizar_tabla(base_de_datos, "equipos", actualizar, id);
        }
        public void cargar_equipo(string categoria, string ubicacion,string nombre,string temperatura, string observaciones)
        {
            string columna = "";
            string valores = "";
            //categoria
            columna = funciones.armar_query_columna(columna, "categoria", false);
            valores = funciones.armar_query_valores(valores, categoria, false);
            //ubicacion
            columna = funciones.armar_query_columna(columna, "ubicacion", false);
            valores = funciones.armar_query_valores(valores, ubicacion, false);
            //temperatura
            columna = funciones.armar_query_columna(columna, "temperatura", false);
            valores = funciones.armar_query_valores(valores, temperatura, false);
            //observaciones
            columna = funciones.armar_query_columna(columna, "observaciones", false);
            valores = funciones.armar_query_valores(valores, observaciones, false);
            //nombre
            columna = funciones.armar_query_columna(columna, "nombre", true);
            valores = funciones.armar_query_valores(valores, nombre, true);

            consultas.insertar_en_tabla(base_de_datos, "equipos", columna, valores);
        }
        public void cargar_ubicacion(string ubicacion)
        {
            string columna = "";
            string valores= "";

            columna = funciones.armar_query_columna(columna, "ubicacion",true);
            valores = funciones.armar_query_valores(valores, ubicacion,true);

            consultas.insertar_en_tabla(base_de_datos, "Ubicaciones_de_equipos",columna, valores);
        }
        #endregion
        #region metodos consultas
        private void consultar_ubicaciones()
        {
            ubicaciones = consultas.consultar_tabla(base_de_datos,"Ubicaciones_de_equipos");
        }
        private void consultar_equipos()
        {
            equipos = consultas.consultar_tabla(base_de_datos, "equipos");
        }
        #endregion
        #region metodos get/set
        public DataTable get_ubicaciones()
        {
            consultar_ubicaciones();
            return ubicaciones;
        }
        public DataTable get_equipos()
        {
            consultar_equipos();
            return equipos;
        }
        #endregion
    }
}
