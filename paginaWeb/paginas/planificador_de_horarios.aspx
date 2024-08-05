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
                    <div class="col">
                        <h1>Planificador de Horarios</h1>
                        <div class="row">
                            <div class="col">
                                <div class=" input-group">
                                    <div class="col">
                                        <label>Fecha de Inicio de Rango:</label>
                                        <asp:TextBox ID="textbox_fecha_inicio" CssClass="form-control" type="date" runat="server" OnTextChanged="textbox_fecha_inicio_TextChanged" AutoPostBack="true" />
                                    </div>

                                    <div class="col">
                                        <label>Fecha de Fin de Rango:</label>
                                        <asp:TextBox ID="textbox_fecha_fin" CssClass="form-control" type="date" runat="server" OnTextChanged="textbox_fecha_fin_TextChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col">
                                <h3>
                                    <label class="badge rounded-pill text-bg-success">Verde = Horario Asignado</label>
                                </h3>
                                <h3>
                                    <label class="badge rounded-pill text-bg-warning">Amarillo = Franco Asignado</label>
                                </h3>
                                <h3>
                                    <label class="badge rounded-pill text-bg-danger">Rojo = Horario no Asignado</label>
                                </h3>
                            </div>
                        </div>
                    </div>
                    <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark table-striped gridview-custom" OnRowDataBound="gridview_empleados_RowDataBound">
                        <Columns>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
