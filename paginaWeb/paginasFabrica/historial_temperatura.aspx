<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_temperatura.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_temperatura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <h1>Historial de temperaturas</h1>
        <div class="container-fluid">
            <div class="row">
                <div class="col-1"></div>
                <div class="col-10">
                    <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                        <TitleStyle BackColor="gray"
                            ForeColor="White"></TitleStyle>
                        <DayStyle BackColor="gray"></DayStyle>
                        <SelectedDayStyle BackColor="LightGray"
                            Font-Bold="True"></SelectedDayStyle>
                    </asp:Calendar>
                    <h3 class="pt-5">
                        <asp:Label ID="label_fecha"  CssClass="alert alert-primary" Text="Ingrese su nombre" runat="server" />
                    </h3>
                    <h3 class="pt-5">
                        <asp:Label  CssClass="alert alert-danger" Text="Color rojo = Temperatura/Horario Fuera de rango" runat="server" />
                    </h3>
                    <div id="div_equipos" class="alert alert-light">

                        <div class="input-group">
                            <asp:DropDownList ID="dropdown_ubicaciones" CssClass=" form-control dropdown" AutoPostBack="true" OnSelectedIndexChanged="dropdown_ubicaciones_SelectedIndexChanged" runat="server">
                            </asp:DropDownList>
                        </div>

                        <asp:GridView Caption="LISTA DE EQUIPOS" CaptionAlign="Top" runat="server" ID="gridview_equipos" AutoGenerateColumns="false" CssClass="table table-dark table-striped pt-5" OnRowDataBound="gridview_equipos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="ubicacion" DataField="ubicacion" />
                                <asp:BoundField HeaderText="nombre" DataField="nombre" />
                                <asp:BoundField HeaderText="categoria" DataField="categoria" />
                                <asp:BoundField HeaderText="temperatura optima" DataField="temperatura" />
                                <asp:BoundField HeaderText="Turno 1: 08:00Hs a 12:00Hs" DataField="turno_1" />
                                <asp:BoundField HeaderText="Turno 2: 16:00Hs a 18:00Hs" DataField="turno_2" />
                                <asp:BoundField HeaderText="Turno 3: 21:00Hs a 23:00Hs" DataField="turno_3" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-1"></div>
            </div>
        </div>
    </div>
</asp:Content>
