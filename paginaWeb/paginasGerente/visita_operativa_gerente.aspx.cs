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
            DropDown_sucursal.Items.Clear();
            for (int fila = 0; fila <= sucursales.Rows.Count - 1; fila++)
            {
                DropDown_sucursal.Items.Add(sucursales.Rows[fila]["sucursal"].ToString());
            }
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
            if (!IsPostBack)
            {
                Session.Add("sucursal", visita.get_sucursal(DropDown_sucursal.SelectedItem.Text));
               
            }
        }



       

       

        protected void boton_encargado_Click(object sender, EventArgs e)
        {
            Session.Add("encargado", !(bool)Session["encargado"]);
            configurar_cargos();


        }

        protected void boton_cajero_Click(object sender, EventArgs e)
        {
            Session.Add("cajero", !(bool)Session["cajero"]);
            configurar_cargos();

        }

        protected void boton_shawarmero_Click(object sender, EventArgs e)
        {
            Session.Add("shawarmero", !(bool)Session["shawarmero"]);
            configurar_cargos();

        }

        protected void boton_atencion_Click(object sender, EventArgs e)
        {
            Session.Add("atencion", !(bool)Session["atencion"]);
            configurar_cargos();

        }

        protected void boton_cocina_Click(object sender, EventArgs e)
        {
            Session.Add("cocina", !(bool)Session["cocina"]);
            configurar_cargos();

        }

        protected void boton_limpieza_Click(object sender, EventArgs e)
        {
            Session.Add("limpieza", !(bool)Session["limpieza"]);
            configurar_cargos();

        }





        protected void DropDown_sucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session.Add("sucursal", visita.get_sucursal(DropDown_sucursal.SelectedItem.Text));
            sucursal = (DataTable)Session["sucursal"];
            lista_de_empleadoBD = visita.get_lista_de_empleado_origen(sucursal.Rows[0]["id"].ToString(), fecha_de_hoy);
            Session.Add("lista_de_empleadoBD", lista_de_empleadoBD);
           
        }

        protected void boton_evaluar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/paginasSupervision/evaluar_visita_operativa.aspx");
        }

        

        protected void boton_historial_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/paginasSupervision/historial_visita_operativa.aspx", false);
        }
    }
}