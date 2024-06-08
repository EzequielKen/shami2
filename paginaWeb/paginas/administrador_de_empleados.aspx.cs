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
    public partial class administrador_de_empleados : System.Web.UI.Page
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
                if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == dropDown_tipo.SelectedItem.Text ||
                    dropDown_tipo.SelectedItem.Text == "Todos")
                {
                    lista_de_empleado.Rows.Add();
                    ultima_fila = lista_de_empleado.Rows.Count - 1;
                    lista_de_empleado.Rows[ultima_fila]["id"] = lista_de_empleadoBD.Rows[fila]["id"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["id_sucursal"] = lista_de_empleadoBD.Rows[fila]["id_sucursal"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["nombre"] = lista_de_empleadoBD.Rows[fila]["nombre"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["apellido"] = lista_de_empleadoBD.Rows[fila]["apellido"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["dni"] = lista_de_empleadoBD.Rows[fila]["dni"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["telefono"] = lista_de_empleadoBD.Rows[fila]["telefono"].ToString();
                    lista_de_empleado.Rows[ultima_fila]["cargo"] = lista_de_empleadoBD.Rows[fila]["cargo"].ToString();
                }
            }
        }
        private void cargar_lista_empleados()
        {
            llenar_tabla_empleado();
            gridview_empleados.DataSource = lista_de_empleado;
            gridview_empleados.DataBind();
            calcular_cantidad_empleados();
        }
        private void calcular_cantidad_empleados()
        {
            int total = 0;
            int encargado = 0;
            int cajeros=0;
            int Shawarmero = 0;
            int atencion_cliente = 0;
            int cocina = 0;
            int limpieza = 0;
            for (int fila = 0; fila <= lista_de_empleadoBD.Rows.Count-1; fila++)
            {
                if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == "Encargado")
                {
                    encargado++;
                }
                else if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString()=="Cajero")
                {
                    cajeros ++;
                }
                else if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == "Shawarmero")
                {
                    Shawarmero++;
                }
                else if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == "Atencion al Cliente")
                {
                    atencion_cliente++;
                }
                else if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == "Cocina")
                {
                    cocina++;
                }
                else if (lista_de_empleadoBD.Rows[fila]["cargo"].ToString() == "Limpieza")
                {
                    limpieza++;
                }
            }
            label_total_encargado.Text = "Total Encargado: " + encargado.ToString();
            label_total_cajeros.Text = "Total Cajeros: " + cajeros.ToString();
            label_total_shawarmero.Text = "Total Shawarmero: " + Shawarmero.ToString();
            label_total_atencion_al_cliente.Text = "Total Atencion Cliente: " + atencion_cliente.ToString();
            label_total_cocina.Text = "Total Empleados Cocina: " + cocina.ToString();
            label_total_limpieza.Text = "Total Empleados Limpieza: " + limpieza.ToString();
            total = encargado+ cajeros + Shawarmero+atencion_cliente+cocina+limpieza;    
            label_total.Text = "Total Plantilla de Personal: "+total.ToString();
            
        }
        #endregion
        #region crear empleado
        private void crear_tabla_empleado()
        {
            empleado = new DataTable();
            empleado.Columns.Add("id_sucursal", typeof(string));
            empleado.Columns.Add("nombre", typeof(string));
            empleado.Columns.Add("apellido", typeof(string));
            empleado.Columns.Add("dni", typeof(string));
            empleado.Columns.Add("telefono", typeof(string));
            empleado.Columns.Add("cargo", typeof(string));
        }
        private void cargar_datos_empleado()
        {
            if (verificar_carga())
            {
                crear_tabla_empleado();
                empleado.Rows.Add();
                empleado.Rows[0]["id_sucursal"] = usuariosBD.Rows[0]["sucursal"].ToString();
                empleado.Rows[0]["nombre"] = textbox_nombre.Text;
                empleado.Rows[0]["apellido"] = textbox_apellido.Text;
                empleado.Rows[0]["dni"] = textbox_dni.Text;
                empleado.Rows[0]["telefono"] = textbox_telefono.Text;
                empleado.Rows[0]["cargo"] = dropdown_cargo.SelectedItem.Text;
                administrador.registrar_empleado(empleado);
                textbox_nombre.Text = string.Empty;
                textbox_apellido.Text = string.Empty;
                textbox_dni.Text = string.Empty;
                textbox_telefono.Text = string.Empty;
            }
        }
        private bool verificar_carga()
        {
            bool retorno = true;
            string mensaje = string.Empty;
            if (textbox_nombre.Text == string.Empty)
            {
                mensaje = mensaje + "Falta ingresar nombre. ";
            }
            if (textbox_apellido.Text == string.Empty)
            {
                mensaje = mensaje + "Falta ingresar apellido. ";
            }
            if (textbox_dni.Text == string.Empty)
            {
                mensaje = mensaje + "Falta ingresar DNI. ";
            }
            if (textbox_telefono.Text == string.Empty)
            {
                mensaje = mensaje + "Falta ingresar telefono. ";
            }

            if (mensaje != string.Empty)
            {
                label_advertencia.Text = mensaje;
                label_advertencia.Visible = true;
                retorno = false;
            }
            else
            {
                label_advertencia.Visible = false;
            }
            return retorno;
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            if (lista_de_empleadoBD.Rows.Count > 0)
            {
                llenar_dropDownList(lista_de_empleadoBD);
            }
        }

        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "cargo";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;


            tipo_seleccionado = "Todos";
            item = new ListItem("Todos", num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            tipo_seleccionado = dt.Rows[0]["cargo"].ToString();
            item = new ListItem(dt.Rows[0]["cargo"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["cargo"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["cargo"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        #endregion
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administrar_empleados administrador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;
        DataTable lista_de_empleadoBD;
        DataTable lista_de_empleado;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["administracion_de_empleados"] == null)
            {
                Session.Add("administracion_de_empleados", new cls_administrar_empleados(usuariosBD));
            }
            administrador = (cls_administrar_empleados)Session["administracion_de_empleados"];
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_lista_empleados();
            }
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            cargar_datos_empleado();
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void textbox_nombre_empleado_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_nombre_empleado = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_nombre_empleado.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_empleados.Rows[rowIndex].Cells[0].Text;
            string campo = "nombre";
            string dato = textbox_nombre_empleado.Text;
            administrador.actualizar_dato_empleado(id, campo, dato);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void textbox_apellido_empleado_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_apellido_empleado = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_apellido_empleado.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_empleados.Rows[rowIndex].Cells[0].Text;
            string campo = "apellido";
            string dato = textbox_apellido_empleado.Text;
            administrador.actualizar_dato_empleado(id, campo, dato);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void textbox_dni_empleado_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_dni_empleado = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dni_empleado.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_empleados.Rows[rowIndex].Cells[0].Text;
            string campo = "dni";
            string dato = textbox_dni_empleado.Text;
            administrador.actualizar_dato_empleado(id, campo, dato);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void textbox_telefono_empleado_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_telefono_empleado = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_telefono_empleado.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_empleados.Rows[rowIndex].Cells[0].Text;
            string campo = "telefono";
            string dato = textbox_telefono_empleado.Text;
            administrador.actualizar_dato_empleado(id, campo, dato);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void dropdown_cargo_empleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_cargo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_cargo.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridview_empleados.Rows[rowIndex].Cells[0].Text;
            string campo = "cargo";
            string dato = dropdown_cargo.SelectedItem.Text;
            administrador.actualizar_dato_empleado(id, campo, dato);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();
        }

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridview_empleados.Rows.Count - 1; fila++)
            {
                TextBox textbox_nombre_empleado = (gridview_empleados.Rows[fila].Cells[0].FindControl("textbox_nombre_empleado") as TextBox);
                TextBox textbox_apellido_empleado = (gridview_empleados.Rows[fila].Cells[0].FindControl("textbox_apellido_empleado") as TextBox);
                TextBox textbox_dni_empleado = (gridview_empleados.Rows[fila].Cells[0].FindControl("textbox_dni_empleado") as TextBox);
                TextBox textbox_telefono_empleado = (gridview_empleados.Rows[fila].Cells[0].FindControl("textbox_telefono_empleado") as TextBox);
                DropDownList dropdown_cargo_empleado = (gridview_empleados.Rows[fila].Cells[2].FindControl("dropdown_cargo_empleado") as DropDownList);

                textbox_nombre_empleado.Text = lista_de_empleado.Rows[fila]["nombre"].ToString();
                textbox_apellido_empleado.Text = lista_de_empleado.Rows[fila]["apellido"].ToString();
                textbox_dni_empleado.Text = lista_de_empleado.Rows[fila]["dni"].ToString();
                textbox_telefono_empleado.Text = lista_de_empleado.Rows[fila]["telefono"].ToString();
                dropdown_cargo_empleado.SelectedValue = lista_de_empleado.Rows[fila]["cargo"].ToString();
                if (dropdown_cargo_empleado.SelectedItem.Text == "Cajero")
                {
                    gridview_empleados.Rows[fila].CssClass = "table-success";
                    dropdown_cargo_empleado.CssClass = "form-control-lg bg-success";
                }

            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_lista_empleados();
        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {
            Button boton_eliminar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_eliminar.NamingContainer;
            int rowIndex = row.RowIndex;
            administrador.eliminar_empleado(gridview_empleados.Rows[rowIndex].Cells[0].Text);
            lista_de_empleadoBD = administrador.get_lista_de_empleado(usuariosBD.Rows[0]["sucursal"].ToString());
            configurar_controles();
            cargar_lista_empleados();

        }
    }
}