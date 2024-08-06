using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class tabla_produccion : System.Web.UI.Page
    {
        #region cargar lista
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("id_historial", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto_local", typeof(string));

            resumen.Columns.Add("ventas", typeof(string));
            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("produccion", typeof(string));
            resumen.Columns.Add("objetivo_produccion", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            bool estado_de_dia = tabla_produccion_empleado.get_dia_alto_o_bajo(sucursal.Rows[0]["id"].ToString());
            int ultima_fila;
            double stock, venta, objetivo_produccion;
            for (int fila = 0; fila <= lista_productosBD.Rows.Count - 1; fila++)
            {
                if (dropDown_tipo.SelectedItem.Text == lista_productosBD.Rows[fila]["tipo_producto_local"].ToString() ||
                    dropDown_tipo.SelectedItem.Text == "todos")
                {
                    resumen.Rows.Add();
                    ultima_fila = resumen.Rows.Count - 1;

                    resumen.Rows[ultima_fila]["id"] = lista_productosBD.Rows[fila]["id"].ToString();
                    resumen.Rows[ultima_fila]["id_historial"] = lista_productosBD.Rows[fila]["id_historial"].ToString();
                    resumen.Rows[ultima_fila]["producto"] = lista_productosBD.Rows[fila]["producto"].ToString();
                    resumen.Rows[ultima_fila]["tipo_producto_local"] = lista_productosBD.Rows[fila]["tipo_producto_local"].ToString();
                    resumen.Rows[ultima_fila]["objetivo_produccion"] = lista_productosBD.Rows[fila]["objetivo_produccion"].ToString();


                    if (estado_de_dia)
                    {
                        if (dropdown_turno.SelectedItem.Text == "Turno 1")
                        {
                            resumen.Rows[ultima_fila]["ventas"] = lista_productosBD.Rows[fila]["venta_alta_turno1"].ToString();
                            if (lista_productosBD.Rows[fila]["stock"].ToString() != "N/A")
                            {
                                venta = double.Parse(lista_productosBD.Rows[fila]["venta_alta_turno1"].ToString());
                                stock = double.Parse(lista_productosBD.Rows[fila]["stock"].ToString());
                                objetivo_produccion = venta - stock;
                                resumen.Rows[ultima_fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                            }
                        }
                        else
                        {
                            resumen.Rows[ultima_fila]["ventas"] = lista_productosBD.Rows[fila]["venta_alta_turno2"].ToString();
                            if (lista_productosBD.Rows[fila]["stock"].ToString() != "N/A")
                            {
                                venta = double.Parse(lista_productosBD.Rows[fila]["venta_alta_turno2"].ToString());
                                stock = double.Parse(lista_productosBD.Rows[fila]["stock"].ToString());
                                objetivo_produccion = venta - stock;
                                resumen.Rows[ultima_fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (dropdown_turno.SelectedItem.Text == "Turno 1")
                        {
                            resumen.Rows[ultima_fila]["ventas"] = lista_productosBD.Rows[fila]["venta_baja_turno1"].ToString();
                            if (lista_productosBD.Rows[fila]["stock"].ToString() != "N/A")
                            {
                                venta = double.Parse(lista_productosBD.Rows[fila]["venta_baja_turno1"].ToString());
                                stock = double.Parse(lista_productosBD.Rows[fila]["stock"].ToString());
                                objetivo_produccion = venta - stock;
                                resumen.Rows[ultima_fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                            }
                        }
                        else
                        {
                            resumen.Rows[ultima_fila]["ventas"] = lista_productosBD.Rows[fila]["venta_baja_turno2"].ToString();
                            if (lista_productosBD.Rows[fila]["stock"].ToString() != "N/A")
                            {
                                venta = double.Parse(lista_productosBD.Rows[fila]["venta_baja_turno2"].ToString());
                                stock = double.Parse(lista_productosBD.Rows[fila]["stock"].ToString());
                                objetivo_produccion = venta - stock;
                                resumen.Rows[ultima_fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                            }
                        }
                    }

                }
            }
        }

        private void cargar_lista_producto()
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            if (!(bool)lista_productosBD.Rows[0]["crear_nuevo_registro"])
            {
                llenar_tabla_resumen();

                gridview_productos.DataSource = resumen;
                gridview_productos.DataBind();
            }
            else
            {
                gridview_productos.DataSource = null;
                gridview_productos.DataBind();
            }
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

        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_tabla_produccion tabla_produccion_empleado;
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
        double porcentaje_turno_1 = 0;
        double porcentaje_turno_2 = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            empleado = (DataTable)Session["empleado"];
            sucursal = (DataTable)Session["sucursal"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            
            tabla_produccion_empleado = new cls_tabla_produccion(usuariosBD);

            if (!IsPostBack)
            {
                lista_productosBD = tabla_produccion_empleado.get_lista_productos(sucursal, empleado, dropdown_turno.SelectedItem.Text);
                Session.Add("lista_productosBD", lista_productosBD);
                if (lista_productosBD.Rows.Count > 0)
                {
                    configurar_controles();
                    cargar_lista_producto();
                }
            }
            
        }

        protected void dropdown_turno_SelectedIndexChanged(object sender, EventArgs e)
        {
            lista_productosBD = tabla_produccion_empleado.get_lista_productos(sucursal, empleado, dropdown_turno.SelectedItem.Text);
            Session.Add("lista_productosBD", lista_productosBD);
            if (lista_productosBD.Rows.Count > 0)
            {
                configurar_controles();
                cargar_lista_producto();
            }
            else
            {
                gridview_productos.DataSource = null;
                gridview_productos.DataBind();
            }
            if (dropdown_turno.SelectedItem.Text == "Turno 1")
            {
                dropdown_turno.CssClass = "form-control bg-danger";
            }
            else
            {
                dropdown_turno.CssClass = "form-control bg-success";

            }
        }
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_lista_producto();
        }

        protected void textbox_stock_TextChanged(object sender, EventArgs e)
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            TextBox textbox_stock = (TextBox)sender;
            if (textbox_stock.Text != string.Empty)
            {
                double stock;
                if (double.TryParse(textbox_stock.Text, out stock))
                {
                    lista_productosBD = (DataTable)Session["lista_productosBD"];
                    GridViewRow row = (GridViewRow)textbox_stock.NamingContainer;
                    int fila = row.RowIndex;
                    string id = gridview_productos.Rows[fila].Cells[0].Text;
                    string id_historial = gridview_productos.Rows[fila].Cells[1].Text;
                    int fila_producto = funciones.buscar_fila_por_id(id, lista_productosBD);
                    lista_productosBD.Rows[fila_producto]["stock"] = stock.ToString();
                    Session.Add("lista_productosBD", lista_productosBD);
                    tabla_produccion_empleado.modificar_registro(id_historial, "stock", stock.ToString());

                }
            }
            cargar_lista_producto();
        }

        protected void textbox_produccion_TextChanged(object sender, EventArgs e)
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            TextBox textbox_produccion = (TextBox)sender;
            if (textbox_produccion.Text != string.Empty)
            {
                double produccion;
                if (double.TryParse(textbox_produccion.Text, out produccion))
                {
                    lista_productosBD = (DataTable)Session["lista_productosBD"];
                    GridViewRow row = (GridViewRow)textbox_produccion.NamingContainer;
                    int fila = row.RowIndex;
                    string id = gridview_productos.Rows[fila].Cells[0].Text;
                    string id_historial = gridview_productos.Rows[fila].Cells[1].Text;
                    int fila_producto = funciones.buscar_fila_por_id(id, lista_productosBD);
                    lista_productosBD.Rows[fila_producto]["produccion"] = produccion.ToString();
                    Session.Add("lista_productosBD", lista_productosBD);
                    tabla_produccion_empleado.modificar_registro(id_historial, "produccion", produccion.ToString());
                }
            }
            cargar_lista_producto();
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            lista_productosBD = (DataTable)Session["lista_productosBD"];
            string id;
            int fila_producto;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                id = gridview_productos.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, lista_productosBD);
                TextBox textbox_stock = (gridview_productos.Rows[fila].Cells[2].FindControl("textbox_stock") as TextBox);
                TextBox textbox_produccion = (gridview_productos.Rows[fila].Cells[5].FindControl("textbox_produccion") as TextBox);
                if (lista_productosBD.Rows[fila_producto]["stock"].ToString() != "N/A")
                {
                    textbox_stock.Text = lista_productosBD.Rows[fila_producto]["stock"].ToString();
                }
                if (lista_productosBD.Rows[fila_producto]["produccion"].ToString() != "N/A")
                {
                    textbox_produccion.Text = lista_productosBD.Rows[fila_producto]["produccion"].ToString();
                }

            }
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            string id_tabla_produccion = tabla_produccion_empleado.get_id_tabla_produccion(sucursal.Rows[0]["id"].ToString()); ;
            tabla_produccion_empleado.cargar_registro_tabla_produccion(sucursal, empleado, (DataTable)Session["lista_productosBD"], id_tabla_produccion, dropdown_turno.SelectedItem.Text);
            lista_productosBD = tabla_produccion_empleado.get_lista_productos(sucursal, empleado, dropdown_turno.SelectedItem.Text);
            Session.Add("lista_productosBD", lista_productosBD);
            cargar_lista_producto();
        }
    }
}