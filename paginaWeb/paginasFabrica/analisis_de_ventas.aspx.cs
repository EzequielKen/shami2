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
    public partial class analisis_de_ventas : System.Web.UI.Page
    {
        cls_analisis_de_ventas analisis;
        DataTable usuariosBD;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            analisis = new cls_analisis_de_ventas(usuariosBD);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + "analisis- id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            //analisis.crear_pdf(ruta_archivo,imgdata);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

            }
            catch (Exception)
            {

                Response.Redirect(strUrl, false);
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {

        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}