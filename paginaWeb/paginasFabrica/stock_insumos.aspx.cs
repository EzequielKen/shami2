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
    public partial class stock_insumos : System.Web.UI.Page
    {
        #region carga a base de datos
        private void guardar_registro()
        {
            // tipo_usuario.Rows[0]["rol"].ToString()
            stock_insumo.guardar_registro(tipo_usuario.Rows[0]["rol"].ToString(), (DataTable)Session["insumos_fabricaBD_stock"], (DataTable)Session["insumos_fabricaBD_stock_copia"]);
            Response.Redirect("~/paginasFabrica/sucursales.aspx", false);

        }
        #endregion
        #region stock
        private void crear_tabla_presentaciones()
        {
            presentaciones = new DataTable();
            presentaciones.Columns.Add("id", typeof(string));
            presentaciones.Columns.Add("stock", typeof(string));
            presentaciones.Columns.Add("nuevo_stock", typeof(string));
            presentaciones.Columns.Add("presentacionBD", typeof(string));
            presentaciones.Columns.Add("nombre_columna", typeof(string));
            Session.Add("presentaciones", presentaciones);

        }
        private void llenar_tabla_presentaciones(string id_insumo)
        {
            insumos_fabricaBD = (DataTable)Session["insumos_fabricaBD_stock"];
            presentaciones = (DataTable)Session["presentaciones"];
            presentaciones.Rows.Clear();
            int fila_presentaciones = 0;
            int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabricaBD);
            string dato, tipo_paquete, cant_unidad, unidad, cantidad, nombre_columna;
            for (int columna = insumos_fabricaBD.Columns["producto_1"].Ordinal; columna <= insumos_fabricaBD.Columns.Count - 1; columna++)
            {
                if (insumos_fabricaBD.Rows[fila_insumo][columna].ToString() == "N/A")
                {
                    break;
                }
                else
                {
                    tipo_paquete = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 1);
                    cant_unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 2);
                    unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 3);
                    cantidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 4);
                    double cant = double.Parse(cantidad);

                    if (tipo_paquete == "Unidad")
                    {
                        dato = cantidad + " x" + cant_unidad + "" + unidad;
                    }
                    else
                    {
                        dato = cantidad + " " + tipo_paquete + " x" + cant_unidad + " " + unidad;
                    }
                    presentaciones.Rows.Add();
                    presentaciones.Rows[fila_presentaciones]["id"] = id_insumo;
                    presentaciones.Rows[fila_presentaciones]["stock"] = dato;
                    presentaciones.Rows[fila_presentaciones]["nuevo_stock"] = "N/A";
                    presentaciones.Rows[fila_presentaciones]["presentacionBD"] = insumos_fabricaBD.Rows[fila_insumo][columna].ToString();
                    presentaciones.Rows[fila_presentaciones]["nombre_columna"] = insumos_fabricaBD.Columns[columna].ColumnName;
                    fila_presentaciones++;
                }
            }
        }
        private void cargar_nuevo_stock(string nombre_columna, string dato)
        {
            presentaciones = (DataTable)Session["presentaciones"];

            int fila_stock = buscar_fila_por_nombre_columna(nombre_columna, presentaciones);
            double cantidad;
            if (double.TryParse(dato, out cantidad))
            {
                presentaciones.Rows[fila_stock]["nuevo_stock"] = cantidad.ToString();
            }
            else
            {
                presentaciones.Rows[fila_stock]["nuevo_stock"] = "N/A";
            }
            Session.Add("presentaciones", presentaciones);
        }
        private int buscar_fila_por_nombre_columna(string nombre_columna, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                if (nombre_columna == dt.Rows[fila]["nombre_columna"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void confirmar_stock()
        {
            insumos_fabricaBD = (DataTable)Session["insumos_fabricaBD_stock"];
            presentaciones = (DataTable)Session["presentaciones"];
            string id_insumo = presentaciones.Rows[0]["id"].ToString();
            int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabricaBD);

            string nombre_columna;
            string tipo_paquete, unidad, tipo_unidad, nuevo_stock, dato;
            for (int fila = 0; fila <= presentaciones.Rows.Count - 1; fila++)
            {
                nombre_columna = presentaciones.Rows[fila]["nombre_columna"].ToString();
                tipo_paquete = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][nombre_columna].ToString(), 1);
                unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][nombre_columna].ToString(), 2);
                tipo_unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][nombre_columna].ToString(), 3);
                nuevo_stock = presentaciones.Rows[fila]["nuevo_stock"].ToString();
                if (nuevo_stock != "N/A")
                {
                    dato = tipo_paquete + "-" + unidad + "-" + tipo_unidad + "-" + nuevo_stock + "-" + nuevo_stock;
                    insumos_fabricaBD.Rows[fila_insumo][nombre_columna] = dato;
                    insumos_fabricaBD.Rows[fila_insumo]["nuevo_stock"] = "si";
                }
            }
            presentaciones.Rows.Clear();
            Session.Add("insumos_fabricaBD_stock", insumos_fabricaBD);
        }
        #endregion

        #region cargar insumos
        private void crear_tabla_insumos()
        {
            insumos_fabrica = new DataTable();
            insumos_fabrica.Columns.Add("id", typeof(string));
            insumos_fabrica.Columns.Add("producto", typeof(string));
            insumos_fabrica.Columns.Add("stock", typeof(string));
            insumos_fabrica.Columns.Add("unidad_medida", typeof(string));
            insumos_fabrica.Columns.Add("stock_presentacion", typeof(string));
            insumos_fabrica.Columns.Add("stock_nuevo", typeof(string));
        }

        private void llenar_tabla_insumos()
        {
            insumos_fabricaBD = (DataTable)Session["insumos_fabricaBD_stock"];
            crear_tabla_insumos();
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumos_fabricaBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, insumos_fabricaBD.Rows[fila]["producto"].ToString()) &&
                    funciones.verificar_tipo_producto(insumos_fabricaBD.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                {
                    insumos_fabrica.Rows.Add();
                    insumos_fabrica.Rows[fila_insumo]["id"] = insumos_fabricaBD.Rows[fila]["id"].ToString();
                    insumos_fabrica.Rows[fila_insumo]["producto"] = insumos_fabricaBD.Rows[fila]["producto"].ToString();
                    insumos_fabrica.Rows[fila_insumo]["stock"] = insumos_fabricaBD.Rows[fila]["stock"].ToString();
                    insumos_fabrica.Rows[fila_insumo]["unidad_medida"] = insumos_fabricaBD.Rows[fila]["unidad_medida"].ToString();
                    fila_insumo++;
                }
            }
        }

        private void cargar_insumos()
        {
            llenar_tabla_insumos();
            gridview_productos.DataSource = insumos_fabrica;
            gridview_productos.DataBind();
            presentaciones = (DataTable)Session["presentaciones"];
            gridview_presentaciones.DataSource = presentaciones;
            gridview_presentaciones.DataBind();
            if (presentaciones.Rows.Count == 0)
            {
                boton_confirmar_stock.Visible = false;
            }
            else
            {
                boton_confirmar_stock.Visible = true;
            }
        }
        #endregion

        #region configurar controles
        private void configurar_controles()
        {
            llenar_dropDSownList(insumos_fabricaBD);
        }
        private void llenar_dropDSownList(DataTable dt)
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
        #endregion

        #region atributos
        cls_stock_insumos stock_insumo;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_usuario;
        string tipo_seleccionado;

        DataTable insumos_fabricaBD;
        DataTable insumos_fabricaBD_copia;
        DataTable insumos_fabrica;
        DataTable presentaciones;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];

            if (Session["stock_insumo"] == null)
            {
                Session.Add("stock_insumo", new cls_stock_insumos(usuariosBD));
            }
            stock_insumo = (cls_stock_insumos)Session["stock_insumo"];
            stock_insumo.actualizar_stock_insumos();


            if (!IsPostBack)
            {
                insumos_fabricaBD = stock_insumo.get_insumos_fabrica();
                insumos_fabricaBD_copia = insumos_fabricaBD;
                Session.Add("insumos_fabricaBD_stock", insumos_fabricaBD);
                Session.Add("insumos_fabricaBD_stock_copia", insumos_fabricaBD_copia);
                configurar_controles();
                crear_tabla_presentaciones();
                cargar_insumos();

            }
        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id_insumo = gridview_productos.SelectedRow.Cells[0].Text;
            llenar_tabla_presentaciones(id_insumo);
            cargar_insumos();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_insumos();
        }

        protected void textbox_stock_nuevo_TextChanged1(object sender, EventArgs e)
        {
            TextBox textbox_dato = (TextBox)sender;
            GridViewRow row = (GridViewRow)textbox_dato.NamingContainer;
            int rowIndex = row.RowIndex;
            string nombre_columna = gridview_presentaciones.Rows[rowIndex].Cells[4].Text;
            cargar_nuevo_stock(nombre_columna, textbox_dato.Text);
            cargar_insumos();
        }

        protected void boton_confirmar_stock_Click(object sender, EventArgs e)
        {
            confirmar_stock();
            cargar_insumos();
        }

        protected void boton_guardar_registro_Click(object sender, EventArgs e)
        {
            guardar_registro();
        }
    }
}