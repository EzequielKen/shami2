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
    public partial class recepcion_de_fabrica_fatay : System.Web.UI.Page
    {
        #region metodos

        private void cargar_detalle(string id_historial, string nombre_proveedor)
        {
            gridview_detalle_produccion.DataSource = recepcion_fatay.get_detalle_produccion(id_historial, nombre_proveedor);
            gridview_detalle_produccion.DataBind();
        }
        private void crear_tabla_historial()
        {
            historial_despacho = new DataTable();

            historial_despacho.Columns.Add("id", typeof(string));
            historial_despacho.Columns.Add("fecha", typeof(string));
            historial_despacho.Columns.Add("proveedor", typeof(string));
            historial_despacho.Columns.Add("receptor", typeof(string));
            historial_despacho.Columns.Add("estado", typeof(string));

        }

        private void llenar_tabla_historial()
        {
            crear_tabla_historial();
            fechaBD = (DateTime)Session["fechaBD"];
            int fila_historial = 0;
            for (int fila = 0; fila <= historial_despachoBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha_completa(historial_despachoBD.Rows[fila]["fecha"].ToString(), fechaBD.Day.ToString(), fechaBD.Month.ToString(), fechaBD.Year.ToString()))
                {
                    historial_despacho.Rows.Add();

                    historial_despacho.Rows[fila_historial]["id"] = historial_despachoBD.Rows[fila]["id"].ToString();
                    historial_despacho.Rows[fila_historial]["fecha"] = historial_despachoBD.Rows[fila]["fecha"].ToString();
                    historial_despacho.Rows[fila_historial]["proveedor"] = historial_despachoBD.Rows[fila]["proveedor"].ToString();
                    historial_despacho.Rows[fila_historial]["receptor"] = historial_despachoBD.Rows[fila]["receptor"].ToString();
                    historial_despacho.Rows[fila_historial]["estado"] = historial_despachoBD.Rows[fila]["estado"].ToString();

                    fila_historial++;
                }
            }
        }
        private void cargar_historial()
        {
            llenar_tabla_historial();
            gridview_historial.DataSource = historial_despacho;
            gridview_historial.DataBind();
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_recepcion_de_fabrica_fatay recepcion_fatay;
        cls_funciones funciones = new cls_funciones();
        cls_landing_page landing;
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuarioBD;

        DataTable historial_despachoBD;
        DataTable historial_despacho;
        DateTime fechaBD;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];

            landing = new cls_landing_page(usuariosBD);
            if (landing.verificar_si_registro("2") ||
                "Shami Villa Maipu Expedicion" != tipo_usuarioBD.Rows[0]["rol"].ToString())
            {
                recepcion_fatay = new cls_recepcion_de_fabrica_fatay(usuariosBD);
                historial_despachoBD = recepcion_fatay.get_todo_historial_de_despacho();
                if (!IsPostBack)
                {

                    label_fecha.Text = DateTime.Now.ToString();//.ToString("yyyy-MM-dd HH:mm:ss")
                    Session.Add("fechaBD", DateTime.Now);
                    fechaBD = (DateTime)Session["fechaBD"];
                    cargar_historial();
                }
                fechaBD = (DateTime)Session["fechaBD"];
            }
            else
            {
                Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);

            }

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

                recepcion_fatay.crear_pdf_historial_produccion(ruta_archivo, imgdata, id, proveedorBD.Rows[0]["nombre_en_BD"].ToString()); //crear_pdf();

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
                cargar_detalle(id, proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            }
            else if (e.CommandName == "confirmar")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;

                Session.Add("id_historial", id);
                Response.Redirect("/paginasFabrica/recepcion_de_despacho_fatay.aspx", false);
            }
            else if (e.CommandName == "cancelar")
            {
                string gridview_historial_index = e.CommandArgument.ToString();

                string id = gridview_historial.Rows[int.Parse(gridview_historial_index)].Cells[0].Text;
                recepcion_fatay.cancelar_produccion(id);
                if ("Shami Villa Maipu Produccion" == tipo_usuarioBD.Rows[0]["rol"].ToString())
                {
                    //historial_produccionBD = historial_produccion_cls.get_historial_produccion_proveedor_cliente(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), tipo_usuarioBD.Rows[0]["rol"].ToString(), "Shami Villa Maipu Expedicion");
                    historial_despachoBD = recepcion_fatay.get_todo_historial_de_produccion_segun_fabrica(proveedorBD.Rows[0]["nombre_en_BD"].ToString());

                }
                else
                {
                    //historial_produccionBD = historial_produccion_cls.get_historial_produccion_proveedor_cliente(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), "Shami Villa Maipu Produccion", tipo_usuarioBD.Rows[0]["rol"].ToString());
                    historial_despachoBD = recepcion_fatay.get_todo_historial_de_despacho();
                }
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