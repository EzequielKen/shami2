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
    public partial class sucursales_carrefour : System.Web.UI.Page
    {
        #region cargar tablas
        private void crear_tabla_sucursal()
        {
            sucursales = new DataTable();
            sucursales.Columns.Add("id", typeof(string));
            sucursales.Columns.Add("sucursal", typeof(string));
            sucursales.Columns.Add("direccion", typeof(string));
        }
        private void llenar_tabla_sucursal()
        {
            crear_tabla_sucursal();
            int fila_sucursal = 0;
            for (int fila = 0; fila <= sucursalesBD.Rows.Count-1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, sucursalesBD.Rows[fila]["sucursal"].ToString()))
                {
                    sucursales.Rows.Add();
                    sucursales.Rows[fila_sucursal]["id"] = sucursalesBD.Rows[fila]["id"].ToString();
                    sucursales.Rows[fila_sucursal]["sucursal"] = sucursalesBD.Rows[fila]["sucursal"].ToString();
                    sucursales.Rows[fila_sucursal]["direccion"] = sucursalesBD.Rows[fila]["direccion"].ToString();
                    fila_sucursal++;
                }
            }
        }

        private void cargar_sucursales()
        {
            llenar_tabla_sucursal();
            gridView_sucursales.DataSource = sucursales;
            gridView_sucursales.DataBind();
        }
        #endregion
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_sucursales_carrefour sucursal_carrefour;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursalesBD;
        DataTable sucursales;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["sucursal_carrefour"]==null)
            {
                Session.Add("sucursal_carrefour", new cls_sucursales_carrefour(usuariosBD));
            }
            sucursal_carrefour = (cls_sucursales_carrefour)Session["sucursal_carrefour"];
            sucursalesBD = sucursal_carrefour.get_sucursales_carrefour();
            if (!IsPostBack)
            {
                cargar_sucursales();
            }
        }

        protected void gridView_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("carrefour_seleccionado", gridView_sucursales.SelectedRow.Cells[0].Text);
            Session.Add("sucursal_carrefour_seleccionada", gridView_sucursales.SelectedRow.Cells[1].Text);
            Response.Redirect("~/paginasCarrefour/planilla_movimientos.aspx", false);

        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_sucursales();
        }
    }
}














