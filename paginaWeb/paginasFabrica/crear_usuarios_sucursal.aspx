<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_usuarios_sucursal.aspx.cs" Inherits="paginaWeb.paginasFabrica.administrar_usuarios_sucursal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <script type="text/javascript">
        function mostrarModal() {
            alert("Usuario Creado Exitosamente.");
            $('#ticketModal').modal('show');
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="alert alert-light">
                        <div class="input-group">
                            <label class="align-content-center">Usuario</label>
                            <asp:TextBox ID="textbox_usuario_nuevo" placeholder="Ingrese Usuario..." CssClass="form-control" runat="server" />
                            <label class="align-content-center">Contraseña</label>
                            <asp:TextBox ID="textbox_contraseña_nuevo" placeholder="Ingrese Contraseña..." CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <asp:Button ID="boton_crear_usuario" CssClass="btn btn-primary" Text="Crear Usuario" OnClick="boton_crear_usuario_Click" runat="server" />

                </div>
                <hr />
                <div class="row">

                    <h1>Editar Usuarios</h1>

                    <div class="alert alert-light">
                        <div class="input-group">
                            <label class="col-form-label">Buscar:</label>
                            <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="Buscar..." runat="server" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" />
                        </div>
                    </div>

                    <div class="alert alert-light">
                        <asp:GridView Caption="LISTA DE USUARIOS" CaptionAlign="Top" runat="server" ID="gridview_usuarios" OnRowDataBound="gridview_usuarios_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>

                                <asp:BoundField HeaderText="id" DataField="id" />

                                <asp:TemplateField HeaderText="Activo">
                                    <ItemTemplate>
                                        <label>activo</label>
                                        <asp:DropDownList ID="dropdown_activo" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="dropdown_activo_SelectedIndexChanged" runat="server">
                                            <asp:ListItem Text="Si" />
                                            <asp:ListItem Text="No" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Usuario">
                                    <ItemTemplate>
                                        <label>Usuario</label>
                                        <asp:TextBox ID="textbox_usuario" CssClass="form-control" AutoPostBack="true" OnTextChanged="textbox_usuario_TextChanged" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contraseña">
                                    <ItemTemplate>
                                        <label>Contraseña</label>
                                        <asp:TextBox ID="textbox_contraseña" CssClass="form-control" AutoPostBack="true" OnTextChanged="textbox_contraseña_TextChanged" runat="server" />
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
