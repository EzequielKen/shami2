<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="analisis_movimientos.aspx.cs" Inherits="paginaWeb.paginasCarrefour.analisis_movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <div class="col">
                                    <asp:Label Text="Sucursal" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_sucursales" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_sucursales_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Label Text="Mes" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_mes" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Label Text="Año" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_año" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col"></div>
                </div>

                <div class="row">
                    <div class="alert alert-light">
                        <asp:GridView ID="gridView_resumen" runat="server" Caption="RESUMEN" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Total Devoluciones" DataField="devolucion" />
                                <asp:BoundField HeaderText="Porcentaje Devoluciones vs Vendido" DataField="porcentaje_devolucion" />
                                <asp:BoundField HeaderText="Total Vendido" DataField="vendido" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
