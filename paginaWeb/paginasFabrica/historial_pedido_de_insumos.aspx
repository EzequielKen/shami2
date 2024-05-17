<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_pedido_de_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_pedido_de_insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="container">

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="row">
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
                        <div class="alert alert-light">
                            <asp:Button ID="boton_crear_oc" Text="Crear OC" CssClass=" btn btn-primary" OnClick="boton_crear_oc_Click" runat="server" />
                            <asp:GridView ID="gridView_resumen_orden_pedido" runat="server" Caption="PRODUCTOS SELECCIONADOS" CaptionAlign="Top" OnRowCommand="gridView_resumen_orden_pedido_RowCommand" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida" />
                                    <asp:ButtonField HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="eliminar" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>

            </div>

            <div class="container">
                <hr />
            </div>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <asp:GridView ID="gridView_pedidos" runat="server" Caption="PEDIDOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnRowCommand="gridView_pedidos_RowCommand" OnRowDataBound="gridView_pedidos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Solicita" DataField="solicita" />
                                        <asp:BoundField HeaderText="Estado" DataField="estado" />
                                        <asp:BoundField HeaderText="Fecha de Pedido " DataField="fecha" />
                                        <asp:ButtonField HeaderText="Abrir Pedido" Text="Abrir" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="abrir_pedido" />
                                        <asp:ButtonField HeaderText="Cargar Entrega" Text="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_entrega" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <asp:Button ID="boton_PDF" OnClick="boton_PDF_Click" Text="PDF" CssClass="btn btn-primary" Visible="false" runat="server" />
                            <asp:GridView ID="gridView_resumen" runat="server" Caption="RESUMEN" CaptionAlign="Top" OnRowCommand="gridView_resumen_RowCommand" OnRowDataBound="gridView_resumen_RowDataBound" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida" />
                                    <asp:BoundField HeaderText="Cantidad Entregada" DataField="cantidad_entregada" />
                                </Columns>
                            </asp:GridView>
                        </div>


                    </div>



                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
