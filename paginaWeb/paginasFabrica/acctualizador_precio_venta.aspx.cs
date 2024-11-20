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
    public partial class acctualizador_precio_venta : System.Web.UI.Page
    {

        #region carga productos
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id",typeof(string));
            resumen.Columns.Add("producto",typeof(string));
            resumen.Columns.Add("precio_compra",typeof (string));
            resumen.Columns.Add("presentacion_compra",typeof(string));
            resumen.Columns.Add("precio_venta",typeof(string));
            resumen.Columns.Add("unidad_de_medida_local",typeof(string));
        }
        #endregion
        #region configurar controles 
        private void configurar_controles()
        {
            llenar_dropDownList(productosBD);
        }
        private void cargar_tipo_de_acuerdo()
        {
            for (int fila = 0; fila <= tipo_de_acuerdo.Rows.Count - 1; fila++)
            {
                dropdown_acuerdo.Items.Add(tipo_de_acuerdo.Rows[fila]["tipo_de_acuerdo"].ToString());
            }
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
        #endregion
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_acctualizador_precio_venta actualizador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable tipo_de_acuerdo;
        DataTable productosBD;
        DataTable resumen;
        string tipo_seleccionado;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            actualizador = new cls_acctualizador_precio_venta(usuariosBD);

            if (!IsPostBack)
            {
                tipo_de_acuerdo = actualizador.get_tipo_acuerdo();
                cargar_tipo_de_acuerdo();
                productosBD = actualizador.get_productos(dropdown_acuerdo.SelectedItem.Text);
                configurar_controles();
            }
        }

        protected void texbox_precio_TextChanged(object sender, EventArgs e)
        {

        }
    }
}