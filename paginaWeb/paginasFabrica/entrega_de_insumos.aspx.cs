using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class entrega_de_insumos : System.Web.UI.Page
    {
        #region tabla resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("Producto", typeof(string));
            resumen.Columns.Add("presentaciones", typeof(List<string>));
            resumen.Columns.Add("entrega", typeof(string));
            resumen.Columns.Add("cantidad_entrega", typeof(string));
            resumen.Columns.Add("presentacion", typeof(string));
            resumen.Columns.Add("index", typeof(string));
            resumen.Columns.Add("dato", typeof(string)); 
            resumen.Columns.Add("tipo_producto", typeof(string)); 

            Session.Add("resumen_entrega_insumos", resumen);
        }
        private void cargar_insumo_en_resumen(string id_insumo, string nombre_insumo)
        {
            int fila_insumo = funciones.buscar_fila_por_id_nombre(id_insumo, nombre_insumo, insumos_fabrica);
            resumen = (DataTable)Session["resumen_entrega_insumos"];
            resumen.Rows.Add();
            int ultima_fila = resumen.Rows.Count - 1;
            resumen.Rows[ultima_fila]["id"] = id_insumo;
            resumen.Rows[ultima_fila]["Producto"] = nombre_insumo; 
            resumen.Rows[ultima_fila]["tipo_producto"] = insumos_fabrica.Rows[fila_insumo]["tipo_producto"].ToString(); 
            if ("19-Sub Productos" == insumos_fabrica.Rows[fila_insumo]["tipo_producto"].ToString())
            {
                List<string> lista = new List<string>();
                lista.Add(insumos_fabrica.Rows[fila_insumo]["unidad_medida"].ToString());
                resumen.Rows[ultima_fila]["presentaciones"] =lista;
            }
            else
            {
                resumen.Rows[ultima_fila]["presentaciones"] = entrega_insumos.get_presentaciones_de_insumo(id_insumo);
            }
            resumen.Rows[ultima_fila]["entrega"] = "N/A";
            resumen.Rows[ultima_fila]["cantidad_entrega"] = "N/A";
            resumen.Rows[ultima_fila]["presentacion"] = "N/A";
            resumen.Rows[ultima_fila]["index"] = "N/A";
            resumen.Rows[ultima_fila]["dato"] = insumos_fabrica.Rows[fila_insumo]["dato"].ToString();
            Session.Add("resumen_entrega_insumos", resumen);
        }
        #endregion

        #region carga de insumos
        private void cargar_insumos()
        {
            gridview_insumos.DataSource = insumos_fabrica;
            gridview_insumos.DataBind();
        }

        private void cargar_insumos_seleccionados()
        {
            gridview_insumos_del_proveedor.DataSource = (DataTable)Session["resumen_entrega_insumos"];
            gridview_insumos_del_proveedor.DataBind();
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_entrega_de_insumos entrega_insumos;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;

        DataTable insumos_fabrica;
        DataTable resumen;
        string id_pedido_insumo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            id_pedido_insumo_seleccionado = Session["id_pedido_insumo_seleccionado"].ToString();
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            if (Session["entrega_insumos"] == null)
            {
                Session.Add("entrega_insumos", new cls_entrega_de_insumos(usuariosBD));
            }
            entrega_insumos = (cls_entrega_de_insumos)Session["entrega_insumos"];
            insumos_fabrica = entrega_insumos.get_insumos_fabrica(id_pedido_insumo_seleccionado);
            if (!IsPostBack)
            {
                crear_tabla_resumen();
                cargar_insumos();
                cargar_insumos_seleccionados();
            }

        }

        protected void boton_guardar_Click(object sender, EventArgs e)
        {
            entrega_insumos.entregar_insumos((DataTable)Session["resumen_entrega_insumos"], Session["id_pedido_insumo_seleccionado"].ToString(), tipo_usuario.Rows[0]["rol"].ToString());
            Response.Redirect("/paginasFabrica/historial_pedido_de_insumos.aspx", false);
        }

        protected void gridview_insumos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gridview_insumos.SelectedRow.Cells[0].Text;
            string nombre = gridview_insumos.SelectedRow.Cells[1].Text;
            cargar_insumo_en_resumen(id, nombre);
            cargar_insumos_seleccionados();
        }

        #region lista resumen
        protected void gridview_insumos_del_proveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            resumen = (DataTable)Session["resumen_entrega_insumos"];
            string paquete, unidad, tipo_unidad, dato;
            List<string> lista;
            int fila_tabla = 0;
            for (int fila = 0; fila <= gridview_insumos_del_proveedor.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, resumen);
                DropDownList dropdown_tipo_presentacion = (gridview_insumos_del_proveedor.Rows[fila].Cells[3].FindControl("dropdown_tipo_presentacion") as DropDownList);
                dropdown_tipo_presentacion.Items.Clear();
                lista = (List<string>)resumen.Rows[fila_tabla]["presentaciones"];
                for (int fila_lista = 0; fila_lista <= lista.Count - 1; fila_lista++)
                {
                    paquete = funciones.obtener_dato(lista[fila_lista], 1);
                    unidad = funciones.obtener_dato(lista[fila_lista], 2);
                    tipo_unidad = funciones.obtener_dato(lista[fila_lista], 3); 
                    if ("19-Sub Productos"== resumen.Rows[fila_tabla]["tipo_producto"].ToString())
                    {
                        dato = fila_lista + "-" + "Unidad" + " " + "1" + " " + tipo_unidad;
                        dropdown_tipo_presentacion.Items.Add(dato);
                    }
                    else
                    {
                        dato = fila_lista + "-" + paquete + " " + unidad + " " + tipo_unidad;
                        dropdown_tipo_presentacion.Items.Add(dato);
                    }
                }
            }
        }

        protected void gridview_insumos_del_proveedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_aplicar_cambios")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                resumen = (DataTable)Session["resumen_entrega_insumos"];
                int fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, resumen);

                DropDownList dropdown_tipo_presentacion = (gridview_insumos_del_proveedor.Rows[fila].Cells[3].FindControl("dropdown_tipo_presentacion") as DropDownList);
                TextBox textbox_cantidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[2].FindControl("textbox_cantidad") as TextBox);

                int index = int.Parse(funciones.obtener_dato(dropdown_tipo_presentacion.SelectedItem.Text, 1));
                string paquete, unidad, tipo_unidad, dato;
                List<string> lista = (List<string>)resumen.Rows[fila_tabla]["presentaciones"];
                
                paquete = funciones.obtener_dato(lista[index], 1);
                unidad = funciones.obtener_dato(lista[index], 2);
                tipo_unidad = funciones.obtener_dato(lista[index], 3);

                double cantidad;
                if (double.TryParse(textbox_cantidad.Text, out cantidad))
                {
                    resumen.Rows[fila_tabla]["entrega"] = cantidad + " " + paquete + " " + unidad + " " + tipo_unidad;
                    resumen.Rows[fila_tabla]["cantidad_entrega"] = cantidad;
                    resumen.Rows[fila_tabla]["presentacion"] = lista[index].ToString();
                    string dat = lista[index].ToString();
                    //index++;
                    resumen.Rows[fila_tabla]["index"] = index.ToString();
                }
                else
                {
                    resumen.Rows[fila_tabla]["entrega"] = "N/A";
                    resumen.Rows[fila_tabla]["cantidad_entrega"] = "N/A";
                    resumen.Rows[fila_tabla]["presentacion"] = "N/A";
                    resumen.Rows[fila_tabla]["index"] = "N/A";
                }
                Session.Add("resumen_entrega_insumos", resumen);
                cargar_insumos_seleccionados();
            }
            else if (e.CommandName == "boton_eliminar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                resumen = (DataTable)Session["resumen_entrega_insumos"];
                int fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, resumen);
                resumen.Rows[fila_tabla].Delete();
                Session.Add("resumen_entrega_insumos", resumen);
                cargar_insumos_seleccionados();
            }
        }

        protected void dropdown_tipo_presentacion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

    }
}