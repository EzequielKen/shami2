<%@ Page Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraFabrica.Master" AutoEventWireup="true" CodeBehind="stock_producto_terminado.aspx.cs" Inherits="paginaWeb.paginasFabrica.stock_producto_terminado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="container">
                <h2>Actualizador de Stock</h2>
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4 col-xl-4">
                        <div class="row alert alert-light">
                            <asp:Button Text="enviar" ID="boton_enviar" runat="server" CssClass="btn btn-primary btn-sm" OnClick="boton_enviar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <hr />
            </div>

            <div class=" container">

                <div class=" row  ">
                    <div class="col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <div class=" alert alert-light">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="textbox_busqueda" OnTextChanged="textbox_busqueda_TextChanged" AutoPostBack="true" />
                                <asp:Button Text="buscar" CssClass="btn btn-outline-secondary" runat="server" />
                                <div>
                                    <asp:DropDownList runat="server" CssClass="form-select" ID="dropDown_tipo" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>
                        <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark table-striped">

                            <Columns>
                                <asp:BoundField HeaderText="id" DataField="id" />
                                <asp:BoundField HeaderText="producto" DataField="producto" />
                                <asp:BoundField HeaderText="unidad de medida" DataField="unidad_de_medida" />
                                <asp:TemplateField HeaderText="Ingrese nuevo precio">
                                    <ItemTemplate>
                                        <asp:TextBox ID="texbox_precio" CssClass="input-group-text" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="precio nuevo" DataField="precio_nuevo" />

                                <asp:BoundField HeaderText="diferencia" DataField="diferencia" />
                                <asp:BoundField HeaderText="porcentaje aumento" DataField="porcentaje_aumento" />

                                <asp:CommandField ShowSelectButton="true" SelectText="Cargar" HeaderText="Cargar" ControlStyle-CssClass="btn btn-primary btn-sm" />

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
