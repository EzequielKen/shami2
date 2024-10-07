using _03___sistemas_fabrica;
using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class resumen_deuda_locales : System.Web.UI.Page
    {
        private void crear_tabla_locales()
        {
            locales = new DataTable();
            locales.Columns.Add("id", typeof(string));
            locales.Columns.Add("sucursal", typeof(string));
            locales.Columns.Add("deuda", typeof(string));
        }
        private void llenar_tabla_proveedores()
        {
            crear_tabla_locales();
            int fila_lista = 0;
            for (int fila = 0; fila <= localesBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, localesBD.Rows[fila]["sucursal"].ToString()))
                {
                    locales.Rows.Add();

                    locales.Rows[fila_lista]["id"] = localesBD.Rows[fila]["id"].ToString();
                    locales.Rows[fila_lista]["sucursal"] = localesBD.Rows[fila]["sucursal"].ToString();
                    locales.Rows[fila_lista]["deuda"] = funciones.formatCurrency(double.Parse(localesBD.Rows[fila]["deuda"].ToString()));
                    fila_lista++;
                }
            }
        }
        private void cargar_proveedores()
        {
            localesBD = (DataTable)Session["resumen_de_deuda_locales"];

            llenar_tabla_proveedores();
            gridView_proveedores.DataSource = locales;
            gridView_proveedores.DataBind();
            label_deuda_total.Text = "Deuda Total: " + resumen_deuda.get_deuda_total();
        }
        private void cargar_nota(string id_orden, string nota)
        {
            if (nota != string.Empty)
            {
                cuentas_Por_cobrar.cargar_nota(id_orden, nota);
            }
        }

        #region imputaciones

        private void sumar_imputaciones()
        {
            label_total_imputacion.Text = cuentas_Por_cobrar.sumar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text);
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
            imputaciones.Columns.Add("Autorizado", typeof(string));
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
                if (cuentas_Por_cobrar.verificar_fecha(imputacionesBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    imputaciones.Rows.Add();

                    efectivo = double.Parse(imputacionesBD.Rows[fila]["abono_efectivo"].ToString());
                    transferencia = double.Parse(imputacionesBD.Rows[fila]["abono_digital"].ToString());
                    mercadoPago = double.Parse(imputacionesBD.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    total = efectivo + transferencia + mercadoPago;

                    imputaciones.Rows[fila_dt]["id"] = imputacionesBD.Rows[fila]["id"].ToString();
                    imputaciones.Rows[fila_dt]["Efectivo"] = funciones.formatCurrency(efectivo);
                    imputaciones.Rows[fila_dt]["Transferencia"] = funciones.formatCurrency(transferencia);
                    imputaciones.Rows[fila_dt]["Mercado_Pago"] = funciones.formatCurrency(mercadoPago);
                    imputaciones.Rows[fila_dt]["Total"] = funciones.formatCurrency(total);
                    imputaciones.Rows[fila_dt]["Fecha"] = imputacionesBD.Rows[fila]["fecha"].ToString();
                    imputaciones.Rows[fila_dt]["nota"] = imputacionesBD.Rows[fila]["nota"].ToString();

                    imputaciones.Rows[fila_dt]["Autorizado"] = imputacionesBD.Rows[fila]["autorizado"].ToString();


                    fila_dt++;
                }
            }
        }
        #endregion
        #region carga tabla orden de compra
        private void crear_tabla_orden()
        {
            cuentas_por_pagar = new DataTable();
            cuentas_por_pagar.Columns.Add("id", typeof(string));
            cuentas_por_pagar.Columns.Add("sucursal", typeof(string));
            cuentas_por_pagar.Columns.Add("num_pedido", typeof(string));
            cuentas_por_pagar.Columns.Add("valor_remito", typeof(string));
            cuentas_por_pagar.Columns.Add("fecha_remito", typeof(string));
            cuentas_por_pagar.Columns.Add("nota", typeof(string));
            cuentas_por_pagar.Columns.Add("cobrado", typeof(string));
        }

        private void llenar_tabla_orden()
        {
            crear_tabla_orden();
            cuentas_por_pagarBD = (DataTable)Session["cuentas_por_pagarBD"];
            DateTime fecha_remito, fecha_actual, fecha_vencimiento;
            int condicion_pago;
            int fila_orden = 0;
            for (int fila = 0; fila <= cuentas_por_pagarBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(cuentas_por_pagarBD.Rows[fila]["fecha_remito"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    cuentas_por_pagar.Rows.Add();

                    cuentas_por_pagar.Rows[fila_orden]["id"] = cuentas_por_pagarBD.Rows[fila]["id"].ToString();
                    cuentas_por_pagar.Rows[fila_orden]["sucursal"] = cuentas_por_pagarBD.Rows[fila]["sucursal"].ToString();
                    cuentas_por_pagar.Rows[fila_orden]["num_pedido"] = cuentas_por_pagarBD.Rows[fila]["num_pedido"].ToString();
                    cuentas_por_pagar.Rows[fila_orden]["valor_remito"] = funciones.formatCurrency(cuentas_por_pagarBD.Rows[fila]["valor_remito"]);
                    cuentas_por_pagar.Rows[fila_orden]["fecha_remito"] = cuentas_por_pagarBD.Rows[fila]["fecha_remito"].ToString();
                    cuentas_por_pagar.Rows[fila_orden]["nota"] = cuentas_por_pagarBD.Rows[fila]["nota"].ToString();
                    cuentas_por_pagar.Rows[fila_orden]["cobrado"] = cuentas_por_pagarBD.Rows[fila]["cobrado"].ToString();

                    fila_orden++;
                }
            }
        }

        private void cargar_cuentas_por_pagar(string sucursal)
        {
            cuentas_por_pagarBD = cuentas_Por_cobrar.get_remitos(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text);
            Session.Add("cuentas_por_pagarBD", cuentas_por_pagarBD);

            llenar_tabla_orden();

            gridView_remitos.DataSource = cuentas_por_pagar;
            gridView_remitos.DataBind();
            imputacionesBD = cuentas_Por_cobrar.get_imputaciones(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text);
            Session.Add("imputacionesBD", imputacionesBD);
            llenar_tabla_imputaciones();
            gridView_imputaciones.DataSource = imputaciones;
            gridView_imputaciones.DataBind();
            cargar_saldo(sucursal);
        }
        private void cargar_saldo(string sucursal)
        {
            label_saldo_anterior.Text = "Deuda meses anteriores: " + funciones.formatCurrency(calculo_deuda_locales.calcular_deuda_mes_anterior(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_saldo.Text = "Deuda actual: " + funciones.formatCurrency(calculo_deuda_locales.calcular_deuda_del_mes(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes.Text = "Compra del mes: " + funciones.formatCurrency(calculo_deuda_locales.calcular_compra_del_mes(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes.Text = "Total pagado del mes: " + funciones.formatCurrency(calculo_deuda_locales.calcular_pagos_del_mes(sucursal, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
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
        cls_resumen_deuda_locales resumen_deuda;
        cls_sistema_cuentas_por_cobrar cuentas_Por_cobrar;
        cls_calculo_deuda_locales calculo_deuda_locales;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable localesBD;
        DataTable locales;

        DataTable lista_proveedores;
        DataTable cuentas_por_pagarBD;
        DataTable cuentas_por_pagar;
        DataTable imputacionesBD;
        DataTable imputaciones;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];

            resumen_deuda = new cls_resumen_deuda_locales(usuariosBD);
            calculo_deuda_locales = new cls_calculo_deuda_locales(usuariosBD);
            cuentas_Por_cobrar = new cls_sistema_cuentas_por_cobrar(usuariosBD);
            if (!IsPostBack)
            {
                configurar_controles();
                localesBD = resumen_deuda.get_proveedores_de_fabrica();
                Session.Add("resumen_de_deuda_locales", localesBD);
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
            int fila_local;
            double deuda;
            localesBD = (DataTable)Session["resumen_de_deuda_locales"];
            for (int fila = 0; fila <= gridView_proveedores.Rows.Count - 1; fila++)
            {
                id = gridView_proveedores.Rows[fila].Cells[0].Text;
                fila_local = funciones.buscar_fila_por_id(id, localesBD);
                deuda = double.Parse(localesBD.Rows[fila_local]["deuda"].ToString());
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

            string id_local = gridView_proveedores.Rows[rowIndex].Cells[0].Text;
            string nombre_sucursal = gridView_proveedores.Rows[rowIndex].Cells[1].Text;
            Session.Add("sucursal_seleccionada", nombre_sucursal);
            cargar_cuentas_por_pagar(nombre_sucursal);
        }

        protected void gridView_remitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id;
            int num_pedido, fila_remito;
            for (int fila = 0; fila <= gridView_remitos.Rows.Count - 1; fila++)
            {
                if (int.TryParse(gridView_remitos.Rows[fila].Cells[2].Text, out num_pedido))
                {
                    if (num_pedido == 0)
                    {
                        gridView_remitos.Rows[fila].Cells[5].Controls[0].Visible = false;
                    }
                }
                else
                {
                    gridView_remitos.Rows[fila].Cells[5].Controls[0].Visible = false;
                    Button boton_iva = (gridView_remitos.Rows[fila].Cells[8].FindControl("boton_iva") as Button);
                    boton_iva.Visible = false;
                }
                id = gridView_remitos.Rows[fila].Cells[0].Text;
                fila_remito = funciones.buscar_fila_por_id(id, cuentas_por_pagarBD);
                if (cuentas_por_pagarBD.Rows[fila_remito]["cobrado"].ToString() == "Si")
                {
                    Button boton_cobrado = (gridView_remitos.Rows[fila].Cells[7].FindControl("boton_cobrado") as Button);
                    boton_cobrado.Text = "Desmarcar";
                    boton_cobrado.CssClass = "btn btn-danger";
                    gridView_remitos.Rows[fila].CssClass = "table-success";
                }
                if (cuentas_por_pagarBD.Rows[fila_remito]["aumento"].ToString() != "0")
                {
                    Button boton_iva = (gridView_remitos.Rows[fila].Cells[8].FindControl("boton_iva") as Button);
                    boton_iva.Visible = false;
                }
            }
        }
        protected void gridView_remitos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int dato;
            if (int.TryParse(gridView_remitos.SelectedRow.Cells[2].Text, out dato))

            {

                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = Session["sucursal"].ToString() + " pedido-" + gridView_remitos.SelectedRow.Cells[1].Text + "- id-" + dato_hora + ".pdf";
                string ruta = "~/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                string ruta_logo = "~/imagenes/logo-completo.png";
                cuentas_Por_cobrar.crear_pdf(ruta_archivo, gridView_remitos.SelectedRow.Cells[0].Text, "Fabrica villa maipu", imgdata, Session["nivel_seguridad"].ToString(), Session["sucursal_seleccionada"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text); //crear_pdf();

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();

            }
        }

        protected void boton_cobrado_Click(object sender, EventArgs e)
        {
            cuentas_por_pagarBD = (DataTable)Session["cuentas_por_pagarBD"];

            Button boton_cobrado = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cobrado.NamingContainer;
            int fila = row.RowIndex;

            string id = gridView_remitos.Rows[fila].Cells[0].Text;

            int fila_remitos = funciones.buscar_fila_por_id(id, cuentas_por_pagarBD);

            string estado = cuentas_por_pagarBD.Rows[fila_remitos]["cobrado"].ToString();

            if (estado == "Si")
            {
                cuentas_Por_cobrar.marcar_cobrado(id, "No");
            }
            else
            {
                cuentas_Por_cobrar.marcar_cobrado(id, "Si");
            }

            cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());
        }
        protected void boton_iva_Click(object sender, EventArgs e)
        {
            cuentas_por_pagarBD = (DataTable)Session["cuentas_por_pagarBD"];

            Button boton_iva = (Button)sender;
            GridViewRow row = (GridViewRow)boton_iva.NamingContainer;
            int fila = row.RowIndex;

            string id = gridView_remitos.Rows[fila].Cells[0].Text;

            int fila_remitos = funciones.buscar_fila_por_id(id, cuentas_por_pagarBD);

            double valor_remito = double.Parse(cuentas_por_pagarBD.Rows[fila_remitos]["valor_remito"].ToString());

            double porcentaje = (valor_remito * 21) / 100;

            valor_remito = valor_remito + porcentaje;

            string proveedor = cuentas_por_pagarBD.Rows[fila_remitos]["proveedor"].ToString();
            string sucursal = gridView_remitos.Rows[fila].Cells[1].Text;
            string num_pedido = gridView_remitos.Rows[fila].Cells[2].Text;
            cuentas_Por_cobrar.cargar_iva(id, proveedor, sucursal, num_pedido, valor_remito.ToString());

            DateTime fecha_actual = DateTime.Now;
            int mes_seleccionado = int.Parse(dropDown_mes.SelectedItem.Text);
            int año_seleccionado = int.Parse(dropDown_año.SelectedItem.Text);
            bool seguir = true;
            while (seguir)
            {
                cuentas_Por_cobrar.get_deuda_total_mes(sucursal, mes_seleccionado.ToString(), año_seleccionado.ToString());
                if (mes_seleccionado == fecha_actual.Month &&
                    año_seleccionado == fecha_actual.Year)
                {
                    seguir = false;
                }
                else
                {
                    if (mes_seleccionado < fecha_actual.Month &&
                        año_seleccionado == fecha_actual.Year)
                    {
                        mes_seleccionado++;
                    }
                    else if (mes_seleccionado == 12 &&
                             año_seleccionado < fecha_actual.Year)
                    {
                        mes_seleccionado = 1;
                        año_seleccionado++;
                    }
                }
            }
            cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());
        }
        #region imputaciones
        #region cargar imputaciones
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
            cuentas_Por_cobrar.enviar_imputacion_de_local_como_fabrica(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text, "proveedor_villaMaipu", Session["sucursal_seleccionada"].ToString());

            cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

            textBox_efectivo.Text = string.Empty;
            textBox_transferencia.Text = string.Empty;
            textBox_mercadoPago.Text = string.Empty;
            label_total_imputacion.Text = funciones.formatCurrency(0);
        }
        #endregion

        #region carga boleta
        protected void textBox_monto_boleta_TextChanged(object sender, EventArgs e)
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
        protected void boton_cargar_boleta_Click(object sender, EventArgs e)
        {
            double cantidad = 0;
            if (double.TryParse(textBox_monto_boleta.Text, out cantidad) &&
                textBox_detalle_boleta.Text != string.Empty)
            {
                if (cantidad < 0)
                {
                    textBox_monto_boleta.Text = string.Empty;
                }
                else
                {
                    string cantidad_a_cargar = cantidad.ToString();
                    cuentas_Por_cobrar.cargar_nota_credito(Session["sucursal_seleccionada"].ToString(), textBox_detalle_boleta.Text, cantidad_a_cargar);
                    textBox_monto_boleta.Text = string.Empty;
                    textBox_detalle_boleta.Text = string.Empty;
                    label_total_boleta.Text = "Total: $0.00";
                }
                cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

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
                    string cantidad_a_cargar = "-" + cantidad.ToString();
                    cuentas_Por_cobrar.cargar_nota_credito(Session["sucursal_seleccionada"].ToString(), textBox_detalle.Text, cantidad_a_cargar);
                    textBox_monto.Text = string.Empty;
                    textBox_detalle.Text = string.Empty;
                    label_monto.Text = "Total: $0.00";
                }
                cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

            }
        }
        #endregion
        protected void gridView_imputaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "autorizar")
            {
                string index = e.CommandArgument.ToString();

                string id_imputacion = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                cuentas_Por_cobrar.autorizar_imputacion(id_imputacion);

                cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

            }
            else if (e.CommandName == "cargar_nota")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                TextBox textbox_nota = (gridView_imputaciones.Rows[int.Parse(index)].Cells[8].FindControl("textbox_nota") as TextBox);

                cargar_nota(id, textbox_nota.Text);
                cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());


            }
            else if (e.CommandName == "eliminar")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;

                cuentas_Por_cobrar.eliminar_imputacion(id);

                cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());


            }
        }






        #endregion

        #region filtros

        #endregion

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_cuentas_por_pagar(Session["sucursal_seleccionada"].ToString());

        }
    }
}