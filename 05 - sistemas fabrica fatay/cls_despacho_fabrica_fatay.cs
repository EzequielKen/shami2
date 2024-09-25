using _01___modulos;
using _02___sistemas;
using _03___sistemas_fabrica;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05___sistemas_fabrica_fatay
{
    public class cls_despacho_fabrica_fatay
    {
        public cls_despacho_fabrica_fatay(DataTable usuario_BD)
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
            movimientos_stock_producto = new cls_movimientos_stock_producto(usuario_BD);
            stock_productos_fabrica_fatay = new cls_stock_producto_terminado_fabrica_fatay(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_movimientos_stock_producto movimientos_stock_producto;
        cls_stock_producto_terminado_fabrica_fatay stock_productos_fabrica_fatay;
        cls_sistema_pedidos pedidos;
        cls_funciones funciones = new cls_funciones();
        cls_whatsapp whatsapp = new cls_whatsapp();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_produccion;
        DataTable productos_proveedor;
        DataTable insumos_fabrica;
        DataTable recetas;
        DataTable sucursales;
        DataTable resumen;
        #endregion

        #region metodos facturacion automatica
        private void crear_dataTable_resumen(DataTable despacho, string id_sucursal)
        {
            DataTable productos_proveedor_pedido = pedidos.get_productos_proveedor(id_sucursal);
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("cantidad", typeof(string));
            resumen.Columns.Add("unidad de medida", typeof(string));
            resumen.Columns.Add("precio", typeof(string));
            resumen.Columns.Add("multiplicador", typeof(string));
            resumen.Columns.Add("bonificable", typeof(bool));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));

            string id_producto, producto;
            int fila_producto, ultima_fila;
            for (int fila = 0; fila <= despacho.Rows.Count - 1; fila++)
            {
                id_producto = despacho.Rows[fila]["id"].ToString();
                producto = despacho.Rows[fila]["producto"].ToString();
                fila_producto = funciones.buscar_fila_por_id_nombre(id_producto, producto, productos_proveedor_pedido);

                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;

                resumen.Rows[ultima_fila]["id"] = productos_proveedor_pedido.Rows[fila_producto]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = productos_proveedor_pedido.Rows[fila_producto]["producto"].ToString();
                resumen.Rows[ultima_fila]["cantidad"] = despacho.Rows[fila]["cantidad"].ToString();
                resumen.Rows[ultima_fila]["unidad de medida"] = productos_proveedor_pedido.Rows[fila_producto]["unidad_medida_local"].ToString();
                resumen.Rows[ultima_fila]["precio"] = productos_proveedor_pedido.Rows[fila_producto]["precio"].ToString();
                resumen.Rows[ultima_fila]["multiplicador"] = productos_proveedor_pedido.Rows[fila_producto]["multiplicador"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto"] = productos_proveedor_pedido.Rows[fila_producto]["tipo_producto"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = productos_proveedor_pedido.Rows[fila_producto]["proveedor"].ToString();
            }
        }
        private void crear_cuentas_por_pagar(DataTable resumen, DataTable sucursal, string num_pedido, string nota)
        {
            double valor_remito, precio, multiplicador, cantidad,sub_total;
            valor_remito = 0;
            for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
            {
                precio = double.Parse(resumen.Rows[fila]["precio"].ToString());
                multiplicador = double.Parse(resumen.Rows[fila]["multiplicador"].ToString());
                cantidad = double.Parse(resumen.Rows[fila]["cantidad"].ToString());

                precio = precio * multiplicador;
                sub_total = cantidad * precio;

                valor_remito = valor_remito + sub_total;
            }

            string columnas = string.Empty;
            string valores = string.Empty;
            //sucursal
            columnas = funciones.armar_query_columna(columnas,"sucursal",false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(),false);
            //num_pedido
            columnas = funciones.armar_query_columna(columnas, "num_pedido", false);
            valores = funciones.armar_query_valores(valores, num_pedido, false);
            //nombre_remito
            columnas = funciones.armar_query_columna(columnas, "nombre_remito", false);
            valores = funciones.armar_query_valores(valores, "pedido", false);
            //valor_remito
            columnas = funciones.armar_query_columna(columnas, "valor_remito", false);
            valores = funciones.armar_query_valores(valores, valor_remito.ToString(), false);
            //aumento
            columnas = funciones.armar_query_columna(columnas, "aumento", false);
            valores = funciones.armar_query_valores(valores, "0", false);
            //fecha_remito
            columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", false);
            valores = funciones.armar_query_valores(valores, "proveedor_villaMaipu", false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "cuenta_por_pagar", columnas, valores);
        }
        #endregion

        #region metodos consulta
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        private void consultar_recetas()
        {
            recetas = consultas.consultar_tabla_completa(base_de_datos, "recetas");
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, nombre_proveedor);
        }
        private void consultar_productos_produccion(string nombre_proveedor)
        {
            productos_produccion = consultas.consultar_productos_produccion_fabrica_fatay(base_de_datos, nombre_proveedor);
            for (int fila = 0; fila <= productos_produccion.Rows.Count-1; fila++)
            {
                string id = productos_produccion.Rows[fila]["id"].ToString();
                productos_produccion.Rows[fila]["stock"] = stock_productos_fabrica_fatay.get_ultimo_stock_producto_terminado(id);
            }
        }
        #endregion

        #region metodos get/set
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            sucursales.DefaultView.Sort = "sucursal asc";
            sucursales = sucursales.DefaultView.ToTable();
            return sucursales;
        }
        public DataTable get_productos_produccion(string nombre_proveedor)
        {
            consultar_productos_produccion(nombre_proveedor);
            return productos_produccion;
        }
        #endregion

        #region metodos carga a base de datos

        private bool verificar_si_cargo_en_BD(string fabrica, string proveedor, string receptor, string fecha)
        {
            bool retorno = false;
            DataTable dt = consultas.consultar_produccion(fabrica, proveedor, receptor, fecha);
            if (dt.Rows.Count > 0)
            {
                retorno = true;
            }
            return retorno;
        }
        public void cargar_despacho(DataTable resumen_pedido, string fabrica, string proveedor, string fecha, string rol_usuario, string receptor)
        {
            consultar_productos_proveedor("proveedor_villaMaipu");

            string nota = "Despachado a cliente: " + receptor;
            //actualizar_stock_insumos(resumen_pedido);
            string columnas = "";
            string valores = "";

            string id, producto, cantidad_despachada, valor_final;
            int producto_index;

            //fabrica
            columnas = armar_query_columna(columnas, "fabrica", false);
            valores = armar_query_valores(valores, fabrica, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor, false);

            //receptor
            columnas = armar_query_columna(columnas, "receptor", false);
            valores = armar_query_valores(valores, receptor, false);
            //estado
            if (receptor == "Fabrica Villa Maipu")
            {
                columnas = armar_query_columna(columnas, "estado", false);
                valores = armar_query_valores(valores, "Despachado", false);
            }
            else
            {
                columnas = armar_query_columna(columnas, "estado", false);
                valores = armar_query_valores(valores, "Recibido", false);
            }
            //fecha
            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //USAR FUNCIONES COMO OBTENER ACUERDO DE PRECIO PARA  TENER INFO EN TIEMPO REAL Y NO RECIBIRLA POR PARAMETRO.
            int fila_producto;
            producto_index = 1;
            for (int fila = 0; fila < resumen_pedido.Rows.Count - 1; fila++)
            {
                id = resumen_pedido.Rows[fila]["id"].ToString();
                producto = resumen_pedido.Rows[fila]["producto"].ToString();
                cantidad_despachada = resumen_pedido.Rows[fila]["cantidad"].ToString().Replace(",", ".");

                fila_producto = funciones.buscar_fila_por_id(id, productos_proveedor);

                if (receptor == "Fabrica Villa Maipu")
                {
                    valor_final = id + "-" + producto + "-" + cantidad_despachada + "-N/A";
                }
                else
                {
                    valor_final = id + "-" + producto + "-" + cantidad_despachada + "-" + cantidad_despachada;
                }

                stock_productos_fabrica_fatay.cargar_historial_stock("Shami Fabrica Fatay", id, "despacho", cantidad_despachada, nota);

                columnas = armar_query_columna(columnas, "producto_" + producto_index, false);
                valores = armar_query_valores(valores, valor_final, false);

                producto_index = producto_index + 1;
            }
            int ultima_fila = resumen_pedido.Rows.Count - 1;
            id = resumen_pedido.Rows[ultima_fila]["id"].ToString();
            producto = resumen_pedido.Rows[ultima_fila]["producto"].ToString();
            cantidad_despachada = resumen_pedido.Rows[ultima_fila]["cantidad"].ToString().Replace(",", ".");

            fila_producto = funciones.buscar_fila_por_id(id, productos_proveedor);

            if (receptor == "Fabrica Villa Maipu")
            {
                valor_final = id + "-" + producto + "-" + cantidad_despachada + "-N/A";
            }
            else
            {
                valor_final = id + "-" + producto + "-" + cantidad_despachada + "-" + cantidad_despachada;
            }


            stock_productos_fabrica_fatay.cargar_historial_stock("Shami Fabrica Fatay", id, "despacho", cantidad_despachada, nota);


            columnas = armar_query_columna(columnas, "producto_" + producto_index, true);
            valores = armar_query_valores(valores, valor_final, true);

            consultas.insertar_en_tabla(base_de_datos, "despacho_fabrica_fatay", columnas, valores);
            if (receptor != "Fabrica Villa Maipu")
            {
                //FACTURACION AUTOMATICA
                DataTable sucursal = consultas.consultar_sucursal_por_nombre(receptor);
                pedidos = new cls_sistema_pedidos(usuarioBD, sucursal);
                crear_dataTable_resumen(resumen_pedido, sucursal.Rows[0]["id"].ToString());
                nota = "Facturado por Shami Fabrica Fatay";
                string num_pedido = pedidos.enviar_pedido_automatico(resumen,nota);
                crear_cuentas_por_pagar(resumen,sucursal,num_pedido,nota);
            }

        }
        private void actualizar_stock_insumos(DataTable resumen_pedido)
        {

            string id, id_insumo;
            double cantidad_receta, cantidad_despachada, cantidad_a_descontar, stock_actual_insumo, stock_real, nuevo_stock_insumo, nuevo_stock_insumo_real;
            string tipo_paquete, cantidad_unidades, unidad_medida;
            int fila_receta, fila_insumo;
            int columna;
            string dato, actualizar;
            consultar_insumos_fabrica();
            consultar_recetas();
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {
                id = resumen_pedido.Rows[fila]["id"].ToString();
                fila_receta = funciones.buscar_fila_por_id(id, recetas);
                cantidad_despachada = double.Parse(resumen_pedido.Rows[fila]["cantidad"].ToString());
                columna = recetas.Columns["producto_1"].Ordinal;
                while (columna <= recetas.Columns.Count - 1 &&
                       recetas.Rows[fila_receta][columna].ToString() != "N/A")
                {
                    id_insumo = funciones.obtener_dato(recetas.Rows[fila_receta][columna].ToString(), 1);
                    fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabrica);
                    if (insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString() != "N/A")
                    {
                        cantidad_receta = double.Parse(funciones.obtener_dato(recetas.Rows[fila_receta][columna].ToString(), 3));
                        cantidad_a_descontar = cantidad_despachada * cantidad_receta;


                        tipo_paquete = funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString(), 1);
                        cantidad_unidades = funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString(), 2);
                        unidad_medida = funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString(), 3);

                        stock_actual_insumo = double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString(), 4));
                        stock_real = double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila_insumo]["producto_1"].ToString(), 5));

                        nuevo_stock_insumo = stock_actual_insumo - cantidad_a_descontar;
                        nuevo_stock_insumo_real = stock_real - cantidad_a_descontar;

                        dato = tipo_paquete + "-" + cantidad_unidades + "-" + unidad_medida + "-" + nuevo_stock_insumo + "-" + nuevo_stock_insumo_real;

                        actualizar = "`producto_1` = '" + dato + "'";
                        consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
                        consultar_insumos_fabrica();
                    }

                    columna++;

                }
            }
        }
        private string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
        {
            string retorno = "";
            string separador_columna = "`";

            retorno = columnas + separador_columna + columna_valor + separador_columna;

            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        private string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
        {
            string retorno = "";
            string separador_valores = "'";

            retorno = valores + separador_valores + valor_a_insertar + separador_valores;
            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        #endregion
    }
}
