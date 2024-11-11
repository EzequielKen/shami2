using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Configuration;
using System.Data;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_crear_orden_de_compras
    {
        public cls_crear_orden_de_compras(DataTable usuario_BD)
        {
            usuarioBD = usuario_BD;
            servidor = usuarioBD.Rows[0]["servidor"].ToString();
            puerto = usuarioBD.Rows[0]["puerto"].ToString();
            usuario_dato = usuarioBD.Rows[0]["usuario_BD"].ToString();
            usuario_dato = usuarioBD.Rows[0]["usuario_BD"].ToString();
            contraseña_BD = usuarioBD.Rows[0]["contraseña_BD"].ToString();
            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }
            consultas = new cls_consultas_Mysql(servidor, puerto, usuario_dato, contraseña_BD, base_de_datos);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_whatsapp whatsapp = new cls_whatsapp();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        cls_funciones funciones = new cls_funciones();
        DataTable linkeo_de_insumos;
        DataTable insumos_fabrica;
        DataTable insumos_proveedor;
        DataTable proveedor;
        DataTable ordenes_de_compra;
        DataTable orden_de_pedido;
        DataTable linkeo_de_insumos_activo;
        #endregion

        #region metodos PDF
        public void crear_pdf_orden_de_compra(string ruta_archivo, byte[] logo, DataTable orden_de_compra, string id_proveedor, string fecha_entrega_estimada, string total)
        {
            consultar_proveedor_fabrica(id_proveedor);
            string num_orden = obtener_numero_orden_de_compra();
            string fecha_pedido = funciones.get_fecha();
            PDF.GenerarPDF_orden_de_compra_con_precio(ruta_archivo, logo, orden_de_compra, proveedor, num_orden, fecha_pedido, fecha_entrega_estimada, total, "N/A");
        }
        #endregion

        #region metodos carga a base de datos
        public string crear_orden_de_compra(string id_proveedor, string fecha_estimada, DataTable resumen, string id_orden_pedido_a_modificar, DataTable resumen_orden_pedido, string cantidad_a_pagar, string condicion_pago)
        {
            if (id_orden_pedido_a_modificar != "N/A")
            {
                consultar_ordenes_de_compra();
                int ultima_fila_compras = ordenes_de_compra.Rows.Count - 1;
                int nuevo_num_orden_de_compra = int.Parse(ordenes_de_compra.Rows[ultima_fila_compras]["id"].ToString()) + 1;

                consultar_orden_de_pedido();
                int fila_orden_pedido = funciones.buscar_fila_por_id(id_orden_pedido_a_modificar, orden_de_pedido);
                string id_producto;
                DateTime fecha_actual = DateTime.Now;
                string id_dato, producto_dato, cantidad, unidad, estado, num_orden, fecha, dato_a_cargar, actualizar, nomb_columna;
                int columna;
                int fila_resumen;
                for (int fila = 0; fila <= resumen_orden_pedido.Rows.Count - 1; fila++)
                {
                    id_producto = resumen_orden_pedido.Rows[fila]["id"].ToString();
                    fila_resumen = funciones.buscar_fila_por_id(id_producto, resumen);
                    columna = orden_de_pedido.Columns["producto_1"].Ordinal;
                    if (fila_resumen != -1)
                    {
                        while (columna <= orden_de_pedido.Columns.Count - 1)
                        {
                            if (orden_de_pedido.Rows[fila_orden_pedido][columna].ToString() != "N/A")
                            {
                                if (id_producto == funciones.obtener_dato(orden_de_pedido.Rows[fila_orden_pedido][columna].ToString(), 1))
                                {
                                    id_dato = funciones.obtener_dato(orden_de_pedido.Rows[fila_orden_pedido][columna].ToString(), 1);
                                    producto_dato = funciones.obtener_dato(orden_de_pedido.Rows[fila_orden_pedido][columna].ToString(), 2);
                                    cantidad = funciones.obtener_dato(orden_de_pedido.Rows[fila_orden_pedido][columna].ToString(), 3);
                                    unidad = funciones.obtener_dato(orden_de_pedido.Rows[fila_orden_pedido][columna].ToString(), 4);
                                    estado = "Pedido";
                                    num_orden = nuevo_num_orden_de_compra.ToString();
                                    fecha = fecha_actual.ToString("dd/MM/yyyy");

                                    dato_a_cargar = id_dato + "-" + producto_dato + "-" + cantidad + "-" + unidad + "-" + estado + "-" + num_orden + "-" + fecha;

                                    nomb_columna = orden_de_pedido.Columns[columna].ColumnName;

                                    actualizar = "`" + nomb_columna + "` = '" + dato_a_cargar + "'";
                                    consultas.actualizar_tabla(base_de_datos, "orden_de_pedido", actualizar, id_orden_pedido_a_modificar);
                                }
                            }
                            columna++;
                        }
                    }
                }

            }
            consultar_proveedor_fabrica(id_proveedor);
            consular_acuerdo_de_precio_fabrica_proveedor(id_proveedor);

            string columnas = "";
            string valores = "";
            //id_proveedor
            columnas = funciones.armar_query_columna(columnas, "id_proveedor", false);
            valores = funciones.armar_query_valores(valores, proveedor.Rows[0]["id"].ToString(), false);
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", false);
            valores = funciones.armar_query_valores(valores, proveedor.Rows[0]["proveedor"].ToString(), false);
            //acuerdo_de_precios
            columnas = funciones.armar_query_columna(columnas, "acuerdo_de_precios", false);
            valores = funciones.armar_query_valores(valores, linkeo_de_insumos.Rows[0]["acuerdo"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //fecha_entrega_estimada
            columnas = funciones.armar_query_columna(columnas, "fecha_entrega_estimada", false);
            valores = funciones.armar_query_valores(valores, fecha_estimada, false);
            //condicion_pago
            columnas = funciones.armar_query_columna(columnas, "condicion_pago", false);
            valores = funciones.armar_query_valores(valores, condicion_pago, false);
            //cantidad_a_pagar tipo_de_pago
            columnas = funciones.armar_query_columna(columnas, "cantidad_a_pagar", false);
            valores = funciones.armar_query_valores(valores, cantidad_a_pagar, false);
            //legacy
            columnas = funciones.armar_query_columna(columnas, "legacy", false);
            valores = funciones.armar_query_valores(valores, "no", false);
            //producto_1
            int index = 1;
            string dato, id, producto, precio, tipo_paquete, cantidad_unidades, unidad_medida, cantidad_pedida;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                producto = resumen.Rows[fila]["producto"].ToString();
                precio = resumen.Rows[fila]["precio_dato"].ToString();

                tipo_paquete = resumen.Rows[fila]["tipo_paquete"].ToString();
                cantidad_unidades = resumen.Rows[fila]["cantidad_unidades"].ToString();
                unidad_medida = resumen.Rows[fila]["unidad_medida"].ToString();


                cantidad_pedida = resumen.Rows[fila]["cantidad"].ToString();

                dato = id + "-" + producto + "-" + precio + "-" + tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + cantidad_pedida + "-" + "N/A";
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);
                index++;
            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            producto = resumen.Rows[ultima_fila]["producto"].ToString();
            precio = resumen.Rows[ultima_fila]["precio_dato"].ToString();

            tipo_paquete = resumen.Rows[ultima_fila]["tipo_paquete"].ToString();
            cantidad_unidades = resumen.Rows[ultima_fila]["cantidad_unidades"].ToString();
            unidad_medida = resumen.Rows[ultima_fila]["unidad_medida"].ToString();

            cantidad_pedida = resumen.Rows[ultima_fila]["cantidad"].ToString();

            dato = id + "-" + producto + "-" + precio + "-" + tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + cantidad_pedida + "-" + "N/A";
            columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "ordenes_de_compra", columnas, valores);
            if (verificar_si_hay_nuevo_precio(resumen))
            {
                int nuevo_acuerdo = actualizar_acuerdo_de_precio(id_proveedor, resumen);
                actualizar_acuerdo_en_ordenes_de_compra(id_proveedor, nuevo_acuerdo);
            }
            return whatsapp.notificar_nueva_orden_de_compra();
        }
        private void actualizar_acuerdo_en_ordenes_de_compra(string id_proveedor, int nuevo_acuerdo)
        {
            ordenes_de_compra = consultas.consultar_ordenes_de_compra_de_proveedor_abiertas(id_proveedor);
            string id_orden;
            string actualizar;
            for (int fila = 0; fila <= ordenes_de_compra.Rows.Count - 1; fila++)
            {
                id_orden = ordenes_de_compra.Rows[fila]["id"].ToString();
                actualizar = "`acuerdo_de_precios` = '" + nuevo_acuerdo.ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
            }

        }
        private bool verificar_si_hay_nuevo_precio(DataTable orden_de_compra)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= orden_de_compra.Rows.Count - 1)
            {
                if (orden_de_compra.Rows[fila]["precio_unidad_nuevo"].ToString() != "N/A")
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void desactivar_acuerdo_de_precios_fabrica_a_proveedores_activo(string id_acuerdo)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", actualizar, id_acuerdo);
        }
        private int actualizar_acuerdo_de_precio(string id_proveedor, DataTable orden_de_compra)
        {
            linkeo_de_insumos_activo = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_activo(base_de_datos, "linkeo_de_insumos", id_proveedor);
            for (int fila = 0; fila <= linkeo_de_insumos_activo.Rows.Count-1; fila++)
            {
                desactivar_acuerdo_de_precios_fabrica_a_proveedores_activo(linkeo_de_insumos_activo.Rows[fila]["id"].ToString());
            }
            string columnas = "";
            string valores = "";
            string dato, id_producto, producto, paquete, cant_unidad, unidad, precio;
            int fila_producto;
            int index = 1;
            int acuerdo=1;
            for (int fila = 0; fila <= linkeo_de_insumos.Rows.Count - 1; fila++)
            {

                //id_proveedor
                columnas = funciones.armar_query_columna(columnas, "id_proveedor", false);
                valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["id_proveedor"].ToString(), false);

                //proveedor
                columnas = funciones.armar_query_columna(columnas, "proveedor", false);
                valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["proveedor"].ToString(), false);
                //acuerdo
                acuerdo = int.Parse(linkeo_de_insumos_activo.Rows[fila]["acuerdo"].ToString()) + 1;

                columnas = funciones.armar_query_columna(columnas, "acuerdo", false);
                valores = funciones.armar_query_valores(valores, acuerdo.ToString(), false);
                //fecha
                columnas = funciones.armar_query_columna(columnas, "fecha", false);
                valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);

                //id_producto
                columnas = funciones.armar_query_columna(columnas, "id_producto", false);
                valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["id_producto"].ToString(), false);

                //producto
                columnas = funciones.armar_query_columna(columnas, "producto", false);
                valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["producto"].ToString(), false);

                //presentacion
                columnas = funciones.armar_query_columna(columnas, "presentacion", false);
                valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["presentacion"].ToString(), false);


                fila_producto = funciones.buscar_fila_por_id(linkeo_de_insumos_activo.Rows[fila]["id_producto"].ToString(), orden_de_compra);
                if (fila_producto != -1)
                {

                    if (orden_de_compra.Rows[fila_producto]["precio_unidad_nuevo"].ToString() != "N/A")
                    {
                        precio = orden_de_compra.Rows[fila_producto]["precio_unidad_nuevo"].ToString();
                        //precio
                        columnas = funciones.armar_query_columna(columnas, "precio", true);
                        valores = funciones.armar_query_valores(valores, precio, true);

                    }
                    else
                    {
                        //precio
                        columnas = funciones.armar_query_columna(columnas, "precio", true);
                        valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["precio"].ToString(), true);

                    }
                }
                else
                {
                    //precio
                    columnas = funciones.armar_query_columna(columnas, "precio", true);
                    valores = funciones.armar_query_valores(valores, linkeo_de_insumos_activo.Rows[fila]["precio"].ToString(), true);
                }

                consultas.insertar_en_tabla(base_de_datos, "linkeo_de_insumos", columnas, valores);
                columnas = string.Empty;
                valores = string.Empty;
            }

            return acuerdo;
        }

        #endregion

        #region metodos privados
        private string obtener_numero_orden_de_compra()
        {
            consultar_ordenes_de_compra();
            int ultima_fila = ordenes_de_compra.Rows.Count - 1;
            int num_orden = int.Parse(ordenes_de_compra.Rows[ultima_fila]["id"].ToString()) + 1;
            return num_orden.ToString();
        }
        private void crear_tabla_insumos()
        {
            insumos_proveedor = new DataTable();

            insumos_proveedor.Columns.Add("id");
            insumos_proveedor.Columns.Add("producto");
            insumos_proveedor.Columns.Add("tipo_producto");

            insumos_proveedor.Columns.Add("tipo_paquete");
            insumos_proveedor.Columns.Add("cantidad_unidades");
            insumos_proveedor.Columns.Add("unidad_medida");

            insumos_proveedor.Columns.Add("presentacion");
            insumos_proveedor.Columns.Add("precio");
            insumos_proveedor.Columns.Add("nuevo_precio");
            insumos_proveedor.Columns.Add("precio_unidad_nuevo");
        }
        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            string id_insumo, tipo_paquete, cantidad_unidades, unidad_medida, precio;
            int fila_insumo;
            if (linkeo_de_insumos.Rows.Count > 0)
            {
                for (int fila = 0; fila <= linkeo_de_insumos.Rows.Count - 1; fila++)
                {

                    id_insumo = linkeo_de_insumos.Rows[fila]["id_producto"].ToString();
                    tipo_paquete = funciones.obtener_dato(linkeo_de_insumos.Rows[fila]["presentacion"].ToString(), 1);
                    cantidad_unidades = funciones.obtener_dato(linkeo_de_insumos.Rows[fila]["presentacion"].ToString(), 2);
                    unidad_medida = funciones.obtener_dato(linkeo_de_insumos.Rows[fila]["presentacion"].ToString(), 3);
                    precio = linkeo_de_insumos.Rows[fila]["precio"].ToString();

                    fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabrica);
                    if (fila_insumo != -1)
                    {

                        insumos_proveedor.Rows.Add();
                        int ultima_fila = insumos_proveedor.Rows.Count - 1;
                        insumos_proveedor.Rows[ultima_fila]["id"] = insumos_fabrica.Rows[fila_insumo]["id"].ToString();
                        insumos_proveedor.Rows[ultima_fila]["producto"] = insumos_fabrica.Rows[fila_insumo]["producto"].ToString();
                        insumos_proveedor.Rows[ultima_fila]["tipo_producto"] = insumos_fabrica.Rows[fila_insumo]["tipo_producto"].ToString();
                        insumos_proveedor.Rows[ultima_fila]["tipo_paquete"] = tipo_paquete;
                        insumos_proveedor.Rows[ultima_fila]["cantidad_unidades"] = cantidad_unidades;
                        insumos_proveedor.Rows[ultima_fila]["unidad_medida"] = unidad_medida;
                        if (tipo_paquete == "Unidad")
                        {
                            insumos_proveedor.Rows[ultima_fila]["presentacion"] = cantidad_unidades + " x " + unidad_medida;
                        }
                        else
                        {
                            insumos_proveedor.Rows[ultima_fila]["presentacion"] = tipo_paquete + " x " + cantidad_unidades + " " + unidad_medida;
                        }
                        insumos_proveedor.Rows[ultima_fila]["precio"] = precio;
                        insumos_proveedor.Rows[ultima_fila]["nuevo_precio"] = "N/A";
                        insumos_proveedor.Rows[ultima_fila]["precio_unidad_nuevo"] = "N/A";

                    }
                }
            }
        }

        #endregion

        #region metodos consultas
        private void consultar_orden_de_pedido()
        {
            orden_de_pedido = consultas.consultar_tabla_completa(base_de_datos, "orden_de_pedido");
        }
        private void consultar_ordenes_de_compra()
        {
            ordenes_de_compra = consultas.consultar_tabla_completa(base_de_datos, "ordenes_de_compra");
        }
        private void consultar_proveedor_fabrica(string id_proveedor)
        {
            proveedor = consultas.consultar_proveeedor_de_fabrica_por_id(id_proveedor);
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_insumos_fabricas_optimizado();
        }
        private void consular_acuerdo_de_precio_fabrica_proveedor(string id_proveedor)
        {
            linkeo_de_insumos = consultas.consultar_acuerdo_de_precio_fabrica_proveedor(base_de_datos, "linkeo_de_insumos", id_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_proveedor(string id_proveedor)
        {
            consular_acuerdo_de_precio_fabrica_proveedor(id_proveedor);
            consultar_insumos_fabrica();
            llenar_tabla_insumos();
            return insumos_proveedor;
        }
        public DataTable get_insumos_proveedor_optimizado(string id_proveedor, DataTable insumos_fabricaBD)
        {
            consular_acuerdo_de_precio_fabrica_proveedor(id_proveedor);
            insumos_fabrica = insumos_fabricaBD;
            llenar_tabla_insumos();
            return insumos_proveedor;
        }
        public DataTable get_proveedor(string id_proveedor)
        {
            consultar_proveedor_fabrica(id_proveedor);
            return proveedor;
        }
        #endregion


    }
}
