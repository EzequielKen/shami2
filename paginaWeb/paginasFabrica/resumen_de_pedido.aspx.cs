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
    public partial class resumen_de_pedido : System.Web.UI.Page
    {
        private void crear_pdf()
        {
            if (resumen_sucursales.Rows.Count > 0)
            {
                pedidos_fabrica.actualizar_estado_de_pedido(pedidos_no_cargados);
                DateTime hora = DateTime.Now;

                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = "RESUMEN DE PEDIDO -" + " - id-" + dato_hora + ".pdf";
                string ruta = "/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                pedidos_fabrica.crear_pdf_resumen_pedido(pedidos_no_cargados, resumen_sucursales, ruta_archivo, imgdata);

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();*/
            }
        }

        private void crear_pdf_produccion_diaria()
        {
            if (resumen_sucursales.Rows.Count > 0)
            {
                pedidos_fabrica.actualizar_estado_de_pedido(pedidos_no_cargados);

                DateTime hora = DateTime.Now;

                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = "RESUMEN DE PEDIDO -" + " - id-" + dato_hora + ".pdf";
                string ruta = "/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                pedidos_fabrica.crear_pdf_resumen_pedido_produccion_diaria((DataTable)Session["proveedorBD"], resumen_sucursales, ruta_archivo, imgdata);

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();*/
            }
        }
        private void crear_pdf_panificados()
        {
            if (resumen_sucursales.Rows.Count > 0)
            {
                pedidos_fabrica.actualizar_estado_de_pedido(pedidos_no_cargados);

                DateTime hora = DateTime.Now;

                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = "RESUMEN DE PEDIDO -" + " - id-" + dato_hora + ".pdf";
                string ruta = "/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                pedidos_fabrica.crear_pdf_resumen_panificados((DataTable)Session["proveedorBD"], resumen_sucursales, ruta_archivo, imgdata);

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();*/
            }
        }
        private void cargar_sucursal_segun_dia_de_enrega(string dia)
        {
            resumen_sucursales.Rows.Clear();
            string sucursal = "";
            for (int fila_dia = 0; fila_dia <= calendario_entrega.Rows.Count - 1; fila_dia++)
            {
                if (calendario_entrega.Rows[fila_dia][dia].ToString() != "N/A")
                {
                    sucursal = calendario_entrega.Rows[fila_dia][dia].ToString();
                    for (int fila_lista = 0; fila_lista <= gridView_sucursales.Rows.Count - 1; fila_lista++)
                    {
                        if (sucursal == gridView_sucursales.Rows[fila_lista].Cells[1].Text)
                        {
                            id_sucursal = gridView_sucursales.Rows[fila_lista].Cells[0].Text;
                            sucursal_seleccionada = gridView_sucursales.Rows[fila_lista].Cells[1].Text;
                            if (!verificar_si_cargo())
                            {
                                resumen_sucursales.Rows.Add();
                                int fila = resumen_sucursales.Rows.Count - 1;
                                resumen_sucursales.Rows[fila]["id"] = id_sucursal;
                                resumen_sucursales.Rows[fila]["sucursal"] = sucursal_seleccionada;
                                Session.Remove("resumen_sucursal");
                                Session.Add("resumen_sucursal", resumen_sucursales);
                            }
                        }
                    }
                }
            }
        }
        private void crear_pdf_pedido_de_panificados()
        {
            if (resumen_sucursales.Rows.Count > 0)
            {
                pedidos_fabrica.actualizar_estado_de_pedido(pedidos_no_cargados);

                DateTime hora = DateTime.Now;

                string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
                string id_pedido = "RESUMEN DE PEDIDO -" + " - id-" + dato_hora + ".pdf";
                string ruta = "/paginasFabrica/pdf/" + id_pedido;
                string ruta_archivo = Server.MapPath(ruta);

                byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
                pedidos_fabrica.crear_pdf_pedido_panificados((DataTable)Session["proveedorBD"], resumen_sucursales, ruta_archivo, imgdata);

                //           Response.Redirect("~/archivo.pdf");
                string strUrl = "/paginasFabrica/pdf/" + id_pedido;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
                //GenerarPDF_Click();*/
            }
        }
        private void cargar_sucursal_en_resumen()
        {
            if (!verificar_si_cargo())
            {
                resumen_sucursales.Rows.Add();
                int fila = resumen_sucursales.Rows.Count - 1;
                resumen_sucursales.Rows[fila]["id"] = id_sucursal;
                resumen_sucursales.Rows[fila]["sucursal"] = sucursal_seleccionada;
                Session.Remove("resumen_sucursal");
                Session.Add("resumen_sucursal", resumen_sucursales);
            }

        }
        private void cargar_todas_las_sucursales_en_resumen()
        {
            for (int fila = 0; fila <= gridView_sucursales.Rows.Count - 1; fila++)
            {
                id_sucursal = gridView_sucursales.Rows[fila].Cells[0].Text;
                sucursal_seleccionada = gridView_sucursales.Rows[fila].Cells[1].Text;
                if (!verificar_si_cargo())
                {
                    resumen_sucursales.Rows.Add();
                    int fila_resumen = resumen_sucursales.Rows.Count - 1;
                    resumen_sucursales.Rows[fila_resumen]["id"] = id_sucursal;
                    resumen_sucursales.Rows[fila_resumen]["sucursal"] = sucursal_seleccionada;
                    Session.Remove("resumen_sucursal");
                    Session.Add("resumen_sucursal", resumen_sucursales);
                }
            }

        }
        private void eliminar_sucursal_en_resumen()
        {
            int fila = buscar_fila_sucursal();
            resumen_sucursales.Rows[fila].Delete();
        }
        private void cargar_sucursales()
        {
            llenar_sucursal_usuario();
            gridView_sucursales.DataSource = sucursales_usuario;
            gridView_sucursales.DataBind();


            gridview_resumen.DataSource = resumen_sucursales;
            gridview_resumen.DataBind();
        }
        private void llenar_sucursal_usuario()
        {
            crear_sucursal_usuario();
            string sucursal;
            int fila_sucursal, fila_usuarios;
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                sucursal = pedidos_no_cargados.Rows[fila]["sucursal"].ToString();
                if (!verificar_si_cargo_sucursal(sucursal))
                {
                    fila_sucursal = buscar_fila_sucursal_a_cargar(sucursal);
                    sucursales_usuario.Rows.Add();
                    fila_usuarios = sucursales_usuario.Rows.Count - 1;

                    sucursales_usuario.Rows[fila_usuarios]["id"] = sucursalesBD.Rows[fila_sucursal]["id"].ToString();
                    sucursales_usuario.Rows[fila_usuarios]["sucursal"] = sucursalesBD.Rows[fila_sucursal]["sucursal"].ToString();
                }
            }
        }
        private void crear_sucursal_usuario()
        {
            sucursales_usuario = new DataTable();
            sucursales_usuario.Columns.Add("id", typeof(string));
            sucursales_usuario.Columns.Add("sucursal", typeof(string));
        }
        private void crear_tabla_resumen()
        {
            resumen_sucursales = new DataTable();
            resumen_sucursales.Columns.Add("id", typeof(string));
            resumen_sucursales.Columns.Add("sucursal", typeof(string));
        }
        private bool verificar_si_cargo_sucursal(string sucursal)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= sucursales_usuario.Rows.Count - 1)
            {
                if (sucursal == sucursales_usuario.Rows[fila]["sucursal"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_sucursal_a_cargar(string sucursal)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= sucursalesBD.Rows.Count - 1)
            {
                if (sucursal == sucursalesBD.Rows[fila]["sucursal"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
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
        private int buscar_fila_sucursal()
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= resumen_sucursales.Rows.Count - 1)
            {
                if (id_sucursal == resumen_sucursales.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        ///////////////////////////////////////////////////////////////////////////
        cls_sistema_pedidos_fabrica pedidos_fabrica;
        cls_dia_de_entrega dia_entrega;
        DataTable sucursalesBD;
        DataTable sucursales_usuario;
        DataTable resumen_sucursales;
        DataTable pedidos_no_cargados;
        DataTable productos_proveedor;
        DataTable proveedorBD;
        DataTable usuariosBD;
        DataTable tipo_usuario;
        DataTable calendario_entrega;
        string id_sucursal, sucursal_seleccionada;
        protected void Page_Load(object sender, EventArgs e)
        {

            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            usuariosBD = (DataTable)Session["usuariosBD"];

            //calcular remitos nuevos

            pedidos_fabrica = new cls_sistema_pedidos_fabrica((DataTable)Session["usuariosBD"]);


            sucursalesBD = pedidos_fabrica.get_sucursales();




            dia_entrega = new cls_dia_de_entrega(usuariosBD);
            calendario_entrega = dia_entrega.get_dias_de_entrega();
            if (Session["resumen_sucursal"] == null)
            {
                crear_tabla_resumen();
                Session.Add("resumen_sucursal", resumen_sucursales);
            }
            resumen_sucursales = (DataTable)Session["resumen_sucursal"];
            pedidos_no_cargados = pedidos_fabrica.get_pedidos_no_cargados_nuevo();
            productos_proveedor = pedidos_fabrica.get_productos_nuevo();
            cargar_sucursales();
        }

        protected void gridview_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_sucursal = gridView_sucursales.SelectedRow.Cells[0].Text;
            sucursal_seleccionada = gridView_sucursales.SelectedRow.Cells[1].Text;
            cargar_sucursal_en_resumen();
            cargar_sucursales();
        }

        protected void gridview_resumen_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_sucursal = gridview_resumen.SelectedRow.Cells[0].Text;
            eliminar_sucursal_en_resumen();
            cargar_sucursales();

        }

        protected void boton_crear_pdf_Click(object sender, EventArgs e)
        {//Generar PDF resumen segun sucursal
            crear_pdf();


        }

        protected void boton_resumir_todo_Click(object sender, EventArgs e)
        {
            cargar_todas_las_sucursales_en_resumen();
            cargar_sucursales();
            crear_pdf();


        }

        protected void boton_resumir_produccion_diaria_Click(object sender, EventArgs e)
        {
            cargar_todas_las_sucursales_en_resumen();
            cargar_sucursales();
            crear_pdf_produccion_diaria();
        }

        protected void boton_resumir_produccion_diaria_segun_sucursal_Click(object sender, EventArgs e)
        {
            crear_pdf_produccion_diaria();

        }

        protected void Boton_panificados_Click(object sender, EventArgs e)
        {
            crear_pdf_panificados();
        }

        protected void Boton_pedir_panificados_Click(object sender, EventArgs e)
        {
            crear_pdf_pedido_de_panificados();
        }

        #region dias
        protected void boton_lunes_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("lunes");
            cargar_sucursales();

        }

        protected void boton_martes_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("martes");
            cargar_sucursales();

        }

        protected void boton_miercoles_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("miercoles");
            cargar_sucursales();

        }

        protected void boton_jueves_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("jueves");
            cargar_sucursales();

        }

        protected void boton_viernes_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("viernes");
            cargar_sucursales();

        }

        protected void boton_sabado_Click(object sender, EventArgs e)
        {
            cargar_sucursal_segun_dia_de_enrega("sabado");
            cargar_sucursales();

        }
        #endregion
        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {

        }

    }
}