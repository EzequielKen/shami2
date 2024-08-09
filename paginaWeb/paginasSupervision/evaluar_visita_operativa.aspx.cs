using _02___sistemas;
using _07_sistemas_supervision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasSupervision
{
    public partial class visita_operativa : System.Web.UI.Page
    {
        [Serializable]
        public class MiClaseSerializable
        {
            public int Numero { get; set; }
            public string Texto { get; set; }
            // Otros miembros de la clase...
        }
        #region resumen

        private void crear_tabla_resumenBD()
        {
            resumenBD = new DataTable();
            resumenBD.Columns.Add("id", typeof(string));
            resumenBD.Columns.Add("actividad", typeof(string));
            resumenBD.Columns.Add("categoria", typeof(string));
            resumenBD.Columns.Add("nota", typeof(string));
            resumenBD.Columns.Add("area", typeof(string));
            resumenBD.Columns.Add("orden", typeof(int));
            Session.Add("resumen_chequeo", resumenBD);
        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
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
            Session.Add("resumen", resumen);
        }
        private void cargar_actividad_en_resumen(string id)
        {
            resumenBD = (DataTable)Session["resumen_chequeo"];
            int fila_actividad = funciones.buscar_fila_por_id(id, lista_de_chequeoBD);
            int fila_resumen = funciones.buscar_fila_por_id(id, resumenBD);
            if (fila_resumen == -1 && fila_actividad != -1)
            {
                resumenBD.Rows.Add();
                int ultima_fila = resumenBD.Rows.Count - 1;

                resumenBD.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila_actividad]["id"].ToString();
                resumenBD.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila_actividad]["actividad"].ToString();
                resumenBD.Rows[ultima_fila]["categoria"] = lista_de_chequeoBD.Rows[fila_actividad]["categoria"].ToString();
                resumenBD.Rows[ultima_fila]["area"] = lista_de_chequeoBD.Rows[fila_actividad]["area"].ToString();
                resumenBD.Rows[ultima_fila]["orden"] = int.Parse(lista_de_chequeoBD.Rows[fila_actividad]["orden"].ToString());
            }
            else if (fila_resumen != -1)
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
            configuracion = Visita.get_configuracion_de_chequeo(perfil);
            for (int fila = 0; fila <= configuracion.Rows.Count - 1; fila++)
            {
                id_actividad = configuracion.Rows[fila]["id"].ToString();
                cargar_actividad_en_resumen(id_actividad);
            }



        }
        #endregion
        #region configurar controles
        private void configurar_control_empleados()
        {
            lista_de_empleadoBD = (DataTable)Session["lista_de_empleadoBD"];
            for (int fila = 0; fila <= lista_de_empleadoBD.Rows.Count-1; fila++)
            {
                if (lista_de_empleadoBD.Rows[fila]["seleccionado"].ToString()=="1")
                {
                    dropdown_empleado.Items.Add(lista_de_empleadoBD.Rows[fila]["nombre"].ToString());
                }
            }
        }
        private void configurar_controles()
        {

            llenar_dropDownList(resumenBD);
            llenar_dropDownList_categiria(resumenBD, dropDown_tipo.SelectedItem.Text);
        }
        private void reinicar_botones_cargos()
        {
            boton_encargado.Visible = false;
            boton_atencion.Visible = false;
            boton_cajero.Visible = false;
            boton_cocina.Visible = false;
            boton_limpieza.Visible = false;
            boton_shawarmero.Visible = false;

        }
        private void configurar_botones_cargos(string cargos)
        {
            reinicar_botones_cargos();
            int iteraciones = int.Parse(funciones.obtener_dato(cargos, 1));
            int posicion = 2;
            string cargo;
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
        #endregion
        private void cargar_lista_chequeo()
        {


            crear_tabla_resumen();
            llenar_tabla_resumen_local();
            resumen.DefaultView.Sort = "orden asc";
            resumen = resumen.DefaultView.ToTable();
            gridview_chequeos.DataSource = resumen;
            gridview_chequeos.DataBind();

            registrar_todo();
            calcular_puntaje();
            historial_evaluacion = Visita.get_historial(DateTime.Now, empleado_lista_chequeo.Rows[0]["turno_logueado"].ToString(), empleado_lista_chequeo.Rows[0]["id"].ToString(), empleado_lista_chequeo.Rows[0]["id_sucursal"].ToString());
            historial_evaluacion = (DataTable)Session["historial_evaluacion"];
            if (historial_evaluacion.Rows.Count == 0)
            {
                cargar_lista_chequeo();
            }
        }
        private void registrar_chequeo(string id_actividad, string nota, string punto_teorico, string punto_real)
        {
            string actividad = id_actividad;//+ "-" + actividad;
            Visita.registrar_chequeo(empleado_lista_chequeo, actividad, nota, punto_teorico, punto_real);
        }
        private void modificar_chequeo(string id_actividad_historial, string punto_real)
        {
            Visita.actualizar_punto_real(id_actividad_historial, punto_real);
        }
        private void registrar_todo()
        {
            resumen = (DataTable)Session["resumen"];
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {

                string id_actividad = resumen.Rows[fila]["id"].ToString();
                string nota;
                nota = "N/A";
                historial_evaluacion = (DataTable)Session["historial_evaluacion"];
                int fila_actividad = funciones.buscar_fila_por_id(id_actividad, lista_de_chequeoBD);
                int fila_historial = funciones.buscar_fila_por_id(id_actividad, historial_evaluacion);
                string punto_teorico = lista_de_chequeoBD.Rows[fila_actividad]["punto"].ToString();
                string punto_real = punto_teorico;
                if (fila_historial == -1)
                {
                    registrar_chequeo(id_actividad, nota, punto_teorico, punto_real);
                }
            }
            
        }
        private void calcular_puntaje()
        {
            resumen = (DataTable)Session["resumen"];
            historial_evaluacion = (DataTable)Session["historial_evaluacion"];
            string id_actividad;
            int fila_historial;
            double puntaje = 0;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {

                id_actividad = resumen.Rows[fila]["id"].ToString();
                fila_historial = funciones.buscar_fila_por_id(id_actividad, historial_evaluacion);
                if (fila_historial != -1)
                {
                    puntaje = puntaje + double.Parse(historial_evaluacion.Rows[fila_historial]["punto_real"].ToString());
                }
            }
            label_puntaje.Text = "Puntaje: " + puntaje.ToString();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_evaluar_visita_operativa Visita;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD_lista_chequeo;
        DataTable empleado_lista_chequeo;
        DataTable sucursal_lista_chequeo;

        DataTable lista_de_empleadoBD;
        DataTable lista_de_chequeoBD;
        DataTable configuracion;
        DataTable resumenBD;
        DataTable resumen;
        DataTable historial_evaluacion;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Add("usuariosBD_lista_chequeo", (DataTable)Session["usuariosBD"]);
                Session.Add("sucursal_lista_chequeo", (DataTable)Session["sucursal"]);
            }
            
            usuariosBD_lista_chequeo = (DataTable)Session["usuariosBD_lista_chequeo"];
            Visita = new cls_evaluar_visita_operativa(usuariosBD_lista_chequeo);
          
            if (!IsPostBack)
            {
                configurar_control_empleados();
                lista_de_empleadoBD = (DataTable)Session["lista_de_empleadoBD"];
                int fila = funciones.buscar_fila_empleado_por_nombre(dropdown_empleado.SelectedItem.Text, lista_de_empleadoBD);
                Session.Add("empleado", Visita.get_empleado(lista_de_empleadoBD.Rows[fila]["id"].ToString()));
                Session.Add("empleado_lista_chequeo", (DataTable)Session["empleado"]);
            }
            
            empleado_lista_chequeo = (DataTable)Session["empleado_lista_chequeo"];
            sucursal_lista_chequeo = (DataTable)Session["sucursal_lista_chequeo"];
            if (Session["perfil_seleccionado"] == null)
            {
                Session.Add("perfil_seleccionado", "N/A");
            }
            lista_de_chequeoBD = Visita.get_lista_de_chequeo();

            string nombre = empleado_lista_chequeo.Rows[0]["nombre"].ToString();
            string apellido = empleado_lista_chequeo.Rows[0]["apellido"].ToString();
            string cargos = empleado_lista_chequeo.Rows[0]["cargo"].ToString();
            label_nombre.Text = "Empleado: " + nombre + " " + apellido;
            label_fecha.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyyy");

            configurar_botones_cargos(cargos);

        }

        protected void gridview_chequeos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id;
            int fila_historial;
            empleado_lista_chequeo = (DataTable)Session["empleado"];
            historial_evaluacion = Visita.get_historial(DateTime.Now, empleado_lista_chequeo.Rows[0]["turno_logueado"].ToString(), empleado_lista_chequeo.Rows[0]["id"].ToString(), empleado_lista_chequeo.Rows[0]["id_sucursal"].ToString());
            Session.Add("historial_evaluacion", historial_evaluacion);
            for (int fila = 0; fila <= gridview_chequeos.Rows.Count - 1; fila++)
            {
                id = gridview_chequeos.Rows[fila].Cells[0].Text;
                fila_historial = funciones.buscar_fila_por_id(id, historial_evaluacion);
                if (fila_historial != -1)
                {
                    Button boton_cargar = (gridview_chequeos.Rows[fila].Cells[0].FindControl("boton_cargar") as Button);
                    if (historial_evaluacion.Rows[fila_historial]["punto_real"].ToString() != "0")
                    {
                        gridview_chequeos.Rows[fila].CssClass = "table-success";
                        gridview_chequeos.Rows[fila].Cells[4].Text = historial_evaluacion.Rows[fila_historial]["nota"].ToString();
                        boton_cargar.CssClass = "btn btn-danger";
                        boton_cargar.Text = "Desmarcar";
                    }
                    else
                    {
                        gridview_chequeos.Rows[fila].CssClass = "table-danger";
                        gridview_chequeos.Rows[fila].Cells[4].Text = historial_evaluacion.Rows[fila_historial]["nota"].ToString();
                        boton_cargar.CssClass = "btn btn-primary";
                        boton_cargar.Text = "Marcar";
                    }
                    gridview_chequeos.Rows[fila].Cells[5].Text = historial_evaluacion.Rows[fila_historial]["id_historial"].ToString();
                    gridview_chequeos.Rows[fila].Cells[6].Text = historial_evaluacion.Rows[fila_historial]["punto_teorico"].ToString();
                    gridview_chequeos.Rows[fila].Cells[7].Text = historial_evaluacion.Rows[fila_historial]["punto_real"].ToString();

                }
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

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            Button boton_cargar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_cargar.NamingContainer;
            int fila = row.RowIndex;
            TextBox textbox_nota = (gridview_chequeos.Rows[fila].Cells[2].FindControl("textbox_nota") as TextBox);

            string id_actividad = gridview_chequeos.Rows[fila].Cells[0].Text;
            string id_actividad_historial = gridview_chequeos.Rows[fila].Cells[5].Text;
            string nota;
            if (textbox_nota.Text == string.Empty)
            {
                nota = "N/A";
            }
            else
            {
                nota = textbox_nota.Text;
            }
            historial_evaluacion = (DataTable)Session["historial_evaluacion"];
            int fila_actividad = funciones.buscar_fila_por_id(id_actividad, lista_de_chequeoBD);
            int fila_historial = funciones.buscar_fila_por_id(id_actividad, historial_evaluacion);
            string punto_teorico = lista_de_chequeoBD.Rows[fila_actividad]["punto"].ToString();
            string punto_real = punto_teorico;
            if (fila_historial != -1)
            {
                if (historial_evaluacion.Rows[fila_historial]["punto_real"].ToString() != "0")
                {
                    punto_real = "0";
                }
                else
                {
                    punto_real = punto_teorico;
                }
            }
            modificar_chequeo(id_actividad_historial, punto_real);
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());

            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            cargar_lista_chequeo();
        }

        protected void boton_encargado_Click(object sender, EventArgs e)
        {
            string perfil = "Encargado";
            Session.Add("perfil_seleccionado", perfil);
            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cajero_Click(object sender, EventArgs e)
        {
            string perfil = "Cajero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_shawarmero_Click(object sender, EventArgs e)
        {
            string perfil = "Shawarmero";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_atencion_Click(object sender, EventArgs e)
        {
            string perfil = "Atencion al Cliente";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_cocina_Click(object sender, EventArgs e)
        {
            string perfil = "Cocina";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void boton_limpieza_Click(object sender, EventArgs e)
        {
            string perfil = "Limpieza";
            Session.Add("perfil_seleccionado", perfil);

            configurar_botones_cargos_activos(Session["perfil_seleccionado"].ToString());
            configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());
            llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());
            configurar_controles();
            cargar_lista_chequeo();
        }

        protected void textbox_nota_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_nota = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_nota.NamingContainer;
            int fila = row.RowIndex;

            string id_actividad = gridview_chequeos.Rows[fila].Cells[5].Text;
            if (id_actividad != string.Empty)
            {
                string nota;
                if (textbox_nota.Text == string.Empty)
                {
                    nota = "N/A";
                }
                else
                {
                    nota = textbox_nota.Text;
                }
                Visita.actualizar_nota(id_actividad, nota);
                configuracion = Visita.get_configuracion_de_chequeo(Session["perfil_seleccionado"].ToString());

                llenar_resumen_con_configuracion(Session["perfil_seleccionado"].ToString());

                cargar_lista_chequeo();
            }

        }

        protected void boton_cerrar_turno_Click(object sender, EventArgs e)
        {
            Session.Add("empleado", Visita.cerrar_turno((DataTable)Session["empleado"], sucursal_lista_chequeo.Rows[0]["sucursal"].ToString()));
            Response.Redirect("~/paginas/lista_de_chequeo.aspx", false);
        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf(ruta_archivo, imgdata, lista_de_chequeoBD);
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

        protected void boton_encargado_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "ADMINISTRACION", "Encargado");
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

        protected void boton_cajero_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "CAJERO", "Operaciones");
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

        protected void boton_shawarmero_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "SHAWARMERO", "Operaciones");
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

        protected void boton_atencion_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "AREA DE SERVICIO", "Operaciones");
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

        protected void boton_cocina_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "COCINA", "Operaciones");
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

        protected void boton_limpieza_pdf_Click(object sender, EventArgs e)
        {
            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " lista chequeo - id-" + dato_hora + ".pdf";
            string ruta = "/paginas/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));
            string ruta_logo = "~/imagenes/logo-completo.png";
            Visita.crear_pdf_segun_categoria(ruta_archivo, imgdata, lista_de_chequeoBD, "LIMPIEZA", "Operaciones");
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

        protected void dropdown_empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            lista_de_empleadoBD = (DataTable)Session["lista_de_empleadoBD"];
            int fila = funciones.buscar_fila_empleado_por_nombre(dropdown_empleado.SelectedItem.Text, lista_de_empleadoBD);
            Session.Add("empleado", Visita.get_empleado(lista_de_empleadoBD.Rows[fila]["id"].ToString()));
            Session.Add("empleado_lista_chequeo", (DataTable)Session["empleado"]);

            empleado_lista_chequeo = (DataTable)Session["empleado_lista_chequeo"];
            sucursal_lista_chequeo = (DataTable)Session["sucursal_lista_chequeo"];
            if (Session["perfil_seleccionado"] == null)
            {
                Session.Add("perfil_seleccionado", "N/A");
            }
            lista_de_chequeoBD = Visita.get_lista_de_chequeo();

            string nombre = empleado_lista_chequeo.Rows[0]["nombre"].ToString();
            string apellido = empleado_lista_chequeo.Rows[0]["apellido"].ToString();
            string cargos = empleado_lista_chequeo.Rows[0]["cargo"].ToString();
            label_nombre.Text = "Empleado: " + nombre + " " + apellido;
            label_fecha.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyyy");

            configurar_botones_cargos(cargos);
            gridview_chequeos.DataSource = null;
            gridview_chequeos.DataBind();
            label_puntaje.Text = "";
        }
    }
}