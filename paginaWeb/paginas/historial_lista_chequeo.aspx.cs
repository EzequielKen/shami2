using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class historial_lista_chequeo : System.Web.UI.Page
    {
        #region carga empleado
        private void crear_lista_de_empleado()
        {
            lista_de_empleado = new DataTable();
            lista_de_empleado.Columns.Add("id", typeof(string));
            lista_de_empleado.Columns.Add("id_sucursal", typeof(string));
            lista_de_empleado.Columns.Add("nombre", typeof(string));
            lista_de_empleado.Columns.Add("apellido", typeof(string));
            lista_de_empleado.Columns.Add("dni", typeof(string));
            lista_de_empleado.Columns.Add("telefono", typeof(string));
            lista_de_empleado.Columns.Add("cargo", typeof(string));
        }
        private void llenar_tabla_empleado()
        {
            crear_lista_de_empleado();
            int ultima_fila = 0;
            for (int fila = 0; fila <= lista_de_empleadoBD.Rows.Count - 1; fila++)
            {
                if (dropdown_turno.SelectedItem.Text == historial.verificar_horario_turno((DateTime)lista_de_empleadoBD.Rows[fila]["fecha_logueo"]) ||
                    dropdown_turno.SelectedItem.Text == "Todos")
                {
                    if (!verificar_si_cargo(lista_de_empleadoBD.Rows[fila]["id_empleado"].ToString()))
                    {
                        lista_de_empleado.Rows.Add();
                        ultima_fila = lista_de_empleado.Rows.Count - 1;
                        lista_de_empleado.Rows[ultima_fila]["id"] = lista_de_empleadoBD.Rows[fila]["id_empleado"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["id_sucursal"] = lista_de_empleadoBD.Rows[fila]["id_sucursal"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["nombre"] = lista_de_empleadoBD.Rows[fila]["nombre"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["apellido"] = lista_de_empleadoBD.Rows[fila]["apellido"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["dni"] = lista_de_empleadoBD.Rows[fila]["dni"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["telefono"] = lista_de_empleadoBD.Rows[fila]["telefono"].ToString();
                        lista_de_empleado.Rows[ultima_fila]["cargo"] = lista_de_empleadoBD.Rows[fila]["cargo"].ToString();
                    }
                }
            }
        }
        private bool verificar_si_cargo(string id_empleado)
        {
            bool retorno = false;
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                if (id_empleado == lista_de_empleado.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
            }
            return retorno;
        }
        private void cargar_lista_empleados()
        {
            llenar_tabla_empleado();
            gridview_empleados.DataSource = lista_de_empleado;
            gridview_empleados.DataBind();
        }
        #endregion

        #region resumen
        private void limpiar_resumen()
        {
            label_empleado.Visible = false;
            label_fecha_historial.Visible = false;
            label_area.Visible = false;
            label_categoria.Visible = false;
            dropDown_tipo.Visible = false;
            dropDown_categoria.Visible = false;

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
        private void crear_tabla_resumenBD()
        {
            resumenBD = new DataTable();
            resumenBD.Columns.Add("id", typeof(string));
            resumenBD.Columns.Add("actividad", typeof(string));
            resumenBD.Columns.Add("categoria", typeof(string));
            resumenBD.Columns.Add("area", typeof(string));
            resumenBD.Columns.Add("nota", typeof(string));
            resumenBD.Columns.Add("fecha", typeof(string));
            resumenBD.Columns.Add("orden", typeof(int));
            Session.Add("resumen_chequeo", resumenBD);
        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("area", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
            resumen.Columns.Add("fecha", typeof(string));
            resumen.Columns.Add("orden", typeof(int));
            Session.Add("resumen_chequeo_local", resumen);
        }
        private void llenar_tabla_resumen_local()
        {
            resumenBD = (DataTable)Session["resumen_chequeo"];

            crear_tabla_resumen();
            int ultima_fila;
            for (int fila = 0; fila <= resumenBD.Rows.Count - 1; fila++)
            {
                if (resumenBD.Rows[fila]["categoria"].ToString() == dropDown_categoria.SelectedItem.Text)
                {
                    resumen.Rows.Add();
                    ultima_fila = resumen.Rows.Count - 1;
                    resumen.Rows[ultima_fila]["id"] = resumenBD.Rows[fila]["id"].ToString();
                    resumen.Rows[ultima_fila]["actividad"] = resumenBD.Rows[fila]["actividad"].ToString();
                    resumen.Rows[ultima_fila]["orden"] = int.Parse(resumenBD.Rows[fila]["orden"].ToString());
                }
            }
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
        private void cargar_toda_la_categoria()
        {
            string area = dropDown_tipo.SelectedItem.Text;
            string categoria = dropDown_categoria.SelectedItem.Text;

            for (int fila = 0; fila <= lista_de_chequeoBD.Rows.Count - 1; fila++)
            {
                if (lista_de_chequeoBD.Rows[fila]["area"].ToString() == area &&
                    lista_de_chequeoBD.Rows[fila]["categoria"].ToString() == categoria)
                {
                    cargar_actividad_en_resumen_todo(lista_de_chequeoBD.Rows[fila]["id"].ToString());
                }
            }
        }
        private void llenar_resumen_con_configuracion(string perfil)
        {
            crear_tabla_resumenBD();
            string id_actividad;
            configuracion = historial.get_configuracion_de_chequeo(perfil);
            for (int fila = 0; fila <= configuracion.Rows.Count - 1; fila++)
            {
                id_actividad = configuracion.Rows[fila]["id"].ToString();
                cargar_actividad_en_resumen(id_actividad);
            }
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            empleado = (DataTable)Session["empleado_historial"];

            string nombre = empleado.Rows[0]["nombre"].ToString();
            string apellido = empleado.Rows[0]["apellido"].ToString();
            string cargo = empleado.Rows[0]["cargo"].ToString();


            llenar_dropDownList(resumenBD);
            llenar_dropDownList_categiria(resumenBD, dropDown_tipo.SelectedItem.Text);
        }
        private void llenar_dropDownList_categiria(DataTable dt, string area)
        {
            dropDown_categoria.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "area asc, categoria asc";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["categoria"].ToString();
            if (dt.Rows[0]["area"].ToString() != area)
            {

                for (int fil = 0; fil <= dt.Rows.Count - 1; fil++)
                {
                    if (dt.Rows[fil]["area"].ToString() == area)
                    {
                        item = new ListItem(dt.Rows[fil]["categoria"].ToString(), num_item.ToString());
                        dropDown_categoria.Items.Add(item);
                        break;
                    }
                }
            }
            else
            {
                item = new ListItem(dt.Rows[0]["categoria"].ToString(), num_item.ToString());
                dropDown_categoria.Items.Add(item);
            }

            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_categoria.Items[num_item - 2].Text != dt.Rows[fila]["categoria"].ToString() &&
                    dt.Rows[fila]["area"].ToString() == area)
                {
                    item = new ListItem(dt.Rows[fila]["categoria"].ToString(), num_item.ToString());
                    dropDown_categoria.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "area";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["area"].ToString();
            item = new ListItem(dt.Rows[0]["area"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["area"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["area"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
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

            for (int index = 0; index <= iteraciones; index++)
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


            crear_tabla_resumen();
            llenar_tabla_resumen_local();
            resumen.DefaultView.Sort = "orden asc";
            resumen = resumen.DefaultView.ToTable();
            gridview_chequeos.DataSource = resumen;
            gridview_chequeos.DataBind();


        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_lista_chequeo historial;
        cls_lista_de_chequeo lista_chequeo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;
        DataTable lista_de_empleadoBD;
        DataTable lista_de_empleado;
        DataTable lista_de_chequeoBD;
        DataTable configuracion;
        DataTable resumenBD;
        DataTable resumen;
        DataTable historial_chequeo;

        DateTime fecha_de_hoy = DateTime.Now;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["historial_de_chequeo"] == null)
            {
                Session.Add("historial_de_chequeo", new cls_historial_lista_chequeo(usuariosBD));
            }
            historial = (cls_historial_lista_chequeo)Session["historial_de_chequeo"];
            if (Session["lista_chequeo"] == null)
            {
                Session.Add("lista_chequeo", new cls_lista_de_chequeo(usuariosBD));
            }
            if (Session["perfil_seleccionado"] == null)
            {
                Session.Add("perfil_seleccionado", "N/A");
            }
            lista_chequeo = (cls_lista_de_chequeo)Session["lista_chequeo"];
            lista_de_chequeoBD = historial.get_lista_de_chequeo();
            if (!IsPostBack)
            {

                label_fecha.Text = "fecha seleccionada: " + fecha_de_hoy.ToString("dd/MM/yyyy");
                Session.Add("fecha_historial_chequeo", fecha_de_hoy);
                lista_de_empleadoBD = historial.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString(), fecha_de_hoy);
                cargar_lista_empleados();
            }
        }
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            resumenBD = (DataTable)Session["resumen_chequeo"];

            llenar_dropDownList_categiria(resumenBD, dropDown_tipo.SelectedItem.Text);

            cargar_lista_chequeo();
        }

        protected void dropDown_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_lista_chequeo();
        }

        protected void gridview_chequeos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id;
            int fila_historial;
            empleado = (DataTable)Session["empleado_historial"];
            historial_chequeo = (DataTable)Session["historial_chequeo"];
            for (int fila = 0; fila <= gridview_chequeos.Rows.Count - 1; fila++)
            {
                id = gridview_chequeos.Rows[fila].Cells[0].Text;
                fila_historial = funciones.buscar_fila_por_id(id, historial_chequeo);
                if (fila_historial != -1)
                {
                    gridview_chequeos.Rows[fila].CssClass = "table-success";
                    gridview_chequeos.Rows[fila].Cells[2].Text = historial_chequeo.Rows[fila_historial]["nota"].ToString();
                    gridview_chequeos.Rows[fila].Cells[3].Text = historial_chequeo.Rows[fila_historial]["fecha"].ToString();

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
            if (dropdown_turno.SelectedItem.Text != "Todos")
            {
                historial_chequeo = lista_chequeo.get_historial(fecha, dropdown_turno.SelectedItem.Text, empleado.Rows[0]["id"].ToString(), empleado.Rows[0]["id_sucursal"].ToString());

            }
            else
            {
                DropDownList dropdown_turno_empleado = (gridview_empleados.Rows[fila].Cells[4].FindControl("dropdown_turno_empleado") as DropDownList);

                historial_chequeo = lista_chequeo.get_historial(fecha, dropdown_turno_empleado.SelectedItem.Text, empleado.Rows[0]["id"].ToString(), empleado.Rows[0]["id_sucursal"].ToString());

            }
            Session.Add("historial_chequeo", historial_chequeo);
            if (historial_chequeo.Rows.Count > 0)
            {

                lista_de_chequeoBD = historial.get_lista_de_chequeo();
                label_empleado.Text = "Empleado: " + nombre + " " + apellido;
                label_fecha_historial.Text = "Fecha: " + fecha.ToString("dd/MM/yyyy");
                label_empleado.Visible = true;
                label_fecha_historial.Visible = true;
                label_area.Visible = true;
                label_categoria.Visible = true;
                dropDown_tipo.Visible = true;
                dropDown_categoria.Visible = true;
                gridview_chequeos.Visible = true;

                label_alerta_registro.Visible = false;
                gridview_chequeos.DataSource = null;
                gridview_chequeos.DataBind();
                configurar_botones_cargos(historial_chequeo.Rows[0]["cargo"].ToString());
            }
            else
            {
                label_empleado.Visible = false;
                label_fecha_historial.Visible = false;
                label_area.Visible = false;
                label_categoria.Visible = false;
                dropDown_tipo.Visible = false;
                dropDown_categoria.Visible = false;

                boton_encargado.Visible = false;
                boton_cajero.Visible = false;
                boton_shawarmero.Visible = false;
                boton_atencion.Visible = false;
                boton_cocina.Visible = false;
                boton_limpieza.Visible = false;
                gridview_chequeos.Visible = false;
                DateTime fecha_seleccionada = (DateTime)Session["fecha_historial_chequeo"];
                string nombre_empleado = empleado.Rows[0]["nombre"].ToString() + " " + empleado.Rows[0]["apellido"].ToString();
                if (dropdown_turno.SelectedItem.Text != "Todos")
                {
                    label_alerta_registro.Text = "No hay registros del empleado " + nombre_empleado + " para la fecha " + fecha_seleccionada.ToString("dd/MM/yyyy") + " en el " + dropdown_turno.SelectedItem.Text;

                }
                else
                {
                    DropDownList dropdown_turno_empleado = (gridview_empleados.Rows[fila].Cells[4].FindControl("dropdown_turno_empleado") as DropDownList);

                    label_alerta_registro.Text = "No hay registros del empleado " + nombre_empleado + " para la fecha " + fecha_seleccionada.ToString("dd/MM/yyyy") + " en el " + dropdown_turno_empleado.SelectedItem.Text;

                }
                label_alerta_registro.Visible = true;
            }
        }

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (dropdown_turno.SelectedItem.Text != "Todos")
            {
                gridview_empleados.Columns[4].Visible = false;
            }
            else
            {
                gridview_empleados.Columns[4].Visible = true;
            }
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha = calendario.SelectedDate.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            label_fecha.Text = "fecha seleccionada: " + fecha.ToString("dd/MM/yyyy");
            Session.Add("fecha_historial_chequeo", fecha);
            lista_de_empleadoBD = historial.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString(), (DateTime)Session["fecha_historial_chequeo"]);
            cargar_lista_empleados();
            limpiar_resumen();
        }
        protected void boton_encargado_Click(object sender, EventArgs e)
        {
            string perfil = "Encargado";
            Session.Add("perfil_seleccionado", perfil);
            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cajero_Click(object sender, EventArgs e)
        {
            string perfil = "Cajero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_shawarmero_Click(object sender, EventArgs e)
        {
            string perfil = "Shawarmero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_atencion_Click(object sender, EventArgs e)
        {
            string perfil = "Atencion al Cliente";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cocina_Click(object sender, EventArgs e)
        {
            string perfil = "Cocina";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_limpieza_Click(object sender, EventArgs e)
        {
            string perfil = "Limpieza";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = lista_chequeo.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void dropdown_turno_SelectedIndexChanged(object sender, EventArgs e)
        {
            lista_de_empleadoBD = historial.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString(), (DateTime)Session["fecha_historial_chequeo"]);
            cargar_lista_empleados();
            limpiar_resumen();
        }
    }
}
