using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class actualizador_de_precios : System.Web.UI.Page
    {
        #region funciones privadas

        private void aumentar_porcentaje(string porcentaje_dato)
        {
            insumoBD = (DataTable)Session["insumoBD"];

            double porcenaje, multiplicador, precio_venta_nuevo_por_unidad;
            double precio_compra, x_porciento, precio_venta, precio_venta_actual, porcentaje_Actual, diferencia_actual;
            if (double.TryParse(porcentaje_dato, out porcenaje))
            {
                for (int fila = 0; fila <= insumoBD.Rows.Count - 1; fila++)
                {
                    if (dropDown_tipo.SelectedItem.Text== insumoBD.Rows[fila]["tipo_producto"].ToString())
                    {
                        
                        precio_compra = double.Parse(insumoBD.Rows[fila]["precio_compra"].ToString());
                        precio_venta_actual = double.Parse(insumoBD.Rows[fila]["precio_venta_actual"].ToString());
                        diferencia_actual = precio_venta_actual - precio_compra;
                        porcentaje_Actual = (diferencia_actual * 100) / precio_venta_actual;

                        x_porciento = (porcenaje * precio_compra) / 100;
                        precio_venta = precio_compra + x_porciento;
                        multiplicador = double.Parse(insumoBD.Rows[fila]["multiplicador"].ToString());
                        precio_venta_nuevo_por_unidad = precio_venta / multiplicador;
                        if (porcentaje_Actual <= porcenaje)
                        {
                            insumoBD.Rows[fila]["precio_venta_nuevo"] = precio_venta;
                            insumoBD.Rows[fila]["diferencia"] = x_porciento;
                            insumoBD.Rows[fila]["porcentaje_aumento"] = porcenaje + "%";
                            insumoBD.Rows[fila]["precio_venta_nuevo_por_unidad"] = precio_venta_nuevo_por_unidad;
                        }
                        else
                        {
                            insumoBD.Rows[fila]["precio_venta_nuevo"] = "N/A";
                            insumoBD.Rows[fila]["diferencia"] = "N/A";
                            insumoBD.Rows[fila]["porcentaje_aumento"] = "N/A";
                            insumoBD.Rows[fila]["precio_venta_nuevo_por_unidad"] = "N/A";
                        }
                    }
                }
                Session.Add("insumoBD", insumoBD);
            }
        }
        #endregion
        #region carga de insumos 
        private void crear_tabla_insumos()
        {
            insumo = new DataTable();
            insumo.Columns.Add("id", typeof(string));
            insumo.Columns.Add("producto", typeof(string));
            insumo.Columns.Add("unidad_medida", typeof(string));
            insumo.Columns.Add("precio_compra", typeof(string));
            insumo.Columns.Add("porcentaje_ganancia_actual", typeof(string));
            insumo.Columns.Add("precio_venta_actual", typeof(string));
            insumo.Columns.Add("diferencia", typeof(string));
            insumo.Columns.Add("porcentaje_aumento", typeof(string));
            insumo.Columns.Add("precio_venta_nuevo", typeof(string));
            insumo.Columns.Add("precio_venta_personalizado", typeof(string));
            insumo.Columns.Add("presentacion", typeof(string));
        }
        private void llenar_tabla_insumo()
        {
            crear_tabla_insumos();
            insumoBD = (DataTable)Session["insumoBD"];
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumoBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, insumoBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(dropDown_tipo.SelectedItem.Text, insumoBD.Rows[fila]["tipo_producto"].ToString()))
                {
                    insumo.Rows.Add();
                    insumo.Rows[fila_insumo]["id"] = insumoBD.Rows[fila]["id"].ToString();
                    insumo.Rows[fila_insumo]["producto"] = insumoBD.Rows[fila]["producto"].ToString();
                    insumo.Rows[fila_insumo]["unidad_medida"] = insumoBD.Rows[fila]["unidad_medida"].ToString();
                    insumo.Rows[fila_insumo]["precio_compra"] = funciones.formatCurrency(double.Parse(insumoBD.Rows[fila]["precio_compra"].ToString()));
                    insumo.Rows[fila_insumo]["precio_venta_actual"] = funciones.formatCurrency(double.Parse(insumoBD.Rows[fila]["precio_venta_actual"].ToString()));
                    insumo.Rows[fila_insumo]["porcentaje_ganancia_actual"] = insumoBD.Rows[fila]["porcentaje_ganancia_actual"].ToString();
                    insumo.Rows[fila_insumo]["presentacion"] = insumoBD.Rows[fila]["presentacion"].ToString();
                    if (insumoBD.Rows[fila]["precio_venta_nuevo"].ToString() != "N/A")
                    {
                        insumo.Rows[fila_insumo]["precio_venta_nuevo"] = funciones.formatCurrency(double.Parse(insumoBD.Rows[fila]["precio_venta_nuevo"].ToString()));
                    }
                    else
                    {
                        insumo.Rows[fila_insumo]["precio_venta_nuevo"] = insumoBD.Rows[fila]["precio_venta_nuevo"].ToString();
                    }
                    if (insumoBD.Rows[fila]["diferencia"].ToString() != "N/A")
                    {
                        insumo.Rows[fila_insumo]["diferencia"] = funciones.formatCurrency(double.Parse(insumoBD.Rows[fila]["diferencia"].ToString()));
                    }
                    else
                    {
                        insumo.Rows[fila_insumo]["diferencia"] = insumoBD.Rows[fila]["diferencia"].ToString();
                    }
                    if (insumoBD.Rows[fila]["precio_venta_personalizado"].ToString() != "N/A")
                    {
                        insumo.Rows[fila_insumo]["precio_venta_personalizado"] = funciones.formatCurrency(double.Parse(insumoBD.Rows[fila]["precio_venta_personalizado"].ToString()));
                    }
                    else
                    {
                        insumo.Rows[fila_insumo]["precio_venta_personalizado"] = insumoBD.Rows[fila]["precio_venta_personalizado"].ToString();
                    }
                    insumo.Rows[fila_insumo]["porcentaje_aumento"] = insumoBD.Rows[fila]["porcentaje_aumento"].ToString();

                    fila_insumo++;
                }
            }
        }
        private void cargar_insumos()
        {
            llenar_tabla_insumo();
            gridview_productos.DataSource = insumo;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            insumoBD = (DataTable)Session["insumoBD"];
            llenar_dropDownList(insumoBD);
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
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_actualizador_de_precios_insumos actualizador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable insumoBD;
        DataTable insumo;
        string tipo_seleccionado;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["actualizador"] == null)
            {
                Session.Add("actualizador", new cls_actualizador_de_precios_insumos(usuariosBD));
            }
            actualizador = (cls_actualizador_de_precios_insumos)Session["actualizador"];
            if (!IsPostBack)
            {
                actualizador.actualizar_precios_fabrica_fatay();
                insumoBD = actualizador.get_insumos();
                Session.Add("insumoBD", insumoBD);
                configurar_controles();
                cargar_insumos();
            }
        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            actualizador.actualizar((DataTable)Session["insumoBD"]);
            Response.Redirect("~/paginasFabrica/sucursales.aspx", false);
        }

        protected void boton_aumentar_porcentaje_Click(object sender, EventArgs e)
        {
            aumentar_porcentaje(textbox_porcentaje_aumento.Text);
            cargar_insumos();

        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }

        protected void dropdown_tipo_paquete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*            insumoBD = (DataTable)Session["insumoBD"];
                        string id_insumo;
                        int fila_insumo;
                        for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
                        {
                            id_insumo = gridview_productos.Rows[fila].Cells[0].Text;
                            fila_insumo = funciones.buscar_fila_por_id(id_insumo,insumoBD);
                            DropDownList dropdown_tipo_paquete = (gridview_productos.Rows[fila].Cells[7].FindControl("dropdown_tipo_paquete") as DropDownList);
                            dropdown_tipo_paquete.SelectedValue = insumoBD.Rows[fila_insumo]["tipo_paquete"].ToString();

                        }*/
        }

        protected void texbox_precio_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int fila = row.RowIndex;

            insumoBD = (DataTable)Session["insumoBD"];
            int fila_tabla = funciones.buscar_fila_por_id(gridview_productos.Rows[fila].Cells[0].Text, insumoBD);

            TextBox texbox_precio = (gridview_productos.Rows[fila].Cells[7].FindControl("texbox_precio") as TextBox);
            double cantidad, multiplicador;
            if (double.TryParse(texbox_precio.Text, out cantidad))
            {
                insumoBD.Rows[fila_tabla]["precio_venta_personalizado"] = cantidad.ToString();

                multiplicador = double.Parse(insumoBD.Rows[fila_tabla]["multiplicador"].ToString());
                cantidad = cantidad / multiplicador;
                insumoBD.Rows[fila_tabla]["precio_venta_personalizado_por_unidad"] = cantidad.ToString();
            }
            else
            {
                insumoBD.Rows[fila_tabla]["precio_venta_personalizado"] = "N/A";
                insumoBD.Rows[fila_tabla]["precio_venta_personalizado_por_unidad"] = "N/A";
            }
            cargar_insumos();
        }
    }
}