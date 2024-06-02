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
    public partial class historial_temperatura : System.Web.UI.Page
    {
        private void cargar_ubicaciones()
        {
            ubicaciones = historial.get_ubicaciones();
            dropdown_ubicaciones.Items.Add("todos");
            for (int fila = 0; fila <= ubicaciones.Rows.Count - 1; fila++)
            {
                dropdown_ubicaciones.Items.Add(ubicaciones.Rows[fila]["ubicacion"].ToString());
            }
        }
        private void cargar_equipos()
        {
            DateTime fecha = DateTime.Now;
            equipos = historial.get_equipos(dropdown_ubicaciones.SelectedItem.Text, fecha);
            gridview_equipos.DataSource = equipos;
            gridview_equipos.DataBind();
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_temperatura historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable ubicaciones;
        DataTable equipos;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["historial_de_equipos"] == null)
            {
                Session.Add("historial_de_equipos", new cls_historial_temperatura(usuariosBD));
            }
            historial = (cls_historial_temperatura)Session["historial_de_equipos"];
            if (!IsPostBack)
            {
                cargar_ubicaciones();
                cargar_equipos();
            }
        }
    }
}