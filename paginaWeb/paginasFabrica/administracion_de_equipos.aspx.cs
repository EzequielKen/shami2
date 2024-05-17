using _03___sistemas_fabrica;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class administracion_de_equipos : System.Web.UI.Page
    {
        private void cargar_datos_ubicacion()
        {
            ubicacion = administrador.get_ubicaciones();
            gridview_ubicacion.DataSource = ubicacion;
            gridview_ubicacion.DataBind();
            dropdown_ubicaciones.Items.Clear();
            for (int fila = 0; fila <= ubicacion.Rows.Count - 1; fila++)
            {
                dropdown_ubicaciones.Items.Add(ubicacion.Rows[fila]["ubicacion"].ToString());
            }
        }
        private void cargar_datos_equipos()
        {
            equipos = administrador.get_equipos();
            gridview_equipos.DataSource = equipos;
            gridview_equipos.DataBind();
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administracion_de_equipos administrador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable ubicacion;
        DataTable equipos;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Add("tipo_equipo", "N/A");
            }
            if ("N/A" == Session["tipo_equipo"].ToString())
            {
                textbox_temperatura.Visible = false;
                textbox_observacion.Visible = false;
                textbox_equipo.Visible = false;
                boton_equipo.Visible = false;
                dropdown_ubicaciones.Visible = false;
            }
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["administrador"] == null)
            {
                Session.Add("administrador", new cls_administracion_de_equipos(usuariosBD));
            }
            administrador = (cls_administracion_de_equipos)Session["administrador"];
            cargar_datos_ubicacion();
            if (!IsPostBack)
            {
                cargar_datos_equipos();
            }
        }

        protected void boton_ubicacion_Click(object sender, EventArgs e)
        {
            if (textbox_ubicacion.Text != string.Empty)
            {
                administrador.cargar_ubicacion(textbox_ubicacion.Text);
                textbox_ubicacion.Text = string.Empty;
                cargar_datos_ubicacion();
            }
        }

        protected void boton_congelacion_Click(object sender, EventArgs e)
        {
            Session.Add("tipo_equipo", "Congelacion");
            textbox_temperatura.Visible = true;
            textbox_temperatura.Text = "0 °C A -18 °C";//Congelacion
            textbox_observacion.Visible = true;
            textbox_equipo.Visible = true;
            boton_equipo.Visible = true;
            dropdown_ubicaciones.Visible = true;
            label_tipo_seleccionado.Text = "Tipo Seleccionado: " + Session["tipo_equipo"].ToString();
            label_tipo_seleccionado.CssClass = "bg-primary";
            textbox_temperatura.CssClass = "form-control bg-primary";
        }

        protected void boton_refrigeracion_Click(object sender, EventArgs e)
        {
            Session.Add("tipo_equipo", "Refrigeracion");
            textbox_temperatura.Visible = true;
            textbox_temperatura.Text = "0 °C A 5 °C";
            textbox_observacion.Visible = true;
            textbox_equipo.Visible = true;
            boton_equipo.Visible = true;
            dropdown_ubicaciones.Visible = true;
            label_tipo_seleccionado.Text = "Tipo Seleccionado: " + Session["tipo_equipo"].ToString();
            label_tipo_seleccionado.CssClass = "bg-warning";
            textbox_temperatura.CssClass = "form-control bg-warning";
        }

        protected void boton_coccion_Click(object sender, EventArgs e)
        {
            Session.Add("tipo_equipo", "Coccion");
            textbox_temperatura.Visible = true;
            textbox_temperatura.Text = string.Empty;
            textbox_observacion.Visible = true;
            textbox_equipo.Visible = true;
            boton_equipo.Visible = true;
            dropdown_ubicaciones.Visible = true;
            label_tipo_seleccionado.Text = "Tipo Seleccionado: " + Session["tipo_equipo"].ToString();
            label_tipo_seleccionado.CssClass = "bg-danger";
            textbox_temperatura.CssClass = "form-control bg-danger";

        }

        protected void boton_equipo_Click(object sender, EventArgs e)
        {
            if (textbox_equipo.Text != string.Empty)
            {
                administrador.cargar_equipo(Session["tipo_equipo"].ToString(), dropdown_ubicaciones.SelectedItem.Text, textbox_equipo.Text, textbox_temperatura.Text, textbox_observacion.Text);
                textbox_equipo.Text = string.Empty;
                cargar_datos_equipos();
            }
        }

        protected void gridview_equipos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridview_equipos.Rows.Count - 1; fila++)
            {
                TextBox textbox_nombre_ubicacion = (gridview_equipos.Rows[fila].Cells[0].FindControl("textbox_nombre_ubicacion") as TextBox);
                DropDownList dropdown_Categoria_equipo = (gridview_equipos.Rows[fila].Cells[1].FindControl("dropdown_Categoria_equipo") as DropDownList);
                DropDownList dropdown_ubicacion_equipo = (gridview_equipos.Rows[fila].Cells[2].FindControl("dropdown_ubicacion_equipo") as DropDownList);
                TextBox textbox_temperatura_equipo = (gridview_equipos.Rows[fila].Cells[3].FindControl("textbox_temperatura_equipo") as TextBox);
                TextBox textbox_observacion_equipo = (gridview_equipos.Rows[fila].Cells[4].FindControl("textbox_observacion_equipo") as TextBox);

                textbox_nombre_ubicacion.Text = equipos.Rows[fila]["nombre"].ToString();
                textbox_temperatura_equipo.Text = equipos.Rows[fila]["temperatura"].ToString();
                textbox_observacion_equipo.Text = equipos.Rows[fila]["observaciones"].ToString();
                dropdown_Categoria_equipo.SelectedValue = equipos.Rows[fila]["categoria"].ToString();
                if (dropdown_ubicacion_equipo.Items.Count == 0)
                {
                    for (int fila_ubicacion = 0; fila_ubicacion <= ubicacion.Rows.Count - 1; fila_ubicacion++)
                    {
                        dropdown_ubicacion_equipo.Items.Add(ubicacion.Rows[fila_ubicacion]["ubicacion"].ToString());
                    }
                }
                dropdown_ubicacion_equipo.SelectedValue = equipos.Rows[fila]["ubicacion"].ToString();

                if (dropdown_Categoria_equipo.SelectedItem.Text == "Congelacion")//Refrigeracion Coccion
                {
                    dropdown_Categoria_equipo.CssClass = "btn btn-primary dropdown-toggle";
                }
                else if (dropdown_Categoria_equipo.SelectedItem.Text == "Refrigeracion")//Refrigeracion Coccion
                {
                    dropdown_Categoria_equipo.CssClass = "btn btn-warning dropdown-toggle";

                }
                else if (dropdown_Categoria_equipo.SelectedItem.Text == "Coccion")//Refrigeracion Coccion
                {
                    dropdown_Categoria_equipo.CssClass = "btn btn-danger dropdown-toggle";

                }
            }
        }
        protected void textbox_nombre_ubicacion_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_equipos.Rows[rowIndex].Cells[0].Text;
            administrador.modificar_equipo_nombre(id, textbox_dato.Text);
            cargar_datos_equipos();

        }

        protected void dropdown_Categoria_equipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_categoria = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_categoria.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_equipos.Rows[rowIndex].Cells[0].Text;
            administrador.modificar_equipo_categoria(id, dropdown_categoria.SelectedItem.Text);
            if ("Refrigeracion" == dropdown_categoria.SelectedItem.Text)
            {
                administrador.modificar_equipo_temperatura(id, "0 °C A 5 °C");

            }
            else if ("Congelacion" == dropdown_categoria.SelectedItem.Text)
            {
                administrador.modificar_equipo_temperatura(id, "0 °C A -18 °C");
            }
            else if ("Coccion" == dropdown_categoria.SelectedItem.Text)
            {
                administrador.modificar_equipo_temperatura(id, "N/A");
            }
            cargar_datos_equipos();

        }

        protected void dropdown_ubicacion_equipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_ubicacion = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_ubicacion.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_equipos.Rows[rowIndex].Cells[0].Text;
            administrador.modificar_equipo_ubicacion(id, dropdown_ubicacion.SelectedItem.Text);
            cargar_datos_equipos();

        }

        protected void textbox_temperatura_equipo_TextChanged(object sender, EventArgs e)
        {
            cargar_datos_equipos();
        }

        protected void textbox_observacion_equipo_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_observacion_equipo = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_observacion_equipo.NamingContainer;
            int rowIndex = row.RowIndex;

            string id = gridview_equipos.Rows[rowIndex].Cells[0].Text;
            administrador.modificar_equipo_observaciones(id, textbox_observacion_equipo.Text);
            cargar_datos_equipos();
        }

        protected void textbox_temperatura_TextChanged(object sender, EventArgs e)
        {
            if ("Refrigeracion" == Session["tipo_equipo"].ToString())
            {
                textbox_temperatura.Text = "0 °C A 5 °C";
            }
            else if ("Congelacion" == Session["tipo_equipo"].ToString())
            {
                textbox_temperatura.Text = "0 °C A -18 °C";//Congelacion
            }
        }
    }
}