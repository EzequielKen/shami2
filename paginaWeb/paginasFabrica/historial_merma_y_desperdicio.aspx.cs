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
    public partial class historial_merma_y_desperdicio : System.Web.UI.Page
    {
        private void cargar_nota(string id_orden, string nota)
        {
            if (nota != string.Empty)
            {
                historial_merma_desperdicio.cargar_nota(id_orden, nota);
            }
        }
        #region cargar datos detalle
        private void crear_tabla_detalle()
        {
            detalle = new DataTable();
            detalle.Columns.Add("id", typeof(string));  
            detalle.Columns.Add("producto", typeof(string));  
            detalle.Columns.Add("presentacion", typeof(string)); 
        }
        private void llenar_tabla_detalle(string id_desperdicio_merma)
        {
            crear_tabla_detalle();
            string presentacion;
            int fila_desperdicio = funciones.buscar_fila_por_id(id_desperdicio_merma, merma_y_desperdicioBD);
            int fila_detalle =0;
            for (int columna = merma_y_desperdicioBD.Columns["producto_1"].Ordinal; columna <= merma_y_desperdicioBD.Columns.Count-1; columna++)
            {
                if (merma_y_desperdicioBD.Rows[fila_desperdicio][columna].ToString()!="N/A")
                {
                    detalle.Rows.Add();
                    detalle.Rows[fila_detalle]["id"] = funciones.obtener_dato(merma_y_desperdicioBD.Rows[fila_desperdicio][columna].ToString(),1);
                    detalle.Rows[fila_detalle]["producto"] = funciones.obtener_dato(merma_y_desperdicioBD.Rows[fila_desperdicio][columna].ToString(),2);
                    presentacion = funciones.obtener_dato(merma_y_desperdicioBD.Rows[fila_desperdicio][columna].ToString(), 3) + funciones.obtener_dato(merma_y_desperdicioBD.Rows[fila_desperdicio][columna].ToString(), 4);
                    detalle.Rows[fila_detalle]["presentacion"] = presentacion;
                    fila_detalle++;
                }
            }
        }
        private void cargar_detalle(string id_desperdicio_merma)
        {
            llenar_tabla_detalle(id_desperdicio_merma);
            gridView_detalle.DataSource = detalle;
            gridView_detalle.DataBind();
        }
        #endregion
        #region cargar datos
        private void crear_tabla_merma_desperdicio()
        {
            merma_y_desperdicio = new DataTable();
            merma_y_desperdicio.Columns.Add("id", typeof(string));
            merma_y_desperdicio.Columns.Add("fecha", typeof(string)); 
            merma_y_desperdicio.Columns.Add("tipo", typeof(string)); 
            merma_y_desperdicio.Columns.Add("nota", typeof(string));
        }

        private void llenar_tabla_merma_desperdicio()
        {
            crear_tabla_merma_desperdicio();
            merma_y_desperdicioBD = historial_merma_desperdicio.get_merma_y_desperdicio();
            int fila_merma = 0;
            for (int fila = 0; fila <= merma_y_desperdicioBD.Rows.Count-1; fila++)
            {
                if (funciones.verificar_fecha(merma_y_desperdicioBD.Rows[fila]["fecha"].ToString(),dropDown_mes.SelectedItem.Text,dropDown_año.SelectedItem.Text))
                {
                    merma_y_desperdicio.Rows.Add();

                    merma_y_desperdicio.Rows[fila_merma]["id"] = merma_y_desperdicioBD.Rows[fila]["id"].ToString();
                    merma_y_desperdicio.Rows[fila_merma]["fecha"] = merma_y_desperdicioBD.Rows[fila]["fecha"].ToString(); 
                    merma_y_desperdicio.Rows[fila_merma]["tipo"] = merma_y_desperdicioBD.Rows[fila]["tipo"].ToString(); 
                    merma_y_desperdicio.Rows[fila_merma]["nota"] = merma_y_desperdicioBD.Rows[fila]["nota"].ToString();
                    fila_merma++;
                }
            }
        }

        private void cargar_merma_desperdicio()
        {
            llenar_tabla_merma_desperdicio();
            gridView_desperdicio_merma.DataSource = merma_y_desperdicio;
            gridView_desperdicio_merma.DataBind();
        }
        #endregion
        #region configurar controles
        private void configurar_controles()
        {

            cargar_dropDowns();
        }
        private void cargar_dropDowns()
        {
            cargar_mes();
            cargar_año();
        }
       
        private void cargar_mes()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int mes = 1; mes <= 12; mes++)
            {
                item = new System.Web.UI.WebControls.ListItem(mes.ToString(), num_item.ToString());
                dropDown_mes.Items.Add(item);
                num_item++;
            }
            dropDown_mes.SelectedValue = DateTime.Now.Month.ToString();
        }
        private void cargar_año()
        {
            int num_item = 1;
            System.Web.UI.WebControls.ListItem item;
            for (int año = 2022; año <= DateTime.Now.Year; año++)
            {
                item = new System.Web.UI.WebControls.ListItem(año.ToString(), num_item.ToString());
                dropDown_año.Items.Add(item);
                num_item++;
            }
            string añ = DateTime.Now.Year.ToString();

            dropDown_año.SelectedIndex = dropDown_año.Items.Count - 1;
        }
        #endregion
        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        #region atributos
        cls_historial_merma_y_desperdicio historial_merma_desperdicio;
        cls_cargar_orden_de_compra orden_compra;
        cls_funciones funciones = new cls_funciones();
        DataTable usuariosBD;
        DataTable proveedorBD;
        DataTable tipo_usuario;

        DataTable merma_y_desperdicioBD;
        DataTable merma_y_desperdicio;
        DataTable detalle;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            usuariosBD = (DataTable)Session["usuariosBD"];
            proveedorBD = (DataTable)Session["proveedorBD"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            if (Session["historial_merma_desperdicio"]==null)
            {
                Session.Add("historial_merma_desperdicio", new cls_historial_merma_y_desperdicio(usuariosBD));
            }
            historial_merma_desperdicio = (cls_historial_merma_y_desperdicio)Session["historial_merma_desperdicio"];
            merma_y_desperdicioBD = historial_merma_desperdicio.get_merma_y_desperdicio();
            if (!IsPostBack)
            {
                configurar_controles();
                cargar_merma_desperdicio();
            }
        }

       

        protected void dropDown_mes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropDown_año_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridView_desperdicio_merma_SelectedIndexChanged1(object sender, EventArgs e)
        {
        }

        protected void gridView_desperdicio_merma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "cargar")
            {
                int fila = int.Parse(e.CommandArgument.ToString());
                label_id.Text= "ID:"+ gridView_desperdicio_merma.Rows[fila].Cells[0].Text + " | ";
                label_fecha.Text= gridView_desperdicio_merma.Rows[fila].Cells[1].Text;
                label_tipo.Text= gridView_desperdicio_merma.Rows[fila].Cells[2].Text;
                label_nota.Text= gridView_desperdicio_merma.Rows[fila].Cells[6].Text;
                cargar_detalle(gridView_desperdicio_merma.Rows[fila].Cells[0].Text);

            }
            else if (e.CommandName == "cargar_nota")
            {
                string index = e.CommandArgument.ToString();

                string id = gridView_desperdicio_merma.Rows[int.Parse(index)].Cells[0].Text;
                TextBox textbox_nota = (gridView_desperdicio_merma.Rows[int.Parse(index)].Cells[4].FindControl("textbox_nota") as TextBox);

                //cancelar pedido
                cargar_nota(id, textbox_nota.Text);
                cargar_merma_desperdicio();
            }
        }
    }
}