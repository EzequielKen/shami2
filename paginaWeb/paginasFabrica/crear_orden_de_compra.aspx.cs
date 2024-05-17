using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class crear_orden_de_compra : System.Web.UI.Page
    {
        #region cargar resumen
        private void cargar_resumen_pedido()
        {
            gridView_resumen_pedido.DataSource = (DataTable)Session["resumen_orden_pedido"];
            gridView_resumen_pedido.DataBind();
        }
        #endregion
        private void crear_tabla_insumos()
        {
            insumos_proveedor = new DataTable();

            insumos_proveedor.Columns.Add("id");
            insumos_proveedor.Columns.Add("producto");
            insumos_proveedor.Columns.Add("tipo_producto");

            insumos_proveedor.Columns.Add("tipo_paquete");
            insumos_proveedor.Columns.Add("cantidad_unidades");
            insumos_proveedor.Columns.Add("unidad_medida");

            insumos_proveedor.Columns.Add("presentacion");
            insumos_proveedor.Columns.Add("precio");
            insumos_proveedor.Columns.Add("nuevo_precio");
            insumos_proveedor.Columns.Add("precio_unidad_nuevo");
        }
        private void crear_dataTable_resumen()
        {
            resumen_pedido.Rows.Clear();
            resumen_pedido.Columns.Clear();
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("producto", typeof(string));
            resumen_pedido.Columns.Add("cantidad", typeof(string));
            resumen_pedido.Columns.Add("unidad de medida", typeof(string));
            resumen_pedido.Columns.Add("precio", typeof(string));
            resumen_pedido.Columns.Add("sub_total", typeof(string));
            resumen_pedido.Columns.Add("sub_total_dato", typeof(string));
            resumen_pedido.Columns.Add("precio_dato", typeof(string));
            resumen_pedido.Columns.Add("tipo_producto", typeof(string));
            resumen_pedido.Columns.Add("nuevo_precio", typeof(string));
            resumen_pedido.Columns.Add("precio_unidad_nuevo", typeof(string));

            resumen_pedido.Columns.Add("tipo_paquete");
            resumen_pedido.Columns.Add("cantidad_unidades");
            resumen_pedido.Columns.Add("unidad_medida");
        }

        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            int fila_insumo = 0;
            double precio, multiplicador;
            for (int fila = 0; fila <= insumos_proveedorBD.Rows.Count - 1; fila++)
            {
                if (verificar_tipo_producto(insumos_proveedorBD.Rows[fila]["tipo_producto"].ToString()) &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, insumos_proveedorBD.Rows[fila]["producto"].ToString()))
                {

                    insumos_proveedor.Rows.Add();


                    insumos_proveedor.Rows[fila_insumo]["id"] = insumos_proveedorBD.Rows[fila]["id"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["producto"] = insumos_proveedorBD.Rows[fila]["producto"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["tipo_producto"] = insumos_proveedorBD.Rows[fila]["tipo_producto"].ToString();

                    insumos_proveedor.Rows[fila_insumo]["tipo_paquete"] = insumos_proveedorBD.Rows[fila]["tipo_paquete"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["cantidad_unidades"] = insumos_proveedorBD.Rows[fila]["cantidad_unidades"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["unidad_medida"] = insumos_proveedorBD.Rows[fila]["unidad_medida"].ToString();


                    insumos_proveedor.Rows[fila_insumo]["presentacion"] = insumos_proveedorBD.Rows[fila]["presentacion"].ToString();
                    precio = double.Parse(insumos_proveedorBD.Rows[fila]["precio"].ToString());
                    multiplicador = double.Parse(insumos_proveedorBD.Rows[fila]["cantidad_unidades"].ToString());
                    precio = precio * multiplicador;
                    insumos_proveedor.Rows[fila_insumo]["precio"] = funciones.formatCurrency(precio);


                    fila_insumo++;
                }
            }
        }
        private void cargar_insumos()
        {
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);


            llenar_tabla_insumos();

            gridview_productos.DataSource = insumos_proveedor;
            gridview_productos.DataBind();
            cargar_resumen();
        }

        #region resumen
        private void cargar_resumen()
        {
            gridview_resumen.DataSource = Session["resumen"];
            gridview_resumen.DataBind();
            calcular_total();
        }
        private string calcular_total()
        {
            double total = 0;
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {
                total = total + double.Parse(resumen_pedido.Rows[fila]["sub_total_dato"].ToString());
            }
            label_total.Text = "Total: " + funciones.formatCurrency(total);
            return funciones.formatCurrency(total);
        }
        private string calcular_total_numero()
        {
            double total = 0;
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {
                total = total + double.Parse(resumen_pedido.Rows[fila]["sub_total_dato"].ToString());
            }
            return total.ToString();
        }
        private void cargar_producto_en_resumen(string cantidad_dato, string precio)
        {
            resumen_pedido = (DataTable)Session["resumen"];
            string id_producto;
            id_producto = gridview_productos.SelectedRow.Cells[0].Text;

            double unidad_de_presentacion, precio_unidad, divisor;
            int fila = 0;
            while (fila <= insumos_proveedorBD.Rows.Count - 1)
            {
                if (id_producto == insumos_proveedorBD.Rows[fila]["id"].ToString() &&
                    !verificar_sicargo(id_producto))
                {
                    resumen_pedido.Rows.Add();
                    int fila_resumen = resumen_pedido.Rows.Count - 1;
                    resumen_pedido.Rows[fila_resumen]["id"] = insumos_proveedorBD.Rows[fila]["id"].ToString();
                    resumen_pedido.Rows[fila_resumen]["producto"] = insumos_proveedorBD.Rows[fila]["producto"].ToString();

                    resumen_pedido.Rows[fila_resumen]["tipo_paquete"] = insumos_proveedorBD.Rows[fila]["tipo_paquete"].ToString();
                    resumen_pedido.Rows[fila_resumen]["cantidad_unidades"] = insumos_proveedorBD.Rows[fila]["cantidad_unidades"].ToString();
                    resumen_pedido.Rows[fila_resumen]["unidad_medida"] = insumos_proveedorBD.Rows[fila]["unidad_medida"].ToString();


                    resumen_pedido.Rows[fila_resumen]["unidad de medida"] = insumos_proveedorBD.Rows[fila]["presentacion"].ToString();
                    resumen_pedido.Rows[fila_resumen]["cantidad"] = cantidad_dato;
                    unidad_de_presentacion = double.Parse(insumos_proveedorBD.Rows[fila]["cantidad_unidades"].ToString());
                    precio_unidad = double.Parse(insumos_proveedorBD.Rows[fila]["precio"].ToString()) * unidad_de_presentacion;
                    if (precio == "N/A")
                    {
                        resumen_pedido.Rows[fila_resumen]["precio"] = funciones.formatCurrency(precio_unidad);
                        resumen_pedido.Rows[fila_resumen]["nuevo_precio"] = "N/A";
                        resumen_pedido.Rows[fila_resumen]["precio_unidad_nuevo"] = "N/A";
                        resumen_pedido.Rows[fila_resumen]["precio_dato"] = insumos_proveedorBD.Rows[fila]["precio"].ToString();
                        resumen_pedido.Rows[fila_resumen]["sub_total"] = funciones.formatCurrency(double.Parse(cantidad_dato) * precio_unidad);
                        resumen_pedido.Rows[fila_resumen]["sub_total_dato"] = double.Parse(cantidad_dato) * precio_unidad;


                    }
                    else
                    {
                        divisor = double.Parse(insumos_proveedorBD.Rows[fila]["cantidad_unidades"].ToString());
                        precio_unidad = double.Parse(precio) / divisor;
                        resumen_pedido.Rows[fila_resumen]["precio"] = funciones.formatCurrency(double.Parse(precio));
                        resumen_pedido.Rows[fila_resumen]["precio_dato"] = precio;
                        resumen_pedido.Rows[fila_resumen]["nuevo_precio"] = precio;
                        resumen_pedido.Rows[fila_resumen]["precio_unidad_nuevo"] = precio_unidad;
                        resumen_pedido.Rows[fila_resumen]["sub_total"] = funciones.formatCurrency(double.Parse(cantidad_dato) * double.Parse(precio));
                        resumen_pedido.Rows[fila_resumen]["sub_total_dato"] = double.Parse(cantidad_dato) * double.Parse(precio);

                    }

                    resumen_pedido.Rows[fila_resumen]["tipo_producto"] = insumos_proveedorBD.Rows[fila]["tipo_producto"].ToString();

                    break;
                }
                fila = fila + 1;
            }

            Session.Remove("resumen");
            Session.Add("resumen", resumen_pedido);


            textbox_busqueda.Text = "";
        }
        private bool verificar_sicargo(string id_producto)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= resumen_pedido.Rows.Count - 1)
            {
                if (id_producto == resumen_pedido.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        private void eliminar_producto_en_resumen(string id_seleccionada)
        {
            resumen_pedido = (DataTable)Session["resumen"];
            int fila = 0;
            while (fila <= resumen_pedido.Rows.Count - 1)
            {
                if (id_seleccionada == resumen_pedido.Rows[fila]["id"].ToString())
                {
                    resumen_pedido.Rows[fila].Delete();
                    break;
                }
                fila = fila + 1;
            }
            Session.Remove("resumen");
            Session.Add("resumen", resumen_pedido);
        }
        #endregion
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

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
        private bool verificar_tipo_producto(string tipo_producto)
        {
            tipo_seleccionado = Session["tipo_seleccionado"].ToString();

            bool retorno = false;
            if (tipo_seleccionado == tipo_producto || tipo_seleccionado == "Todos")
            {
                retorno = true;
            }
            return retorno;
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_orden_de_compras ordenes_compra;

        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        string tipo_seleccionado;
        string id_proveedor_fabrica_seleccionado;
        DataTable resumen_pedido = new DataTable();
        DataTable insumos_proveedorBD;
        DataTable insumos_proveedor;
        DataTable proveedor;
        DataTable resumen_orden_pedido;
        string fechaBD;
        protected void Page_Load(object sender, EventArgs e)
        {
            label_titulo.Text = Session["titulo_proveedor"].ToString();
            gridview_productos.Columns[5].Visible = true;
            //id_proveedor_fabrica_seleccionado
            id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();
            usuariosBD = (DataTable)Session["usuariosBD"];

            if (Session["id_orden_pedido_seleccionado"] != null)
            {
                if (Session["resumen_orden_pedido"] != null)
                {
                    resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];
                    if (resumen_orden_pedido.Rows.Count > 0)
                    {
                        cargar_resumen_pedido();
                        Session.Add("id_orden_pedido_a_modificar", Session["id_orden_pedido_seleccionado"].ToString());
                    }
                    else
                    {
                        Session.Add("id_orden_pedido_a_modificar", "N/A");
                    }
                }
            }
            else
            {
                Session.Add("id_orden_pedido_a_modificar", "N/A");
            }


            if (Session["ordenes_compra"] == null)
            {
                Session.Add("ordenes_compra", new cls_crear_orden_de_compras(usuariosBD));
            }
            ordenes_compra = (cls_crear_orden_de_compras)Session["ordenes_compra"];
            insumos_proveedorBD = ordenes_compra.get_insumos_proveedor(id_proveedor_fabrica_seleccionado);
            proveedor = ordenes_compra.get_proveedor(id_proveedor_fabrica_seleccionado);
            if (!IsPostBack)
            {
                Session.Add("fechaBD", "N/A");

                if (insumos_proveedorBD.Rows.Count > 0)
                {
                    llenar_dropDownList(insumos_proveedorBD);
                    Session.Add("tipo_seleccionado", tipo_seleccionado);

                    crear_dataTable_resumen();
                    Session.Add("resumen", resumen_pedido);
                    cargar_insumos();
                    calcular_total();

                }
            }
            fechaBD = Session["fechaBD"].ToString();
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            fechaBD = Session["fechaBD"].ToString();
            resumen_pedido = (DataTable)Session["resumen"];
            id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();

            if (fechaBD != "N/A" && resumen_pedido.Rows.Count > 0)
            {
                string condicion_pago = proveedor.Rows[0]["condicion_pago"].ToString();
                string url_whatsapp = ordenes_compra.crear_orden_de_compra(id_proveedor_fabrica_seleccionado, fechaBD, resumen_pedido, Session["id_orden_pedido_a_modificar"].ToString(), (DataTable)Session["resumen_orden_pedido"],calcular_total_numero(), condicion_pago);

                Session.Remove("id_orden_pedido_a_modificar");

                Response.Redirect("~/paginasFabrica/proveedores_fabrica.aspx", false);

                //Response.Redirect(url_whatsapp, false);
                //   Response.Redirect(url_whatsapp, false);
            }
            else
            {
                if (resumen_pedido.Rows.Count == 0 && fechaBD == "N/A")
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar productos y fecha.";
                }
                else if (resumen_pedido.Rows.Count == 0)
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar productos.";
                }
                else if (fechaBD == "N/A")
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar fecha.";
                }
                label_mensaje_de_alerta.Visible = true;
            }
        }
        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {


            cargar_insumos();

        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox txtValor = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_cantidad") as TextBox);
            TextBox txtPrecio = (gridview_productos.SelectedRow.Cells[5].FindControl("texbox_nuevo_precio") as TextBox);
            double cantidad, precio;
            if (txtPrecio.Text != string.Empty)
            {
                if (double.TryParse(txtValor.Text.Replace(",", "."), out cantidad) &&
                    double.TryParse(txtPrecio.Text.Replace(",", "."), out precio))
                {

                    cargar_producto_en_resumen(cantidad.ToString(), precio.ToString());
                    cargar_insumos();

                }
            }
            else
            {
                if (double.TryParse(txtValor.Text.Replace(",", "."), out cantidad))
                {

                    cargar_producto_en_resumen(cantidad.ToString(), "N/A");
                    cargar_insumos();
                    label_mensaje_de_alerta.Visible = false;
                }
            }
        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_seleccionada;

            id_seleccionada = gridview_resumen.SelectedRow.Cells[0].Text;


            eliminar_producto_en_resumen(id_seleccionada);
            cargar_insumos();
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha.Text = calendario.SelectedDate.ToString("dd/MM/yyyy");
            Session.Add("fechaBD", calendario.SelectedDate.ToString("yyyy-MM-dd"));
            label_mensaje_de_alerta.Visible = false;
        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            fechaBD = Session["fechaBD"].ToString();
            resumen_pedido = (DataTable)Session["resumen"];
            id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();

            if (fechaBD != "N/A" && resumen_pedido.Rows.Count > 0)
            {
                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = "Orden de Compra id-" + dato_hora + ".pdf";
                string ruta = "~/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

                ordenes_compra.crear_pdf_orden_de_compra(ruta_archivo, imgdata, resumen_pedido, id_proveedor_fabrica_seleccionado, fechaBD, calcular_total());
                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();

                //   Session.Contents.RemoveAll();
                //   Response.Redirect(url_whatsapp, false);
            }
            else
            {
                if (resumen_pedido.Rows.Count == 0 && fechaBD == "N/A")
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar productos y fecha.";
                }
                else if (resumen_pedido.Rows.Count == 0)
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar productos.";
                }
                else if (fechaBD == "N/A")
                {
                    label_mensaje_de_alerta.Text = "Falta seleccionar fecha.";
                }
                label_mensaje_de_alerta.Visible = true;
            }
        }
    }
}