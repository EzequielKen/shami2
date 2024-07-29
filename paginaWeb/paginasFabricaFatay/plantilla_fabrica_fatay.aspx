<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="plantilla_fabrica_fatay.aspx.cs" Inherits="paginaWeb.paginasFabricaFatay.plantilla_fabrica_fatay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
    <div class="row">

        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
        </div>
        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
            
            <div class="alert alert-light">
                <asp:Button ID="boton_plantilla_insumo" CssClass="btn btn-primary" Text="Plantilla de stock - INSUMOS" runat="server" OnClick="boton_plantilla_insumo_Click" />
            </div>
        </div>
        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
        </div>

    </div>
</div>
</asp:Content>
