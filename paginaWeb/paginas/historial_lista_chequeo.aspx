<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_lista_chequeo.aspx.cs" Inherits="paginaWeb.paginas.historial_lista_chequeo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <div class="row">
                        <div class=" col">
                            <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                                <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                                <TitleStyle BackColor="gray"
                                    ForeColor="White"></TitleStyle>
                                <DayStyle BackColor="gray"></DayStyle>
                                <SelectedDayStyle BackColor="LightGray"
                                    Font-Bold="True"></SelectedDayStyle>
                            </asp:Calendar>
                            <asp:DropDownList ID="dropdown_turno" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Turno 1" />
                                <asp:ListItem Text="Turno 2" />
                            </asp:DropDownList>
                        </div>
                        <div class=" col">
                            <h2>
                                <asp:Label ID="label_fecha" Text="text" runat="server" />
                            </h2>
                        </div>
                    </div>
                    <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                            <asp:BoundField HeaderText="Apellido" DataField="apellido" />
                            <asp:TemplateField HeaderText="Historial">
                                <ItemTemplate>
                                    <asp:Button ID="boton_historial" CssClass="btn btn-primary" Text="Ver Detalle" runat="server" OnClick="boton_historial_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <h4>
                        <asp:Label ID="label_empleado" Visible="false" Text="Empleado" runat="server" />
                    </h4>
                    <h4>
                        <asp:Label ID="label_fecha_historial" Visible="false" Text="Empleado" runat="server" />
                    </h4>
                    <div class="row">
                        <div class="col">
                            <asp:Button ID="boton_encargado" Visible="false" CssClass="btn btn-primary" Text="Encargado" runat="server" OnClick="boton_encargado_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_cajero" Visible="false" CssClass="btn btn-primary" Text="Cajero" runat="server" OnClick="boton_cajero_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_shawarmero" Visible="false" CssClass="btn btn-primary" Text="Shawarmero" runat="server" OnClick="boton_shawarmero_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_atencion" Visible="false" CssClass="btn btn-primary" Text="Atencion al Cliente" runat="server" OnClick="boton_atencion_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_cocina" Visible="false" CssClass="btn btn-primary" Text="Cocina" runat="server" OnClick="boton_cocina_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_limpieza" Visible="false" CssClass="btn btn-primary" Text="Limpieza" runat="server" OnClick="boton_limpieza_Click" />
                        </div>
                    </div>

                    <div class="input-group">
                        <asp:Label ID="label_area" CssClass="form-control" Visible="false" Text="Area" runat="server" />
                        <asp:DropDownList ID="dropDown_tipo" Visible="false" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="input-group">
                        <asp:Label ID="label_categoria" CssClass="form-control" Visible="false" Text="Categoria" runat="server" />
                        <asp:DropDownList ID="dropDown_categoria" Visible="false" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_categoria_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    
                        <asp:Label ID="label_alerta_registro" CssClass="alert alert-danger" Visible="false" Text="No hay registros" runat="server" />
                    
                    <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Actividad" DataField="actividad" />
                            <asp:BoundField HeaderText="Nota" DataField="nota" />
                            <asp:BoundField HeaderText="Fecha" DataField="fecha" />


                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
