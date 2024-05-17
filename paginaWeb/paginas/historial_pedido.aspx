<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="historial_pedido.aspx.cs" Inherits="paginaWeb.paginas.historial_pedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 
            <div class="container">

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <asp:Label Text="Proveedor" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_proveedores" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_proveedores_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Label Text="Mes" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_mes" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_mes_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:Label Text="Año" CssClass=" form-label" runat="server" />
                                    <asp:DropDownList ID="dropDown_año" runat="server" CssClass="form-select" OnSelectedIndexChanged="dropDown_año_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12 col-md-6 col-lg-6 col-xl-6">
                    </div>

                </div>

            </div>

            <div class="container">
                <hr />
            </div>

            <div class="container">
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class="container-fluid">
                            <div class="alert alert-light">
                                <asp:GridView ID="gridView_pedidos" runat="server" Caption="PEDIDOS" CaptionAlign="Top" AutoGenerateColumns="false" CssClass="table table-striped table table-dark table-striped" OnRowCommand="gridView_pedidos_RowCommand" OnRowDataBound="gridView_pedidos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="id" DataField="id" />
                                        <asp:BoundField HeaderText="proveedor" DataField="proveedor" />
                                        <asp:BoundField HeaderText="N° pedido" DataField="num_pedido" />
                                        <asp:BoundField HeaderText="fecha" DataField="fecha" />
                                        <asp:ButtonField HeaderText="PDF" Text="PDF" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="crear_pdf"/>
                                        <asp:ButtonField HeaderText="Cancelar" Text="Cancelar pedido" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="cancelar_pedido"/>
                                        <asp:BoundField HeaderText="estado" DataField="estado" />
                                        <asp:ButtonField Visible="false" HeaderText="Iniciar reclamo" Text="Confirmar" ControlStyle-CssClass="btn btn-primary btn-sm" CommandName="iniciar_reclamo"/>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
      
</asp:Content>
