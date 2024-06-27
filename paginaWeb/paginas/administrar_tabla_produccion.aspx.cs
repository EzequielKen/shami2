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
    public partial class administrar_tabla_produccion : System.Web.UI.Page
    {
        #region cargar lista
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto_local", typeof(string));
            resumen.Columns.Add("venta_alta", typeof(string));
            resumen.Columns.Add("venta_baja", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            int ultima_fila;

            for (int fila = 0; fila <= lista_productosBD.Rows.Count-1; fila++)
            {
                if (dropDown_tipo.SelectedItem.Text == lista_productosBD.Rows[fila]["tipo_producto_local"].ToString() ||
                    dropDown_tipo.SelectedItem.Text == "todos")
                {
                    resumen.Rows.Add();
                    ultima_fila = resumen.Rows.Count - 1;

                    resumen.Rows[ultima_fila]["id"] = lista_productosBD.Rows[fila]["id"].ToString();
                    resumen.Rows[ultima_fila]["producto"] = lista_productosBD.Rows[fila]["producto"].ToString();
                    resumen.Rows[ultima_fila]["tipo_producto_local"] = lista_productosBD.Rows[fila]["tipo_producto_local"].ToString();

                    resumen.Rows[ultima_fila]["venta_alta"] = lista_productosBD.Rows[fila]["venta_alta"].ToString();
                    resumen.Rows[ultima_fila]["venta_baja"] = lista_productosBD.Rows[fila]["venta_baja"].ToString();
                }
            }
        }

        private void cargar_lista_producto()
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            llenar_tabla_resumen();

            gridview_productos.DataSource = resumen;
            gridview_productos.DataBind();
        }
        #endregion
        #region historial


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
            double porcentaje_turno_1 = 0;
            double porcentaje_turno_2 = 0;
            double porcentaje_total;
            if (total != 0)
            {
                porcentaje_turno_1 = (turno_1 * 100) / total;
                porcentaje_turno_2 = (turno_2 * 100) / total;
            }
            porcentaje_total = porcentaje_turno_1 + porcentaje_turno_2;
            label_total_turno1.Text = "Total Ventas Turno 1: " + funciones.formatCurrency(turno_1);
            label_total_turno2.Text = "Total Ventas Turno 2: " + funciones.formatCurrency(turno_2);
            label_total_ventas.Text = "Total Ventas: " + funciones.formatCurrency(total);

            label_porcentaje_turno_1.Text = "Porcentaje de Venta Diaria Turno 1: %" + Math.Round(porcentaje_turno_1, 2).ToString();
            label_porcentaje_turno_2.Text = "Porcentaje de Venta Diaria Turno 2: %" + Math.Round(porcentaje_turno_2, 2).ToString();
            label_total_porcentaje.Text = "Total Porcentaje de Venta Diaria: %" + Math.Round(porcentaje_total, 2).ToString();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            llenar_dropDownList(lista_productosBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            dt.DefaultView.Sort = "tipo_producto_local";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            dropDown_tipo.Items.Add("todos");
            num_item = num_item + 1;
            tipo_seleccionado = dt.Rows[0]["tipo_producto_local"].ToString();
            dropDown_tipo.Items.Add(dt.Rows[0]["tipo_producto_local"].ToString());
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto_local"].ToString())
                {

                    dropDown_tipo.Items.Add(dt.Rows[fila]["tipo_producto_local"].ToString());
                    num_item = num_item + 1;
                }

            }
        }
            private void configurar_estados_de_dias()
        {
            Session.Add("lunes", false);
            Session.Add("martes", false);
            Session.Add("miercoles", false);
            Session.Add("jueves", false);
            Session.Add("viernes", false);
            Session.Add("sabado", false);
            Session.Add("domingo", false);
        }
        private void configurar_botontes_de_dias()
        {
            if ((bool)Session["lunes"])
            {
                boton_lunes.CssClass = "btn btn-success";
            }
            else
            {
                boton_lunes.CssClass = "btn btn-danger";
            }

            if ((bool)Session["martes"])
            {
                boton_martes.CssClass = "btn btn-success";
            }
            else
            {
                boton_martes.CssClass = "btn btn-danger";
            }

            if ((bool)Session["miercoles"])
            {
                boton_miercoles.CssClass = "btn btn-success";
            }
            else
            {
                boton_miercoles.CssClass = "btn btn-danger";
            }

            if ((bool)Session["jueves"])
            {
                boton_jueves.CssClass = "btn btn-success";
            }
            else
            {
                boton_jueves.CssClass = "btn btn-danger";
            }

            if ((bool)Session["viernes"])
            {
                boton_viernes.CssClass = "btn btn-success";
            }
            else
            {
                boton_viernes.CssClass = "btn btn-danger";
            }

            if ((bool)Session["sabado"])
            {
                boton_sabado.CssClass = "btn btn-success";
            }
            else
            {
                boton_sabado.CssClass = "btn btn-danger";
            }

            if ((bool)Session["domingo"])
            {
                boton_domingo.CssClass = "btn btn-success";
            }
            else
            {
                boton_domingo.CssClass = "btn btn-danger";
            }
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administrar_tabla_produccion tabla_produccion;
        cls_lista_de_chequeo lista_chequeo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;
        DataTable sucursal;
        DataTable tipo_usuario;

        DataTable registro_venta_localBD;
        DataTable lista_productosBD;
        DataTable resumen;


        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            empleado = (DataTable)Session["empleado"];
            sucursal = (DataTable)Session["sucursal"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            if (Session["tabla_produccion"] == null)
            {
                Session.Add("tabla_produccion", new cls_administrar_tabla_produccion(usuariosBD));
            }
            tabla_produccion = (cls_administrar_tabla_produccion)Session["tabla_produccion"];

            registro_venta_localBD = tabla_produccion.get_ventas(sucursal.Rows[0]["id"].ToString());
            sumar_ventas();
            if (!IsPostBack)
            {
                lista_productosBD = tabla_produccion.get_lista_productos();
                Session.Add("lista_productosBD", lista_productosBD);
                configurar_controles();
                configurar_estados_de_dias();
                cargar_lista_producto();

            }
            configurar_botontes_de_dias();
        }

        #region dia de la semana
        protected void boton_lunes_Click(object sender, EventArgs e)
        {
            Session.Add("lunes", !(bool)Session["lunes"]);
            configurar_botontes_de_dias();
        }

        protected void boton_martes_Click(object sender, EventArgs e)
        {
            Session.Add("martes", !(bool)Session["martes"]);
            configurar_botontes_de_dias();
        }

        protected void boton_miercoles_Click(object sender, EventArgs e)
        {
            Session.Add("miercoles", !(bool)Session["miercoles"]);
            configurar_botontes_de_dias();
        }

        protected void boton_jueves_Click(object sender, EventArgs e)
        {
            Session.Add("jueves", !(bool)Session["jueves"]);
            configurar_botontes_de_dias();
        }

        protected void boton_viernes_Click(object sender, EventArgs e)
        {
            Session.Add("viernes", !(bool)Session["viernes"]);
            configurar_botontes_de_dias();
        }

        protected void boton_sabado_Click(object sender, EventArgs e)
        {
            Session.Add("sabado", !(bool)Session["sabado"]);
            configurar_botontes_de_dias();
        }

        protected void boton_domingo_Click(object sender, EventArgs e)
        {
            Session.Add("domingo", !(bool)Session["domingo"]);
            configurar_botontes_de_dias();
        }
        #endregion

        #region lista
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_lista_producto();
        }

        protected void textbox_venta_alta_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_venta_alta = (TextBox)sender;
            if (textbox_venta_alta.Text!=string.Empty)
            {
                double venta;
                if (double.TryParse(textbox_venta_alta.Text,out venta))
                {
                    lista_productosBD = (DataTable)Session["lista_productosBD"];
                    GridViewRow row = (GridViewRow)textbox_venta_alta.NamingContainer;
                    int fila = row.RowIndex;
                    string id = gridview_productos.Rows[fila].Cells[0].Text;
                    int fila_producto = funciones.buscar_fila_por_id(id,lista_productosBD);
                    lista_productosBD.Rows[fila_producto]["venta_alta"] = venta.ToString();
                    Session.Add("lista_productosBD", lista_productosBD);
                }
            }
            cargar_lista_producto();

        }

        protected void textbox_venta_baja_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_venta_baja = (TextBox)sender;
            if (textbox_venta_baja.Text != string.Empty)
            {
                double venta;
                if (double.TryParse(textbox_venta_baja.Text, out venta))
                {
                    lista_productosBD = (DataTable)Session["lista_productosBD"];
                    GridViewRow row = (GridViewRow)textbox_venta_baja.NamingContainer;
                    int fila = row.RowIndex;
                    string id = gridview_productos.Rows[fila].Cells[0].Text;
                    int fila_producto = funciones.buscar_fila_por_id(id, lista_productosBD);
                    lista_productosBD.Rows[fila_producto]["venta_baja"] = venta.ToString();
                    Session.Add("lista_productosBD", lista_productosBD);
                }
            }
            cargar_lista_producto();

        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            string id;
            int fila_producto;
            for (int fila = 0; fila <=gridview_productos.Rows.Count-1; fila++)
            {
                id = gridview_productos.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id,lista_productosBD);
                TextBox textbox_venta_alta = (gridview_productos.Rows[fila].Cells[2].FindControl("textbox_venta_alta") as TextBox);
                TextBox textbox_venta_baja = (gridview_productos.Rows[fila].Cells[3].FindControl("textbox_venta_baja") as TextBox);

                textbox_venta_alta.Text = lista_productosBD.Rows[fila_producto]["venta_alta"].ToString();
                textbox_venta_baja.Text = lista_productosBD.Rows[fila_producto]["venta_baja"].ToString();

                gridview_productos.Rows[fila].Cells[2].CssClass= "table-success";
                gridview_productos.Rows[fila].Cells[3].CssClass= "table-danger";
            }
        }

        #endregion
    }
}