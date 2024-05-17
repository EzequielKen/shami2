using _04___sistemas_carrefour;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasCarrefour
{
    public partial class detalle_de_movimientos : System.Web.UI.Page
    {
        #region carga de productos
        private void configurar_gridview()
        {
            Session.Add("productos_detalle", detalles.get_productos_carrefour(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text));
            productos_detalle = (DataTable)Session["productos_detalle"];

            gridView_resumen.Columns.Clear();

            BoundField textColumn = new BoundField();
            textColumn.HeaderText = "id";
            textColumn.DataField = "id"; // Asigna el nombre de tu campo en la fuente de datos
            gridView_resumen.Columns.Add(textColumn);

            textColumn = new BoundField();
            textColumn.HeaderText = "Producto";
            textColumn.DataField = "producto"; // Asigna el nombre de tu campo en la fuente de datos
            gridView_resumen.Columns.Add(textColumn);

            textColumn = new BoundField();
            textColumn.HeaderText = "Medida";
            textColumn.DataField = "medida"; // Asigna el nombre de tu campo en la fuente de datos
            gridView_resumen.Columns.Add(textColumn);

            for (int columna = productos_detalle.Columns["total"].Ordinal; columna <= productos_detalle.Columns.Count-1; columna++)
            {
                textColumn = new BoundField();
                textColumn.HeaderText = productos_detalle.Columns[columna].ColumnName;
                textColumn.DataField = productos_detalle.Columns[columna].ColumnName; // Asigna el nombre de tu campo en la fuente de datos
                gridView_resumen.Columns.Add(textColumn);
            }
        }
        private void cargar_productos()
        {
            configurar_gridview();
            gridView_resumen.DataSource = productos_detalle;
            gridView_resumen.DataBind();
        }
        #endregion

        #region configurar controles
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_proveedores();
            cargar_mes();
            cargar_año();
        }
        private void cargar_proveedores()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            sucursales_carrefour = (DataTable)Session["sucursales_carrefour"];

            for (int fila = 0; fila <= sucursales_carrefour.Rows.Count - 1; fila++)
            {

                item = new System.Web.UI.WebControls.ListItem(sucursales_carrefour.Rows[fila]["sucursal"].ToString(), num_item.ToString());

                dropDown_sucursales.Items.Add(item);


                num_item++;

            }
        }
        private void cargar_mes()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                dropDown_mes.Items.Add(item);
                num_item++;
            }
            dropDown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                dropDown_año.Items.Add(item);
                num_item++;
            }
            string añ = DateTime.Now.Year.ToString();

            dropDown_año.SelectedIndex = dropDown_año.Items.Count - 1;
        }
        #endregion

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_detalle_de_movimientos detalles;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable sucursales_carrefour;
        DataTable productos_detalle;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["detalle_de_movimientos"] == null)
            {
                Session.Add("detalle_de_movimientos", new cls_detalle_de_movimientos(usuariosBD));
            }
            detalles = (cls_detalle_de_movimientos)Session["detalle_de_movimientos"];
            if (!IsPostBack)
            {
                sucursales_carrefour = detalles.get_sucursales_carrefour();
                Session.Add("sucursales_carrefour", sucursales_carrefour);



                configurar_controles();

                cargar_productos();

            }
        }

        protected void dropDown_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();
        }

        protected void gridView_resumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }
    }
}