using _02___sistemas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class historial_pedido : System.Web.UI.Page
    {
        #region funciones
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_proveedores();
            cargar_mes();
            cargar_año();
        }
        private void cargar_proveedores()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            lista_proveedores = (DataTable)Session["lista_proveedores"];

            for (int fila = 0; fila <= lista_proveedores.Rows.Count - 1; fila++)
            {
                if (lista_proveedores.Rows[fila]["activa"].ToString() == "1")
                {
                    item = new System.Web.UI.WebControls.ListItem(lista_proveedores.Rows[fila]["nombre_proveedor"].ToString(), num_item.ToString());

                    dropDown_proveedores.Items.Add(item);


                    num_item++;
                }
            }
        }
        private void cargar_mes()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                dropDown_mes.Items.Add(item);
                num_item++;
            }
            dropDown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                dropDown_año.Items.Add(item);
                num_item++;
            }
            string añ = DateTime.Now.Year.ToString();

            dropDown_año.SelectedIndex = dropDown_año.Items.Count - 1;
        }
        private void crear_tabla_pedidos()
        {
            pedidos = new DataTable();

            pedidos.Columns.Add("id", typeof(string));
            pedidos.Columns.Add("proveedor", typeof(string));
            pedidos.Columns.Add("num_pedido", typeof(string));
            pedidos.Columns.Add("fecha", typeof(string));
            pedidos.Columns.Add("estado", typeof(string));

        }
        private void llenar_tabla_pedido()
        {
            crear_tabla_pedidos();
            int fila_tabla = 0;
            string proveedor, proveedor_seleccionado, fecha;
            proveedor_seleccionado = dropDown_proveedores.SelectedItem.Text;
            pedidosBD.DefaultView.Sort = "num_pedido DESC";
            pedidosBD = pedidosBD.DefaultView.ToTable();
            for (int fila = 0; fila <= pedidosBD.Rows.Count - 1; fila++)
            {
                proveedor = pedidosBD.Rows[fila]["proveedor"].ToString();
                fecha = pedidosBD.Rows[fila]["fecha"].ToString();
                if (sucursal == pedidosBD.Rows[fila]["sucursal"].ToString() && sistema_Administracion.verificar_proveedor(proveedor, proveedor_seleccionado, (DataTable)Session["lista_proveedores"]) && sistema_Administracion.verificar_fecha(fecha, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    pedidos.Rows.Add();
                    pedidos.Rows[fila_tabla]["id"] = pedidosBD.Rows[fila]["id"].ToString();
                    pedidos.Rows[fila_tabla]["proveedor"] = dropDown_proveedores.SelectedItem.Text;
                    pedidos.Rows[fila_tabla]["num_pedido"] = pedidosBD.Rows[fila]["num_pedido"].ToString();
                    pedidos.Rows[fila_tabla]["fecha"] = pedidosBD.Rows[fila]["fecha"].ToString();
                    if (pedidosBD.Rows[fila]["estado"].ToString() == "local")
                    {
                        pedidos.Rows[fila_tabla]["estado"] = "pedido recibido";
                    }
                    else
                    {
                        pedidos.Rows[fila_tabla]["estado"] = pedidosBD.Rows[fila]["estado"].ToString();
                    }
                    fila_tabla++;


                }
            }
        }
        private void cargar_pedidos()
        {
            llenar_tabla_pedido();

            

            gridView_pedidos.DataSource = pedidos;
            gridView_pedidos.DataBind();
        }
        #endregion

        //#########################################################################################################
        #region atributos
        cls_sistema_cuentas_por_pagar sistema_Administracion;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucusalBD;
        DataTable lista_proveedores;
        DataTable pedidosBD;
        DataTable pedidos;
        int seguridad;
        string id_pedido;
        string sucursal;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            seguridad = int.Parse(Session["nivel_seguridad"].ToString());
            if (seguridad > 2)
            {


            }
            else if (seguridad == 2)
            {

            }

            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                usuariosBD = (DataTable)Session["usuariosBD"];
                sucusalBD = (DataTable)Session["sucursal"];


                sistema_Administracion = new cls_sistema_cuentas_por_pagar(usuariosBD, sucusalBD);

                if (!IsPostBack)
                {
                    Session.Add("lista_proveedores",sistema_Administracion.get_lista_proveedores());
                    //calcular remitos nuevos
                    sistema_Administracion.actualizar_remitos();
                    configurar_controles();
                }


                sucursal = sucusalBD.Rows[0]["sucursal"].ToString();

                pedidosBD = sistema_Administracion.get_pedidos();
                cargar_pedidos();
               
            }
         
            


        }

        protected void dropDown_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedidos();

          
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedidos();

           
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedidos();

           
        }

        protected void gridView_pedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_pedidos.Rows.Count - 1; fila++)
            {
                if ("pedido recibido" != gridView_pedidos.Rows[fila].Cells[6].Text)
                {
                    gridView_pedidos.Rows[fila].Cells[5].Controls[0].Visible = false;

                }
                else
                {
                    gridView_pedidos.Rows[fila].Cells[5].Controls[0].Visible = true;
                }

                if ("Listo para despachar" != gridView_pedidos.Rows[fila].Cells[6].Text)
                {
                    gridView_pedidos.Rows[fila].Cells[7].Controls[0].Visible = false;

                }
                else
                {
                    gridView_pedidos.Rows[fila].Cells[7].Controls[0].Visible = true;
                }
            }
          
           

        }

        protected void gridView_pedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "crear_pdf")
            {
                string gridview_pedidos_index = e.CommandArgument.ToString();
                id_pedido = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[2].Text;
                DateTime hora = DateTime.Now;
                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_remito = Session["sucursal"].ToString() + " pedido-" + gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[1].Text + "- id-" + dato_hora + ".pdf";
                string ruta = "/paginas/pdf/" + id_remito;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                sistema_Administracion.crear_pdf_resumen_pedido_operativo(ruta_archivo, gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[2].Text, sucursal, dropDown_proveedores.SelectedItem.Text, imgdata);
                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginas/pdf/" + id_remito;
                try
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

                }
                catch (Exception)
                {

                    Response.Redirect(strUrl, false);
                }
            }
            else if (e.CommandName == "cancelar_pedido")
            {
                //DAR DE BAJA LOGICA EN BASE DE DATOS
                string gridview_pedidos_index = e.CommandArgument.ToString();

                string estado = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[6].Text;
                if ("cargado" != estado)
                {
                    string id_pedido_seleccionado = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text;
                    string num_pedido_seleccionado = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[2].Text;
                    Response.Redirect(sistema_Administracion.cancelar_pedido(id_pedido_seleccionado, num_pedido_seleccionado, dropDown_proveedores.SelectedItem.Text, (DataTable)Session["usuariosBD"], (DataTable)Session["sucursal"]), false);
                }
            }
            else if (e.CommandName == "iniciar_reclamo")
            {
                //ENVIAR WHATSAPP DE RECLAMO
                /*string gridview_pedidos_index = e.CommandArgument.ToString();
                string num_pedido_seleccionado = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[2].Text;
                string id_pedido = gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text;
                Session.Add("id_pedido",id_pedido);
                Session.Add("num_pedido_seleccionado", num_pedido_seleccionado);
                Session.Add("proveedor_seleccionado",sistema_Administracion.get_nombre_proveedor(dropDown_proveedores.SelectedItem.Text));
                Response.Redirect("~/paginas/confirmar_recepcion.aspx",false);*/
            }
          
           
        }


    }
}