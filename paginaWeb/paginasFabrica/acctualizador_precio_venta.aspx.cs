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
            if (textbox_buscar.Text!=string.Empty)
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
    }
}