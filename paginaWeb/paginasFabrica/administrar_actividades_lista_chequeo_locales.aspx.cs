using _02___sistemas;
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
    public partial class administrar_lista_chequeo_locales : System.Web.UI.Page
    {
        #region resumen
        private void crear_actividad()
        {
            if (textbox_actividad_nueva.Text != string.Empty)
            {
                string actividad = textbox_actividad_nueva.Text;
                string area = dropDown_tipo.SelectedItem.Text;
                string categoria;
                int nuevo_orden = 1;
                if (textbox_categoria_nueva.Text == string.Empty)
                {
                    categoria = dropdown_categoria_nueva.SelectedItem.Text;
                    int ultima_fila = gridview_chequeos.Rows.Count - 1;
                    TextBox textbox_ultimo = (gridview_chequeos.Rows[ultima_fila].Cells[1].FindControl("textbox_orden") as TextBox);
                    nuevo_orden = int.Parse(textbox_ultimo.Text) + 1;
                }
                else
                {
                    categoria = textbox_categoria_nueva.Text;
                }

                administrador.crear_actividad(actividad, area, categoria, nuevo_orden.ToString());
                textbox_actividad_nueva.Text = string.Empty;
                textbox_categoria_nueva.Text = string.Empty;
            }
        }
        #endregion
        #region llenar datos
        private void crear_tabla_chequeo()
        {
            lista_de_chequeo = new DataTable();
            lista_de_chequeo.Columns.Add("id", typeof(string));
            lista_de_chequeo.Columns.Add("orden", typeof(string));
            lista_de_chequeo.Columns.Add("actividad", typeof(string));
            lista_de_chequeo.Columns.Add("activa", typeof(string));
        }
        private void llenar_tabla_chequeo()
        {
            crear_tabla_chequeo();
            lista_de_chequeoBD.DefaultView.Sort = "area asc, categoria asc, orden asc";
            lista_de_chequeoBD = lista_de_chequeoBD.DefaultView.ToTable();
            int ultima_fila = 0;
            for (int fila = 0; fila <= lista_de_chequeoBD.Rows.Count - 1; fila++)
            {
                if (lista_de_chequeoBD.Rows[fila]["categoria"].ToString() == dropDown_categoria.SelectedItem.Text)
                {
                    lista_de_chequeo.Rows.Add();
                    ultima_fila = lista_de_chequeo.Rows.Count - 1;
                    lista_de_chequeo.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila]["id"].ToString();
                    lista_de_chequeo.Rows[ultima_fila]["orden"] = lista_de_chequeoBD.Rows[fila]["orden"].ToString();
                    lista_de_chequeo.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila]["actividad"].ToString();
                    lista_de_chequeo.Rows[ultima_fila]["activa"] = lista_de_chequeoBD.Rows[fila]["activa"].ToString();
                }
            }

        }
        private void cargar_lista_chequeo()
        {
            llenar_tabla_chequeo();
            gridview_chequeos.DataSource = lista_de_chequeo;
            gridview_chequeos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            llenar_dropDownList(lista_de_chequeoBD);
            llenar_dropDownList_categoria(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
            llenar_dropDownList_categoria_nueva(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
        }
        private void llenar_dropDownList_categoria(DataTable dt, string area)
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
        private void llenar_dropDownList_categoria_nueva(DataTable dt, string area)
        {
            dropdown_categoria_nueva.Items.Clear();
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
                        dropdown_categoria_nueva.Items.Add(item);
                        break;
                    }
                }
            }
            else
            {
                item = new ListItem(dt.Rows[0]["categoria"].ToString(), num_item.ToString());
                dropdown_categoria_nueva.Items.Add(item);
            }

            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropdown_categoria_nueva.Items[num_item - 2].Text != dt.Rows[fila]["categoria"].ToString() &&
                    dt.Rows[fila]["area"].ToString() == area)
                {
                    item = new ListItem(dt.Rows[fila]["categoria"].ToString(), num_item.ToString());
                    dropdown_categoria_nueva.Items.Add(item);
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
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administrar_lista_chequeo_locales administrador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable lista_de_chequeoBD;
        DataTable lista_de_chequeo;
        DataTable resumen;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["administracion_de_chequeo_locales"] == null)
            {
                Session.Add("administracion_de_chequeo_locales", new cls_administrar_lista_chequeo_locales(usuariosBD));
            }
            administrador = (cls_administrar_lista_chequeo_locales)Session["administracion_de_chequeo_locales"];
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();

            if (!IsPostBack)
            {

                configurar_controles();
                cargar_lista_chequeo();

            }
        }
        protected void gridview_chequeos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string id, estado;
            for (int fila = 0; fila <= gridview_chequeos.Rows.Count - 1; fila++)
            {
                TextBox textbox_actividad = (gridview_chequeos.Rows[fila].Cells[0].FindControl("textbox_actividad") as TextBox);
                textbox_actividad.Text = lista_de_chequeo.Rows[fila]["actividad"].ToString();
                TextBox textbox_orden = (gridview_chequeos.Rows[fila].Cells[1].FindControl("textbox_orden") as TextBox);
                textbox_orden.Text = lista_de_chequeo.Rows[fila]["orden"].ToString();
                id = gridview_chequeos.Rows[fila].Cells[0].Text;
                estado = gridview_chequeos.Rows[fila].Cells[4].Text;
                if (estado == "0")
                {
                    gridview_chequeos.Rows[fila].CssClass = "table-danger";
                }
                /*if (fila_resumen != -1)
                {
                    gridview_chequeos.Rows[fila].CssClass = "table-success";
                    Button boton_cargar = (Button)gridview_chequeos.Rows[fila].Cells[2].Controls[0].FindControl("boton_cargar");
                    boton_cargar.Text = "Eliminar";
                    boton_cargar.CssClass = "btn btn-primary btn-sm btn-danger";
                }
                else
                {
                    Button boton_cargar = (Button)gridview_chequeos.Rows[fila].Cells[2].Controls[0].FindControl("boton_cargar");
                    boton_cargar.Text = "Cargar";
                    boton_cargar.CssClass = "btn btn-primary btn-sm ";
                }*/
            }
        }
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenar_dropDownList_categoria(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
            llenar_dropDownList_categoria_nueva(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
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

            cargar_lista_chequeo();

        }



        protected void boton_eliminar_Click(object sender, EventArgs e)
        {
            Button boton_eliminar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_eliminar.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_chequeos.Rows[rowIndex].Cells[0].Text;
            string estado = gridview_chequeos.Rows[rowIndex].Cells[4].Text;
            if (estado == "1")
            {
                estado = "0";
            }
            else
            {
                estado = "1";
            }
            administrador.actualizar_dato_actividad(id, "activa", estado);
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();
            cargar_lista_chequeo();
        }

        protected void textbox_actividad_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_actividad = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_actividad.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_chequeos.Rows[rowIndex].Cells[0].Text;
            string actividad = textbox_actividad.Text;

            administrador.actualizar_dato_actividad(id, "actividad", actividad);
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();
            cargar_lista_chequeo();
        }

        protected void boton_crear_actividad_Click(object sender, EventArgs e)
        {
            crear_actividad();
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();
            if (textbox_actividad_nueva.Text != string.Empty)
            {
                configurar_controles();

            }
            cargar_lista_chequeo();
        }

        protected void textbox_orden_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_orden = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_orden.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_chequeos.Rows[rowIndex].Cells[0].Text;


            TextBox textbox_primero = (gridview_chequeos.Rows[0].Cells[1].FindControl("textbox_orden") as TextBox);
            int ultima_fila = gridview_chequeos.Rows.Count - 1;
            TextBox textbox_ultimo = (gridview_chequeos.Rows[ultima_fila].Cells[1].FindControl("textbox_orden") as TextBox);

            int nuevo_orden = int.Parse(textbox_orden.Text);
            int primer_orden = int.Parse(textbox_primero.Text);
            int ultimo_orden = int.Parse(textbox_ultimo.Text);
            if (nuevo_orden >= primer_orden && nuevo_orden <= ultimo_orden)
            {
                administrador.set_orden_actividad(id, nuevo_orden.ToString());
                lista_de_chequeoBD = administrador.get_lista_de_chequeo();
                cargar_lista_chequeo();
            }
        }
    }
}