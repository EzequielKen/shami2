<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="movimiento_mercaderia_interna_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.movimiento_mercaderia_interna_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">
            <div class="alert alert-light">
                <h3>
                    <label>Campos obligatorios</label>
                </h3>
                <div class="input-group mb-3">
                    <span class="input-group-text">Entrega</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_entrega" runat="server" />
                </div>

                <div class="input-group mb-3">
                    <span class="input-group-text">Recibe</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_recibe" runat="server" />
                </div>

                <div class="input-group mb-3">
                    <span class="input-group-text">Direccion</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_direccion" runat="server" />
                </div>
                <div class="input-group mb-3">
                    <span class="input-group-text">Contacto</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_contacto" runat="server" />
                </div>
                <div class="input-group mb-3">
                    <span class="input-group-text">Producto</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_producto" runat="server" />
                </div>
            </div>

            <div class="alert alert-light">


                <div class="input-group mb-3">
                    <span class="input-group-text">Nota</span>
                    <asp:TextBox CssClass="form-control" ID="textbox_nota" TextMode="MultiLine" runat="server" />
                </div>

            </div>
            <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" OnClick="boton_cargar_Click" runat="server" />

        </div>

    </div>
    <hr />
</asp:Content>
