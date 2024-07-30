<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="asignador_de_cargos_cajero.aspx.cs" Inherits="paginaWeb.paginas.administrador_de_empleados_cajero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <h1>Asignador de Posiciones</h1>
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
                                    <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                                    <asp:BoundField HeaderText="Apellido" DataField="apellido" />
                                    <asp:BoundField HeaderText="Telefono" DataField="telefono" />

                                    <asp:BoundField HeaderText="Encargado" DataField="Encargado" />
                                    <asp:BoundField HeaderText="Cajero" DataField="Cajero" />

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

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
