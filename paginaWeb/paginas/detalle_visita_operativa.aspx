<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="detalle_visita_operativa.aspx.cs" Inherits="paginaWeb.paginas.detalle_visita_operativa" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:HiddenField ID="hiddenFolderPath" runat="server" />

    <script type="text/javascript">
        function openModal(id) {
            var extensions = [".jpg", ".jpeg", ".png", ".gif", ".mp4"];
            var found = false;

            for (var i = 0; i < extensions.length; i++) {
                var fileUrl = '/FotosSubidas/visitas_operativas/' + id + extensions[i];
                var request = new XMLHttpRequest();
                request.open('HEAD', fileUrl, false);
                request.send();

                if (request.status === 200) {
                    if (extensions[i] === ".mp4") {
                        document.getElementById('modalVideo').src = fileUrl;
                        document.getElementById('modalVideo').classList.remove('d-none');
                        document.getElementById('modalImage').classList.add('d-none');
                    } else {
                        document.getElementById('modalImage').src = fileUrl;
                        document.getElementById('modalImage').classList.remove('d-none');
                        document.getElementById('modalVideo').classList.add('d-none');
                    }
                    found = true;
                    break;
                }
            }

            if (found) {
                var myModal = new bootstrap.Modal(document.getElementById('fotoModal'), {
                    keyboard: false
                });
                myModal.show();

                var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
                document.querySelector('.modal-dialog').style.top = scrollTop + 'px';
            } else {
                alert("El archivo no existe.");
            }
        }


    </script>

    <!-- Modal Foto-->
    <div class="modal fade" id="fotoModal" tabindex="-1" aria-labelledby="fotoModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="fotoModalLabel">Archivo de la Visita Operativa</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                    <img id="modalImage" class="img-fluid d-none" src="#" alt="Archivo de la Visita Operativa" />
                    <video id="modalVideo" class="img-fluid d-none" controls>
                        <source src="#" type="video/mp4">
                        Tu navegador no soporta la reproducción de videos.
                    </video>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <div class="row">
                        <div class="col">
                        </div>
                        <div class="col">
                            <h2>
                                <asp:Label ID="label_fecha" Text="text" runat="server" />
                            </h2>
                            <h3>
                                <asp:Label ID="label_promedio_evaluados" Text="Promedio Evaluados: N/A" runat="server" />
                            </h3>
                            <asp:Button ID="boton_pdf" Text="PDF" CssClass="btn btn-success" OnClick="boton_pdf_Click" runat="server" />
                        </div>
                    </div>
                    <asp:Label ID="label_observacion" Text="" runat="server" />
                    <asp:GridView Caption="LISTA DE EVALUADOS" CaptionAlign="Top" runat="server" ID="gridview_empleados" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_empleados_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                            <asp:BoundField HeaderText="Apellido" DataField="apellido" />
                            <asp:TemplateField HeaderText="Historial">
                                <ItemTemplate>
                                    <asp:Button ID="boton_historial" CssClass="btn btn-primary" Text="Ver Detalle" runat="server" OnClick="boton_historial_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Puntaje Promedio" DataField="promedio" />
                            <asp:BoundField HeaderText="cargos evaluados" DataField="cargos_evaluados" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="col">
                <div class="alert alert-light">
                    <h4>
                        <asp:Label ID="label_empleado" Visible="false" Text="Empleado" runat="server" />
                    </h4>
                    <h4>
                        <asp:Label ID="label_fecha_historial" Visible="false" Text="Empleado" runat="server" />
                    </h4>
                    <h4>
                        <asp:Label ID="label_puntaje_promedio" Text="" runat="server" />
                    </h4>

                    <div class="row">
                        <div class="col">
                            <asp:Button ID="boton_encargado" Visible="false" CssClass="btn btn-primary" Text="Encargado" runat="server" OnClick="boton_encargado_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_cajero" Visible="false" CssClass="btn btn-primary" Text="Cajero" runat="server" OnClick="boton_cajero_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_shawarmero" Visible="false" CssClass="btn btn-primary" Text="Shawarmero" runat="server" OnClick="boton_shawarmero_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_atencion" Visible="false" CssClass="btn btn-primary" Text="Atencion al Cliente" runat="server" OnClick="boton_atencion_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_cocina" Visible="false" CssClass="btn btn-primary" Text="Cocina" runat="server" OnClick="boton_cocina_Click" />
                        </div>
                        <div class="col">
                            <asp:Button ID="boton_limpieza" Visible="false" CssClass="btn btn-primary" Text="Limpieza" runat="server" OnClick="boton_limpieza_Click" />
                        </div>
                    </div>

                    <asp:Label ID="label_alerta_registro" CssClass="alert alert-danger" Visible="false" Text="No hay registros" runat="server" />

                    <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="Actividad" DataField="actividad" />
                            <asp:BoundField HeaderText="Nota" DataField="nota" />
                            <asp:BoundField HeaderText="Fecha" DataField="fecha" />
                            <asp:BoundField HeaderText="Punto Teorico" DataField="punto_teorico" />
                            <asp:BoundField HeaderText="Punto Real" DataField="punto_real" />

                            <asp:TemplateField HeaderText="Ver Foto">
                                <ItemTemplate>
                                    <asp:Button
                                        ID="boton_ver_foto"
                                        Text="Ver Foto"
                                        CssClass="btn btn-primary"
                                        runat="server"
                                        OnClientClick='<%# "openModal(\"" + Eval("id") + "\"); return false;" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
