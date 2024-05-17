<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraCarrefour.Master" AutoEventWireup="true" CodeBehind="sucursales_carrefour.aspx.cs" Inherits="paginaWeb.paginasCarrefour.sucursales_carrefour" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container">
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
          <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
          </div>

          <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
              <asp:GridView OnSelectedIndexChanged="gridView_sucursales_SelectedIndexChanged" runat="server" ID="gridView_sucursales" AutoGenerateColumns="false" CssClass="table table-striped">
                  <Columns>
                      <asp:BoundField HeaderText="id" DataField="id" />
                      <asp:BoundField HeaderText="Sucursal" DataField="sucursal" />
                      <asp:BoundField HeaderText="Direccion" DataField="direccion" />
                      <asp:CommandField ShowSelectButton="true" SelectText="abrir" HeaderText="seleccionar" ControlStyle-CssClass="btn btn-primary btn-sm" />
                  </Columns>
              </asp:GridView>
          </div>
          <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4 text-center">
          </div>
      </div>
  </div>
</asp:Content>
