<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="visita_operativa_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.visita_operativa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h1>Visitas Operativas.</h1>

            <div class="container">
                <div class="row">
                    <div class="col">
                        <asp:Calendar ID="calendario" CssClass="table-bordered" OnSelectionChanged="calendario_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray" ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray" Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                    <div class="col">
                        <asp:GridView Caption="LISTA DE SUCURSALES" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Sucursal" DataField="sucursal" />

                                <asp:TemplateField HeaderText="Historial">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_historial" CssClass="btn btn-primary" Text="Ver Historial" OnClick="boton_historial_Click" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
