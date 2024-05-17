<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="actualizador_de_precios.aspx.cs" Inherits="paginaWeb.paginasCarrefour.actualizador_de_precios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class=" alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <asp:Label Text="Buscar" runat="server" CssClass="  col-form-label" />
                                    <div class="input-group">
                                    <asp:TextBox ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" runat="server" CssClass=" form-control" AutoPostBack="true" />
                                    <asp:Button ID="Boton_buscar" Text="Buscar" CssClass="btn btn-primary" runat="server" />
                                    </div>

                                </div>
                                <div class="col">
                                </div>
                                <div class="col text-lg-end">
                                    <asp:Button ID="boton_enviar" OnClick="boton_enviar_Click" Text="Enviar" CssClass="btn btn-primary" runat="server" />
                                </div>
                            </div>

                            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" OnRowCommand="gridview_productos_RowCommand" OnRowDataBound="gridview_productos_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Precio Actual" DataField="precio" />
                                    <asp:TemplateField HeaderText="Ingrese Precio Nuevo">
                                        <ItemTemplate>
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="textbox_precio_nuevo" OnTextChanged="textbox_precio_nuevo_TextChanged" CssClass="input-group-text w-50" runat="server" AutoPostBack="true">
                                                </asp:TextBox>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Precio Nuevo" DataField="precio_nuevo" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
