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
    public partial class conteo_de_stock : System.Web.UI.Page
    {
        #region metodos cargas
        private void crear_tabla_productos()
        {
            productos = new DataTable();
            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("tipo_producto", typeof(string));
            productos.Columns.Add("conteo_stock", typeof(string));
            productos.Columns.Add("unidad_de_medida_local", typeof(string));
        }
        private void llenar_productos()
        {
            productosBD = (DataTable)Session["productosBD"];
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_tipo_producto(productosBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["conteo_stock"] = productosBD.Rows[fila]["conteo_stock"].ToString();
                    productos.Rows[ultima_fila]["unidad_de_medida_local"] = productosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                }
            }
        }
        private void cargar_productos()
        {
            llenar_productos();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        private void llenar_productos_buscar()
        {
            productosBD = (DataTable)Session["productosBD"];
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["conteo_stock"] = productosBD.Rows[fila]["conteo_stock"].ToString();
                    productos.Rows[ultima_fila]["unidad_de_medida_local"] = productosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                }
            }
        }
        private void cargar_productos_buscar()
        {
            llenar_productos_buscar();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            productosBD = (DataTable)Session["productosBD"];
            llenar_dropDownList(productosBD);
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
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>

        #region  atributos
        cls_conteo_de_stock conteo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable productosBD;
        DataTable productos;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            conteo = new cls_conteo_de_stock(usuariosBD);
            if (!IsPostBack)
            {
                productosBD = conteo.get_productos();
                Session.Add("productosBD", productosBD);
                configurar_controles();
                cargar_productos();
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_productos_buscar();
            textbox_buscar.Text = string.Empty;
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
            textbox_buscar.Text = string.Empty;
        }

        protected void texbox_stock_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_stock = (TextBox)sender;
            double cantidad;
            if (!double.TryParse(texbox_stock.Text, out cantidad))
            {
                texbox_stock.Text = string.Empty;
            }
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow fila = (GridViewRow)boton_cargar.NamingContainer;
            TextBox texbox_stock = (TextBox)fila.FindControl("texbox_stock");
            string id_producto = fila.Cells[0].Text;
            if (texbox_stock.Text != string.Empty)
            {
                productosBD = (DataTable)Session["productosBD"];
                int fila_producto = funciones.buscar_fila_por_id(id_producto, productosBD);
                productosBD.Rows[fila_producto]["conteo_stock"] = texbox_stock.Text;
                Session["productosBD"] = productosBD;
                cargar_productos();
            }
        }

        protected void boton_cargar_conteo_Click(object sender, EventArgs e)
        {
            productosBD = (DataTable)Session["productosBD"];
            conteo.cargar_conteo(productosBD);
            Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);
        }
    }
}