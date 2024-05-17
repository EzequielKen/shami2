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
    public partial class recepcion_de_insumos : System.Web.UI.Page
    {
        #region cargar cantidad
        private void cargar_cantidad_recibida(string id_producto, string cantidad_dato)
        {
            pedido = (DataTable)Session["pedido_recepcion_insumo"];
            int fila_producto = funciones.buscar_fila_por_id(id_producto, pedido);
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                if (cantidad > 0)
                {

                    pedido.Rows[fila_producto]["cantidad_recibida"] = cantidad.ToString() + " " + pedido.Rows[fila_producto]["presentacion_despachada"].ToString();
                    pedido.Rows[fila_producto]["cantidad_recibida_dato"] = cantidad.ToString();
                }
                else
                {
                    pedido.Rows[fila_producto]["cantidad_recibida"] = "N/A";
                    pedido.Rows[fila_producto]["cantidad_recibida_dato"] = "N/A";
                }
            }
            else
            {
                pedido.Rows[fila_producto]["cantidad_recibida"] = "N/A";
                pedido.Rows[fila_producto]["cantidad_recibida_dato"] = "N/A";
            }
            Session.Add("pedido_recepcion_insumo", pedido);
        }
        #endregion
        #region carga pedido
        private void cargar_pedido()
        {
            gridview_insumos_del_proveedor.DataSource = (DataTable)Session["pedido_recepcion_insumo"];
            gridview_insumos_del_proveedor.DataBind();
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_recepcion_de_insumos recepcion_insumos;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable pedido;
        string id_pedido_insumo_seleccionado;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            id_pedido_insumo_seleccionado = Session["id_pedido_insumo_seleccionado"].ToString();
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["recepcion_insumos"] == null)
            {
                Session.Add("recepcion_insumos", new cls_recepcion_de_insumos(usuariosBD));
            }
            recepcion_insumos = (cls_recepcion_de_insumos)Session["recepcion_insumos"];
            if (!IsPostBack)
            {
                pedido = recepcion_insumos.get_pedido(id_pedido_insumo_seleccionado);
                Session.Add("pedido_recepcion_insumo", pedido);
                cargar_pedido();
            }

        }

        protected void boton_guardar_Click(object sender, EventArgs e)
        {
            recepcion_insumos.recibir_insumos((DataTable)Session["pedido_recepcion_insumo"], Session["id_pedido_insumo_seleccionado"].ToString());
            Response.Redirect("/paginasFabrica/historial_pedido_de_insumos.aspx", false);
        }

        protected void gridview_insumos_del_proveedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_aplicar_cambios")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                string id_producto = gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text;
                TextBox textbox_cantidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[4].FindControl("textbox_cantidad") as TextBox);
                cargar_cantidad_recibida(id_producto, textbox_cantidad.Text);
                cargar_pedido();
            }
        }

        protected void gridview_insumos_del_proveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridview_insumos_del_proveedor.Rows.Count - 1; fila++)
            {
                if (gridview_insumos_del_proveedor.Rows[fila].Cells[3].Text == "N/A")
                {
                    TextBox textbox_cantidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[4].FindControl("textbox_cantidad") as TextBox);
                    textbox_cantidad.Visible = false;
                    gridview_insumos_del_proveedor.Rows[fila].Cells[6].Controls[0].Visible = false;
                }
            }
        }
    }
}