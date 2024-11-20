<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="acctualizador_precio_venta.aspx.cs" Inherits="paginaWeb.paginasFabrica.acctualizador_precio_venta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="alert alert-light">
                        <div class="input-group">
                            <label>Tipo de acuerdo</label>
                            <asp:DropDownList ID="dropdown_acuerdo" CssClass="form-select" runat="server">
                            </asp:DropDownList>
                            <label>Tipo Producto</label>
                            <asp:DropDownList ID="dropDown_tipo" CssClass="form-select" runat="server">
                            </asp:DropDownList>
                        </div>
                        <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="Buscar..." runat="server" />
                    </div>
                </div>

                <hr />

                <div class="alert-light">
                    <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="producto" DataField="producto" />
                            <asp:BoundField HeaderText="precio compra" DataField="precio_compra" />
                            <asp:BoundField HeaderText="presentacion compra" DataField="presentacion_compra" />
                            <asp:BoundField HeaderText="precio venta actual" DataField="precio_venta" />
                            <asp:BoundField HeaderText="precio venta actual" DataField="unidad_de_medida_local" />

                            <asp:TemplateField HeaderText="Ingrese Precio Venta">
                                <ItemTemplate>
                                    <asp:TextBox ID="texbox_precio" CssClass="input-group-text" runat="server" OnTextChanged="texbox_precio_TextChanged" AutoPostBack="true">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
