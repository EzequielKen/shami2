<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraMarketing.Master" AutoEventWireup="true" CodeBehind="tickets_marketing.aspx.cs" Inherits="paginaWeb.paginasMarketing.tickets_marketing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" /> 
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="container-fluid">
            <h1>Tickets</h1>
            <div class="row">
                <div class="alert alert-light">
                    <div class="input-group">
                        <label class="form-control">Mes</label>
                        <asp:DropDownList ID="dropdown_mes" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_mes_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <label class="form-control">Año</label>
                        <asp:DropDownList ID="dropdown_año" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_año_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <label class="form-control">Tipo de Ticket</label>
                        <asp:DropDownList ID="dropdown_tipo" CssClass="form-select" runat="server" OnSelectedIndexChanged="dropdown_tipo_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Informe de Error" />
                            <asp:ListItem Text="Solicitud de Desarrollo" />
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <style>
                .gridview-container {
                    overflow-x: auto; /* Permite el desplazamiento horizontal si es necesario */
                    white-space: normal; /* Permite que el texto se ajuste automáticamente a múltiples líneas */
                }

                    .gridview-container table {
                        width: 100%; /* Usar el 100% del ancho disponible */
                        table-layout: auto; /* Permite que las celdas ajusten su tamaño según el contenido */
                    }

                    .gridview-container td {
                        white-space: normal; /* Permitir el ajuste del texto a múltiples líneas */
                        word-wrap: break-word; /* Dividir palabras largas para evitar desbordamientos */
                        overflow-wrap: break-word; /* Asegurar que las palabras largas también se ajusten */
                    }

                        .gridview-container td.detalle {
                            max-width: 100%; /* Permitir que la columna de Detalle use el máximo ancho disponible */
                        }
            </style>

            <asp:Button ID="boton_ordenar_area" CssClass="btn btn-primary" Text="Ordenar Por Area" OnClick="boton_ordenar_area_Click" runat="server" />

            <div class="row">
                <div class="alert alert-light gridview-container" style="width: 100%;">
                    <asp:GridView ID="gridView_tickets" runat="server" Caption="TICKETS" CaptionAlign="Top" AutoGenerateColumns="false"
                        CssClass="table table-striped text-center table-responsive"
                        OnRowDataBound="gridView_tickets_RowDataBound"
                        Style="width: 100%;">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />

                            <asp:TemplateField HeaderText="Prioridad">
                                <ItemTemplate>
                                    <asp:TextBox ID="textbox_prioridad" CssClass="form-control" runat="server"
                                        OnTextChanged="textbox_prioridad_TextChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField HeaderText="Estado" DataField="estado" />
                            <asp:BoundField HeaderText="Fecha Solicitud" DataField="fecha_solicitud" />
                            <asp:BoundField HeaderText="Fecha Resolucion Solicitada" DataField="fecha_resolucion_solicitada" />
                            <asp:BoundField HeaderText="Fecha Resolucion" DataField="fecha_resolucion" />
                            <asp:BoundField HeaderText="Solicita" DataField="solicita" />
                            <asp:BoundField HeaderText="Asunto" DataField="asunto" />
                            <asp:BoundField HeaderText="Detalle" DataField="detalle" ItemStyle-CssClass="detalle" />

                            <asp:TemplateField HeaderText="Resolver">
                                <ItemTemplate>
                                    <asp:Button ID="boton_resolver" CssClass="btn btn-success" Text="Resolver" OnClick="boton_resolver_Click" runat="server" />
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
