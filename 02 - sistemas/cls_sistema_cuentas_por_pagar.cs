
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01___modulos;
using modulos;
namespace _02___sistemas
{
    public class cls_sistema_cuentas_por_pagar
    {
        public cls_sistema_cuentas_por_pagar(DataTable usuarioBD, DataTable sucursal_BD)
        {
            usuario = usuarioBD;
            sucursalBD = sucursal_BD;
            administracion = new cls_cuentas_por_pagar(usuarioBD, sucursal_BD);
            sucursal = sucursalBD.Rows[0]["sucursal"].ToString();
            pedidos = administracion.get_pedidos();
            lista_proveedores = administracion.get_lista_proveedores();
            acuerdo_de_precios_parametreados = administracion.get_acuerdo_de_precios();

        }

        #region atributos
        cls_cuentas_por_pagar administracion;
        cls_PDF PDF = new cls_PDF();
        string sucursal;
        DataTable usuario;
        DataTable sucursalBD;

        DataTable remitos;
        DataTable lista_proveedores;
        DataTable pedidos_no_calculados;
        DataTable acuerdo_de_precios_parametreados;
        DataTable resumen_pedido;
        DataTable resumen_bonificado;
        DataTable productos_proveedor;
        DataTable imputaciones;
        DataTable pedidos;

        DataTable todos_los_remitos;
        DataTable todas_las_imputaciones;
        #endregion

        #region metodos


        public double calcular_deuda_mes(string proveedor_seleccionado, string mes, string año)
        {
            double retorno, compra, pagado, pagado_subTotal, deuda_mes_anterior;
            retorno = 0;
            deuda_mes_anterior = calcular_deuda_mes_anterior(proveedor_seleccionado, mes, año);
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            compra = 0;
            for (int fila = 0; fila <= remitos.Rows.Count - 1; fila++)
            {
                if (remitos.Rows[fila]["sucursal"].ToString() == sucursal &&
                    verificar_proveedor_calculo(remitos.Rows[fila]["proveedor"].ToString(), proveedor_seleccionado) &&
                    verificar_fecha(remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos.Rows[fila]["valor_remito"].ToString());
                }
            }

            pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones.Rows[fila]["sucursal"].ToString() == sucursal &&
                    verificar_proveedor_calculo(imputaciones.Rows[fila]["proveedor"].ToString(), proveedor_seleccionado) &&
                    verificar_fecha(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno + deuda_mes_anterior;
        }
        public double calcular_compra_mes(string proveedor_seleccionado, string mes, string año)
        {
            double compra;
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            compra = 0;
            for (int fila = 0; fila <= remitos.Rows.Count - 1; fila++)
            {
                if (remitos.Rows[fila]["sucursal"].ToString() == sucursal &&
                    verificar_proveedor_calculo(remitos.Rows[fila]["proveedor"].ToString(), proveedor_seleccionado) &&
                    verificar_fecha(remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos.Rows[fila]["valor_remito"].ToString());
                }
            }

            return compra;
        }
        private bool verificar_proveedor_calculo(string proveedor_dato, string proveedor_seleccionado)
        {
            bool retorno = false;
            if (proveedor_seleccionado == "proveedor_villaMaipu")
            {
                if (proveedor_dato == proveedor_seleccionado ||
                    proveedor_dato == "insumos_fabrica")
                {
                    retorno = true;
                }
            }
            else if (proveedor_dato == proveedor_seleccionado)
            {
                retorno = true;
            }
            return retorno;

        }
        public double calcular_pago_mes(string proveedor_seleccionado, string mes, string año)
        {
            double pagado, pagado_subTotal;
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones.Rows[fila]["sucursal"].ToString() == sucursal && imputaciones.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            return pagado;
        }
        public double calcular_deuda_mes_anterior(string proveedor_seleccionado, string mes, string año)
        {
            double retorno, compra, pagado, pagado_subTotal;
            retorno = 0;
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            consultar_remitos();
            consultar_imputaciones();
            compra = 0;
            for (int fila = 0; fila <= remitos.Rows.Count - 1; fila++)
            {
                if (remitos.Rows[fila]["sucursal"].ToString() == sucursal &&
                    verificar_proveedor_calculo(remitos.Rows[fila]["proveedor"].ToString(), proveedor_seleccionado) &&
                    verificar_fecha_anterior(remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos.Rows[fila]["valor_remito"].ToString());
                }
            }

            pagado = 0;
            for (int fila = 0; fila <= imputaciones.Rows.Count - 1; fila++)
            {
                if (imputaciones.Rows[fila]["autorizado"].ToString() == "Si" &&
                    imputaciones.Rows[fila]["sucursal"].ToString() == sucursal &&
                    verificar_proveedor_calculo(imputaciones.Rows[fila]["proveedor"].ToString(), proveedor_seleccionado) &&
                    verificar_fecha_anterior(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno;
        }

        public double deuda_total_del_mes(string mes, string año)
        {
            double retorno = 0;
            double deuda_total_mes_anterior = deuda_total_del_mes_anterior(mes, año);
            consultar_todos_los_remitos();
            consultar_todas_las_imputaciones();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = deuda_total_mes_anterior + (compra - pagado);
            return retorno;
        }
        public double deuda_total_del_mes_anterior(string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos();
            consultar_todas_las_imputaciones();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha_anterior(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["activa"].ToString() == "2" && todas_las_imputaciones.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha_anterior(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = compra - pagado;
            return retorno;
        }
        public double compra_total_del_mes(string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            retorno = compra;
            return retorno;
        }
        public double pago_total_del_mes(string mes, string año)
        {
            double retorno = 0;
            consultar_todas_las_imputaciones();

            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones.Rows[fila]["sucursal"].ToString() == sucursal && verificar_fecha(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = pagado;
            return retorno;
        }


        public bool verificar_fecha(string fecha_dato, string mes, string año)
        {
            return verificar_fecha_actual(fecha_dato, mes, año);
        }
        public bool verificar_proveedor(string proveedor_dato, string proveedor_seleccionado, DataTable lista_proov)
        {
            lista_proveedores = lista_proov;
            bool retorno = false;
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            if (proveedor_seleccionado == "proveedor_villaMaipu")
            {
                if (proveedor_dato == proveedor_seleccionado ||
                    proveedor_dato == "insumos_fabrica")
                {
                    retorno = true;
                }
            }
            else if (proveedor_dato == proveedor_seleccionado)
            {
                retorno = true;
            }
            return retorno;
        }
        public string sumar_imputacion(string efectivo_dato, string transferencia_dato, string mercado_pago_dato)
        {

            return formatCurrency(suma_de_imputacion(efectivo_dato, transferencia_dato, mercado_pago_dato));

        }
        public bool enviar_imputacion(string efectivo_dato, string transferencia_dato, string mercado_pago_dato, string proveedor_seleccionado)
        {
            double efectivo, transferencia, mercado_pago;


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
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);

            return administracion.cargar_imputacion(efectivo, transferencia, mercado_pago, proveedor_seleccionado);
        }

        public void crear_pdf(string ruta, string id_remito, string proveedor_seleccionado, byte[] logo, string nivel_seguridad) //
        {

            string acuerdo, num_acuerdo, nombre_proveedor,nota;
            int fila_pedido, fila_acuerdo, fila_remito;
            int seguridad = int.Parse(nivel_seguridad);
            consultar_acuerdo_de_precios();
            fila_pedido = obtener_fila_de_pedido_por_id(id_remito);
            acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString(); 

            nombre_proveedor = pedidos.Rows[fila_pedido]["proveedor"].ToString();
            nota = pedidos.Rows[fila_pedido]["nota"].ToString(); 
            fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, nombre_proveedor);

            consultar_productos_proveedor(nombre_proveedor);
            abrir_pedido(fila_pedido, fila_acuerdo, nombre_proveedor);

            fila_remito = obtener_fila_de_remito(id_remito);
            if (seguridad <= 2)
            {
                PDF.GenerarPDF(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_remito, remitos, sucursalBD, nota); // resumen_pedido,resumen_bonificado
            }
            else
            {
           //     PDF.GenerarPDF_remito_de_carga(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_remito, remitos, sucursalBD); // resumen_pedido,resumen_bonificado
            }

        }
        public void crear_pdf_resumen_pedido_operativo(string ruta, string num_pedido, string sucursal_dato, string proveedor_seleccionado, byte[] logo)
        {
            string nombre_proveedor;
            int fila_pedido, fila_acuerdo;
            string acuerdo, num_acuerdo;
            sucursal = sucursal_dato;
            fila_pedido = obtener_fila_de_pedido(num_pedido);

            nombre_proveedor = pedidos.Rows[fila_pedido]["proveedor"].ToString();
            acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString();

            consultar_productos_proveedor(nombre_proveedor);
            fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, nombre_proveedor);
            abrir_pedido(fila_pedido, fila_acuerdo, nombre_proveedor);

            PDF.GenerarPDF_operativo(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_pedido, pedidos, sucursalBD);

        }
        public string cancelar_pedido(string id_pedido, string num_pedido, string proveedor, DataTable usuariosBD, DataTable sucursalBD)
        {
            return administracion.cancelar_pedido(id_pedido, num_pedido, proveedor, usuariosBD, sucursalBD);
        }
        public void cargar_remito(string sucursal, string num_pedido, string valor_remito, string proveedor, string id_pedido, DataTable pedido)
        {
            administracion.cargar_remito(sucursal, num_pedido, valor_remito, proveedor);
            administracion.actualizar_estado_pedido_a_entregado(id_pedido);
            administracion.cargar_cantidad_recibida_pedido(id_pedido, pedido);
            administracion.actualizar_cuenta_por_pagar(num_pedido, proveedor, sucursal, valor_remito);

        }
        #endregion

        #region actualizar remitos
        public void actualizar_remitos()
        {
            consultar_pedidos_no_calculados();
            string proveedor, acuerdo_de_precios, tipo_de_acuerdo;
            int id_producto;
            double cantidad_fabrica, precio, sub_total;

            string sucursal_pedido, num_pedido, id_pedido;
            double porcentaje_descuento, descuento;

            double total_remito = 0;
            for (int fila = 0; fila <= pedidos_no_calculados.Rows.Count - 1; fila++)
            {
                proveedor = pedidos_no_calculados.Rows[fila]["proveedor"].ToString();
                acuerdo_de_precios = pedidos_no_calculados.Rows[fila]["acuerdo_de_precios"].ToString();
                tipo_de_acuerdo = pedidos_no_calculados.Rows[fila]["tipo_de_acuerdo"].ToString();
                consultar_acuerdo_de_precios_segun_parametros(proveedor, acuerdo_de_precios, tipo_de_acuerdo);
                total_remito = 0;
                double multiplicador;
                for (int columna = pedidos_no_calculados.Columns["producto_1"].Ordinal; columna < pedidos_no_calculados.Columns.Count - 1; columna++)
                {
                    if (IsNotDBNull(pedidos_no_calculados.Rows[fila][columna]))
                    {
                        if (pedidos_no_calculados.Rows[fila][columna].ToString() != "")
                        {

                            id_producto = int.Parse(obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString(), 2));
                            cantidad_fabrica = double.Parse(obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString().Replace(",", "."), 5).ToString());//


                            precio = double.Parse(acuerdo_de_precios_parametreados.Rows[0]["producto_" + id_producto.ToString()].ToString());
                            if (proveedor == "insumos_fabrica")
                            {
                                if ("N/A" != obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString().Replace(",", "."), 7))
                                {
                                    multiplicador = double.Parse(obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString().Replace(",", "."), 7));
                                }
                                else
                                {
                                    multiplicador = 1;
                                }
                                precio = precio * multiplicador;
                            }

                            sub_total = cantidad_fabrica * precio;
                            total_remito = Math.Round(total_remito + sub_total, 2);



                        }
                    }
                }
                //cargar remito

                sucursal_pedido = pedidos_no_calculados.Rows[fila]["sucursal"].ToString();
                num_pedido = pedidos_no_calculados.Rows[fila]["num_pedido"].ToString();
                tipo_de_acuerdo = pedidos_no_calculados.Rows[fila]["tipo_de_acuerdo"].ToString();

                if (IsNotDBNull(acuerdo_de_precios_parametreados.Rows[0]["descuentos"]))
                {
                    porcentaje_descuento = double.Parse(acuerdo_de_precios_parametreados.Rows[0]["descuentos"].ToString());
                    descuento = Math.Round(total_remito * porcentaje_descuento, 2);
                    total_remito = Math.Round(total_remito - descuento, 2);
                }
                else
                {
                    descuento = 0;
                }
                string fecha_remito = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cargar_remito_en_BD(proveedor, sucursal_pedido, num_pedido, tipo_de_acuerdo, total_remito.ToString().Replace(",", "."), fecha_remito, descuento.ToString().Replace(",", "."));
                id_pedido = pedidos_no_calculados.Rows[fila]["id"].ToString();
                actualizar_lectura_de_pedido(id_pedido);
            }
        }
        private void cargar_remito_en_BD(string proveedor, string sucursal, string num_pedido, string tipo_de_acuerdoBD, string valor_remito, string fecha_remito, string descuento)
        {
            administracion.cargar_remito(proveedor, sucursal, num_pedido, tipo_de_acuerdoBD, valor_remito, fecha_remito, descuento);
        }
        private void actualizar_lectura_de_pedido(string id_pedido)
        {
            administracion.actualizar_lectura_de_pedido(id_pedido);
        }

        #endregion

        #region metogos get/set
        public DataTable get_pedido_seleccionado(string num_pedido)
        {
            consultar_pedidos();
            int fila_pedido = obtener_fila_de_pedido(num_pedido);
            string acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            string num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
            string proveedor = pedidos.Rows[fila_pedido]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor);
            int fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, proveedor);
            abrir_pedido(fila_pedido, fila_acuerdo, proveedor);
            return resumen_pedido;
        }
        public DataTable get_remitos()
        {
            consultar_remitos();
            return remitos;
        }
        public DataTable get_imputaciones()
        {
            consultar_imputaciones();
            return imputaciones;
        }
        public DataTable get_pedidos()
        {
            consultar_pedidos();
            return pedidos;
        }
        public string get_nombre_proveedor(string proveedor)
        {
            consultar_lista_proveedores();
            return obtener_nombre_proveedor(proveedor);
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------------
        #region metodos privados

        private int obtener_fila_remito(string id)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= remitos.Rows.Count - 1)
            {
                if (id == remitos.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int obtener_fila_de_acuerdo(string acuerdo, string num_acuerdo, string proveedor)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios_parametreados.Rows.Count - 1)
            {
                if (acuerdo == acuerdo_de_precios_parametreados.Rows[fila]["tipo_de_acuerdo"].ToString() && num_acuerdo == acuerdo_de_precios_parametreados.Rows[fila]["acuerdo"].ToString() && proveedor == acuerdo_de_precios_parametreados.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int obtener_fila_de_pedido(string num_pedido)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos.Rows.Count - 1)
            {
                if (num_pedido == pedidos.Rows[fila]["num_pedido"].ToString() && sucursal == pedidos.Rows[fila]["sucursal"].ToString())
                {
                    retorno = fila; break;
                }
                fila++;
            }

            return retorno;
        }
        private int obtener_fila_de_pedido_por_id(string id_pedido)
        {
            int retorno = 0;
            int fila = 0;
            int fila_remito = 0;
            while (fila <= remitos.Rows.Count - 1)
            {
                if (id_pedido == remitos.Rows[fila]["id"].ToString())
                {
                    fila_remito = fila;
                    break;
                }
                fila++;
            }
            string num_pedido = remitos.Rows[fila_remito]["num_pedido"].ToString();
            retorno = obtener_fila_de_pedido(num_pedido);
            return retorno;
        }
        private int obtener_fila_de_remito(string id_pedido)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= remitos.Rows.Count - 1)
            {
                if (id_pedido == remitos.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        private string obtener_nombre_proveedor(string provedor_seleccionado)
        {

            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= lista_proveedores.Rows.Count - 1)
            {
                if (provedor_seleccionado == lista_proveedores.Rows[fila]["nombre_proveedor"].ToString())
                {
                    retorno = lista_proveedores.Rows[fila]["nombre_en_BD"].ToString();
                    fila = lista_proveedores.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }

        private void abrir_pedido(int fila_pedido, int fila_acuerdo, string nombre_proveedor)
        {
            string precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, pedido_dato, tipo_paquete, unidad_insumo, tipo_unidad, dato;
            crear_tabla_resumen();
            crear_tabla_bonificado();

            int i = 1;
            for (int columna = pedidos.Columns["producto_1"].Ordinal; columna <= pedidos.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(pedidos.Rows[fila_pedido]["producto_" + i.ToString()]))
                {
                    if (pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                    {
                        pedido_dato = pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString();
                        //extraer precio
                        precio = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                        //extraer id
                        id = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);
                        //extraer producto
                        producto = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad pedida
                        cantidad_pedida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                        //extraer cantidad entregada
                        cantidad_entregada = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);
                        //extraer cantidad entregada
                        cantidad_recibida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);
                        if (nombre_proveedor == "insumos_fabrica")
                        {
                            tipo_paquete = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 6);//;
                            unidad_insumo = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 7);//;
                            tipo_unidad = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 8);//;
                            if (tipo_paquete == "Unidad" &&
                                unidad_insumo == "1" &&
                                tipo_unidad == "unid.")
                            {
                                dato = "Unidad";
                            }
                            else
                            {
                                dato = tipo_paquete + " " + unidad_insumo + " " + tipo_unidad;
                            }
                        }
                        else
                        {
                            dato = "N/A";
                            unidad_insumo = "N/A";
                        }

                        //cargar normal
                        cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, fila_acuerdo, pedido_dato, dato, unidad_insumo);

                    }
                }
                i++;
            }
        }
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
        private void cargar_producto(string precio, string id, string producto, string cantidad_pedida, string cantidad_entregada, string cantidad_recibida, int fila_acuerdo, string pedido_dato, string unidad_insumo, string multiplicador)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;

            resumen_pedido.Rows[fila]["pedido_dato"] = pedido_dato;
            int fila_producto = buscar_fila_producto(id, productos_proveedor);
            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["producto"] = producto;
            resumen_pedido.Rows[fila]["pedido"] = cantidad_pedida;
            resumen_pedido.Rows[fila]["unid.pedida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
            resumen_pedido.Rows[fila]["entregado"] = cantidad_entregada;
            if (unidad_insumo == "N/A")
            {
                resumen_pedido.Rows[fila]["unid.entregada"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"].ToString();
            }
            else
            {
                resumen_pedido.Rows[fila]["unid.entregada"] = unidad_insumo;
            }
            resumen_pedido.Rows[fila]["tipo"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
            resumen_pedido.Rows[fila]["recibido"] = cantidad_recibida;
            precio = acuerdo_de_precios_parametreados.Rows[fila_acuerdo]["producto_" + id].ToString();
            //  precio = precio.Replace(",",".");

            if (cantidad_recibida == string.Empty || cantidad_recibida == "N/A")
            {
                cantidad_recibida = "0";
            }

            if (multiplicador != "N/A")
            {
                double nuevo_precio = double.Parse(multiplicador) * double.Parse(precio);
                precio = nuevo_precio.ToString();
            }

            decimal sub_total = decimal.Parse(cantidad_recibida) * decimal.Parse(precio);
            resumen_pedido.Rows[fila]["precio"] = decimal.Parse(precio);
            resumen_pedido.Rows[fila]["sub.total"] = sub_total;


        }
        private void cargar_bonificado(string precio, string id, string producto, string cantidad_pedida, string cantidad_entregada, int fila_acuerdo)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;

            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["producto"] = producto;
            resumen_pedido.Rows[fila]["pedido"] = cantidad_pedida;
            resumen_pedido.Rows[fila]["unid.pedida"] = productos_proveedor.Rows[int.Parse(id)]["unidad_de_medida_local"].ToString();
            resumen_pedido.Rows[fila]["entregado"] = cantidad_entregada;
            resumen_pedido.Rows[fila]["unid.entregada"] = productos_proveedor.Rows[int.Parse(id)]["unidad_de_medida_fabrica"].ToString();

            if (precio == "BONIFICADO")
            {
                resumen_pedido.Rows[fila]["precio"] = formatCurrency(0);
                resumen_pedido.Rows[fila]["sub.total"] = formatCurrency(0);
                resumen_pedido.Rows[fila]["tipo"] = "BONIFICADO";
            }
            else
            {
                double precio_especial = double.Parse(acuerdo_de_precios_parametreados.Rows[fila_acuerdo]["precio_bonificado"].ToString());
                resumen_pedido.Rows[fila]["precio"] = formatCurrency(acuerdo_de_precios_parametreados.Rows[fila_acuerdo]["precio_bonificado"]);
                resumen_pedido.Rows[fila]["sub.total"] = formatCurrency(double.Parse(cantidad_entregada) * precio_especial);
                resumen_pedido.Rows[fila]["tipo"] = "BONIFICADO ESPECIAL";
            }
        }
        private string obtener_dato_pedido(string dato, int posicion_dato)
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

        private void crear_tabla_resumen()
        {
            resumen_pedido = new DataTable();
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("producto", typeof(string));
            resumen_pedido.Columns.Add("pedido", typeof(string));
            resumen_pedido.Columns.Add("unid.pedida", typeof(string));
            resumen_pedido.Columns.Add("entregado", typeof(string));
            resumen_pedido.Columns.Add("unid.entregada", typeof(string));
            resumen_pedido.Columns.Add("recibido", typeof(string));
            resumen_pedido.Columns.Add("tipo", typeof(string));
            resumen_pedido.Columns.Add("precio", typeof(string));
            resumen_pedido.Columns.Add("sub.total", typeof(string));

            resumen_pedido.Columns.Add("pedido_dato", typeof(string));


        }
        private void crear_tabla_bonificado()
        {
            resumen_bonificado = new DataTable();
            resumen_bonificado.Columns.Add("id", typeof(string));
            resumen_bonificado.Columns.Add("producto", typeof(string));
            resumen_bonificado.Columns.Add("pedido", typeof(string));
            resumen_bonificado.Columns.Add("unid.pedida", typeof(string));
            resumen_bonificado.Columns.Add("entregado", typeof(string));
            resumen_bonificado.Columns.Add("unid.entregada", typeof(string));
            resumen_bonificado.Columns.Add("tipo", typeof(string));
            resumen_bonificado.Columns.Add("precio", typeof(string));
            resumen_bonificado.Columns.Add("sub.total", typeof(string));

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
        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
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
        private string formatDate(string valor)
        {
            DateTime fecha = DateTime.Parse(valor);
            return fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString();
        }
        private string formatDateBD(string valor)
        {
            DateTime fecha = DateTime.Parse(valor);
            return fecha.Year.ToString() + "-" + fecha.Month.ToString() + "-" + fecha.Day.ToString();
        }
        #endregion


        #region metodos consulta
        private void consultar_pedidos()
        {
            pedidos = administracion.get_pedidos();
        }
        private void consultar_todas_las_imputaciones()
        {
            todas_las_imputaciones = administracion.get_imputaciones();
        }
        private void consultar_todos_los_remitos()
        {
            todos_los_remitos = administracion.get_remitos();
        }
        private void consultar_imputaciones()
        {
            imputaciones = administracion.get_imputaciones();
        }
        private void consultar_remitos()
        {
            remitos = administracion.get_remitos();
        }
        private void consultar_productos_proveedor(string proveedor_seleccionado)
        {
            productos_proveedor = administracion.get_productos_proveedor(proveedor_seleccionado);
        }

        private void consultar_pedidos_no_calculados()
        {
            pedidos_no_calculados = administracion.get_pedidos_no_calculados();
        }

        private void consultar_acuerdo_de_precios_segun_parametros(string proveedor, string acuerdo_de_precio, string tipo_de_acuerdo)
        {
            acuerdo_de_precios_parametreados = administracion.get_acuerdo_de_precios_segun_parametros(proveedor, acuerdo_de_precio, tipo_de_acuerdo);
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios_parametreados = administracion.get_acuerdo_de_precios();
        }
        private void consultar_lista_proveedores()
        {
            lista_proveedores = administracion.get_lista_proveedores();
        }

        #endregion
    }
}
