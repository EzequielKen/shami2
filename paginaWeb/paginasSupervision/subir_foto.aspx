<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="subir_foto.aspx.cs" Inherits="paginaWeb.paginasSupervision.subir_foto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function previewFile() {
            const fileUpload = document.getElementById('<%= fileUploadFoto.ClientID %>');
            const previewContainer = document.getElementById('previewContainer');
            const previewImage = document.getElementById('previewImage');
            const previewVideo = document.getElementById('previewVideo');
            const file = fileUpload.files[0];
            const reader = new FileReader();
    
            if (file) {
                reader.onloadend = function () {
                    const fileType = file.type.split('/')[0];

                    if (fileType === 'image') {
                        previewImage.src = reader.result;
                        previewImage.classList.remove('d-none');
                        previewVideo.classList.add('d-none');
                    } else if (fileType === 'video') {
                        previewVideo.src = reader.result;
                        previewVideo.classList.remove('d-none');
                        previewImage.classList.add('d-none');
                    }

                    previewContainer.classList.remove('d-none');
                }

                reader.readAsDataURL(file);
            } else {
                previewContainer.classList.add('d-none');
                previewImage.classList.add('d-none');
                previewVideo.classList.add('d-none');
            }
        }
    </script>

    <div class="container mt-5">
        <h2 class="mb-4">Subir Foto o Video</h2>
        <div class="form-group">
            <label for="fileUploadFoto" class="form-label">Selecciona una foto o video:</label>
            <asp:FileUpload ID="fileUploadFoto" runat="server" CssClass="form-control" OnChange="previewFile()" />
        </div>
        <asp:HiddenField ID="hiddenId" runat="server" />

        <div class="form-group mt-3">
            <!-- Contenedor para la vista previa de la imagen o video -->
            <div id="previewContainer" class="d-none">
                <img id="previewImage" class="img-thumbnail mt-2 d-none" />
                <video id="previewVideo" class="img-thumbnail mt-2 d-none" controls></video>
            </div>
        </div>

        <div class="form-group mt-3">
            <asp:Button ID="btnSubirFoto" runat="server" Text="Subir Archivo" CssClass="btn btn-primary" OnClick="btnSubirFoto_Click" />
        </div>
        <div class="form-group mt-3">
            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" />
        </div>
    </div>


</asp:Content>
