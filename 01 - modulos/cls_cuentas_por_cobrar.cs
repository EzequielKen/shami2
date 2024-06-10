using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01___modulos
{
    public class cls_cuentas_por_cobrar
    {
        public cls_cuentas_por_cobrar(DataTable usuarios_BD)
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

        }
        #region atributos
        private cls_consultas_Mysql consultas;
        DataTable usuariosBD;
        cls_funciones funciones = new cls_funciones();

        private string servidor;
        private string puerto;
        private string usuario;
        private string password;
        private string base_de_datos;
        private string sucursal;

        DataTable remitos;
        DataTable remitos_proveedores_a_fabrica;
        DataTable sucursalBD;
        DataTable sucursales;
        DataTable lista_proveedores;
        DataTable lista_proveedores_fabrica;
        DataTable pedidos;
        DataTable pedidos_no_calculados;
        DataTable acuerdo_de_precios;
        DataTable acuerdo_de_precios_segun_parametros;
        DataTable productos_proveedor;
        DataTable imputaciones;
        DataTable imputaciones_fabrica_a_proveedor;
        DataTable pedidos_fabrica;
        #endregion

        #region cargar nota
        public void eliminar_imputacion(string id_imputacion)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "imputaciones", actualizar, id_imputacion);

        }
        public void cargar_nota(string id_orden, string nota)
        {
            string actualizar = "`nota` = '" + nota + "' ";
            consultas.actualizar_tabla(base_de_datos, "imputaciones", actualizar, id_orden);

        }
        public void cargar_nota_credito(string sucursal, string nota, string valor_remito, string fecha_nota)
        {
            cargar_nota_credito_en_cuentas_por_pagar(sucursal, nota, valor_remito, fecha_nota);
            cargar_nota_credito_en_remitos(sucursal, nota, valor_remito, fecha_nota);
        }

        private void cargar_nota_credito_en_cuentas_por_pagar(string sucursal, string nota, string valor_remito, string fecha_nota)
        {
            string columnas = "";
            string valores = "";
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal, false);
            //num_pedido
            columnas = funciones.armar_query_columna(columnas, "num_pedido", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //nombre_remito
            columnas = funciones.armar_query_columna(columnas, "nombre_remito", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //valor_remito
            columnas = funciones.armar_query_columna(columnas, "valor_remito", false);
            valores = funciones.armar_query_valores(valores, valor_remito, false);
            //fecha_remito
            columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
            valores = funciones.armar_query_valores(valores, fecha_nota, false);
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", true);
            valores = funciones.armar_query_valores(valores, "proveedor_villaMaipu", true);

            consultas.insertar_en_tabla(base_de_datos, "cuenta_por_pagar", columnas, valores);
        }
        private void cargar_nota_credito_en_remitos(string sucursal, string nota, string valor_remito, string fecha_nota)
        {
            string columnas = "";
            string valores = "";
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal, false);
            //num_pedido
            columnas = funciones.armar_query_columna(columnas, "num_pedido", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //valor_remito
            columnas = funciones.armar_query_columna(columnas, "valor_remito", false);
            valores = funciones.armar_query_valores(valores, valor_remito, false);
            //fecha_remito
            columnas = funciones.armar_query_columna(columnas, "fecha_remito", false);
            valores = funciones.armar_query_valores(valores, fecha_nota, false);
            //proveedor
            columnas = funciones.armar_query_columna(columnas, "proveedor", true);
            valores = funciones.armar_query_valores(valores, "proveedor_villaMaipu", true);

            consultas.insertar_en_tabla(base_de_datos, "remitos", columnas, valores);
        }
        #endregion


        #region metodos privados de consulta
        private void consultar_sucursal(string id)
        {
            sucursalBD = consultas.consultar_sucursal(int.Parse(id));
        }
        private void consultar_pedidos_fabrica()
        {
            pedidos_fabrica = consultas.consultar_tabla(base_de_datos, "pedidos_fabrica_a_proveedor");
        }
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        private void consultar_imputaciones()
        {
            imputaciones = consultas.consultar_tabla(base_de_datos, "imputaciones");
        }
        private void consultar_imputaciones_fabrica_a_proveedor()
        {
            imputaciones_fabrica_a_proveedor = consultas.consultar_tabla(base_de_datos, "imputaciones_fabrica_a_proveedor");
        }
        private void consultar_remitos()
        {
            remitos = consultas.consultar_tabla(base_de_datos, "cuenta_por_pagar");
        }
        private void consultar_remitos_proveedores_a_fabrica()
        {
            remitos_proveedores_a_fabrica = consultas.consultar_tabla(base_de_datos, "remitos_proveedores_a_fabrica");
        }
        private void consultar_lista_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla(base_de_datos, "lista_proveedores");
        }
        private void consultar_lista_proveedores_fabrica()
        {
            lista_proveedores_fabrica = consultas.consultar_tabla(base_de_datos, "lista_proveedores_fabrica");
        }
        private void consultar_pedidos()
        {
            pedidos = consultas.consultar_tabla(base_de_datos, "pedidos");
        }
        private void consultar_pedidos_no_calculados()
        {
            pedidos_no_calculados = consultas.consultar_pedidos_no_calculados_fabrica(base_de_datos, "pedidos");
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
        public DataTable get_sucursales_id(string id)
        {
            consultar_sucursal(id);
            return sucursalBD;
        }
        public DataTable get_pedidos_fabrica()
        {
            consultar_pedidos_fabrica();
            return pedidos_fabrica;
        }
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        public DataTable get_imputaciones()
        {
            consultar_imputaciones();
            return imputaciones;
        }
        public DataTable get_imputaciones_fabrica_a_proveedor()
        {
            consultar_imputaciones_fabrica_a_proveedor();
            return imputaciones_fabrica_a_proveedor;
        }
        public DataTable get_remitos()
        {
            consultar_remitos();
            return remitos;
        }
        public DataTable get_remitos_proveedores_a_fabrica()
        {
            consultar_remitos_proveedores_a_fabrica();
            return remitos_proveedores_a_fabrica;
        }
        public DataTable get_lista_proveedores()
        {
            consultar_lista_proveedores();
            return lista_proveedores;
        }
        public DataTable get_acuerdo_de_precio()
        {
            consultar_acuerdo_de_precios();
            return acuerdo_de_precios;
        }
        public DataTable get_lista_proveedores_fabrica()
        {
            consultar_lista_proveedores_fabrica();
            return lista_proveedores_fabrica;
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

        public void autorizar_imputacion(string id_imputacion)
        {
            consultas.actualizar_tabla(base_de_datos, "imputaciones", "`autorizado` = 'Si'", id_imputacion);
        }
        public void cargar_remito(string proveedor, string sucursal, string num_pedido, string tipo_de_acuerdoBD, string valor_remito, string fecha_remito, string descuento)
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

            columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = armar_query_valores(valores, tipo_de_acuerdoBD, false);

            columnas = armar_query_columna(columnas, "valor_remito", false);
            valores = armar_query_valores(valores, valor_remito, false);

            columnas = armar_query_columna(columnas, "fecha_remito", false);
            valores = armar_query_valores(valores, fecha_remito, false);

            columnas = armar_query_columna(columnas, "descuento", true);
            valores = armar_query_valores(valores, descuento, true);

            consultas.insertar_en_tabla(base_de_datos, "remitos", columnas, valores);
        }
        public void actualizar_lectura_de_pedido(string id_pedido)
        {
            string actualizar = "`calculado_local` = 'si'";
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
        }
        public bool cargar_imputacion(double efectivo, double transferencia, double mercado_pago, string proveedor_seleccionado)
        {
            string columnas, valores, fecha_now;
            columnas = "";
            valores = "";
            columnas = armar_query_columna(columnas, "activa", false);
            valores = armar_query_valores(valores, "2", false);

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

            System.DateTime fecha = System.DateTime.Now;
            fecha_now = fecha.Year.ToString() + "-" + fecha.Month.ToString() + "-" + fecha.Day.ToString();
            columnas = armar_query_columna(columnas, "fecha", true);
            valores = armar_query_valores(valores, fecha_now, true);

            return consultas.insertar_en_tabla(base_de_datos, "imputaciones", columnas, valores);
        }
        public bool cargar_imputacion_de_local_como_fabrica(double efectivo, double transferencia, double mercado_pago, string proveedor_seleccionado, string sucursal_seleccionada)
        {
            string columnas, valores, fecha_now;
            columnas = "";
            valores = "";
            columnas = armar_query_columna(columnas, "autorizado", false);
            valores = armar_query_valores(valores, "Si", false);

            columnas = armar_query_columna(columnas, "sucursal", false);
            valores = armar_query_valores(valores, sucursal_seleccionada, false);

            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor_seleccionado, false);

            columnas = armar_query_columna(columnas, "abono_efectivo", false);
            valores = armar_query_valores(valores, efectivo.ToString(), false);

            columnas = armar_query_columna(columnas, "abono_digital", false);
            valores = armar_query_valores(valores, transferencia.ToString(), false);

            columnas = armar_query_columna(columnas, "abono_digital_mercadoPago", false);
            valores = armar_query_valores(valores, mercado_pago.ToString(), false);

            System.DateTime fecha = System.DateTime.Now;
            fecha_now = fecha.Year.ToString() + "-" + fecha.Month.ToString() + "-" + fecha.Day.ToString();
            columnas = armar_query_columna(columnas, "fecha", true);
            valores = armar_query_valores(valores, fecha_now, true);

            return consultas.insertar_en_tabla(base_de_datos, "imputaciones", columnas, valores);
        }
        public bool cargar_imputacion_fabrica(double efectivo, double transferencia, double mercado_pago, string fabrica_seleccionada, string proveedor_seleccionado)
        {
            string columnas, valores;
            columnas = "";
            valores = "";
            columnas = armar_query_columna(columnas, "autorizado", false);
            valores = armar_query_valores(valores, "Si", false);

            columnas = armar_query_columna(columnas, "fabrica", false);
            valores = armar_query_valores(valores, fabrica_seleccionada, false);

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

            return consultas.insertar_en_tabla(base_de_datos, "imputaciones_fabrica_a_proveedor", columnas, valores);
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
