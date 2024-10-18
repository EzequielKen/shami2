using _08___sistemas_marketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasMarketing
{
    public partial class solicitud_abierta : System.Web.UI.Page
    {
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        #region atributos
        cls_solicitud_franquicia solicitudes;
        DataTable usuariosBD;
        DataTable solicitud;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];

            solicitudes = new cls_solicitud_franquicia(usuariosBD);

            solicitud = solicitudes.get_solicitud(Session["id_solicitud"].ToString());

            label_fecha.Text ="Fecha: " + solicitud.Rows[0]["fecha"].ToString();
            label_nombre.Text ="Nombre: " + solicitud.Rows[0]["nombre"].ToString();
            label_apellido.Text ="Apellido: " + solicitud.Rows[0]["apellido"].ToString();
            label_dni.Text ="D.N.I: " + solicitud.Rows[0]["dni"].ToString();

            label_provincia.Text = "Provincia: " + solicitud.Rows[0]["provincia"].ToString();
            label_localidad.Text = "Localidad: " + solicitud.Rows[0]["localidad"].ToString();

            label_mail.Text = "Mail: " + solicitud.Rows[0]["mail"].ToString();
            label_telefono.Text = "Telefono: " + solicitud.Rows[0]["telefono"].ToString();
            
            label_conocieron.Text = solicitud.Rows[0]["conocieron"].ToString();
            label_experiencia.Text = solicitud.Rows[0]["experiencia"].ToString();

            label_presentacion.Text = solicitud.Rows[0]["presentacion"].ToString();

        }
    }
}