using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_historial_merma_y_desperdicio
    {
        public cls_historial_merma_y_desperdicio(DataTable usuario_BD)
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
            estadisticas = new cls_estadisticas_de_pedidos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_estadisticas_de_pedidos estadisticas;
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable merma_y_desperdicio;
        #endregion

        #region metodos consultas
        private void consultar_merma_y_desperdicio()
        {
            merma_y_desperdicio = consultas.consultar_tabla(base_de_datos, "merma_y_desperdicio");
        }
        #endregion
        #region cargar nota
        public void cargar_nota(string id_orden, string nota)
        {
            string actualizar = "`nota` = '" + nota + "' ";
            consultas.actualizar_tabla(base_de_datos, "merma_y_desperdicio", actualizar, id_orden);
        }
        #endregion
        #region metodos get/set
        public DataTable get_merma_y_desperdicio()
        {
            consultar_merma_y_desperdicio();
            merma_y_desperdicio.DefaultView.Sort = "fecha DESC";
            merma_y_desperdicio = merma_y_desperdicio.DefaultView.ToTable();
            return merma_y_desperdicio;
        }
        #endregion
    }
}
