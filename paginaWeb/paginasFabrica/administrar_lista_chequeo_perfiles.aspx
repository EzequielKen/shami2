<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="administrar_lista_chequeo_perfiles.aspx.cs" Inherits="paginaWeb.paginasFabrica.administrador_de_actividades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Perfil" runat="server" />
                                <asp:DropDownList ID="dropdown_perfil" OnSelectedIndexChanged="dropdown_perfil_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Encargado" />
                                    <asp:ListItem Text="Cajero" />
                                    <asp:ListItem Text="Shawarmero" />
                                    <asp:ListItem Text="Atencion al Cliente" />
                                    <asp:ListItem Text="Cocina" />
                                    <asp:ListItem Text="Limpieza" />
                                </asp:DropDownList>
                            </div>
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Area" runat="server" />
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Categoria" runat="server" />
                                <asp:DropDownList ID="dropDown_categoria" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_categoria_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:Button ID="boton_guardar" Text="Guardar" CssClass="btn btn-primary" runat="server" OnClick="boton_guardar_Click" />
                        <asp:Button ID="boton_cargar_todo" Text="Cargar Todo" CssClass="btn btn-primary" runat="server" OnClick="boton_cargar_todo_Click" Style="float: right;" />
                        <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark  table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="actividad" DataField="actividad" />
                                <asp:TemplateField HeaderText="Cargar">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" runat="server" OnClick="boton_cargar_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Puntuacion">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_puntuacion" CssClass="form-control" runat="server" OnTextChanged="textbox_puntuacion_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
