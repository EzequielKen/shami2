<%@ Page  Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="caja_chica.aspx.cs" Inherits="paginaWeb.paginasGerente.caja_chica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col"></div>
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="text-center">
                                <h2>Efectivo en caja</h2>
                                <h2>
                                    <asp:Label ID="label_efectivo_en_caja" Text="N/A" CssClass="badge bg-secondary" runat="server" />
                                </h2>
                            </div>
                        </div>
                    </div>
                    <div class="col"></div>
                </div>

                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                                <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                                <TitleStyle BackColor="gray"
                                    ForeColor="White"></TitleStyle>
                                <DayStyle BackColor="gray"></DayStyle>
                                <SelectedDayStyle BackColor="LightGray"
                                    Font-Bold="True"></SelectedDayStyle>
                            </asp:Calendar>
                            <h3>
                                <asp:Label ID="label_fecha" Text="Seleccione una fecha." runat="server" />
                            </h3>
                        </div>
                    </div>
                    <div class="col">
                        <div class="alert alert-light">
                            <h5>
                                <asp:Label Text="Tipo movimiento" runat="server" />
                                <asp:Button ID="boton_administrar" CssClass="btn btn-primary" OnClick="boton_administrar_Click1" Text="Administrar movimientos" runat="server" />
                            </h5>
                            <asp:Button ID="boton_ingreso" Text="Ingreso" CssClass="btn btn-success" OnClick="boton_ingreso_Click" runat="server" />
                            <asp:Button ID="boton_egreso" Text="Egreso" CssClass="btn btn-danger" OnClick="boton_egreso_Click" runat="server" />
                            <h5>
                                <asp:Label Text="Concepto"  runat="server" />
                            </h5>
                            <asp:DropDownList ID="DropDown_conceptos" Visible="false" CssClass="btn btn-success dropdown-toggle" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="alert alert-light">
                            <h5>
                                <asp:Label Text="Detalle" runat="server" />
                            </h5>
                            <asp:TextBox ID="textbox_detalle" placeholder="Ingrese detalle" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                    <div class="col">
                        <div class="alert alert-light">
                            <h5>
                                <asp:Label Text="Cantidad" runat="server" />
                            </h5>
                            <h5>
                                <asp:Label ID="label_cantidad" Text="$0,00" runat="server" />
                            </h5>
                            <asp:TextBox ID="textbox_cantidad" OnTextChanged="textbox_cantidad_TextChanged" AutoPostBack="true" placeholder="Ingrese cantidad utilizando la coma solo para decimales. Ejemplo: 100.50" CssClass="form-control" runat="server" />
                            <asp:Button ID="boton_cargar" OnClick="boton_cargar_Click" CssClass="btn btn-primary" Text="Cargar" runat="server" />

                        </div>
                        <asp:Label CssClass="alert alert-danger " ID="label_mensaje_de_alerta" Text="Falta seleccionar una fecha." runat="server" Visible="false" />
                    </div>
                </div>
            </div>
            <div>
                <hr />
            </div>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-1"></div>
                    <div class="col-10">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col"></div>
                                <div class="col">
                                    <asp:Label Text="Mes" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_mes" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Label Text="Año" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_año" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col"></div>
                            </div>
                            <asp:GridView ID="gridView_movimientos" OnRowDataBound="gridView_movimientos_RowDataBound" runat="server" Caption="MOVIMIENTOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnSelectedIndexChanged="gridView_movimientos_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Detalle" DataField="detalle" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                    <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                                    <asp:BoundField HeaderText="Tipo Movimiento" DataField="tipo_movimiento" />
                                    <asp:BoundField HeaderText="Inicial" DataField="inicial" />
                                    <asp:BoundField HeaderText="Movimiento" DataField="movimiento" />
                                    <asp:BoundField HeaderText="Final" DataField="final" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="eliminar" HeaderText="eliminar" ControlStyle-CssClass="btn btn-danger btn-sm" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-1"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
