using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class resumen_deuda_proveedores_gerente : System.Web.UI.Page
    {
        private void cargar_datos_proveedor(string id)
        {

            proveedores_de_fabrica_BD = (DataTable)Session["resumen_de_deuda_a_proveedores"];
            int fila_proveedor = funciones.buscar_fila_por_id(id, proveedores_de_fabrica_BD);

            Label_nombre_proveedor.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["proveedor"].ToString();
            labe_telefono.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["telefono"].ToString();
            label_condicion_pago.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["condicion_pago"].ToString();
            label_CBU_1.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["CBU_1"].ToString();
            label_CBU_2.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["CBU_2"].ToString();
            label_CBU_3.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["CBU_3"].ToString();
            label_CBU_4.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["CBU_4"].ToString();
            label_CBU_5.Text = proveedores_de_fabrica_BD.Rows[fila_proveedor]["CBU_5"].ToString();

        }
        private void crear_tabla_proveedores()
        {
            proveedores_de_fabrica = new DataTable();
            proveedores_de_fabrica.Columns.Add("id", typeof(string));
            proveedores_de_fabrica.Columns.Add("proveedor", typeof(string));
            proveedores_de_fabrica.Columns.Add("deuda", typeof(string));
            proveedores_de_fabrica.Columns.Add("entrega_parcial", typeof(string));
        }
        private void llenar_tabla_proveedores()
        {
            crear_tabla_proveedores();
            int fila_lista = 0;
            for (int fila = 0; fila <= proveedores_de_fabrica_BD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, proveedores_de_fabrica_BD.Rows[fila]["proveedor"].ToString()))
                {
                    proveedores_de_fabrica.Rows.Add();

                    proveedores_de_fabrica.Rows[fila_lista]["id"] = proveedores_de_fabrica_BD.Rows[fila]["id"].ToString();
                    proveedores_de_fabrica.Rows[fila_lista]["proveedor"] = proveedores_de_fabrica_BD.Rows[fila]["proveedor"].ToString();
                    proveedores_de_fabrica.Rows[fila_lista]["deuda"] = funciones.formatCurrency(double.Parse(proveedores_de_fabrica_BD.Rows[fila]["deuda"].ToString()));
                    proveedores_de_fabrica.Rows[fila_lista]["entrega_parcial"] = proveedores_de_fabrica_BD.Rows[fila]["entrega_parcial"].ToString();
                    fila_lista++;
                }
            }
        }
        private void cargar_proveedores()
        {
            proveedores_de_fabrica_BD = (DataTable)Session["resumen_de_deuda_a_proveedores"];

            llenar_tabla_proveedores();
            gridView_proveedores.DataSource = proveedores_de_fabrica;
            gridView_proveedores.DataBind();
            label_deuda_total.Text = "Deuda Total: " + resumen_deuda.get_deuda_total();
        }
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
            cuentas_Por_Pagar.enviar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text, Label_nombre_proveedor.Text);

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
                if (cuentas_Por_Pagar.verificar_proveedor(imputacionesBD.Rows[fila]["id_proveedor"].ToString(), Label_nombre_proveedor.Text)
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
                    ordenes_de_compraBD.Rows[fila]["id_proveedor"].ToString() == cuentas_Por_Pagar.get_id_proveedor(Label_nombre_proveedor.Text))
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
            ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(Label_nombre_proveedor.Text);
            Session.Add("ordenes_de_compraBD", ordenes_de_compraBD);

            llenar_tabla_orden();

            gridView_remitos.DataSource = ordenes_de_compra;
            gridView_remitos.DataBind();
            imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
            Session.Add("imputacionesBD", imputacionesBD);
            llenar_tabla_imputaciones();
            gridView_imputaciones.DataSource = imputaciones;
            gridView_imputaciones.DataBind();
            cargar_saldo();
        }
        private void cargar_saldo()
        {

            label_saldo_anterior.Text = "Deuda meses anteriores: " + funciones.formatCurrency(cuentas_Por_Pagar.deuda_total_del_mes_anterior(Label_nombre_proveedor.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_saldo.Text = "Deuda actual: " + funciones.formatCurrency(cuentas_Por_Pagar.calcular_deuda_mes(Label_nombre_proveedor.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes.Text = "Compra del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.calcular_Total_compra_del_mes(Label_nombre_proveedor.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes.Text = "Total pagado del mes: " + funciones.formatCurrency(cuentas_Por_Pagar.pago_total_del_mes(Label_nombre_proveedor.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
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
        /// //////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_resumen_deuda_proveedores resumen_deuda;
        cls_cuentas_por_pagar cuentas_Por_Pagar;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable proveedores_de_fabrica_BD;
        DataTable proveedores_de_fabrica;

        DataTable lista_proveedores;
        DataTable ordenes_de_compraBD;
        DataTable ordenes_de_compra;
        DataTable imputacionesBD;
        DataTable imputaciones;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["resumen_deuda"] == null)
            {
                Session.Add("resumen_deuda", new cls_resumen_deuda_proveedores(usuariosBD));
            }
            resumen_deuda = (cls_resumen_deuda_proveedores)Session["resumen_deuda"];

            if (Session["cuentas_Por_Pagar"] == null)
            {
                Session.Add("cuentas_Por_Pagar", new cls_cuentas_por_pagar(usuariosBD));
            }
            cuentas_Por_Pagar = (cls_cuentas_por_pagar)Session["cuentas_Por_Pagar"];
            if (!IsPostBack)
            {
                configurar_controles();
                proveedores_de_fabrica_BD = resumen_deuda.get_proveedores_de_fabrica();
                Session.Add("resumen_de_deuda_a_proveedores", proveedores_de_fabrica_BD);
                cargar_proveedores();


            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_proveedores();
        }

        protected void gridView_proveedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id;
            int fila_proveedor;
            double deuda;
            proveedores_de_fabrica_BD = (DataTable)Session["resumen_de_deuda_a_proveedores"];
            for (int fila = 0; fila <= gridView_proveedores.Rows.Count - 1; fila++)
            {
                id = gridView_proveedores.Rows[fila].Cells[0].Text;
                fila_proveedor = funciones.buscar_fila_por_id(id, proveedores_de_fabrica_BD);
                deuda = double.Parse(proveedores_de_fabrica_BD.Rows[fila_proveedor]["deuda"].ToString());
                if (deuda < 0)
                {
                    gridView_proveedores.Rows[fila].CssClass = "table-success";
                    Button boton_detalle = gridView_proveedores.Rows[fila].Cells[3].Controls[0].FindControl("boton_detalle") as Button;
                    boton_detalle.ControlStyle.CssClass = "btn btn-primary btn-sm btn-success";
                    //  boton_detalle = (gridView_proveedores.Rows[fila].Cells[3].FindControl("boton_detalle") as ButtonField);
                    //Button boton_detalle = (Button)gridView_proveedores.Rows[fila].Cells[3].FindControl("boton_detalle");

                }
                else if (deuda > 0)
                {
                    gridView_proveedores.Rows[fila].CssClass = "table-danger";

                    Button boton_detalle = gridView_proveedores.Rows[fila].Cells[3].Controls[0].FindControl("boton_detalle") as Button;
                    boton_detalle.ControlStyle.CssClass = "btn btn-primary btn-sm btn-danger";


                    // gridView_proveedores.Rows[fila].Cells[3].Controls[0].CssClass = "btn btn-primary btn-sm btn-danger";
                    // Button boton_detalle = (Button)gridView_proveedores.Rows[fila].Cells[3].FindControl("boton_detalle");
                }
            }
        }

        protected void boton_detalle_Click(object sender, EventArgs e)
        {
            Button boton_detalle = (Button)sender;
            GridViewRow row = (GridViewRow)boton_detalle.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridView_proveedores.Rows[rowIndex].Cells[0].Text;
            cargar_datos_proveedor(id);
            cargar_ordenes_de_compra();
        }

        protected void gridView_remitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
            ordenes_de_compraBD = cuentas_Por_Pagar.get_cuenta_por_pagar_fabrica(Label_nombre_proveedor.Text);
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

            proveedores_de_fabrica_BD = resumen_deuda.get_proveedores_de_fabrica();
            Session.Add("resumen_de_deuda_a_proveedores", proveedores_de_fabrica_BD);
            cargar_proveedores();
        }
        #endregion


        #region filtros
        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_ordenes_de_compra();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_ordenes_de_compra();
        }
        #endregion

        protected void eliminar_imputacion_Click(object sender, EventArgs e)
        {
            Button eliminar_imputacion = (Button)sender;
            GridViewRow row = (GridViewRow)eliminar_imputacion.NamingContainer;
            int index = row.RowIndex;

            string id_imputacion = gridView_imputaciones.Rows[index].Cells[0].Text;
            cuentas_Por_Pagar.eliminar_imputacion(id_imputacion);
            imputacionesBD = cuentas_Por_Pagar.get_imputaciones();
            Session.Add("imputacionesBD", imputacionesBD);
            llenar_tabla_imputaciones();
            gridView_imputaciones.DataSource = imputaciones;
            gridView_imputaciones.DataBind();
            cargar_saldo();
        }

        protected void cargar_nota_Click(object sender, EventArgs e)
        {
            Button boton_cargar_nota = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar_nota.NamingContainer;
            int index = row.RowIndex;

            string id = gridView_imputaciones.Rows[index].Cells[0].Text;
            TextBox textbox_nota = (gridView_imputaciones.Rows[index].Cells[7].FindControl("textbox_nota") as TextBox);

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
}