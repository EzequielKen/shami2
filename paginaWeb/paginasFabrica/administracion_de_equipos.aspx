<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="administracion_de_equipos.aspx.cs" Inherits="paginaWeb.paginasFabrica.administracion_de_equipos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">

                    <div class="col">
                        <div class="alert alert-light">
                            <h3>Administrador de Ubicacion de Equipo.</h3>
                            <div class="input-group">
                                <asp:TextBox ID="textbox_ubicacion" placeholder="Ingrese nueva ubicacion" CssClass="form-control" runat="server" />
                                <asp:Button ID="boton_ubicacion" CssClass="form-control btn btn-primary" Text="Cargar" OnClick="boton_ubicacion_Click" runat="server" />
                            </div>
                            <asp:GridView Caption="LISTA DE UBICACIONES" CaptionAlign="Top" runat="server" ID="gridview_ubicacion" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Ubicacion" DataField="ubicacion" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col">
                        <div class="alert alert-light">
                            <h3>Seleccione Tipo de Equipo:</h3>
                            <div class="row">
                                <div class="col">
                                    <asp:Button CssClass="btn btn-primary" Text="Congelacion" runat="server" ID="boton_congelacion" OnClick="boton_congelacion_Click" />
                                </div>
                                <div class="col">
                                    <asp:Button CssClass="btn btn-warning" Text="Refrigeracion" runat="server" ID="boton_refrigeracion" OnClick="boton_refrigeracion_Click" />
                                </div>
                                <div class="col">
                                    <asp:Button CssClass="btn btn-danger" Text="Coccion" runat="server" ID="boton_coccion" OnClick="boton_coccion_Click" />
                                </div>
                            </div>
                            <h3>
                                <asp:Label ID="label_tipo_seleccionado" Text="Tipo Seleccionado: N/A" runat="server" />
                            </h3>
                            <h3>Temperaturas Optimas.</h3>
                            <div class="input-group">
                                <asp:TextBox ID="textbox_temperatura" placeholder="Ejemplo: 0 °C A -18 °C" CssClass="form-control" OnTextChanged="textbox_temperatura_TextChanged" AutoPostBack="true" runat="server" />
                            </div>
                            <h3>Nombre del Equipo.</h3>
                            <div class="input-group">
                                <asp:TextBox ID="textbox_equipo" placeholder="Ingrese el nombre del equipo." CssClass="form-control" runat="server" />
                            </div>
                            <h3>Seleccione ubicacion del equipo:</h3>
                            <asp:DropDownList ID="dropdown_ubicaciones" CssClass="btn btn-dark dropdown-toggle" runat="server">
                            </asp:DropDownList>

                            <h3>Observaciones.</h3>
                            <div class="input-group">
                                <asp:TextBox ID="textbox_observacion" placeholder="Ingrese Observacion" CssClass="form-control" runat="server" />
                            </div>
                                <asp:Button ID="boton_equipo" OnClick="boton_equipo_Click" CssClass="form-control btn btn-primary" Text="Cargar" runat="server" />

                        </div>
                        <div id="div_equipos" class="alert alert-light">

                            <asp:GridView Caption="LISTA DE EQUIPOS" CaptionAlign="Top" runat="server" ID="gridview_equipos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_equipos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:TemplateField HeaderText="Nombre del Equipo">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nombre_ubicacion" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_nombre_ubicacion_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Categoria">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_Categoria_equipo" CssClass="form-control-lg" runat="server" OnSelectedIndexChanged="dropdown_Categoria_equipo_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                                <asp:ListItem Text="Congelacion" />
                                                <asp:ListItem Text="Refrigeracion" />
                                                <asp:ListItem Text="Coccion" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ubicacion">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_ubicacion_equipo" CssClass="form-control-lg" runat="server" OnSelectedIndexChanged="dropdown_ubicacion_equipo_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Temperatura">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_temperatura_equipo" CssClass="form-control-lg" OnTextChanged="textbox_temperatura_equipo_TextChanged" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Observaciones">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_observacion_equipo" CssClass="form-control-lg" OnTextChanged="textbox_observacion_equipo_TextChanged" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col"></div>
                </div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
