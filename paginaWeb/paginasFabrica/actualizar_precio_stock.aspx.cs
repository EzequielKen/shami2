using _03___sistemas_fabrica;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class actualizar_precio_stock : System.Web.UI.Page
    {
        private void aumentar_todo_segun_porcentaje()
        {
            double porcentaje_aumento, precio, precio_nuevo, diferencia;
            if (double.TryParse(textbox_porcentaje_aumento.Text.Replace(",", "."), out porcentaje_aumento))
            {
                for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
                {
                    precio = double.Parse(productosBD.Rows[fila]["precio"].ToString());
                    productosBD.Rows[fila]["porcentaje_aumento"] = porcentaje_aumento.ToString() + "%";
                    diferencia = (porcentaje_aumento * precio) / 100;
                    productosBD.Rows[fila]["diferencia"] = diferencia;
                    precio_nuevo = precio + diferencia;
                    productosBD.Rows[fila]["precio_nuevo"] = precio_nuevo.ToString();

                }
            }
        }
        private void cargar_precioStock(string precio_dato, string stock_dato)
        {
            int fila_producto = buscar_fila_producto();
            string dato = stock_dato.Replace(",", ".");
            double stock_actual, nuevo_stock, precio, diferencia, precio_nuevo, porcentaje_aumento;
            if (stock_dato != string.Empty &&
                double.TryParse(dato, out nuevo_stock))
            {
                stock_actual = double.Parse(productosBD.Rows[fila_producto]["stock"].ToString());
                productosBD.Rows[fila_producto]["stock_nuevo"] = nuevo_stock;
            }
            dato = precio_dato.Replace(",", ".");
            if (precio_dato != string.Empty &&
                double.TryParse(dato, out precio_nuevo))
            {
                precio = double.Parse(productosBD.Rows[fila_producto]["precio"].ToString());

                productosBD.Rows[fila_producto]["precio_nuevo"] = precio_nuevo;

                diferencia = precio_nuevo - precio;

                productosBD.Rows[fila_producto]["diferencia"] = diferencia;

                porcentaje_aumento = Math.Round((diferencia * 100) / precio, 2);

                productosBD.Rows[fila_producto]["porcentaje_aumento"] = porcentaje_aumento.ToString() + "%";

            }
            Session.Remove("productosBD");
            Session.Add("productosBD", productosBD);
        }
        private int buscar_fila_producto()
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= productosBD.Rows.Count - 1)
            {
                if (id_producto == productosBD.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        //-----------------------------------------------
        private void crear_tabla_productos()
        {
            productos = new DataTable();

            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("precio", typeof(string));
            productos.Columns.Add("precio_nuevo", typeof(string));
            productos.Columns.Add("diferencia", typeof(string));
            productos.Columns.Add("porcentaje_aumento", typeof(string));
            productos.Columns.Add("stock", typeof(string));
            productos.Columns.Add("stock_nuevo", typeof(string));
            productos.Columns.Add("tipo_producto", typeof(string));
            productos.Columns.Add("unidad_de_medida", typeof(string));
        }
        private void crear_tabla_productosBD()
        {
            productosBD = new DataTable();

            productosBD.Columns.Add("id", typeof(string));
            productosBD.Columns.Add("producto", typeof(string));
            productosBD.Columns.Add("precio", typeof(string));
            productosBD.Columns.Add("precio_nuevo", typeof(string));
            productosBD.Columns.Add("diferencia", typeof(string));
            productosBD.Columns.Add("porcentaje_aumento", typeof(string));
            productosBD.Columns.Add("stock", typeof(string));
            productosBD.Columns.Add("stock_nuevo", typeof(string));
            productosBD.Columns.Add("tipo_producto", typeof(string));
            productosBD.Columns.Add("unidad_de_medida", typeof(string));

            int fila_producto = 0;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count - 1; fila++)
            {
                productosBD.Rows.Add();
                productosBD.Rows[fila_producto]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                productosBD.Rows[fila_producto]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();
                productosBD.Rows[fila_producto]["precio"] = productos_proveedorBD.Rows[fila]["precio"].ToString();
                productosBD.Rows[fila_producto]["stock"] = productos_proveedorBD.Rows[fila]["stock"].ToString();
                productosBD.Rows[fila_producto]["stock_nuevo"] = "N/A";
                productosBD.Rows[fila_producto]["tipo_producto"] = productos_proveedorBD.Rows[fila]["tipo_producto"].ToString();
                productosBD.Rows[fila_producto]["unidad_de_medida"] = productos_proveedorBD.Rows[fila]["unidad_de_medida_fabrica"].ToString();

                productosBD.Rows[fila_producto]["precio_nuevo"] = "0";
                productosBD.Rows[fila_producto]["diferencia"] = "0";


                fila_producto++;
            }
            Session.Add("productosBD", productosBD);
        }
        private void llenar_datatable()
        {
            crear_tabla_productos();
            productosBD = (DataTable)Session["productosBD"];
            tipo_seleccionado = Session["tipo_seleccionado"].ToString();
            int fila_producto = 0;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (tipo_seleccionado == productosBD.Rows[fila]["tipo_producto"].ToString() &&
                    buscar_alguna_coincidencia(textbox_busqueda.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    productos.Rows[fila_producto]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[fila_producto]["producto"] = productosBD.Rows[fila]["producto"].ToString();

                    productos.Rows[fila_producto]["precio"] = formatCurrency(double.Parse(productosBD.Rows[fila]["precio"].ToString()));
                    productos.Rows[fila_producto]["precio_nuevo"] = formatCurrency(double.Parse(productosBD.Rows[fila]["precio_nuevo"].ToString()));
                    productos.Rows[fila_producto]["diferencia"] = formatCurrency(double.Parse(productosBD.Rows[fila]["diferencia"].ToString()));

                    productos.Rows[fila_producto]["porcentaje_aumento"] = productosBD.Rows[fila]["porcentaje_aumento"].ToString();
                    productos.Rows[fila_producto]["stock"] = productosBD.Rows[fila]["stock"].ToString();
                    productos.Rows[fila_producto]["stock_nuevo"] = productosBD.Rows[fila]["stock_nuevo"].ToString();
                    productos.Rows[fila_producto]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[fila_producto]["unidad_de_medida"] = productosBD.Rows[fila]["unidad_de_medida"].ToString();
                    fila_producto++;
                }
            }
        }
        private void cargar_productos()
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            llenar_datatable();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        //------------------------------------------------
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {
                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        private bool buscar_alguna_coincidencia(string dato_usuario, string dato_MySQL)
        {
            int posicion_MySQL, posicion_dato, i;
            bool resultado;
            dato_usuario = dato_usuario.ToUpper();
            dato_MySQL = dato_MySQL.ToUpper();
            resultado = false;
            if (dato_usuario.Length < 1)
            {
                resultado = true;
                return resultado;
            }
            try
            {
                posicion_MySQL = 0;
                posicion_dato = 0;
                while (posicion_MySQL <= dato_MySQL.Length - 2 && dato_usuario.Length > 1)
                {
                    i = posicion_MySQL;
                    if (dato_usuario[0] == dato_MySQL[posicion_MySQL] && dato_usuario[0 + 1] == dato_MySQL[posicion_MySQL + 1])
                    {
                        i = posicion_MySQL;
                        while (posicion_dato <= dato_usuario.Length - 1)
                        {
                            if (dato_usuario[posicion_dato] == dato_MySQL[i])
                            {
                                resultado = true;
                                if (posicion_dato == dato_usuario.Length - 1)
                                {
                                    posicion_MySQL = dato_MySQL.Length;
                                }
                            }
                            else
                            {
                                resultado = false;
                                posicion_dato = dato_usuario.Length;
                                posicion_MySQL = dato_MySQL.Length;
                            }
                            posicion_dato = posicion_dato + 1;
                            i = i + 1;
                        }

                    }
                    posicion_MySQL = posicion_MySQL + 1;
                }
            }
            catch (Exception)
            {

                resultado = false;
            }
            return resultado;
        }
        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
        }

        ///////////////////////////////////////////////////////////////////////////
        cls_sistema_pedidos_fabrica pedidos_fabrica;
        DataTable proveedorBD;
        DataTable sucursalesBD;
        DataTable productos_proveedorBD;
        DataTable productosBD;
        DataTable productos;
        DataTable tipo_usuario;
        int seguridad;
        string tipo_seleccionado;
        string id_producto;

        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            seguridad = int.Parse(Session["nivel_seguridad"].ToString());
            if (seguridad >= 2)
            {
                gridview_productos.Columns[3].Visible = false;
                gridview_productos.Columns[4].Visible = false;
                gridview_productos.Columns[5].Visible = false;
                gridview_productos.Columns[6].Visible = false;
                gridview_productos.Columns[7].Visible = false;
                gridview_productos.Columns[8].Visible = false;

                label_cartel.Visible = false;
                textbox_porcentaje_aumento.Visible = false;
                boton_aumentar_porcentaje.Visible = false;
            }
            else if (seguridad <= 2)
            {
                gridview_productos.Columns[3].Visible = true;
                gridview_productos.Columns[4].Visible = true;
                gridview_productos.Columns[5].Visible = true;
                gridview_productos.Columns[6].Visible = true;
                gridview_productos.Columns[7].Visible = true;
            }
            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                pedidos_fabrica = new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]);

                if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Compras")
                {
                    

                }

                if (Session["sucursalesBD"] == null)
                {
                    sucursalesBD = pedidos_fabrica.get_sucursales();

                    Session.Add("sucursalesBD", sucursalesBD);
                }
                else
                {
                    sucursalesBD = (DataTable)Session["sucursalesBD"];
                }


                if (Session["productos_proveedorBD"] == null)
                {
                    Session.Add("productos_proveedorBD", pedidos_fabrica.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString()));
                    productos_proveedorBD = (DataTable)Session["productos_proveedorBD"];
                }
                else
                {
                    productos_proveedorBD = (DataTable)Session["productos_proveedorBD"];
                }
                if (Session["productosBD"] == null)
                {
                    crear_tabla_productosBD();

                }
                productosBD = (DataTable)Session["productosBD"];
                if (Session["id_producto"] != null)
                {
                    id_producto = Session["id_producto"].ToString();
                }
                if (!IsPostBack)
                {

                    llenar_dropDownList(productos_proveedorBD);
                    cargar_productos();

                }
            }


        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            pedidos_fabrica.actualizar_preciosStock(productosBD, proveedorBD.Rows[0]["nombre_en_BD"].ToString(), tipo_usuario.Rows[0]["rol"].ToString());
            Response.Redirect("~/paginasFabrica/sucursales.aspx", false);


        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_productos();


        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;
            textbox_busqueda.Text = string.Empty;
            cargar_productos();


        }


        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_producto = gridview_productos.SelectedRow.Cells[0].Text;
            Session.Add("id_producto", id_producto);

            TextBox txtValor_precio = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_precio") as TextBox);
            TextBox txtValor_stock = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_Stock") as TextBox);


            cargar_precioStock(txtValor_precio.Text, txtValor_stock.Text);
            cargar_productos();


        }

        protected void boton_aumentar_porcentaje_Click(object sender, EventArgs e)
        {
            aumentar_todo_segun_porcentaje();
            cargar_productos();
            textbox_porcentaje_aumento.Text = string.Empty;


        }
    }
}