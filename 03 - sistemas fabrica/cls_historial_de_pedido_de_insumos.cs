using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_historial_de_pedido_de_insumos
    {
        public cls_historial_de_pedido_de_insumos(DataTable usuario_BD)
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


        #region desactivar pedido
        public void desactivar_pedido(string id)
        {
            string actualizar = "`activa` = '0' ";
            consultas.actualizar_tabla(base_de_datos, "pedido_de_insumos",actualizar, id);
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

        #region crear pdf
        public void crear_pdf(string ruta_archivo, byte[] logo, DataTable resumen)
        {
            PDF.GenerarPDF_resumen_de_pedidos_de_insumos_a_expedicion(ruta_archivo, logo, resumen);
        }
        #endregion
        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen_de_pedido = new DataTable();
            resumen_de_pedido.Columns.Add("id", typeof(string));
            resumen_de_pedido.Columns.Add("producto", typeof(string));
            resumen_de_pedido.Columns.Add("cantidad_pedida", typeof(string));
            resumen_de_pedido.Columns.Add("cantidad_entregada", typeof(string));
        }

        private DataTable abrir_pedido(string id_pedido)
        {
            consultar_orden_de_pedido();
            int fila_pedido = funciones.buscar_fila_por_id(id_pedido, orden_de_pedido);
            string id, producto, cantidad_pedidas, cantidad_entregada, tipo_paquete, unidad_paquete, tipo_unidad;
            int index = 0;
            crear_tabla_resumen();
            for (int columna = orden_de_pedido.Columns["producto_1"].Ordinal; columna <= orden_de_pedido.Columns.Count - 1; columna++)
            {
                if (orden_de_pedido.Rows[fila_pedido][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 1);
                    producto = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 2);
                    cantidad_pedidas = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 3) + funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 4);
                    cantidad_entregada = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 5);
                    tipo_paquete = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 6);
                    unidad_paquete = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 7);
                    tipo_unidad = funciones.obtener_dato(orden_de_pedido.Rows[fila_pedido][columna].ToString(), 8);

                    resumen_de_pedido.Rows.Add();
                    resumen_de_pedido.Rows[index]["id"] = id;
                    resumen_de_pedido.Rows[index]["producto"] = producto;
                    resumen_de_pedido.Rows[index]["cantidad_pedida"] = cantidad_pedidas;
                    if (cantidad_entregada=="N/A")
                    {
                        resumen_de_pedido.Rows[index]["cantidad_entregada"] = cantidad_entregada;
                    }
                    else
                    {
                        resumen_de_pedido.Rows[index]["cantidad_entregada"] = cantidad_entregada + " " + tipo_paquete + " " + unidad_paquete + " " + tipo_unidad;
                    }

                    index++;
                }
            }
            return resumen_de_pedido;
        }
        #endregion

        #region metodos consultas
        private void consultar_orden_de_pedido()
        {
            orden_de_pedido = consultas.consultar_tabla(base_de_datos, "pedido_de_insumos");
        }
        #endregion

        #region metodos get/set
        public DataTable get_orden_de_pedido()
        {
            consultar_orden_de_pedido();
            orden_de_pedido.DefaultView.Sort = "fecha DESC";
            orden_de_pedido = orden_de_pedido.DefaultView.ToTable();
            return orden_de_pedido;
        }
        public DataTable get_resumen_de_pedido(string id_pedido)
        {
            return abrir_pedido(id_pedido);
        }
        #endregion
    }
}
