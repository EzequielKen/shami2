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
        private void cambiar_estado_producto(string id, string nota)
        {
            productosBD = (DataTable)Session["productosBD"];

            int fila_producto = funciones.buscar_fila_por_id(id, productosBD);
            string faltante = productosBD.Rows[fila_producto]["faltante"].ToString();
            if (faltante == "N/A")
            {
                productosBD.Rows[fila_producto]["faltante"] = "Si";
                productosBD.Rows[fila_producto]["nota"] = nota;
            }
            else
            {
                productosBD.Rows[fila_producto]["faltante"] = "N/A";
            }
            string id_sucursal = sucursal.Rows[0]["id"].ToString();
            string nombre_sucursal = sucursal.Rows[0]["sucursal"].ToString();
            string id_faltante = productosBD.Rows[fila_producto]["id_faltante"].ToString();
            if (id_faltante != "N/A")
            {
                faltantes.desactivar_lista_faltante(id_faltante);
            }
            faltantes.cargar_lista_faltante(id_sucursal, nombre_sucursal, productosBD);
        }
        private void cargar_nota(string id, string nota)
        {
            productosBD = (DataTable)Session["productosBD"];

            int fila_producto = funciones.buscar_fila_por_id(id, productosBD);
            string faltante = productosBD.Rows[fila_producto]["faltante"].ToString();

            productosBD.Rows[fila_producto]["faltante"] = "Si";
            productosBD.Rows[fila_producto]["nota"] = nota;

            string id_sucursal = sucursal.Rows[0]["id"].ToString();
            string nombre_sucursal = sucursal.Rows[0]["sucursal"].ToString();
            string id_faltante = productosBD.Rows[fila_producto]["id_faltante"].ToString();
            if (id_faltante != "N/A")
            {
                faltantes.desactivar_lista_faltante(id_faltante);
            }
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
        private void crear_tabla_productos_faltantes()
        {
            productos_faltantes = new DataTable();
            productos_faltantes.Columns.Add("id", typeof(string));
            productos_faltantes.Columns.Add("producto", typeof(string));
            productos_faltantes.Columns.Add("nota", typeof(string));
            productos_faltantes.Columns.Add("tipo_producto", typeof(string));
            productos_faltantes.Columns.Add("proveedor", typeof(string));
            productos_faltantes.Columns.Add("faltante", typeof(string));
        }
        private void llenar_tabla_productos()
        {
            productosBD = (DataTable)Session["productosBD"];
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
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productosBD.Rows[fila]["producto"].ToString()))
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
            crear_tabla_productos_faltantes();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["faltante"].ToString() == "Si")
                {
                    productos_faltantes.Rows.Add();
                    ultima_fila = productos_faltantes.Rows.Count - 1;
                    productos_faltantes.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos_faltantes.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    productos_faltantes.Rows[ultima_fila]["nota"] = productosBD.Rows[fila]["nota"].ToString();
                    productos_faltantes.Rows[ultima_fila]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos_faltantes.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    productos_faltantes.Rows[ultima_fila]["faltante"] = productosBD.Rows[fila]["faltante"].ToString();
                }
            }
        }

        private void cargar_productos()
        {
            llenar_tabla_productos();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
            cargar_productos_faltantes();
        }
        private void cargar_productos_busqueda()
        {
            llenar_tabla_productos_busqueda();
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
            cargar_productos_faltantes();
        }
        private void cargar_productos_faltantes()
        {
            llenar_tabla_productos_faltantes();
            gridview_resumen.DataSource = productos_faltantes;
            gridview_resumen.DataBind();
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
        DataTable productos_faltantes;
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

            TextBox textbox_nota = (gridview_productos.Rows[rowIndex].Cells[2].FindControl("textbox_nota") as TextBox);

            string id = gridview_productos.Rows[rowIndex].Cells[0].Text;
            string nota = "N/A";
            if (textbox_nota.Text != string.Empty)
            {
                nota = textbox_nota.Text;
            }
            cambiar_estado_producto(id, nota);
            productosBD = faltantes.get_lista_producto(sucursal.Rows[0]["id"].ToString());
            Session.Add("productosBD", productosBD);

            Response.Redirect("~/paginas/lista_de_faltantes.aspx", false);

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
                Button boton_nota = (gridview_productos.Rows[fila].Cells[2].FindControl("boton_nota") as Button);
                Button boton_faltante = (gridview_productos.Rows[fila].Cells[3].FindControl("boton_faltante") as Button);
                if (productosBD.Rows[fila_producto]["faltante"].ToString() == "Si")
                {
                    gridview_productos.Rows[fila].CssClass = "table-danger";
                    boton_nota.Visible = true;
                    boton_faltante.CssClass = "btn btn-danger";
                    boton_faltante.Text = "Desmarcar";
                }
                else
                {
                //    boton_nota.Visible = false;
                    boton_faltante.CssClass = "btn btn-primary";
                    boton_faltante.Text = "Marcar Como Faltante";
                }
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {

            cargar_productos_busqueda();

        }

        protected void dropdown_filtro_SelectedIndexChanged(object sender, EventArgs e)
        {

            cargar_productos();

        }

        protected void gridview_resumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            productosBD = (DataTable)Session["productosBD"];
            string id_producto, producto;
            int fila_producto;
            for (int fila = 0; fila <= gridview_resumen.Rows.Count - 1; fila++)
            {
                id_producto = gridview_resumen.Rows[fila].Cells[0].Text;
                producto = gridview_resumen.Rows[fila].Cells[1].Text;
                fila_producto = funciones.buscar_fila_por_id_nombre(id_producto, producto, productosBD);
                Button boton_faltante_resumen = (gridview_resumen.Rows[fila].Cells[2].FindControl("boton_faltante_resumen") as Button);
                if (productosBD.Rows[fila_producto]["faltante"].ToString() == "Si")
                {
                    gridview_resumen.Rows[fila].CssClass = "table-danger";

                    boton_faltante_resumen.CssClass = "btn btn-danger";
                    boton_faltante_resumen.Text = "Desmarcar";
                }
                else
                {
                    boton_faltante_resumen.CssClass = "btn btn-primary";
                    boton_faltante_resumen.Text = "Marcar Como Faltante";
                }
            }
        }

        protected void boton_faltante_resumen_Click(object sender, EventArgs e)
        {
            Button boton_faltante = (Button)sender;
            GridViewRow row = (GridViewRow)boton_faltante.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_resumen.Rows[rowIndex].Cells[0].Text;
            cambiar_estado_producto(id, "N/A");
            productosBD = faltantes.get_lista_producto(sucursal.Rows[0]["id"].ToString());
            Session.Add("productosBD", productosBD);
            Response.Redirect("~/paginas/lista_de_faltantes.aspx", false);

        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " resumen id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            faltantes.crear_pdf(ruta_archivo, imgdata, (DataTable)Session["productosBD"], sucursal.Rows[0]["sucursal"].ToString());
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginas/pdf/" + id_pedido;
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

            }
            catch (Exception)
            {

                Response.Redirect(strUrl, false);
            }
            //GenerarPDF_Click();

        }



        protected void boton_nota_Click(object sender, EventArgs e)
        {
            Button boton_faltante = (Button)sender;
            GridViewRow row = (GridViewRow)boton_faltante.NamingContainer;
            int rowIndex = row.RowIndex;

            TextBox textbox_nota = (gridview_productos.Rows[rowIndex].Cells[2].FindControl("textbox_nota") as TextBox);

            string id = gridview_productos.Rows[rowIndex].Cells[0].Text;
            string nota = "N/A";
            if (textbox_nota.Text != string.Empty)
            {
                nota = textbox_nota.Text;
            }
            cargar_nota(id, nota);
            productosBD = faltantes.get_lista_producto(sucursal.Rows[0]["id"].ToString());
            Session.Add("productosBD", productosBD);

            Response.Redirect("~/paginas/lista_de_faltantes.aspx", false);
        }
    }
}