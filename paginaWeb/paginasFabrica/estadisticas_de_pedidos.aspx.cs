using _02___sistemas;
using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class estadisticas_de_pedidos : System.Web.UI.Page
    {
        #region cargar tabla sucursales
        private void eliminar_sucursal_en_resumen()
        {

            int fila = funciones.buscar_fila_por_id(id_sucursal, resumen_sucursales);
            resumen_sucursales.Rows[fila].Delete();
        }
        private void crear_tabla_resumen_sucursales()
        {
            resumen_sucursales = new DataTable();
            resumen_sucursales.Columns.Add("id", typeof(string));
            resumen_sucursales.Columns.Add("sucursal", typeof(string));
        }
        private void cargar_sucursales()
        {
            gridView_sucursales.DataSource = sucursales;
            gridView_sucursales.DataBind();

            gridview_resumen.DataSource = resumen_sucursales;
            gridview_resumen.DataBind();
        }
        private void cargar_sucursal_en_resumen()
        {
            if (!verificar_si_cargo())
            {
                resumen_sucursales.Rows.Add();
                int fila = resumen_sucursales.Rows.Count - 1;
                resumen_sucursales.Rows[fila]["id"] = id_sucursal;
                resumen_sucursales.Rows[fila]["sucursal"] = sucursal_seleccionada;
                Session.Add("resumen_sucursal", resumen_sucursales);
            }

        }
        private void cargar_franquicia_en_resumen()
        {
            resumen_sucursales.Rows.Clear();
            for (int fila_sucursal = 0; fila_sucursal <= sucursales.Rows.Count - 1; fila_sucursal++)
            {
                if (sucursales.Rows[fila_sucursal]["franquicia"].ToString() == dropDown_franquicia.SelectedItem.Text)
                {
                    id_sucursal = sucursales.Rows[fila_sucursal]["id"].ToString();
                    sucursal_seleccionada = sucursales.Rows[fila_sucursal]["sucursal"].ToString();
                    if (!verificar_si_cargo())
                    {
                        resumen_sucursales.Rows.Add();
                        int fila = resumen_sucursales.Rows.Count - 1;
                        resumen_sucursales.Rows[fila]["id"] = id_sucursal;
                        resumen_sucursales.Rows[fila]["sucursal"] = sucursal_seleccionada;
                        Session.Add("resumen_sucursal", resumen_sucursales);
                    }
                }
            }
        }
        private bool verificar_si_cargo()
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= resumen_sucursales.Rows.Count - 1)
            {
                if (id_sucursal == resumen_sucursales.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion
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
            resumen.Columns.Add("venta_teorica", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            double venta_teorica = 0;
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
                    resumen.Rows[fila_resumen]["venta_teorica"] = funciones.formatCurrency(double.Parse(estadisticas_de_pedidos_seleccionados.Rows[fila]["venta_teorica"].ToString()));
                    venta_teorica = venta_teorica + double.Parse(estadisticas_de_pedidos_seleccionados.Rows[fila]["venta_teorica"].ToString());
                }
            }
            label_total.Text = "Total: " + funciones.formatCurrency(venta_teorica);
        }
        private void llenar_tabla_resumen_buscar()
        {
            crear_tabla_resumen();
            int fila_resumen;
            double venta_teorica = 0;
            estadisticas_de_pedidos_seleccionados = (DataTable)Session["estadisticas_de_pedidos"];
            for (int fila = 0; fila <= estadisticas_de_pedidos_seleccionados.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, estadisticas_de_pedidos_seleccionados.Rows[fila]["producto"].ToString()))
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["cantidad_pedida"].ToString();
                    resumen.Rows[fila_resumen]["presentacion"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["presentacion"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = estadisticas_de_pedidos_seleccionados.Rows[fila]["proveedor"].ToString();
                    resumen.Rows[fila_resumen]["venta_teorica"] = funciones.formatCurrency(double.Parse(estadisticas_de_pedidos_seleccionados.Rows[fila]["venta_teorica"].ToString()));
                    venta_teorica = venta_teorica + double.Parse(estadisticas_de_pedidos_seleccionados.Rows[fila]["venta_teorica"].ToString());
                }
            }
            label_total.Text = "Total: " + funciones.formatCurrency(venta_teorica);
        }
        private void cargar_resumen()
        {
            llenar_tabla_resumen();
            gridView_productos.DataSource = resumen;
            gridView_productos.DataBind();
        }
        private void cargar_resumen_buscar()
        {
            llenar_tabla_resumen_buscar();
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
        private void llenar_dropDownList_franquicia(DataTable dt)
        {
            dropDown_franquicia.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "franquicia";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;
            if (dt.Rows.Count > 0)
            {
                string tipo_seleccionado = dt.Rows[0]["franquicia"].ToString();
                item = new ListItem(dt.Rows[0]["franquicia"].ToString(), num_item.ToString());
                dropDown_franquicia.Items.Add(item);
                num_item = num_item + 1;
                for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
                {
                    if (dropDown_franquicia.Items[num_item - 2].Text != dt.Rows[fila]["franquicia"].ToString())
                    {
                        item = new ListItem(dt.Rows[fila]["franquicia"].ToString(), num_item.ToString());
                        dropDown_franquicia.Items.Add(item);
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
            
            estadisticas = new _02___sistemas.cls_estadisticas_de_entrega(usuariosBD);
            sucursales = estadisticas.get_sucursal();
            if (!IsPostBack)
            {
                crear_tabla_resumen_sucursales();
                llenar_dropDownList_franquicia(sucursales);
                Session.Add("resumen_sucursal", resumen_sucursales);
                Session.Add("fecha_estadistica_inicio", "N/A");
                Session.Add("fecha_estadistica_fin", "N/A");
                label_fecha_inicio.Text = "Seleccione fecha inicio.";
                label_fecha_final.Text = "Seleccione fecha final.";
            }
            resumen_sucursales = (DataTable)Session["resumen_sucursal"];

            cargar_sucursales();
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
                resumen_sucursales = (DataTable)Session["resumen_sucursal"];

                estadisticas_de_pedidos_seleccionados = estadisticas.obtener_estadisticas_de_pedido_segun_sucursales(resumen_sucursales, Session["fecha_estadistica_inicio"].ToString(), Session["fecha_estadistica_fin"].ToString());
                Session.Add("estadisticas_de_pedidos", estadisticas_de_pedidos_seleccionados);
                llenar_dropDownList(estadisticas_de_pedidos_seleccionados);
                cargar_resumen();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_resumen();
        }

        protected void gridView_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_sucursal = gridView_sucursales.SelectedRow.Cells[0].Text;
            sucursal_seleccionada = gridView_sucursales.SelectedRow.Cells[1].Text;
            cargar_sucursal_en_resumen();
            cargar_sucursales();
        }

        protected void boton_carga_Click(object sender, EventArgs e)
        {
            cargar_franquicia_en_resumen();
            cargar_sucursales();
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_resumen_buscar();
        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_sucursal = gridview_resumen.SelectedRow.Cells[0].Text;
            eliminar_sucursal_en_resumen();
            cargar_sucursales();
        }
    }
}