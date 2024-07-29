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
    public class cls_movimientos_stock_insumos
    {
        public cls_movimientos_stock_insumos(DataTable usuario_BD)
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

        DataTable productos_proveedor;
        DataTable historial_producto;
        #endregion
        /*
        Movimientos suma stock: produccion, devolucion.
        movimientos resta stock: despacho, desperdicio. 
        */
        #region carga base de datos
        public void registrar_movimiento_en_historial(int fila_insumo,int columna_stock,string rol_usuario,string fecha, DataTable insumos, DataTable insumos_copia, string tipo_movimiento)
        {
            string columnas="";
            string valores="";
            //rol_usuario
            columnas= funciones.armar_query_columna(columnas, "rol_usuario",false);
            valores = funciones.armar_query_valores(valores,rol_usuario,false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha, false);
            //id_producto
            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo]["id"].ToString(), false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento , false);
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, insumos_copia.Rows[fila_insumo][columna_stock].ToString(), false);
            //movimiento
            double stock_inicial;
            if (double.TryParse(funciones.obtener_dato(insumos_copia.Rows[fila_insumo][columna_stock].ToString(), 4),out stock_inicial))
            {
                stock_inicial = double.Parse(funciones.obtener_dato(insumos_copia.Rows[fila_insumo][columna_stock].ToString(), 4));
            }
            else
            {
                stock_inicial = 0;
                string dato = funciones.obtener_dato(insumos.Rows[fila_insumo][columna_stock].ToString(), 4);
            }
            double stock_final = double.Parse(funciones.obtener_dato(insumos.Rows[fila_insumo][columna_stock].ToString(), 4));
            double movimiento = stock_final - stock_inicial;
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento.ToString(), false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", false);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo][columna_stock].ToString(), false);
            //columna_stock
            columnas = funciones.armar_query_columna(columnas, "columna_stock", true);
            valores = funciones.armar_query_valores(valores, insumos.Columns[columna_stock].ColumnName, true);
            if (movimiento!=0)
            {
                consultas.insertar_en_tabla(base_de_datos, "movimientos_stock_insumos", columnas, valores);
            }
        }
        public void registrar_movimiento_en_historial_sub_producto(int fila_insumo, int columna_stock, string rol_usuario, string fecha, DataTable insumos, DataTable insumos_copia, string tipo_movimiento)
        {
            string columnas = "";
            string valores = "";
            //rol_usuario
            columnas = funciones.armar_query_columna(columnas, "rol_usuario", false);
            valores = funciones.armar_query_valores(valores, rol_usuario, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha, false);
            //id_producto
            columnas = funciones.armar_query_columna(columnas, "id_producto", false);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo]["id"].ToString(), false);
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, insumos_copia.Rows[fila_insumo][columna_stock].ToString(), false);
            //movimiento
            double stock_inicial;
            if (double.TryParse(insumos_copia.Rows[fila_insumo][columna_stock].ToString(), out stock_inicial))
            {
                stock_inicial = double.Parse(insumos_copia.Rows[fila_insumo][columna_stock].ToString());
            }
            else
            {
                stock_inicial = 0;
                string dato = insumos_copia.Rows[fila_insumo][columna_stock].ToString();
            }
            double stock_final = double.Parse(insumos.Rows[fila_insumo][columna_stock].ToString());
            double movimiento = stock_final - stock_inicial;
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento.ToString(), false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", true);
            valores = funciones.armar_query_valores(valores, insumos.Rows[fila_insumo][columna_stock].ToString(), true);
           
            if (movimiento != 0)
            {
                consultas.insertar_en_tabla(base_de_datos, "movimientos_stock_producto_terminado", columnas, valores);
            }
        }
        #endregion
    }
}
