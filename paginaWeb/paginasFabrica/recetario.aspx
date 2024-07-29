<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="recetario.aspx.cs" Inherits="paginaWeb.paginasFabrica.recetario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-4col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                                <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnRowCommand="gridview_productos_RowCommand" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:ButtonField HeaderText="Ver Receta" Text="Ver Receta" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_ver_receta" />
                                <asp:ButtonField HeaderText="Editar Receta" Text="Editar Receta" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_editar_receta" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-8 col-lg-8 col-xl-8">
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
