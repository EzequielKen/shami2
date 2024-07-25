using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class pedidos : System.Web.UI.Page
    {
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("sucursal", typeof(string));
            resumen.Columns.Add("num_pedido", typeof(string));
            resumen.Columns.Add("fecha", typeof(string));
            resumen.Columns.Add("total_pedido", typeof(string)); 
            resumen.Columns.Add("proveedor", typeof(string)); 
            Session.Add("resumen_de_pedidos", resumen);
        }
        private void cargar_pedido_en_resumen(string id_pedido)
        {
            resumen = (DataTable)Session["resumen_de_pedidos"];
            int fila_pedidos = funciones.buscar_fila_por_id(id_pedido, pedidos_sucursal);
            int fila_resumen = funciones.buscar_fila_por_id(id_pedido, resumen);
            if (-1 == fila_resumen)
            {
                resumen.Rows.Add();
                int ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = pedidos_sucursal.Rows[fila_pedidos]["id"].ToString();
                resumen.Rows[ultima_fila]["sucursal"] = pedidos_sucursal.Rows[fila_pedidos]["sucursal"].ToString();
                resumen.Rows[ultima_fila]["num_pedido"] = pedidos_sucursal.Rows[fila_pedidos]["num_pedido"].ToString();
                resumen.Rows[ultima_fila]["fecha"] = pedidos_sucursal.Rows[fila_pedidos]["fecha"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = pedidos_sucursal.Rows[fila_pedidos]["proveedor"].ToString();
                resumen.Rows[ultima_fila]["total_pedido"] = "0";
                Session.Add("resumen_de_pedidos", resumen);
            }
        }
        private void eliminar_pedido_en_resumen(string id_pedido)
        {
            resumen = (DataTable)Session["resumen_de_pedidos"];
            int fila_resumen = funciones.buscar_fila_por_id(id_pedido, resumen);
            resumen.Rows[fila_resumen].Delete();
        }
        private void cargar_pedidos()
        {
            gridView_pedidos.DataSource = pedidos_sucursal;
            gridView_pedidos.DataBind();
            resumen = (DataTable)Session["resumen_de_pedidos"];
            gridView_resumen.DataSource = resumen;
            gridView_resumen.DataBind();
            if (0 < resumen.Rows.Count)
            {
                label_titulo_resumen.Visible = true;
                boton_abrir.Visible = true;
            }
            else
            {
                label_titulo_resumen.Visible = false;
                boton_abrir.Visible = false;
            }
        }
        ///////////////////////////////////////////////////////////////////////////

        cls_sistema_pedidos_fabrica pedidos_fabrica;
        cls_funciones funciones = new cls_funciones();
        cls_landing_page landing;
        DataTable tipo_usuarioBD;
        DataTable sucursalesBD;
        DataTable usuariosBD;
        DataTable pedidos_sucursal;
        DataTable proveedorBD;
        DataTable resumen;
        protected void Page_Load(object sender, EventArgs e)
        {
            
                proveedorBD = (DataTable)Session["proveedorBD"];
                usuariosBD = (DataTable)Session["usuariosBD"];
                if (!IsPostBack)
                {

                    //calcular remitos nuevos
                    Session.Add("pedidos_fabrica", new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]));

                    pedidos_fabrica = (cls_sistema_pedidos_fabrica)Session["pedidos_fabrica"];

                    crear_tabla_resumen();
                }

                if (Session["sucursalesBD"] == null)
                {
                    sucursalesBD = pedidos_fabrica.get_sucursales();

                    Session.Add("sucursalesBD", sucursalesBD);
                }
                else
                {
                    sucursalesBD = (DataTable)Session["sucursalesBD"];
                }


                pedidos_fabrica = new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]);



                Session.Remove("pedidos_sucursal");
                Session.Add("pedidos_sucursal", pedidos_fabrica.get_pedidos_sucursal(Session["nombre_sucursal"].ToString(), proveedorBD.Rows[0]["nombre_en_BD"].ToString(), usuariosBD.Rows[0]["proveedor"].ToString()));

                if (Session["pedido"] != null)
                {
                    Session.Remove("pedido");
                }
                label_nombre_sucursal.Text = Session["nombre_sucursal"].ToString();
                pedidos_sucursal = (DataTable)Session["pedidos_sucursal"];
                cargar_pedidos();
            


        }

        protected void gridView_pedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_abrir")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                resumen = (DataTable)Session["resumen_de_pedidos"];
                resumen.Rows.Clear();
                Session.Add("resumen_de_pedidos", resumen);
                cargar_pedido_en_resumen(gridView_pedidos.Rows[fila].Cells[0].Text);
                Response.Redirect("~/paginasFabrica/cargar_pedido.aspx", false);
            }
            else if (e.CommandName == "boton_seleccionar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                cargar_pedido_en_resumen(gridView_pedidos.Rows[fila].Cells[0].Text);
                cargar_pedidos();
            }
        }

        protected void gridView_resumen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_eliminar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                eliminar_pedido_en_resumen(gridView_resumen.Rows[fila].Cells[0].Text);
                cargar_pedidos();
            }
        }

        protected void boton_abrir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasFabrica/cargar_pedido.aspx", false);
        }
    }
}