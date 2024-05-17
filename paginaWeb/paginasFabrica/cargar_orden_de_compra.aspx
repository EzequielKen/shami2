<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="cargar_orden_de_compra.aspx.cs" Inherits="paginaWeb.paginasFabrica.cargar_orden_de_compra" %>

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
                <h2>Recibir/Modificar Orden de Compra</h2>

                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <h2>
                                <asp:Label ID="label_num_orden_de_compra" Text="text" runat="server" />
                            </h2>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <h2>
                                <asp:Label ID="label_proveedor_de_fabrica_seleeccionado" Text="text" runat="server" />
                            </h2>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="row alert alert-light">
                            <div class="col">
                                <asp:Button Text="Entrega Parcial" ID="boton_parcial" runat="server" CssClass="btn btn-primary btn-sm " OnClick="boton_parcial_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal" />

                            </div>
                            <div class="col text-end">
                                <asp:Button Text="Entrega Total" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm btn-warning" OnClick="boton_enviar_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal" />

                            </div>
                        </div>
                    </div>
                </div>

            </div>
            </div>


            <div class=" container">
                <hr />
            </div>


            <div class=" container-fluid">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">


                        <div class="container">
                            <div class="alert alert-light">
                                <div class="row">
                                    <div class="col">
                                        <div class="input-group mb-3 ">
                                            <asp:TextBox ID="textbox_buscar" runat="server" CssClass="form-control" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" />
                                            <asp:Button Text="Buscar" runat="server" CssClass="form-control btn btn-primary" />
                                            <asp:Label ID="label_total" Text="Total: $999" CssClass="form-control" runat="server" />
                                        </div>
                                        <asp:Label ID="label_total_con_impuestos" Text="Total con impuestos: N/A" Visible="false" CssClass="form-control" runat="server" />

                                    </div>

                                    <div class="col">
                                        <div class="d-flex">
                                            <div class="form-group mr-1">
                                                <asp:TextBox ID="textbox_dia" runat="server" CssClass="form-control" TextMode="Number" placeholder="Día" />
                                            </div>
                                            <div class="form-group mr-1">
                                                <span class="form-control">/</span>
                                            </div>
                                            <div class="form-group mr-1">
                                                <asp:TextBox ID="textbox_mes" runat="server" CssClass="form-control" TextMode="Number" placeholder="Mes" />
                                            </div>
                                            <div class="form-group mr-1">
                                                <span class="form-control">/</span>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox ID="textbox_año" runat="server" CssClass="form-control" TextMode="Number" placeholder="Año" />
                                            </div>
                                        </div>

                                        <div class="input-group mb-3">
                                            <asp:TextBox ID="textbox_impuestos" runat="server" CssClass="form-control" />
                                            <asp:Button ID="boton_cargar_impuestos" Text="Cargar impuestos" runat="server" CssClass="form-control btn btn-primary" OnClick="boton_cargar_impuestos_Click" />
                                        </div>
                                        <asp:Label ID="label_total_impuesto" Text="Impuestos varios: N/A" CssClass="form-control" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

            <asp:GridView Caption="LISTA DE INSUMOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_pedido_SelectedIndexChanged" OnDataBound="gridview_pedido_DataBound" runat="server" ID="gridview_pedido" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                <Columns>
                    <asp:BoundField HeaderText="id" DataField="id" />
                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                    <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida" />
                    <asp:TemplateField HeaderText="Ingrese Cantidad Recibida">
                        <ItemTemplate>
                            <asp:TextBox ID="texbox_cantidad" CssClass="input-group-text" runat="server" OnTextChanged="texbox_cantidad_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="unidad de medida" DataField="unidad_pedida" />
                    <asp:BoundField HeaderText="Cantidad Recibida" DataField="cantidad_entrega" />
                    <asp:BoundField HeaderText="Cantidad Recibida Total" DataField="total_entrega" />
                    <asp:BoundField HeaderText="Precio" DataField="precio" />
                    <asp:TemplateField HeaderText="Ingrese Nuevo Precio">
                        <ItemTemplate>
                            <asp:TextBox ID="texbox_nuevo_precio" CssClass="input-group-text" runat="server" OnTextChanged="texbox_nuevo_precio_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nuevo Precio" DataField="nuevo_precio" />
                    <asp:BoundField HeaderText="Sub Total" DataField="sub_total" />
                    <asp:BoundField HeaderText="stock" DataField="stock" />
                    <asp:BoundField HeaderText="stock nuevo" DataField="nuevo_stock" />

                </Columns>
            </asp:GridView>

            </div>
            </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
