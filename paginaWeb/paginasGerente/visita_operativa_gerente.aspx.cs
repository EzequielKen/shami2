using _07_sistemas_supervision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class visita_operativa : System.Web.UI.Page
    {
        #region modificar empleado
       
        private bool verificar_cargos_cargados(int fila_empleado)
        {
            bool retorno = false;
            int cantidad_cargos = 0;
            if (lista_de_empleadoBD.Rows[fila_empleado]["Encargado"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (lista_de_empleadoBD.Rows[fila_empleado]["Cajero"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (lista_de_empleadoBD.Rows[fila_empleado]["Shawarmero"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (lista_de_empleadoBD.Rows[fila_empleado]["Atencion al Cliente"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (lista_de_empleadoBD.Rows[fila_empleado]["Cocina"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (lista_de_empleadoBD.Rows[fila_empleado]["Limpieza"].ToString() != "N/A")
            {
                cantidad_cargos++;
            }

            if (cantidad_cargos > 0)
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
        #region carga empleado
        private void crear_lista_de_empleado()
        {
            lista_de_empleado = new DataTable();
            lista_de_empleado.Columns.Add("id", typeof(string));
            lista_de_empleado.Columns.Add("id_sucursal", typeof(string));
            lista_de_empleado.Columns.Add("nombre", typeof(string));
            lista_de_empleado.Columns.Add("apellido", typeof(string));
            lista_de_empleado.Columns.Add("dni", typeof(string));
            lista_de_empleado.Columns.Add("telefono", typeof(string));
            lista_de_empleado.Columns.Add("cargo", typeof(string));

            lista_de_empleado.Columns.Add("Encargado", typeof(string));
            lista_de_empleado.Columns.Add("Cajero", typeof(string));
            lista_de_empleado.Columns.Add("Shawarmero", typeof(string));
            lista_de_empleado.Columns.Add("Atencion al Cliente", typeof(string));
            lista_de_empleado.Columns.Add("Cocina", typeof(string));
            lista_de_empleado.Columns.Add("Limpieza", typeof(string));
        }
       
        
    
        #endregion
        #region crear empleado
        private void crear_tabla_empleado()
        {
            empleado = new DataTable();
            empleado.Columns.Add("id_sucursal", typeof(string));
            empleado.Columns.Add("nombre", typeof(string));
            empleado.Columns.Add("apellido", typeof(string));
            empleado.Columns.Add("dni", typeof(string));
            empleado.Columns.Add("telefono", typeof(string));
            empleado.Columns.Add("cargo", typeof(string));
        }

        private string configurar_cargo()
        {
            string retorno = string.Empty;
            int cantidad_cargos = 0;
            string cargos = string.Empty;
            if ((bool)Session["Encargado"])
            {
                cargos = cargos + "-Encargado";
                cantidad_cargos++;
            }

            if ((bool)Session["Cajero"])
            {
                cargos = cargos + "-Cajero";
                cantidad_cargos++;
            }

            if ((bool)Session["Shawarmero"])
            {
                cargos = cargos + "-Shawarmero";
                cantidad_cargos++;
            }

            if ((bool)Session["atencion"])
            {
                cargos = cargos + "-Atencion al Cliente";
                cantidad_cargos++;
            }

            if ((bool)Session["Cocina"])
            {
                cargos = cargos + "-Cocina";
                cantidad_cargos++;
            }

            if ((bool)Session["Limpieza"])
            {
                cargos = cargos + "-Limpieza";
                cantidad_cargos++;
            }
            retorno = cantidad_cargos.ToString() + cargos;
            return retorno;
        }

        #endregion
        #region configurar controles
     
       

        private void configurar_cargos()
        {
            if (Session["encargado"] == null)
            {
                bool encargado = false;
                Session.Add("encargado", encargado);
            }
            if (Session["cajero"] == null)
            {
                bool cajero = false;
                Session.Add("cajero", cajero);
            }
            if (Session["shawarmero"] == null)
            {
                bool shawarmero = false;
                Session.Add("shawarmero", shawarmero);
            }
            if (Session["atencion"] == null)
            {
                bool atencion = false;
                Session.Add("atencion", atencion);
            }
            if (Session["cocina"] == null)
            {
                bool cocina = false;
                Session.Add("cocina", cocina);
            }
            if (Session["limpieza"] == null)
            {
                bool limpieza = false;
                Session.Add("limpieza", limpieza);
            }

        }
        #endregion
      
        private void cargar_sucursales()
        {
          
        }
      
        /// <summary>
        /// ////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_visita_operativa visita;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable sucursal;
        DataTable empleado;

        DataTable sucursales;
        DataTable lista_de_empleadoBD;
        DataTable lista_de_empleado;
        DateTime fecha_de_hoy = DateTime.Now;



        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];

            visita = new cls_visita_operativa(usuariosBD);
            if (!IsPostBack)
            {
                sucursales = visita.get_sucursales();
                Session.Add("sucursales", sucursales);
                cargar_sucursales();
            }
            sucursales = (DataTable)Session["sucursales"];
           
        }


        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha = calendario.SelectedDate;
            gridview_chequeos.DataSource = visita.get_historial_evaluacion_chequeo(fecha);
            gridview_chequeos.DataBind();
        }

        protected void boton_historial_Click(object sender, EventArgs e)
        {
            Button boton_historial = (Button)sender;
            GridViewRow row = (GridViewRow)boton_historial.NamingContainer;
            int fila = row.RowIndex;

            string sucursal = gridview_chequeos.Rows[fila].Cells[1].Text;
            Session.Add("sucursal", visita.get_sucursal(sucursal));
            Response.Redirect("~/paginasSupervision/historial_visita_operativa.aspx", false);
        }

       
    }
}