using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using _03___sistemas_fabrica;

namespace paginaWeb.paginasFabrica
{
    public partial class landing_page : System.Web.UI.Page
    {
        private void construir_pagina()
        {
            PlaceHolder1.Controls.Clear();
            landing.consultar_registro_actividad_de_empleado();
            bool esta_registrado;
            int fila = 0;
            while (fila <= cargos.Rows.Count - 1)
            {
                //CREAR ROW

                HtmlGenericControl row_div = new HtmlGenericControl("div");
                row_div.Attributes["class"] = "row";

                for (int col = 1; col <= 3; col++)
                {

                    //CREAR COL TEXT CENTER
                    HtmlGenericControl col_div = new HtmlGenericControl("div");
                    col_div.Attributes["class"] = "col text-center";
                    if (fila <= cargos.Rows.Count - 1)
                    {
                        esta_registrado = landing.verificar_si_registro(cargos.Rows[fila]["id"].ToString());
                        HtmlGenericControl card_div = new HtmlGenericControl("div");
                        card_div.Attributes["class"] = "card mx-auto ";
                        card_div.Attributes["style"] = "width: 18rem;";

                        HtmlImage img = new HtmlImage();
                        if (esta_registrado)
                        {
                            img.Src = "/imagenes/landing/check.png";
                        }
                        else
                        {
                            img.Src = cargos.Rows[fila]["url"].ToString();
                        }
                        img.Attributes["class"] = "card-img-top";

                        HtmlGenericControl card_Body_Div = new HtmlGenericControl("div");
                        card_Body_Div.Attributes["class"] = "card-body";

                        HtmlGenericControl h5 = new HtmlGenericControl("h5");
                        h5.Attributes["class"] = "card-title";
                        h5.InnerText = cargos.Rows[fila]["tarea"].ToString();

                        HtmlGenericControl p = new HtmlGenericControl("p");
                        p.Attributes["class"] = "card-text";
                        p.InnerText = cargos.Rows[fila]["sub_tarea"].ToString();
                        if (landing.verificar_correltividad(cargos.Rows[fila]["correlatividad"].ToString()) &&
                            !landing.verificar_si_registro(cargos.Rows[fila]["id"].ToString()))
                        {
                            Button boton = new Button();
                            boton.ID = "boton_" + cargos.Rows[fila]["id"].ToString();
                            boton.Text = "Hacer clic";
                            boton.CssClass = "btn btn-primary";
                            boton.CommandArgument = cargos.Rows[fila]["id"].ToString();
                            boton.Click += new EventHandler(boton_id_Click);

                            card_Body_Div.Controls.Add(h5);
                            card_Body_Div.Controls.Add(p);
                            card_Body_Div.Controls.Add(boton);

                        }
                        else
                        {
                            card_Body_Div.Controls.Add(h5);
                            card_Body_Div.Controls.Add(p);
                        }
                       

                        card_div.Controls.Add(img);
                        card_div.Controls.Add(card_Body_Div);

                        col_div.Controls.Add(card_div);
                        fila++;


                    }
                    row_div.Controls.Add(col_div);


                }

                PlaceHolder1.Controls.Add(row_div);
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        cls_landing_page landing;
        DataTable tipo_usuario;
        DataTable cargos;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["landing"] == null)
            {
                Session.Add("landing", new cls_landing_page((DataTable)Session["usuariosBD"]));
            }
            landing = (cls_landing_page)Session["landing"];
            tipo_usuario = (DataTable)Session["tipo_usuario"];
            cargos = landing.get_descripcion_de_cargos(tipo_usuario.Rows[0]["rol"].ToString());
            construir_pagina();
        }

        protected void boton_id_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            string id = boton.CommandArgument;

            landing.registrar_actividad(id);
            string redirect_url = landing.get_redirect(id);
            Response.Redirect(redirect_url, false);
        }


    }
}