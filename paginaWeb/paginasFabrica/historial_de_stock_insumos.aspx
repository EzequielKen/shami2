<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_de_stock_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_de_stock_insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-4col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                                <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_productos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />

                                <asp:TemplateField HeaderText="Nuevo Stock">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_nuevo_stock" placeholder="Cantidad" CssClass="input-group-text" runat="server" OnTextChanged="texbox_nuevo_stock_TextChanged" AutoPostBack="true">
                                        </asp:TextBox>
                                        <asp:TextBox ID="texbox_nota" placeholder="nota" CssClass="input-group-text" runat="server">
                                        </asp:TextBox>
                                        <asp:Button ID="boton_cargar" CssClass="input-group btn btn-primary" Text="Cargar" OnClick="boton_cargar_Click" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Presentacion">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dropdown_presentacion" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                        
                                        <asp:Button ID="boton_historial" CssClass="input-group btn btn-primary" Text="Cargar Historial" OnClick="boton_historial_Click" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-8 col-lg-8 col-xl-8">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <label class="form-control">mes</label>
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="DropDown_mes" OnSelectedIndexChanged="DropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <label class="form-control">año</label>
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="DropDown_año" OnSelectedIndexChanged="DropDown_año_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <asp:GridView Caption="HISTORIAL DE STOCK" CaptionAlign="Top" runat="server" ID="gridview_historial" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                <asp:BoundField HeaderText="Usuario" DataField="rol_usuario" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Tipo Movimiento" DataField="tipo_movimiento" />
                                <asp:BoundField HeaderText="Presentacion" DataField="presentacion" />
                                <asp:BoundField HeaderText="Stock inicial" DataField="stock_inicial" />
                                <asp:BoundField HeaderText="Movimiento" DataField="movimiento" />
                                <asp:BoundField HeaderText="Stock Final" DataField="stock_final" />
                                <asp:BoundField HeaderText="Nota" DataField="nota" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
