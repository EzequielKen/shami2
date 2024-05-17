<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="historial_movimientos.aspx.cs" Inherits="paginaWeb.paginasCarrefour.historial_movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <h1>
                                        <asp:Label ID="label_deuda_total_mes" Text="Deuda total del mes (todas las Sucursales)" CssClass="badge bg-secondary" runat="server" />
                                    </h1>


                                </div>
                                <div class="col">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                </div>
                                <div class="col">
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <hr />
            </div>


            <div class="container-fluid">

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <asp:Label Text="Sucursal" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_sucursales" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_sucursales_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
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
                            <div class="alert alert-light">
                                <div class="col">
                                    <h3>
                                        <asp:Label ID="label_saldo_anterior" Text="Saldo mes anterior" CssClass="badge bg-secondary" runat="server" />
                                    </h3>
                                    <h2>
                                        <asp:Label ID="label_venta_mes_titulo" Text="Total venta del mes:" CssClass="badge bg-secondary" runat="server" />
                                    </h2>
                                </div>
                                <div class="col-1"></div>
                                <div class="col">
                                    <h2>
                                        <asp:Label ID="label_pagado_mes_titulo" Text="Total pagado del mes:" CssClass="badge bg-secondary" runat="server" />
                                    </h2>
                                    <h1>
                                        <asp:Label ID="label_saldo" Text="SALDO:" CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <div class="alert alert-light">
                            <h2>
                                <asp:Label ID="label_pagado_mes" Text="Total pagado del mes:" CssClass="badge bg-secondary" runat="server" />
                            </h2>
                            <asp:GridView ID="gridView_imputaciones" runat="server" Caption="IMPUTACIONES" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnRowCommand="gridView_imputaciones_RowCommand">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Efectivo" DataField="efectivo" />
                                    <asp:BoundField HeaderText="Transferencia" DataField="transferencia" />
                                    <asp:BoundField HeaderText="Mercado Pago" DataField="mercado_pago" />
                                    <asp:BoundField HeaderText="Total" DataField="total" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                </Columns>
                            </asp:GridView>
                            <div class="row alert alert-light">
                                <h3>carga de imputaciones</h3>
                                <div class="col">
                                    <asp:Label Text="Efectivo" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_efectivo" CssClass="form-control" OnTextChanged="textBox_efectivo_TextChanged" AutoPostBack="true" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:Label Text="Transferencia" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_transferencia" CssClass="form-control" OnTextChanged="textBox_transferencia_TextChanged" AutoPostBack="true" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:Label Text="Mercado Pago" CssClass=" form-label" runat="server" />
                                    <asp:TextBox ID="textBox_mercadoPago" CssClass="form-control" OnTextChanged="textBox_mercadoPago_TextChanged" AutoPostBack="true" runat="server" />
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

            <div class="container">
                <hr />
            </div>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <h2>
                                    <asp:Label ID="label_venta_mes" Text="Total venta del mes:" CssClass="badge bg-secondary" runat="server" />
                                </h2>

                                <asp:GridView ID="gridView_movimientos" runat="server" Caption="REMITOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnSelectedIndexChanged="gridView_movimientos_SelectedIndexChanged">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                        <asp:BoundField HeaderText="Valor" DataField="valor_venta" />
                                        <asp:CommandField HeaderText="Abrir" ShowSelectButton="true" SelectText="Abrir" ControlStyle-CssClass="btn btn-primary btn-sm"/>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <asp:GridView ID="gridView_resumen" runat="server" Caption="RESUMEN" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                                        <asp:BoundField HeaderText="Stock Inicial" DataField="stock_inicial" />
                                        <asp:BoundField HeaderText="Devolucion" DataField="devolucion" />
                                        <asp:BoundField HeaderText="Reposicion" DataField="reposicion" />
                                        <asp:BoundField HeaderText="Stock Final" DataField="stock_final" />
                                        <asp:BoundField HeaderText="Vendido" DataField="vendido" />
                                        <asp:BoundField HeaderText="Vendido" DataField="sub_total" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container">
                <hr />
            </div>

            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6"></div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
