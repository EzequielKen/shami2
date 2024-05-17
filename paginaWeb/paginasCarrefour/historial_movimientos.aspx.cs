using _04___sistemas_carrefour;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasCarrefour
{
    public partial class historial_movimientos : System.Web.UI.Page
    {
        #region carga imputaciones
        private void crear_tabla_imputaciones()
        {
            imputaciones_carrefour = new DataTable();       
            imputaciones_carrefour.Columns.Add("id",typeof(string));
            imputaciones_carrefour.Columns.Add("efectivo", typeof(string));
            imputaciones_carrefour.Columns.Add("transferencia", typeof(string));
            imputaciones_carrefour.Columns.Add("mercado_pago", typeof(string));
            imputaciones_carrefour.Columns.Add("total", typeof(string));
            imputaciones_carrefour.Columns.Add("fecha", typeof(string));
        }
        private void llenar_tabla_imputaciones()
        {
            crear_tabla_imputaciones();
            double total, efectivo, transferencia, mercado_pago;
            int fila_imputaciones=0;
            for (int fila = 0; fila <= imputaciones_carrefourBD.Rows.Count-1; fila++)
            {
                if (funciones.verificar_fecha(imputaciones_carrefourBD.Rows[fila]["fecha"].ToString(),dropDown_mes.SelectedItem.Text,dropDown_año.SelectedItem.Text))
                {
                    efectivo = double.Parse(imputaciones_carrefourBD.Rows[fila]["efectivo"].ToString());
                    transferencia = double.Parse(imputaciones_carrefourBD.Rows[fila]["transferencia"].ToString());
                    mercado_pago = double.Parse(imputaciones_carrefourBD.Rows[fila]["mercado_pago"].ToString());

                    total = efectivo+transferencia+mercado_pago;

                    imputaciones_carrefour.Rows.Add();
                    imputaciones_carrefour.Rows[fila_imputaciones]["id"] = imputaciones_carrefourBD.Rows[fila]["id"].ToString();
                    imputaciones_carrefour.Rows[fila_imputaciones]["efectivo"] = funciones.formatCurrency(efectivo);
                    imputaciones_carrefour.Rows[fila_imputaciones]["transferencia"] = funciones.formatCurrency(transferencia);
                    imputaciones_carrefour.Rows[fila_imputaciones]["mercado_pago"] = funciones.formatCurrency(mercado_pago);
                    imputaciones_carrefour.Rows[fila_imputaciones]["total"] = funciones.formatCurrency(total);
                    imputaciones_carrefour.Rows[fila_imputaciones]["fecha"] = imputaciones_carrefourBD.Rows[fila]["fecha"].ToString();

                    fila_imputaciones++;
                }
            }
        }
        private void cargar_imputaciones()
        {
            imputaciones_carrefourBD = movimientos.get_imputaciones_carrefour(dropDown_sucursales.SelectedItem.Text);
            llenar_tabla_imputaciones();
            gridView_imputaciones.DataSource = imputaciones_carrefour;
            gridView_imputaciones.DataBind();

            label_venta_mes.Text = "Total venta del mes: " + funciones.formatCurrency(movimientos.get_comprado_en_el_mes(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_venta_mes_titulo.Text = "Total venta del mes: " + funciones.formatCurrency(movimientos.get_comprado_en_el_mes(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            
            label_pagado_mes.Text = "Total pagado del mes: " + funciones.formatCurrency(movimientos.get_pagado_en_el_mes(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            label_pagado_mes_titulo.Text = "Total pagado del mes: " + funciones.formatCurrency(movimientos.get_pagado_en_el_mes(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
        
            label_saldo_anterior.Text= "Saldo mes anterior: " + funciones.formatCurrency(movimientos.get_saldo_anterior(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text)); 
            label_saldo.Text= "SALDO: " + funciones.formatCurrency(movimientos.get_saldo_actual(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));

            label_deuda_total_mes.Text = "Deuda total del mes (todas las Sucursales): " + funciones.formatCurrency(movimientos.get_saldo_general());

        }
        #endregion
        #region carga movimientos
        private void crear_tabla_movimientos()
        {
            movimientos_carrefour = new DataTable();
            movimientos_carrefour.Columns.Add("id", typeof(string));
            movimientos_carrefour.Columns.Add("fecha", typeof(DateTime));
            movimientos_carrefour.Columns.Add("valor_venta", typeof(string));
        }
        private void llenar_tabla_movimientos()
        {
            crear_tabla_movimientos();
            int fila_movimientos = 0;
            for (int fila = 0; fila <= movimientos_carrefourBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(movimientos_carrefourBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    movimientos_carrefour.Rows.Add();
                    movimientos_carrefour.Rows[fila_movimientos]["id"] = movimientos_carrefourBD.Rows[fila]["id"].ToString();
                    movimientos_carrefour.Rows[fila_movimientos]["fecha"] = movimientos_carrefourBD.Rows[fila]["fecha"].ToString();
                    movimientos_carrefour.Rows[fila_movimientos]["valor_venta"] = funciones.formatCurrency(double.Parse(movimientos_carrefourBD.Rows[fila]["valor_venta"].ToString()));
                    fila_movimientos++;
                }
            }
        }
        private void cargar_movimientos()
        {
            movimientos_carrefourBD = movimientos.get_movimientos_carrefour(dropDown_sucursales.SelectedItem.Text);
            llenar_tabla_movimientos();
            gridView_movimientos.DataSource = movimientos_carrefour;
            gridView_movimientos.DataBind();

            gridView_resumen.DataSource = null;
            gridView_resumen.DataBind();
        }
        private void cargar_resumen(string id)
        {
            
            DataTable dt = movimientos.get_movimiento_abierto(id);

            gridView_resumen.DataSource = dt;
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
            cargar_proveedores();
            cargar_mes();
            cargar_año();
        }
        private void cargar_proveedores()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            sucursales_carrefour = (DataTable)Session["sucursales_carrefour"];

            for (int fila = 0; fila <= sucursales_carrefour.Rows.Count - 1; fila++)
            {

                item = new System.Web.UI.WebControls.ListItem(sucursales_carrefour.Rows[fila]["sucursal"].ToString(), num_item.ToString());

                dropDown_sucursales.Items.Add(item);


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
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_movimientos_carrefour movimientos;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable sucursales_carrefour;
        DataTable movimientos_carrefourBD;
        DataTable movimientos_carrefour;
        DataTable imputaciones_carrefourBD;
        DataTable imputaciones_carrefour;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["movimientos_carrefour"] == null)
            {
                Session.Add("movimientos_carrefour", new cls_historial_movimientos_carrefour(usuariosBD));
            }
            movimientos = (cls_historial_movimientos_carrefour)Session["movimientos_carrefour"];
            movimientos.actualizar_movimientos_carrefour_no_calculados();
            if (!IsPostBack)
            {
                label_total_imputacion.Text = "Total: " + funciones.formatCurrency(sumar_imputaciones());
                Session.Add("sucursales_carrefour", movimientos.get_sucursales_carrefour());
                configurar_controles();
                cargar_movimientos();
                cargar_imputaciones();
            }
        }

        #region dropdowns
        protected void dropDown_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_movimientos();
            cargar_imputaciones();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_movimientos();
            cargar_imputaciones();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_movimientos();
            cargar_imputaciones();
        }
        #endregion

        #region listas
        protected void gridView_movimientos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gridView_movimientos.SelectedRow.Cells[0].Text;
            cargar_resumen(id);
        }
        protected void gridView_movimientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gridView_imputaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        #endregion

        #region imputaciones
        protected void textBox_efectivo_TextChanged(object sender, EventArgs e)
        {
            label_total_imputacion.Text = "Total: " + funciones.formatCurrency(sumar_imputaciones());
        }

        protected void textBox_transferencia_TextChanged(object sender, EventArgs e)
        {
            label_total_imputacion.Text = "Total: " + funciones.formatCurrency(sumar_imputaciones());
        }

        protected void textBox_mercadoPago_TextChanged(object sender, EventArgs e)
        {
            label_total_imputacion.Text = "Total: " + funciones.formatCurrency(sumar_imputaciones());
        }

        protected void boton_cargarImputacion_Click(object sender, EventArgs e)
        {
            movimientos.cargar_imputacion(dropDown_sucursales.SelectedItem.Text,textBox_efectivo.Text,textBox_transferencia.Text,textBox_mercadoPago.Text);
            textBox_efectivo.Text = "0";
            textBox_transferencia.Text = "0";
            textBox_mercadoPago.Text = "0";
            label_total_imputacion.Text = "Total: " + funciones.formatCurrency(sumar_imputaciones());
        }
        #endregion

        #region sumar imputaciones

        private double sumar_imputaciones()
        {
            double total, efectivo, transferencia, mercadoPago;

            if (double.TryParse(textBox_efectivo.Text, out efectivo))
            {

            }
            else
            {
                efectivo = 0;
            }

            if (double.TryParse(textBox_transferencia.Text, out transferencia))
            {

            }
            else
            {
                efectivo = 0;
            }

            if (double.TryParse(textBox_mercadoPago.Text, out mercadoPago))
            {

            }
            else
            {
                efectivo = 0;
            }

            total = efectivo + transferencia + mercadoPago;
            textBox_efectivo.Text = efectivo.ToString();
            textBox_transferencia.Text = transferencia.ToString();
            textBox_mercadoPago.Text = mercadoPago.ToString();
            return total;
        }

        #endregion

       
    }
}