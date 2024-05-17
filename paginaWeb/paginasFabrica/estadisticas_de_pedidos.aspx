<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="estadisticas_de_pedidos.aspx.cs" Inherits="paginaWeb.paginasFabrica.estadisticas_de_pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class=" container">
                <div class=" row  ">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">

                        <asp:GridView Caption="LISTA DE SUCURSALES" CaptionAlign="Top" OnSelectedIndexChanged="gridView_sucursales_SelectedIndexChanged" runat="server" ID="gridView_sucursales" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="sucursal" DataField="sucursal" />
                                <asp:CommandField ShowSelectButton="true" SelectText="seleccionar" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="input-group">
                            <asp:DropDownList ID="dropDown_franquicia" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <asp:Button ID="boton_carga" OnClick="boton_carga_Click" CssClass="btn btn-primary" Text="Carga" runat="server" />
                        </div>
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
            <div class="container-fluid">
                <div class="row">
                    <div class="col d-flex justify-content-center">
                        <asp:Calendar ID="calendario_rango_inicial" CssClass="table-bordered" OnSelectionChanged="calendario_rango_inicial_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray" ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray" Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                    <div class="col ">
                        <h2>
                            <asp:Label ID="label_fecha_inicio" Text="Fecha inicio:" runat="server" />
                        </h2>

                        <h2>
                            <asp:Label ID="label_fecha_final" Text="Fecha fin:" runat="server" />
                        </h2>

                        <asp:Button ID="boton_calcular" CssClass="btn btn-primary" Text="Calcular Analisis" OnClick="boton_calcular_Click" runat="server" />
                    </div>
                    <div class="col d-flex justify-content-center">
                        <asp:Calendar ID="calendario_rango_final" CssClass="table-bordered" OnSelectionChanged="calendario_rango_final_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray" ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray" Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col">
                    <div class="input-group">
                        <asp:TextBox ID="textbox_buscar"  CssClass="form-control" AutoPostBack="true" OnTextChanged="textbox_buscar_TextChanged" placeholder="Buscar" runat="server" />
                        <asp:Label Text="Tipo Producto" runat="server" CssClass=" form-control" />
                        <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <asp:GridView ID="gridView_productos" runat="server" Caption="Estadisticas" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="producto" DataField="producto" />
                            <asp:BoundField HeaderText="cantidad pedida" DataField="cantidad_pedida" />
                            <asp:BoundField HeaderText="presentacion" DataField="presentacion" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
