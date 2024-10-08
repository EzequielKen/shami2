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
    public class cls_movimiento_mercaderia_interna_gerente
    {
        public cls_movimiento_mercaderia_interna_gerente(DataTable usuario_BD)
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
        DataTable movimiento_mercaderia_interna;
        #endregion

        #region PDF
        public void Generar_PDF(string id,string ruta_archivo, byte[] imgdata)
        {
            DataTable movimiento_mercaderia = consultas.consultar_movimiento_mercaderia_interna_por_id(id);
            PDF.GenerarPDF_movimiento_mercaderia_interna(ruta_archivo,imgdata, movimiento_mercaderia);
        }
        #endregion

        #region carga a base de datos
        public void eliminar_movimiento(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "movimiento_mercaderia_interna",actualizar,id);
        }
        public void cargar_transaccion(DataTable transaccion)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //fecha
            columnas = funciones.armar_query_columna(columnas,"fecha",false);
            valores = funciones.armar_query_valores(valores,funciones.get_fecha(),false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["producto"].ToString(), false);
            //cantidad
            columnas = funciones.armar_query_columna(columnas, "cantidad", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["cantidad"].ToString(), false);
            //entrega
            columnas = funciones.armar_query_columna(columnas, "entrega", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["entrega"].ToString(), false);
            //recibe
            columnas = funciones.armar_query_columna(columnas, "recibe", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["recibe"].ToString(), false);
            //direccion
            columnas = funciones.armar_query_columna(columnas, "direccion", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["direccion"].ToString(), false);
            //contacto
            columnas = funciones.armar_query_columna(columnas, "contacto", false);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["contacto"].ToString(), false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, transaccion.Rows[0]["nota"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "movimiento_mercaderia_interna",columnas, valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_movimiento_mercaderia_interna(DateTime fecha)
        {
            movimiento_mercaderia_interna = consultas.consultar_movimiento_mercaderia_interna_segun_fecha(fecha.Day.ToString(),fecha.Month.ToString(),fecha.Year.ToString());
        }
        #endregion

        #region metodos get/set
        public DataTable get_movimiento_mercaderia_interna(DateTime fecha)
        {
            consultar_movimiento_mercaderia_interna(fecha);
            return movimiento_mercaderia_interna;
        }
        #endregion
    }
}
