<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="analisis_de_produccion.aspx.cs" Inherits="paginaWeb.paginasFabrica.analisis_de_produccion1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col d-flex justify-content-center">
                        <asp:Calendar ID="calendario_rango_inicial" CssClass="table-bordered" OnSelectionChanged="calendario_rango_inicial_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray" ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray" Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                    <div class="col ">
                        <h2>
                            <asp:Label ID="label_fecha_inicio" Text="Fecha inicio:" runat="server" />
                        </h2>

                        <h2>
                            <asp:Label ID="label_fecha_final" Text="Fecha fin:" runat="server" />
                        </h2>

                        <asp:Button ID="boton_calcular" CssClass="btn btn-primary" Text="Calcular Analisis" OnClick="boton_calcular_Click" runat="server" />
                    </div>
                    <div class="col d-flex justify-content-center">
                        <asp:Calendar ID="calendario_rango_final" CssClass="table-bordered" OnSelectionChanged="calendario_rango_final_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray" ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray" Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col">
                    <div class="input-group">
                        <asp:Label Text="Tipo Producto" runat="server" CssClass=" form-control" />
                        <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <asp:GridView ID="gridView_productos" runat="server" Caption="Estadisticas" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="producto" DataField="producto" />
                            <asp:BoundField HeaderText="cantidad producida" DataField="cantidad_producida" />
                            <asp:BoundField HeaderText="cantidad recibida" DataField="cantidad_recibida" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
