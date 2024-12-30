<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_producto_terminado.aspx.cs" Inherits="paginaWeb.paginasFabrica.crear_producto_terminado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">

            <h1>Editar Insumos</h1>

            <div class="alert alert-light">
                <div class="input-group">
                    <label class="col-form-label">Tipo Producto:</label>
                    <asp:DropDownList ID="dropDown_tipo" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="alert alert-light">
                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" OnRowDataBound="gridview_productos_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                    <Columns>

                        <asp:BoundField HeaderText="id" DataField="id" />

                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <label>activo</label>
                                <asp:DropDownList ID="dropdown_activo" CssClass="form-select" AutoPostBack="true" runat="server">
                                    <asp:ListItem Text="Si" />
                                    <asp:ListItem Text="No" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Producto">
                            <ItemTemplate>
                                <label>Producto</label>
                                <asp:TextBox ID="texbox_producto" CssClass="input-group-text" runat="server"  AutoPostBack="true">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tipo Producto">
                            <ItemTemplate>
                                <label>Tipo Producto</label>
                                <asp:DropDownList ID="dropdown_tipo_producto" CssClass="form-select" runat="server"  AutoPostBack="true">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Categoria Producto">
                            <ItemTemplate>
                                <label>Categoria Producto</label>
                                <asp:DropDownList ID="dropdown_categoria_producto" CssClass="form-select" runat="server"  AutoPostBack="true">
                                    <asp:ListItem Text="Alimento" />
                                    <asp:ListItem Text="Bebida" />
                                    <asp:ListItem Text="Descartable" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Venta">
                            <ItemTemplate>
                                <label>Venta</label>
                                <asp:DropDownList ID="dropdown_venta" CssClass="form-select" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="Si" />
                                    <asp:ListItem Text="No" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Peresentacion venta">
                            <ItemTemplate>
                                <label>Paquete</label>
                                <asp:DropDownList ID="dropdown_paquete" CssClass="form-select" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="Unidad" />
                                    <asp:ListItem Text="Bidon" />
                                    <asp:ListItem Text="Caja" />
                                    <asp:ListItem Text="Bulto" />
                                    <asp:ListItem Text="Pack" />
                                </asp:DropDownList>
                                <label>Unidad</label>
                                <asp:TextBox ID="textbox_unidad" TextMode="Number" CssClass="form-control" runat="server" AutoPostBack="true" />
                                <label>Tipo Unidad</label>
                                <asp:DropDownList ID="dropdown_tipo_unidad" CssClass="form-select" runat="server" AutoPostBack="true">
                                    <asp:ListItem Text="unid." />
                                    <asp:ListItem Text="Lts" />
                                    <asp:ListItem Text="Kg" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

            </div>
        </div>
        
    </div>
</asp:Content>
