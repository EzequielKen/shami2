using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_acctualizador_precio_venta
    {
        public cls_acctualizador_precio_venta(DataTable usuario_BD)
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
        DataTable precio_compra;
        DataTable precio_venta;
        DataTable tipo_acuerdo;
        #endregion

        #region metodos consultas
        private void consultar_productos()
        {
            productos = consultas.consultar_tabla(base_de_datos, "productos");
        }
        private void consultar_precio_venta(string proveedor, string tipo_de_acuerdo)
        {
            precio_venta = consultas.consultar_precio_venta_activo(proveedor, tipo_de_acuerdo);
        }
        private void consultar_tipo_acuerdo()
        {
            tipo_acuerdo = consultas.consultar_tabla(base_de_datos, "tipo_acuerdo");
        }
        private void consultar_precio_compra()
        {
            precio_compra = consultas.consultar_precios_compra_activo();
        }
        #endregion

        #region metodos privados
        private void cargar_precio_venta_en_productos()
        {
            productos.Columns.Add("precio_venta", typeof(string));
            int fila_precio;
            string id_producto;
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                id_producto = productos.Rows[fila]["id"].ToString();
                fila_precio = funciones.buscar_fila_por_dato_en_columna(id_producto, "id_producto", precio_venta);
                if (fila_precio != -1)
                {
                    productos.Rows[fila]["precio_venta"] = precio_venta.Rows[fila_precio]["precio"].ToString();
                }
                else
                {
                    productos.Rows[fila]["precio_venta"] = "N/A";
                }
            }
        }
        private void cargar_precio_compra_en_productos()
        {
            productos.Columns.Add("precio_compra", typeof(string));
            productos.Columns.Add("presentacion_compra", typeof(string));
            int fila_precio;
            string id_producto, presentacion;
            double unidad_multiplicador;
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                id_producto = productos.Rows[fila]["id"].ToString();
                fila_precio = funciones.buscar_fila_por_dato_en_columna(id_producto, "id_producto", precio_compra);
                if (fila_precio != -1)
                {
                    presentacion = precio_compra.Rows[fila_precio]["presentacion"].ToString();
                    productos.Rows[fila]["presentacion_compra"] = presentacion;
                    unidad_multiplicador = double.Parse(funciones.obtener_dato(presentacion, 2));
                    productos.Rows[fila]["precio_compra"] = double.Parse(precio_compra.Rows[fila_precio]["precio"].ToString()) * unidad_multiplicador;
                }
                else
                {
                    productos.Rows[fila]["precio_compra"] = "N/A";
                    productos.Rows[fila]["presentacion_compra"] = "N/A";
                }

            }
        }
        private void ordenar_productos()
        {
            productos.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                productos.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(productos.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            productos.DefaultView.Sort = "orden asc";
            productos = productos.DefaultView.ToTable();
        }
        #endregion
        #region metodos get/set
        public DataTable get_productos(string tipo_de_acuerdo)
        {
            consultar_productos();
            consultar_precio_compra();
            consultar_precio_venta("proveedor_villaMaipu", tipo_de_acuerdo);
            cargar_precio_compra_en_productos();
            cargar_precio_venta_en_productos();
            ordenar_productos();

            productos.Columns.Add("precio_nuevo", typeof(string));
            for (int fila = 0; fila <= productos.Rows.Count-1; fila++)
            {
                productos.Rows[fila]["precio_nuevo"] = "N/A";
            }
            return productos;
        }
        public DataTable get_tipo_acuerdo()
        {
            consultar_tipo_acuerdo();
            DataTable resumen = new DataTable();
            resumen.Columns.Add("tipo_de_acuerdo", typeof(string));
            int fila_resumen = 0;
            string tipo_de_acuerdo;
            for (int fila = 0; fila <= tipo_acuerdo.Rows.Count - 1; fila++)
            {
                tipo_de_acuerdo = tipo_acuerdo.Rows[fila]["proveedor_villaMaipu"].ToString();
                fila_resumen = funciones.buscar_fila_por_dato_en_columna(tipo_de_acuerdo, "tipo_de_acuerdo", resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    resumen.Rows[resumen.Rows.Count - 1]["tipo_de_acuerdo"] = tipo_de_acuerdo;
                }
            }
            return resumen;
        }
        #endregion
    }
}
