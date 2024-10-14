using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace paginaWeb
{
    public partial class pedido : System.Web.UI.Page
    {
        #region funciones
        private void cargar_productos()
        {
            Session.Remove("tipo_seleccionado");
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            llenar_dataTable();
            gridview_productos.DataSource = null;
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();

            cargar_resumen();
        }
        private void cargar_productos_por_busqueda()
        {
            Session.Remove("tipo_seleccionado");
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            llenar_dataTable_por_busqueda();
            gridview_productos.DataSource = null;
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();

            cargar_resumen();
        }
        private void cargar_resumen()
        {
            gridview_resumen.DataSource = Session["resumen"];
            gridview_resumen.DataBind();

        }
        private void crear_dataTable()
        {
            productos = new DataTable();
            productos.Rows.Clear();
            productos.Columns.Clear();
            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("unidad_medida", typeof(string));
            productos.Columns.Add("precio", typeof(string));
            productos.Columns.Add("bonificable", typeof(bool));
            productos.Columns.Add("tipo_producto", typeof(string));
            productos.Columns.Add("proveedor", typeof(string));
        }
        private void crear_dataTablePDF()
        {
            productosPDF = new DataTable();
            productosPDF.Rows.Clear();
            productosPDF.Columns.Clear();
            productosPDF.Columns.Add("id", typeof(string));
            productosPDF.Columns.Add("producto", typeof(string));
            productosPDF.Columns.Add("unidad_medida", typeof(string));
            productosPDF.Columns.Add("precio", typeof(string));
            productosPDF.Columns.Add("bonificable", typeof(bool));
            productosPDF.Columns.Add("tipo_producto", typeof(string));
            productosPDF.Columns.Add("proveedor", typeof(string));
        }
        private void crear_datatable_bonificado()
        {
            resumen_bonificado.Rows.Clear();
            resumen_bonificado.Columns.Clear();
            resumen_bonificado.Columns.Add("id", typeof(string));
            resumen_bonificado.Columns.Add("producto", typeof(string));
            resumen_bonificado.Columns.Add("unidad de medida", typeof(string));
            resumen_bonificado.Columns.Add("cantidad", typeof(string));
            resumen_bonificado.Columns.Add("precio", typeof(string));
            resumen_bonificado.Columns.Add("tipo_bonificado", typeof(string));
            resumen_bonificado.Columns.Add("tipo_producto", typeof(string));
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
            resumen_pedido.Columns.Add("multiplicador", typeof(string));
            resumen_pedido.Columns.Add("bonificable", typeof(bool));
            resumen_pedido.Columns.Add("tipo_producto", typeof(string));
            resumen_pedido.Columns.Add("proveedor", typeof(string));
            resumen_pedido.Columns.Add("alimento", typeof(string));
            resumen_pedido.Columns.Add("bebida", typeof(string));
            resumen_pedido.Columns.Add("descartable", typeof(string));
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "orden_tipo asc";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;
            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            if (verificar_sucursal())
            {

                item = new ListItem("4-Dulces", num_item.ToString());
                dropDown_tipo.Items.Add(item);
                num_item = num_item + 1;

            }
            else
            {

                item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
                dropDown_tipo.Items.Add(item);
                num_item = num_item + 1;

            }

            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {
                    if (verificar_sucursal())
                    {//6-Jugos
                        if (dt.Rows[fila]["tipo_producto"].ToString() == "2-Empanadas" ||
                            dt.Rows[fila]["tipo_producto"].ToString() == "6-Jugos")
                        {
                            item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                            dropDown_tipo.Items.Add(item);
                            num_item = num_item + 1;
                        }
                    }

                    else
                    {
                        if (sucusalBD.Rows[0]["id"].ToString() == "13" &&
                            Session["nombre_proveedor"].ToString() == "Fabrica villa maipu")
                        {
                            if (dt.Rows[fila]["tipo_producto"].ToString() != "2-Empanadas")
                            {
                                item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                                dropDown_tipo.Items.Add(item);
                                num_item = num_item + 1;
                            }
                        }
                        else
                        {
                            item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                            dropDown_tipo.Items.Add(item);
                            num_item = num_item + 1;
                        }
                    }
                }

            }
        }
        private bool verificar_sucursal()
        {
            bool retorno = false;
            if (usuariosBD.Rows[0]["usuario"].ToString() == "saleh" &&
                Session["nombre_proveedor"].ToString() == "Fabrica villa maipu")
            {
                retorno = true;
            }
            else if (usuariosBD.Rows[0]["usuario"].ToString() == "alhalabi" &&
                Session["nombre_proveedor"].ToString() == "Fabrica villa maipu")
            {
                retorno = true;
            }

            return retorno;
        }
        private bool verificar_tipo_producto(string tipo_producto)
        {
            tipo_seleccionado = Session["tipo_seleccionado"].ToString();

            bool retorno = false;
            if (tipo_seleccionado == tipo_producto)
            {
                retorno = true;
            }


            return retorno;
        }
        private void llenar_dataTable()
        {
            productos_proveedor = (DataTable)Session["productos_proveedor"];
            int fila_producto = 0;
            string dato_usuario = textbox_busqueda.Text;
            double precio, multiplicador;
            crear_dataTable();
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                if (verificar_tipo_producto(productos_proveedor.Rows[fila]["tipo_producto"].ToString()))
                {
                    productos.Rows.Add();
                    productos.Rows[fila_producto]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                    productos.Rows[fila_producto]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();

                    productos.Rows[fila_producto]["unidad_medida"] = productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();

                    precio = double.Parse(productos_proveedor.Rows[fila]["precio"].ToString());
                    multiplicador = double.Parse(productos_proveedor.Rows[fila]["multiplicador"].ToString());
                    precio = precio * multiplicador;
                    if (Session["nombre_proveedor"].ToString() == "Shami Insumos")
                    {
                        productos.Rows[fila_producto]["precio"] = formatCurrency(precio);
                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio"] = formatCurrency(precio) + productos_proveedor.Rows[fila]["presentacion"].ToString();
                    }

                    fila_producto++;
                }
            }
        }
        private void llenar_dataTable_por_busqueda()
        {
            productos_proveedor = (DataTable)Session["productos_proveedor"];
            int fila_producto = 0;
            string dato_usuario = textbox_busqueda.Text;
            double precio, multiplicador;
            crear_dataTable();
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(dato_usuario, productos_proveedor.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    productos.Rows[fila_producto]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                    productos.Rows[fila_producto]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                    productos.Rows[fila_producto]["unidad_medida"] = productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();
                    precio = double.Parse(productos_proveedor.Rows[fila]["precio"].ToString());
                    multiplicador = double.Parse(productos_proveedor.Rows[fila]["multiplicador"].ToString());
                    precio = precio * multiplicador;
                    if (Session["nombre_proveedor"].ToString() == "Shami Insumos")
                    {
                        productos.Rows[fila_producto]["precio"] = formatCurrency(precio);
                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio"] = formatCurrency(precio) + productos_proveedor.Rows[fila]["presentacion"].ToString();
                    }

                    fila_producto++;
                }
            }
        }
        private void llenar_dataTablePDF()
        {
            productos_proveedor = (DataTable)Session["productos_proveedor"];
            int fila_producto = 0;
            string dato_usuario = textbox_busqueda.Text;
            double precio, multiplicador;
            crear_dataTablePDF();
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                if (verificar_tipo_a_cargar(productos_proveedor.Rows[fila]["tipo_producto"].ToString()))
                {
                    productosPDF.Rows.Add();
                    productosPDF.Rows[fila_producto]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                    if (productos_proveedor.Rows[fila]["id"].ToString() == "1")
                    {
                        string stop = productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();
                    }
                    productosPDF.Rows[fila_producto]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                    productosPDF.Rows[fila_producto]["unidad_medida"] = productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();
                    precio = double.Parse(productos_proveedor.Rows[fila]["precio"].ToString());
                    multiplicador = double.Parse(productos_proveedor.Rows[fila]["multiplicador"].ToString());
                    precio = precio * multiplicador;
                    if (productos_proveedor.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        productosPDF.Rows[fila_producto]["precio"] = formatCurrency(precio) + " x" + productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();
                    }
                    else
                    {
                        productosPDF.Rows[fila_producto]["precio"] = formatCurrency(precio) + productos_proveedor.Rows[fila]["presentacion"].ToString();
                    }

                    fila_producto++;
                }
            }
            Session.Add("productosPDF", productosPDF);
        }
        private bool verificar_tipo_a_cargar(string tipo_producto)
        {
            bool retorno = false;
            if (verificar_sucursal())
            {
                if (tipo_producto == "4-Dulces" || tipo_producto == "2-Empanadas")
                {
                    retorno = true;
                }
            }
            else
            {
                retorno = true;
            }
            return retorno;
        }
        private void llenar_datatable_bonificados()
        {
            int fila_producto = 0;
            int fila = 0;
            string precio_especial = sistema_pedidos.obtener_precio_bonificado_especial();
            string dato_usuario = textbox_busqueda.Text;
            int id;
            crear_dataTable();
            List<int> lista_bonificados = sistema_pedidos.obtener_lista_bonificados();
            List<int> lista_bonificados_especiales = sistema_pedidos.obtener_lista_bonificados_especiales();
            for (int i = 0; i <= lista_bonificados.Count - 1; i++)
            {
                id = lista_bonificados[i];
                while (fila <= productos_proveedor.Rows.Count - 1)
                {
                    if (id.ToString() == productos_proveedor.Rows[fila]["id"].ToString() && buscar_alguna_coincidencia(dato_usuario, productos_proveedor.Rows[fila]["producto"].ToString()))
                    {
                        productos.Rows.Add();
                        productos.Rows[fila_producto]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                        productos.Rows[fila_producto]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                        productos.Rows[fila_producto]["unidad de medida"] = productos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();

                        productos.Rows[fila_producto]["precio"] = formatCurrency(0);

                        fila_producto++;
                        break;
                    }
                    fila++;
                }
            }
            for (int i = 0; i < lista_bonificados_especiales.Count - 1; i++)
            {
                id = lista_bonificados_especiales[i];
                while (fila <= productos_proveedor.Rows.Count - 1)
                {
                    if (id.ToString() == productos_proveedor.Rows[fila]["id"].ToString() && buscar_alguna_coincidencia(dato_usuario, productos_proveedor.Rows[fila]["producto"].ToString()))
                    {
                        productos.Rows.Add();
                        productos.Rows[fila_producto]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                        productos.Rows[fila_producto]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                        productos.Rows[fila_producto]["unidad de medida"] = productos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();

                        productos.Rows[fila_producto]["precio"] = formatCurrency(decimal.Parse(precio_especial));

                        fila_producto++;
                        break;
                    }
                    fila++;
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
            return resultado;
        }
        private void cargar_producto_en_resumen(string cantidad_dato)
        {
            productos_proveedor = (DataTable)Session["productos_proveedor"];
            resumen_pedido = (DataTable)Session["resumen"];
            string id_producto, nombre_producto;
            id_producto = gridview_productos.SelectedRow.Cells[0].Text;
            nombre_producto = gridview_productos.SelectedRow.Cells[1].Text;

            double precio, multiplicador;
            int fila = 0;
            while (fila <= productos_proveedor.Rows.Count - 1)
            {
                if (id_producto == productos_proveedor.Rows[fila]["id"].ToString() &&
                    nombre_producto == productos_proveedor.Rows[fila]["producto"].ToString() &&
                    !verificar_sicargo(id_producto))
                {
                    resumen_pedido.Rows.Add();
                    int fila_resumen = resumen_pedido.Rows.Count - 1;
                    precio = double.Parse(productos_proveedor.Rows[fila]["precio"].ToString());
                    multiplicador = double.Parse(productos_proveedor.Rows[fila]["multiplicador"].ToString());
                    precio = precio * multiplicador;
                    resumen_pedido.Rows[fila_resumen]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                    resumen_pedido.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                    resumen_pedido.Rows[fila_resumen]["unidad de medida"] = productos_proveedor.Rows[fila]["unidad_medida_local"].ToString();
                    resumen_pedido.Rows[fila_resumen]["cantidad"] = cantidad_dato;
                    if (productos_proveedor.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        resumen_pedido.Rows[fila_resumen]["precio"] = formatCurrency(precio) + productos_proveedor.Rows[fila]["presentacion"].ToString();
                    }
                    else
                    {
                        resumen_pedido.Rows[fila_resumen]["precio"] = formatCurrency(precio) + productos_proveedor.Rows[fila]["presentacion"].ToString();
                    }

                    resumen_pedido.Rows[fila_resumen]["tipo_producto"] = productos_proveedor.Rows[fila]["tipo_producto"].ToString();
                    resumen_pedido.Rows[fila_resumen]["proveedor"] = productos_proveedor.Rows[fila]["proveedor"].ToString();
                    resumen_pedido.Rows[fila_resumen]["alimento"] = productos_proveedor.Rows[fila]["alimento"].ToString();
                    resumen_pedido.Rows[fila_resumen]["bebida"] = productos_proveedor.Rows[fila]["bebida"].ToString();
                    resumen_pedido.Rows[fila_resumen]["descartable"] = productos_proveedor.Rows[fila]["descartable"].ToString();

                    break;
                }
                fila = fila + 1;
            }

            Session.Remove("resumen");
            Session.Add("resumen", resumen_pedido);


            textbox_busqueda.Text = "";
            label_productoSelecionado.Text = mensaje_default;
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
        private void eliminar_producto_en_bonificado(string id_seleccionada)
        {
            resumen_bonificado = (DataTable)Session["bonificados"];
            int fila = 0;
            while (fila <= resumen_bonificado.Rows.Count - 1)
            {
                if (id_seleccionada == resumen_bonificado.Rows[fila]["id"].ToString())
                {
                    resumen_bonificado.Rows[fila].Delete();
                    break;
                }
                fila = fila + 1;
            }
            Session.Remove("bonificados");
            Session.Add("bonificados", resumen_pedido);
        }
        private void activar_modo_bonificado()
        {

            Session.Remove("modo_bonificado");
            Session.Add("modo_bonificado", true);
        }
        private void desactivar_modo_bonificado()
        {
            Session.Remove("modo_bonificado");
            Session.Add("modo_bonificado", false);
        }

        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
        }
        #endregion


        //########################################################################################
        #region atributos
        string mensaje_default = "seleccione un producto";
        cls_sistema_pedidos sistema_pedidos;
        cls_funciones funciones = new cls_funciones();

        DataTable usuariosBD;
        DataTable sucusalBD;
        DataTable productos = new DataTable();
        DataTable productosPDF = new DataTable();
        DataTable resumen_pedido = new DataTable();
        DataTable resumen_bonificado = new DataTable();
        DataTable productos_proveedor;
        int seguridad;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucusalBD = (DataTable)Session["sucursal"];
            if (sucusalBD.Rows[0]["id"].ToString() == "19" ||
                sucusalBD.Rows[0]["id"].ToString() == "22")
            {
                textbox_nota.Visible = true;
            }
            else
            {
                textbox_nota.Visible = false;
            }

            sistema_pedidos = new cls_sistema_pedidos(usuariosBD, sucusalBD);




            if (!IsPostBack)
            {

                productos_proveedor = sistema_pedidos.get_productos_proveedor(sucusalBD.Rows[0]["id"].ToString());
                Session.Add("productos_proveedor", productos_proveedor);

                label_productoSelecionado.Text = mensaje_default;

                llenar_dropDownList(productos_proveedor);
                Session.Add("tipo_seleccionado", tipo_seleccionado);
                crear_dataTable_resumen();
                Session.Add("resumen", resumen_pedido);

                crear_datatable_bonificado();
                Session.Remove("bonificados");
                Session.Add("bonificados", resumen_bonificado);

                Session.Add("modo_bonificado", false);

                cargar_productos();
            }
            resumen_bonificado = (DataTable)Session["bonificados"];



        }



        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_seleccionada;

            id_seleccionada = gridview_resumen.SelectedRow.Cells[0].Text;


            eliminar_producto_en_resumen(id_seleccionada);
            cargar_productos();


        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = mensaje_default;
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;
            textbox_busqueda.Text = string.Empty;
            cargar_productos();



        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {

            cargar_productos();





        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = mensaje_default;
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;
            cargar_productos_por_busqueda();


        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["resumen"];

            if (dt.Rows.Count > 0)
            {
                string nota = "N/A";
                if (textbox_nota.Text != string.Empty)
                {
                    nota = textbox_nota.Text;
                }
                string url_whatsapp = sistema_pedidos.enviar_pedido((DataTable)Session["resumen"], nota);
                Session.Contents.RemoveAll();
                //Response.Write("<script>window.open('" + url_whatsapp + "','_blank');</script>");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + url_whatsapp + "','_blank')", true);
                Response.Redirect(url_whatsapp, false);
                //Response.Redirect("/paginas/historial_pedido.aspx", false);
                //Response.Redirect("/paginas/proveedores.aspx", false);
            }
        }

        protected void gridview_bonificado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_seleccionada;
            id_seleccionada = gridview_bonificado.SelectedRow.Cells[0].Text;

            eliminar_producto_en_bonificado(id_seleccionada);
            cargar_productos();


        }

        /* protected void gridview_productos_SelectedIndexChanged1(object sender, EventArgs e)
         {
         }*/

        protected void textbox_carga_TextChanged(object sender, EventArgs e)
        {

        }

        /*        protected void gridview_productos_RowCommand(object sender, GridViewCommandEventArgs e)
                {

                    if (e.CommandName == "boton_carga")
                    {
                        int numFila = ((GridViewRow)((LinkButton)e.CommandSource).Parent.Parent).RowIndex;

                        TextBox txtValor = (gridview_productos.Rows[numFila].Cells[5].FindControl("texbox_cantidad") as TextBox);

                    }

                }*/

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = gridview_productos.SelectedRow.Cells[1].Text;

            //Buscas el control ubicandolo por fila y columna, y lo agregas a un textbox  
            TextBox txtValor = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_cantidad") as TextBox);
            double cantidad;
            if (double.TryParse(txtValor.Text.Replace(",", "."), out cantidad))
            {

                cargar_producto_en_resumen(cantidad.ToString());
                cargar_productos();

            }



        }

        protected void Button_lista_de_precios_Click(object sender, EventArgs e)
        {
            llenar_dataTablePDF();

            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " Lista de precio - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            productosPDF = (DataTable)Session["productosPDF"];
            sistema_pedidos.crear_pdf_lista_de_precios(ruta_archivo, imgdata, productosPDF); //crear_pdf();

            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginas/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

        }
    }
}