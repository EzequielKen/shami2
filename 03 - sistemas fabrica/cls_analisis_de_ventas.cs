using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_analisis_de_ventas
    {
        public cls_analisis_de_ventas(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        DataTable pedidos_del_mes;
        DataTable acuerdo_de_precios;
        DataTable resumen;
        
        #endregion

        #region tabla resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("unidad_medida", typeof(string));
            resumen.Columns.Add("cantidad_vendida", typeof(string));
            resumen.Columns.Add("sub_total", typeof(string));
        }
        #endregion

        #region metodos privados
        private string calcular_total()
        {
            double total = 0;
            for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
            {
                total = total + double.Parse(resumen.Rows[fila]["sub_total"].ToString());
            }
            return total.ToString();
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            string id_producto;
            int ultima_fila, fila_producto;
            double cantidad_vendida, cantidad_cargada, precio, sub_total, total;
            for (int fila_pedido = 0; fila_pedido <= pedidos_del_mes.Rows.Count - 1; fila_pedido++)
            {
                if (pedidos_del_mes.Rows[fila_pedido]["estado"].ToString() == "Entregado")
                {
                    for (int columna_pedido = pedidos_del_mes.Columns["producto_1"].Ordinal; columna_pedido <= pedidos_del_mes.Columns.Count - 1; columna_pedido++)
                    {
                        if (funciones.IsNotDBNull(pedidos_del_mes.Rows[fila_pedido][columna_pedido]))
                        {
                            if (pedidos_del_mes.Rows[fila_pedido][columna_pedido].ToString() != "")
                            {
                                id_producto = funciones.obtener_dato(pedidos_del_mes.Rows[fila_pedido][columna_pedido].ToString(), 2);
                               
                                if (verificar_si_es_dulce(id_producto))
                                {
                                    cantidad_vendida = double.Parse(funciones.obtener_dato(pedidos_del_mes.Rows[fila_pedido][columna_pedido].ToString(), 5));
                                    precio = obtener_precio(id_producto, pedidos_del_mes.Rows[fila_pedido]["proveedor"].ToString(), pedidos_del_mes.Rows[fila_pedido]["acuerdo_de_precios"].ToString(), pedidos_del_mes.Rows[fila_pedido]["tipo_de_acuerdo"].ToString());
                                    sub_total = precio * cantidad_vendida;
                                    if (verificar_si_cargo(id_producto)) //verificar si cargo en resumen
                                    {
                                        //si cargo en resumen sumar nuevo precio
                                        fila_producto = funciones.buscar_fila_por_id(id_producto, resumen);
                                        cantidad_cargada = double.Parse(resumen.Rows[fila_producto]["cantidad_vendida"].ToString());
                                        cantidad_cargada = cantidad_cargada + cantidad_vendida;
                                        resumen.Rows[fila_producto]["cantidad_vendida"] = cantidad_cargada.ToString();

                                        total = double.Parse(resumen.Rows[fila_producto]["sub_total"].ToString());
                                        total = total + sub_total;
                                        resumen.Rows[fila_producto]["sub_total"] = total.ToString();
                                    }
                                    else
                                    {
                                        fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
                                        resumen.Rows.Add();
                                        ultima_fila = resumen.Rows.Count - 1;
                                        resumen.Rows[ultima_fila]["id"] = id_producto;
                                        resumen.Rows[ultima_fila]["producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                        resumen.Rows[ultima_fila]["unidad_medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"].ToString();
                                        resumen.Rows[ultima_fila]["cantidad_vendida"] = cantidad_vendida.ToString();
                                        resumen.Rows[ultima_fila]["sub_total"] = sub_total.ToString();
                                        //si no cargo en resumen cargar en resumen con precio
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private double obtener_precio(string id_producto, string proveedor, string acuerdo_de_precio, string tipo_de_acuerdo)
        {
            int fila_acuerdo = buscar_fila_acuerdo(proveedor, acuerdo_de_precio, tipo_de_acuerdo);

            return double.Parse(acuerdo_de_precios.Rows[fila_acuerdo]["producto_" + id_producto].ToString());
        }
        private int buscar_fila_acuerdo(string proveedor, string acuerdo_de_precio, string tipo_de_acuerdo)
        {
            int fila = 0;
            int retorno = -1;
            while (fila <= acuerdo_de_precios.Rows.Count - 1)
            {
                if (acuerdo_de_precios.Rows[fila]["proveedor"].ToString() == proveedor &&
                    acuerdo_de_precios.Rows[fila]["acuerdo"].ToString() == acuerdo_de_precio &&
                    acuerdo_de_precios.Rows[fila]["tipo_de_acuerdo"].ToString() == tipo_de_acuerdo)
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private bool verificar_si_cargo(string id_producto)
        {
            bool retorno = false;
            int fila_producto = funciones.buscar_fila_por_id(id_producto, resumen);
            if (fila_producto > -1)
            {
                retorno = true;
            }

            return retorno;
        }
        private bool verificar_si_es_dulce(string id_producto)
        {
            bool retorno = false;
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedor);
           
            if (productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString() == "Dulces / Frutos secos")
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_productos_proveedor()
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villaMaipu");
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios");
        }
        private void consultar_remitos_del_mes(string nombre_proveedor)
        {
            pedidos_del_mes = consultas.consultar_pedido_segun_mes_y_año(base_de_datos, "pedidos", nombre_proveedor, "12", "2023");
        }
        #endregion

        public void crear_pdf(string ruta_archivo, byte[] logo)
        {
            //consultar_remitos_del_mes();
            consultar_acuerdo_de_precios();
            consultar_productos_proveedor();
            llenar_tabla_resumen();
            PDF.GenerarPDF_analisis_de_venta(ruta_archivo, logo, resumen, calcular_total());
        }
    }
}
