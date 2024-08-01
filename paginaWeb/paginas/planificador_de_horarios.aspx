<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="planificador_de_horarios.aspx.cs" Inherits="paginaWeb.paginas.planificador_de_horarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /* Aplica la línea de separación solo a las celdas a partir de la cuarta columna (lunes en adelante) */
        .gridview-custom td:nth-child(n+3), .gridview-custom th:nth-child(n+3) {
            border-right: 1px solid #ccc; /* Línea de separación */
        }

        /* Quita la línea de la última columna */
        .gridview-custom td:last-child, .gridview-custom th:last-child {
            border-right: none;
        }
    </style>

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark table-striped gridview-custom" OnRowDataBound="gridview_empleados_RowDataBound">
                            <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                            <asp:BoundField HeaderText="Apellido" DataField="apellido" />

                            <asp:TemplateField HeaderText="Lunes">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_lunes_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_lunes_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Martes">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_martes_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_martes_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Miercoles">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_miercoles_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_miercoles_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Jueves">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_jueves_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_jueves_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Viernes">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_viernes_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_viernes_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sabado">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_sabado_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_sabado_salida" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Domingo">
                                <ItemTemplate>
                                    <div class="form-group">
                                        <label for="timePickerLunes">Hora Entrada:</label>
                                        <asp:TextBox ID="textbox_domingo_entrada" CssClass="form-control" type="time" runat="server" />
                                        <label for="timePickerLunes">Hora Salida:</label>
                                        <asp:TextBox ID="textbox_lunes_entrada" CssClass="form-control" type="time" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
