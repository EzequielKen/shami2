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
    public class cls_conteo_de_stock
    {
        public cls_conteo_de_stock(DataTable usuario_BD)
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
            stock = new cls_stock_producto_terminado(usuarioBD);
        }

        #region atributos
        cls_stock_producto_terminado stock;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos;
        DataTable conteo_stock_cargado;
        #endregion

        #region carga a base de datos
        public void cargar_conteo(DataTable resumen)
        {
            string columnas = "";
            string valores = "";
            string fecha = funciones.get_fecha();
            double conteo, stock_actual, diferencia;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {
                if (resumen.Rows[fila]["conteo_stock"].ToString() != "N/A")
                {
                    if (verificar_si_existe(resumen.Rows[fila]["id"].ToString()))
                    {
                        if (conteo_stock_cargado.Rows[0]["aprobado"].ToString()=="No")
                        {

                            //actualizar  resumen.Rows[fila]["conteo_stock"].ToString()
                            string id = conteo_stock_cargado.Rows[0]["id"].ToString();
                            string actualizar = "`conteo` = '" + resumen.Rows[fila]["conteo_stock"].ToString() + "'";
                            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);

                            //stock
                            stock_actual = double.Parse(stock.get_ultimo_stock_producto_terminado(resumen.Rows[fila]["id"].ToString()));
                            actualizar = "`stock` = '" + stock_actual.ToString() + "'";
                            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);

                            //diferencia
                            conteo = double.Parse(resumen.Rows[fila]["conteo_stock"].ToString());
                            diferencia = conteo - stock_actual;
                            actualizar = "`diferencia` = '" + diferencia.ToString() + "'";
                            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);
                            //aprobado
                            if (diferencia == 0)
                            {
                                actualizar = "`aprobado` = 'Si'";
                            }
                            else
                            {
                                actualizar = "`aprobado` = 'No'";
                            }
                            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);
                            //fecha
                            actualizar = "`fecha` = '" + funciones.get_fecha() + "'";
                            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);
                        }

                    }
                    else
                    {
                        if (resumen.Rows[fila]["conteo_stock"].ToString() != "N/A")
                        {
                            //id_producto
                            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
                            valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["id"].ToString(), false);
                            //producto
                            columnas = funciones.armar_query_columna(columnas, "producto", false);
                            valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["producto"].ToString(), false);
                            //unidad_medida
                            columnas = funciones.armar_query_columna(columnas, "unidad_medida", false);
                            valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["unidad_de_medida_local"].ToString(), false);
                            //fecha
                            columnas = funciones.armar_query_columna(columnas, "fecha", false);
                            valores = funciones.armar_query_valores(valores, fecha, false);
                            //conteo
                            columnas = funciones.armar_query_columna(columnas, "conteo", false);
                            valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["conteo_stock"].ToString(), false);
                            //stock
                            columnas = funciones.armar_query_columna(columnas, "stock", false);
                            stock_actual = double.Parse(stock.get_ultimo_stock_producto_terminado(resumen.Rows[fila]["id"].ToString()));
                            valores = funciones.armar_query_valores(valores, stock_actual.ToString(), false);
                            //diferencia
                            columnas = funciones.armar_query_columna(columnas, "diferencia", false);
                            conteo = double.Parse(resumen.Rows[fila]["conteo_stock"].ToString());
                            diferencia = conteo - stock_actual;
                            valores = funciones.armar_query_valores(valores, diferencia.ToString(), false);

                            //aprobado
                            columnas = funciones.armar_query_columna(columnas, "aprobado", true);
                            if (diferencia == 0)
                            {
                                valores = funciones.armar_query_valores(valores, "Si", true);
                            }
                            else
                            {
                                valores = funciones.armar_query_valores(valores, "No", true);
                            }

                            consultas.insertar_en_tabla(base_de_datos, "conteo_stock", columnas, valores);
                            columnas = string.Empty;
                            valores = string.Empty;
                        }
                    }
                }

            }
        }
        private bool verificar_si_existe(string id_producto)
        {
            bool retorno = false;
            string dia = DateTime.Now.Day.ToString();
            string mes = DateTime.Now.Month.ToString();
            string año = DateTime.Now.Year.ToString();
            conteo_stock_cargado = consultas.consultar_conteo_stock_segun_fecha_e_id(id_producto, dia, mes, año);
            if (conteo_stock_cargado.Rows.Count > 0)
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_productos()
        {
            productos = consultas.consultar_tabla(base_de_datos, "proveedor_villamaipu");
            productos.Columns.Add("orden", typeof(int));
            productos.Columns.Add("conteo_stock", typeof(string));
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                productos.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(productos.Rows[fila]["tipo_producto"].ToString(), 1));
                productos.Rows[fila]["conteo_stock"] = "N/A";
            }
            productos.DefaultView.Sort = "orden ASC";
            productos = productos.DefaultView.ToTable();
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos()
        {
            consultar_productos();
            return productos;
        }
        #endregion
    }
}
