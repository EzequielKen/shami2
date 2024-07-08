
using paginaWeb;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static QuestPDF.Helpers.Colors;
using static System.Net.Mime.MediaTypeNames;


namespace _01___modulos
{
    public class cls_PDF
    {
        cls_funciones funciones = new cls_funciones();


        #region questPDF
        public void GenerarPDF(string ruta_archivo, byte[] logo, DataTable pedido, string proveedor_seleccionado, int fila_remito, DataTable remitos, DataTable usuario,string nota,string aumento)
        {

            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedor_seleccionado).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Parse(remitos.Rows[fila_remito]["fecha_remito"].ToString());
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("PEDIDO: " + remitos.Rows[fila_remito]["num_pedido"].ToString()).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("resumen de pedido").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("FECHA: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text("Datos del cliente").Underline().Bold();
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Franquicia: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["franquicia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Sucursal: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["sucursal"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Provincia: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["provincia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Localidad: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["localidad"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["direccion"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Nota: ").SemiBold().FontSize(10);
                                txt.Span(nota).FontSize(10);

                            });
                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Tipo producto").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Pedido").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Despachado").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Recibido").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Sub total").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["producto"].ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["tipo"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["pedido"].ToString() + pedido.Rows[fila]["unid.pedida"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["entregado"].ToString() + pedido.Rows[fila]["unid.entregada"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["recibido"].ToString() + pedido.Rows[fila]["unid.entregada"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(formatCurrency(double.Parse(pedido.Rows[fila]["precio"].ToString()))).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(formatCurrency(double.Parse(pedido.Rows[fila]["sub.total"].ToString()))).FontSize(10);
                            }
                        });

                        double total = double.Parse(remitos.Rows[fila_remito]["valor_remito"].ToString());
                        if (aumento!="0")
                        {
                            double totalOriginal = total; // Cambia esto al valor que necesites
                            double porcentajeIva = 1+ double.Parse(aumento)/100; // Porcentaje de IVA personalizado (por ejemplo, 15%)
                            
                            double totalSinIva = totalOriginal / porcentajeIva;
                            double diferencia = totalOriginal - totalSinIva;
                            col.Item().AlignRight().Text("Total Sin Impuesto:" + formatCurrency(totalSinIva)).FontSize(12);
                            col.Item().AlignRight().Text("Impuesto: %" + aumento +" "+ formatCurrency(diferencia)).FontSize(12);

                        }
                        col.Item().AlignRight().Text("TOTAL:" + formatCurrency(total)).FontSize(12);
                        col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_operativo(string ruta_archivo, byte[] logo, DataTable pedido, string proveedor_seleccionado, int fila_pedido, DataTable pedidos, DataTable usuario)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedor_seleccionado).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Parse(pedidos.Rows[fila_pedido]["fecha"].ToString());
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString() + "Hora: " + fecha_dato.Hour.ToString() + ":" + fecha_dato.Minute.ToString() + ":" + fecha_dato.Second.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("PEDIDO: " + pedidos.Rows[fila_pedido]["num_pedido"].ToString()).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("resumen de pedido").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("FECHA: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text("Datos del cliente").Underline().Bold();
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Franquicia: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["franquicia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Sucursal: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["sucursal"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Provincia: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["provincia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Localidad: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["localidad"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span(usuario.Rows[0]["direccion"].ToString()).FontSize(10);

                            });
                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("tipo producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("cantidad Pedida").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("unidad pedida").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["tipo"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["pedido"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(pedido.Rows[fila]["unid.pedida"].ToString()).FontSize(10);



                            }
                        });


                        col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        private int buscar_fila_sucural(string sucursal, DataTable lista_de_sucursales)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= lista_de_sucursales.Rows.Count - 1)
            {
                if (sucursal == lista_de_sucursales.Rows[fila]["sucursal"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public void GenerarPDF_remito_de_carga(string ruta_archivo, byte[] logo, DataTable resumen, DataTable pedido, DataTable sucursales)
        {
            int fila_sucursal = 0;
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                for (int fila_resumen = 0; fila_resumen <= resumen.Rows.Count - 1; fila_resumen++)
                {
                    document.Page(page =>
                    {
                        page.Margin(30);

                        page.Header().ShowOnce().Row(row =>
                        {

                            row.ConstantItem(150).Image(logo);

                            row.RelativeItem().Column(col =>
                            {
                                //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                                //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                            });

                            row.RelativeItem().Column(col =>
                            {

                            });
                        });




                        fila_sucursal = buscar_fila_sucural(resumen.Rows[fila_resumen]["sucursal"].ToString(), sucursales);
                        page.Content().PaddingVertical(10).Column(col =>
                        {


                            col.Item().AlignCenter().Text("Proveedor: " + resumen.Rows[fila_resumen]["proveedor"].ToString()).Bold().FontSize(14);


                            DateTime fecha_dato = DateTime.Parse(resumen.Rows[fila_resumen]["fecha_remito"].ToString());
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Column(col_clientes =>
                            {
                                col_clientes.Item().Text("Datos del cliente").Underline().Bold();
                                col_clientes.Item().Text(txt =>
                                {
                                    txt.Span("Franquicia: ").Bold().FontSize(14);
                                    txt.Span(sucursales.Rows[fila_sucursal]["franquicia"].ToString()).Bold().FontSize(14);

                                });
                                col_clientes.Item().Text(txt =>
                                {
                                    txt.Span("Sucursal: ").Bold().FontSize(14);
                                    txt.Span(sucursales.Rows[fila_sucursal]["sucursal"].ToString()).Bold().FontSize(14);

                                });
                                col_clientes.Item().Text(txt =>
                                {
                                    txt.Span("Provincia: ").Bold().FontSize(14);
                                    txt.Span(sucursales.Rows[fila_sucursal]["provincia"].ToString()).Bold().FontSize(14);

                                });
                                col_clientes.Item().Text(txt =>
                                {
                                    txt.Span("Localidad: ").Bold().FontSize(14);
                                    txt.Span(sucursales.Rows[fila_sucursal]["localidad"].ToString()).Bold().FontSize(14);

                                });
                                col_clientes.Item().Text(txt =>
                                {
                                    txt.Span("Direccion: ").Bold().FontSize(14);
                                    txt.Span(sucursales.Rows[fila_sucursal]["direccion"].ToString()).Bold().FontSize(14);

                                });
                            });

                            col.Item().LineHorizontal(0.5f);


                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("PEDIDO: " + resumen.Rows[fila_resumen]["num_pedido"].ToString()).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("resumen de pedido").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("FECHA: " + fecha).Bold().FontSize(14);


                            col.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    /*columns.RelativeColumn();
                                     columns.RelativeColumn();
                                     columns.RelativeColumn();*/
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#257272").Padding(2).Text("Tipo producto").FontColor("#fff");
                                    header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                    header.Cell().Background("#257272").Padding(2).Text("Despachado").FontColor("#fff");

                                    /*header.Cell().Background("#257272").Padding(2).Text("Recibido").FontColor("#fff");

                                      header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");
                                      header.Cell().Background("#257272").Padding(2).Text("Sub total").FontColor("#fff");*/
                                });

                                string sucur, sucurBD, num, numBD, A, B;
                                for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
                                {
                                    sucur = pedido.Rows[fila]["sucursal_seleccionada"].ToString();
                                    sucurBD = resumen.Rows[fila_resumen]["sucursal"].ToString();
                                    num = pedido.Rows[fila]["num_pedido"].ToString();
                                    numBD = resumen.Rows[fila_resumen]["num_pedido"].ToString();
                                    A = pedido.Rows[fila]["nombre_proveedor"].ToString();
                                    B = resumen.Rows[fila_resumen]["proveedor"].ToString(); //sucursal_seleccionada num_pedido nombre_proveedor
                                    if (pedido.Rows[fila]["sucursal_seleccionada"].ToString() == resumen.Rows[fila_resumen]["sucursal"].ToString() &&
                                        pedido.Rows[fila]["num_pedido"].ToString() == resumen.Rows[fila_resumen]["num_pedido"].ToString() &&
                                        pedido.Rows[fila]["nombre_proveedor"].ToString() == resumen.Rows[fila_resumen]["proveedor"].ToString())
                                    {
                                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                        .Text(pedido.Rows[fila]["tipo"].ToString()).FontSize(10);

                                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                        .Text(pedido.Rows[fila]["producto"].ToString()).FontSize(10);

                                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                        .Text(pedido.Rows[fila]["entregado"].ToString() + pedido.Rows[fila]["unid.entregada"].ToString()).FontSize(10);
                                    }

                                }
                            });
                            col.Item().LineHorizontal(0.5f);

                            col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Entrega conforme / firma:").FontSize(12);
                                column.Item().Text("recibe conforme / firma:").FontSize(12);
                                column.Item().Text("Canastos limpios:").FontSize(12);
                                column.Item().Text("Canastos sucios:").FontSize(12);
                                column.Spacing(20);
                            });

                            col.Spacing(10);
                        });


                        page.Footer().AlignRight().Text(txt =>
                        {
                            txt.Span("pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                    });
                }

            }).GeneratePdf(ruta_archivo);
        }

        public void GenerarPDF_orden_de_compra(string ruta_archivo, byte[] logo, DataTable orden_de_compra, DataTable proveedor, string num_orden, string fecha_pedido, string fecha_entrega_estimada)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedor.Rows[0]["proveedor"].ToString()).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Orden de Compra N°: " + num_orden).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Fecha Pedido: " + fecha_pedido).Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Fecha entrega estimada: " + fecha_entrega_estimada).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Proveedor: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["proveedor"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Provincia: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["provincia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Localidad: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["localidad"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["direccion"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Telefono: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["telefono"].ToString()).FontSize(10);

                            });
                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad Pedida").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad Entregada").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Unidad Pedida").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= orden_de_compra.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["cantidad_pedida"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["cantidad"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["unidad de medida"].ToString()).FontSize(10);

                            }
                        });

                        col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_orden_de_compra_con_precio(string ruta_archivo, byte[] logo, DataTable orden_de_compra, DataTable proveedor, string num_orden, string fecha_pedido, string fecha_entrega_estimada, string total, string impuestos)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedor.Rows[0]["proveedor"].ToString()).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Orden de Compra N°: " + num_orden).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Fecha Pedido: " + fecha_pedido).Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Fecha entrega estimada: " + fecha_entrega_estimada).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Proveedor: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["proveedor"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Provincia: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["provincia"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Localidad: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["localidad"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Direccion: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["direccion"].ToString()).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Telefono: ").SemiBold().FontSize(10);
                                txt.Span(proveedor.Rows[0]["telefono"].ToString()).FontSize(10);

                            });
                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad Recibida").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("unidad de medida").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Sub Total").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= orden_de_compra.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["cantidad"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["unidad de medida"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["precio"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(orden_de_compra.Rows[fila]["sub_total"].ToString()).FontSize(10);


                            }
                        });
                        col.Item().AlignRight().Text("TOTAL: " + funciones.formatCurrency(total)).FontSize(12);
                        if (impuestos != "N/A")
                        {
                            double total_factura = double.Parse(total);
                            double impuestos_factura = double.Parse(impuestos);
                            col.Item().AlignRight().Text("Impuestos:" + funciones.formatCurrency(impuestos_factura)).FontSize(12);
                            total_factura = total_factura + impuestos_factura;
                            col.Item().AlignRight().Text("TOTAL FINAL:" + funciones.formatCurrency(total_factura)).FontSize(12);

                        }
                        col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }

        public void GenerarPDF_resumen_de_pedidos(string ruta_archivo, byte[] logo, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Resumen de pedidos.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Resumen de pedidos.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                for (int columna = resumen.Columns["total"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            tabla.Header(header =>
                            {
                                for (int columna = resumen.Columns["total"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    header.Cell().Background("#257272").Padding(2).Text(resumen.Columns[columna].ColumnName.ToString()).FontColor("#fff");
                                }

                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {
                                for (int columna = resumen.Columns["total"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                    .Text(resumen.Rows[fila][columna].ToString()).FontSize(10);
                                }
                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }

        public void GenerarPDF_pedido_de_panificados(string ruta_archivo, byte[] logo, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Pedido de Panes.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Pedido de Panes.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Total").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Unidad de Medida").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["total"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["unidad de medida"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["producto"].ToString()).FontSize(10);

                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }

        public void GenerarPDF_reporteria(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable productos_proveedor, DataTable proveedorBD, string promedio_ganacia)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedorBD.Rows[0]["nombre_proveedor"].ToString()).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("ACUERDO DE PRECIO: " + acuerdo).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Resumen de precios").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text(fecha_acuerdo).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text("Datos relevantes:").Underline().Bold();
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(promedio_ganacia).FontSize(15).Bold();

                            });

                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Unidad medida").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Precio compra").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Precio venta").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Ganancia en $").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Ganancia en %").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["producto"].ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["unidad_de_medida_fabrica"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["precio_compra"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["precio_venta"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["ganancia_pesos"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["ganancia_porcentaje"].ToString()).FontSize(10);
                            }
                        });

                        /*if (IsNotDBNull(remitos.Rows[fila_remito]["descuento"]))
                        {
                            if (remitos.Rows[fila_remito]["descuento"].ToString() == "0" || remitos.Rows[fila_remito]["descuento"].ToString() == "")
                            {
                                col.Item().AlignRight().Text("TOTAL:" + formatCurrency(total)).FontSize(12);



                            }
                            else
                            {
                                decimal descuento = decimal.Parse(remitos.Rows[fila_remito]["descuento"].ToString());
                                decimal valor_pedido = total + descuento;

                                col.Item().AlignRight().Text("VALOR PEDIDO:" + formatCurrency(valor_pedido)).FontSize(12);
                                col.Item().AlignRight().Text("DESCUENTO:" + formatCurrency(descuento)).FontSize(12);
                                col.Item().AlignRight().Text("TOTAL PEDIDO:" + formatCurrency(total)).FontSize(12);
                            }
                        }
                        else
                        {

                        }*/


                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_reporteria_lista_de_precios(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable productos_proveedor, DataTable proveedorBD)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("proveedor: " + proveedorBD.Rows[0]["nombre_proveedor"].ToString()).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("LISTA DE PRECIO: " + acuerdo).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Resumen de precios").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text(fecha_acuerdo).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        /*  col.Item().Column(col_clientes =>
                          {
                              col_clientes.Item().Text("Datos de cliente").Underline().Bold();
                              col_clientes.Item().Text(txt =>
                              {
                                  txt.Span("Franquicia: ").SemiBold().FontSize(10);
                                  txt.Span(usuario.Rows[0]["franquicia"].ToString()).FontSize(10);

                              });
                              col_clientes.Item().Text(txt =>
                              {
                                  txt.Span("Sucursal: ").SemiBold().FontSize(10);
                                  txt.Span(usuario.Rows[0]["sucursal"].ToString()).FontSize(10);

                              });
                              col_clientes.Item().Text(txt =>
                              {
                                  txt.Span("Provincia: ").SemiBold().FontSize(10);
                                  txt.Span(usuario.Rows[0]["provincia"].ToString()).FontSize(10);

                              });
                              col_clientes.Item().Text(txt =>
                              {
                                  txt.Span("Localidad: ").SemiBold().FontSize(10);
                                  txt.Span(usuario.Rows[0]["localidad"].ToString()).FontSize(10);

                              });
                              col_clientes.Item().Text(txt =>
                              {
                                  txt.Span("Direccion: ").SemiBold().FontSize(10);
                                  txt.Span(usuario.Rows[0]["direccion"].ToString()).FontSize(10);

                              });
                          });*/

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Unidad medida").FontColor("#fff");


                                header.Cell().Background("#257272").Padding(2).Text("Precio venta").FontColor("#fff");


                            });

                            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["producto"].ToString()).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["unidad_de_medida_fabrica"].ToString()).FontSize(10);




                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos_proveedor.Rows[fila]["precio_venta"].ToString()).FontSize(10);


                            }
                        });

                        /*if (IsNotDBNull(remitos.Rows[fila_remito]["descuento"]))
                        {
                            if (remitos.Rows[fila_remito]["descuento"].ToString() == "0" || remitos.Rows[fila_remito]["descuento"].ToString() == "")
                            {
                                col.Item().AlignRight().Text("TOTAL:" + formatCurrency(total)).FontSize(12);



                            }
                            else
                            {
                                decimal descuento = decimal.Parse(remitos.Rows[fila_remito]["descuento"].ToString());
                                decimal valor_pedido = total + descuento;

                                col.Item().AlignRight().Text("VALOR PEDIDO:" + formatCurrency(valor_pedido)).FontSize(12);
                                col.Item().AlignRight().Text("DESCUENTO:" + formatCurrency(descuento)).FontSize(12);
                                col.Item().AlignRight().Text("TOTAL PEDIDO:" + formatCurrency(total)).FontSize(12);
                            }
                        }
                        else
                        {

                        }*/


                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_rendiciones(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable proveedorBD, DataTable resumen, DataTable rendiciones_usuario)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A3);
                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Proveedor: " + proveedorBD.Rows[0]["nombre_proveedor"].ToString()).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("ACUERDO DE PRECIO: " + acuerdo).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Resumen de precios").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text(fecha_acuerdo).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text("Resumen").Underline().Bold();
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["dinero_disponible"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["compra_de_vegetales"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["venta_de_vegetales"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["gasto_en_nafta"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["venta_cajones"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["compra_cajones"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["egresos"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["pagos_de_locales"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["rendicion_teorico"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["diferencia"].ToString()).FontSize(15).Bold();

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span(resumen.Rows[0]["nuevo_disponible"].ToString()).FontSize(15).Bold();

                            });
                        });

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272").Padding(2).Text("Detalle").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Unid. medida").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Concepto").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Valor").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Stock inicial").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Stock final").FontColor("#fff");

                                header.Cell().Background("#257272").Padding(2).Text("Fecha").FontColor("#fff");
                            });

                            for (int fila = 0; fila <= rendiciones_usuario.Rows.Count - 1; fila++)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["detalle"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["cantidad"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["unidad_de_medida"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["concepto"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["precio"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["valor"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["stock_inicial"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["stock_final"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(rendiciones_usuario.Rows[fila]["fecha"].ToString()).FontSize(10);
                            }
                        });

                        /*if (IsNotDBNull(remitos.Rows[fila_remito]["descuento"]))
                        {
                            if (remitos.Rows[fila_remito]["descuento"].ToString() == "0" || remitos.Rows[fila_remito]["descuento"].ToString() == "")
                            {
                                col.Item().AlignRight().Text("TOTAL:" + formatCurrency(total)).FontSize(12);



                            }
                            else
                            {
                                decimal descuento = decimal.Parse(remitos.Rows[fila_remito]["descuento"].ToString());
                                decimal valor_pedido = total + descuento;

                                col.Item().AlignRight().Text("VALOR PEDIDO:" + formatCurrency(valor_pedido)).FontSize(12);
                                col.Item().AlignRight().Text("DESCUENTO:" + formatCurrency(descuento)).FontSize(12);
                                col.Item().AlignRight().Text("TOTAL PEDIDO:" + formatCurrency(total)).FontSize(12);
                            }
                        }
                        else
                        {

                        }*/


                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }

        public void GenerarPDF_historial_produccion(string ruta_archivo, byte[] logo, int fila_historial, DataTable historial, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Historial De Produccion.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            string proveedor = historial.Rows[fila_historial]["proveedor"].ToString();
                            string receptor = historial.Rows[fila_historial]["receptor"].ToString();
                            DateTime fecha_dato = DateTime.Now;
                            string fecha_imprimir = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Proveedor: " + proveedor).Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Receptor: " + receptor).Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha_imprimir).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        string fecha = historial.Rows[fila_historial]["fecha"].ToString();
                        string id = historial.Rows[fila_historial]["id"].ToString();
                        string proveedor = historial.Rows[fila_historial]["proveedor"].ToString();
                        string receptor = historial.Rows[fila_historial]["receptor"].ToString();
                        string estado = historial.Rows[fila_historial]["estado"].ToString();
                        col.Item().Column(col_clientes =>
                        {
                            col_clientes.Item().Text("Datos de produccion").Underline().Bold();
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Proveedor: ").SemiBold().FontSize(10);
                                txt.Span(proveedor).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Receptor: ").SemiBold().FontSize(10);
                                txt.Span(receptor).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Fecha: ").SemiBold().FontSize(10);
                                txt.Span(fecha).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("Estado (al momento de imprimir): ").SemiBold().FontSize(10);
                                txt.Span(estado).FontSize(10);

                            });
                            col_clientes.Item().Text(txt =>
                            {
                                txt.Span("id de produccion: ").SemiBold().FontSize(10);
                                txt.Span(id).FontSize(10);

                            });
                        });
                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                for (int columna = 0; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            tabla.Header(header =>
                            {
                                for (int columna = 0; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    header.Cell().Background("#257272").Padding(2).Text(resumen.Columns[columna].ColumnName.ToString()).FontColor("#fff");
                                }

                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {
                                for (int columna = 0; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                    .Text(resumen.Rows[fila][columna].ToString()).FontSize(10);
                                }
                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_estadisticas_de_pedidos(string ruta_archivo, byte[] logo, DataTable resumen, string fecha)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Estadisticas de pedidos.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Estadisticas de pedidos.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Mes y año: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {

                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                //Tipo producto
                                header.Cell().Background("#257272").Padding(2).Text("Tipo producto").FontColor("#fff");

                                //Productos
                                header.Cell().Background("#257272").Padding(2).Text("Productos").FontColor("#fff");

                                //Total pedido locales
                                header.Cell().Background("#257272").Padding(2).Text("Total pedido locales").FontColor("#fff");

                                //Promedio historico
                                header.Cell().Background("#257272").Padding(2).Text("Promedio historico").FontColor("#fff");

                                //Unidad medida
                                header.Cell().Background("#257272").Padding(2).Text("Unidad medida").FontColor("#fff");


                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["tipo_producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["totales_del_mes"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["promedio_del_mes"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["unidad_de_medida_fabrica"].ToString()).FontSize(10);

                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_plantilla_de_stock(string ruta_archivo, byte[] logo, DataTable productos, string tipo_stock)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text(tipo_stock).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Plantilla de stock.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Fecha: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {

                                columns.ConstantColumn(50);
                                columns.ConstantColumn(100);
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                //id
                                header.Cell().Background("#257272").Padding(2).Text("id").FontColor("#fff");

                                //Tipo producto
                                header.Cell().Background("#257272").Padding(2).Text("Tipo producto").FontColor("#fff");

                                //Productos
                                header.Cell().Background("#257272").Padding(2).Text("Productos").FontColor("#fff");

                                //stock
                                header.Cell().Background("#257272").Padding(2).Text("Stock").FontColor("#fff");


                            });

                            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["id"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["tipo_producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text("").FontSize(10);

                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_objetivo_de_produccion(string ruta_archivo, byte[] logo, string tipo_producto, DataTable productos)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text(tipo_producto).Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Objetivo de Produccion.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                //Productos
                                header.Cell().Background("#257272").Padding(2).Text("Productos").FontColor("#fff");

                                //Promedio historico
                                header.Cell().Background("#257272").Padding(2).Text("Promedio historico").FontColor("#fff");

                                //% Incremento
                                header.Cell().Background("#257272").Padding(2).Text("% Incremento").FontColor("#fff");

                                //Stock Actual
                                header.Cell().Background("#257272").Padding(2).Text("Stock Actual").FontColor("#fff");

                                //Objetivo de produccion
                                header.Cell().Background("#257272").Padding(2).Text("Objetivo de produccion").FontColor("#fff");

                                //Unidad medida
                                header.Cell().Background("#257272").Padding(2).Text("Unidad medida").FontColor("#fff");

                            });

                            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["promedio_pedido"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["incremento"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["stock"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["objetivo_produccion"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["unidad_de_medida_fabrica"].ToString()).FontSize(10);

                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_analisis_de_venta(string ruta_archivo, byte[] logo, DataTable productos, string total)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Analisis de venta").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Analisis de venta mes Diciembre.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                //Productos
                                header.Cell().Background("#257272").Padding(2).Text("Productos").FontColor("#fff");

                                //unidad_medida
                                header.Cell().Background("#257272").Padding(2).Text("Unidad Medida").FontColor("#fff");

                                //cantidad_vendida
                                header.Cell().Background("#257272").Padding(2).Text("Cantidad Vendida").FontColor("#fff");

                                //sub_total
                                header.Cell().Background("#257272").Padding(2).Text("Sub total").FontColor("#fff");

                            });

                            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["producto"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["unidad_medida"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(productos.Rows[fila]["cantidad_vendida"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(formatCurrency(double.Parse(productos.Rows[fila]["sub_total"].ToString()))).FontSize(10);

                            }


                        });

                        col.Item().AlignRight().Text("TOTAL:" + formatCurrency(double.Parse(total))).FontSize(12);
                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_lista_de_precios(string ruta_archivo, byte[] logo, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Lista de precios.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Lista de precios.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {

                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {

                                header.Cell().Background("#257272").Padding(2).Text("id").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#257272").Padding(2).Text("Precio").FontColor("#fff");


                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                .Text(resumen.Rows[fila]["id"].ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                 .Text(resumen.Rows[fila]["producto"].ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                 .Text(resumen.Rows[fila]["precio"].ToString()).FontSize(10);

                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        public void GenerarPDF_resumen_de_pedidos_de_insumos_a_expedicion(string ruta_archivo, byte[] logo, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);//Landscape()

                    page.Margin(30);
                    page.Header().ShowOnce().Row(row =>
                    {

                        row.ConstantItem(150).Image(logo);

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Resumen de pedidos.").Bold().FontSize(14);
                            //col.Item().AlignCenter().Text(direccion_seleccionado).FontSize(9);
                            //col.Item().AlignCenter().Text(telefono_seleccionado).FontSize(9);

                        });

                        row.RelativeItem().Column(col =>
                        {
                            DateTime fecha_dato = DateTime.Now;
                            string fecha = fecha_dato.Day.ToString() + "/" + fecha_dato.Month.ToString() + "/" + fecha_dato.Year.ToString();

                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Shami Shawarma").Bold().FontSize(14);
                            col.Item().Background("#257272").Border(1).BorderColor("#257272").AlignCenter().Text("Resumen de pedidos.").Bold().FontSize(14).FontColor("#fff");
                            col.Item().Border(1).BorderColor("#257272").AlignCenter().Text("Impreso el: " + fecha).Bold().FontSize(14);

                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {

                        col.Item().LineHorizontal(0.5f);

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                for (int columna = resumen.Columns["id"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            tabla.Header(header =>
                            {
                                for (int columna = resumen.Columns["id"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    header.Cell().Background("#257272").Padding(2).Text(resumen.Columns[columna].ColumnName.ToString()).FontColor("#fff");
                                }

                            });

                            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
                            {
                                for (int columna = resumen.Columns["id"].Ordinal; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(2)
                                    .Text(resumen.Rows[fila][columna].ToString()).FontSize(10);
                                }
                            }


                        });


                        /*col.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Entrega conforme / firma:").FontSize(12);
                            column.Item().Text("recibe conforme / firma:").FontSize(12);
                            column.Spacing(20);
                        });*/

                        col.Spacing(10);
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(ruta_archivo);
        }
        #endregion

        private string formatCurrency(object valor)
        {
            return string.Format("{0:C2}", valor);
        }

        private bool IsNotDBNull(object valor)
        {
            bool retorno = false;
            if (!DBNull.Value.Equals(valor))
            {
                retorno = true;
            }
            return retorno;
        }
    }
}