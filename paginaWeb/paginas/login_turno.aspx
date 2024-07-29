<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraLogin.Master" AutoEventWireup="true" CodeBehind="login_turno.aspx.cs" Inherits="paginaWeb.paginas.login_turno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="alert alert-light">
                <h1>Seleccione su Turno.</h1>

                <div class="row">
                    <asp:Button ID="boton_turno_1" CssClass="btn btn-primary" Text="Turno 1" runat="server" OnClick="boton_turno_1_Click"/>
                </div>
                <div class="row pt-5">
                    <asp:Button ID="boton_turno_2" CssClass="btn btn-primary" Text="Turno 2" runat="server" OnClick="boton_turno_2_Click"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
