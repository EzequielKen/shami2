using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasMaestras
{
    public partial class paginaMaestraFabrica : System.Web.UI.MasterPage
    {
        public int seguridad;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                seguridad = int.Parse(Session["nivel_seguridad"].ToString());
                DataTable proveedorBD = (DataTable)Session["proveedorBD"];
                DataTable tipo_usuario = (DataTable)Session["tipo_usuario"];
                if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Fabrica Fatay")
                {
                    label_proveedor.Text = tipo_usuario.Rows[0]["tipo"].ToString();
                }
                else
                {
                    label_proveedor.Text = proveedorBD.Rows[0]["nombre_proveedor"].ToString() + " - " + tipo_usuario.Rows[0]["tipo"].ToString();
                }
            }
            catch (Exception)
            {

                Response.Redirect("/paginas/login.aspx", false);
            }
            
        }
    }
}