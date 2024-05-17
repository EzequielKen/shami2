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

namespace _06___sistemas_gerente
{
    public class cls_analisis_caja_chica
    {
        public cls_analisis_caja_chica(DataTable usuario_BD)
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

        cls_PDF PDF = new cls_PDF();
        DataTable tipo_movimientos_caja_chica;
        DataTable movimientos_caja_chica;
        DataTable conceptos_ingresos;
        DataTable detalles;
        #endregion

        #region metodos conceptos
        private void calcular_totales_de_conceptos(string mes, string año)
        {
            string concepto;
            double total=0;
            double cantidad_movimiento=0;
            for (int fila = 0; fila <= tipo_movimientos_caja_chica.Rows.Count - 1; fila++)
            {
                concepto = tipo_movimientos_caja_chica.Rows[fila]["concepto"].ToString();
                total=0;
                cantidad_movimiento=0;
                for (int fila_movimiento = 0; fila_movimiento <= movimientos_caja_chica.Rows.Count - 1; fila_movimiento++)
                {
                    if (concepto == movimientos_caja_chica.Rows[fila_movimiento]["concepto"].ToString() &&
                        funciones.verificar_fecha(movimientos_caja_chica.Rows[fila_movimiento]["fecha"].ToString(),mes,año))
                    {
                        cantidad_movimiento = cantidad_movimiento + 1;
                        total = total + double.Parse(movimientos_caja_chica.Rows[fila_movimiento]["movimiento"].ToString());
                    }
                }
                tipo_movimientos_caja_chica.Rows[fila]["cantidad_movimiento"] = cantidad_movimiento.ToString(); 
                tipo_movimientos_caja_chica.Rows[fila]["total"] = total; 
            }
        }
        #endregion

        #region metodos privados
     
        private bool verificar_fecha(DateTime fecha, DateTime fecha_movimiento)
        {
            bool retorno = false;
            if (fecha_movimiento.Year < fecha.Year)
            {
                retorno = true;
            }
            if (fecha_movimiento.Year == fecha.Year)
            {
                if (fecha_movimiento.Month < fecha.Month)
                {
                    retorno = true;
                }
                else if (fecha_movimiento.Month == fecha.Month)
                {
                    if (fecha_movimiento.Day <= fecha.Day)
                    {
                        retorno = true;
                    }
                }
            }
            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_tipo_movimientos_caja_chica()
        {
            tipo_movimientos_caja_chica = consultas.consultar_tabla(base_de_datos, "tipo_movimientos_caja_chica");
            tipo_movimientos_caja_chica.Columns.Add("cantidad_movimiento", typeof(string));
            tipo_movimientos_caja_chica.Columns.Add("total", typeof(double));
            for (int fila = 0; fila <= tipo_movimientos_caja_chica.Rows.Count - 1; fila++)
            {
                tipo_movimientos_caja_chica.Rows[fila]["cantidad_movimiento"] = "0";
                tipo_movimientos_caja_chica.Rows[fila]["total"] = 0;
            }
        }
        private void consultar_detalles(string concepto, string mes, string año)
        {
            detalles=consultas.consultar_detalles_caja_chica(concepto,mes,año);
            detalles.Columns.Add("mov",typeof(double));
            detalles.Columns.Add("total",typeof(string));
            detalles.Columns.Add("fecha_simple",typeof(string));
        }
        private void consultar_movimientos_caja_chica()
        {
            movimientos_caja_chica = consultas.consultar_tabla(base_de_datos, "movimientos_caja_chica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_totales_de_conceptos(string mes, string año)
        {
            consultar_tipo_movimientos_caja_chica();
            consultar_movimientos_caja_chica();
            calcular_totales_de_conceptos(mes,año);
            tipo_movimientos_caja_chica.DefaultView.Sort = "total DESC";
            tipo_movimientos_caja_chica = tipo_movimientos_caja_chica.DefaultView.ToTable();
            return tipo_movimientos_caja_chica;
        }
        public DataTable get_detalles(string concepto,string mes, string año)
        {
            consultar_detalles(concepto,mes,año);
            DateTime fecha;
            for (int fila = 0; fila <= detalles.Rows.Count-1; fila++)
            {
                fecha = DateTime.Parse(detalles.Rows[fila]["fecha"].ToString());
                detalles.Rows[fila]["fecha_simple"] = fecha.ToString("dd/MM/yyyy"); ;
                detalles.Rows[fila]["mov"] = double.Parse(detalles.Rows[fila]["movimiento"].ToString());
                detalles.Rows[fila]["total"] = funciones.formatCurrency(detalles.Rows[fila]["movimiento"].ToString());
            }
            detalles.DefaultView.Sort = "mov DESC";
            detalles = detalles.DefaultView.ToTable();
            return detalles;
        }
        #endregion
    }
}
