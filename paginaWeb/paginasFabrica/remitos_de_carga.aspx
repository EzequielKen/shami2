<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="remitos_de_carga.aspx.cs" Inherits="paginaWeb.paginasFabrica.remitos_de_carga" %>

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

                        <asp:GridView Caption="LISTA DE REMITOS A IMPRIMIR" CaptionAlign="Top" OnRowCommand="gridview_resumen_RowCommand" runat="server" ID="gridview_resumen" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                                <asp:BoundField HeaderText="fecha" DataField="fecha_remito" />
                                <asp:ButtonField HeaderText="eliminar" Text="eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_eliminar" />

                            </Columns>
                        </asp:GridView>
                        <asp:Button CssClass="btn btn-primary" Text="PDF" ID="boton_pdf" OnClick="boton_pdf_Click" runat="server" />
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
                        <asp:GridView Caption="LISTA DE REMITOS" CaptionAlign="Top" OnRowCommand="gridview_remitos_RowCommand" runat="server" ID="gridview_remitos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                                <asp:BoundField HeaderText="fecha" DataField="fecha_remito" />
                                <asp:BoundField HeaderText="proveedor" DataField="proveedor" />
                                <asp:ButtonField HeaderText="PDF" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_abrir" />
                                <asp:ButtonField HeaderText="seleccionar" Text="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_seleccionar" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
