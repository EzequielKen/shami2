<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="analisis_de_ventas.aspx.cs" Inherits="paginaWeb.paginasFabrica.analisis_de_ventas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="alert alert-light">
                <h1>Seleccione el rango de fechas para el análisis de ventas</h1>
                <div class="input-group">
                    <h2 class="form-label">Fecha Inicial</h2>
                    <asp:TextBox ID="textbox_fecha_inicial" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <h2 class="form-label">Fecha Final</h2>
                    <asp:TextBox ID="textbox_fecha_final" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
            </div>

            <div class="alert alert-light">
                <div class="input-group">
                    <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="Buscar..." OnTextChanged="textbox_buscar_TextChanged" runat="server" />
                    <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" runat="server">
                    </asp:DropDownList>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
