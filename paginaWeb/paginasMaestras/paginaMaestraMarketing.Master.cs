using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasMaestras
{
    public partial class paginaMaestraMarketing : System.Web.UI.MasterPage
    {
        DataTable tipo_usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            tipo_usuario = (DataTable)Session["tipo_usuario"];

            label_tipo_usuario.Text = tipo_usuario.Rows[0]["rol"].ToString();
        }
    }
}