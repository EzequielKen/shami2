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
    public partial class historial_orden_de_compras : System.Web.UI.Page
    {
        #region PDF
        private void crear_pdf_con_precio(string id_orden, string proveedor)
        {

            string id_factura = cuentas_Por_Pagar.get_id(id_orden, proveedor);
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Orden de Compra id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            cuentas_Por_Pagar.crear_PDF_orden_de_compra_con_precio(ruta_archivo, imgdata, id_factura, id_orden);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }
        private void crear_pdf_sin_precio(string id)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Orden de Compra id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            historial.crear_PDF_orden_de_compra(ruta_archivo, imgdata, id);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }
        #endregion
        #region consultas
        private void cargar_nota(string id_orden, string nota)
        {
            if (nota != string.Empty)
            {
                historial.cargar_nota(id_orden, nota);
            }
        }
        private void cancelar_orden_de_compra(string id_orden)
        {
            historial.cancelar_orden_de_compra(id_orden);
            consultar_ordenes_de_compra_de_proveedor();
            cargar_ordenes();
        }
        private void consultar_ordenes_de_compra_de_proveedor()
        {
            ordenes_de_compra_de_proveedorBD = historial.get_ordenes_de_compra(dropDown_mes.SelectedItem.Text,dropDown_año.SelectedItem.Text);
            Session.Add("ordenes_de_compra_de_proveedorBD", ordenes_de_compra_de_proveedorBD);
        }
        #endregion


        #region carga productos
        private void crear_tabla_ordenes()
        {
            ordenes_de_compra_de_proveedor = new DataTable();
            ordenes_de_compra_de_proveedor.Columns.Add("id", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("proveedor", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("estado", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("fecha", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("fecha_entrega_estimada", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("fecha_entrega", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("condicion_pago", typeof(string));
            ordenes_de_compra_de_proveedor.Columns.Add("nota", typeof(string));
        }

        private void llenar_tabla_ordenes()
        {
            crear_tabla_ordenes();
            ordenes_de_compra_de_proveedorBD.DefaultView.Sort = "fecha DESC, id DESC";
            ordenes_de_compra_de_proveedorBD = ordenes_de_compra_de_proveedorBD.DefaultView.ToTable();
            DateTime fecha_entrega;
            int fila_ordenes = 0;
            for (int fila = 0; fila <= ordenes_de_compra_de_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString()) &&
                    funciones.verificar_fecha(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text) &&
                    verificar_estado(ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString()))
                {
                    ordenes_de_compra_de_proveedor.Rows.Add();

                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["id"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["id"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["proveedor"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["estado"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha"].ToString()).ToString("dd/MM/yyyy");
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega_estimada"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega_estimada"].ToString()).ToString("dd/MM/yyyy"); ;
                    if (DateTime.TryParse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega"].ToString(), out fecha_entrega))
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = fecha_entrega.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = "N/A";
                    }
                    if ("N/A" != ordenes_de_compra_de_proveedorBD.Rows[fila]["condicion_pago"].ToString())
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["condicion_pago"] = "A " + ordenes_de_compra_de_proveedorBD.Rows[fila]["condicion_pago"].ToString() + " dias";
                    }
                    else
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["condicion_pago"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["condicion_pago"].ToString();
                    }
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["nota"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["nota"].ToString();
                    fila_ordenes++;
                }
            }
        }

        private void llenar_tabla_ordenes_abiertas()
        {
            crear_tabla_ordenes();
            ordenes_de_compra_de_proveedorBD.DefaultView.Sort = "fecha DESC, id DESC";
            ordenes_de_compra_de_proveedorBD = ordenes_de_compra_de_proveedorBD.DefaultView.ToTable();
            DateTime fecha_entrega;
            int fila_ordenes = 0;
            for (int fila = 0; fila <= ordenes_de_compra_de_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString()) &&
                    verificar_estado_de_pedido(ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString()))
                {
                    ordenes_de_compra_de_proveedor.Rows.Add();

                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["id"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["id"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["proveedor"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["estado"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha"].ToString()).ToString("dd/MM/yyyy");
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega_estimada"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega_estimada"].ToString()).ToString("dd/MM/yyyy"); ;
                    if (DateTime.TryParse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega"].ToString(), out fecha_entrega))
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = fecha_entrega.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = "N/A";
                    }
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["nota"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["nota"].ToString();
                    fila_ordenes++;
                }
            }
        }
        private bool verificar_estado_de_pedido(string estado)
        {
            bool retorno = false;
            if (estado == "Abierta" || estado == "Entrega Parcial")
            {
                retorno = true;
            }
            return retorno;
        }
        private void llenar_tabla_ordenes_recibidas()
        {
            crear_tabla_ordenes();
            ordenes_de_compra_de_proveedorBD.DefaultView.Sort = "fecha_entrega DESC, id DESC";
            ordenes_de_compra_de_proveedorBD = ordenes_de_compra_de_proveedorBD.DefaultView.ToTable();
            DateTime fecha_entrega;
            int fila_ordenes = 0;
            for (int fila = 0; fila <= ordenes_de_compra_de_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString()) &&
                    ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString() == "Recibido")
                {
                    ordenes_de_compra_de_proveedor.Rows.Add();

                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["id"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["id"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["proveedor"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["proveedor"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["estado"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["estado"].ToString();
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha"].ToString()).ToString("dd/MM/yyyy");
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega_estimada"] = DateTime.Parse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega_estimada"].ToString()).ToString("dd/MM/yyyy"); ;
                    if (DateTime.TryParse(ordenes_de_compra_de_proveedorBD.Rows[fila]["fecha_entrega"].ToString(), out fecha_entrega))
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = fecha_entrega.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["fecha_entrega"] = "N/A";
                    }
                    ordenes_de_compra_de_proveedor.Rows[fila_ordenes]["nota"] = ordenes_de_compra_de_proveedorBD.Rows[fila]["nota"].ToString();
                    fila_ordenes++;
                }
            }
        }
        private bool verificar_estado(string estado)
        {
            bool retorno = false;
            if (DropDown_estado.SelectedItem.Text == "Todos")
            {
                retorno = true;
            }
            else if (DropDown_estado.SelectedItem.Text == "Abierta" || DropDown_estado.SelectedItem.Text == "Entrega Parcial")
            {
                retorno = true;
            }
            else if (DropDown_estado.SelectedItem.Text == estado)
            {
                retorno = true;
            }
            return retorno;
        }
        private void cargar_ordenes()
        {
            ordenes_de_compra_de_proveedorBD = (DataTable)Session["ordenes_de_compra_de_proveedorBD"];
            llenar_tabla_ordenes();

            gridView_ordenes.DataSource = ordenes_de_compra_de_proveedor;
            gridView_ordenes.DataBind();
        }

        private void cargar_ordenes_abiertas()
        {
            ordenes_de_compra_de_proveedorBD = (DataTable)Session["ordenes_de_compra_de_proveedorBD"];
            llenar_tabla_ordenes_abiertas();

            gridView_ordenes.DataSource = ordenes_de_compra_de_proveedor;
            gridView_ordenes.DataBind();
        }
        private void cargar_ordenes_recibidas()
        {
            ordenes_de_compra_de_proveedorBD = (DataTable)Session["ordenes_de_compra_de_proveedorBD"];
            llenar_tabla_ordenes_recibidas();

            gridView_ordenes.DataSource = ordenes_de_compra_de_proveedor;
            gridView_ordenes.DataBind();
        }
        #endregion


        #region configurar controles
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            //   cargar_proveedores();
            cargar_mes();
            cargar_año();
        }
        private void cargar_proveedores()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            proveedores_de_fabrica = (DataTable)Session["proveedores_de_fabrica"];

            for (int fila = 0; fila <= proveedores_de_fabrica.Rows.Count - 1; fila++)
            {

                item = new System.Web.UI.WebControls.ListItem(proveedores_de_fabrica.Rows[fila]["proveedor"].ToString(), num_item.ToString());

                // dropDown_proveedores.Items.Add(item);


                num_item++;

            }
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
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_orden_de_compras historial;
        cls_cuentas_por_pagar cuentas_Por_Pagar;
        cls_funciones funciones = new cls_funciones();
        cls_landing_page landing;
        DataTable tipo_usuarioBD;
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;

        DataTable proveedores_de_fabrica;
        DataTable ordenes_de_compra_de_proveedorBD;
        DataTable ordenes_de_compra_de_proveedor;
        string proveedores_de_fabrica_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];

            landing = new cls_landing_page((DataTable)Session["usuariosBD"]);
            if (landing.verificar_si_registro("5") ||
                "Shami Villa Maipu Expedicion" != tipo_usuarioBD.Rows[0]["rol"].ToString())
            {
                Session.Remove("num_orden_de_compra_seleccionada");
                usuariosBD = (DataTable)Session["usuariosBD"];
                proveedorBD = (DataTable)Session["proveedorBD"];
                tipo_usuario = (DataTable)Session["tipo_usuario"];
             
                cuentas_Por_Pagar = new cls_cuentas_por_pagar(usuariosBD);
             
                historial = new cls_historial_orden_de_compras(usuariosBD);
                if (!IsPostBack)
                {
                    configurar_controles();
                    consultar_ordenes_de_compra_de_proveedor();
                    cargar_ordenes();
                }


                if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                {
                    gridView_ordenes.Columns[6].Visible = false;
                    gridView_ordenes.Columns[8].Visible = false;
                    gridView_ordenes.Columns[9].Visible = false;
                }
            }
            else
            {
                Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);
            }

        }

        protected void dropDown_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {

            cargar_ordenes();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
                    consultar_ordenes_de_compra_de_proveedor();

            cargar_ordenes();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            consultar_ordenes_de_compra_de_proveedor();

            cargar_ordenes();
        }
        protected void DropDown_estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_ordenes();
        }
        protected void gridView_ordenes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "confirmar_pedido")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_ordenes.Rows[int.Parse(index)].Cells[0].Text;

                Session.Add("proveedores_de_fabrica_seleccionado", gridView_ordenes.Rows[int.Parse(index)].Cells[1].Text);
                Session.Add("estado_pedido", gridView_ordenes.Rows[int.Parse(index)].Cells[7].Text);
                Session.Add("num_orden_de_compra_seleccionada", id);
                Response.Redirect("/paginasFabrica/cargar_orden_de_compra.aspx", false);

            }
            else if (e.CommandName == "crear_pdf")//cancelar_pedido
            {
                if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                    (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Compras" ||
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Admin" ||
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Administrativo"))
                {
                    string index = e.CommandArgument.ToString();

                    string id_factura = gridView_ordenes.Rows[int.Parse(index)].Cells[0].Text;
                    string proveedor = gridView_ordenes.Rows[int.Parse(index)].Cells[1].Text;
                    //crear_pdf_sin_precio(id);
                    if (gridView_ordenes.Rows[int.Parse(index)].Cells[7].Text == "Recibido" ||
                        gridView_ordenes.Rows[int.Parse(index)].Cells[7].Text == "Entrega Parcial")
                    {
                        crear_pdf_con_precio(id_factura, proveedor);
                    }
                    else
                    {
                        crear_pdf_sin_precio(id_factura);
                    }

                }
                else
                {
                    string index = e.CommandArgument.ToString();

                    string id = gridView_ordenes.Rows[int.Parse(index)].Cells[0].Text;
                    crear_pdf_sin_precio(id);
                }

            }
            else if (e.CommandName == "cancelar_pedido")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_ordenes.Rows[int.Parse(index)].Cells[0].Text;
                //cancelar pedido
                cancelar_orden_de_compra(id);
                cargar_ordenes();

            }
            else if (e.CommandName == "cargar_nota")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_ordenes.Rows[int.Parse(index)].Cells[0].Text;
                TextBox textbox_nota = (gridView_ordenes.Rows[int.Parse(index)].Cells[9].FindControl("textbox_nota") as TextBox);

                //cancelar pedido
                cargar_nota(id, textbox_nota.Text);
                cargar_ordenes();

            }
        }
        protected void gridView_ordenes_DataBound(object sender, EventArgs e)
        {
            for (int fila = 0; fila <= gridView_ordenes.Rows.Count - 1; fila++)
            {
                if ("Recibido" == gridView_ordenes.Rows[fila].Cells[7].Text)
                {
                    gridView_ordenes.Rows[fila].Cells[8].Controls[0].Visible = false;
                    if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                        tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
                    {
                        gridView_ordenes.Rows[fila].Cells[6].Controls[0].Visible = false;
                    }
                }
                else if ("Cancelada" == gridView_ordenes.Rows[fila].Cells[7].Text)
                {
                    gridView_ordenes.Rows[fila].Cells[6].Controls[0].Visible = false;
                    gridView_ordenes.Rows[fila].Cells[8].Controls[0].Visible = false;
                }
                else if ("Entrega Parcial" == gridView_ordenes.Rows[fila].Cells[7].Text)
                {
                    gridView_ordenes.Rows[fila].Cells[8].Controls[0].Visible = false;
                }
            }
        }

        protected void boton_solo_abiertas_Click(object sender, EventArgs e)
        {
            cargar_ordenes_abiertas();
        }

        protected void boton_solo_recibidas_Click(object sender, EventArgs e)
        {
            cargar_ordenes_recibidas();
        }
    }
}