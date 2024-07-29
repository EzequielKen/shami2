using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_sistema_actualizador_precios_vegetales
    {
        public cls_sistema_actualizador_precios_vegetales(DataTable usuario_BD)
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

        DataTable usuarioBD;
        DataTable acuerdo_de_precio;
        DataTable pedidos_no_cargados;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        #endregion

        #region metodos privados
        private void cargar_precios_en_productos()
        {
            string id;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                id = productos_proveedor.Rows[fila]["id"].ToString();
                productos_proveedor.Rows[fila]["precio"] = acuerdo_de_precio.Rows[0]["producto_" + id].ToString();
            }
        }

        #endregion

        #region metodos consultas
        private void consultar_acuerdo_de_precio(string nombre_prooveedor_en_BD)
        {
            acuerdo_de_precio = consultas.consultar_acuerdo_de_precio_activo(base_de_datos, nombre_prooveedor_en_BD);
        }
        private void consultar_productos_proveedor(string nombre_prooveedor_en_BD)
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos, nombre_prooveedor_en_BD);
        }
        private void consultar_pedidos_no_cargados(string nombre_prooveedor_en_BD)
        {
            pedidos_no_cargados = consultas.consultar_pedidos_no_cargados(base_de_datos, "pedidos", nombre_prooveedor_en_BD);
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor(string nombre_prooveedor_en_BD)
        {
            consultar_productos_proveedor(nombre_prooveedor_en_BD);
            consultar_acuerdo_de_precio(nombre_prooveedor_en_BD);
            cargar_precios_en_productos();
            return productos_proveedor;
        }
        #endregion

        #region enviar a base de datos
        public void cargar_nuevos_precio(DataTable productosBD, string nombre_proveedor)
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            cargar_precio_compra(productosBD, fecha);
            cargar_precio_venta(productosBD, fecha);
            actualizar_stock(productosBD, nombre_proveedor);
            cargar_compra_en_rendiciones(productosBD, fecha);
        }
        private void cargar_compra_en_rendiciones(DataTable productosBD, string fecha)
        {
            string columnas = "", valores = "";
            int nuevo_acuerdo = int.Parse(acuerdo_de_precio.Rows[0]["acuerdo"].ToString()) + 1;
            double stock, stock_nuevo, comprado, precio_compra, valor;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["precio_compra"].ToString() != "N/A")
                {
                    //id_productto
                    columnas = armar_query_columna(columnas, "id_producto", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["id"].ToString(), false);
                    //detalle
                    columnas = armar_query_columna(columnas, "detalle", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["producto"].ToString(), false);
                    //unidad_de_medida 
                    columnas = armar_query_columna(columnas, "unidad_de_medida", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["unidad_de_medida"].ToString(), false);
                    //concepto
                    columnas = armar_query_columna(columnas, "concepto", false);
                    valores = armar_query_valores(valores, "Compra", false);
                    //valor
                    stock = double.Parse(productosBD.Rows[fila]["stock"].ToString());
                    stock_nuevo = double.Parse(productosBD.Rows[fila]["stock_nuevo"].ToString());
                    comprado = stock_nuevo - stock;

                    precio_compra = double.Parse(productosBD.Rows[fila]["precio_compra"].ToString());
                    valor = Math.Round(comprado * precio_compra, 2);
                    columnas = armar_query_columna(columnas, "valor", false);
                    valores = armar_query_valores(valores, valor.ToString(), false);
                    //stock_inicial
                    columnas = armar_query_columna(columnas, "stock_inicial", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["stock"].ToString(), false);
                    //stock_final
                    columnas = armar_query_columna(columnas, "stock_final", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["stock_nuevo"].ToString(), false);
                    //proveedor
                    columnas = armar_query_columna(columnas, "proveedor", false);
                    valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["proveedor"].ToString(), false);


                    //fecha
                    columnas = armar_query_columna(columnas, "fecha", false);
                    valores = armar_query_valores(valores, fecha, false);

                    //acuerdo_de_precio
                    columnas = armar_query_columna(columnas, "acuerdo_de_precio", true);
                    valores = armar_query_valores(valores, nuevo_acuerdo.ToString(), true);

                    consultas.insertar_en_tabla(base_de_datos, "rendiciones", columnas, valores);

                    columnas = "";
                    valores = "";
                }

            }


        }
        private void cargar_precio_compra(DataTable productosBD, string fecha)
        {
            string columnas = "", valores = "", id = "";
            int nuevo_acuerdo = int.Parse(acuerdo_de_precio.Rows[0]["acuerdo"].ToString()) + 1;
            //activa
            columnas = armar_query_columna(columnas, "activa", false);
            valores = armar_query_valores(valores, "0", false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["proveedor"].ToString(), false);
            //acuerdo
            columnas = armar_query_columna(columnas, "acuerdo", false);
            valores = armar_query_valores(valores, nuevo_acuerdo.ToString(), false);
            //tipo_de_acuerdo
            columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = armar_query_valores(valores, "compra_a_proveedor", false);
            //fecha

            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            for (int fila = 0; fila < productosBD.Rows.Count - 1; fila++)
            {
                id = productosBD.Rows[fila]["id"].ToString();
                columnas = armar_query_columna(columnas, "producto_" + id, false);
                if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                {
                    valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["producto_" + id].ToString().Replace(",", "."), false);//
                }
                else
                {

                    valores = armar_query_valores(valores, productosBD.Rows[fila]["precio_compra"].ToString().Replace(",", "."), false);
                }
            }
            int ultima_fila = productosBD.Rows.Count - 1;
            id = productosBD.Rows[ultima_fila]["id"].ToString();
            columnas = armar_query_columna(columnas, "producto_" + id, true);
            if (productosBD.Rows[ultima_fila]["precio_compra"].ToString() == "N/A")
            {
                valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["producto_" + id].ToString().Replace(",", "."), true);
            }
            else
            {
                valores = armar_query_valores(valores, productosBD.Rows[ultima_fila]["precio_compra"].ToString().Replace(",", "."), true);
            }
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columnas, valores);
        }
        private void cargar_precio_venta(DataTable productosBD, string fecha)
        {
            double precio, diferencia, precio_nuevo;
            string columnas = "", valores = "", id = "";
            int nuevo_acuerdo = int.Parse(acuerdo_de_precio.Rows[0]["acuerdo"].ToString()) + 1;
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["proveedor"].ToString(), false);
            //acuerdo
            columnas = armar_query_columna(columnas, "acuerdo", false);
            valores = armar_query_valores(valores, nuevo_acuerdo.ToString(), false);
            //tipo_de_acuerdo
            columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = armar_query_valores(valores, acuerdo_de_precio.Rows[0]["tipo_de_acuerdo"].ToString(), false);
            //fecha

            columnas = armar_query_columna(columnas, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            for (int fila = 0; fila < productosBD.Rows.Count - 1; fila++)
            {
                id = productosBD.Rows[fila]["id"].ToString();
                columnas = armar_query_columna(columnas, "producto_" + id, false);
                if (productosBD.Rows[fila]["precio_compra"].ToString() == "N/A")
                {
                    precio = double.Parse(acuerdo_de_precio.Rows[0]["producto_" + id].ToString());
                    diferencia = (25 * precio) / 100;
                    precio_nuevo = precio + diferencia;
                    valores = armar_query_valores(valores, precio_nuevo.ToString().Replace(",", "."), false);
                }
                else //precio_final
                {
                    if (productosBD.Rows[fila]["precio_final"].ToString() == "N/A")
                    {

                        valores = armar_query_valores(valores, productosBD.Rows[fila]["precio_nuevo"].ToString().Replace(",", "."), false);
                    }
                    else
                    {
                        valores = armar_query_valores(valores, productosBD.Rows[fila]["precio_final"].ToString().Replace(",", "."), false);
                    }
                }
            }
            int ultima_fila = productosBD.Rows.Count - 1;
            id = productosBD.Rows[ultima_fila]["id"].ToString();
            columnas = armar_query_columna(columnas, "producto_" + id, true);
            if (productosBD.Rows[ultima_fila]["precio_compra"].ToString() == "N/A")
            {
                precio = double.Parse(acuerdo_de_precio.Rows[0]["producto_" + id].ToString());
                diferencia = (25 * precio) / 100;
                precio_nuevo = precio + diferencia;
                valores = armar_query_valores(valores, precio_nuevo.ToString().Replace(",", "."), true);
            }
            else
            {
                if (productosBD.Rows[ultima_fila]["precio_final"].ToString() == "N/A")
                {
                    valores = armar_query_valores(valores, productosBD.Rows[ultima_fila]["precio_nuevo"].ToString().Replace(",", "."), true);
                }
                else
                {
                    valores = armar_query_valores(valores, productosBD.Rows[ultima_fila]["precio_final"].ToString().Replace(",", "."), true);
                }
            }
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columnas, valores);
            desactivar_acuerdo_vigente(acuerdo_de_precio.Rows[0]["id"].ToString());
            //cambiar acuerdo de precios a pedidos no cargados
            consultar_pedidos_no_cargados(acuerdo_de_precio.Rows[0]["proveedor"].ToString());
            actualizar_acuerdo_en_pedidos_no_cargados(nuevo_acuerdo.ToString());
        }
        private void actualizar_stock(DataTable productosBD, string nombre_proveedor)
        {
            string id;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["stock_nuevo"].ToString() != "N/A")
                {
                    id = productosBD.Rows[fila]["id"].ToString();
                    consultas.actualizar_tabla(base_de_datos, nombre_proveedor, "`stock` = '" + productosBD.Rows[fila]["stock_nuevo"].ToString() + "'", id);
                }
            }
        }
        private void actualizar_acuerdo_en_pedidos_no_cargados(string nuevo_acuerdo)
        {
            string id_pedido = "";
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                id_pedido = pedidos_no_cargados.Rows[fila]["id"].ToString();
                consultas.actualizar_tabla(base_de_datos, "pedidos", "`acuerdo_de_precios` = '" + nuevo_acuerdo + "'", id_pedido);
            }
        }
        private void desactivar_acuerdo_vigente(string id_acuerdo)
        {
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios", "`activa` = '0'", id_acuerdo);
        }
        #endregion
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
    }

}
