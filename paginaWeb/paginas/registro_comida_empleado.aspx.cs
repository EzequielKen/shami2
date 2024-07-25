using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class registro_comida_empleado : System.Web.UI.Page
    {
        #region resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("cantidad", typeof(string)); 
            resumen.Columns.Add("costo", typeof(string)); 
            Session.Add("resumen_comida_empleados", resumen);
        }
        private void cargar_producto_en_resumen(string id_producto, string cantidad)
        {
            resumen = (DataTable)Session["resumen_comida_empleados"];
            int ultima_fila, fila_producto;
            if (!funciones.verificar_si_cargo(id_producto, resumen))
            {
                fila_producto = funciones.buscar_fila_por_id(id_producto, productosBD);
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = productosBD.Rows[fila_producto]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = productosBD.Rows[fila_producto]["producto"].ToString();
                resumen.Rows[ultima_fila]["cantidad"] = cantidad; 
                resumen.Rows[ultima_fila]["costo"] = productosBD.Rows[fila_producto]["costo"].ToString();

            }
            Session.Add("resumen_comida_empleados", resumen);
        }

        #endregion
        #region cargar lista
        private void crear_tabla_productos()
        {
            productos = new DataTable();
            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_tipo_producto(productosBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                }
            }
        }
        private void llenar_tabla_productos_busqueda()
        {
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    ultima_fila = productos.Rows.Count - 1;
                    productos.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                }
            }
        }
        private void cargar_productos()
        {
            llenar_tabla_productos();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
            cargar_resumen();

        }
        private void cargar_productos_busqueda()
        {
            llenar_tabla_productos_busqueda();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
            cargar_resumen();

        }
        private void cargar_resumen()
        {
            resumen = (DataTable)Session["resumen_comida_empleados"];
            gridview_RESUMEN.DataSource = resumen;
            gridview_RESUMEN.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            llenar_dropDownList(productosBD);
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
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_registro_comida_empleado registro;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;

        DataTable lista_de_empleadoBD;
        DataTable productosBD;
        DataTable productos;
        DataTable resumen;

        string nombre_empleado, apellido_empleado, nombre_completo;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            lista_de_empleadoBD = (DataTable)Session["lista_de_empleadoBD"];
            
            nombre_empleado = Session["nombre_empleado"].ToString();
            apellido_empleado = Session["apellido_empleado"].ToString();
            nombre_completo = nombre_empleado + " " + apellido_empleado;
            label_nombre_empleado.Text = "Empleado Seleccionado: " + nombre_completo;
            registro = new cls_registro_comida_empleado(usuariosBD);
            productosBD = registro.get_lista_productos_comida_empleados();
            if (!IsPostBack)
            {
                crear_tabla_resumen();
                configurar_controles();
                cargar_productos();
            }
        }
        #region controles
        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            if (textbox_buscar.Text == string.Empty)
            {
                cargar_productos();
            }
            else
            {
                cargar_productos_busqueda();
            }
        }

        protected void boton_buscar_Click(object sender, EventArgs e)
        {
            if (textbox_buscar.Text == string.Empty)
            {
                cargar_productos();
            }
            else
            {
                cargar_productos_busqueda();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        #endregion

        #region lista
        protected void textbox_cantidad_TextChanged(object sender, EventArgs e)
        {

        }

        protected void boton_registrar_Click(object sender, EventArgs e)
        {
          
            resumen = (DataTable)Session["resumen_comida_empleados"];
            registro.registrar_consumo(Session["id_empleado"].ToString(), Session["nombre_empleado"].ToString(), Session["apellido_empleado"].ToString(), resumen);
            Response.Redirect("~/paginas/comida_empleados.aspx", false);
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int rowIndex = row.RowIndex;

            TextBox textbox_cantidad = (gridview_productos.Rows[rowIndex].Cells[0].FindControl("textbox_cantidad") as TextBox);


            string id = gridview_productos.Rows[rowIndex].Cells[0].Text;
            double cantidad;
            if (double.TryParse(textbox_cantidad.Text, out cantidad))
            {
                cargar_producto_en_resumen(id, cantidad.ToString());
            }
            if (textbox_buscar.Text == string.Empty)
            {
                cargar_productos();
            }
            else
            {
                cargar_productos_busqueda();
            }
        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {

        }
        #endregion


    }
}