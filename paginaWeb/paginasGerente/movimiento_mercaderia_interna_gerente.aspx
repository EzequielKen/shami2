<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="movimiento_mercaderia_interna_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.movimiento_mercaderia_interna_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <h1>Carga de movimientos</h1>
                    <div class="alert alert-light">
                        <h3>
                            <label>Campos obligatorios</label>
                        </h3>
                        <div class="input-group mb-3">
                            <span class="input-group-text">Entrega</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_entrega" runat="server" />
                        </div>

                        <div class="input-group mb-3">
                            <span class="input-group-text">Recibe</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_recibe" runat="server" />
                        </div>

                        <div class="input-group mb-3">
                            <span class="input-group-text">Direccion</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_direccion" runat="server" />
                        </div>
                        <div class="input-group mb-3">
                            <span class="input-group-text">Contacto</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_contacto" runat="server" />
                        </div>
                        <div class="input-group mb-3">
                            <span class="input-group-text">Producto</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_producto" runat="server" />
                        </div>
                    </div>

                    <div class="alert alert-light">


                        <div class="input-group mb-3">
                            <span class="input-group-text">Nota</span>
                            <asp:TextBox CssClass="form-control" ID="textbox_nota" TextMode="MultiLine" runat="server" />
                        </div>

                    </div>
                    <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_cargar_Click" runat="server" />

                </div>

            </div>
            <hr />

            <div class="container">
                <div class="row">
                    <h1>Historial de movimientos</h1>
                    <div class="col">
                        <h3>
                            <asp:Label ID="label_fecha_seleccionada" Text="Fecha Seleccionada: N/A" runat="server" />
                        </h3>
                        <asp:Calendar ID="calendario" CssClass="  table-bordered " OnSelectionChanged="calendario_SelectionChanged" runat="server">
                            <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>
                            <TitleStyle BackColor="gray"
                                ForeColor="White"></TitleStyle>
                            <DayStyle BackColor="gray"></DayStyle>
                            <SelectedDayStyle BackColor="LightGray"
                                Font-Bold="True"></SelectedDayStyle>
                        </asp:Calendar>
                    </div>
                    <div class="col">
                        <asp:GridView ID="gridView_movimientos" runat="server" Caption="MOVIMIENTOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                <asp:BoundField HeaderText="Producto" DataField="producto" />
                                <asp:BoundField HeaderText="Entrega" DataField="entrega" />
                                <asp:BoundField HeaderText="Recibe" DataField="recibe" />
                                <asp:BoundField HeaderText="Direccion" DataField="direccion" />
                                <asp:BoundField HeaderText="Contacto" DataField="contacto" />
                                <asp:BoundField HeaderText="Nota" DataField="nota" />
                                <asp:TemplateField HeaderText="PDF">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_pdf" OnClick="boton_pdf_Click" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm btn-success" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_eliminar" OnClick="boton_eliminar_Click" Text="Deshabilitar" ControlStyle-CssClass="btn btn-primary btn-sm btn-danger" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
