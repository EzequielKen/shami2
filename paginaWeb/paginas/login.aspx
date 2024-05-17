<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraLogin.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="paginaWeb.login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Modal -->
    <div class="modal fade" id="spinnerModal" tabindex="-1" aria-labelledby="spinnerModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body text-center">
                    <div class="spinner-border" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <main class="form-signin w-100 m-auto ">



        <img class="mb-4" src="/imagenes/logo-completo.png" alt="" width="200">
        <h1 class="h3 mb-3 fw-normal text-white">Por favor, ingrese.</h1>

        <div class="form-floating ">
            <asp:TextBox ID="textbox_usuario" CssClass="form-control" runat="server" />
            <label for="floatingInput">Ingrese su usuario</label>
        </div>
        <div class="form-floating">
            <asp:TextBox type="password" class="form-control" ID="textbox_contraseña" runat="server" />
            <label for="floatingPassword">Ingrese su contraseña</label>
        </div>

        <asp:Button Text="Iniciar sesión" CssClass="btn btn-primary w-100 py-2" ID="boton_login" OnClick="boton_login_Click" runat="server" data-bs-toggle="modal" data-bs-target="#spinnerModal" />





    </main>





</asp:Content>

