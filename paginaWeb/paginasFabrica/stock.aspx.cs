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

    public partial class stock : System.Web.UI.Page
    {
        private void crear_tabla_productos()
        {
            productos = new DataTable();

            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("unidad_de_medida_local", typeof(string));
            productos.Columns.Add("stock_produccion", typeof(string));
            productos.Columns.Add("stock_expedicion", typeof(string));
            productos.Columns.Add("stock_carrefour", typeof(string));
            productos.Columns.Add("stock", typeof(string));
            productos.Columns.Add("pedido", typeof(string)); 
            productos.Columns.Add("diferencia", typeof(string)); 
            productos.Columns.Add("promedio_pedido", typeof(string)); 
        }
        private void llenar_tabla_productos()
        {
            crear_tabla_productos();
            int fila_produto = 0;
            for (int fila = 0; fila <= productosBD.Rows.Count-1; fila++)
            {
                if (verificar_tipo_producto(productosBD.Rows[fila]["tipo_producto"].ToString()) &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();

                    productos.Rows[fila_produto]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[fila_produto]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[fila_produto]["unidad_de_medida_local"] = productosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                    productos.Rows[fila_produto]["stock_produccion"] = productosBD.Rows[fila]["stock_produccion"].ToString();
                    productos.Rows[fila_produto]["stock_expedicion"] = productosBD.Rows[fila]["stock_expedicion"].ToString();
                    productos.Rows[fila_produto]["stock_carrefour"] = productosBD.Rows[fila]["stock_carrefour"].ToString();
                    productos.Rows[fila_produto]["stock"] = productosBD.Rows[fila]["stock"].ToString();
                    productos.Rows[fila_produto]["pedido"] = productosBD.Rows[fila]["pedido"].ToString(); 
                    productos.Rows[fila_produto]["diferencia"] = productosBD.Rows[fila]["diferencia"].ToString(); 
                    productos.Rows[fila_produto]["promedio_pedido"] = productosBD.Rows[fila]["promedio_pedido"].ToString(); 

                    fila_produto++;
                }
            }
        }

        private void cargar_productos()
        {
            llenar_tabla_productos();

            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        private bool verificar_tipo_producto(string tipo_producto)
        {
            tipo_seleccionado = Session["tipo_seleccionado"].ToString();

            bool retorno = false;
            if (tipo_seleccionado == tipo_producto || tipo_seleccionado == "Todos")
            {
                retorno = true;
            }
            return retorno;
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

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
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_stock stockBD;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;
        DataTable productosBD;
        DataTable productos;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (System.Data.DataTable)Session["tipo_usuario"];

            if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
            {
                gridview_productos.Columns[3].Visible = false;
                gridview_productos.Columns[4].Visible = false;
            }
            else if(proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" && 
                tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
            {
                gridview_productos.Columns[3].Visible = false;
                gridview_productos.Columns[4].Visible = false;
            }
            if (Session["stock"]==null)
            {
                Session.Add("stock",new cls_stock(usuariosBD));
            }
            stockBD = (cls_stock)Session["stock"];
            productosBD = stockBD.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            if (!IsPostBack)
            {
                llenar_dropDownList(productosBD);
                Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);

                cargar_productos();
            }
        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            cargar_productos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            cargar_productos();
        }
    }
}