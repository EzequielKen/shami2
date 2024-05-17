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
    public partial class actualizador_precio_vegetales : System.Web.UI.Page
    {
        private string calcular_promedio_de_ganancia()
        {
            double retorno = 0;
            int iteraciones=0;
            double total=0;
            for (int fila = 0; fila < productosBD.Rows.Count-1; fila++)
            {
                if (productosBD.Rows[fila]["precio_compra"] !="N/A")
                {
                    if (productosBD.Rows[fila]["precio_final"] != "N/A")//precio_final
                    {
                        
                        total = total + double.Parse(productosBD.Rows[fila]["porcentaje_final"].ToString());
                    }
                    else
                    {
                        total = total + double.Parse(productosBD.Rows[fila]["porcentaje_aumento"].ToString());
                    }
                    iteraciones ++;
                }
            }
            if (total == 0 && iteraciones==0)
            {
                retorno = 0;
            }
            else
            {
                retorno = Math.Round(total / iteraciones, 2);
            }
            return retorno.ToString() + "%";
        }
        #region carga precios
        private string calcular_total_compra()
        { //sub_total_compra
            double total_compra = 0;


            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["sub_total_compra"].ToString() != "N/A")
                {
                    total_compra = total_compra + double.Parse(productosBD.Rows[fila]["sub_total_compra"].ToString());
                }
            }
            return funciones.formatCurrency(total_compra);

        }
        private void cargar_precio_stock(string id, string precio_dato, string stock_dato, string redondeo_dato)
        {
            double precio_compra, precio, diferencia, porcentaje, precio_nuevo, stock_actual, stock_nuevo, precio_final, porcentaje_final, diferencia_final;
            int fila = funciones.buscar_fila_por_id(id, productosBD);
            if (double.TryParse(precio_dato, out precio))
            {

                if (precio == 0)
                {
                    productosBD.Rows[fila]["precio_compra"] = "N/A";
                }
                else
                {
                    productosBD.Rows[fila]["precio_compra"] = precio.ToString();

                }

                if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                {
                    precio = 0;
                }
                else
                {
                    precio = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                }

                porcentaje = double.Parse(productosBD.Rows[fila]["porcentaje_aumento"].ToString());

                diferencia = (porcentaje * precio) / 100;

                precio_nuevo = precio + diferencia;
                productosBD.Rows[fila]["precio_nuevo"] = precio_nuevo.ToString();
                productosBD.Rows[fila]["diferencia"] = diferencia.ToString();
                productosBD.Rows[fila]["porcentaje_aumento"] = porcentaje.ToString();
            }
            if (double.TryParse(stock_dato, out stock_nuevo))
            {
                if (stock_nuevo == 0)
                {
                    productosBD.Rows[fila]["stock_nuevo"] = "N/A";
                }
                else
                {

                    stock_actual = double.Parse(productosBD.Rows[fila]["stock"].ToString());
                    stock_nuevo = stock_actual + stock_nuevo;
                    productosBD.Rows[fila]["stock_nuevo"] = stock_nuevo.ToString();
                }



            }
            if (double.TryParse(redondeo_dato, out precio_final))
            {
                if (precio_final == 0)
                {
                    productosBD.Rows[fila]["precio_final"] = "N/A";
                }
                else
                {
                    productosBD.Rows[fila]["precio_final"] = precio_final.ToString();

                }

                if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
                {
                    precio_compra = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                    diferencia_final = precio_final - precio_compra;

                    porcentaje_final = (diferencia_final * 100) / precio_compra;
                    porcentaje_final = Math.Round(porcentaje_final, 2);
                    productosBD.Rows[fila]["precio_final"] = precio_final.ToString();
                }
                else
                {
                    diferencia_final = 0;
                    porcentaje_final = 0;
                    productosBD.Rows[fila]["precio_final"] = "N/A";
                }


                productosBD.Rows[fila]["diferencia_final"] = diferencia_final.ToString();
                productosBD.Rows[fila]["porcentaje_final"] = porcentaje_final.ToString();

            }

            if (productosBD.Rows[fila]["stock_nuevo"].ToString() != "N/A" && productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
            {
                double comprado, precio_pagado;
                comprado = double.Parse(productosBD.Rows[fila]["stock_nuevo"].ToString()) - double.Parse(productosBD.Rows[fila]["stock"].ToString());
                precio_pagado = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                productosBD.Rows[fila]["sub_total_compra"] = Math.Round(comprado * precio_pagado, 2);
            }
            else
            {
                productosBD.Rows[fila]["sub_total_compra"] = "N/A";
            }


            Session.Remove("productosBD_vegetales");
            Session.Add("productosBD_vegetales", productosBD);

        }
        private void cargar_porcentaje()
        {
            double porcentaje;
            double precio, diferencia, precio_nuevo;

            if (double.TryParse(textbox_porcentaje_de_aumento.Text.Replace(",", "."), out porcentaje))
            {
                textbox_porcentaje_de_aumento.Text = string.Empty;
                porcentaje_de_aumento = porcentaje;
                Session.Remove("porcentaje_de_aumento");
                Session.Add("porcentaje_de_aumento", porcentaje_de_aumento);
                label_porcentaje.Text = "Porcentaje de aumento: %" + porcentaje_de_aumento.ToString();
                for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
                {
                    if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                    {
                        precio = 0;
                    }
                    else
                    {
                        precio = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                    }

                    diferencia = (porcentaje * precio) / 100;

                    precio_nuevo = precio + diferencia;
                    productosBD.Rows[fila]["precio_nuevo"] = precio_nuevo.ToString();
                    productosBD.Rows[fila]["diferencia"] = diferencia.ToString();
                    productosBD.Rows[fila]["porcentaje_aumento"] = porcentaje.ToString();
                }
            }
        }
        #endregion
        #region creacion
        private void crear_tabla_productos()
        {
            productos = new DataTable();

            productos.Columns.Add("id", typeof(string));
            productos.Columns.Add("producto", typeof(string));
            productos.Columns.Add("precio_compra", typeof(string));
            productos.Columns.Add("sub_total_compra", typeof(string));
            productos.Columns.Add("precio", typeof(string));
            productos.Columns.Add("precio_nuevo", typeof(string));
            productos.Columns.Add("diferencia", typeof(string));
            productos.Columns.Add("porcentaje_aumento", typeof(string));
            productos.Columns.Add("stock", typeof(string));
            productos.Columns.Add("stock_nuevo", typeof(string));
            productos.Columns.Add("tipo_producto", typeof(string));
            productos.Columns.Add("unidad_de_medida", typeof(string));

            productos.Columns.Add("diferencia_final", typeof(string));
            productos.Columns.Add("porcentaje_final", typeof(string));
            productos.Columns.Add("precio_final", typeof(string));

        }
        private void crear_tabla_productosBD()
        {
            productosBD = new DataTable();

            productosBD.Columns.Add("id", typeof(string));
            productosBD.Columns.Add("producto", typeof(string));
            productosBD.Columns.Add("precio_compra", typeof(string));
            productosBD.Columns.Add("sub_total_compra", typeof(string));
            productosBD.Columns.Add("precio", typeof(string));
            productosBD.Columns.Add("precio_nuevo", typeof(string));
            productosBD.Columns.Add("diferencia", typeof(string));
            productosBD.Columns.Add("porcentaje_aumento", typeof(string));
            productosBD.Columns.Add("stock", typeof(string));
            productosBD.Columns.Add("stock_nuevo", typeof(string));
            productosBD.Columns.Add("tipo_producto", typeof(string));
            productosBD.Columns.Add("unidad_de_medida", typeof(string));

            productosBD.Columns.Add("diferencia_final", typeof(string));
            productosBD.Columns.Add("porcentaje_final", typeof(string));
            productosBD.Columns.Add("precio_final", typeof(string));

            int fila_producto = 0;
            for (int fila = 0; fila <= productos_proveedorBD.Rows.Count - 1; fila++)
            {
                productosBD.Rows.Add();
                productosBD.Rows[fila_producto]["id"] = productos_proveedorBD.Rows[fila]["id"].ToString();
                productosBD.Rows[fila_producto]["producto"] = productos_proveedorBD.Rows[fila]["producto"].ToString();
                productosBD.Rows[fila_producto]["precio_compra"] = "N/A";
                productosBD.Rows[fila_producto]["sub_total_compra"] = "N/A";
                productosBD.Rows[fila_producto]["precio"] = productos_proveedorBD.Rows[fila]["precio"].ToString();
                productosBD.Rows[fila_producto]["stock"] = productos_proveedorBD.Rows[fila]["stock"].ToString();
                productosBD.Rows[fila_producto]["stock_nuevo"] = "N/A";
                productosBD.Rows[fila_producto]["tipo_producto"] = productos_proveedorBD.Rows[fila]["tipo_producto"].ToString();
                productosBD.Rows[fila_producto]["unidad_de_medida"] = productos_proveedorBD.Rows[fila]["unidad_de_medida_fabrica"].ToString();

                productosBD.Rows[fila_producto]["precio_nuevo"] = "0";
                productosBD.Rows[fila_producto]["diferencia"] = "0";

                productosBD.Rows[fila_producto]["diferencia_final"] = "0";
                productosBD.Rows[fila_producto]["porcentaje_final"] = "0";
                productosBD.Rows[fila_producto]["precio_final"] = "N/A";


                fila_producto++;
            }
            Session.Add("productosBD_vegetales", productosBD);
        }

        private void llenar_datatable()
        {
            crear_tabla_productos();
            productosBD = (DataTable)Session["productosBD_vegetales"];

            tipo_seleccionado = Session["tipo_seleccionado"].ToString();
            int fila_producto = 0;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (tipo_seleccionado == productosBD.Rows[fila]["tipo_producto"].ToString() &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    productos.Rows[fila_producto]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[fila_producto]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["precio_compra"] = productosBD.Rows[fila]["precio_compra"].ToString();

                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio_compra"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_compra"].ToString()));

                    }

                    if (productosBD.Rows[fila]["sub_total_compra"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["sub_total_compra"] = productosBD.Rows[fila]["sub_total_compra"].ToString();

                    }
                    else
                    {
                        productos.Rows[fila_producto]["sub_total_compra"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["sub_total_compra"].ToString()));

                    }


                    productos.Rows[fila_producto]["precio"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio"].ToString()));
                    productos.Rows[fila_producto]["precio_nuevo"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_nuevo"].ToString()));
                    productos.Rows[fila_producto]["diferencia"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["diferencia"].ToString()));

                    productos.Rows[fila_producto]["porcentaje_aumento"] = productosBD.Rows[fila]["porcentaje_aumento"].ToString() + "%";
                    productos.Rows[fila_producto]["stock"] = productosBD.Rows[fila]["stock"].ToString();
                    productos.Rows[fila_producto]["stock_nuevo"] = productosBD.Rows[fila]["stock_nuevo"].ToString();
                    productos.Rows[fila_producto]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[fila_producto]["unidad_de_medida"] = productosBD.Rows[fila]["unidad_de_medida"].ToString();

                    productos.Rows[fila_producto]["diferencia_final"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["diferencia_final"].ToString()));
                    productos.Rows[fila_producto]["porcentaje_final"] = productosBD.Rows[fila]["porcentaje_final"].ToString() + "%";

                    if (productosBD.Rows[fila]["precio_final"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["precio_final"] = productosBD.Rows[fila]["precio_final"].ToString();
                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio_final"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_final"].ToString()));

                    }
                    fila_producto++;
                }
            }
        }
        private void llenar_datatable_solo_comprados()
        {
            crear_tabla_productos();
            productosBD = (DataTable)Session["productosBD_vegetales"];

            tipo_seleccionado = Session["tipo_seleccionado"].ToString();
            int fila_producto = 0;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A" &&
                    tipo_seleccionado == productosBD.Rows[fila]["tipo_producto"].ToString() &&
                    funciones.buscar_alguna_coincidencia(textbox_busqueda.Text, productosBD.Rows[fila]["producto"].ToString()))
                {
                    productos.Rows.Add();
                    productos.Rows[fila_producto]["id"] = productosBD.Rows[fila]["id"].ToString();
                    productos.Rows[fila_producto]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["precio_compra"] = productosBD.Rows[fila]["precio_compra"].ToString();

                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio_compra"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_compra"].ToString()));

                    }

                    if (productosBD.Rows[fila]["sub_total_compra"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["sub_total_compra"] = productosBD.Rows[fila]["sub_total_compra"].ToString();

                    }
                    else
                    {
                        productos.Rows[fila_producto]["sub_total_compra"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["sub_total_compra"].ToString()));

                    }


                    productos.Rows[fila_producto]["precio"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio"].ToString()));
                    productos.Rows[fila_producto]["precio_nuevo"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_nuevo"].ToString()));
                    productos.Rows[fila_producto]["diferencia"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["diferencia"].ToString()));

                    productos.Rows[fila_producto]["porcentaje_aumento"] = productosBD.Rows[fila]["porcentaje_aumento"].ToString() + "%";
                    productos.Rows[fila_producto]["stock"] = productosBD.Rows[fila]["stock"].ToString();
                    productos.Rows[fila_producto]["stock_nuevo"] = productosBD.Rows[fila]["stock_nuevo"].ToString();
                    productos.Rows[fila_producto]["tipo_producto"] = productosBD.Rows[fila]["tipo_producto"].ToString();
                    productos.Rows[fila_producto]["unidad_de_medida"] = productosBD.Rows[fila]["unidad_de_medida"].ToString();

                    productos.Rows[fila_producto]["diferencia_final"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["diferencia_final"].ToString()));
                    productos.Rows[fila_producto]["porcentaje_final"] = productosBD.Rows[fila]["porcentaje_final"].ToString() + "%";

                    if (productosBD.Rows[fila]["precio_final"].ToString() == "N/A")
                    {
                        productos.Rows[fila_producto]["precio_final"] = productosBD.Rows[fila]["precio_final"].ToString();
                    }
                    else
                    {
                        productos.Rows[fila_producto]["precio_final"] = funciones.formatCurrency(double.Parse(productosBD.Rows[fila]["precio_final"].ToString()));

                    }
                    fila_producto++;
                }
            }
        }
        private void cargar_productos()
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);

            llenar_datatable();
            gridview_productos.DataSource = null;
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();
            label_promedio.Text = "Promedio de ganancia: " + calcular_promedio_de_ganancia();

        }
        private void cargar_productos_solo_comprados()
        {
            Session.Add("tipo_seleccionado", dropDown_tipo.SelectedItem.Text);

            llenar_datatable_solo_comprados();
            gridview_productos.DataSource = null;
            gridview_productos.DataSource = productos;
            gridview_productos.DataBind();

            label_promedio.Text = "Promedio de ganancia: " + calcular_promedio_de_ganancia();
        }
        private void configurar_controles()
        {
            llenar_dropDownList(productos_proveedorBD);
        }
        private void llenar_dropDownList(DataTable dt)
        {
            dropDown_tipo.Items.Clear();
            int num_item = 1;
            ListItem item;
            dt.DefaultView.Sort = "tipo_producto";
            dt = dt.DefaultView.ToTable();

            tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
            item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
            dropDown_tipo.Items.Add(item);
            num_item = num_item + 1;
            for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
            {
                if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
                {
                    item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                    dropDown_tipo.Items.Add(item);
                    num_item = num_item + 1;
                }
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////
        cls_sistema_actualizador_precios_vegetales actualizador_precio;
        cls_funciones funciones = new cls_funciones();
        DataTable proveedorBD;
        DataTable usuariosBD;

        DataTable productos_proveedorBD;
        DataTable productosBD;
        DataTable productos;
        double porcentaje_de_aumento = 0;
        string tipo_seleccionado;

        protected void Page_Load(object sender, EventArgs e)
        {
            proveedorBD = (DataTable)Session["proveedorBD"];
            usuariosBD = (DataTable)Session["usuariosBD"];
            if (Session["usuariosBD"] == null)
            {
                Response.Redirect("/paginas/login.aspx", false);
            }
            else
            {
                if (Session["actualizador_precio"] == null)
                {
                    Session.Add("actualizador_precio", new cls_sistema_actualizador_precios_vegetales(usuariosBD));
                }
                actualizador_precio = (cls_sistema_actualizador_precios_vegetales)Session["actualizador_precio"];

                if (Session["productos_proveedor"] == null)
                {
                    Session.Add("productos_proveedor", actualizador_precio.get_productos_proveedor(proveedorBD.Rows[0]["nombre_en_BD"].ToString()));
                }
                productos_proveedorBD = (DataTable)Session["productos_proveedor"];
                if (Session["productosBD_vegetales"] == null)
                {
                    crear_tabla_productosBD();

                }

                productosBD = (DataTable)Session["productosBD_vegetales"];




                if (Session["porcentaje_de_aumento"] != null)
                {
                    porcentaje_de_aumento = (double)Session["porcentaje_de_aumento"];   
                    label_porcentaje.Text = "Porcentaje de aumento: " + porcentaje_de_aumento.ToString() + "%";
                }
                else
                {
                    porcentaje_de_aumento = 25;
                    Session.Add("porcentaje_de_aumento", porcentaje_de_aumento);
                    label_porcentaje.Text = "Porcentaje de aumento: " + porcentaje_de_aumento.ToString() + "%";
                    for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
                    {
                        productosBD.Rows[fila]["porcentaje_aumento"] = porcentaje_de_aumento.ToString();
                    }
                }
                if (!IsPostBack)
                {

                    configurar_controles();
                    cargar_productos();

                }



            }
           

        }

        protected void boton_enviar_Click(object sender, EventArgs e)
        {
            actualizador_precio.cargar_nuevos_precio((DataTable)Session["productosBD_vegetales"], proveedorBD.Rows[0]["nombre_en_BD"].ToString());
            Response.Redirect("~/paginasFabrica/sucursales.aspx", false);
            
        
        }

        protected void textbox_busqueda_TextChanged(object sender, EventArgs e)
        {
            cargar_productos();

           
        }

        protected void dropDown_tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargar_productos();

           
        }

        protected void gridview_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox txtValor_compra = (gridview_productos.SelectedRow.Cells[3].FindControl("texbox_precio_compra") as TextBox);
            TextBox txtValor_stock = (gridview_productos.SelectedRow.Cells[5].FindControl("texbox_stock_nuevo") as TextBox);
            TextBox txtValor_redondeo = (gridview_productos.SelectedRow.Cells[10].FindControl("texbox_precio_redondeado") as TextBox);

            string id = gridview_productos.SelectedRow.Cells[0].Text;
            cargar_precio_stock(id, txtValor_compra.Text.Replace(",", "."), txtValor_stock.Text.Replace(",", "."), txtValor_redondeo.Text.Replace(",", "."));
           // textbox_busqueda.Text = string.Empty;
            cargar_productos();
            label_total_compra.Text = "TOTAL COMPRADO:" + calcular_total_compra();
          
            

        }

        protected void textbox_porcentaje_de_aumento_TextChanged(object sender, EventArgs e)
        {
            cargar_porcentaje();
            cargar_productos();
           
            
        }



        protected void boton_filtro_Click(object sender, EventArgs e)
        {
            cargar_productos_solo_comprados();

          
        }

        protected void mostrar_todo_Click(object sender, EventArgs e)
        {
            cargar_productos();

          
        }
    }
}