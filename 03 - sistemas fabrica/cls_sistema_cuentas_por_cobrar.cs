using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_sistema_cuentas_por_cobrar
    {
        public cls_sistema_cuentas_por_cobrar(DataTable usuarioBD)
        {
            usuario = usuarioBD;

            administracion = new cls_cuentas_por_cobrar(usuarioBD);

            pedidos = administracion.get_pedidos();
            lista_proveedores = administracion.get_lista_proveedores();
            acuerdo_de_precios_parametreados = administracion.get_acuerdo_de_precios();

        }
        #region atributos
        cls_cuentas_por_cobrar administracion;
        cls_PDF PDF = new cls_PDF();
        string sucursal;
        DataTable usuario;
        DataTable sucursalBD;

        DataTable remitos;
        DataTable remitos_proveedores_a_fabrica;
        DataTable sucursales;
        DataTable lista_proveedores;
        DataTable lista_proveedores_fabrica;
        DataTable pedidos_no_calculados;
        DataTable acuerdo_de_precios_parametreados;
        DataTable resumen_pedido;
        DataTable resumen_bonificado;
        DataTable productos_proveedor;
        DataTable imputaciones;
        DataTable imputaciones_fabrica_a_proveedor;
        DataTable pedidos;
        DataTable pedidos_fabrica_a_proveedor;
        DataTable acuerdo_de_precios;

        DataTable todos_los_remitos;
        DataTable todas_las_imputaciones;

        DataTable todos_los_remitos_fabrica;
        DataTable todas_las_imputaciones_fabrica;
        DataTable deuda_mes_anterior;
        DataTable deuda_mes;
        DataTable deuda_actual;
        #endregion
        #region cargar iva
        public void cargar_iva(string id_remito, string proveedor, string sucursal, string num_pedido, string valor_remito)
        {
            administracion.cargar_iva(id_remito,proveedor,sucursal,num_pedido,valor_remito);
        }
        #endregion
        #region cargar nota
        public void marcar_cobrado(string id_remito, string estado)
        {
            administracion.marcar_cobrado(id_remito, estado);
        }
        public void eliminar_imputacion(string id_imputacion)
        {
            administracion.eliminar_imputacion(id_imputacion);
        }
        public void cargar_nota(string id_orden, string nota)
        {
            administracion.cargar_nota(id_orden, nota);

        }
        public void cargar_nota_credito(string sucursal, string nota, string valor_remito)
        {
            administracion.cargar_nota_credito(sucursal, nota, valor_remito);
        }
        #endregion


        #region metodos

        public void autorizar_imputacion(string id_imputacion)
        {
            administracion.autorizar_imputacion(id_imputacion);

        }
        public double calcular_deuda_mes(DataTable proveedorBD, string sucursal, string mes, string año)
        {
            string proveedor_seleccionado = proveedorBD.Rows[0]["nombre_en_BD"].ToString();
            double retorno, compra, pagado, pagado_subTotal, deuda_mes_anterior;
            retorno = 0;
            deuda_mes_anterior = calcular_deuda_mes_anterior(proveedorBD, sucursal, mes, año);

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
                if (imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones.Rows[fila]["sucursal"].ToString() == sucursal && imputaciones.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno + deuda_mes_anterior;
        }
        public double calcular_deuda_mes_anterior(DataTable proveedorBD, string sucursal, string mes, string año)
        {
            string proveedor_seleccionado = proveedorBD.Rows[0]["nombre_en_BD"].ToString();
            double retorno, compra, pagado, pagado_subTotal;
            retorno = 0;

            consultar_remitos(sucursal, mes, año);
            //consultar_imputaciones();
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



        public double deuda_total_del_mes(string nombre_proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            double deuda_total_mes_anterior = deuda_total_del_mes_anterior(nombre_proveedor_seleccionado, mes, año);
            consultar_todos_los_remitos(sucursal, mes, año);
            //consultar_todas_las_imputaciones();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = deuda_total_mes_anterior + (compra - pagado);
            return retorno;
        }
        public double deuda_total_del_mes_anterior(string nombre_proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos(sucursal, mes, año);
            //consultar_todas_las_imputaciones();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha_anterior(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha_anterior(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = compra - pagado;
            return retorno;
        }
        public double compra_total_del_mes(string nombre_proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos(sucursal, mes, año);

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha(todos_los_remitos.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos.Rows[fila]["valor_remito"].ToString());
                }
            }
            retorno = compra;
            return retorno;
        }
        public double pago_total_del_mes(string nombre_proveedor_seleccionado, string mes, string año)
        {
            double retorno = 0;
            //consultar_todas_las_imputaciones();

            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones.Rows[fila]["proveedor"].ToString() == nombre_proveedor_seleccionado && verificar_fecha(todas_las_imputaciones.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = pagado;
            return retorno;
        }

        public double deuda_total_del_mes_fabrica(string fabrica_seleccionada, string mes, string año)
        {
            double retorno = 0;
            double deuda_total_mes_anterior = deuda_total_del_mes_anterior(fabrica_seleccionada, mes, año);
            consultar_todos_los_remitos_fabrica();
            consultar_todas_las_imputaciones_fabrica();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos_fabrica.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha(todos_los_remitos_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones_fabrica.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones_fabrica.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha(todas_las_imputaciones_fabrica.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = deuda_total_mes_anterior + (compra - pagado);
            return retorno;
        }
        public double deuda_total_del_mes_anterior_fabrica(string fabrica_seleccionada, string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos_fabrica();
            consultar_todas_las_imputaciones_fabrica();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos_fabrica.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha_anterior(todos_los_remitos_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }
            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones_fabrica.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones_fabrica.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha_anterior(todas_las_imputaciones_fabrica.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = compra - pagado;
            return retorno;
        }
        public double compra_total_del_mes_fabrica(string fabrica_seleccionada, string mes, string año)
        {
            double retorno = 0;
            consultar_todos_los_remitos_fabrica();

            double compra = 0;
            for (int fila = 0; fila <= todos_los_remitos_fabrica.Rows.Count - 1; fila++)
            {
                if (todos_los_remitos_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha(todos_los_remitos_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(todos_los_remitos_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }
            retorno = compra;
            return retorno;
        }
        public double pago_total_del_mes_fabrica(string fabrica_seleccionada, string mes, string año)
        {
            double retorno = 0;
            consultar_todas_las_imputaciones_fabrica();

            double pagado_subTotal;
            double pagado = 0;
            for (int fila = 0; fila <= todas_las_imputaciones_fabrica.Rows.Count - 1; fila++)
            {
                if (todas_las_imputaciones_fabrica.Rows[fila]["autorizado"].ToString() == "Si" && todas_las_imputaciones_fabrica.Rows[fila]["fabrica"].ToString() == fabrica_seleccionada && verificar_fecha(todas_las_imputaciones_fabrica.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital"].ToString()) + double.Parse(todas_las_imputaciones_fabrica.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }
            retorno = pagado;
            return retorno;
        }

        public double calcular_deuda_mes_fabrica(string proveedor_seleccionado, string fabrica, string mes, string año)
        {
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);
            double retorno, compra, pagado, pagado_subTotal, deuda_mes_anterior;
            retorno = 0;
            deuda_mes_anterior = calcular_deuda_mes_anterior_fabrica(proveedor_seleccionado, fabrica, mes, año);

            compra = 0;
            for (int fila = 0; fila <= remitos_proveedores_a_fabrica.Rows.Count - 1; fila++)
            {
                if (remitos_proveedores_a_fabrica.Rows[fila]["fabrica"].ToString() == fabrica && remitos_proveedores_a_fabrica.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(remitos_proveedores_a_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos_proveedores_a_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }

            pagado = 0;
            for (int fila = 0; fila <= imputaciones_fabrica_a_proveedor.Rows.Count - 1; fila++)
            {
                if (imputaciones_fabrica_a_proveedor.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones_fabrica_a_proveedor.Rows[fila]["fabrica"].ToString() == fabrica && imputaciones_fabrica_a_proveedor.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(imputaciones_fabrica_a_proveedor.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno + deuda_mes_anterior;
        }
        public double calcular_deuda_mes_anterior_fabrica(string proveedor_seleccionado, string fabrica, string mes, string año)
        {
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);
            double retorno, compra, pagado, pagado_subTotal;
            retorno = 0;

            consultar_remitos(sucursal, mes, año);
            //consultar_imputaciones();
            consultar_remitos_proveedores_a_fabrica();
            compra = 0;
            for (int fila = 0; fila <= remitos_proveedores_a_fabrica.Rows.Count - 1; fila++)
            {
                if (remitos_proveedores_a_fabrica.Rows[fila]["fabrica"].ToString() == fabrica && remitos_proveedores_a_fabrica.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha_anterior(remitos_proveedores_a_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos_proveedores_a_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }
            consultar_imputaciones_fabrica_a_proveedor();
            pagado = 0;
            for (int fila = 0; fila <= imputaciones_fabrica_a_proveedor.Rows.Count - 1; fila++)
            {
                if (imputaciones_fabrica_a_proveedor.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones_fabrica_a_proveedor.Rows[fila]["fabrica"].ToString() == fabrica && imputaciones_fabrica_a_proveedor.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha_anterior(imputaciones_fabrica_a_proveedor.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            retorno = compra - pagado;
            return retorno;
        }
        public double calcular_compra_mes_fabrica(string proveedor_seleccionado, string fabrica, string mes, string año)
        {
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);
            double compra;

            compra = 0;
            for (int fila = 0; fila <= remitos_proveedores_a_fabrica.Rows.Count - 1; fila++)
            {
                if (remitos_proveedores_a_fabrica.Rows[fila]["fabrica"].ToString() == fabrica && remitos_proveedores_a_fabrica.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(remitos_proveedores_a_fabrica.Rows[fila]["fecha_remito"].ToString(), mes, año))
                {
                    compra = compra + double.Parse(remitos_proveedores_a_fabrica.Rows[fila]["valor_remito"].ToString());
                }
            }

            return compra;
        }
        public double calcular_pago_mes_fabrica(string proveedor_seleccionado, string fabrica, string mes, string año)
        {
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);
            double pagado, pagado_subTotal;
            pagado = 0;
            for (int fila = 0; fila <= imputaciones_fabrica_a_proveedor.Rows.Count - 1; fila++)
            {
                if (imputaciones_fabrica_a_proveedor.Rows[fila]["autorizado"].ToString() == "Si" && imputaciones_fabrica_a_proveedor.Rows[fila]["fabrica"].ToString() == fabrica && imputaciones_fabrica_a_proveedor.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado && verificar_fecha(imputaciones_fabrica_a_proveedor.Rows[fila]["fecha"].ToString(), mes, año))
                {
                    pagado_subTotal = double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_efectivo"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital"].ToString()) + double.Parse(imputaciones_fabrica_a_proveedor.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    pagado = pagado + pagado_subTotal;
                }
            }

            return pagado;
        }

        public bool verificar_fecha(string fecha_dato, string mes, string año)
        {
            return verificar_fecha_actual(fecha_dato, mes, año);
        }
        public bool verificar_proveedor(string proveedor_dato, DataTable proveedor_seleccionado)
        {
            bool retorno = false;
            if (proveedor_seleccionado.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu")
            {
                if (proveedor_dato == proveedor_seleccionado.Rows[0]["nombre_en_BD"].ToString() ||
                    proveedor_dato == "insumos_fabrica")
                {
                    retorno = true;
                }
            }
            else if (proveedor_dato == proveedor_seleccionado.Rows[0]["nombre_en_BD"].ToString())
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_proveedor_de_fabrica(string proveedor_dato, string proveedor_seleccionado)
        {
            bool retorno = false;
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);
            if (proveedor_dato == proveedor_seleccionado)
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


            if (!double.TryParse(efectivo_dato.Replace(".", ","), out efectivo))
            {
                efectivo_dato = string.Empty;
                efectivo = 0;
            }

            if (!double.TryParse(transferencia_dato.Replace(".", ","), out transferencia))
            {
                transferencia_dato = string.Empty;
                transferencia = 0;
            }

            if (!double.TryParse(mercado_pago_dato.Replace(".", ","), out mercado_pago))
            {
                mercado_pago_dato = string.Empty;
                mercado_pago = 0;
            }
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);

            return administracion.cargar_imputacion(efectivo, transferencia, mercado_pago, proveedor_seleccionado);
        }
        public bool enviar_imputacion_de_local_como_fabrica(string efectivo_dato, string transferencia_dato, string mercado_pago_dato, string proveedor_seleccionado, string sucursal_seleccionada)
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

            return administracion.cargar_imputacion_de_local_como_fabrica(efectivo, transferencia, mercado_pago, proveedor_seleccionado, sucursal_seleccionada);
        }
        public bool enviar_imputacion_fabrica(string fabrica_seleccionada, string efectivo_dato, string transferencia_dato, string mercado_pago_dato, string proveedor_seleccionado)
        {
            double efectivo, transferencia, mercado_pago;


            if (!double.TryParse(efectivo_dato.Replace(".", ","), out efectivo))
            {
                efectivo_dato = string.Empty;
                efectivo = 0;
            }

            if (!double.TryParse(transferencia_dato.Replace(".", ","), out transferencia))
            {
                transferencia_dato = string.Empty;
                transferencia = 0;
            }

            if (!double.TryParse(mercado_pago_dato.Replace(".", ","), out mercado_pago))
            {
                mercado_pago_dato = string.Empty;
                mercado_pago = 0;
            }
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica(proveedor_seleccionado);

            return administracion.cargar_imputacion_fabrica(efectivo, transferencia, mercado_pago, fabrica_seleccionada, proveedor_seleccionado);
        }
        private string buscar_id_sucursal(string nombre, DataTable sucursales_BD)
        {
            string retorno = "";
            int fila = 0;
            while (fila <= sucursales_BD.Rows.Count - 1)
            {
                if (nombre == sucursales_BD.Rows[fila]["sucursal"].ToString())
                {
                    retorno = sucursales_BD.Rows[fila]["id"].ToString();
                    break;
                }
                fila++;
            }

            return retorno;
        }
        //ruta_archivo, gridView_remitos.SelectedRow.Cells[0].Text, proveedor_seleccionado, imgdata, Session["nivel_seguridad"].ToString(), Session["sucursal_seleccionada"].ToString(), sistema_Administracion.get_sucursales()
        public void crear_pdf(string ruta, string id_remito, string proveedor_seleccionado, byte[] logo, string nivel_seguridad, string sucursal_seleccionada,string mes,string año) //
        {

            string acuerdo, num_acuerdo, nombre_proveedor, nota,impuesto;
            int fila_pedido, fila_acuerdo, fila_remito;
            int seguridad = int.Parse(nivel_seguridad);
            consultar_sucursales();
            consultar_remitos_todas_las_sucursales(mes,año);

            consultar_acuerdo_de_precios();
            fila_pedido = obtener_fila_de_pedido_mediante_idRemito(id_remito, sucursal_seleccionada);
            acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString();

            nombre_proveedor = pedidos.Rows[fila_pedido]["proveedor"].ToString();
            nota = pedidos.Rows[fila_pedido]["nota"].ToString();
            impuesto = pedidos.Rows[fila_pedido]["aumento"].ToString();
            fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, nombre_proveedor);
            string id = buscar_id_sucursal(sucursal_seleccionada, sucursales);
            consultar_sucursal(id);
            consultar_productos_proveedor(nombre_proveedor);
            abrir_pedido(fila_pedido, fila_acuerdo, nombre_proveedor);

            fila_remito = obtener_fila_de_remito(id_remito);
            if (seguridad < 2)
            {
                PDF.GenerarPDF(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_remito, remitos, sucursalBD, nota,impuesto); // resumen_pedido,resumen_bonificado
            }


        }
        public void crear_pdf_fabrica(string ruta, byte[] logo, string id_remito, string nombre_fabrica, DataTable proveedor_BD) //
        {

            /* string acuerdo, num_acuerdo, nombre_proveedor;
             int fila_pedido, fila_acuerdo, fila_remito;
             int seguridad = int.Parse(nivel_seguridad);

             fila_pedido = obtener_fila_de_pedido_por_id(id_remito, sucursal_seleccionada);
             acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
             num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString();

             nombre_proveedor = obtener_nombre_proveedor(proveedor_seleccionado);
             fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, nombre_proveedor);
             string id = buscar_id_sucursal(sucursal_seleccionada, sucursales_BD);
             consultar_sucursal(id);
             consultar_productos_proveedor(nombre_proveedor);
             abrir_pedido(fila_pedido, fila_acuerdo);

             fila_remito = obtener_fila_de_remito(id_remito);
             if (seguridad <= 2)
             {
                 PDF.GenerarPDF_orden_de_compra(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_remito, remitos, sucursalBD); // resumen_pedido,resumen_bonificado
             }
             else
             {
                 PDF.GenerarPDF_administrativo(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_remito, remitos, sucursalBD); // resumen_pedido,resumen_bonificado
             }*/

            consultar_pedidos_fabrica();
            int fila_remito = obtener_fila_remito_fabrica(id_remito);
            int fila_pedido = obtener_fila_de_pedido_por_id(id_remito, nombre_fabrica);
            int fila_acuerdo = buscar_fila_acuerdo_pedido_fabrica(fila_pedido);

            string proveedor_seleccionado = pedidos_fabrica_a_proveedor.Rows[fila_pedido]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor_seleccionado);
            proveedor_seleccionado = obtener_nombre_proveedor_fabrica_remito(proveedor_seleccionado);
            abrir_pedido_fabrica(fila_pedido, fila_acuerdo);

            //PDF.GenerarPDF_orden_de_compra(ruta, logo, resumen_pedido, proveedor_seleccionado, fila_pedido, pedidos_fabrica_a_proveedor, proveedor_BD, remitos_proveedores_a_fabrica, fila_remito);

        }
        public void crear_pdf_remito_de_carga(string ruta, DataTable resumen, byte[] logo,DateTime fecha) //
        {

            consultar_remitos_todas_las_sucursales(fecha.Month.ToString(), fecha.Year.ToString());
            abrir_pedido_remito_de_carga(resumen);

            PDF.GenerarPDF_remito_de_carga(ruta, logo, resumen, resumen_pedido, sucursales); // resumen_pedido,resumen_bonificado

        }
        #endregion
        #region actualizar remitos
        public void actualizar_remitos()
        {
            consultar_pedidos_no_calculados();
            string proveedor, acuerdo_de_precios, tipo_de_acuerdo;
            int id_producto;
            double cantidad_fabrica, precio, sub_total;

            string sucursal_pedido, num_pedido, fecha_remito, id_pedido;
            double porcentaje_descuento, descuento;

            double total_remito = 0;
            for (int fila = 0; fila <= pedidos_no_calculados.Rows.Count - 1; fila++)
            {
                proveedor = pedidos_no_calculados.Rows[fila]["proveedor"].ToString();
                acuerdo_de_precios = pedidos_no_calculados.Rows[fila]["acuerdo_de_precios"].ToString();
                tipo_de_acuerdo = pedidos_no_calculados.Rows[fila]["tipo_de_acuerdo"].ToString();
                consultar_acuerdo_de_precios_segun_parametros(proveedor, acuerdo_de_precios, tipo_de_acuerdo);
                total_remito = 0;
                for (int columna = pedidos_no_calculados.Columns["producto_1"].Ordinal; columna < pedidos_no_calculados.Columns.Count - 1; columna++)
                {
                    if (IsNotDBNull(pedidos_no_calculados.Rows[fila][columna]))
                    {
                        if (pedidos_no_calculados.Rows[fila][columna].ToString() != "")
                        {
                            id_producto = int.Parse(obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString(), 2));
                            cantidad_fabrica = double.Parse(obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString(), 5).ToString());//.Replace(",",".")

                            if ("BONIFICADO ESPECIAL" == obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString(), 1))
                            {
                                precio = double.Parse(acuerdo_de_precios_parametreados.Rows[0]["precio_bonificado"].ToString());
                                sub_total = cantidad_fabrica * precio;
                                total_remito = total_remito + sub_total;
                            }
                            else if ("BONIFICADO" == obtener_dato_pedido(pedidos_no_calculados.Rows[fila][columna].ToString(), 1))
                            {
                                precio = 0;
                                sub_total = cantidad_fabrica * precio;
                                total_remito = total_remito + sub_total;
                            }
                            else
                            {
                                precio = double.Parse(acuerdo_de_precios_parametreados.Rows[0]["producto_" + id_producto.ToString()].ToString());
                                sub_total = cantidad_fabrica * precio;
                                total_remito = Math.Round(total_remito + sub_total, 2);
                            }
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
                fecha_remito = formatDateBD(pedidos_no_calculados.Rows[fila]["fecha"].ToString());
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
        public double get_deuda_actual(string sucursal)
        {
            DateTime fecha = DateTime.Now;
            double deuda = 0;
            consultar_deuda_actual(sucursal);
            if (deuda_actual.Rows.Count > 0)
            {
                for (int fila = 0; fila <= deuda_actual.Rows.Count - 1; fila++)
                {
                    if (deuda_actual.Rows[fila]["sucursal"].ToString() == sucursal &&
                        deuda_actual.Rows[fila]["mes"].ToString() == fecha.Month.ToString() &&
                        deuda_actual.Rows[fila]["año"].ToString() == fecha.Year.ToString())
                    {
                        deuda = double.Parse(deuda_actual.Rows[fila]["deuda_del_mes"].ToString());
                        break;
                    }
                }
            }
            return deuda;
        }
        public string get_deuda_total_mes(string sucursal, string mes, string año)
        {
            string deuda = "0";
            sucursalBD = administracion.get_sucursal_nombre(sucursal);
            string id_sucursal = sucursalBD.Rows[0]["id"].ToString();
            double deuda_registrada, deuda_calculada;
            if (deuda_mes == null)
            {
                consultar_deuda_mes(id_sucursal, mes, año);
            }
            else if (deuda_mes.Rows[0]["mes"].ToString() != mes ||
                     deuda_mes.Rows[0]["año"].ToString() != año ||
                     deuda_mes.Rows[0]["sucursal"].ToString() != sucursal)
            {
                consultar_deuda_mes(id_sucursal, mes, año);
            }
            if (deuda_mes.Rows.Count > 0)
            {
                deuda = deuda_mes.Rows[0]["deuda_del_mes"].ToString();
                deuda_registrada = Math.Round(double.Parse(deuda), 2);
                deuda_calculada = Math.Round(calcular_deuda_del_mes(sucursal, mes, año), 2);
                if (deuda_registrada != deuda_calculada)
                {
                    //actualizar deuda en bd
                    string id_deuda = deuda_mes.Rows[0]["id"].ToString();
                    administracion.actualizar_deuda_del_mes(id_deuda, deuda_calculada.ToString());
                    consultar_deuda_mes(id_sucursal, mes, año);
                    deuda = deuda_mes.Rows[0]["deuda_del_mes"].ToString();
                }
            }
            else
            {
                administracion.crear_deuda_del_mes(id_sucursal, sucursal, mes, año, "0");
                consultar_deuda_mes(id_sucursal, mes, año);
                deuda = deuda_mes.Rows[0]["deuda_del_mes"].ToString();
                deuda_registrada = Math.Round(double.Parse(deuda), 2);
                deuda_calculada = Math.Round(calcular_deuda_del_mes(sucursal, mes, año), 2);
                if (deuda_registrada != deuda_calculada)
                {
                    //actualizar deuda en bd
                    string id_deuda = deuda_mes.Rows[0]["id"].ToString();
                    administracion.actualizar_deuda_del_mes(id_deuda, deuda_calculada.ToString());
                    consultar_deuda_mes(id_sucursal, mes, año);
                    deuda = deuda_mes.Rows[0]["deuda_del_mes"].ToString();
                }
            }
            return deuda;
        }
        public double calcular_deuda_del_mes(string sucursal, string mes, string año)
        {
            double deuda_del_mes = 0;
            get_remitos(sucursal, mes, año);
            get_imputaciones(sucursal, mes, año);

            double deuda_anterior = get_deuda_mes_anterior(sucursal, mes, año);

            double total_compra = 0;
            double total_cobrado = 0;
            double abono_efectivo, abono_digital, abono_digital_mercadoPago, total;
            for (int fila_remito = 0; fila_remito <= remitos.Rows.Count - 1; fila_remito++)
            {
                total_compra = total_compra + double.Parse(remitos.Rows[fila_remito]["valor_remito"].ToString());
            }
            for (int fila_imputacion = 0; fila_imputacion <= imputaciones.Rows.Count - 1; fila_imputacion++)
            {
                if (imputaciones.Rows[fila_imputacion]["autorizado"].ToString() == "Si")
                {
                    abono_efectivo = double.Parse(imputaciones.Rows[fila_imputacion]["abono_efectivo"].ToString());
                    abono_digital = double.Parse(imputaciones.Rows[fila_imputacion]["abono_digital"].ToString());
                    abono_digital_mercadoPago = double.Parse(imputaciones.Rows[fila_imputacion]["abono_digital_mercadoPago"].ToString());
                    total = abono_efectivo + abono_digital + abono_digital_mercadoPago;
                    total_cobrado = total_cobrado + total;
                }
            }
            deuda_del_mes = (deuda_anterior + total_compra) - total_cobrado;
            return deuda_del_mes;
        }
        public double calcular_compra_mes(string sucursal, string mes, string año)
        {
            get_remitos(sucursal, mes, año);


            double total_compra = 0;
            for (int fila_remito = 0; fila_remito <= remitos.Rows.Count - 1; fila_remito++)
            {
                total_compra = total_compra + double.Parse(remitos.Rows[fila_remito]["valor_remito"].ToString());
            }

            return total_compra;
        }
        public double calcular_pago_mes(string sucursal, string mes, string año)
        {
            get_imputaciones(sucursal, mes, año);


            double total_cobrado = 0;
            double abono_efectivo, abono_digital, abono_digital_mercadoPago, total;

            for (int fila_imputacion = 0; fila_imputacion <= imputaciones.Rows.Count - 1; fila_imputacion++)
            {
                if (imputaciones.Rows[fila_imputacion]["autorizado"].ToString() == "Si")
                {
                    abono_efectivo = double.Parse(imputaciones.Rows[fila_imputacion]["abono_efectivo"].ToString());
                    abono_digital = double.Parse(imputaciones.Rows[fila_imputacion]["abono_digital"].ToString());
                    abono_digital_mercadoPago = double.Parse(imputaciones.Rows[fila_imputacion]["abono_digital_mercadoPago"].ToString());
                    total = abono_efectivo + abono_digital + abono_digital_mercadoPago;
                    total_cobrado = total_cobrado + total;
                }
            }
            return total_cobrado;
        }
        public double get_deuda_mes_anterior(string sucursal, string mes, string año)
        {
            double retorno = 0;
            sucursalBD = administracion.get_sucursal_nombre(sucursal);
            string id_sucursal = sucursalBD.Rows[0]["id"].ToString();
            string mes_anterior = obtener_mes_anterior(mes);
            string año_anterior = obtener_año_anterior(mes, año);
           
                consultar_deuda_mes_anterior(id_sucursal, mes_anterior, año_anterior);
            if (deuda_mes_anterior.Rows.Count > 0)
            {
                for (int fil = 0; fil <= deuda_mes_anterior.Rows.Count - 1; fil++)
                {
                    if (deuda_mes_anterior.Rows[fil]["mes"].ToString() == mes_anterior &&
                     deuda_mes_anterior.Rows[fil]["año"].ToString() == año_anterior &&
                     deuda_mes_anterior.Rows[fil]["id_sucursal"].ToString() == id_sucursal)
                    {
                        retorno = double.Parse(deuda_mes_anterior.Rows[fil]["deuda_del_mes"].ToString());
                        break;
                    }
                }
            }
            return retorno;
        }

        public DataTable get_sucursal(string id)
        {
            consultar_sucursal(id);
            return sucursalBD;
        }
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        public DataTable get_remitos(string sucursal, string mes, string año)
        {
            consultar_remitos(sucursal, mes, año);

          /*  if (remitos == null)
            {
                consultar_remitos(sucursal, mes, año);
            }
            else if (remitos.Rows.Count == 0)
            {
                consultar_remitos(sucursal, mes, año);
            }
            else
            {
                DateTime fecha_remito = (DateTime)remitos.Rows[0]["fecha_remito"];
                if (fecha_remito.Month.ToString() != mes ||
                     fecha_remito.Year.ToString() != año ||
                     remitos.Rows[0]["sucursal"].ToString() != sucursal)
                {
                    consultar_remitos(sucursal, mes, año);
                }
            }*/
            return remitos;
        }
        public DataTable get_remitos_proveedores_a_fabrica()
        {
            consultar_remitos_proveedores_a_fabrica();
            return remitos_proveedores_a_fabrica;
        }
        public DataTable get_imputaciones(string sucursal, string mes, string año)
        {
            consultar_imputaciones(sucursal, mes, año);

           /* if (imputaciones == null)
            {
                consultar_imputaciones(sucursal, mes, año);
            }
            else if (imputaciones.Rows.Count == 0)
            {
                consultar_imputaciones(sucursal, mes, año);
            }
            else
            {
                if (imputaciones.Rows.Count > 0)
                {
                    DateTime fecha = (DateTime)imputaciones.Rows[0]["fecha"];
                    if (fecha.Month.ToString() != mes ||
                         fecha.Year.ToString() != año ||
                         imputaciones.Rows[0]["sucursal"].ToString() != sucursal)
                    {
                        consultar_imputaciones(sucursal, mes, año);
                    }
                }
            }*/
            return imputaciones;
        }
        public DataTable get_imputaciones_fabrica_a_proveedor()
        {
            consultar_imputaciones_fabrica_a_proveedor();
            return imputaciones_fabrica_a_proveedor;
        }
        public DataTable get_lista_proveedores_fabrica()
        {
            consultar_lista_proveedores_fabrica();
            return lista_proveedores_fabrica;
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------------
        #region metodos privados

        private int buscar_fila_acuerdo_pedido_fabrica(int fila_pedido)
        {
            consultar_acuerdo_de_precios();
            string tipo_de_acuerdo, acuerdo, proveedor;

            tipo_de_acuerdo = pedidos_fabrica_a_proveedor.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            acuerdo = pedidos_fabrica_a_proveedor.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
            proveedor = pedidos_fabrica_a_proveedor.Rows[fila_pedido]["proveedor"].ToString();

            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios.Rows.Count - 1)
            {
                if (tipo_de_acuerdo == acuerdo_de_precios.Rows[fila]["tipo_de_acuerdo"].ToString() && acuerdo == acuerdo_de_precios.Rows[fila]["acuerdo"].ToString() && proveedor == acuerdo_de_precios.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_pedido_fabrica(string id_pedido)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos_fabrica_a_proveedor.Rows.Count - 1)
            {
                if (id_pedido == pedidos_fabrica_a_proveedor.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
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
        private int obtener_fila_remito_fabrica(string id)
        {
            consultar_remitos_proveedores_a_fabrica();
            int retorno = 0;
            int fila = 0;
            while (fila <= remitos_proveedores_a_fabrica.Rows.Count - 1)
            {
                if (id == remitos_proveedores_a_fabrica.Rows[fila]["id"].ToString())
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
        private int obtener_fila_de_pedido(string num_pedido, string sucursal_seleccionada)
        {
            consultar_pedidos();
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos.Rows.Count - 1)
            {
                if (num_pedido == pedidos.Rows[fila]["num_pedido"].ToString() && sucursal_seleccionada == pedidos.Rows[fila]["sucursal"].ToString())
                {
                    retorno = fila; break;
                }
                fila++;
            }

            return retorno;
        }
        private int obtener_fila_de_pedido_mediante_idRemito(string id_pedido, string sucursal_seleccionada)
        {
            consultar_pedidos();
            int fila_remito = obtener_fila_de_remito(id_pedido);
            string num_pedido = remitos.Rows[fila_remito]["num_pedido"].ToString();
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos.Rows.Count - 1)
            {
                if (num_pedido == pedidos.Rows[fila]["num_pedido"].ToString() && sucursal_seleccionada == pedidos.Rows[fila]["sucursal"].ToString())
                {
                    retorno = fila; break;
                }
                fila++;
            }

            return retorno;
        }
        private int obtener_fila_de_pedido_fabrica(string num_pedido, string fabrica_seleccionada)
        {
            consultar_pedidos_fabrica();
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos_fabrica_a_proveedor.Rows.Count - 1)
            {
                if (num_pedido == pedidos_fabrica_a_proveedor.Rows[fila]["num_pedido"].ToString() && fabrica_seleccionada == pedidos_fabrica_a_proveedor.Rows[fila]["fabrica"].ToString())
                {
                    retorno = fila; break;
                }
                fila++;
            }

            return retorno;
        }

        private int obtener_fila_de_pedido_por_id(string id_pedido, string fabrica_seleccionada)
        {
            consultar_remitos_proveedores_a_fabrica();
            int retorno = 0;
            int fila = 0;
            int fila_remito = 0;
            while (fila <= remitos_proveedores_a_fabrica.Rows.Count - 1)
            {
                if (id_pedido == remitos_proveedores_a_fabrica.Rows[fila]["id"].ToString())
                {
                    fila_remito = fila;
                    break;
                }
                fila++;
            }
            string num_pedido = remitos_proveedores_a_fabrica.Rows[fila_remito]["num_pedido"].ToString();
            retorno = obtener_fila_de_pedido_fabrica(num_pedido, fabrica_seleccionada);
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
        private string obtener_nombre_proveedor_fabrica(string provedor_seleccionado)
        {

            string retorno = provedor_seleccionado;
            int fila;
            fila = 0;
            consultar_lista_proveedores_fabrica();
            while (fila <= lista_proveedores_fabrica.Rows.Count - 1)
            {
                if (provedor_seleccionado == lista_proveedores_fabrica.Rows[fila]["nombre_proveedor"].ToString())
                {
                    retorno = lista_proveedores_fabrica.Rows[fila]["nombre_en_BD"].ToString();
                    break;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_nombre_proveedor_fabrica_remito(string provedor_seleccionado)
        {

            string retorno = provedor_seleccionado;
            int fila;
            fila = 0;
            consultar_lista_proveedores_fabrica();
            while (fila <= lista_proveedores_fabrica.Rows.Count - 1)
            {
                if (provedor_seleccionado == lista_proveedores_fabrica.Rows[fila]["nombre_en_BD"].ToString())
                {
                    retorno = lista_proveedores_fabrica.Rows[fila]["nombre_proveedor"].ToString();
                    break;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private void abrir_pedido(int fila_pedido, int fila_acuerdo, string nombre_proveedor)
        {
            string precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, tipo_paquete, unidad_insumo, tipo_unidad, dato;
            crear_tabla_resumen();
            crear_tabla_bonificado();

            int i = 1;
            for (int columna = pedidos.Columns["producto_1"].Ordinal; columna <= pedidos.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(pedidos.Rows[fila_pedido]["producto_" + i.ToString()]))
                {
                    if (pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                    {
                        //extraer precio
                        precio = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                        //extraer id
                        id = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);
                        //extraer producto
                        producto = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad pedida
                        cantidad_pedida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                        //extraer cantidad entregada
                        cantidad_entregada = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);//
                                                                                                                                                        //extraer cantidad de kilos
                        cantidad_recibida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);//;
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
                        if (cantidad_recibida == "")
                        {
                            cantidad_recibida = "0";
                        }
                        //cargar normal
                        cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, fila_acuerdo, dato, unidad_insumo, "N/A", "N/A", "N/A");
                    }
                }
                i++;
            }
        }
        private void abrir_pedido_remito_de_carga(DataTable resumen)
        {
            string acuerdo, num_acuerdo, nombre_proveedor, num_pedido;
            int fila_pedido, fila_acuerdo;
           
            consultar_sucursales();

            consultar_acuerdo_de_precios();
            //   consultar_remitos(sucursal, mes, año);
            crear_tabla_resumen();
            string id_remito, sucursal_seleccionada, id_sucursal;
            for (int fila_resumen = 0; fila_resumen <= resumen.Rows.Count - 1; fila_resumen++)
            {
                id_remito = resumen.Rows[fila_resumen]["id"].ToString();
                sucursal_seleccionada = resumen.Rows[fila_resumen]["sucursal"].ToString();
                fila_pedido = obtener_fila_de_pedido_mediante_idRemito(id_remito, sucursal_seleccionada);

                acuerdo = pedidos.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
                num_pedido = pedidos.Rows[fila_pedido]["num_pedido"].ToString();
                num_acuerdo = pedidos.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
                nombre_proveedor = pedidos.Rows[fila_pedido]["proveedor"].ToString();

                fila_acuerdo = obtener_fila_de_acuerdo(acuerdo, num_acuerdo, nombre_proveedor);
                id_sucursal = buscar_id_sucursal(sucursal_seleccionada, sucursales);
                consultar_sucursal(id_sucursal);
                consultar_productos_proveedor(nombre_proveedor);

                ////////////////////////////////////////////////////////////////
                string precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, tipo_paquete, unidad_insumo, tipo_unidad, dato;


                int i = 1;
                for (int columna = pedidos.Columns["producto_1"].Ordinal; columna <= pedidos.Columns.Count - 1; columna++)
                {
                    if (IsNotDBNull(pedidos.Rows[fila_pedido]["producto_" + i.ToString()]))
                    {
                        if (pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                        {
                            //extraer precio
                            precio = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                            //extraer id
                            id = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);
                            //extraer producto
                            producto = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                            //extraer cantidad pedida
                            cantidad_pedida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                            //extraer cantidad entregada
                            cantidad_entregada = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);//
                                                                                                                                                            //extraer cantidad de kilos
                            cantidad_recibida = obtener_dato_pedido(pedidos.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);//;
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
                            if (cantidad_recibida == "")
                            {
                                cantidad_recibida = "0";
                            }
                            //cargar normal
                            cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, fila_acuerdo, dato, unidad_insumo, sucursal_seleccionada, num_pedido, nombre_proveedor); // sucursal_seleccionada num_pedido nombre_proveedor
                        }
                    }
                    i++;
                }
            }




        }
        private void abrir_pedido_fabrica(int fila_pedido, int fila_acuerdo)
        {
            string precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida;
            crear_tabla_resumen();
            crear_tabla_bonificado();

            int i = 1;
            for (int columna = pedidos_fabrica_a_proveedor.Columns["producto_1"].Ordinal; columna <= pedidos_fabrica_a_proveedor.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()]))
                {
                    if (pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                    {
                        //extraer precio
                        precio = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                        //extraer id
                        id = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);
                        //extraer producto
                        producto = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad pedida
                        cantidad_pedida = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                        //extraer cantidad entregada
                        cantidad_entregada = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 5);//
                                                                                                                                                                            //extraer cantidad recibida                                                                                                                                                //extraer cantidad de kilos
                        cantidad_recibida = obtener_dato_pedido(pedidos_fabrica_a_proveedor.Rows[fila_pedido]["producto_" + i.ToString()].ToString().Replace(",", "."), 6);//;
                        if (cantidad_recibida == "")
                        {
                            cantidad_recibida = "0";
                        }

                        if (precio == "BONIFICADO" || precio == "BONIFICADO ESPECIAL")
                        {
                            //cargar bonificado
                            cargar_bonificado(precio, id, producto, cantidad_pedida, cantidad_entregada, fila_acuerdo);
                        }
                        else
                        {
                            //cargar normal
                            cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, cantidad_recibida, fila_acuerdo, "N/A", "N/A", "N/A", "N/A", "N/A");
                        }
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
        private void cargar_producto(string precio, string id, string producto, string cantidad_pedida, string cantidad_entregada, string cantidad_recibida, int fila_acuerdo, string unidad_insumo, string multiplicador, string sucursal_seleccionada, string num_pedido, string nombre_proveedor)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;
            int fila_producto = buscar_fila_producto(id, productos_proveedor);
            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["producto"] = producto;
            resumen_pedido.Rows[fila]["pedido"] = cantidad_pedida;
            resumen_pedido.Rows[fila]["unid.pedida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
            resumen_pedido.Rows[fila]["entregado"] = cantidad_entregada;
            resumen_pedido.Rows[fila]["sucursal_seleccionada"] = sucursal_seleccionada;
            resumen_pedido.Rows[fila]["num_pedido"] = num_pedido;
            resumen_pedido.Rows[fila]["nombre_proveedor"] = nombre_proveedor;
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
            //  precio = precio.Replace(",", ".");
            decimal sub_total = 0;
            if (cantidad_recibida == "")
            {
                sub_total = decimal.Parse("0") * decimal.Parse(precio);

            }
            if (multiplicador != "N/A")
            {
                double nuevo_precio = double.Parse(multiplicador) * double.Parse(precio);
                precio = nuevo_precio.ToString();
            }
            sub_total = decimal.Parse(cantidad_recibida) * decimal.Parse(precio);

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

            resumen_pedido.Columns.Add("facturacion por", typeof(string));
            resumen_pedido.Columns.Add("kilos", typeof(string));
            resumen_pedido.Columns.Add("factura_por_kilo", typeof(string));

            resumen_pedido.Columns.Add("sucursal_seleccionada", typeof(string));
            resumen_pedido.Columns.Add("num_pedido", typeof(string));
            resumen_pedido.Columns.Add("nombre_proveedor", typeof(string));
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
        private string obtener_mes_anterior(string mes_dato)
        {
            string retorno = mes_dato.ToString(); ;
            int mes = int.Parse(mes_dato);

            if (mes > 1 && mes <= 12)
            {
                mes = mes - 1;
                retorno = mes.ToString();
            }
            else if (mes == 1)
            {
                mes = 12;
                retorno = mes.ToString();
            }
            return retorno;
        }
        private string obtener_año_anterior(string mes_dato, string año_dato)
        {
            string retorno = año_dato.ToString(); ;
            int mes = int.Parse(mes_dato);
            int año = int.Parse(año_dato);

            if (mes == 1)
            {
                año = año - 1;
                retorno = año.ToString();
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
        private void consultar_deuda_actual(string sucursal)
        {
            deuda_actual = administracion.get_deuda_actual(sucursal);
        }
        private void consultar_deuda_mes_anterior(string id_sucursal, string mes, string año)
        {
            deuda_mes_anterior = administracion.get_deuda_mes(id_sucursal, mes, año);
        }
        private void consultar_deuda_mes(string id_sucursal, string mes, string año)
        {
            deuda_mes = administracion.get_deuda_mes(id_sucursal, mes, año);
        }
        private void consultar_todas_las_imputaciones_fabrica()
        {
            todas_las_imputaciones_fabrica = administracion.get_imputaciones_fabrica_a_proveedor();
        }
        private void consultar_todos_los_remitos_fabrica()
        {
            todos_los_remitos_fabrica = administracion.get_remitos_proveedores_a_fabrica();
        }
        private void consultar_todas_las_imputaciones(string sucursal, string mes, string año)
        {
            todas_las_imputaciones = administracion.get_imputaciones(sucursal, mes, año);
        }
        private void consultar_todos_los_remitos(string sucursal, string mes, string año)
        {
            todos_los_remitos = administracion.get_remitos(sucursal, mes, año);
        }
        private void consultar_pedidos_fabrica()
        {
            pedidos_fabrica_a_proveedor = administracion.get_pedidos_fabrica();
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = administracion.get_acuerdo_de_precio();
        }
        private void consultar_lista_proveedores_fabrica()
        {
            lista_proveedores_fabrica = administracion.get_lista_proveedores_fabrica();
            lista_proveedores_fabrica.DefaultView.Sort = "proveedor ASC";
            lista_proveedores_fabrica = lista_proveedores_fabrica.DefaultView.ToTable();
        }
        private void consultar_pedidos()
        {
            pedidos = administracion.get_pedidos();
        }
        private void consultar_sucursal(string id)
        {
            sucursalBD = administracion.get_sucursales_id(id);
        }
        private void consultar_sucursales()
        {
            sucursales = administracion.get_sucursales();
        }
        private void consultar_imputaciones(string sucursal, string mes, string año)
        {
            imputaciones = administracion.get_imputaciones(sucursal, mes, año);
        }
        private void consultar_imputaciones_fabrica_a_proveedor()
        {
            imputaciones_fabrica_a_proveedor = administracion.get_imputaciones_fabrica_a_proveedor();
        }
        private void consultar_remitos(string sucursal, string mes, string año)
        {
            remitos = administracion.get_remitos(sucursal, mes, año);
        }
        private void consultar_remitos_todas_las_sucursales( string mes, string año)
        {
            remitos = administracion.get_remitos_todas_las_sucursales( mes, año);
        }
        private void consultar_remitos_proveedores_a_fabrica()
        {
            remitos_proveedores_a_fabrica = administracion.get_remitos_proveedores_a_fabrica();
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
        #endregion
    }
}
