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
    [Serializable]
    public class cls_actualizador_de_precios_insumos
    {
        public cls_actualizador_de_precios_insumos(DataTable usuario_BD)
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

        #region carga a base de datos
        public void actualizar(DataTable resumen)
        {
            consultar_precios();
            desactivar_precios_activos(precios.Rows[0]["id"].ToString());
            string columnas = "";
            string valores = "";
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", false);
            valores = funciones.armar_query_valores(valores, "insumos_fabrica", false);
            //acuerdo
            double acuerdo = double.Parse(precios.Rows[0]["acuerdo"].ToString()) + 1;
            columnas = funciones.armar_query_columna(columnas, "acuerdo", false);
            valores = funciones.armar_query_valores(valores, acuerdo.ToString(), false);
            //tipo_de_acuerdo
            columnas = funciones.armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = funciones.armar_query_valores(valores, precios.Rows[0]["tipo_de_acuerdo"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);

            string id;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();//
                /*if (resumen.Rows[fila]["unidad_de_medida_local"].ToString() != resumen.Rows[fila]["unidad_medida"].ToString())
                  {
                      actualizar_presentacion(id, resumen.Rows[fila]["unidad_de_medida_local"].ToString());

                  }*/
                //  
                //producto_1
                columnas = funciones.armar_query_columna(columnas, "producto_" + id, false);
                if (resumen.Rows[fila]["precio_venta_personalizado_por_unidad"].ToString() != "N/A")
                {
                    valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["precio_venta_personalizado_por_unidad"].ToString(), false);
                }
                else if (resumen.Rows[fila]["precio_venta_nuevo_por_unidad"].ToString() != "N/A")
                {
                    valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["precio_venta_nuevo_por_unidad"].ToString(), false);
                }
                else
                {
                    valores = funciones.armar_query_valores(valores, precios.Rows[0]["producto_" + id].ToString(), false);
                }
            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            /*if (resumen.Rows[ultima_fila]["unidad_de_medida_local"].ToString() != resumen.Rows[ultima_fila]["unidad_medida"].ToString())
            {
                actualizar_presentacion(id, resumen.Rows[ultima_fila]["unidad_de_medida_local"].ToString());

            }*/
            //producto_1
            columnas = funciones.armar_query_columna(columnas, "producto_" + id, true);
            if (resumen.Rows[ultima_fila]["precio_venta_personalizado_por_unidad"].ToString() != "N/A")
            {
                valores = funciones.armar_query_valores(valores, resumen.Rows[ultima_fila]["precio_venta_personalizado_por_unidad"].ToString(), true);
            }
            else if (resumen.Rows[ultima_fila]["precio_venta_nuevo_por_unidad"].ToString() != "N/A")
            {
                valores = funciones.armar_query_valores(valores, resumen.Rows[ultima_fila]["precio_venta_nuevo_por_unidad"].ToString(), true);
            }
            else
            {
                valores = funciones.armar_query_valores(valores, precios.Rows[0]["producto_" + id].ToString(), true);
            }
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columnas, valores);
        }
        private void desactivar_precios_activos(string id_precios)
        {
            string actualizar = "`activa` = '0' ";
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios", actualizar, id_precios);
        }
        private void actualizar_presentacion(string id_insumo, string presentacion)
        {
            string actualizar = "`unidad_de_medida_local` = '" + presentacion + "'";
            consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
            actualizar = "`unidad_de_medida_fabrica` = '" + presentacion + "'";
            consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
        }
        #endregion

        #region acualizar precio fabrica fatay
        public void actualizar_precio_fabrica_fatay(DataTable resumen)
        {
            consultar_precios_fabrica_fatay();
            desactivar_precios_activos(precios.Rows[0]["id"].ToString());
            string columnas = "";
            string valores = "";
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", false);
            valores = funciones.armar_query_valores(valores, "insumos_fabrica", false);
            //acuerdo
            double acuerdo = double.Parse(precios.Rows[0]["acuerdo"].ToString()) + 1;
            columnas = funciones.armar_query_columna(columnas, "acuerdo", false);
            valores = funciones.armar_query_valores(valores, acuerdo.ToString(), false);
            //tipo_de_acuerdo
            columnas = funciones.armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = funciones.armar_query_valores(valores, precios.Rows[0]["tipo_de_acuerdo"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);

            string id;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();//
               
                columnas = funciones.armar_query_columna(columnas, "producto_" + id, false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["precio_venta_fabrica_fatay"].ToString(), false);

            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
           
            //producto_1
            columnas = funciones.armar_query_columna(columnas, "producto_" + id, true);
            valores = funciones.armar_query_valores(valores, resumen.Rows[ultima_fila]["precio_venta_fabrica_fatay"].ToString(), true);
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columnas, valores);
        }
        #endregion

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos;
        DataTable precios_compra_insumos;
        DataTable precios;
        #endregion

        #region metodos privados
        private string obtener_presentacion_compra(string dato)
        {
            string retorno = "";
            string tipo_paquete, unidades, unidad;
            tipo_paquete = funciones.obtener_dato(dato, 3);
            unidades = funciones.obtener_dato(dato, 4);
            unidad = funciones.obtener_dato(dato, 5);
            retorno = tipo_paquete + " - " + unidades + " - " + unidad;
            return retorno;
        }
        private string obtener_presentacion_venta(string dato)
        {
            string retorno = "";
            string tipo_paquete, unidades, unidad;
            tipo_paquete = funciones.obtener_dato(dato, 1);
            unidades = funciones.obtener_dato(dato, 2);
            unidad = funciones.obtener_dato(dato, 3);
            retorno = tipo_paquete + " - " + unidades + " - " + unidad;
            return retorno;
        }
        private void setear_precios_en_cero()
        {
            insumos.Columns.Add("precio_compra", typeof(string));
            insumos.Columns.Add("precio_venta_actual", typeof(string));
            insumos.Columns.Add("porcentaje_ganancia_actual", typeof(string));
            insumos.Columns.Add("diferencia", typeof(string));
            insumos.Columns.Add("porcentaje_aumento", typeof(string));
            insumos.Columns.Add("precio_venta_nuevo", typeof(string));
            insumos.Columns.Add("precio_venta_nuevo_por_unidad", typeof(string));
            insumos.Columns.Add("precio_venta_personalizado", typeof(string));
            insumos.Columns.Add("precio_venta_personalizado_por_unidad", typeof(string));
            insumos.Columns.Add("presentacion", typeof(string));
            insumos.Columns.Add("presentacion_fabrica", typeof(string));
            insumos.Columns.Add("multiplicador", typeof(string));
            insumos.Columns.Add("precio_venta_fabrica_fatay", typeof(string));

            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                insumos.Rows[fila]["precio_compra"] = "0";
                insumos.Rows[fila]["precio_venta_actual"] = "0";
                insumos.Rows[fila]["precio_venta_fabrica_fatay"] = "0";
                insumos.Rows[fila]["diferencia"] = "N/A";
                insumos.Rows[fila]["porcentaje_aumento"] = "N/A";
                insumos.Rows[fila]["precio_venta_nuevo"] = "N/A";
                insumos.Rows[fila]["precio_venta_nuevo_por_unidad"] = "N/A";
                insumos.Rows[fila]["precio_venta_personalizado"] = "N/A";
                insumos.Rows[fila]["precio_venta_personalizado_por_unidad"] = "N/A";
                insumos.Rows[fila]["presentacion"] = obtener_presentacion_venta(insumos.Rows[fila]["unidad_de_medida_local"].ToString());
                insumos.Rows[fila]["multiplicador"] = funciones.obtener_dato(insumos.Rows[fila]["unidad_de_medida_local"].ToString(), 2);
            }
        }
        private void calcular_precios()
        {
            string id;
            int fila_insumo;
            double precio, precio_compra, unidad;
            for (int fila = 0; fila <= precios_compra_insumos.Rows.Count - 1; fila++)
            {
                for (int columna = precios_compra_insumos.Columns["producto_1"].Ordinal; columna <= precios_compra_insumos.Columns.Count - 1; columna++)
                {

                    if (precios_compra_insumos.Rows[fila][columna].ToString() != "N/A")
                    {
                        id = funciones.obtener_dato(precios_compra_insumos.Rows[fila][columna].ToString(), 1);
                        
                        fila_insumo = funciones.buscar_fila_por_id(id, insumos);
                        if (fila_insumo != -1)
                        {

                            precio = double.Parse(insumos.Rows[fila_insumo]["precio_compra"].ToString());
                            unidad = double.Parse(funciones.obtener_dato(precios_compra_insumos.Rows[fila][columna].ToString(), 4));
                            precio_compra = double.Parse(funciones.obtener_dato(precios_compra_insumos.Rows[fila][columna].ToString(), 6));

                            precio_compra = precio_compra * unidad;

                            if (precio < precio_compra)
                            {
                                insumos.Rows[fila_insumo]["precio_compra"] = precio_compra;
                                insumos.Rows[fila_insumo]["unidad_medida"] = obtener_presentacion_compra(precios_compra_insumos.Rows[fila][columna].ToString());

                            }
                        }

                    }
                }
            }
            double diferencia_actual, precio_venta_actual, porcentaje_ganancia_actual,presentacion_compra,presentacion_venta;
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                id = insumos.Rows[fila]["id"].ToString();
                insumos.Rows[fila]["precio_venta_actual"] = double.Parse(precios.Rows[0]["producto_" + id].ToString()) * double.Parse(insumos.Rows[fila]["multiplicador"].ToString());//multiplicador
                precio_compra = double.Parse(insumos.Rows[fila]["precio_compra"].ToString());

                if (double.TryParse(funciones.obtener_dato(insumos.Rows[fila]["unidad_medida"].ToString().Replace(" ", ""), 2),out presentacion_compra))
                {
                    if (double.TryParse(funciones.obtener_dato(insumos.Rows[fila]["presentacion"].ToString().Replace(" ", ""), 2), out presentacion_venta))
                    {
                        precio_compra = precio_compra / presentacion_compra;
                        precio_compra = precio_compra * presentacion_venta;
                    }
                }

                precio_venta_actual = double.Parse(insumos.Rows[fila]["precio_venta_actual"].ToString());
                diferencia_actual = precio_venta_actual - precio_compra;
                porcentaje_ganancia_actual = (diferencia_actual * 100) / precio_compra;
                porcentaje_ganancia_actual = Math.Round(porcentaje_ganancia_actual, 2);
                if (porcentaje_ganancia_actual <= 0)
                {
                    //insumos.Rows[fila]["porcentaje_ganancia_actual"] = "0%";
                    insumos.Rows[fila]["porcentaje_ganancia_actual"] = porcentaje_ganancia_actual.ToString() + "%";
                }
                else
                {
                    insumos.Rows[fila]["porcentaje_ganancia_actual"] = porcentaje_ganancia_actual.ToString() + "%";
                }

            }
        }
        private void calcular_precios_fabrica_fatay()
        {
            string id;
            int fila_insumo;
            double precio, precio_compra, unidad;
            for (int fila = 0; fila <= precios_compra_insumos.Rows.Count - 1; fila++)
            {
                for (int columna = precios_compra_insumos.Columns["producto_1"].Ordinal; columna <= precios_compra_insumos.Columns.Count - 1; columna++)
                {
                    if (precios_compra_insumos.Rows[fila][columna].ToString() != "N/A")
                    {
                        id = funciones.obtener_dato(precios_compra_insumos.Rows[fila][columna].ToString(), 1);
                       
                        fila_insumo = funciones.buscar_fila_por_id(id, insumos);
                        if (fila_insumo != -1)
                        {
                            precio = double.Parse(insumos.Rows[fila_insumo]["precio_compra"].ToString());
                            unidad = double.Parse(funciones.obtener_dato(insumos.Rows[fila_insumo]["unidad_de_medida_local"].ToString(), 2));
                            precio_compra = double.Parse(funciones.obtener_dato(precios_compra_insumos.Rows[fila][columna].ToString(), 6));

                            if (precio < precio_compra)
                            {

                                insumos.Rows[fila_insumo]["precio_compra"] = precio_compra;
                                insumos.Rows[fila_insumo]["precio_venta_fabrica_fatay"] = precio_compra;
                                insumos.Rows[fila_insumo]["unidad_medida"] = obtener_presentacion_compra(precios_compra_insumos.Rows[fila][columna].ToString());
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region consultas
        private void consultar_insumos()
        {
           // insumos = consultas.consultar_insumos_fabrica_venta_para_actualizar(base_de_datos, "insumos_fabrica");
          insumos = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");

        }
        private void consultar_insumos_fabrica_fatay()
        {
            insumos = consultas.consultar_insumos_fabrica_fatay(base_de_datos, "insumos_fabrica");

        }
        private void consultar_acuerdo_de_precios_fabrica_a_proveedores()
        {
            precios_compra_insumos = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_activo();
        }
        private void consultar_precios()
        {
            precios = consultas.consultar_acuerdo_de_precio_activo(base_de_datos, "insumos_fabrica");
        }
        private void consultar_precios_fabrica_fatay()
        {
            precios = consultas.consultar_acuerdo_de_precio_activo_fabrica_fatay(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos()
        {
            consultar_insumos();
            consultar_acuerdo_de_precios_fabrica_a_proveedores();
            consultar_precios();
            setear_precios_en_cero();
            calcular_precios();
            return insumos;
        }
        public void actualizar_precios_fabrica_fatay()
        {
            consultar_insumos_fabrica_fatay();
            consultar_acuerdo_de_precios_fabrica_a_proveedores();
            consultar_precios_fabrica_fatay();
            setear_precios_en_cero();
            calcular_precios_fabrica_fatay();
            actualizar_precio_fabrica_fatay(insumos);
        }
        #endregion
    }
}
