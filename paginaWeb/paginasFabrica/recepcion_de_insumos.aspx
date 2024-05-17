<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="recepcion_de_insumos.aspx.cs" Inherits="paginaWeb.paginasFabrica.recepcion_de_insumos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="alert alert-light">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-lg-end">
                            <asp:Button ID="boton_guardar" Text="Guardar" CssClass="btn btn-primary align-content-end" OnClick="boton_guardar_Click" runat="server" />
                        </div>
                    </div>
                    <asp:GridView Caption="LISTA INSUMOS DEL PROVEEDOR" CaptionAlign="Top" runat="server" ID="gridview_insumos_del_proveedor" OnRowCommand="gridview_insumos_del_proveedor_RowCommand" OnRowDataBound="gridview_insumos_del_proveedor_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Insumo" DataField="Producto" />
                            <asp:BoundField HeaderText="Cantidad Pedida" DataField="cantidad_pedida"/>
                            <asp:BoundField HeaderText="Cantidad Despachada" DataField="cantidad_despachada"/>
                            <asp:TemplateField HeaderText="Ingrese cantidad recibida">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_cantidad" CssClass="input-group-text" runat="server">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Cantidad Recibida" DataField="cantidad_recibida" />
                            <asp:ButtonField HeaderText="Confirmar" Text="Aplicar cambios" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_aplicar_cambios" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
