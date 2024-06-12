using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_administrar_lista_chequeo_locales
    {
        public cls_administrar_lista_chequeo_locales(DataTable usuario_BD)
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
        public void actualizar_orden_actividad(string id, string campo, string dato)
        {
            string actualizar = "`" + campo + "` = '" + dato + "' ";
            consultas.actualizar_tabla(base_de_datos, "lista_de_chequeo", actualizar, id);
        }
        public void actualizar_dato_actividad(string id, string campo, string dato)
        {
            string actualizar = "`" + campo + "` = '" + dato + "' ";
            consultas.actualizar_tabla(base_de_datos, "lista_de_chequeo", actualizar, id);
        }
        public void crear_actividad(string actividad, string area, string categoria,string nuevo_orden)
        {
            string columnas = "";
            string valores = "";

            //orden
            columnas = funciones.armar_query_columna(columnas, "orden", false);
            valores = funciones.armar_query_valores(valores, nuevo_orden, false);
            //actividad
            columnas = funciones.armar_query_columna(columnas, "actividad", false);
            valores = funciones.armar_query_valores(valores, actividad, false);
            //categoria
            columnas = funciones.armar_query_columna(columnas, "area", false);
            valores = funciones.armar_query_valores(valores, area, false);
            //area
            columnas = funciones.armar_query_columna(columnas, "categoria", true);
            valores = funciones.armar_query_valores(valores, categoria, true);
            consultas.insertar_en_tabla(base_de_datos, "lista_de_chequeo", columnas, valores);
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
        private void reordenar_lista_chequeo(string id_actividad, string nuevo_orden)
        {
            //actualizar_orden_actividad(id_actividad,"orden",nuevo_orden);
            lista_de_chequeo.DefaultView.Sort = "area asc, categoria asc, orden asc";
            lista_de_chequeo = lista_de_chequeo.DefaultView.ToTable();
            int fila_actividad = funciones.buscar_fila_por_id(id_actividad, lista_de_chequeo);
            string categoria = lista_de_chequeo.Rows[fila_actividad]["categoria"].ToString();
            string area = lista_de_chequeo.Rows[fila_actividad]["area"].ToString();

            int orden, viejo_orden, orden_nuevo;
            string id;
            for (int fila = 0; fila <= lista_de_chequeo.Rows.Count - 1; fila++)
            {
                if (area == lista_de_chequeo.Rows[fila]["area"].ToString() &&
                    categoria == lista_de_chequeo.Rows[fila]["categoria"].ToString())
                {
                    orden = int.Parse(lista_de_chequeo.Rows[fila]["orden"].ToString());
                    if (int.Parse(nuevo_orden) == orden)
                    {
                        viejo_orden = int.Parse(lista_de_chequeo.Rows[fila_actividad]["orden"].ToString());
                        if (int.Parse(nuevo_orden) < viejo_orden)
                        {
                            for (int fila_orden = 0; fila_orden <= fila; fila_orden++)
                            {
                                if (area == lista_de_chequeo.Rows[fila]["area"].ToString() &&
                                    categoria == lista_de_chequeo.Rows[fila]["categoria"].ToString())
                                {
                                    orden_nuevo = int.Parse(lista_de_chequeo.Rows[fila_orden]["orden"].ToString());
                                    orden_nuevo++;
                                    lista_de_chequeo.Rows[fila_orden]["orden"] = orden_nuevo.ToString();
                                }

                            }
                        }
                        else if (int.Parse(nuevo_orden) > viejo_orden)
                        {
                            for (int fila_orden = fila; fila_orden >= 0; fila_orden--)
                            {
                                if (area == lista_de_chequeo.Rows[fila]["area"].ToString() &&
                                    categoria == lista_de_chequeo.Rows[fila]["categoria"].ToString())
                                {
                                    orden_nuevo = int.Parse(lista_de_chequeo.Rows[fila_orden]["orden"].ToString());
                                    orden_nuevo--;
                                    lista_de_chequeo.Rows[fila_orden]["orden"] = orden_nuevo.ToString();
                                }
                            }
                        }
                    }
                }
            }
            lista_de_chequeo.Rows[fila_actividad]["orden"] = nuevo_orden.ToString();
            DataTable lista_de_chequeo_ordenada = lista_de_chequeo;
            lista_de_chequeo_ordenada.DefaultView.Sort = "area asc, categoria asc, orden asc";
            lista_de_chequeo_ordenada = lista_de_chequeo.DefaultView.ToTable();
            orden_nuevo = 1;
            for (int fila = 0; fila <= lista_de_chequeo_ordenada.Rows.Count - 1; fila++)
            {
                if (area == lista_de_chequeo_ordenada.Rows[fila]["area"].ToString() &&
                      categoria == lista_de_chequeo_ordenada.Rows[fila]["categoria"].ToString())
                {
                    id = lista_de_chequeo_ordenada.Rows[fila]["id"].ToString();
                    string dat = lista_de_chequeo_ordenada.Rows[fila]["orden"].ToString();
                    actualizar_orden_actividad(id, "orden", orden_nuevo.ToString());
                    orden_nuevo++;
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
            lista_de_chequeo = consultas.consultar_tabla_completa(base_de_datos, "lista_de_chequeo");
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
            return configuracion_de_chequeo;
        }
        public DataTable get_lista_de_chequeo()
        {
            consultar_lista_de_chequeo();
            return lista_de_chequeo;
        }
        public void set_orden_actividad(string id_actividad, string nuevo_orden)
        {
            consultar_lista_de_chequeo();
            reordenar_lista_chequeo(id_actividad, nuevo_orden);
        }
        #endregion
    }
}
