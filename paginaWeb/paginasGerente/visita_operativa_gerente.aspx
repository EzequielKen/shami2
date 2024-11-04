<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="visita_operativa_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.visita_operativa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h1>Visita Operativa.</h1>


            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Sucursal" runat="server" />
                                <asp:DropDownList ID="DropDown_sucursal" CssClass="form-control" runat="server" OnSelectedIndexChanged="DropDown_sucursal_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <asp:Button ID="boton_historial" CssClass="btn btn-primary" Text="Historial Visita Operativa" OnClick="boton_historial_Click" runat="server" />
                        </div>
                        <hr />
           
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
