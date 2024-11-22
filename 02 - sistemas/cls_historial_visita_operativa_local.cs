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
    public class cls_historial_visita_operativa_local
    {
        public cls_historial_visita_operativa_local(DataTable usuario_BD)
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

        DataTable historial_evaluacion_chequeo;
        #endregion

        #region metodos consultas
        private void consultar_historial_evaluacion_chequeo(string año,string mes, string id_sucursal)
        {
            historial_evaluacion_chequeo = consultas.consultar_historial_evaluacion_chequeo_segun_mes_y_año(año,mes,id_sucursal);
        }
        #endregion

        #region metodos get/set
        public DataTable get_historial_evaluacion_chequeo(string año, string mes,string id_sucursal)
        {
            consultar_historial_evaluacion_chequeo(año,mes,id_sucursal);
            DataTable resumen = new DataTable();
            resumen.Columns.Add("fecha", typeof(string));
            resumen.Columns.Add("fecha_historial", typeof(string));
            DateTime fecha_evaluacion;
            string fecha;
            for (int fila = 0; fila <= historial_evaluacion_chequeo.Rows.Count - 1; fila++)
            {
                fecha_evaluacion =  DateTime.Parse(historial_evaluacion_chequeo.Rows[fila]["fecha"].ToString());
                fecha = fecha_evaluacion.ToString("dd/MM/yyyy");
                if (-1 == funciones.buscar_fila_por_dato(fecha,"fecha",resumen))
                {
                    resumen.Rows.Add();
                    resumen.Rows[resumen.Rows.Count - 1]["fecha"] = fecha;
                    resumen.Rows[resumen.Rows.Count - 1]["fecha_historial"] = historial_evaluacion_chequeo.Rows[fila]["fecha"].ToString();
                }
            }
            return resumen;
        }
        #endregion
    }
}
