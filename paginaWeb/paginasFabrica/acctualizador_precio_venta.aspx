<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="acctualizador_precio_venta.aspx.cs" Inherits="paginaWeb.paginasFabrica.acctualizador_precio_venta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="alert alert-light">
                        <div class="input-group">
                            <label>Tipo de acuerdo</label>
                            <asp:DropDownList ID="dropdown_acuerdo" CssClass="form-select" runat="server">
                            </asp:DropDownList>
                            <label>Tipo Producto</label>
                            <asp:DropDownList ID="dropdown_tipo" CssClass="form-select" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
