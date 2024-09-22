using modulos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using acceso_a_base_de_datos;
using _01___modulos;
using System.Configuration;
using paginaWeb;

namespace modulos
{
    public class cls_cuentas_por_pagar
    {
        public cls_cuentas_por_pagar(DataTable usuarios_BD, DataTable sucursal_BD)
        {
            usuariosBD = usuarios_BD;
            servidor = usuariosBD.Rows[0]["servidor"].ToString();
            puerto = usuariosBD.Rows[0]["puerto"].ToString();
            usuario = usuariosBD.Rows[0]["usuario_BD"].ToString();
            password = usuariosBD.Rows[0]["contraseña_BD"].ToString();
            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }

            consultas = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);

            funciones = new cls_funciones();

            sucursal = sucursal_BD.Rows[0]["sucursal"].ToString();
        }
        #region atributos
        private cls_consultas_Mysql consultas;
        private cls_whatsapp whatsapp;
        cls_funciones funciones;
        DataTable usuariosBD;

        private string servidor;
        private string puerto;
        private string usuario;
        private string password;
        private string base_de_datos;
        private string sucursal;

        DataTable remitos;
        DataTable lista_proveedores;
        DataTable pedidos;
        DataTable pedidos_no_calculados;
        DataTable acuerdo_de_precios;
        DataTable acuerdo_de_precios_segun_parametros;
        DataTable productos_proveedor;
        DataTable imputaciones;
        DataTable cuenta_por_pagar;
        DataTable deuda_actual;
        DataTable deuda_mes;
        #endregion

        #region carga a base de datos
        public void actualizar_deuda_del_mes(string id_deuda, string deuda_del_mes)
        {
            string actualizar = "`deuda_del_mes` = '" + deuda_del_mes + "'";
            consultas.actualizar_tabla(base_de_datos, "deudas_local_a_fabrica", actualizar, id_deuda);
        }
        public void crear_deuda_del_mes(string id_sucursal, string sucursal, string mes, string año, string deuda_del_mes)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, id_sucursal, false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal, false);
            //mes
            columnas = funciones.armar_query_columna(columnas, "mes", false);
            valores = funciones.armar_query_valores(valores, mes, false);
            //año
            columnas = funciones.armar_query_columna(columnas, "año", false);
            valores = funciones.armar_query_valores(valores, año, false);
            //deuda_del_mes
            columnas = funciones.armar_query_columna(columnas, "deuda_del_mes", true);
            valores = funciones.armar_query_valores(valores, deuda_del_mes, true);

            consultas.insertar_en_tabla(base_de_datos, "deudas_local_a_fabrica", columnas, valores);
        }
        #endregion

        #region metodos privados de consulta
        private void consultar_imputaciones(string sucursal, string mes, string año)
        {
            imputaciones = consultas.consultar_imputaciones_segun_fecha(sucursal, mes, año);
        }
        private void consultar_remitos(string sucursal, string mes, string año)
        {
            remitos = consultas.consultar_remitos_segun_fecha(sucursal, mes, año);
        }
        private void consultar_deuda_mes_local(string id_sucursal, string mes, string año)
        {
            deuda_mes = consultas.consultar_deuda_mes_local(id_sucursal, mes, año);
        }
        private void consultar_deuda_actual(string sucursal)
        {
            deuda_actual = consultas.consultar_deudas_de_sucursal(sucursal);
        }
        private void consultar_cuenta_por_pagar()
        {
            cuenta_por_pagar = consultas.consultar_tabla(base_de_datos, "cuenta_por_pagar");
        }
        private void consultar_imputaciones()
        {
            imputaciones = consultas.consultar_tabla(base_de_datos, "imputaciones");
        }
        private void consultar_remitos()
        {
            remitos = consultas.consultar_tabla(base_de_datos, "remitos");
        }
        private void consultar_lista_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla(base_de_datos, "lista_proveedores");
        }
        private void consultar_pedidos()
        {
            pedidos = consultas.consultar_tabla(base_de_datos, "pedidos");
        }
        private void consultar_pedidos_no_calculados()
        {
            pedidos_no_calculados = consultas.consultar_pedidos_no_calculados(base_de_datos, "pedidos");
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios");
        }
        private void consultar_acuerdo_de_precios_segun_parametros(string proveedor, string acuerdo_de_precios, string tipo_de_acuerdo)
        {
            acuerdo_de_precios_segun_parametros = consultas.consultar_acuerdo_de_precios_segun_parametros(base_de_datos, "acuerdo_de_precios", proveedor, acuerdo_de_precios, tipo_de_acuerdo);
        }
        private void consultar_productos_proveedor(string proveedor_seleccionado)
        {
            productos_proveedor = consultas.consultar_tabla_completa(base_de_datos, proveedor_seleccionado);
        }
        #endregion

        #region metodos get/set
        public DataTable get_imputaciones(string sucursal, string mes, string año)
        {
            consultar_imputaciones(sucursal, mes, año);
            return imputaciones;
        }
        public DataTable get_remitos(string sucursal, string mes, string año)
        {
            consultar_remitos(sucursal, mes, año);
            return remitos;
        }
        public DataTable get_deuda_mes(string id_sucursal, string mes, string año)
        {
            consultar_deuda_mes_local(id_sucursal, mes, año);
            return deuda_mes;
        }
        public DataTable get_deuda_actual(string sucursal)
        {
            consultar_deuda_actual(sucursal);
            deuda_actual.DefaultView.Sort = "año DESC, mes DESC";
            deuda_actual = deuda_actual.DefaultView.ToTable();
            return deuda_actual;
        }
        public DataTable get_imputaciones()
        {
            consultar_imputaciones();
            return imputaciones;
        }
        public DataTable get_remitos()
        {
            consultar_remitos();
            return remitos;
        }
        public DataTable get_cuentas_por_pagar()
        {
            consultar_cuenta_por_pagar();
            return cuenta_por_pagar;
        }
        public DataTable get_lista_proveedores()
        {
            consultar_lista_proveedores();
            return lista_proveedores;
        }
        public DataTable get_pedidos()
        {
            consultar_pedidos();
            return pedidos;
        }

        public DataTable get_pedidos_no_calculados()
        {
            consultar_pedidos_no_calculados();
            return pedidos_no_calculados;
        }
        public DataTable get_acuerdo_de_precios()
        {
            consultar_acuerdo_de_precios();
            return acuerdo_de_precios;
        }
        public DataTable get_acuerdo_de_precios_segun_parametros(string proveedor, string acuerdo_de_precio, string tipo_de_acuerdo)
        {
            consultar_acuerdo_de_precios_segun_parametros(proveedor, acuerdo_de_precio, tipo_de_acuerdo);
            return acuerdo_de_precios_segun_parametros;
        }
        public DataTable get_productos_proveedor(string proveedor_seleccionado)
        {
            consultar_productos_proveedor(proveedor_seleccionado);
            return productos_proveedor;
        }
        #endregion

        public void eliminar_imputacion(string id_imputacion)
        {
            string actualizar= "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "imputaciones", actualizar,id_imputacion);
        }
        public string cancelar_pedido(string id_pedido, string num_pedido, string proveedor, DataTable usuariosBD, DataTable sucursalBD)
        {
            consultas.actualizar_tabla(base_de_datos, "pedidos", "`estado` = 'Cancelado'", id_pedido);
            consultar_lista_proveedores();
            whatsapp = new cls_whatsapp();
            return whatsapp.cancelar_pedido(num_pedido, lista_proveedores, sucursalBD);
        }
        public void cargar_remito(string proveedor, string sucursal, string num_pedido, string tipo_de_acuerdoBD, string valor_remito, string fecha_remito, string descuento,string impuesto)
        {
            string columnas, valores;
            columnas = "";
            valores = "";



            columnas = armar_query_columna(columnas, "activa", false);
            valores = armar_query_valores(valores, "1", false);

            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor, false);

            columnas = armar_query_columna(columnas, "sucursal", false);
            valores = armar_query_valores(valores, sucursal, false);

            columnas = armar_query_columna(columnas, "num_pedido", false);
            valores = armar_query_valores(valores, num_pedido, false);

            columnas = armar_query_columna(columnas, "valor_remito", false);
            valores = armar_query_valores(valores, valor_remito, false);

            columnas = armar_query_columna(columnas, "aumento", false);
            valores = armar_query_valores(valores, impuesto, false);

            columnas = armar_query_columna(columnas, "fecha_remito", true);
            valores = armar_query_valores(valores, fecha_remito, true);


            actualizar_o_crear_remito(proveedor, sucursal, num_pedido, valor_remito,columnas,valores);

            actualizar_remito_fabrica(proveedor,sucursal,num_pedido,valor_remito);
        }
        private void actualizar_o_crear_remito(string proveedor, string sucursal, string num_pedido, string valor_remito,string columnas,string valores)
        {
            DataTable remito = consultas.consultar_remito_de_pedido(base_de_datos, proveedor, sucursal, num_pedido);
            if (remito.Rows.Count > 0)
            {
                string actualizar = "`valor_remito` = '" + valor_remito + "'";
                string id_remito_fabrica = remito.Rows[0]["id"].ToString();
                consultas.actualizar_tabla(base_de_datos, "remitos", actualizar, id_remito_fabrica);
            }
            else
            {
                consultas.insertar_en_tabla(base_de_datos, "remitos", columnas, valores);
            }
        }
        private void actualizar_remito_fabrica(string proveedor, string sucursal, string num_pedido, string valor_remito)
        {
            DataTable remito_fabrica = consultas.consultar_cuentas_por_pagar_de_pedido(base_de_datos, proveedor, sucursal, num_pedido);
            if (remito_fabrica.Rows.Count > 0)
            {
                string actualizar = "`valor_remito` = '" + valor_remito + "'";
                string id_remito_fabrica= remito_fabrica.Rows[0]["id"].ToString();
                consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar",actualizar,id_remito_fabrica);
            }
        }
        public void actualizar_lectura_de_pedido(string id_pedido)
        {
            string actualizar = "`estado` = 'Entregado', `calculado_local` = 'si'";
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
        }
        public bool cargar_imputacion(double efectivo, double transferencia, double mercado_pago, string proveedor_seleccionado)
        {
            string columnas, valores;
            columnas = "";
            valores = "";
            columnas = armar_query_columna(columnas, "autorizado", false);
            valores = armar_query_valores(valores, "No", false);

            columnas = armar_query_columna(columnas, "sucursal", false);
            valores = armar_query_valores(valores, sucursal, false);

            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor_seleccionado, false);

            columnas = armar_query_columna(columnas, "abono_efectivo", false);
            valores = armar_query_valores(valores, efectivo.ToString(), false);

            columnas = armar_query_columna(columnas, "abono_digital", false);
            valores = armar_query_valores(valores, transferencia.ToString(), false);

            columnas = armar_query_columna(columnas, "abono_digital_mercadoPago", false);
            valores = armar_query_valores(valores, mercado_pago.ToString(), false);

            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columnas = armar_query_columna(columnas, "fecha", true);
            valores = armar_query_valores(valores, fecha, true);

            return consultas.insertar_en_tabla(base_de_datos, "imputaciones", columnas, valores);
        }
        public void cargar_remito(string sucursal, string num_pedido, string valor_remito, string proveedor)
        {
            string columnas, valores;
            columnas = "";
            valores = "";
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //fecha_remito
            columnas = armar_query_columna(columnas, "sucursal", false);
            valores = armar_query_valores(valores, sucursal, false);

            columnas = armar_query_columna(columnas, "num_pedido", false);
            valores = armar_query_valores(valores, num_pedido, false);

            columnas = armar_query_columna(columnas, "valor_remito", false);
            valores = armar_query_valores(valores, valor_remito, false);

            columnas = armar_query_columna(columnas, "fecha_remito", false);
            valores = armar_query_valores(valores, fecha, false);

            columnas = armar_query_columna(columnas, "proveedor", true);
            valores = armar_query_valores(valores, proveedor, true);

            consultas.insertar_en_tabla(base_de_datos, "remitos", columnas, valores);
        }
        public void actualizar_estado_pedido_a_entregado(string id_pedido)
        {
            consultas.actualizar_tabla(base_de_datos, "pedidos", "`estado` = 'Entregado'", id_pedido);
        }
        public void cargar_cantidad_recibida_pedido(string id_pedido, DataTable pedido)
        {
            // string actualizar = `producto_4` = '$ 4.488,00-5-Shawarma de cordero-200-200123'
            string actualizar;
            int i = 1;
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                actualizar = "`producto_" + i.ToString() + "` = '" + pedido.Rows[fila]["pedido_dato"].ToString() + pedido.Rows[fila]["nueva_cantidad"].ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
                i++;
            }
        }
        public void actualizar_cuenta_por_pagar(string num_pedido, string proveedor, string sucursal, string valor_remito)
        {
            consultar_cuenta_por_pagar();
            int fila = 0;
            string id_cuenta = "";
            while (fila <= cuenta_por_pagar.Rows.Count - 1)
            {
                if (num_pedido == cuenta_por_pagar.Rows[fila]["num_pedido"].ToString() &&
                    proveedor == cuenta_por_pagar.Rows[fila]["proveedor"].ToString() &&
                    sucursal == cuenta_por_pagar.Rows[fila]["sucursal"].ToString())
                {
                    id_cuenta = cuenta_por_pagar.Rows[fila]["id"].ToString();
                    break;
                }
                fila++;
            }
            string actualizar = "`valor_remito` = '" + valor_remito + "'";
            consultas.actualizar_tabla(base_de_datos, "cuenta_por_pagar", actualizar, id_cuenta);
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
    }
}
