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
    public class cls_linkear_insumos_a_proveedor_de_fabrica
    {
        public cls_linkear_insumos_a_proveedor_de_fabrica(DataTable usuario_BD)
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
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable proveedor_fabrica_seleccionado;
        DataTable insumos;
        DataTable acuerdo_de_precios_fabrica_a_proveedores;
        #endregion

        #region carga a base de datos
        public void linkear_insumos_a_proveedor(DataTable acuerdo_de_precios_fabrica_a_proveedoresBD, string id_proveedor, DataTable insumos_proveedor)
        {
            desactivar_acuerdo_de_precios_vigente(acuerdo_de_precios_fabrica_a_proveedoresBD);
            string acuerdo = obtener_numero_de_acuerdo(acuerdo_de_precios_fabrica_a_proveedoresBD);

            string columna = "";
            string valor = "";

            //id_proveedor
            columna = armar_query_columna(columna, "id_proveedor", false);
            valor = armar_query_valores(valor, id_proveedor, false);
            //acuerdo
            columna = armar_query_columna(columna, "acuerdo", false);
            valor = armar_query_valores(valor, acuerdo, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valor = armar_query_valores(valor, fecha, false);
            int id = 1;
            string dato, id_insumo, producto, tipo_paquete, cantidad_de_unidad, unidad, precio;
            for (int fila = 0; fila < insumos_proveedor.Rows.Count - 1; fila++)
            {
                id_insumo = insumos_proveedor.Rows[fila]["id"].ToString();
                producto = insumos_proveedor.Rows[fila]["producto"].ToString();
                tipo_paquete = insumos_proveedor.Rows[fila]["tipo_paquete"].ToString();
                cantidad_de_unidad = insumos_proveedor.Rows[fila]["cantidad_unidades"].ToString();
                unidad = insumos_proveedor.Rows[fila]["unidad_medida"].ToString();
                precio = insumos_proveedor.Rows[fila]["precio_unidad"].ToString();

                dato = id_insumo + "-" + producto + "-" + tipo_paquete + "-" + cantidad_de_unidad + "-" + unidad + "-" + precio;
                columna = armar_query_columna(columna, "producto_" + id.ToString(), false);
                valor = armar_query_valores(valor, dato, false);
                id++;
            }
            int ultima_fila = insumos_proveedor.Rows.Count - 1;
            id_insumo = insumos_proveedor.Rows[ultima_fila]["id"].ToString();
            producto = insumos_proveedor.Rows[ultima_fila]["producto"].ToString();
            tipo_paquete = insumos_proveedor.Rows[ultima_fila]["tipo_paquete"].ToString();
            cantidad_de_unidad = insumos_proveedor.Rows[ultima_fila]["cantidad_unidades"].ToString();
            unidad = insumos_proveedor.Rows[ultima_fila]["unidad_medida"].ToString();
            precio = insumos_proveedor.Rows[ultima_fila]["precio_unidad"].ToString();

            dato = id_insumo + "-" + producto + "-" + tipo_paquete + "-" + cantidad_de_unidad + "-" + unidad + "-" + precio;

            columna = armar_query_columna(columna, "producto_" + id.ToString(), true);
            valor = armar_query_valores(valor, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", columna, valor);
        }

        private string obtener_numero_de_acuerdo(DataTable acuerdo_de_precios_fabrica_a_proveedoresBD)
        {
            int retorno = 1;
            if (acuerdo_de_precios_fabrica_a_proveedoresBD.Rows.Count > 0)
            {
                retorno = int.Parse(acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0]["acuerdo"].ToString()) + 1;
            }
            return retorno.ToString();
        }
        public void desactivar_acuerdo_de_precios_vigente(DataTable acuerdo_de_precios_fabrica_a_proveedoresBD)
        {
            if (acuerdo_de_precios_fabrica_a_proveedoresBD.Rows.Count > 0)
            {
                string id = acuerdo_de_precios_fabrica_a_proveedoresBD.Rows[0]["id"].ToString();
                string actualizar = "`activa` = '0'";
                consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", actualizar, id);

            }
        }
        public void actualizar_datos_proveedor(string id, string proveedor, string provincia, string localidad, string direccion, string telefono, string CBU_1, string CBU_2, string CBU_3, string CBU_4, string CBU_5, string condicion_pago)
        {
            string actualizar = "`proveedor` = '" + proveedor + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`provincia` = '" + provincia + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`localidad` = '" + localidad + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`direccion` = '" + direccion + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`telefono` = '" + telefono + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`condicion_pago` = '" + condicion_pago + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);

            actualizar = "`CBU_1` = '" + CBU_1 + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`CBU_2` = '" + CBU_2 + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`CBU_3` = '" + CBU_3 + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`CBU_4` = '" + CBU_4 + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);
            actualizar = "`CBU_5` = '" + CBU_5 + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedores_de_fabrica", actualizar, id);

        }
        #endregion

        #region metodos consultas
        private void consultar_insumos()
        {
            insumos = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        private void consultar_proveedor_fabrica_seleccionado(string id_proveedor)
        {
            proveedor_fabrica_seleccionado = consultas.consultar_proveeedor_de_fabrica_por_id(id_proveedor);
        }
        private void consultar_acuerdo_de_precios_fabrica_a_proveedores(string id_proveedor)
        {
            acuerdo_de_precios_fabrica_a_proveedores = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_activo(base_de_datos, "acuerdo_de_precios_fabrica_a_proveedores", id_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_proveedor_fabrica_seleccionado(string id_proveedor)
        {
            consultar_proveedor_fabrica_seleccionado(id_proveedor);
            return proveedor_fabrica_seleccionado;
        }
        public DataTable get_insumos()
        {
            consultar_insumos();
            return insumos;
        }
        public DataTable get_acuerdo_de_precios_fabrica_a_proveedores(string id_proveedor)
        {
            consultar_acuerdo_de_precios_fabrica_a_proveedores(id_proveedor);
            return acuerdo_de_precios_fabrica_a_proveedores;
        }
        #endregion

        #region funciones
        private int buscar_fila_producto(string id, DataTable productos)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= productos.Rows.Count - 1)
            {
                if (id == productos.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private string obtener_dato(string dato, int posicion_dato)
        {
            string retorno = "";
            int posicion = 0;
            int i = 0;
            while (i <= dato.Length - 1)
            {
                if (dato[i].ToString() == "-")
                {
                    posicion++;
                    if (posicion == posicion_dato)
                    {
                        break;
                    }
                    else
                    {
                        retorno = "";
                    }
                }
                else
                {
                    retorno = retorno + dato[i].ToString();
                }
                i++;
            }
            return retorno;
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
        public bool verificar_fecha(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year < int.Parse(año_dato))
            {
                retorno = true;
            }
            else if (fecha_1.Year == int.Parse(año_dato))
            {
                if (fecha_1.Month <= int.Parse(mes_dato))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
        public bool verificar_fecha_exacta(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year < int.Parse(año_dato))
            {
                retorno = true;
            }
            else if (fecha_1.Year == int.Parse(año_dato))
            {
                if (fecha_1.Month == int.Parse(mes_dato))
                {
                    retorno = true;
                }
            }
            return retorno;
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
