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
    [Serializable]
    public class cls_stock
    {
        public cls_stock(DataTable usuario_BD)
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
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        DataTable pedidos_no_cargados;
        DataTable promedio_del_mes;
        #endregion

        #region metodos privados
        private void cargar_promedio_pedidos(string nombre_proveedor)
        {
            int mes = obtener_mes(DateTime.Now.Month);
            int año = obtener_año(DateTime.Now.Month, DateTime.Now.Year);
            promedio_del_mes = estadisticas.get_promedio_del_mes(nombre_proveedor, mes.ToString(), año.ToString());
            string id;
            double promedio_semanal, promedio_mensual;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                id = productos_proveedor.Rows[fila]["id"].ToString();
                if (double.TryParse(promedio_del_mes.Rows[0]["producto_" + id].ToString(), out promedio_mensual))
                {
                    promedio_semanal = Math.Ceiling(promedio_mensual / 4);
                    productos_proveedor.Rows[fila]["promedio_pedido"] = promedio_semanal.ToString();

                }
                else
                {
                    productos_proveedor.Rows[fila]["promedio_pedido"] = "N/A";
                }
            }
        }
        private void cargar_cantidad_pedida()
        {
            string id_producto;
            int fila_producto;
            double cantidad, cantidad_pedida, pedido;
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                for (int columna = pedidos_no_cargados.Columns["producto_1"].Ordinal; columna <= pedidos_no_cargados.Columns.Count - 1; columna++)
                {
                    if (funciones.IsNotDBNull(pedidos_no_cargados.Rows[fila][columna]))
                    {
                        if (pedidos_no_cargados.Rows[fila][columna].ToString() != string.Empty)
                        {

                            cantidad_pedida = double.Parse(funciones.obtener_dato(pedidos_no_cargados.Rows[fila][columna].ToString(), 4));
                            id_producto = funciones.obtener_dato(pedidos_no_cargados.Rows[fila][columna].ToString(), 2);
                            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
                            cantidad = double.Parse(productos_proveedor.Rows[fila_producto]["pedido"].ToString());
                             pedido = cantidad + cantidad_pedida;
                            productos_proveedor.Rows[fila_producto]["pedido"] = pedido;

                        }
                    }
                }
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_pedidos_no_cargados(string nombre_proveedor)
        {
            pedidos_no_cargados = consultas.consultar_pedidos_no_cargados(base_de_datos, "pedidos", nombre_proveedor);
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos, nombre_proveedor);
            productos_proveedor.Columns.Add("pedido", typeof(string));
            productos_proveedor.Columns.Add("diferencia", typeof(string));
            productos_proveedor.Columns.Add("promedio_pedido", typeof(string));
            double stock_expedicion, stock;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                stock_expedicion = double.Parse(productos_proveedor.Rows[fila]["stock_expedicion"].ToString());
                stock = double.Parse(productos_proveedor.Rows[fila]["stock"].ToString());

                productos_proveedor.Rows[fila]["pedido"] = "0";
                productos_proveedor.Rows[fila]["diferencia"] = stock_expedicion - stock;
                productos_proveedor.Rows[fila]["promedio_pedido"] = "0";
            }
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_pedidos_no_cargados(nombre_proveedor);
            consultar_productos_proveedor(nombre_proveedor);
            cargar_promedio_pedidos(nombre_proveedor);
            cargar_cantidad_pedida();
            return productos_proveedor;
        }
        #endregion

        #region funciones
        private int obtener_mes(int mes)
        {
            int retorno = mes;
            if (mes == 1)
            {
                retorno = 12;
            }
            else
            {
                retorno = mes - 1;
            }
            return retorno;
        }
        private int obtener_año(int mes, int año)
        {
            int retorno = año;
            if (mes == 1)
            {
                retorno = año - 1;
            }
            return retorno;
        }
        #endregion
    }
}
