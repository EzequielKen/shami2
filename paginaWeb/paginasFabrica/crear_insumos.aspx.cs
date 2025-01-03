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
    public partial class crear_insumos : System.Web.UI.Page
    {
        #region carga a base de datos
        private void crear_tabla_insumo()
        {
            insumo = new DataTable();
            insumo.Columns.Add("producto", typeof(string));
            insumo.Columns.Add("tipo_producto", typeof(string));
            insumo.Columns.Add("alimento", typeof(string));
            insumo.Columns.Add("bebida", typeof(string));
            insumo.Columns.Add("descartable", typeof(string));
            insumo.Columns.Add("tipo_producto_local", typeof(string));
            insumo.Columns.Add("unidad_tabla_produccion", typeof(string));
            insumo.Columns.Add("venta", typeof(string));
            insumo.Columns.Add("productos_fabrica_fatay", typeof(string));
            insumo.Columns.Add("productos_caballito", typeof(string));
            insumo.Columns.Add("unidad_de_medida_local", typeof(string));
            insumo.Columns.Add("equivalencia", typeof(string));
            insumo.Columns.Add("unidad_medida", typeof(string));
            insumo.Columns.Add("tabla_produccion", typeof(string));
        }
        private void cargar_datos_insumo()
        {
            crear_tabla_insumo();
            insumo.Rows.Clear();
            insumo.Rows.Add();

            insumo.Rows[0]["producto"] = texbox_producto_nuevo.Text.Replace("-", "/");

            if (textbox_nuevo_tipo.Text == string.Empty)
            {
                insumo.Rows[0]["tipo_producto"] = dropdown_tipo_producto_nuevo.SelectedItem.Text;
            }
            else
            {
                int ultimo_num_categoria = int.Parse(creador_insumos.get_ultimo_num_categoria()) + 1;
                string nuevo_tipo = textbox_nuevo_tipo.Text.Replace("-", "/");
                insumo.Rows[0]["tipo_producto"] = ultimo_num_categoria.ToString() + "-" + nuevo_tipo;
            }

            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Alimento")
            {
                insumo.Rows[0]["alimento"] = "1";
            }
            else
            {
                insumo.Rows[0]["alimento"] = "0";

            }
            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Bebida")
            {
                insumo.Rows[0]["bebida"] = "1";
            }
            else
            {
                insumo.Rows[0]["bebida"] = "0";

            }
            if (dropdown_categoria_producto_nuevo.SelectedItem.Text == "Descartable")
            {
                insumo.Rows[0]["descartable"] = "1";
            }
            else
            {
                insumo.Rows[0]["descartable"] = "0";

            }

            insumo.Rows[0]["tipo_producto_local"] = "N/A";
            insumo.Rows[0]["unidad_tabla_produccion"] = "N/A";

            if (dropdown_venta_nuevo.SelectedItem.Text == "Si")
            {
                insumo.Rows[0]["venta"] = "1";
            }
            else
            {
                insumo.Rows[0]["venta"] = "0";
            }

            if (dropdown_fabrica_fatay_nuevo.SelectedItem.Text == "Si")
            {
                insumo.Rows[0]["productos_fabrica_fatay"] = "1";
            }
            else
            {
                insumo.Rows[0]["productos_fabrica_fatay"] = "0";
            }

            if (dropdown_caballito_nuevo.SelectedItem.Text == "Si")
            {
                insumo.Rows[0]["productos_caballito"] = "1";
            }
            else
            {
                insumo.Rows[0]["productos_caballito"] = "0";
            }

            string unidad_de_medida_local = dropdown_paquete_nuevo.SelectedItem.Text + "-" +
                                            textbox_unidad_nuevo.Text + "-" +
                                            dropdown_tipo_unidad.SelectedItem.Text;

            insumo.Rows[0]["unidad_de_medida_local"] = unidad_de_medida_local;
            insumo.Rows[0]["equivalencia"] = unidad_de_medida_local;
            insumo.Rows[0]["unidad_medida"] = dropdown_tipo_unidad.SelectedItem.Text;

            insumo.Rows[0]["tabla_produccion"] = "0";
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

            if (textbox_unidad_nuevo.Text == string.Empty)
            {
                textbox_unidad_nuevo.CssClass = "form-control bg-danger";
                retorno = false;
            }
            else
            {
                textbox_unidad_nuevo.CssClass = "form-control";
            }
            return retorno;
        }
        #endregion
        #region carga de insumos 
        private void crear_tabla_insumos()
        {
            insumos = new DataTable();
            insumos.Columns.Add("id", typeof(string));
            insumos.Columns.Add("producto", typeof(string));
            insumos.Columns.Add("tipo_producto", typeof(string));
            insumos.Columns.Add("alimento", typeof(string));
            insumos.Columns.Add("bebida", typeof(string));
            insumos.Columns.Add("descartable", typeof(string));
            insumos.Columns.Add("tipo_producto_local", typeof(string));
            insumos.Columns.Add("unidad_tabla_produccion", typeof(string));
            insumos.Columns.Add("venta", typeof(string));
            insumos.Columns.Add("productos_fabrica_fatay", typeof(string));
            insumos.Columns.Add("productos_caballito", typeof(string));
            insumos.Columns.Add("unidad_de_medida_local", typeof(string));
            insumos.Columns.Add("equivalencia", typeof(string));
        }
        private void llenar_tabla_insumo()
        {
            crear_tabla_insumos();
            insumosBD = (DataTable)Session["insumoBD"];
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, insumosBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(dropDown_tipo.SelectedItem.Text, insumosBD.Rows[fila]["tipo_producto"].ToString()))
                {
                    insumos.Rows.Add();
                    insumos.Rows[fila_insumo]["id"] = insumosBD.Rows[fila]["id"].ToString();
                    insumos.Rows[fila_insumo]["producto"] = insumosBD.Rows[fila]["producto"].ToString();
                    insumos.Rows[fila_insumo]["tipo_producto"] = insumosBD.Rows[fila]["tipo_producto"].ToString();
                    insumos.Rows[fila_insumo]["alimento"] = insumosBD.Rows[fila]["alimento"].ToString();
                    insumos.Rows[fila_insumo]["bebida"] = insumosBD.Rows[fila]["bebida"].ToString();
                    insumos.Rows[fila_insumo]["descartable"] = insumosBD.Rows[fila]["descartable"].ToString();
                    insumos.Rows[fila_insumo]["tipo_producto_local"] = insumosBD.Rows[fila]["tipo_producto_local"].ToString();
                    insumos.Rows[fila_insumo]["unidad_tabla_produccion"] = insumosBD.Rows[fila]["unidad_tabla_produccion"].ToString();
                    insumos.Rows[fila_insumo]["venta"] = insumosBD.Rows[fila]["venta"].ToString();
                    insumos.Rows[fila_insumo]["productos_fabrica_fatay"] = insumosBD.Rows[fila]["productos_fabrica_fatay"].ToString();
                    insumos.Rows[fila_insumo]["productos_caballito"] = insumosBD.Rows[fila]["productos_caballito"].ToString();
                    insumos.Rows[fila_insumo]["unidad_de_medida_local"] = insumosBD.Rows[fila]["unidad_de_medida_local"].ToString();
                    insumos.Rows[fila_insumo]["equivalencia"] = insumosBD.Rows[fila]["equivalencia"].ToString();

                    fila_insumo++;
                }
            }
        }
        private void cargar_insumos()
        {
            llenar_tabla_insumo();
            gridview_productos.DataSource = insumos;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            insumosBD = (DataTable)Session["insumoBD"];
            llenar_dropDownList(insumosBD);
            llenar_TipoProducto((DataTable)Session["categoriasBD"]);
            llenar_TipoProducto_local(insumosBD);
            llenar_unidad_tabla_produccion(insumosBD);
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
        private void llenar_TipoProducto_local(DataTable dt)
        {
            dropdown_tipo_producto_local_nuevo.Items.Clear();
            int num_item = 1;
            ListItem item;
            // dt.DefaultView.Sort = "tipo_producto asc";
            // dt = dt.DefaultView.ToTable();
            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropdown_tipo_producto_local_nuevo.Items.Add(dt.Rows[0]["tipo_producto"].ToString());
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {

                if (dropdown_tipo_producto_local_nuevo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {

                    string dato = dt.Rows[fila]["tipo_producto"].ToString();
                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropdown_tipo_producto_local_nuevo.Items.Add(dt.Rows[fila]["tipo_producto"].ToString());
                    num_item = num_item + 1;

                }

            }

        }
        private void llenar_unidad_tabla_produccion(DataTable dt)
        {
            dropdown_unidad_tabla_produccion_nuevo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "unidad_tabla_produccion asc";
            dt = dt.DefaultView.ToTable();
            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["unidad_tabla_produccion"].ToString();
            item = new ListItem(dt.Rows[0]["unidad_tabla_produccion"].ToString(), num_item.ToString());
            dropdown_unidad_tabla_produccion_nuevo.Items.Add(dt.Rows[0]["unidad_tabla_produccion"].ToString());
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropdown_unidad_tabla_produccion_nuevo.Items[num_item - 2].Text != dt.Rows[fila]["unidad_tabla_produccion"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["unidad_tabla_produccion"].ToString(), num_item.ToString());
                    dropdown_unidad_tabla_produccion_nuevo.Items.Add(dt.Rows[fila]["unidad_tabla_produccion"].ToString());
                    num_item = num_item + 1;

                }

            }
            dropdown_unidad_tabla_produccion_nuevo.SelectedValue = "N/A";
        }

        #endregion

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_insumos creador_insumos;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable insumosBD;
        DataTable insumos;
        DataTable insumo;
        string tipo_seleccionado;

        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            creador_insumos = new cls_crear_insumos(usuariosBD);

            if (!IsPostBack)
            {
                insumosBD = creador_insumos.get_insumos_fabrica();
                Session.Add("insumoBD", insumosBD);
                Session.Add("categoriasBD", creador_insumos.get_categorias());
                configurar_controles();
                cargar_insumos();
            }
        }

        #region carga de insumos
        protected void gridview_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            insumosBD = (DataTable)Session["insumoBD"];
            string id;
            int fila_producto;
            for (int fila = 0; fila <= gridview_productos.Rows.Count - 1; fila++)
            {
                id = gridview_productos.Rows[fila].Cells[0].Text;
                fila_producto = funciones.buscar_fila_por_id(id, insumosBD);
                //ACTIVA
                DropDownList dropdown_activo = (gridview_productos.Rows[fila].Cells[1].FindControl("dropdown_activo") as DropDownList);
                if (insumosBD.Rows[fila_producto]["activa"].ToString() == "1")
                {
                    dropdown_activo.SelectedValue = "Si";
                }
                else
                {
                    dropdown_activo.SelectedValue = "No";
                }
                //PRODUCTO
                TextBox texbox_producto = (gridview_productos.Rows[fila].Cells[2].FindControl("texbox_producto") as TextBox);
                texbox_producto.Text = insumosBD.Rows[fila_producto]["producto"].ToString();

                //TIPO PRODUCTO
                DropDownList dropdown_tipo_producto = (gridview_productos.Rows[fila].Cells[3].FindControl("dropdown_tipo_producto") as DropDownList);
                dropdown_tipo_producto.Items.Clear();
                int num_item = 1;
                ListItem item;

                //        item = new ListItem("Todos", num_item.ToString());
                //        dropDown_tipo.Items.Add(item);
                //        num_item = num_item + 1;

                tipo_seleccionado = insumosBD.Rows[0]["tipo_producto"].ToString();
                item = new ListItem(insumosBD.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
                dropdown_tipo_producto.Items.Add(item);
                num_item = num_item + 1;
                for (int fila_insumos = 0; fila_insumos <= insumosBD.Rows.Count - 1; fila_insumos++)
                {


                    if (dropdown_tipo_producto.Items[num_item - 2].Text != insumosBD.Rows[fila_insumos]["tipo_producto"].ToString())
                    {

                        dropdown_tipo_producto.Items.Add(insumosBD.Rows[fila_insumos]["tipo_producto"].ToString());
                        num_item = num_item + 1;
                    }

                }
                dropdown_tipo_producto.SelectedValue = insumosBD.Rows[fila_producto]["tipo_producto"].ToString();

                //CATEGORIA PRODUCTO
                DropDownList dropdown_categoria_producto = (gridview_productos.Rows[fila].Cells[4].FindControl("dropdown_categoria_producto") as DropDownList);
                if (insumosBD.Rows[fila_producto]["alimento"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Alimento";
                }
                if (insumosBD.Rows[fila_producto]["bebida"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Bebida";
                }
                if (insumosBD.Rows[fila_producto]["descartable"].ToString() == "1")
                {
                    dropdown_categoria_producto.SelectedValue = "Descartable";
                }

                //VENTA

                DropDownList dropdown_venta = (gridview_productos.Rows[fila].Cells[5].FindControl("dropdown_venta") as DropDownList);
                fila_producto = funciones.buscar_fila_por_id(id, insumosBD);
                int venta = int.Parse(insumosBD.Rows[fila_producto]["venta"].ToString());
                if (venta != 0)
                {
                    dropdown_venta.SelectedValue = "Si";
                }
                else
                {
                    dropdown_venta.SelectedValue = "No";
                }

                //PRODUCTO FABRICA FATAY
                DropDownList dropdown_fabrica_fatay = (gridview_productos.Rows[fila].Cells[6].FindControl("dropdown_fabrica_fatay") as DropDownList);
                fila_producto = funciones.buscar_fila_por_id(id, insumosBD);
                int productos_fabrica_fatay = int.Parse(insumosBD.Rows[fila_producto]["productos_fabrica_fatay"].ToString());
                if (productos_fabrica_fatay != 0)
                {
                    dropdown_fabrica_fatay.SelectedValue = "Si";
                }
                else
                {
                    dropdown_fabrica_fatay.SelectedValue = "No";
                }

                //PRODUCTO CABALLITO
                DropDownList dropdown_caballito = (gridview_productos.Rows[fila].Cells[7].FindControl("dropdown_caballito") as DropDownList);
                fila_producto = funciones.buscar_fila_por_id(id, insumosBD);
                int productos_caballito = int.Parse(insumosBD.Rows[fila_producto]["productos_caballito"].ToString());
                if (productos_caballito != 0)
                {
                    dropdown_caballito.SelectedValue = "Si";
                }
                else
                {
                    dropdown_caballito.SelectedValue = "No";
                }

                //PRESENTACION VENTA
                DropDownList dropdown_paquete = (gridview_productos.Rows[fila].Cells[8].FindControl("dropdown_paquete") as DropDownList);
                TextBox textbox_unidad = (gridview_productos.Rows[fila].Cells[8].FindControl("textbox_unidad") as TextBox);
                DropDownList dropdown_tipo_unidad = (gridview_productos.Rows[fila].Cells[8].FindControl("dropdown_tipo_unidad") as DropDownList);
                fila_producto = funciones.buscar_fila_por_id(id, insumosBD);

                string paquete = funciones.obtener_dato(insumosBD.Rows[fila_producto]["unidad_de_medida_local"].ToString(), 1);
                string unidad = funciones.obtener_dato(insumosBD.Rows[fila_producto]["unidad_de_medida_local"].ToString(), 2);
                string tipo_unidad = funciones.obtener_dato(insumosBD.Rows[fila_producto]["unidad_de_medida_local"].ToString(), 3);

                dropdown_paquete.SelectedValue = paquete;
                textbox_unidad.Text = unidad;
                dropdown_tipo_unidad.SelectedValue = tipo_unidad;

            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }
        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            if (verificar_carga())
            {
                cargar_datos_insumo();
                creador_insumos.crear_insumo(insumo);
                texbox_producto_nuevo.Text = string.Empty;
                textbox_unidad_nuevo.Text = string.Empty;
                textbox_nuevo_tipo.Text = string.Empty;
                insumosBD = creador_insumos.get_insumos_fabrica();
                Session.Add("insumoBD", insumosBD);
                Session.Add("categoriasBD", creador_insumos.get_categorias());
                configurar_controles();
                cargar_insumos();
            }
        }
        #endregion

        #region modificar insumos


        protected void dropdown_activo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_activo = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_activo.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_activo.SelectedItem.Text == "Si")
            {
                creador_insumos.actualizar_dato(id, "activa", "1");
            }
            else
            {
                creador_insumos.actualizar_dato(id, "activa", "0");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();

        }

        protected void texbox_producto_TextChanged(object sender, EventArgs e)
        {
            TextBox texbox_producto = (TextBox)sender;
            GridViewRow row = (GridViewRow)texbox_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (texbox_producto.Text != string.Empty)
            {
                creador_insumos.actualizar_dato(id, "producto", texbox_producto.Text);
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_tipo_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_tipo_producto = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            creador_insumos.actualizar_dato(id, "tipo_producto", dropdown_tipo_producto.SelectedItem.Text);
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_categoria_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_categoria_producto = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_categoria_producto.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_categoria_producto.SelectedItem.Text == "Alimento")
            {
                creador_insumos.actualizar_dato(id, "alimento", "1");
                creador_insumos.actualizar_dato(id, "bebida", "0");
                creador_insumos.actualizar_dato(id, "descartable", "0");
            }
            else if (dropdown_categoria_producto.SelectedItem.Text == "Bebida")
            {
                creador_insumos.actualizar_dato(id, "alimento", "0");
                creador_insumos.actualizar_dato(id, "bebida", "1");
                creador_insumos.actualizar_dato(id, "descartable", "0");
            }
            else if (dropdown_categoria_producto.SelectedItem.Text == "Descartable")
            {
                creador_insumos.actualizar_dato(id, "alimento", "0");
                creador_insumos.actualizar_dato(id, "bebida", "0");
                creador_insumos.actualizar_dato(id, "descartable", "1");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_tipo_producto_local_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_tipo_producto_local = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_producto_local.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            creador_insumos.actualizar_dato(id, "tipo_producto_local", dropdown_tipo_producto_local.SelectedItem.Text);
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_unidad_tabla_produccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_unidad_tabla_produccion = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_unidad_tabla_produccion.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            creador_insumos.actualizar_dato(id, "unidad_tabla_produccion", dropdown_unidad_tabla_produccion.SelectedItem.Text);
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_venta_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_venta = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_venta.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_venta.SelectedItem.Text == "Si")
            {
                creador_insumos.actualizar_dato(id, "venta", "1");
            }
            else
            {
                creador_insumos.actualizar_dato(id, "venta", "0");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }

        protected void dropdown_fabrica_fatay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_fabrica_fatay = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_fabrica_fatay.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_fabrica_fatay.SelectedItem.Text == "Si")
            {
                creador_insumos.actualizar_dato(id, "productos_fabrica_fatay", "1");
            }
            else
            {
                creador_insumos.actualizar_dato(id, "productos_fabrica_fatay", "0");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }

        protected void dropdown_caballito_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_caballito = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_caballito.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_caballito.SelectedItem.Text == "Si")
            {
                creador_insumos.actualizar_dato(id, "productos_caballito", "1");
            }
            else
            {
                creador_insumos.actualizar_dato(id, "productos_caballito", "0");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }

        protected void dropdown_paquete_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_paquete = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_paquete.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            TextBox textbox_unidad = (gridview_productos.Rows[fila].Cells[10].FindControl("textbox_unidad") as TextBox);
            DropDownList dropdown_tipo_unidad = (gridview_productos.Rows[fila].Cells[10].FindControl("dropdown_tipo_unidad") as DropDownList);

            string paquete = dropdown_paquete.SelectedItem.Text;
            string unidad = textbox_unidad.Text;
            string tipo_unidad = dropdown_tipo_unidad.SelectedItem.Text;

            string dato = paquete + "-" + unidad + "-" + tipo_unidad;
            creador_insumos.actualizar_dato(id, "unidad_de_medida_local", dato);
            creador_insumos.actualizar_dato(id, "equivalencia", dato);
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void textbox_unidad_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox_unidad = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_unidad.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (textbox_unidad.Text != string.Empty && int.Parse(textbox_unidad.Text) > 0)
            {
                DropDownList dropdown_paquete = (gridview_productos.Rows[fila].Cells[10].FindControl("dropdown_paquete") as DropDownList);
                DropDownList dropdown_tipo_unidad = (gridview_productos.Rows[fila].Cells[10].FindControl("dropdown_tipo_unidad") as DropDownList);

                string paquete = dropdown_paquete.SelectedItem.Text;
                string unidad = textbox_unidad.Text;
                string tipo_unidad = dropdown_tipo_unidad.SelectedItem.Text;

                string dato = paquete + "-" + unidad + "-" + tipo_unidad;
                creador_insumos.actualizar_dato(id, "unidad_de_medida_local", dato);
                creador_insumos.actualizar_dato(id, "equivalencia", dato);
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }

        protected void dropdown_tipo_unidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_tipo_unidad = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tipo_unidad.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            DropDownList dropdown_paquete = (gridview_productos.Rows[fila].Cells[10].FindControl("dropdown_paquete") as DropDownList);
            TextBox textbox_unidad = (gridview_productos.Rows[fila].Cells[10].FindControl("textbox_unidad") as TextBox);

            string paquete = dropdown_paquete.SelectedItem.Text;
            string unidad = textbox_unidad.Text;
            string tipo_unidad = dropdown_tipo_unidad.SelectedItem.Text;

            string dato = paquete + "-" + unidad + "-" + tipo_unidad;
            creador_insumos.actualizar_dato(id, "unidad_de_medida_local", dato);
            creador_insumos.actualizar_dato(id, "equivalencia", dato);
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }


        protected void dropdown_tabla_produccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_tabla_produccion = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_tabla_produccion.NamingContainer;
            int fila = row.RowIndex;

            string id = gridview_productos.Rows[fila].Cells[0].Text;

            if (dropdown_tabla_produccion.SelectedItem.Text == "Si")
            {
                creador_insumos.actualizar_dato(id, "tabla_produccion", "1");
            }
            else
            {
                creador_insumos.actualizar_dato(id, "tabla_produccion", "0");
            }
            insumosBD = creador_insumos.get_insumos_fabrica();
            Session.Add("insumoBD", insumosBD);
            cargar_insumos();
        }

        #endregion

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }
    }
}