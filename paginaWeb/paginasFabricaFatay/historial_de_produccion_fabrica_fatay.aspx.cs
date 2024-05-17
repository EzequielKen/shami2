using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabricaFatay
{
    public partial class historial_de_produccion_fabrica_fatay : System.Web.UI.Page
    {
        #region metodos

        private void cargar_detalle(string id_historial, string nombre_proveedor)
        {
            gridview_detalle_produccion.DataSource = historial_produccion_cls.get_detalle_produccion(id_historial, nombre_proveedor);
            gridview_detalle_produccion.DataBind();
        }
        private void crear_tabla_historial()
        {
            historial_produccion = new DataTable();

            historial_produccion.Columns.Add("id", typeof(string));
            historial_produccion.Columns.Add("fecha", typeof(string));
            historial_produccion.Columns.Add("proveedor", typeof(string));
            historial_produccion.Columns.Add("receptor", typeof(string));
            historial_produccion.Columns.Add("estado", typeof(string));

        }

        private void llenar_tabla_historial()
        {
            crear_tabla_historial();
            fechaBD = (DateTime)Session["fechaBD"];
            int fila_historial = 0;
            for (int fila = 0; fila <= historial_produccionBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha_completa(historial_produccionBD.Rows[fila]["fecha"].ToString(), fechaBD.Day.ToString(), fechaBD.Month.ToString(), fechaBD.Year.ToString()))
                {
                    historial_produccion.Rows.Add();

                    historial_produccion.Rows[fila_historial]["id"] = historial_produccionBD.Rows[fila]["id"].ToString();
                    historial_produccion.Rows[fila_historial]["fecha"] = historial_produccionBD.Rows[fila]["fecha"].ToString();
                    historial_produccion.Rows[fila_historial]["proveedor"] = historial_produccionBD.Rows[fila]["proveedor"].ToString();
                    historial_produccion.Rows[fila_historial]["receptor"] = historial_produccionBD.Rows[fila]["receptor"].ToString();
                    historial_produccion.Rows[fila_historial]["estado"] = historial_produccionBD.Rows[fila]["estado"].ToString();

                    fila_historial++;
                }
            }
        }
        private void cargar_historial()
        {
            llenar_tabla_historial();
            gridview_historial.DataSource = historial_produccion;
            gridview_historial.DataBind();
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_de_produccion historial_produccion_cls;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuarioBD;

        DataTable historial_produccionBD;
        DataTable historial_produccion;
        DateTime fechaBD;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];

            if (Session["historial_produccion"] == null)
            {
                Session.Add("historial_produccion", new cls_historial_de_produccion(usuariosBD));
            }
            historial_produccion_cls = (cls_historial_de_produccion)Session["historial_produccion"];
          
            historial_produccionBD = historial_produccion_cls.get_todo_historial_de_produccion_segun_fabrica("Fabrica Fatay Callao");
            if (!IsPostBack)
            {

                label_fecha.Text = DateTime.Now.ToString();//.ToString("yyyy-MM-dd HH:mm:ss")
                Session.Add("fechaBD", DateTime.Now);
                fechaBD = (DateTime)Session["fechaBD"];
                cargar_historial();
            }
            fechaBD = (DateTime)Session["fechaBD"];
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            Session.Add("fechaBD", calendario.SelectedDate);
            fechaBD = (DateTime)Session["fechaBD"];
            label_fecha.Text = fechaBD.ToString();
            gridview_detalle_produccion.DataSource = null;
            gridview_detalle_produccion.DataBind();
            cargar_historial();
        }



        protected void gridview_historial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "crear_pdf")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;
                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = Session["sucursal"].ToString() + " id-" + id + "- -" + dato_hora + ".pdf";
                string ruta = "~/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

                historial_produccion_cls.crear_pdf_historial_produccion(ruta_archivo, imgdata, id, "proveedor_villaMaipu"); //crear_pdf();

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();
            }
            else if (e.CommandName == "abrir")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;
                label_id.Text = "ID: " + gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;
                label_fecha_seleccionada.Text = "Fecha: " + gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[1].Text;
                label_proveedor.Text = "Despacha: " + gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[2].Text;
                label_receptor.Text = "Recibe: " + gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[3].Text;
                label_estado.Text = "Estado: " + gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[4].Text;
                cargar_detalle(id, "proveedor_villaMaipu");
            }
            else if (e.CommandName == "confirmar")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;

                Session.Add("id_historial", id);
                Response.Redirect("/paginasFabrica/recepcion_de_produccion.aspx", false);
            }
            else if (e.CommandName == "cancelar")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;
                historial_produccion_cls.cancelar_produccion(id);
                historial_produccionBD = historial_produccion_cls.get_todo_historial_de_produccion_segun_fabrica("Fabrica Fatay Callao");
                cargar_historial();
            }
        }

        protected void gridview_historial_DataBound(object sender, EventArgs e)
        {
            for (int fila = 0; fila <= gridview_historial.Rows.Count - 1; fila++)
            {
                if ("Despachado" == gridview_historial.Rows[fila].Cells[4].Text && "Shami Villa Maipu Expedicion" == tipo_usuarioBD.Rows[0]["rol"].ToString())
                {
                    gridview_historial.Rows[fila].Cells[7].Controls[0].Visible = true;
                }

                else if ("Recibido" == gridview_historial.Rows[fila].Cells[4].Text || "Shami Villa Maipu Produccion" == tipo_usuarioBD.Rows[0]["rol"].ToString())
                {
                    gridview_historial.Rows[fila].Cells[7].Controls[0].Visible = false;
                }

                if ("Recibido" == gridview_historial.Rows[fila].Cells[4].Text)
                {
                    gridview_historial.Rows[fila].Cells[8].Controls[0].Visible = false;
                }

            }
        }
    }
}