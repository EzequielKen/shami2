<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_ticket.aspx.cs" Inherits="paginaWeb.paginasFabrica.crear_ticket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function mostrarModal() {
                    alert("Ticket Enviado Exitosamente.");
                    $('#ticketModal').modal('show');
                }
            </script>

            <!-- Modal -->
            <div class="modal fade" id="ticketModal" tabindex="-1" aria-labelledby="ticketModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="ticketModalLabel">Éxito</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            Ticket enviado con éxito
   
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container">
                <div class="row">
                    <div class="alert alert-light">
                        <h3>¿Como podemos ayudarte?</h3>
                        <asp:DropDownList ID="dropdown_tipo_ticket" CssClass="form-select" runat="server">
                            <asp:ListItem Text="Informe de Error" />
                            <asp:ListItem Text="Solicitud de Desarrollo" />
                        </asp:DropDownList>
                    </div>

                    <div class="alert alert-light">
                        <div class="input-group pb-3">
                            <label class="form-label">
                                Escriba el motivo de su Ticket</labe>
                    <asp:TextBox ID="textbox_asunto" CssClass="form-control" placeholder="Ingrese asunto..." runat="server" />
                                <label class="form-label">Seleccione fecha para cuando necesite el ticket resuelto</label>
                                <asp:TextBox ID="textbox_fecha" CssClass="form-control" TextMode="Date" runat="server" />
                        </div>
                        <asp:Label ID="label_error_asunto" Visible="false" CssClass="alert alert-danger" Text="No puede seleccionar una fecha anterior" runat="server" />
                        <asp:Label ID="label_error_fecha" Visible="false" CssClass="alert alert-danger" Text="No puede seleccionar una fecha anterior" runat="server" />
                    </div>

                    <div class="alert alert-light">
                        <div class="input-group pb-3">
                            <label class="form-label">Agrega toda la información relevante para el ticktet</label>
                            <asp:TextBox ID="textbox_detalle" CssClass="form-control pb-5" placeholder="Ingrese detalle..." TextMode="MultiLine" runat="server" />
                        </div>
                        <asp:Label ID="label_error_detalle" Visible="false" CssClass="alert alert-danger" Text="No puede seleccionar una fecha anterior" runat="server" />
                    </div>
                    <asp:Button ID="boton_enviar" CssClass="btn btn-primary" Text="Enviar" runat="server" OnClick="boton_enviar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
