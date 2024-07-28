using _02___sistemas;
using _03___sistemas_fabrica;
using System;
using System.Configuration;
using System.Data;
using System.IO;
namespace paginaWeb
{
    public partial class login : System.Web.UI.Page
    {

        private void crear_tipo_usuario()
        {
            tipo_usuario = new DataTable();
            tipo_usuario.Columns.Add("tipo", typeof(string));
            tipo_usuario.Columns.Add("seguridad", typeof(string));
            tipo_usuario.Columns.Add("rol", typeof(string));
            tipo_usuario.Columns.Add("encargado", typeof(string));

            tipo_usuario.Rows.Add();
            tipo_usuario.Rows[0]["tipo"] = "empleado";
            tipo_usuario.Rows[0]["seguridad"] = "1";
            tipo_usuario.Rows[0]["rol"] = "shami empleado";
        }
        cls_sistema_login login_sistema = new cls_sistema_login();
        cls_estadisticas_de_pedidos estadisticas;
        cls_stock_insumos stock_insumo;

        DataTable usuarioBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;
        DataTable sucursal;
        protected void Page_Load(object sender, EventArgs e)
        {

            borrar_pdf();


        }

        protected void boton_login_Click(object sender, EventArgs e)
        {

            if (login_sistema.login(textbox_usuario.Text, textbox_contraseña.Text))
            {

                usuarioBD = login_sistema.get_usuarios();

                sucursal = login_sistema.get_sucursal();
                if ("1" == ConfigurationManager.AppSettings["desarrollo"])
                {
                    usuarioBD.Rows[0]["servidor"] = "201.235.217.38";
                }
                Session.Add("sucursal", sucursal);
                Session.Add("usuariosBD", usuarioBD);
                tipo_usuario = login_sistema.get_tipo_usuario();
                Session.Add("tipo_usuario", tipo_usuario);
                Session.Add("nivel_seguridad", tipo_usuario.Rows[0]["seguridad"].ToString());
                if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Gerente")
                {
                    Session.Add("proveedorBD", login_sistema.get_proveedor_seleccionado(usuarioBD.Rows[0]["proveedor"].ToString()));
                    proveedorBD = (DataTable)Session["proveedorBD"];

                    Response.Redirect("~/paginasGerente/caja_chica.aspx", false);
                }
                else
                {

                    if (usuarioBD.Rows[0]["proveedor"].ToString() != "0")
                    {
                        Session.Add("proveedorBD", login_sistema.get_proveedor_seleccionado(usuarioBD.Rows[0]["proveedor"].ToString()));
                        proveedorBD = (DataTable)Session["proveedorBD"];

                        if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                        {

                            estadisticas = (cls_estadisticas_de_pedidos)Session["estadisticas"];
                            if (Session["stock_insumo"] == null)
                            {
                                Session.Add("stock_insumo", new cls_stock_insumos(usuarioBD));
                            }
                            stock_insumo = (cls_stock_insumos)Session["stock_insumo"];
                            stock_insumo.actualizar_stock_insumos();
                        }


                        if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Operaciones")
                        {
                            Response.Redirect("~/paginasFabrica/temperatura_de_equipos.aspx", false);
                        }
                        else if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Compras")
                        {
                            Response.Redirect("~/paginasFabrica/proveedores_fabrica.aspx", false);
                        }
                        else if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Admin")
                        {
                            Response.Redirect("~/paginasFabrica/produccion.aspx", false);
                        }
                        else
                        {
                            if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Expedicion")
                            {
                                Response.Redirect("~/paginasFabrica/landing_page_expedicion.aspx", false);
                            }
                            else if (tipo_usuario.Rows[0]["rol"].ToString() == "Shami Villa Maipu Produccion")
                            {
                                Response.Redirect("~/paginasFabrica/produccion.aspx", false);
                            }
                            else if (tipo_usuario.Rows[0]["rol"].ToString() == "Amir")
                            {
                                Response.Redirect("~/paginasFabrica/cuentas_por_cobrar.aspx", false);
                            }
                            else
                            {
                                Response.Redirect("~/paginasFabrica/sucursales.aspx", false);
                            }
                        }

                    }
                    else if (usuarioBD.Rows[0]["carrefour"].ToString() == "1")
                    {
                        Response.Redirect("~/paginasCarrefour/sucursales_carrefour.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("~/paginas/proveedores.aspx", false);
                        //Response.Redirect("~/paginas/landing_page_local.aspx", false);
                    }
                }
            }
            else if (login_sistema.login_empleado(textbox_usuario.Text, textbox_contraseña.Text))
            {
                usuarioBD = login_sistema.get_usuarios();
                if ("1" == ConfigurationManager.AppSettings["desarrollo"])
                {
                    usuarioBD.Rows[0]["servidor"] = "201.235.217.38";
                }
                Session.Add("sucursal", login_sistema.get_sucursal());
                Session.Add("empleado", login_sistema.get_empleado());
                DataTable empleado = (DataTable)Session["empleado"];
                Session.Add("usuariosBD", usuarioBD);
                crear_tipo_usuario();
                tipo_usuario.Rows[0]["encargado"] = login_sistema.get_estado_encargado((DataTable)Session["empleado"]);
                Session.Add("tipo_usuario", tipo_usuario);
                login_sistema.registrar_logueo_empleado((DataTable)Session["empleado"]);
                if (login_sistema.verificar_brecha_horaria(DateTime.Now) &&
                    empleado.Rows[0]["turno_logueado"].ToString() == "Turno 1") //
                {
                    Response.Redirect("~/paginas/login_turno.aspx", false);
                }
                else
                {
                    Response.Redirect("~/paginas/lista_de_chequeo.aspx", false);
                }
            }
            textbox_usuario.Attributes.Add("placeholder", "ingrese su usuario");
            textbox_contraseña.Attributes.Add("placeholder", "ingrese contraseña");
        }

        private void borrar_pdf()
        {
            string directorio = Server.MapPath("~/paginas/pdf");
            string extension = ".pdf";
            string[] files = Directory.GetFiles(directorio, "*" + extension);
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            directorio = Server.MapPath("~/paginasFabrica/pdf");
            extension = ".pdf";
            files = Directory.GetFiles(directorio, "*" + extension);
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }




    }
}