using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Design;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class lista_de_faltantes : System.Web.UI.Page
    {
        #region carga base de datos
        private void cambiar_estado_producto(string id)
        {
            productosBD = (DataTable)Session["productosBD"];

            int fila_producto = funciones.buscar_fila_por_id(id, productosBD);
            string faltante = productosBD.Rows[fila_producto]["faltante"].ToString();
            if (faltante == "N/A")
            {
                productosBD.Rows[fila_producto]["faltante"] = "Si";
            }
            else
            {
                productosBD.Rows[fila_producto]["faltante"] = "N/A";
            }
            string id_sucursal = sucursal.Rows[0]["id"].ToString();
            string nombre_sucursal = sucursal.Rows[0]["sucursal"].ToString();
            faltantes.cargar_lista_faltante(id_sucursal, nombre_sucursal, productosBD);
        }
        #endregion
        #region lista
        private void crear_tabla_productos()
        {
            productos = new DataTable();
            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("tipo_producto", typeof(string));
            productos.Columns.Add("proveedor", typeof(string));
            productos.Columns.Add("faltante", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            productosBD = (DataTable)Session["productosBD"];
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_tipo_producto(productosBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text) &&
                    verificar_filtro(productosBD.Rows[fila]["faltante"].ToString()))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    productos.Rows[ultima_fila]["faltante"] = productosBD.Rows[fila]["faltante"].ToString();
                }
            }
        }
        private void llenar_tabla_productos_busqueda()
        {
            productosBD = (DataTable)Session["productosBD"];
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productosBD.Rows[fila]["producto"].ToString()) &&
                    verificar_filtro(productosBD.Rows[fila]["faltante"].ToString()))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    productos.Rows[ultima_fila]["faltante"] = productosBD.Rows[fila]["faltante"].ToString();
                }
            }
        }
        private void llenar_tabla_productos_faltantes()
        {
            productosBD = (DataTable)Session["productosBD"];
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productosBD.Rows[fila]["producto"].ToString()) &&
                    verificar_filtro(productosBD.Rows[fila]["faltante"].ToString()))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    productos.Rows[ultima_fila]["faltante"] = productosBD.Rows[fila]["faltante"].ToString();
                }
            }
        }
        private bool verificar_filtro(string faltante)
        {
            bool retorno = false;
            if (dropdown_filtro.SelectedItem.Text == "Todos")
            {
                retorno = true;
            }
            else if (dropdown_filtro.SelectedItem.Text == "Faltantes" &&
                    faltante == "Si")
            {
                retorno = true;
            }
            return retorno;
        }
        private void cargar_productos()
        {
            llenar_tabla_productos();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        private void cargar_productos_busqueda()
        {
            llenar_tabla_productos_busqueda();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        private void cargar_productos_faltantes()
        {
            llenar_tabla_productos_faltantes();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
        }
        #endregion
        #region controles
        private void configurar_controles()
        {
            productosBD = (DataTable)Session["productosBD"];
            llenar_dropDownList(productosBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "orden_tipo ASC";
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
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_lista_de_faltantes faltantes;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;
        DataTable empleado;
        DataTable productosBD;
        DataTable productos;
        DateTime fecha_de_hoy = DateTime.Now;



        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            if (Session["faltantes"] == null)
            {
                Session.Add("faltantes", new cls_lista_de_faltantes(usuariosBD));
            }
            faltantes = (cls_lista_de_faltantes)Session["faltantes"];

            if (!IsPostBack)
            {
                productosBD = faltantes.get_lista_producto(sucursal.Rows[0]["id"].ToString());
                Session.Add("productosBD", productosBD);
                configurar_controles();
                cargar_productos();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged1(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void boton_faltante_Click(object sender, EventArgs e)
        {
            Button boton_faltante = (Button)sender;
            GridViewRow row = (GridViewRow)boton_faltante.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_productos.Rows[rowIndex].Cells[0].Text;
            cambiar_estado_producto(id);
            productosBD = faltantes.get_lista_producto(sucursal.Rows[0]["id"].ToString());
            Session.Add("productosBD", productosBD);
            if (dropdown_filtro.SelectedItem.Text == "Todos")
            {
                cargar_productos();
            }
            else
            {
                cargar_productos_faltantes();
            }
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            productosBD = (DataTable)Session["productosBD"];
            string id_producto, producto;
            int fila_producto;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                id_producto = gridview_productos.Rows[fila].Cells[0].Text;
                producto = gridview_productos.Rows[fila].Cells[1].Text;
                fila_producto = funciones.buscar_fila_por_id_nombre(id_producto, producto, productosBD);
                Button boton_faltante = (gridview_productos.Rows[fila].Cells[0].FindControl("boton_faltante") as Button);
                if (productosBD.Rows[fila_producto]["faltante"].ToString() == "Si")
                {
                    gridview_productos.Rows[fila].CssClass = "table-danger";

                    boton_faltante.CssClass = "btn btn-danger";
                    boton_faltante.Text = "Desmarcar";
                }
                else
                {
                    boton_faltante.CssClass = "btn btn-primary";
                    boton_faltante.Text = "Marcar Como Faltante";
                }
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            if (textbox_buscar.Text == string.Empty &&
                dropdown_filtro.SelectedItem.Text == "Todos")
            {
                cargar_productos();
            }
            else
            {
                if (dropdown_filtro.SelectedItem.Text == "Todos")
                {
                    cargar_productos_busqueda();
                }
                else
                {
                    cargar_productos_faltantes();
                }
            }
        }

        protected void dropdown_filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropdown_filtro.SelectedItem.Text == "Todos")
            {
                cargar_productos();
            }
            else
            {
                cargar_productos_faltantes();
            }
        }
    }
}