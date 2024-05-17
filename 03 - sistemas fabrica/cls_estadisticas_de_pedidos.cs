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
    public class cls_estadisticas_de_pedidos
    {
        public cls_estadisticas_de_pedidos(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable pedidos_segun_fecha;
        DataTable totales_segun_fecha;
        DataTable totales_completo;
        DataTable productos_proveedor;
        DataTable promedio_del_mes;
        DataTable insumos_fabrica;
        DataTable resumen;
        #endregion

        #region PDF
        public void crear_pdf_estadisticas(string ruta_archivo, byte[] logo, DataTable resumen, string fecha)
        {
            resumen.DefaultView.Sort = "tipo_producto ASC";
            resumen = resumen.DefaultView.ToTable();
            PDF.GenerarPDF_estadisticas_de_pedidos(ruta_archivo, logo, resumen, fecha);
        }
        #endregion

        #region metodos publicos
        public void actualizar_totales_y_promedios(string proveedor, string año, string mes)
        {
            consultar_productos_proveedor(proveedor);
            consultar_pedidos_segun_fecha(proveedor, mes, año);
            if (verificar_si_existen_pedidos())
            {
                calcular_totales(proveedor, mes, año);
                calcular_totales_hasta_el_mes(mes, año, proveedor);

                calcular_promedios(proveedor, mes, año);
            }
        }
        #endregion

        #region carga a base de datos
        private void crear_totales_en_BD(string año, string mes, string proveedor)
        {
            string id, dato;
            string columnas = "";
            string valores = "";

            //tipo_registro
            columnas = armar_query_columna(columnas, "tipo_registro", false);
            valores = armar_query_valores(valores, "totales_del_mes", false);
            //fecha
            string fecha = año + "-" + mes + "-01";
            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor, false);
            //producto_1
            int index = 1;
            for (int fila = 0; fila < productos_proveedor.Rows.Count - 1; fila++)
            {
                if (productos_proveedor.Rows[fila]["totales_del_mes"].ToString() != "0")
                {
                    id = productos_proveedor.Rows[fila]["id"].ToString();
                    columnas = armar_query_columna(columnas, "producto_" + index.ToString(), false);
                    dato = id + "-" + productos_proveedor.Rows[fila]["totales_del_mes"].ToString();
                    valores = armar_query_valores(valores, dato, false);
                    index++;
                }
            }
            int ultima_fila = productos_proveedor.Rows.Count - 1;
            id = productos_proveedor.Rows[ultima_fila]["id"].ToString();
            columnas = armar_query_columna(columnas, "producto_" + index.ToString(), true);
            dato = id + "-" + productos_proveedor.Rows[ultima_fila]["totales_del_mes"].ToString();
            valores = armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "totales_y_promedios_de_pedidos", columnas, valores);
        }
        private void actualizar_totales_en_BD(string nombre_proveedor)
        {
            int fila_totales = buscar_fila_de_totales_segun_proveedor(nombre_proveedor);
            if (fila_totales != -1)
            {

                int columna_totales;
                string id_totales = totales_segun_fecha.Rows[fila_totales]["id"].ToString();
                string actualizar;
                string id_producto;
                string dato;
                double total_registrado, total_del_mes;
                for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                {
                    id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                    columna_totales = buscar_columna_totales(id_producto, fila_totales, totales_segun_fecha);
                    if (columna_totales != -1)
                    {
                        total_registrado = double.Parse(funciones.obtener_dato(totales_segun_fecha.Rows[fila_totales][columna_totales].ToString(), 2));
                        total_del_mes = double.Parse(productos_proveedor.Rows[fila]["totales_del_mes"].ToString());
                        if (total_del_mes != total_registrado)
                        {
                            dato = id_producto + "-" + total_del_mes.ToString();
                            //actualizar
                            actualizar = "`" + totales_segun_fecha.Columns[columna_totales].ColumnName + "` = '" + dato + "'";
                            consultas.actualizar_tabla(base_de_datos, "totales_y_promedios_de_pedidos", actualizar, id_totales);
                        }
                    }

                }
            }

        }
        private int buscar_columna_totales(string id_producto, int fila_totales, DataTable dt)
        {
            int retorno = -1;
            int columna = dt.Columns["producto_1"].Ordinal;
            while (columna <= dt.Columns.Count - 1)
            {
                if (id_producto == funciones.obtener_dato(dt.Rows[fila_totales][columna].ToString(), 1))
                {
                    retorno = columna;
                    break;
                }
                columna++;
            }
            return retorno;
        }
        private void crear_promedios_en_BD(string año, string mes, string proveedor)
        {
            string id;
            string columnas = "";
            string valores = "";
            string dato;
            //tipo_registro
            columnas = armar_query_columna(columnas, "tipo_registro", false);
            valores = armar_query_valores(valores, "promedio_del_mes", false);
            //fecha
            string fecha = año + "-" + mes + "-01";
            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor, false);
            //producto_1
            int index = 1;
            for (int fila = 0; fila < productos_proveedor.Rows.Count - 1; fila++)
            {
                if (productos_proveedor.Rows[fila]["promedio_del_mes"].ToString() != "0")
                {


                    id = productos_proveedor.Rows[fila]["id"].ToString();
                    dato = id + "-" + productos_proveedor.Rows[fila]["promedio_del_mes"].ToString();
                    columnas = armar_query_columna(columnas, "producto_" + index.ToString(), false);
                    valores = armar_query_valores(valores, dato, false);
                    index++;
                }
            }
            int ultima_fila = productos_proveedor.Rows.Count - 1;
            id = productos_proveedor.Rows[ultima_fila]["id"].ToString();
            dato = id + "-" + productos_proveedor.Rows[ultima_fila]["promedio_del_mes"].ToString();
            columnas = armar_query_columna(columnas, "producto_" + index.ToString(), true);
            valores = armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "totales_y_promedios_de_pedidos", columnas, valores);
        }
        private void actualizar_promedio_en_BD(string proveedor)
        {
            int fila_promedio = buscar_fila_de_promedio_segun_proveedor(proveedor);
            if (fila_promedio != -1)
            {

                int columna_promedio;
                string id_promedio = totales_segun_fecha.Rows[fila_promedio]["id"].ToString();
                string actualizar;
                string id_producto;
                double total_registrado, promedio_del_mes;
                string dato;
                for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                {

                    id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                    columna_promedio = buscar_columna_totales_hasta_el_mes(id_producto, fila_promedio, totales_segun_fecha);
                    if (columna_promedio != -1)
                    {
                        total_registrado = double.Parse(funciones.obtener_dato(totales_segun_fecha.Rows[fila_promedio][columna_promedio].ToString(), 2));
                        promedio_del_mes = double.Parse(productos_proveedor.Rows[fila]["promedio_del_mes"].ToString());
                        if (promedio_del_mes != total_registrado)
                        {
                            //actualizar
                            dato = id_producto + "-" + promedio_del_mes.ToString();
                            actualizar = "`" + totales_segun_fecha.Columns[columna_promedio].ColumnName + "` = '" + dato + "'";
                            consultas.actualizar_tabla(base_de_datos, "totales_y_promedios_de_pedidos", actualizar, id_promedio);
                        }
                    }
                }

            }
        }
        private void crear_totales_hasta_el_mes_BD(string iteraciones, string año, string mes, string proveedor)
        {
            string id;
            string columnas = "";
            string valores = "";
            string dato;
            //tipo_registro
            columnas = armar_query_columna(columnas, "tipo_registro", false);
            valores = armar_query_valores(valores, "totales_hasta_el_mes", false);
            //fecha
            string fecha = año + "-" + mes + "-01";
            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor, false);
            //iteraciones
            columnas = armar_query_columna(columnas, "iteraciones", false);
            valores = armar_query_valores(valores, iteraciones, false);
            //producto_1
            int index = 1;
            for (int fila = 0; fila < productos_proveedor.Rows.Count - 1; fila++)
            {
                if (productos_proveedor.Rows[fila]["total"].ToString() != "0")
                {
                    id = productos_proveedor.Rows[fila]["id"].ToString();
                    columnas = armar_query_columna(columnas, "producto_" + index.ToString(), false);
                    dato = id + "-" + productos_proveedor.Rows[fila]["total"].ToString();
                    valores = armar_query_valores(valores, dato, false);
                    index++;
                }

            }
            int ultima_fila = productos_proveedor.Rows.Count - 1;
            id = productos_proveedor.Rows[ultima_fila]["id"].ToString();
            columnas = armar_query_columna(columnas, "producto_" + index.ToString(), true);
            dato = id + "-" + productos_proveedor.Rows[ultima_fila]["total"].ToString();
            valores = armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "totales_y_promedios_de_pedidos", columnas, valores);
        }
        private void actualizar_totales_hasta_el_mes_BD(string id_totales_hasta_mes, int fila_totales_hasta_el_mes, string iteraciones)
        {
            string actualizar;
            string id_producto;
            string total_registrado, total;
            int columna_totales_hasta_el_mes;
            string dato;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {

                id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                columna_totales_hasta_el_mes = buscar_columna_totales_hasta_el_mes(id_producto, fila_totales_hasta_el_mes, totales_completo);
                if (columna_totales_hasta_el_mes != -1)
                {

                    total_registrado = funciones.obtener_dato(totales_completo.Rows[fila_totales_hasta_el_mes][columna_totales_hasta_el_mes].ToString(), 2);
                    if (columna_totales_hasta_el_mes != -1)
                    {

                        total = productos_proveedor.Rows[fila]["total"].ToString();
                        if (total != total_registrado)
                        {
                            //actualizar
                            dato = id_producto + "-" + total.ToString();
                            actualizar = "`" + totales_completo.Columns[columna_totales_hasta_el_mes].ColumnName + "` = '" + dato + "'";
                            consultas.actualizar_tabla(base_de_datos, "totales_y_promedios_de_pedidos", actualizar, id_totales_hasta_mes);
                        }

                        actualizar = "`iteraciones` = '" + iteraciones + "'";
                        consultas.actualizar_tabla(base_de_datos, "totales_y_promedios_de_pedidos", actualizar, id_totales_hasta_mes);
                    }

                }


            }
        }
        private int buscar_columna_totales_hasta_el_mes(string id_producto, int fila_totales_hasta_el_mes, DataTable dt)
        {
            int retorno = -1;
            int columna = dt.Columns["producto_1"].Ordinal;
            while (columna <= dt.Columns.Count - 1)
            {
                if (id_producto == funciones.obtener_dato(dt.Rows[fila_totales_hasta_el_mes][columna].ToString(), 1))
                {
                    retorno = columna;
                    break;
                }
                columna++;
            }
            return retorno;
        }
        #endregion

        #region metodos estadisticas
        #region porcentajes
        private void calcular_promedios(string nombre_proveedor, string mes, string año)
        {
            //consultar todos los totales
            consultar_totales_completo(nombre_proveedor);

            int fila_totales_hasta_el_mes = buscar_fila_totales_hasta_el_mes(mes, año, nombre_proveedor);
            int columna_totales_hasta_el_mes = 0;
            string id_producto;
            double promedio_del_mes, cantidad_total, iteraciones;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {


                id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                columna_totales_hasta_el_mes = buscar_columna_totales_hasta_el_mes(id_producto, fila_totales_hasta_el_mes, totales_completo);
                if (columna_totales_hasta_el_mes != -1)
                {

                    iteraciones = double.Parse(obtener_dato(totales_completo.Rows[fila_totales_hasta_el_mes][columna_totales_hasta_el_mes].ToString(), 2));
                    cantidad_total = double.Parse(obtener_dato(totales_completo.Rows[fila_totales_hasta_el_mes][columna_totales_hasta_el_mes].ToString(), 3));
                    promedio_del_mes = Math.Ceiling(cantidad_total / iteraciones);
                    productos_proveedor.Rows[fila]["promedio_del_mes"] = promedio_del_mes.ToString();
                }


            }
            //sumar promedio hasta mes y año actual
            //registrar si es nuevo, actualizar si existe
            if (totales_segun_fecha != null)
            {

                if (-1 == buscar_fila_de_promedio())
                {
                    //crear
                    crear_promedios_en_BD(año, mes, nombre_proveedor);
                }
                else
                {
                    //actualizar
                    actualizar_promedio_en_BD(nombre_proveedor);
                }
            }

        }
        private int buscar_fila_de_promedio()
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= totales_segun_fecha.Rows.Count - 1)
            {
                if ("promedio_del_mes" == totales_segun_fecha.Rows[fila]["tipo_registro"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_de_promedio_segun_proveedor(string proveedor)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= totales_segun_fecha.Rows.Count - 1)
            {
                if ("promedio_del_mes" == totales_segun_fecha.Rows[fila]["tipo_registro"].ToString() &&
                    proveedor == totales_segun_fecha.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void calcular_totales_hasta_el_mes(string mes, string año, string nombre_proveedor)
        {
            consultar_totales_completo(nombre_proveedor);

            int fila_totales, columna_totales, iteraciones = 0, iteracionesBD = 0;
            double cantidad_total;
            string id_producto, fecha;

            if (verificar_si_existe_totales_hasta_el_mes(mes, año))//verificar si exite totales_hasta_el_mes
            {
                if (mes == DateTime.Now.Month.ToString() &&
                    año == DateTime.Now.Year.ToString())
                {
                    for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                    {
                        id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                        cantidad_total = 0;
                        fila_totales = 0;
                        columna_totales = 0;
                        iteraciones = 0;
                        while (fila_totales <= totales_completo.Rows.Count - 1)
                        {
                            fecha = totales_completo.Rows[fila_totales]["fecha"].ToString();
                            if (!verificar_fecha(fecha, mes, año))
                            {
                                break;
                            }
                            else
                            {
                                if (totales_completo.Rows[fila_totales]["tipo_registro"].ToString() == "totales_del_mes")
                                {
                                    columna_totales = buscar_columna_totales(id_producto, fila_totales, totales_completo);
                                    if (columna_totales != -1)
                                    {

                                        if (totales_completo.Rows[fila_totales][columna_totales].ToString() != "0")
                                        {
                                            cantidad_total = cantidad_total + double.Parse(funciones.obtener_dato(totales_completo.Rows[fila_totales][columna_totales].ToString(), 2));
                                            iteraciones++;
                                        }
                                    }

                                }
                            }
                            fila_totales++;//promedio_del_mes
                            iteracionesBD = iteraciones;
                        }
                        productos_proveedor.Rows[fila]["total"] = iteraciones.ToString() + "-" + cantidad_total.ToString();
                    }
                    //si exite totales_hasta_el_mes actualizar
                    int fila_totales_hasta_el_mes = buscar_fila_totales_hasta_el_mes(mes, año, nombre_proveedor);
                    string id_totales_hasta_mes = totales_completo.Rows[fila_totales_hasta_el_mes]["id"].ToString();
                    actualizar_totales_hasta_el_mes_BD(id_totales_hasta_mes, fila_totales_hasta_el_mes, iteraciones.ToString());
                }
            }
            else
            {
                for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                {
                    id_producto = productos_proveedor.Rows[fila]["id"].ToString();
                    cantidad_total = 0;
                    fila_totales = 0;
                    columna_totales = 0;
                    iteraciones = 0;
                    while (fila_totales <= totales_completo.Rows.Count - 1)
                    {
                        fecha = totales_completo.Rows[fila_totales]["fecha"].ToString();
                        if (!verificar_fecha(fecha, mes, año))
                        {
                            break;
                        }
                        else
                        {
                            if (totales_completo.Rows[fila_totales]["tipo_registro"].ToString() == "totales_del_mes")
                            {
                                columna_totales = buscar_columna_totales(id_producto, fila_totales, totales_completo);
                                if (columna_totales != -1)
                                {
                                    if (totales_completo.Rows[fila_totales][columna_totales].ToString() != "0")
                                    {
                                        cantidad_total = cantidad_total + double.Parse(funciones.obtener_dato(totales_completo.Rows[fila_totales][columna_totales].ToString(), 2));
                                        iteraciones++;
                                    }
                                }

                            }
                        }
                        fila_totales++;//promedio_del_mes
                        iteracionesBD = iteraciones;

                    }
                    if (cantidad_total == 0)
                    {
                        productos_proveedor.Rows[fila]["total"] = "0";
                    }
                    else
                    {
                        productos_proveedor.Rows[fila]["total"] = iteraciones.ToString() + "-" + cantidad_total.ToString();
                    }
                }
                //si no exite totales_hasta_el_mes crear
                crear_totales_hasta_el_mes_BD(iteracionesBD.ToString(), año, mes, nombre_proveedor);
            }


        }
        private bool verificar_si_existe_totales_hasta_el_mes(string mes, string año)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= totales_completo.Rows.Count - 1)
            {
                if ("totales_hasta_el_mes" == totales_completo.Rows[fila]["tipo_registro"].ToString())
                {
                    if (verificar_fecha_exacta(totales_completo.Rows[fila]["fecha"].ToString(), mes, año))
                    {
                        retorno = true;
                        break;
                    }
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_totales_hasta_el_mes(string mes, string año, string proveedor)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= totales_completo.Rows.Count - 1)
            {
                if ("totales_hasta_el_mes" == totales_completo.Rows[fila]["tipo_registro"].ToString() &&
                    proveedor == totales_completo.Rows[fila]["proveedor"].ToString())
                {
                    if (verificar_fecha_exacta(totales_completo.Rows[fila]["fecha"].ToString(), mes, año))
                    {
                        retorno = fila;
                        break;
                    }
                }
                fila++;
            }
            return retorno;
        }
        #endregion
        #region totales
        private void calcular_totales(string nombre_proveedor, string mes, string año)
        {

            if (verificar_si_existen_pedidos())//verificar si exiten pedidos en el mes
            {
                consultar_totales_segun_fecha(nombre_proveedor, mes, año);

                calcular_totales_del_mes();

                if (verificar_si_existen_totales())//si existen totales modificar
                {
                    //verificar que totales son diferentes y actualizar en tales casos
                    actualizar_totales_en_BD(nombre_proveedor);
                }
                else//si no existen totales crear totales
                {
                    //crear totales del mes en BD
                    crear_totales_en_BD(año, mes, nombre_proveedor);
                }
            }
        }
        private int buscar_fila_de_totales()
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= totales_segun_fecha.Rows.Count - 1)
            {
                if ("totales_del_mes" == totales_segun_fecha.Rows[fila]["tipo_registro"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_de_totales_segun_proveedor(string proveedor)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= totales_segun_fecha.Rows.Count - 1)
            {
                if ("totales_del_mes" == totales_segun_fecha.Rows[fila]["tipo_registro"].ToString() &&
                    proveedor == totales_segun_fecha.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void calcular_totales_del_mes()
        {
            int fila_producto;
            double cantidad_pedido, cantidad_total;
            string id_producto;
            for (int fila = 0; fila <= pedidos_segun_fecha.Rows.Count - 1; fila++)
            {
                for (int columna = pedidos_segun_fecha.Columns["producto_1"].Ordinal; columna <= pedidos_segun_fecha.Columns.Count - 1; columna++)
                {
                    if (IsNotDBNull(pedidos_segun_fecha.Rows[fila][columna]))
                    {
                        if (pedidos_segun_fecha.Rows[fila][columna].ToString() != string.Empty)
                        {
                            cantidad_pedido = double.Parse(obtener_dato(pedidos_segun_fecha.Rows[fila][columna].ToString(), 4));
                            id_producto = obtener_dato(pedidos_segun_fecha.Rows[fila][columna].ToString(), 2);
                            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);//buscar_fila_producto(id_producto, productos_proveedor);

                            cantidad_total = double.Parse(productos_proveedor.Rows[fila_producto]["totales_del_mes"].ToString());
                            cantidad_total = cantidad_total + cantidad_pedido;
                            productos_proveedor.Rows[fila_producto]["totales_del_mes"] = cantidad_total.ToString();
                        }
                    }
                }
            }
        }
        private bool verificar_si_existen_pedidos()
        {
            bool retorno = false;
            if (0 < pedidos_segun_fecha.Rows.Count)
            {
                retorno = true;
            }
            return retorno;
        }
        private bool verificar_si_existen_totales()
        {
            bool retorno = false;
            if (0 < totales_segun_fecha.Rows.Count)
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
        #endregion

        #region resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("totales_del_mes", typeof(string)); 
            resumen.Columns.Add("totales_del_mes_dato", typeof(string)); 
            resumen.Columns.Add("promedio_del_mes", typeof(string));
            resumen.Columns.Add("unidad_de_medida", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("stock", typeof(List<string>));
            resumen.Columns.Add("presentacion", typeof(string));
        }
        #endregion

        #region metodos consultas
        private void consultar_promedio_del_mes(string nombre_proveedor, string mes, string año)
        {
            promedio_del_mes = consultas.consultar_promedio_segun_mes_y_año(base_de_datos, "totales_y_promedios_de_pedidos", nombre_proveedor, mes, año);
        }
        private void consultar_totales_completo(string nombre_proveedor)
        {
            totales_completo = consultas.consultar_totales_completo(base_de_datos, "totales_y_promedios_de_pedidos", nombre_proveedor);
            //ordenar totales por meses
            totales_completo.DefaultView.Sort = "fecha asc";
            totales_completo = totales_completo.DefaultView.ToTable();
        }
        private void consultar_pedidos_segun_fecha(string proveedor, string mes, string año)
        {
            pedidos_segun_fecha = consultas.consultar_pedido_segun_mes_y_año(base_de_datos, "pedidos", proveedor, mes, año);
        }
        private void consultar_totales_segun_fecha(string proveedor, string mes, string año)
        {
            totales_segun_fecha = consultas.consultar_totales_segun_mes_y_año(base_de_datos, "totales_y_promedios_de_pedidos", proveedor, mes, año);
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, nombre_proveedor);
            productos_proveedor.Columns.Add("totales_del_mes", typeof(string));
            productos_proveedor.Columns.Add("total", typeof(string));
            productos_proveedor.Columns.Add("promedio_del_mes", typeof(string));
            productos_proveedor.Columns.Add("iteraciones", typeof(string));
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                productos_proveedor.Rows[fila]["totales_del_mes"] = "0";
                productos_proveedor.Rows[fila]["total"] = "0";
                productos_proveedor.Rows[fila]["promedio_del_mes"] = "0";
                productos_proveedor.Rows[fila]["iteraciones"] = "0";
            }
        }
        private void consultar_insumos_proveedor()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
            insumos_fabrica.Columns.Add("totales_del_mes", typeof(string));
            insumos_fabrica.Columns.Add("total", typeof(string));
            insumos_fabrica.Columns.Add("promedio_del_mes", typeof(string));
            insumos_fabrica.Columns.Add("iteraciones", typeof(string));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                insumos_fabrica.Rows[fila]["totales_del_mes"] = "0";
                insumos_fabrica.Rows[fila]["total"] = "0";
                insumos_fabrica.Rows[fila]["promedio_del_mes"] = "0";
                insumos_fabrica.Rows[fila]["iteraciones"] = "0";
            }
        }
        #endregion

        #region metodos get/set
        public DataTable get_promedio_del_mes(string nombre_proveedor, string mes, string año)
        {
            consultar_promedio_del_mes(nombre_proveedor, mes, año);
            return promedio_del_mes;
        }
        public DataTable get_pedidos_segun_fecha(string nombre_proveedor, string mes, string año)
        {
            consultar_pedidos_segun_fecha(nombre_proveedor, mes, año);
            return pedidos_segun_fecha;
        }
        public DataTable get_productos_proveedor(string año, string mes)
        {

            consultar_productos_proveedor("proveedor_villaMaipu");
            consultar_insumos_proveedor();
            cargar_resumen();
            consultar_pedidos_segun_fecha("proveedor_villaMaipu", mes, año);
            if (verificar_si_existen_pedidos())
            {
                consultar_totales_segun_fecha("proveedor_villaMaipu", mes, año);
                cargar_totales_y_promedios_del_mes("proveedor_villaMaipu");
            }
            consultar_pedidos_segun_fecha("insumos_fabrica", mes, año);
            if (verificar_si_existen_pedidos())
            {
                consultar_totales_segun_fecha("insumos_fabrica", mes, año);
                cargar_totales_y_promedios_del_mes("insumos_fabrica");
            }
            return resumen;
        }
        #endregion

        #region metodos interfaz
        private void cargar_resumen()
        {
            crear_tabla_resumen();
            int fila_resumen = 0;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                if (productos_proveedor.Rows[fila]["activa"].ToString() == "1")
                {
                    resumen.Rows.Add();
                    resumen.Rows[fila_resumen]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_proveedor.Rows[fila]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                    resumen.Rows[fila_resumen]["totales_del_mes"] = productos_proveedor.Rows[fila]["totales_del_mes"].ToString();
                    resumen.Rows[fila_resumen]["promedio_del_mes"] = productos_proveedor.Rows[fila]["promedio_del_mes"].ToString();
                    resumen.Rows[fila_resumen]["unidad_de_medida"] = productos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = "proveedor_villaMaipu";
                    
                    resumen.Rows[fila_resumen]["stock"] = obtener_stock(fila, "proveedor_villaMaipu", productos_proveedor); //obtener_stock
                    resumen.Rows[fila_resumen]["presentacion"] = productos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();


                    fila_resumen++;
                }

            }

            if (insumos_fabrica != null)
            {

                for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
                {
                    if (insumos_fabrica.Rows[fila]["activa"].ToString() == "1")
                    {

                        resumen.Rows.Add();
                        resumen.Rows[fila_resumen]["id"] = insumos_fabrica.Rows[fila]["id"].ToString();
                        resumen.Rows[fila_resumen]["tipo_producto"] = insumos_fabrica.Rows[fila]["tipo_producto"].ToString();
                        resumen.Rows[fila_resumen]["producto"] = insumos_fabrica.Rows[fila]["producto"].ToString();
                        resumen.Rows[fila_resumen]["totales_del_mes"] = insumos_fabrica.Rows[fila]["totales_del_mes"].ToString();
                        resumen.Rows[fila_resumen]["promedio_del_mes"] = insumos_fabrica.Rows[fila]["promedio_del_mes"].ToString();
                        resumen.Rows[fila_resumen]["unidad_de_medida"] = insumos_fabrica.Rows[fila]["unidad_medida"].ToString();
                        resumen.Rows[fila_resumen]["proveedor"] = "insumos_fabrica";
                        resumen.Rows[fila_resumen]["stock"] = obtener_stock(fila, "insumos_fabrica", insumos_fabrica); //obtener_stock
                        resumen.Rows[fila_resumen]["presentacion"] = insumos_fabrica.Rows[fila]["producto_1"].ToString();

                        fila_resumen++;
                    }

                }
            }

        }
        private List<string> obtener_stock(int fila_producto, string proveedor, DataTable productos)
        {
            List<string> list = new List<string>();
            string dato, tipo_paquete, cant_unidad, unidad, cantidad;
            if (proveedor == "insumos_fabrica")
            {
                for (int columna = productos.Columns["producto_1"].Ordinal; columna <= productos.Columns.Count - 1; columna++)
                {
                    if (productos.Rows[fila_producto][columna].ToString() != "N/A" &&
                        productos.Rows[fila_producto][columna].ToString() != "0")
                    {
                        tipo_paquete = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 1);
                        cant_unidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 2);
                        unidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 3);
                        cantidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 4);
                        double cant = double.Parse(cantidad);
                        if (cant < 0)
                        {
                            cantidad="0";
                        }
                        if (tipo_paquete == "Unidad")
                        {
                            dato = cantidad + "- x" + cant_unidad + "" + unidad;
                            string control = productos.Rows[fila_producto][columna].ToString();
                            list.Add(dato);
                        }
                        else
                        {
                            dato = cantidad + "- " + tipo_paquete + " x" + cant_unidad + " " + unidad;
                            string control = productos.Rows[fila_producto][columna].ToString();
                            list.Add(dato);
                        }
                    }
                }
            }
            else if (proveedor == "proveedor_villaMaipu")
            {
                double cant = double.Parse(productos.Rows[fila_producto]["stock"].ToString());
                if (cant < 0)
                {
                    cant = 0;
                }
                dato = cant.ToString() + "- " + productos.Rows[fila_producto]["unidad_de_medida_fabrica"].ToString();
                list.Add(dato);
            }
            return list;
        }
        private void cargar_totales_y_promedios_del_mes(string proveedor)
        {

            string id;
            int fila_totales = buscar_fila_de_totales();
            int fila_promedio = buscar_fila_de_promedio();
            int columna_totales, columna_promedio;
            double totales_del_mes, promedio_del_mes;
            string dato, tipo_paquete, cant_unidad, unidad, cantidad;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {
                if (proveedor == resumen.Rows[fila]["proveedor"].ToString())
                {
                    fila_totales = buscar_fila_de_totales_segun_proveedor(resumen.Rows[fila]["proveedor"].ToString());
                    fila_promedio = buscar_fila_de_promedio_segun_proveedor(resumen.Rows[fila]["proveedor"].ToString());
                    if (fila_totales != -1 &&
                        fila_promedio != -1)
                    {
                        id = resumen.Rows[fila]["id"].ToString();
                        columna_totales = buscar_columna_totales(id, fila_totales, totales_segun_fecha);
                        columna_promedio = buscar_columna_totales(id, fila_promedio, totales_segun_fecha);
                        if (columna_totales != -1)
                        {
                            if (proveedor == "insumos_fabrica")
                            {
                                tipo_paquete = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 1);
                                cant_unidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 2);
                                unidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 3);
                                cantidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 4);
                                if (tipo_paquete == "Unidad")
                                {
                                    dato = " x" + cant_unidad + "" + unidad;
                                }
                                else
                                {
                                    dato = " " + tipo_paquete + " x" + cant_unidad + " " + unidad;

                                }
                                totales_del_mes = double.Parse(funciones.obtener_dato(totales_segun_fecha.Rows[fila_totales][columna_totales].ToString(), 2));
                                resumen.Rows[fila]["totales_del_mes"] = totales_del_mes.ToString() + " " + dato; 
                                resumen.Rows[fila]["totales_del_mes_dato"] = totales_del_mes.ToString();
                               

                            }
                            else
                            {
                                resumen.Rows[fila]["totales_del_mes"] = funciones.obtener_dato(totales_segun_fecha.Rows[fila_totales][columna_totales].ToString(), 2) + " " + resumen.Rows[fila]["presentacion"].ToString(); //presentacion
                                resumen.Rows[fila]["totales_del_mes_dato"] = funciones.obtener_dato(totales_segun_fecha.Rows[fila_totales][columna_totales].ToString(), 2);
                               

                            }
                        }
                        if (columna_promedio != -1)
                        {

                            if (proveedor == "insumos_fabrica")
                            {
                                tipo_paquete = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 1);
                                cant_unidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 2);
                                unidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 3);
                                cantidad = funciones.obtener_dato(resumen.Rows[fila]["presentacion"].ToString(), 4);
                                if (tipo_paquete == "Unidad")
                                {
                                    dato = " x" + cant_unidad + "" + unidad;
                                }
                                else
                                {
                                    dato = " " + tipo_paquete + " x" + cant_unidad + " " + unidad;

                                }
                                promedio_del_mes = double.Parse(funciones.obtener_dato(totales_segun_fecha.Rows[fila_promedio][columna_promedio].ToString(), 2));
                                resumen.Rows[fila]["promedio_del_mes"] = promedio_del_mes.ToString() + " " + dato;
                            }
                            else
                            {
                                resumen.Rows[fila]["promedio_del_mes"] = obtener_dato(totales_segun_fecha.Rows[fila_promedio][columna_promedio].ToString(), 2) + " " + resumen.Rows[fila]["presentacion"].ToString();
                            }
                        }

                    }
                }
            }
        }
        #endregion


        #region funciones
        private int buscar_fila_producto(string id, DataTable productos)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= productos.Rows.Count - 1)
            {
                if (id == productos.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private string obtener_dato(string dato, int posicion_dato)
        {
            string retorno = "";
            int posicion = 0;
            int i = 0;
            while (i <= dato.Length - 1)
            {
                if (dato[i].ToString() == "-")
                {
                    posicion++;
                    if (posicion == posicion_dato)
                    {
                        break;
                    }
                    else
                    {
                        retorno = "";
                    }
                }
                else
                {
                    retorno = retorno + dato[i].ToString();
                }
                i++;
            }
            return retorno;
        }
        private bool IsNotDBNull(object valor)
        {
            bool retorno = false;
            if (!DBNull.Value.Equals(valor))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year < int.Parse(año_dato))
            {
                retorno = true;
            }
            else if (fecha_1.Year == int.Parse(año_dato))
            {
                if (fecha_1.Month <= int.Parse(mes_dato))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
        public bool verificar_fecha_exacta(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year == int.Parse(año_dato))
            {
                if (fecha_1.Month == int.Parse(mes_dato))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
        private string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
        {
            string retorno = "";
            string separador_columna = "`";

            retorno = columnas + separador_columna + columna_valor + separador_columna;

            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        private string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
        {
            string retorno = "";
            string separador_valores = "'";

            retorno = valores + separador_valores + valor_a_insertar + separador_valores;
            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        #endregion
    }
}


