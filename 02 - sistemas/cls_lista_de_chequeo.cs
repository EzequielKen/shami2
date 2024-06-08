using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_lista_de_chequeo
    {
        public cls_lista_de_chequeo(DataTable usuario_BD)
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

        DataTable lista_de_chequeo;
        DataTable configuracion_de_chequeo;
        DataTable resumen;
        DataTable historial;
        DataTable historial_resumen;
        #endregion

        #region carga a base de datos
        public void registrar_chequeo(DataTable empleado, string actividad)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas,"id_sucursal",false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id_sucursal"].ToString(),false);
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id"].ToString(), false);
            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["nombre"].ToString(), false);
            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["apellido"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //actividad
            columnas = funciones.armar_query_columna(columnas, "actividad", true);
            valores = funciones.armar_query_valores(valores, actividad, true);
            consultas.insertar_en_tabla(base_de_datos, "historial_lista_chequeo", columnas,valores);
        }
        #endregion
        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("area", typeof(string));
        }
        private void crear_tabla_historial_resumen()
        {
            historial_resumen = new DataTable();
            historial_resumen.Columns.Add("id", typeof(string));
            historial_resumen.Columns.Add("actividad", typeof(string));
           
        }
        private void llenar_resumen()
        {
            crear_tabla_resumen();
            string id, actividad, categoria, area;
            int ultima_fila;
            for (int columna = configuracion_de_chequeo.Columns["producto_1"].Ordinal; columna <= configuracion_de_chequeo.Columns.Count - 1; columna++)
            {
                if (configuracion_de_chequeo.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 1);
                    actividad = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 2);
                    categoria = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 3);
                    area = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 4);

                    resumen.Rows.Add();
                    ultima_fila = resumen.Rows.Count - 1;

                    resumen.Rows[ultima_fila]["id"] = id;
                    resumen.Rows[ultima_fila]["actividad"] = actividad;
                    resumen.Rows[ultima_fila]["categoria"] = categoria;
                    resumen.Rows[ultima_fila]["area"] = area;
                }
            }
        }
        private void llenar_historial_resumen()
        {
            crear_tabla_historial_resumen();
            string id, actividad;
            int ultima_fila;
            for (int fila =0; fila <= historial.Rows.Count - 1; fila++)
            {
                    id = funciones.obtener_dato(historial.Rows[fila]["actividad"].ToString(), 1);
                    actividad = funciones.obtener_dato(historial.Rows[fila]["actividad"].ToString(), 2);

                    historial_resumen.Rows.Add();
                    ultima_fila = historial_resumen.Rows.Count - 1;

                    historial_resumen.Rows[ultima_fila]["id"] = id;
                    historial_resumen.Rows[ultima_fila]["actividad"] = actividad;
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_historial(DateTime fecha,string id_empleado)
        {
            historial = consultas.consultar_historial_chequeo_segun_fecha(fecha.Year.ToString(),fecha.Month.ToString(),fecha.Day.ToString(), id_empleado);
        }
        private void consultar_configuracion_de_chequeo(string perfil)
        {
            configuracion_de_chequeo = consultas.consultar_configuracion_chequeo(perfil);
        }
        private void consultar_lista_de_chequeo()
        {
            lista_de_chequeo = consultas.consultar_tabla_completa(base_de_datos, "lista_de_chequeo");
        }
        #endregion

        #region metodos get/set
        public DataTable get_historial(DateTime fecha,string id_empleado)
        {
            consultar_historial(fecha, id_empleado);
            llenar_historial_resumen();
            return historial_resumen;
        }
        public string get_id_chequeo_activo(string perfil)
        {
            string retorno = "N/A";
            consultar_configuracion_de_chequeo(perfil);
            if (configuracion_de_chequeo.Rows.Count > 0)
            {
                retorno = configuracion_de_chequeo.Rows[0]["id"].ToString();
            }
            return retorno;
        }
        public DataTable get_configuracion_de_chequeo(string perfil)
        {
            consultar_configuracion_de_chequeo(perfil);
            llenar_resumen();
            return resumen;
        }
        public DataTable get_lista_de_chequeo()
        {
            consultar_lista_de_chequeo();
            return lista_de_chequeo;
        }
        #endregion
    }
}

