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
    public partial class recetario : System.Web.UI.Page
    {
        #region carga productos
        private void crear_tabla_productos()
        {
            productos_proveedor = new DataTable();
            productos_proveedor.Columns.Add("id", typeof(string));
            productos_proveedor.Columns.Add("producto", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila < productos_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productos_proveedorBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(productos_proveedorBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    productos_proveedor.Rows.Add();
                    productos_proveedor.Rows[fila_producto]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                    productos_proveedor.Rows[fila_producto]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();
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
       
        #endregion
        #region dropDowns
        private void configurar_controles()
        {
            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            llenar_dropDownList(productos_proveedorBD);
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
        cls_recetario lista_productos_recetario;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuarioBD;

        DataTable productos_proveedorBD;
        DataTable productos_proveedor;
        DataTable historial_producto;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            
            lista_productos_recetario = new cls_recetario(usuariosBD);
            productos_proveedorBD = lista_productos_recetario.get_productos_proveedor();
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_productos();
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

        protected void gridview_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "boton_ver_receta")
            {

            }
            else if (e.CommandName== "boton_editar_receta")
            {
                Response.Redirect("/paginasFabrica/recetario_linkear_sub_producto.aspx");
            }
        }
    }
}