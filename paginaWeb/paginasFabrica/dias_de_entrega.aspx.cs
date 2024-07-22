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
    public partial class dias_de_entrega : System.Web.UI.Page
    {
        private void aplicar_cambios(int fila, int fila_tabla)
        {
            for (int columna = 1; columna <= 7; columna++)
            {
                DropDownList dropdown_dia = (gridview_dia_de_entrega.Rows[fila].Cells[columna].FindControl("dropdown_" + gridview_dia_de_entrega.Columns[columna].HeaderText) as DropDownList);

                calendario_entrega.Rows[fila_tabla][gridview_dia_de_entrega.Columns[columna].HeaderText] = dropdown_dia.SelectedItem.Text;
            }
            Session.Add("calendario_entrega", calendario_entrega);
        }
        private void agregar_fila_a_calendario()
        {
            calendario_entrega = (DataTable)Session["calendario_entrega"];
            int nueva_id = buscar_ultima_id() - 1;
            calendario_entrega.Rows.Add();
            int ultima_fila = calendario_entrega.Rows.Count - 1;
            calendario_entrega.Rows[ultima_fila]["id"] = 0 - Math.Abs(nueva_id);
            for (int columna = calendario_entrega.Columns["lunes"].Ordinal; columna <= calendario_entrega.Columns.Count - 1; columna++)
            {
                calendario_entrega.Rows[ultima_fila][columna] = "N/A";
            }
            Session.Add("calendario_entrega", calendario_entrega);
        }
        private int buscar_ultima_id()
        {
            int retorno = 0;
            if (calendario_entrega.Rows.Count > 0)
            {
                int ultima_fila = calendario_entrega.Rows.Count - 1;
                retorno = int.Parse(calendario_entrega.Rows[ultima_fila]["id"].ToString());
            }
            return retorno;
        }
        private void cargar_dias_de_entrega()
        {
            calendario_entrega = (DataTable)Session["calendario_entrega"];
            gridview_dia_de_entrega.DataSource = calendario_entrega;
            gridview_dia_de_entrega.DataBind();

            sucursales.DefaultView.Sort = "sucursal ASC";
            sucursales = sucursales.DefaultView.ToTable();
            gridView_sucursales.DataSource = sucursales;
            gridView_sucursales.DataBind();

            gridView_usuarios.DataSource = usuarios;
            gridView_usuarios.DataBind();
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_dia_de_entrega dia_entrega;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;

        DataTable sucursales;
        DataTable usuarios;
        DataTable calendario_entrega;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["dia_entrega"] == null)
            {
                Session.Add("dia_entrega", new cls_dia_de_entrega(usuariosBD));
            }
            dia_entrega = (cls_dia_de_entrega)Session["dia_entrega"];
            sucursales = dia_entrega.get_sucursales();
            usuarios = dia_entrega.get_usuarios();
            if (Session["calendario_entrega"] == null)
            {
                calendario_entrega = dia_entrega.get_dias_de_entrega();
                Session.Add("calendario_entrega", calendario_entrega);
            }
            calendario_entrega = (DataTable)Session["calendario_entrega"];
            if (!IsPostBack)
            {
                cargar_dias_de_entrega();
            }
        }
        #region lista
        protected void gridview_dia_de_entrega_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "boton_eliminar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                calendario_entrega = (DataTable)Session["calendario_entrega"];

                int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);

                calendario_entrega.Rows[fila_tabla].Delete();
                Session.Add("calendario_entrega", calendario_entrega);

                cargar_dias_de_entrega();
            }
        }

        protected void gridview_dia_de_entrega_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            calendario_entrega = (DataTable)Session["calendario_entrega"];
            int fila_tabla;
            for (int fila = 0; fila <= gridview_dia_de_entrega.Rows.Count - 1; fila++)
            {
                fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
                for (int columna = 1; columna <= 7; columna++)
                {
                    DropDownList dropdown_dia = (gridview_dia_de_entrega.Rows[fila].Cells[columna].FindControl("dropdown_" + gridview_dia_de_entrega.Columns[columna].HeaderText) as DropDownList);
                    if (dropdown_dia.Items.Count == 0)
                    {
                        dropdown_dia.Items.Add("N/A");
                        for (int fila_sucursal = 0; fila_sucursal <= sucursales.Rows.Count - 1; fila_sucursal++)
                        {
                            dropdown_dia.Items.Add(sucursales.Rows[fila_sucursal]["sucursal"].ToString());
                        }
                    }
                    if (calendario_entrega.Rows[fila_tabla][gridview_dia_de_entrega.Columns[columna].HeaderText] != "N/A")
                    {
                        dropdown_dia.SelectedValue = calendario_entrega.Rows[fila_tabla][gridview_dia_de_entrega.Columns[columna].HeaderText].ToString();
                    }
                    if (int.Parse(calendario_entrega.Rows[fila_tabla]["id"].ToString()) > 0)
                    {
                        gridview_dia_de_entrega.Rows[fila].Cells[8].Controls[0].Visible = false;
                    }
                }
            }
        }
        #endregion
        #region dias de la semana
        protected void dropdown_lunes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_martes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_miercoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_jueves_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_viernes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_sabado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }

        protected void dropdown_domingo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown_dato = (DropDownList)sender;
            GridViewRow row = (GridViewRow)dropdown_dato.NamingContainer;
            int fila = row.RowIndex;
            calendario_entrega = (DataTable)Session["calendario_entrega"];

            int fila_tabla = funciones.buscar_fila_por_id(gridview_dia_de_entrega.Rows[fila].Cells[0].Text, calendario_entrega);
            aplicar_cambios(fila, fila_tabla);
            cargar_dias_de_entrega();
        }
        #endregion
        protected void boton_agregar_fila_Click(object sender, EventArgs e)
        {
            agregar_fila_a_calendario();
            cargar_dias_de_entrega();
        }

        protected void boton_guardar_cambios_Click(object sender, EventArgs e)
        {
            calendario_entrega = (DataTable)Session["calendario_entrega"];
            dia_entrega.cargar_dias(calendario_entrega);
            calendario_entrega = dia_entrega.get_dias_de_entrega();
            Session.Add("calendario_entrega", calendario_entrega);
            cargar_dias_de_entrega();

        }
    }
}