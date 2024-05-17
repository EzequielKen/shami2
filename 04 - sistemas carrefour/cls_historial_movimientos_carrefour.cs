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

namespace _04___sistemas_carrefour
{
    public class cls_historial_movimientos_carrefour
    {
        public cls_historial_movimientos_carrefour(DataTable usuario_BD)
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

        DataTable sucursales_carrefour;
        DataTable movimientos_carrefour_no_calculados;
        DataTable acuerdo_de_precios_carrefour;
        DataTable movimientos_carrefour;
        DataTable imputaciones_carrefour;
        DataTable todos_movimientos_carrefour;
        DataTable todos_imputaciones_carrefour;
        DataTable movimientos_seleccionado;
        DataTable resumen_movimientos;
        #endregion

        #region carga a base de datos

        public void cargar_imputacion(string sucursal, string efectivo, string transferencia, string mercado_pago)
        {
            string columnas = "";
            string valores = "";

            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal, false);
            //efectivo
            columnas = funciones.armar_query_columna(columnas, "efectivo", false);
            valores = funciones.armar_query_valores(valores, efectivo, false);
            //transferencia
            columnas = funciones.armar_query_columna(columnas, "transferencia", false);
            valores = funciones.armar_query_valores(valores, transferencia, false);
            //mercado_pago
            columnas = funciones.armar_query_columna(columnas, "mercado_pago", false);
            valores = funciones.armar_query_valores(valores, mercado_pago, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", true);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), true);

            consultas.insertar_en_tabla(base_de_datos, "imputaciones_carrefour", columnas, valores);

        }

        #endregion

        #region metodos privados
        private void calcular_movimientos_carrefour_no_calculados()
        {
            consultar_movimientos_carrefour_no_calculados();
            consultar_acuerdo_de_precios_carrefour();
            string tipo_de_acuerdo, acuerdo;
            int fila_acuerdo;
            double valor_venta, precio_prodcuto, cant_venta, sub_total;
            string id_producto;
            string id_movimiento, actualizar;
            for (int fila = 0; fila <= movimientos_carrefour_no_calculados.Rows.Count - 1; fila++)
            {
                tipo_de_acuerdo = movimientos_carrefour_no_calculados.Rows[fila]["tipo_acuerdo"].ToString();
                acuerdo = movimientos_carrefour_no_calculados.Rows[fila]["acuerdo"].ToString();
                fila_acuerdo = buscar_fila_acuerdo(tipo_de_acuerdo, acuerdo);
                valor_venta = 0;
                for (int columna = movimientos_carrefour_no_calculados.Columns["producto_1"].Ordinal; columna <= movimientos_carrefour_no_calculados.Columns.Count - 1; columna++)
                {
                    if (movimientos_carrefour_no_calculados.Rows[fila][columna].ToString() != "N/A")
                    {
                        id_producto = funciones.obtener_dato(movimientos_carrefour_no_calculados.Rows[fila][columna].ToString(), 1);

                        precio_prodcuto = double.Parse(acuerdo_de_precios_carrefour.Rows[fila_acuerdo]["producto_" + id_producto].ToString());
                        cant_venta = double.Parse(funciones.obtener_dato(movimientos_carrefour_no_calculados.Rows[fila][columna].ToString(), 7));

                        sub_total = cant_venta * precio_prodcuto;

                        valor_venta = Math.Round(valor_venta + sub_total, 2);
                    }
                }
                id_movimiento = movimientos_carrefour_no_calculados.Rows[fila]["id"].ToString();

                actualizar = "`calculado` = 'Si'";
                consultas.actualizar_tabla(base_de_datos, "movimientos_carrefour", actualizar, id_movimiento);

                actualizar = "`valor_venta` = '" + valor_venta.ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "movimientos_carrefour", actualizar, id_movimiento);
            }
        }
        private int buscar_fila_acuerdo(string tipo_de_acuerdo, string acuerdo)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios_carrefour.Rows.Count - 1)
            {
                if (tipo_de_acuerdo == acuerdo_de_precios_carrefour.Rows[fila]["tipo_de_acuerdo"].ToString() &&
                    acuerdo == acuerdo_de_precios_carrefour.Rows[fila]["acuerdo"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_sucursales_carrefour()
        {
            sucursales_carrefour = consultas.consultar_tabla_completa(base_de_datos, "sucursales_carrefour");
        }
        private void consultar_movimientos_carrefour_no_calculados()
        {
            movimientos_carrefour_no_calculados = consultas.consultar_movimientos_carrefour_no_calculados();
        }
        private void consultar_acuerdo_de_precios_carrefour()
        {
            acuerdo_de_precios_carrefour = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios_carrefour");
        }
        private void consultar_movimientos_carrefour(string sucursal)
        {
            movimientos_carrefour = consultas.consultar_movimientos_carrefour(sucursal);
        }
        private void consultar_imputaciones_carrefour(string sucursal)
        {
            imputaciones_carrefour = consultas.consultar_imputaciones_carrefour_segun_sucursal(sucursal);
        }
        private void consultar_todos_movimientos_carrefour()
        {
            todos_movimientos_carrefour = consultas.consultar_tabla(base_de_datos, "movimientos_carrefour");

        }
        private void consultar_todos_imputaciones_carrefour()
        {
            todos_imputaciones_carrefour = consultas.consultar_tabla(base_de_datos, "imputaciones_carrefour");
        }
        private void consultar_movimientos_seleccionado(string id_movimiento)
        {
            movimientos_seleccionado = consultas.consultar_movimientos_seleccionado(id_movimiento);
        }
        #endregion

        #region tabla resumen
        private void crear_tabla_resumen()
        {
            resumen_movimientos = new DataTable();
            /*
             id
            producto
            stock_inicial
            devolucion
            reposicion
            stock_final
            vendido
             * */
            resumen_movimientos.Columns.Add("id", typeof(string));
            resumen_movimientos.Columns.Add("producto", typeof(string));
            resumen_movimientos.Columns.Add("stock_inicial", typeof(string));
            resumen_movimientos.Columns.Add("devolucion", typeof(string));
            resumen_movimientos.Columns.Add("reposicion", typeof(string));
            resumen_movimientos.Columns.Add("stock_final", typeof(string));
            resumen_movimientos.Columns.Add("vendido", typeof(string));
            resumen_movimientos.Columns.Add("sub_total", typeof(string));
        }
        private void abrir_resumen(int fila_acuerdo)
        {
            crear_tabla_resumen();
            string id, producto, stock_inicial, devolucion, reposicion, stock_final, vendido;
            for (int columna = movimientos_seleccionado.Columns["producto_1"].Ordinal; columna <= movimientos_seleccionado.Columns.Count - 1; columna++)
            {
                if (movimientos_seleccionado.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 2);
                    stock_inicial = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 3);
                    devolucion = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 4);
                    reposicion = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 5);
                    stock_final = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 6);
                    vendido = funciones.obtener_dato(movimientos_seleccionado.Rows[0][columna].ToString(), 7);

                    cargar_producto(fila_acuerdo, id, producto, stock_inicial, devolucion, reposicion, stock_final, vendido);
                }

            }

        }
        private void cargar_producto(int fila_acuerdo, string id, string producto, string stock_inicial, string devolucion, string reposicion, string stock_final, string vendido_dato)
        {
            double precio = double.Parse(acuerdo_de_precios_carrefour.Rows[fila_acuerdo]["producto_" + id].ToString());
            double vendido = double.Parse(vendido_dato);
            double sub_total = precio * vendido;

            resumen_movimientos.Rows.Add();
            int fila = resumen_movimientos.Rows.Count - 1;

            resumen_movimientos.Rows[fila]["id"] = id;
            resumen_movimientos.Rows[fila]["producto"] = producto;
            resumen_movimientos.Rows[fila]["stock_inicial"] = stock_inicial;
            resumen_movimientos.Rows[fila]["devolucion"] = devolucion;
            resumen_movimientos.Rows[fila]["reposicion"] = reposicion;
            resumen_movimientos.Rows[fila]["stock_final"] = stock_final;
            resumen_movimientos.Rows[fila]["vendido"] = vendido_dato;
            resumen_movimientos.Rows[fila]["sub_total"] = funciones.formatCurrency(sub_total);
        }
        #endregion

        #region metodos get/set
        public DataTable get_sucursales_carrefour()
        {
            consultar_sucursales_carrefour();
            return sucursales_carrefour;
        }
        public DataTable get_movimientos_carrefour(string sucursal)
        {
            consultar_movimientos_carrefour(sucursal);
            movimientos_carrefour.DefaultView.Sort = "fecha DESC";
            movimientos_carrefour = movimientos_carrefour.DefaultView.ToTable();
            return movimientos_carrefour;
        }
        public DataTable get_imputaciones_carrefour(string sucursal)
        {
            consultar_imputaciones_carrefour(sucursal);
            imputaciones_carrefour.DefaultView.Sort = "fecha DESC";
            imputaciones_carrefour = imputaciones_carrefour.DefaultView.ToTable();
            return imputaciones_carrefour;
        }
        public DataTable get_movimiento_abierto(string id_movimiento)
        {
            consultar_movimientos_seleccionado(id_movimiento);
            consultar_acuerdo_de_precios_carrefour();
            string tipo_acuerdo = movimientos_seleccionado.Rows[0]["tipo_acuerdo"].ToString();
            string acuerdo = movimientos_seleccionado.Rows[0]["acuerdo"].ToString();
            int fila_acuerdo = buscar_fila_acuerdo(tipo_acuerdo, acuerdo);
            abrir_resumen(fila_acuerdo);
            return resumen_movimientos;
        }
        #endregion

        #region metodos calculos
        public double get_comprado_en_el_mes(string sucursal, string mes, string año)
        {
            consultar_movimientos_carrefour(sucursal);
            double total = 0;
            for (int fila = 0; fila <= movimientos_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(movimientos_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    total = total + double.Parse(movimientos_carrefour.Rows[fila]["valor_venta"].ToString());
                }
            }
            return total;
        }
        public double get_pagado_en_el_mes(string sucursal, string mes, string año)
        {
            consultar_imputaciones_carrefour(sucursal);
            double total = 0, sub_total, efectivo, transferencia, mercado_pago;
            for (int fila = 0; fila <= imputaciones_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(imputaciones_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    efectivo = double.Parse(imputaciones_carrefour.Rows[fila]["efectivo"].ToString());
                    transferencia = double.Parse(imputaciones_carrefour.Rows[fila]["transferencia"].ToString());
                    mercado_pago = double.Parse(imputaciones_carrefour.Rows[fila]["mercado_pago"].ToString());

                    sub_total = efectivo + transferencia + mercado_pago;
                    total = total + sub_total;
                }
            }
            return total;
        }
        public double get_saldo_anterior(string sucursal, string mes, string año)
        {
            consultar_movimientos_carrefour(sucursal);
            double total = 0;
            double venta = 0;
            for (int fila = 0; fila <= movimientos_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha_anterior(movimientos_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    venta = venta + double.Parse(movimientos_carrefour.Rows[fila]["valor_venta"].ToString());
                }
            }

            consultar_imputaciones_carrefour(sucursal);
            double pagado = 0;
            double sub_total, efectivo, transferencia, mercado_pago;

            for (int fila = 0; fila <= imputaciones_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha_anterior(imputaciones_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    efectivo = double.Parse(imputaciones_carrefour.Rows[fila]["efectivo"].ToString());
                    transferencia = double.Parse(imputaciones_carrefour.Rows[fila]["transferencia"].ToString());
                    mercado_pago = double.Parse(imputaciones_carrefour.Rows[fila]["mercado_pago"].ToString());

                    sub_total = efectivo + transferencia + mercado_pago;
                    pagado = pagado + sub_total;
                }
            }
            return total = venta - pagado;
        }
        public double get_saldo_actual(string sucursal, string mes, string año)
        {
            double total = 0;

            double saldo_anterior = get_saldo_anterior(sucursal, mes, año);

            double venta = 0;
            for (int fila = 0; fila <= movimientos_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(movimientos_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    venta = venta + double.Parse(movimientos_carrefour.Rows[fila]["valor_venta"].ToString());
                }
            }

            double pagado = 0;
            double sub_total, efectivo, transferencia, mercado_pago;

            for (int fila = 0; fila <= imputaciones_carrefour.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(imputaciones_carrefour.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    efectivo = double.Parse(imputaciones_carrefour.Rows[fila]["efectivo"].ToString());
                    transferencia = double.Parse(imputaciones_carrefour.Rows[fila]["transferencia"].ToString());
                    mercado_pago = double.Parse(imputaciones_carrefour.Rows[fila]["mercado_pago"].ToString());

                    sub_total = efectivo + transferencia + mercado_pago;
                    pagado = pagado + sub_total;
                }
            }
            return total = saldo_anterior + venta - pagado;
        }

        public double get_saldo_general()
        {
            consultar_todos_movimientos_carrefour();
            double total = 0;
            double venta = 0;
            for (int fila = 0; fila <= todos_movimientos_carrefour.Rows.Count - 1; fila++)
            {

                venta = venta + double.Parse(todos_movimientos_carrefour.Rows[fila]["valor_venta"].ToString());

            }

            consultar_todos_imputaciones_carrefour();
            double pagado = 0;
            double sub_total, efectivo, transferencia, mercado_pago;

            for (int fila = 0; fila <= todos_imputaciones_carrefour.Rows.Count - 1; fila++)
            {
                efectivo = double.Parse(todos_imputaciones_carrefour.Rows[fila]["efectivo"].ToString());
                transferencia = double.Parse(todos_imputaciones_carrefour.Rows[fila]["transferencia"].ToString());
                mercado_pago = double.Parse(todos_imputaciones_carrefour.Rows[fila]["mercado_pago"].ToString());

                sub_total = efectivo + transferencia + mercado_pago;
                pagado = pagado + sub_total;
            }
            return total = venta - pagado;
        }
        #endregion

        #region metodos publicos
        public void actualizar_movimientos_carrefour_no_calculados()
        {
            calcular_movimientos_carrefour_no_calculados();
        }
        #endregion
    }
}
