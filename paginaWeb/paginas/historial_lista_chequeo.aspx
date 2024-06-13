<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_lista_chequeo.aspx.cs" Inherits="paginaWeb.paginas.historial_lista_chequeo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                            <asp:BoundField HeaderText="Apellido" DataField="apellido" />
                            <asp:TemplateField HeaderText="Cargo">
                                <ItemTemplate>
                                    <asp:DropDownList ID="dropdown_cargo" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
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
