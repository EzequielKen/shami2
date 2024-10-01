using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class confirmar_conteo_stock : System.Web.UI.Page
    {
        private void cargar_conteo()
        {
            DateTime feca_seleccionada = (DateTime)Session["fecha_seleccionada"];
            conteo_stock = conteo.get_conteo_stock(feca_seleccionada);
            Session.Add("conteo_stock", conteo_stock);
            gridview_conteos.DataSource = conteo_stock;
            gridview_conteos.DataBind();
            label_fecha.Text = "Fecha Seleccionada: " + feca_seleccionada.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_confirmar_conteo_stock conteo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;

        DataTable conteo_stock;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            conteo = new cls_confirmar_conteo_stock(usuariosBD);
            if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
            {
                gridview_conteos.Columns[6].Visible = false;
                gridview_conteos.Columns[7].Visible = false;
                gridview_conteos.Columns[8].Visible = false;
                gridview_conteos.Columns[9].Visible = false;
                gridview_conteos.Columns[10].Visible = false;

            }
            if (!IsPostBack)
            {
                calendario.SelectedDate = DateTime.Now;
                Session.Add("fecha_seleccionada", calendario.SelectedDate);
                Session.Add("fecha_conteo_selecccionada", DateTime.Now.ToString("dd/MM/yyyy"));
                cargar_conteo();
            }
            conteo_stock = (DataTable)Session["conteo_stock"];
            if (conteo_stock.Rows.Count == 0)
            {
                boton_pdf.Visible = false;
            }
            else
            {
                boton_pdf.Visible = true;
            }
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha = calendario.SelectedDate;
            Session.Add("fecha_seleccionada", calendario.SelectedDate);
            Session.Add("fecha_conteo_selecccionada", fecha.ToString("dd/MM/yyyy"));
            cargar_conteo();
            conteo_stock = (DataTable)Session["conteo_stock"];
            if (conteo_stock.Rows.Count == 0)
            {
                boton_pdf.Visible = false;
            }
            else
            {
                boton_pdf.Visible = true;
            }
        }

        protected void boton_aprobar_Click(object sender, EventArgs e)
        {
            Button boton_aprobar = (Button)sender;
            GridViewRow fila = (GridViewRow)boton_aprobar.NamingContainer;
            string id = fila.Cells[0].Text;
            string rol_usuario = tipo_usuario.Rows[0]["rol"].ToString();
            string id_producto = fila.Cells[1].Text;
            string movimiento = fila.Cells[5].Text;
            TextBox nota = (TextBox)fila.FindControl("textbox_nota");
            conteo.aprobar_conteo(id, rol_usuario, id_producto, movimiento, nota.Text);
            cargar_conteo();
        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {
            Button boton_elimnar = (Button)sender;
            GridViewRow fila = (GridViewRow)boton_elimnar.NamingContainer;
            string id = fila.Cells[0].Text;
            conteo.eliminar_conteo(id);
            cargar_conteo();
        }

        protected void gridview_conteos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            conteo_stock = (DataTable)Session["conteo_stock"];
            string id;
            int fila_conteo;
            DateTime fecha = (DateTime)Session["fecha_seleccionada"];
            DateTime fecha_actual = DateTime.Now;
            for (int fila = 0; fila <= gridview_conteos.Rows.Count - 1; fila++)
            {

                TextBox textbox_nota = (gridview_conteos.Rows[fila].Cells[8].FindControl("textbox_nota") as TextBox);
                Button boton_aprobar = (gridview_conteos.Rows[fila].Cells[9].FindControl("boton_aprobar") as Button);
                Button boton_eliminar = (gridview_conteos.Rows[fila].Cells[10].FindControl("boton_eliminar") as Button);

                id = gridview_conteos.Rows[fila].Cells[0].Text;
                fila_conteo = funciones.buscar_fila_por_id(id, conteo_stock);
                if (conteo_stock.Rows[fila_conteo]["aprobado"].ToString() == "Si" ||
                    fecha.ToString("dd/MM/yyyy") != fecha_actual.ToString("dd/MM/yyyy") ||
                    conteo_stock.Rows[fila_conteo]["diferencia"].ToString() == "0")
                {
                    textbox_nota.Visible = false;
                    boton_aprobar.Visible = false;
                    boton_eliminar.Visible = false;
                }
                //diferencia
                if (conteo_stock.Rows[fila_conteo]["diferencia"].ToString() != "0")
                {
                    gridview_conteos.Rows[fila].CssClass = "table-danger";
                }
                else if (conteo_stock.Rows[fila_conteo]["diferencia"].ToString() == "0")
                {
                    gridview_conteos.Rows[fila].CssClass = "table-success";
                }
            }
        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;

            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "CONTEO DE STOCK" + " - id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            conteo.pdf_conteo(ruta_archivo, (DataTable)Session["conteo_stock"], imgdata, Session["fecha_conteo_selecccionada"].ToString());

            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();*/
        }
    }
}