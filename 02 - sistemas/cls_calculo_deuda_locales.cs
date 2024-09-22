using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    internal class cls_calculo_deuda_locales
    {
        public cls_calculo_deuda_locales(DataTable usuario_BD)
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

        DataTable todos_los_remitos;
        DataTable todas_las_imputaciones;
        DataTable remitos_local;
        DataTable imputaciones_local;
        DataTable remitos_local_del_mes;
        DataTable imputaciones_local_del_mes;
        #endregion

        #region metodos consultas
        private void consultar_todos_los_remitos()
        {
            todos_los_remitos = consultas.consultar_todos_los_remitos_cuenta_por_pagar();
        }
        private void consultar_todas_las_imputaciones()
        {
            todas_las_imputaciones = consultas.consultar_todas_las_imputaciones_local();
        }
        private void consultar_remitos_local(string sucursal)
        {
            remitos_local = consultas.consultar_cuentas_por_pagar_segun_sucursal(sucursal);
        }
        private void consultar_imputaciones_local(string sucursal)
        {
            imputaciones_local = consultas.consultar_imputaciones_segun_sucursal(sucursal);
        }
        private void consultar_remitos_local_del_mes(string sucursal, string mes, string año)
        {
            remitos_local_del_mes = consultas.consultar_cuentas_por_pagar_segun_fecha(sucursal, mes, año);
        }
        private void consultar_imputaciones_local_del_mes(string sucursal, string mes, string año)
        {
            imputaciones_local_del_mes = consultas.consultar_imputaciones_segun_fecha(sucursal, mes, año);
        }
        #endregion

        #region metodos privados

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

        #endregion

        #region metodos calculos
        public double calcular_compra_del_mes(string sucursal, string mes_actual, string año_actual)
        {
            double compra = 0;
            consultar_remitos_local_del_mes(sucursal, mes_actual, año_actual);
            string fecha_dato;
            for (int fila = 0; fila <= remitos_local_del_mes.Rows.Count - 1; fila++)
            {
                fecha_dato = remitos_local_del_mes.Rows[fila]["fecha_remito"].ToString();
                if (verificar_fecha_actual(fecha_dato, mes_actual, año_actual))
                {
                    compra = compra + double.Parse(remitos_local_del_mes.Rows[fila]["valor_remito"].ToString());
                }
            }
            return compra;
        }
        public double calcular_pagos_del_mes(string sucursal, string mes_actual, string año_actual)
        {
            double pago = 0;
            consultar_imputaciones_local_del_mes(sucursal, mes_actual, año_actual);
            double sub_total = 0;
            double abono_efectivo, abono_digital, abono_digital_mercadoPago;
            for (int fila = 0; fila <= imputaciones_local_del_mes.Rows.Count - 1; fila++)
            {
                if (imputaciones_local_del_mes.Rows[fila]["autorizado"].ToString() == "Si")
                {
                    abono_efectivo = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_efectivo"].ToString());
                    abono_digital = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_digital"].ToString());
                    abono_digital_mercadoPago = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_digital_mercadoPago"].ToString());

                    sub_total = abono_efectivo + abono_digital + abono_digital_mercadoPago;
                    pago = pago + sub_total;
                }
            }
            return pago;
        }
        public double calcular_deuda_del_mes(string sucursal, string mes_actual, string año_actual)
        {
            double retorno = 0;
            double deuda_mes_anterior = calcular_deuda_mes_anterior(sucursal, mes_actual, año_actual);
            consultar_remitos_local_del_mes(sucursal, mes_actual, año_actual);
            string fecha_dato;
            double compra = 0;
            for (int fila = 0; fila <= remitos_local_del_mes.Rows.Count - 1; fila++)
            {
                fecha_dato = remitos_local_del_mes.Rows[fila]["fecha_remito"].ToString();
                if (verificar_fecha_actual(fecha_dato, mes_actual, año_actual))
                {
                    compra = compra + double.Parse(remitos_local_del_mes.Rows[fila]["valor_remito"].ToString());
                }
            }

            consultar_imputaciones_local_del_mes(sucursal, mes_actual, año_actual);
            double sub_total = 0;
            double abono_efectivo, abono_digital, abono_digital_mercadoPago;
            double pago = 0;
            for (int fila = 0; fila <= imputaciones_local_del_mes.Rows.Count - 1; fila++)
            {
                fecha_dato = imputaciones_local_del_mes.Rows[fila]["fecha"].ToString();
                if (verificar_fecha_actual(fecha_dato, mes_actual, año_actual))
                {
                    if (imputaciones_local_del_mes.Rows[fila]["autorizado"].ToString() == "Si")
                    {
                        abono_efectivo = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_efectivo"].ToString());
                        abono_digital = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_digital"].ToString());
                        abono_digital_mercadoPago = double.Parse(imputaciones_local_del_mes.Rows[fila]["abono_digital_mercadoPago"].ToString());

                        sub_total = abono_efectivo + abono_digital + abono_digital_mercadoPago;
                        pago = pago + sub_total;
                    }
                }
            }
            retorno = (deuda_mes_anterior + compra) - pago;
            return retorno;
        }
        public double calcular_deuda_mes_anterior(string sucursal, string mes_actual, string año_actual)
        {
            double retorno = 0;

            consultar_remitos_local(sucursal);
            string fecha_dato;
            double compra = 0;
            for (int fila = 0; fila <= remitos_local.Rows.Count - 1; fila++)
            {
                fecha_dato = remitos_local.Rows[fila]["fecha_remito"].ToString();
                if (verificar_fecha_anterior(fecha_dato, mes_actual, año_actual))
                {
                    compra = compra + double.Parse(remitos_local.Rows[fila]["valor_remito"].ToString());
                }
            }

            consultar_imputaciones_local(sucursal);
            double sub_total = 0;
            double abono_efectivo, abono_digital, abono_digital_mercadoPago;
            double pago = 0;
            for (int fila = 0; fila <= imputaciones_local.Rows.Count - 1; fila++)
            {
                fecha_dato = imputaciones_local.Rows[fila]["fecha"].ToString();
                if (verificar_fecha_anterior(fecha_dato, mes_actual, año_actual))
                {
                    if (imputaciones_local.Rows[fila]["autorizado"].ToString() == "Si")
                    {
                        abono_efectivo = double.Parse(imputaciones_local.Rows[fila]["abono_efectivo"].ToString());
                        abono_digital = double.Parse(imputaciones_local.Rows[fila]["abono_digital"].ToString());
                        abono_digital_mercadoPago = double.Parse(imputaciones_local.Rows[fila]["abono_digital_mercadoPago"].ToString());

                        sub_total = abono_efectivo + abono_digital + abono_digital_mercadoPago;
                        pago = pago + sub_total;
                    }
                }
            }
            retorno = compra - pago;
            return retorno;
        }
        #endregion
    }
}
