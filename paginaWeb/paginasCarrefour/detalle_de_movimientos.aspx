<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="detalle_de_movimientos.aspx.cs" Inherits="paginaWeb.paginasCarrefour.detalle_de_movimientos" %>

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
                        <asp:GridView ID="gridView_resumen" runat="server" Caption="RESUMEN" CaptionAlign="Top" OnRowDataBound="gridView_resumen_RowDataBound" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                            <Columns>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
