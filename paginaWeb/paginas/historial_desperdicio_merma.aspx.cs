using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class historial_desperdicio_merma : System.Web.UI.Page
    {
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_desperdicio_merma historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            historial = new cls_historial_desperdicio_merma(usuariosBD);
            if (!IsPostBack)
            {
                label_fecha_seleccionada.Text = "Fecha Seleccionada: " + DateTime.Now.ToString("dd/MM/yyyy");
                Session.Add("fecha_historial_merma_desperdicio_local", DateTime.Now);
                gridview_consumo.DataSource = historial.get_desperdicio_merma_local(sucursal.Rows[0]["id"].ToString(), DateTime.Now,dropdown_categoria.SelectedItem.Text);
                gridview_consumo.DataBind();
            }
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha_seleccionada.Text = "Fecha Seleccionada: " + calendario.SelectedDate.ToString("dd/MM/yyyy");
            Session.Add("fecha_historial_merma_desperdicio_local", calendario.SelectedDate);
            gridview_consumo.DataSource = historial.get_desperdicio_merma_local(sucursal.Rows[0]["id"].ToString(), calendario.SelectedDate, dropdown_categoria.SelectedItem.Text);
            gridview_consumo.DataBind();
        }

        protected void dropdown_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime fecha = (DateTime)Session["fecha_historial_merma_desperdicio_local"];
            gridview_consumo.DataSource = historial.get_desperdicio_merma_local(sucursal.Rows[0]["id"].ToString(), fecha, dropdown_categoria.SelectedItem.Text);
            gridview_consumo.DataBind();
        }
    }
}