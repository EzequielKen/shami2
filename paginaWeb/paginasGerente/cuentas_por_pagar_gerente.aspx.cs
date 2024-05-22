using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace paginaWeb.paginasGerente
{
    public partial class cuentas_por_pagar_gerente : System.Web.UI.Page
    {
        private void cargar_nota(string id_orden, string nota)
        {
            if (nota != string.Empty)
            {
                cuentas_Por_Pagar.cargar_nota(id_orden, nota);
            }
        }
        #region PDF
        private void crear_pdf_con_precio(string id, string id_factura)
        {


            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = "Orden de Compra id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            cuentas_Por_Pagar.crear_PDF_orden_de_compra_con_precio(ruta_archivo, imgdata, id, id_factura);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }
        #endregion
        #region imputaciones
        private void cargar_imputaciones()
        {
            cuentas_Por_Pagar.enviar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text, dropDown_proveedores.SelectedItem.Text);

            //     Session.Add("imputacionesBD", sistema_Administracion.get_imputaciones());
            //   imputacionesBD = (DataTable)Session["imputacionesBD"];

        }
        private void sumar_imputaciones()
        {
            label_total_imputacion.Text = cuentas_Por_Pagar.sumar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text);
        }
        #endregion
        #region carga tabla imputaciones
        private void crear_tabla_imputaciones()
        {
            imputaciones = new DataTable();
            imputaciones.Columns.Add("id", typeof(string));
            imputaciones.Columns.Add("Efectivo", typeof(string));
            imputaciones.Columns.Add("Transferencia", typeof(string));
            imputaciones.Columns.Add("Mercado_Pago", typeof(string));
            imputaciones.Columns.Add("Total", typeof(string));
            imputaciones.Columns.Add("Fecha", typeof(string));
            imputaciones.Columns.Add("Autorizado", typeof(bool));
            imputaciones.Columns.Add("nota", typeof(string));

        }
        private void llenar_tabla_imputaciones()
        {
            crear_tabla_imputaciones();
            imputacionesBD = (DataTable)Session["imputacionesBD"];
            imputacionesBD.DefaultView.Sort = "fecha DESC, id DESC";
            imputacionesBD = imputacionesBD.DefaultView.ToTable();
            int fila_dt = 0;
            double efectivo, transferencia, mercadoPago, total;

            for (int fila = 0; fila <= imputacionesBD.Rows.Count - 1; fila++)
            {
                if (cuentas_Por_Pagar.verificar_proveedor(imputacionesBD.Rows[fila]["id_proveedor"].ToString(), dropDown_proveedores.SelectedItem.Text)
                    && cuentas_Por_Pagar.verificar_fecha(imputacionesBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    imputaciones.Rows.Add();

                    efectivo = double.Parse(imputacionesBD.Rows[fila]["efectivo"].ToString());
                    transferencia = double.Parse(imputacionesBD.Rows[fila]["transferencia"].ToString());
                    mercadoPago = double.Parse(imputacionesBD.Rows[fila]["mercado_pago"].ToString());
                    total = efectivo + transferencia + mercadoPago;

                    imputaciones.Rows[fila_dt]["id"] = imputacionesBD.Rows[fila]["id"].ToString();
                    imputaciones.Rows[fila_dt]["Efectivo"] = funciones.formatCurrency(efectivo);
                    imputaciones.Rows[fila_dt]["Transferencia"] = funciones.formatCurrency(transferencia);
                    imputaciones.Rows[fila_dt]["Mercado_Pago"] = funciones.formatCurrency(mercadoPago);
                    imputaciones.Rows[fila_dt]["Total"] = funciones.formatCurrency(total);
                    imputaciones.Rows[fila_dt]["Fecha"] = imputacionesBD.Rows[fila]["fecha"].ToString();
                    imputaciones.Rows[fila_dt]["nota"] = imputacionesBD.Rows[fila]["nota"].ToString();

                    imputaciones.Rows[fila_dt]["Autorizado"] = true;


                    fila_dt++;
                }
            }
        }
        #endregion
        #region carga tabla orden de compra
        private void crear_tabla_orden()
        {
            ordenes_de_compra = new DataTable();
            ordenes_de_compra.Columns.Add("id", typeof(string));
            ordenes_de_compra.Columns.Add("num_orden", typeof(string));
            ordenes_de_compra.Columns.Add("fecha_remito", typeof(string));
            ordenes_de_compra.Columns.Add("fecha_vencimiento", typeof(string));
            ordenes_de_compra.Columns.Add("valor_orden", typeof(string));
            ordenes_de_compra.Columns.Add("condicion_pago", typeof(string));
            ordenes_de_compra.Columns.Add("estado_pago", typeof(string));
            ordenes_de_compra.Columns.Add("estado_entrega", typeof(string));
        }

        private void llenar_tabla_orden()
        {
            crear_tabla_orden();
            ordenes_de_compraBD = (DataTable)Session["ordenes_de_compraBD"];
            DateTime fecha_remito, fecha_actual, fecha_vencimiento;
            int condicion_pago;
            int fila_orden = 0;
            for (int fila = 0; fila <= ordenes_de_compraBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(ordenes_de_compraBD.Rows[fila]["fecha_remito"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text) &&
                    ordenes_de_compraBD.Rows[fila]["id_proveedor"].ToString() == cuentas_Por_Pagar.get_id_proveedor(dropDown_proveedores.SelectedItem.Text))
                {
                    ordenes_de_compra.Rows.Add();

                    ordenes_de_compra.Rows[fila_orden]["id"] = ordenes_de_compraBD.Rows[fila]["id"].ToString();
                    ordenes_de_compra.Rows[fila_orden]["num_orden"] = ordenes_de_compraBD.Rows[fila]["num_orden"].ToString();
                    ordenes_de_compra.Rows[fila_orden]["fecha_remito"] = ordenes_de_compraBD.Rows[fila]["fecha_remito"].ToString();

                    fecha_remito = (DateTime)ordenes_de_compraBD.Rows[fila]["fecha_remito"];
                    fecha_actual = DateTime.Today;
                    if (int.TryParse(ordenes_de_compraBD.Rows[fila]["condicion_pago"].ToString(), out condicion_pago))
                    {
                        fecha_vencimiento = fecha_remito.AddDays(condicion_pago);
                        ordenes_de_compra.Rows[fila_orden]["fecha_vencimiento"] = fecha_vencimiento.ToString("dd/MM/yyyy");
                        if (ordenes_de_compraBD.Rows[fila]["estado_pago"].ToString() != "Pagado")
                        {
                            if (fecha_vencimiento == fecha_actual)
                            {
                                ordenes_de_compra.Rows[fila_orden]["estado_pago"] = "Vence hoy";
                            }
                            else if (fecha_vencimiento < fecha_actual)
                            {
                                ordenes_de_compra.Rows[fila_orden]["estado_pago"] = "Vencida";
                            }
                            else if (fecha_vencimiento > fecha_actual)
                            {
                                ordenes_de_compra.Rows[fila_orden]["estado_pago"] = "Pago pendiente";
                            }
                        }
                        else
                        {
                            ordenes_de_compra.Rows[fila_orden]["estado_pago"] = ordenes_de_compraBD.Rows[fila]["estado_pago"].ToString();
                        }
                    }
                    else
                    {
                        ordenes_de_compra.Rows[fila_orden]["fecha_vencimiento"] = "N/A";
                        ordenes_de_compra.Rows[fila_orden]["estado_pago"] = ordenes_de_compraBD.Rows[fila]["estado_pago"].ToString();
                    }


                    ordenes_de_compra.Rows[fila_orden]["condicion_pago"] = ordenes_de_compraBD.Rows[fila]["condicion_pago"].ToString();
                    ordenes_de_compra.Rows[fila_orden]["estado_entrega"] = ordenes_de_compraBD.Rows[fila]["estado_entrega"].ToString();
                    ordenes_de_compra.Rows[fila_orden]["valor_orden"] = funciones.formatCurrency(double.Parse(ordenes_de_compraBD.Rows[fila]["valor_orden"].ToString()));

                    fila_orden++;
                }
            }
        }

        private void cargar_ordenes_de_compra()
        {
            llenar_tabla_orden();

            gridView_remitos.DataSource = ordenes_de_compra;
            gridView_remitos.DataBind();
            imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
            Session.Add("imputacionesBD", imputacionesBD);
            llenar_tabla_imputaciones();
            gridView_imputaciones.DataSource = imputaciones;
            gridView_imputaciones.DataBind();
            cargar_saldo();
            cargar_datos_proveedor();
        }
        private void cargar_saldo()
        {
            label_deuda_total_mes.Text = "Deuda Actual a Proveedores: " + funciones.formatCurrency(cuentas_Por_Pagar.deuda_total_del_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));

            label_saldo_anterior.Text = "Deuda meses anteriores: " + funciones.formatCurrency(cuentas_Por_Pagar.deuda_total_del_mes_anterior(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_saldo.Text = "Deuda actual: " + funciones.formatCurrency(cuentas_Por_Pagar.calcular_deuda_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes.Text = "Compra del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.calcular_Total_compra_del_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes_titulo.Text = "Compra del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.calcular_Total_compra_del_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes.Text = "Total pagado del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.pago_total_del_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes_titulo.Text = "Total pagado del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.pago_total_del_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
        }
        private void cargar_datos_proveedor()
        {
            DataTable proveedor_fabrica_seleccionado = cuentas_Por_Pagar.get_proveedor_fabrica_seleccionado(dropDown_proveedores.SelectedItem.Text);



            labe_telefono.Text = proveedor_fabrica_seleccionado.Rows[0]["telefono"].ToString();
            label_condicion_pago.Text = proveedor_fabrica_seleccionado.Rows[0]["condicion_pago"].ToString();
            label_CBU_1.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_1"].ToString();
            label_CBU_2.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_2"].ToString();
            label_CBU_3.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_3"].ToString();
            label_CBU_4.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_4"].ToString();
            label_CBU_5.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_5"].ToString();

        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_proveedores();
            cargar_mes();
            cargar_año();
        }
        private void cargar_proveedores()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            lista_proveedores = (DataTable)Session["lista_proveedores"];

            for (int fila = 0; fila <= lista_proveedores.Rows.Count - 1; fila++)
            {
                item = new System.Web.UI.WebControls.ListItem(lista_proveedores.Rows[fila]["proveedor"].ToString(), num_item.ToString());

                dropDown_proveedores.Items.Add(item);


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
        private void configurar_controles_nota_credito()
        {
            cargar_dias_nota();
            cargar_mes_nota();
            cargar_año_nota();
        }
        private void cargar_dias_nota()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 31; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                DropDow_nota_dia.Items.Add(item);
                num_item++;
            }
            DropDow_nota_dia.SelectedValue = DateTime.Now.Day.ToString();
        }
        private void cargar_año_nota()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                DropDow_nota_año.Items.Add(item);
                num_item++;
            }

            DropDow_nota_año.SelectedIndex = dropDown_año.Items.Count - 1;
        }
        private void cargar_mes_nota()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                DropDow_nota_mes.Items.Add(item);
                num_item++;
            }
            DropDow_nota_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_cuentas_por_pagar cuentas_Por_Pagar;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable lista_proveedores;
        DataTable ordenes_de_compraBD;
        DataTable ordenes_de_compra;
        DataTable imputacionesBD;
        DataTable imputaciones;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["cuentas_Por_Pagar"] == null)
            {
                Session.Add("cuentas_Por_Pagar", new cls_cuentas_por_pagar(usuariosBD));
            }
            cuentas_Por_Pagar = (cls_cuentas_por_pagar)Session["cuentas_Por_Pagar"];
            lista_proveedores = cuentas_Por_Pagar.get_lista_proveedores_fabrica();
            Session.Add("lista_proveedores", lista_proveedores);
            imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
            Session.Add("imputacionesBD", imputacionesBD);
            if (!IsPostBack)
            {

                configurar_controles();
                ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(dropDown_proveedores.SelectedItem.Text);
                Session.Add("ordenes_de_compraBD", ordenes_de_compraBD);
                cargar_ordenes_de_compra();
                configurar_controles_nota_credito();
            }
        }

        #region filtros
        protected void dropDown_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(dropDown_proveedores.SelectedItem.Text);
            Session.Add("ordenes_de_compraBD", ordenes_de_compraBD);
            cargar_ordenes_de_compra();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_ordenes_de_compra();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_ordenes_de_compra();
        }
        #endregion
        #region listas
        protected void gridView_remitos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        #endregion
        #region imputaciones
        protected void textBox_efectivo_TextChanged(object sender, EventArgs e)
        {
            sumar_imputaciones();
        }

        protected void textBox_transferencia_TextChanged(object sender, EventArgs e)
        {
            sumar_imputaciones();
        }

        protected void textBox_mercadoPago_TextChanged(object sender, EventArgs e)
        {
            sumar_imputaciones();
        }

        protected void boton_cargarImputacion_Click(object sender, EventArgs e)
        {
            cargar_imputaciones();
            imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
            cargar_ordenes_de_compra();
            textBox_efectivo.Text = string.Empty;
            textBox_transferencia.Text = string.Empty;
            textBox_mercadoPago.Text = string.Empty;
            label_total_imputacion.Text = funciones.formatCurrency(0);
        }
        #endregion

        protected void gridView_imputaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "eliminar_imputacion")
            {
                string index = e.CommandArgument.ToString();

                string id_imputacion = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                cuentas_Por_Pagar.eliminar_imputacion(id_imputacion);
                imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
                Session.Add("imputacionesBD", imputacionesBD);
                llenar_tabla_imputaciones();
                gridView_imputaciones.DataSource = imputaciones;
                gridView_imputaciones.DataBind();
                cargar_saldo();
            }
            else if (e.CommandName == "cargar_nota")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                TextBox textbox_nota = (gridView_imputaciones.Rows[int.Parse(index)].Cells[7].FindControl("textbox_nota") as TextBox);

                //cancelar pedido
                cargar_nota(id, textbox_nota.Text);
                imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
                Session.Add("imputacionesBD", imputacionesBD);
                llenar_tabla_imputaciones();
                gridView_imputaciones.DataSource = imputaciones;
                gridView_imputaciones.DataBind();
                cargar_saldo();
            }
        }

        protected void gridView_remitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {//    
            for (int fila = 0; fila <= gridView_remitos.Rows.Count - 1; fila++)
            {
                if (gridView_remitos.Rows[fila].Cells[5].Text == "Pagado")
                {
                    gridView_remitos.Rows[fila].CssClass = "table-success";
                    //ButtonField btnPagado = (ButtonField);
                    Button boton_pagar = (Button)gridView_remitos.Rows[fila].Cells[8].Controls[0].FindControl("boton_pagar");
                    boton_pagar.Text = "Desmarcar";
                    boton_pagar.CssClass = "btn btn-primary btn-sm btn-danger";

                }
                else if (gridView_remitos.Rows[fila].Cells[5].Text == "Vence hoy")
                {
                    gridView_remitos.Rows[fila].CssClass = "table-warning";
                }
                else if (gridView_remitos.Rows[fila].Cells[5].Text == "Pago pendiente")
                {
                    gridView_remitos.Rows[fila].CssClass = "table-primary";
                }
                else if (gridView_remitos.Rows[fila].Cells[5].Text == "Vencida")
                {
                    gridView_remitos.Rows[fila].CssClass = "table-danger";
                }
                int dato;
                if (!int.TryParse(gridView_remitos.Rows[fila].Cells[1].Text, out dato))
                {
                    Button boton_PDF = gridView_remitos.Rows[fila].Cells[7].FindControl("boton_PDF") as Button;
                    Button boton_pagar = gridView_remitos.Rows[fila].Cells[8].FindControl("boton_pagar") as Button;
                    boton_PDF.Visible = false;
                    boton_pagar.Visible = false;
                }
            }
        }

        protected void boton_pagar_Click(object sender, EventArgs e)
        {
            Button boton_pagar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_pagar.NamingContainer;
            int rowIndex = row.RowIndex;

            string id_orden = gridView_remitos.Rows[rowIndex].Cells[0].Text;
            ordenes_de_compraBD = (DataTable)Session["ordenes_de_compraBD"];
            int fila_orden = funciones.buscar_fila_por_id(id_orden, ordenes_de_compraBD);

            if (ordenes_de_compraBD.Rows[fila_orden]["estado_pago"].ToString() == "Pagado")
            {
                cuentas_Por_Pagar.desmarcar_factura_como_pagada(id_orden);
            }
            else
            {
                cuentas_Por_Pagar.marcar_factura_como_pagada(id_orden);
            }
            ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(dropDown_proveedores.SelectedItem.Text);
            Session.Add("ordenes_de_compraBD", ordenes_de_compraBD);
            cargar_ordenes_de_compra();
        }

        protected void boton_PDF_Click(object sender, EventArgs e)
        {
            Button boton_PDF = (Button)sender;
            GridViewRow row = (GridViewRow)boton_PDF.NamingContainer;
            int rowIndex = row.RowIndex;

            string id_orden = gridView_remitos.Rows[rowIndex].Cells[0].Text;
            ordenes_de_compraBD = (DataTable)Session["ordenes_de_compraBD"];
            int fila_orden = funciones.buscar_fila_por_id(id_orden, ordenes_de_compraBD);
            string id_de_factura = ordenes_de_compraBD.Rows[fila_orden]["num_orden"].ToString();

            crear_pdf_con_precio(id_orden, id_de_factura);
        }
        protected void textBox_monto_TextChanged(object sender, EventArgs e)
        {
            double cantidad = 0;
            if (double.TryParse(textBox_monto.Text, out cantidad))
            {
                if (cantidad < 0)
                {
                    textBox_monto.Text = string.Empty;
                }
                else
                {
                    label_monto.Text = funciones.formatCurrency(cantidad);
                }
            }
        }
        protected void boton_carga_nota_credido_Click(object sender, EventArgs e)
        {
            double cantidad = 0;
            if (double.TryParse(textBox_monto.Text, out cantidad) &&
                textBox_detalle.Text != string.Empty)
            {
                if (cantidad < 0)
                {
                    textBox_monto.Text = string.Empty;
                }
                else
                {
                    string fecha_nota = DropDow_nota_año.SelectedItem.Text + "-" + DropDow_nota_mes.SelectedItem.Text + "-" + DropDow_nota_dia.SelectedItem.Text;
                    fecha_nota = fecha_nota + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                    string cantidad_a_cargar = "-" + cantidad.ToString();
                    cuentas_Por_Pagar.cargar_nota_credito(dropDown_proveedores.SelectedItem.Text, textBox_detalle.Text, cantidad_a_cargar, fecha_nota);
                    textBox_monto.Text = string.Empty;
                    textBox_detalle.Text = string.Empty;
                    label_monto.Text = "Total: $0.00";
                    ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(dropDown_proveedores.SelectedItem.Text);
                    Session.Add("ordenes_de_compraBD", ordenes_de_compraBD);
                    cargar_ordenes_de_compra();
                }
            }
        }
    }
}