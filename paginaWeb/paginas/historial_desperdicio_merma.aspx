<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_desperdicio_merma.aspx.cs" Inherits="paginaWeb.paginas.historial_desperdicio_merma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <h2>Seleccione Merma o Desperdicio.</h2>
                        <asp:DropDownList ID="dropdown_categoria" CssClass="form-control" OnSelectedIndexChanged="dropdown_categoria_SelectedIndexChanged" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Desperdicio" />
                            <asp:ListItem Text="Merma" />
                        </asp:DropDownList>
                        <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray"
                                ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray"
                                Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                        <h3>
                            <asp:Label ID="label_fecha_seleccionada" runat="server" Text="Label"></asp:Label>
                        </h3>
                    </div>
                    <div class="col">
                        <asp:GridView Caption="LISTA DE DESPERDICIO Y MERMA" CaptionAlign="Top" runat="server" ID="gridview_consumo" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Cantidad" DataField="cantidad" />
                                <asp:BoundField HeaderText="nota" DataField="nota" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
