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
    public partial class panificados : System.Web.UI.Page
    {
        #region carga de stock
        private void cargar_stock(string id_producto, string stock_nuevo)
        {
            productos_panificados = (DataTable)Session["productos_panificados"];
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_panificados);
            double cantidad;
            if (double.TryParse(stock_nuevo, out cantidad))
            {
                productos_panificados.Rows[fila_producto]["stock_nuevo"] = cantidad.ToString();
            }
            Session.Add("productos_panificados", productos_panificados);
        }
        #endregion
        #region carga productos
        private void cargar_producto()
        {
            gridview_productos.DataSource = (DataTable)Session["productos_panificados"];
            gridview_productos.DataBind();
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_panificados panificado;

        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable productos_panificados;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["panificado"] == null)
            {
                Session.Add("panificado", new cls_panificados(usuariosBD));
            }
            panificado = (cls_panificados)Session["panificado"];
            if (!IsPostBack)
            {
                Session.Add("productos_panificados", panificado.get_productos_panificados());
                productos_panificados = (DataTable)Session["productos_panificados"];
                cargar_producto();
            }
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            panificado.actualizar_stock((DataTable)Session["productos_panificados"]);
            Response.Redirect("/paginasFabrica/sucursales.aspx", false);
        }

        protected void boton_aumentar_porcentaje_Click(object sender, EventArgs e)
        {

        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {

        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridview_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_aplicar_cambios")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                TextBox texbox_Stock = (gridview_productos.Rows[fila].Cells[5].FindControl("texbox_Stock") as TextBox);
                string id_producto = gridview_productos.Rows[fila].Cells[0].Text;
                cargar_stock(id_producto, texbox_Stock.Text);
                cargar_producto();
            }
        }
    }
}