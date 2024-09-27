using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_recepcion_de_fabrica_fatay
    {
        public cls_recepcion_de_fabrica_fatay(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable historial_despacho;
        DataTable productos_proveedor;
        DataTable resumen_pedido;

        #endregion

        #region metodos consultas
        private void consultar_historial_produccion(string fabrica, string proveedor, string cliente)
        {
            historial_despacho = consultas.consultar_historial_produccion_proveedor_cliente(base_de_datos, "produccion_diaria", fabrica, proveedor, cliente);
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, nombre_proveedor);
        }
        private void consultar_todo_historial_de_despacho(string rol)
        {
            historial_despacho = consultas.consultar_historial_despacho_proveedor_receptor(base_de_datos, "despacho_fabrica_fatay",rol);
        }
        private void consultar_todo_historial_de_produccion_segun_fabrica(string nombre_proveedor)
        {
            historial_despacho = consultas.consultar_todo_el_historial_produccion(base_de_datos, "produccion_diaria", nombre_proveedor);
            // historial_produccion_proveedor_cliente = consultas.consultar_tabla(base_de_datos, "produccion_diaria");
        }
        #endregion

        #region metodos get/set
        public void cancelar_produccion(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "produccion_diaria", actualizar, id);
        }
        public DataTable get_todo_historial_de_despacho(string rol)
        {
            rol = "Fabrica Villa Maipu";
            consultar_todo_historial_de_despacho(rol);
            return historial_despacho;
        }
        public DataTable get_todo_historial_de_produccion_segun_fabrica(string nombre_proveedor)
        {
            consultar_todo_historial_de_produccion_segun_fabrica(nombre_proveedor);
            return historial_despacho;
        }

        public DataTable get_historial_produccion_proveedor_cliente(string fabrica, string proveedor, string cliente)
        {
            consultar_historial_produccion(fabrica, proveedor, cliente);
            return historial_despacho;
        }
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            return productos_proveedor;
        }
        public DataTable get_detalle_produccion(string id_historial, string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            int fila_historial = buscar_fila_historial_por_id(id_historial, historial_despacho);
            abrir_pedido(fila_historial, historial_despacho);
            return resumen_pedido;
        }
        #endregion

        #region PDF
        public void crear_pdf_historial_produccion(string ruta, byte[] logo, string id_historial, string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            int fila_historial = buscar_fila_historial_por_id(id_historial, historial_despacho);
            abrir_pedido(fila_historial, historial_despacho);
            PDF.GenerarPDF_historial_produccion(ruta, logo, fila_historial, historial_despacho, resumen_pedido);
        }

        private void crear_tabla_resumen()
        {
            resumen_pedido = new DataTable();
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("Tipo Producto", typeof(string));
            resumen_pedido.Columns.Add("Producto", typeof(string));
            resumen_pedido.Columns.Add("Unid.Medida", typeof(string));
            resumen_pedido.Columns.Add("Cant.Entregada", typeof(string));
            resumen_pedido.Columns.Add("Cant.Recibida", typeof(string));



        }
        private void abrir_pedido(int fila_historial, DataTable historial)
        {
            string id, producto, cantidad_entregada, cantidad_recibida;
            crear_tabla_resumen();

            int i = 1;
            for (int columna = historial.Columns["producto_1"].Ordinal; columna <= historial.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(historial.Rows[fila_historial]["producto_" + i.ToString()]))
                {
                    if (historial.Rows[fila_historial]["producto_" + i.ToString()].ToString() != "")
                    {
                        //extraer id
                        id = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 1);
                        //extraer producto
                        producto = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 2);
                        //extraer cantidad_entregada
                        cantidad_entregada = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad_recibida
                        cantidad_recibida = obtener_dato_pedido(historial.Rows[fila_historial]["producto_" + i.ToString()].ToString().Replace(",", "."), 4);//;
                        /*if (historial.Rows[fila_historial]["estado"].ToString() != "Recibido")
                         {
                             cantidad_recibida = "N/A";
                         }*/


                        //cargar normal
                        cargar_producto(id, cantidad_entregada, cantidad_recibida);

                    }
                }
                i++;
            }
        }
        private void cargar_producto(string id, string cantidad_entregada, string cantidad_recibida)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;

            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["Tipo Producto"] = productos_proveedor.Rows[int.Parse(id) - 1]["tipo_producto"].ToString();
            resumen_pedido.Rows[fila]["Producto"] = productos_proveedor.Rows[int.Parse(id) - 1]["producto"].ToString();
            resumen_pedido.Rows[fila]["Unid.Medida"] = productos_proveedor.Rows[int.Parse(id) - 1]["unidad_de_medida_produccion"].ToString();
            resumen_pedido.Rows[fila]["Cant.Entregada"] = cantidad_entregada;
            resumen_pedido.Rows[fila]["Cant.Recibida"] = cantidad_recibida;


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
    }
}
