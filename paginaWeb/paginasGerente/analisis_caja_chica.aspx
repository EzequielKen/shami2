<%@ Page EnableEventValidation="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="analisis_caja_chica.aspx.cs" Inherits="paginaWeb.paginasGerente.analisis_caja_chica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <div class="text-center">
                        <h2>Total ingresos</h2>
                        <h2>
                            <asp:Label ID="label_total_ingresos" Text="N/A" CssClass="badge bg-success" runat="server" />
                        </h2>
                    </div>
                </div>
                <asp:GridView ID="gridView_conceptos_ingresos" runat="server" Caption="Conceptos Ingresos" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-success text-center" OnSelectedIndexChanged="gridView_conceptos_ingresos_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                        <asp:BoundField HeaderText="Cantidad Movimientos" DataField="cantidad_movimiento" />
                        <asp:BoundField HeaderText="Total" DataField="total" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Ver Detalle" HeaderText="Detalle" ControlStyle-CssClass="btn btn-primary btn-sm" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <div class="row">
                        <div class="col"></div>
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
                        <div class="col"></div>
                    </div>


                </div>
                <div class="alert alert-light">
                    <div class="row">
                        <asp:TextBox ID="textbox_detalle" placeholder="Buscar." OnTextChanged="textbox_detalle_TextChanged" AutoPostBack="true"  runat="server" />
                        <asp:GridView ID="gridView_detalle" runat="server" Caption="Detalle" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table text-center" OnRowDataBound="gridView_detalle_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Fecha" DataField="fecha_simple" />
                                <asp:BoundField HeaderText="Detalle" DataField="detalle" />
                                <asp:BoundField HeaderText="Cantidad" DataField="total" />
                                <asp:BoundField HeaderText="Tipo Movimiento" DataField="tipo_movimiento" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <div class="text-center">
                        <h2>Total egresos</h2>
                        <h2>
                            <asp:Label ID="label_total_egresos" Text="N/A" CssClass="badge bg-danger" runat="server" />
                        </h2>
                    </div>
                </div>
                <asp:GridView ID="gridView_conceptos_egresos" runat="server" Caption="Conceptos Egresos" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-danger text-center" OnSelectedIndexChanged="gridView_conceptos_egresos_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField ShowSelectButton="true" SelectText="Ver Detalle" HeaderText="Detalle" ControlStyle-CssClass="btn btn-primary btn-sm" />
                        <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                        <asp:BoundField HeaderText="Cantidad Movimiento" DataField="cantidad_movimiento" />
                        <asp:BoundField HeaderText="Total" DataField="total" />
                    </Columns>
                </asp:GridView>

            </div>

        </div>
    </div>
</asp:Content>
