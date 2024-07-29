<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="merma_y_desperdicio.aspx.cs" Inherits="paginaWeb.paginasFabrica.merma_y_desperdicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>


            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <a href="/paginasFabrica/historial_merma_y_desperdicio.aspx" class="btn btn-primary">Historial de merma y desperdicio</a>
                                <div class="row">

                                    <div class="col">
                                        <asp:Label Text="Tipo Producto" CssClass=" form-label" runat="server" />
                                        <asp:DropDownList ID="dropDown_tipo" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Buscar Producto" CssClass=" form-label" runat="server" />
                                        <div class=" input-group">
                                            <asp:TextBox ID="textbox_busqueda" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="textbox_busqueda_TextChanged" />
                                            <asp:Button Text="Buscar" runat="server" CssClass="form-control btn btn-primary" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Carga de perdidida o merma" CssClass=" form-label" runat="server" />

                                        <asp:Button ID="boton_carga_desperdicio" Text="Cargar Desperdicio" runat="server" CssClass="form-control btn btn-primary" OnClick="boton_carga_desperdicio_Click" />
                                    </div>
                                </div>
                                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" ID="gridView_productos" OnRowCommand="gridView_productos_RowCommand" OnRowDataBound="gridView_productos_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="ID" DataField="id" />
                                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                                        <asp:TemplateField HeaderText="Ingrese Unidades Perdidas">
                                            <ItemTemplate>
                                                <asp:TextBox ID="texbox_cantidad" CssClass="input-group-text" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Seleccione Unidad">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="dropdown_unidad" runat="server" OnSelectedIndexChanged="dropdown_unidad_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="N/A" />
                                                    <asp:ListItem Text="Kg" />
                                                    <asp:ListItem Text="Lts" />
                                                    <asp:ListItem Text="unid." />
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField HeaderText="Desperdicio" Text="Confirmar Desperdicio" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_en_desperdicio" />

                                        <asp:BoundField HeaderText="Cant. desperdicio" DataField="presentacion" />

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="container">
                <hr />
            </div>

            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <div class="row">

                                    <div class="col">
                                        <asp:Label Text="Tipo Insumo" CssClass=" form-label" runat="server" />
                                        <asp:DropDownList ID="DropDown_tipo_insumo" runat="server" CssClass="form-select" OnSelectedIndexChanged="DropDown_tipo_insumo_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Buscar Insumo" CssClass=" form-label" runat="server" />
                                        <div class=" input-group">
                                            <asp:TextBox ID="textbox_buscar_insumo" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="textbox_buscar_insumo_TextChanged" />
                                            <asp:Button Text="Buscar" runat="server" CssClass="form-control btn btn-primary" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <asp:Label Text="Cargar Merma" CssClass=" form-label" runat="server" />

                                        <asp:Button ID="boton_carga_merma" Text="Cargar Merma" runat="server" CssClass="form-control btn btn-primary" OnClick="boton_carga_merma_Click" />
                                    </div>
                                </div>
                                <asp:GridView Caption="LISTA DE INSUMOS" CaptionAlign="Top" ID="gridView_insumos" OnRowCommand="gridView_insumos_RowCommand" OnRowDataBound="gridView_insumos_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                    <Columns>
                                        <asp:BoundField HeaderText="ID" DataField="id" />
                                        <asp:BoundField HeaderText="Insumo" DataField="producto" />
                                        <asp:TemplateField HeaderText="Ingrese Unidades Perdidas">
                                            <ItemTemplate>
                                                <asp:TextBox ID="texbox_cantidad_insumo" CssClass="input-group-text" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Seleccione Unidad">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="dropdown_unidad_insumo" runat="server" OnSelectedIndexChanged="dropdown_unidad_insumo_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="N/A" />
                                                    <asp:ListItem Text="Kg" />
                                                    <asp:ListItem Text="Lts" />
                                                    <asp:ListItem Text="unid." />
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField HeaderText="Merma" Text="Confirmar Merma" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_en_merma" />

                                        <asp:BoundField HeaderText="Cant. Merma" DataField="presentacion" />

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
