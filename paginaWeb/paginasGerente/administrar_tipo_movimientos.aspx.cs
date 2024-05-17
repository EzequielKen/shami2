using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using _06___sistemas_gerente;

namespace paginaWeb.paginasGerente
{
    public partial class administrar_tipo_movimientos : System.Web.UI.Page
    {
        private void cargar_movientos()
        {
            gridView_tipo_movimientos.DataSource = tipo_movimientos_caja_chica;
            gridView_tipo_movimientos.DataBind();
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_caja_chica caja;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable tipo_movimientos_caja_chica;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["caja_chica"] == null)
            {
                Session.Add("caja_chica", new cls_caja_chica(usuariosBD));
            }
            caja = (cls_caja_chica)Session["caja_chica"];

            tipo_movimientos_caja_chica = caja.get_tipo_movimientos_caja_chica();
            cargar_movientos();
        }
    }
}