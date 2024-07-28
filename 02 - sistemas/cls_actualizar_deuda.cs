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
    public class cls_actualizar_deuda
    {
        public cls_actualizar_deuda(DataTable usuario_BD)
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
        cls_sistema_cuentas_por_pagar cuentas_por_pagar;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable sucursales;
        #endregion

        #region actualizar deuda
        public void actualizar_deuda(DataTable sucursal, DataTable usuario)
        {
            if (sucursal.Rows.Count>0)
            {
                DateTime fecha_dato = new DateTime(2023, 10, 1);
                DateTime fecha_actual = DateTime.Now;
                string id_sucursal, sucursal_nombre, mes, año;
                cuentas_por_pagar = new cls_sistema_cuentas_por_pagar(usuario, sucursal);
                while (fecha_dato.Year != fecha_actual.Year && fecha_dato.Month != fecha_actual.Month)
                {
                    id_sucursal = sucursal.Rows[0]["id"].ToString();
                    sucursal_nombre = sucursal.Rows[0]["sucursal"].ToString();
                    mes = fecha_dato.Month.ToString();
                    año = fecha_dato.Year.ToString();
                    cuentas_por_pagar.actualizar_deuda_total_mes(id_sucursal, sucursal_nombre, mes, año);
                    fecha_dato = fecha_dato.AddMonths(1);
                }
            }
        }
        #endregion

        #region metodos consultas
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla_completa(base_de_datos, "sucursal");
        }
        #endregion

    }
}
