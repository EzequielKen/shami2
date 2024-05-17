using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class administrar_tipo_movimiento_caja_chica : System.Web.UI.Page
    {
        private void crear_tipo_movimientos()
        {
            tipo_movimientos = new DataTable();
            tipo_movimientos.Columns.Add("id", typeof(string));
            tipo_movimientos.Columns.Add("tipo_movimiento", typeof(string));
            tipo_movimientos.Columns.Add("concepto", typeof(string));
            tipo_movimientos.Columns.Add("estado", typeof(string));
        }
        private void llenar_tipo_movimientos()
        {
            crear_tipo_movimientos();
            int ultima_fila;
            for (int fila = 0; fila <= tipo_movimientosBD.Rows.Count - 1; fila++)
            {
                if (DropDown_tipo_movimiento.SelectedItem.Text == tipo_movimientosBD.Rows[fila]["tipo_movimiento"].ToString())
                {
                    tipo_movimientos.Rows.Add();
                    ultima_fila = tipo_movimientos.Rows.Count - 1;
                    tipo_movimientos.Rows[ultima_fila]["id"] = tipo_movimientosBD.Rows[fila]["id"].ToString();
                    tipo_movimientos.Rows[ultima_fila]["tipo_movimiento"] = tipo_movimientosBD.Rows[fila]["tipo_movimiento"].ToString();
                    tipo_movimientos.Rows[ultima_fila]["concepto"] = tipo_movimientosBD.Rows[fila]["concepto"].ToString();
                    tipo_movimientos.Rows[ultima_fila]["estado"] = tipo_movimientosBD.Rows[fila]["estado"].ToString();
                }
            }
        }
        private void cargar_tipo_movimientos()
        {
            tipo_movimientosBD = caja_Chica.get_tipo_movimientos_caja_chica();
            llenar_tipo_movimientos();
            gridView_conceptos.DataSource = tipo_movimientos;
            gridView_conceptos.DataBind();
            if (DropDown_tipo_movimiento.SelectedItem.Text == "Ingreso")
            {
                DropDown_tipo_movimiento.CssClass = "btn btn-success dropdown-toggle";
                gridView_conceptos.CssClass= "table table-success text-center";
            }
            else
            {
                DropDown_tipo_movimiento.CssClass = "btn btn-danger dropdown-toggle";
                gridView_conceptos.CssClass = "table table-danger text-center";

            }
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_caja_chica caja_Chica;
        DataTable usuariosBD;
        DataTable tipo_movimientosBD;
        DataTable tipo_movimientos;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["caja_Chica"] == null)
            {
                Session.Add("caja_Chica", new cls_caja_chica(usuariosBD));
            }
            caja_Chica = (cls_caja_chica)Session["caja_Chica"];
            if (!IsPostBack)
            {
                cargar_tipo_movimientos();
            }
        }

        protected void DropDown_tipo_movimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_tipo_movimientos();
        }

        protected void boton_registrar_Click(object sender, EventArgs e)
        {
            caja_Chica.registrar_concepto(DropDown_tipo_movimiento.SelectedItem.Text, textbox_concepto.Text);
            textbox_concepto.Text = string.Empty;
            cargar_tipo_movimientos();
        }

        protected void gridView_conceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int fila = 0; fila <= gridView_conceptos.Rows.Count - 1; fila++)
            {
                if (gridView_conceptos.Rows[fila].Cells[2].Text == "Habilitado")
                {
                    Button boton_habilitacion = (Button)gridView_conceptos.Rows[fila].Cells[3].Controls[0].FindControl("boton_habilitacion");
                    boton_habilitacion.Text = "Deshabilitar";
                    boton_habilitacion.CssClass = "btn btn-warning";
                }
                else if (gridView_conceptos.Rows[fila].Cells[2].Text == "Deshabilitado")
                {
                    Button boton_habilitacion = (Button)gridView_conceptos.Rows[fila].Cells[3].Controls[0].FindControl("boton_habilitacion");
                    boton_habilitacion.Text = "Habilitar";
                    boton_habilitacion.CssClass = "btn btn-success";
                }
            }
        }

        protected void boton_habilitacion_Click(object sender, EventArgs e)
        {
            Button boton_habilitacion = (Button)sender;
            GridViewRow row = (GridViewRow)boton_habilitacion.NamingContainer;
            int rowIndex = row.RowIndex;
            string id_concepto = gridView_conceptos.Rows[rowIndex].Cells[0].Text;

            if (gridView_conceptos.Rows[rowIndex].Cells[2].Text== "Habilitado")
            {
                caja_Chica.deshabilitar_concepto(id_concepto);
            }
            else if (gridView_conceptos.Rows[rowIndex].Cells[2].Text == "Deshabilitado")
            {
                caja_Chica.habilitar_concepto(id_concepto);
            }
            cargar_tipo_movimientos();

        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {
            Button boton_habilitacion = (Button)sender;
            GridViewRow row = (GridViewRow)boton_habilitacion.NamingContainer;
            int rowIndex = row.RowIndex;
            string id_concepto = gridView_conceptos.Rows[rowIndex].Cells[0].Text;
            caja_Chica.eliminar_concepto(id_concepto);

           
            cargar_tipo_movimientos();
        }

        protected void boton_modificar_Click(object sender, EventArgs e)
        {
            Button boton_modificar = (Button)sender;
            GridViewRow row = (GridViewRow)boton_modificar.NamingContainer;
            int rowIndex = row.RowIndex;

            TextBox textbox_modificar = (TextBox)gridView_conceptos.Rows[rowIndex].FindControl("textbox_modificar");
            
            string id_concepto = gridView_conceptos.Rows[rowIndex].Cells[0].Text;
            string concepto = gridView_conceptos.Rows[rowIndex].Cells[1].Text;
            string nuevo_concepto = textbox_modificar.Text;
            caja_Chica.modificar_concepto(id_concepto, concepto,nuevo_concepto);


            cargar_tipo_movimientos();
        }
    }
}