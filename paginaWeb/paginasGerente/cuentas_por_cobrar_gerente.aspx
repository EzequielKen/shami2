<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="cuentas_por_cobrar_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.cuentas_por_cobrar_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <h2>Cuentas por Cobrar</h2>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">


                                    <h1>
                                        <asp:Label ID="label_deuda_total_mes" Text="Deuda total del mes" CssClass="badge bg-secondary" runat="server" />
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


            <div class="container">

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <asp:Label Text="Cliente" CssClass=" form-label" runat="server" />
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
                        </div>
                    </div>

                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="col">
                                <h3>
                                    <asp:Label ID="label_saldo_anterior" Text="Saldo mes anterior" CssClass="badge bg-secondary" runat="server" />
                                </h3>
                                <h2>
                                    <asp:Label ID="label_total_compra_titulo" Text="Total compra:" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                            </div>
                            <div class="col-1"></div>
                            <div class="col">
                                <h2>
                                    <asp:Label ID="label_total_pago_titulo" Text="Total pago:" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                                <h1>
                                    <asp:Label ID="label_saldo" Text="SALDO:" CssClass="badge bg-secondary" runat="server" />
                                </h1>
                                <h1>
                                    <asp:Label ID="label_deuda_actual" Text="SALDO:" CssClass="badge bg-secondary" runat="server" />
                                </h1>
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
                        <div class="alert alert-light">
                            <div class="container-fluid">
                                <h2>
                                    <asp:Label ID="label_total_compra" Text="Total compra:" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                                <asp:GridView ID="gridView_remitos" runat="server" Caption="REMITOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnSelectedIndexChanged="gridView_remitos_SelectedIndexChanged" OnRowDataBound="gridView_remitos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                        <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                                        <asp:BoundField HeaderText="fecha" DataField="fecha_remito" />
                                        <asp:BoundField HeaderText="valor" DataField="valor_remito" />
                                        <asp:CommandField HeaderText="PDF" ShowSelectButton="true" SelectText="PDF" ControlStyle-CssClass="btn btn-primary btn-sm" />
                                        <asp:BoundField HeaderText="nota" DataField="nota" />
                                        <asp:TemplateField HeaderText="Cobrado">
                                            <ItemTemplate>
                                                <asp:Button ID="boton_cobrado" CssClass="btn btn-primary" Text="Cobrado" runat="server" OnClick="boton_cobrado_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aplicar IVA">
                                            <ItemTemplate>
                                                <asp:Button ID="boton_iva" CssClass="btn btn-primary" Text="Aplicar IVA" runat="server" OnClick="boton_iva_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <h2>
                                    <asp:Label ID="label_total_pago" Text="Total pago:" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                                <asp:GridView ID="gridView_imputaciones" runat="server" Caption="IMPUTACIONES" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnRowCommand="gridView_imputaciones_RowCommand">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Efectivo" DataField="Efectivo" />
                                        <asp:BoundField HeaderText="Transferencia" DataField="Transferencia" />
                                        <asp:BoundField HeaderText="Mercado Pago" DataField="Mercado_Pago" />
                                        <asp:BoundField HeaderText="Total" DataField="Total" />
                                        <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                                        <asp:BoundField HeaderText="Autorizado" DataField="Autorizado" />
                                        <asp:ButtonField HeaderText="Autorizado" Text="Autorizado" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="autorizar" />
                                        <asp:TemplateField HeaderText="Ingrese nota">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_nota" CssClass="input-group-text" runat="server" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField HeaderText="Cargar Notas" Text="Cargar Nota" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_nota" />

                                        <asp:BoundField HeaderText="nota" DataField="nota" />
                                        <asp:ButtonField HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-danger btn-sm" CommandName="eliminar" />
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

            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="row">
                            <div class="col">
                                <div class="alert alert-light">
                                    <h3>Cargar Boleta</h3>
                                    <div class="row">
                                        <div class="col">
                                            <asp:Label Text="Detalle" CssClass=" form-label" runat="server" />
                                            <asp:TextBox ID="textBox_detalle_boleta" CssClass="form-control" OnTextChanged="textBox_detalle_boleta_TextChanged" AutoPostBack="true" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Monto" CssClass=" form-label" runat="server" />
                                        <asp:TextBox ID="textBox_monto_boleta" CssClass="form-control" OnTextChanged="textBox_monto_boleta_TextChanged" TextMode="Number" AutoPostBack="true" runat="server" />
                                    </div>
                                    <div class="col">
                                        <asp:Label ID="label_total_boleta" CssClass="form-label" Text="Total: $0.00" runat="server" />
                                        <br />
                                        <asp:Button ID="boton_cargar_boleta" CssClass="btn btn-primary" OnClick="boton_cargar_boleta_Click" Text="Cargar" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col">
                                <div class="alert alert-light">
                                    <h3>Carga de Notas de Credito</h3>
                                    <div class="row">
                                        <div class="col">
                                            <asp:Label Visible="false" Text="Dia" CssClass=" form-label" runat="server" />
                                            <asp:DropDownList Visible="false" ID="DropDow_nota_dia" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col">
                                            <asp:Label Visible="false" Text="Mes" CssClass=" form-label" runat="server" />
                                            <asp:DropDownList Visible="false" ID="DropDow_nota_mes" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col">
                                            <asp:Label Visible="false" Text="Año" CssClass=" form-label" runat="server" />
                                            <asp:DropDownList Visible="false" ID="DropDow_nota_año" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col">
                                            <asp:Label Text="Detalle" CssClass=" form-label" runat="server" />
                                            <asp:TextBox ID="textBox_detalle" CssClass="form-control" OnTextChanged="textBox_detalle_TextChanged" AutoPostBack="true" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Monto" CssClass=" form-label" runat="server" />
                                        <asp:TextBox ID="textBox_monto" CssClass="form-control" OnTextChanged="textBox_monto_TextChanged" TextMode="Number" AutoPostBack="true" runat="server" />
                                    </div>
                                    <div class="col">
                                        <asp:Label ID="label_monto" CssClass="form-label" Text="Total: $0.00" runat="server" />
                                        <br />
                                        <asp:Button ID="boton_carga_nota_credido" CssClass="btn btn-primary" OnClick="boton_carga_nota_credido_Click" Text="Cargar" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="container-fluid">
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
