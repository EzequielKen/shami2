<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="lista_de_faltantes.aspx.cs" Inherits="paginaWeb.paginas.lista_de_faltantes" %>

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
                            <label>Tipo Producto</label>
                            <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged1" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_productos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />

                                    <asp:TemplateField HeaderText="Marcar Faltante">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_faltante" CssClass="btn btn-primary" Text="Marcar Como Faltante" runat="server" OnClick="boton_faltante_Click" />
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
