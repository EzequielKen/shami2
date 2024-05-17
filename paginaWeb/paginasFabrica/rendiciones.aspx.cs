using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class rendiciones : System.Web.UI.Page
    {
        #region carga
        private int buscar_fila_acuerdo(string acuerdo, string tipo_acuerdo)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdos_de_precio.Rows.Count - 1)
            {
                if (acuerdo == acuerdos_de_precio.Rows[fila]["acuerdo"].ToString() &&
                    tipo_acuerdo == acuerdos_de_precio.Rows[fila]["tipo_de_acuerdo"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void crear_tabla_rendiciones()
        {
            rendiciones_usuario = new DataTable();
            rendiciones_usuario.Columns.Add("id", typeof(string));
            rendiciones_usuario.Columns.Add("id_producto", typeof(string));
            rendiciones_usuario.Columns.Add("detalle", typeof(string));
            rendiciones_usuario.Columns.Add("unidad_de_medida", typeof(string));
            rendiciones_usuario.Columns.Add("concepto", typeof(string));
            rendiciones_usuario.Columns.Add("valor", typeof(string));
            rendiciones_usuario.Columns.Add("cantidad", typeof(string));
            rendiciones_usuario.Columns.Add("precio", typeof(string));
            rendiciones_usuario.Columns.Add("stock_inicial", typeof(string));
            rendiciones_usuario.Columns.Add("stock_final", typeof(string));
            rendiciones_usuario.Columns.Add("proveedor", typeof(string));
            rendiciones_usuario.Columns.Add("fecha", typeof(string));
            rendiciones_usuario.Columns.Add("acuerdo_de_precio", typeof(string));
        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();

            resumen.Columns.Add("dinero_disponible", typeof(string));
            resumen.Columns.Add("compra_de_vegetales", typeof(string));
            resumen.Columns.Add("venta_de_vegetales", typeof(string));
            resumen.Columns.Add("gasto_en_nafta", typeof(string));
            resumen.Columns.Add("venta_cajones", typeof(string));
            resumen.Columns.Add("compra_cajones", typeof(string));
            resumen.Columns.Add("egresos", typeof(string));
            resumen.Columns.Add("pagos_de_locales", typeof(string));
            resumen.Columns.Add("rendicion_teorico", typeof(string));
            resumen.Columns.Add("diferencia", typeof(string));
            resumen.Columns.Add("nuevo_disponible", typeof(string));

        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            /*Session.Add("nafta", 0);
                        Session.Add("cajones_compra", 0);
                        Session.Add("cajones_venta", 0);
                        Session.Add("rendicion_teorica", 0);
                        Session.Add("rendicion_real", 0);
                        Session.Add("egreso", 0); 
                        Session.Add("pagos", 0);*/
            resumen.Rows.Add();
            resumen.Rows[0]["dinero_disponible"] = "Dinero disponible: " + buscar_disponible();
            resumen.Rows[0]["compra_de_vegetales"] = "Compra de vegetales: " + calcular_total_compra();
            resumen.Rows[0]["venta_de_vegetales"] = "Venta de vegetales: " + calcular_total_venta();
            resumen.Rows[0]["gasto_en_nafta"] = "Gasto en nafta: " + calcular_total_nafta();
            resumen.Rows[0]["venta_cajones"] = "Venta cajones: " + calcular_total_venta_cajones();
            resumen.Rows[0]["compra_cajones"] = "Compra cajones: " + calcular_total_compra_cajones();
            resumen.Rows[0]["egresos"] = "Egresos: " + calcular_total_egreso();
            resumen.Rows[0]["pagos_de_locales"] = "Pagos de locales: " + calcular_total_pagos();
            resumen.Rows[0]["rendicion_teorico"] = "Rendicion teorica: " + calcular_rendicion_teorica();
            resumen.Rows[0]["diferencia"] = "Diferencia: " + calcular_diferencia();
            resumen.Rows[0]["nuevo_disponible"] = "Nuevo disponible: " + buscar_nuevo_disponible();

            Session.Add("resumen", resumen);
        }
        private void llenar_datatable()
        {
            crear_tabla_rendiciones();
            rendicionesBD.DefaultView.Sort = "fecha ASC";
            rendicionesBD = rendicionesBD.DefaultView.ToTable();
            int fila_usuario = 0;
            double cantidad, stock_inicial, stock_final, total_compra;
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(rendicionesBD.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text) &&
                    rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1) &&
                    verificar_filtro_concepto(rendicionesBD.Rows[fila]["concepto"].ToString(), Session["filtro"].ToString()))
                {
                    rendiciones_usuario.Rows.Add();
                    rendiciones_usuario.Rows[fila_usuario]["id"] = rendicionesBD.Rows[fila]["id"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["id_producto"] = rendicionesBD.Rows[fila]["id_producto"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["detalle"] = rendicionesBD.Rows[fila]["detalle"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["unidad_de_medida"] = rendicionesBD.Rows[fila]["unidad_de_medida"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["concepto"] = rendicionesBD.Rows[fila]["concepto"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["valor"] = funciones.formatCurrency(double.Parse(rendicionesBD.Rows[fila]["valor"].ToString()));
                    rendiciones_usuario.Rows[fila_usuario]["stock_inicial"] = rendicionesBD.Rows[fila]["stock_inicial"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["stock_final"] = rendicionesBD.Rows[fila]["stock_final"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["proveedor"] = rendicionesBD.Rows[fila]["proveedor"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["fecha"] = rendicionesBD.Rows[fila]["fecha"].ToString();
                    rendiciones_usuario.Rows[fila_usuario]["acuerdo_de_precio"] = rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString();

                    if (rendicionesBD.Rows[fila]["stock_inicial"].ToString() != "N/A" &&
                        rendicionesBD.Rows[fila]["stock_final"].ToString() != "N/A")
                    {


                        stock_inicial = double.Parse(rendicionesBD.Rows[fila]["stock_inicial"].ToString());
                        stock_final = double.Parse(rendicionesBD.Rows[fila]["stock_final"].ToString());

                        cantidad = stock_final - stock_inicial;
                        cantidad = Math.Abs(cantidad);
                        rendiciones_usuario.Rows[fila_usuario]["cantidad"] = Math.Round(cantidad, 2).ToString();

                        total_compra = double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());

                        rendiciones_usuario.Rows[fila_usuario]["precio"] = funciones.formatCurrency(Math.Abs(Math.Round(total_compra / cantidad, 2)));
                    }
                    else
                    {
                        rendiciones_usuario.Rows[fila_usuario]["cantidad"] = "N/A";
                        rendiciones_usuario.Rows[fila_usuario]["precio"] = "N/A";
                    }
                    fila_usuario++;
                }
            }
            Session.Add("rendiciones_usuario", rendiciones_usuario);
        }
        private bool verificar_filtro_concepto(string concepto, string filtro)
        {
            bool retorno = false;
            if (filtro == "Mostrar todo")
            {
                retorno = true;
            }
            else if (concepto == filtro)
            {
                retorno = true;
            }
            return retorno;
        }
        private void cargar_movimientos()
        {
            if (Session["acuerdo_seleccionado_historial"] == null)
            {
                cargar_acuerdos();

            }

            if (Session["acuerdo_seleccionado_historial"].ToString() != "N/A")
            {
                llenar_datatable();
                int fila_acuerdo = buscar_fila_acuerdo(funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1), "compra_a_proveedor");
                label_fecha.Text = acuerdos_de_precio.Rows[fila_acuerdo]["fecha"].ToString();
                gridview_movimientos.DataSource = rendiciones_usuario;
                gridview_movimientos.DataBind();

                label_disponible.Text = "Dinero disponible: " + buscar_disponible();
                label_compra.Text = "Compra de vegetales: " + calcular_total_compra();
                label_venta.Text = "Venta de vegetales: " + calcular_total_venta();
                label_egresos.Text = "Egresos: " + calcular_total_egreso();
                label_pagos.Text = "Pagos de locales: " + calcular_total_pagos();

                label_total_nafta.Text = "Gasto en nafta: " + calcular_total_nafta();
                label_total_compra_cajones.Text = "Compra cajones: " + calcular_total_compra_cajones();
                label_total_venta_cajones.Text = "Venta cajones: " + calcular_total_venta_cajones();
                label_rendicion_real.Text = funciones.formatCurrency(double.Parse(Session["rendicion_real"].ToString()));


                label_rendicion_teorica.Text = "Rendicion teorica: " + calcular_rendicion_teorica();
                label_diferencia.Text = "Diferencia: " + calcular_diferencia();

                label_nuevo_disponible.Text = "Nuevo disponible: " + buscar_nuevo_disponible();
            }
            else
            {
                label_disponible.Text = "Dinero disponible: N/A";
                label_compra.Text = "Compra de vegetales: N/A";
                label_venta.Text = "Venta de vegetales: N/A";
                label_egresos.Text = "Egresos: N/A";
                label_pagos.Text = "Pagos de locales: N/A";

                label_total_nafta.Text = "Gasto en nafta: N/A";
                label_total_compra_cajones.Text = "Compra cajones: N/A";
                label_total_venta_cajones.Text = "Venta cajones: N/A";
                label_rendicion_real.Text = " N/A";

                label_rendicion_teorica.Text = "Rendicion teorica: N/A";
                label_diferencia.Text = "Diferencia: N/A";

                label_nuevo_disponible.Text = "Nuevo disponible: N/A";
            }

        }
        #endregion

        #region funciones
        private string calcular_total_venta_cajones()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Venta cajones")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("cajones_venta", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_compra_cajones()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Compra cajones")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("cajones_compra", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_nafta()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Gasto en nafta")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("nafta", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_pagos()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Pagos")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("pagos", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_egreso()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Egreso")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("egreso", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_compra()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Compra")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            Session.Add("compra", retorno);
            return funciones.formatCurrency(retorno);
        }
        private string calcular_total_venta()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            for (int fila = 0; fila <= rendicionesBD.Rows.Count - 1; fila++)
            {
                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Venta")
                {
                    retorno = retorno + double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                }
            }
            return funciones.formatCurrency(retorno);
        }
        private string buscar_disponible()
        {
            double retorno = 0;
            string acuerdo = Session["acuerdo_seleccionado_historial"].ToString();
            int fila = 0;
            while (fila <= rendicionesBD.Rows.Count - 1)
            {

                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == acuerdo &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Disponible")
                {
                    retorno = double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                    Session.Add("disponible", rendicionesBD.Rows[fila]["valor"].ToString());
                    break;
                }
                fila++;
            }

            return funciones.formatCurrency(retorno);
        }
        private string calcular_rendicion_teorica()
        {
            double retorno = 0;
            double disponible = 0;
            if (Session["disponible"]!=null)
            {
                 disponible = double.Parse(Session["disponible"].ToString());
                
            }
            double compra = double.Parse(Session["compra"].ToString());
            double nafta = double.Parse(Session["nafta"].ToString());
            double cajones_compra = double.Parse(Session["cajones_compra"].ToString());
            double cajones_venta = double.Parse(Session["cajones_venta"].ToString());
            double rendicion_teorica = double.Parse(Session["rendicion_teorica"].ToString());

            double egresos = double.Parse(Session["egreso"].ToString());
            double pagos = double.Parse(Session["pagos"].ToString());


            retorno = disponible - compra - nafta - cajones_compra + cajones_venta - egresos + pagos;
            return funciones.formatCurrency(retorno);
        }
        private string calcular_diferencia()
        {
            //rendicion_real
            double retorno = 0;
            double disponible = 0;
            if (Session["disponible"] != null)
            {
                disponible = double.Parse(Session["disponible"].ToString());

            }
            double compra = double.Parse(Session["compra"].ToString());
            double nafta = double.Parse(Session["nafta"].ToString());
            double cajones_compra = double.Parse(Session["cajones_compra"].ToString());
            double cajones_venta = double.Parse(Session["cajones_venta"].ToString());

            double egresos = double.Parse(Session["egreso"].ToString());
            double pagos = double.Parse(Session["pagos"].ToString());


            double rendicion_teorica = disponible - compra - nafta - cajones_compra + cajones_venta - egresos + pagos;
            double rendicion_real = double.Parse(Session["rendicion_real"].ToString());
            double nuevo_disponible;
            if (double.TryParse(buscar_nuevo_disponible_dato(), out nuevo_disponible))
            {
                retorno = nuevo_disponible - rendicion_teorica;
            }
            else
            {
                retorno = rendicion_real - rendicion_teorica;
            }
            Session.Add("diferencia", retorno);

            return funciones.formatCurrency(retorno);
        }

        private string buscar_nuevo_disponible()
        {
            double retorno = 0;
            int siguiente_acuerdo = int.Parse(Session["acuerdo_seleccionado_historial"].ToString()) + 1;
            int fila = 0;
            while (fila <= rendicionesBD.Rows.Count - 1)
            {

                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == siguiente_acuerdo.ToString() &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Disponible")
                {
                    retorno = double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                    break;
                }
                fila++;
            }

            if (retorno == 0)
            {
                return "Nuevo disponible aun no cargado";
            }
            else
            {
                return funciones.formatCurrency(retorno);
            }
        }
        private string buscar_nuevo_disponible_dato()
        {
            double retorno = 0;
            int siguiente_acuerdo = int.Parse(Session["acuerdo_seleccionado_historial"].ToString()) + 1;
            int fila = 0;
            while (fila <= rendicionesBD.Rows.Count - 1)
            {

                if (rendicionesBD.Rows[fila]["acuerdo_de_precio"].ToString() == siguiente_acuerdo.ToString() &&
                    rendicionesBD.Rows[fila]["concepto"].ToString() == "Disponible")
                {
                    retorno = double.Parse(rendicionesBD.Rows[fila]["valor"].ToString());
                    break;
                }
                fila++;
            }

            if (retorno == 0)
            {
                return "Nuevo disponible aun no cargado";
            }
            else
            {
                return retorno.ToString();
            }
        }
        #endregion
        #region dropDowns
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();

            cargar_filtro_lista();
        }
        private void cargar_acuerdos()
        {
            dropdown_acuerdo.Items.Clear();
            List<string> lista_acuerdo = new List<string>();
            System.Web.UI.WebControls.ListItem item;
            for (int fila = 0; fila <= acuerdos_de_precio.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(acuerdos_de_precio.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text) &&
                    !verificar_si_cargo_acuerdo(lista_acuerdo, acuerdos_de_precio.Rows[fila]["acuerdo"].ToString() + "-" + acuerdos_de_precio.Rows[fila]["fecha"].ToString()) &&
                    acuerdos_de_precio.Rows[fila]["proveedor"].ToString() == proveedorBD.Rows[0]["nombre_en_BD"].ToString())
                {
                    lista_acuerdo.Add(acuerdos_de_precio.Rows[fila]["acuerdo"].ToString() + "-" + acuerdos_de_precio.Rows[fila]["fecha"].ToString());
                }
            }
            int num_item = 1;
            for (int fila = 0; fila <= lista_acuerdo.Count - 1; fila++)
            {
                item = new System.Web.UI.WebControls.ListItem(lista_acuerdo[fila].ToString(), num_item.ToString());
                dropdown_acuerdo.Items.Add(item);
                num_item++;
            }
            int dato = lista_acuerdo.Count;
            dropdown_acuerdo.SelectedValue = dato.ToString();

            
            if(dato > 0)
            {
                Session.Add("acuerdo_seleccionado_historial", funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1));
            }
            else
            {
                Session.Add("acuerdo_seleccionado_historial", "N/A");
            }
        }
        private bool verificar_si_cargo_acuerdo(List<string> lista_acuerdo, string acuerdo)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= lista_acuerdo.Count - 1)
            {
                if (lista_acuerdo[fila].ToString() == acuerdo)
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
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
        private void cargar_filtro_lista()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;

            item = new System.Web.UI.WebControls.ListItem("Mostrar todo", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            item = new System.Web.UI.WebControls.ListItem("Disponible", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            item = new System.Web.UI.WebControls.ListItem("Compra", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            item = new System.Web.UI.WebControls.ListItem("Venta", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            item = new System.Web.UI.WebControls.ListItem("Pagos", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            item = new System.Web.UI.WebControls.ListItem("Egreso", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;


            item = new System.Web.UI.WebControls.ListItem("Venta cajones", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;



            item = new System.Web.UI.WebControls.ListItem("Compra cajones", num_item.ToString());
            dropdown_filtro_lista.Items.Add(item);
            num_item++;

            Session.Add("filtro", dropdown_filtro_lista.SelectedItem.Text);

        }
        #endregion

        #region atributos
        cls_sistema_rendiciones rendicion;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable rendicionesBD;
        DataTable rendiciones_usuario;
        DataTable acuerdos_de_precio;
        DataTable resumen;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (!IsPostBack)
            {
                Session.Remove("acuerdo_seleccionado_historial");
            }
            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                if (Session["rendiciones"] == null)
                {
                    rendicion = new cls_sistema_rendiciones((DataTable)Session["usuariosBD"]);
                    Session.Add("rendiciones", rendicion);
                }
                rendicion = (cls_sistema_rendiciones)Session["rendiciones"];

                acuerdos_de_precio = rendicion.get_acuerdos_de_precios();
                rendicionesBD = rendicion.get_rendiciones();
                if (!IsPostBack)
                {
                    Session.Add("nafta", 0);
                    Session.Add("cajones_compra", 0);
                    Session.Add("cajones_venta", 0);
                    Session.Add("rendicion_teorica", 0);
                    Session.Add("rendicion_real", 0);
                    Session.Add("egreso", 0);
                    Session.Add("pagos", 0);
                    Session.Add("diferencia", 0);

                    configurar_controles();
                    cargar_movimientos();
                }

            }

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("acuerdo_seleccionado_historial");

            cargar_movimientos();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("acuerdo_seleccionado_historial");                          
            cargar_movimientos();
        }

        protected void dropdown_acuerdo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("acuerdo_seleccionado_historial", funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1));
            cargar_movimientos();


        }

        protected void boton_generar_pdf_Click(object sender, EventArgs e)
        {
            llenar_tabla_resumen();
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = proveedorBD.Rows[0]["nombre_proveedor"].ToString() + " acuerdo-" + funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1) + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            rendicion.crear_PDF_rendicion(ruta_archivo, imgdata, label_fecha.Text, funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1), (DataTable)Session["proveedorBD"], (DataTable)Session["resumen"], (DataTable)Session["rendiciones_usuario"]);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();


        }

        protected void boton_carga_nafta_Click(object sender, EventArgs e)
        {
            double valor;
            if (double.TryParse(textbox_nafta.Text, out valor))
            {
                rendicion.cargar_nafta(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), dropdown_acuerdo.SelectedItem.Text, valor.ToString());
                rendicionesBD = rendicion.get_rendiciones();
                textbox_nafta.Text = string.Empty;
                cargar_movimientos();
            }


        }

        protected void boton_carga_cajones_compra_Click(object sender, EventArgs e)
        {
            double valor;
            if (double.TryParse(textbox_cajones_compra.Text, out valor))
            {
                rendicion.cargar_compra_cajones(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), dropdown_acuerdo.SelectedItem.Text, valor.ToString());
                rendicionesBD = rendicion.get_rendiciones();
                textbox_cajones_compra.Text = string.Empty;
                cargar_movimientos();

            }



        }

        protected void boton_carga_cajones_venta_Click(object sender, EventArgs e)
        {
            double valor;
            if (double.TryParse(textbox_cajones_ventas.Text, out valor))
            {
                rendicion.cargar_venta_cajones(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), dropdown_acuerdo.SelectedItem.Text, valor.ToString());
                rendicionesBD = rendicion.get_rendiciones();
                textbox_cajones_ventas.Text = string.Empty;
                cargar_movimientos();

            }


        }

        protected void boton_carga_rendicionReal_Click(object sender, EventArgs e)
        {
            double precio;
            if (double.TryParse(textbox_rendicion_real.Text, out precio))
            {
                Session.Add("rendicion_real", precio);
                textbox_rendicion_real.Text = string.Empty;
            }
            cargar_movimientos();


        }
        protected void boton_cargar_datos_Click(object sender, EventArgs e)
        {
            rendicion.cargar_nuevo_disponible(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1), Session["rendicion_real"].ToString());
            rendicionesBD = rendicion.get_rendiciones();
            Session.Add("nafta", 0);
            Session.Add("cajones_compra", 0);
            Session.Add("cajones_venta", 0);
            Session.Add("rendicion_teorica", 0);
            Session.Add("rendicion_real", 0);
            Session.Add("egreso", 0);
            Session.Add("pagos", 0);
            cargar_movimientos();
        }

        protected void boton_egreso_Click(object sender, EventArgs e)
        {
            double valor;
            if (double.TryParse(textbox_egreso_cantidad.Text, out valor) && textbox_egreso_detalle.Text != string.Empty)
            {
                rendicion.cargar_egreso(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), dropdown_acuerdo.SelectedItem.Text, valor.ToString(), textbox_egreso_detalle.Text);
                rendicionesBD = rendicion.get_rendiciones();
                textbox_egreso_cantidad.Text = string.Empty;
                textbox_egreso_detalle.Text = string.Empty;
                cargar_movimientos();

            }


        }

        protected void boton_pago_Click(object sender, EventArgs e)
        {
            double valor;
            if (double.TryParse(textbox_pago_cantidad.Text, out valor) && textbox_pago_detalle.Text != string.Empty)
            {
                rendicion.cargar_pago(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), dropdown_acuerdo.SelectedItem.Text, valor.ToString(), textbox_pago_detalle.Text);
                rendicionesBD = rendicion.get_rendiciones();
                textbox_pago_cantidad.Text = string.Empty;
                textbox_pago_detalle.Text = string.Empty;
                cargar_movimientos();

            }


        }

        protected void dropdown_filtro_lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("filtro", dropdown_filtro_lista.SelectedItem.Text);
            cargar_movimientos();

        }
    }
}