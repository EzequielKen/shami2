<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="lista_de_faltantes.aspx.cs" Inherits="paginaWeb.paginas.lista_de_faltantes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
             <!-- Modal -->
 <div class="modal fade" id="spinnerModal" tabindex="-1" aria-labelledby="spinnerModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
                 <h3>Cargando... Espere...</h3>
             </div>
             <div class="modal-body text-center">
                 <div class="spinner-border" role="status">
                     <span class="visually-hidden">Cargando... espere...</span>
                 </div>
             </div>
         </div>
     </div>
 </div>
            <div class="container">
                <div class="row">
                    <div class="col">
                        <div class="alert alert-light">
                            <div class="input-group">
                                <h4>
                                    <label>Buscar</label>
                                </h4>
                                <asp:TextBox ID="textbox_buscar" CssClass="form-control" placeholder="Buscar..." runat="server" OnTextChanged="textbox_buscar_TextChanged" AutoPostBack="true" />

                                <h4>
                                    <label>Tipo Producto</label>
                                </h4>
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged1" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:Button ID="boton_pdf" Text="PDF Faltantes" CssClass="btn btn-primary" OnClick="boton_pdf_Click"  runat="server" />
                            </div>
                            <asp:GridView Caption="LISTA DE PRODUCTOS" CaptionAlign="Top" runat="server" ID="gridview_productos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_productos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:TemplateField HeaderText="Nota">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nota" CssClass=" form-control" runat="server"/>
                                            <asp:Button ID="boton_nota" Visible="false" Text="Modificar Nota" CssClass="btn btn-primary" OnClick="boton_nota_Click" runat="server" data-bs-toggle="modal" data-bs-target="#spinnerModal" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Marcar Faltante">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_faltante" CssClass="btn btn-primary" Text="Marcar Como Faltante" runat="server" OnClick="boton_faltante_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="alert alert-light">
                            <asp:GridView Caption="LISTA DE PRODUCTOS FALTANTES" CaptionAlign="Top" runat="server" ID="gridview_resumen" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_resumen_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="Producto" DataField="producto" />
                                    <asp:BoundField HeaderText="Nota" DataField="nota" />
                                    <asp:TemplateField HeaderText="Marcar Faltante">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_faltante_resumen" CssClass="btn btn-primary" Text="Marcar Como Faltante" runat="server" OnClick="boton_faltante_resumen_Click" data-bs-toggle="modal" data-bs-target="#spinnerModal"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
