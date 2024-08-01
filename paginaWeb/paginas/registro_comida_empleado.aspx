<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="registro_comida_empleado.aspx.cs" Inherits="paginaWeb.paginas.registro_comida_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <h1>
                            <asp:Label ID="label_nombre_empleado" Text="text" runat="server" />
                        </h1>
                        <div class="alert alert-light">
                            <div class="input-group pb-3">
                                <label>Buscar</label>
                                <asp:TextBox ID="textbox_buscar" placeholder="Buscar..." CssClass="form-control" runat="server" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" />
                                <asp:Button ID="boton_buscar" CssClass="btn btn-primary" Text="Buscar" runat="server" OnClick="boton_buscar_Click" />

                                <label>Tipo Producto</label>
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <asp:Button ID="boton_registrar" CssClass="btn btn-primary" Text="Registrar Consumo" runat="server" OnClick="boton_registrar_Click" />
                            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />

                                    <asp:TemplateField HeaderText="Cargar Consumo de Comida">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_cantidad" Text="1" CssClass="form-control" runat="server" OnTextChanged="textbox_cantidad_TextChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cargar Consumo de Comida">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nota" placeholder="Ingrese nota..." CssClass="form-control" runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cargar Consumo de Comida">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cargar" CssClass="btn btn-primary" Text="Cargar" runat="server" OnClick="boton_cargar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="container">
                    <hr />
                </div>
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <asp:GridView Caption="LISTA DE RESUMEN" CaptionAlign="Top" runat="server" ID="gridview_RESUMEN" AutoGenerateColumns="false" CssClass="table table-dark   table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Cantidad" DataField="cantidad" />
                                    <asp:BoundField HeaderText="Nota" DataField="nota" />

                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_eliminar" CssClass="btn btn-danger" Text="Eliminar" runat="server" OnClick="boton_eliminar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
