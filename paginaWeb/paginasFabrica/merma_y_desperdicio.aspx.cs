using _03___sistemas_fabrica;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class merma_y_desperdicio : System.Web.UI.Page
    {
        #region carga a base de datos
        private void sumar_stock(string id_producto, string cantidad_dato)
        {
            
        }
        private void cargar_desperdicio()
        {
            merma_y_deperdicio.cargar_desperdicio(proveedorBD.Rows[0]["nombre_en_BD"].ToString(),productos_proveedorBD);
            productos_proveedorBD = merma_y_deperdicio.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            Session.Add("productos_proveedorBD_merma", productos_proveedorBD);
        }
        private void cargar_merma()
        {
            merma_y_deperdicio.cargar_merma(proveedorBD.Rows[0]["nombre_en_BD"].ToString(), insumosBD);
            insumosBD = merma_y_deperdicio.get_insumos_proveedor();
            Session.Add("insumosBD_merma", insumosBD);
        }
        #endregion
        #region carga merma/desperdicio
        private void configurar_desperdicio(string id_producto, string cantidad_dato,string unidad)
        {
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_proveedorBD);
                if (cantidad > 0)
                {
                    productos_proveedorBD.Rows[fila_producto]["cantidad_desperdicio"] = cantidad.ToString();
                    productos_proveedorBD.Rows[fila_producto]["unidad"] = unidad; 
                    productos_proveedorBD.Rows[fila_producto]["presentacion"] = cantidad.ToString() + " " + unidad;
                }
                else
                {
                    productos_proveedorBD.Rows[fila_producto]["cantidad_desperdicio"] = "N/A";
                    productos_proveedorBD.Rows[fila_producto]["unidad"] = "N/A";
                    productos_proveedorBD.Rows[fila_producto]["presentacion"] = "N/A";
                }
            }
        }
        private void configurar_merma(string id_insumo, string cantidad_dato, string unidad)
        {
            double cantidad;
            if (double.TryParse(cantidad_dato, out cantidad))
            {
                int fila_insumo =funciones.buscar_fila_por_id(id_insumo,insumosBD);
                if (cantidad > 0)
                {
                    insumosBD.Rows[fila_insumo]["cantidad_merma"] = cantidad.ToString();
                    insumosBD.Rows[fila_insumo]["unidad"] = unidad;
                    insumosBD.Rows[fila_insumo]["presentacion"] = cantidad.ToString() + " " + unidad;
                }
                else
                {
                    insumosBD.Rows[fila_insumo]["cantidad_merma"] = "N/A";
                    insumosBD.Rows[fila_insumo]["unidad"] = "N/A";
                    insumosBD.Rows[fila_insumo]["presentacion"] = "N/A";
                }
            }

        }
        #endregion
        #region carga de productos
        private void crear_tabla_insumos()
        {
            insumos = new DataTable();

            insumos.Columns.Add("id", typeof(string));
            insumos.Columns.Add("producto", typeof(string));
            insumos.Columns.Add("cantidad_merma", typeof(string));
            insumos.Columns.Add("unidad", typeof(string)); 
            insumos.Columns.Add("presentacion", typeof(string)); 
        }
        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumosBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar_insumo.Text, insumosBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(insumosBD.Rows[fila]["tipo_producto"].ToString(), Session["tipo_seleccionado_insumo"].ToString()))
                {
                    insumos.Rows.Add();

                    insumos.Rows[fila_insumo]["id"] = insumosBD.Rows[fila]["id"].ToString();
                    insumos.Rows[fila_insumo]["producto"] = insumosBD.Rows[fila]["producto"].ToString();
                    insumos.Rows[fila_insumo]["cantidad_merma"] = insumosBD.Rows[fila]["cantidad_merma"].ToString();
                    insumos.Rows[fila_insumo]["unidad"] = insumosBD.Rows[fila]["unidad"].ToString(); 
                    insumos.Rows[fila_insumo]["presentacion"] = insumosBD.Rows[fila]["presentacion"].ToString(); 

                    fila_insumo++;
                }
            }
        }
        private void cargar_insumo()
        {
            llenar_tabla_insumos();

            gridView_insumos.DataSource = insumos;
            gridView_insumos.DataBind();
        }
        private void crear_tabla_productos()
        {
            productos_proveedor = new DataTable();

            productos_proveedor.Columns.Add("id", typeof(string));
            productos_proveedor.Columns.Add("producto", typeof(string));
            productos_proveedor.Columns.Add("cantidad_desperdicio", typeof(string));
            productos_proveedor.Columns.Add("unidad_de_medida_fabrica", typeof(string));
            productos_proveedor.Columns.Add("unidad", typeof(string)); 
            productos_proveedor.Columns.Add("presentacion", typeof(string)); 
        }
        private void llenar_tabla_producto()
        {
            crear_tabla_productos();
            int fila_producto = 0;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productos_proveedorBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(productos_proveedorBD.Rows[fila]["tipo_producto"].ToString(), Session["tipo_seleccionado"].ToString()))
                {
                    productos_proveedor.Rows.Add();

                    productos_proveedor.Rows[fila_producto]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                    productos_proveedor.Rows[fila_producto]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();
                    productos_proveedor.Rows[fila_producto]["cantidad_desperdicio"] = productos_proveedorBD.Rows[fila]["cantidad_desperdicio"].ToString();
                    productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"] = productos_proveedorBD.Rows[fila]["unidad_de_medida_fabrica"].ToString();
                    productos_proveedor.Rows[fila_producto]["unidad"] = productos_proveedorBD.Rows[fila]["unidad"].ToString(); 
                    productos_proveedor.Rows[fila_producto]["presentacion"] = productos_proveedorBD.Rows[fila]["presentacion"].ToString(); 

                    fila_producto++;
                }
            }
        }
        private void cargar_producto()
        {
            llenar_tabla_producto();

            gridView_productos.DataSource = productos_proveedor;
            gridView_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

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
        private void llenar_dropDownList_insumo(DataTable dt)
        {
            DropDown_tipo_insumo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            DropDown_tipo_insumo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (DropDown_tipo_insumo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {

                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    DropDown_tipo_insumo.Items.Add(item);
                    num_item = num_item + 1;

                }

            }
        }
        #endregion

        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_merma_y_desperdicio merma_y_deperdicio;
        cls_cargar_orden_de_compra orden_compra;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;

        DataTable productos_proveedorBD;
        DataTable productos_proveedor;
        DataTable insumosBD;
        DataTable insumos;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            
            merma_y_deperdicio = new cls_merma_y_desperdicio(usuariosBD);
            if (!IsPostBack)
            {
                productos_proveedorBD = merma_y_deperdicio.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString());
                Session.Add("productos_proveedorBD_merma", productos_proveedorBD);
                insumosBD = merma_y_deperdicio.get_insumos_proveedor();
                Session.Add("insumosBD_merma", insumosBD);
                llenar_dropDownList(productos_proveedorBD);
                Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);

                llenar_dropDownList_insumo(insumosBD);
                Session.Add("tipo_seleccionado_insumo", DropDown_tipo_insumo.SelectedItem.Text);

                cargar_producto();
                cargar_insumo();
            }
            productos_proveedorBD = (DataTable)Session["productos_proveedorBD_merma"];
            insumosBD = (DataTable)Session["insumosBD_merma"];
        }

        #region productos
        protected void boton_carga_desperdicio_Click(object sender, EventArgs e)
        {
            cargar_desperdicio();
            cargar_producto();
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_producto();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);
            cargar_producto();
        }
        protected void gridView_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int fila_tabla=0;
            for (int fila = 0; fila <= gridView_productos.Rows.Count-1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridView_productos.Rows[fila].Cells[0].Text, productos_proveedorBD);

                DropDownList dropdown_unidad = (gridView_productos.Rows[fila].Cells[3].FindControl("dropdown_unidad") as DropDownList);
                dropdown_unidad.SelectedValue = productos_proveedorBD.Rows[fila_tabla]["unidad"].ToString();
            }
        }
        protected void gridView_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cargar_en_desperdicio")
            {
                int fila = int.Parse(e.CommandArgument.ToString());

                TextBox textbox_cantidad_desperdicio = (gridView_productos.Rows[fila].Cells[2].FindControl("texbox_cantidad") as TextBox);

                DropDownList dropdown_unidad = (gridView_productos.Rows[fila].Cells[3].FindControl("dropdown_unidad") as DropDownList);

                configurar_desperdicio(gridView_productos.Rows[fila].Cells[0].Text, textbox_cantidad_desperdicio.Text, dropdown_unidad.SelectedItem.Text);

                cargar_producto();
            }
            else if (e.CommandName == "cargar_en_stock")
            {
                int fila = int.Parse(e.CommandArgument.ToString());

                TextBox texbox_cantidad_stock = (gridView_productos.Rows[fila].Cells[5].FindControl("texbox_cantidad_stock") as TextBox);

                sumar_stock(gridView_productos.Rows[fila].Cells[0].Text, texbox_cantidad_stock.Text);

                cargar_producto();
            }
        }
        #endregion

        #region insumos
       
        protected void boton_carga_merma_Click(object sender, EventArgs e)
        {
            cargar_merma();
            cargar_insumo();
        }
        protected void textbox_buscar_insumo_TextChanged(object sender, EventArgs e)
        {
            cargar_insumo();
        }

        protected void DropDown_tipo_insumo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("tipo_seleccionado_insumo", DropDown_tipo_insumo.SelectedItem.Text);
            cargar_insumo();
        }
        protected void gridView_insumos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int fila_tabla = 0;
            for (int fila = 0; fila <= gridView_insumos.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridView_insumos.Rows[fila].Cells[0].Text, insumosBD);

                DropDownList dropdown_unidad_insumo = (gridView_insumos.Rows[fila].Cells[3].FindControl("dropdown_unidad_insumo") as DropDownList);
                dropdown_unidad_insumo.SelectedValue = insumosBD.Rows[fila_tabla]["unidad"].ToString();
            }
        }
        protected void gridView_insumos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "cargar_en_merma")
            {
                int fila = int.Parse(e.CommandArgument.ToString());

                TextBox texbox_cantidad_insumo = (gridView_insumos.Rows[fila].Cells[2].FindControl("texbox_cantidad_insumo") as TextBox);

                DropDownList dropdown_unidad_insumo = (gridView_insumos.Rows[fila].Cells[3].FindControl("dropdown_unidad_insumo") as DropDownList);

                configurar_merma(gridView_insumos.Rows[fila].Cells[0].Text, texbox_cantidad_insumo.Text, dropdown_unidad_insumo.SelectedItem.Text);

                cargar_insumo();
            }
        }



        #endregion

        protected void dropdown_unidad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropdown_unidad_insumo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}