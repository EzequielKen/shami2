<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="pedidos.aspx.cs" Inherits="paginaWeb.paginasFabrica.pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">

        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                <h1>
                    <asp:Label ID="label_nombre_sucursal" Text="text" runat="server" />
                </h1>
                <asp:GridView OnRowCommand="gridView_pedidos_RowCommand" runat="server" ID="gridView_pedidos" AutoGenerateColumns="false" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField HeaderText="cliente" DataField="sucursal" />
                        <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                        <asp:BoundField HeaderText="fecha pedido" DataField="fecha" />
                        <asp:ButtonField HeaderText="abrir" Text="abrir" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_abrir" />
                        <asp:ButtonField Visible="false" HeaderText="seleccionar" Text="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_seleccionar" />
                        <asp:BoundField HeaderText="Nota" DataField="nota" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                <h1>
                    <asp:Label ID="label_titulo_resumen" Text="Resumen" runat="server" />
                </h1>
                <asp:GridView OnRowCommand="gridView_resumen_RowCommand" runat="server" ID="gridView_resumen" AutoGenerateColumns="false" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="cliente" DataField="sucursal" />
                        <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                        <asp:BoundField HeaderText="fecha pedido" DataField="fecha" />
                        <asp:ButtonField HeaderText="eliminar" Text="eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_eliminar" />
                    </Columns>
                </asp:GridView>
                <asp:Button CssClass="btn btn-primary" Text="abrir" ID="boton_abrir" OnClick="boton_abrir_Click" runat="server" />
            </div>
        </div>
    </div>


</asp:Content>
