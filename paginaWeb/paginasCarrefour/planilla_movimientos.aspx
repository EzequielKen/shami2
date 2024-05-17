<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="planilla_movimientos.aspx.cs" Inherits="paginaWeb.paginasCarrefour.planilla_movimientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">

                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>

                            <TitleStyle BackColor="gray"
                                ForeColor="White"></TitleStyle>

                            <DayStyle BackColor="gray"></DayStyle>

                            <SelectedDayStyle BackColor="LightGray"
                                Font-Bold="True"></SelectedDayStyle>

                        </asp:Calendar>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <h2>
                            <asp:Label ID="label_fecha" Text="text" runat="server" />
                        </h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">

                        <div class=" alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <h1>
                                        <asp:Label ID="label_fecha_ultima_visita" Text="Ultima visita: N/A" runat="server" />

                                    </h1>
                                </div>
                                <div class="col">
                                    <h1>
                                        <asp:Label ID="label_sucursal_seleccionada" Text="N/A" runat="server" />
                                    </h1>
                                </div>
                                <div class="col text-lg-end">
                                    <asp:Button ID="boton_enviar" OnClick="boton_enviar_Click" Text="Enviar" CssClass="btn btn-primary" runat="server" />
                                </div>
                            </div>
                            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" OnRowCommand="gridview_productos_RowCommand" OnRowDataBound="gridview_productos_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="Producto" />
                                    
                                    <asp:BoundField HeaderText="Stock Inicial" DataField="Ultimo_stock" />
                                    <asp:TemplateField HeaderText="Devolucion">
                                        <ItemTemplate>
                                            <asp:Label Text="Devolucion" runat="server" />
                                            <div class="input-group mb-3">
                                                <asp:Button ID="boton_restar_devolucion" Text="-" CssClass="btn btn-primary" runat="server" CommandName="restar_devolucion" CommandArgument='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="textbox_devolucion" CssClass="input-group-text w-50" runat="server" AutoPostBack="true" OnTextChanged="textbox_devolucion_TextChanged">
                                                </asp:TextBox>
                                                <asp:Button ID="boton_sumar_devolucion" Text="+" CssClass="btn btn-primary" runat="server" CommandName="sumar_devolucion" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </div>
                                            <asp:Label Text="Reposicion" runat="server" />
                                            <div class="input-group mb-3">
                                                <asp:Button ID="boton_restar_reposicion" Text="-" CssClass="btn btn-primary" runat="server" CommandName="restar_reposicion" CommandArgument='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="textbox_reposicion" CssClass="input-group-text w-50" runat="server" AutoPostBack="true" OnTextChanged="textbox_reposicion_TextChanged">
                                                </asp:TextBox>
                                                <asp:Button ID="boton_sumar_resposicion" Text="+" CssClass="btn btn-primary" runat="server" CommandName="sumar_reposicion" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </div>
                                            <asp:Label Text="Stock Final" runat="server" />
                                            <div class="input-group mb-3">
                                                <asp:Button ID="boton_restar_stock_actual" Text="-" CssClass="btn btn-primary" runat="server" CommandName="restar_stock_actual" CommandArgument='<%# Container.DataItemIndex %>' />
                                                <asp:TextBox ID="textbox_stock_actual" CssClass="input-group-text w-50" runat="server" AutoPostBack="true" OnTextChanged="textbox_stock_actual_TextChanged">
                                                </asp:TextBox>
                                                <asp:Button ID="boton_sumar_stock_actual" Text="+" CssClass="btn btn-primary" runat="server" CommandName="sumar_stock_actual" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField HeaderText="Vendio" DataField="vendio" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
