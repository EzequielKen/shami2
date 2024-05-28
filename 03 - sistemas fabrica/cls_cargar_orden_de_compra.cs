using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_cargar_orden_de_compra
    {
        public cls_cargar_orden_de_compra(DataTable usuario_BD)
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
            stock_Insumos = new cls_stock_insumos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_stock_insumos stock_Insumos;
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos_fabrica;
        DataTable insumos_fabrica_copia;
        DataTable cuenta_por_pagar_fabrica;
        DataTable acuerdo_de_precios_fabrica_a_proveedores;
        DataTable acuerdo_de_precios_fabrica_a_proveedores_de_pedido;
        DataTable acuerdo_de_precios_fabrica_a_proveedores_activo;
        DataTable orden_de_compraBD;
        DataTable orden_de_compra;
        #endregion

        #region carga a base de datos
        public void actualizar_orden_de_compra(string id_orden, DataTable orden_de_compra, string id_proveedor, string valor_orden, string nombre_fabrica, string nombre_proveedor, string total_impuestos, string rol_usuario, string estado_pedido, string condicion_pago, string fecha_factura,string estado_entrega)
        {
            actualizar_valor_de_orden_de_compra(id_orden, id_proveedor, valor_orden, nombre_fabrica, nombre_proveedor, total_impuestos, condicion_pago, fecha_factura, estado_entrega);
            consultar_orden_de_compra(id_orden);
            consultar_insumos_fabrica();
            string actualizar;

            actualizar = "`fecha_entrega` = '" + funciones.get_fecha() + "'";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);


            actualizar = "`estado` = 'Recibido'";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);



            int index = 1;
            string dato, id, producto, precio, tipo_paquete, cantidad_unidades, unidad_medida, cantidad_pedida, cantidad_entrega;
            string id_insumo;
            for (int fila = 0; fila <= orden_de_compra.Rows.Count - 1; fila++)
            {
                id = orden_de_compra.Rows[fila]["id"].ToString();
                producto = orden_de_compra.Rows[fila]["producto"].ToString();
                if (orden_de_compra.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                {
                    precio = orden_de_compra.Rows[fila]["nuevo_precio"].ToString();
                }
                else
                {
                    precio = orden_de_compra.Rows[fila]["precio"].ToString();
                }

                tipo_paquete = orden_de_compra.Rows[fila]["tipo_paquete"].ToString();
                cantidad_unidades = orden_de_compra.Rows[fila]["cantidad_unidades"].ToString();
                unidad_medida = orden_de_compra.Rows[fila]["unidad_medida"].ToString();


                cantidad_pedida = orden_de_compra.Rows[fila]["cantidad_pedida"].ToString();
                cantidad_entrega = orden_de_compra.Rows[fila]["cantidad_entrega"].ToString();
                if (cantidad_entrega == "N/A")
                {
                    cantidad_entrega = orden_de_compra.Rows[fila]["total_entrega"].ToString();
                    if (cantidad_entrega == "N/A")
                    {
                        cantidad_entrega = "0";
                    }
                }
                dato = id + "-" + producto + "-" + precio + "-" + tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + cantidad_pedida + "-" + cantidad_entrega;

                actualizar = "`producto_" + index.ToString() + "` = '" + dato + "'";
                consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
                index++;

                id_insumo = orden_de_compra.Rows[fila]["id"].ToString();

                if (orden_de_compra.Rows[fila]["nuevo_stock"].ToString() == "N/A")
                {
                    actualizar_stock_en_insumo(id_insumo, orden_de_compra.Rows[fila]["stock"].ToString(), tipo_paquete, cantidad_unidades, unidad_medida, cantidad_entrega);
                }
                else
                {
                    actualizar_stock_en_insumo(id_insumo, orden_de_compra.Rows[fila]["nuevo_stock"].ToString(), tipo_paquete, cantidad_unidades, unidad_medida, cantidad_entrega);
                }
            }
            if (estado_pedido != "Recibido")
            {
                stock_Insumos.guardar_registro_compra(rol_usuario, insumos_fabrica, insumos_fabrica_copia);
            }
            if (verificar_si_hay_nuevo_precio(orden_de_compra))
            {
                actualizar_acuerdo_de_precio(id_orden, id_proveedor, orden_de_compra);
            }
        }
        public void actualizar_orden_de_compra_entrega_parcial(string id_orden, DataTable orden_de_compra, string id_proveedor, string valor_orden, string nombre_fabrica, string nombre_proveedor, string total_impuestos, string rol_usuario, string condicion_pago, string estado_entrega)
        {
            actualizar_valor_de_orden_de_compra(id_orden, id_proveedor, valor_orden, nombre_fabrica, nombre_proveedor, total_impuestos, condicion_pago, "N/A", estado_entrega);
            consultar_orden_de_compra(id_orden);
            consultar_insumos_fabrica();
            DataTable insumos_fabrica_copia = insumos_fabrica;
            string actualizar;

            actualizar = "`fecha_entrega` = '" + funciones.get_fecha() + "'";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);

            actualizar = "`estado` = 'Recibido'";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);

            int index = 1;
            string dato, id, producto, precio, tipo_paquete, cantidad_unidades, unidad_medida, cantidad_pedida, cantidad_entrega;
            string id_insumo;
            for (int fila = 0; fila <= orden_de_compra.Rows.Count - 1; fila++)
            {
                id = orden_de_compra.Rows[fila]["id"].ToString();
                producto = orden_de_compra.Rows[fila]["producto"].ToString();
                if (orden_de_compra.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                {
                    precio = orden_de_compra.Rows[fila]["nuevo_precio"].ToString();
                }
                else
                {
                    precio = orden_de_compra.Rows[fila]["precio"].ToString();
                }

                tipo_paquete = orden_de_compra.Rows[fila]["tipo_paquete"].ToString();
                cantidad_unidades = orden_de_compra.Rows[fila]["cantidad_unidades"].ToString();
                unidad_medida = orden_de_compra.Rows[fila]["unidad_medida"].ToString();


                cantidad_pedida = orden_de_compra.Rows[fila]["cantidad_pedida"].ToString();
                cantidad_entrega = orden_de_compra.Rows[fila]["total_entrega"].ToString();

                dato = id + "-" + producto + "-" + precio + "-" + tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + cantidad_pedida + "-" + cantidad_entrega;

                actualizar = "`producto_" + index.ToString() + "` = '" + dato + "'";
                consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
                index++;

                id_insumo = orden_de_compra.Rows[fila]["id"].ToString();

                if (orden_de_compra.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    actualizar_stock_en_insumo(id_insumo, orden_de_compra.Rows[fila]["nuevo_stock"].ToString(), tipo_paquete, cantidad_unidades, unidad_medida, cantidad_entrega);
                }
            }
            stock_Insumos.guardar_registro_compra(rol_usuario, insumos_fabrica, insumos_fabrica_copia);
            /*if (verificar_si_hay_nuevo_precio(orden_de_compra))
            {
                actualizar_acuerdo_de_precio(id_orden, id_proveedor, orden_de_compra);
            }*/
        }
        private void crear_nueva_orden_de_compra(DataTable orden_de_compra)
        {
            string columna = "";
            string valores = "";
            //id_proveedor
            //proveedor
            //acuerdo_de_precios
            //estado
            //fecha
            //fecha_entrega_estimada
            //nota
            //tipo_de_pago
            //condicion_pago
            //cantidad_a_pagar
            for (int fila = 0; fila <= orden_de_compraBD.Rows.Count-1; fila++)
            {
                
            }
        }
        private void actualizar_valor_de_orden_de_compra(string num_orden, string id_proveedor, string valor_orden, string nombre_fabrica, string nombre_proveedor, string total_impuestos, string condicion_pago, string fecha_entrega,string estado_entrega)
        {
            consultar_cuenta_por_pagar_fabrica();
            if (verificar_si_existe_cuenta_por_pagar_fabrica(num_orden, id_proveedor))
            {
                //si existe modificar
                string id_cuenta_por_pagar_fabrica = buscar_id_de_cuenta_por_pagar_fabrica(num_orden, id_proveedor);
                string actualizar = "`valor_orden` = '" + valor_orden + "'";
                consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_cuenta_por_pagar_fabrica);
                if (total_impuestos != "N/A")
                {
                    actualizar = "`impuestos` = '" + total_impuestos + "'";
                    consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_cuenta_por_pagar_fabrica);

                }
                if (fecha_entrega != "N/A")
                {
                    actualizar = "`fecha_remito` = '" + fecha_entrega + "'";
                    consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_cuenta_por_pagar_fabrica);
                }

                actualizar = "`estado_entrega` = 'Recibido'";
                consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar_fabrica", actualizar, id_cuenta_por_pagar_fabrica);



            }
            else
            {
                //si no existe crear condicion_pago
                string columnas = "";
                string valores = "";
                if (total_impuestos != "N/A")
                {
                    //impuestos
                    columnas = funciones.armar_query_columna(columnas, "impuestos", false);
                    valores = funciones.armar_query_valores(valores, total_impuestos, false);
                }
                if (condicion_pago != "N/A")
                {
                    //condicion_pago
                    columnas = funciones.armar_query_columna(columnas, "condicion_pago", false);
                    valores = funciones.armar_query_valores(valores, condicion_pago, false);
                }
                if (fecha_entrega != "N/A")
                {
                    //fecha_remito 
                    columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
                    valores = funciones.armar_query_valores(valores, fecha_entrega, false);
                }
                else
                {
                    //fecha_remito 
                    columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
                    valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
                }
                //fabrica
                columnas = funciones.armar_query_columna(columnas, "fabrica", false);
                valores = funciones.armar_query_valores(valores, nombre_fabrica, false);
                //num_orden
                columnas = funciones.armar_query_columna(columnas, "num_orden", false);
                valores = funciones.armar_query_valores(valores, num_orden, false);
                //valor_orden
                columnas = funciones.armar_query_columna(columnas, "valor_orden", false);
                valores = funciones.armar_query_valores(valores, valor_orden, false);


                //proveedor
                columnas = funciones.armar_query_columna(columnas, "proveedor", false);
                valores = funciones.armar_query_valores(valores, nombre_proveedor, false);

                //estado_entrega
                columnas = funciones.armar_query_columna(columnas, "estado_entrega", false);
                valores = funciones.armar_query_valores(valores, estado_entrega, false);

                //id_proveedor
                columnas = funciones.armar_query_columna(columnas, "id_proveedor", true);
                valores = funciones.armar_query_valores(valores, id_proveedor, true);

                consultas.insertar_en_tabla(base_de_datos, "cuenta_por_pagar_fabrica", columnas, valores);
            }
        }
        private void actualizar_acuerdo_de_precio(string id_orden, string id_proveedor, DataTable orden_de_compra)
        {
            acuerdo_de_precios_fabrica_a_proveedores_activo = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_activo(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", id_proveedor);
            desactivar_acuerdo_de_precios_fabrica_a_proveedores_activo(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0]["id"].ToString());
            string columnas = "";
            string valores = "";
            string dato, id_producto, producto, paquete, cant_unidad, unidad, precio;
            int fila_producto;
            int index = 1;
            //id_proveedor
            columnas = funciones.armar_query_columna(columnas, "id_proveedor", false);
            valores = funciones.armar_query_valores(valores, acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0]["id_proveedor"].ToString(), false);
            //acuerdo
            int acuerdo = int.Parse(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0]["acuerdo"].ToString()) + 1;

            columnas = funciones.armar_query_columna(columnas, "acuerdo", false);
            valores = funciones.armar_query_valores(valores, acuerdo.ToString(), false);
            string actualizar = "`acuerdo_de_precios` = '" + acuerdo.ToString() + "'";
            consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            for (int columna = acuerdo_de_precios_fabrica_a_proveedores_activo.Columns["producto_1"].Ordinal; columna < acuerdo_de_precios_fabrica_a_proveedores_activo.Columns.Count - 1; columna++)
            {
                if (acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString() != "N/A")
                {
                    id_producto = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 2);
                    paquete = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 3);
                    cant_unidad = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 4);
                    unidad = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 5);
                    precio = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][columna].ToString(), 6);

                    fila_producto = funciones.buscar_fila_por_id(id_producto, orden_de_compra);
                    if (fila_producto != -1)
                    {

                        if (orden_de_compra.Rows[fila_producto]["nuevo_precio"].ToString() != "N/A")
                        {
                            precio = orden_de_compra.Rows[fila_producto]["nuevo_precio_unidad"].ToString();
                        }
                    }

                    dato = id_producto + "-" + producto + "-" + paquete + "-" + cant_unidad + "-" + unidad + "-" + precio;

                    columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), false);
                    valores = funciones.armar_query_valores(valores, dato, false);

                }
                else
                {
                    columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), false);
                    valores = funciones.armar_query_valores(valores, "N/A", false);
                }
                index++;
            }
            int ultima_columna = acuerdo_de_precios_fabrica_a_proveedores_activo.Columns.Count - 1;
            if (acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString() != "N/A")
            {
                id_producto = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 1);
                producto = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 2);
                paquete = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 3);
                cant_unidad = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 4);
                unidad = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 5);
                precio = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_activo.Rows[0][ultima_columna].ToString(), 6);

                fila_producto = funciones.buscar_fila_por_id(id_producto, orden_de_compra);

                if (fila_producto != -1)
                {
                    if (orden_de_compra.Rows[fila_producto]["nuevo_precio"].ToString() != "N/A")
                    {
                        precio = orden_de_compra.Rows[fila_producto]["nuevo_precio_unidad"].ToString();
                    }
                }

                dato = id_producto + "-" + producto + "-" + paquete + "-" + cant_unidad + "-" + unidad + "-" + precio;

                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
                valores = funciones.armar_query_valores(valores, dato, true);

            }
            else
            {
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
                valores = funciones.armar_query_valores(valores, "N/A", true);
            }
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", columnas, valores);
            actualizar_acuerdo_en_ordenes_de_compra(id_proveedor, acuerdo);
        }
        private void actualizar_acuerdo_en_ordenes_de_compra(string id_proveedor, int nuevo_acuerdo)
        {
            DataTable ordenes_de_compra = consultas.consultar_ordenes_de_compra_de_proveedor_abiertas(id_proveedor);
            string id_orden;
            string actualizar;
            for (int fila = 0; fila <= ordenes_de_compra.Rows.Count - 1; fila++)
            {
                id_orden = ordenes_de_compra.Rows[fila]["id"].ToString();
                actualizar = "`acuerdo_de_precios` = '" + nuevo_acuerdo.ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "ordenes_de_compra", actualizar, id_orden);
            }

        }
        private void desactivar_acuerdo_de_precios_fabrica_a_proveedores_activo(string id_acuerdo)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", actualizar, id_acuerdo);
        }
        private void actualizar_stock_en_insumo(string id_insumo, string nuevo_stock_insumo, string tipo_paquete, string cantidad_unidades, string unidad_medida, string cantidad_entrega)
        {
            int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabrica);
            double nuevo_real = 0;
            string dato, stock_real;
            bool encontro = false;
            for (int columna = insumos_fabrica.Columns["producto_1"].Ordinal; columna <= insumos_fabrica.Columns.Count - 1; columna++)
            {
                if (insumos_fabrica.Rows[fila_insumo][columna].ToString() == "N/A")
                {
                    if (!encontro)
                    {

                        insumos_fabrica.Rows[fila_insumo][columna] = tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + cantidad_entrega + "-" + cantidad_entrega;
                        insumos_fabrica.Rows[fila_insumo]["nuevo_stock"] = "si";
                    }

                    break;
                }
                else if (tipo_paquete == funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo][columna].ToString(), 1) &&
                        cantidad_unidades == funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo][columna].ToString(), 2) &&
                        unidad_medida == funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo][columna].ToString(), 3))
                {
                    stock_real = funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo][columna].ToString(), 4);
                    nuevo_real = double.Parse(stock_real) + double.Parse(cantidad_entrega);
                    dato = tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + nuevo_stock_insumo + "-" + nuevo_real.ToString();
                    insumos_fabrica.Rows[fila_insumo]["nuevo_stock"] = "si";
                    insumos_fabrica.Rows[fila_insumo][columna] = dato;
                    encontro = true;
                }
            }
        }

        private bool verificar_si_hay_nuevo_precio(DataTable orden_de_compra)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= orden_de_compra.Rows.Count - 1)
            {
                if (orden_de_compra.Rows[fila]["nuevo_precio"].ToString() != "N/A")
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private bool verificar_si_existe_cuenta_por_pagar_fabrica(string num_orden, string id_proveedor)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= cuenta_por_pagar_fabrica.Rows.Count - 1)
            {
                if (cuenta_por_pagar_fabrica.Rows[fila]["num_orden"].ToString() == num_orden &&
                    cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString() == id_proveedor)
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private string buscar_id_de_cuenta_por_pagar_fabrica(string num_orden, string id_proveedor)
        {
            string retorno = "";
            int fila = 0;
            while (fila <= cuenta_por_pagar_fabrica.Rows.Count - 1)
            {
                if (cuenta_por_pagar_fabrica.Rows[fila]["num_orden"].ToString() == num_orden &&
                    cuenta_por_pagar_fabrica.Rows[fila]["id_proveedor"].ToString() == id_proveedor)
                {
                    retorno = cuenta_por_pagar_fabrica.Rows[fila]["id"].ToString();
                    break;
                }
                fila++;
            }

            return retorno;
        }
        #endregion

        #region crear tablas
        private void crear_tabla_orden()
        {
            orden_de_compra = new DataTable();
            orden_de_compra.Columns.Add("id", typeof(string));
            orden_de_compra.Columns.Add("producto", typeof(string));
            orden_de_compra.Columns.Add("cantidad_pedida", typeof(string));
            orden_de_compra.Columns.Add("cantidad_entrega", typeof(string));
            orden_de_compra.Columns.Add("total_entrega", typeof(string));
            orden_de_compra.Columns.Add("precio", typeof(string));
            orden_de_compra.Columns.Add("precio_unidad", typeof(string));
            orden_de_compra.Columns.Add("nuevo_precio", typeof(string));
            orden_de_compra.Columns.Add("nuevo_precio_unidad", typeof(string));
            orden_de_compra.Columns.Add("sub_total", typeof(string));
            orden_de_compra.Columns.Add("stock", typeof(string));
            orden_de_compra.Columns.Add("nuevo_stock", typeof(string));
            orden_de_compra.Columns.Add("unidad_pedida", typeof(string));
            orden_de_compra.Columns.Add("dato", typeof(string));

            orden_de_compra.Columns.Add("tipo_paquete", typeof(string));
            orden_de_compra.Columns.Add("cantidad_unidades", typeof(string));
            orden_de_compra.Columns.Add("unidad_medida", typeof(string));
            orden_de_compra.Columns.Add("condicion_pago", typeof(string));
        }
        #endregion
        #region metodos privados
        private int obtener_columna_precio(string id_producto)
        {
            int retorno = -1;
            string id;
            for (int columna = acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Columns["producto_1"].Ordinal; columna <= acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Columns.Count - 1; columna++)
            {
                id = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Rows[0][columna].ToString(), 1);
                if (id_producto == id)
                {
                    retorno = columna;
                    break;
                }
            }
            return retorno;
        }
        private void llenar_tabla_orden()
        {
            crear_tabla_orden();
            string id, producto, tipo_paquete, cantidad_unidades, unidad_medida, unidad_pedida, cantidad_pedida, precio_unidad, dato, stock, cantidad_entrega, condicion_pago;
            double precio;
            int fila_producto;
            int columna_precio;
            int fila_orden = 0;
            for (int columna = orden_de_compraBD.Columns["producto_1"].Ordinal; columna <= orden_de_compraBD.Columns.Count - 1; columna++)
            {
                if (orden_de_compraBD.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(orden_de_compraBD.Rows[0][columna].ToString(), 1);
                    columna_precio = obtener_columna_precio(id);
                    producto = funciones.obtener_dato(orden_de_compraBD.Rows[0][columna].ToString(), 2);
                    precio_unidad = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Rows[0][columna_precio].ToString(), 6);

                    tipo_paquete = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Rows[0][columna_precio].ToString(), 3);
                    cantidad_unidades = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Rows[0][columna_precio].ToString(), 4);
                    unidad_medida = funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores_de_pedido.Rows[0][columna_precio].ToString(), 5);
                    condicion_pago = orden_de_compraBD.Rows[0]["condicion_pago"].ToString();

                    if (tipo_paquete == "Unidad")
                    {
                        unidad_pedida = " x " + cantidad_unidades + " " + unidad_medida;//cantidad_unidades + " " +
                    }
                    else
                    {
                        unidad_pedida = tipo_paquete + " x " + cantidad_unidades + " " + unidad_medida;
                    }

                    cantidad_pedida = funciones.obtener_dato(orden_de_compraBD.Rows[0][columna].ToString(), 7);
                    cantidad_entrega = funciones.obtener_dato(orden_de_compraBD.Rows[0][columna].ToString(), 8);
                    dato = id + "-" + producto + "-" + precio_unidad + "-" + unidad_pedida + "-" + cantidad_pedida;

                    fila_producto = funciones.buscar_fila_por_id(id, insumos_fabrica);
                    stock = obtener_stock(fila_producto, tipo_paquete, cantidad_unidades, unidad_medida); //buscar stock
                    orden_de_compra.Rows.Add();
                    //id
                    orden_de_compra.Rows[fila_orden]["id"] = id;
                    //producto
                    orden_de_compra.Rows[fila_orden]["producto"] = producto;
                    //cantidad_pedida
                    orden_de_compra.Rows[fila_orden]["cantidad_pedida"] = cantidad_pedida;

                    //cantidad_entrega
                    orden_de_compra.Rows[fila_orden]["cantidad_entrega"] = cantidad_entrega;

                    orden_de_compra.Rows[fila_orden]["total_entrega"] = cantidad_entrega;

                    //precio_unidad
                    orden_de_compra.Rows[fila_orden]["precio_unidad"] = precio_unidad;
                    precio = double.Parse(precio_unidad) * double.Parse(cantidad_unidades);
                    //precio
                    orden_de_compra.Rows[fila_orden]["precio"] = precio.ToString();

                    //nuevo_precio
                    orden_de_compra.Rows[fila_orden]["nuevo_precio"] = "N/A";

                    //sub_total
                    orden_de_compra.Rows[fila_orden]["sub_total"] = "N/A";

                    //stock
                    orden_de_compra.Rows[fila_orden]["stock"] = stock;

                    //nuevo_stock
                    orden_de_compra.Rows[fila_orden]["nuevo_stock"] = "N/A";

                    //tipo_paquete
                    orden_de_compra.Rows[fila_orden]["tipo_paquete"] = tipo_paquete;
                    //cantidad_unidades
                    orden_de_compra.Rows[fila_orden]["cantidad_unidades"] = cantidad_unidades;
                    //unidad_medida
                    orden_de_compra.Rows[fila_orden]["unidad_medida"] = unidad_medida;

                    //unidad_pedida
                    orden_de_compra.Rows[fila_orden]["unidad_pedida"] = unidad_pedida;

                    //dato
                    orden_de_compra.Rows[fila_orden]["dato"] = dato;

                    //condicion_pago
                    orden_de_compra.Rows[fila_orden]["condicion_pago"] = condicion_pago;

                    fila_orden++;
                }
            }
        }
        private string obtener_stock(int fila_producto, string tipo_paquete, string cantidad_unidades, string unidad_medida)
        {
            string retorno = "0";
            //insumos_fabrica.Rows[fila_producto]["stock"].ToString();
            for (int columna = insumos_fabrica.Columns["producto_1"].Ordinal; columna < insumos_fabrica.Columns.Count - 1; columna++)
            {
                if (insumos_fabrica.Rows[fila_producto][columna].ToString() != "N/A")
                {
                    if (tipo_paquete == funciones.obtener_dato(insumos_fabrica.Rows[fila_producto][columna].ToString(), 1) &&
                        cantidad_unidades == funciones.obtener_dato(insumos_fabrica.Rows[fila_producto][columna].ToString(), 2) &&
                        unidad_medida == funciones.obtener_dato(insumos_fabrica.Rows[fila_producto][columna].ToString(), 3))
                    {
                        retorno = funciones.obtener_dato(insumos_fabrica.Rows[fila_producto][columna].ToString(), 4);
                        break;
                    }
                }
            }
            return retorno;
        }
        #endregion
        #region metodos consultas
        private void consultar_cuenta_por_pagar_fabrica()
        {
            cuenta_por_pagar_fabrica = consultas.consultar_tabla(base_de_datos, "cuenta_por_pagar_fabrica");
        }
        private void consultar_acuerdo_de_precios_fabrica_a_proveedores(string acuerdo, string id_proveedor)
        {
            acuerdo_de_precios_fabrica_a_proveedores = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_de_orden(acuerdo, id_proveedor);
        }
        private void consultar_orden_de_compra(string id_orden)
        {
            orden_de_compraBD = consultas.consultar_ordenes_de_compra_de_proveedor_por_id(id_orden);
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
            insumos_fabrica_copia = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
            insumos_fabrica.Columns.Add("nuevo_stock", typeof(string));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                insumos_fabrica.Rows[fila]["nuevo_stock"] = "N/A";
            }

        }
        private void consultar_acuerdo_de_precios_fabrica_a_proveedores_de_pedido(string acuerdo, string id_proveedor)
        {
            acuerdo_de_precios_fabrica_a_proveedores_de_pedido = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_de_orden(acuerdo, id_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_orden_de_compra(string id_orden)
        {
            consultar_insumos_fabrica();
            consultar_orden_de_compra(id_orden);
            string id_proveedor = orden_de_compraBD.Rows[0]["id_proveedor"].ToString();
            string acuerdo_de_precios = orden_de_compraBD.Rows[0]["acuerdo_de_precios"].ToString();
            consultar_acuerdo_de_precios_fabrica_a_proveedores_de_pedido(acuerdo_de_precios, id_proveedor);
            llenar_tabla_orden();
            return orden_de_compra;
        }
        public string get_estado_orden_de_compra(string id_orden)
        {
            consultar_orden_de_compra(id_orden);

            return orden_de_compraBD.Rows[0]["estado"].ToString();
        }
        #endregion
    }
}
