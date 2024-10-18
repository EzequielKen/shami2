<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraMarketing.Master" AutoEventWireup="true" CodeBehind="solicitud_abierta.aspx.cs" Inherits="paginaWeb.paginasMarketing.solicitud_abierta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <h2>
                        <asp:Label ID="label_fecha" Text="text" runat="server" />
                    </h2>
                    <h2>
                        <asp:Label ID="label_nombre" Text="text" runat="server" />
                    </h2>
                    <h2>
                        <asp:Label ID="label_apellido" Text="text" runat="server" />
                    </h2>
                    <h2>
                        <asp:Label ID="label_dni" Text="text" runat="server" />
                    </h2>
                </div>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <h2>
                        <asp:Label ID="label_provincia" Text="text" runat="server" />
                    </h2>
                    <h2>
                        <asp:Label ID="label_localidad" Text="text" runat="server" />
                    </h2>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <h2>
                        <asp:Label ID="label_mail" Text="text" runat="server" />
                    </h2>
                    <h2>
                        <asp:Label ID="label_telefono" Text="text" runat="server" />
                    </h2>
                </div>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <h2>Como nos conocieron:</h2>
                    <asp:Label ID="label_conocieron" Text="text" runat="server" />
                    <h2>Experiencia:</h2>
                    <asp:Label ID="label_experiencia" Text="text" runat="server" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <h2>Presentacion:</h2>
                    <asp:Label ID="label_presentacion" Text="text" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
