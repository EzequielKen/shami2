<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="recepcion_de_fabrica_fatay.aspx.cs" Inherits="paginaWeb.paginasFabrica.recepcion_de_fabrica_fatay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">

                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>

                            <TitleStyle BackColor="gray"
                                ForeColor="White"></TitleStyle>

                            <DayStyle BackColor="gray"></DayStyle>

                            <SelectedDayStyle BackColor="LightGray"
                                Font-Bold="True"></SelectedDayStyle>

                        </asp:Calendar>


                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <h2>
                                <asp:Label Text="Fecha seleccionada: " runat="server" />
                            </h2>
                            <h2>
                                <asp:Label ID="label_fecha" Text="text" runat="server" />
                            </h2>
                        </div>
                        <asp:GridView Caption="LISTA DE PRODUCCION" CaptionAlign="Top" OnRowCommand="gridview_historial_RowCommand" OnDataBound="gridview_historial_DataBound" runat="server" ID="gridview_historial" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="fecha" DataField="fecha" />
                                <asp:BoundField HeaderText="proveedor" DataField="proveedor" />
                                <asp:BoundField HeaderText="receptor" DataField="receptor" />
                                <asp:BoundField HeaderText="estado" DataField="estado" />
                                <asp:ButtonField HeaderText="PDF" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="crear_pdf" />
                                <asp:ButtonField HeaderText="Abrir" Text="Abrir" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="abrir" />
                                <asp:ButtonField HeaderText="Confirmar" Text="Confirmar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="confirmar" />
                                <asp:ButtonField HeaderText="Cancelar" Text="Cancelar" ControlStyle-CssClass="btn btn-danger btn-sm" CommandName="cancelar" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class=" container">
                <hr />
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <h2>
                            <asp:Label ID="label_id" Text="" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_fecha_seleccionada" Text="" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_proveedor" Text="" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_receptor" Text="" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_estado" Text="" runat="server" />
                        </h2>
                        <asp:GridView Caption="DETALLE DE PRODUCCION" CaptionAlign="Top" runat="server" ID="gridview_detalle_produccion" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="tipo producto" DataField="Tipo Producto" />

                                <asp:BoundField HeaderText="cantidad entregada" DataField="Cant.Entregada" />
                                <asp:BoundField HeaderText="unidad de medida" DataField="Unid.Medida" />
                                <asp:BoundField HeaderText="producto" DataField="Producto" />

                                <asp:BoundField HeaderText="cantidad recibida" DataField="Cant.Recibida" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
