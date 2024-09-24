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
    public class cls_recepcion_de_fatay
    {
        public cls_recepcion_de_fatay(DataTable usuario_BD)
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
            movimientos_stock_producto = new cls_movimientos_stock_producto(usuario_BD);
            stock_producto_terminado = new cls_stock_producto_terminado(usuario_BD);
        }
        #region atributos
        cls_consultas_Mysql consultas;
        cls_movimientos_stock_producto movimientos_stock_producto;
        cls_stock_producto_terminado stock_producto_terminado;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable historial_produccion_proveedor_cliente;
        DataTable productos_proveedor;
        DataTable resumen_pedido;

        #endregion

        #region metodos consultas
        private void consultar_historial_produccion(string fabrica, string proveedor, string cliente)
        {
            historial_produccion_proveedor_cliente = consultas.consultar_historial_produccion_proveedor_cliente(base_de_datos, "produccion_diaria", fabrica, proveedor, cliente);
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, nombre_proveedor);
        }
        private void consultar_todo_historial_de_produccion(string nombre_proveedor)
        {
            historial_produccion_proveedor_cliente = consultas.consultar_tabla(base_de_datos, "despacho_fabrica_fatay");
        }
        #endregion

        #region metodos get/set
        public DataTable get_todo_historial_de_produccion(string nombre_proveedor)
        {
            consultar_todo_historial_de_produccion(nombre_proveedor);
            return historial_produccion_proveedor_cliente;
        }
        public DataTable get_historial_produccion_proveedor_cliente(string fabrica, string proveedor, string cliente)
        {
            consultar_historial_produccion(fabrica, proveedor, cliente);
            return historial_produccion_proveedor_cliente;
        }
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            return productos_proveedor;
        }
        public DataTable get_detalle_produccion(string id_historial, string nombre_proveedor)
        {

            consultar_productos_proveedor(nombre_proveedor);
            int fila_historial = buscar_fila_historial_por_id(id_historial, historial_produccion_proveedor_cliente);
            abrir_pedido(fila_historial, historial_produccion_proveedor_cliente);
            return resumen_pedido;
        }
        public string get_proveedor_de_producccion(string id_historial)
        {
            int fila_historial = buscar_fila_historial_por_id(id_historial, historial_produccion_proveedor_cliente);
            return historial_produccion_proveedor_cliente.Rows[fila_historial]["proveedor"].ToString();
        }
        #endregion

        #region detalle produccion
        private void crear_tabla_resumen()
        {
            resumen_pedido = new DataTable();
            resumen_pedido.Columns.Add("dato_pedido", typeof(string));
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("Tipo Producto", typeof(string));
            resumen_pedido.Columns.Add("Producto", typeof(string));
            resumen_pedido.Columns.Add("Unid.Medida", typeof(string));
            resumen_pedido.Columns.Add("Cant.Entregada", typeof(string));
            resumen_pedido.Columns.Add("Cant.Recibida", typeof(string));
            resumen_pedido.Columns.Add("stock", typeof(string));
            resumen_pedido.Columns.Add("nuevo_stock", typeof(string));
            resumen_pedido.Columns.Add("confirmacion_automatica_stock", typeof(string));



        }
        private void abrir_pedido(int fila_historial, DataTable historial)
        {
            string id, producto, cantidad_entregada, cantidad_recibida, dato_pedido;
            crear_tabla_resumen();

            int i = 1;
            for (int columna = historial.Columns["producto_1"].Ordinal; columna <= historial.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(historial.Rows[fila_historial]["producto_" + i.ToString()]))
                {
                    if (historial.Rows[fila_historial]["producto_" + i.ToString()].ToString() != "")
                    {
                        //dato_pedido
                        // dato_pedido = historial.Rows[fila_historial]["producto_" + i.ToString()].ToString();
                        //extraer id
                        id = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 1);
                        //extraer producto
                        producto = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 2);
                        //extraer cantidad_entregada
                        cantidad_entregada = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad_recibida
                        cantidad_recibida = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString().Replace(",", "."), 4);//;
                        dato_pedido = id + "-" + producto + "-" + cantidad_entregada;
                        /*if (historial.Rows[fila_historial]["estado"].ToString() != "Recibido")
                        {
                            cantidad_recibida = "N/A";
                        }*/


                        //cargar normal
                        cargar_producto(id, cantidad_entregada, cantidad_recibida, dato_pedido);

                    }
                }
                i++;
            }
        }
        private void cargar_producto(string id, string cantidad_entregada, string cantidad_recibida, string dato_pedido)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;
            int fila_producto = funciones.buscar_fila_por_id(id, productos_proveedor);
            resumen_pedido.Rows[fila]["dato_pedido"] = dato_pedido;
            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["Tipo Producto"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
            resumen_pedido.Rows[fila]["Producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
            resumen_pedido.Rows[fila]["Unid.Medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_produccion"].ToString();
            resumen_pedido.Rows[fila]["Cant.Entregada"] = cantidad_entregada;
            resumen_pedido.Rows[fila]["Cant.Recibida"] = cantidad_recibida;

            resumen_pedido.Rows[fila]["stock"] = stock_producto_terminado.get_ultimo_stock_producto_terminado(id);
            resumen_pedido.Rows[fila]["nuevo_stock"] = "N/A";
            resumen_pedido.Rows[fila]["confirmacion_automatica_stock"] = productos_proveedor.Rows[fila_producto]["confirmacion_automatica_stock"].ToString();



        }
        private int buscar_fila_historial_por_id(string id_historial, DataTable hisorial)
        {
            int retorno = 0;
            int fila = 0;

            while (fila <= hisorial.Rows.Count - 1)
            {
                if (id_historial == hisorial.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }

            return retorno;
        }
        private bool IsNotDBNull(object valor)
        {
            bool retorno = false;
            if (!DBNull.Value.Equals(valor))
            {
                retorno = true;
            }
            return retorno;
        }
        private string obtener_dato_pedido(string dato, int posicion_dato)
        {
            string retorno = "";
            int posicion = 0;
            int i = 0;
            while (i <= dato.Length - 1)
            {
                if (dato[i].ToString() == "-")
                {
                    posicion++;
                    if (posicion == posicion_dato)
                    {
                        break;
                    }
                    else
                    {
                        retorno = "";
                    }
                }
                else
                {
                    retorno = retorno + dato[i].ToString();
                }
                i++;
            }
            return retorno;
        }

        #endregion

        #region carga a base de datos
        public void confirmar_produccion(string id_historial, DataTable detalle_produccion, string proveedor, string rol_usuario)
        {
            int index = 1;
            string actualizar, id_producto;
            string nombre_proveedor = "proveedor_villaMaipu";
            int fila_producto;
            double cantidad_recibida, stock_produccion, nuevo_stock_produccion, stock_fabrica_fatay, nuevo_stock_fabrica_fatay, stock_expedicion, nuevo_stock_expedicion, stock;
            for (int fila = 0; fila <= detalle_produccion.Rows.Count - 1; fila++)
            {
                if (detalle_produccion.Rows[fila]["confirmacion_automatica_stock"].ToString() == "0")
                {
                    if (detalle_produccion.Rows[fila]["Cant.Recibida"].ToString() != "N/A")
                    {
                        actualizar = "`producto_" + index.ToString() + "` = '" + detalle_produccion.Rows[fila]["dato_pedido"].ToString() + "-" + detalle_produccion.Rows[fila]["Cant.Recibida"].ToString() + "'";
                        id_producto = detalle_produccion.Rows[fila]["id"].ToString();


                        fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
                        cantidad_recibida = double.Parse(detalle_produccion.Rows[fila]["Cant.Recibida"].ToString());
                        stock_producto_terminado.cargar_historial_stock(rol_usuario, id_producto, "produccion", cantidad_recibida.ToString(), "N/A");
                        index++;
                    }
                    else
                    {
                        actualizar = "`producto_" + index.ToString() + "` = '" + detalle_produccion.Rows[fila]["dato_pedido"].ToString() + "-0'";
                        index++;
                    }
                }
                else
                {
                    if (detalle_produccion.Rows[fila]["Cant.Recibida"].ToString() != "N/A")
                    {
                        actualizar = "`producto_" + index.ToString() + "` = '" + detalle_produccion.Rows[fila]["dato_pedido"].ToString() + "-" + detalle_produccion.Rows[fila]["Cant.Recibida"].ToString() + "'";
                        id_producto = detalle_produccion.Rows[fila]["id"].ToString();


                        fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
                        cantidad_recibida = double.Parse(detalle_produccion.Rows[fila]["Cant.Recibida"].ToString());
                        stock_expedicion = double.Parse(productos_proveedor.Rows[fila_producto]["stock_expedicion"].ToString());

                        //stock_producto_terminado.cargar_historial_stock(rol_usuario, id_producto, "produccion", cantidad_recibida.ToString(), "N/A");

                        nuevo_stock_expedicion = stock_expedicion + cantidad_recibida;

                        index++;
                    }
                    else
                    {
                        actualizar = "`producto_" + index.ToString() + "` = '" + detalle_produccion.Rows[fila]["dato_pedido"].ToString() + "-0'";
                        index++;
                    }
                }
                consultas.actualizar_tabla(base_de_datos, "despacho_fabrica_fatay", actualizar, id_historial);
            }
            consultas.actualizar_tabla(base_de_datos, "despacho_fabrica_fatay", "`estado` = 'Recibido' ", id_historial);

        }
        #endregion
    }
}
