using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabricaFatay
{
    public partial class analisis_de_produccion_fabrica_fatay : System.Web.UI.Page
    {
        #region cargar tabla
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("cantidad_pedida", typeof(string));
            resumen.Columns.Add("presentacion", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("incremento", typeof(string));
            resumen.Columns.Add("objetivo", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            int fila_resumen;
            estadisticas_de_pedidos_seleccionados = (DataTable)Session["estadisticas_de_pedidos"];
            for (int fila = 0; fila <= estadisticas_de_pedidos_seleccionados.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_tipo_producto(estadisticas_de_pedidos_seleccionados.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["cantidad_pedida"].ToString();
                    resumen.Rows[fila_resumen]["presentacion"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["presentacion"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["proveedor"].ToString();
                    resumen.Rows[fila_resumen]["stock"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["stock"].ToString();
                    resumen.Rows[fila_resumen]["incremento"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["incremento"].ToString();
                    resumen.Rows[fila_resumen]["objetivo"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["objetivo"].ToString();
                }
            }
        }
        private void cargar_resumen()
        {
            llenar_tabla_resumen();
            gridView_productos.DataSource = resumen;
            gridView_productos.DataBind();


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
            if (dt.Rows.Count > 0)
            {
                string tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
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

        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_estadisticas_de_entrega estadisticas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;
        DataTable estadisticas_de_pedidos_seleccionados;
        DataTable resumen;
        DataTable sucursales;
        DataTable resumen_sucursales;
        string id_sucursal, sucursal_seleccionada;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            
            estadisticas = new cls_estadisticas_de_entrega(usuariosBD);
            if (!IsPostBack)
            {
                Session.Add("fecha_estadistica_inicio", "N/A");
                Session.Add("fecha_estadistica_fin", "N/A");
                label_fecha_inicio.Text = "Seleccione fecha inicio.";
                label_fecha_final.Text = "Seleccione fecha final.";
            }

        }
        protected void calendario_rango_inicial_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha_inicio.Text = "Fecha inicio: " + calendario_rango_inicial.SelectedDate.ToString("dd/MM/yyyy");
            Session.Add("fecha_estadistica_inicio", calendario_rango_inicial.SelectedDate.ToString("yyyy-MM-dd"));
        }

        protected void calendario_rango_final_SelectionChanged(object sender, EventArgs e)
        {
            label_fecha_final.Text = "Fecha fin: " + calendario_rango_final.SelectedDate.ToString("dd/MM/yyyy");
            Session.Add("fecha_estadistica_fin", calendario_rango_final.SelectedDate.ToString("yyyy-MM-dd"));
        }

        protected void boton_calcular_Click(object sender, EventArgs e)
        {
            if (Session["fecha_estadistica_inicio"].ToString() != "N/A" &&
                Session["fecha_estadistica_fin"].ToString() != "N/A")
            {

                estadisticas_de_pedidos_seleccionados = estadisticas.get_analisis_produccion_fabrica_fatay(Session["fecha_estadistica_inicio"].ToString(), Session["fecha_estadistica_fin"].ToString());
                Session.Add("estadisticas_de_pedidos", estadisticas_de_pedidos_seleccionados);
                llenar_dropDownList(estadisticas_de_pedidos_seleccionados);
                cargar_resumen();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_resumen();
        }

    }
}