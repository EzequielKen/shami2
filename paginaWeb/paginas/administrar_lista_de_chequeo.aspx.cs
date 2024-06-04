using _02___sistemas;
using _03___sistemas_fabrica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginas
{
    public partial class administrar_lista_de_chequeo : System.Web.UI.Page
    {
        #region llenar datos
        private void crear_tabla_chequeo()
        {
            lista_de_chequeo = new DataTable();
            lista_de_chequeo.Columns.Add("id",typeof(string));
            lista_de_chequeo.Columns.Add("actividad", typeof(string));
        }
        private void llenar_tabla_chequeo()
        {
            crear_tabla_chequeo();
            int ultima_fila = 0;    
            for (int fila = 0; fila <=lista_de_chequeoBD.Rows.Count-1; fila++)
            {
                if (lista_de_chequeoBD.Rows[fila]["categoria"].ToString()==dropDown_tipo.SelectedItem.Text)
                {
                    lista_de_chequeo.Rows.Add();
                    ultima_fila= lista_de_chequeo.Rows.Count-1;
                    lista_de_chequeo.Rows[ultima_fila]["id"] = lista_de_chequeoBD.Rows[fila]["id"].ToString();
                    lista_de_chequeo.Rows[ultima_fila]["actividad"] = lista_de_chequeoBD.Rows[fila]["actividad"].ToString();  
                }
            }
        }
        private void cargar_lista_chequeo()
        {
            llenar_tabla_chequeo();
            gridview_chequeos.DataSource = lista_de_chequeo;
            gridview_chequeos.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {
           llenar_dropDownList(lista_de_chequeoBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "categoria";
            dt = dt.DefaultView.ToTable();

            //        item = new ListItem("Todos", num_item.ToString());
            //        dropDown_tipo.Items.Add(item);
            //        num_item = num_item + 1;

            tipo_seleccionado = dt.Rows[0]["categoria"].ToString();
            item = new ListItem(dt.Rows[0]["categoria"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {


                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["categoria"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["categoria"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        #endregion
        /// <summary>
        /// //////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_administrar_lista_de_chequeo administrador;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable lista_de_chequeoBD;
        DataTable lista_de_chequeo;
        DataTable equipos;

        string tipo_seleccionado;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["administracion_de_chequeo"] == null)
            {
                Session.Add("administracion_de_chequeo", new cls_administrar_lista_de_chequeo(usuariosBD));
            }
            administrador = (cls_administrar_lista_de_chequeo)Session["administracion_de_chequeo"];
            lista_de_chequeoBD = administrador.get_lista_de_chequeo();
            if (!IsPostBack)
            {
                configurar_controles();
            }
            cargar_lista_chequeo();
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_lista_chequeo();

        }
    }
}