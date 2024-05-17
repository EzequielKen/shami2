using _04___sistemas_carrefour;
using paginaWeb.paginasFabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasCarrefour
{
    public partial class planilla_movimientos : System.Web.UI.Page
    {
        private void registrar_movimiento()
        {
            planilla.registrar_movimiento((DataTable)Session["productos_carrefourBD"], Session["carrefour_seleccionado"].ToString(), Session["fechaBD"].ToString());
            Response.Redirect("~/paginasCarrefour/sucursales_carrefour.aspx", false);
        }
        private void calcular_vendidos(string id)
        {
            double ultimo_stock, devolucion, reposicion, stock_final, vendio;
            for (int fila = 0; fila < productos_carrefourBD.Rows.Count; fila++)
            {
                if (id== productos_carrefourBD.Rows[fila]["id"].ToString())
                {
                    ultimo_stock = double.Parse(productos_carrefourBD.Rows[fila]["ultimo_stock"].ToString());
                    devolucion = double.Parse(productos_carrefourBD.Rows[fila]["devolucion"].ToString());
                    reposicion = double.Parse(productos_carrefourBD.Rows[fila]["reposicion"].ToString());
                    stock_final = double.Parse(productos_carrefourBD.Rows[fila]["stock_final"].ToString());

                    vendio = ultimo_stock - devolucion + reposicion - stock_final;
                    
                    productos_carrefourBD.Rows[fila]["vendio"] = vendio;
                    
                }

            }
        }
        #region cargar tablas
        private void crear_tabla_productos()
        {
            productos_carrefour = new DataTable();
            productos_carrefour.Columns.Add("id", typeof(string));
            productos_carrefour.Columns.Add("producto", typeof(string));
            productos_carrefour.Columns.Add("medida", typeof(string));
            productos_carrefour.Columns.Add("marca", typeof(string));
            productos_carrefour.Columns.Add("RNPA", typeof(string));
            productos_carrefour.Columns.Add("vida_util", typeof(string));
            productos_carrefour.Columns.Add("stock_ideal", typeof(string));

            productos_carrefour.Columns.Add("ultimo_stock", typeof(string));
            productos_carrefour.Columns.Add("devolucion", typeof(string));
            productos_carrefour.Columns.Add("reposicion", typeof(string));
            productos_carrefour.Columns.Add("stock_final", typeof(string));
            productos_carrefour.Columns.Add("vendio", typeof(string));

        }
        private void llenar_tabla_productos()
        {
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila <= productos_carrefourBD.Rows.Count - 1; fila++)
            {
               
                    productos_carrefour.Rows.Add();
                    productos_carrefour.Rows[fila_producto]["id"] = productos_carrefourBD.Rows[fila]["id"].ToString();
                    productos_carrefour.Rows[fila_producto]["producto"] = productos_carrefourBD.Rows[fila]["producto"].ToString();
                    productos_carrefour.Rows[fila_producto]["medida"] = productos_carrefourBD.Rows[fila]["medida"].ToString();
                    productos_carrefour.Rows[fila_producto]["marca"] = productos_carrefourBD.Rows[fila]["marca"].ToString();
                    productos_carrefour.Rows[fila_producto]["RNPA"] = productos_carrefourBD.Rows[fila]["RNPA"].ToString();
                    productos_carrefour.Rows[fila_producto]["vida_util"] = productos_carrefourBD.Rows[fila]["vida_util"].ToString();
                    productos_carrefour.Rows[fila_producto]["stock_ideal"] = productos_carrefourBD.Rows[fila]["stock_ideal"].ToString();

                    productos_carrefour.Rows[fila_producto]["ultimo_stock"] = productos_carrefourBD.Rows[fila]["ultimo_stock"].ToString();
                    productos_carrefour.Rows[fila_producto]["devolucion"] = productos_carrefourBD.Rows[fila]["devolucion"].ToString();
                    productos_carrefour.Rows[fila_producto]["reposicion"] = productos_carrefourBD.Rows[fila]["reposicion"].ToString();
                    productos_carrefour.Rows[fila_producto]["stock_final"] = productos_carrefourBD.Rows[fila]["stock_final"].ToString();
                    productos_carrefour.Rows[fila_producto]["vendio"] = productos_carrefourBD.Rows[fila]["vendio"].ToString();

                    fila_producto++;
            }
        }

        private void cargar_productos()
        {
            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];
            llenar_tabla_productos();
            gridview_productos.DataSource = productos_carrefour;
            gridview_productos.DataBind();
        }
        #endregion
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_planilla_movimientos planilla;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        string id_carrefour_seleccionado;
        DataTable productos_carrefourBD;
        DataTable productos_carrefour;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            id_carrefour_seleccionado = Session["carrefour_seleccionado"].ToString();
            label_sucursal_seleccionada.Text = "Sucursal: "+ Session["sucursal_carrefour_seleccionada"].ToString();
            if (Session["planilla_carrefour"] == null)
            {
                Session.Add("planilla_carrefour", new cls_planilla_movimientos(usuariosBD));
            }
            planilla = (cls_planilla_movimientos)Session["planilla_carrefour"];
            if (!IsPostBack)
            {
                productos_carrefourBD = planilla.get_productos_carrefour(id_carrefour_seleccionado);
                label_fecha_ultima_visita.Text = "Ultima visita: " + planilla.get_fecha_ultima_visita(id_carrefour_seleccionado);
                Session.Add("productos_carrefourBD", productos_carrefourBD);
                cargar_productos();
                label_fecha.Text = DateTime.Now.ToString();
                Session.Add("fechaBD", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }




        #region lista
        protected void textbox_buscar_TextChanged1(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];

            int fila_tabla = 0;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridview_productos.Rows[fila].Cells[0].Text, productos_carrefourBD);

                TextBox textbox_devolucion = (gridview_productos.Rows[fila].Cells[3].FindControl("textbox_devolucion") as TextBox);
                TextBox textbox_reposicion = (gridview_productos.Rows[fila].Cells[3].FindControl("textbox_reposicion") as TextBox);
                TextBox textbox_stock_actual = (gridview_productos.Rows[fila].Cells[3].FindControl("textbox_stock_actual") as TextBox);

                textbox_devolucion.Text = productos_carrefourBD.Rows[fila]["devolucion"].ToString();
                textbox_reposicion.Text = productos_carrefourBD.Rows[fila]["reposicion"].ToString();
                textbox_stock_actual.Text = productos_carrefourBD.Rows[fila]["stock_final"].ToString();

            }
        }

        protected void gridview_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];

            int fila = int.Parse(e.CommandArgument.ToString());

            int fila_tabla = funciones.buscar_fila_por_id(gridview_productos.Rows[fila].Cells[0].Text, productos_carrefourBD);

            double resultado;
            if (e.CommandName == "restar_devolucion")
            {
                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["devolucion"].ToString()) - 1;
                productos_carrefourBD.Rows[fila_tabla]["devolucion"] = resultado.ToString();
            }
            else if (e.CommandName == "sumar_devolucion")
            {

                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["devolucion"].ToString()) + 1;
                productos_carrefourBD.Rows[fila_tabla]["devolucion"] = resultado.ToString();
            }
            else if (e.CommandName == "restar_reposicion")
            {
                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["reposicion"].ToString()) - 1;
                productos_carrefourBD.Rows[fila_tabla]["reposicion"] = resultado.ToString();
            }
            else if (e.CommandName == "sumar_reposicion")
            {
                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["reposicion"].ToString()) + 1;
                productos_carrefourBD.Rows[fila_tabla]["reposicion"] = resultado.ToString();
            }
            else if (e.CommandName == "restar_stock_actual")
            {
                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["stock_final"].ToString()) - 1;
                productos_carrefourBD.Rows[fila_tabla]["stock_final"] = resultado.ToString();
            }
            else if (e.CommandName == "sumar_stock_actual")
            {
                resultado = double.Parse(productos_carrefourBD.Rows[fila_tabla]["stock_final"].ToString()) + 1;
                productos_carrefourBD.Rows[fila_tabla]["stock_final"] = resultado.ToString();
            }
            calcular_vendidos(gridview_productos.Rows[fila].Cells[0].Text);
            Session.Add("productos_carrefourBD", productos_carrefourBD);
            cargar_productos();
        }

        #endregion

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            registrar_movimiento();
        }

        #region text box

        protected void textbox_devolucion_TextChanged(object sender, EventArgs e)
        {

            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;

            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];
            string id_producto = gridview_productos.Rows[rowIndex].Cells[0].Text;
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, productos_carrefourBD);
            TextBox textbox_devolucion = (gridview_productos.Rows[rowIndex].Cells[3].FindControl("textbox_devolucion") as TextBox);
            double cantidad;
            if (double.TryParse(textbox_devolucion.Text,out cantidad))
            {
                productos_carrefourBD.Rows[fila_tabla]["devolucion"] = cantidad.ToString();
            }
            else
            {
                textbox_devolucion.Text = productos_carrefourBD.Rows[fila_tabla]["devolucion"].ToString();
            }
            calcular_vendidos(gridview_productos.Rows[rowIndex].Cells[0].Text);
            Session.Add("productos_carrefourBD", productos_carrefourBD);
            cargar_productos();

        }
        protected void textbox_reposicion_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;

            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];
            string id_producto = gridview_productos.Rows[rowIndex].Cells[0].Text;
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, productos_carrefourBD);
            TextBox textbox_reposicion = (gridview_productos.Rows[rowIndex].Cells[3].FindControl("textbox_reposicion") as TextBox);
            double cantidad;
            if (double.TryParse(textbox_reposicion.Text, out cantidad))
            {
                productos_carrefourBD.Rows[fila_tabla]["reposicion"] = cantidad.ToString();
            }
            else
            {
                textbox_reposicion.Text = productos_carrefourBD.Rows[fila_tabla]["reposicion"].ToString();
            }
            calcular_vendidos(gridview_productos.Rows[rowIndex].Cells[0].Text);
            Session.Add("productos_carrefourBD", productos_carrefourBD);
            cargar_productos();
        }
        protected void textbox_stock_actual_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;

            productos_carrefourBD = (DataTable)Session["productos_carrefourBD"];
            string id_producto = gridview_productos.Rows[rowIndex].Cells[0].Text;
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, productos_carrefourBD);
            TextBox textbox_stock_actual = (gridview_productos.Rows[rowIndex].Cells[3].FindControl("textbox_stock_actual") as TextBox);
            double cantidad;
            if (double.TryParse(textbox_stock_actual.Text, out cantidad))
            {
                productos_carrefourBD.Rows[fila_tabla]["stock_final"] = cantidad.ToString();
            }
            else
            {
                textbox_stock_actual.Text = productos_carrefourBD.Rows[fila_tabla]["stock_final"].ToString();
            }
            calcular_vendidos(gridview_productos.Rows[rowIndex].Cells[0].Text);
            Session.Add("productos_carrefourBD", productos_carrefourBD);
            cargar_productos();
        }
        #endregion

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha.Text = calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).ToString();
            Session.Add("fechaBD", calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}