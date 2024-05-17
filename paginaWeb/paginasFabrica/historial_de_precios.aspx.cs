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
    public partial class reporteria : System.Web.UI.Page
    {
        private void añadir_columnas_a_prodocutos()
        {
            productos_proveedor.Columns.Add("precio_compra", typeof(string));
            productos_proveedor.Columns.Add("precio_venta", typeof(string));
            productos_proveedor.Columns.Add("ganancia_pesos", typeof(string));
            productos_proveedor.Columns.Add("ganancia_porcentaje", typeof(string));
        }
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
        private void llenar_datatable()
        {
            string id;
            double precio_compra, precio_venta, ganancia_pesos, ganancia_porcentaje;
            int fila_acuerdo_venta = buscar_fila_acuerdo(funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text,1), "fabrica_a_local");
            int fila_acuerdo_compra = buscar_fila_acuerdo(funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1), "compra_a_proveedor");
            int cantidad_iteraciones = 0;
            double sumatoria_de_ganancias=0;
            label_fecha.Text = "FECHA DE ACUERDO: " + acuerdos_de_precio.Rows[fila_acuerdo_compra]["fecha"].ToString();
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                id = productos_proveedor.Rows[fila]["id"].ToString();
                productos_proveedor.Rows[fila]["precio_compra"] = funciones.formatCurrency(double.Parse(acuerdos_de_precio.Rows[fila_acuerdo_compra]["producto_" + id].ToString()));
                productos_proveedor.Rows[fila]["precio_venta"] = funciones.formatCurrency(double.Parse(acuerdos_de_precio.Rows[fila_acuerdo_venta]["producto_" + id].ToString()));
                precio_compra = double.Parse(acuerdos_de_precio.Rows[fila_acuerdo_compra]["producto_" + id].ToString());
                precio_venta = double.Parse(acuerdos_de_precio.Rows[fila_acuerdo_venta]["producto_" + id].ToString());
                ganancia_pesos = precio_venta-precio_compra;
                productos_proveedor.Rows[fila]["ganancia_pesos"] = funciones.formatCurrency(ganancia_pesos);
                if (precio_compra==0)
                {
                    ganancia_porcentaje=0;
                }
                else
                {
                    ganancia_porcentaje = Math.Round((ganancia_pesos * 100) / precio_compra, 2);
                    sumatoria_de_ganancias = sumatoria_de_ganancias + ganancia_porcentaje;
                    cantidad_iteraciones++;
                }
                productos_proveedor.Rows[fila]["ganancia_porcentaje"] = ganancia_porcentaje.ToString() +"%";
            }
            Session.Add("productos_proveedor_reporteria", productos_proveedor);
            label_promedio_ganancia.Text = "Promedio de ganancia: " + Math.Round(sumatoria_de_ganancias/cantidad_iteraciones,2).ToString() + "%";
        }
        private void cargar_productos()
        {
            if(Session["acuerdo_seleccionado_historial"]==null)
            {
                cargar_acuerdos();

            }
            if (Session["acuerdo_seleccionado_historial"].ToString() != "N/A")
            {
                llenar_datatable();
                gridview_productos.DataSource = productos_proveedor;
                gridview_productos.DataBind();
            }
            
        }

        #region dropDowns
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();
           
        }
        private void cargar_acuerdos()
        {
            dropdown_acuerdo.Items.Clear();
            List<string> lista_acuerdo = new List<string>();
            System.Web.UI.WebControls.ListItem item;
            for (int fila = 0; fila <= acuerdos_de_precio.Rows.Count - 1; fila++)
            {
                if (funciones.verificar_fecha(acuerdos_de_precio.Rows[fila]["fecha"].ToString(), dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text) &&
                    !verificar_si_cargo_acuerdo(lista_acuerdo, acuerdos_de_precio.Rows[fila]["acuerdo"].ToString() + "-" + acuerdos_de_precio.Rows[fila]["fecha"].ToString()))
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
            if (dato > 0)
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
        #endregion

        #region atributos
        cls_funciones funciones = new cls_funciones();
        cls_historial_de_precios_fabrica reportes;
        DataTable proveedorBD;
        DataTable productos_proveedor;
        DataTable acuerdos_de_precio;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                proveedorBD = (DataTable)Session["proveedorBD"];
                if (!IsPostBack)
                {
                    Session.Remove("acuerdo_seleccionado_historial");
                    //calcular remitos nuevos
                    Session.Add("reportes", new cls_historial_de_precios_fabrica((DataTable)Session["usuariosBD"]));

                }
                reportes = (cls_historial_de_precios_fabrica)Session["reportes"];
                productos_proveedor = reportes.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
                acuerdos_de_precio = reportes.get_acuerdo_de_precios(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
                añadir_columnas_a_prodocutos();
                if (!IsPostBack)
                {
                 
                    configurar_controles();

                    cargar_productos();

                }
            }
          
        }

        protected void dropdown_acuerdo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();

        
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("acuerdo_seleccionado_historial");

            cargar_productos();

         
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Remove("acuerdo_seleccionado_historial");

            cargar_productos();

         
        }

        protected void boton_generar_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = proveedorBD.Rows[0]["nombre_proveedor"].ToString() + " acuerdo-" + funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1) + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            reportes.crear_PDF(ruta_archivo, imgdata, label_fecha.Text, funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text,1), (DataTable)Session["productos_proveedor_reporteria"], (DataTable)Session["proveedorBD"], label_promedio_ganancia.Text);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
      
            
        }

        protected void Button_lista_de_precios_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = proveedorBD.Rows[0]["nombre_proveedor"].ToString() + " acuerdo-" + funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1) + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            reportes.crear_lista_de_precios_PDF(ruta_archivo, imgdata, label_fecha.Text, funciones.obtener_dato(dropdown_acuerdo.SelectedItem.Text, 1), (DataTable)Session["productos_proveedor_reporteria"], (DataTable)Session["proveedorBD"]);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
          
           
        }
    }
}