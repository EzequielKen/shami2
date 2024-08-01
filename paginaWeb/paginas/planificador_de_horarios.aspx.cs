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
    public partial class planificador_de_horarios : System.Web.UI.Page
    {
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_planificador_de_horarios planificador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            usuariosBD = (DataTable)Session["usuariosBD"];
            sucursal = (DataTable)Session["sucursal"];
            planificador = new cls_planificador_de_horarios(usuariosBD);

            gridview_empleados.DataSource = planificador.get_lista_de_empleado(sucursal.Rows[0]["id"].ToString());
            gridview_empleados.DataBind();

        }

        protected void gridview_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}