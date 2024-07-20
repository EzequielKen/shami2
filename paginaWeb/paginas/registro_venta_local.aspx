<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="registro_venta_local.aspx.cs" Inherits="paginaWeb.paginas.registro_venta_local" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <!-- Modal -->
            <div class="modal fade" id="spinnerModal" tabindex="-1" aria-labelledby="spinnerModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="spinnerModalLabel">Registrando Venta... espere...</h5>
                        </div>
                        <div class="modal-body text-center">
                            <div class="spinner-border" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class=" container-fluid">
                <h1>Registro / Historial de Ventas Diarias.</h1>
                <% System.Data.DataTable tipo_usuario = (System.Data.DataTable)Session["tipo_usuario"];%>
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <asp:Button ID="boton_torno_1" CssClass="btn btn-primary " Text="Turno 1" OnClick="boton_torno_1_Click" runat="server" />
                            <asp:Button ID="boton_torno_2" CssClass="btn btn-primary" Text="Turno 2" OnClick="boton_torno_2_Click" runat="server" />
                            <asp:Label ID="label_turno" Text="Turno seleccionado: N/A" runat="server" />
                            <asp:TextBox ID="textbox_venta" CssClass="form-control" placeholder="Ingrese venta" runat="server" OnTextChanged="textbox_venta_TextChanged" AutoPostBack="true" />
                            <asp:TextBox ID="textbox_nota" CssClass="form-control" placeholder="Ingrese nota" runat="server" />
                            <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Registrar" OnClick="boton_cargar_Click" runat="server"/>
                            <asp:Label ID="label_total" Text="Total: $0.00" runat="server" />
                        </div>
                    </div>
                    <div class="col">

                        <h2>
                            <asp:Label ID="label_fecha" Text="Fecha: 18/06/2024" runat="server" />
                        </h2>
                        <asp:Calendar ID="calendario_fecha_registro" CssClass="pb-5 table-bordered " OnSelectionChanged="calendario_fecha_registro_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray"
                                ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray"
                                Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>

                        <asp:Label ID="label_alerta" Visible="false" CssClass="alert alert-danger" Text="text" runat="server" />
                    </div>
                </div>
                <hr />


                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <h2>Historial de Registro de Ventas</h2>
                            <div class="row">
                                <div class="row">
                                    <div class="col">
                                        <h2>
                                            <asp:Label ID="label_total_turno1" Text="Total Turno 1" runat="server" />
                                        </h2>
                                        <h2>
                                            <asp:Label ID="label_total_turno2" Text="Total Turno 2" runat="server" />
                                        </h2>
                                        <h2>
                                            <asp:Label ID="label_total_ventas" Text="Total Venta del dia" runat="server" />
                                        </h2>
                                    </div>
                                    <div class="col">
                                        <h2>
                                            <asp:Label ID="label_porcentaje_turno_1" Text="Total Turno 1" runat="server" />
                                        </h2>
                                        <h2>
                                            <asp:Label ID="label_porcentaje_turno_2" Text="Total Turno 2" runat="server" />
                                        </h2>
                                        <h2>
                                            <asp:Label ID="label_total_porcentaje" Text="Total Venta del dia" runat="server" />
                                        </h2>
                                    </div>
                                </div>


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
                                <h3>Seleccione turno para ver detalle.
                                </h3>
                                <asp:DropDownList ID="dropdown_turno" CssClass="form-control" OnSelectedIndexChanged="dropdown_turno_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <asp:ListItem Text="Turno 1" />
                                    <asp:ListItem Text="Turno 2" />
                                </asp:DropDownList>
                            </div>
                            <asp:GridView Caption="DETALLE DE VENTAS" CaptionAlign="Top" runat="server" ID="gridview_historial" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_historial_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="id Empleado" DataField="id_empleado" />
                                    <asp:BoundField HeaderText="Nombre Empleado" DataField="nombre" />
                                    <asp:BoundField HeaderText="Apellido Empleado" DataField="apellido" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                    <asp:BoundField HeaderText="Turno" DataField="turno" />
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
