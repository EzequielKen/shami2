<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="administrador_de_empleados.aspx.cs" Inherits="paginaWeb.paginas.administrador_de_empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div>
                                <h4>
                                    <asp:Label CssClass=" form-label" Text="Nombre" runat="server" />
                                </h4>
                                <asp:TextBox placeholder="Ingrese nombre" ID="textbox_nombre" CssClass="form-control" runat="server" />
                                <h4>
                                    <asp:Label CssClass="form-label" Text="Apellido" runat="server" />
                                </h4>
                                <asp:TextBox placeholder="Ingrese apellido" ID="textbox_apellido" CssClass="form-control" runat="server" />
                            </div>
                            <div>
                                <h4>
                                    <asp:Label CssClass="form-label" Text="DNI" runat="server" />
                                </h4>
                                <asp:TextBox placeholder="Ingrese DNI" ID="textbox_dni" CssClass="form-control" runat="server" />
                                <h4>
                                    <asp:Label CssClass="form-label" Text="Telefono" runat="server" />
                                </h4>
                                <asp:TextBox placeholder="Ingrese telefono" ID="textbox_telefono" CssClass="form-control" runat="server" />
                            </div>
                            <div>
                                <h4>
                                    <asp:Label CssClass="form-label" Text="cargo" runat="server" />
                                </h4>
                                <div class="row">
                                    <div class="col">
                                        <asp:Button ID="boton_encargado" CssClass="btn btn-primary" Text="Encargado" runat="server" OnClick="boton_encargado_Click" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="boton_cajero" CssClass="btn btn-primary" Text="Cajero" runat="server" OnClick="boton_cajero_Click" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="boton_shawarmero" CssClass="btn btn-primary" Text="Shawarmero" runat="server" OnClick="boton_shawarmero_Click" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="boton_atencion" CssClass="btn btn-primary" Text="Atencion al Cliente" runat="server" OnClick="boton_atencion_Click" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="boton_cocina" CssClass="btn btn-primary" Text="Cocina" runat="server" OnClick="boton_cocina_Click" />
                                    </div>
                                    <div class="col">
                                        <asp:Button ID="boton_limpieza" CssClass="btn btn-primary" Text="Limpieza" runat="server" OnClick="boton_limpieza_Click" />
                                    </div>
                                </div>

                            </div>
                            <div class="row pt-5">
                                <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Cargar" runat="server" OnClick="boton_cargar_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col">
                        <h2>
                            <asp:Label Text="Administrador de empleados." runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_encargado" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_cajeros" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_shawarmero" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_atencion_al_cliente" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_cocina" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label Visible="false" ID="label_total_limpieza" Text="text" runat="server" />
                        </h2>
                    </div>
                    <h4 class="pt-4">
                        <asp:Label Visible="false" ID="label_advertencia" CssClass=" pt-4 alert alert-danger" Text="text" runat="server" />
                    </h4>
                </div>
            </div>

            <hr />

            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="cargo" runat="server" />
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <span class="badge rounded-pill text-bg-primary">Azul = No Seleccionado</span>
                            <span class="badge rounded-pill text-bg-success">Verde = Seleccionado</span>
                            <h2>
                                <asp:Label ID="label_total" Text="text" runat="server" />
                            </h2>
                            <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:TemplateField HeaderText="Nombre">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nombre_empleado" CssClass=" form-control-sm" runat="server" OnTextChanged="textbox_nombre_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Apellido">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_apellido_empleado" CssClass="form-control-sm" runat="server" OnTextChanged="textbox_apellido_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="DNI">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_dni_empleado" CssClass="form-control-sm" runat="server" OnTextChanged="textbox_dni_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Telefono">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_telefono_empleado" CssClass="form-control-sm" runat="server" OnTextChanged="textbox_telefono_empleado_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Encargado">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_encargado_empleado" CssClass="btn btn-primary" Text="Encargado" runat="server" OnClick="boton_encargado_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cajero">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cajero_empleado" CssClass="btn btn-primary" Text="Cajero" runat="server" OnClick="boton_cajero_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Shawarmero">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_shawarmero_empleado" CssClass="btn btn-primary" Text="Shawarmero" runat="server" OnClick="boton_shawarmero_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Atencion a cliente">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_atencion_empleado" CssClass="btn btn-primary" Text="Atencion a Cliente" runat="server" OnClick="boton_atencion_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cocina">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cocina_empleado" CssClass="btn btn-primary" Text="Cocina" runat="server" OnClick="boton_cocina_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Limpieza">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_limpieza_empleado" CssClass="btn btn-primary" Text="Limpieza" runat="server" OnClick="boton_limpieza_empleado_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_eliminar" CssClass="btn btn-danger" Text="Eliminar" runat="server" OnClick="boton_eliminar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
