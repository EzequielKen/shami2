<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_visita_operativa.aspx.cs" Inherits="paginaWeb.paginas.historial_visita_operativa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="alert alert-light">
                <div class="input-group">
                    <label>Mes</label>
                    <asp:DropDownList ID="dropdown_mes" CssClass="form-select" OnSelectedIndexChanged="dropdown_mes_SelectedIndexChanged" AutoPostBack="true" runat="server">
                    </asp:DropDownList>
                    <label>Año</label>
                    <asp:DropDownList ID="dropdown_año" CssClass="form-select" OnSelectedIndexChanged="dropdown_año_SelectedIndexChanged" AutoPostBack="true" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <hr />

            <div class="alert alert-light">
                <asp:GridView Caption="LISTA DE VISITAS" CaptionAlign="Top" runat="server" ID="gridview_visitas" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                    <Columns>
                        <asp:BoundField HeaderText="fecha de visita" DataField="fecha" />

                        <asp:TemplateField HeaderText="Ingrese Precio Venta">
                            <ItemTemplate>
                                <asp:Button ID="boton_historial" CssClass="btn btn-primary" Text="Ver Historial" OnClick="boton_historial_Click" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
