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
    public partial class linkear_insumos_a_proveedor : System.Web.UI.Page
    {
        private string obtener_presentacion_recomendada(string dato)
        {
            string retorno ="";
            string tipo_paquete,unidades,unidad;
            tipo_paquete = funciones.obtener_dato(dato,1);
            unidades = funciones.obtener_dato(dato,2);
            unidad = funciones.obtener_dato(dato,3);
            retorno = tipo_paquete +" - "+ unidades + " - " + unidad;
            return retorno;
        }
        #region linkear productos
        private void linkear_productos_a_proveedor()
        {
            if (verificar_insumos_de_proveedorBD())
            {
                insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
                id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();

                linkear.linkear_insumos_a_proveedor(acuerdo_de_precios_fabrica_a_proveedoresBD, id_proveedor_fabrica_seleccionado, insumos_de_proveedorBD);
                acuerdo_de_precios_fabrica_a_proveedoresBD = linkear.get_acuerdo_de_precios_fabrica_a_proveedores(id_proveedor_fabrica_seleccionado);

                Session.Remove("insumos_de_proveedor");

                llenar_dropDownList_insumos(insumosBD);
                Session.Add("tipo_seleccionado", dropDown_tipo_insumos.SelectedItem.Text);


                llenar_tabla_insumos_de_proveedorBD();

                cargar_insumos();
                cargar_insumos_proveedor();
                cargar_datos_proveedor();
                label_mensaje_de_alerta.Visible = false;
                label_mensaje_de_exito.Visible=true;
            }
            else
            {
                label_mensaje_de_alerta.Visible = true;
                label_mensaje_de_exito.Visible = false;
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

                //tipo_paquete
                insumos_de_proveedorBD.Rows[ultima_fila]["tipo_paquete"] = "N/A";
                //cantidad_unidades
                insumos_de_proveedorBD.Rows[ultima_fila]["cantidad_unidades"] = "1";
                //unidad_medida
                insumos_de_proveedorBD.Rows[ultima_fila]["unidad_medida"] = insumosBD.Rows[fila_insumo]["unidad_medida"].ToString();
                //precio
                insumos_de_proveedorBD.Rows[ultima_fila]["precio"] = "0";
                insumos_de_proveedorBD.Rows[ultima_fila]["precio_unidad"] = "N/A";
                //precio
                insumos_de_proveedorBD.Rows[ultima_fila]["presentacion"] = "N/A";

                insumos_de_proveedorBD.Rows[ultima_fila]["producto_1"] = obtener_presentacion_recomendada(insumosBD.Rows[fila_insumo]["producto_1"].ToString());


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

            insumos_de_proveedor.Columns.Add("tipo_paquete", typeof(string));
            insumos_de_proveedor.Columns.Add("cantidad_unidades", typeof(string));
            insumos_de_proveedor.Columns.Add("unidad_medida", typeof(string));

            insumos_de_proveedor.Columns.Add("precio", typeof(string));
            insumos_de_proveedor.Columns.Add("precio_unidad", typeof(string));

            insumos_de_proveedor.Columns.Add("presentacion", typeof(string));
            insumos_de_proveedor.Columns.Add("producto_1", typeof(string));
        }

        private void crear_tabla_insumos_de_proveedorBD()
        {
            insumos_de_proveedorBD = new DataTable();

            insumos_de_proveedorBD.Columns.Add("id", typeof(string));
            insumos_de_proveedorBD.Columns.Add("producto", typeof(string));
            insumos_de_proveedorBD.Columns.Add("tipo_producto", typeof(string));


            insumos_de_proveedorBD.Columns.Add("tipo_paquete", typeof(string));
            insumos_de_proveedorBD.Columns.Add("cantidad_unidades", typeof(string));
            insumos_de_proveedorBD.Columns.Add("unidad_medida", typeof(string));

            insumos_de_proveedorBD.Columns.Add("precio", typeof(string));
            insumos_de_proveedorBD.Columns.Add("precio_unidad", typeof(string));

            insumos_de_proveedorBD.Columns.Add("presentacion", typeof(string)); 
            insumos_de_proveedorBD.Columns.Add("producto_1", typeof(string)); 
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
                double precio_unidad, precio, cant_unidades;
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
                            if (fila_insumo != -1)
                            {

                                insumos_de_proveedorBD.Rows.Add();

                                //id
                                insumos_de_proveedorBD.Rows[fila]["id"] = id_producto;
                                //producto 
                                insumos_de_proveedorBD.Rows[fila]["producto"] = insumosBD.Rows[fila_insumo]["producto"].ToString(); 
                                insumos_de_proveedorBD.Rows[fila]["producto_1"] = obtener_presentacion_recomendada(insumosBD.Rows[fila_insumo]["producto_1"].ToString()); 

                                //tipo_producto
                                insumos_de_proveedorBD.Rows[fila]["tipo_producto"] = insumosBD.Rows[fila_insumo]["tipo_producto"].ToString();

                                //tipo_paquete
                                tipo_paquete = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 3);
                                insumos_de_proveedorBD.Rows[fila]["tipo_paquete"] = tipo_paquete;

                                //cantidad_unidades
                                cantidad_unidades = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 4);
                                insumos_de_proveedorBD.Rows[fila]["cantidad_unidades"] = cantidad_unidades;

                                //unidad_medida
                                unidad_medida = insumosBD.Rows[fila_insumo]["unidad_medida"].ToString();
                                insumos_de_proveedorBD.Rows[fila]["unidad_medida"] = unidad_medida;

                                //precio
                                insumos_de_proveedorBD.Rows[fila]["precio_unidad"] = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0][columna].ToString(), 6);
                                cant_unidades = double.Parse(cantidad_unidades);
                                precio_unidad = double.Parse(insumos_de_proveedorBD.Rows[fila]["precio_unidad"].ToString());
                                precio = Math.Round(precio_unidad * cant_unidades, 2);
                                insumos_de_proveedorBD.Rows[fila]["precio"] = precio.ToString();
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
            double precio, cantidad_unidades, precio_unidad;
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
                    insumos_de_proveedor.Rows[fila_insumo]["producto_1"] = obtener_presentacion_recomendada(insumos_de_proveedorBD.Rows[fila]["producto_1"].ToString());

                    //tipo_paquete
                    insumos_de_proveedor.Rows[fila_insumo]["tipo_paquete"] = insumos_de_proveedorBD.Rows[fila]["tipo_paquete"].ToString();
                    //cantidad_unidades
                    insumos_de_proveedor.Rows[fila_insumo]["cantidad_unidades"] = insumos_de_proveedorBD.Rows[fila]["cantidad_unidades"].ToString();
                    //unidad_medida
                    insumos_de_proveedor.Rows[fila_insumo]["unidad_medida"] = insumos_de_proveedorBD.Rows[fila]["unidad_medida"].ToString();


                    //precio
                    if (double.TryParse(insumos_de_proveedorBD.Rows[fila]["precio"].ToString(), out precio))
                    {
                        insumos_de_proveedor.Rows[fila_insumo]["precio"] = funciones.formatCurrency(precio);

                    }
                    else
                    {
                        insumos_de_proveedor.Rows[fila_insumo]["precio"] = insumos_de_proveedorBD.Rows[fila]["precio"].ToString();
                    }

                    //precio
                    if (double.TryParse(insumos_de_proveedorBD.Rows[fila]["precio_unidad"].ToString(), out precio_unidad))
                    {
                        insumos_de_proveedor.Rows[fila_insumo]["precio_unidad"] = funciones.formatCurrency(precio_unidad);

                    }
                    else
                    {
                        insumos_de_proveedor.Rows[fila_insumo]["precio_unidad"] = insumos_de_proveedorBD.Rows[fila]["precio_unidad"].ToString();
                    }

                    //precio
                    insumos_de_proveedor.Rows[fila_insumo]["presentacion"] = insumos_de_proveedorBD.Rows[fila]["presentacion"].ToString();
                    fila_insumo++;
                }
            }
        }
        private void cargar_insumos_proveedor()
        {
            llenar_tabla_insumos_de_proveedor();

            gridview_insumos_del_proveedor.DataSource = insumos_de_proveedor;
            gridview_insumos_del_proveedor.DataBind();
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
            insumos.Columns.Add("producto_1", typeof(string));

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
                    insumos.Rows[fila_insumos]["producto_1"] = insumosBD.Rows[fila]["producto_1"].ToString();

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
        #region datos proveedor
        private void cargar_datos_proveedor()
        {
            proveedor_fabrica_seleccionado = linkear.get_proveedor_fabrica_seleccionado(id_proveedor_fabrica_seleccionado);

            textbox_nombre.Text = proveedor_fabrica_seleccionado.Rows[0]["proveedor"].ToString();
            textbox_provincia.Text = proveedor_fabrica_seleccionado.Rows[0]["provincia"].ToString();
            textbox_localidad.Text = proveedor_fabrica_seleccionado.Rows[0]["localidad"].ToString();
            textbox_direccion.Text = proveedor_fabrica_seleccionado.Rows[0]["direccion"].ToString();
            textboc_telefono.Text = proveedor_fabrica_seleccionado.Rows[0]["telefono"].ToString();
            textbox_condicion_pago.Text = proveedor_fabrica_seleccionado.Rows[0]["condicion_pago"].ToString();
                 
            textbox_cbu_1.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_1"].ToString();
            textbox_cbu_2.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_2"].ToString();
            textbox_cbu_3.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_3"].ToString();
            textbox_cbu_4.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_4"].ToString();
            textbox_cbu_5.Text = proveedor_fabrica_seleccionado.Rows[0]["CBU_5"].ToString();

            Session.Add("proveedor_fabrica_seleccionado", proveedor_fabrica_seleccionado);
        }
        private void actualizar_datos_proveedor()
        {
            linkear.actualizar_datos_proveedor(id_proveedor_fabrica_seleccionado, textbox_nombre.Text, textbox_provincia.Text, textbox_localidad.Text, textbox_direccion.Text, textboc_telefono.Text, textbox_cbu_1.Text, textbox_cbu_2.Text, textbox_cbu_3.Text, textbox_cbu_4.Text, textbox_cbu_5.Text,textbox_condicion_pago.Text);
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
                if (insumos_de_proveedorBD.Rows[fila]["tipo_paquete"].ToString() == "N/A" ||
                    insumos_de_proveedorBD.Rows[fila]["cantidad_unidades"].ToString() == "N/A" ||
                    insumos_de_proveedorBD.Rows[fila]["unidad_medida"].ToString() == "N/A" ||
                    insumos_de_proveedorBD.Rows[fila]["precio"].ToString() == "N/A" ||
                    insumos_de_proveedorBD.Rows[fila]["precio"].ToString() == "0")
                {
                    retorno = false;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_linkear_insumos_a_proveedor_de_fabrica linkear;
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
            id_proveedor_fabrica_seleccionado = Session["id_proveedor_fabrica_seleccionado"].ToString();
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["linkear"] == null)
            {
                Session.Add("linkear", new cls_linkear_insumos_a_proveedor_de_fabrica(usuariosBD));
            }
            linkear = (cls_linkear_insumos_a_proveedor_de_fabrica)Session["linkear"];
            insumosBD = linkear.get_insumos();
            acuerdo_de_precios_fabrica_a_proveedoresBD = linkear.get_acuerdo_de_precios_fabrica_a_proveedores(id_proveedor_fabrica_seleccionado);
            if (!IsPostBack)
            {
                Session.Remove("insumos_de_proveedor");

                llenar_dropDownList_insumos(insumosBD);
                Session.Add("tipo_seleccionado", dropDown_tipo_insumos.SelectedItem.Text);


                llenar_tabla_insumos_de_proveedorBD();

                cargar_insumos();
                cargar_insumos_proveedor();
                cargar_datos_proveedor();
            }

        }
        #region datos proveedor
        protected void boton_modificar_datos_proveedor_Click(object sender, EventArgs e)
        {
            actualizar_datos_proveedor();
            cargar_datos_proveedor();
        }
        #endregion

        #region insumos
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

        #region insumos del proveedor
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
            insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
            int fila_tabla = 0;
            for (int fila = 0; fila <= gridview_insumos_del_proveedor.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, insumos_de_proveedorBD);

                DropDownList dropdown_tipo_paquete = (gridview_insumos_del_proveedor.Rows[fila].Cells[3].FindControl("dropdown_tipo_paquete") as DropDownList);
                dropdown_tipo_paquete.SelectedValue = insumos_de_proveedorBD.Rows[fila_tabla]["tipo_paquete"].ToString();

                //  DropDownList dropDownList_unidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[5].FindControl("dropdown_unidad") as DropDownList);
                //  dropDownList_unidad.SelectedValue = insumos_de_proveedorBD.Rows[fila_tabla]["unidad_medida"].ToString();
            }
        }
        protected void dropdown_unidad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gridview_insumos_del_proveedor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_aplicar_cambios")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                insumos_de_proveedorBD = (DataTable)Session["insumos_de_proveedor"];
                int fila_tabla = funciones.buscar_fila_por_id(gridview_insumos_del_proveedor.Rows[fila].Cells[0].Text, insumos_de_proveedorBD);

                DropDownList dropdown_tipo_paquete = (gridview_insumos_del_proveedor.Rows[fila].Cells[3].FindControl("dropdown_tipo_paquete") as DropDownList);
                insumos_de_proveedorBD.Rows[fila_tabla]["tipo_paquete"] = dropdown_tipo_paquete.SelectedItem.Text;

                //  DropDownList dropDownList_unidad = (gridview_insumos_del_proveedor.Rows[fila].Cells[5].FindControl("dropdown_unidad") as DropDownList);
                insumos_de_proveedorBD.Rows[fila_tabla]["unidad_medida"] = gridview_insumos_del_proveedor.Rows[fila].Cells[5].Text;
                double precio, cantidad_unidades, precio_unidad;

                double cantidad;
                TextBox textbox_cantidad_unidades = (gridview_insumos_del_proveedor.Rows[fila].Cells[4].FindControl("textbox_cantidad_unidades") as TextBox);
                if (double.TryParse(textbox_cantidad_unidades.Text, out cantidad))
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["cantidad_unidades"] = cantidad.ToString();

                    if ("N/A" != insumos_de_proveedorBD.Rows[fila_tabla]["precio_unidad"].ToString())
                    {
                        precio = double.Parse(insumos_de_proveedorBD.Rows[fila_tabla]["precio"].ToString());
                        cantidad_unidades = double.Parse(insumos_de_proveedorBD.Rows[fila_tabla]["cantidad_unidades"].ToString());
                        precio_unidad = Math.Round(precio / cantidad_unidades, 2);
                        insumos_de_proveedorBD.Rows[fila_tabla]["precio_unidad"] = precio_unidad;
                    }
                }

                if (insumos_de_proveedorBD.Rows[fila_tabla]["tipo_paquete"].ToString() == "Unidad")
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["presentacion"] = insumos_de_proveedorBD.Rows[fila_tabla]["cantidad_unidades"].ToString() + " " + insumos_de_proveedorBD.Rows[fila_tabla]["unidad_medida"].ToString();
                }
                else
                {
                    insumos_de_proveedorBD.Rows[fila_tabla]["presentacion"] = insumos_de_proveedorBD.Rows[fila_tabla]["tipo_paquete"].ToString() + " x " + insumos_de_proveedorBD.Rows[fila_tabla]["cantidad_unidades"].ToString() + " " + insumos_de_proveedorBD.Rows[fila_tabla]["unidad_medida"].ToString();
                }

                TextBox textbox_precio = (gridview_insumos_del_proveedor.Rows[fila].Cells[7].FindControl("textbox_precio") as TextBox);
                if (double.TryParse(textbox_precio.Text, out precio))
                {
                    string dato = insumos_de_proveedorBD.Rows[fila_tabla]["precio_unidad"].ToString();
                    insumos_de_proveedorBD.Rows[fila_tabla]["precio"] = precio;
                    cantidad_unidades = double.Parse(insumos_de_proveedorBD.Rows[fila_tabla]["cantidad_unidades"].ToString());
                    precio_unidad = Math.Round(precio / cantidad_unidades, 2);
                    insumos_de_proveedorBD.Rows[fila_tabla]["precio_unidad"] = precio_unidad;
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

        #endregion



        protected void boton_guardar_Click(object sender, EventArgs e)
        {
            linkear_productos_a_proveedor();
           // Response.Redirect("~/paginasFabrica/proveedores_fabrica.aspx", false);
        }

        protected void dropdown_tipo_paquete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}