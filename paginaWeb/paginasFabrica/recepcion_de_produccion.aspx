<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="recepcion_de_produccion.aspx.cs" Inherits="paginaWeb.paginasFabrica.importar_produccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class=" container">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <asp:Label Text="" runat="server" CssClass="form-control " ID="label_productoSelecionado" />


                        <div class="input-group mb-3 ">
                        </div>

                      
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_produccion_SelectedIndexChanged" OnRowDataBound="gridview_produccion_RowDataBound" runat="server" ID="gridview_produccion" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Tipo_producto" DataField="Tipo Producto" />
                                <asp:BoundField HeaderText="Producto" DataField="Producto" />
                                <asp:BoundField HeaderText="Cantidad despachada" DataField="Cant.Entregada" />
                                <asp:BoundField HeaderText="Unidad de medida" DataField="Unid.Medida" />
                                <asp:TemplateField HeaderText="Ingrese cantidad recibida">
                                    <ItemTemplate>
                                        <asp:TextBox ID="textbox_cantidad" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                                <asp:BoundField HeaderText="Cantidad recibida" DataField="Cant.Recibida" />
                                <asp:BoundField HeaderText="Stock" DataField="stock" />
                                <asp:BoundField HeaderText="Stock nuevo" DataField="nuevo_stock" />

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
            <div class=" container">
                <hr />
            </div>
            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">


                        <div class="row alert alert-light">
                            <asp:Label Visible="false" Text="" ID="label_cantidad_bonificable" runat="server" />
                            <asp:Label Visible="false" Text="asfa" ID="label_total_pedido" runat="server" />
                            <asp:Button Text="enviar" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click1" />
                        </div>



                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
