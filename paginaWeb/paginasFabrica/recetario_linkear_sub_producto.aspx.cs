using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class recetario_linkear_sub_producto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void boton_crear_sub_producto_Click(object sender, EventArgs e)
        {
            Response.Redirect("/paginasFabrica/recetario_crear_sub_producto.aspx");
        }
    }
}