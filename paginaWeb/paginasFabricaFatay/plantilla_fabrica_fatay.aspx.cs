using _05___sistemas_fabrica_fatay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabricaFatay
{
    public partial class plantilla_fabrica_fatay : System.Web.UI.Page
    {
        private void crear_pdf_plantilla_stock()
        {

            DateTime hora = DateTime.Now;

            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Plantilla stock -" + " - id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            plantilla.crear_PDF_plantilla_de_todos_los_productos(ruta_archivo, imgdata, proveedorBD.Rows[0]["nombre_en_BD"].ToString());

            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();*/

        }
        private void crear_pdf_plantilla_insumo()
        {

            DateTime hora = DateTime.Now;

            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Plantilla insumo -" + " - id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            plantilla.crear_PDF_plantilla_de_insuoms(ruta_archivo, imgdata);


            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();*/

        }
        private void crear_pdf_productos_terminados()
        {
            DateTime hora = DateTime.Now;

            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Plantilla insumo -" + " - id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            plantilla.crear_PDF_plantilla_de_productos_terminados(ruta_archivo, imgdata, proveedorBD.Rows[0]["nombre_en_BD"].ToString());


            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();*/

        }
        #region atributos
        cls_plantillas_fabrica_fatay plantilla;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            
            plantilla = new cls_plantillas_fabrica_fatay(usuariosBD);
        }

        protected void boton_plantilla_stock_Click(object sender, EventArgs e)
        {
            crear_pdf_plantilla_stock();
        }

        protected void boton_plantilla_insumo_Click(object sender, EventArgs e)
        {
            crear_pdf_plantilla_insumo();
        }

        protected void boton_productos_terminados_Click(object sender, EventArgs e)
        {
            crear_pdf_productos_terminados();
        }

        protected void boton_plantilla_producto_terminado_Click(object sender, EventArgs e)
        {
            crear_pdf_productos_terminados();
        }
    }
}