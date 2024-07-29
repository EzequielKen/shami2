<%@ Page  Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="stock_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.stock_insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
           
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
            <div class="container">
                <div class=" row  ">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                            <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                            <div>
                                <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6 text-lg-end">
                        <asp:Button ID="boton_guardar_registro" OnClick="boton_guardar_registro_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal" Text="Guardar Registro" CssClass="btn btn-primary " runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <asp:GridView Caption="LISTA DE INSUMOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Insumo" DataField="producto" />
                                <asp:BoundField HeaderText="Stock" DataField="stock" />
                                <asp:BoundField HeaderText="Unidad de Medida" DataField="unidad_medida" />

                                <asp:CommandField ShowSelectButton="true" SelectText="Ver Detalle" HeaderText="Detalle" ControlStyle-CssClass="btn btn-primary btn-sm" />

                            </Columns>
                        </asp:GridView>

                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <asp:GridView Caption="LISTA DE PRESENTACIONES" CaptionAlign="Top" runat="server" ID="gridview_presentaciones" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Stock Actual" DataField="stock" />
                                <asp:TemplateField HeaderText="Ingrese Stock Nuevo">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_stock_nuevo" OnTextChanged="textbox_stock_nuevo_TextChanged1" AutoPostBack="true" CssClass="input-group-text" runat="server">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Stock nuevo" DataField="nuevo_stock" />
                                <asp:BoundField HeaderText="Nombre Columna en Base de Datos" DataField="nombre_columna" />
                                <asp:BoundField HeaderText="presentacionBD" DataField="presentacionBD" />
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="boton_confirmar_stock" Visible="false" Text="Confirmar Stock" OnClick="boton_confirmar_stock_Click" CssClass="btn btn-primary" runat="server" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
