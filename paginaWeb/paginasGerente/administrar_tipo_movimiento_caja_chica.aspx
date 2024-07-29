<%@ Page  Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="administrar_tipo_movimiento_caja_chica.aspx.cs" Inherits="paginaWeb.paginasGerente.administrar_tipo_movimiento_caja_chica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col"></div>
            <div class="col">
                <div class="alert alert-light">
                    <h4>
                        <asp:Label Text="Tipo Movimiento" runat="server" />
                    </h4>
                    <asp:DropDownList ID="DropDown_tipo_movimiento" OnSelectedIndexChanged="DropDown_tipo_movimiento_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true" runat="server">
                        <asp:ListItem Text="Ingreso" />
                        <asp:ListItem Text="Egreso" />
                    </asp:DropDownList>
                    <hr />
                    <asp:Label Text="Concepto" runat="server" />
                    <div class="input-group">
                        <asp:TextBox ID="textbox_concepto" placeholder="Ingrese concepto nuevo" CssClass="form-control" runat="server" />
                        <asp:Button ID="boton_registrar" Text="Registrar" CssClass="btn btn-primary" OnClick="boton_registrar_Click" runat="server" />
                    </div>
                    <asp:GridView ID="gridView_conceptos" runat="server" Caption="CONCEPTOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped text-center" OnRowDataBound="gridView_conceptos_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Concepto" DataField="concepto" />
                            <asp:BoundField HeaderText="estado" DataField="estado" />

                            <asp:TemplateField HeaderText="Habilitar/Deshabilitar">
                                <ItemTemplate>
                                    <asp:Button ID="boton_habilitacion" OnClick="boton_habilitacion_Click" Text="Deshabilitar" ControlStyle-CssClass="btn btn-primary btn-sm btn-success" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:Button ID="boton_eliminar" OnClick="boton_eliminar_Click" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm btn-danger" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Ingrese dato">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_modificar"  placeholder="Ingrese nuevo concepto" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Modificar">
                                <ItemTemplate>
                                    <asp:Button ID="boton_modificar" OnClick="boton_modificar_Click" Text="Modificar" ControlStyle-CssClass="btn btn-primary btn-sm" runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col"></div>
        </div>
    </div>

</asp:Content>
