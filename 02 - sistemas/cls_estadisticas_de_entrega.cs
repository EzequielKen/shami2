using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_estadisticas_de_entrega
    {
        public cls_estadisticas_de_entrega(DataTable usuario_BD)
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

        DataTable insumos_fabrica;
        DataTable productos_terminados;
        DataTable pedidos;
        DataTable resumen;
        DataTable resumen_fecha;
        DataTable sucursales;
        DataTable pedido_de_insumos;
        List<DataTable> lista_pedidos_sucursales = new List<DataTable>();
        DataTable acuerdo_de_precios;
        #endregion

        #region PDF

        public void crear_pdf(string ruta_archivo, byte[] logo, DataTable resumen, string fecha_inicio, string fecha_fin, string total_teorico, string categoria)
        {
            PDF.GenerarPDF_resumen_de_estadisticas_de_pedidos(ruta_archivo, logo, resumen, fecha_inicio, fecha_fin, total_teorico, categoria);
        }
        public void crear_pdf_completo(string ruta_archivo, byte[] logo, DataTable resumen, string fecha_inicio, string fecha_fin, string total_teorico, string categoria, string total_real)
        {
            PDF.GenerarPDF_resumen_de_estadisticas_de_pedidos_completo(ruta_archivo, logo, resumen, fecha_inicio, fecha_fin, total_teorico, categoria, total_real);
        }
        public void crear_pdf_por_fecha(string ruta_archivo, byte[] logo, DataTable resumen_sucursales, string fecha_inicio, string fecha_fin, string fecha_estadistica_inicio, string fecha_estadistica_fin)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos_segun_sucursal(resumen_sucursales, fecha_estadistica_inicio, fecha_estadistica_fin);
            //consultar_orden_de_pedido_segun_sucursal(resumen_sucursales, fecha_inicio, fecha_fin);  Session["fecha_estadistica_inicio"].ToString(), Session["fecha_estadistica_fin"].ToString()
            calcular_estadisticas_por_fecha();
            string sucursales_seleccionadas = string.Empty;
            for (int fila = 0; fila <= resumen_sucursales.Rows.Count - 1; fila++)
            {
                sucursales_seleccionadas = sucursales_seleccionadas + resumen_sucursales.Rows[fila]["sucursal"].ToString() + ". ";
            }
            PDF.GenerarPDF_resumen_de_estadisticas_de_pedidos_segun_fecha(ruta_archivo, logo, resumen_fecha, fecha_inicio, fecha_fin, sucursales_seleccionadas);
        }
        #endregion

        #region crear tabla resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("cantidad_pedida", typeof(string));
            resumen.Columns.Add("cantidad_kilos", typeof(string));
            resumen.Columns.Add("cantidad_entregada", typeof(string));
            resumen.Columns.Add("porcentaje_satisfaccion", typeof(string));
            resumen.Columns.Add("presentacion", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("incremento", typeof(string));
            resumen.Columns.Add("objetivo", typeof(string));
            resumen.Columns.Add("venta_teorica", typeof(string));
            resumen.Columns.Add("venta_real", typeof(string));
            resumen.Columns.Add("pincho", typeof(string));
            resumen.Columns.Add("cantidad_pincho", typeof(string));
        }
        private void crear_resumen_fecha()
        {
            resumen_fecha = new DataTable();
            resumen_fecha.Columns.Add("id", typeof(string));
            resumen_fecha.Columns.Add("producto", typeof(string));
            resumen_fecha.Columns.Add("tipo_producto", typeof(string));
            resumen_fecha.Columns.Add("cantidad_pedida", typeof(string));
            resumen_fecha.Columns.Add("cantidad_entregada", typeof(string));
            resumen_fecha.Columns.Add("porcentaje_satisfaccion", typeof(string));
            resumen_fecha.Columns.Add("presentacion", typeof(string));
            resumen_fecha.Columns.Add("proveedor", typeof(string));
            resumen_fecha.Columns.Add("stock", typeof(string));
            resumen_fecha.Columns.Add("venta_teorica", typeof(string));
            resumen_fecha.Columns.Add("venta_real", typeof(string));
            resumen_fecha.Columns.Add("total", typeof(string));
            resumen_fecha.Columns.Add("inicio", typeof(string));
        }
        #endregion
        #region analisis de produccion
        private void calcular_incremento()
        {
            double cantidad_pedida, stock, incremento, objetivo;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {

                cantidad_pedida = double.Parse(resumen.Rows[fila]["cantidad_pedida"].ToString());
                incremento = (20 * cantidad_pedida) / 100;
                incremento = incremento + cantidad_pedida;
                resumen.Rows[fila]["incremento"] = Math.Ceiling(incremento);
                stock = double.Parse(resumen.Rows[fila]["stock"].ToString());
                objetivo = incremento - stock;
                resumen.Rows[fila]["objetivo"] = Math.Ceiling(objetivo);
            }
        }
        private void calcular_analisis_de_producccion()
        {
            crear_tabla_resumen();

            string id_producto;
            int columna;
            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                columna = pedidos.Columns["producto_1"].Ordinal;
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {
                    if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                    pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        columna = pedidos.Columns["producto_1"].Ordinal;
                        while (columna <= pedidos.Columns.Count - 1 &&
                        funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                        {
                            id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                            if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                            {
                                cargar_estadistica_de_produccion(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu");
                            }
                            /* else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                             {
                                 cargar_estadistica_de_produccion(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica");
                             }*/
                            columna++;
                        }
                    }
                }
            }
            calcular_incremento();
        }
        private void cargar_estadistica_de_produccion(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor)
        {
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            if (fila_producto != -1)
            {

                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 4);
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                    resumen.Rows[fila_resumen]["stock"] = productos_seleccionados.Rows[fila_producto]["stock"].ToString();
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 4));
                }
            }
        }
        #endregion
        #region calculo de estadisticas
        private void calcular_estadisticas()
        {
            crear_tabla_resumen();

            string id_producto;
            string legado = string.Empty;
            int columna = pedidos.Columns["producto_1"].Ordinal;
            for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
            {
                if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                {
                    legado = pedidos.Rows[fila]["legado"].ToString();
                    columna = pedidos.Columns["producto_1"].Ordinal;
                    while (columna <= pedidos.Columns.Count - 1 &&
                    funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                    {
                        id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                        if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                        {
                            cargar_estadistica(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu", pedidos, 4,legado);
                        }
                        else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                        {
                            cargar_estadistica(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica", pedidos, 4, legado);
                        }
                        columna++;
                    }
                }
            }
        }
        private void calcular_estadisticas_por_fecha()
        {
            crear_resumen_fecha();
            string tipo_de_acuerdo, acuerdo_de_precios_dato, proveedor;
            string id_producto;
            int columna;

            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {

                    DateTime fecha = DateTime.Parse(pedidos.Rows[fila]["fecha"].ToString());
                    string fecha_dato = fecha.ToString("dd/MM/yyyy");
                    int columna_fecha = funciones.buscar_columna_por_nombre_columna(fecha_dato, resumen_fecha);
                    if (columna_fecha == -1)
                    {
                        resumen_fecha.Columns.Add(fecha_dato, typeof(double));
                    }
                }
            }
            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                columna = pedidos.Columns["producto_1"].Ordinal;
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {
                    tipo_de_acuerdo = pedidos.Rows[fila]["tipo_de_acuerdo"].ToString();
                    acuerdo_de_precios_dato = pedidos.Rows[fila]["acuerdo_de_precios"].ToString();
                    proveedor = pedidos.Rows[fila]["proveedor"].ToString();
                    consultar_acuerdo_de_precios(proveedor, acuerdo_de_precios_dato, tipo_de_acuerdo);
                    if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                    pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        columna = pedidos.Columns["producto_1"].Ordinal;
                        while (columna <= pedidos.Columns.Count - 1 &&
                        funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                        {
                            id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                            if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                            {
                                cargar_estadistica_fecha(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu", pedidos, 4);
                            }
                            else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                            {
                                cargar_estadistica_fecha(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica", pedidos, 4);
                            }
                            columna++;
                        }
                    }
                }
            }

            double total = 0;
            double cantidad;
            for (int fila = 0; fila <= resumen_fecha.Rows.Count - 1; fila++)
            {
                total = 0;
                for (int column = resumen_fecha.Columns["inicio"].Ordinal + 1; column <= resumen_fecha.Columns.Count - 1; column++)
                {
                    if (double.TryParse(resumen_fecha.Rows[fila][column].ToString(), out cantidad))
                    {
                        total = total + cantidad;
                    }
                }
                resumen_fecha.Rows[fila]["total"] = total.ToString();
            }
        }
        private void calcular_estadisticas_segun_sucursal()
        {
            crear_tabla_resumen();
            string tipo_de_acuerdo, acuerdo_de_precios_dato, proveedor;
            string id_producto;
            string legado = string.Empty;
            int columna;
            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                columna = pedidos.Columns["producto_1"].Ordinal;
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {
                    legado = pedidos.Rows[fila]["legado"].ToString();
                    tipo_de_acuerdo = pedidos.Rows[fila]["tipo_de_acuerdo"].ToString();
                    acuerdo_de_precios_dato = pedidos.Rows[fila]["acuerdo_de_precios"].ToString();
                    proveedor = pedidos.Rows[fila]["proveedor"].ToString();
                    consultar_acuerdo_de_precios(proveedor, acuerdo_de_precios_dato, tipo_de_acuerdo);
                    if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                    pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {

                        columna = pedidos.Columns["producto_1"].Ordinal;
                        while (columna <= pedidos.Columns.Count - 1 &&
                        funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                        {
                            id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                            if (id_producto == "")
                            {
                                string stop = pedidos.Rows[fila]["id"].ToString();
                            }
                            if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                            {
                                cargar_estadistica(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu", pedidos, 4, legado);
                            }
                            else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                            {
                                double precio = double.Parse(acuerdo_de_precios.Rows[0]["producto_" + id_producto.ToString()].ToString());
                                if (id_producto == "74" && precio != 1360)
                                {
                                    string strop = pedidos.Rows[fila]["id"].ToString();
                                }
                                cargar_estadistica(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica", pedidos, 4, legado);
                            }
                            columna++;
                        }
                    }
                }
            }
            int ante_ultima_fila = resumen.Rows.Count - 1;
            string id;
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida, cantidad_entrega, porcentaje_satisfaccion;
            for (int filas = 0; filas <= resumen.Rows.Count - 1; filas++)
            {
                if (resumen.Rows[filas]["cantidad_pedida"].ToString() != "")
                {
                    id = resumen.Rows[filas]["id"].ToString();


                    string datop = resumen.Rows[filas]["cantidad_entregada"].ToString();
                    cantidad_pedida = double.Parse(resumen.Rows[filas]["cantidad_pedida"].ToString());
                    if (resumen.Rows[filas]["cantidad_entregada"].ToString() != "")
                    {
                        if (resumen.Rows[filas]["proveedor"].ToString() == "proveedor_villaMaipu")
                        {
                            fila_producto = funciones.buscar_fila_por_id(id, productos_terminados);
                            if (productos_terminados.Rows[fila_producto]["pincho"].ToString() == "si")
                            {
                                double cant_entregada = double.Parse(resumen.Rows[filas]["cantidad_entregada"].ToString());
                                double venta_real = double.Parse(resumen.Rows[filas]["venta_real"].ToString());
                                double precio_venta = Math.Round(venta_real/ cant_entregada, 2);


                                double equivalencia_pincho = double.Parse(productos_terminados.Rows[fila_producto]["equivalencia_pincho"].ToString());
                                cantidad_entrega = double.Parse(resumen.Rows[filas]["cantidad_entregada"].ToString());
                                cantidad_pedida = cantidad_pedida * equivalencia_pincho;
                                
                                resumen.Rows[filas]["cantidad_kilos"] = cantidad_pedida.ToString() + "Kg";
                                resumen.Rows[filas]["venta_teorica"] = cantidad_pedida*precio_venta;

                                porcentaje_satisfaccion = (cantidad_entrega * 100) / cantidad_pedida;
                                porcentaje_satisfaccion = Math.Round(porcentaje_satisfaccion, 2);
                                resumen.Rows[filas]["porcentaje_satisfaccion"] = porcentaje_satisfaccion.ToString() + "%";
                                

                                string cantidad_pincho = resumen.Rows[filas]["cantidad_pincho"].ToString();
                                string cantidad_entregada = resumen.Rows[filas]["cantidad_entregada"].ToString();
                                resumen.Rows[filas]["cantidad_entregada"] = cantidad_pincho + " PINCHOS | " + cantidad_entregada + "Kg";
                            }
                            else
                            {
                                cantidad_entrega = double.Parse(resumen.Rows[filas]["cantidad_entregada"].ToString());
                                porcentaje_satisfaccion = (cantidad_entrega * 100) / cantidad_pedida;
                                porcentaje_satisfaccion = Math.Round(porcentaje_satisfaccion, 2);
                                resumen.Rows[filas]["porcentaje_satisfaccion"] = porcentaje_satisfaccion.ToString() + "%";
                            }
                        }
                        else
                        {
                            cantidad_entrega = double.Parse(resumen.Rows[filas]["cantidad_entregada"].ToString());
                            porcentaje_satisfaccion = (cantidad_entrega * 100) / cantidad_pedida;
                            porcentaje_satisfaccion = Math.Round(porcentaje_satisfaccion, 2);
                            resumen.Rows[filas]["porcentaje_satisfaccion"] = porcentaje_satisfaccion.ToString() + "%";
                        }
                    }
                }

            }
        }
        private void cargar_estadistica_fecha(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor, DataTable pedido, int posicion)
        {
            DateTime fecha = DateTime.Parse(pedido.Rows[fila]["fecha"].ToString());
            string fecha_dato = fecha.ToString("dd/MM/yyyy");
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida, cantidad_entregada, venta_teorica, venta_real, precio, venta_teorica_historico, venta_real_historico;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            precio = double.Parse(acuerdo_de_precios.Rows[0]["producto_" + id_producto.ToString()].ToString());
            if (fila_producto != -1)
            {
                // cantidad_entregada porcentaje_satisfaccion
                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen_fecha);
                if (fila_resumen == -1)
                {
                    resumen_fecha.Rows.Add();
                    fila_resumen = resumen_fecha.Rows.Count - 1;
                    resumen_fecha.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen_fecha.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen_fecha.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();

                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado")
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {
                            int columna_fecha = funciones.buscar_columna_por_nombre_columna(fecha_dato, resumen_fecha);

                            cantidad_entregada = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));
                            resumen_fecha.Rows[fila_resumen][columna_fecha] = cantidad_entregada.ToString();
                            //  venta_real = cantidad_entregada * precio;
                            //  resumen_fecha.Rows[fila_resumen]["venta_real"] = venta_real.ToString();
                        }
                    }
                    resumen_fecha.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen_fecha.Rows[fila_resumen]["proveedor"] = proveedor;
                }
                else
                {
                    int columna_fecha = funciones.buscar_columna_por_nombre_columna(fecha_dato, resumen_fecha);

                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado")
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {

                            string dato = resumen_fecha.Rows[fila_resumen][columna_fecha].ToString();
                            if (resumen_fecha.Rows[fila_resumen][columna_fecha].ToString() == "")
                            {
                                cantidad_entregada = 0;
                            }
                            else
                            {
                                cantidad_entregada = double.Parse(resumen_fecha.Rows[fila_resumen][columna_fecha].ToString());
                            }
                            resumen_fecha.Rows[fila_resumen][columna_fecha] = cantidad_entregada + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));

                            //    cantidad_entregada = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));
                            //    venta_real_historico = double.Parse(resumen.Rows[fila_resumen]["venta_real"].ToString());
                            //    venta_real = cantidad_entregada * precio;
                            //    venta_real = venta_real_historico + venta_real;
                            //    resumen.Rows[fila_resumen]["venta_real"] = venta_real.ToString();
                        }
                    }

                }
            }
        }
        private void cargar_estadistica(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor, DataTable pedido, int posicion, string legado)
        {
            
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida, cantidad_entregada, venta_teorica, venta_real, precio, venta_teorica_historico, venta_real_historico;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            precio = double.Parse(acuerdo_de_precios.Rows[0]["producto_" + id_producto.ToString()].ToString());
            if (id_producto == "74" && precio!=1360)
            {
                string strop = "";
            }
            if (fila_producto != -1)
            {
                // cantidad_entregada porcentaje_satisfaccion
                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    if ("" == funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion))
                    {
                        resumen.Rows[fila_resumen]["cantidad_pedida"] = "0";
                        resumen.Rows[fila_resumen]["venta_teorica"] = "0";
                        resumen.Rows[fila_resumen]["venta_real"] = "0";
                        resumen.Rows[fila_resumen]["cantidad_pincho"] = "0";

                    }
                    else
                    {
                        cantidad_pedida = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                        resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida.ToString();
                        venta_teorica = cantidad_pedida * precio;
                        resumen.Rows[fila_resumen]["venta_teorica"] = venta_teorica.ToString();
                        resumen.Rows[fila_resumen]["venta_real"] = "0";
                        resumen.Rows[fila_resumen]["cantidad_pincho"] = "0";



                    }
                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado")
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {
                            cantidad_entregada = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));
                            resumen.Rows[fila_resumen]["cantidad_entregada"] = cantidad_entregada.ToString();
                            venta_real = cantidad_entregada * precio;
                            resumen.Rows[fila_resumen]["venta_real"] = venta_real.ToString();

                            if (proveedor == "proveedor_villaMaipu")
                            {
                                if (productos_seleccionados.Rows[fila_producto]["pincho"].ToString() == "si")
                                {
                                    if (legado == "si")
                                    {
                                        double equivalencia_pincho = double.Parse(productos_seleccionados.Rows[fila_producto]["equivalencia_pincho"].ToString());
                                        double cantidad_pincho = double.Parse(resumen.Rows[fila_resumen]["cantidad_pincho"].ToString());
                                        cantidad_pincho = Math.Round(cantidad_pincho + (cantidad_entregada / equivalencia_pincho),2);
                                        resumen.Rows[fila_resumen]["pincho"] = "si";
                                        resumen.Rows[fila_resumen]["cantidad_pincho"] = cantidad_pincho.ToString();
                                    }
                                    else
                                    {
                                        double pinchos_entregados = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), 8));
                                        double cantidad_pincho = double.Parse(resumen.Rows[fila_resumen]["cantidad_pincho"].ToString());
                                        cantidad_pincho = cantidad_pincho + pinchos_entregados;
                                        resumen.Rows[fila_resumen]["pincho"] = "si";
                                        resumen.Rows[fila_resumen]["cantidad_pincho"] = cantidad_pincho.ToString();
                                    }
                                }
                            }
                        }
                    }
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                    cantidad_pedida = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                    venta_teorica = cantidad_pedida * precio;
                    venta_teorica_historico = double.Parse(resumen.Rows[fila_resumen]["venta_teorica"].ToString());
                    double venta_teorica_nueva = venta_teorica_historico + venta_teorica;
                    resumen.Rows[fila_resumen]["venta_teorica"] = venta_teorica_nueva.ToString();
                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado" && "" != resumen.Rows[fila_resumen]["cantidad_entregada"].ToString())
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {
                            string dato = resumen.Rows[fila_resumen]["cantidad_entregada"].ToString();
                            cantidad_entregada = double.Parse(resumen.Rows[fila_resumen]["cantidad_entregada"].ToString());
                            resumen.Rows[fila_resumen]["cantidad_entregada"] = cantidad_entregada + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));

                            cantidad_entregada = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));
                            venta_real_historico = double.Parse(resumen.Rows[fila_resumen]["venta_real"].ToString());
                            venta_real = cantidad_entregada * precio;
                            venta_real = venta_real_historico + venta_real;
                            resumen.Rows[fila_resumen]["venta_real"] = venta_real.ToString();

                            if (proveedor == "proveedor_villaMaipu")
                            {
                                if (productos_seleccionados.Rows[fila_producto]["pincho"].ToString() == "si")
                                {
                                    double cantidad_pincho ;
                                    if (legado == "si")
                                    {
                                        double equivalencia_pincho = double.Parse(productos_seleccionados.Rows[fila_producto]["equivalencia_pincho"].ToString());
                                        cantidad_pincho = double.Parse(resumen.Rows[fila_resumen]["cantidad_pincho"].ToString());
                                        cantidad_pincho = Math.Round(cantidad_pincho + (cantidad_entregada / equivalencia_pincho), 2);
                                        resumen.Rows[fila_resumen]["pincho"] = "si";
                                        resumen.Rows[fila_resumen]["cantidad_pincho"] = cantidad_pincho.ToString();
                                    }
                                    else
                                    {
                                        double pinchos_entregados = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), 8));
                                        cantidad_pincho = double.Parse(resumen.Rows[fila_resumen]["cantidad_pincho"].ToString());
                                        cantidad_pincho = cantidad_pincho + pinchos_entregados;
                                        resumen.Rows[fila_resumen]["pincho"] = "si";
                                        resumen.Rows[fila_resumen]["cantidad_pincho"] = cantidad_pincho.ToString();
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        private void cargar_estadistica_produccion(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor, DataTable pedido, int posicion)
        {
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            if (fila_producto != -1)
            {

                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                }
            }

        }
        #endregion

        #region metodos consultas
        private void consultar_acuerdo_de_precios(string proveedor, string acuerdo_de_precios_dato, string tipo_de_acuerdo)
        {
            acuerdo_de_precios = consultas.consultar_acuerdo_de_precios_segun_parametros(base_de_datos, "acuerdo_de_precios", proveedor, acuerdo_de_precios_dato, tipo_de_acuerdo);
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villaMaipu");
        }

        private void consultar_productos_terminados_fabrica_fatay()
        {
            productos_terminados = consultas.consultar_productos_produccion_fabrica_fatay(base_de_datos, "proveedor_villaMaipu");
        }
        private void consultar_pedidos(string sucursal, string fecha_inicio, string fecha_fin)
        {
            pedidos = consultas.consultar_pedidos_segun_rango_de_fecha(sucursal, fecha_inicio, fecha_fin);
        }
        private void consultar_pedidos_segun_sucursal(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {
            lista_pedidos_sucursales.Clear();
            for (int fila = 0; fila <= resumen_sucursales.Rows.Count - 1; fila++)
            {
                lista_pedidos_sucursales.Add(consultas.consultar_pedidos_entregados_segun_rango_de_fecha(resumen_sucursales.Rows[fila]["sucursal"].ToString(), fecha_inicio, fecha_fin));
            }
        }
        private void consultar_orden_de_pedido_segun_sucursal(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {

            pedido_de_insumos = consultas.consultar_pedidos_produccion_segun_rango_de_fecha("Shami Villa Maipu Limpieza", fecha_inicio, fecha_fin);
        }

        private void consultar_sucursal()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        #endregion

        #region metodos get/set
        public DataTable obtener_estadisticas_de_pedido(string sucursal, string fecha_inicio, string fecha_fin)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos(sucursal, fecha_inicio, fecha_fin);
            calcular_estadisticas();
            return resumen;
        }
        public DataTable obtener_estadisticas_de_pedido_segun_sucursales(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos_segun_sucursal(resumen_sucursales, fecha_inicio, fecha_fin);
            //consultar_orden_de_pedido_segun_sucursal(resumen_sucursales, fecha_inicio, fecha_fin);
            calcular_estadisticas_segun_sucursal();
            return resumen;
        }
        public DataTable get_analisis_produccion(string fecha_inicio, string fecha_fin)
        {
            consultar_sucursal();

            //consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos_segun_sucursal(sucursales, fecha_inicio, fecha_fin);
            calcular_analisis_de_producccion();
            return resumen;
        }

        public DataTable get_analisis_produccion_fabrica_fatay(string fecha_inicio, string fecha_fin)
        {
            consultar_sucursal();

            //consultar_insumos_fabrica();
            consultar_productos_terminados_fabrica_fatay();
            consultar_pedidos_segun_sucursal(sucursales, fecha_inicio, fecha_fin);
            calcular_analisis_de_producccion();
            return resumen;
        }
        public DataTable get_sucursal()
        {
            consultar_sucursal();
            sucursales.DefaultView.Sort = "sucursal asc";
            sucursales = sucursales.DefaultView.ToTable();
            return sucursales;
        }
        #endregion
    }
}
