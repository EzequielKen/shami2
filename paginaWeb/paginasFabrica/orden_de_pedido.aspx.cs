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

    public partial class orden_de_pedido : System.Web.UI.Page
    {
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

            resumen_pedido.Columns.Add("tipo_paquete");
            resumen_pedido.Columns.Add("cantidad_unidades");
            resumen_pedido.Columns.Add("unidad_medida");
        }

        private void llenar_tabla_insumos()
        {
            insumos_proveedorBD = (DataTable)Session["insumos_proveedorBD_OP"];
            crear_tabla_insumos();
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumos_proveedorBD.Rows.Count - 1; fila++)
            {
                if (verificar_tipo_producto(insumos_proveedorBD.Rows[fila]["tipo_producto"].ToString()) &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, insumos_proveedorBD.Rows[fila]["producto"].ToString()))
                {
                    insumos_proveedor.Rows.Add();
                    insumos_proveedor.Rows[fila_insumo]["id"] = insumos_proveedorBD.Rows[fila]["id"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["producto"] = insumos_proveedorBD.Rows[fila]["producto"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["tipo_producto"] = insumos_proveedorBD.Rows[fila]["tipo_producto"].ToString();
                    insumos_proveedor.Rows[fila_insumo]["unidad_medida"] = insumos_proveedorBD.Rows[fila]["unidad_medida"].ToString();
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
        }

        private void cargar_producto_en_resumen(string cantidad_dato, string precio)
        {
            insumos_proveedorBD = (DataTable)Session["insumos_proveedorBD_OP"];
            resumen_pedido = (DataTable)Session["resumen"];
            string id_producto;
            id_producto = gridview_productos.SelectedRow.Cells[0].Text;


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
                    resumen_pedido.Rows[fila_resumen]["unidad_medida"] = insumos_proveedorBD.Rows[fila]["unidad_medida"].ToString();

                    resumen_pedido.Rows[fila_resumen]["cantidad"] = cantidad_dato;

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
        cls_orden_de_pedido orden_pedido;

        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        string tipo_seleccionado;
        string id_proveedor_fabrica_seleccionado;
        DataTable resumen_pedido = new DataTable();
        DataTable insumos_proveedorBD;
        DataTable insumos_proveedor;
        DataTable proveedor;
        string fechaBD;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["orden_pedido"] == null)
            {
                Session.Add("orden_pedido", new cls_orden_de_pedido(usuariosBD));
            }
            orden_pedido = (cls_orden_de_pedido)Session["orden_pedido"];
            
            if (!IsPostBack)
            {
                insumos_proveedorBD = orden_pedido.get_insumos_fabrica();
                Session.Add("insumos_proveedorBD_OP", insumos_proveedorBD);

                llenar_dropDownList(insumos_proveedorBD);
                Session.Add("tipo_seleccionado", tipo_seleccionado);

                crear_dataTable_resumen();
                Session.Add("resumen", resumen_pedido);
                cargar_insumos();
            }
        }



        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            resumen_pedido = (DataTable)Session["resumen"];

            if (resumen_pedido.Rows.Count > 0)
            {

                // string url_whatsapp = ordenes_compra.crear_orden_de_compra(id_proveedor_fabrica_seleccionado, fechaBD, resumen_pedido);

                string url_whatsapp = orden_pedido.cargar_orden_pedido(resumen_pedido, (DataTable)Session["tipo_usuario"]);

                Response.Redirect(url_whatsapp, false);
                //   Response.Redirect(url_whatsapp, false);
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
            TextBox txtValor = (gridview_productos.SelectedRow.Cells[2].FindControl("texbox_cantidad") as TextBox);
            double cantidad, precio;

            insumos_proveedorBD = (DataTable)Session["insumos_proveedorBD_OP"];
            int fila_tabla = funciones.buscar_fila_por_id(gridview_productos.SelectedRow.Cells[0].Text, insumos_proveedorBD);
            DropDownList dropdown_tipo_paquete = (gridview_productos.SelectedRow.Cells[3].FindControl("dropdown_tipo_paquete") as DropDownList);
            string seleccionado = dropdown_tipo_paquete.SelectedItem.Text;
            insumos_proveedorBD.Rows[fila_tabla]["unidad_medida"] = seleccionado;
            Session.Add("insumos_proveedorBD_OP", insumos_proveedorBD);


            if (double.TryParse(txtValor.Text.Replace(",", "."), out cantidad))
            {

                cargar_producto_en_resumen(cantidad.ToString(), "N/A");
                cargar_insumos();

            }

        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_seleccionada;

            id_seleccionada = gridview_resumen.SelectedRow.Cells[0].Text;


            eliminar_producto_en_resumen(id_seleccionada);
            cargar_insumos();

        }

        protected void dropdown_tipo_paquete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            insumos_proveedorBD = (DataTable)Session["insumos_proveedorBD_OP"];

            int fila_tabla = 0;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridview_productos.Rows[fila].Cells[0].Text, insumos_proveedorBD);

                DropDownList dropdown_tipo_paquete = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_tipo_paquete") as DropDownList);
                dropdown_tipo_paquete.SelectedValue = insumos_proveedorBD.Rows[fila_tabla]["unidad_medida"].ToString();

            }
        }
    }
}