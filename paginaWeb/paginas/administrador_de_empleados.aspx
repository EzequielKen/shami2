<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="administrador_de_empleados.aspx.cs" Inherits="paginaWeb.paginas.administrador_de_empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <div class="input-group">
                        <asp:Label CssClass="form-control" Text="nombre" runat="server" />
                        <asp:TextBox placeholder="Ingrese nombre" ID="textbox_nombre" CssClass="form-control" runat="server" />
                        <asp:Label CssClass="form-control" Text="apellido" runat="server" />
                        <asp:TextBox placeholder="Ingrese apellido" ID="textbox_apellido" CssClass="form-control" runat="server" />
                    </div>
                    <div class="input-group">
                        <asp:Label CssClass="form-control" Text="DNI" runat="server" />
                        <asp:TextBox placeholder="Ingrese DNI" ID="textbox_dni" CssClass="form-control" runat="server" />
                        <asp:Label CssClass="form-control" Text="Telefono" runat="server" />
                        <asp:TextBox placeholder="Ingrese telefono" ID="textbox_telefono" CssClass="form-control" runat="server" />
                    </div>
                    <div class="input-group">
                        <asp:Label CssClass="form-control" Text="cargo" runat="server" />
                        <asp:DropDownList ID="dropdown_cargo" CssClass="form-control" runat="server">
                            <asp:ListItem Text="Cajero" />
                            <asp:ListItem Text="Empleado" />
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Cargar" runat="server" OnClick="boton_cargar_Click" />
                </div>
            </div>
            <div class="col"></div>
            <h4 class="pt-4">
                <asp:Label Visible="false" ID="label_advertencia" CssClass=" pt-4 alert alert-danger" Text="text" runat="server" />
            </h4>
        </div>
        <hr />
        <div class="row">
            <div class="col"></div>
            <div class="col">
                <div class="alert alert-light">
                    <div class="input-group">
                        <asp:Label CssClass="form-control" Text="cargo" runat="server" />
                        <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark  table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_nombre_empleado" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_nombre_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Apellido">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_apellido_empleado" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_apellido_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DNI">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_dni_empleado" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_dni_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Telefono">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_telefono_empleado" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_telefono_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Cargo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="dropdown_cargo_empleado" CssClass="form-control-lg" runat="server" OnSelectedIndexChanged="dropdown_cargo_empleado_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                        <asp:ListItem Text="Cajero" />
                                        <asp:ListItem Text="Empleado" />
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col"></div>
        </div>
    </div>
</asp:Content>
