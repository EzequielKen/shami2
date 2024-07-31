<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/paginasMaestras/paginaMaestraSistema.Master" AutoEventWireup="true" CodeBehind="lista_de_chequeo.aspx.cs" Inherits="paginaWeb.paginas.lista_de_chequeo" %>

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
            <div class=" container">
                <div class=" row">
                    <div class="col">
                        <h1>
                            <asp:Label ID="label_nombre" Text="text" runat="server" />
                        </h1>
                        <h2>
                            <asp:Label ID="label_fecha" Text="text" runat="server" />
                        </h2>
                        <h2>
                            <asp:Label ID="label_turno" Text="text" runat="server" />
                        </h2>
                        <asp:Button ID="boton_pdf" Text="Generar PDF" CssClass="btn btn-primary" runat="server" OnClick="boton_pdf_Click" />
                        <asp:Button ID="boton_cerrar_turno" Text="Cerrar Turno" CssClass="btn btn-warning" OnClick="boton_cerrar_turno_Click" runat="server" />
                        <div class="alert alert-light">
                            <div class="row">
                                <div class="col">
                                    <div class=" input-group">
                                        <asp:Button ID="boton_encargado" Visible="false" CssClass="btn btn-primary" Text="Encargado" runat="server" OnClick="boton_encargado_Click" />
                                        <asp:Button ID="boton_encargado_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_encargado_pdf_Click" />
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="input-group">
                                        <asp:Button ID="boton_cajero" Visible="false" CssClass="btn btn-primary" Text="Cajero" runat="server" OnClick="boton_cajero_Click" />
                                        <asp:Button ID="boton_cajero_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_cajero_pdf_Click"/>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="input-group">
                                        <asp:Button ID="boton_shawarmero" Visible="false" CssClass="btn btn-primary" Text="Shawarmero" runat="server" OnClick="boton_shawarmero_Click" />
                                        <asp:Button ID="boton_shawarmero_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_shawarmero_pdf_Click"/>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="input-group">
                                        <asp:Button ID="boton_atencion" Visible="false" CssClass="btn btn-primary" Text="Area de Servicio" runat="server" OnClick="boton_atencion_Click" />
                                        <asp:Button ID="boton_atencion_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_atencion_pdf_Click"/>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="input-group">
                                        <asp:Button ID="boton_cocina" Visible="false" CssClass="btn btn-primary" Text="Cocinero" runat="server" OnClick="boton_cocina_Click" />
                                        <asp:Button ID="boton_cocina_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_cocina_pdf_Click"/>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="input-group">
                                        <asp:Button ID="boton_limpieza" Visible="false" CssClass="btn btn-primary" Text="Limpieza" runat="server" OnClick="boton_limpieza_Click" />
                                        <asp:Button ID="boton_limpieza_pdf" Visible="false" Text="PDF" CssClass="btn btn-success" runat="server" OnClick="boton_limpieza_pdf_Click"/>
                                    </div>
                                </div>
                            </div>
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Area" runat="server" />
                                <asp:DropDownList ID="dropDown_tipo" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_tipo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="input-group">
                                <asp:Label CssClass="form-control" Text="Categoria" runat="server" />
                                <asp:DropDownList ID="dropDown_categoria" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropDown_categoria_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <asp:GridView Caption="LISTA DE CHEQUEO" CaptionAlign="Top" runat="server" ID="gridview_chequeos" AutoGenerateColumns="false" CssClass="table table-dark   table-striped" OnRowDataBound="gridview_chequeos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="id" DataField="id" />
                                    <asp:BoundField HeaderText="actividad" DataField="actividad" />
                                    <asp:TemplateField HeaderText="Cargar">
                                        <ItemTemplate>
                                            <asp:TextBox ID="textbox_nota" placeholder="Ingrese Nota" CssClass="form-control" runat="server" OnTextChanged="textbox_nota_TextChanged" AutoPostBack="true" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cargar">
                                        <ItemTemplate>
                                            <asp:Button ID="boton_cargar" Text="Cargar" CssClass="btn btn-primary" runat="server" OnClick="boton_cargar_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Nota" DataField="nota" />
                                    <asp:BoundField HeaderText="id historial" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
