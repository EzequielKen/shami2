<%@ Page Async="true" Title="" Language="C#" MasterPageFile="/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="proveedores.aspx.cs" Inherits="paginaWeb.pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
            <div class=" row container-fluid m-auto">

                <div class="col"></div>
                <div class="col  text-center">
                    <asp:GridView OnSelectedIndexChanged="gridview_proveedores_SelectedIndexChanged" runat="server" ID="gridview_proveedores" AutoGenerateColumns="false" CssClass="table table-striped">
                        <Columns>
                            <asp:BoundField HeaderText="id" DataField="id" />
                            <asp:BoundField HeaderText="proveedor" DataField="nombre_proveedor" />
                            <asp:CommandField ShowSelectButton="true" SelectText="abrir" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                        </Columns> 
                    </asp:GridView>
                </div>
                <div class="col"></div>

            </div>
      
</asp:Content>
