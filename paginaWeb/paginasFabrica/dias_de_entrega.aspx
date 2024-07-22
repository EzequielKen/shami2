<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="dias_de_entrega.aspx.cs" Inherits="paginaWeb.paginasFabrica.dias_de_entrega" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <% System.Data.DataTable tipo_usuario = (System.Data.DataTable)Session["tipo_usuario"];%>
            <div class="conteinter-fluid">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <asp:GridView Caption="DIAS DE ENTREGA" CaptionAlign="Top" runat="server" ID="gridview_dia_de_entrega" OnRowCommand="gridview_dia_de_entrega_RowCommand" OnRowDataBound="gridview_dia_de_entrega_RowDataBound" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:TemplateField HeaderText="lunes">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_lunes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_lunes_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="martes">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_martes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_martes_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="miercoles">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_miercoles" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_miercoles_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="jueves">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_jueves" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_jueves_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="viernes">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_viernes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_viernes_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="sabado">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_sabado" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_sabado_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="domingo">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="dropdown_domingo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_domingo_SelectedIndexChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="Eliminar" Text="Eliminar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="boton_eliminar" />
                                </Columns>
                            </asp:GridView>

                            <%if (tipo_usuario.Rows[0]["rol"].ToString() != "Shami Villa Maipu Expedicion")
                                {%>
                            <div class="row">
                                <div class="col">
                                    <asp:Button ID="boton_agregar_fila" CssClass="btn btn-primary" Text="Agregar Fila" OnClick="boton_agregar_fila_Click" runat="server" />

                                </div>
                                <div class="col"></div>
                                <div class="col text-lg-end">
                                    <asp:Button ID="boton_guardar_cambios" CssClass="btn btn-primary" Text="Guardar Cambios" OnClick="boton_guardar_cambios_Click" runat="server" />

                                </div>
                            </div>
                            <%}%>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col">
                        <h2>Sucursales.</h2>
                        <asp:GridView runat="server" ID="gridView_sucursales" AutoGenerateColumns="false" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Cliente" DataField="sucursal" />
                                <asp:BoundField HeaderText="Direccion" DataField="direccion" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col">



                        <%if (tipo_usuario.Rows[0]["rol"].ToString() != "Shami Villa Maipu Expedicion")
                            {%>
                        <h2>Usuarios.</h2>
                        <asp:GridView runat="server" ID="gridView_usuarios" AutoGenerateColumns="false" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                                <asp:BoundField HeaderText="Contraseña" DataField="contraseña" />
                            </Columns>
                        </asp:GridView>
                        <%}%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
