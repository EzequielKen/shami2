<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="orden_de_pedido_fabrica_fatay.aspx.cs" Inherits="paginaWeb.paginasFabricaFatay.orden_de_pedido_fabrica_fatay" %>

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

            <div class=" container">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <asp:Label Text="" runat="server" CssClass="form-control " ID="label_productoSelecionado" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                            <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                            <div>
                                <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>

                        </div>




                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" OnRowDataBound="gridview_productos_RowDataBound" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:TemplateField HeaderText="cantidad a pedir">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_cantidad" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Seleccione tipo paquete">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dropdown_tipo_paquete" runat="server" OnSelectedIndexChanged="dropdown_tipo_paquete_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="N/A" />
                                            <asp:ListItem Text="Kg" />
                                            <asp:ListItem Text="Lts" />
                                            <asp:ListItem Text="Unidad" />
                                            <asp:ListItem Text="Caja" />
                                            <asp:ListItem Text="Bulto" />
                                            <asp:ListItem Text="Bidon" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <asp:GridView Caption="RESUMEN PEDIDOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_resumen_SelectedIndexChanged" runat="server" ID="gridview_resumen" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:BoundField HeaderText="cantidad" DataField="cantidad" />
                                <asp:BoundField HeaderText="unidad de medida" DataField="unidad_medida" />
                                <asp:CommandField ShowSelectButton="true" SelectText="eliminar" HeaderText="eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                            </Columns>
                        </asp:GridView>


                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
