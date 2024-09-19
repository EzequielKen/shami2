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
                    <asp:TextBox ID="textbox_fecha_inicial" runat="server" CssClass="form-control" TextMode="Date" OnTextChanged="textbox_fecha_inicial_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <h2 class="form-label">Fecha Final</h2>
                    <asp:TextBox ID="textbox_fecha_final" runat="server" CssClass="form-control" TextMode="Date" OnTextChanged="textbox_fecha_final_TextChanged" AutoPostBack="true"></asp:TextBox>
                </div>
            </div>

            <div class="alert alert-light">
                <div class="input-group">
                    <h2 class="form-label">Tipo Producto:</h2>
                    <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true" runat="server">
                    </asp:DropDownList>
                </div>
                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="producto" DataField="producto" />
                        <asp:TemplateField HeaderText="Analisis">
                            <ItemTemplate>
                                <asp:Button ID="boton_analisis" CssClass="input-group btn btn-primary" Text="Analizar ventas" OnClick="boton_analisis_Click" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
