<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestra.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="paginaWeb.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Masthead-->
    <header class="masthead">
        <div class="container px-4 px-lg-5 h-100">
            <div class="row gx-4 gx-lg-5 h-100 align-items-center justify-content-center text-center">
                <div class="col-lg-8 align-self-end">
                    <div class=" container">
                        <img class="img-fluid" src="/imagenes/logo-completo.png" alt="Alternate Text"/></div>
 
                    <h1 class="text-white font-weight-bold">Sistema Integral de Controles Administrativos Shami</h1>
                    <h1 class="text-white font-weight-bold">(S.I.C.A.S.)</h1>
                    <!-- Sabores del Cercano Oriente en Buenos Aires -->
                    <hr class="divider" />
                </div>
                <div class="col-lg-8 align-self-baseline">
                    <p class="text-white-75 mb-5">Somos SHAMI! Tenemos el Shawarma mas rico que vas a probar en tu vida. Veni a disfrutarlo con nosotros en uno de nuestros mas de 10 locales.</p>
                  <!--  <a class="btn btn-primario btn-xl" href="#about">Encontrar un local</a>-->
                </div>
            </div>
        </div>
    </header>

   <!-- <section class="section_menu" id="section_menu">
        <div class="container">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <style>
                        .section_menu .section_title:before {
                            content: "Menu"
                        }

                        @media ( max-width: 768px ) {
                            .section_menu .section_title:before {
                                display: none;
                            }
                        }
                    </style>
                    <h2 class="section_title">− Menu −</h2>
                    <hr>
                    <p class="section_caption">
                        En nuestro menú encontrarás una gran variedad de propuestas árabes para satisfacer todos los gustos. ¡Explorá todos nuestros platos!
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <div class="text-center">
                        <asp:Button CssClass="btn btn-primario" Text="ver menu" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </section>-->
</asp:Content>
