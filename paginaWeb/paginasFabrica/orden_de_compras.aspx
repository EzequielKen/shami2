<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="orden_de_compras.aspx.cs" Inherits="paginaWeb.paginasFabrica.orden_de_compras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        
        <h2>Crear Orden de Compra</h2>
        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                <div class="alert alert-light">
                    <div class="input-group">
                        <asp:TextBox CssClass=" form-control" ID="textbox_buscar_producto" OnTextChanged="textbox_buscar_producto_TextChanged" AutoPostBack="true" runat="server" />
                        <asp:Button CssClass=" form-control" Text="Buscar" runat="server" />
                    </div>
                </div>


            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                <div class=" alert alert-light">
                    <div class="input-group">
                        <asp:TextBox CssClass=" form-control" ID="textbox_buscar" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" runat="server" />
                        <asp:Button CssClass=" form-control" Text="Buscar" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center"></div>
        </div>

        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                <div class="input-group">
                    <asp:GridView runat="server" ID="gridView_producto" AutoGenerateColumns="false" CssClass="table table-striped" OnSelectedIndexChanged="gridView_producto_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Producto" DataField="producto" />
                            <asp:CommandField ShowSelectButton="true" SelectText="Seleccionar" HeaderText="Seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">


                <asp:GridView runat="server" ID="gridView_proveedores" AutoGenerateColumns="false" CssClass="table table-striped" OnSelectedIndexChanged="gridView_proveedores_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="Proveedor" DataField="proveedor" />
                        <asp:BoundField HeaderText="Direccion" DataField="direccion" />
                        <asp:CommandField ShowSelectButton="true" SelectText="abrir" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                    </Columns>
                </asp:GridView>

            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                <asp:GridView ID="gridView_resumen" runat="server" Caption="RESUMEN DE ORDEN DE PEDIDO" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                        <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
