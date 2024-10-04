<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="crear_proveedor.aspx.cs" Inherits="paginaWeb.paginasFabrica.crear_proveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Crear Proveedor</h2>
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">
            </div>
            <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">

                <div class="alert alert-warning">
                    <h3>
                        <label>Campos obligatorios</label>
                    </h3>
                    <div class="input-group mb-3">
                        <span class="input-group-text">Nombre proveedor</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_nombre_proveedor" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">Provincia</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_provincia" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">Localidad</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_localidad" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">Direccion</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_direccion" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">Telefono</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_telefono" runat="server" />
                    </div>
                </div>
                <div class="alert alert-light">
                    <div class="input-group mb-3">
                        <span class="input-group-text">Pago a</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_condicion_pago" runat="server" TextMode="Number" />
                        <span class="input-group-text">Dias</span>
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">CBU/ALIAS</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_cbu_1" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">CBU/ALIAS</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_cbu_2" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">CBU/ALIAS</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_cbu_3" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">CBU/ALIAS</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_cbu_4" runat="server" />
                    </div>

                    <div class="input-group mb-3">
                        <span class="input-group-text">CBU/ALIAS</span>
                        <asp:TextBox CssClass="form-control" ID="textbox_cbu_5" runat="server" />
                    </div>

                </div>
        </div>
        <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">
        </div>
        <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">
        </div>
        <div class="col-sm-12 col-md-12 col-lg-4 col-xl-4">
            <asp:Button ID="boton_carga" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_carga_Click" runat="server" />
        </div>
    </div>
    </div>
</asp:Content>
