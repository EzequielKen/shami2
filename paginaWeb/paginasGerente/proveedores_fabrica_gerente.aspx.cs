using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class proveedores_fabrica_gerente : System.Web.UI.Page
    {

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

        private void cargar_proveedores()
        {
            llenar_tabla_proveedores();

            gridView_proveedores.DataSource = proveedores;
            gridView_proveedores.DataBind();
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_proveedores_fabrica proveedores_de_fabrica;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable proveedoresBD;
        DataTable proveedores;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Remove("id_proveedor_fabrica_seleccionado");
            Session.Remove("proveedor_fabrica_seleccionado");
            Session.Remove("insumos_de_proveedor");
            usuariosBD = (DataTable)Session["usuariosBD"];
            

            proveedores_de_fabrica = new cls_proveedores_fabrica(usuariosBD);

            proveedoresBD = proveedores_de_fabrica.get_proveedores_de_fabrica();
            cargar_proveedores();
        }
        protected void boton_crear_proveedor_Click(object sender, EventArgs e)
        {
            Response.Redirect("/paginasFabrica/crear_proveedor.aspx", false);
        }

        protected void textbox_buscar_TextChanged(object sender, EventArgs e)
        {
            cargar_proveedores();
        }

        protected void gridView_proveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("id_proveedor_fabrica_seleccionado", gridView_proveedores.SelectedRow.Cells[0].Text);
            Response.Redirect("/paginasFabrica/linkear_insumos_a_proveedor.aspx", false);
        }
    }
}