<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_orden_de_compras.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_orden_de_compras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <h2>Historial de Ordenes de Compra</h2>

        <div class="row">
            <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                <div class="alert alert-light">
                    <div class="row">
                        <div class="col">
                            <asp:Label Text="Proveedor" CssClass=" form-label" runat="server" />
                            <div class=" input-group">
                                <asp:TextBox ID="textbox_busqueda" runat="server" AutoPostBack="true" CssClass="form-control" />
                                <asp:Button Text="Buscar" runat="server" CssClass="form-control btn btn-primary" />
                            </div>
                        </div>
                        <div class="col">
                            <asp:Label Text="Mes" CssClass=" form-label" runat="server" />
                            <asp:DropDownList ID="dropDown_mes" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <div class="col">
                            <asp:Label Text="Año" CssClass=" form-label" runat="server" />
                            <asp:DropDownList ID="dropDown_año" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
            </div>

        </div>

    </div>

    <div class="container">
        <hr />
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                <div class="container-fluid">
                    <div class="alert alert-light">
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="boton_solo_abiertas" CssClass="btn btn-primary" Text="Mostrar Solo OC Abiertas" runat="server" OnClick="boton_solo_abiertas_Click" />
                                <asp:Button ID="boton_solo_recibidas" CssClass="btn btn-primary" Text="Mostrar Solo OC Recibidas" runat="server" OnClick="boton_solo_recibidas_Click" />

                            </div>
                            <div class="col"></div>
                            <div class="col">
                                <asp:Label Text="Estado" CssClass=" form-label" runat="server" />
                                <asp:DropDownList ID="DropDown_estado" runat="server" CssClass="form-select" OnSelectedIndexChanged="DropDown_estado_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Todos" />
                                    <asp:ListItem Text="Abierta" />
                                    <asp:ListItem Text="Entrega Parcial" />
                                    <asp:ListItem Text="Recibido" />
                                    <asp:ListItem Text="Cancelada" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <asp:GridView ID="gridView_ordenes" runat="server" Caption="PEDIDOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnRowCommand="gridView_ordenes_RowCommand" OnDataBound="gridView_ordenes_DataBound">
                            <Columns>
                                <asp:BoundField HeaderText="N° Orden" DataField="id" />
                                <asp:BoundField HeaderText="Proveedor" DataField="proveedor" />
                                <asp:BoundField HeaderText="Fecha Pedido" DataField="fecha" />
                                <asp:BoundField HeaderText="Fecha Entrega Estimada" DataField="fecha_entrega_estimada" />
                                <asp:BoundField HeaderText="Fecha Entrega" DataField="fecha_entrega" />
                                <asp:ButtonField HeaderText="PDF" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="crear_pdf" />
                                <asp:ButtonField HeaderText="Confirmar/Editar" Text="Confirmar/Editar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="confirmar_pedido" />
                                <asp:BoundField HeaderText="Estado" DataField="estado" />
                                <asp:ButtonField HeaderText="Cancelar Pedido" Text="Cancelar/Rechazar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cancelar_pedido" />
                                <asp:BoundField HeaderText="Condicion de pago" DataField="condicion_pago" />
                                <asp:TemplateField HeaderText="Ingrese nota">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_nota" CssClass="input-group-text" runat="server" >
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField HeaderText="Cargar Notas" Text="Cargar Nota" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_nota" />
                                <asp:BoundField HeaderText="Notas" DataField="nota" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
