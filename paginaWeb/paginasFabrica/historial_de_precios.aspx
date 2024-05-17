<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_de_precios.aspx.cs" Inherits="paginaWeb.paginasFabrica.reporteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                <div class="input-group mb-3">
                    <h2 class="form-control-lg">Año:</h2>
                    <asp:DropDownList ID="dropDown_año" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>

                    <h2 class="form-control-lg">Mes:</h2>
                    <asp:DropDownList ID="dropDown_mes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>

                    <h2 class="form-control-lg">Acuerdo de precio:</h2>
                    <asp:DropDownList ID="dropdown_acuerdo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_acuerdo_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>

                </div>
            </div>
            <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                <div class="input-group mb-3">

                    <asp:Label Text="FECHA DE ACUERDO:" CssClass="form-control-lg" ID="label_fecha" runat="server" />
                    <asp:Button ID="boton_generar_pdf" Text="Generar PDF de acuerdo de precio" CssClass="btn btn-primary" OnClick="boton_generar_pdf_Click" runat="server" />

                </div>

                <div class="input-group mb-3">

                    <asp:Button ID="Button_lista_de_precios" Text="Lista de precio PDF" CssClass="btn btn-primary" OnClick="Button_lista_de_precios_Click" runat="server" />

                </div>

            </div>
        </div>
        <div class="container">
            <div class="row">
                <h1>
                    <asp:Label ID="label_promedio_ganancia" Text="Promedio de ganancia:" runat="server" />
                </h1>
            </div>
        </div>
        <div class="container">
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                    <asp:GridView ID="gridview_productos" Caption="Lista de productos" CaptionAlign="Top" CssClass="table table-dark table-striped" AutoGenerateColumns="false" runat="server">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Productos" DataField="producto" />
                            <asp:BoundField HeaderText="U.M" DataField="unidad_de_medida_fabrica" />
                            <asp:BoundField HeaderText="Precio compra" DataField="precio_compra" />
                            <asp:BoundField HeaderText="Precio venta" DataField="precio_venta" />
                            <asp:BoundField HeaderText="Ganancia en $" DataField="ganancia_pesos" />
                            <asp:BoundField HeaderText="Ganancia en %" DataField="ganancia_porcentaje" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
