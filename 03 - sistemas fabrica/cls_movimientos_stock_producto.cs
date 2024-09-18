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
    public class cls_movimientos_stock_producto
    {
        public cls_movimientos_stock_producto(DataTable usuario_BD)
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
            stock_Producto_Terminado = new cls_stock_producto_terminado(usuario_BD);
        }

        #region atributos
        cls_stock_producto_terminado stock_Producto_Terminado;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        DataTable historial_producto;
        #endregion
        /*
        Movimientos suma stock: produccion, devolucion.
        movimientos resta stock: despacho, desperdicio. 
        */
        #region carga base de datos
        public void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota)
        {
            stock_Producto_Terminado.cargar_historial_stock(rol_usuario,id_producto,tipo_movimiento,movimiento,nota);
        }
        public void cargar_actualizacion_stock(string rol_usuario, string id_producto, string stock_final_dato, string nota)
        {
            consultar_productos_proveedor();
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
            string columnas = "";
            string valores = "";
            double stock_inicial, movimiento, stock_final;
            //rol_usuario
            columnas = funciones.armar_query_columna(columnas, "rol_usuario", false);
            valores = funciones.armar_query_valores(valores, rol_usuario, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //id_producto
            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
            valores = funciones.armar_query_valores(valores, id_producto, false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, productos_proveedor.Rows[fila_producto]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, "Actualizacion Stock", false);
            stock_inicial = double.Parse(productos_proveedor.Rows[fila_producto]["stock"].ToString());
            stock_final = double.Parse(stock_final_dato);
            movimiento = stock_final - stock_inicial;
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, stock_inicial.ToString(), false);
            //movimiento
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento.ToString(), false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", false);
            valores = funciones.armar_query_valores(valores, stock_final.ToString(), false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "movimientos_stock_producto_terminado", columnas, valores);
        }

        public void cargar_suma_stock(string rol_usuario,string id_producto,string tipo_movimiento,string movimiento_dato,string nota)
        {
            consultar_productos_proveedor();
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
            string columnas ="";
            string valores="";
            double stock_inicial, movimiento , stock_final;
            //rol_usuario
            columnas = funciones.armar_query_columna(columnas,"rol_usuario",false);
            valores = funciones.armar_query_valores(valores, rol_usuario,false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //id_producto
            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
            valores = funciones.armar_query_valores(valores, id_producto, false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, productos_proveedor.Rows[fila_producto]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            stock_inicial = double.Parse(productos_proveedor.Rows[fila_producto]["stock"].ToString());
            movimiento = double.Parse(movimiento_dato);
            stock_final = stock_inicial + movimiento;
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, stock_inicial.ToString(), false);
            //movimiento
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento.ToString(), false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", false);
            valores = funciones.armar_query_valores(valores, stock_final.ToString(), false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "movimientos_stock_producto_terminado",columnas, valores);
        }

        public void cargar_resta_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento_dato ,string nota)
        {
            consultar_productos_proveedor();
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
            string columnas = "";
            string valores = "";
            double stock_inicial, movimiento, stock_final;
            //rol_usuario
            columnas = funciones.armar_query_columna(columnas, "rol_usuario", false);
            valores = funciones.armar_query_valores(valores, rol_usuario, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //id_producto
            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
            valores = funciones.armar_query_valores(valores, id_producto, false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, productos_proveedor.Rows[fila_producto]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            stock_inicial = double.Parse(productos_proveedor.Rows[fila_producto]["stock"].ToString());
            movimiento = Math.Abs(double.Parse(movimiento_dato));
            stock_final = stock_inicial - movimiento;
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, stock_inicial.ToString(), false);
            //movimiento
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento.ToString(), false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", false);
            valores = funciones.armar_query_valores(valores, stock_final.ToString(), false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "movimientos_stock_producto_terminado", columnas, valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_productos_proveedor()
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos, "proveedor_villaMaipu");
            productos_proveedor.Columns.Add("ultimo_stock", typeof(string));
            string id;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count-1; fila++)
            {
                id = productos_proveedor.Rows[fila]["id"].ToString();
                productos_proveedor.Rows[fila]["ultimo_stock"] = stock_Producto_Terminado.get_ultimo_stock_producto_terminado(id);
            }
        }
        private void consultar_historial_producto(string id_producto, string mes, string año)
        {
            historial_producto = consultas.consultar_historial_producto_segun_mes_y_año(base_de_datos, "stock_producto_terminado", id_producto, mes,año);
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor()
        {
            consultar_productos_proveedor();
            return productos_proveedor;
        }
        public DataTable get_historial_producto(string id_producto, string mes, string año)
        {
            consultar_historial_producto(id_producto,mes,año);
            historial_producto.DefaultView.Sort = "fecha DESC, id DESC";
            historial_producto = historial_producto.DefaultView.ToTable();
            return historial_producto;
        }
        #endregion
    }
}
