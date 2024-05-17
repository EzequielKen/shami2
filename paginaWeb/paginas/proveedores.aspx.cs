using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb
{
    public partial class pedidos : System.Web.UI.Page
    {
        private void crear_tabla_proveedores()
        {
            proveedores = new DataTable();
            proveedores.Columns.Add("id", typeof(string));
            proveedores.Columns.Add("nombre_proveedor", typeof(string));
        }

        private void llenar_tabla_proveedores()
        {
            crear_tabla_proveedores();
            proveedoresBD = (DataTable)Session["lista_proveedores"];
            int fila_proveedor = 0;
            for (int fila = 0; fila <= proveedoresBD.Rows.Count - 1; fila++)
            {
                if (proveedoresBD.Rows[fila]["id"].ToString()=="1")
                {
                    proveedores.Rows.Add();
                    proveedores.Rows[fila_proveedor]["id"] = proveedoresBD.Rows[fila]["id"].ToString();
                    proveedores.Rows[fila_proveedor]["nombre_proveedor"] = proveedoresBD.Rows[fila]["nombre_proveedor"].ToString();
                    fila_proveedor++;
                }
            }
        }

        private void cargar_lista_proveedores()
        {
            llenar_tabla_proveedores();
            gridview_proveedores.DataSource = proveedores;
            gridview_proveedores.DataBind();
        }

        private bool verificar_si_cargar_insumos(string nombre_proveedor)
        {
            bool retorno = true;
            if (nombre_proveedor == "Shami Insumos")
            {

                if (sucusalBD.Rows[0]["id"].ToString() == "18" ||
                    sucusalBD.Rows[0]["id"].ToString() == "19" ||
                    sucusalBD.Rows[0]["id"].ToString() == "17" ||
                    sucusalBD.Rows[0]["id"].ToString() == "24" ||
                    sucusalBD.Rows[0]["id"].ToString() == "13")
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }

            return retorno;
        }
        private bool verificar_si_cargar_vegetales(string nombre_proveedor)
        {
            bool retorno = true;
            if (nombre_proveedor == "Shami Vegetales")
            {

                if (sucusalBD.Rows[0]["id"].ToString() == "16" ||
                    sucusalBD.Rows[0]["id"].ToString() == "22")
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }

            return retorno;
        }
        cls_sistema_pedidos sistema_pedidos;
        DataTable usuariosBD;
        DataTable sucusalBD;
        DataTable proveedoresBD;
        DataTable proveedores;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("Default.aspx", false);
            }
            else
            {
                usuariosBD = (DataTable)Session["usuariosBD"];
                sucusalBD = (DataTable)Session["sucursal"];

                if (!IsPostBack)
                {
                    if (HttpContext.Current.Session["lista_proveedores"] == null)
                    {
                        sistema_pedidos = new cls_sistema_pedidos(usuariosBD, sucusalBD);
                        Session.Add("lista_proveedores", sistema_pedidos.cargar_lista_proveedores());
                    }
                    if (HttpContext.Current.Session["productos_proveedor"] != null)
                    {
                        Session.Remove("productos_proveedor");
                    }

                }
                cargar_lista_proveedores();

            }


        }

        protected void gridview_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("nombre_proveedor", gridview_proveedores.SelectedRow.Cells[1].Text);
            Response.Redirect("pedido.aspx", false);


        }
    }
}