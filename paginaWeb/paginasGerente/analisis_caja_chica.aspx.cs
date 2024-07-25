using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class analisis_caja_chica : System.Web.UI.Page
    {
        #region carga tablas
        private void crear_tabla_conceptos_ingresos()
        {
            totales_de_conceptos_ingresos = new DataTable();
            totales_de_conceptos_ingresos.Columns.Add("concepto", typeof(string));
            totales_de_conceptos_ingresos.Columns.Add("cantidad_movimiento", typeof(string));
            totales_de_conceptos_ingresos.Columns.Add("total", typeof(string));
        }
        private void llenar_tabla_conceptos_ingresos()
        {
            double total = 0;
            crear_tabla_conceptos_ingresos();
            for (int fila = 0; fila <= totales_de_conceptosBD.Rows.Count - 1; fila++)
            {
                if (totales_de_conceptosBD.Rows[fila]["tipo_movimiento"].ToString() == "Ingreso")
                {
                    totales_de_conceptos_ingresos.Rows.Add();
                    int ultima_fila = totales_de_conceptos_ingresos.Rows.Count - 1;
                    totales_de_conceptos_ingresos.Rows[ultima_fila]["concepto"] = totales_de_conceptosBD.Rows[fila]["concepto"].ToString();
                    totales_de_conceptos_ingresos.Rows[ultima_fila]["cantidad_movimiento"] = totales_de_conceptosBD.Rows[fila]["cantidad_movimiento"].ToString();
                    totales_de_conceptos_ingresos.Rows[ultima_fila]["total"] = funciones.formatCurrency(double.Parse(totales_de_conceptosBD.Rows[fila]["total"].ToString()));
                    total = total + double.Parse(totales_de_conceptosBD.Rows[fila]["total"].ToString());
                }
            }
            label_total_ingresos.Text = funciones.formatCurrency(total);
        }
        private void crear_tabla_conceptos_egresos()
        {
            totales_de_conceptos_egresos = new DataTable();
            totales_de_conceptos_egresos.Columns.Add("concepto", typeof(string));
            totales_de_conceptos_egresos.Columns.Add("cantidad_movimiento", typeof(string));
            totales_de_conceptos_egresos.Columns.Add("total", typeof(string));
        }
        private void llenar_tabla_conceptos_egresos()
        {
            double total = 0;
            crear_tabla_conceptos_egresos();
            for (int fila = 0; fila <= totales_de_conceptosBD.Rows.Count - 1; fila++)
            {
                if (totales_de_conceptosBD.Rows[fila]["tipo_movimiento"].ToString() == "Egreso")
                {
                    totales_de_conceptos_egresos.Rows.Add();
                    int ultima_fila = totales_de_conceptos_egresos.Rows.Count - 1;
                    totales_de_conceptos_egresos.Rows[ultima_fila]["concepto"] = totales_de_conceptosBD.Rows[fila]["concepto"].ToString();
                    totales_de_conceptos_egresos.Rows[ultima_fila]["cantidad_movimiento"] = totales_de_conceptosBD.Rows[fila]["cantidad_movimiento"].ToString();
                    totales_de_conceptos_egresos.Rows[ultima_fila]["total"] = funciones.formatCurrency(double.Parse(totales_de_conceptosBD.Rows[fila]["total"].ToString()));
                    total = total + double.Parse(totales_de_conceptosBD.Rows[fila]["total"].ToString());
                }
            }
            label_total_egresos.Text = funciones.formatCurrency(total);
        }
        private void cargar_totales()
        {
            llenar_tabla_conceptos_ingresos();
            llenar_tabla_conceptos_egresos();
            gridView_conceptos_ingresos.DataSource = totales_de_conceptos_ingresos;
            gridView_conceptos_ingresos.DataBind();
            gridView_conceptos_egresos.DataSource = totales_de_conceptos_egresos;
            gridView_conceptos_egresos.DataBind();
        }
        private void crear_tabla_detalle()
        {
            detalle = new DataTable();
            detalle.Columns.Add("fecha_simple", typeof(string));
            detalle.Columns.Add("detalle", typeof(string));
            detalle.Columns.Add("total", typeof(string));
            detalle.Columns.Add("tipo_movimiento", typeof(string));
        } 

        private void llenar_tabla_detalle(string concepto)
        {
            int ultima_fila;
            crear_tabla_detalle();
            detalleBD = analisis_caja.get_detalles(concepto, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text);
            for (int fila = 0; fila <= detalleBD.Rows.Count-1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_detalle.Text, detalleBD.Rows[fila]["detalle"].ToString()))
                {
                    detalle.Rows.Add();
                    ultima_fila = detalle.Rows.Count-1;

                    detalle.Rows[ultima_fila]["fecha_simple"] = detalleBD.Rows[fila]["fecha_simple"].ToString();
                    detalle.Rows[ultima_fila]["detalle"] = detalleBD.Rows[fila]["detalle"].ToString();
                    detalle.Rows[ultima_fila]["total"] = detalleBD.Rows[fila]["total"].ToString();
                    detalle.Rows[ultima_fila]["tipo_movimiento"] = detalleBD.Rows[fila]["tipo_movimiento"].ToString();
                }
            }
            
        }
        private void cargar_detalle(string concepto)
        {
            llenar_tabla_detalle(concepto);
            gridView_detalle.DataSource = detalle;
            gridView_detalle.DataBind();
        }
        #endregion
        #region configurar controles
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
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_analisis_caja_chica analisis_caja;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable totales_de_conceptosBD;
        DataTable totales_de_conceptos_ingresos;
        DataTable totales_de_conceptos_egresos;
        DataTable detalleBD;
        DataTable detalle;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            
            analisis_caja = new cls_analisis_caja_chica(usuariosBD);
            if (!IsPostBack)
            {
                cargar_dropDowns();
            }
            totales_de_conceptosBD = analisis_caja.get_totales_de_conceptos(dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text);
            cargar_totales();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_totales();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_totales();
        }

       

        protected void gridView_conceptos_ingresos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string concepto = gridView_conceptos_ingresos.SelectedRow.Cells[0].Text;
            Session.Add("concepto_seleccionado", concepto);
            cargar_detalle(concepto);
        }
        protected void gridView_conceptos_egresos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string concepto = gridView_conceptos_egresos.SelectedRow.Cells[1].Text;
            Session.Add("concepto_seleccionado", concepto);
            cargar_detalle(concepto);

        }

        protected void gridView_detalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_detalle.Rows.Count-1; fila++)
            {
                if (gridView_detalle.Rows[fila].Cells[3].Text== "Ingreso")
                {
                    gridView_detalle.Rows[fila].CssClass = "table-success";
                }
                else if (gridView_detalle.Rows[fila].Cells[3].Text == "Egreso")
                {
                    gridView_detalle.Rows[fila].CssClass = "table-danger";
                }
            }
        }

        protected void textbox_detalle_TextChanged(object sender, EventArgs e)
        {
            cargar_detalle(Session["concepto_seleccionado"].ToString());
        }
    }
}