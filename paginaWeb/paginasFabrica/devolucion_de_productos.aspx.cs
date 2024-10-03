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
    public partial class devolucion_de_productos : System.Web.UI.Page
    {
        #region carga a base de datos
        private void enviar_stock()
        {
            merma_y_deperdicio.sumar_stock((DataTable)Session["productos_proveedorBD_devolucion"], proveedorBD.Rows[0]["nombre_en_BD"].ToString(), tipo_usuario.Rows[0]["rol"].ToString(),textbox_nota.Text);
        }
        private void sumar_stock(string id_producto, string cantidad_dato)
        {
            double cantidad_a_sumar, cantidad, nueva_cantidad;
            if (double.TryParse(cantidad_dato, out cantidad_a_sumar))
            {
                int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedorBD);
                if (cantidad_a_sumar > 0)
                {

                    cantidad = double.Parse(productos_proveedorBD.Rows[fila_producto]["stock"].ToString());
                    nueva_cantidad = cantidad + cantidad_a_sumar;

                    productos_proveedorBD.Rows[fila_producto]["cantidad_devuelta"] = cantidad_a_sumar.ToString();
                    productos_proveedorBD.Rows[fila_producto]["nuevo_stock"] = nueva_cantidad.ToString();
                    Session.Add("productos_proveedorBD_devolucion", productos_proveedorBD);


                }
            }
        }
        private void solo_cargar_desperdicio(string id_producto, string cantidad_dato)
        {
            double cantidad_a_sumar, cantidad, nueva_cantidad;
            if (double.TryParse(cantidad_dato, out cantidad_a_sumar))
            {
                int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedorBD);
                if (cantidad_a_sumar > 0)
                {

                    cantidad = double.Parse(productos_proveedorBD.Rows[fila_producto]["stock"].ToString());
                    nueva_cantidad = cantidad - cantidad_a_sumar;

                    productos_proveedorBD.Rows[fila_producto]["cantidad_devuelta"] = cantidad_a_sumar.ToString();
                    productos_proveedorBD.Rows[fila_producto]["nuevo_stock"] = "N/A";
                    Session.Add("productos_proveedorBD_devolucion", productos_proveedorBD);


                }
            }
        }
       
        
      
        #endregion
        #region carga merma/desperdicio
        private void configurar_desperdicio(string id_producto, string cantidad_dato, string unidad)
        {
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedorBD);
                if (cantidad > 0)
                {
                    productos_proveedorBD.Rows[fila_producto]["cantidad_desperdicio"] = cantidad.ToString();
                    productos_proveedorBD.Rows[fila_producto]["unidad"] = unidad;
                    productos_proveedorBD.Rows[fila_producto]["presentacion"] = cantidad.ToString() + " " + unidad;
                }
                else
                {
                    productos_proveedorBD.Rows[fila_producto]["cantidad_desperdicio"] = "N/A";
                    productos_proveedorBD.Rows[fila_producto]["unidad"] = "N/A";
                    productos_proveedorBD.Rows[fila_producto]["presentacion"] = "N/A";
                }
            }
        }
        private void configurar_merma(string id_insumo, string cantidad_dato, string unidad)
        {
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumosBD);
                if (cantidad > 0)
                {
                    insumosBD.Rows[fila_insumo]["cantidad_merma"] = cantidad.ToString();
                    insumosBD.Rows[fila_insumo]["unidad"] = unidad;
                    insumosBD.Rows[fila_insumo]["presentacion"] = cantidad.ToString() + " " + unidad;
                }
                else
                {
                    insumosBD.Rows[fila_insumo]["cantidad_merma"] = "N/A";
                    insumosBD.Rows[fila_insumo]["unidad"] = "N/A";
                    insumosBD.Rows[fila_insumo]["presentacion"] = "N/A";
                }
            }

        }
        #endregion
        #region carga de productos

        private void crear_tabla_productos()
        {
            productos_proveedor = new DataTable();

            productos_proveedor.Columns.Add("id", typeof(string));
            productos_proveedor.Columns.Add("producto", typeof(string));
            productos_proveedor.Columns.Add("cantidad_desperdicio", typeof(string));
            productos_proveedor.Columns.Add("unidad_de_medida_fabrica", typeof(string));
            productos_proveedor.Columns.Add("unidad", typeof(string));
            productos_proveedor.Columns.Add("presentacion", typeof(string));
            productos_proveedor.Columns.Add("stock", typeof(string));
            productos_proveedor.Columns.Add("nuevo_stock", typeof(string));
            productos_proveedor.Columns.Add("cantidad_devuelta", typeof(string));
        }
        private void llenar_tabla_producto()
        {
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productos_proveedorBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(productos_proveedorBD.Rows[fila]["tipo_producto"].ToString(), Session["tipo_seleccionado"].ToString()))
                {
                    productos_proveedor.Rows.Add();

                    productos_proveedor.Rows[fila_producto]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                    productos_proveedor.Rows[fila_producto]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();
                    productos_proveedor.Rows[fila_producto]["cantidad_desperdicio"] = productos_proveedorBD.Rows[fila]["cantidad_desperdicio"].ToString();
                    productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"] = productos_proveedorBD.Rows[fila]["unidad_de_medida_fabrica"].ToString();
                    productos_proveedor.Rows[fila_producto]["unidad"] = productos_proveedorBD.Rows[fila]["unidad"].ToString();
                    productos_proveedor.Rows[fila_producto]["presentacion"] = productos_proveedorBD.Rows[fila]["presentacion"].ToString();
                    productos_proveedor.Rows[fila_producto]["stock"] = productos_proveedorBD.Rows[fila]["stock"].ToString();
                    productos_proveedor.Rows[fila_producto]["nuevo_stock"] = productos_proveedorBD.Rows[fila]["nuevo_stock"].ToString();
                    productos_proveedor.Rows[fila_producto]["cantidad_devuelta"] = productos_proveedorBD.Rows[fila]["cantidad_devuelta"].ToString();

                    fila_producto++;
                }
            }
        }
        private void cargar_producto()
        {
            llenar_tabla_producto();

            gridView_productos.DataSource = productos_proveedor;
            gridView_productos.DataBind();
        }
        #endregion
        #region configurar controles
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

        #endregion

        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_merma_y_desperdicio merma_y_deperdicio;
        cls_cargar_orden_de_compra orden_compra;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;

        DataTable productos_proveedorBD;
        DataTable productos_proveedor;
        DataTable insumosBD;
        DataTable insumos;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];

            merma_y_deperdicio = new cls_merma_y_desperdicio(usuariosBD);
            if (!IsPostBack)
            {
                productos_proveedorBD = merma_y_deperdicio.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
                Session.Add("productos_proveedorBD_devolucion", productos_proveedorBD);
                // insumosBD = merma_y_deperdicio.get_insumos_proveedor();
                //  Session.Add("insumosBD_merma", insumosBD);
                llenar_dropDownList(productos_proveedorBD);
                Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);



                cargar_producto();
            }
            productos_proveedorBD = (DataTable)Session["productos_proveedorBD_devolucion"];
            //  insumosBD = (DataTable)Session["insumosBD_merma"];
        }

        #region productos
        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_producto();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            cargar_producto();
        }
        protected void gridView_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void gridView_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cargar_en_stock")
            {
                int fila = int.Parse(e.CommandArgument.ToString());

                TextBox texbox_cantidad_stock = (gridView_productos.Rows[fila].Cells[2].FindControl("texbox_cantidad_stock") as TextBox);

                sumar_stock(gridView_productos.Rows[fila].Cells[0].Text, texbox_cantidad_stock.Text);

                cargar_producto();
            }
        }
        #endregion

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            enviar_stock();
            Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx",false);
        }

       
    }
}