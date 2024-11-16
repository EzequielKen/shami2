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
    public partial class tickets_por_area : System.Web.UI.Page
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
            tickets_area = sys_tickets.get_tickets_area(dropdown_mes.SelectedItem.Text, dropdown_año.SelectedItem.Text, dropdown_solicitante.SelectedItem.Text);
            Session.Add("tickets_area", tickets_area);
            gridView_tickets.DataSource = tickets_area;
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
        private void cargar_solicitantes()
        {
            ticketsBD = (DataTable)Session["ticketsBD"];
            DataTable solicitantes = new DataTable();
            solicitantes.Columns.Add("solicitante", typeof(string));
            string solicitante;
            for (int fila = 0; fila <= ticketsBD.Rows.Count - 1; fila++)
            {
                solicitante = ticketsBD.Rows[fila]["solicita"].ToString();
                if (!funciones.verificar_si_cargo_por_columna(solicitante, "solicitante", solicitantes))
                {
                    solicitantes.Rows.Add();
                    solicitantes.Rows[solicitantes.Rows.Count - 1]["solicitante"] = solicitante;
                    dropdown_solicitante.Items.Add(solicitante);
                }
            }
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_tickets sys_tickets;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;

        DataTable ticketsBD;
        DataTable tickets_area;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];

            sys_tickets = new cls_tickets(usuariosBD);
            if (!IsPostBack)
            {
                configurar_controles();
                ticketsBD = sys_tickets.get_todos_tickets_abiertos(dropdown_mes.SelectedItem.Text, dropdown_año.SelectedItem.Text);
                Session.Add("ticketsBD", ticketsBD);
                cargar_solicitantes();

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

        protected void dropdown_solicitante_SelectedIndexChanged(object sender, EventArgs e)
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
                sys_tickets.reordenar_tickets_area(id, nuevaPrioridad.ToString(), dropdown_mes.SelectedItem.Text, dropdown_año.SelectedItem.Text, dropdown_solicitante.SelectedItem.Text);
            }
            cargar_tickets();
        }


        protected void gridView_tickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            tickets_area = (DataTable)Session["tickets_area"];
            string id;
            int fila_ticket;
            for (int fila = 0; fila <= gridView_tickets.Rows.Count - 1; fila++)
            {
                id = gridView_tickets.Rows[fila].Cells[0].Text;
                fila_ticket = funciones.buscar_fila_por_id(id, tickets_area);
                TextBox textbox_prioridad = (gridView_tickets.Rows[fila].Cells[1].FindControl("textbox_prioridad") as TextBox);
                Button boton_resolver = (gridView_tickets.Rows[fila].Cells[9].FindControl("boton_resolver") as Button);
                textbox_prioridad.Text = tickets_area.Rows[fila_ticket]["prioridad_area"].ToString();

                if (tickets_area.Rows[fila_ticket]["estado"].ToString() == "abierto")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-warning text-center table-responsive";
                }
                else if (tickets_area.Rows[fila_ticket]["estado"].ToString() == "resuelto")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-success text-center table-responsive";
                    boton_resolver.Visible = false;
                }
                else if (tickets_area.Rows[fila_ticket]["estado"].ToString() == "cancelado")
                {
                    gridView_tickets.Rows[fila].CssClass = "table table-danger text-center table-responsive";
                    boton_resolver.Visible = false;
                }
            }
            if (tipo_usuario.Rows[0]["rol"].ToString() != "Shami Villa Maipu Admin" &&
                            tipo_usuario.Rows[0]["rol"].ToString() != "Shami Sistemas")
            {
                gridView_tickets.Columns[1].Visible = false;
                gridView_tickets.Columns[9].Visible = false;
            }
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

        protected void boton_volver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasFabrica/tickets.aspx", false);

        }
    }
}