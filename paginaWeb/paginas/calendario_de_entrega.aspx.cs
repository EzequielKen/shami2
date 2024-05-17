using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class calendario_de_entrega : System.Web.UI.Page
    {
        #region atributos
        cls_dia_de_entrega calendario_entrega;
        DataTable usuariosBD;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];

            calendario_entrega = new cls_dia_de_entrega(usuariosBD);
            
            gridview_dias_de_entrega.DataSource = calendario_entrega.get_dias_de_entrega();
            gridview_dias_de_entrega.DataBind();
        }
    }
}