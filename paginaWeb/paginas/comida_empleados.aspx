<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="comida_empleados.aspx.cs" Inherits="paginaWeb.paginas.comida_empleados" %>

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
                                <asp:Label CssClass="form-control" Text="cargo" runat="server" />
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <span class="badge rounded-pill text-bg-primary">Azul = No Seleccionado</span>
                            <span class="badge rounded-pill text-bg-success">Verde = Seleccionado</span>
                            <h2>
                                <asp:Label ID="label_total" Text="text" runat="server" />
                            </h2>
                            <asp:GridView Caption="LISTA DE EMPLEADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                                    <asp:BoundField HeaderText="Apellido" DataField="apellido" />
                                    
                                    <asp:TemplateField HeaderText="Cargar Consumo de Comida">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cargar_comida" CssClass="btn btn-primary" Text="Cargar" runat="server" OnClick="boton_cargar_comida_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
