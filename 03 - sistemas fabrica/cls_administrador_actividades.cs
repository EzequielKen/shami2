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
    [Serializable]
    public class cls_administrador_actividades
    {
        public cls_administrador_actividades(DataTable usuario_BD)
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
        #endregion
        #region carga a base de datos
        public void registrar_chequeo(DataTable resumen, string perfil)
        {
            desactivar_configuracion_activa(get_id_chequeo_activo(perfil));
            string columna = "";
            string valores = "";

            //perfil
            columna = funciones.armar_query_columna(columna, "perfil", false);
            valores = funciones.armar_query_valores(valores, perfil, false);

            string id, actividad, categoria, area, dato;
            int index = 1;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                actividad = resumen.Rows[fila]["actividad"].ToString();
                categoria = resumen.Rows[fila]["categoria"].ToString();
                area = resumen.Rows[fila]["area"].ToString();

                dato = id + "-" + actividad + "-" + categoria + "-" + area;

                columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);
                index++;
            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            actividad = resumen.Rows[ultima_fila]["actividad"].ToString();
            categoria = resumen.Rows[ultima_fila]["categoria"].ToString();
            area = resumen.Rows[ultima_fila]["area"].ToString();

            dato = id + "-" + actividad + "-" + categoria + "-" + area;

            columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);
            consultas.insertar_en_tabla(base_de_datos, "configuracion_de_chequeo", columna, valores);

        }
        private void desactivar_configuracion_activa(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "configuracion_de_chequeo", actualizar, id);
        }
        public void cambiar_puntaje(string id, string punto)
        {
            string actualizar = "`punto` = '" + punto + "'";
            consultas.actualizar_tabla(base_de_datos, "lista_de_chequeo", actualizar, id);
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
        private void llenar_resumen()
        {
            crear_tabla_resumen();
            string id, actividad, categoria, area;
            int ultima_fila;
            if (configuracion_de_chequeo.Rows.Count > 0)
            {

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
        }
        #endregion
        #region metodos consultas
        private void consultar_configuracion_de_chequeo(string perfil)
        {
            configuracion_de_chequeo = consultas.consultar_configuracion_chequeo(perfil);
        }
        private void consultar_lista_de_chequeo()
        {
            lista_de_chequeo = consultas.consultar_tabla(base_de_datos, "lista_de_chequeo");
        }
        #endregion

        #region metodos get/set
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
