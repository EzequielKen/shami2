<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_orden_de_compra.aspx.cs" Inherits="paginaWeb.paginasFabrica.crear_orden_de_compra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        <asp:Label ID="label_titulo" Text="text" runat="server" />
    </h1>

    <div class="container">
        <h2>Crear Orden de Compra</h2>

        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">

                    <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>

                    <TitleStyle BackColor="gray"
                        ForeColor="White"></TitleStyle>

                    <DayStyle BackColor="gray"></DayStyle>

                    <SelectedDayStyle BackColor="LightGray"
                        Font-Bold="True"></SelectedDayStyle>

                </asp:Calendar>

            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                <h3>
                    <asp:Label Text="Seleccione una fecha de entrega estimada." runat="server" />
                </h3>
                <h3>
                    <asp:Label Text="Fecha de entrega seleccionada:" runat="server" />
                </h3>
                <h3>
                    <asp:Label ID="label_fecha" Text="N/A " runat="server" />
                </h3>


            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">


                <div class="row alert alert-light">
                    <asp:Button Text="Crear orden de compra" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click" />
                    <asp:Label ID="label_total" Text="text" CssClass=" form-control" runat="server" />
                    <asp:Button Text="PDF" ID="boton_pdf" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_pdf_Click" />
                </div>
                <asp:Label CssClass="alert alert-danger " ID="label_mensaje_de_alerta" Text="Falta seleccionar una fecha." runat="server" Visible="false" />



            </div>
        </div>
    </div>




    <div class=" container">

        <hr />
    </div>

    <div class=" container-fluid">
        <div class="row">
            <div class="col">
                <asp:GridView ID="gridView_resumen_pedido" runat="server" Caption="RESUMEN DE ORDEN DE PEDIDO" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                        <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>

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




                <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" OnSelectedIndexChanged="gridview_productos_SelectedIndexChanged" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="producto" DataField="producto" />
                        <asp:BoundField HeaderText="precio" DataField="precio" />

                        <asp:TemplateField HeaderText="cantidad a pedir">
                            <ItemTemplate>
                                <asp:TextBox ID="texbox_cantidad" CssClass="input-group-text" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="unidad de medida" DataField="presentacion" />
                        <asp:TemplateField HeaderText="Nuevo precio">
                            <ItemTemplate>
                                <asp:TextBox ID="texbox_nuevo_precio" CssClass="input-group-text" runat="server"></asp:TextBox>
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
                        <asp:BoundField HeaderText="Producto" DataField="producto" />
                        <asp:BoundField HeaderText="Cantidad" DataField="cantidad" />
                        <asp:BoundField HeaderText="Unidad de medida" DataField="unidad de medida" />
                        <asp:BoundField HeaderText="Precio" DataField="precio" />
                        <asp:BoundField HeaderText="Sub Total" DataField="sub_total" />
                        <asp:CheckBoxField Visible="false" HeaderText="bonificable" DataField="bonificable" />
                        <asp:CommandField ShowSelectButton="true" SelectText="eliminar" HeaderText="eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                    </Columns>
                </asp:GridView>


            </div>
        </div>


    </div>




</asp:Content>
