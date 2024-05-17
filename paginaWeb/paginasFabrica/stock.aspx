<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="stock.aspx.cs" Inherits="paginaWeb.paginasFabrica.stock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class=" row  ">
            <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                <asp:Label Text="" runat="server" CssClass="form-control " ID="label_productoSelecionado" />
                <div class="input-group">
                    <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                    <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                    <div>
                        <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="producto" DataField="producto" />
                        <asp:BoundField HeaderText="unidad de medida" DataField="unidad_de_medida_local" />

                        <asp:BoundField HeaderText="Stock Produccion Teorico" DataField="stock_produccion" />
                        <asp:BoundField HeaderText="Stock Expedicion Teorico" DataField="stock_expedicion" />
                        <asp:BoundField HeaderText="Stock Expedicion Real" DataField="stock" />
                        <asp:BoundField HeaderText="Diferencia" DataField="diferencia" />
                        <asp:BoundField HeaderText="Promedio pedido" DataField="promedio_pedido" />

                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>
