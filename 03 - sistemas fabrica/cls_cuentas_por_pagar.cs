using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_cuentas_por_pagar
    {
        public cls_cuentas_por_pagar(DataTable usuario_BD)
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
        #region cargar nota
        public void cargar_nota(string id_orden, string nota)
        {
            string actualizar = "`nota` = '" + nota + "' ";
            consultas.actualizar_tabla(base_de_datos, "imputaciones_fabrica", actualizar, id_orden);
        }
        public void cargar_nota_credito(string proveedor, string nota, string valor_remito, string fecha_nota)
        {
            
            string columnas = "";
            string valores = "";
            //fabrica
            columnas = funciones.armar_query_columna(columnas, "fabrica", false);
            valores = funciones.armar_query_valores(valores, "Fabrica villa maipu", false);
            //num_orden
            columnas = funciones.armar_query_columna(columnas, "num_orden", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //valor_orden
            columnas = funciones.armar_query_columna(columnas, "valor_orden", false);
            valores = funciones.armar_query_valores(valores, valor_remito, false);
            //fecha_remito
            columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
            valores = funciones.armar_query_valores(valores, fecha_nota, false);
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", false);
            valores = funciones.armar_query_valores(valores, proveedor, false);
            //id_proveedor
            columnas = funciones.armar_query_columna(columnas, "id_proveedor", true);
            valores = funciones.armar_query_valores(valores, get_id_proveedor(proveedor), true);

            consultas.insertar_en_tabla(base_de_datos, "cuenta_por_pagar_fabrica", columnas, valores);
        }
        #endregion
        #region PDF
        public void crear_PDF_orden_de_compra_con_precio(string ruta_archivo, byte[] logo, string id_orden, string id_factura)//
        {
            consultar_orden_de_compra_por_id(id_factura);
            DataTable factura = consultas.consultar_cuentas_por_pagar_fabrica_por_id(base_de_datos, id_orden);
            consultar_proveedores_de_fabrica_seleccionado(orden_compra.Rows[0]["id_proveedor"].ToString());

            abrir_orden();
            string num_orden;
            DateTime fecha_pedido, fecha_estimada;
            num_orden = orden_compra.Rows[0]["id"].ToString();
            fecha_pedido = (DateTime)orden_compra.Rows[0]["fecha"];
            fecha_estimada = (DateTime)orden_compra.Rows[0]["fecha_entrega_estimada"];
            PDF.GenerarPDF_orden_de_compra_con_precio(ruta_archivo, logo, resumen_orden, proveedores_de_fabrica_seleccionado, num_orden, fecha_pedido.ToString("dd/MM/yyyy"), fecha_estimada.ToString("dd/MM/yyyy"), calcular_total_orden(), factura.Rows[0]["impuestos"].ToString());
            //            PDF.GenerarPDF_orden_de_compra()
        }
        private string calcular_total_orden()
        {
            double total = 0;
            for (int fila = 0; fila <= resumen_orden.Rows.Count - 1; fila++)
            {
                total = total + double.Parse(resumen_orden.Rows[fila]["sub_total_dato"].ToString());
            }
            return total.ToString();
        }
        public void crear_PDF_orden_de_compra(string ruta_archivo, byte[] logo, string id_orden)//
        {
            consultar_orden_de_compra_por_id(id_orden);

            consultar_proveedores_de_fabrica_seleccionado(orden_compra.Rows[0]["id_proveedor"].ToString());

            abrir_orden();
            string num_orden, fecha_pedido, fecha_estimada;
            num_orden = orden_compra.Rows[0]["id"].ToString();
            fecha_pedido = orden_compra.Rows[0]["fecha"].ToString();
            fecha_estimada = orden_compra.Rows[0]["fecha_entrega_estimada"].ToString();
            PDF.GenerarPDF_orden_de_compra(ruta_archivo, logo, resumen_orden, proveedores_de_fabrica_seleccionado, num_orden, fecha_pedido, fecha_estimada);
            //            PDF.GenerarPDF_orden_de_compra()
        }
        private void abrir_orden()
        {
            crear_tabla_resumen();
            string id, producto, cantidad_pedida, cantidad, tipo_paquete, cantidad_unidades, unidad_medida, unidad_de_medida, precio;

            for (int columna = orden_compra.Columns["producto_1"].Ordinal; columna <= orden_compra.Columns.Count - 1; columna++)
            {
                if (orden_compra.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 2);
                    precio = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 3);

                    tipo_paquete = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 4);
                    cantidad_unidades = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 5);
                    unidad_medida = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 6);

                    if (tipo_paquete == "Unidad")
                    {
                        unidad_de_medida = " x " + cantidad_unidades + unidad_medida;//cantidad_unidades + " " +
                    }
                    else
                    {
                        unidad_de_medida = tipo_paquete + " x " + cantidad_unidades + " " + unidad_medida;
                    }
                    cantidad_pedida = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 7);
                    cantidad = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 8);
                    if (cantidad == "N/A")
                    {
                        cantidad = "0";
                    }
                    cargar_producto(id, producto, cantidad_pedida, cantidad, unidad_de_medida, precio);
                }
            }
        }
        private void cargar_producto(string id, string producto, string cantidad_pedida, string cantidad, string unidad_de_medida, string precio)
        {
            resumen_orden.Rows.Add();
            int ultima_fila = resumen_orden.Rows.Count - 1;

            resumen_orden.Rows[ultima_fila]["id"] = id;
            resumen_orden.Rows[ultima_fila]["producto"] = producto;
            resumen_orden.Rows[ultima_fila]["cantidad_pedida"] = cantidad_pedida;
            resumen_orden.Rows[ultima_fila]["cantidad"] = cantidad;
            resumen_orden.Rows[ultima_fila]["precio"] = funciones.formatCurrency(double.Parse(precio));
            resumen_orden.Rows[ultima_fila]["sub_total"] = funciones.formatCurrency(double.Parse(cantidad) * double.Parse(precio));
            resumen_orden.Rows[ultima_fila]["sub_total_dato"] = Math.Round(double.Parse(cantidad) * double.Parse(precio), 2).ToString();



            resumen_orden.Rows[ultima_fila]["unidad de medida"] = unidad_de_medida;
        }
        private void crear_tabla_resumen()
        {
            resumen_orden = new DataTable();
            resumen_orden.Columns.Add("id", typeof(string));
            resumen_orden.Columns.Add("producto", typeof(string));
            resumen_orden.Columns.Add("cantidad_pedida", typeof(string));
            resumen_orden.Columns.Add("cantidad", typeof(string));
            resumen_orden.Columns.Add("unidad de medida", typeof(string));
            resumen_orden.Columns.Add("precio", typeof(string));
            resumen_orden.Columns.Add("sub_total", typeof(string));
            resumen_orden.Columns.Add("sub_total_dato", typeof(string));
        }
        #endregion

        #region atributos
        cls_consultas_Mysql consultas;
        cls_PDF PDF = new cls_PDF();
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable lista_proveedores_fabrica;
        DataTable cuenta_por_pagar_fabrica;
        DataTable imputaciones;
        DataTable todas_las_cuenta_por_pagar_fabrica;
        DataTable todas_las_imputaciones_fabrica;
        DataTable orden_compra;
        DataTable proveedores_de_fabrica_seleccionado;
        DataTable resumen_orden;
        DataTable proveedor_fabrica_seleccionado;
        #endregion

        #region metodos consultas

        private void consultar_orden_de_compra_por_id(string id)
        {
            orden_compra = consultas.consultar_ordenes_de_compra_de_proveedor_por_id(id);
        }
        private void consultar_proveedores_de_fabrica_seleccionado(string id_proveedor)
        {
            proveedores_de_fabrica_seleccionado = consultas.consultar_proveedores_de_fabrica_seleccionado(id_proveedor);
        }
        private void consultar_cuenta_por_pagar_fabrica(string id_proveedor)
        {
            cuenta_por_pagar_fabrica = consultas.consultar_cuentas_por_pagar_fabrica(base_de_datos, id_proveedor);
        }
        private void consultar_lista_proveedores_fabrica()
        {
            lista_proveedores_fabrica = consultas.consultar_tabla_completa(base_de_datos, "proveedores_de_fabrica");
            lista_proveedores_fabrica.DefaultView.Sort = "proveedor ASC";
            lista_proveedores_fabrica = lista_proveedores_fabrica.DefaultView.ToTable();    
        }
        private void consultar_imputaciones()
        {
            imputaciones = consultas.consultar_tabla(base_de_datos, "imputaciones_fabrica");
        }
        private void consultar_todas_las_cuenta_por_pagar_fabrica()
        {
            todas_las_cuenta_por_pagar_fabrica = consultas.consultar_tabla(base_de_datos, "cuenta_por_pagar_fabrica");
        }
        private void consultar_todas_las_imputaciones_fabrica()
        {
            todas_las_imputaciones_fabrica = consultas.consultar_tabla(base_de_datos, "imputaciones_fabrica");
        }
        private void consultar_proveedor_fabrica_seleccionado(string id_proveedor)
        {
            proveedor_fabrica_seleccionado = consultas.consultar_proveeedor_de_fabrica_por_id(id_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_proveedor_fabrica_seleccionado(string nombre_proveedor)
        {
            string id_proveedor = obtener_id_proveedor(nombre_proveedor);
            consultar_proveedor_fabrica_seleccionado(id_proveedor);
            return proveedor_fabrica_seleccionado;
        }
        public DataTable get_cuenta_por_pagar_fabrica(string nombre_proveedor)
        {
            string id_proveedor = obtener_id_proveedor(nombre_proveedor);
            consultar_cuenta_por_pagar_fabrica(id_proveedor);
            cuenta_por_pagar_fabrica.DefaultView.Sort = " fecha_remito DESC";
            cuenta_por_pagar_fabrica = cuenta_por_pagar_fabrica.DefaultView.ToTable();
            return cuenta_por_pagar_fabrica;
        }
        public DataTable get_lista_proveedores_fabrica()
        {
            consultar_lista_proveedores_fabrica();
            lista_proveedores_fabrica.DefaultView.Sort = "proveedor ASC";
            lista_proveedores_fabrica = lista_proveedores_fabrica.DefaultView.ToTable();
            return lista_proveedores_fabrica;
        }
        public string get_id_proveedor(string nombre_proveedor)
        {
            return obtener_id_proveedor(nombre_proveedor);
        }
        public DataTable get_imputaciones()
        {
            consultar_imputaciones();
            return imputaciones;
        }
        public string get_id(string id_factura, string proveedor)
        {
            string retorno = "-1";
            consultar_lista_proveedores_fabrica();
            string id_proveedor = obtener_id_proveedor(proveedor);
            consultar_cuenta_por_pagar_fabrica(id_proveedor);
            for (int fila = 0; fila <= cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                if (cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString() == id_proveedor &&
                    cuenta_por_pagar_fabrica.Rows[fila]["num_orden"].ToString() == id_factura)
                {
                    retorno = cuenta_por_pagar_fabrica.Rows[fila]["id"].ToString();
                    break;
                }
            }
            return retorno;
        }
        #endregion

        #region metodos publicos
        public double calcular_deuda_mes(string proveedor_seleccionado, string mes, string año)
        {
            consultar_lista_proveedores_fabrica();
            double retorno, compra, pagado, pagado_subTotal, deuda_mes_anterior;
            retorno = 0;
            deuda_mes_anterior = calcular_deuda_mes_anterior(proveedor_seleccionado, mes, año);
            string id_proveedor = obtener_id_proveedor(proveedor_seleccionado);
            compra = 0;
            for (int fila = 0; fila <= cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha(cuenta_por_pagar_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(cuenta_por_pagar_fabrica.Rows[fila]["valor_orden"].ToString());
                }
            }

            pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(imputaciones.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["transferencia"].ToString()) + double.Parse(imputaciones.Rows[fila]["mercado_pago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            if (proveedor_seleccionado == "Tres Arroyos")
            {
                string stop = "";
            }
            retorno = compra - pagado;
            retorno = retorno + deuda_mes_anterior;
            return Math.Round(retorno);

        }
        public double calcular_deuda_mes_anterior(string proveedor_seleccionado, string mes, string año)
        {
            double retorno, compra, pagado, pagado_subTotal;
            retorno = 0;
            string id_proveedor = obtener_id_proveedor(proveedor_seleccionado);
            consultar_cuenta_por_pagar_fabrica(id_proveedor);
            consultar_imputaciones();
            compra = 0;
            for (int fila = 0; fila <= cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha_anterior(cuenta_por_pagar_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(cuenta_por_pagar_fabrica.Rows[fila]["valor_orden"].ToString());
                }
            }

            pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(imputaciones.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha_anterior(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["transferencia"].ToString()) + double.Parse(imputaciones.Rows[fila]["mercado_pago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno;
        }

        public double deuda_total_del_mes(string proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            // double deuda_total_mes_anterior = deuda_total_del_mes_anterior(proveedor_seleccionado, mes, año);
            consultar_todas_las_cuenta_por_pagar_fabrica();
            consultar_todas_las_imputaciones_fabrica();

            double compra = 0;
            for (int fila = 0; fila <= todas_las_cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                compra = compra + double.Parse(todas_las_cuenta_por_pagar_fabrica.Rows[fila]["valor_orden"].ToString());

            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones_fabrica.Rows.Count - 1; fila++)
            {

                pagado_subTotal = double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["transferencia"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["mercado_pago"].ToString());
                pagado = pagado + pagado_subTotal;

            }
            retorno = (compra - pagado);
            return retorno;
        }
        public double deuda_total_del_mes_anterior(string proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            consultar_todas_las_cuenta_por_pagar_fabrica();
            consultar_todas_las_imputaciones_fabrica();

            double compra = 0;
            for (int fila = 0; fila <= todas_las_cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(todas_las_cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha_anterior(todas_las_cuenta_por_pagar_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todas_las_cuenta_por_pagar_fabrica.Rows[fila]["valor_orden"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones_fabrica.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(todas_las_imputaciones_fabrica.Rows[fila]["id_proveedor"].ToString(), proveedor_seleccionado) && verificar_fecha_anterior(todas_las_imputaciones_fabrica.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["transferencia"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["mercado_pago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = compra - pagado;
            return retorno;
        }
        public double calcular_Total_compra_del_mes(string nombre_proveedor, string mes, string año)
        {
            double retorno = 0;
            for (int fila = 0; fila <= cuenta_por_pagar_fabrica.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(cuenta_por_pagar_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año) &&
                    cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString() == get_id_proveedor(nombre_proveedor))
                {
                    retorno = retorno + double.Parse(cuenta_por_pagar_fabrica.Rows[fila]["valor_orden"].ToString());
                }
            }
            return retorno;
        }
        public double pago_total_del_mes(string nombre_proveedor, string mes, string año)
        {
            double retorno = 0;
            consultar_imputaciones();

            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (verificar_proveedor(imputaciones.Rows[fila]["id_proveedor"].ToString(), nombre_proveedor) && verificar_fecha(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["transferencia"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["mercado_pago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = pagado;
            return retorno;
        }
        public string sumar_imputacion(string efectivo_dato, string transferencia_dato, string mercado_pago_dato)
        {

            return funciones.formatCurrency(suma_de_imputacion(efectivo_dato, transferencia_dato, mercado_pago_dato));

        }
        public bool verificar_proveedor(string id_proveedor, string proveedor_seleccionado)
        {
            bool retorno = false;
            string id_proveedor_seleccionado = obtener_id_proveedor(proveedor_seleccionado);
            if (id_proveedor == id_proveedor_seleccionado)
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha(string fecha_dato, string mes, string año)
        {
            return verificar_fecha_actual(fecha_dato, mes, año);
        }
        private bool verificar_fecha_actual(string fecha_dato, string mes, string año)
        {
            bool retorno = false;
            System.DateTime fecha = DateTime.Parse(fecha_dato);
            if (mes == fecha.Month.ToString() && año == fecha.Year.ToString())
            {
                retorno = true;
            }
            return retorno;
        }
        private bool verificar_fecha_anterior(string fecha_dato, string mes, string año)
        {
            bool retorno = false;
            System.DateTime fecha = DateTime.Parse(fecha_dato);

            if (fecha.Year < int.Parse(año))
            {
                retorno = true;
            }
            else if (fecha.Year.ToString() == año && fecha.Month < int.Parse(mes))
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
        #region carga a base de datos
        public void marcar_factura_como_pagada(string id_orden)
        {
            string actualizar = "`estado_pago` = 'Pagado'";
            consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_orden);
        }
        public void desmarcar_factura_como_pagada(string id_orden)
        {
            string actualizar = "`estado_pago` = 'N/A'";
            consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_orden);
        }
        public void eliminar_imputacion(string id_imputacion)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "imputaciones_fabrica", actualizar, id_imputacion);
        }
        private void cargar_imputacion(string efectivo, string transferencia, string mercado_pago, string id_proveedor, string proveedor_seleccionado)
        {
            string columna = "";
            string valor = "";
            //id_proveedor
            columna = funciones.armar_query_columna(columna, "id_proveedor", false);
            valor = funciones.armar_query_valores(valor, id_proveedor, false);
            //proveedor
            columna = funciones.armar_query_columna(columna, "proveedor", false);
            valor = funciones.armar_query_valores(valor, proveedor_seleccionado, false);
            //efectivo
            columna = funciones.armar_query_columna(columna, "efectivo", false);
            valor = funciones.armar_query_valores(valor, efectivo, false);
            //transferencia
            columna = funciones.armar_query_columna(columna, "transferencia", false);
            valor = funciones.armar_query_valores(valor, transferencia, false);
            //mercado_pago
            columna = funciones.armar_query_columna(columna, "mercado_pago", false);
            valor = funciones.armar_query_valores(valor, mercado_pago, false);
            //fecha   
            columna = funciones.armar_query_columna(columna, "fecha", true);
            valor = funciones.armar_query_valores(valor, funciones.get_fecha(), true);
            consultas.insertar_en_tabla(base_de_datos, "imputaciones_fabrica", columna, valor);
        }
        #endregion
        #region funciones
        private string obtener_id_proveedor(string nombre_proveedor)
        {
            if (lista_proveedores_fabrica == null)
            {
                consultar_lista_proveedores_fabrica();
            }
            string retorno = "";
            int fila = 0;
            while (fila <= lista_proveedores_fabrica.Rows.Count - 1)
            {
                if (lista_proveedores_fabrica.Rows[fila]["proveedor"].ToString() == nombre_proveedor)
                {
                    retorno = lista_proveedores_fabrica.Rows[fila]["id"].ToString();
                    break;
                }
                fila++;
            }
            return retorno;
        }

        private double suma_de_imputacion(string efectivo_dato, string transferencia_dato, string mercado_pago_dato)
        {
            double efectivo, transferencia, mercado_pago, retorno;


            if (!double.TryParse(efectivo_dato.Replace(",", "."), out efectivo))
            {
                efectivo_dato = string.Empty;
                efectivo = 0;
            }

            if (!double.TryParse(transferencia_dato.Replace(",", "."), out transferencia))
            {
                transferencia_dato = string.Empty;
                transferencia = 0;
            }

            if (!double.TryParse(mercado_pago_dato.Replace(",", "."), out mercado_pago))
            {
                mercado_pago_dato = string.Empty;
                mercado_pago = 0;
            }

            retorno = efectivo + transferencia + mercado_pago;
            return retorno;
        }
        public void enviar_imputacion(string efectivo_dato, string transferencia_dato, string mercado_pago_dato, string proveedor_seleccionado)
        {
            double efectivo, transferencia, mercado_pago;
            string id_proveedor = obtener_id_proveedor(proveedor_seleccionado);


            if (!double.TryParse(efectivo_dato.Replace(",", "."), out efectivo))
            {
                efectivo_dato = string.Empty;
                efectivo = 0;
            }

            if (!double.TryParse(transferencia_dato.Replace(",", "."), out transferencia))
            {
                transferencia_dato = string.Empty;
                transferencia = 0;
            }

            if (!double.TryParse(mercado_pago_dato.Replace(",", "."), out mercado_pago))
            {
                mercado_pago_dato = string.Empty;
                mercado_pago = 0;
            }

            cargar_imputacion(efectivo.ToString(), transferencia.ToString(), mercado_pago.ToString(), id_proveedor, proveedor_seleccionado);
        }
        #endregion
    }
}
