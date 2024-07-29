<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="recetario_linkear_sub_producto.aspx.cs" Inherits="paginaWeb.paginasFabrica.recetario_linkear_sub_producto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col">
                        <asp:Button CssClass="btn btn-primary" Text="Crear Sub Producto" runat="server" ID="boton_crear_sub_producto" OnClick="boton_crear_sub_producto_Click" />
                    </div>
                    <div class="col"></div>
                    <div class="col"></div>
                </div>
            </div>

            <hr />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
