using _03___sistemas_fabrica;
using _05___sistemas_fabrica_fatay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabricaFatay
{
    public partial class produccion_fabrica_fatay : System.Web.UI.Page
    {
        private void crear_tabla_productos()
        {
            productos_produccion = new DataTable();

            productos_produccion.Columns.Add("id", typeof(string));
            productos_produccion.Columns.Add("producto", typeof(string));
            productos_produccion.Columns.Add("unidad_de_medida", typeof(string));
        }
        private void crear_dataTable_resumen()
        {
            resumen_pedido.Rows.Clear();
            resumen_pedido.Columns.Clear();
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("producto", typeof(string));
            resumen_pedido.Columns.Add("cantidad", typeof(string));
            resumen_pedido.Columns.Add("unidad de medida", typeof(string));
            resumen_pedido.Columns.Add("tipo_producto", typeof(string));
        }
        private void llenar_tabla_producto()
        {
            crear_tabla_productos();
            tipo_seleccionado = Session["tipo_seleccionado"].ToString();
            int fila_producto = 0;
            for (int fila = 0; fila <= productos_produccionBD.Rows.Count - 1; fila++)
            {
                if (productos_produccionBD.Rows[fila]["tipo_producto"].ToString() == tipo_seleccionado &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productos_produccionBD.Rows[fila]["producto"].ToString()))
                {
                    productos_produccion.Rows.Add();

                    productos_produccion.Rows[fila_producto]["id"] = productos_produccionBD.Rows[fila]["id"].ToString();
                    productos_produccion.Rows[fila_producto]["producto"] = productos_produccionBD.Rows[fila]["producto"].ToString();
                    productos_produccion.Rows[fila_producto]["unidad_de_medida"] = productos_produccionBD.Rows[fila]["unidad_de_medida_fabrica"].ToString();

                    fila_producto++;
                }

            }
        }
        private void cargar_productos()
        {

            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            productos_produccionBD = (DataTable)Session["productos_produccionBD"];
            llenar_tabla_producto();
            gridview_productos.DataSource = productos_produccion;
            gridview_productos.DataBind();



            cargar_resumen();
        }
        private void cargar_resumen()
        {
            gridview_resumen.DataSource = Session["resumen"];
            gridview_resumen.DataBind();

        }
        private void cargar_producto_en_resumen(string cantidad_dato)
        {
            resumen_pedido = (DataTable)Session["resumen"];
            string id_producto;
            id_producto = gridview_productos.SelectedRow.Cells[0].Text;
            productos_produccionBD = (DataTable)Session["productos_produccionBD"];

            int fila = 0;
            while (fila <= productos_produccionBD.Rows.Count - 1)
            {
                if (id_producto == productos_produccionBD.Rows[fila]["id"].ToString() &&
                    !verificar_sicargo(id_producto))// 

                {
                    resumen_pedido.Rows.Add();
                    int fila_resumen = resumen_pedido.Rows.Count - 1;
                    resumen_pedido.Rows[fila_resumen]["id"] = productos_produccionBD.Rows[fila]["id"].ToString();
                    resumen_pedido.Rows[fila_resumen]["producto"] = productos_produccionBD.Rows[fila]["producto"].ToString();
                    resumen_pedido.Rows[fila_resumen]["unidad de medida"] = productos_produccionBD.Rows[fila]["unidad_de_medida_local"].ToString();
                    resumen_pedido.Rows[fila_resumen]["cantidad"] = cantidad_dato;

                    resumen_pedido.Rows[fila_resumen]["tipo_producto"] = productos_produccionBD.Rows[fila]["tipo_producto"].ToString();

                    break;
                }
                fila = fila + 1;
            }

            Session.Remove("resumen");
            Session.Add("resumen", resumen_pedido);


            textbox_busqueda.Text = "";
            label_productoSelecionado.Text = mensaje_default;
        }
        #region funciones
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
        #endregion
        #region dropdowns
        private void configurar_controles()
        {
            llenar_dropDownList(productos_produccionBD);
        }
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

            tipo_seleccionado = "2-Empanadas";
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
        #endregion
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        string mensaje_default = "seleccione un producto";

        cls_produccion_fabrica_fatay produccion;
        cls_stock_insumos stock_insumo;
        cls_funciones funciones = new cls_funciones();
        DataTable resumen_pedido = new DataTable();

        DataTable proveedorBD;
        DataTable productos_produccionBD;
        DataTable productos_produccion;
        DataTable usuariosBD;
        DataTable tipo_usuarioBD;
        string tipo_seleccionado;
        string fechaBD;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];
            stock_insumo = (cls_stock_insumos)Session["stock_insumo"];

            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                produccion = new cls_produccion_fabrica_fatay(usuariosBD);
                productos_produccionBD = produccion.get_productos_produccion("proveedor_villaMaipu");
                Session.Add("productos_produccionBD", productos_produccionBD);

                if (!IsPostBack)
                {
                    label_productoSelecionado.Text = mensaje_default;
                    crear_dataTable_resumen();
                    Session.Add("resumen", resumen_pedido);
                    configurar_controles();
                    cargar_productos();
                    label_fecha.Text = DateTime.Now.ToString();
                    Session.Add("fechaBD", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                }
            }
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["resumen"];

            if (dt.Rows.Count > 0)
            {
                string url = produccion.cargar_produccion_diaria((DataTable)Session["resumen"], "Fabrica Fatay Callao", tipo_usuarioBD.Rows[0]["rol"].ToString(), Session["fechaBD"].ToString(), tipo_usuarioBD.Rows[0]["rol"].ToString());
                Session.Clear();
                Response.Redirect(url, false);
                //Response.Redirect("~/paginas/proveedores.aspx", false);
                // string url_whatsapp = sistema_pedidos.enviar_pedido((DataTable)Session["resumen"], (DataTable)Session["bonificados"], (DataTable)Session["lista_proveedores"], Session["nombre_proveedor"].ToString());
                //Response.Write("<script>window.open('" + url_whatsapp + "','_blank');</script>");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + url_whatsapp + "','_blank')", true);
                //   Session.Add("url_whatsapp", url_whatsapp);
                //   Response.Redirect("/paginas/historial_pedido.aspx", false);
                //Response.Redirect("/paginas/proveedores.aspx", false);
            }
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = mensaje_default;
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;
            cargar_productos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = mensaje_default;
            tipo_seleccionado = dropDown_tipo.SelectedItem.Text;

            cargar_productos();
        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_productoSelecionado.Text = gridview_productos.SelectedRow.Cells[1].Text;

            //Buscas el control <cubicandolo por fila y columna, y lo agregas a un textbox  
            TextBox txtValor = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_cantidad") as TextBox);
            double cantidad;
            if (double.TryParse(txtValor.Text.Replace(",", "."), out cantidad))
            {

                cargar_producto_en_resumen(cantidad.ToString());
                cargar_productos();

            }
        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_seleccionada;

            id_seleccionada = gridview_resumen.SelectedRow.Cells[0].Text;


            eliminar_producto_en_resumen(id_seleccionada);
            cargar_productos();

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropdown_acuerdo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha.Text = calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).ToString();
            Session.Add("fechaBD", calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}