using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasFabrica
{
    public partial class movimiento_mercaderia_interna : System.Web.UI.Page
    {
        private void crear_tabla_transaccion()
        {
            transaccion = new DataTable();
            transaccion.Columns.Add("producto", typeof(string));
            transaccion.Columns.Add("cantidad", typeof(string));
            transaccion.Columns.Add("entrega", typeof(string));
            transaccion.Columns.Add("recibe", typeof(string));
            transaccion.Columns.Add("direccion", typeof(string));
            transaccion.Columns.Add("contacto", typeof(string));
            transaccion.Columns.Add("nota", typeof(string));
        }
        private void cargar_transaccion()
        {
            crear_tabla_transaccion();
            transaccion.Rows.Add();

            transaccion.Rows[0]["producto"] = textbox_producto.Text;
            transaccion.Rows[0]["cantidad"] = textbox_cantidad.Text;
            transaccion.Rows[0]["entrega"] = textbox_entrega.Text;
            transaccion.Rows[0]["recibe"] = textbox_recibe.Text;
            transaccion.Rows[0]["direccion"] = textbox_direccion.Text;
            transaccion.Rows[0]["contacto"] = textbox_contacto.Text;
            transaccion.Rows[0]["nota"] = textbox_nota.Text;

        }
        private bool verificar_campos_oblogatorios()
        {
            bool verificado = true;
            if (textbox_entrega.Text == string.Empty)
            {
                verificado = false;
            }
            if (textbox_recibe.Text == string.Empty)
            {
                verificado = false;
            }
            if (textbox_direccion.Text == string.Empty)
            {
                verificado = false;
            }
            if (textbox_contacto.Text == string.Empty)
            {
                verificado = false;
            }
            if (textbox_producto.Text == string.Empty)
            {
                verificado = false;
            }
            if (textbox_cantidad.Text == string.Empty)
            {
                verificado = false;
            }

            return verificado;
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_movimiento_mercaderia_interna_gerente movimientos;
        DataTable usuariosBD;

        DataTable transaccion;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            movimientos = new cls_movimiento_mercaderia_interna_gerente(usuariosBD);

        }

        protected void boton_cargar_Click(object sender, EventArgs e)
        {
            if (verificar_campos_oblogatorios())
            {
                cargar_transaccion();
                movimientos.cargar_transaccion(transaccion);
                textbox_entrega.Text = string.Empty;
                textbox_recibe.Text = string.Empty;
                textbox_direccion.Text = string.Empty;
                textbox_contacto.Text = string.Empty;
                textbox_producto.Text = string.Empty;
                textbox_cantidad.Text = string.Empty;
                textbox_nota.Text = string.Empty;
            }
        }

        protected void calendario_SelectionChanged(object sender, EventArgs e)
        {
            DateTime fecha = calendario.SelectedDate;
            Session.Add("fecha_movimiento_mercaderia", fecha);
            label_fecha_seleccionada.Text = "Fecha Seleccionada: " + fecha.ToString("dd/MM/yyyy");
            gridView_movimientos.DataSource = movimientos.get_movimiento_mercaderia_interna(fecha);
            gridView_movimientos.DataBind();
        }

        protected void boton_pdf_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridView_movimientos.Rows[rowIndex].Cells[0].Text;

            DateTime hora = DateTime.Now;
            string dato_hora = hora.DayOfYear.ToString() + hora.Hour.ToString() + hora.Minute.ToString() + hora.Second.ToString();
            string id_pedido = Session["sucursal"].ToString() + " pedido-" + "- id-" + dato_hora + ".pdf";
            string ruta = "~/paginasFabrica/pdf/" + id_pedido;
            string ruta_archivo = Server.MapPath(ruta);

            byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/imagenes/logo-completo.png"));

            movimientos.Generar_PDF(id, ruta_archivo, imgdata);
            //           Response.Redirect("~/archivo.pdf");
            string strUrl = "/paginasFabrica/pdf/" + id_pedido;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            //GenerarPDF_Click();
        }

        protected void boton_eliminar_Click(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int rowIndex = row.RowIndex;
            string id = gridView_movimientos.Rows[rowIndex].Cells[0].Text;

            movimientos.eliminar_movimiento(id);
            DateTime fecha = (DateTime)Session["fecha_movimiento_mercaderia"];
            label_fecha_seleccionada.Text = "Fecha Seleccionada: " + fecha.ToString("dd/MM/yyyy");
            gridView_movimientos.DataSource = movimientos.get_movimiento_mercaderia_interna(fecha);
            gridView_movimientos.DataBind();
        }
    }
}