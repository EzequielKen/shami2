<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="lista_de_chequeo.aspx.cs" Inherits="paginaWeb.paginas.lista_de_chequeo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container">
                <div class=" row">
                    <div class="col">
                        <h1>
                            <asp:Label ID="label_nombre" Text="text" runat="server" />
                        </h1>
                        <h2>
                            <asp:Label ID="label_cargo" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_fecha" Text="text" runat="server" />
                        </h2>

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
                            <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="actividad" DataField="actividad" />
                                    <asp:TemplateField HeaderText="Cargar">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nota" placeholder="Ingrese Nota" CssClass="form-control" runat="server" CommandArgument='<%# Container.DataItemIndex %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cargar">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" runat="server" OnClick="boton_cargar_Click" CommandArgument='<%# Container.DataItemIndex %>'/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Nota" DataField="nota" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
