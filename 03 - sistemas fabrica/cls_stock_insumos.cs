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
    public class cls_stock_insumos
    {
        public cls_stock_insumos(DataTable usuario_BD)
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

        DataTable insumos_fabrica;
        DataTable historial_stock;
        #endregion

        #region carga a base de datos
        private void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string stock_inicial, string movimiento, string stock_final, string nota,string presentacion)
        {
            //obtener fila de producto
            int fila_producto = funciones.buscar_fila_por_id(id_producto, insumos_fabrica);

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
            valores = funciones.armar_query_valores(valores, insumos_fabrica.Rows[fila_producto]["producto"].ToString(), false);
            //tipo_movimiento
            columnas = funciones.armar_query_columna(columnas, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            //unidad_medida
            columnas = funciones.armar_query_columna(columnas, "presentacion", false);
            valores = funciones.armar_query_valores(valores, presentacion, false);
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

            consultas.insertar_en_tabla(base_de_datos, "stock_insumos", columnas, valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_historial_stock(string id_producto,string presentacion)
        {
            historial_stock = consultas.consultar_stock_insumos_segun_producto(id_producto, presentacion);
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
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

        private void crear_stock_inicial(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota,string presentacion)
        {
            //obtener ultimo stock
            double stock_inicial = 0;
            double stock_final = 0;
            if (tipo_movimiento == "compra") //si es compra
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            if (tipo_movimiento == "devolucion") //si es devolucion
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            if (tipo_movimiento == "produccion") //si es compra
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
            cargar_historial_stock(rol_usuario, id_producto, tipo_movimiento, stock_inicial.ToString(), movimiento, stock_final.ToString(), nota,presentacion);
        }
        private void crear_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota,string presentacion)
        {
            if (movimiento == "")
            {
                movimiento = "0";
            }
            //obtener ultimo stock
            double stock_inicial = obtener_ultimo_stock();
            double stock_final = 0;
            if (tipo_movimiento == "compra") //si es compra
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            if (tipo_movimiento == "devolucion") //si es devolucion
            {//sumar
                stock_final = stock_inicial + double.Parse(movimiento);
            }
            if (tipo_movimiento == "produccion") //si es compra
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
            cargar_historial_stock(rol_usuario, id_producto, tipo_movimiento, stock_inicial.ToString(), movimiento, stock_final.ToString(), nota, presentacion);
        }
        private double obtener_ultimo_stock()
        {
            double stock;
            //obtener ultimo stock
            historial_stock.DefaultView.Sort = "fecha DESC,id DESC";
            historial_stock = historial_stock.DefaultView.ToTable(true);
            if (historial_stock.Rows.Count == 0)
            {
                stock = 0;
            }
            else
            {
                stock = double.Parse(historial_stock.Rows[0]["stock_final"].ToString());
            }
            return stock;
        }
        #endregion

        #region metodos get/set
        public void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota, string presentacion)
        {
            consultar_insumos_fabrica();
            consultar_historial_stock(id_producto,presentacion);

            //VERIFICAR SI EXISTE HISTORIAL
            if (!verificar_existencia_historial())
            {
                //si no existe crear partiendo de cero
                crear_stock_inicial(rol_usuario, id_producto, tipo_movimiento, movimiento, nota, presentacion);
            }
            else //si existe
            {
                crear_stock(rol_usuario, id_producto, tipo_movimiento, movimiento, nota, presentacion);
            }
        }
        public string get_ultimo_stock_insumo_fabrica(string id_producto, string presentacion)
        {
            consultar_historial_stock(id_producto,presentacion);
            return obtener_ultimo_stock().ToString();
        }
        #endregion
    }
}