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
                    <div class="col-2"></div>
                    <div class="col-8">

                        <div id="div_equipos" class="alert alert-light">
                            <asp:DropDownList ID="dropdown_ubicaciones" CssClass="dropdown" OnSelectedIndexChanged="dropdown_ubicaciones_SelectedIndexChanged" AutoPostBack="true" runat="server">
                            </asp:DropDownList>
                            <asp:GridView Caption="LISTA DE EQUIPOS" CaptionAlign="Top" runat="server" ID="gridview_equipos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_equipos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="ubicacion" DataField="ubicacion" />
                                    <asp:BoundField HeaderText="nombre" DataField="nombre" />
                                    <asp:BoundField HeaderText="categoria" DataField="categoria" />
                                    <asp:BoundField HeaderText="observaciones" DataField="observaciones" />
                                    <asp:BoundField HeaderText="temperatura optima" DataField="temperatura" />
                                    <asp:TemplateField HeaderText="Temperatura diaria">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_temperatura_diaria" placeholder="Ingrese temperatura registrada" CssClass="form-control-lg" runat="server" OnTextChanged="textbox_temperatura_diaria_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
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
                    <div class="col-2"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
