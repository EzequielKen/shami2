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
    public partial class historial_de_stock_insumos : System.Web.UI.Page
    {
        #region carga productos
        private void crear_tabla_productos()
        {
            productos_proveedor = new DataTable();
            productos_proveedor.Columns.Add("id", typeof(string));
            productos_proveedor.Columns.Add("producto", typeof(string));
            productos_proveedor.Columns.Add("ultimo_stock", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila < insumos_fabricaBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, insumos_fabricaBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(insumos_fabricaBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    productos_proveedor.Rows.Add();
                    productos_proveedor.Rows[fila_producto]["id"] = insumos_fabricaBD.Rows[fila]["id"].ToString();
                    productos_proveedor.Rows[fila_producto]["producto"] = insumos_fabricaBD.Rows[fila]["producto"].ToString();
                    //productos_proveedor.Rows[fila_producto]["ultimo_stock"] = insumos_fabricaBD.Rows[fila]["ultimo_stock"].ToString();
                    productos_proveedor.Rows[fila_producto]["ultimo_stock"] = "N/A";
                    fila_producto++;
                }
            }
        }
        private void cargar_productos()
        {
            llenar_tabla_productos();
            gridview_productos.DataSource = productos_proveedor;
            gridview_productos.DataBind();
        }
        private void cargar_historial(string presentacion)
        {
            gridview_historial.DataSource = historial_stock_insumos.get_historial_insumo(Session["id_producto_historial"].ToString(), presentacion, DropDown_mes.SelectedItem.Text, DropDown_año.SelectedItem.Text);
            gridview_historial.DataBind();
        }
        #endregion
        #region dropDowns
        private void configurar_controles()
        {
            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();
            llenar_dropDownList(insumos_fabricaBD);
        }

        private void cargar_mes()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                DropDown_mes.Items.Add(item);
                num_item++;
            }
            DropDown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                DropDown_año.Items.Add(item);
                num_item++;
            }
            string añ = DateTime.Now.Year.ToString();

            DropDown_año.SelectedIndex = DropDown_año.Items.Count - 1;
        }

        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            //dt.DefaultView.Sort = "tipo_producto";
            //dt = dt.DefaultView.ToTable();

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
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_movimientos_stock_insumos historial_stock_insumos;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;
        DataTable proveedorBD;
        DataTable tipo_usuarioBD;

        DataTable insumos_fabricaBD;
        DataTable productos_proveedor;
        DataTable historial_producto;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            historial_stock_insumos = new cls_movimientos_stock_insumos(usuariosBD);
            insumos_fabricaBD = historial_stock_insumos.get_insumos_fabrica();
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_productos();
            }
            if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
            {
                gridview_productos.Columns[3].Visible = false;
            }
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        protected void DropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_historial(Session["presentacion_seleccionada"].ToString());
        }

        protected void DropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_historial(Session["presentacion_seleccionada"].ToString());
        }

        protected void texbox_nuevo_stock_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_nuevo_stock = (TextBox)sender;
            double cantidad;
            if (!double.TryParse(textbox_nuevo_stock.Text, out cantidad))
            {
                textbox_nuevo_stock.Text = string.Empty;
            }
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int fila = row.RowIndex;

            TextBox texbox_nuevo_stock = (gridview_productos.Rows[fila].Cells[2].FindControl("texbox_nuevo_stock") as TextBox);
            TextBox texbox_nota = (gridview_productos.Rows[fila].Cells[2].FindControl("texbox_nota") as TextBox);
            DropDownList dropdown_presentacion = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_presentacion") as DropDownList);

            string rol_usuario = tipo_usuario.Rows[0]["rol"].ToString();
            string id_producto = gridview_productos.Rows[fila].Cells[0].Text;
            if (texbox_nuevo_stock.Text != string.Empty)
            {
                historial_stock_insumos.cargar_historial_stock(rol_usuario, id_producto, "conteo stock", texbox_nuevo_stock.Text, texbox_nota.Text,dropdown_presentacion.SelectedItem.Text);
                Session.Add("id_producto_historial", id_producto);
                cargar_historial(Session["presentacion_seleccionada"].ToString());
                texbox_nuevo_stock.Text = string.Empty;
                texbox_nota.Text = string.Empty;
            }
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id_insumo, tipo_paquete, unidades, unidad, presentacion;
            int fila_insumo;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                id_insumo = gridview_productos.Rows[fila].Cells[0].Text;
                fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabricaBD);
                DropDownList dropdown_presentacion = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_presentacion") as DropDownList);
                dropdown_presentacion.Items.Clear();
                for (int columna = insumos_fabricaBD.Columns["producto_1"].Ordinal; columna <= insumos_fabricaBD.Columns.Count - 1; columna++)
                {
                    if (insumos_fabricaBD.Rows[fila_insumo][columna].ToString() != "N/A")
                    {
                        tipo_paquete = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 1);
                        unidades = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 2);
                        unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 3);
                        presentacion = tipo_paquete + "-" + unidades + "-" + unidad;
                        dropdown_presentacion.Items.Add(presentacion);
                    }
                    else if (insumos_fabricaBD.Rows[fila_insumo][columna].ToString() == "N/A")
                    {
                        break;
                    }
                }
            }
        }

        protected void boton_historial_Click(object sender, EventArgs e)
        {
            Button boton_historial = (Button)sender;
            GridViewRow row = (GridViewRow)boton_historial.NamingContainer;
            int fila = row.RowIndex;

            DropDownList dropdown_presentacion = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_presentacion") as DropDownList);
            Session.Add("presentacion_seleccionada", dropdown_presentacion.SelectedItem.Text);

            Session.Add("id_producto_historial", gridview_productos.Rows[fila].Cells[0].Text);
            cargar_historial(Session["presentacion_seleccionada"].ToString());
        }
    }
}