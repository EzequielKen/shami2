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
            DateTime fecha = DateTime.Now;
            equipos = temperaturas.get_equipos(dropdown_ubicaciones.SelectedItem.Text, fecha);
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
        private string verificar_horario()
        {
            string retorno = "fuera de rango";
            DateTime miFecha = DateTime.Now;
            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 8, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 12, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "rango 1";
            }
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 8, 0, 0); // 8:00 AM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 12, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2)
            {
                retorno = "rango 2";
            }
            DateTime horaInicio_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 8, 0, 0); // 8:00 AM
            DateTime horaFin_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 12, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango3 && miFecha <= horaFin_rango3)
            {
                retorno = "rango 3";
            }
            return retorno;
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
            if (textbox_nombre.Text != string.Empty)
            {
                label_cartel_advertencia.Visible = false;
                Button boton_cargar = (Button)sender;
                GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
                int fila = row.RowIndex;
                TextBox textbox_temperatura_diaria = (gridview_equipos.Rows[fila].Cells[6].FindControl("textbox_temperatura_diaria") as TextBox);
                DropDownList dropdown_nota = (gridview_equipos.Rows[fila].Cells[7].FindControl("dropdown_nota") as DropDownList);
                {
                    string temperatura = "";
                    if (textbox_temperatura_diaria.Text != string.Empty)
                    {
                        temperatura = textbox_temperatura_diaria.Text;
                    }
                    temperaturas.registrar_temperatura(textbox_nombre.Text, gridview_equipos.Rows[fila].Cells[0].Text, gridview_equipos.Rows[fila].Cells[2].Text, temperatura, dropdown_nota.SelectedItem.Text);
                    textbox_temperatura_diaria.Text = string.Empty;
                    cargar_equipos();

                }
            }
            else
            {
                label_cartel_advertencia.Visible = true;
            }
        }

        protected void gridview_equipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridview_equipos.Rows.Count - 1; fila++)
            {
                if (gridview_equipos.Rows[fila].Cells[3].Text == "Congelacion")
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