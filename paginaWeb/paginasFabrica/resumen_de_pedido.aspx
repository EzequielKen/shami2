<%@ Page Async="true"  Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="resumen_de_pedido.aspx.cs" Inherits="paginaWeb.paginasFabrica.resumen_de_pedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <h2>Produccion</h2>
                            <div class="row alert alert-light">
                                <asp:Button Text="Resumir produccion diaria segun sucursal" ID="boton_resumir_produccion_diaria_segun_sucursal" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_resumir_produccion_diaria_segun_sucursal_Click" />
                            </div>
                            <div class="row alert alert-light">
                                <asp:Button Text="Resumir toda produccion diaria" ID="boton_resumir_produccion_diaria" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_resumir_produccion_diaria_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <h2>Panificados</h2>
                            <div class="row alert alert-light">
                                <asp:Button Text="Resumen de Pedido Sin Stock" ID="Boton_panificados" runat="server" CssClass="btn btn-primary btn-sm" OnClick="Boton_panificados_Click" />
                            </div>
                            <div class="row alert alert-light">
                                <asp:Button Text="Pedido a Saleh Con Stock Incluido" ID="Boton_pedir_panificados" runat="server" CssClass="btn btn-primary btn-sm" OnClick="Boton_pedir_panificados_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="alert alert-light">
                            <h2>Expedicion</h2>
                            <div class="row alert alert-light">
                                <asp:Button Text="Generar PDF resumen segun sucursal" ID="boton_crear_pdf" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_crear_pdf_Click" />
                            </div>
                            <div class="row alert alert-light">
                                <asp:Button Text="Generar PDF resumen todas las sucursales"
                                    ID="boton_resumir_todo" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_resumir_todo_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class=" container">
                <hr />
            </div>

            <div class="container">
                <div class=" alert alert-light">
                    <div class="row">

                        <div class="col text-center">
                            <asp:Button Id="boton_lunes" CssClass="btn btn-primary" Text="Lunes" OnClick="boton_lunes_Click" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Button Id="boton_martes" CssClass="btn btn-primary" Text="Martes" OnClick="boton_martes_Click" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Button Id="boton_miercoles" CssClass="btn btn-primary" Text="Miercoles" OnClick="boton_miercoles_Click" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Button Id="boton_jueves" CssClass="btn btn-primary" Text="Jueves" OnClick="boton_jueves_Click" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Button Id="boton_viernes" CssClass="btn btn-primary" Text="Viernes" OnClick="boton_viernes_Click" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Button Id="boton_sabado" CssClass="btn btn-primary" Text="Sabado" OnClick="boton_sabado_Click" runat="server" />
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
                            <asp:TextBox Visible="false" runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                            <asp:Button Visible="false" Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />


                        </div>




                        <asp:GridView Caption="LISTA DE SUCURSALES" CaptionAlign="Top" OnSelectedIndexChanged="gridview_sucursales_SelectedIndexChanged" runat="server" ID="gridView_sucursales" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                <asp:CommandField ShowSelectButton="true" SelectText="seleccionar" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />

                            </Columns>
                        </asp:GridView>

                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <asp:GridView Caption="SUCURSALES SELECCIONADAS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_resumen_SelectedIndexChanged" runat="server" ID="gridview_resumen" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                <asp:CommandField ShowSelectButton="true" SelectText="eliminar" HeaderText="eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                            </Columns>
                        </asp:GridView>


                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
