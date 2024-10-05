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
        DataTable tipo_movimientos_caja_chica;
        DataTable movimientos_caja_chica;
        DataTable conceptos_ingresos;
        DataTable detalles;
        #endregion

        #region carga a base de datos
        public void cargar_transaccion()
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //fecha
            columnas = funciones.armar_query_columna(columnas,"fecha",false);
            valores = funciones.armar_query_valores(valores,funciones.get_fecha(),false);
            //producto
            //entrega
            //recibe
            //direccion
            //contacto
            //nota
        }
        #endregion

    }
}
