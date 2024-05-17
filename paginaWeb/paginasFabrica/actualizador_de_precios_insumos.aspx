<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="actualizador_de_precios_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.actualizador_de_precios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
        <h2>Actualizador de Precios de Insumos</h2>

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

            <div class="container-fluid">
                <hr />
            </div>

            <div class=" container-fluid">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class=" alert alert-light">
                            <div class="input-group mb-3 ">

                                <asp:Label Visible="false" ID="label_cartel" CssClass="form-control" Text="Ingrese porcentaje de aumento:" runat="server" />
                                <asp:TextBox Visible="false" runat="server" CssClass="form-control form-control" ID="textbox_porcentaje_aumento" />


                                <asp:Button Visible="false" Text="cargar" CssClass="btn btn-outline-secondary" runat="server" ID="boton_aumentar_porcentaje" OnClick="boton_aumentar_porcentaje_Click" />

                            </div>
                        </div>

                        <div class=" alert alert-light">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                                <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>





                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" OnRowDataBound="gridview_productos_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:BoundField HeaderText="precio compra" DataField="precio_compra" />
                                <asp:BoundField HeaderText="presentacion compra" DataField="unidad_medida" />
                                <asp:BoundField HeaderText="precio venta actual" DataField="precio_venta_actual" />
                                <asp:BoundField HeaderText="% ganancia actual" DataField="porcentaje_ganancia_actual" />
                                <asp:BoundField HeaderText="porcentaje aumento" DataField="porcentaje_aumento" />
                                <asp:BoundField HeaderText="diferencia" DataField="diferencia" />
                                <asp:BoundField HeaderText="precio nuevo con %" DataField="precio_venta_nuevo" />

                                <asp:TemplateField HeaderText="Ingrese Precio Venta">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_precio" CssClass="input-group-text" runat="server" OnTextChanged="texbox_precio_TextChanged" AutoPostBack="true">
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Precio Venta Personalizado" DataField="precio_venta_personalizado" />
                                <asp:BoundField HeaderText="presentacion venta" DataField="presentacion" />

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
