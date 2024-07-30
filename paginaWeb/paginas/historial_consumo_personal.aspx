<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_consumo_personal.aspx.cs" Inherits="paginaWeb.paginas.historial_consumo_personal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
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
                        <asp:GridView Caption="LISTA DE CONSUMO" CaptionAlign="Top" runat="server" ID="gridview_consumo" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="nombre" />
                                <asp:BoundField HeaderText="Nombre" DataField="apellido" />
                                <asp:BoundField HeaderText="Apellido" DataField="producto" />
                                <asp:BoundField HeaderText="Apellido" DataField="cantidad" />
                                <asp:BoundField HeaderText="Apellido" DataField="costo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
