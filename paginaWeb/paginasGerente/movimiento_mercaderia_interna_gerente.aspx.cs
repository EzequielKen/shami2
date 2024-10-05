using _06___sistemas_gerente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace paginaWeb.paginasGerente
{
    public partial class movimiento_mercaderia_interna_gerente : System.Web.UI.Page
    {
        private void crear_tabla_transaccion()
        {
            transaccion = new DataTable();
            transaccion.Columns.Add("producto",typeof(string));
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
            }
        }
    }
}