using _02___sistemas;
using _07_sistemas_supervision;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasSupervision
{
    public partial class historial_visita_operativa : System.Web.UI.Page
    {
        #region carga empleado
        private void crear_lista_de_evaluados()
        {
            lista_de_evaluados = new DataTable();
            lista_de_evaluados.Columns.Add("id", typeof(string));
            lista_de_evaluados.Columns.Add("id_sucursal", typeof(string));
            lista_de_evaluados.Columns.Add("nombre", typeof(string));
            lista_de_evaluados.Columns.Add("apellido", typeof(string));
            lista_de_evaluados.Columns.Add("dni", typeof(string));
            lista_de_evaluados.Columns.Add("telefono", typeof(string));
            lista_de_evaluados.Columns.Add("cargo", typeof(string));
            lista_de_evaluados.Columns.Add("promedio", typeof(string));
        }
        private void llenar_tabla_evaluados()
        {
            crear_lista_de_evaluados();
            int ultima_fila = 0;
            for (int fila = 0; fila <= lista_de_evaluadosBD.Rows.Count - 1; fila++)
            {
                if (!verificar_si_cargo(lista_de_evaluadosBD.Rows[fila]["id_empleado"].ToString()))
                {
                    lista_de_evaluados.Rows.Add();
                    ultima_fila = lista_de_evaluados.Rows.Count - 1;
                    lista_de_evaluados.Rows[ultima_fila]["id"] = lista_de_evaluadosBD.Rows[fila]["id_empleado"].ToString();
                    lista_de_evaluados.Rows[ultima_fila]["id_sucursal"] = lista_de_evaluadosBD.Rows[fila]["id_sucursal"].ToString();
                    lista_de_evaluados.Rows[ultima_fila]["nombre"] = lista_de_evaluadosBD.Rows[fila]["nombre"].ToString();
                    lista_de_evaluados.Rows[ultima_fila]["apellido"] = lista_de_evaluadosBD.Rows[fila]["apellido"].ToString();
                }
            }
            for (int fila = 0; fila <= lista_de_evaluados.Rows.Count - 1; fila++)
            {
                lista_de_evaluados.Rows[fila]["promedio"] = historial.get_promedio_de_puntaje(lista_de_evaluados.Rows[fila]["id"].ToString(), lista_de_evaluadosBD);
            }
        }
        private bool verificar_si_cargo(string id_empleado)
        {
            bool retorno = false;
            for (int fila = 0; fila <= lista_de_evaluados.Rows.Count - 1; fila++)
            {
                if (id_empleado == lista_de_evaluados.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
            }
            return retorno;
        }
        private void cargar_lista_evaluados()
        {
            llenar_tabla_evaluados();
            gridview_empleados.DataSource = lista_de_evaluados;
            gridview_empleados.DataBind();
            Session.Add("lista_de_evaluados", lista_de_evaluados);
            calcular_promedio_evaluados();
        }

        private void calcular_promedio_evaluados()
        {
            double promedio = 0;
            double suma = 0;
            for (int fila = 0; fila <= lista_de_evaluados.Rows.Count - 1; fila++)
            {
                suma = suma + double.Parse(lista_de_evaluados.Rows[fila]["promedio"].ToString());
            }
            promedio = suma / lista_de_evaluados.Rows.Count;
            label_promedio_evaluados.Text = "Promedio de puntajes: " + promedio.ToString("0.00");
            Session.Add("promedio_local", promedio.ToString("0.00"));

        }
        #endregion

        #region resumen
        private void limpiar_resumen()
        {
            label_empleado.Visible = false;
            label_fecha_historial.Visible = false;


            boton_encargado.Visible = false;
            boton_cajero.Visible = false;
            boton_shawarmero.Visible = false;
            boton_atencion.Visible = false;
            boton_cocina.Visible = false;
            boton_limpieza.Visible = false;
            gridview_chequeos.Visible = false;

            gridview_chequeos.DataSource = null;
            gridview_chequeos.DataBind();
        }

        private void llenar_tabla_actividades_evaluadas_con_actividades()
        {
            actividades_evaluadas = (DataTable)Session["actividades_evaluadas"];

            string id_actividad;
            int fila_actividad;
            for (int fila = 0; fila <= actividades_evaluadas.Rows.Count - 1; fila++)
            {
                id_actividad = actividades_evaluadas.Rows[fila]["actividad"].ToString();
                fila_actividad = funciones.buscar_fila_por_id(id_actividad, lista_de_chequeoBD);
                actividades_evaluadas.Rows[fila]["actividad"] = lista_de_chequeoBD.Rows[fila_actividad]["actividad"].ToString();
            }
            Session.Add("actividades_evaluadas", actividades_evaluadas);
        }
        private void cargar_actividad_en_resumen(string id)
        {
            resumenBD = (DataTable)Session["resumen_chequeo"];
            int fila_actividad = funciones.buscar_fila_por_id(id, lista_de_chequeoBD);
            int fila_resumen = funciones.buscar_fila_por_id(id, resumenBD);
            if (fila_resumen == -1)
            {
                resumenBD.Rows.Add();
                int ultima_fila = resumenBD.Rows.Count - 1;

                resumenBD.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila_actividad]["id"].ToString();
                resumenBD.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila_actividad]["actividad"].ToString();
                resumenBD.Rows[ultima_fila]["categoria"] = lista_de_chequeoBD.Rows[fila_actividad]["categoria"].ToString();
                resumenBD.Rows[ultima_fila]["area"] = lista_de_chequeoBD.Rows[fila_actividad]["area"].ToString();
                resumenBD.Rows[ultima_fila]["orden"] = int.Parse(lista_de_chequeoBD.Rows[fila_actividad]["orden"].ToString());
            }
            else
            {
                resumenBD.Rows[fila_resumen].Delete();
            }

            Session.Add("resumen_chequeo", resumenBD);

        }
        private void cargar_actividad_en_resumen_todo(string id)
        {
            resumenBD = (DataTable)Session["resumen_chequeo"];
            int fila_actividad = funciones.buscar_fila_por_id(id, lista_de_chequeoBD);
            int fila_resumen = funciones.buscar_fila_por_id(id, resumenBD);
            if (fila_resumen == -1)
            {
                resumenBD.Rows.Add();
                int ultima_fila = resumenBD.Rows.Count - 1;

                resumenBD.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila_actividad]["id"].ToString();
                resumenBD.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila_actividad]["actividad"].ToString();
                resumenBD.Rows[ultima_fila]["categoria"] = lista_de_chequeoBD.Rows[fila_actividad]["categoria"].ToString();
                resumenBD.Rows[ultima_fila]["area"] = lista_de_chequeoBD.Rows[fila_actividad]["area"].ToString();
            }

            Session.Add("resumen_chequeo", resumenBD);

        }


        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            empleado = (DataTable)Session["empleado_historial"];

            string nombre = empleado.Rows[0]["nombre"].ToString();
            string apellido = empleado.Rows[0]["apellido"].ToString();
            string cargo = empleado.Rows[0]["cargo"].ToString();


        }


        private void configurar_botones_cargos(string cargos)
        {
            int iteraciones = int.Parse(funciones.obtener_dato(cargos, 1));
            int posicion = 2;
            string cargo;
            boton_encargado.Visible = false;
            boton_cajero.Visible = false;
            boton_shawarmero.Visible = false;
            boton_atencion.Visible = false;
            boton_cocina.Visible = false;
            boton_limpieza.Visible = false;

            for (int index = 1; index <= iteraciones; index++)
            {
                cargo = funciones.obtener_dato(cargos, posicion);
                if (cargo == "Encargado")
                {
                    boton_encargado.Visible = true;
                }


                if (cargo == "Cajero")
                {
                    boton_cajero.Visible = true;
                }


                if (cargo == "Shawarmero")
                {
                    boton_shawarmero.Visible = true;
                }


                if (cargo == "Atencion al Cliente")
                {
                    boton_atencion.Visible = true;
                }


                if (cargo == "Cocina")
                {
                    boton_cocina.Visible = true;
                }


                if (cargo == "Limpieza")
                {
                    boton_limpieza.Visible = true;
                }

                posicion++;
            }
        }
        private void configurar_botones_cargos_activos(string cargo)
        {
            if (cargo == "Encargado")
            {
                boton_encargado.CssClass = "btn btn-success";
            }
            else
            {
                boton_encargado.CssClass = "btn btn-primary";
            }

            if (cargo == "Cajero")
            {
                boton_cajero.CssClass = "btn btn-success";
            }
            else
            {
                boton_cajero.CssClass = "btn btn-primary";
            }

            if (cargo == "Shawarmero")
            {
                boton_shawarmero.CssClass = "btn btn-success";
            }
            else
            {
                boton_shawarmero.CssClass = "btn btn-primary";
            }

            if (cargo == "Atencion al Cliente")
            {
                boton_atencion.CssClass = "btn btn-success";
            }
            else
            {
                boton_atencion.CssClass = "btn btn-primary";
            }

            if (cargo == "Cocina")
            {
                boton_cocina.CssClass = "btn btn-success";
            }
            else
            {
                boton_cocina.CssClass = "btn btn-primary";
            }

            if (cargo == "Limpieza")
            {
                boton_limpieza.CssClass = "btn btn-success";
            }
            else
            {
                boton_limpieza.CssClass = "btn btn-primary";
            }
        }
        #endregion
        private void cargar_lista_chequeo()
        {


            llenar_tabla_actividades_evaluadas_con_actividades();
            actividades_evaluadas = (DataTable)Session["actividades_evaluadas"];
            gridview_chequeos.DataSource = actividades_evaluadas;
            gridview_chequeos.DataBind();

            string promedio_local = historial.get_total_actividades_evaluadas(actividades_evaluadas);
            label_puntaje_promedio.Text = "Puntaje: " + historial.get_total_actividades_evaluadas(actividades_evaluadas);
        }
        private void primeras_ejecuciones()
        {
            label_fecha.Text = "fecha seleccionada: " + fecha_de_hoy.ToString("dd/MM/yyyy");
            Session.Add("fecha_historial_chequeo", fecha_de_hoy);
            Session.Add("fecha_pdf", fecha_de_hoy.ToString("dd/MM/yyyy"));
            observaciones = historial.get_observaciones(sucursal.Rows[0]["id"].ToString(), DateTime.Now);
            Session.Add("observaciones", observaciones);
            if (observaciones.Rows.Count != 0)
            {
                textbox_observaciones.Text = observaciones.Rows[0]["observacion"].ToString();

            }
            else
            {
                textbox_observaciones.Text = "";
            }
            lista_de_evaluadosBD = historial.get_lista_de_evaluados(sucursal.Rows[0]["id"].ToString(), fecha_de_hoy);
            historial_evaluacion_chequeo = lista_de_evaluadosBD;
            Session.Add("historial_evaluacion_chequeo", historial_evaluacion_chequeo);
            cargar_lista_evaluados();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_visita_operativa historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;
        DataTable empleado;
        DataTable lista_de_evaluadosBD;
        DataTable lista_de_evaluados;
        DataTable lista_de_chequeoBD;
        DataTable historial_evaluacion_chequeo;
        DataTable actividades_evaluadas;
        DataTable resumenBD;
        DataTable resumen;
        DataTable historial_chequeo;
        DataTable observaciones;
        DateTime fecha_de_hoy = DateTime.Now;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];

            historial = new cls_historial_visita_operativa(usuariosBD);
            if (Session["perfil_seleccionado"] == null)
            {
                Session.Add("perfil_seleccionado", "N/A");
            }
            lista_de_chequeoBD = historial.get_lista_de_chequeo();
            if (!IsPostBack)
            {
                if (Session["fecha_pdf"] == null)
                {
                    primeras_ejecuciones();
                }
                else
                {
                    if (Session["fecha_pdf"].ToString() == fecha_de_hoy.ToString("dd/MM/yyyy"))
                    {
                        primeras_ejecuciones();
                    }
                }

            }
        }

        protected void gridview_chequeos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id, idHistorial;
            int fila_historial;
            actividades_evaluadas = (DataTable)Session["actividades_evaluadas"];
            for (int fila = 0; fila <= gridview_chequeos.Rows.Count - 1; fila++)
            {
                id = gridview_chequeos.Rows[fila].Cells[0].Text;
                fila_historial = funciones.buscar_fila_por_id(id, actividades_evaluadas);
                if (actividades_evaluadas.Rows[fila]["punto_real"].ToString() == "0")
                {
                    gridview_chequeos.Rows[fila].CssClass = "table-danger";
                }
                else
                {
                    gridview_chequeos.Rows[fila].CssClass = "table-success";
                }

                // Obtén la ID del historial desde la fila (suponiendo que está en la columna 5)
                idHistorial = id;

                // Definir las extensiones de archivo que deseas verificar
                string[] fileExtensions = { ".jpg", ".png", ".gif", ".mp4", ".pdf" };

                // Ruta base donde se almacenan los archivos
                string folderPath = Server.MapPath("~/FotosSubidas/visitas_operativas/");

                // Variable para guardar si se encontró algún archivo
                bool fileExists = false;

                // Verificar si existe un archivo con alguna de las extensiones
                foreach (string extension in fileExtensions)
                {
                    string filePath = Path.Combine(folderPath, idHistorial + extension);
                    if (File.Exists(filePath))
                    {
                        fileExists = true;
                        break; // Si se encuentra el archivo, sal del bucle
                    }
                }

                // Configurar el botón o indicador si se encontró un archivo
                Button botonVerFoto = gridview_chequeos.Rows[fila].Cells[6].FindControl("boton_ver_foto") as Button;
                if (botonVerFoto != null)
                {
                    if (fileExists)
                    {
                        botonVerFoto.Visible = true; // Habilitar el botón si el archivo existe
                    }
                    else
                    {
                        botonVerFoto.Visible = false; // Deshabilitar si no existe
                    }
                }
            }


        }
        protected void boton_historial_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int fila = row.RowIndex;

            string id_empleado = gridview_empleados.Rows[fila].Cells[0].Text;
            string nombre = gridview_empleados.Rows[fila].Cells[1].Text;
            string apellido = gridview_empleados.Rows[fila].Cells[2].Text;
            empleado = historial.get_empleado(id_empleado);
            Session.Add("empleado_historial", empleado);
            DateTime fecha = (DateTime)Session["fecha_historial_chequeo"];

            historial_chequeo = (DataTable)Session["historial_evaluacion_chequeo"];

            Session.Add("historial_chequeo", historial_chequeo);
            if (historial_chequeo.Rows.Count > 0)
            {

                lista_de_chequeoBD = historial.get_lista_de_chequeo();
                label_empleado.Text = "Empleado: " + nombre + " " + apellido;
                label_fecha_historial.Text = "Fecha: " + fecha.ToString("dd/MM/yyyy");
                label_empleado.Visible = true;
                label_fecha_historial.Visible = true;
                label_puntaje_promedio.Text = "Puntaje: N/A";

                gridview_chequeos.Visible = true;

                label_alerta_registro.Visible = false;
                gridview_chequeos.DataSource = null;
                gridview_chequeos.DataBind();
                configurar_botones_cargos(historial.get_cargos_de_empleado(empleado, historial_chequeo));
            }
            else
            {
                label_empleado.Visible = false;
                label_fecha_historial.Visible = false;

                boton_encargado.Visible = false;
                boton_cajero.Visible = false;
                boton_shawarmero.Visible = false;
                boton_atencion.Visible = false;
                boton_cocina.Visible = false;
                boton_limpieza.Visible = false;
                gridview_chequeos.Visible = false;
                DateTime fecha_seleccionada = (DateTime)Session["fecha_historial_chequeo"];
                string nombre_empleado = empleado.Rows[0]["nombre"].ToString() + " " + empleado.Rows[0]["apellido"].ToString();

                label_alerta_registro.Text = "No hay registros del empleado " + nombre_empleado + " para la fecha " + fecha_seleccionada.ToString("dd/MM/yyyy");

                label_alerta_registro.Visible = true;
            }
        }

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha = calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            label_fecha.Text = "fecha seleccionada: " + fecha.ToString("dd/MM/yyyy");
            Session.Add("fecha_historial_chequeo", fecha);
            Session.Add("fecha_pdf", fecha.ToString("dd/MM/yyyy"));

            observaciones = historial.get_observaciones(sucursal.Rows[0]["id"].ToString(), fecha);
            Session.Add("observaciones", observaciones);
            if (observaciones.Rows.Count != 0)
            {
                textbox_observaciones.Text = observaciones.Rows[0]["observacion"].ToString();

            }
            else
            {
                textbox_observaciones.Text = "";
            }
            lista_de_evaluadosBD = historial.get_lista_de_evaluados(sucursal.Rows[0]["id"].ToString(), (DateTime)Session["fecha_historial_chequeo"]);
            historial_evaluacion_chequeo = lista_de_evaluadosBD;
            Session.Add("historial_evaluacion_chequeo", historial_evaluacion_chequeo);
            cargar_lista_evaluados();
            limpiar_resumen();
        }
        protected void boton_encargado_Click(object sender, EventArgs e)
        {
            string perfil = "Encargado";
            Session.Add("perfil_seleccionado", perfil);
            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cajero_Click(object sender, EventArgs e)
        {
            string perfil = "Cajero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);

            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_shawarmero_Click(object sender, EventArgs e)
        {
            string perfil = "Shawarmero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);

            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_atencion_Click(object sender, EventArgs e)
        {
            string perfil = "Atencion al Cliente";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);

            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cocina_Click(object sender, EventArgs e)
        {
            string perfil = "Cocina";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);

            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_limpieza_Click(object sender, EventArgs e)
        {
            string perfil = "Limpieza";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            actividades_evaluadas = historial.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString(), (DataTable)Session["empleado_historial"], (DataTable)Session["historial_evaluacion_chequeo"]);
            Session.Add("actividades_evaluadas", actividades_evaluadas);

            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " evaluacion lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            observaciones = (DataTable)Session["observaciones"];
            string observacion;
            if (observaciones.Rows.Count != 0)
            {
                observacion = observaciones.Rows[0]["observacion"].ToString();

            }
            else
            {
                observacion = "";
            }

            historial.crear_pdf_evaluacion(ruta_archivo, imgdata, (DataTable)Session["lista_de_evaluados"], (DataTable)Session["historial_evaluacion_chequeo"], sucursal.Rows[0]["sucursal"].ToString(), Session["fecha_pdf"].ToString(), Session["promedio_local"].ToString(), observacion);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginas/pdf/" + id_pedido;
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);

            }
            catch (Exception)
            {

                Response.Redirect(strUrl, false);
            }
        }

        protected void textbox_observaciones_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_observaciones = (TextBox)sender;

            observaciones = (DataTable)Session["observaciones"];

            if (observaciones.Rows.Count != 0)
            {
                string id_observacion = observaciones.Rows[0]["id"].ToString();

                historial.actualizar_observaciones(id_observacion, textbox_observaciones.Text);
            }
            else
            {
                historial.crear_observacion(sucursal, (DateTime)Session["fecha_historial_chequeo"], textbox_observaciones.Text);
            }


        }
    }
}