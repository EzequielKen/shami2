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
    public class cls_historial_orden_de_pedidos
    {
        public cls_historial_orden_de_pedidos(DataTable usuario_BD)
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

        #region carga a base de datos
        public void cancelar_pedido(string columna,string dato,string id_orden)
        {
            string actualizar = "`"+columna+"` = '"+dato+"'";
            consultas.actualizar_tabla(base_de_datos, "orden_de_pedido", actualizar,id_orden);
        }
        #endregion

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable orden_de_pedido;
        DataTable resumen_de_pedido;
        #endregion

        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen_de_pedido = new DataTable();
            resumen_de_pedido.Columns.Add("id", typeof(string));
            resumen_de_pedido.Columns.Add("producto", typeof(string));
            resumen_de_pedido.Columns.Add("cantidad_pedida", typeof(string)); 
            resumen_de_pedido.Columns.Add("estado", typeof(string)); 
            resumen_de_pedido.Columns.Add("num_orden_compra", typeof(string)); 
            resumen_de_pedido.Columns.Add("fecha_orden_compra", typeof(string));
            resumen_de_pedido.Columns.Add("id_orden", typeof(string));
            resumen_de_pedido.Columns.Add("dato", typeof(string));
            resumen_de_pedido.Columns.Add("columna", typeof(string));
        }

        private DataTable abrir_pedido(string id_pedido)
        {
            consultar_orden_de_pedido();
            int fila_pedido = funciones.buscar_fila_por_id(id_pedido, orden_de_pedido);
            string id, producto, cantidad_pedidas, estado, num_orden_compra, fecha_orden_compra;
            int index = 0;
            crear_tabla_resumen();
            for (int columna = orden_de_pedido.Columns["producto_1"].Ordinal; columna <= orden_de_pedido.Columns.Count - 1; columna++)
            {
                if (orden_de_pedido.Rows[fila_pedido][columna].ToString() != "N/A")
                {
                    id=funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(),1);
                    producto = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(),2);
                    cantidad_pedidas = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(),3)+ funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 4);
                    estado = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 5);
                    num_orden_compra = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 6);
                    fecha_orden_compra = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 7);

                    resumen_de_pedido.Rows.Add();
                    resumen_de_pedido.Rows[index]["id"] = id;
                    resumen_de_pedido.Rows[index]["producto"] = producto;
                    resumen_de_pedido.Rows[index]["cantidad_pedida"] = cantidad_pedidas;
                    resumen_de_pedido.Rows[index]["estado"] = estado;
                    resumen_de_pedido.Rows[index]["num_orden_compra"] = num_orden_compra;
                    resumen_de_pedido.Rows[index]["fecha_orden_compra"] = fecha_orden_compra;
                    resumen_de_pedido.Rows[index]["id_orden"] = orden_de_pedido.Rows[fila_pedido]["id"].ToString();
                    resumen_de_pedido.Rows[index]["dato"] = orden_de_pedido.Rows[fila_pedido][columna].ToString();
                    resumen_de_pedido.Rows[index]["columna"] = orden_de_pedido.Columns[columna].ColumnName;

                    index++;
                }
            }
            return resumen_de_pedido;
        }
        #endregion

        #region metodos consultas
        private void consultar_orden_de_pedido()
        {
            orden_de_pedido = consultas.consultar_tabla(base_de_datos, "orden_de_pedido");
        }
        #endregion

        #region metodos get/set
        public DataTable get_orden_de_pedido()
        {
            consultar_orden_de_pedido();
            orden_de_pedido.DefaultView.Sort = "fecha DESC";
            orden_de_pedido= orden_de_pedido.DefaultView.ToTable();
            return orden_de_pedido;
        }
        public DataTable get_resumen_de_pedido(string id_pedido)
        {
            return abrir_pedido(id_pedido);
        }
        #endregion
    }
}
