using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class planificador_de_horarios : System.Web.UI.Page
    {
        #region cargar empleados
        private void generar_columna_segun_rango()
        {
            DateTime fecha_inicio_dato = DateTime.Parse(textbox_fecha_inicio.Text);
            DateTime fecha_fin_dato = DateTime.Parse(textbox_fecha_fin.Text);
            if (fecha_inicio_dato <= fecha_fin_dato)
            {
                fecha_inicio = fecha_inicio_dato;
                fecha_fin = fecha_fin_dato;
                horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
                Session["horarios_de_empleados"] = horarios_de_empleados;
                GenerarColumnasDinamicas();  // Llamada a la generación de columnas dinámicas
            }
        }

        private void cargar_empleados()
        {
            empleados = (DataTable)Session["lista_empleados"];

            gridview_empleados.DataSource = empleados;
            gridview_empleados.DataBind();
        }
        #endregion

        #region atributos
        cls_planificador_de_horarios planificador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;

        DataTable empleados;
        DataTable horarios_de_empleados;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            planificador = new cls_planificador_de_horarios(usuariosBD);

            if (!IsPostBack)
            {
                fecha_inicio = DateTime.Now;
                fecha_fin = DateTime.Now.AddDays(6);
                textbox_fecha_inicio.Text = fecha_inicio.ToString("yyyy-MM-dd");
                textbox_fecha_fin.Text = fecha_fin.ToString("yyyy-MM-dd");
                horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
                Session["horarios_de_empleados"] = horarios_de_empleados;
                empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
                Session["lista_empleados"] = empleados;
                generar_columna_segun_rango();
                cargar_empleados();
            }
            else
            {
                // Recrear los controles dinámicos en cada postback
                // generar_columna_segun_rango();
                cargar_empleados();
            }
        }

        #region rango de fechas
        protected void textbox_fecha_inicio_TextChanged(object sender, EventArgs e)
        {
            horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
            Session["horarios_de_empleados"] = horarios_de_empleados;
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            Session["lista_empleados"] = empleados;
            generar_columna_segun_rango();
            cargar_empleados();
        }

        protected void textbox_fecha_fin_TextChanged(object sender, EventArgs e)
        {
            horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
            Session["horarios_de_empleados"] = horarios_de_empleados;
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            Session["lista_empleados"] = empleados;
            generar_columna_segun_rango();
            cargar_empleados();
        }
        #endregion

        #region generar columnas

        private void GenerarColumnasDinamicas()
        {
            // Captura las fechas de los TextBox
            fecha_inicio = DateTime.Parse(textbox_fecha_inicio.Text);
            fecha_fin = DateTime.Parse(textbox_fecha_fin.Text);

            // Limpia las columnas actuales
            gridview_empleados.Columns.Clear();

            // Agrega las columnas fijas (id, nombre, apellido)
            gridview_empleados.Columns.Add(new BoundField { HeaderText = "id", DataField = "id" });
            gridview_empleados.Columns.Add(new BoundField { HeaderText = "Empleado", DataField = "nombre_completo" });
            gridview_empleados.Columns.Add(new BoundField { HeaderText = "Cargo", DataField = "cargo_original" });

            // Genera las columnas dinámicas solo una vez por fecha
            for (DateTime fecha = fecha_inicio; fecha <= fecha_fin; fecha = fecha.AddDays(1))
            {
                string fechaDia = fecha.ToString("dd-MM-yyyy"); // Formato de fecha
                string columnaHeaderText = $"{fechaDia}";

                // Crea una nueva columna BoundField
                BoundField columna = new BoundField
                {
                    HeaderText = columnaHeaderText
                };

                // Agrega la columna al GridView
                gridview_empleados.Columns.Add(columna);
            }
        }
        #endregion

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id_empleado;
            DateTime fecha;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                id_empleado = e.Row.Cells[0].Text; // El ID del empleado está en la primera columna

                for (int columna_index = 3; columna_index <= gridview_empleados.Columns.Count - 1; columna_index++)
                {
                    // Obtener la fecha desde el encabezado de la columna
                    string fechaDia = gridview_empleados.Columns[columna_index].HeaderText;
                    fecha = DateTime.ParseExact(fechaDia, "dd-MM-yyyy", null);

                    // Crea el <h1> y el Label
                    HtmlGenericControl heading = new HtmlGenericControl("h3");
                    Label label = new Label
                    {
                        ID = $"label_{fecha}_{fecha.DayOfWeek.ToString()}_{id_empleado}",
                        CssClass = "badge rounded-pill text-bg-primary",
                        Text = fecha.ToString("dddd dd/MM/yyyy", new CultureInfo("es-ES"))
                    };

                    // Crear los controles para esta celda
                    TextBox textboxEntrada = new TextBox
                    {
                        ID = $"{fecha.ToString("dd-MM-yyyy")}-entrada-{id_empleado}",
                        CssClass = "form-control",
                        TextMode = TextBoxMode.Time,
                        AutoPostBack = true
                    };
                    textboxEntrada.TextChanged += TextboxEntrada_TextChanged;
                    textboxEntrada.AutoPostBack = true;

                    TextBox textboxSalida = new TextBox
                    {
                        ID = $"{fecha.ToString("dd-MM-yyyy")}-salida-{id_empleado}",
                        CssClass = "form-control",
                        TextMode = TextBoxMode.Time,
                        AutoPostBack = true
                    };
                    textboxSalida.TextChanged += TextboxSalida_TextChanged;
                    textboxSalida.AutoPostBack = true;
                    CheckBox checkbox = new CheckBox
                    {
                        ID = $"{fecha.ToString("dd-MM-yyyy")}-franco-{id_empleado}",
                        CssClass = "form-control",
                        Text = "Franco"
                    };
                    checkbox.CheckedChanged += Checkbox_CheckedChanged;
                    checkbox.AutoPostBack = true;
                    // Añadir los controles a la celda
                    // e.Row.Cells[columna_index].Controls.Add(label);
                    heading.Controls.Add(label);
                    e.Row.Cells[columna_index].Controls.Add(heading);
                    e.Row.Cells[columna_index].Controls.Add(new Literal { Text = "Entrada: " });
                    e.Row.Cells[columna_index].Controls.Add(textboxEntrada);
                    e.Row.Cells[columna_index].Controls.Add(new Literal { Text = "<br />Salida: " });
                    e.Row.Cells[columna_index].Controls.Add(textboxSalida);
                    e.Row.Cells[columna_index].Controls.Add(new Literal { Text = "<br />" });
                    e.Row.Cells[columna_index].Controls.Add(checkbox);
                }
            }
            //// Implementa cualquier lógica adicional que necesites durante el enlace de datos en cada fila
            horarios_de_empleados = (DataTable)Session["horarios_de_empleados"];
            int fila_horario;
            for (int fila = 0; fila <= gridview_empleados.Rows.Count - 1; fila++)
            {
                id_empleado = gridview_empleados.Rows[fila].Cells[0].Text;
                for (int columna = 3; columna <= gridview_empleados.Columns.Count - 1; columna++)
                {
                    fecha = DateTime.Parse(gridview_empleados.Columns[columna].HeaderText);
                    fila_horario = buscar_fila_horario(id_empleado, fecha.ToString("yyyy-MM-dd"));
                    if (fila_horario == -1)
                    {
                        gridview_empleados.Rows[fila].Cells[columna].CssClass = "table table-danger table-striped gridview-custom";
                    }
                    else
                    {
                        TextBox textbox_entrada = (gridview_empleados.Rows[fila].Cells[columna].FindControl($"{fecha.ToString("dd-MM-yyyy")}-entrada-{id_empleado}") as TextBox);
                        TextBox textbox_salida = (gridview_empleados.Rows[fila].Cells[columna].FindControl($"{fecha.ToString("dd-MM-yyyy")}-salida-{id_empleado}") as TextBox);
                        CheckBox checbox = (gridview_empleados.Rows[fila].Cells[columna].FindControl($"{fecha.ToString("dd-MM-yyyy")}-franco-{id_empleado}") as CheckBox);

                        if (horarios_de_empleados.Rows[fila_horario]["franco"].ToString() == "Si")
                        {
                            gridview_empleados.Rows[fila].Cells[columna].CssClass = "table table-warning table-striped gridview-custom";
                            textbox_entrada.Visible=false;
                            textbox_salida.Visible=false;
                        }
                        else
                        {
                            gridview_empleados.Rows[fila].Cells[columna].CssClass = "table table-success table-striped gridview-custom";
                            textbox_entrada.Visible = true;
                            textbox_salida.Visible = true;
                        }
                        DateTime hora_entrada = DateTime.Parse(horarios_de_empleados.Rows[fila_horario]["horario_entrada"].ToString());
                        DateTime hora_salida = DateTime.Parse(horarios_de_empleados.Rows[fila_horario]["horario_salida"].ToString());

                        textbox_entrada.Text = hora_entrada.ToString("HH:mm");
                        textbox_salida.Text = hora_salida.ToString("HH:mm");
                        if (horarios_de_empleados.Rows[fila_horario]["franco"].ToString() == "Si")
                        {
                            checbox.Checked = true;
                        }
                        else
                        {
                            checbox.Checked = false;
                        }
                    }
                }
            }
        }



        private int buscar_fila_horario(string id_empleado, string fecha)
        {
            int retorno = -1;
            int fila = 0;
            DateTime fecha_dato;
            while (fila <= horarios_de_empleados.Rows.Count - 1)
            {
                fecha_dato = DateTime.Parse(horarios_de_empleados.Rows[fila]["fecha"].ToString());
                string dato = fecha_dato.ToString("yyyy-MM-dd");
                if (id_empleado == horarios_de_empleados.Rows[fila]["id_empleado"].ToString() &&
                    fecha == dato)
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        // Funciones movidas a planificador_de_horarios

        private void TextboxEntrada_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxEntrada = (TextBox)sender;
            GridViewRow row = (GridViewRow)textboxEntrada.NamingContainer;
            string id_empleado = row.Cells[0].Text;

            string fecha_dato = funciones.obtener_dato(textboxEntrada.ID, 1);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(textboxEntrada.ID, 2);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(textboxEntrada.ID, 3);

            DateTime fecha = DateTime.Parse(fecha_dato);
            string hora = textboxEntrada.Text;
            string condicion = "entrada";
            string franco = "N/A";
            modificar_horario_empleado(sucursal.Rows[0]["id"].ToString(), id_empleado, fecha, hora, condicion, franco);

            // Recrear controles para que el GridView mantenga su estado
            horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
            Session["horarios_de_empleados"] = horarios_de_empleados;
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            Session["lista_empleados"] = empleados;
            generar_columna_segun_rango();
            cargar_empleados();
        }

        private void TextboxSalida_TextChanged(object sender, EventArgs e)
        {
            // Lógica a ejecutar cuando el texto del TextBox de Hora Salida cambia
            TextBox textboxSalida = (TextBox)sender;
            GridViewRow row = (GridViewRow)textboxSalida.NamingContainer;
            string id_empleado = row.Cells[0].Text;

            string fecha_dato = funciones.obtener_dato(textboxSalida.ID, 1);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(textboxSalida.ID, 2);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(textboxSalida.ID, 3);

            DateTime fecha = DateTime.Parse(fecha_dato);
            string hora = textboxSalida.Text;
            string condicion = "salida";
            string franco = "N/A";
            modificar_horario_empleado(sucursal.Rows[0]["id"].ToString(), id_empleado, fecha, hora, condicion, franco);

            // Recrear controles para que el GridView mantenga su estado
            horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
            Session["horarios_de_empleados"] = horarios_de_empleados;
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            Session["lista_empleados"] = empleados;
            generar_columna_segun_rango();
            cargar_empleados();
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            string id_empleado = row.Cells[0].Text;

            string fecha_dato = funciones.obtener_dato(checkbox.ID, 1);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(checkbox.ID, 2);
            fecha_dato = fecha_dato + "-" + funciones.obtener_dato(checkbox.ID, 3);

            DateTime fecha = DateTime.Parse(fecha_dato);
            string condicion = "franco";
            string franco = "N/A";
            if (checkbox.Checked)
            {
                franco = "Si";
            }
            else
            {
                condicion = "franco no";
            }
            modificar_horario_empleado(sucursal.Rows[0]["id"].ToString(), id_empleado, fecha, "00:00", condicion, franco);

            // Recrear controles para que el GridView mantenga su estado
            horarios_de_empleados = planificador.get_horarios_de_empleados(sucursal.Rows[0]["id"].ToString(), fecha_inicio, fecha_fin);
            Session["horarios_de_empleados"] = horarios_de_empleados;
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            Session["lista_empleados"] = empleados;
            generar_columna_segun_rango();
            cargar_empleados();
        }
        private void modificar_horario_empleado(string id_sucursal, string id_empleado, DateTime fecha, string hora, string condicion, string franco)
        {
            DataTable horario = planificador.get_horarios_de_empleado(id_sucursal, id_empleado, fecha, fecha);
            DataTable lista_empleados = (DataTable)Session["lista_empleados"];
            DataTable sucursal = (DataTable)Session["sucursal"];
            int fila_empleado = funciones.buscar_fila_por_id(id_empleado, lista_empleados);
            DateTime horario_entrada_dato, horario_salida_dato;
            string horario_entrada, horario_salida;
            if (condicion == "entrada")
            {
                horario_entrada_dato = fecha.Add(TimeSpan.Parse(hora));
                horario_entrada = horario_entrada_dato.ToString("yyyy-MM-dd HH-mm-ss");
                horario_salida = horario_entrada_dato.ToString("yyyy-MM-dd HH-mm-ss");
            }
            else
            {
                horario_salida_dato = fecha.Add(TimeSpan.Parse(hora));
                horario_salida = horario_salida_dato.ToString("yyyy-MM-dd HH-mm-ss");
                horario_entrada = horario_salida_dato.ToString("yyyy-MM-dd HH-mm-ss");
            }
            if (horario.Rows.Count == 0)
            {
                planificador.insertar_horario_empleado(sucursal, lista_empleados, fila_empleado, fecha, horario_entrada, horario_salida, franco);
            }
            else
            {
                string horario_modificar = "N/A";
                string id_horario = horario.Rows[0]["id"].ToString();
                if (condicion == "entrada")
                {
                    horario_entrada_dato = fecha.Add(TimeSpan.Parse(hora));
                    horario_modificar = horario_entrada_dato.ToString("yyyy-MM-dd HH-mm-ss");
                }
                else if (condicion == "salida")
                {
                    horario_salida_dato = fecha.Add(TimeSpan.Parse(hora));
                    horario_modificar = horario_salida_dato.ToString("yyyy-MM-dd HH-mm-ss");
                }

                planificador.modificar_horario_empleado(id_horario, horario_modificar, condicion);
            }
        }

        // Clase interna CustomTemplate
        //   public class CustomTemplate : ITemplate
        //   {
        //       private string fecha;
        //       private string dia;
        //       private string id;
        //       private planificador_de_horarios parent;
        //
        //       public CustomTemplate(string fecha, string dia, string id, planificador_de_horarios parent)
        //       {
        //           this.fecha = fecha;
        //           this.dia = dia;
        //           this.id = id;
        //           this.parent = parent;
        //       }
        //
        //       public void InstantiateIn(Control container)
        //       {
        //           // Crea el <h1> y el Label
        //           HtmlGenericControl heading = new HtmlGenericControl("h3");
        //           Label label = new Label
        //           {
        //               ID = $"label_{fecha}_{dia}_{id}",
        //               CssClass = "badge rounded-pill text-bg-primary",
        //               Text = dia
        //           };
        //
        //           // Crea el CheckBox
        //           CheckBox checkbox = new CheckBox
        //           {
        //               ID = $"check_{fecha}_{dia}_{id}",
        //               CssClass = "form-control",
        //               Text = "Franco"
        //           };
        //
        //           // Crea el TextBox de Hora Entrada
        //           TextBox textboxEntrada = new TextBox
        //           {
        //               ID = $"{fecha}-entrada-{id}",
        //               CssClass = "form-control",
        //               TextMode = TextBoxMode.Time
        //           };
        //
        //           // Añade el evento OnTextChanged al TextBox de Hora Entrada
        //           textboxEntrada.TextChanged += parent.TextboxEntrada_TextChanged;
        //           textboxEntrada.AutoPostBack = true;
        //
        //           // Crea el TextBox de Hora Salida
        //           TextBox textboxSalida = new TextBox
        //           {
        //               ID = $"{fecha}-salida-{id}",
        //               CssClass = "form-control",
        //               TextMode = TextBoxMode.Time
        //           };
        //
        //           // Añade el evento OnTextChanged al TextBox de Hora Salida
        //           textboxSalida.TextChanged += parent.TextboxSalida_TextChanged;
        //           textboxSalida.AutoPostBack = true;
        //
        //           // Agrega los controles al contenedor
        //           heading.Controls.Add(label);
        //           container.Controls.Add(heading);
        //           container.Controls.Add(new Literal { Text = "<br />Hora Entrada: " });
        //           container.Controls.Add(textboxEntrada);
        //           container.Controls.Add(new Literal { Text = "<br />Hora Salida: " });
        //           container.Controls.Add(textboxSalida);
        //           container.Controls.Add(checkbox);
        //       }
        //   }
    }
}