using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class registro_venta_local : System.Web.UI.Page
    {
        #region historial
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("id_empleado", typeof(string));
            resumen.Columns.Add("nombre", typeof(string));
            resumen.Columns.Add("apellido", typeof(string));
            resumen.Columns.Add("turno", typeof(string));
            resumen.Columns.Add("fecha", typeof(string));
            resumen.Columns.Add("venta", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();

            for (int fila = 0; fila <= registro_venta_localBD.Rows.Count - 1; fila++)
            {
                if (dropdown_turno.SelectedItem.Text == registro_venta_localBD.Rows[fila]["turno"].ToString())
                {
                    resumen.Rows.Add();
                    int ultima_fila = resumen.Rows.Count - 1;
                    resumen.Rows[ultima_fila]["id"] = registro_venta_localBD.Rows[fila]["id"].ToString();
                    resumen.Rows[ultima_fila]["id_empleado"] = registro_venta_localBD.Rows[fila]["id_empleado"].ToString();
                    resumen.Rows[ultima_fila]["nombre"] = registro_venta_localBD.Rows[fila]["nombre"].ToString();
                    resumen.Rows[ultima_fila]["apellido"] = registro_venta_localBD.Rows[fila]["apellido"].ToString();
                    resumen.Rows[ultima_fila]["turno"] = registro_venta_localBD.Rows[fila]["turno"].ToString();
                    resumen.Rows[ultima_fila]["fecha"] = registro_venta_localBD.Rows[fila]["fecha"].ToString();
                    resumen.Rows[ultima_fila]["venta"] = funciones.formatCurrency(double.Parse(registro_venta_localBD.Rows[fila]["venta"].ToString()));
                    resumen.Rows[ultima_fila]["nota"] = registro_venta_localBD.Rows[fila]["nota"].ToString();
                }
            }
        }
        private void caragar_registro_venta_local()
        {
            registro_venta_localBD = (DataTable)Session["registro_venta_localBD"];
            llenar_tabla_resumen();
            gridview_historial.DataSource = resumen;
            gridview_historial.DataBind();
            sumar_ventas();
        }
        private void sumar_ventas()
        {
            double turno_1 = 0;
            double turno_2 = 0;
            double total = 0;

            for (int fila = 0; fila <= registro_venta_localBD.Rows.Count - 1; fila++)
            {
                if (registro_venta_localBD.Rows[fila]["turno"].ToString() == "Turno 1")
                {
                    turno_1 = turno_1 + double.Parse(registro_venta_localBD.Rows[fila]["venta"].ToString());
                }
                if (registro_venta_localBD.Rows[fila]["turno"].ToString() == "Turno 2")
                {
                    turno_2 = turno_2 + double.Parse(registro_venta_localBD.Rows[fila]["venta"].ToString());
                }
            }
            total = turno_1 + turno_2;
            label_total_turno1.Text = "Total Ventas Turno 1: " + funciones.formatCurrency(turno_1);
            label_total_turno2.Text = "Total Ventas Turno 2: " + funciones.formatCurrency(turno_2);
            label_total_ventas.Text = "Total Ventas: " + funciones.formatCurrency(total);
        }
        #endregion
        #region carga a base de datos
        private bool verificar_carga()
        {
            bool retorno = true;
            string mensaje = string.Empty;
            label_alerta.Visible = false;
            if ("N/A" == Session["turno"].ToString())
            {
                mensaje = mensaje + "Falta seleccionar turno.";
                label_alerta.Text = mensaje;
                label_alerta.Visible = true;
                retorno = false;
            }

            if (textbox_venta.Text == string.Empty)
            {
                mensaje = mensaje + " Falta ingresar venta del dia.";
                label_alerta.Text = mensaje;
                label_alerta.Visible = true;
                retorno = false;
            }
            return retorno;
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            string turno = Session["turno"].ToString();
            label_turno.Text = "Turno seleccionado: N/A";

            if (turno == "Turno 1")
            {
                boton_torno_1.CssClass = "btn btn-success";
                label_turno.Text = "Turno seleccionado: Turno 1";
            }
            else
            {
                boton_torno_1.CssClass = "btn btn-primary ";
            }

            if (turno == "Turno 2")
            {
                boton_torno_2.CssClass = "btn btn-success";
                label_turno.Text = "Turno seleccionado: Turno 2";
            }
            else
            {
                boton_torno_2.CssClass = "btn btn-primary ";
            }
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_registro_venta_local registro_venta;
        cls_lista_de_chequeo lista_chequeo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;
        DataTable sucursal;
        DataTable tipo_usuario;
        DataTable registro_venta_localBD;
        DataTable resumen;


        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            empleado = (DataTable)Session["empleado"];
            sucursal = (DataTable)Session["sucursal"];
            tipo_usuario = (DataTable)Session["tipo_usuario"]; 
            if (Session["registro_venta"] == null)
            {
                Session.Add("registro_venta", new cls_registro_venta_local(usuariosBD));
            }
            registro_venta = (cls_registro_venta_local)Session["registro_venta"];


            if (!IsPostBack)
            {
                Session.Add("turno", "N/A");
                Session.Add("fecha_registro_seleccionada_inicio", DateTime.Now);
                Session.Add("fecha_registro_seleccionada_fin", DateTime.Now);
                Session.Add("fecha_registro_seleccionada", DateTime.Now);

                registro_venta_localBD = registro_venta.get_registro_venta_local(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_registro_seleccionada_inicio"], (DateTime)Session["fecha_registro_seleccionada_fin"]);
                Session.Add("registro_venta_localBD", registro_venta_localBD);
                caragar_registro_venta_local();
                DateTime fecha_inicio = (DateTime)Session["fecha_registro_seleccionada_inicio"];
                DateTime fecha_fin = (DateTime)Session["fecha_registro_seleccionada_fin"];
                DateTime fecha_registro_seleccionada = (DateTime)Session["fecha_registro_seleccionada"];
                label_fecha_historial_inicio.Text = "Fecha Inicio: " + fecha_inicio.ToString("dd/MM/yyyy");
                label_fecha_historial_fin.Text = "Fecha Fin: " + fecha_fin.ToString("dd/MM/yyyy");
                label_fecha.Text = "Fecha de registro: " + fecha_registro_seleccionada.ToString("dd/MM/yyyy");
            }
            configurar_controles();
        }


        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected void textbox_venta_TextChanged(object sender, EventArgs e)
        {
            double monto = 0;
            if (double.TryParse(textbox_venta.Text, out monto))
            {
                label_total.Text = "Total: " + funciones.formatCurrency(monto);
            }
            else
            {
                textbox_venta.Text = string.Empty;
                label_total.Text = "Total: $0.00";

            }
        }

        protected void boton_torno_1_Click(object sender, EventArgs e)
        {
            Session.Add("turno", "Turno 1");
            configurar_controles();

        }

        protected void boton_torno_2_Click(object sender, EventArgs e)
        {
            Session.Add("turno", "Turno 2");
            configurar_controles();
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                string nota = "N/A";
                if (textbox_nota.Text != string.Empty)
                {
                    nota = textbox_nota.Text;
                }
                if (tipo_usuario.Rows[0]["rol"].ToString() != "franquiciado")
                {
                    registro_venta.registrar_venta(sucursal, empleado, Session["turno"].ToString(), textbox_venta.Text, nota);
                }
                else if (tipo_usuario.Rows[0]["rol"].ToString() == "franquiciado")
                {
                    registro_venta.registrar_venta_franquiciado(sucursal, (DateTime)Session["fecha_registro_seleccionada"], Session["turno"].ToString(), textbox_venta.Text, nota);
                }

                Session.Add("turno", "N/A");
                textbox_venta.Text = string.Empty;
                textbox_nota.Text = string.Empty;
                configurar_controles();
                registro_venta_localBD = registro_venta.get_registro_venta_local(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_registro_seleccionada_inicio"], (DateTime)Session["fecha_registro_seleccionada_fin"]);

                Session.Add("registro_venta_localBD", registro_venta_localBD);
                caragar_registro_venta_local();
                label_total.Text = "Total: $0.00";
            }
        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_historial.Rows[fila].Cells[0].Text;
            registro_venta.eliminar_registro(id);

            registro_venta_localBD = registro_venta.get_registro_venta_local(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_registro_seleccionada_inicio"], (DateTime)Session["fecha_registro_seleccionada_fin"]);

            Session.Add("registro_venta_localBD", registro_venta_localBD);
            caragar_registro_venta_local();
        }

        protected void dropdown_turno_SelectedIndexChanged(object sender, EventArgs e)
        {
            caragar_registro_venta_local();
        }

        protected void gridview_historial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            registro_venta_localBD = (DataTable)Session["registro_venta_localBD"];
            string id, id_empleado;
            int fila_registro;
            DateTime fecha_registroBD;
            string fecha_registro, fecha_hoy;
            for (int fila = 0; fila <= gridview_historial.Rows.Count - 1; fila++)
            {
                id = gridview_historial.Rows[fila].Cells[0].Text;
                id_empleado = gridview_historial.Rows[fila].Cells[1].Text;
                fila_registro = funciones.buscar_fila_por_id(id, registro_venta_localBD);
                if (fila_registro != -1)
                {
                    fecha_hoy = DateTime.Now.ToString("dd/MM/yyyy");
                    fecha_registroBD = (DateTime)registro_venta_localBD.Rows[fila_registro]["fecha"];
                    fecha_registro = fecha_registroBD.ToString("dd/MM/yyyy");
                    if (empleado != null)
                    {
                        if (fecha_hoy != fecha_registro ||
                            id_empleado != empleado.Rows[0]["id"].ToString())
                        {
                            Button boton_eliminar = (gridview_historial.Rows[fila].Cells[7].FindControl("boton_eliminar") as Button);
                            boton_eliminar.Visible = false;
                        }
                    }
                    else if (id_empleado!="N/A")
                    {
                        Button boton_eliminar = (gridview_historial.Rows[fila].Cells[7].FindControl("boton_eliminar") as Button);
                        boton_eliminar.Visible = false;
                    }

                }
            }
        }

        protected void calendario_inicio_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha_seleccionada = calendario_inicio.SelectedDate;
            Session.Add("fecha_registro_seleccionada_inicio", fecha_seleccionada);

            registro_venta_localBD = registro_venta.get_registro_venta_local(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_registro_seleccionada_inicio"], (DateTime)Session["fecha_registro_seleccionada_fin"]);

            Session.Add("registro_venta_localBD", registro_venta_localBD);
            caragar_registro_venta_local();
            DateTime fecha_inicio = (DateTime)Session["fecha_registro_seleccionada_inicio"];
            DateTime fecha_fin = (DateTime)Session["fecha_registro_seleccionada_fin"];
            label_fecha_historial_inicio.Text = "Fecha Inicio: " + fecha_inicio.ToString("dd/MM/yyyy");
            label_fecha_historial_fin.Text = "Fecha Fin: " + fecha_fin.ToString("dd/MM/yyyy");

        }

        protected void calendario_fin_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha_seleccionada = calendario_fin.SelectedDate;
            Session.Add("fecha_registro_seleccionada_fin", fecha_seleccionada);

            registro_venta_localBD = registro_venta.get_registro_venta_local(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_registro_seleccionada_inicio"], (DateTime)Session["fecha_registro_seleccionada_fin"]);

            Session.Add("registro_venta_localBD", registro_venta_localBD);
            caragar_registro_venta_local();
            DateTime fecha_inicio = (DateTime)Session["fecha_registro_seleccionada_inicio"];
            DateTime fecha_fin = (DateTime)Session["fecha_registro_seleccionada_fin"];
            label_fecha_historial_inicio.Text = "Fecha Inicio: " + fecha_inicio.ToString("dd/MM/yyyy");
            label_fecha_historial_fin.Text = "Fecha Fin: " + fecha_fin.ToString("dd/MM/yyyy");
        }

        protected void calendario_fecha_registro_SelectionChanged(object sender, EventArgs e)
        {
            Session.Add("fecha_registro_seleccionada", calendario_fecha_registro.SelectedDate);
            DateTime fecha_registro_seleccionada = (DateTime)Session["fecha_registro_seleccionada"];
            label_fecha.Text = "Fecha de registro: " + fecha_registro_seleccionada.ToString("dd/MM/yyyy");

        }
    }
}