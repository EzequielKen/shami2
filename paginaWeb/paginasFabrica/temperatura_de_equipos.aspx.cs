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
    public partial class temperatura_de_equipos : System.Web.UI.Page
    {
        private void cargar_equipos()
        {
            equipos = temperaturas.get_equipos(dropdown_ubicaciones.SelectedItem.Text);
            gridview_equipos.DataSource = equipos;
            gridview_equipos.DataBind();
        }
        private void cargar_ubicaciones()
        {
            ubicaciones = temperaturas.get_ubicaciones();
            dropdown_ubicaciones.Items.Add("todos");
            for (int fila = 0; fila <= ubicaciones.Rows.Count - 1; fila++)
            {
                dropdown_ubicaciones.Items.Add(ubicaciones.Rows[fila]["ubicacion"].ToString());
            }
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_temperatura_de_equipos temperaturas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable ubicaciones;
        DataTable equipos;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["temperaturas_de_equipos"] == null)
            {
                Session.Add("temperaturas_de_equipos", new cls_temperatura_de_equipos(usuariosBD));
            }
            temperaturas = (cls_temperatura_de_equipos)Session["temperaturas_de_equipos"];
            if (!IsPostBack)
            {
                cargar_ubicaciones();
                cargar_equipos();
            }
        }

        protected void boton_administrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasFabrica/administracion_de_equipos.aspx", false);
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {

        }

        protected void gridview_equipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <=gridview_equipos.Rows.Count-1; fila++)
            {
                if (gridview_equipos.Rows[fila].Cells[3].Text== "Congelacion")
                {
                    gridview_equipos.Rows[fila].CssClass = "table-primary";
                }
                else if (gridview_equipos.Rows[fila].Cells[3].Text == "Refrigeracion")
                {
                    gridview_equipos.Rows[fila].CssClass = "table-warning";
                }
                else if (gridview_equipos.Rows[fila].Cells[3].Text == "Coccion")
                {
                    gridview_equipos.Rows[fila].CssClass = "table-danger";
                }
            }
        }

        protected void dropdown_ubicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_equipos();
        }

        protected void textbox_temperatura_diaria_1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void textbox_temperatura_diaria_2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void textbox_temperatura_diaria_3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}