<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraMarketing.Master" AutoEventWireup="true" CodeBehind="solicitud_franquicia.aspx.cs" Inherits="paginaWeb.paginasMarketing.solicitud_franquicia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="alert alert-light">
                        <div class="input-group">
                            <asp:Label CssClass="form-control" Text="Mes" runat="server" />
                            <asp:DropDownList ID="dropdown_mes" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_mes_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:Label CssClass="form-control" Text="Año" runat="server" />
                            <asp:DropDownList ID="dropdown_año" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropdown_año_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <asp:GridView Caption="LISTA DE SOLICITUDES" CaptionAlign="Top" runat="server" ID="gridview_solicitudes" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="Nombre" DataField="nombre"/>
                                <asp:BoundField HeaderText="Apellido" DataField="apellido"/>
                                <asp:BoundField HeaderText="D.N.I" DataField="dni"/>
                                <asp:BoundField HeaderText="Telefono" DataField="telefono"/>

                                <asp:TemplateField HeaderText="Nuevo Stock">
                                    <ItemTemplate>
                                        <asp:Button ID="boton_abrir" CssClass="btn btn-primary" Text="Abrir" OnClick="boton_abrir_Click" runat="server" />
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
