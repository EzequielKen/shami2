<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="desperdicio_merma.aspx.cs" Inherits="paginaWeb.paginas.desperdicio_merma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h1>Desperdicio y merma</h1>
            <div class="container">
                <div class="row">
                    <div class="col">
                        <div class="card">
                            <div class="card-header">
                                <asp:DropDownList ID="dropdown_categoria" CssClass="form-control" OnSelectedIndexChanged="dropdown_categoria_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="Desperdicio" />
                                    <asp:ListItem Text="Merma" />
                                </asp:DropDownList>
                            </div>
                            <div class="card-body">
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:GridView ID="gridview_productos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="id" />
                                        <asp:BoundField DataField="producto" HeaderText="producto" />
                                        <asp:TemplateField HeaderText="cantidad">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_cantidad" CssClass=" form-control" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="nota">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_nota" CssClass=" form-control" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cargar">
                                            <ItemTemplate>
                                                <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Cargar" runat="server" OnClick="boton_cargar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
