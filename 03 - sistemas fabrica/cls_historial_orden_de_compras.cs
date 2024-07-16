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
    public class cls_historial_orden_de_compras
    {
        public cls_historial_orden_de_compras(DataTable usuario_BD)
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
            carga_ordenes = new cls_cargar_orden_de_compra(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_cargar_orden_de_compra carga_ordenes;
        cls_PDF PDF = new cls_PDF();
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable proveedores_de_fabrica;
        DataTable proveedores_de_fabrica_seleccionado;
        DataTable ordenes_de_compra_de_proveedor;
        DataTable ordenes_de_compra;
        DataTable orden_compra;
        DataTable resumen_orden;
        #endregion
        #region cargar nota
        public void cargar_nota(string id_orden, string nota)
        {
            string actualizar = "`nota` = '"+ nota + "' ";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
        }
        #endregion
        #region cancelar orden de compra
        public void cancelar_orden_de_compra(string id_orden)
        {
            string actualizar= "`estado` = 'Cancelada' ";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra",actualizar, id_orden);
        }
        #endregion
        #region PDF
        public void crear_PDF_orden_de_compra_con_precio(string ruta_archivo, byte[] logo, string id_orden)//
        {
            consultar_orden_de_compra_por_id(id_orden);

            consultar_proveedores_de_fabrica_seleccionado(orden_compra.Rows[0]["id_proveedor"].ToString());

            abrir_orden();
            string num_orden, fecha_pedido, fecha_estimada;
            num_orden = orden_compra.Rows[0]["id"].ToString();
            fecha_pedido = orden_compra.Rows[0]["fecha"].ToString();
            fecha_estimada = orden_compra.Rows[0]["fecha_entrega_estimada"].ToString();
            PDF.GenerarPDF_orden_de_compra_con_precio(ruta_archivo, logo, resumen_orden, proveedores_de_fabrica_seleccionado, num_orden, fecha_pedido, fecha_estimada, calcular_total_orden(), orden_compra.Rows[0]["impuestos"].ToString());
            //            PDF.GenerarPDF_orden_de_compra()
        }
        private string calcular_total_orden()
        {
            double total = 0;
            for (int fila = 0; fila <= resumen_orden.Rows.Count-1; fila++)
            {
                total = total + double.Parse(resumen_orden.Rows[fila]["sub_total_dato"].ToString());
            }
            return funciones.formatCurrency(total);
        }
        public void crear_PDF_orden_de_compra(string ruta_archivo, byte[] logo, string id_orden)//
        {
           consultar_orden_de_compra_por_id(id_orden);

            consultar_proveedores_de_fabrica_seleccionado(orden_compra.Rows[0]["id_proveedor"].ToString());

            abrir_orden();
            string num_orden;
            DateTime fecha_pedido, fecha_estimada;
            num_orden = orden_compra.Rows[0]["id"].ToString();
            fecha_pedido = (DateTime)orden_compra.Rows[0]["fecha"];
            fecha_estimada = (DateTime)orden_compra.Rows[0]["fecha_entrega_estimada"];
            PDF.GenerarPDF_orden_de_compra(ruta_archivo, logo, resumen_orden, proveedores_de_fabrica_seleccionado, num_orden, fecha_pedido.ToString("dd/MM/yyyy"), fecha_estimada.ToString("dd/MM/yyyy"));
            //            PDF.GenerarPDF_orden_de_compra()
        }

        #endregion
        #region abrir orden de compra
        private void crear_tabla_resumen()
        {
            resumen_orden = new DataTable();
            resumen_orden.Columns.Add("id", typeof(string));
            resumen_orden.Columns.Add("producto", typeof(string));
            resumen_orden.Columns.Add("cantidad_pedida", typeof(string)); 
            resumen_orden.Columns.Add("cantidad", typeof(string)); 
            resumen_orden.Columns.Add("unidad de medida", typeof(string));
            resumen_orden.Columns.Add("precio", typeof(string)); 
            resumen_orden.Columns.Add("sub_total", typeof(string)); 
            resumen_orden.Columns.Add("sub_total_dato", typeof(string)); 
        }
        private void abrir_orden()
        {
            crear_tabla_resumen();
            string id, producto, cantidad_pedida, cantidad, tipo_paquete, cantidad_unidades, unidad_medida, unidad_de_medida,precio;

            for (int columna = orden_compra.Columns["producto_1"].Ordinal; columna <= orden_compra.Columns.Count - 1; columna++)
            {
                if (orden_compra.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 2);
                    precio = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 3);

                    tipo_paquete = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 4);
                    cantidad_unidades = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 5);
                    unidad_medida = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(),6);

                    if (tipo_paquete == "Unidad")
                    {
                        unidad_de_medida = " x " + cantidad_unidades + " " + unidad_medida;//cantidad_unidades + " " +
                    }
                    else
                    {
                        unidad_de_medida = tipo_paquete + " x " + cantidad_unidades + " " + unidad_medida;
                    }
                    cantidad_pedida = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 7);
                    cantidad = funciones.obtener_dato(orden_compra.Rows[0][columna].ToString(), 8);
                    if (cantidad=="N/A")
                    {
                        cantidad="0";
                    }
                    cargar_producto(id, producto, cantidad_pedida, cantidad, unidad_de_medida, precio);
                }
            }
        }
        private void cargar_producto(string id, string producto, string cantidad_pedida, string cantidad, string unidad_de_medida, string precio)
        {
            resumen_orden.Rows.Add();
            int ultima_fila = resumen_orden.Rows.Count - 1;

            resumen_orden.Rows[ultima_fila]["id"] = id;
            resumen_orden.Rows[ultima_fila]["producto"] = producto;
            resumen_orden.Rows[ultima_fila]["cantidad_pedida"] = cantidad_pedida;
            resumen_orden.Rows[ultima_fila]["cantidad"] = cantidad;
            resumen_orden.Rows[ultima_fila]["precio"] = funciones.formatCurrency(double.Parse(precio));
            resumen_orden.Rows[ultima_fila]["sub_total"] = funciones.formatCurrency(double.Parse(cantidad) * double.Parse(precio));
            resumen_orden.Rows[ultima_fila]["sub_total_dato"] = Math.Round(double.Parse(cantidad) * double.Parse(precio), 2).ToString();



            resumen_orden.Rows[ultima_fila]["unidad de medida"] = unidad_de_medida;
        }
        #endregion
        #region metodos privados
        private string buscar_id_de_proveedor(string nombre_proveedor)
        {
            string retorno = "";
            int fila = 0;
            while (fila <= proveedores_de_fabrica.Rows.Count - 1)
            {
                string dato = proveedores_de_fabrica.Rows[fila]["proveedor"].ToString();
                if (nombre_proveedor == proveedores_de_fabrica.Rows[fila]["proveedor"].ToString())
                {
                    retorno = proveedores_de_fabrica.Rows[fila]["id"].ToString();
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion
        #region metodos consultas
        private void consultar_ordenes_de_compra(string mes, string año)
        {
            ordenes_de_compra = consultas.consultar_orden_de_compras_segun_mes(mes,año);
        } 
        private void consultar_orden_de_compra_por_id(string id)
        {
            orden_compra = consultas.consultar_ordenes_de_compra_de_proveedor_por_id(id);
        }
        private void consultar_proveedores_de_fabrica_seleccionado(string id_proveedor)
        {
            proveedores_de_fabrica_seleccionado = consultas.consultar_proveedores_de_fabrica_seleccionado(id_proveedor);
        }
        private void consultar_ordenes_de_compra_de_proveedor(string id_proveedor)
        {
            ordenes_de_compra_de_proveedor = consultas.consultar_ordenes_de_compra_de_proveedor(id_proveedor);
        }
        private void consultar_lista_proveedores_fabrica()
        {
            proveedores_de_fabrica = consultas.consultar_tabla_completa(base_de_datos, "proveedores_de_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_proveedores_de_fabrica()
        {
            consultar_lista_proveedores_fabrica();
            return proveedores_de_fabrica;
        }
        public DataTable get_ordenes_de_compra(string mes,string año)
        {
            consultar_ordenes_de_compra(mes,año);
            return ordenes_de_compra;
        }
        public DataTable get_ordenes_de_compra_de_proveedor(string nombre_proveedor)
        {
            string id_proveedor = buscar_id_de_proveedor(nombre_proveedor);
            consultar_ordenes_de_compra_de_proveedor(id_proveedor);
            return ordenes_de_compra_de_proveedor;
        }
        public DataTable get_proveedores_de_fabrica_seleccionado(string nombre_proveedor)
        {
            consultar_lista_proveedores_fabrica();

            string id_proveedor = buscar_id_de_proveedor(nombre_proveedor);
            consultar_proveedores_de_fabrica_seleccionado(id_proveedor);
            return proveedores_de_fabrica_seleccionado;
        }
        #endregion
    }
}
