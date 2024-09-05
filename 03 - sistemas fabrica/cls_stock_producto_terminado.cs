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
    public class cls_stock_producto_terminado
    {
        public cls_stock_producto_terminado(DataTable usuario_BD)
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
        DataTable historial_stock;
        #endregion

        #region carga a base de datos
        private void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string stock_inicial, string movimiento, string stock_final, string nota)
        {
            //obtener fila de producto
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);

            string columnas = string.Empty;
            string valores = string.Empty;
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
            //unidad_medida
            columnas = funciones.armar_query_columna(columnas, "unidad_medida", false);
            valores = funciones.armar_query_valores(valores, productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString(), false);
            //stock_inicial
            columnas = funciones.armar_query_columna(columnas, "stock_inicial", false);
            valores = funciones.armar_query_valores(valores, stock_inicial, false);
            //movimiento
            columnas = funciones.armar_query_columna(columnas, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento, false);
            //stock_final
            columnas = funciones.armar_query_columna(columnas, "stock_final", false);
            valores = funciones.armar_query_valores(valores, stock_final, false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "stock_producto_terminado", columnas, valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_historial_stock(string id_producto)
        {
            historial_stock = consultas.consultar_stock_producto_terminado_segun_producto(id_producto);
        }
        private void consultar_productos_proveedor()
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villamaipu");
        }
        #endregion
        ///COMPRA PRODUCCION = SUMA
        ///DESPACHO = RESTA
        ///CONTEO STOCK = DIFERENCIA
        ///STOCK INICIAL = STOCK FINAL
        #region metodos privados
        private bool verificar_existencia_historial()
        {
            if (historial_stock.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void crear_stock_inicial(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota)
        {
            //obtener ultimo stock
            double stock_inicial = 0;
            double stock_final = 0;
            if (tipo_movimiento == "compra") //si es compra
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            else if (tipo_movimiento == "despacho") //si es despacho
            {//restar
                stock_final = stock_inicial - double.Parse(movimiento);
            }
            else if (tipo_movimiento == "conteo stock") //si es conteo
            {//diferencia
                stock_final = double.Parse(movimiento);
                movimiento = (stock_final - stock_inicial).ToString();
            }
            cargar_historial_stock(rol_usuario, id_producto, tipo_movimiento, stock_inicial.ToString(), movimiento, stock_final.ToString(), nota);
        }
        private void crear_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota)
        {
            //obtener ultimo stock
            double stock_inicial = obtener_ultimo_stock();
            double stock_final = 0;
            if (tipo_movimiento=="compra") //si es compra
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            else if (tipo_movimiento=="despacho") //si es despacho
            {//restar
                stock_final = stock_inicial - double.Parse(movimiento);
            }
            else if (tipo_movimiento == "conteo stock") //si es conteo
            {//diferencia
                stock_final = double.Parse(movimiento);
                movimiento = (stock_final - stock_inicial).ToString();
            }
            cargar_historial_stock(rol_usuario, id_producto, tipo_movimiento, stock_inicial.ToString(), movimiento, stock_final.ToString(), nota);
        }
        private double obtener_ultimo_stock()
        {
            //obtener ultimo stock
            historial_stock.DefaultView.Sort = "fecha DESC";
            historial_stock = historial_stock.DefaultView.ToTable(true);
            return double.Parse(historial_stock.Rows[0]["stock_final"].ToString());
        }
        #endregion

        #region metodos get/set
        public void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota)
        {
            consultar_productos_proveedor();
            consultar_historial_stock(id_producto);

            //VERIFICAR SI EXISTE HISTORIAL
            if (!verificar_existencia_historial())
            {
                //si no existe crear partiendo de cero
                crear_stock_inicial(rol_usuario, id_producto, tipo_movimiento, movimiento, nota);
            }
            else //si existe
            {
                crear_stock(rol_usuario, id_producto, tipo_movimiento, movimiento, nota);
            }
        }

        #endregion
    }
}
