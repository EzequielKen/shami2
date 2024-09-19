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
    public partial class analisis_de_ventas : System.Web.UI.Page
    {
        #region carga productos
        private void crear_tabla_productos()
        {
            productos_proveedor = new DataTable();
            productos_proveedor.Columns.Add("id",typeof(string));
            productos_proveedor.Columns.Add("producto",typeof(string));
        }
        private void llenar_tabla_producto()
        {
            crear_tabla_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count-1; fila++)
            {
                if (funciones.verificar_tipo_producto(productos_proveedorBD.Rows[fila]["tipo_producto"].ToString(),dropDown_tipo.SelectedItem.Text))
                {
                    productos_proveedor.Rows.Add();
                    ultima_fila = productos_proveedor.Rows.Count-1;
                    productos_proveedor.Rows[ultima_fila]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                    productos_proveedor.Rows[ultima_fila]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();

                }
            }
        }
        private void cargar_productos()
        {
            llenar_tabla_producto();
            gridview_productos.DataSource = productos_proveedor;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            llenar_dropDownList(productos_proveedorBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;

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
        /// /////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_analisis_de_ventas analisis;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable productos_proveedorBD;
        DataTable productos_proveedor;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            analisis = new cls_analisis_de_ventas(usuariosBD);
            productos_proveedorBD = analisis.get_productos_proveedor();
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_productos();
            }
        }
        protected void textbox_fecha_inicial_TextChanged(object sender, EventArgs e)
        {
            Session.Add("fecha_inicial", DateTime.Parse(textbox_fecha_inicial.Text));
        }

        protected void textbox_fecha_final_TextChanged(object sender, EventArgs e)
        {
            Session.Add("fecha_final", DateTime.Parse(textbox_fecha_final.Text));
        }
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + "analisis- id-" + dato_hora + ".pdf";
            string ruta = "/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            //analisis.crear_pdf(ruta_archivo,imgdata);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

            }
            catch (Exception)
            {

                Response.Redirect(strUrl, false);
            }
        }


        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void boton_analisis_Click(object sender, EventArgs e)
        {

        }


    }
}