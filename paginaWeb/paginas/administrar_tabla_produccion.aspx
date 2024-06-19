<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="administrar_tabla_produccion.aspx.cs" Inherits="paginaWeb.paginas.administrar_tabla_produccion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="container">
            <% System.Data.DataTable tipo_usuario = (System.Data.DataTable)Session["tipo_usuario"];%>
            <div class="row">
                <div class="col">
                    <div class="alert alert-light">
                        <h2>Historial de Registro de Ventas</h2>
                        <div class="row">

                            <h2>
                                <asp:Label ID="label_total_turno1" Text="Total Turno 1" runat="server" />
                            </h2>
                            <h2>
                                <asp:Label ID="label_total_turno2" Text="Total Turno 2" runat="server" />
                            </h2>
                            <h2>
                                <asp:Label ID="label_total_ventas" Text="Total Venta del dia" runat="server" />
                            </h2>

                            <hr />
                            <%  if (tipo_usuario.Rows[0]["rol"].ToString() == "franquiciado")
                                {%>
                            <div class="col">
                                <h2>
                                    <asp:Label ID="label_fecha_historial_inicio" Text="text" runat="server" />
                                </h2>
                                <asp:Calendar ID="calendario_inicio" CssClass="  table-bordered " OnSelectionChanged="calendario_inicio_SelectionChanged" runat="server">
                                    <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                                    <TitleStyle BackColor="gray"
                                        ForeColor="White"></TitleStyle>
                                    <DayStyle BackColor="gray"></DayStyle>
                                    <SelectedDayStyle BackColor="LightGray"
                                        Font-Bold="True"></SelectedDayStyle>
                                </asp:Calendar>


                            </div>
                            <div class="col ">
                                <h2>
                                    <asp:Label ID="label_fecha_historial_fin" Text="text" runat="server" />
                                </h2>
                                <asp:Calendar ID="calendario_fin" CssClass="  table-bordered " OnSelectionChanged="calendario_fin_SelectionChanged" runat="server">
                                    <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                                    <TitleStyle BackColor="gray"
                                        ForeColor="White"></TitleStyle>
                                    <DayStyle BackColor="gray"></DayStyle>
                                    <SelectedDayStyle BackColor="LightGray"
                                        Font-Bold="True"></SelectedDayStyle>
                                </asp:Calendar>

                            </div>
                            <%}%>
                            <h3>
                                <asp:Label Visible="false" Text="Seleccione turno para ver detalle." runat="server" />
                            </h3>
                            <asp:DropDownList Visible="false" ID="dropdown_turno" CssClass="form-control" OnSelectedIndexChanged="dropdown_turno_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                <asp:ListItem Text="Turno 1" />
                                <asp:ListItem Text="Turno 2" />
                            </asp:DropDownList>
                        </div>
                        <asp:GridView Visible="false" Caption="DETALLE DE VENTAS" CaptionAlign="Top" runat="server" ID="gridview_historial" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_historial_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="id Empleado" DataField="id_empleado" />
                                <asp:BoundField HeaderText="Nombre Empleado" DataField="nombre" />
                                <asp:BoundField HeaderText="Apellido Empleado" DataField="apellido" />
                                <asp:BoundField HeaderText="Turno" DataField="turno" />
                                <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                <asp:BoundField HeaderText="Venta" DataField="venta" />
                                <asp:BoundField HeaderText="Nota" DataField="nota" />
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_eliminar" CssClass="btn btn-danger" Text="Eliminar" runat="server" OnClick="boton_eliminar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
