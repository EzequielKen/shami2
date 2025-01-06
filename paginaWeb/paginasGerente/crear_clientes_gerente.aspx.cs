using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class crear_clientes_gerente : System.Web.UI.Page
    {
        #region crear cliente
        private void crear_tabla_sucursal()
        {
            sucursal = new DataTable();
            sucursal.Columns.Add("sucursal", typeof(string));
            sucursal.Columns.Add("franquicia", typeof(string));

            sucursal.Columns.Add("provincia", typeof(string));
            sucursal.Columns.Add("localidad", typeof(string));
            sucursal.Columns.Add("direccion", typeof(string));

            sucursal.Columns.Add("telefono", typeof(string));
        }
        private void llenar_tabla_sucursal()
        {
            crear_tabla_sucursal();
            sucursal.Rows.Add();

            sucursal.Rows[sucursal.Rows.Count - 1]["sucursal"] = textbox_sucursal_nuevo.Text;

            if (textbox_nueva_franquicia.Text == string.Empty)
            {
                sucursal.Rows[sucursal.Rows.Count - 1]["franquicia"] = dropdown_franquicia_nuevo.SelectedItem.Text;
            }
            else
            {
                sucursal.Rows[sucursal.Rows.Count - 1]["franquicia"] = textbox_nueva_franquicia.Text;
            }

            sucursal.Rows[sucursal.Rows.Count - 1]["provincia"] = textbox_provincia_nuevo.Text;
            sucursal.Rows[sucursal.Rows.Count - 1]["localidad"] = textbox_localidad_nuevo.Text;
            sucursal.Rows[sucursal.Rows.Count - 1]["direccion"] = textbox_direccion_nuevo.Text;
            sucursal.Rows[sucursal.Rows.Count - 1]["telefono"] = textbox_telefono_nuevo.Text;

        }
        private bool verificar_carga()
        {
            bool retorno = true;
            if (textbox_sucursal_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_sucursal_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_sucursal_nuevo.CssClass = "form-control";
            }

            if (textbox_provincia_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_provincia_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_provincia_nuevo.CssClass = "form-control";
            }

            if (textbox_localidad_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_localidad_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_localidad_nuevo.CssClass = "form-control";
            }

            if (textbox_direccion_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_direccion_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_direccion_nuevo.CssClass = "form-control";
            }

            if (textbox_telefono_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_telefono_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_telefono_nuevo.CssClass = "form-control";
            }
            return retorno;
        }
        #endregion
        #region cargar clientes
        private void crear_tabla_clientes()
        {
            sucursales = new DataTable();
            sucursales.Columns.Add("id", typeof(string));
            sucursales.Columns.Add("sucursal", typeof(string));
        }
        private void llenar_tabla_clientes()
        {
            crear_tabla_clientes();

            for (int fila = 0; fila <= sucursalesBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, sucursalesBD.Rows[fila]["sucursal"].ToString()) &&
                    funciones.verificar_tipo_producto(dropDown_tipo.SelectedItem.Text, sucursalesBD.Rows[fila]["franquicia"].ToString()))
                {
                    sucursales.Rows.Add();
                    sucursales.Rows[sucursales.Rows.Count - 1]["id"] = sucursalesBD.Rows[fila]["id"].ToString();
                    sucursales.Rows[sucursales.Rows.Count - 1]["sucursal"] = sucursalesBD.Rows[fila]["sucursal"].ToString();
                }
            }
        }
        private void cargar_clientes()
        {
            sucursalesBD = (DataTable)Session["sucursalesBD"];
            llenar_tabla_clientes();
            gridview_clientes.DataSource = sucursales;
            gridview_clientes.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            sucursalesBD = (DataTable)Session["sucursalesBD"];
            llenar_dropDownList(sucursalesBD);
            llenar_dropDownList_franquicia_nuevo(sucursalesBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            dt.DefaultView.Sort = "franquicia asc";
            dt = dt.DefaultView.ToTable();

            tipo_seleccionado = dt.Rows[0]["franquicia"].ToString();
            item = new ListItem(dt.Rows[0]["franquicia"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["franquicia"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["franquicia"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }
        }
        private void llenar_dropDownList_franquicia_nuevo(DataTable dt)
        {
            dropdown_franquicia_nuevo.Items.Clear();
            int num_item = 1;
            ListItem item;

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            dt.DefaultView.Sort = "franquicia asc";
            dt = dt.DefaultView.ToTable();

            tipo_seleccionado = dt.Rows[0]["franquicia"].ToString();
            item = new ListItem(dt.Rows[0]["franquicia"].ToString(), num_item.ToString());
            dropdown_franquicia_nuevo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropdown_franquicia_nuevo.Items[num_item - 2].Text != dt.Rows[fila]["franquicia"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["franquicia"].ToString(), num_item.ToString());
                    dropdown_franquicia_nuevo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_clientes creador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursalesBD;
        DataTable sucursales;
        DataTable sucursal;
        string tipo_seleccionado;

        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            creador = new cls_crear_clientes(usuariosBD);
            if (!IsPostBack)
            {
                sucursalesBD = creador.get_sucursales();
                Session.Add("sucursalesBD", sucursalesBD);
                configurar_controles();
                cargar_clientes();
            }
        }

        #region carga de clientes

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_clientes();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_clientes();
        }

        protected void gridview_clientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            sucursalesBD = (DataTable)Session["sucursalesBD"];
            string id;
            int fila_producto;
            for (int fila = 0; fila <= gridview_clientes.Rows.Count - 1; fila++)
            {
                id = gridview_clientes.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, sucursalesBD);
                //ACTIVA
                DropDownList dropdown_activo = (gridview_clientes.Rows[fila].Cells[1].FindControl("dropdown_activo") as DropDownList);
                if (sucursalesBD.Rows[fila_producto]["activa"].ToString() == "1")
                {
                    dropdown_activo.SelectedValue = "Si";
                }
                else
                {
                    dropdown_activo.SelectedValue = "No";
                }
                //PROVINCIA
                TextBox textbox_pronvincia = (gridview_clientes.Rows[fila].Cells[3].FindControl("textbox_pronvincia") as TextBox);
                textbox_pronvincia.Text = sucursalesBD.Rows[fila_producto]["provincia"].ToString();
                //PROVINCIA
                TextBox textbox_localidad = (gridview_clientes.Rows[fila].Cells[4].FindControl("textbox_localidad") as TextBox);
                textbox_localidad.Text = sucursalesBD.Rows[fila_producto]["localidad"].ToString();
                //PROVINCIA
                TextBox textbox_direccion = (gridview_clientes.Rows[fila].Cells[5].FindControl("textbox_direccion") as TextBox);
                textbox_direccion.Text = sucursalesBD.Rows[fila_producto]["direccion"].ToString();
                //TELEFONO
                TextBox textbox_telefono = (gridview_clientes.Rows[fila].Cells[6].FindControl("textbox_telefono") as TextBox);
                textbox_telefono.Text = sucursalesBD.Rows[fila_producto]["telefono"].ToString();

                //FRANQUICIA
                DropDownList dropdown_franquicia = (gridview_clientes.Rows[fila].Cells[7].FindControl("dropdown_franquicia") as DropDownList);
                sucursalesBD.DefaultView.Sort = "franquicia asc";
                sucursalesBD = sucursalesBD.DefaultView.ToTable();
                id = gridview_clientes.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, sucursalesBD);
                dropdown_franquicia.Items.Clear();
                int num_item = 1;
                ListItem item;

                //        item = new ListItem("Todos", num_item.ToString());
                //        dropDown_tipo.Items.Add(item);
                //        num_item = num_item + 1;

                tipo_seleccionado = sucursalesBD.Rows[0]["franquicia"].ToString();
                item = new ListItem(sucursalesBD.Rows[0]["franquicia"].ToString(), num_item.ToString());
                dropdown_franquicia.Items.Add(item);
                num_item = num_item + 1;
                for (int fila_insumos = 0; fila_insumos <= sucursalesBD.Rows.Count - 1; fila_insumos++)
                {


                    if (dropdown_franquicia.Items[num_item - 2].Text != sucursalesBD.Rows[fila_insumos]["franquicia"].ToString())
                    {

                        dropdown_franquicia.Items.Add(sucursalesBD.Rows[fila_insumos]["franquicia"].ToString());
                        num_item = num_item + 1;
                    }

                }
                dropdown_franquicia.SelectedValue = sucursalesBD.Rows[fila_producto]["franquicia"].ToString();
            }



        }
        #endregion

        #region modificacion de clientes
        protected void dropdown_activo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_activo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_activo.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            if (dropdown_activo.SelectedItem.Text == "Si")
            {
                creador.actualizar_dato(id, "activa", "1");
            }
            else
            {
                creador.actualizar_dato(id, "activa", "0");
            }
            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            cargar_clientes();
        }

        protected void textbox_pronvincia_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_pronvincia = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_pronvincia.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            if (textbox_pronvincia.Text != string.Empty)
            {
                creador.actualizar_dato(id, "provincia", textbox_pronvincia.Text);
            }
            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            cargar_clientes();
        }

        protected void textbox_localidad_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_localidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_localidad.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            if (textbox_localidad.Text != string.Empty)
            {
                creador.actualizar_dato(id, "localidad", textbox_localidad.Text);
            }
            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            cargar_clientes();
        }

        protected void textbox_direccion_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_direccion = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_direccion.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            if (textbox_direccion.Text != string.Empty)
            {
                creador.actualizar_dato(id, "direccion", textbox_direccion.Text);
            }
            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            cargar_clientes();
        }

        protected void textbox_telefono_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_telefono = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_telefono.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            if (textbox_telefono.Text != string.Empty)
            {
                creador.actualizar_dato(id, "telefono", textbox_telefono.Text);
            }
            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            cargar_clientes();
        }

        protected void dropdown_franquicia_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_franquicia = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_franquicia.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;

            creador.actualizar_dato(id, "franquicia", dropdown_franquicia.SelectedItem.Text);

            sucursalesBD = creador.get_sucursales();
            Session.Add("sucursalesBD", sucursalesBD);
            configurar_controles();
            cargar_clientes();
        }

        protected void boton_administrar_usuarios_Click(object sender, EventArgs e)
        {
            Button boton_administrar_usuarios = (Button)sender;
            GridViewRow row = (GridViewRow)boton_administrar_usuarios.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_clientes.Rows[fila].Cells[0].Text;
            Session.Add("id_sucursal_seleccionada", id);
            Response.Redirect("~/paginasGerente/crear_usuarios_sucursal_gerente.aspx");
        }
        #endregion

        protected void boton_crear_cliente_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                llenar_tabla_sucursal();

                creador.crear_sucursal(sucursal);
                textbox_sucursal_nuevo.Text = string.Empty;
                textbox_nueva_franquicia.Text = string.Empty;
                textbox_provincia_nuevo.Text = string.Empty;
                textbox_localidad_nuevo.Text = string.Empty;
                textbox_direccion_nuevo.Text = string.Empty;

                sucursalesBD = creador.get_sucursales();
                Session.Add("sucursalesBD", sucursalesBD);
                configurar_controles();
                cargar_clientes();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);

            }
        }


    }
}