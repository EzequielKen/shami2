<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="panificados.aspx.cs" Inherits="paginaWeb.paginasFabrica.panificados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="row alert alert-light">
                            <asp:Button Text="enviar" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <hr />
            </div>

            <div class=" container">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">

                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnRowCommand="gridview_productos_RowCommand" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Unidad de Medida" DataField="unidad_de_medida" />
                                <asp:BoundField HeaderText="Stock Actual" DataField="stock" />
                                <asp:TemplateField HeaderText="Ingrese stock fisico">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_Stock" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Nuevo Stock" DataField="stock_nuevo" />
                                <asp:ButtonField HeaderText="Confirmar" Text="Aplicar cambios" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_aplicar_cambios" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
