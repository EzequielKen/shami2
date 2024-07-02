<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="tabla_produccion.aspx.cs" Inherits="paginaWeb.paginas.tabla_produccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <div class="row">
            <h2>
                <label>Seleccione Turno:</label>
            </h2>
            <asp:DropDownList ID="dropdown_turno" CssClass="form-control bg-danger" OnSelectedIndexChanged="dropdown_turno_SelectedIndexChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Text="Turno 1" />
                <asp:ListItem Text="Turno 2" />
            </asp:DropDownList>
            <h2>
                <label>Seleccione Categoria de Producto:</label>
            </h2>
            <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true" runat="server">
            </asp:DropDownList>
            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_productos_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="id" DataField="id" />
                    <asp:BoundField HeaderText="id historial" DataField="id_historial" />
                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                    <asp:BoundField HeaderText="Historico Vta. Dia Semana Anterior" DataField="ventas" />
                    <asp:TemplateField HeaderText="Ingrese Stock Actual">
                        <ItemTemplate>
                            <asp:TextBox ID="textbox_stock" placeholder="N/A" CssClass="form-control " OnTextChanged="textbox_stock_TextChanged" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Objetivo de Produccion del Turno" DataField="objetivo_produccion" />

                    <asp:TemplateField HeaderText="Produccion del Turno">
                        <ItemTemplate>
                            <asp:TextBox ID="textbox_produccion"  placeholder="N/A" CssClass="form-control " OnTextChanged="textbox_produccion_TextChanged" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Nuevo Registro" OnClick="boton_cargar_Click" runat="server" />
        </div>
    </div>
</asp:Content>
