<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="tabla_produccion.aspx.cs" Inherits="paginaWeb.paginas.tabla_produccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <div class="row">
            <h2>
                <label>Seleccione Turno:</label>
            </h2>
            <asp:DropDownList ID="dropdown_turno" CssClass="form-control" OnSelectedIndexChanged="dropdown_turno_SelectedIndexChanged" AutoPostBack="true" runat="server">
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
                    <asp:BoundField HeaderText="id Empleado" DataField="producto" />
                    <asp:TemplateField HeaderText="Ingrese Stock">
                        <ItemTemplate>
                            <asp:TextBox ID="textbox_stock" CssClass="form-control " OnTextChanged="textbox_stock_TextChanged" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Venta del Turno" DataField="ventas" />
                    <asp:BoundField HeaderText="Objetivo de Produccion" DataField="objetivo_produccion" />

                    <asp:TemplateField HeaderText="Produccion">
                        <ItemTemplate>
                            <asp:TextBox ID="textbox_produccion" CssClass="form-control " OnTextChanged="textbox_produccion_TextChanged" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
