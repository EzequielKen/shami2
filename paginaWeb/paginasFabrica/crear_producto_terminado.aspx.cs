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
    public partial class crear_producto_terminado : System.Web.UI.Page
    {
        #region carga a base de datos
        private void crear_producto()
        {
            producto = new DataTable();
            producto.Columns.Add("producto", typeof(string));
            producto.Columns.Add("tipo_producto", typeof(string));

            producto.Columns.Add("alimento", typeof(string));
            producto.Columns.Add("bebida", typeof(string));
            producto.Columns.Add("descartable", typeof(string));

            producto.Columns.Add("venta", typeof(string));
            producto.Columns.Add("presentacion", typeof(string));
            producto.Columns.Add("pincho", typeof(string));
        }
        private void cargar_datos_producto()
        {
            crear_producto();
            producto.Rows.Add();
            int ultima_fila = producto.Rows.Count - 1;

            producto.Rows[ultima_fila]["producto"] = texbox_producto_nuevo.Text;
            if (textbox_nuevo_tipo.Text == string.Empty)
            {
                producto.Rows[ultima_fila]["tipo_producto"] = dropdown_tipo_producto_nuevo.SelectedItem.Text;
            }
            else
            {
                int ultimo_num_categoria = int.Parse(creador.get_ultimo_num_categoria()) + 1;
                producto.Rows[ultima_fila]["tipo_producto"] = ultimo_num_categoria + "-" + textbox_nuevo_tipo.Text;
            }

            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Alimento")
            {
                producto.Rows[0]["alimento"] = "1";
            }
            else
            {
                producto.Rows[0]["alimento"] = "0";

            }
            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Bebida")
            {
                producto.Rows[0]["bebida"] = "1";
            }
            else
            {
                producto.Rows[0]["bebida"] = "0";

            }
            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Descartable")
            {
                producto.Rows[0]["descartable"] = "1";
            }
            else
            {
                producto.Rows[0]["descartable"] = "0";

            }
            if (dropdown_venta_nuevo.SelectedItem.Text == "Si")
            {
                producto.Rows[0]["venta"] = "1";
            }
            else
            {
                producto.Rows[0]["venta"] = "0";
            }
            if (dropdown_pincho_nuevo.SelectedItem.Text == "Si")
            {
                producto.Rows[0]["pincho"] = "si";
            }
            else
            {
                producto.Rows[0]["pincho"] = "no";
            }
            producto.Rows[ultima_fila]["presentacion"] = textbox_presentacion_nuevo.Text;
        }
        private bool verificar_carga()
        {
            bool retorno = true;
            if (texbox_producto_nuevo.Text == string.Empty)
            {
                texbox_producto_nuevo.CssClass = "form-control bg-danger";
                retorno = false;
            }
            else
            {
                texbox_producto_nuevo.CssClass = "form-control";
            }

            if (textbox_presentacion_nuevo.Text == string.Empty)
            {
                textbox_presentacion_nuevo.CssClass = "form-control bg-danger";
                retorno = false;
            }
            else
            {
                textbox_presentacion_nuevo.CssClass = "form-control";
            }


            return retorno;
        }
        #endregion
        #region carga de productos
        private void crear_tabla_producto()
        {
            productos_terminados = new DataTable();
            productos_terminados.Columns.Add("activa", typeof(string));
            productos_terminados.Columns.Add("id", typeof(string));
            productos_terminados.Columns.Add("producto", typeof(string));
            productos_terminados.Columns.Add("tipo_producto", typeof(string));
            productos_terminados.Columns.Add("alimento", typeof(string));
            productos_terminados.Columns.Add("bebida", typeof(string));
            productos_terminados.Columns.Add("descartable", typeof(string));
            productos_terminados.Columns.Add("venta", typeof(string));
            productos_terminados.Columns.Add("unidad_de_medida_local", typeof(string));
            productos_terminados.Columns.Add("pincho", typeof(string));
            productos_terminados.Columns.Add("equivalencia_pincho", typeof(string));
        }
        private void llenar_tabla_producto()
        {
            crear_tabla_producto();
            int ultima_fila;
            for (int fila = 0; fila <= productos_terminadosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, productos_terminadosBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(productos_terminadosBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    productos_terminados.Rows.Add();
                    ultima_fila = productos_terminados.Rows.Count - 1;
                    productos_terminados.Rows[ultima_fila]["activa"] = productos_terminadosBD.Rows[fila]["activa"].ToString();
                    productos_terminados.Rows[ultima_fila]["id"] = productos_terminadosBD.Rows[fila]["id"].ToString();
                    productos_terminados.Rows[ultima_fila]["producto"] = productos_terminadosBD.Rows[fila]["producto"].ToString();
                    productos_terminados.Rows[ultima_fila]["tipo_producto"] = productos_terminadosBD.Rows[fila]["tipo_producto"].ToString();
                    productos_terminados.Rows[ultima_fila]["alimento"] = productos_terminadosBD.Rows[fila]["alimento"].ToString();
                    productos_terminados.Rows[ultima_fila]["bebida"] = productos_terminadosBD.Rows[fila]["bebida"].ToString();
                    productos_terminados.Rows[ultima_fila]["descartable"] = productos_terminadosBD.Rows[fila]["descartable"].ToString();
                    productos_terminados.Rows[ultima_fila]["venta"] = productos_terminadosBD.Rows[fila]["venta"].ToString();
                    productos_terminados.Rows[ultima_fila]["unidad_de_medida_local"] = productos_terminadosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                    productos_terminados.Rows[ultima_fila]["pincho"] = productos_terminadosBD.Rows[fila]["pincho"].ToString();
                    productos_terminados.Rows[ultima_fila]["equivalencia_pincho"] = productos_terminadosBD.Rows[fila]["equivalencia_pincho"].ToString();
                }
            }
        }
        private void cargar_productos()
        {
            productos_terminadosBD = (DataTable)Session["productos_terminadosBD"];
            llenar_tabla_producto();
            gridview_productos.DataSource = productos_terminados;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            productos_terminadosBD = (DataTable)Session["productos_terminadosBD"];
            llenar_dropDownList(productos_terminadosBD);
            llenar_TipoProducto((DataTable)Session["categoriasBD"]);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }

        }
        private void llenar_TipoProducto(DataTable dt)
        {
            dropdown_tipo_producto_nuevo.Items.Clear();
            int num_item = 1;
            ListItem item;


            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropdown_tipo_producto_nuevo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropdown_tipo_producto_nuevo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropdown_tipo_producto_nuevo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }
        }
        #endregion
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_producto_terminado creador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable productos_terminadosBD;
        DataTable productos_terminados;
        DataTable producto;
        string tipo_seleccionado;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            creador = new cls_crear_producto_terminado(usuariosBD);
            if (!IsPostBack)
            {
                productos_terminadosBD = creador.get_productos_terminados();
                Session.Add("productos_terminadosBD", productos_terminadosBD);
                Session.Add("categoriasBD", creador.get_categorias());
                configurar_controles();
                cargar_productos();
            }
        }
        #region carga de productos
        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            productos_terminadosBD = creador.get_productos_terminados();
            string id;
            int fila_producto;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                id = gridview_productos.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, productos_terminadosBD);
                //ACTIVA
                DropDownList dropdown_activo = (gridview_productos.Rows[fila].Cells[1].FindControl("dropdown_activo") as DropDownList);
                if (productos_terminadosBD.Rows[fila_producto]["activa"].ToString() == "1")
                {
                    dropdown_activo.SelectedValue = "Si";
                }
                else
                {
                    dropdown_activo.SelectedValue = "No";
                }
                //PRODUCTO
                TextBox texbox_producto = (gridview_productos.Rows[fila].Cells[2].FindControl("texbox_producto") as TextBox);
                texbox_producto.Text = productos_terminadosBD.Rows[fila_producto]["producto"].ToString();

                //TIPO PRODUCTO
                DropDownList dropdown_tipo_producto = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_tipo_producto") as DropDownList);
                dropdown_tipo_producto.Items.Clear();
                int num_item = 1;
                ListItem item;
                tipo_seleccionado = productos_terminadosBD.Rows[0]["tipo_producto"].ToString();
                item = new ListItem(productos_terminadosBD.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
                dropdown_tipo_producto.Items.Add(item);
                num_item = num_item + 1;
                for (int fila_insumos = 0; fila_insumos <= productos_terminadosBD.Rows.Count - 1; fila_insumos++)
                {


                    if (dropdown_tipo_producto.Items[num_item - 2].Text != productos_terminadosBD.Rows[fila_insumos]["tipo_producto"].ToString())
                    {

                        dropdown_tipo_producto.Items.Add(productos_terminadosBD.Rows[fila_insumos]["tipo_producto"].ToString());
                        num_item = num_item + 1;
                    }

                }
                dropdown_tipo_producto.SelectedValue = productos_terminadosBD.Rows[fila_producto]["tipo_producto"].ToString();

                //CATEGORIA PRODUCTO
                DropDownList dropdown_categoria_producto = (gridview_productos.Rows[fila].Cells[4].FindControl("dropdown_categoria_producto") as DropDownList);
                if (productos_terminadosBD.Rows[fila_producto]["alimento"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Alimento";
                }
                if (productos_terminadosBD.Rows[fila_producto]["bebida"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Bebida";
                }
                if (productos_terminadosBD.Rows[fila_producto]["descartable"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Descartable";
                }

                //VENTA

                DropDownList dropdown_venta = (gridview_productos.Rows[fila].Cells[5].FindControl("dropdown_venta") as DropDownList);
                fila_producto = funciones.buscar_fila_por_id(id, productos_terminadosBD);
                int venta = int.Parse(productos_terminadosBD.Rows[fila_producto]["venta"].ToString());
                if (venta != 0)
                {
                    dropdown_venta.SelectedValue = "Si";
                }
                else
                {
                    dropdown_venta.SelectedValue = "No";
                }

                //PRESENTACION VENTA
                TextBox textbox_unidad = (gridview_productos.Rows[fila].Cells[6].FindControl("textbox_unidad") as TextBox);
                fila_producto = funciones.buscar_fila_por_id(id, productos_terminadosBD);
                textbox_unidad.Text = productos_terminadosBD.Rows[fila_producto]["unidad_de_medida_local"].ToString();

                //pincho

                DropDownList dropdown_pincho = (gridview_productos.Rows[fila].Cells[7].FindControl("dropdown_pincho") as DropDownList);
                Label label_equivalencia_pincho = (gridview_productos.Rows[fila].Cells[7].FindControl("label_equivalencia_pincho") as Label);
                TextBox textbox_equivalencia_pincho = (gridview_productos.Rows[fila].Cells[7].FindControl("textbox_equivalencia_pincho") as TextBox);

                fila_producto = funciones.buscar_fila_por_id(id, productos_terminadosBD);
                string pincho = productos_terminadosBD.Rows[fila_producto]["pincho"].ToString();
                if (pincho == "si")
                {
                    dropdown_pincho.SelectedValue = "Si";
                    label_equivalencia_pincho.Visible = true;
                    textbox_equivalencia_pincho.Visible = true;
                    textbox_equivalencia_pincho.Text = productos_terminadosBD.Rows[fila_producto]["equivalencia_pincho"].ToString();
                }
                else
                {
                    dropdown_pincho.SelectedValue = "No";
                    label_equivalencia_pincho.Visible = false;
                    textbox_equivalencia_pincho.Visible = false;
                }
            }
        }


        #endregion

        protected void dropdown_activo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_activo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_activo.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_activo.SelectedItem.Text == "Si")
            {
                creador.actualizar_dato(id, "activa", "1");
            }
            else
            {
                creador.actualizar_dato(id, "activa", "0");
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void texbox_producto_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_producto = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (texbox_producto.Text != string.Empty)
            {
                creador.actualizar_dato(id, "producto", texbox_producto.Text);
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void dropdown_tipo_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_tipo_producto = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            creador.actualizar_dato(id, "tipo_producto", dropdown_tipo_producto.SelectedItem.Text);
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void dropdown_categoria_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_categoria_producto = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_categoria_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_categoria_producto.SelectedItem.Text == "Alimento")
            {
                creador.actualizar_dato(id, "alimento", "1");
                creador.actualizar_dato(id, "bebida", "0");
                creador.actualizar_dato(id, "descartable", "0");
            }
            else if (dropdown_categoria_producto.SelectedItem.Text == "Bebida")
            {
                creador.actualizar_dato(id, "alimento", "0");
                creador.actualizar_dato(id, "bebida", "1");
                creador.actualizar_dato(id, "descartable", "0");
            }
            else if (dropdown_categoria_producto.SelectedItem.Text == "Descartable")
            {
                creador.actualizar_dato(id, "alimento", "0");
                creador.actualizar_dato(id, "bebida", "0");
                creador.actualizar_dato(id, "descartable", "1");
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void dropdown_venta_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_venta = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_venta.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_venta.SelectedItem.Text == "Si")
            {
                creador.actualizar_dato(id, "venta", "1");
            }
            else
            {
                creador.actualizar_dato(id, "venta", "0");
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void textbox_unidad_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_unidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_unidad.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (textbox_unidad.Text != string.Empty)
            {
                creador.actualizar_dato(id, "unidad_de_medida_local", textbox_unidad.Text);
                creador.actualizar_dato(id, "unidad_de_medida_fabrica", textbox_unidad.Text);
                creador.actualizar_dato(id, "unidad_de_medida_produccion", textbox_unidad.Text);
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void dropdown_pincho_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_pincho = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_pincho.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_pincho.SelectedItem.Text == "Si")
            {
                creador.actualizar_dato(id, "pincho", "si");
            }
            else
            {
                creador.actualizar_dato(id, "pincho", "no");
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void textbox_equivalencia_pincho_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_equivalencia_pincho = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_equivalencia_pincho.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (textbox_equivalencia_pincho.Text != string.Empty)
            {
                creador.actualizar_dato(id, "equivalencia_pincho", textbox_equivalencia_pincho.Text);
            }
            productos_terminadosBD = creador.get_productos_terminados();
            Session.Add("productos_terminadosBD", productos_terminadosBD);
            cargar_productos();
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                cargar_datos_producto();
                creador.crear_producto(producto);
                texbox_producto_nuevo.Text = string.Empty;
                textbox_nuevo_tipo.Text = string.Empty;
                textbox_presentacion_nuevo.Text = string.Empty;
                productos_terminadosBD = creador.get_productos_terminados();
                Session.Add("productos_terminadosBD", productos_terminadosBD);
                Session.Add("categoriasBD", creador.get_categorias());
                configurar_controles();
                cargar_productos();
            }
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }
    }
}