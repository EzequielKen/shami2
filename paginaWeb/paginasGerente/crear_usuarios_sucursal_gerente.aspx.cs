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
    public partial class crear_usuarios_sucursal_gerente : System.Web.UI.Page
    {
        #region crear cliente
        private void crear_tabla_usuario()
        {
            usuario = new DataTable();
            usuario.Columns.Add("usuario", typeof(string));
            usuario.Columns.Add("contraseña", typeof(string));

            usuario.Columns.Add("id_sucursal_seleccionada", typeof(string));

        }
        private void llenar_tabla_usuario()
        {
            crear_tabla_usuario();
            usuario.Rows.Add();

            usuario.Rows[usuario.Rows.Count - 1]["usuario"] = textbox_usuario_nuevo.Text;
            usuario.Rows[usuario.Rows.Count - 1]["contraseña"] = textbox_contraseña_nuevo.Text;
            usuario.Rows[usuario.Rows.Count - 1]["id_sucursal_seleccionada"] = Session["id_sucursal_seleccionada"].ToString();



        }
        private bool verificar_carga()
        {
            bool retorno = true;
            if (textbox_usuario_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_usuario_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_usuario_nuevo.CssClass = "form-control";
            }

            if (textbox_contraseña_nuevo.Text == string.Empty)
            {
                retorno = false;
                textbox_contraseña_nuevo.CssClass = "form-control bg-danger";
            }
            else
            {
                textbox_contraseña_nuevo.CssClass = "form-control";
            }

            return retorno;
        }
        #endregion
        #region cargar usuarios
        private void crear_tabla_usuarios()
        {
            usuarios = new DataTable();
            usuarios.Columns.Add("id", typeof(string));
            usuarios.Columns.Add("usuario", typeof(string));
            usuarios.Columns.Add("contraseña", typeof(string));
        }
        private void llenar_usuarios()
        {
            crear_tabla_usuarios();
            for (int fila = 0; fila <= usuarios_BD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, usuarios_BD.Rows[fila]["usuario"].ToString()))
                {
                    usuarios.Rows.Add();
                    usuarios.Rows[usuarios.Rows.Count - 1]["id"] = usuarios_BD.Rows[fila]["id"].ToString();
                    usuarios.Rows[usuarios.Rows.Count - 1]["usuario"] = usuarios_BD.Rows[fila]["usuario"].ToString();
                    usuarios.Rows[usuarios.Rows.Count - 1]["contraseña"] = usuarios_BD.Rows[fila]["contraseña"].ToString();
                }
            }
        }
        private void cargar_usuarios()
        {
            usuarios_BD = (DataTable)Session["usuarios_BD"];
            llenar_usuarios();
            gridview_usuarios.DataSource = usuarios;
            gridview_usuarios.DataBind();
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_usuarios_sucursal creador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable usuarios_BD;
        DataTable usuarios;
        DataTable usuario;
        string tipo_seleccionado;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            creador = new cls_crear_usuarios_sucursal(usuariosBD);
            if (!IsPostBack)
            {
                usuarios_BD = creador.get_usuarios(Session["id_sucursal_seleccionada"].ToString());
                Session.Add("usuarios_BD", usuarios_BD);
                cargar_usuarios();
            }
        }

        #region carga usuarios
        protected void gridview_usuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            usuarios_BD = (DataTable)Session["usuarios_BD"];
            string id;
            int fila_producto;
            for (int fila = 0; fila <= gridview_usuarios.Rows.Count - 1; fila++)
            {
                id = gridview_usuarios.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, usuarios_BD);
                //ACTIVA
                DropDownList dropdown_activo = (gridview_usuarios.Rows[fila].Cells[1].FindControl("dropdown_activo") as DropDownList);
                if (usuarios_BD.Rows[fila_producto]["activa"].ToString() == "1")
                {
                    dropdown_activo.SelectedValue = "Si";
                }
                else
                {
                    dropdown_activo.SelectedValue = "No";
                }
                //USUARIO
                TextBox textbox_usuario = (gridview_usuarios.Rows[fila].Cells[2].FindControl("textbox_usuario") as TextBox);
                textbox_usuario.Text = usuarios_BD.Rows[fila_producto]["usuario"].ToString();
                //CONTRASEÑA
                TextBox textbox_contraseña = (gridview_usuarios.Rows[fila].Cells[3].FindControl("textbox_contraseña") as TextBox);
                textbox_contraseña.Text = usuarios_BD.Rows[fila_producto]["contraseña"].ToString();
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_usuarios();
        }
        #endregion

        #region modificar usuarios
        protected void dropdown_activo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_activo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_activo.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_usuarios.Rows[fila].Cells[0].Text;

            if (dropdown_activo.SelectedItem.Text == "Si")
            {
                creador.actualizar_dato(id, "activa", "1");
            }
            else
            {
                creador.actualizar_dato(id, "activa", "0");
            }
            usuarios_BD = creador.get_usuarios(Session["id_sucursal_seleccionada"].ToString());
            Session.Add("usuarios_BD", usuarios_BD);
            cargar_usuarios();
        }

        protected void textbox_usuario_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_usuario = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_usuario.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_usuarios.Rows[fila].Cells[0].Text;

            if (textbox_usuario.Text != string.Empty)
            {
                creador.actualizar_dato(id, "usuario", textbox_usuario.Text);
            }
            usuarios_BD = creador.get_usuarios(Session["id_sucursal_seleccionada"].ToString());
            Session.Add("usuarios_BD", usuarios_BD);
            cargar_usuarios();
        }

        protected void textbox_contraseña_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_contraseña = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_contraseña.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_usuarios.Rows[fila].Cells[0].Text;

            if (textbox_contraseña.Text != string.Empty)
            {
                creador.actualizar_dato(id, "contraseña", textbox_contraseña.Text);
            }
            usuarios_BD = creador.get_usuarios(Session["id_sucursal_seleccionada"].ToString());
            Session.Add("usuarios_BD", usuarios_BD);
            cargar_usuarios();

        }

        protected void boton_crear_usuario_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                llenar_tabla_usuario();
                creador.crear_usuario(usuario);
                textbox_usuario_nuevo.Text = string.Empty;
                textbox_contraseña_nuevo.Text = string.Empty;
                usuarios_BD = creador.get_usuarios(Session["id_sucursal_seleccionada"].ToString());
                Session.Add("usuarios_BD", usuarios_BD);
                cargar_usuarios();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "mostrarModal();", true);

            }
        }
        #endregion
    }
}