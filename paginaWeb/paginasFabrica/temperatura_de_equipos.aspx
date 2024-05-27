<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="temperatura_de_equipos.aspx.cs" Inherits="paginaWeb.paginasFabrica.temperatura_de_equipos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <asp:Button ID="boton_administrar" CssClass="btn btn-primary" Text="Administrar" runat="server" OnClick="boton_administrar_Click" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-1"></div>
                    <div class="col-10">

                        <div id="div_equipos" class="alert alert-light">

                            <div class="input-group">
                            <asp:DropDownList ID="dropdown_ubicaciones" CssClass=" form-control dropdown" OnSelectedIndexChanged="dropdown_ubicaciones_SelectedIndexChanged" AutoPostBack="true" runat="server">
                            </asp:DropDownList>
                            <asp:TextBox ID="textbox_nombre" placeholder="Ingrese su nombre" CssClass="form-control" runat="server" />
                            </div>
                            
                            <asp:GridView Caption="LISTA DE EQUIPOS" CaptionAlign="Top" runat="server" ID="gridview_equipos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_equipos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="ubicacion" DataField="ubicacion" />
                                    <asp:BoundField HeaderText="nombre" DataField="nombre" />
                                    <asp:BoundField HeaderText="categoria" DataField="categoria" />
                                    <asp:BoundField HeaderText="observaciones" DataField="observaciones" />
                                    <asp:BoundField HeaderText="temperatura optima" DataField="temperatura" />
                                    <asp:TemplateField HeaderText="Turno 1: 08:00Hs a 12:00Hs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_temperatura_diaria_1" placeholder="Ingrese temperatura registrada" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_temperatura_diaria_1_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Turno 2: 16:00Hs a 18:00Hs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_temperatura_diaria_2" placeholder="Ingrese temperatura registrada" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_temperatura_diaria_2_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Turno 3: 21:00Hs a 23:00Hs">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_temperatura_diaria_3" placeholder="Ingrese temperatura registrada" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_temperatura_diaria_3_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cargar">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_cargar_Click" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-1"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
