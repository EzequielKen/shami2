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
    public class cls_conteo_de_stock
    {
        public cls_conteo_de_stock(DataTable usuario_BD)
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

        DataTable productos;
        #endregion

        #region metodos consultas
        private void consultar_productos()
        {
            productos = consultas.consultar_tabla(base_de_datos, "proveedor_villamaipu");
            productos.Columns.Add("orden",typeof(int));
            productos.Columns.Add("conteo_stock", typeof(string));
            for (int fila = 0; fila <= productos.Rows.Count-1; fila++)
            {
                productos.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(productos.Rows[fila]["tipo_producto"].ToString(),1));
            }
            productos.DefaultView.Sort = "orden ASC";
            productos = productos.DefaultView.ToTable();
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos()
        {
            consultar_productos();
            return productos;
        }
        #endregion
    }
}
