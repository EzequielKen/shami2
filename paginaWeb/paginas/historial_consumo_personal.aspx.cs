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
    public partial class historial_consumo_personal : System.Web.UI.Page
    {
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_consumo_personal historial_consumo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            historial_consumo = new cls_historial_consumo_personal(usuariosBD);
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            gridview_consumo.DataSource = historial_consumo.get_consumo(sucursal.Rows[0]["id"].ToString(), calendario.SelectedDate);
            gridview_consumo.DataBind();
        }
    }
}