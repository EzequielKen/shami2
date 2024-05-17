<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="rendiciones.aspx.cs" Inherits="paginaWeb.paginasFabrica.rendiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container">

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <div class="input-group mb-3">
                            <h2 class="form-control-lg">Año:</h2>
                            <asp:DropDownList ID="dropDown_año" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                            <h2 class="form-control-lg">Mes:</h2>
                            <asp:DropDownList ID="dropDown_mes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                            <h2 class="form-control-lg">Acuerdo de precio:</h2>
                            <asp:DropDownList ID="dropdown_acuerdo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_acuerdo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <div class="input-group mb-3">

                            <asp:Label Text="FECHA DE ACUERDO:" CssClass="form-control-lg" ID="label_fecha" runat="server" />
                            <asp:Button ID="boton_generar_pdf" Text="Generar PDF" CssClass="btn btn-primary" OnClick="boton_generar_pdf_Click" runat="server" />

                        </div>

                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <h1 class="text-bg-primary text-center">RESUMEN </h1>

                                    <h1>
                                        <asp:Label ID="label_disponible" Text="Dinero disponible: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <h1>
                                        <asp:Label ID="label_compra" Text="Compra de vegetales: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <h1>
                                        <asp:Label ID="label_venta" Text="Venta de vegetales: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <hr />
                                    <h1>
                                        <asp:Label ID="label_total_nafta" Text="Total nafta: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <h1>
                                        <asp:Label ID="label_total_venta_cajones" Text="Total venta cajones: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <h1>
                                        <asp:Label ID="label_total_compra_cajones" Text="Total compra cajones: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <hr />

                                    <h1>
                                        <asp:Label ID="label_egresos" Text="Egresos: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <h1>
                                        <asp:Label ID="label_pagos" Text="Pagos de locales: " CssClass="badge bg-secondary" runat="server" />
                                    </h1>
                                    <hr />
                                    <h1 class="text-bg-primary text-center">INGRESO DE GASTOS </h1>
                                    <div class="input-group">
                                        <h1>
                                            <asp:Label ID="label1" Text="Nafta: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_nafta" CssClass=" input-group-text" runat="server" />
                                        <asp:Button ID="boton_carga_nafta" Text="Cargar" CssClass=" btn btn-primary" OnClick="boton_carga_nafta_Click" runat="server" />

                                    </div>
                                    <div class="input-group">
                                        <h1>
                                            <asp:Label ID="label2" Text="Cajones compra: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_cajones_compra" CssClass=" input-group-text" runat="server" />
                                        <asp:Button ID="boton_carga_cajones_compra" Text="Cargar" CssClass=" btn btn-primary" OnClick="boton_carga_cajones_compra_Click" runat="server" />

                                    </div>

                                    <div class="input-group">

                                        <h1>
                                            <asp:Label ID="label5" Text="Cajones venta: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_cajones_ventas" CssClass=" input-group-text" runat="server" />
                                        <asp:Button ID="boton_carga_cajones_venta" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_carga_cajones_venta_Click" runat="server" />

                                    </div>

                                    <hr />
                                    <h1 class="text-bg-primary text-center">INGRESO DE PAGOS Y EGRESOS </h1>

                                    <div class="input-group  ">

                                        <h1>
                                            <asp:Label ID="label4" Text="Pago de locales: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_pago_cantidad" CssClass=" input-group-text" runat="server" />

                                        <h1>
                                            <asp:Label ID="label8" Text="Detalle: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_pago_detalle" CssClass=" input-group-text" runat="server" />

                                        <asp:Button ID="boton_pago" Text="Cargar pago" CssClass=" btn btn-primary" OnClick="boton_pago_Click" runat="server" />

                                    </div>
                                    <div class="input-group mt-4">

                                        <h1>
                                            <asp:Label ID="label3" Text="Egreso: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_egreso_cantidad" CssClass=" input-group-text" runat="server" />

                                        <h1>
                                            <asp:Label ID="label7" Text="Detalle: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_egreso_detalle" CssClass=" input-group-text" runat="server" />

                                        <asp:Button ID="boton_egreso" Text="Cargar egreso" CssClass=" btn btn-primary" OnClick="boton_egreso_Click" runat="server" />

                                    </div>


                                    <hr />
                                    <h1 class="text-bg-primary text-center">INGRESO DE RENDICION REAL </h1>

                                    <div class="input-group">
                                        <h1>
                                            <asp:Label ID="label_rendicion_teorica" Text="Rendicion teorica: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                    </div>
                                    <div class="input-group">

                                        <h1>
                                            <asp:Label ID="label6" Text="Rendicion real: " CssClass=" input-group-text badge bg-secondary" runat="server" />
                                        </h1>
                                        <asp:TextBox ID="textbox_rendicion_real" CssClass=" input-group-text " runat="server" />
                                        <asp:Button ID="boton_carga_rendicionReal" Text="Cargar" OnClick="boton_carga_rendicionReal_Click" CssClass="input-group-text " runat="server" />
                                        <h1>
                                            <asp:Label ID="label_rendicion_real" Text="Rendicion real: " CssClass=" input-group-text badge bg-secondary " runat="server" />
                                        </h1>
                                    </div>

                                    <div class="input-group">
                                        <h1>
                                            <asp:Label ID="label_diferencia" Text="Diferencia: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                    </div>
                                    <div class="input-group">
                                        <h1>
                                            <asp:Label ID="label_nuevo_disponible" Text="Nuevo disponible: " CssClass="badge bg-secondary" runat="server" />
                                        </h1>
                                    </div>
                                    <div class="row m-4 ">
                                        <div class="col">
                                            <asp:Button ID="boton_cargar_datos" Text="Confirmar operacion" CssClass="btn btn-primary " OnClick="boton_cargar_datos_Click" runat="server" />
                                        </div>
                                        <div class="col"></div>
                                        <div class="col"></div>

                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">

                        <div class="input-group mb-3">
                            <h2 class="form-control-lg">Filtrar lista:</h2>
                            <asp:DropDownList ID="dropdown_filtro_lista" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_filtro_lista_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class=" row">
                        <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                            <asp:GridView ID="gridview_movimientos" Caption="Lista de movimientos" CaptionAlign="Top" CssClass="table table-dark table-striped" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="id producto" DataField="id_producto" />

                                    <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                                    <asp:BoundField HeaderText="Cantidad" DataField="cantidad" />
                                    <asp:BoundField HeaderText="Unid. medida" DataField="unidad_de_medida" />

                                    <asp:BoundField HeaderText="Detalle" DataField="detalle" />
                                    <asp:BoundField HeaderText="Precio" DataField="precio" />
                                    <asp:BoundField HeaderText="Total" DataField="valor" />
                                    <asp:BoundField HeaderText="Stock inicial" DataField="stock_inicial" />
                                    <asp:BoundField HeaderText="Stock final" DataField="stock_final" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
