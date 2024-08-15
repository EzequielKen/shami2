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
    public class cls_historial_desperdicio_merma
    {
        public cls_historial_desperdicio_merma(DataTable usuario_BD)
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

        DataTable desperdicio_merma_local;
        #endregion

        #region metodos consultas
        private void consultar_desperdicio_merma_local(string id_sucursal, DateTime fecha, string categoria)
        {
            string año = fecha.Year.ToString();
            string mes = fecha.Month.ToString();
            string dia = fecha.Day.ToString();
            desperdicio_merma_local = consultas.consultar_desperdicio_merma_local_segun_fecha(id_sucursal, año, mes, dia, categoria);
        }
        #endregion

        #region metodos get/set
        public DataTable get_desperdicio_merma_local(string id_sucursal, DateTime fecha, string categoria)
        {
            consultar_desperdicio_merma_local(id_sucursal, fecha, categoria);
            return desperdicio_merma_local;
        }
        #endregion
    }
}
