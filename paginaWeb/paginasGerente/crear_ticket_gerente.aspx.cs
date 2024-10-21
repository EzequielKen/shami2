using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class crear_ticket_gerente : System.Web.UI.Page
    {
        #region ticket
        private void crear_tabla_ticket()
        {
            ticket = new DataTable();
            ticket.Columns.Add("fecha_solicitud", typeof(string));
            ticket.Columns.Add("fecha_resolucion_solicitada", typeof(string));
            ticket.Columns.Add("solicita", typeof(string));
            ticket.Columns.Add("tipo_ticket", typeof(string));
            ticket.Columns.Add("asunto", typeof(string));
            ticket.Columns.Add("detalle", typeof(string));
        }
        private void cargar_tabla_ticket()
        {
            crear_tabla_ticket();
            ticket.Rows.Add();
            DateTime fecha_resolucion_solicitada = DateTime.Parse(textbox_fecha.Text);
            ticket.Rows[0]["fecha_solicitud"] = funciones.get_fecha();
            ticket.Rows[0]["fecha_resolucion_solicitada"] = fecha_resolucion_solicitada.ToString("yyyy-MM-dd");
            ticket.Rows[0]["solicita"] = tipo_usuario.Rows[0]["rol"].ToString();
            ticket.Rows[0]["tipo_ticket"] = dropdown_tipo_ticket.SelectedItem.Text;
            ticket.Rows[0]["asunto"] = textbox_asunto.Text;
            ticket.Rows[0]["detalle"] = textbox_detalle.Text;
        }
        #endregion
        #region verificaciones
        private bool verificar_carga()
        {
            bool retorno = true;

            if (textbox_asunto.Text == string.Empty)
            {
                retorno = false;
                label_error_asunto.Text = "Falta ingresar motivo de su Ticket";
                label_error_asunto.Visible = true;
            }
            else
            {
                label_error_asunto.Visible = false;
            }

            if (textbox_fecha.Text == string.Empty)
            {
                label_error_fecha.Text = "Falta seleccionar fecha.";
                label_error_fecha.Visible = true;
            }
            else
            {
                DateTime fecha_seleccionada = DateTime.Parse(textbox_fecha.Text).Date;
                DateTime fecha_hoy = DateTime.Now.Date;  // Solo toma la fecha, sin horas

                if (fecha_seleccionada < fecha_hoy)
                {
                    label_error_fecha.Text = "No puede seleccionar una fecha anterior a " + DateTime.Now.ToString("dd/MM/yyyy");
                    label_error_fecha.Visible = true;
                }
                else
                {
                    label_error_fecha.Visible = false;
                }


            }

            if (textbox_detalle.Text == string.Empty)
            {
                label_error_detalle.Text = "Falta ingresar detalle del ticket.";
                label_error_detalle.Visible = true;
            }
            else
            {
                label_error_detalle.Visible = false;
            }
            return retorno;
        }
        #endregion
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_crear_ticket sys_ticket;
        cls_funciones funciones = new cls_funciones();
        DataTable tipo_usuario;
        DataTable usuariosBD;

        DataTable ticket;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            sys_ticket = new cls_crear_ticket(usuariosBD);
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                cargar_tabla_ticket();
                sys_ticket.crear_ticket(ticket);
                Response.Redirect("~/paginasFabrica/landing_page_local.aspx", false);
            }
        }
    }
}