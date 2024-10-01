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

namespace _03___sistemas_fabrica
{
    public class cls_confirmar_conteo_stock
    {
        public cls_confirmar_conteo_stock(DataTable usuario_BD)
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
            movimientos_stock = new cls_movimientos_stock_producto(usuarioBD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_movimientos_stock_producto movimientos_stock;
        cls_PDF PDF = new cls_PDF();
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable conteo_stock;
        #endregion

        #region PDF
        public void pdf_conteo(string ruta_archivo,DataTable conteo_stock, byte[]logo,string fecha_conteo)
        {
            PDF.GenerarPDF_conteo_stock(ruta_archivo,logo,conteo_stock,fecha_conteo);
        }
        #endregion

        #region carga a base de datos
        public void aprobar_conteo(string id, string rol_usuario, string id_producto, string movimiento, string nota)
        {

            string actualizar = "`aprobado` = 'Si'";
            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);
            actualizar = "`nota` = '" + nota + "'";
            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);

            movimientos_stock.cargar_historial_stock(rol_usuario,id_producto, "conteo stock",movimiento,nota);
        }
        public void eliminar_conteo(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "conteo_stock", actualizar, id);
        }
        #endregion

        #region metodos consultas
        private void consultar_conteo_stock(DateTime fecha)
        {
            conteo_stock = consultas.consultar_conteo_stock_segun_fecha(fecha.Day.ToString(), fecha.Month.ToString(), fecha.Year.ToString());
        }
        #endregion

        #region metodos get/set
        public DataTable get_conteo_stock(DateTime fecha)
        {
            consultar_conteo_stock(fecha);
            return conteo_stock;
        }
        #endregion
    }
}
