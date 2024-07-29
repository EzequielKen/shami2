<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="devolucion_de_productos.aspx.cs" Inherits="paginaWeb.paginasFabrica.devolucion_de_productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <div class="row">

                                    <div class="col">
                                        <asp:Label Text="Tipo Producto" CssClass=" form-label" runat="server" />
                                        <asp:DropDownList ID="dropDown_tipo" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Buscar Producto" CssClass=" form-label" runat="server" />
                                        <div class=" input-group">
                                            <asp:TextBox ID="textbox_busqueda" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="textbox_busqueda_TextChanged" />
                                            <asp:Button Text="Buscar" runat="server" CssClass="form-control btn btn-primary" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Cargar devolucion a stock" CssClass=" form-label" runat="server" />
                                        <div class=" input-group">
                                            <asp:Button ID="boton_enviar" Text="Enviar a" CssClass="btn btn-primary" OnClick="boton_enviar_Click" runat="server" />
                                            <asp:DropDownList ID="DropDownList_enviar_a" runat="server" CssClass="form-select" OnSelectedIndexChanged="DropDownList_enviar_a_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Text="Stock" />
                                                <asp:ListItem Text="Desperdicio" />
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                </div>
                                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" ID="gridView_productos" OnRowCommand="gridView_productos_RowCommand" OnRowDataBound="gridView_productos_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="ID" DataField="id" />
                                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                                        <asp:TemplateField HeaderText="Ingrese Cantidad Devuelta">
                                            <ItemTemplate>
                                                <asp:TextBox ID="texbox_cantidad_stock" CssClass="input-group-text" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Cantidad Devuelta" DataField="cantidad_devuelta" />
                                        <asp:BoundField HeaderText="Stock" DataField="stock" />
                                        <asp:BoundField HeaderText="Nuevo Stock" DataField="nuevo_stock" />
                                        <asp:BoundField HeaderText="Unidad Medida" DataField="unidad_de_medida_fabrica" />
                                        <asp:ButtonField HeaderText="Cargar" Text="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_en_stock" />

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
