<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="calendario_de_entrega.aspx.cs" Inherits="paginaWeb.paginas.calendario_de_entrega" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class=" container-fluid">
        <div class="row">
            <div class="col">
                <div class="alert alert-light">
                    <asp:GridView Caption="DIAS DE ENTREGA DE FABRICA" CaptionAlign="Top"  runat="server" ID="gridview_dias_de_entrega" AutoGenerateColumns="false" CssClass="table table-dark table-striped">
                        <columns>
                            <asp:BoundField HeaderText="Lunes" DataField="lunes" />
                            <asp:BoundField HeaderText="Martes" DataField="martes" />
                            <asp:BoundField HeaderText="Miercoles" DataField="miercoles" />
                            <asp:BoundField HeaderText="Jueves" DataField="jueves" />
                            <asp:BoundField HeaderText="Viernes" DataField="viernes" />
                            <asp:BoundField HeaderText="Sabado" DataField="sabado" />
                            <asp:BoundField HeaderText="Domingo" DataField="domingo" />
                        </columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
