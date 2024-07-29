<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="cargar_pedido.aspx.cs" Inherits="paginaWeb.paginasFabrica.cargar_pedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>



            <!-- Modal -->
            <div class="modal fade" id="spinnerModal" tabindex="-1" aria-labelledby="spinnerModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="spinnerModalLabel">Ingresando al sistema... espere...</h5>
                        </div>
                        <div class="modal-body text-center">
                            <div class="spinner-border" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class=" container">

                <div class=" row  ">

                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">


                        <div class="input-group mb-3 ">
                            <h1>
                                <asp:Label Text="" runat="server" CssClass="form-control " ID="label_titulo" />
                            </h1>


                        </div>
                        <div class="row alert alert-light">
                            <asp:Label Visible="false" Text="asfa" ID="label_total_pedido" runat="server" />

                        </div>

                        <div class=" alert alert-light">
                            <asp:TextBox ID="textbox_porcentaje" CssClass="form-control" placeholder="Ingrese porcentaje de aumento" runat="server" OnTextChanged="textbox_porcentaje_TextChanged"  AutoPostBack="true"/>
                        </div>
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnRowDataBound="gridview_pedido_RowDataBound" runat="server" ID="gridview_pedido" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:BoundField HeaderText="cantidad pedida" DataField="cantidad_pedida" />

                                <asp:TemplateField HeaderText="Presentacion Pedida">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dropdown_tipo_presentacion"  CssClass="input-group-text" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ingrese cantidad a entregar">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_cantidad" CssClass="input-group-text" OnTextChanged="texbox_cantidad_TextChanged" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'></asp:TextBox>
                                        <asp:TextBox ID="textbox_unidades" CssClass="input-group-text" placeholder="Cantidad de Pinchos" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Presentacion Entrega">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dropdown_presentacion_entrega" OnSelectedIndexChanged="dropdown_presentacion_entrega_SelectedIndexChanged" CssClass="input-group-text" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Extraido de">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dropdown_extraido_de" OnSelectedIndexChanged="dropdown_extraido_de_SelectedIndexChanged" CssClass="input-group-text" runat="server" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="cantidad a entregar" DataField="cantidad_entrega" />
                                <asp:BoundField HeaderText="precio" DataField="precio" />
                                <asp:BoundField HeaderText="sub total" DataField="sub_total" />
                                <asp:BoundField HeaderText="id pedido" DataField="id_pedido" />




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

                        <div class="row alert alert-light">
                            <asp:Button Text="Cancelar" ID="boton_cancelar" runat="server" CssClass="btn btn-primary btn-danger btn-sm" OnClick="boton_cancelar_Click" />
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="row alert alert-light">
                            <asp:Button Text="Carga parcial" ID="boton_carga_parcial" runat="server" CssClass="btn btn-primary btn-warning btn-sm" OnClick="boton_carga_parcial_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal" />
                        </div>

                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">



                        <div class="row alert alert-light">
                            <asp:Button Text="Facturar" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal" />
                        </div>



                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
