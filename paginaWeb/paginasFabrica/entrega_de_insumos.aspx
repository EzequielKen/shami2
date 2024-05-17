<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="entrega_de_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.entrega_de_insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <!-- Modal -->
    <div class="modal fade" id="spinnerModal" tabindex="-1" aria-labelledby="spinnerModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="spinnerModalLabel">Guardando registros... espere...</h5>
                </div>
                <div class="modal-body text-center">
                    <div class="spinner-border" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-2 col-lg-2 col-xl-2">
                        <asp:GridView Caption="LISTA DE INSUMOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_insumos_SelectedIndexChanged" runat="server" ID="gridview_insumos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Pedido" DataField="pedido" />
                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                                <asp:BoundField HeaderText="Pedido" DataField="tipo_producto" />
                                </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-10 col-lg-10 col-xl-10">
                        <div class="alert alert-light">
                            <div class="container">
                                <div class="row">
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                    </div>
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                                    </div>
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-lg-end">
                                        <asp:Button ID="boton_guardar" Text="Guardar" CssClass="btn btn-primary align-content-end" OnClick="boton_guardar_Click" runat="server" data-bs-toggle="modal" data-bs-target="#spinnerModal" />
                                    </div>
                                </div>
                                <asp:GridView Caption="LISTA INSUMOS DEL PROVEEDOR" CaptionAlign="Top" runat="server" ID="gridview_insumos_del_proveedor" OnRowCommand="gridview_insumos_del_proveedor_RowCommand" OnRowDataBound="gridview_insumos_del_proveedor_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Insumo" DataField="Producto" />
                                        <asp:TemplateField HeaderText="Ingrese cantidad a entregar">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_cantidad" CssClass="input-group-text" runat="server" AutoPostBack="true">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Seleccione tipo presentacion">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="dropdown_tipo_presentacion" CssClass="input-group-text" runat="server" OnSelectedIndexChanged="dropdown_tipo_presentacion_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Cantidad a entregar" DataField="entrega" />
                                        <asp:ButtonField HeaderText="Confirmar" Text="Aplicar cambios" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_aplicar_cambios" />
                                        <asp:ButtonField HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_eliminar" />
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
