using _02___sistemas;
using _03___sistemas_fabrica;
using paginaWeb.paginasFabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class administrar_lista_de_chequeo : System.Web.UI.Page
    {
        #region resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("area", typeof(string));
            Session.Add("resumen_chequeo", resumen);
        }
        private void cargar_actividad_en_resumen(string id)
        {
            resumen = (DataTable)Session["resumen_chequeo"];
            int fila_actividad = funciones.buscar_fila_por_id(id, lista_de_chequeoBD);
            int fila_resumen = funciones.buscar_fila_por_id(id, resumen);
            if (fila_resumen == -1)
            {
                resumen.Rows.Add();
                int ultima_fila = resumen.Rows.Count - 1;

                resumen.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila_actividad]["id"].ToString();
                resumen.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila_actividad]["actividad"].ToString();
                resumen.Rows[ultima_fila]["categoria"] = lista_de_chequeoBD.Rows[fila_actividad]["categoria"].ToString();
                resumen.Rows[ultima_fila]["area"] = lista_de_chequeoBD.Rows[fila_actividad]["area"].ToString();
            }
            else
            {
                resumen.Rows[fila_resumen].Delete();
            }

            Session.Add("resumen_chequeo", resumen);

        }
        #endregion
        #region llenar datos
        private void crear_tabla_chequeo()
        {
            lista_de_chequeo = new DataTable();
            lista_de_chequeo.Columns.Add("id", typeof(string));
            lista_de_chequeo.Columns.Add("actividad", typeof(string));
        }
        private void llenar_tabla_chequeo()
        {
            crear_tabla_chequeo();
            int ultima_fila = 0;
            for (int fila = 0; fila <= lista_de_chequeoBD.Rows.Count - 1; fila++)
            {
                if (lista_de_chequeoBD.Rows[fila]["categoria"].ToString() == dropDown_categoria.SelectedItem.Text)
                {
                    lista_de_chequeo.Rows.Add();
                    ultima_fila = lista_de_chequeo.Rows.Count - 1;
                    lista_de_chequeo.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila]["id"].ToString();
                    lista_de_chequeo.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila]["actividad"].ToString();
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
            llenar_dropDownList_categiria(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
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
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administrar_lista_de_chequeo administrador;
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
            if (Session["administracion_de_chequeo"] == null)
            {
                Session.Add("administracion_de_chequeo", new cls_administrar_lista_de_chequeo(usuariosBD));
            }
            administrador = (cls_administrar_lista_de_chequeo)Session["administracion_de_chequeo"];
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();
            if (!IsPostBack)
            {
                configurar_controles();
                crear_tabla_resumen();
                cargar_lista_chequeo();

            }
        }
        protected void gridview_chequeos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            resumen = (DataTable)Session["resumen_chequeo"];
            int fila_resumen;
            string id;
            for (int fila = 0; fila <= gridview_chequeos.Rows.Count - 1; fila++)
            {
                id = gridview_chequeos.Rows[fila].Cells[0].Text;
                fila_resumen = funciones.buscar_fila_por_id(id, resumen);
                if (fila_resumen != -1)
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
                }
            }
        }
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenar_dropDownList_categiria(lista_de_chequeoBD, dropDown_tipo.SelectedItem.Text);
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

            cargar_actividad_en_resumen(gridview_chequeos.Rows[fila].Cells[0].Text);
            cargar_lista_chequeo();

        }

        protected void boton_guardar_Click(object sender, EventArgs e)
        {
            administrador.registrar_chequeo(usuariosBD, (DataTable)Session["resumen_chequeo"]);
        }
    }
}