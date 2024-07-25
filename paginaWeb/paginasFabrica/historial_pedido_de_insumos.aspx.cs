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
    public partial class historial_pedido_de_insumos : System.Web.UI.Page
    {
        #region cargar resumen pedido
        private void mostrar_ocultar_boton_oc()
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];
            if (resumen_orden_pedido.Rows.Count > 0)
            {
                boton_crear_oc.Visible = true;
            }
            else
            {
                boton_crear_oc.Visible = false;
            }
        }
        private void crear_resumen_pedido()
        {
            resumen_orden_pedido = new DataTable();
            resumen_orden_pedido.Columns.Add("id", typeof(string));
            resumen_orden_pedido.Columns.Add("producto", typeof(string));
            resumen_orden_pedido.Columns.Add("cantidad_pedida", typeof(string));

            Session.Add("resumen_orden_pedido", resumen_orden_pedido);
        }

        private void cargar_producto_en_resumen(string id, string producto, string cantidad_pedida)
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];
            if (!funciones.verificar_si_cargo(id, resumen_orden_pedido))
            {
                resumen_orden_pedido.Rows.Add();
                int ultima_fila = resumen_orden_pedido.Rows.Count - 1;

                resumen_orden_pedido.Rows[ultima_fila]["id"] = id;
                resumen_orden_pedido.Rows[ultima_fila]["producto"] = producto;
                resumen_orden_pedido.Rows[ultima_fila]["cantidad_pedida"] = cantidad_pedida;
                Session.Add("resumen_orden_pedido", resumen_orden_pedido);
            }
            mostrar_ocultar_boton_oc();

        }
        private void eliminar_producto_de_resumen_orden_pedido(string id)
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];

            int fila_producto = funciones.buscar_fila_por_id(id, resumen_orden_pedido);

            resumen_orden_pedido.Rows[fila_producto].Delete();
            Session.Add("resumen_orden_pedido", resumen_orden_pedido);
            mostrar_ocultar_boton_oc();


        }
        private void cargar_resumen_orden_pedido()
        {
            gridView_resumen_orden_pedido.DataSource = (DataTable)Session["resumen_orden_pedido"];
            gridView_resumen_orden_pedido.DataBind();
        }
        #endregion
        #region cargar resumen
        private void cargar_resumen(string id_pedido)
        {
            Session.Add("resumen_pedido_insumo_a_expedicion", historial_pedido_insumos.get_resumen_de_pedido(id_pedido));
            gridView_resumen.DataSource = (DataTable)Session["resumen_pedido_insumo_a_expedicion"];
            gridView_resumen.DataBind();
            boton_PDF.Visible=true;
        }
        #endregion
        #region carga de ordenes
        private void crear_tabla_pedido()
        {
            orden_de_pedido = new DataTable();

            orden_de_pedido.Columns.Add("id", typeof(string));
            orden_de_pedido.Columns.Add("solicita", typeof(string));
            orden_de_pedido.Columns.Add("estado", typeof(string));
            orden_de_pedido.Columns.Add("fecha", typeof(string));

        }
        private void llenar_tabla_pedido()
        {
            crear_tabla_pedido();
            int fila_pedido = 0;
            for (int fila = 0; fila <= orden_de_pedidoBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(orden_de_pedidoBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    orden_de_pedido.Rows.Add();

                    orden_de_pedido.Rows[fila_pedido]["id"] = orden_de_pedidoBD.Rows[fila]["id"].ToString();
                    orden_de_pedido.Rows[fila_pedido]["solicita"] = orden_de_pedidoBD.Rows[fila]["solicita"].ToString(); 
                    orden_de_pedido.Rows[fila_pedido]["estado"] = orden_de_pedidoBD.Rows[fila]["estado"].ToString(); 
                    orden_de_pedido.Rows[fila_pedido]["fecha"] = orden_de_pedidoBD.Rows[fila]["fecha"].ToString();

                    fila_pedido++;
                }
            }
        }

        private void cargar_pedido()
        {
            llenar_tabla_pedido();
            gridView_pedidos.DataSource = orden_de_pedido;
            gridView_pedidos.DataBind();

            gridView_resumen.DataSource = null;
            gridView_resumen.DataBind();
        }
        #endregion
        #region configurar controles

        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();
        }
        private void cargar_mes()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                dropDown_mes.Items.Add(item);
                num_item++;
            }
            dropDown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                dropDown_año.Items.Add(item);
                num_item++;
            }
            string añ = DateTime.Now.Year.ToString();

            dropDown_año.SelectedIndex = dropDown_año.Items.Count - 1;
        }

        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_historial_de_pedido_de_insumos historial_pedido_insumos;

        cls_funciones funciones = new cls_funciones();
        cls_landing_page landing;
        DataTable tipo_usuarioBD;
        DataTable usuariosBD;
        DataTable tipo_usuario;

        string tipo_seleccionado;
        string id_proveedor_fabrica_seleccionado;
        DataTable resumen_orden_pedido;
        DataTable orden_de_pedidoBD;
        DataTable orden_de_pedido;
        protected void Page_Load(object sender, EventArgs e)
        {
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];

            landing = new cls_landing_page((DataTable)Session["usuariosBD"]);
            if (landing.verificar_si_registro("7") ||
                "Shami Villa Maipu Expedicion" != tipo_usuarioBD.Rows[0]["rol"].ToString())
            {
                usuariosBD = (DataTable)Session["usuariosBD"];
                tipo_usuario = (DataTable)Session["tipo_usuario"];
               
                historial_pedido_insumos = new cls_historial_de_pedido_de_insumos(usuariosBD);
                orden_de_pedidoBD = historial_pedido_insumos.get_orden_de_pedido();
                if (!IsPostBack)
                {
                    Session.Remove("id_pedido_insumo_seleccionado");
                    crear_resumen_pedido();
                    configurar_controles();
                    cargar_pedido();
                }
                mostrar_ocultar_boton_oc();
            }
            else
            {
                Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);
            }
        }
        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedido();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedido();
        }

        protected void gridView_pedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "abrir_pedido")
            {
                string gridview_pedidos_index = e.CommandArgument.ToString();

                cargar_resumen(gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);

                Session.Add("id_pedido_insumo_seleccionado", gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);

            }
            else if (e.CommandName == "cargar_entrega")
            {
                string gridview_pedidos_index = e.CommandArgument.ToString();

                Session.Add("id_pedido_insumo_seleccionado", gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);

                if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
                {
                    Response.Redirect("/paginasFabrica/entrega_de_insumos.aspx", false);
                }
                else if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                {
                    Response.Redirect("/paginasFabrica/recepcion_de_insumos.aspx", false);
                }
                
            }
            else if (e.CommandName == "eliminar")
            {
                string gridview_pedidos_index = e.CommandArgument.ToString();
                historial_pedido_insumos.desactivar_pedido(gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);
                orden_de_pedidoBD = historial_pedido_insumos.get_orden_de_pedido();
                cargar_pedido();
            }
        }

        protected void gridView_pedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_pedidos.Rows.Count - 1; fila++)
            {
                if ("Solicitado" == gridView_pedidos.Rows[fila].Cells[2].Text &&
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
                {
                    gridView_pedidos.Rows[fila].Cells[5].Controls[0].Visible = true;
                }
                else if ("Despachado" == gridView_pedidos.Rows[fila].Cells[2].Text &&
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                {
                    gridView_pedidos.Rows[fila].Cells[5].Controls[0].Visible = true;
                }
                else 
                {
                    gridView_pedidos.Rows[fila].Cells[5].Controls[0].Visible = false;
                    gridView_pedidos.Rows[fila].Cells[6].Controls[0].Visible = false;
                }
            }
        }

        protected void gridView_resumen_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }



        protected void gridView_resumen_orden_pedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }




        protected void boton_crear_oc_Click(object sender, EventArgs e)
        {
        }

        protected void gridView_resumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void boton_PDF_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " pedido-" + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            historial_pedido_insumos.crear_pdf(ruta_archivo,imgdata, (DataTable)Session["resumen_pedido_insumo_a_expedicion"]); //crear_pdf();

            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }
    }
}