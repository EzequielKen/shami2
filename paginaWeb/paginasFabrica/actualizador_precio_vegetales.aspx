<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="actualizador_precio_vegetales.aspx.cs" Inherits="paginaWeb.paginasFabrica.actualizador_precio_vegetales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>




            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">


                        <div class="row alert alert-light">
                            <asp:Button Text="enviar" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click" />
                        </div>



                    </div>
                </div>
            </div>



            <div class=" container">
                <hr />
            </div>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class=" alert alert-light">
                            <div class="row">
                                <div class="container">
                                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="textbox_porcentaje_de_aumento" OnTextChanged="textbox_porcentaje_de_aumento_TextChanged" AutoPostBack="true" />
                                            <asp:Button Text="Cargar" CssClass="btn btn-outline-secondary" runat="server" />
                                            <asp:Label Text="porcentaje de aumento:" ID="label_porcentaje" CssClass="form-control" runat="server" />

                                        </div>
                                        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                                            <div class="input-group">
                                                <h2>
                                                    <asp:Label Text="Total compra: " ID="label_total_compra" CssClass="form-control" runat="server" />
                                                </h2>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="container">


                                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                                        <asp:Button ID="boton_filtro" Text="Filtrar comprados" runat="server" CssClass=" btn btn-primary" OnClick="boton_filtro_Click" />
                                        <asp:Button ID="mostrar_todo" Text="Mostrar todos" runat="server" CssClass=" btn btn-primary" OnClick="mostrar_todo_Click" />
                                        <h1>
                                            <asp:Label ID="label_promedio" Text="Promedio de ganancia:" runat="server" />
                                        </h1>
                                        <div class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                                            <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                                            <div>
                                                <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class=" container-fluid">


                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <asp:Label Text="" runat="server" CssClass="form-control " ID="label_productoSelecionado" />



                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Unidad de medida" DataField="unidad_de_medida" />

                                <asp:BoundField HeaderText="Stock actual" DataField="stock" />

                                <asp:TemplateField HeaderText="Ingrese cantidad comprada">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_stock_nuevo" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Stock nuevo" DataField="stock_nuevo" />

                                <asp:TemplateField HeaderText="Ingrese precio de compra">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_precio_compra" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Precio compra" DataField="precio_compra" />
                                <asp:BoundField HeaderText="Sub total compra" DataField="sub_total_compra" />
                                <asp:BoundField HeaderText="Nuevo precio venta" DataField="precio_nuevo" />
                                <asp:BoundField HeaderText="Precio venta anterior" DataField="precio" />
                                <asp:BoundField HeaderText="Ganancia en $" DataField="diferencia" />
                                <asp:BoundField HeaderText="Ganancia en %" DataField="porcentaje_aumento" />

                                <asp:TemplateField HeaderText="Ingrese precio redondeado">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_precio_redondeado" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Ganancia final en $" DataField="diferencia_final" />
                                <asp:BoundField HeaderText="Ganancia final en %" DataField="porcentaje_final" />
                                <asp:BoundField HeaderText="Precio venta final" DataField="precio_final" />


                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
