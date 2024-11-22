using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class acctualizador_precio_venta : System.Web.UI.Page
    {
        #region calculo de porcentajes
        private void aumentar_segun_porcentaje(string id_producto, double porcentaje)
        {
            // Obtener la tabla de productos desde la sesión
            productosBD = (DataTable)Session["productosBD"];

            // Buscar la fila correspondiente al producto por su ID
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productosBD);

            if (fila_producto != -1) // Verificar si se encontró el producto
            {
                // Obtener los valores actuales de precio_compra y precio_venta
                double precio_compra = double.Parse(productosBD.Rows[fila_producto]["precio_compra"].ToString());

                // Calcular el nuevo precio_venta
                double nuevo_precio_venta = precio_compra * (1 + (porcentaje / 100));

                // Actualizar el precio_venta en la tabla
                productosBD.Rows[fila_producto]["precio_nuevo"] = nuevo_precio_venta.ToString("F2"); // Formato con 2 decimales

                // Guardar nuevamente la tabla actualizada en la sesión
                Session.Add("productosBD", productosBD);
            }
            else
            {
                // Manejo del caso en que no se encuentre el producto
                throw new Exception("Producto no encontrado.");
            }
        }
        private void calcular_porcentaje_ganancia()
        {
            // Agregar una nueva columna para el porcentaje de ganancia
            productosBD = (DataTable)Session["productosBD"];
            double porcentaje_ganancia, diferencia;
            double precio_compra, precio_venta;

            // Iterar sobre las filas del DataTable
            for (int fila = 0; fila < productosBD.Rows.Count; fila++)
            {
                // Obtener los valores de precio_compra y precio_venta
                if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
                {
                    precio_compra = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                }
                else
                {
                    precio_compra = 0;
                }
                if (productosBD.Rows[fila]["precio_venta"].ToString() != "N/A")
                {
                    precio_venta = double.Parse(productosBD.Rows[fila]["precio_venta"].ToString());
                }
                else
                {
                    precio_venta = 0;
                }

                // Calcular la diferencia
                diferencia = precio_venta - precio_compra;

                // Calcular el porcentaje de ganancia
                porcentaje_ganancia = (diferencia / precio_compra) * 100;

                // Asignar el valor calculado a la nueva columna
                productosBD.Rows[fila]["porcentaje_ganancia"] = porcentaje_ganancia.ToString("F2") + " %"; // Formato con dos decimales y símbolo de porcentaje
                Session.Add("productosBD", productosBD);

            }
        }
        #endregion
        #region carga productos
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("precio_compra", typeof(string));
            resumen.Columns.Add("presentacion_compra", typeof(string));
            resumen.Columns.Add("precio_venta", typeof(string));
            resumen.Columns.Add("unidad_de_medida_local", typeof(string));
            resumen.Columns.Add("precio_nuevo", typeof(string));
            resumen.Columns.Add("porcentaje_ganancia", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            string tipo_producto;
            double precio_compra, precio_venta, precio_nuevo;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                tipo_producto = productosBD.Rows[fila]["tipo_producto"].ToString();
                if (funciones.verificar_tipo_producto(tipo_producto, dropDown_tipo.SelectedItem.Text))
                {
                    resumen.Rows.Add();
                    resumen.Rows[resumen.Rows.Count - 1]["id"] = productosBD.Rows[fila]["id"].ToString();
                    resumen.Rows[resumen.Rows.Count - 1]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
                    {
                        precio_compra = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_compra"] = funciones.formatCurrency(precio_compra);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_compra"] = "N/A";
                    }
                    resumen.Rows[resumen.Rows.Count - 1]["presentacion_compra"] = productosBD.Rows[fila]["presentacion_compra"].ToString();

                    if (productosBD.Rows[fila]["precio_venta"].ToString() != "N/A")
                    {
                        precio_venta = double.Parse(productosBD.Rows[fila]["precio_venta"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_venta"] = funciones.formatCurrency(precio_venta);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_venta"] = "N/A";
                    }

                    if (productosBD.Rows[fila]["precio_nuevo"].ToString() != "N/A")
                    {
                        precio_nuevo = double.Parse(productosBD.Rows[fila]["precio_nuevo"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_nuevo"] = funciones.formatCurrency(precio_nuevo);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_nuevo"] = "N/A";
                    }

                    resumen.Rows[resumen.Rows.Count - 1]["unidad_de_medida_local"] = productosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                    resumen.Rows[resumen.Rows.Count - 1]["porcentaje_ganancia"] = productosBD.Rows[fila]["porcentaje_ganancia"].ToString();
                }
            }
        }
        private void llenar_tabla_resumen_busqueda()
        {
            crear_tabla_resumen();
            string producto;
            double precio_compra, precio_venta, precio_nuevo;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                producto = productosBD.Rows[fila]["producto"].ToString();
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, producto))
                {
                    resumen.Rows.Add();
                    resumen.Rows[resumen.Rows.Count - 1]["id"] = productosBD.Rows[fila]["id"].ToString();
                    resumen.Rows[resumen.Rows.Count - 1]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
                    {
                        precio_compra = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_compra"] = funciones.formatCurrency(precio_compra);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_compra"] = "N/A";
                    }
                    resumen.Rows[resumen.Rows.Count - 1]["presentacion_compra"] = productosBD.Rows[fila]["presentacion_compra"].ToString();

                    if (productosBD.Rows[fila]["precio_venta"].ToString() != "N/A")
                    {
                        precio_venta = double.Parse(productosBD.Rows[fila]["precio_venta"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_venta"] = funciones.formatCurrency(precio_venta);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_venta"] = "N/A";
                    }

                    if (productosBD.Rows[fila]["precio_nuevo"].ToString() != "N/A")
                    {
                        precio_nuevo = double.Parse(productosBD.Rows[fila]["precio_nuevo"].ToString());
                        resumen.Rows[resumen.Rows.Count - 1]["precio_nuevo"] = funciones.formatCurrency(precio_nuevo);
                    }
                    else
                    {
                        resumen.Rows[resumen.Rows.Count - 1]["precio_nuevo"] = "N/A";
                    }

                    resumen.Rows[resumen.Rows.Count - 1]["unidad_de_medida_local"] = productosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                }
            }
        }
        private void cargar_productos()
        {
            productosBD = (DataTable)Session["productosBD"];
            llenar_tabla_resumen();
            gridview_productos.DataSource = resumen;
            gridview_productos.DataBind();
        }
        private void cargar_productos_busqueda()
        {
            if (textbox_buscar.Text != string.Empty)
            {
                productosBD = (DataTable)Session["productosBD"];
                llenar_tabla_resumen_busqueda();
                gridview_productos.DataSource = resumen;
                gridview_productos.DataBind();
            }
            else
            {
                cargar_productos();
            }
        }
        #endregion
        #region configurar controles 
        private void configurar_controles()
        {
            llenar_dropDownList(productosBD);
        }
        private void cargar_tipo_de_acuerdo()
        {
            for (int fila = 0; fila <= tipo_de_acuerdo.Rows.Count - 1; fila++)
            {
                dropdown_acuerdo.Items.Add(tipo_de_acuerdo.Rows[fila]["tipo_de_acuerdo"].ToString());
            }
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }
        }
        #endregion
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_acctualizador_precio_venta actualizador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_de_acuerdo;
        DataTable productosBD;
        DataTable resumen;
        string tipo_seleccionado;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            actualizador = new cls_acctualizador_precio_venta(usuariosBD);

            if (!IsPostBack)
            {
                tipo_de_acuerdo = actualizador.get_tipo_acuerdo();
                cargar_tipo_de_acuerdo();
                productosBD = actualizador.get_productos(dropdown_acuerdo.SelectedItem.Text);
                Session.Add("productosBD", productosBD);
                configurar_controles();
                cargar_productos();
            }
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                if (gridview_productos.Rows[fila].Cells[2].Text == "N/A")
                {
                    TextBox texbox_porcentaje_de_aumento = (gridview_productos.Rows[fila].Cells[7].FindControl("texbox_porcentaje_de_aumento") as TextBox);
                    texbox_porcentaje_de_aumento.Visible = false;
                }
            }
        }

        protected void texbox_precio_TextChanged(object sender, EventArgs e)
        {
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_productos_busqueda();
        }

        protected void texbox_porcentaje_de_aumento_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_porcentaje_de_aumento = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_porcentaje_de_aumento.NamingContainer;
            int fila = row.RowIndex;

            string id_producto = gridview_productos.Rows[fila].Cells[0].Text;

            if (double.TryParse(texbox_porcentaje_de_aumento.Text, out double porcentaje))
            {
                aumentar_segun_porcentaje(id_producto, porcentaje);
                if (textbox_buscar.Text != string.Empty)
                {
                    cargar_productos();
                }
                else
                {
                    cargar_productos_busqueda();
                }
            }
            else
            {
                texbox_porcentaje_de_aumento.Text = string.Empty;
            }
        }


    }
}