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
    public partial class recetario_crear_sub_producto : System.Web.UI.Page
    {
        #region linkear productos
        private void crear_sub_producto_enBD()
        {
            if (verificar_insumos_de_proveedorBD())
            {
                label_mensaje_de_alerta.Visible = false;
                insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
                crear_sub_producto.agregar_sub_producto(insumos_de_proveedorBD,label_tipo_receta.Text,textbox_nombre.Text);
                Session.Remove("insumos_de_proveedor");
                Response.Redirect("/paginasFabrica/recetario_linkear_sub_producto.aspx", false);
            }
            else
            {
                label_mensaje_de_alerta.Visible = true;
            }
        }
        #endregion
        #region cargar insumos a proveedor
        private void cargar_insumo_en_proveedor(string id_insumo)
        {
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            if (!verificar_si_esta_cargado(id_insumo))
            {
                insumos_de_proveedorBD.Rows.Add();
                int ultima_fila = insumos_de_proveedorBD.Rows.Count - 1;
                int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumosBD);

                insumos_de_proveedorBD.Rows[ultima_fila]["id"] = insumosBD.Rows[fila_insumo]["id"].ToString();
                insumos_de_proveedorBD.Rows[ultima_fila]["producto"] = insumosBD.Rows[fila_insumo]["producto"].ToString();
                string t = insumosBD.Rows[fila_insumo]["tipo_producto"].ToString();
                insumos_de_proveedorBD.Rows[ultima_fila]["tipo_producto"] = insumosBD.Rows[fila_insumo]["tipo_producto"].ToString();
                insumos_de_proveedorBD.Rows[ultima_fila]["unidad_medida"] = insumosBD.Rows[fila_insumo]["unidad_medida"].ToString();


                //cantidad_unidades
                insumos_de_proveedorBD.Rows[ultima_fila]["cantidad"] = "N/A";

                llenar_dropDownList_insumos_proveedor(insumos_de_proveedorBD);
                Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);

                Session.Add("insumos_de_proveedor", insumos_de_proveedorBD);
            }
        }
        private bool verificar_si_esta_cargado(string id_insumo)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= insumos_de_proveedorBD.Rows.Count - 1)
            {
                if (id_insumo == insumos_de_proveedorBD.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion
        #region insumos del proveedor
        private void crear_tabla_insumos_de_proveedor()
        {
            insumos_de_proveedor = new DataTable();

            insumos_de_proveedor.Columns.Add("id", typeof(string));
            insumos_de_proveedor.Columns.Add("producto", typeof(string));
            insumos_de_proveedor.Columns.Add("tipo_producto", typeof(string));
            insumos_de_proveedor.Columns.Add("unidad_medida", typeof(string));

            insumos_de_proveedor.Columns.Add("cantidad", typeof(string));
        }

        private void crear_tabla_insumos_de_proveedorBD()
        {
            insumos_de_proveedorBD = new DataTable();

            insumos_de_proveedorBD.Columns.Add("id", typeof(string));
            insumos_de_proveedorBD.Columns.Add("producto", typeof(string));
            insumos_de_proveedorBD.Columns.Add("tipo_producto", typeof(string));
            insumos_de_proveedorBD.Columns.Add("unidad_medida", typeof(string));


            insumos_de_proveedorBD.Columns.Add("cantidad", typeof(string));

            Session.Add("insumos_de_proveedor", insumos_de_proveedorBD);
        }

        private void llenar_tabla_insumos_de_proveedorBD()
        {
            if (Session["insumos_de_proveedor"] == null)
            {
                crear_tabla_insumos_de_proveedorBD();
            }
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            if (acuerdo_de_precios_fabrica_a_proveedoresBD.Rows.Count > 0)
            {
                string id_producto, tipo_paquete, cantidad_unidades, unidad_medida;
                int fila_insumo;
                int fila = 0;
                for (int columna = acuerdo_de_precios_fabrica_a_proveedoresBD.Columns["producto_1"].Ordinal; columna <= acuerdo_de_precios_fabrica_a_proveedoresBD.Columns.Count - 1; columna++)
                {
                    if (funciones.IsNotDBNull(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna]))
                    {
                        if (acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString() != "N/A")
                        {
                            id_producto = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 1);
                            fila_insumo = funciones.buscar_fila_por_id(id_producto, insumosBD);
                            insumos_de_proveedorBD.Rows.Add();

                            //id
                            insumos_de_proveedorBD.Rows[fila]["id"] = id_producto;
                            //producto 
                            insumos_de_proveedorBD.Rows[fila]["producto"] = insumosBD.Rows[fila_insumo]["producto"].ToString();

                            //tipo_producto
                            insumos_de_proveedorBD.Rows[fila]["tipo_producto"] = insumosBD.Rows[fila_insumo]["tipo_producto"].ToString();

                            //tipo_paquete
                            tipo_paquete = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 3);
                            insumos_de_proveedorBD.Rows[fila]["tipo_paquete"] = tipo_paquete;

                            //cantidad_unidades
                            cantidad_unidades = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 4);
                            insumos_de_proveedorBD.Rows[fila]["cantidad_unidades"] = cantidad_unidades;

                            //unidad_medida
                            unidad_medida = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 5);
                            insumos_de_proveedorBD.Rows[fila]["unidad_medida"] = unidad_medida;

                            //precio
                            insumos_de_proveedorBD.Rows[fila]["precio"] = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 6);

                            if (tipo_paquete == "Unidad")
                            {
                                insumos_de_proveedorBD.Rows[fila]["presentacion"] = cantidad_unidades + " x " + unidad_medida;
                            }
                            else
                            {
                                insumos_de_proveedorBD.Rows[fila]["presentacion"] = tipo_paquete + " x " + cantidad_unidades + " " + unidad_medida;
                            }
                            //presentacion


                            fila++;

                        }
                        //insumos_de_proveedorBD
                    }
                }
                llenar_dropDownList_insumos_proveedor(insumos_de_proveedorBD);
                Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);
            }
            Session.Add("insumos_de_proveedor", insumos_de_proveedorBD);
        }
        private void llenar_tabla_insumos_de_proveedor()
        {
            crear_tabla_insumos_de_proveedor();
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            int fila_insumo = 0;
            string tipo_producto, tipo_seleccionado;
            for (int fila = 0; fila <= insumos_de_proveedorBD.Rows.Count - 1; fila++)
            {
                tipo_producto = insumos_de_proveedorBD.Rows[fila]["tipo_producto"].ToString();
                tipo_seleccionado = Session["tipo_seleccionado_proveedor"].ToString();
                if (funciones.buscar_alguna_coincidencia(textbox_buscar_insumos_proveedor.Text, insumos_de_proveedorBD.Rows[fila]["producto"].ToString()) &&
                    tipo_producto == tipo_seleccionado)
                {
                    insumos_de_proveedor.Rows.Add();
                    insumos_de_proveedor.Rows[fila_insumo]["id"] = insumos_de_proveedorBD.Rows[fila]["id"].ToString();
                    insumos_de_proveedor.Rows[fila_insumo]["producto"] = insumos_de_proveedorBD.Rows[fila]["producto"].ToString();
                    insumos_de_proveedor.Rows[fila_insumo]["tipo_producto"] = insumos_de_proveedorBD.Rows[fila]["tipo_producto"].ToString();
                    insumos_de_proveedor.Rows[fila_insumo]["unidad_medida"] = insumos_de_proveedorBD.Rows[fila]["unidad_medida"].ToString();

                    //cantidad 
                    insumos_de_proveedor.Rows[fila_insumo]["cantidad"] = insumos_de_proveedorBD.Rows[fila]["cantidad"].ToString();

                    fila_insumo++;
                }
            }
        }
        private void cargar_insumos_proveedor()
        {
            llenar_tabla_insumos_de_proveedor();

            gridview_insumos_del_proveedor.DataSource = insumos_de_proveedor;
            gridview_insumos_del_proveedor.DataBind();

            label_tipo_receta.Text = verificar_tipo_de_receta();
        }
        private void llenar_dropDownList_insumos_proveedor(DataTable dt)
        {
            dropDown_tipo_insumos_proveedor.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                int num_item = 1;
                ListItem item;
                dt.DefaultView.Sort = "tipo_producto";
                dt = dt.DefaultView.ToTable();

                //        item = new ListItem("Todos", num_item.ToString());
                //        dropDown_tipo.Items.Add(item);
                //        num_item = num_item + 1;

                tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
                item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
                dropDown_tipo_insumos_proveedor.Items.Add(item);
                num_item = num_item + 1;
                for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
                {
                    if (dropDown_tipo_insumos_proveedor.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                    {
                        item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                        dropDown_tipo_insumos_proveedor.Items.Add(item);
                        num_item = num_item + 1;
                    }
                }
                Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);
            }
        }
        #endregion
        #region insumos
        private void llenar_dropDownList_insumos(DataTable dt)
        {
            dropDown_tipo_insumos.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropDown_tipo_insumos.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {
                if (dropDown_tipo_insumos.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropDown_tipo_insumos.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        private void crear_tabla_insumos()
        {
            insumos = new DataTable();
            insumos.Columns.Add("id", typeof(string));
            insumos.Columns.Add("producto", typeof(string));
            insumos.Columns.Add("stock", typeof(string));
            insumos.Columns.Add("unidad_medida", typeof(string));

        }
        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            int fila_insumos = 0;
            for (int fila = 0; fila <= insumosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(texbox_buscar_insumos.Text, insumosBD.Rows[fila]["producto"].ToString()) &&
                    insumosBD.Rows[fila]["tipo_producto"].ToString() == Session["tipo_seleccionado"].ToString())
                {
                    insumos.Rows.Add();

                    insumos.Rows[fila_insumos]["id"] = insumosBD.Rows[fila]["id"].ToString();
                    insumos.Rows[fila_insumos]["producto"] = insumosBD.Rows[fila]["producto"].ToString();
                    insumos.Rows[fila_insumos]["stock"] = insumosBD.Rows[fila]["stock"].ToString();
                    insumos.Rows[fila_insumos]["unidad_medida"] = insumosBD.Rows[fila]["unidad_medida"].ToString();

                    fila_insumos++;
                }
            }
        }

        private void cargar_insumos()
        {
            llenar_tabla_insumos();
            gridview_insumos.DataSource = insumos;
            gridview_insumos.DataBind();
        }
        #endregion
        #region funciones
        private bool verificar_insumos_de_proveedorBD()
        {
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            bool retorno = true;
            int fila = 0;
            while (fila <= insumos_de_proveedorBD.Rows.Count - 1)
            {
                if (insumos_de_proveedorBD.Rows[fila]["cantidad"].ToString() == "N/A" ||
                    textbox_nombre.Text == string.Empty)
                {
                    retorno = false;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private string verificar_tipo_de_receta()
        {
            string retorno = "Simple";
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            if (insumos_de_proveedorBD.Rows.Count > 0)
            {
                retorno = "Compuesto";
            }
            return retorno;
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_crear_sub_producto crear_sub_producto;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        string tipo_seleccionado;
        string id_proveedor_fabrica_seleccionado;
        DataTable proveedor_fabrica_seleccionado;
        DataTable insumosBD;
        DataTable insumos;
        DataTable acuerdo_de_precios_fabrica_a_proveedoresBD;
        DataTable insumos_de_proveedorBD;
        DataTable insumos_de_proveedor;
        protected void Page_Load(object sender, EventArgs e)
        {
            //  id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["crear_sub_producto"] == null)
            {
                Session.Add("crear_sub_producto", new cls_crear_sub_producto(usuariosBD));
            }
            crear_sub_producto = (cls_crear_sub_producto)Session["crear_sub_producto"];
            insumosBD = crear_sub_producto.get_insumos();
            if (!IsPostBack)
            {
                Session.Remove("insumos_de_proveedor");
                crear_tabla_insumos_de_proveedorBD();
                llenar_dropDownList_insumos(insumosBD);
                Session.Add("tipo_seleccionado", dropDown_tipo_insumos.SelectedItem.Text);


                //    llenar_tabla_insumos_de_proveedorBD();

                cargar_insumos();
                cargar_insumos_proveedor();
            }
        }

        #region lista de insumos
        protected void dropDown_tipo_insumos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo_insumos.SelectedItem.Text);
            cargar_insumos();
        }

        protected void texbox_buscar_insumos_TextChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo_insumos.SelectedItem.Text);
            cargar_insumos();
        }

        protected void gridview_insumos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_insumo_en_proveedor(gridview_insumos.SelectedRow.Cells[0].Text);
            cargar_insumos_proveedor();
        }

        #endregion

        protected void boton_guardar_Click(object sender, EventArgs e)
        {
            crear_sub_producto_enBD();
        }

        protected void dropDown_tipo_insumos_proveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);
            cargar_insumos_proveedor();
        }

        protected void textbox_buscar_insumos_proveedor_TextChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);
            cargar_insumos_proveedor();
        }

        protected void gridview_insumos_del_proveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridview_insumos_del_proveedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_aplicar_cambios")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
                int fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, insumos_de_proveedorBD);
                
                double cantidad;
                TextBox textbox_cantidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[3].FindControl("textbox_cantidad") as TextBox);
                if (textbox_cantidad.Text == string.Empty)
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["cantidad"] = "N/A";
                }
                else if (double.TryParse(textbox_cantidad.Text, out cantidad))
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["cantidad"] = cantidad.ToString();
                }
                else
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["cantidad"] = "N/A";
                }
                Session.Add("insumos_de_proveedor", insumos_de_proveedorBD);
                Session.Add("tipo_seleccionado_proveedor", dropDown_tipo_insumos_proveedor.SelectedItem.Text);
                cargar_insumos_proveedor();

            }
            else if (e.CommandName == "boton_eliminar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
                int fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, insumos_de_proveedorBD);

                insumos_de_proveedorBD.Rows[fila_tabla].Delete();

                llenar_dropDownList_insumos_proveedor(insumos_de_proveedorBD);

                Session.Add("insumos_de_proveedor", insumos_de_proveedorBD);
                cargar_insumos_proveedor();
            }
        }

        protected void dropdown_tipo_paquete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}