using _02___sistemas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class desperdicio_merma : System.Web.UI.Page
    {
        #region llenar resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));

        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            productos_terminados = (DataTable)Session["productos_terminados"];
            insumos = (DataTable)Session["insumos"];
            int ultima_fila = 0;
            if ("Desperdicio" == dropdown_categoria.SelectedItem.Text)
            {
                for (int fila = 0; fila <= productos_terminados.Rows.Count - 1; fila++)
                {
                    if (funciones.verificar_tipo_producto(productos_terminados.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                    {
                        resumen.Rows.Add();
                        ultima_fila = resumen.Rows.Count - 1;
                        resumen.Rows[ultima_fila]["id"] = productos_terminados.Rows[fila]["id"].ToString();
                        resumen.Rows[ultima_fila]["producto"] = productos_terminados.Rows[fila]["producto"].ToString();
                        resumen.Rows[ultima_fila]["tipo_producto"] = productos_terminados.Rows[fila]["tipo_producto"].ToString();
                    }
                }
            }
            else
            {
                for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
                {
                    if (funciones.verificar_tipo_producto(insumos.Rows[fila]["tipo_producto"].ToString(), dropDown_tipo.SelectedItem.Text))
                    {
                        resumen.Rows.Add();
                        ultima_fila = resumen.Rows.Count - 1;
                        resumen.Rows[ultima_fila]["id"] = insumos.Rows[fila]["id"].ToString();
                        resumen.Rows[ultima_fila]["producto"] = insumos.Rows[fila]["producto"].ToString();
                        resumen.Rows[ultima_fila]["tipo_producto"] = insumos.Rows[fila]["tipo_producto"].ToString();
                    }
                }
            }
        }
        private void cargar_resumen()
        {
            llenar_tabla_resumen();
            gridview_productos.DataSource = resumen;
            gridview_productos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
            dropDown_tipo.Items.Clear();
            productos_terminados = (DataTable)Session["productos_terminados"];
            insumos = (DataTable)Session["insumos"];
            if (dropdown_categoria.SelectedItem.Text == "Desperdicio")
            {
                llenar_dropDownList(productos_terminados);
            }
            else
            {
                llenar_dropDownList(insumos);
            }
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "orden ASC";
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
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_desperdicio_merma desperdicioMerma;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable productos_terminados;
        DataTable insumos;
        DataTable resumen;
        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            desperdicioMerma = new cls_desperdicio_merma(usuariosBD);

            if (!IsPostBack)
            {
                Session.Add("productos_terminados", desperdicioMerma.get_productos_terminados());
                Session.Add("insumos", desperdicioMerma.get_insumos());
                configurar_controles();
                cargar_resumen();
            }
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_resumen();
        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {

        }

        protected void dropdown_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            configurar_controles();
            cargar_resumen();
        }
    }
}