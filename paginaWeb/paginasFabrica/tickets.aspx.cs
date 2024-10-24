using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class tickets : System.Web.UI.Page
    {
        #region reordenar tickets
        private void ReordenarPrioridades(int id, int nuevaPrioridad)
        {
            // Supongamos que obtienes el DataTable de los tickets
            DataTable dt = (DataTable)Session["ticketsBD"]; // Aquí obtienes tu DataTable actual

            // Obtener la fila del ticket cuya prioridad se está cambiando
            DataRow filaTicket = dt.Rows.Find(id);
            int prioridadAnterior = Convert.ToInt32(filaTicket["prioridad"]);

            // Cambiar la prioridad del ticket seleccionado
            filaTicket["prioridad"] = nuevaPrioridad;

            // Ajustar las prioridades de los otros tickets
            foreach (DataRow row in dt.Rows)
            {
                int prioridadActual = Convert.ToInt32(row["prioridad"]);
                if (row["id"] != filaTicket["id"]) // Omitir el ticket que estamos cambiando
                {
                    // Si la prioridad de este ticket se ve afectada por el cambio, ajustarla
                    if (prioridadActual >= nuevaPrioridad && prioridadActual < prioridadAnterior)
                    {
                        row["prioridad"] = prioridadActual + 1;
                    }
                    else if (prioridadActual <= nuevaPrioridad && prioridadActual > prioridadAnterior)
                    {
                        row["prioridad"] = prioridadActual - 1;
                    }
                }
            }

            // Guardar los cambios en el DataTable o actualizar en la base de datos según sea necesario
            Session.Add("ticketsBD", ticketsBD);
        }

        #endregion
        #region carga tickets
        private void cargar_tickets()
        {
            ticketsBD = sys_tickets.get_tickets(dropdown_mes.SelectedItem.Text, dropdown_año.SelectedItem.Text, dropdown_tipo.SelectedItem.Text);
            Session.Add("ticketsBD", ticketsBD);
            gridView_tickets.DataSource = ticketsBD;
            gridView_tickets.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            cargar_meses();
            cargar_año();
        }
        private void cargar_meses()
        {
            for (int mes = 1; mes <= 12; mes++)
            {
                dropdown_mes.Items.Add(mes.ToString());
            }
            dropdown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            for (int año = 2024; año <= DateTime.Now.Year; año++)
            {
                dropdown_año.Items.Add(año.ToString());
            }
            dropdown_año.SelectedValue = DateTime.Now.Year.ToString();
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_tickets sys_tickets;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable ticketsBD;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sys_tickets = new cls_tickets(usuariosBD);
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_tickets();
            }
        }

        protected void dropdown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_tickets();

        }

        protected void dropdown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_tickets();

        }

        protected void dropdown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_tickets();

        }


        protected void textbox_prioridad_TextChanged(object sender, EventArgs e)
        {
            // Obtener el TextBox que disparó el evento
            TextBox txtPrioridad = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtPrioridad.NamingContainer;
            int fila = row.RowIndex;

            // Obtener el ID del ticket y la nueva prioridad
            string id = gridView_tickets.Rows[fila].Cells[0].Text;
            int nuevaPrioridad;
            if (int.TryParse(txtPrioridad.Text, out nuevaPrioridad))
            {
                // Lógica para reordenar las prioridades
                sys_tickets.reordenar_tickets(id, nuevaPrioridad.ToString(), dropdown_mes.SelectedItem.Text, dropdown_año.SelectedItem.Text, dropdown_tipo.SelectedItem.Text);
            }
            cargar_tickets();
        }


        protected void gridView_tickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ticketsBD = (DataTable)Session["ticketsBD"];
            string id;
            int fila_ticket;
            for (int fila = 0; fila <= gridView_tickets.Rows.Count - 1; fila++)
            {
                id = gridView_tickets.Rows[fila].Cells[0].Text;
                fila_ticket = funciones.buscar_fila_por_id(id, ticketsBD);
                TextBox textbox_prioridad = (gridView_tickets.Rows[fila].Cells[2].FindControl("textbox_prioridad") as TextBox);
                Button boton_resolver = (gridView_tickets.Rows[fila].Cells[9].FindControl("boton_resolver") as Button);
                textbox_prioridad.Text = ticketsBD.Rows[fila_ticket]["prioridad"].ToString();

                if (ticketsBD.Rows[fila_ticket]["estado"].ToString() == "abierto")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-warning text-center table-responsive";
                }
                else if (ticketsBD.Rows[fila_ticket]["estado"].ToString() == "resuelto")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-success text-center table-responsive";
                    boton_resolver.Visible = false;
                }
                else if (ticketsBD.Rows[fila_ticket]["estado"].ToString() == "cancelado")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-danger text-center table-responsive";
                    boton_resolver.Visible = false;
                }
            }
        }

        protected void boton_ordenar_area_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasGerente/tickets_por_area.aspx", false);
        }

        protected void boton_resolver_Click(object sender, EventArgs e)
        {
            // Obtener el TextBox que disparó el evento
            Button boton_resolver = (Button)sender;
            GridViewRow row = (GridViewRow)boton_resolver.NamingContainer;
            int fila = row.RowIndex;

            // Obtener el ID del ticket y la nueva prioridad
            string id = gridView_tickets.Rows[fila].Cells[0].Text;

            sys_tickets.cerrar_ticket(id);
            cargar_tickets();
        }
    }
}