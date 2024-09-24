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
    public partial class recepcion_de_despacho_fatay : System.Web.UI.Page
    {
        private void cargar_cantidad(string cantidad_dato)
        {
            detalle_produccion = (DataTable)Session["detalle_produccion"];

            id_producto = gridview_produccion.SelectedRow.Cells[0].Text;

            int fila_producto = funciones.buscar_fila_por_id(id_producto, (DataTable)Session["detalle_produccion"]);
            double cantidad, stock, stock_total, cantidad_entrega;

            if (double.TryParse(cantidad_dato, out cantidad))
            {
                cantidad_entrega = double.Parse(detalle_produccion.Rows[fila_producto]["Cant.Entregada"].ToString());
                stock = double.Parse(detalle_produccion.Rows[fila_producto]["stock"].ToString());
                stock_total = stock;



                stock_total = stock_total + cantidad;

                if (cantidad == 0)
                {
                    detalle_produccion.Rows[fila_producto]["nuevo_stock"] = "N/A";
                    detalle_produccion.Rows[fila_producto]["Cant.Recibida"] = "N/A";

                }
                else
                {
                    detalle_produccion.Rows[fila_producto]["nuevo_stock"] = stock_total.ToString();
                    detalle_produccion.Rows[fila_producto]["Cant.Recibida"] = cantidad.ToString();

                }


                Session.Add("detalle_produccion", detalle_produccion);




            }

            cargar_detalle_produccion();



        }

        private int buscar_fila_producto()
        {
            detalle_produccion = (DataTable)Session["detalle_produccion"];

            int retorno = 0;
            int fila = 0;
            while (fila <= detalle_produccion.Rows.Count - 1)
            {
                if (id_producto == detalle_produccion.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        private void cargar_detalle_produccion()
        {
            detalle_produccion = (DataTable)Session["detalle_produccion"];
            gridview_produccion.DataSource = detalle_produccion;
            gridview_produccion.DataBind();
        }
        ///////////////////////////////////////////////////////////////////////////
        cls_recepcion_de_fatay recepcion;
        cls_funciones funciones = new cls_funciones();
        DataTable sucursalesBD;
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuarioBD;

        DataTable historial_produccionBD;
        DataTable detalle_produccion;
        string id_historial, id_producto;
        int seguridad;
        protected void Page_Load(object sender, EventArgs e)
        {

            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuarioBD = (DataTable)Session["tipo_usuario"];
            if (Session["recepcion"] == null)
            {
                Session.Add("recepcion", new cls_recepcion_de_fatay(usuariosBD));
            }
            recepcion = (cls_recepcion_de_fatay)Session["recepcion"];
            /*if ("Shami Villa Maipu Produccion" == tipo_usuarioBD.Rows[0]["rol"].ToString())
            {
                historial_produccionBD = recepcion.get_historial_produccion_proveedor_cliente(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), tipo_usuarioBD.Rows[0]["rol"].ToString(), "Shami Villa Maipu Expedicion");
            }
            else if ("Shami Villa Maipu Expedicion" == tipo_usuarioBD.Rows[0]["rol"].ToString())
            {
                historial_produccionBD = recepcion.get_todo_historial_de_produccion(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            }*/
            historial_produccionBD = recepcion.get_todo_historial_de_produccion(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            id_historial = Session["id_historial"].ToString();
            if (!IsPostBack)
            {
                detalle_produccion = recepcion.get_detalle_produccion(id_historial, proveedorBD.Rows[0]["nombre_en_BD"].ToString());
                Session.Add("detalle_produccion", detalle_produccion);
                cargar_detalle_produccion();

            }
        }





        protected void gridview_produccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_producto = gridview_produccion.SelectedRow.Cells[0].Text;
            TextBox txtValor_cantidad = (gridview_produccion.SelectedRow.Cells[5].FindControl("textbox_cantidad") as TextBox);
            int fila = buscar_fila_producto();
            cargar_cantidad(txtValor_cantidad.Text.Replace(",", "."));
        }

        protected void gridview_produccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila < gridview_produccion.Rows.Count; fila++)
            {
                if ("N/A" != gridview_produccion.Rows[fila].Cells[7].Text)
                {
                    gridview_produccion.Rows[fila].Cells[5].Controls[0].Visible = false;
                    gridview_produccion.Rows[fila].Cells[6].Controls[0].Visible = false;
                }
            }
        }

        protected void boton_enviar_Click1(object sender, EventArgs e)
        {
            recepcion.confirmar_produccion(Session["id_historial"].ToString(), (DataTable)Session["detalle_produccion"], recepcion.get_proveedor_de_producccion(id_historial), tipo_usuarioBD.Rows[0]["rol"].ToString());
            Response.Redirect("/paginasFabrica/recepcion_de_fabrica_fatay.aspx", false);
        }
    }
}