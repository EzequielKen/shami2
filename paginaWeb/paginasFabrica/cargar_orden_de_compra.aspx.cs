using _03___sistemas_fabrica;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class cargar_orden_de_compra : System.Web.UI.Page
    {
        #region carga a base de datos
        private bool verificar_fecha_factura()
        {
            bool retorno = false;
            if (textbox_dia.Text != string.Empty &&
                textbox_mes.Text != string.Empty &&
                textbox_año.Text != string.Empty)
            {
                retorno = true;
            }
            return retorno;
        }
        private string obtener_fecha_entrega()
        {
            string retorno = "N/A";
            if (textbox_dia.Text == string.Empty ||
               textbox_mes.Text == string.Empty ||
               textbox_año.Text == string.Empty)
            {
                retorno = "N/A";
            }
            else if (textbox_dia.Text != string.Empty &&
                textbox_mes.Text != string.Empty &&
                textbox_año.Text != string.Empty)
            {
                retorno = textbox_año.Text + "-" + textbox_mes.Text + "-" + textbox_dia.Text;
                DateTime fecha = DateTime.Parse(retorno);

                fecha = fecha.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                retorno = fecha.ToString("yyyy-MM-dd hh-mm-ss");
            }
            return retorno;
        }
        private void actualizar_orden_de_compra()
        {

            num_orden_de_compra_seleccionada = Session["num_orden_de_compra_seleccionada"].ToString();
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            string id_proveedor = proveedores_de_fabrica_seleccionado.Rows[0]["id"].ToString();
            proveedorBD = (DataTable)Session["proveedorBD"];
            string nombre_fabrica = proveedorBD.Rows[0]["nombre_proveedor"].ToString();
            nombre_proveedores_de_fabrica_seleccionado = Session["proveedores_de_fabrica_seleccionado"].ToString();
            string valor_orden = calcular_total_con_impuestos().ToString();
            string condicion_pago = orden_de_compraBD.Rows[0]["condicion_pago"].ToString();


            orden_compra.actualizar_orden_de_compra(num_orden_de_compra_seleccionada, orden_de_compraBD, id_proveedor, valor_orden, nombre_fabrica, nombre_proveedores_de_fabrica_seleccionado, Session["total_impuestos"].ToString(), tipo_usuario.Rows[0]["rol"].ToString(), Session["estado_pedido"].ToString(), condicion_pago, obtener_fecha_entrega(), "Recibido");

        }
        private void actualizar_orden_de_compra_entrega_parcial()
        {
            num_orden_de_compra_seleccionada = Session["num_orden_de_compra_seleccionada"].ToString();
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            string id_proveedor = proveedores_de_fabrica_seleccionado.Rows[0]["id"].ToString();
            proveedorBD = (DataTable)Session["proveedorBD"];
            string nombre_fabrica = proveedorBD.Rows[0]["nombre_proveedor"].ToString();
            nombre_proveedores_de_fabrica_seleccionado = Session["proveedores_de_fabrica_seleccionado"].ToString();
            string valor_orden = calcular_total_con_impuestos().ToString();
            string condicion_pago = orden_de_compraBD.Rows[0]["condicion_pago"].ToString();

            orden_compra.actualizar_orden_de_compra_entrega_parcial(num_orden_de_compra_seleccionada, orden_de_compraBD, id_proveedor, valor_orden, nombre_fabrica, nombre_proveedores_de_fabrica_seleccionado, Session["total_impuestos"].ToString(), tipo_usuario.Rows[0]["rol"].ToString(), condicion_pago, "Entrega parcial");

        }
        #endregion
        #region calculos
        private double calcular_total()
        {
            string estado_pedido = Session["estado_pedido"].ToString();
            double total = 0;
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            for (int fila = 0; fila <= orden_de_compraBD.Rows.Count - 1; fila++)
            {
                if (orden_de_compraBD.Rows[fila]["sub_total"].ToString() != "N/A")
                {

                    total = total + double.Parse(orden_de_compraBD.Rows[fila]["sub_total"].ToString());

                }
            }
            return Math.Round(total, 2);
        }
        private double calcular_total_con_impuestos()
        {
            string estado_pedido = Session["estado_pedido"].ToString();
            double total = 0;
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            for (int fila = 0; fila <= orden_de_compraBD.Rows.Count - 1; fila++)
            {
                if (orden_de_compraBD.Rows[fila]["sub_total"].ToString() != "N/A")
                {

                    total = total + double.Parse(orden_de_compraBD.Rows[fila]["sub_total"].ToString());

                }
            }
            double impuestos;
            total_impuestos = Session["total_impuestos"].ToString();
            if (total_impuestos != "N/A")
            {
                impuestos = double.Parse(total_impuestos);
            }
            else
            {
                impuestos = 0;

            }
            total = total + impuestos;
            return Math.Round(total, 2);
        }
        #endregion
        #region cargar
        private void cargar_cantidad_precio(string id_producto, string cantidad_dato, string precio_dato)
        {
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            int fila_producto = funciones.buscar_fila_por_id(id_producto, orden_de_compraBD);
            double cantidad, precio, cantidad_actal, cantidad_entrega, sub_total;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                cantidad_actal = 0;
                string estado_pedido = Session["estado_pedido"].ToString();
                if (estado_pedido != "Entrega Parcial")
                {
                    orden_de_compraBD.Rows[fila_producto]["cantidad_entrega"] = cantidad.ToString();// "N/A";
                }
                else
                {
                    if (orden_de_compraBD.Rows[fila_producto]["cantidad_entrega"].ToString() == "N/A")
                    {
                        cantidad_actal = 0;
                    }
                    else
                    {
                        cantidad_actal = double.Parse(orden_de_compraBD.Rows[fila_producto]["cantidad_entrega"].ToString());
                    }

                }
                orden_de_compraBD.Rows[fila_producto]["total_entrega"] = cantidad_actal + cantidad;

                precio = double.Parse(orden_de_compraBD.Rows[fila_producto]["precio"].ToString());

                cantidad_entrega = double.Parse(orden_de_compraBD.Rows[fila_producto]["total_entrega"].ToString());

                sub_total = cantidad_entrega * precio;
                orden_de_compraBD.Rows[fila_producto]["sub_total"] = Math.Round(sub_total);


            }


            if (double.TryParse(precio_dato, out precio))
            {
                double cantidad_unidades = double.Parse(orden_de_compraBD.Rows[fila_producto]["cantidad_unidades"].ToString());
                double nuevo_precio_unidad = precio / cantidad_unidades;
                orden_de_compraBD.Rows[fila_producto]["nuevo_precio"] = precio.ToString();// "N/A";
                orden_de_compraBD.Rows[fila_producto]["nuevo_precio_unidad"] = nuevo_precio_unidad.ToString();// "N/A";
            }


            Session.Add("orden_de_compra", orden_de_compraBD);
        }
        #endregion
        #region llenar tabla
        private void crear_tabla_orden()
        {
            orden_de_compra = new DataTable();
            orden_de_compra.Columns.Add("id", typeof(string));
            orden_de_compra.Columns.Add("producto", typeof(string));
            orden_de_compra.Columns.Add("cantidad_pedida", typeof(string));
            orden_de_compra.Columns.Add("cantidad_entrega", typeof(string));
            orden_de_compra.Columns.Add("total_entrega", typeof(string));
            orden_de_compra.Columns.Add("precio", typeof(string));
            orden_de_compra.Columns.Add("nuevo_precio", typeof(string));
            orden_de_compra.Columns.Add("sub_total", typeof(string));
            orden_de_compra.Columns.Add("stock", typeof(string));
            orden_de_compra.Columns.Add("nuevo_stock", typeof(string));
            orden_de_compra.Columns.Add("unidad_pedida", typeof(string));
            orden_de_compra.Columns.Add("dato", typeof(string));

            orden_de_compra.Columns.Add("tipo_paquete", typeof(string));
            orden_de_compra.Columns.Add("cantidad_unidades", typeof(string));
            orden_de_compra.Columns.Add("unidad_medida", typeof(string));
        }
        private void llenar_tabla_orden()
        {
            crear_tabla_orden();
            string estado_pedido = Session["estado_pedido"].ToString();

            double cantidad, precio, stock, cantidad_a_sumar;
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            int fila_orden = 0;
            for (int fila = 0; fila <= orden_de_compraBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, orden_de_compraBD.Rows[fila]["producto"].ToString()))
                {
                    orden_de_compra.Rows.Add();
                    //id
                    orden_de_compra.Rows[fila_orden]["id"] = orden_de_compraBD.Rows[fila]["id"].ToString();
                    //producto
                    orden_de_compra.Rows[fila_orden]["producto"] = orden_de_compraBD.Rows[fila]["producto"].ToString();// producto;
                    //cantidad_pedida
                    orden_de_compra.Rows[fila_orden]["cantidad_pedida"] = orden_de_compraBD.Rows[fila]["cantidad_pedida"].ToString();// cantidad_pedida;

                    //cantidad_entrega
                    orden_de_compra.Rows[fila_orden]["cantidad_entrega"] = orden_de_compraBD.Rows[fila]["cantidad_entrega"].ToString();// "N/A";
                    //total_entrega
                    orden_de_compra.Rows[fila_orden]["total_entrega"] = orden_de_compraBD.Rows[fila]["total_entrega"].ToString();// "N/A";
                    //precio
                    orden_de_compra.Rows[fila_orden]["precio"] = funciones.formatCurrency(double.Parse(orden_de_compraBD.Rows[fila]["precio"].ToString())); //precio;

                    //nuevo_precio
                    if (orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                    {
                        orden_de_compra.Rows[fila_orden]["nuevo_precio"] = funciones.formatCurrency(double.Parse(orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString()));// "N/A";
                    }
                    else
                    {
                        orden_de_compra.Rows[fila_orden]["nuevo_precio"] = orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString();// "N/A";
                    }
                    //sub_total
                    if (orden_de_compraBD.Rows[fila]["sub_total"].ToString() != "N/A")
                    {
                        orden_de_compra.Rows[fila_orden]["sub_total"] = funciones.formatCurrency(double.Parse(orden_de_compraBD.Rows[fila]["sub_total"].ToString())); //"N/A";
                    }
                    else
                    {
                        orden_de_compra.Rows[fila_orden]["sub_total"] = orden_de_compraBD.Rows[fila]["sub_total"].ToString(); //"N/A";
                    }

                    //stock
                    orden_de_compra.Rows[fila_orden]["stock"] = orden_de_compraBD.Rows[fila]["stock"].ToString();// stock;

                    //nuevo_stock
                    orden_de_compra.Rows[fila_orden]["nuevo_stock"] = orden_de_compraBD.Rows[fila]["nuevo_stock"].ToString(); //"N/A";

                    //tipo_paquete
                    orden_de_compra.Rows[fila_orden]["tipo_paquete"] = orden_de_compraBD.Rows[fila]["tipo_paquete"].ToString();// unidad_pedida;
                    //cantidad_unidades
                    orden_de_compra.Rows[fila_orden]["cantidad_unidades"] = orden_de_compraBD.Rows[fila]["cantidad_unidades"].ToString();// unidad_pedida;
                    //unidad_medida
                    orden_de_compra.Rows[fila_orden]["unidad_medida"] = orden_de_compraBD.Rows[fila]["unidad_medida"].ToString();// unidad_pedida;

                    //unidad_pedida
                    orden_de_compra.Rows[fila_orden]["unidad_pedida"] = orden_de_compraBD.Rows[fila]["unidad_pedida"].ToString();// unidad_pedida;

                    //dato
                    orden_de_compra.Rows[fila_orden]["dato"] = orden_de_compraBD.Rows[fila]["id"].ToString();// dato;

                    if (estado_pedido == "Entrega Parcial")
                    {
                        if (orden_de_compraBD.Rows[fila]["total_entrega"].ToString() != "N/A")
                        {
                            cantidad = double.Parse(orden_de_compraBD.Rows[fila]["total_entrega"].ToString());
                            if (orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                            {
                                precio = double.Parse(orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString());
                            }
                            else
                            {
                                precio = double.Parse(orden_de_compraBD.Rows[fila]["precio"].ToString());
                            }
                            orden_de_compraBD.Rows[fila]["sub_total"] = Math.Round(cantidad * precio, 2);
                            orden_de_compra.Rows[fila_orden]["sub_total"] = funciones.formatCurrency(double.Parse(orden_de_compraBD.Rows[fila]["sub_total"].ToString())); //"N/A";
                            stock = double.Parse(orden_de_compraBD.Rows[fila]["stock"].ToString());
                            if (Session["estado_orden_de_compra"].ToString() == "Abierta" ||
                                estado_pedido == "Entrega Parcial")
                            {
                                if (orden_de_compraBD.Rows[fila]["cantidad_entrega"].ToString() == "N/A")
                                {
                                    cantidad_a_sumar = cantidad;
                                }
                                else
                                {
                                    cantidad_a_sumar = cantidad - double.Parse(orden_de_compraBD.Rows[fila]["cantidad_entrega"].ToString());
                                }
                                orden_de_compraBD.Rows[fila]["nuevo_stock"] = Math.Round(stock + cantidad_a_sumar, 2).ToString();
                                orden_de_compra.Rows[fila_orden]["nuevo_stock"] = orden_de_compraBD.Rows[fila]["nuevo_stock"].ToString(); //"N/A";
                            }
                        }
                    }
                    else
                    {
                        if (orden_de_compraBD.Rows[fila]["cantidad_entrega"].ToString() != "N/A")
                        {
                            cantidad = double.Parse(orden_de_compraBD.Rows[fila]["cantidad_entrega"].ToString());
                            if (orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                            {
                                precio = double.Parse(orden_de_compraBD.Rows[fila]["nuevo_precio"].ToString());
                            }
                            else
                            {
                                precio = double.Parse(orden_de_compraBD.Rows[fila]["precio"].ToString());
                            }
                            orden_de_compraBD.Rows[fila]["sub_total"] = Math.Round(cantidad * precio, 2);
                            orden_de_compra.Rows[fila_orden]["sub_total"] = funciones.formatCurrency(double.Parse(orden_de_compraBD.Rows[fila]["sub_total"].ToString())); //"N/A";
                            stock = double.Parse(orden_de_compraBD.Rows[fila]["stock"].ToString());
                            if (Session["estado_orden_de_compra"].ToString() == "Abierta")
                            {
                                orden_de_compraBD.Rows[fila]["nuevo_stock"] = Math.Round(stock + cantidad, 2).ToString();
                                orden_de_compra.Rows[fila_orden]["nuevo_stock"] = orden_de_compraBD.Rows[fila]["nuevo_stock"].ToString(); //"N/A";
                            }
                        }
                    }
                    fila_orden++;
                }
            }
        }
        #endregion
        #region funciones
        private void cargar_orden_compra()
        {
            llenar_tabla_orden();

            gridview_pedido.DataSource = orden_de_compra;
            gridview_pedido.DataBind();

            label_total.Text = "Sub Total: " + funciones.formatCurrency(calcular_total());

            if ("N/A" != Session["total_impuestos"].ToString())
            {
                label_total_con_impuestos.Visible = true;
                label_total_con_impuestos.Text = "Total con impuestos: " + funciones.formatCurrency(calcular_total_con_impuestos());
                label_total_impuesto.Text = "Impuestos varios: " + funciones.formatCurrency(double.Parse(Session["total_impuestos"].ToString()));
            }
            else
            {
                label_total_con_impuestos.Visible = false;
                label_total_impuesto.Text = "Impuestos varios: " + Session["total_impuestos"].ToString();
            }

        }
        private void cargar_impuestos(string cantidad_dato)
        {
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                if (cantidad == 0)
                {
                    total_impuestos = "N/A";
                    Session.Add("total_impuestos", total_impuestos);
                }
                else
                {
                    total_impuestos = cantidad.ToString();
                    Session.Add("total_impuestos", total_impuestos);
                }
            }
            textbox_impuestos.Text = string.Empty;
        }
        #endregion
        //num_orden_de_compra_seleccionada
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_cargar_orden_de_compra orden_compra;
        cls_historial_orden_de_compras historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;

        DataTable proveedores_de_fabrica_seleccionado;
        DataTable orden_de_compraBD;
        DataTable orden_de_compra;
        string nombre_proveedores_de_fabrica_seleccionado;
        string num_orden_de_compra_seleccionada;
        string total_impuestos = "N/A";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            nombre_proveedores_de_fabrica_seleccionado = Session["proveedores_de_fabrica_seleccionado"].ToString();
            num_orden_de_compra_seleccionada = Session["num_orden_de_compra_seleccionada"].ToString();

            usuariosBD = (DataTable)Session["usuariosBD"];
            string estado_pedido = Session["estado_pedido"].ToString();
            if (estado_pedido != "Entrega Parcial")
            {
                gridview_pedido.Columns[5].Visible = false;
            }

            if (proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
            {
                label_total.Visible = false;

                textbox_impuestos.Visible = false;
                boton_cargar_impuestos.Visible = false;
                label_total_impuesto.Visible = false;
                gridview_pedido.Columns[7].Visible = false;//5 
                gridview_pedido.Columns[8].Visible = false;//5 
                gridview_pedido.Columns[9].Visible = false;//5 

                gridview_pedido.Columns[10].Visible = false;//8
            }
            // gridview_pedido.Columns[7].Visible = false;//6
            //  gridview_pedido.Columns[8].Visible = false;//7

            if (Session["historial_orden"] == null)
            {
                Session.Add("historial_orden", new cls_historial_orden_de_compras(usuariosBD));
            }
            historial = (cls_historial_orden_de_compras)Session["historial_orden"];
            if (Session["orden_compra"] == null)
            {
                Session.Add("orden_compra", new cls_cargar_orden_de_compra(usuariosBD));
            }
            orden_compra = (cls_cargar_orden_de_compra)Session["orden_compra"];

            if (!IsPostBack)
            {
                proveedores_de_fabrica_seleccionado = historial.get_proveedores_de_fabrica_seleccionado(nombre_proveedores_de_fabrica_seleccionado);
                Session.Add("proveedores_de_fabrica_seleccionado", proveedores_de_fabrica_seleccionado);
            }
            proveedores_de_fabrica_seleccionado = (DataTable)Session["proveedores_de_fabrica_seleccionado"];
            label_num_orden_de_compra.Text = "N° orden de compra: " + num_orden_de_compra_seleccionada;
            label_proveedor_de_fabrica_seleeccionado.Text = proveedores_de_fabrica_seleccionado.Rows[0]["proveedor"].ToString();
            if (!IsPostBack)
            {
                Session.Add("total_impuestos", total_impuestos);

                orden_de_compraBD = orden_compra.get_orden_de_compra(num_orden_de_compra_seleccionada);
                Session.Add("orden_de_compra", orden_de_compraBD);
                Session.Add("estado_orden_de_compra", orden_compra.get_estado_orden_de_compra(num_orden_de_compra_seleccionada));
               
                cargar_orden_compra();
            }
            if (Session["estado_orden_de_compra"].ToString() == "Recibido" &&
                    proveedorBD.Rows[0]["nombre_en_BD"].ToString() == "proveedor_villaMaipu" &&
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
            {
                gridview_pedido.Columns[3].Visible = false;//5 
            }
            total_impuestos = Session["total_impuestos"].ToString();
        }

        protected void gridview_pedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox texbox_cantidad = (gridview_pedido.SelectedRow.Cells[3].FindControl("texbox_cantidad") as TextBox);
            TextBox texbox_nuevo_precio = (gridview_pedido.SelectedRow.Cells[6].FindControl("texbox_nuevo_precio") as TextBox);
            cargar_cantidad_precio(gridview_pedido.SelectedRow.Cells[0].Text, texbox_cantidad.Text, texbox_nuevo_precio.Text);
            cargar_orden_compra();
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            actualizar_orden_de_compra();
            Response.Redirect("/paginasFabrica/historial_orden_de_compras.aspx", false);
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
        }

        protected void boton_parcial_Click(object sender, EventArgs e)
        {
            actualizar_orden_de_compra_entrega_parcial();
            Response.Redirect("/paginasFabrica/historial_orden_de_compras.aspx", false);
        }

        protected void gridview_pedido_DataBound(object sender, EventArgs e)
        {
            orden_de_compraBD = (DataTable)Session["orden_de_compra"];
            string id_producto;
            int fila_producto;
            double cantidad_entrega, precio, sub_total;
            for (int fila = 0; fila <= gridview_pedido.Rows.Count - 1; fila++)
            {
                id_producto = gridview_pedido.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id_producto, orden_de_compraBD);
                if (orden_de_compraBD.Rows[fila_producto]["total_entrega"].ToString() != "N/A")
                {
                    cantidad_entrega = double.Parse(orden_de_compraBD.Rows[fila_producto]["total_entrega"].ToString());
                    if (orden_de_compraBD.Rows[fila_producto]["nuevo_precio"].ToString() != "N/A")
                    {
                        precio = double.Parse(orden_de_compraBD.Rows[fila_producto]["nuevo_precio"].ToString());
                    }
                    else
                    {
                        precio = double.Parse(orden_de_compraBD.Rows[fila_producto]["precio"].ToString());
                    }
                    sub_total = cantidad_entrega * precio;
                    orden_de_compraBD.Rows[fila_producto]["sub_total"] = Math.Round(sub_total);
                }
            }
            Session.Add("orden_de_compra", orden_de_compraBD);
        }
        protected void boton_cargar_impuestos_Click(object sender, EventArgs e)
        {
            cargar_impuestos(textbox_impuestos.Text);
            cargar_orden_compra();
        }

        protected void texbox_cantidad_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_cantidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_cantidad.NamingContainer;
            int rowIndex = row.RowIndex;

            // TextBox texbox_cantidad = (gridview_pedido.SelectedRow.Cells[3].FindControl("texbox_cantidad") as TextBox);
            TextBox texbox_nuevo_precio = (gridview_pedido.Rows[rowIndex].Cells[6].FindControl("texbox_nuevo_precio") as TextBox);
            cargar_cantidad_precio(gridview_pedido.Rows[rowIndex].Cells[0].Text, texbox_cantidad.Text, texbox_nuevo_precio.Text);
            cargar_orden_compra();
        }

        protected void texbox_nuevo_precio_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_nuevo_precio = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_nuevo_precio.NamingContainer;
            int rowIndex = row.RowIndex;

            TextBox texbox_cantidad = (gridview_pedido.Rows[rowIndex].Cells[3].FindControl("texbox_cantidad") as TextBox);
            //TextBox texbox_nuevo_precio = (gridview_pedido.Rows[rowIndex].Cells[6].FindControl("texbox_nuevo_precio") as TextBox);
            cargar_cantidad_precio(gridview_pedido.Rows[rowIndex].Cells[0].Text, texbox_cantidad.Text, texbox_nuevo_precio.Text);
            cargar_orden_compra();
        }
    }
}