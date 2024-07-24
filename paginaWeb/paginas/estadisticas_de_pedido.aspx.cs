using _02___sistemas;
using _03___sistemas_fabrica;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class estadisticas_de_pedido : System.Web.UI.Page
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
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            int fila_resumen;
            estadisticas_de_pedidos = (DataTable)Session["estadisticas_de_pedidos"];
            for (int fila = 0; fila <= estadisticas_de_pedidos.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_tipo_producto(estadisticas_de_pedidos.Rows[fila]["tipo_producto"].ToString(),dropDown_tipo.SelectedItem.Text))
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = estadisticas_de_pedidos.Rows[fila]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = estadisticas_de_pedidos.Rows[fila]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = estadisticas_de_pedidos.Rows[fila]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = estadisticas_de_pedidos.Rows[fila]["cantidad_pedida"].ToString();
                    resumen.Rows[fila_resumen]["presentacion"] = estadisticas_de_pedidos.Rows[fila]["presentacion"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = estadisticas_de_pedidos.Rows[fila]["proveedor"].ToString();
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
        _02___sistemas.cls_estadisticas_de_entrega estadisticas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;
        DataTable estadisticas_de_pedidos;
        DataTable resumen;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = funciones.Convertir_JArray_a_DataTable((JArray)Session["usuariosBD"]);
            sucursal = funciones.Convertir_JArray_a_DataTable((JArray)Session["sucursal"]);

            if (Session["estadisticas"] == null)
            {
                base.Session.Add("estadisticas", new _02___sistemas.cls_estadisticas_de_entrega(usuariosBD));
            }
            estadisticas = (_02___sistemas.cls_estadisticas_de_entrega)base.Session["estadisticas"];

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
                estadisticas_de_pedidos = estadisticas.obtener_estadisticas_de_pedido(sucursal.Rows[0]["sucursal"].ToString(), Session["fecha_estadistica_inicio"].ToString(), Session["fecha_estadistica_fin"].ToString());
                Session.Add("estadisticas_de_pedidos", estadisticas_de_pedidos);
                llenar_dropDownList(estadisticas_de_pedidos);
                cargar_resumen();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_resumen();
        }
    }
}