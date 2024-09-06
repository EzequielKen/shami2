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
    public partial class orden_de_compras : System.Web.UI.Page
    {
        #region cargar resumen
        private void cargar_resumen()
        {

            gridView_resumen.DataSource = (DataTable)Session["resumen_orden_pedido"];
            gridView_resumen.DataBind();
        }
        #endregion
        private void crear_tabla_insumos()
        {
            insumos_fabrica = new DataTable();

            insumos_fabrica.Columns.Add("id", typeof(string));
            insumos_fabrica.Columns.Add("producto", typeof(string));
        }
        private void llenar_tabla_insumo(string nombre_producto)
        {
            crear_tabla_insumos();
            int fila_insumo = 0;
            for (int fila = 0; fila <= insumos_fabricaBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(nombre_producto, insumos_fabricaBD.Rows[fila]["producto"].ToString()))
                {
                    insumos_fabrica.Rows.Add();

                    insumos_fabrica.Rows[fila_insumo]["id"] = insumos_fabricaBD.Rows[fila]["id"].ToString();
                    insumos_fabrica.Rows[fila_insumo]["producto"] = insumos_fabricaBD.Rows[fila]["producto"].ToString();

                    fila_insumo++;
                }
            }
            Session.Add("insumos_fabrica_orden_pedido", insumos_fabrica);

        }
        private void crear_tabla_proveedores()
        {
            proveedores = new DataTable();

            proveedores.Columns.Add("id", typeof(string));
            proveedores.Columns.Add("proveedor", typeof(string));
            proveedores.Columns.Add("direccion", typeof(string));
        }

        private void llenar_tabla_proveedores()
        {
            crear_tabla_proveedores();
            int fila_proveedor = 0;
            for (int fila = 0; fila <= proveedoresBD.Rows.Count - 1; fila++)
            {
                if (funciones.buscar_alguna_coincidencia(textbox_buscar.Text, proveedoresBD.Rows[fila]["proveedor"].ToString()))
                {
                    proveedores.Rows.Add();

                    proveedores.Rows[fila_proveedor]["id"] = proveedoresBD.Rows[fila]["id"].ToString();
                    proveedores.Rows[fila_proveedor]["proveedor"] = proveedoresBD.Rows[fila]["proveedor"].ToString();
                    proveedores.Rows[fila_proveedor]["direccion"] = proveedoresBD.Rows[fila]["direccion"].ToString();
                    fila_proveedor++;
                }
            }
        }
        private void llenar_tabla_proveedores_por_id()
        {
            crear_tabla_proveedores();
            int fila_proveedor = 0;
            string id_proveedor;
            for (int fila_lista = 0; fila_lista <= id_proveedores_seleccionados.Count - 1; fila_lista++)
            {
                id_proveedor = id_proveedores_seleccionados[fila_lista].ToString();
                for (int fila = 0; fila <= proveedoresBD.Rows.Count - 1; fila++)
                {
                    if (proveedoresBD.Rows[fila]["id"].ToString() == id_proveedor)
                    {
                        proveedores.Rows.Add();

                        proveedores.Rows[fila_proveedor]["id"] = proveedoresBD.Rows[fila]["id"].ToString();
                        proveedores.Rows[fila_proveedor]["proveedor"] = proveedoresBD.Rows[fila]["proveedor"].ToString();
                        proveedores.Rows[fila_proveedor]["direccion"] = proveedoresBD.Rows[fila]["direccion"].ToString();
                        fila_proveedor++;
                    }
                }
            }
            gridView_proveedores.DataSource = proveedores;
            gridView_proveedores.DataBind();
        }
        private void cargar_proveedores()
        {
            llenar_tabla_proveedores();

            gridView_proveedores.DataSource = proveedores;
            gridView_proveedores.DataBind();

            gridView_producto.DataSource = (DataTable)Session["insumos_fabrica_orden_pedido"];
            gridView_producto.DataBind();
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_proveedores_fabrica proveedores_de_fabrica;

        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable proveedoresBD;
        DataTable proveedores;
        DataTable insumos_fabricaBD;
        DataTable insumos_fabrica;
        List<string> id_proveedores_seleccionados;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Remove("id_proveedor_fabrica_seleccionado");
            Session.Remove("proveedor_fabrica_seleccionado");
            Session.Remove("insumos_de_proveedor");
            usuariosBD = (DataTable)Session["usuariosBD"];

            if (Session["id_orden_pedido_seleccionado"] != null)
            {
                if (Session["resumen_orden_pedido"] != null)
                {
                    cargar_resumen();
                }
            }



            proveedores_de_fabrica = new cls_proveedores_fabrica(usuariosBD);

            if (!IsPostBack)
            {
                proveedoresBD = proveedores_de_fabrica.get_proveedores_de_fabrica();
                Session.Add("lista_proveedoresBD", proveedoresBD);
                insumos_fabricaBD = proveedores_de_fabrica.get_insumos_fabrica_optimizado();
                Session.Add("insumos_fabricaBD", insumos_fabricaBD);
            }
            proveedoresBD = (DataTable)Session["lista_proveedoresBD"];
            insumos_fabricaBD = (DataTable)Session["insumos_fabricaBD"];
            if (!IsPostBack)
            {
                cargar_proveedores();
            }
        }

        protected void gridView_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("id_proveedor_fabrica_seleccionado", gridView_proveedores.SelectedRow.Cells[0].Text);
            Session.Add("titulo_proveedor", gridView_proveedores.SelectedRow.Cells[1].Text);
            Response.Redirect("/paginasFabrica/crear_orden_de_compra.aspx", false);
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_proveedores();
        }

        protected void textbox_buscar_producto_TextChanged(object sender, EventArgs e)
        {
            llenar_tabla_insumo(textbox_buscar_producto.Text);
            cargar_proveedores();
        }

        protected void gridView_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_proveedores_seleccionados = proveedores_de_fabrica.get_proveedores_seleccionados(gridView_producto.SelectedRow.Cells[0].Text);
            llenar_tabla_proveedores_por_id();
        }
    }
}