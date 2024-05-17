using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasMaestras
{
    public partial class paginaMaestraCarrefour : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable usuariosBD = (DataTable)Session["usuariosBD"];
            label_sucursal.Text = usuariosBD.Rows[0]["usuario"].ToString();
        }
    }
}