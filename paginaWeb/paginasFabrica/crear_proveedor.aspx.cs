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
    public partial class crear_proveedor : System.Web.UI.Page
    {
        cls_crear_proveedor crear_Proveedor;
        DataTable usuariosBD;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            crear_Proveedor = new cls_crear_proveedor(usuariosBD);
        }

        protected void boton_carga_Click(object sender, EventArgs e)
        {
            if (verificar_datos())
            {
                crear_Proveedor.crear_proveedor(textbox_nombre_proveedor.Text, textbox_provincia.Text, textbox_localidad.Text, textbox_direccion.Text, textbox_telefono.Text, textbox_condicion_pago.Text, textbox_cbu_1.Text, textbox_cbu_2.Text, textbox_cbu_3.Text, textbox_cbu_4.Text, textbox_cbu_5.Text);
                Response.Redirect("/paginasFabrica/proveedores_fabrica.aspx", false);
            }
        }

        private bool verificar_datos()
        {
            bool retorno = true;
            if (textbox_nombre_proveedor.Text == string.Empty ||
                textbox_provincia.Text == string.Empty ||
                textbox_localidad.Text == string.Empty ||
                textbox_direccion.Text == string.Empty ||
                textbox_telefono.Text == string.Empty)
            {
                retorno = false;
            }
            return retorno;
        }
    }
}