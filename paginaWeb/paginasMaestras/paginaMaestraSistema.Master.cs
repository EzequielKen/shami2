using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb
{
    public partial class paginaMaestra : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable sucursal = (DataTable)Session["sucursal"];
                label_sucursal.Text = sucursal.Rows[0]["sucursal"].ToString();

            }
            catch (Exception)
            {

                Response.Redirect("/paginas/login.aspx", false);
            }
            
        }
    }
}