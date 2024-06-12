<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="administrar_actividades_lista_chequeo_locales.aspx.cs" Inherits="paginaWeb.paginasFabrica.administrar_lista_chequeo_locales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <style>
                .textbox_personalizado {
                    /* Tus estilos personalizados aquí */
                    width: auto;
                    font-family: Arial, Helvetica, sans-serif;
                    font-size: 0.9em;
                    border: 1px solid #a1d19d;
                    /* Otros estilos... */
                }
            </style>

            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">

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

                        <div class="alert alert-light">
                            <h3>Crear Actividad</h3>
                            <div class="input-group">

                                <h4>Actividad
                                </h4>
                                <asp:TextBox ID="textbox_actividad_nueva" placeholder="Ingrese actividad" CssClass="form-control" runat="server" />
                                <h4>Categoria
                                </h4>
                                <asp:TextBox ID="textbox_categoria_nueva" placeholder="Ingrese Categoria Nueva" CssClass="form-control" runat="server" />

                                <asp:DropDownList ID="dropdown_categoria_nueva" CssClass=" form-control" runat="server">
                                </asp:DropDownList>
                                <asp:Button ID="boton_crear_actividad" CssClass="btn btn-primary" Text="Cargar" OnClick="boton_crear_actividad_Click" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark  table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:TemplateField HeaderText="Orden">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_orden" CssClass="form-control form-control-sm" runat="server" OnTextChanged="textbox_orden_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actividad">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_actividad" CssClass="form-control" runat="server" OnTextChanged="textbox_actividad_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Habilitar/Deshabilitar">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_eliminar" Text="Habilitar/Deshabilitar" CssClass="btn btn-danger" runat="server" OnClick="boton_eliminar_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Activa" DataField="activa" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
