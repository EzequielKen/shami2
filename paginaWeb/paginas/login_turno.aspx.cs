using _02___sistemas;
using Newtonsoft.Json.Linq;
using paginaWeb.paginasFabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class login_turno : System.Web.UI.Page
    {
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_sistema_login login = new cls_sistema_login();
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable empleado;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            empleado = (DataTable)Session["empleado"];

        }

        protected void boton_turno_1_Click(object sender, EventArgs e)
        {
            string id_empleado = empleado.Rows[0]["id"].ToString();
            login.actualizar_turno_empleado(id_empleado,"Turno 1");
                Response.Redirect("~/paginas/lista_de_chequeo.aspx", false);
        }

        protected void boton_turno_2_Click(object sender, EventArgs e)
        {
            string id_empleado = empleado.Rows[0]["id"].ToString();
            login.actualizar_turno_empleado(id_empleado, "Turno 2");
            Response.Redirect("~/paginas/lista_de_chequeo.aspx", false);
        }
    }
}