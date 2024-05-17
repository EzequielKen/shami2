using _04___sistemas_carrefour;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasCarrefour
{
    public partial class actualizador_de_precios : System.Web.UI.Page
    {
        #region carga productos
        private void crear_tabla_productos()
        {
            productos_carrefour = new DataTable();
            productos_carrefour.Columns.Add("id", typeof(string));
            productos_carrefour.Columns.Add("producto", typeof(string));
            productos_carrefour.Columns.Add("precio", typeof(string));
            productos_carrefour.Columns.Add("precio_nuevo", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            productos_carrefourBD = (DataTable)Session["productos_carrefourBD_acualizador"];
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila <= productos_carrefourBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productos_carrefourBD.Rows[fila]["producto"].ToString()))
                {
                    productos_carrefour.Rows.Add();
                    productos_carrefour.Rows[fila_producto]["id"] = productos_carrefourBD.Rows[fila]["id"].ToString();
                    productos_carrefour.Rows[fila_producto]["producto"] = productos_carrefourBD.Rows[fila]["producto"].ToString();
                    productos_carrefour.Rows[fila_producto]["precio"] = funciones.formatCurrency(double.Parse(productos_carrefourBD.Rows[fila]["precio"].ToString()));

                    if (productos_carrefourBD.Rows[fila]["precio_nuevo"].ToString() != "N/A")
                    {
                        productos_carrefour.Rows[fila_producto]["precio_nuevo"] = funciones.formatCurrency(double.Parse(productos_carrefourBD.Rows[fila]["precio_nuevo"].ToString()));
                    }
                    else
                    {
                        productos_carrefour.Rows[fila_producto]["precio_nuevo"] = productos_carrefourBD.Rows[fila]["precio_nuevo"].ToString();
                    }
                    fila_producto++;
                }

            }
        }
        private void cargar_productos()
        {
            llenar_tabla_productos();
            gridview_productos.DataSource = productos_carrefour;
            gridview_productos.DataBind();
        }
        #endregion 
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_actualizador_de_precios_carrefour actualizador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable productos_carrefourBD;
        DataTable productos_carrefour;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["actualizador_de_precios_carrefour"] == null)
            {
                Session.Add("actualizador_de_precios_carrefour", new cls_actualizador_de_precios_carrefour(usuariosBD));
            }
            actualizador = (cls_actualizador_de_precios_carrefour)Session["actualizador_de_precios_carrefour"];
            if (!IsPostBack)
            {
                Session.Add("productos_carrefourBD_acualizador", actualizador.get_productos_carrefour());
                productos_carrefourBD = (DataTable)Session["productos_carrefourBD_acualizador"];
                cargar_productos();
            }
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            actualizador.actualizar_precios((DataTable)Session["productos_carrefourBD_acualizador"]);
            Response.Redirect("~/paginasCarrefour/sucursales_carrefour.aspx", false);
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            productos_carrefourBD = (DataTable)Session["productos_carrefourBD_acualizador"];
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {

                //  TextBox textbox_precio_nuevo = (gridview_productos.Rows[fila].Cells[3].FindControl("textbox_precio_nuevo") as TextBox);

                //  textbox_precio_nuevo.Text = productos_carrefourBD.Rows[fila]["precio_nuevo"].ToString();

            }
        }

        protected void gridview_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void textbox_precio_nuevo_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;

            productos_carrefourBD = (DataTable)Session["productos_carrefourBD_acualizador"];
            string id_producto = gridview_productos.Rows[rowIndex].Cells[0].Text;
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, productos_carrefourBD);
            TextBox textbox_precio_nuevo = (gridview_productos.Rows[rowIndex].Cells[3].FindControl("textbox_precio_nuevo") as TextBox);
            double cantidad;
            if (double.TryParse(textbox_precio_nuevo.Text, out cantidad))
            {
                productos_carrefourBD.Rows[fila_tabla]["precio_nuevo"] = cantidad.ToString();
            }
            cargar_productos();
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }
    }
}