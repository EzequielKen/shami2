<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="crear_clientes_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.crear_clientes_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <script type="text/javascript">
        function mostrarModal() {
            alert("Cliente Creado Exitosamente.");
            $('#ticketModal').modal('show');
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">

                    <div class="alert alert-light">
                        <div class="input-group">
                            <label class="align-content-center">Sucursal</label>
                            <asp:TextBox ID="textbox_sucursal_nuevo" placeholder="Ingrese Nombre..." CssClass="form-control" runat="server" />
                            <label class="align-content-center">Franquicia</label>
                            <asp:DropDownList ID="dropdown_franquicia_nuevo" CssClass="form-select" runat="server">
                            </asp:DropDownList>
                            <asp:TextBox ID="textbox_nueva_franquicia" placeholder="Nueva franquicia..." CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <div class="alert alert-light">
                        <div class="input-group">
                            <label class="align-content-center">Provincia</label>
                            <asp:TextBox ID="textbox_provincia_nuevo" placeholder="Ingrese Provincia..." CssClass="form-control" runat="server" />
                            <label class="align-content-center">Localidad</label>
                            <asp:TextBox ID="textbox_localidad_nuevo" placeholder="Ingrese Localidad..." CssClass="form-control" runat="server" />
                            <label class="align-content-center">Direccion</label>
                            <asp:TextBox ID="textbox_direccion_nuevo" placeholder="Ingrese Direccion..." CssClass="form-control" runat="server" />
                            <label class="align-content-center">Telefono</label>
                            <asp:TextBox ID="textbox_telefono_nuevo" placeholder="Ej: +54 9 11 1234-5678" CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <asp:Button ID="boton_crear_cliente" CssClass="btn btn-primary" Text="Crear Clientes" OnClick="boton_crear_cliente_Click" runat="server" />
                </div>
                <hr />
                <div class="row">

                    <h1>Editar Clientes</h1>

                    <div class="alert alert-light">
                        <div class="input-group">
                            <label class="col-form-label">Buscar:</label>
                            <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="Buscar..." runat="server" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" />
                            <label class="col-form-label">Franquicia:</label>
                            <asp:DropDownList ID="dropDown_tipo" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="alert alert-light">
                        <asp:GridView Caption="LISTA DE CLIENTES" CaptionAlign="Top" runat="server" ID="gridview_clientes" OnRowDataBound="gridview_clientes_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>

                                <asp:BoundField HeaderText="id" DataField="id" />

                                <asp:TemplateField HeaderText="Activo">
                                    <ItemTemplate>
                                        <label>activo</label>
                                        <asp:DropDownList ID="dropdown_activo" CssClass="form-select" OnSelectedIndexChanged="dropdown_activo_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Text="Si" />
                                            <asp:ListItem Text="No" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />

                                <asp:TemplateField HeaderText="Provincia">
                                    <ItemTemplate>
                                        <label>Provincia</label>
                                        <asp:TextBox ID="textbox_pronvincia" CssClass="form-control" OnTextChanged="textbox_pronvincia_TextChanged" AutoPostBack="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Localidad">
                                    <ItemTemplate>
                                        <label>Localidad</label>
                                        <asp:TextBox ID="textbox_localidad" CssClass="form-control" OnTextChanged="textbox_localidad_TextChanged" AutoPostBack="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Direccion">
                                    <ItemTemplate>
                                        <label>Direccion</label>
                                        <asp:TextBox ID="textbox_direccion" CssClass="form-control" OnTextChanged="textbox_direccion_TextChanged" AutoPostBack="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Telefono">
                                    <ItemTemplate>
                                        <label>Telefono</label>
                                        <asp:TextBox ID="textbox_telefono" CssClass="form-control" OnTextChanged="textbox_telefono_TextChanged" AutoPostBack="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Franquicia">
                                    <ItemTemplate>
                                        <label>Franquicia</label>
                                        <asp:DropDownList ID="dropdown_franquicia" CssClass="form-select" OnSelectedIndexChanged="dropdown_franquicia_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Administrar Usuarios">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_administrar_usuarios" CssClass="btn btn-primary" Text="Administrar usuarios" OnClick="boton_administrar_usuarios_Click" runat="server" />
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
