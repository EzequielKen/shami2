<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="administrar_tabla_produccion.aspx.cs" Inherits="paginaWeb.paginas.administrar_tabla_produccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <h1>Administrar Tablas de Produccion.</h1>
                <% System.Data.DataTable tipo_usuario = (System.Data.DataTable)Session["tipo_usuario"];%>
                <div class="row">
                    <div class="alert alert-light">
                        <div class="row">
                            <h3>
                                <label>Seleccione Dias de </label>
                                <span class="badge rounded-pill bg-success">Alta Venta</span>
                            </h3>
                            <h3>
                                <label>Dias de </label>
                                <span class="badge rounded-pill bg-danger">Baja Venta</span>
                            </h3>
                            <div class="col text-center">
                                <asp:Button ID="boton_lunes" Text="Lunes" CssClass="btn btn-danger" runat="server" OnClick="boton_lunes_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_martes" Text="Martes" CssClass="btn btn-danger" runat="server" OnClick="boton_martes_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_miercoles" Text="Miercoles" CssClass="btn btn-danger" runat="server" OnClick="boton_miercoles_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_jueves" Text="Jueves" CssClass="btn btn-danger" runat="server" OnClick="boton_jueves_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_viernes" Text="Viernes" CssClass="btn btn-danger" runat="server" OnClick="boton_viernes_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_sabado" Text="Sabado" CssClass="btn btn-danger" runat="server" OnClick="boton_sabado_Click" />
                            </div>
                            <div class="col text-center">
                                <asp:Button ID="boton_domingo" Text="Domingo" CssClass="btn btn-danger" runat="server" OnClick="boton_domingo_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="alert alert-light">
                        <h2>Total de Ventas de los Ultimos 7 Dias:</h2>
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

                        </div>
                        <h2>
                            <label>Seleccione Categoria de Producto:</label>
                        </h2>
                        <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true" runat="server">
                        </asp:DropDownList>
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_productos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="id Empleado" DataField="producto" />
                                <asp:TemplateField HeaderText="Ingrese Unidades Vendidas Dias de Alto vol. Venta">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_venta_alta" CssClass="form-control " OnTextChanged="textbox_venta_alta_TextChanged" AutoPostBack="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ingrese Unidades Vendidas Dias de Bajo vol. Venta">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_venta_baja" CssClass="form-control " OnTextChanged="textbox_venta_baja_TextChanged" AutoPostBack="true" runat="server" />
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
