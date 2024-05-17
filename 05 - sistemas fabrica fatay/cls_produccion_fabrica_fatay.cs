using _01___modulos;
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
    public class cls_produccion_fabrica_fatay
    {
        public cls_produccion_fabrica_fatay(DataTable usuario_BD)
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
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_movimientos_stock_producto movimientos_stock_producto;
        cls_funciones funciones = new cls_funciones();
        cls_whatsapp whatsapp = new cls_whatsapp();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_produccion;
        DataTable productos_proveedor;
        DataTable insumos_fabrica;
        DataTable recetas;
        #endregion

        #region metodos consulta
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
        }
        #endregion

        #region metodos get/set
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
        public string cargar_produccion_diaria(DataTable resumen_pedido, string fabrica, string proveedor, string fecha, string rol_usuario)
        {
            consultar_productos_proveedor("proveedor_villaMaipu");
            if (!verificar_si_cargo_en_BD(fabrica, proveedor, "Shami Villa Maipu Expedicion", fecha))
            {


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
                valores = armar_query_valores(valores, "Shami Villa Maipu Expedicion", false);
                if (proveedor == "Shami Villa Maipu Expedicion")
                {
                    //estado
                    columnas = armar_query_columna(columnas, "estado", false);
                    valores = armar_query_valores(valores, "Recibido", false);
                }
                else
                {
                    //estado
                    columnas = armar_query_columna(columnas, "estado", false);
                    valores = armar_query_valores(valores, "Despachado", false);
                }
                //fecha
                columnas = armar_query_columna(columnas, "fecha", false);

                valores = armar_query_valores(valores, fecha, false);
                //USAR FUNCIONES COMO OBTENER ACUERDO DE PRECIO PARA  TENER INFO EN TIEMPO REAL Y NO RECIBIRLA POR PARAMETRO.
                int fila_producto;
                double stock_produccion,  cantidad_producida, nuevo_stock;
                string actualizar;
                producto_index = 1;
                for (int fila = 0; fila < resumen_pedido.Rows.Count - 1; fila++)
                {
                    id = resumen_pedido.Rows[fila]["id"].ToString();
                    producto = resumen_pedido.Rows[fila]["producto"].ToString();
                    cantidad_despachada = resumen_pedido.Rows[fila]["cantidad"].ToString().Replace(",", ".");

                    fila_producto = funciones.buscar_fila_por_id(id, productos_proveedor);

                    cantidad_producida = double.Parse(cantidad_despachada);

                    stock_produccion = double.Parse(productos_proveedor.Rows[fila_producto]["stock_produccion"].ToString());
                    nuevo_stock = stock_produccion + cantidad_producida;
                    actualizar = "`stock_fabrica_fatay` = '" + nuevo_stock.ToString() + "'";
                    consultas.actualizar_tabla(base_de_datos, "proveedor_villaMaipu", actualizar, id);

                    valor_final = id + "-" + producto + "-" + cantidad_despachada + "-N/A";



                    columnas = armar_query_columna(columnas, "producto_" + producto_index, false);
                    valores = armar_query_valores(valores, valor_final, false);

                    producto_index = producto_index + 1;
                }
                int ultima_fila = resumen_pedido.Rows.Count - 1;
                id = resumen_pedido.Rows[ultima_fila]["id"].ToString();
                producto = resumen_pedido.Rows[ultima_fila]["producto"].ToString();
                cantidad_despachada = resumen_pedido.Rows[ultima_fila]["cantidad"].ToString().Replace(",", ".");

                fila_producto = funciones.buscar_fila_por_id(id, productos_proveedor);

                cantidad_producida = double.Parse(cantidad_despachada);

                stock_produccion = double.Parse(productos_proveedor.Rows[fila_producto]["stock_produccion"].ToString());
                nuevo_stock = stock_produccion + cantidad_producida;
                actualizar = "`stock_fabrica_fatay` = '" + nuevo_stock.ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "proveedor_villaMaipu", actualizar, id);


                valor_final = id + "-" + producto + "-" + cantidad_despachada + "-N/A";


                columnas = armar_query_columna(columnas, "producto_" + producto_index, true);
                valores = armar_query_valores(valores, valor_final, true);

                consultas.insertar_en_tabla(base_de_datos, "produccion_diaria", columnas, valores);
            }
           return whatsapp.notificar_carga_de_produccion_fatay();
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
