using _02___sistemas;
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
    public partial class historial_orden_de_pedidos : System.Web.UI.Page
    {
        #region cargar resumen pedido
        private void mostrar_ocultar_boton_oc()
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];
            if (resumen_orden_pedido.Rows.Count > 0)
            {
                boton_crear_oc.Visible = true;
            }
            else
            {
                boton_crear_oc.Visible = false;
            }
        }
        private void crear_resumen_pedido()
        {
            resumen_orden_pedido = new DataTable();
            resumen_orden_pedido.Columns.Add("id", typeof(string));
            resumen_orden_pedido.Columns.Add("producto", typeof(string));
            resumen_orden_pedido.Columns.Add("cantidad_pedida", typeof(string));

            Session.Add("resumen_orden_pedido", resumen_orden_pedido);
        }

        private void cargar_producto_en_resumen(string id, string producto, string cantidad_pedida)
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];
            if (!funciones.verificar_si_cargo(id, resumen_orden_pedido))
            {
                resumen_orden_pedido.Rows.Add();
                int ultima_fila = resumen_orden_pedido.Rows.Count - 1;

                resumen_orden_pedido.Rows[ultima_fila]["id"] = id;
                resumen_orden_pedido.Rows[ultima_fila]["producto"] = producto;
                resumen_orden_pedido.Rows[ultima_fila]["cantidad_pedida"] = cantidad_pedida;
                Session.Add("resumen_orden_pedido", resumen_orden_pedido);
            }
            mostrar_ocultar_boton_oc();

        }
        private void eliminar_producto_de_resumen_orden_pedido(string id)
        {
            resumen_orden_pedido = (DataTable)Session["resumen_orden_pedido"];

            int fila_producto = funciones.buscar_fila_por_id(id, resumen_orden_pedido);

            resumen_orden_pedido.Rows[fila_producto].Delete();
            Session.Add("resumen_orden_pedido", resumen_orden_pedido);
            mostrar_ocultar_boton_oc();


        }
        private void cargar_resumen_orden_pedido()
        {
            gridView_resumen_orden_pedido.DataSource = (DataTable)Session["resumen_orden_pedido"];
            gridView_resumen_orden_pedido.DataBind();
        }
        #endregion
        #region cargar resumen
        private void crear_tabla_resumen_de_pedido()
        {
            resumen_de_pedido = new DataTable();
            resumen_de_pedido.Columns.Add("id", typeof(string));
            resumen_de_pedido.Columns.Add("producto", typeof(string));
            resumen_de_pedido.Columns.Add("cantidad_pedida", typeof(string));
            resumen_de_pedido.Columns.Add("estado", typeof(string));
            resumen_de_pedido.Columns.Add("num_orden_compra", typeof(string));
            resumen_de_pedido.Columns.Add("fecha_orden_compra", typeof(string));
            resumen_de_pedido.Columns.Add("id_orden", typeof(string));
            resumen_de_pedido.Columns.Add("dato", typeof(string)); 
            resumen_de_pedido.Columns.Add("columna", typeof(string)); 
        }
        private void llenar_tabla_resumen_de_pedido(string id_pedido)
        {
            crear_tabla_resumen_de_pedido();
            resumen_de_pedidoBD = historial_pedido.get_resumen_de_pedido(id_pedido);

            int index = 0;
            for (int fila = 0; fila <= resumen_de_pedidoBD.Rows.Count-1; fila++)
            {
                resumen_de_pedido.Rows.Add();
                resumen_de_pedido.Rows[index]["id"] = resumen_de_pedidoBD.Rows[fila]["id"].ToString();
                resumen_de_pedido.Rows[index]["producto"] = resumen_de_pedidoBD.Rows[fila]["producto"].ToString(); 
                resumen_de_pedido.Rows[index]["cantidad_pedida"] = resumen_de_pedidoBD.Rows[fila]["cantidad_pedida"].ToString();
                resumen_de_pedido.Rows[index]["estado"] = resumen_de_pedidoBD.Rows[fila]["estado"].ToString(); 
                resumen_de_pedido.Rows[index]["num_orden_compra"] = resumen_de_pedidoBD.Rows[fila]["num_orden_compra"].ToString(); 
                resumen_de_pedido.Rows[index]["fecha_orden_compra"] = resumen_de_pedidoBD.Rows[fila]["fecha_orden_compra"].ToString(); 
                resumen_de_pedido.Rows[index]["id_orden"] = resumen_de_pedidoBD.Rows[fila]["id_orden"].ToString(); 
                resumen_de_pedido.Rows[index]["dato"] = resumen_de_pedidoBD.Rows[fila]["dato"].ToString(); 
                resumen_de_pedido.Rows[index]["columna"] = resumen_de_pedidoBD.Rows[fila]["columna"].ToString(); 

                index++;
            }
            Session.Add("resumen_de_pedido_a_compras", resumen_de_pedido);
        }
        private void cargar_resumen(string id_pedido)
        {
            llenar_tabla_resumen_de_pedido( id_pedido);
            gridView_resumen.DataSource = (DataTable)Session["resumen_de_pedido_a_compras"];
            gridView_resumen.DataBind();
        }
        #endregion
        #region carga de ordenes
        private void crear_tabla_pedido()
        {
            orden_de_pedido = new DataTable();

            orden_de_pedido.Columns.Add("id", typeof(string));
            orden_de_pedido.Columns.Add("solicita", typeof(string));
            orden_de_pedido.Columns.Add("fecha", typeof(string));
            orden_de_pedido.Columns.Add("pendientes", typeof(string));

        }
        private void llenar_tabla_pedido()
        {
            crear_tabla_pedido();
            int fila_pedido = 0;
            for (int fila = 0; fila <= orden_de_pedidoBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(orden_de_pedidoBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text))
                {
                    orden_de_pedido.Rows.Add();

                    orden_de_pedido.Rows[fila_pedido]["id"] = orden_de_pedidoBD.Rows[fila]["id"].ToString();
                    orden_de_pedido.Rows[fila_pedido]["solicita"] = orden_de_pedidoBD.Rows[fila]["solicita"].ToString();
                    orden_de_pedido.Rows[fila_pedido]["fecha"] = orden_de_pedidoBD.Rows[fila]["fecha"].ToString();
                    orden_de_pedido.Rows[fila_pedido]["pendientes"] = obtener_pendientes(fila);

                    fila_pedido++;
                }
            }
        }
        private string obtener_pendientes(int fila)
        {
            int retorno= 0;
            for (int columna = orden_de_pedidoBD.Columns["producto_1"].Ordinal; columna <= orden_de_pedidoBD.Columns.Count-1; columna++)
            {
                if (orden_de_pedidoBD.Rows[fila][columna].ToString()!="N/A")
                {
                    if (funciones.obtener_dato(orden_de_pedidoBD.Rows[fila][columna].ToString(),5)== "No pedido")
                    {
                        retorno++;
                    }
                }
                else
                {
                    break;
                }
            }
            return retorno.ToString();
        }
        private void cargar_pedido()
        {
            llenar_tabla_pedido();
            gridView_pedidos.DataSource = orden_de_pedido;
            gridView_pedidos.DataBind();

            gridView_resumen.DataSource = null;
            gridView_resumen.DataBind();
        }
        #endregion
        #region configurar controles

        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();
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

        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_historial_orden_de_pedidos historial_pedido;

        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;

        string tipo_seleccionado;
        string id_proveedor_fabrica_seleccionado;
        DataTable resumen_orden_pedido;
        DataTable orden_de_pedidoBD;
        DataTable orden_de_pedido;
        DataTable resumen_de_pedidoBD;
        DataTable resumen_de_pedido;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            if (Session["historial_pedido"] == null)
            {
                Session.Add("historial_pedido", new cls_historial_orden_de_pedidos(usuariosBD));
            }
            historial_pedido = (cls_historial_orden_de_pedidos)Session["historial_pedido"];
            orden_de_pedidoBD = historial_pedido.get_orden_de_pedido();
            if (!IsPostBack)
            {
                Session.Remove("id_orden_pedido_seleccionado");
                crear_resumen_pedido();
                configurar_controles();
                cargar_pedido();
            }
            mostrar_ocultar_boton_oc();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedido();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_pedido();
        }

        protected void gridView_pedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "abrir_pedido")
            {
                string gridview_pedidos_index = e.CommandArgument.ToString();
                Session.Add("id_de_pedido_de_compra", gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);
                cargar_resumen(Session["id_de_pedido_de_compra"].ToString());

                Session.Add("id_orden_pedido_seleccionado", gridView_pedidos.Rows[int.Parse(gridview_pedidos_index)].Cells[0].Text);

            }
            else if (e.CommandName == "crear_orden_compra")
            {

            }
        }

        protected void gridView_pedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void gridView_resumen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "seleccionar")
            {         // id producto cantidad_pedida
                string gridView_resumen_index = e.CommandArgument.ToString();

                string id = gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[0].Text;
                string producto = gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[1].Text;
                string cantidad_pedida = gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[2].Text;

                cargar_producto_en_resumen(id, producto, cantidad_pedida);
                cargar_resumen_orden_pedido();
            }
            else if(e.CommandName == "cancelar")
            {
                string gridView_resumen_index = e.CommandArgument.ToString();

                string id = gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[0].Text;
                cancelar_pedido(id);
               /* gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[3].Text = "Cancelado";
                gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[6].Controls[0].Visible = false;
                gridView_resumen.Rows[int.Parse(gridView_resumen_index)].Cells[7].Controls[0].Visible = false;*/
                
                orden_de_pedidoBD = historial_pedido.get_orden_de_pedido();
                cargar_pedido();
                cargar_resumen(Session["id_de_pedido_de_compra"].ToString());

            }
        }

        private void cancelar_pedido(string id_producto)
        {
            resumen_de_pedido = (DataTable)Session["resumen_de_pedido_a_compras"];

            int fila_producto = funciones.buscar_fila_por_id(id_producto,resumen_de_pedido);

            string id = funciones.obtener_dato(resumen_de_pedido.Rows[fila_producto]["dato"].ToString(),1);
            string producto = funciones.obtener_dato(resumen_de_pedido.Rows[fila_producto]["dato"].ToString(),2);
            string cantidad = funciones.obtener_dato(resumen_de_pedido.Rows[fila_producto]["dato"].ToString(),3);
            string unidad = funciones.obtener_dato(resumen_de_pedido.Rows[fila_producto]["dato"].ToString(),4);
            string estado = "Cancelado-N/A-N/A";

            string dato = id +"-"+producto +"-"+cantidad+"-"+unidad+"-"+estado;
            string columna = resumen_de_pedido.Rows[fila_producto]["columna"].ToString();
            string id_orden = resumen_de_pedido.Rows[fila_producto]["id_orden"].ToString();
            historial_pedido.cancelar_pedido(columna, dato, id_orden);
        }

        protected void gridView_resumen_orden_pedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "eliminar")
            {         // id producto cantidad_pedida
                string gridView_resumen_index = e.CommandArgument.ToString();

                string id = gridView_resumen_orden_pedido.Rows[int.Parse(gridView_resumen_index)].Cells[0].Text;

                eliminar_producto_de_resumen_orden_pedido(id);
                cargar_resumen_orden_pedido();
            }
        }




        protected void boton_crear_oc_Click(object sender, EventArgs e)
        {
            Response.Redirect("/paginasFabrica/orden_de_compras.aspx", false);
        }

        protected void gridView_resumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_resumen.Rows.Count - 1; fila++)
            {
               if ("No pedido" != gridView_resumen.Rows[fila].Cells[3].Text ||
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion" ||
                    tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                {
                    gridView_resumen.Rows[fila].Cells[6].Controls[0].Visible = false;
                    gridView_resumen.Rows[fila].Cells[7].Controls[0].Visible = false;

                }
            }
        }
    }
}