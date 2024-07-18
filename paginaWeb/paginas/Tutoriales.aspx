<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="Tutoriales.aspx.cs" Inherits="paginaWeb.paginas.Tutoriales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col">
                <div class="card" style="width: 18rem;">
                    <video class="card-img-top" controls="controls">
                        <source src="https://shamisicas.com.ar/videos_pagina/tutoriales/productos/tutorial_falafel.mp4" type="video/mp4" />
                        Tu navegador no soporta el elemento de video.
               
                    </video>
                    <div class="card-body">
                        <h5 class="card-title">Tutorial Preparacion de Sandwich Falafel</h5>
                        <p class="card-text">Tutorial paso a paso de la preparacion del falafel.</p>
                    </div>
                </div>
            </div>
            <div class="col">
                <div class="card" style="width: 18rem;">
                    <video class="card-img-top" controls="controls">
                        <source src="https://shamisicas.com.ar/videos_pagina/tutoriales/productos/tutorial_falafel2.mp4" type="video/mp4" />
                        Tu navegador no soporta el elemento de video.
               
                    </video>
                    <div class="card-body">
                        <h5 class="card-title">Tutorial Preparacion de Sandwich Falafel parte 2</h5>
                        <p class="card-text">Tutorial paso a paso de la preparacion del falafel.</p>
                    </div>
                </div>
            </div>
            <div class="col"></div>

        </div>
    </div>
</asp:Content>
