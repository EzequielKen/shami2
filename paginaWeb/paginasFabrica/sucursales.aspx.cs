using _02___sistemas;
using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{

    public partial class sucursales : System.Web.UI.Page
    {

        private void crear_tabla_sucursales()
        {
            sucursales_usuario = new DataTable();

            sucursales_usuario.Columns.Add("id", typeof(string));
            sucursales_usuario.Columns.Add("sucursal", typeof(string));
            sucursales_usuario.Columns.Add("direccion", typeof(string));
            sucursales_usuario.Columns.Add("pedidos_pendiente", typeof(string));

        }
        private void llenar_tabla_sucursales()
        {
            crear_tabla_sucursales();
            int fila_sucursal = 0;
            for (int fila = 0; fila <= sucursalesBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, sucursalesBD.Rows[fila]["sucursal"].ToString()))
                {
                    sucursales_usuario.Rows.Add();

                    sucursales_usuario.Rows[fila_sucursal]["id"] = sucursalesBD.Rows[fila]["id"].ToString();
                    sucursales_usuario.Rows[fila_sucursal]["sucursal"] = sucursalesBD.Rows[fila]["sucursal"].ToString();
                    sucursales_usuario.Rows[fila_sucursal]["direccion"] = sucursalesBD.Rows[fila]["direccion"].ToString();
                    sucursales_usuario.Rows[fila_sucursal]["pedidos_pendiente"] = cantidad_de_pedidos_pendientes(sucursalesBD.Rows[fila]["sucursal"].ToString());

                    fila_sucursal++;
                }

            }
        }
        private int cantidad_de_pedidos_pendientes(string sucursal)
        {
            DataTable recuento = new DataTable();
            recuento.Columns.Add("num_pedido", typeof(string));
            int retorno = 0;
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                if (pedidos_no_cargados.Rows[fila]["sucursal"].ToString() == sucursal)
                {
                    if (-1 == funciones.buscar_fila_por_dato(pedidos_no_cargados.Rows[fila]["num_pedido"].ToString(), "num_pedido", recuento))
                    {
                        recuento.Rows.Add();
                        recuento.Rows[recuento.Rows.Count - 1]["num_pedido"] = pedidos_no_cargados.Rows[fila]["num_pedido"].ToString();
                        retorno++;
                    }
                }
            }
            return retorno;
        }
        private void cargar_sucursales()
        {
            llenar_tabla_sucursales();
            gridView_sucursales.DataSource = sucursales_usuario;
            gridView_sucursales.DataBind();
        }

        ///////////////////////////////////////////////////////////////////////////
        cls_sistema_pedidos_fabrica pedidos_fabrica;
        cls_funciones funciones = new cls_funciones();
        cls_landing_page landing;
        DataTable tipo_usuarioBD;

        DataTable usuariosBD;
        DataTable tipo_usuario;

        DataTable sucursalesBD;
        DataTable sucursales_usuario;
        DataTable proveedorBD;


        DataTable pedidos_no_cargados;
        protected void Page_Load(object sender, EventArgs e)
        {
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];

            landing = new cls_landing_page((DataTable)Session["usuariosBD"]);
            if (landing.verificar_si_registro("3") || landing.verificar_si_registro("4") ||
                "Shami Villa Maipu Expedicion" != tipo_usuarioBD.Rows[0]["rol"].ToString())
            {

                int nivel_seguridad = int.Parse(Session["nivel_seguridad"].ToString());
                if (nivel_seguridad > 2)
                {
                    gridView_sucursales.Visible = false;
                }
                proveedorBD = (DataTable)Session["proveedorBD"];
                usuariosBD = (DataTable)Session["usuariosBD"];
                tipo_usuario = (DataTable)Session["tipo_usuario"];
                pedidos_fabrica = new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]);

                if (Session["sucursalesBD"] == null)
                {
                    sucursalesBD = pedidos_fabrica.get_sucursales();

                    Session.Add("sucursalesBD", sucursalesBD);
                }
                else
                {
                    sucursalesBD = (DataTable)Session["sucursalesBD"];
                }

                pedidos_no_cargados = pedidos_fabrica.get_pedidos_no_cargados_nuevo();
                cargar_sucursales();



            }
            else
            {
                Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);
            }
        }

        protected void gridView_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("id_sucursal", gridView_sucursales.SelectedRow.Cells[0].Text);
            Session.Add("nombre_sucursal", gridView_sucursales.SelectedRow.Cells[1].Text);
            Response.Redirect("~/paginasFabrica/pedidos.aspx", false);


        }
        protected void boton_resumen_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasFabrica/resumen_de_pedido.aspx", false);



        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_sucursales();

        }
    }
}