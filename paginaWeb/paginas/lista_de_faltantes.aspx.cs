using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class lista_de_faltantes : System.Web.UI.Page
    {
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
            for (int fila = 0; fila <= productosBD.Rows.Count-1; fila++)
            {
                if (funciones.verificar_tipo_producto(productosBD.Rows[fila]["tipo_producto"].ToString(),dropDown_tipo.SelectedItem.Text))
                {
                    productos.Rows.Add();
                    ultima_fila=productos.Rows.Count-1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    productos.Rows[ultima_fila]["faltante"] = productosBD.Rows[fila]["faltante"].ToString();
                }
            }
        }
        private void cargar_productos()
        {
            llenar_tabla_productos();
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
        DataTable empleado;
        DataTable productosBD;
        DataTable productos;
        DateTime fecha_de_hoy = DateTime.Now;



        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["faltantes"] == null)
            {
                Session.Add("faltantes", new cls_lista_de_faltantes(usuariosBD));
            }
            faltantes = (cls_lista_de_faltantes)Session["faltantes"];
            
            if (!IsPostBack)
            {
                productosBD = faltantes.get_lista_producto();
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

        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}