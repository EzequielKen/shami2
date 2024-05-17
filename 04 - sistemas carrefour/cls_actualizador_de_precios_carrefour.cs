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

namespace _04___sistemas_carrefour
{
    public class cls_actualizador_de_precios_carrefour
    {
        public cls_actualizador_de_precios_carrefour(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_carrefour;
        DataTable acuerdo_de_precios_carrefour;
        #endregion

        #region carga a base de datos
        public void actualizar_precios(DataTable productosBD)
        {
            consultar_acuerdo_de_precios_carrefour();
            string id_acuerdo_activo = acuerdo_de_precios_carrefour.Rows[0]["id"].ToString();
            string num_acuerdo_actual = acuerdo_de_precios_carrefour.Rows[0]["acuerdo"].ToString();
            int num_nuevo_acuerdo = int.Parse(num_acuerdo_actual)+1;

            string columna ="";
            string valores ="";
            //tipo_de_acuerdo
            columna = funciones.armar_query_columna(columna, "tipo_de_acuerdo", false);
            valores = funciones.armar_query_valores(valores, acuerdo_de_precios_carrefour.Rows[0]["tipo_de_acuerdo"].ToString(), false);
            //acuerdo
            columna = funciones.armar_query_columna(columna, "acuerdo", false);
            valores = funciones.armar_query_valores(valores, num_nuevo_acuerdo.ToString(), false);
            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //producto_1
            string id_producto = productosBD.Rows[0]["id"].ToString();
            for (int fila = 0; fila < productosBD.Rows.Count-1; fila++)
            {
                id_producto = productosBD.Rows[fila]["id"].ToString();
                columna = funciones.armar_query_columna(columna, "producto_"+ id_producto, false);
                if (productosBD.Rows[fila]["precio_nuevo"].ToString()!="N/A")
                {
                    valores = funciones.armar_query_valores(valores, productosBD.Rows[fila]["precio_nuevo"].ToString(), false);
                }
                else
                {
                    valores = funciones.armar_query_valores(valores, productosBD.Rows[fila]["precio"].ToString(), false);
                }
            }
            int ultima_fila= productosBD.Rows.Count - 1;
            id_producto = productosBD.Rows[ultima_fila]["id"].ToString();
            columna = funciones.armar_query_columna(columna, "producto_" + id_producto, true);
            if (productosBD.Rows[ultima_fila]["precio_nuevo"].ToString() != "N/A")
            {
                valores = funciones.armar_query_valores(valores, productosBD.Rows[ultima_fila]["precio_nuevo"].ToString(), true);
            }
            else
            {
                valores = funciones.armar_query_valores(valores, productosBD.Rows[ultima_fila]["precio"].ToString(), true);
            }

            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios_carrefour",columna, valores);
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios_carrefour", "`activa` = '0'", id_acuerdo_activo);
        }
        #endregion

        #region metodos consultas
        private void consultar_acuerdo_de_precios_carrefour()
        {
           acuerdo_de_precios_carrefour = consultas.consultar_acuerdo_de_precio_activo_carrefour();
        }
        private void consultar_productos_carrefour()
        {
            productos_carrefour = consultas.consultar_tabla_completa(base_de_datos, "productos_carrefour");
        }
        #endregion

        #region metodos get/set

        public DataTable get_productos_carrefour()
        {
            consultar_productos_carrefour();
            consultar_acuerdo_de_precios_carrefour();
            string id_producto;
            productos_carrefour.Columns.Add("precio", typeof(string)); 
            productos_carrefour.Columns.Add("precio_nuevo", typeof(string)); 
            for (int fila = 0; fila <= productos_carrefour.Rows.Count-1; fila++)
            {
                id_producto = productos_carrefour.Rows[fila]["id"].ToString();
                productos_carrefour.Rows[fila]["precio"] = acuerdo_de_precios_carrefour.Rows[0]["producto_"+ id_producto].ToString();
                productos_carrefour.Rows[fila]["precio_nuevo"] = "N/A";
            }
            return productos_carrefour;
        }

        #endregion
    }
}
