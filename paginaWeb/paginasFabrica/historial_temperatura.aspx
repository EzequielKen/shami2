<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="historial_temperatura.aspx.cs" Inherits="paginaWeb.paginasFabrica.historial_temperatura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <div class="container-fluid">
            <div class="row">
                <div class="col-1"></div>
                <div class="col-10">
                    <h3>
                        <asp:Label ID="label_cartel_advertencia" Visible="false" CssClass="alert alert-danger" Text="Ingrese su nombre" runat="server" />
                    </h3>
                    <div id="div_equipos" class="alert alert-light">

                        <div class="input-group">
                            <asp:DropDownList ID="dropdown_ubicaciones" CssClass=" form-control dropdown" AutoPostBack="true" runat="server">
                            </asp:DropDownList>
                        </div>

                        <asp:GridView Caption="LISTA DE EQUIPOS" CaptionAlign="Top" runat="server" ID="gridview_equipos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="ubicacion" DataField="ubicacion" />
                                <asp:BoundField HeaderText="nombre" DataField="nombre" />
                                <asp:BoundField HeaderText="categoria" DataField="categoria" />
                                <asp:BoundField HeaderText="observaciones" DataField="observaciones" />
                                <asp:BoundField HeaderText="temperatura optima" DataField="temperatura" />
                                <asp:BoundField HeaderText="Turno 1: 08:00Hs a 12:00Hs" DataField="turno_1" />
                                <asp:BoundField HeaderText="Turno 2: 16:00Hs a 18:00Hs" DataField="turno_2" />
                                <asp:BoundField HeaderText="Turno 3: 21:00Hs a 23:00Hs" DataField="turno_3" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-1"></div>
            </div>
        </div>
    </div>
</asp:Content>
