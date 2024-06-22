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
    public partial class remitos_de_carga : System.Web.UI.Page
    {
        private void crear_tabla_remitos()
        {
            remitos = new DataTable();

            remitos.Columns.Add("id", typeof(string));
            remitos.Columns.Add("sucursal", typeof(string));
            remitos.Columns.Add("num_pedido", typeof(string));
            remitos.Columns.Add("fecha_remito", typeof(string));
            remitos.Columns.Add("proveedor", typeof(string));

        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();

            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("sucursal", typeof(string));
            resumen.Columns.Add("num_pedido", typeof(string));
            resumen.Columns.Add("fecha_remito", typeof(string)); 
            resumen.Columns.Add("proveedor", typeof(string)); 
            Session.Add("resumen_de_PDF", resumen);
        }
        private void cargar_pedido_en_resumen(string id_pedido)
        {
            resumen = (DataTable)Session["resumen_de_PDF"];
            int fila_pedidos = funciones.buscar_fila_por_id(id_pedido, cuentas_por_pagar);
            int fila_resumen = funciones.buscar_fila_por_id(id_pedido, resumen);
            if (-1 == fila_resumen)
            {
                resumen.Rows.Add();
                int ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = cuentas_por_pagar.Rows[fila_pedidos]["id"].ToString();
                resumen.Rows[ultima_fila]["sucursal"] = cuentas_por_pagar.Rows[fila_pedidos]["sucursal"].ToString();
                resumen.Rows[ultima_fila]["num_pedido"] = cuentas_por_pagar.Rows[fila_pedidos]["num_pedido"].ToString();
                resumen.Rows[ultima_fila]["fecha_remito"] = cuentas_por_pagar.Rows[fila_pedidos]["fecha_remito"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = cuentas_por_pagar.Rows[fila_pedidos]["proveedor"].ToString();
                Session.Add("resumen_de_PDF", resumen);
            }
        }
        private void llenar_datatable_remitos()
        {
            crear_tabla_remitos();
            int fila_remito = 0;
            for (int fila = 0; fila <= cuentas_por_pagar.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha_completa(cuentas_por_pagar.Rows[fila]["fecha_remito"].ToString(),fecha.Day.ToString(), fecha.Month.ToString(), fecha.Year.ToString()))
                {
                    remitos.Rows.Add();

                    remitos.Rows[fila_remito]["id"] = cuentas_por_pagar.Rows[fila]["id"].ToString();
                    remitos.Rows[fila_remito]["sucursal"] = cuentas_por_pagar.Rows[fila]["sucursal"].ToString();
                    remitos.Rows[fila_remito]["num_pedido"] = cuentas_por_pagar.Rows[fila]["num_pedido"].ToString();
                    remitos.Rows[fila_remito]["fecha_remito"] = cuentas_por_pagar.Rows[fila]["fecha_remito"].ToString(); 
                    remitos.Rows[fila_remito]["proveedor"] = cuentas_por_pagar.Rows[fila]["proveedor"].ToString(); 

                    fila_remito++;
                }
            }
        }
        private void eliminar_pedido_en_resumen(string id_pedido)
        {
            resumen = (DataTable)Session["resumen_de_PDF"];
            int fila_resumen = funciones.buscar_fila_por_id(id_pedido, resumen);
            resumen.Rows[fila_resumen].Delete();
        }
        private void cargar_remitos()
        {
            llenar_datatable_remitos();
            gridview_remitos.DataSource = remitos;
            gridview_remitos.DataBind();

            resumen = (DataTable)Session["resumen_de_PDF"];
            gridview_resumen.DataSource = resumen;
            gridview_resumen.DataBind();
            if (0 < resumen.Rows.Count)
            {
                boton_pdf.Visible = true;
            }
            else
            {
                boton_pdf.Visible = false;
            }
        }
        private void generar_pdf()
        {

            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " pedido-" + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            cuentas_Por_Cobrar.crear_pdf_remito_de_carga(ruta_archivo, (DataTable)Session["resumen_de_PDF"], imgdata, (DateTime)Session["fecha"]); //crear_pdf();

            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }
        #region atributos
        cls_remitos_de_carga remitos_carga;
        cls_sistema_cuentas_por_cobrar cuentas_Por_Cobrar;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable cuentas_por_pagar;
        DataTable remitos;
        DataTable resumen;
        DateTime fecha;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            cuentas_Por_Cobrar = new cls_sistema_cuentas_por_cobrar(usuariosBD);
            if (Session["remitos_carga"] == null)
            {
                Session.Add("remitos_carga", new cls_remitos_de_carga(usuariosBD));
            }
            remitos_carga = (cls_remitos_de_carga)Session["remitos_carga"];

            cuentas_por_pagar = remitos_carga.get_cuentas_por_pagar();
            if (!IsPostBack)
            {
                Session.Add("fecha", DateTime.Now);
                crear_tabla_resumen();
            }
            fecha = (DateTime)Session["fecha"];
            label_fecha.Text = fecha.ToString();
            cargar_remitos();
        }
        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            Session.Add("fecha", calendario.SelectedDate);
            fecha = (DateTime)Session["fecha"];
            label_fecha.Text = fecha.ToString();

            cargar_remitos();
        }
        protected void gridview_remitos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 < int.Parse(gridview_remitos.SelectedRow.Cells[2].Text))
            {

                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = Session["sucursal"].ToString() + " pedido-" + gridview_remitos.SelectedRow.Cells[1].Text + "- id-" + dato_hora + ".pdf";
                string ruta = "~/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

                cuentas_Por_Cobrar.crear_pdf_remito_de_carga(ruta_archivo, (DataTable)Session["resumen_de_PDF"],imgdata, (DateTime)Session["fecha"]); //crear_pdf();

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();

            }
        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            generar_pdf();

        }

        protected void gridview_remitos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "boton_abrir")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                resumen = (DataTable)Session["resumen_de_PDF"];
                resumen.Rows.Clear();
                Session.Add("resumen_de_PDF", resumen);
                cargar_pedido_en_resumen(gridview_remitos.Rows[fila].Cells[0].Text);
                generar_pdf();

            }
            else if (e.CommandName == "boton_seleccionar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                cargar_pedido_en_resumen(gridview_remitos.Rows[fila].Cells[0].Text);
                cargar_remitos();
            }
        }

        protected void gridview_resumen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_eliminar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                eliminar_pedido_en_resumen(gridview_resumen.Rows[fila].Cells[0].Text);
                cargar_remitos();
            }
        }
    }
}