<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_producto_terminado.aspx.cs" Inherits="paginaWeb.paginasFabrica.crear_producto_terminado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <script type="text/javascript">
        function mostrarModal() {
            alert("Producto Creado Exitosamente.");
            $('#ticketModal').modal('show');
        }
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <h1>Crear Producto</h1>

                    <div class="alert alert-light">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <div class="input-group">
                                    <label class="col-form-label">Producto</label>
                                    <asp:TextBox ID="texbox_producto_nuevo" CssClass="form-control" placeholder="Ingrese nombre de producto..." runat="server" AutoPostBack="true">
                                    </asp:TextBox>
                                    <label class="col-form-label">Tipo Producto</label>
                                    <asp:DropDownList ID="dropdown_tipo_producto_nuevo" CssClass="form-select" runat="server">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="textbox_nuevo_tipo" CssClass="form-control" placeholder="Nuevo tipo..." runat="server" />
                                </div>
                                <label class="col-form-label">Categoria Producto</label>
                                <asp:DropDownList ID="dropdown_categoria_producto_nuevo" CssClass="form-select" runat="server">
                                    <asp:ListItem Text="Alimento" />
                                    <asp:ListItem Text="Bebida" />
                                    <asp:ListItem Text="Descartable" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="alert alert-light">

                            <div class="input-group">
                                <label class="col-form-label">Venta</label>
                                <asp:DropDownList ID="dropdown_venta_nuevo" CssClass="form-select" runat="server">
                                    <asp:ListItem Text="Si" />
                                    <asp:ListItem Text="No" />
                                </asp:DropDownList>
                                <label class="col-form-label">Es Pincho</label>
                                <asp:DropDownList ID="dropdown_pincho_nuevo" CssClass="form-select" runat="server">
                                    <asp:ListItem Text="Si" />
                                    <asp:ListItem Text="No" />
                                </asp:DropDownList>

                            </div>
                        </div>
                        <div class="alert alert-light">

                            <label class="col-form-label">Presentacion</label>
                            <asp:TextBox ID="textbox_presentacion_nuevo" CssClass="form-control" runat="server" />

                        </div>
                        <asp:Button ID="boton_cargar" Text="Cargar Producto" CssClass="btn btn-primary" runat="server" OnClick="boton_cargar_Click" />
                    </div>
                </div>
                <hr />
                <div class="row">

                    <h1>Editar Insumos</h1>

                    <div class="alert alert-light">
                        <div class="input-group">
                            <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="buscar..." OnTextChanged="textbox_buscar_TextChanged" runat="server" AutoPostBack="true" />
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
                                        <asp:DropDownList ID="dropdown_activo" CssClass="form-select" OnSelectedIndexChanged="dropdown_activo_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Text="Si" />
                                            <asp:ListItem Text="No" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Producto">
                                    <ItemTemplate>
                                        <label>Producto</label>
                                        <asp:TextBox ID="texbox_producto" CssClass="input-group-text" runat="server" OnTextChanged="texbox_producto_TextChanged" AutoPostBack="true">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tipo Producto">
                                    <ItemTemplate>
                                        <label>Tipo Producto</label>
                                        <asp:DropDownList ID="dropdown_tipo_producto" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_tipo_producto_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Categoria Producto">
                                    <ItemTemplate>
                                        <label>Categoria Producto</label>
                                        <asp:DropDownList ID="dropdown_categoria_producto" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_categoria_producto_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Alimento" />
                                            <asp:ListItem Text="Bebida" />
                                            <asp:ListItem Text="Descartable" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Venta">
                                    <ItemTemplate>
                                        <label>Venta</label>
                                        <asp:DropDownList ID="dropdown_venta" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_venta_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Si" />
                                            <asp:ListItem Text="No" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Peresentacion venta">
                                    <ItemTemplate>
                                        <label>Presentacion Venta</label>
                                        <asp:TextBox ID="textbox_unidad" CssClass="form-control" runat="server" OnTextChanged="textbox_unidad_TextChanged" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Pincho">
                                    <ItemTemplate>
                                        <label>Es Pincho</label>
                                        <asp:DropDownList ID="dropdown_pincho" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_pincho_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Si" />
                                            <asp:ListItem Text="No" />
                                        </asp:DropDownList>
                                        <asp:Label ID="label_equivalencia_pincho" Text="Equivalencia Pincho" runat="server" />
                                        <asp:TextBox ID="textbox_equivalencia_pincho" TextMode="Number" CssClass="form-control" runat="server" OnTextChanged="textbox_equivalencia_pincho_TextChanged" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>

                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
