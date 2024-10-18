using _08___sistemas_marketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasMarketing
{
    public partial class solicitud_franquicia : System.Web.UI.Page
    {
        #region carga de solicitudes
        private void cargar_solicitudes()
        {
            gridview_solicitudes.DataSource = solicitudes.get_solicitud_franquicia(dropdown_mes.SelectedItem.Text,dropdown_año.SelectedItem.Text);
            gridview_solicitudes.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            configurar_meses();
            configurar_año();
        }
        private void configurar_meses()
        {
            for (int mes = 1; mes <= 12; mes++)
            {
                dropdown_mes.Items.Add(mes.ToString());
            }
            dropdown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void configurar_año()
        {
            for (int año = 2024; año <= DateTime.Now.Year; año++)
            {
                dropdown_año.Items.Add(año.ToString());
            }
            dropdown_mes.SelectedValue = DateTime.Now.Year.ToString();
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_solicitud_franquicia solicitudes;
        DataTable usuariosBD;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];

            solicitudes = new cls_solicitud_franquicia(usuariosBD);

            if (!IsPostBack)
            {
                configurar_controles();
                cargar_solicitudes();
            }
        }

        protected void boton_abrir_Click(object sender, EventArgs e)
        {
            Button boton_abrir = (Button)sender;
            GridViewRow row = (GridViewRow)boton_abrir.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_solicitudes.Rows[fila].Cells[0].Text;

            Session.Add("id_solicitud", id);
            Response.Redirect("/paginasMarketing/solicitud_abierta.aspx",false);
        }

        protected void dropdown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_solicitudes();
        }

        protected void dropdown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_solicitudes();
        }
    }
}