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
    public partial class caja_chica : System.Web.UI.Page
    {

        #region carga de movimiento
        private void cargar_movimiento()
        {
            double saldo_inicial = 0;
            double final = 0;
            if (verificar_si_se_puede_cargar())
            {
                DateTime fecha_movimiento = (DateTime)movimientosBD.Rows[0]["fecha"]; 
                DateTime fecha_calendario = (DateTime)Session["fecha"]; movimientosBD.Rows[0]["fecha"].ToString();
                if (funciones.verificar_fecha_anterior_con_dia(fecha_calendario.ToString(), fecha_movimiento.Day.ToString(), fecha_movimiento.Month.ToString(), fecha_movimiento.Year.ToString()))
                {
                     saldo_inicial = double.Parse(caja_Chica.get_saldo_en_caja_inicial(Session["fecha_datoBD"].ToString()));
                }
                else
                {
                     saldo_inicial = double.Parse(movimientosBD.Rows[0]["final"].ToString());
                }
                double movimiento = double.Parse(textbox_cantidad.Text.Replace(",", "."));
                if (tipo_movimiento == "Ingreso")
                {
                    final = saldo_inicial + movimiento;
                }
                else if (tipo_movimiento == "Egreso")
                {
                    final = saldo_inicial - movimiento;
                }

                caja_Chica.cargar_movimiento(textbox_detalle.Text, Session["fecha_datoBD"].ToString(), DropDown_conceptos.SelectedItem.Text, tipo_movimiento, saldo_inicial.ToString(), movimiento.ToString(), final.ToString());
                if (funciones.verificar_fecha_anterior_con_dia(fecha_calendario.ToString(), fecha_movimiento.Day.ToString(), fecha_movimiento.Month.ToString(), fecha_movimiento.Year.ToString()))
                {
                    caja_Chica.calcular_total_en_caja();
                }
                textbox_cantidad.Text = string.Empty;
                textbox_detalle.Text = string.Empty;
            }
        }
        private bool verificar_si_se_puede_cargar()
        {
            string mensaje_error = "falta: ";
            bool retorno = true;
            if (Session["fecha_datoBD"].ToString() == "N/A")
            {
                retorno = false;
                mensaje_error += "Cargar fecha / ";
            }
            if (textbox_cantidad.Text == string.Empty)
            {
                retorno = false;
                mensaje_error += "Cargar cantidad / ";

            }
            if (textbox_detalle.Text == string.Empty)
            {
                retorno = false;
                mensaje_error += "Cargar detalle  / ";

            }
            if (tipo_movimiento == "N/A")
            {
                retorno = false;
                mensaje_error += "Cargar Tipo Movimiento";

            }

            if (retorno == false)
            {
                label_mensaje_de_alerta.Text = mensaje_error;
                label_mensaje_de_alerta.Visible = true;
            }
            else
            {
                label_mensaje_de_alerta.Visible = false;
            }
            return retorno;
        }
        #endregion
        private void verificar_cantidad()
        {
            double cantidad;
            if (double.TryParse(textbox_cantidad.Text.Replace(",","."), out cantidad))
            {
                label_cantidad.Text = funciones.formatCurrency(cantidad);
            }
            else
            {
                label_cantidad.Text = funciones.formatCurrency(0);
            }
        }
        #region carga de datos
        private void crear_tabla_movimientos()
        {
            movimientos = new DataTable();
            movimientos.Columns.Add("id", typeof(string));
            movimientos.Columns.Add("detalle", typeof(string));
            movimientos.Columns.Add("fecha", typeof(string));
            movimientos.Columns.Add("concepto", typeof(string));
            movimientos.Columns.Add("tipo_movimiento", typeof(string));
            movimientos.Columns.Add("inicial", typeof(string));
            movimientos.Columns.Add("movimiento", typeof(string));
            movimientos.Columns.Add("final", typeof(string));
        }
        private void llenar_tabla_movimientos()
        {
            crear_tabla_movimientos();
            DateTime fecha;
            int ultima_fila;
            for (int fila = 0; fila <= movimientosBD.Rows.Count - 1; fila++)
            {
                fecha = DateTime.Parse(movimientosBD.Rows[fila]["fecha"].ToString());
                if (fecha.Month.ToString() == dropDown_mes.SelectedItem.Text &&
                    fecha.Year.ToString() == dropDown_año.SelectedItem.Text)
                {
                    movimientos.Rows.Add();
                    ultima_fila = movimientos.Rows.Count - 1;
                    movimientos.Rows[ultima_fila]["id"] = movimientosBD.Rows[fila]["id"].ToString();
                    movimientos.Rows[ultima_fila]["detalle"] = movimientosBD.Rows[fila]["detalle"].ToString();
                    movimientos.Rows[ultima_fila]["fecha"] = fecha.ToString("dd/MM/yyyy");
                    movimientos.Rows[ultima_fila]["concepto"] = movimientosBD.Rows[fila]["concepto"].ToString();
                    movimientos.Rows[ultima_fila]["tipo_movimiento"] = movimientosBD.Rows[fila]["tipo_movimiento"].ToString();
                    movimientos.Rows[ultima_fila]["inicial"] = funciones.formatCurrency(movimientosBD.Rows[fila]["inicial"]);
                    movimientos.Rows[ultima_fila]["movimiento"] = funciones.formatCurrency(movimientosBD.Rows[fila]["movimiento"]);
                    movimientos.Rows[ultima_fila]["final"] = funciones.formatCurrency(movimientosBD.Rows[fila]["final"]);
                }
            }
        }
        private void cargar_datos()
        {
            llenar_tabla_movimientos();
            gridView_movimientos.DataSource = movimientos;
            gridView_movimientos.DataBind();
            double saldo = caja_Chica.get_saldo_actual();
            label_efectivo_en_caja.Text = funciones.formatCurrency(saldo);
            if (saldo > 0)
            {
                label_efectivo_en_caja.CssClass = "badge bg-success";
            }
            else if (saldo == 0)
            {
                label_efectivo_en_caja.CssClass = "badge bg-secondary";

            }
            else if (saldo < 0)
            {
                label_efectivo_en_caja.CssClass = "badge bg-danger";

            }
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            cargar_conceptos();
            cargar_dropDowns();

        }
        private void cargar_conceptos()
        {
            tipo_movimiento = Session["tipo_movimiento"].ToString();
            DropDown_conceptos.Items.Clear();
            for (int fila = 0; fila <= tipo_movimientosBD.Rows.Count - 1; fila++)
            {
                if (tipo_movimientosBD.Rows[fila]["tipo_movimiento"].ToString() == tipo_movimiento &&
                    tipo_movimientosBD.Rows[fila]["estado"].ToString() == "Habilitado")
                {
                    DropDown_conceptos.Items.Add(tipo_movimientosBD.Rows[fila]["concepto"].ToString());
                }
            }
            if (tipo_movimiento == "Ingreso")
            {
                DropDown_conceptos.CssClass = "btn btn-success dropdown-toggle";
            }
            else
            {
                DropDown_conceptos.CssClass = "btn btn-danger dropdown-toggle";
            }
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
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_caja_chica caja_Chica;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_movimientosBD;
        DataTable movimientosBD;
        DataTable movimientos;

        string tipo_movimiento;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["caja_Chica"] == null)
            {
                Session.Add("caja_Chica", new cls_caja_chica(usuariosBD));
            }
            caja_Chica = (cls_caja_chica)Session["caja_Chica"];
            tipo_movimientosBD = caja_Chica.get_tipo_movimientos_caja_chica();
            movimientosBD = caja_Chica.get_movimientos_caja_chica();
            if (!IsPostBack)
            {
                Session.Add("tipo_movimiento", "N/A");
                Session.Add("fecha_seleccionada", "N/A");
                Session.Add("fecha_datoBD", "N/A");
                configurar_controles();
            }
            tipo_movimiento = Session["tipo_movimiento"].ToString();
            label_fecha.Text = "Fecha seleccionada: " + Session["fecha_seleccionada"].ToString();
            cargar_datos();
        }
        protected void DropDown_tipo_movimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            configurar_controles();
        }
        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            Session.Add("fecha_seleccionada", calendario.SelectedDate.ToString("dd/MM/yyyy"));
            Session.Add("fecha_datoBD", calendario.SelectedDate.ToString("yyyy-MM-dd"));
            Session.Add("fecha", calendario.SelectedDate);
            label_fecha.Text = "Fecha seleccionada: " + Session["fecha_seleccionada"].ToString();
        }

        protected void boton_administrar_Click(object sender, EventArgs e)
        {
        }

        protected void boton_administrar_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasGerente/administrar_tipo_movimiento_caja_chica.aspx", false);
        }

        protected void textbox_cantidad_TextChanged(object sender, EventArgs e)
        {
            verificar_cantidad();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            cargar_movimiento();
            movimientosBD = caja_Chica.get_movimientos_caja_chica();
            cargar_datos();
        }

        protected void gridView_movimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_movimientos.Rows.Count - 1; fila++)
            {
                if (gridView_movimientos.Rows[fila].Cells[4].Text == "Ingreso")
                {
                    gridView_movimientos.Rows[fila].CssClass = "table-success";
                }
                else if (gridView_movimientos.Rows[fila].Cells[4].Text == "Egreso")
                {
                    gridView_movimientos.Rows[fila].CssClass = "table-danger";
                }
            }
        }

        protected void gridView_movimientos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gridView_movimientos.SelectedRow.Cells[0].Text;
            caja_Chica.eliminar_movimiento(id);
            movimientosBD = caja_Chica.get_movimientos_caja_chica();
            cargar_datos();
        }

        protected void boton_ingreso_Click(object sender, EventArgs e)
        {
            Session.Add("tipo_movimiento", "Ingreso");
            DropDown_conceptos.Visible = true;
            cargar_conceptos();
        }

        protected void boton_egreso_Click(object sender, EventArgs e)
        {
            Session.Add("tipo_movimiento", "Egreso");
            DropDown_conceptos.Visible = true;
            cargar_conceptos();
        }


    }
}