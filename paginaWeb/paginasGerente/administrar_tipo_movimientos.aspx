<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="administrar_tipo_movimientos.aspx.cs" Inherits="paginaWeb.paginasGerente.administrar_tipo_movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="col"></div>
            <div class="col">
                <div class="alert alert-light">
                    <h5>
                        <asp:Label Text="Tipo movimiento" runat="server" />
                    </h5>
                    <asp:DropDownList CssClass=" form-control-sm" runat="server">
                        <asp:ListItem Text="Ingreso" />
                        <asp:ListItem Text="Egreso" />
                    </asp:DropDownList>
                    <asp:GridView ID="gridView_tipo_movimientos" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Tipo Movimiento" DataField="tipo_movimiento" />
                            <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col"></div>
        </div>
    </div>
</asp:Content>
