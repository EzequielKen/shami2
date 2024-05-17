using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;



namespace paginaWeb.paginas
{
    public partial class administracion : System.Web.UI.Page
    {
        #region funciones
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
                if (lista_proveedores.Rows[fila]["activa"].ToString() == "1")
                {
                    if (lista_proveedores.Rows[fila]["nombre_proveedor"].ToString()!= "Shami Insumos")
                    {

                        item = new System.Web.UI.WebControls.ListItem(lista_proveedores.Rows[fila]["nombre_proveedor"].ToString(), num_item.ToString());

                        dropDown_proveedores.Items.Add(item);


                        num_item++;
                    }
                }
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
        private void crear_datatable()
        {
            remitos = new DataTable();
            remitos.Rows.Clear();
            remitos.Columns.Clear();

            remitos.Columns.Add("id", typeof(string));
            remitos.Columns.Add("num_pedido", typeof(string));
            remitos.Columns.Add("valor_remito", typeof(string));
            remitos.Columns.Add("fecha_remito", typeof(string));

            imputaciones = new DataTable();
            imputaciones.Rows.Clear();
            imputaciones.Columns.Clear();

            imputaciones.Columns.Add("id", typeof(string));
            imputaciones.Columns.Add("Efectivo", typeof(string));
            imputaciones.Columns.Add("Transferencia", typeof(string));
            imputaciones.Columns.Add("Mercado_Pago", typeof(string));
            imputaciones.Columns.Add("Total", typeof(string));
            imputaciones.Columns.Add("Fecha", typeof(string));
            imputaciones.Columns.Add("Autorizado", typeof(bool));
        }
        private void llenar_dataTable()
        {
            crear_datatable();
            remitos.Rows.Clear();
            remitosBD.DefaultView.Sort = "fecha_remito DESC,num_pedido DESC";
            remitosBD = remitosBD.DefaultView.ToTable();
            DataTable sucursalBD = (DataTable)Session["sucursal"];
            int fila_dt = 0;
            for (int fila = 0; fila <= remitosBD.Rows.Count - 1; fila++)
            {
                if (remitosBD.Rows[fila]["sucursal"].ToString() == sucursalBD.Rows[0]["sucursal"].ToString() && 
                    sistema_Administracion.verificar_proveedor(remitosBD.Rows[fila]["proveedor"].ToString(), dropDown_proveedores.SelectedItem.Text, (DataTable)Session["lista_proveedores"]) && 
                    sistema_Administracion.verificar_fecha(remitosBD.Rows[fila]["fecha_remito"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    remitos.Rows.Add();

                    remitos.Rows[fila_dt]["id"] = remitosBD.Rows[fila]["id"].ToString();
                    remitos.Rows[fila_dt]["num_pedido"] = remitosBD.Rows[fila]["num_pedido"].ToString();
                    remitos.Rows[fila_dt]["valor_remito"] = formatCurrency(remitosBD.Rows[fila]["valor_remito"]);
                    remitos.Rows[fila_dt]["fecha_remito"] = formatDate(remitosBD.Rows[fila]["fecha_remito"].ToString());

                    fila_dt++;
                }

            }


            imputacionesBD.DefaultView.Sort = "fecha DESC, id DESC";
            imputacionesBD = imputacionesBD.DefaultView.ToTable();
            fila_dt = 0;
            double efectivo, transferencia, mercadoPago, total;

            for (int fila = 0; fila <= imputacionesBD.Rows.Count - 1; fila++)
            {
                if (imputacionesBD.Rows[fila]["sucursal"].ToString() == sucursalBD.Rows[0]["sucursal"].ToString()
                    && sistema_Administracion.verificar_proveedor(imputacionesBD.Rows[fila]["proveedor"].ToString(), dropDown_proveedores.SelectedItem.Text, (DataTable)Session["lista_proveedores"])
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
                    if (imputacionesBD.Rows[fila]["autorizado"].ToString() == "Si")
                    {
                        imputaciones.Rows[fila_dt]["Autorizado"] = true;
                    }
                    else
                    {
                        imputaciones.Rows[fila_dt]["Autorizado"] = false;
                    }
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
            label_deuda_total_mes.Text = "Deuda total del mes: " + formatCurrency(sistema_Administracion.deuda_total_del_mes(dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));

            label_saldo_anterior.Text = "Deuda meses anteriores: " + formatCurrency(sistema_Administracion.calcular_deuda_mes_anterior(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_saldo.Text = "Deuda actual: " + formatCurrency(sistema_Administracion.calcular_deuda_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes.Text = "Compra del mes: " + formatCurrency(sistema_Administracion.calcular_compra_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_compra_mes_titulo.Text = "Compra del mes: " + formatCurrency(sistema_Administracion.calcular_compra_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes.Text = "Total pagado del mes: " + formatCurrency(sistema_Administracion.calcular_pago_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes_titulo.Text = "Total pagado del mes: " + formatCurrency(sistema_Administracion.calcular_pago_mes(dropDown_proveedores.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
        }
        private void sumar_imputaciones()
        {
            label_total_imputacion.Text = sistema_Administracion.sumar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text);
        }
        private void cargar_imputaciones()
        {
            sistema_Administracion.enviar_imputacion(textBox_efectivo.Text, textBox_transferencia.Text, textBox_mercadoPago.Text, dropDown_proveedores.SelectedItem.Text);

            Session.Remove("imputacionesBD");
            Session.Add("imputacionesBD", sistema_Administracion.get_imputaciones());
            imputacionesBD = (DataTable)Session["imputacionesBD"];

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
        cls_sistema_cuentas_por_pagar sistema_Administracion;
        DataTable lista_proveedores;
        DataTable remitosBD;
        DataTable imputacionesBD;
        DataTable remitos;
        DataTable imputaciones;
        int seguridad;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            seguridad = int.Parse(Session["nivel_seguridad"].ToString());
            if (seguridad > 2)
            {
                label_saldo_anterior.Visible = false;
                label_saldo.Visible = false;
                label_compra_mes.Visible = false;
                label_compra_mes_titulo.Visible = false;
                label_pagado_mes.Visible = false;
                label_pagado_mes_titulo.Visible = false;
                label_deuda_total_mes.Visible = false;

                gridView_remitos.Columns[3].Visible = false;
                gridView_imputaciones.Visible = false;

            }
            else if (seguridad == 2)
            {
                label_saldo_anterior.Visible = false;
                label_saldo.Visible = false;
                label_compra_mes.Visible = false;
                label_pagado_mes.Visible = false;
            }

            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {

                if (!IsPostBack)
                {

                    //calcular remitos nuevos
                    Session.Add("sistema_Administracion", new cls_sistema_cuentas_por_pagar((DataTable)Session["usuariosBD"], (DataTable)Session["sucursal"]));

                    sistema_Administracion = (cls_sistema_cuentas_por_pagar)Session["sistema_Administracion"];
                    sistema_Administracion.actualizar_remitos();
                    configurar_controles();
                }
                sistema_Administracion = (cls_sistema_cuentas_por_pagar)Session["sistema_Administracion"];

                if (Session["remitos"] == null)
                {
                    remitosBD = sistema_Administracion.get_remitos();

                    Session.Add("remitos", remitosBD);
                }
                else
                {
                    remitosBD = (DataTable)Session["remitos"];
                }

                if (Session["imputacionesBD"] == null)
                {
                    imputacionesBD = sistema_Administracion.get_imputaciones();

                    Session.Add("imputacionesBD", imputacionesBD);
                }
                else
                {
                    imputacionesBD = sistema_Administracion.get_imputaciones();

                    Session.Add("imputacionesBD", imputacionesBD);
                    imputacionesBD = (DataTable)Session["imputacionesBD"];
                }

                if (Session["sistema_Administracion"] == null)
                {
                    Session.Add("sistema_Administracion", new cls_sistema_cuentas_por_pagar((DataTable)Session["usuariosBD"], (DataTable)Session["sucursal"]));
                }

                cargar_remitos();
            }


        }

        #region LISTAS
        protected void dropDown_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            if (int.TryParse(gridView_remitos.SelectedRow.Cells[1].Text, out dato))
            {

                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = Session["sucursal"].ToString() + " pedido-" + gridView_remitos.SelectedRow.Cells[1].Text + "- id-" + dato_hora + ".pdf";
                string ruta = "/paginas/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                sistema_Administracion.crear_pdf(ruta_archivo, gridView_remitos.SelectedRow.Cells[0].Text, dropDown_proveedores.SelectedItem.Text, imgdata, Session["nivel_seguridad"].ToString()); //crear_pdf();

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginas/pdf/" + id_pedido;
                try
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

                }
                catch (Exception)
                {

                    Response.Redirect(strUrl, false);
                }
                //GenerarPDF_Click();

            }



        }
        #endregion

        #region TEXT BOX
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
        #endregion

        protected void boton_cargarImputacion_Click(object sender, EventArgs e)
        {
            cargar_imputaciones();
            imputacionesBD = sistema_Administracion.get_imputaciones();
            cargar_remitos();
            textBox_efectivo.Text = string.Empty;
            textBox_transferencia.Text = string.Empty;
            textBox_mercadoPago.Text = string.Empty;
            label_total_imputacion.Text = formatCurrency(0);
            //cargar_remitos();


        }

        protected void gridView_remitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_remitos.Rows.Count - 1; fila++)
            {
                int num_pedido;
                if (int.TryParse(gridView_remitos.Rows[fila].Cells[1].Text, out num_pedido))
                {
                    if (num_pedido == 0)
                    {
                        gridView_remitos.Rows[fila].Cells[4].Controls[0].Visible = false;
                    }
                }
                else
                {
                    gridView_remitos.Rows[fila].Cells[4].Controls[0].Visible = false;
                }
            }
        }
    }
}