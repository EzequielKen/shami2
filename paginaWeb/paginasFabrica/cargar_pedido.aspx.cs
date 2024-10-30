using _03___sistemas_fabrica;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class cargar_pedido : System.Web.UI.Page
    {
        private void crear_datatable()
        {
            pedido_usuario = new DataTable();
            pedido_usuario.Columns.Clear();
            pedido_usuario.Columns.Add("id", typeof(string));
            pedido_usuario.Columns.Add("producto", typeof(string));
            pedido_usuario.Columns.Add("cantidad_pedida", typeof(string));
            pedido_usuario.Columns.Add("unidad_pedida", typeof(string));
            pedido_usuario.Columns.Add("cantidad_entrega", typeof(string));
            pedido_usuario.Columns.Add("unidad_entrega", typeof(List<string>));
            pedido_usuario.Columns.Add("presentacion_entrega", typeof(List<string>));
            pedido_usuario.Columns.Add("equivalencia", typeof(string));
            pedido_usuario.Columns.Add("presentacion_extraccion", typeof(List<string>));
            pedido_usuario.Columns.Add("precio", typeof(string));
            pedido_usuario.Columns.Add("sub_total", typeof(string));
            pedido_usuario.Columns.Add("stock", typeof(string));
            pedido_usuario.Columns.Add("nuevo_stock", typeof(string));
            pedido_usuario.Columns.Add("proveedor", typeof(string));
            pedido_usuario.Columns.Add("presentacion_entrega_seleccionada", typeof(string));
            pedido_usuario.Columns.Add("presentacion_extraccion_seleccionada", typeof(string));
            pedido_usuario.Columns.Add("id_pedido", typeof(string));
            pedido_usuario.Columns.Add("cantidad_pincho", typeof(string));

            //pedido_usuario.Columns.Add("facturacion por", typeof(string));
            //pedido_usuario.Columns.Add("kilos", typeof(string));
            //pedido_usuario.Columns.Add("factura_por_kilo", typeof(string));

            pedido = (DataTable)Session["pedido"];
            int fila_usuario = 0;
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                pedido_usuario.Rows.Add(fila);
                pedido_usuario.Rows[fila_usuario]["id"] = pedido.Rows[fila]["id"].ToString();
                pedido_usuario.Rows[fila_usuario]["producto"] = pedido.Rows[fila]["producto"].ToString();
                pedido_usuario.Rows[fila_usuario]["cantidad_pedida"] = pedido.Rows[fila]["cantidad_pedida"].ToString();
                pedido_usuario.Rows[fila_usuario]["unidad_pedida"] = pedido.Rows[fila]["unidad_pedida"].ToString();
                pedido_usuario.Rows[fila_usuario]["cantidad_entrega"] = pedido.Rows[fila]["cantidad_entrega"].ToString();
                pedido_usuario.Rows[fila_usuario]["unidad_entrega"] = (List<string>)pedido.Rows[fila]["unidad_entrega"];
                pedido_usuario.Rows[fila_usuario]["presentacion_entrega"] = (List<string>)pedido.Rows[fila]["presentacion_entrega"];
                pedido_usuario.Rows[fila_usuario]["presentacion_extraccion"] = (List<string>)pedido.Rows[fila]["presentacion_extraccion"];
                pedido_usuario.Rows[fila_usuario]["precio"] = formatCurrency(double.Parse(pedido.Rows[fila]["precio"].ToString()));
                pedido_usuario.Rows[fila_usuario]["sub_total"] = formatCurrency(Double.Parse(pedido.Rows[fila]["sub_total"].ToString()));
                pedido_usuario.Rows[fila_usuario]["stock"] = pedido.Rows[fila]["stock"].ToString();
                pedido_usuario.Rows[fila_usuario]["nuevo_stock"] = pedido.Rows[fila]["nuevo_stock"].ToString();
                pedido_usuario.Rows[fila_usuario]["proveedor"] = pedido.Rows[fila]["proveedor"].ToString();
                pedido_usuario.Rows[fila_usuario]["presentacion_entrega_seleccionada"] = pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString();
                pedido_usuario.Rows[fila_usuario]["presentacion_extraccion_seleccionada"] = pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();
                pedido_usuario.Rows[fila_usuario]["id_pedido"] = pedido.Rows[fila]["id_pedido"].ToString();
                pedido_usuario.Rows[fila_usuario]["equivalencia"] = pedido.Rows[fila]["equivalencia"].ToString();
                pedido_usuario.Rows[fila_usuario]["cantidad_pincho"] = pedido.Rows[fila]["cantidad_pincho"].ToString();

                //pedido_usuario.Rows[fila_usuario]["facturacion por"] = pedido.Rows[fila]["facturacion por"].ToString();
                //pedido_usuario.Rows[fila_usuario]["kilos"] = pedido.Rows[fila]["kilos"].ToString();
                //pedido_usuario.Rows[fila_usuario]["factura_por_kilo"] = pedido.Rows[fila]["factura_por_kilo"].ToString();

                fila_usuario++;
            }
            Session.Add("pedido_usuario", pedido_usuario);
        }
        private void calcular_total_pedido()
        {
            pedido = (DataTable)Session["pedido"];
            resumen_de_pedidos = (DataTable)Session["resumen_de_pedidos"];

            double total = 0;
            double sub_total = 0;
            string id_pedido = string.Empty;
            double cantidad_entrega, precio, multiplicador, porcentaje, impuesto;

            for (int fila_resumen = 0; fila_resumen <= resumen_de_pedidos.Rows.Count - 1; fila_resumen++)
            {
                id_pedido = resumen_de_pedidos.Rows[fila_resumen]["id"].ToString();
                sub_total = 0;
                total = 0;
                for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
                {
                    if (id_pedido == pedido.Rows[fila]["id_pedido"].ToString())
                    {
                        sub_total = double.Parse(pedido.Rows[fila]["sub_total"].ToString());
                        total = total + sub_total;
                    }
                }

                resumen_de_pedidos.Rows[fila_resumen]["total_pedido"] = total;
            }
            Session.Add("resumen_de_pedidos", resumen_de_pedidos);
        }
        private bool verificar_carga_a_BD()
        {
            bool retorno = true;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                if (resumen_de_pedidos.Rows[fila]["total_pedido"].ToString() == "0")
                {
                    retorno = false;
                }
            }
            return retorno;
        }
        private string imprimir_totales_pedidos()
        {
            string retorno = string.Empty;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                retorno += "Total pedido N°" + resumen_de_pedidos.Rows[fila]["num_pedido"].ToString() + ": " + funciones.formatCurrency(double.Parse(resumen_de_pedidos.Rows[fila]["total_pedido"].ToString())) + " | ";
            }
            return retorno;
        }
        private void cargar_pedido_abierto()
        {
            crear_datatable();


            gridview_pedido.DataSource = pedido_usuario;
            gridview_pedido.DataBind();

            label_total_pedido.Text = imprimir_totales_pedidos();
            calcular_total_pedido();

        }

        private int buscar_fila_producto()
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= pedido.Rows.Count - 1)
            {
                if (id_producto == pedido.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
        }
        ///////////////////////////////////////////////////////////////////////////

        cls_sistema_pedidos_fabrica pedidos_fabrica;
        cls_funciones funciones = new cls_funciones();
        DataTable sucursalesBD;
        DataTable pedidos_sucursal;
        DataTable pedido;
        DataTable pedido_usuario;
        DataTable proveedorBD;
        DataTable tipo_usuario;
        DataTable usuariosBD;
        DataTable resumen_de_pedidos;

        string mensaje_default = "Seleccione un producto.";
        string proveedor_pedido;
        string id_producto;
        int seguridad;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            seguridad = int.Parse(Session["nivel_seguridad"].ToString());


            if (seguridad > 1)
            {
                label_total_pedido.Visible = false;
                gridview_pedido.Columns[9].Visible = false;
                gridview_pedido.Columns[11].Visible = false;
            }
            else if (seguridad < 2)
            {
                label_total_pedido.Visible = true;
                gridview_pedido.Columns[9].Visible = true;
                gridview_pedido.Columns[11].Visible = true;
            }
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            resumen_de_pedidos = (DataTable)Session["resumen_de_pedidos"];
            if (1 < resumen_de_pedidos.Rows.Count)
            {
                boton_cancelar.Visible = false;
            }
            pedidos_fabrica = new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]);

            if (Session["sucursalesBD"] == null)
            {
                sucursalesBD = pedidos_fabrica.get_sucursales();

                Session.Add("sucursalesBD", sucursalesBD);
            }
            else
            {
                sucursalesBD = (DataTable)Session["sucursalesBD"];
            }




            if (Session["pedidos_sucursal"] == null)
            {
               // Session.Add("pedidos_sucursal", pedidos_fabrica.get_pedidos_sucursal(Session["nombre_sucursal"].ToString(), proveedorBD.Rows[0]["nombre_en_BD"].ToString(), usuariosBD.Rows[0]["proveedor"].ToString()));
            }
            pedidos_sucursal = (DataTable)Session["pedidos_sucursal"];

            if (Session["pedido"] == null)
            {
                Session.Add("pedido", pedidos_fabrica.get_pedido(Session["id_sucursal"].ToString(), Session["num_pedido"].ToString()));
            }
            //label_titulo.Text = pedidos_fabrica.get_titulo_pedido(resumen_de_pedidos);
            //   proveedor_pedido = pedidos_fabrica.get_proveedor_pedido(Session["id_pedido"].ToString(), pedidos_sucursal);
            pedido = (DataTable)Session["pedido"];
            if (!IsPostBack)
            {
                cargar_pedido_abierto();
            }




        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            if (verificar_carga_a_BD())
            {
                string impuesto_carga = "0";
                double impuesto = 0;
                if (double.TryParse(textbox_porcentaje.Text, out impuesto))
                {
                    impuesto_carga = impuesto.ToString();
                }
                pedidos_fabrica.enviar_carga_de_pedido(pedidos_sucursal, pedido, resumen_de_pedidos, tipo_usuario.Rows[0]["rol"].ToString(), impuesto_carga);
                Response.Redirect("/paginasFabrica/sucursales.aspx", false);
            }




        }


        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {

        }



        private void cargar_cantidad(string cantidad_dato, string presentacion_entrega, string presentacion_extraccion, string id_producto_dato, string nombre_producto, string id_pedido)
        {

            id_producto = id_producto_dato;

            int fila_producto = funciones.buscar_fila_por_id_y_nombre(id_producto, nombre_producto, id_pedido, pedido);

            double cantidad, stock, stock_total, cantidad_entrega;
            pedido.Rows[fila_producto]["presentacion_entrega_seleccionada"] = presentacion_entrega;
            pedido.Rows[fila_producto]["presentacion_extraccion_seleccionada"] = presentacion_extraccion;
            double porcentaje, impuesto;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                if ((pedido.Rows[fila_producto]["pincho"].ToString() == "no") || (pedido.Rows[fila_producto]["pincho"].ToString() == "si" && pedido.Rows[fila_producto]["cantidad_pincho"].ToString() != "N/A"))
                {

                    cantidad_entrega = double.Parse(pedido.Rows[fila_producto]["cantidad_entrega"].ToString());
                    //stock = double.Parse(pedido.Rows[fila_producto]["stock"].ToString());
                   // stock_total = stock;
                    double precio = double.Parse(pedido.Rows[fila_producto]["precio"].ToString());
                    double sub_total;
                    if (pedido.Rows[fila_producto]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        double multiplicador = double.Parse(funciones.obtener_dato(presentacion_entrega, 2));
                        precio = precio * multiplicador;
                    }
                    sub_total = cantidad * precio;
                    if (double.TryParse(textbox_porcentaje.Text, out impuesto))
                    {
                        porcentaje = (sub_total * impuesto) / 100;
                        sub_total = sub_total + porcentaje;
                    }
                   // stock_total = stock_total - cantidad;
                    pedido.Rows[fila_producto]["cantidad_entrega"] = cantidad.ToString();
                    pedido.Rows[fila_producto]["sub_total"] = sub_total.ToString();
                  //  pedido.Rows[fila_producto]["nuevo_stock"] = stock_total.ToString();

                    Session.Add("pedido", pedido);
                }
            }
            cargar_pedido_abierto();
        }
        private void cargar_cantidad_pinchos(string id_producto_dato, string nombre_producto, string id_pedido, string cantidad_pincho)
        {
            pedido = (DataTable)Session["pedido"];
            id_producto = id_producto_dato;
            int fila_producto = funciones.buscar_fila_por_id_y_nombre(id_producto, nombre_producto, id_pedido, pedido);
            pedido.Rows[fila_producto]["cantidad_pincho"] = cantidad_pincho;
            Session.Add("pedido", pedido);
            cargar_pedido_abierto();
        }
        private void cargar_presentacion(string id_producto, string presentacion)
        {

            pedido = (DataTable)Session["pedido"];

            int fila_producto = funciones.buscar_fila_por_id(id_producto, pedido);
            pedido.Rows[fila_producto]["presentacion_entrega_seleccionada"] = presentacion;

            Session.Add("pedido", pedido);





            cargar_pedido_abierto();
        }
        private void cargar_presentacion_extraccion(string id_producto, string presentacion)
        {

            pedido = (DataTable)Session["pedido"];

            int fila_producto = funciones.buscar_fila_por_id(id_producto, pedido);
            pedido.Rows[fila_producto]["presentacion_extraccion_seleccionada"] = presentacion;

            Session.Add("pedido", pedido);





            cargar_pedido_abierto();
        }
        protected void boton_cancelar_Click(object sender, EventArgs e)
        {
            pedido = (DataTable)Session["pedido"];
            string id_pedido = pedido.Rows[0]["id_pedido"].ToString();
            pedidos_fabrica.cancelar_pedido(id_pedido);
            Response.Redirect("/paginasFabrica/sucursales.aspx", false);

        }

        protected void gridview_pedido_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            pedido = (DataTable)Session["pedido"];
            int fila_tabla = 0;
            double porcentaje, impuesto;
            for (int fila = 0; fila <= gridview_pedido.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id_y_nombre(gridview_pedido.Rows[fila].Cells[0].Text, gridview_pedido.Rows[fila].Cells[1].Text, gridview_pedido.Rows[fila].Cells[10].Text, pedido);
                cargar_dropdown_presentacion_pedida(fila, fila_tabla, 3, "unidad_entrega", "dropdown_tipo_presentacion");
                cargar_dropdown_presentacion_pedida(fila, fila_tabla, 5, "presentacion_entrega", "dropdown_presentacion_entrega");
                cargar_dropdown_presentacion_pedida(fila, fila_tabla, 6, "presentacion_extraccion", "dropdown_extraido_de");



                double cantidad_entrega = double.Parse(pedido.Rows[fila_tabla]["cantidad_entrega"].ToString());
                double precio = double.Parse(pedido.Rows[fila_tabla]["precio"].ToString());
                double sub_total;
                if (pedido.Rows[fila_tabla]["proveedor"].ToString() == "insumos_fabrica" &&
                    funciones.obtener_dato(pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString(), 2) != "N/A")
                {
                    double multiplicador = double.Parse(funciones.obtener_dato(pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString(), 2));
                    precio = precio * multiplicador;
                }
                else if (pedido.Rows[fila_tabla]["proveedor"].ToString() == "insumos_fabrica")
                {
                    DropDownList dropdown_presentacion_entrega = (gridview_pedido.Rows[fila].Cells[5].FindControl("dropdown_presentacion_entrega") as DropDownList);
                    double multiplicador = double.Parse(funciones.obtener_dato(dropdown_presentacion_entrega.SelectedItem.Text, 2));
                    precio = precio * multiplicador;
                }
                sub_total = cantidad_entrega * precio;
                if (double.TryParse(textbox_porcentaje.Text, out impuesto))
                {
                    porcentaje = (sub_total * impuesto) / 100;
                    sub_total = sub_total + porcentaje;
                }
                pedido.Rows[fila_tabla]["cantidad_entrega"] = cantidad_entrega.ToString();
                pedido.Rows[fila_tabla]["sub_total"] = sub_total.ToString();
                gridview_pedido.Rows[fila].Cells[9].Text = funciones.formatCurrency(precio);

                TextBox Textbox_pinchos = (gridview_pedido.Rows[fila].Cells[4].FindControl("Textbox_pinchos") as TextBox);
                TextBox texbox_cantidad = (gridview_pedido.Rows[fila].Cells[4].FindControl("texbox_cantidad") as TextBox);
                if (pedido.Rows[fila_tabla]["proveedor"].ToString() == "proveedor_villaMaipu")
                {
                    if (pedido.Rows[fila_tabla]["pincho"].ToString() == "no")
                    {
                        Textbox_pinchos.Visible = false;
                    }
                }
                else
                {
                    Textbox_pinchos.Visible = false;

                }
            }
            Session.Add("pedido", pedido);

            label_total_pedido.Text = imprimir_totales_pedidos();
            calcular_total_pedido();
        }



        protected void texbox_cantidad_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_cantidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_cantidad.NamingContainer;
            int rowIndex = row.RowIndex;

            string id_producto = gridview_pedido.Rows[rowIndex].Cells[0].Text;
            DropDownList dropdown_presentacion_entrega = (gridview_pedido.Rows[rowIndex].Cells[5].FindControl("dropdown_presentacion_entrega") as DropDownList);
            DropDownList dropdown_extraido_de = (gridview_pedido.Rows[rowIndex].Cells[6].FindControl("dropdown_extraido_de") as DropDownList);

            cargar_cantidad(texbox_cantidad.Text.Replace(",", "."), dropdown_presentacion_entrega.SelectedItem.Text, dropdown_extraido_de.SelectedItem.Text, id_producto, gridview_pedido.Rows[rowIndex].Cells[1].Text, gridview_pedido.Rows[rowIndex].Cells[10].Text);
        }

        protected void boton_carga_parcial_Click(object sender, EventArgs e)
        {
            pedidos_fabrica.enviar_carga_parcial_de_pedido(pedidos_sucursal, pedido, resumen_de_pedidos, tipo_usuario.Rows[0]["rol"].ToString());
            Response.Redirect("/paginasFabrica/sucursales.aspx", false);
        }

        private void cargar_dropdown_presentacion_pedida(int fila, int fila_tabla, int columna_lista, string nombre_columna, string id_dropdown)
        {
            List<string> lista;

            DropDownList dropdown_tipo_presentacion = (gridview_pedido.Rows[fila].Cells[columna_lista].FindControl(id_dropdown) as DropDownList);

            lista = (List<string>)pedido.Rows[fila_tabla][nombre_columna];
            if (dropdown_tipo_presentacion.Items.Count == 0 && lista.Count>0)
            {
                if (true)
                {
                    //dropdown_tipo_presentacion.Items.Add("N/A");
                    for (int fila_lista = 0; fila_lista <= lista.Count - 1; fila_lista++)
                    {
                        dropdown_tipo_presentacion.Items.Add(lista[fila_lista].ToString());
                    }
                    //  DropDownList dropDownList_unidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[5].FindControl("dropdown_unidad") as DropDownList);
                    //  dropDownList_unidad.SelectedValue = insumos_de_proveedorBD.Rows[fila_tabla]["unidad_medida"].ToString();
                    pedido.Rows[fila_tabla]["presentacion_selecccionada"] = lista[0].ToString();

                    if (id_dropdown == "dropdown_presentacion_entrega")
                    {
                        if (pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString() == "N/A")
                        {
                            dropdown_tipo_presentacion.SelectedValue = lista[0].ToString();
                        }
                        else
                        {
                            dropdown_tipo_presentacion.SelectedValue = pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString();
                        }
                    }
                    else if (id_dropdown == "dropdown_extraido_de")
                    {
                        if (pedido.Rows[fila_tabla]["presentacion_extraccion_seleccionada"].ToString() == "N/A")
                        {
                            dropdown_tipo_presentacion.SelectedValue = lista[0].ToString();
                        }
                        else
                        {
                            dropdown_tipo_presentacion.SelectedValue = pedido.Rows[fila_tabla]["presentacion_extraccion_seleccionada"].ToString();
                        }

                    }
                    else if (id_dropdown == "dropdown_tipo_presentacion")
                    {
                        dropdown_tipo_presentacion.SelectedValue = lista[0].ToString();
                    }
                }
                else
                {
                    dropdown_tipo_presentacion.Items.Add(lista[0].ToString());
                    pedido.Rows[fila_tabla]["presentacion_selecccionada"] = lista[0].ToString();
                }
            }
        }

        protected void dropdown_presentacion_entrega_SelectedIndexChanged(object sender, EventArgs e)
        {
            pedido = (DataTable)Session["pedido"];

            DropDownList dropdown_tipo_presentacion = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_presentacion.NamingContainer;
            int rowIndex = row.RowIndex;
            string id_producto = gridview_pedido.Rows[rowIndex].Cells[0].Text;//cantidad_entrega
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, pedido);
            DropDownList dropdown_extraido_de = (gridview_pedido.Rows[rowIndex].Cells[6].FindControl("dropdown_extraido_de") as DropDownList);

            double cantidad_entrega;
            if (double.TryParse(pedido.Rows[fila_tabla]["cantidad_entrega"].ToString(), out cantidad_entrega))
            {
                cargar_presentacion(id_producto, dropdown_tipo_presentacion.SelectedItem.Text);
                cargar_cantidad(cantidad_entrega.ToString(), pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString(), dropdown_extraido_de.SelectedItem.Text, id_producto, gridview_pedido.Rows[rowIndex].Cells[1].Text, gridview_pedido.Rows[rowIndex].Cells[10].Text);
            }

        }

        protected void dropdown_extraido_de_SelectedIndexChanged(object sender, EventArgs e)
        {
            pedido = (DataTable)Session["pedido"];

            DropDownList dropdown_tipo_presentacion = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_presentacion.NamingContainer;
            int rowIndex = row.RowIndex;
            string id_producto = gridview_pedido.Rows[rowIndex].Cells[0].Text;//cantidad_entrega
            int fila_tabla = funciones.buscar_fila_por_id(id_producto, pedido);
            DropDownList dropdown_extraido_de = (gridview_pedido.Rows[rowIndex].Cells[6].FindControl("dropdown_extraido_de") as DropDownList);

            double cantidad_entrega;
            if (double.TryParse(pedido.Rows[fila_tabla]["cantidad_entrega"].ToString(), out cantidad_entrega))
            {
                cargar_presentacion_extraccion(id_producto, dropdown_tipo_presentacion.SelectedItem.Text);
                cargar_cantidad(cantidad_entrega.ToString(), pedido.Rows[fila_tabla]["presentacion_entrega_seleccionada"].ToString(), dropdown_extraido_de.SelectedItem.Text, id_producto, gridview_pedido.Rows[rowIndex].Cells[1].Text, gridview_pedido.Rows[rowIndex].Cells[10].Text);
            }
        }

        protected void textbox_porcentaje_TextChanged(object sender, EventArgs e)
        {
            double porcentaje;
            if (!double.TryParse(textbox_porcentaje.Text, out porcentaje))
            {
                textbox_porcentaje.Text = string.Empty;
            }
            else
            {
                cargar_pedido_abierto();
            }
        }

        protected void Textbox_pinchos_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_pincho = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_pincho.NamingContainer;
            int rowIndex = row.RowIndex;

            string id_producto = gridview_pedido.Rows[rowIndex].Cells[0].Text;
            double cantidad;
            if (double.TryParse(texbox_pincho.Text, out cantidad))
            {
                cargar_cantidad_pinchos(id_producto, gridview_pedido.Rows[rowIndex].Cells[1].Text, gridview_pedido.Rows[rowIndex].Cells[10].Text, texbox_pincho.Text);
            }
            else
            {
                texbox_pincho.Text = string.Empty;
            }

        }
    }
}