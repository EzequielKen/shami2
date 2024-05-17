using _02___sistemas;
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
    public partial class administracion_fabrica : System.Web.UI.Page
    {
        private void cargar_nota(string id_orden, string nota)
        {
            if (nota != string.Empty)
            {
                sistema_Administracion.cargar_nota(id_orden, nota);
            }
        }
        private void sumar_imputaciones()
        {
            label_total_imputacion.Text = sistema_Administracion.sumar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text);
        }
        private void cargar_imputaciones()
        {
            sistema_Administracion.enviar_imputacion_de_local_como_fabrica(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text, "proveedor_villaMaipu", dropDown_sucursales.SelectedItem.Text);
        }
        #region funciones
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_sucursales();
            cargar_mes();
            cargar_año();

           
        }
        private void configurar_controles_nota_credito()
        {
            cargar_dias_nota();
            cargar_mes_nota();
            cargar_año_nota();
        }
        private void cargar_sucursales()
        {
            sucursales = sistema_Administracion.get_sucursales();
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            sucursales = (DataTable)Session["sucursalesBD"];
            for (int fila = 0; fila <= sucursales.Rows.Count - 1; fila++)
            {

                item = new System.Web.UI.WebControls.ListItem(sucursales.Rows[fila]["sucursal"].ToString(), num_item.ToString());

                dropDown_sucursales.Items.Add(item);


                num_item++;

            }
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

            dropDown_año.SelectedIndex = dropDown_año.Items.Count - 1;
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
        private void crear_datatable()
        {
            remitos = new DataTable();
            remitos.Rows.Clear();
            remitos.Columns.Clear();

            remitos.Columns.Add("id", typeof(string));
            remitos.Columns.Add("sucursal", typeof(string));
            remitos.Columns.Add("num_pedido", typeof(string));
            remitos.Columns.Add("valor_remito", typeof(string));
            remitos.Columns.Add("fecha_remito", typeof(string)); 
            remitos.Columns.Add("nota", typeof(string)); 

            imputaciones = new DataTable();
            imputaciones.Rows.Clear();
            imputaciones.Columns.Clear();

            imputaciones.Columns.Add("id", typeof(string));
            imputaciones.Columns.Add("Efectivo", typeof(string));
            imputaciones.Columns.Add("Transferencia", typeof(string));
            imputaciones.Columns.Add("Mercado_Pago", typeof(string));
            imputaciones.Columns.Add("Total", typeof(string));
            imputaciones.Columns.Add("Fecha", typeof(string));
            imputaciones.Columns.Add("Autorizado", typeof(string));
            imputaciones.Columns.Add("nota", typeof(string));
        }
        private void llenar_dataTable()
        {
            crear_datatable();
            remitos.Rows.Clear();
            remitosBD.DefaultView.Sort = "fecha_remito DESC, id DESC";
            remitosBD = remitosBD.DefaultView.ToTable();
            int fila_dt = 0;
            for (int fila = 0; fila <= remitosBD.Rows.Count - 1; fila++)
            {
                if (remitosBD.Rows[fila]["sucursal"].ToString() == dropDown_sucursales.SelectedItem.Text &&
                    sistema_Administracion.verificar_proveedor(remitosBD.Rows[fila]["proveedor"].ToString(), (DataTable)Session["proveedorBD"]) &&
                    sistema_Administracion.verificar_fecha(remitosBD.Rows[fila]["fecha_remito"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    remitos.Rows.Add();

                    string id = remitosBD.Rows[fila]["id"].ToString();
                    remitos.Rows[fila_dt]["id"] = id;
                    remitos.Rows[fila_dt]["sucursal"] = remitosBD.Rows[fila]["sucursal"].ToString();
                    remitos.Rows[fila_dt]["num_pedido"] = remitosBD.Rows[fila]["num_pedido"].ToString();
                    remitos.Rows[fila_dt]["valor_remito"] = formatCurrency(remitosBD.Rows[fila]["valor_remito"]);
                    remitos.Rows[fila_dt]["fecha_remito"] = formatDate(remitosBD.Rows[fila]["fecha_remito"].ToString()); 
                    remitos.Rows[fila_dt]["nota"] = remitosBD.Rows[fila]["nota"].ToString(); 

                    fila_dt++;
                }

            }


            llenar_imputaciones();

        }
        private void llenar_imputaciones()
        {
            imputacionesBD = (DataTable)Session["imputacionesBD"];

            imputacionesBD.DefaultView.Sort = "fecha DESC, id DESC";
            imputacionesBD = imputacionesBD.DefaultView.ToTable();
            int fila_dt = 0;
            double efectivo, transferencia, mercadoPago, total;

            for (int fila = 0; fila <= imputacionesBD.Rows.Count - 1; fila++)
            {
                if (imputacionesBD.Rows[fila]["sucursal"].ToString() == dropDown_sucursales.SelectedItem.Text
                    && sistema_Administracion.verificar_proveedor(imputacionesBD.Rows[fila]["proveedor"].ToString(), (DataTable)Session["proveedorBD"])
                    && sistema_Administracion.verificar_fecha(imputacionesBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    imputaciones.Rows.Add();

                    efectivo = double.Parse(imputacionesBD.Rows[fila]["abono_efectivo"].ToString());
                    transferencia = double.Parse(imputacionesBD.Rows[fila]["abono_digital"].ToString());
                    mercadoPago = double.Parse(imputacionesBD.Rows[fila]["abono_digital_mercadoPago"].ToString());
                    total = efectivo + transferencia + mercadoPago;

                    imputaciones.Rows[fila_dt]["id"] = imputacionesBD.Rows[fila]["id"].ToString();
                    imputaciones.Rows[fila_dt]["Efectivo"] = formatCurrency(efectivo);
                    imputaciones.Rows[fila_dt]["Transferencia"] = formatCurrency(transferencia);
                    imputaciones.Rows[fila_dt]["Mercado_Pago"] = formatCurrency(mercadoPago);
                    imputaciones.Rows[fila_dt]["Total"] = formatCurrency(total);
                    imputaciones.Rows[fila_dt]["Fecha"] = formatDate(imputacionesBD.Rows[fila]["fecha"].ToString());
                    imputaciones.Rows[fila_dt]["nota"] = imputacionesBD.Rows[fila]["nota"].ToString();

                    imputaciones.Rows[fila_dt]["Autorizado"] = imputacionesBD.Rows[fila]["autorizado"].ToString();

                    fila_dt++;
                }
            }
        }
        private void cargar_remitos()
        {
            llenar_dataTable();
            gridView_remitos.DataSource = remitos;
            gridView_remitos.DataBind();

            gridView_imputaciones.DataSource = null;
            gridView_imputaciones.DataBind();
            gridView_imputaciones.DataSource = imputaciones;
            gridView_imputaciones.DataBind();
            cargar_saldo();
        }
        private void cargar_saldo()
        {
            label_deuda_total_mes.Text = "Deuda total del mes: " + formatCurrency(sistema_Administracion.deuda_total_del_mes(nombre_proveedor_seleccionado, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));


            label_saldo.Text = "Deuda actual: " + formatCurrency(sistema_Administracion.calcular_deuda_mes((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_saldo_anterior.Text = "Deuda meses anteriores: " + formatCurrency(sistema_Administracion.calcular_deuda_mes_anterior((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_total_compra.Text = "Compra del mes: " + formatCurrency(sistema_Administracion.calcular_compra_mes((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_total_compra_titulo.Text = "Compra del mes: " + formatCurrency(sistema_Administracion.calcular_compra_mes((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_total_pago.Text = "Total pagado del mes: " + formatCurrency(sistema_Administracion.calcular_pago_mes((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_total_pago_titulo.Text = "Total pagado del mes: " + formatCurrency(sistema_Administracion.calcular_pago_mes((DataTable)Session["proveedorBD"], dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
        }
        private string formatDate(string valor)
        {
            DateTime fecha = DateTime.Parse(valor);
            return fecha.Day.ToString() + "/" + fecha.Month.ToString() + "/" + fecha.Year.ToString();
        }
        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
        }
        #endregion
        //#########################################################################################################
        #region atributos
        cls_sistema_cuentas_por_cobrar sistema_Administracion;
        cls_sistema_pedidos_fabrica pedidos_fabrica;
        cls_funciones funciones = new cls_funciones();
        DataTable sucursalBD;
        DataTable sucursales;
        DataTable proveedorBD;
        DataTable remitosBD;
        DataTable imputacionesBD;
        DataTable remitos;
        DataTable imputaciones;
        string proveedor_seleccionado;
        string nombre_proveedor_seleccionado;
        int seguridad;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            seguridad = int.Parse(Session["nivel_seguridad"].ToString());
            if (seguridad < 2)
            {
                label_saldo_anterior.Visible = true;
                label_saldo.Visible = true;
                label_total_compra.Visible = true;
                label_total_pago.Visible = true;
                gridView_remitos.Columns[3].Visible = true;
                gridView_imputaciones.Visible = true;

                label_deuda_total_mes.Visible = true;


            }
            else if (seguridad > 1)
            {

                label_saldo_anterior.Visible = false;
                label_saldo.Visible = false;
                label_total_compra.Visible = false;
                label_total_compra_titulo.Visible = false;
                label_total_pago.Visible = false;
                label_total_pago_titulo.Visible = false;
                gridView_remitos.Columns[4].Visible = false;
                gridView_imputaciones.Visible = false;

                label_deuda_total_mes.Visible = false;

            }




            proveedorBD = (DataTable)Session["proveedorBD"];
            sucursalBD = (DataTable)Session["sucursal"];
            Session.Add("pedidos_fabrica", new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]));
            pedidos_fabrica = (cls_sistema_pedidos_fabrica)Session["pedidos_fabrica"];
            sucursales = pedidos_fabrica.get_sucursales();

            Session.Add("sucursalesBD", sucursales);
            sucursales = (DataTable)Session["sucursalesBD"];




            if (!IsPostBack)
            {

                //calcular remitos nuevos
                Session.Add("sistema_Administracion_fabrica", new cls_sistema_cuentas_por_cobrar((DataTable)Session["usuariosBD"]));

                sistema_Administracion = (cls_sistema_cuentas_por_cobrar)Session["sistema_Administracion_fabrica"];
                // sistema_Administracion.actualizar_remitos();
                configurar_controles();

                configurar_controles_nota_credito();


            }
            sistema_Administracion = (cls_sistema_cuentas_por_cobrar)Session["sistema_Administracion_fabrica"];
            imputacionesBD = sistema_Administracion.get_imputaciones();

            Session.Add("imputacionesBD", imputacionesBD);




            if (Session["sistema_Administracion_fabrica"] == null)
            {
                Session.Add("sistema_Administracion_fabrica", new cls_sistema_cuentas_por_cobrar((DataTable)Session["usuariosBD"]));
            }
            if (Session["sucursal_seleccionada"] == null)
            {
                Session.Add("sucursal_seleccionada", dropDown_sucursales.SelectedItem.Text);
            }
            else
            {
                Session.Remove("sucursal_seleccionada");
                Session.Add("sucursal_seleccionada", dropDown_sucursales.SelectedItem.Text);
            }

            sistema_Administracion = (cls_sistema_cuentas_por_cobrar)Session["sistema_Administracion_fabrica"];
            remitosBD = sistema_Administracion.get_remitos();


            proveedor_seleccionado = proveedorBD.Rows[0]["nombre_proveedor"].ToString();
            nombre_proveedor_seleccionado = proveedorBD.Rows[0]["nombre_en_BD"].ToString();



            if (!IsPostBack)
            {
                cargar_remitos();

            }


        }

        protected void dropDown_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["sucursal_seleccionada"] == null)
            {
                Session.Add("sucursal_seleccionada", dropDown_sucursales.SelectedItem.Text);
            }
            else
            {
                Session.Remove("sucursal_seleccionada");
                Session.Add("sucursal_seleccionada", dropDown_sucursales.SelectedItem.Text);
            }
            cargar_remitos();


        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_remitos();

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_remitos();

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

                sistema_Administracion.crear_pdf(ruta_archivo, gridView_remitos.SelectedRow.Cells[0].Text, proveedor_seleccionado, imgdata, Session["nivel_seguridad"].ToString(), Session["sucursal_seleccionada"].ToString()); //crear_pdf();

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();

            }
        }

        protected void gridView_imputaciones_SelectedIndexChanged(object sender, EventArgs e)
        {



        }

        protected void gridView_remitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_remitos.Rows.Count - 1; fila++)
            {
                int num_pedido;
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
                }
            }
        }

        protected void gridView_imputaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "autorizar")
            {
                string index = e.CommandArgument.ToString();

                string id_imputacion = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                sistema_Administracion.autorizar_imputacion(id_imputacion);
                cargar_saldo();
                imputacionesBD = sistema_Administracion.get_imputaciones();

                cargar_remitos();
            }
            else if (e.CommandName == "cargar_nota")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_imputaciones.Rows[int.Parse(index)].Cells[0].Text;
                TextBox textbox_nota = (gridView_imputaciones.Rows[int.Parse(index)].Cells[8].FindControl("textbox_nota") as TextBox);

                cargar_nota(id, textbox_nota.Text);
                imputacionesBD = sistema_Administracion.get_imputaciones();
                Session.Add("imputacionesBD", imputacionesBD);
                cargar_remitos();

            }
        }
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
            cargar_imputaciones();
            imputacionesBD = sistema_Administracion.get_imputaciones();
            Session.Add("imputacionesBD", imputacionesBD);
            cargar_remitos();
            textBox_efectivo.Text = string.Empty;
            textBox_transferencia.Text = string.Empty;
            textBox_mercadoPago.Text = string.Empty;
            label_total_imputacion.Text = funciones.formatCurrency(0);
        }
        #endregion

        protected void textBox_detalle_TextChanged(object sender, EventArgs e)
        {

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
                    fecha_nota  = fecha_nota + " " + DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString();
                    string cantidad_a_cargar = "-" + cantidad.ToString();
                    sistema_Administracion.cargar_nota_credito(dropDown_sucursales.SelectedItem.Text, textBox_detalle.Text, cantidad_a_cargar, fecha_nota);
                    textBox_monto.Text = string.Empty;
                    textBox_detalle.Text = string.Empty;
                    label_monto.Text = "Total: $0.00";
                }
                remitosBD = sistema_Administracion.get_remitos();
                cargar_remitos();
            }
        }

    }
}
