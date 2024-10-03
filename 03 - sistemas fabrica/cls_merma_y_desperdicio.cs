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
    [Serializable]
    public class cls_merma_y_desperdicio
    {
        public cls_merma_y_desperdicio(DataTable usuario_BD)
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
            stock_producto_terminado = new cls_stock_producto_terminado(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_stock_producto_terminado stock_producto_terminado;
        
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        DataTable insumos_proveedor;
        #endregion

        #region carga a base de datos
        public void sumar_stock(DataTable productos_proveedorBD, string nombre_fabrica, string rol_usuario,string nota)
        {
            string nuevo_stock, id_producto;
            double stock_dato, nuevo_stock_dato, cantidad_recibida;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count-1; fila++)
            {
                if (productos_proveedorBD.Rows[fila]["nuevo_stock"].ToString() !="N/A")
                {
                    nuevo_stock = productos_proveedorBD.Rows[fila]["nuevo_stock"].ToString();
                    id_producto = productos_proveedorBD.Rows[fila]["id"].ToString();

                    stock_dato = double.Parse(productos_proveedorBD.Rows[fila]["stock"].ToString());
                    nuevo_stock_dato = double.Parse(productos_proveedorBD.Rows[fila]["nuevo_stock"].ToString());
                    cantidad_recibida = nuevo_stock_dato - stock_dato;

                    stock_producto_terminado.cargar_historial_stock(rol_usuario,id_producto,"devolucion",cantidad_recibida.ToString(), nota);

                    string actualizar = "`stock` = '" + nuevo_stock + "'";
                    consultas.actualizar_tabla(base_de_datos, nombre_fabrica, actualizar, id_producto);

                    actualizar = "`stock_expedicion` = '" + nuevo_stock + "'";
                    consultas.actualizar_tabla(base_de_datos, nombre_fabrica, actualizar, id_producto);
                }
            }

        }
        #endregion

        #region metodos consultas
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_productos_proveedor_productos_terminados(base_de_datos, nombre_proveedor);
        }
        private void consultar_insumos_proveedor()
        {
            insumos_proveedor = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            productos_proveedor.Columns.Add("presentacion", typeof(string));
            productos_proveedor.Columns.Add("cantidad_desperdicio", typeof(string));
            productos_proveedor.Columns.Add("unidad", typeof(string));
            productos_proveedor.Columns.Add("nuevo_stock", typeof(string)); 
            productos_proveedor.Columns.Add("cantidad_devuelta", typeof(string)); 
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                productos_proveedor.Rows[fila]["stock"] = stock_producto_terminado.get_ultimo_stock_producto_terminado(productos_proveedor.Rows[fila]["id"].ToString()) ;
                productos_proveedor.Rows[fila]["presentacion"] = "N/A";
                productos_proveedor.Rows[fila]["cantidad_desperdicio"] = "N/A";
                productos_proveedor.Rows[fila]["unidad"] = "N/A";
                productos_proveedor.Rows[fila]["nuevo_stock"] = "N/A";
                productos_proveedor.Rows[fila]["cantidad_devuelta"] = "N/A";
            }
            return productos_proveedor;
        }
        public DataTable get_insumos_proveedor()
        {
            consultar_insumos_proveedor();
            insumos_proveedor.Columns.Add("presentacion", typeof(string));
            insumos_proveedor.Columns.Add("cantidad_merma", typeof(string));
            insumos_proveedor.Columns.Add("unidad", typeof(string));
            for (int fila = 0; fila <= insumos_proveedor.Rows.Count - 1; fila++)
            {
                insumos_proveedor.Rows[fila]["presentacion"] = "N/A";
                insumos_proveedor.Rows[fila]["cantidad_merma"] = "N/A";
                insumos_proveedor.Rows[fila]["unidad"] = "N/A";
            }
            return insumos_proveedor;
        }
        #endregion
    }
}
