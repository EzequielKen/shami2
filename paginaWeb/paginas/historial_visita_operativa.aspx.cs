using _02___sistemas;
using paginaWeb.paginasFabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class historial_visita_operativa : System.Web.UI.Page
    {
        #region carga de evaluaciones
        private void cargar_evaluaciones()
        {
            historial_evaluacion = historial.get_historial_evaluacion_chequeo(dropdown_año.SelectedItem.Text, dropdown_mes.SelectedItem.Text, sucursal.Rows[0]["id"].ToString());
            Session.Add("historial_evaluacion", historial_evaluacion);
            gridview_visitas.DataSource = historial_evaluacion;
            gridview_visitas.DataBind();
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
        /// ////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_visita_operativa_local historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;

        DataTable historial_evaluacion;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];

            historial = new cls_historial_visita_operativa_local(usuariosBD);
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_evaluaciones();
            }
        }

        protected void dropdown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_evaluaciones();
        }

        protected void dropdown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_evaluaciones();
        }

        protected void boton_historial_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int fila = row.RowIndex;

            string fecha = gridview_visitas.Rows[fila].Cells[0].Text;

            historial_evaluacion = (DataTable)Session["historial_evaluacion"];

            int fila_fecha = funciones.buscar_fila_por_dato(fecha,"fecha", historial_evaluacion);

            Session.Add("fecha_historial_visita", historial_evaluacion.Rows[fila_fecha]["fecha_historial"].ToString());
            Response.Redirect("/paginas/detalle_visita_operativa.aspx",false);
        }
    }
}