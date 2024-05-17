<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraGerente.Master" AutoEventWireup="true" CodeBehind="proveedores_fabrica_gerente.aspx.cs" Inherits="paginaWeb.paginasGerente.proveedores_fabrica_gerente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Proveedores de Fabrica</h2>
        <div class="row">

            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                <div class="alert alert-light">
                    <asp:Button Text="Crear proveedor" CssClass=" btn btn-primary" runat="server" OnClick="boton_crear_proveedor_Click" ID="boton_crear_proveedor" />
                </div>
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
                <div class=" alert alert-light">
                    <div class="input-group">
                        <asp:TextBox CssClass=" form-control" ID="textbox_buscar" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" runat="server" />
                        <asp:Button CssClass=" form-control" Text="Buscar" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">


                <asp:GridView runat="server" ID="gridView_proveedores" AutoGenerateColumns="false" CssClass="table table-striped" OnSelectedIndexChanged="gridView_proveedores_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField HeaderText="id" DataField="id" />
                        <asp:BoundField HeaderText="Proveedor" DataField="proveedor" />
                        <asp:BoundField HeaderText="Direccion" DataField="direccion" />
                        <asp:CommandField ShowSelectButton="true" SelectText="abrir" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                    </Columns>
                </asp:GridView>

            </div>
            <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
            </div>
        </div>
    </div>
</asp:Content>
