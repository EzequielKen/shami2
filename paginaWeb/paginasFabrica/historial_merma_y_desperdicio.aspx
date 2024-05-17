<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_merma_y_desperdicio.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_merma_y_desperdicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class=" container-fluid">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="row">
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
                            <asp:GridView Caption="MERMA Y DESPERDICIO" CaptionAlign="Top" ID="gridView_desperdicio_merma" runat="server" OnRowCommand="gridView_desperdicio_merma_RowCommand" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="id" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                                    <asp:BoundField HeaderText="Tipo" DataField="tipo" />
                                    <asp:ButtonField HeaderText="Abrir" Text="Abrir" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar" />

                                    <asp:TemplateField HeaderText="Ingrese nota">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nota" CssClass="input-group-text" runat="server">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="Cargar Notas" Text="Cargar Nota" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cargar_nota" />
                                    <asp:BoundField HeaderText="Notas" DataField="nota" />
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">

                            <h2>
                                <asp:Label ID="label_id" Text="" runat="server" />
                                <asp:Label ID="label_fecha" Text="Seleccione una fecha." runat="server" />
                            </h2>

                            <h3>
                                <asp:Label ID="label_tipo" Text="" runat="server" />
                            </h3>
                                <asp:Label ID="label_nota" Text="" runat="server" />
                            <asp:GridView Caption="DETALLE" CaptionAlign="Top" ID="gridView_detalle" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Desperdicio/Merma" DataField="presentacion" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
