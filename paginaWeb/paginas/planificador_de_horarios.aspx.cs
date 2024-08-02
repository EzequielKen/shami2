using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
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
                GenerarColumnasDinamicas();  // Llamada a la generación de columnas dinámicas
                cargar_empleados();
            }
        }

        private void cargar_empleados()
        {
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
                fecha_fin = DateTime.Now.AddDays(7);
                textbox_fecha_inicio.Text = fecha_inicio.ToString("yyyy-MM-dd");
                textbox_fecha_fin.Text = fecha_fin.ToString("yyyy-MM-dd");
                GenerarColumnasDinamicas();  // Llamada a la generación de columnas dinámicas
            }
            empleados = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            cargar_empleados();
        }

        #region rango de fechas
        protected void textbox_fecha_inicio_TextChanged(object sender, EventArgs e)
        {
            generar_columna_segun_rango();
        }

        protected void textbox_fecha_fin_TextChanged(object sender, EventArgs e)
        {
            generar_columna_segun_rango();
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
            gridview_empleados.Columns.Add(new BoundField { HeaderText = "Nombre", DataField = "nombre" });
            gridview_empleados.Columns.Add(new BoundField { HeaderText = "Apellido", DataField = "apellido" });

            // Genera las columnas dinámicas
            for (DateTime fecha = fecha_inicio; fecha <= fecha_fin; fecha = fecha.AddDays(1))
            {
                string diaSemana = fecha.ToString("dddd"); // Obtiene el día de la semana
                string fechaDia = fecha.ToString("dd-MM-yyyy"); // Formato de fecha
                string columnaHeaderText = $"{fechaDia} ({diaSemana})";

                // Crea una nueva columna TemplateField
                TemplateField columna = new TemplateField
                {
                    HeaderText = columnaHeaderText
                };

                // Define el contenido de la columna
                columna.ItemTemplate = new CustomTemplate(fechaDia, diaSemana);

                // Agrega la columna al GridView
                gridview_empleados.Columns.Add(columna);
            }
        }
        #endregion

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Implementa cualquier lógica adicional que necesites durante el enlace de datos en cada fila
        }
    }

    public class CustomTemplate : ITemplate
    {
        private string fecha;
        private string dia;

        public CustomTemplate(string fecha, string dia)
        {
            this.fecha = fecha;
            this.dia = dia;
        }

        public void InstantiateIn(Control container)
        {
            // Crea el CheckBox
            CheckBox checkbox = new CheckBox
            {
                ID = $"check_{fecha}_{dia}",
                CssClass = "form-control",
                Text = "Franco"
            };

            // Crea el TextBox de Hora Entrada
            TextBox textboxEntrada = new TextBox
            {
                ID = $"textbox_{fecha}_{dia}_entrada",
                CssClass = "form-control",
                TextMode = TextBoxMode.Time
            };

            // Crea el TextBox de Hora Salida
            TextBox textboxSalida = new TextBox
            {
                ID = $"textbox_{fecha}_{dia}_salida",
                CssClass = "form-control",
                TextMode = TextBoxMode.Time
            };

            // Agrega los controles al contenedor
            container.Controls.Add(new Literal { Text = "<br />Hora Entrada: " });
            container.Controls.Add(textboxEntrada);
            container.Controls.Add(new Literal { Text = "<br />Hora Salida: " });
            container.Controls.Add(textboxSalida);
            container.Controls.Add(checkbox);
        }
    }
}
