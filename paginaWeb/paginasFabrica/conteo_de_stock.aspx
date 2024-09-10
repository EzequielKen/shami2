<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="conteo_de_stock.aspx.cs" Inherits="paginaWeb.paginasFabrica.conteo_de_stock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <div class="input-group">
                        <asp:TextBox ID="textbox_buscar" placeholder="Buscar..." CssClass="form-control input-group" OnTextChanged="textbox_buscar_TextChanged" runat="server" AutoPostBack="true" />
                        <asp:DropDownList ID="dropDown_tipo" CssClass="form-control input-group" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:Button ID="boton_cargar_conteo" CssClass="btn btn-primary" Text="Cargar Conteo" OnClick="boton_cargar_conteo_Click" runat="server" />
                    </div>
                    <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="producto" DataField="producto" />

                            <asp:TemplateField HeaderText="Ingrese Stock">
                                <ItemTemplate>
                                    <asp:TextBox ID="texbox_stock" placeholder="Stock" CssClass="input-group-text" OnTextChanged="texbox_stock_TextChanged" runat="server" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="unidad de medida" DataField="unidad_de_medida_local" />

                            <asp:TemplateField HeaderText="Cargar">
                                <ItemTemplate>
                                    <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_cargar_Click" runat="server" />
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Stock" DataField="conteo_stock" />

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
