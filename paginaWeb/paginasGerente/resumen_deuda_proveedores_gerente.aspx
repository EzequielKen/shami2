<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="resumen_deuda_proveedores_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.resumen_deuda_proveedores_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">

                <div class="row">
                    <div class="col-sm-12 col-md-2 col-lg-2 col-xl-2 text-center">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                        <div class=" alert alert-light">
                            <div class="input-group">
                                <asp:TextBox CssClass=" form-control" ID="textbox_buscar" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" runat="server" />
                                <asp:Button CssClass=" form-control" Text="Buscar" runat="server" />
                            </div>
                            <h2>
                                <asp:Label ID="label_deuda_total" Text="" runat="server" />
                            </h2>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6 text-center">
                    </div>
                </div>

                <div class="row">

                    <div class="col-sm-12 col-md-2 col-lg-2 col-xl-2 text-center">
                        <h1>Detalle Proveedor</h1>
                        <h2>
                            <asp:Label ID="Label_nombre_proveedor" runat="server" /></h2>
                        <div class="input-group mb-3">
                            <span class="input-group-text">Telefono</span>
                            <span class="input-group-text">
                                <asp:Label ID="labe_telefono" runat="server" />
                            </span>
                        </div>
                        <div class="input-group mb-3">
                            <span class="input-group-text">Pago a</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_condicion_pago" runat="server" />
                            </span>
                            <span class="input-group-text">dias</span>
                        </div>
                        <div class="input-group mb-3">
                            <span class="input-group-text">CBU/ALIAS</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_CBU_1" runat="server" />
                            </span>
                        </div>

                        <div class="input-group mb-3">
                            <span class="input-group-text">CBU/ALIAS</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_CBU_2" runat="server" />
                            </span>
                        </div>
                        <div class="input-group mb-3">
                            <span class="input-group-text">CBU/ALIAS</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_CBU_3" runat="server" />
                            </span>
                        </div>

                        <div class="input-group mb-3">
                            <span class="input-group-text">CBU/ALIAS</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_CBU_4" runat="server" />
                            </span>
                        </div>

                        <div class="input-group mb-3">
                            <span class="input-group-text">CBU/ALIAS</span>
                            <span class="input-group-text">
                                <asp:Label ID="label_CBU_5" runat="server" />
                            </span>
                        </div>
                    </div>


                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                        <asp:GridView runat="server" ID="gridView_proveedores" AutoGenerateColumns="false" CssClass="table table-striped" OnRowDataBound="gridView_proveedores_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Proveedor" DataField="proveedor" />
                                <asp:BoundField HeaderText="Deuda" DataField="deuda" />

                                <asp:TemplateField HeaderText="Detalle">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_detalle" OnClick="boton_detalle_Click" Text="Ver Detalle" ControlStyle-CssClass="btn btn-primary btn-sm" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6 text-center">
                        <h2>
                            <asp:Label ID="label_saldo_anterior" Text="Saldo mes anterior" CssClass="badge bg-secondary" runat="server" />
                        </h2>
                        <h1>
                            <asp:Label ID="label_saldo" Text="SALDO:" CssClass="badge bg-secondary" runat="server" />
                        </h1>
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
                        <div class="alert alert-light">
                            <h2>
                                <asp:Label ID="label_compra_mes" Text="Total compra del mes:" CssClass="badge bg-secondary" runat="server" />
                            </h2>

                            <asp:GridView ID="gridView_remitos" runat="server" Caption="REMITOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped" OnRowDataBound="gridView_remitos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="N° pedido" DataField="num_orden" />
                                    <asp:BoundField HeaderText="fecha recepcion" DataField="fecha_remito" />
                                    <asp:BoundField HeaderText="fecha vencimiento" DataField="fecha_vencimiento" />
                                    <asp:BoundField HeaderText="condicion de pago" DataField="condicion_pago" />
                                    <asp:BoundField HeaderText="estado de pago" DataField="estado_pago" />
                                    <asp:BoundField HeaderText="valor" DataField="valor_orden" />

                                    <asp:TemplateField HeaderText="PDF">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_PDF" OnClick="boton_PDF_Click" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm btn-success" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Marcar como pagado">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_pagar" OnClick="boton_pagar_Click" Text="Pagado" ControlStyle-CssClass="btn btn-primary btn-sm btn-success" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="estado entrega" DataField="estado_entrega" />
                                </Columns>
                            </asp:GridView>

                            <div class="alert alert-light">
                                <h2>
                                    <asp:Label ID="label_pagado_mes" Text="Total pagado del mes:" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                                <asp:GridView ID="gridView_imputaciones" runat="server" Caption="IMPUTACIONES" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Efectivo" DataField="Efectivo" />
                                        <asp:BoundField HeaderText="Transferencia" DataField="Transferencia" />
                                        <asp:BoundField HeaderText="Mercado Pago" DataField="Mercado_Pago" />
                                        <asp:BoundField HeaderText="Total" DataField="Total" />
                                        <asp:BoundField HeaderText="Fecha" DataField="Fecha" />

                                        <asp:TemplateField HeaderText="Detalle">
                                            <ItemTemplate>
                                                <asp:Button ID="eliminar_imputacion" OnClick="eliminar_imputacion_Click" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm btn-danger" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ingrese nota">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_nota" CssClass="input-group-text" runat="server" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cargar Notas">
                                            <ItemTemplate>
                                                <asp:Button ID="cargar_nota" OnClick="cargar_nota_Click" Text="Cargar nota" ControlStyle-CssClass="btn btn-primary btn-sm" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="nota" DataField="nota" />

                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="row alert alert-light">
                                <h3>carga de imputaciones</h3>
                                <div class="col">
                                    <asp:Label Text="Efectivo" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_efectivo" CssClass="form-control" OnTextChanged="textBox_efectivo_TextChanged" TextMode="Number" AutoPostBack="true" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:Label Text="Transferencia" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_transferencia" CssClass="form-control" OnTextChanged="textBox_transferencia_TextChanged" TextMode="Number" AutoPostBack="true" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:Label Text="Mercado Pago" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_mercadoPago" CssClass="form-control" OnTextChanged="textBox_mercadoPago_TextChanged" TextMode="Number" AutoPostBack="true" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:Label ID="label_total_imputacion" CssClass="form-label" Text="Total: $0.00" runat="server" />
                                    <br />
                                    <asp:Button ID="boton_cargarImputacion" CssClass="btn btn-primary" OnClick="boton_cargarImputacion_Click" Text="Cargar" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
