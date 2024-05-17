<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="linkear_insumos_a_proveedor.aspx.cs" Inherits="paginaWeb.paginasFabrica.linkear_insumos_a_proveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container-fluid">
                <h2>Linkear Insumos a Proveedor</h2>
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <div class="alert alert-light">

                            <label for="basic-url" class="form-label">Datos del proveedor</label>

                            <div class="input-group mb-3">
                                <span class="input-group-text">Nombre</span>
                                <asp:TextBox ID="textbox_nombre" runat="server" CssClass="form-control" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">provincia</span>
                                <asp:TextBox ID="textbox_provincia" runat="server" CssClass="form-control" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">localidad</span>
                                <asp:TextBox ID="textbox_localidad" runat="server" CssClass="form-control" />
                            </div>
                            <div class="input-group mb-3">
                                <span class="input-group-text">direccion</span>
                                <asp:TextBox ID="textbox_direccion" runat="server" CssClass="form-control" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">telefono</span>
                                <asp:TextBox ID="textboc_telefono" runat="server" CssClass="form-control" />
                            </div>


                            <div class="input-group mb-3">
                                <span class="input-group-text">Pago a</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_condicion_pago" runat="server"/>
                                <span class="input-group-text">Dias</span>
                            </div>

                        </div>
                    </div>

                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <label for="basic-url" class="form-label">CBU/ALIAS del proveedor</label>

                            <div class="input-group mb-3">
                                <span class="input-group-text">CBU/ALIAS</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_cbu_1" runat="server" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">CBU/ALIAS</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_cbu_2" runat="server" />
                            </div>
                            <div class="input-group mb-3">
                                <span class="input-group-text">CBU/ALIAS</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_cbu_3" runat="server" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">CBU/ALIAS</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_cbu_4" runat="server" />
                            </div>

                            <div class="input-group mb-3">
                                <span class="input-group-text">CBU/ALIAS</span>
                                <asp:TextBox CssClass="form-control" ID="textbox_cbu_5" runat="server" />
                            </div>
                            <div class="row">
                                <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                </div>
                                <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                </div>
                                <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                    <asp:Button ID="boton_modificar_datos_proveedor" Text="Modificar" CssClass="btn btn-primary" OnClick="boton_modificar_datos_proveedor_Click" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <hr />
                <div class="row">
                    <div class="col-sm-12 col-md-2 col-lg-2 col-xl-2">
                        <div class="alert alert-light">
                            <label for="basic-url" class="form-label">Todos los insumos</label>

                            <div class="input-group mb-3">
                                <asp:DropDownList ID="dropDown_tipo_insumos" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_insumos_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                </asp:DropDownList>
                                <asp:Label Text="Tipo producto" CssClass="form-control" runat="server" />
                            </div>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="texbox_buscar_insumos" runat="server" CssClass="form-control" OnTextChanged="texbox_buscar_insumos_TextChanged" AutoPostBack="true" />
                                <asp:Button Text="Buscar" runat="server" CssClass=" btn btn-primary" />
                            </div>
                        </div>

                        <asp:GridView Caption="LISTA DE INSUMOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_insumos_SelectedIndexChanged" runat="server" ID="gridview_insumos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />

                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-10 col-lg-10 col-xl-10">
                        <div class="alert alert-light">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                        <label for="basic-url" class="form-label">Insumos del proveedor</label>
                                    </div>
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">

                                        <asp:Label CssClass="alert alert-danger " ID="label_mensaje_de_alerta" Text="Faltan configurar datos." runat="server" Visible="false" />

                                    </div>
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-lg-end">
                                        <asp:Button ID="boton_guardar" Text="Guardar" CssClass="btn btn-primary align-content-end" OnClick="boton_guardar_Click" runat="server" />
                                    </div>
                                </div>


                                <div class="input-group mb-3 mt-3">
                                    <asp:DropDownList ID="dropDown_tipo_insumos_proveedor" CssClass="form-control" OnSelectedIndexChanged="dropDown_tipo_insumos_proveedor_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label Text="Tipo producto" CssClass="form-control" runat="server" />
                                </div>
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="textbox_buscar_insumos_proveedor" runat="server" CssClass="form-control" OnTextChanged="textbox_buscar_insumos_proveedor_TextChanged" AutoPostBack="true" />
                                    <asp:Button Text="Buscar" runat="server" CssClass=" btn btn-primary" />
                                </div>

                                <asp:GridView Caption="LISTA INSUMOS DEL PROVEEDOR" CaptionAlign="Top" runat="server" ID="gridview_insumos_del_proveedor" OnRowCommand="gridview_insumos_del_proveedor_RowCommand" OnRowDataBound="gridview_insumos_del_proveedor_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="Producto" DataField="Producto" />
                                        <asp:BoundField HeaderText="Tipo producto" DataField="tipo_producto" />
                                        <asp:TemplateField HeaderText="Seleccione tipo paquete">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="dropdown_tipo_paquete" runat="server" OnSelectedIndexChanged="dropdown_tipo_paquete_SelectedIndexChanged">
                                                    <asp:ListItem Text="N/A" />
                                                    <asp:ListItem Text="Unidad" />
                                                    <asp:ListItem Text="Pack" />
                                                    <asp:ListItem Text="Caja" />
                                                    <asp:ListItem Text="Bulto" />
                                                    <asp:ListItem Text="Bidon" />
                                                    <asp:ListItem Text="Balde" />
                                                    <asp:ListItem Text="Rollo" />
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ingrese cantidad de unidades">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_cantidad_unidades" CssClass="input-group-text" runat="server">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Unidad Medida" DataField="unidad_medida" />

                                        <asp:BoundField HeaderText="presentacion recomendada" DataField="producto_1" />

                                        <asp:BoundField HeaderText="Presentacion seleccionada" DataField="presentacion" />
                                        <asp:TemplateField HeaderText="Ingrese precio">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textbox_precio" CssClass="input-group-text" runat="server">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Precio" DataField="precio" />
                                        <asp:BoundField HeaderText="Precio unidad" DataField="precio_unidad" />
                                        <asp:ButtonField HeaderText="Confirmar" Text="Aplicar cambios" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_aplicar_cambios" />
                                        <asp:ButtonField HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_eliminar" />
                                    </Columns>
                                </asp:GridView>
                            </div>



                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
