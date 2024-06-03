using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class historial_temperatura : System.Web.UI.Page
    {
        static TimeSpan ConvertirAHorario(string horaStr)
        {
            // Intentar convertir la cadena a TimeSpan
            TimeSpan horario;
            if (TimeSpan.TryParse(horaStr, out horario))
            {
                return horario;
            }
            else
            {
                throw new FormatException("La cadena de horario no tiene el formato correcto.");
            }
        }
        static string ExtraerNumeros(string input)
        {
            // Expresión regular para extraer los números
            string pattern = @"(-?\d+)\s*°C\s*A\s*(-?\d+)\s*°C";

            // Intentar hacer coincidir la cadena de entrada con el patrón
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // Extraer los dos grupos de números capturados
                string numero1 = match.Groups[1].Value;
                string numero2 = match.Groups[2].Value;

                // Formatear y retornar los números
                return $"{numero1}-{numero2}";
            }
            else
            {
                throw new FormatException("La cadena no tiene el formato correcto.");
            }
        }
        private void cargar_ubicaciones()
        {
            ubicaciones = historial.get_ubicaciones();
            dropdown_ubicaciones.Items.Add("todos");
            for (int fila = 0; fila <= ubicaciones.Rows.Count - 1; fila++)
            {
                dropdown_ubicaciones.Items.Add(ubicaciones.Rows[fila]["ubicacion"].ToString());
            }
        }
        private void cargar_equipos()
        {
            label_fecha.Text = "Fecha seleccionada: "+fecha_seleccionada.ToString("dd/MM/yyyy");
            equipos = historial.get_equipos(dropdown_ubicaciones.SelectedItem.Text, fecha_seleccionada);
            gridview_equipos.DataSource = equipos;
            gridview_equipos.DataBind();
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_temperatura historial;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable ubicaciones;
        DataTable equipos;
        DateTime fecha_seleccionada = DateTime.Now;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["historial_de_equipos"] == null)
            {
                Session.Add("historial_de_equipos", new cls_historial_temperatura(usuariosBD));
            }
            historial = (cls_historial_temperatura)Session["historial_de_equipos"];
            if (!IsPostBack)
            {
                cargar_ubicaciones();
                cargar_equipos();
            }
        }

        protected void gridview_equipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int temperatura_dato;
            string temperatura_rango;
            TimeSpan hora1;
            TimeSpan hora2;
            TimeSpan hora3;

            TimeSpan hora_rango_1 = ConvertirAHorario("12:00:00");
            TimeSpan hora_rango_2 = ConvertirAHorario("18:00:00");
            TimeSpan hora_rango_3 = ConvertirAHorario("21:00:00");
            for (int fila = 0; fila <= gridview_equipos.Rows.Count - 1; fila++)
            {
                if (gridview_equipos.Rows[fila].Cells[5].Text != "N/A")
                {
                //    temperatura_dato = int.Parse(funciones.obtener_dato(gridview_equipos.Rows[fila].Cells[5].Text.Replace("°C",""), 1));
              //     temperatura_rango = ExtraerNumeros(gridview_equipos.Rows[fila].Cells[4].Text);
                    hora1 = ConvertirAHorario(funciones.obtener_dato(gridview_equipos.Rows[fila].Cells[5].Text, 3));
                    if (hora1 > hora_rango_1)
                    {
                        gridview_equipos.Rows[fila].Cells[5].CssClass = "table-danger";
                    }
                }

                if (gridview_equipos.Rows[fila].Cells[6].Text != "N/A")
                {

                    hora2 = ConvertirAHorario(funciones.obtener_dato(gridview_equipos.Rows[fila].Cells[6].Text, 3));
                    if (hora2 > hora_rango_2)
                    {
                        gridview_equipos.Rows[fila].Cells[6].CssClass = "table-danger";
                    }
                }

                if (gridview_equipos.Rows[fila].Cells[7].Text != "N/A")
                {

                    hora3 = ConvertirAHorario(funciones.obtener_dato(gridview_equipos.Rows[fila].Cells[7].Text, 3));
                    if (hora3 > hora_rango_3)
                    {
                        gridview_equipos.Rows[fila].Cells[7].CssClass = "table-danger";
                    }
                }
            }
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            fecha_seleccionada = calendario.SelectedDate;
            cargar_equipos();
        }

        protected void dropdown_ubicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_equipos();
        }
    }
}