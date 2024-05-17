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
    public partial class analisis_movimientos : System.Web.UI.Page
    {
        private void cargar_productos()
        {
            gridView_resumen.DataSource = analisis.get_productos_carrefour(dropDown_sucursales.SelectedItem.Text, dropDown_mes.SelectedItem.Text, dropDown_año.SelectedItem.Text);
            gridView_resumen.DataBind();
        }
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
        cls_analisis_movimientos analisis;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable sucursales_carrefour;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["analisis_movimientos"]==null)
            {
                Session.Add("analisis_movimientos", new cls_analisis_movimientos(usuariosBD));
            }
            analisis = (cls_analisis_movimientos)Session["analisis_movimientos"];
            if (!IsPostBack)
            {
                sucursales_carrefour = analisis.get_sucursales_carrefour();
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
    }
}