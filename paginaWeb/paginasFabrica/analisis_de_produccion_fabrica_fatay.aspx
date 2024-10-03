<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="analisis_de_produccion_fabrica_fatay.aspx.cs" Inherits="paginaWeb.paginasFabrica.analisis_de_produccion_fabrica_fatay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col">
                    <div class="input-group">
                        <asp:Label Text="Tipo Producto" runat="server" CssClass=" form-control" />
                        <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <h4>
                        <asp:Label ID="label_fecha_mes_pasado" Text="Fechas mes pasado: " runat="server" />
                    </h4>
                    <h4>
                        <asp:Label ID="label_fecha_produccion" Text="Fechas mes pasado: " runat="server" />
                    </h4>
                    <asp:GridView ID="gridView_productos" runat="server" Caption="Estadisticas" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="producto" DataField="producto" />
                            <asp:BoundField HeaderText="cantidad pedida mes pasado" DataField="cantidad_pedida" />
                            <asp:BoundField HeaderText="20% incremento" DataField="incremento" />
                            <asp:BoundField HeaderText="objetivo semanal" DataField="objetivo" />
                            <asp:BoundField HeaderText="produccion semanal" DataField="produccion_semanal" />
                            <asp:BoundField HeaderText="produccion pendiente" DataField="produccion_pendiente" />
                            <asp:BoundField HeaderText="stock de fabrica" DataField="stock" />
                            <asp:BoundField HeaderText="presentacion" DataField="presentacion" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
