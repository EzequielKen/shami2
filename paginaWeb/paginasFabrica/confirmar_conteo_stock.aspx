<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="confirmar_conteo_stock.aspx.cs" Inherits="paginaWeb.paginasFabrica.confirmar_conteo_stock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <h3>
                    <asp:Label ID="label_fecha" Text="Fecha Seleccionada:" runat="server" />
                </h3>
                <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                    <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                    <TitleStyle BackColor="gray"
                        ForeColor="White"></TitleStyle>
                    <DayStyle BackColor="gray"></DayStyle>
                    <SelectedDayStyle BackColor="LightGray"
                        Font-Bold="True"></SelectedDayStyle>
                </asp:Calendar>
            </div>
            <div class="col">
                <asp:GridView Caption="LISTA DE CONTEOS DE STOCK" CaptionAlign="Top" runat="server" ID="gridview_conteos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_conteos_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="id producto" DataField="id_producto" />
                        <asp:BoundField HeaderText="fecha" DataField="fecha" />
                        <asp:BoundField HeaderText="producto" DataField="producto" />
                        <asp:BoundField HeaderText="unidad de medida" DataField="unidad_medida" />
                        <asp:BoundField HeaderText="conteo de stock" DataField="conteo" />
                        <asp:BoundField HeaderText="stock" DataField="stock" />
                        <asp:BoundField HeaderText="diferencia" DataField="diferencia" />

                        <asp:TemplateField HeaderText="Aprobar">
                            <ItemTemplate>
                                <asp:TextBox ID="textbox_nota" CssClass="form-control" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Aprobar">
                            <ItemTemplate>
                                <asp:Button ID="boton_aprobar" CssClass="btn btn-success" Text="Aprobar" OnClick="boton_aprobar_Click" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Eliminar conteo de stock">
                            <ItemTemplate>
                                <asp:Button ID="boton_eliminar" CssClass="btn btn-danger" Text="Eliminar" OnClick="boton_eliminar_Click" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="Nota" DataField="nota" />
                        <asp:BoundField HeaderText="Aprobado" DataField="aprobado" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
