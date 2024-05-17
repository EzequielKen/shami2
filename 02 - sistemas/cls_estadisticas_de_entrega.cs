using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_estadisticas_de_entrega
    {
        public cls_estadisticas_de_entrega(DataTable usuario_BD)
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
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos_fabrica;
        DataTable productos_terminados;
        DataTable pedidos;
        DataTable resumen;
        DataTable sucursal;
        DataTable pedido_de_insumos;
        List<DataTable> lista_pedidos_sucursales = new List<DataTable>();
        #endregion

        #region crear tabla resumen
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("cantidad_pedida", typeof(string));
            resumen.Columns.Add("cantidad_entregada", typeof(string));
            resumen.Columns.Add("porcentaje_satisfaccion", typeof(string));
            resumen.Columns.Add("presentacion", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("incremento", typeof(string));
            resumen.Columns.Add("objetivo", typeof(string));
        }
        #endregion
        #region analisis de produccion
        private void calcular_incremento()
        {
            double cantidad_pedida, stock, incremento, objetivo;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {

                cantidad_pedida = double.Parse(resumen.Rows[fila]["cantidad_pedida"].ToString());
                incremento = (20 * cantidad_pedida) / 100;
                incremento = incremento + cantidad_pedida;
                resumen.Rows[fila]["incremento"] = Math.Ceiling(incremento);
                stock = double.Parse(resumen.Rows[fila]["stock"].ToString());
                objetivo = incremento - stock;
                resumen.Rows[fila]["objetivo"] = Math.Ceiling(objetivo);
            }
        }
        private void calcular_analisis_de_producccion()
        {
            crear_tabla_resumen();

            string id_producto;
            int columna;
            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                columna = pedidos.Columns["producto_1"].Ordinal;
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {
                    if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                    pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {
                        columna = pedidos.Columns["producto_1"].Ordinal;
                        while (columna <= pedidos.Columns.Count - 1 &&
                        funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                        {
                            id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                            if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                            {
                                cargar_estadistica_de_produccion(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu");
                            }
                            /* else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                             {
                                 cargar_estadistica_de_produccion(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica");
                             }*/
                            columna++;
                        }
                    }
                }
            }
            calcular_incremento();
        }
        private void cargar_estadistica_de_produccion(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor)
        {
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            if (fila_producto != -1)
            {

                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 4);
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                    resumen.Rows[fila_resumen]["stock"] = productos_seleccionados.Rows[fila_producto]["stock"].ToString();
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 4));
                }
            }
        }
        #endregion
        #region calculo de estadisticas
        private void calcular_estadisticas()
        {
            crear_tabla_resumen();

            string id_producto;
            int columna = pedidos.Columns["producto_1"].Ordinal;
            for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
            {
                if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                {
                    columna = pedidos.Columns["producto_1"].Ordinal;
                    while (columna <= pedidos.Columns.Count - 1 &&
                    funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                    {
                        id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                        if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                        {
                            cargar_estadistica(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu", pedidos, 4);
                        }
                        else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                        {
                            cargar_estadistica(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica", pedidos, 4);
                        }
                        columna++;
                    }
                }
            }
        }
        private void calcular_estadisticas_segun_sucursal()
        {
            crear_tabla_resumen();

            string id_producto;
            int columna;
            for (int fila_pedidos = 0; fila_pedidos <= lista_pedidos_sucursales.Count - 1; fila_pedidos++)
            {
                pedidos = lista_pedidos_sucursales[fila_pedidos];
                columna = pedidos.Columns["producto_1"].Ordinal;
                for (int fila = 0; fila <= pedidos.Rows.Count - 1; fila++)
                {
                    if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu" ||
                    pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                    {

                        columna = pedidos.Columns["producto_1"].Ordinal;
                        while (columna <= pedidos.Columns.Count - 1 &&
                        funciones.IsNotDBNull(pedidos.Rows[fila][columna]))
                        {
                            if (fila == 13 && fila_pedidos == 3 && columna == 22)
                            {
                                string stop = "";
                            }
                            id_producto = funciones.obtener_dato(pedidos.Rows[fila][columna].ToString(), 2);
                            if (pedidos.Rows[fila]["proveedor"].ToString() == "proveedor_villaMaipu")
                            {
                                cargar_estadistica(fila, columna, id_producto, productos_terminados, "proveedor_villaMaipu", pedidos, 4);
                            }
                            else if (pedidos.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                            {
                                cargar_estadistica(fila, columna, id_producto, insumos_fabrica, "insumos_fabrica", pedidos, 4);
                            }
                            columna++;
                        }
                    }
                }
            }
            int ante_ultima_fila = resumen.Rows.Count - 1;
            string id;
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida, cantidad_entrega, porcentaje_satisfaccion;
            for (int filas = 0; filas <= resumen.Rows.Count - 1; filas++)
            {
                if (resumen.Rows[filas]["cantidad_pedida"].ToString() != "")
                {

                    id = resumen.Rows[filas]["id"].ToString();
                    if (id == "45" && resumen.Rows[filas]["proveedor"].ToString() == "proveedor_villaMaipu")
                    {
                        string stop = "";
                    }
                    /*if (resumen.Rows[filas]["proveedor"].ToString() == "insumos_fabrica")
                    {

                        fila_producto = funciones.buscar_fila_por_id(id, insumos_fabrica);
                        cantidad_pedida = double.Parse(resumen.Rows[filas]["cantidad_pedida"].ToString());
                        cantidad_pedida = cantidad_pedida / double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila_producto]["unidad_de_medida_local"].ToString(), 2));
                        resumen.Rows[filas]["cantidad_pedida"] = Math.Ceiling(cantidad_pedida);
                    }*/

                    string datop = resumen.Rows[filas]["cantidad_entregada"].ToString();
                    cantidad_pedida = double.Parse(resumen.Rows[filas]["cantidad_pedida"].ToString());
                    if (resumen.Rows[filas]["cantidad_entregada"].ToString() != "")
                    {

                        cantidad_entrega = double.Parse(resumen.Rows[filas]["cantidad_entregada"].ToString());
                        porcentaje_satisfaccion = (cantidad_entrega * 100) / cantidad_pedida;
                        porcentaje_satisfaccion = Math.Round(porcentaje_satisfaccion, 2);
                        resumen.Rows[filas]["porcentaje_satisfaccion"] = porcentaje_satisfaccion.ToString() + "%";
                    }
                }

            }
        }
        private void cargar_estadistica(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor, DataTable pedido, int posicion)
        {

            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida, cantidad_entregada;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            if (fila_producto != -1)
            {
                // cantidad_entregada porcentaje_satisfaccion
                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    if ("" == funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion))
                    {
                        resumen.Rows[fila_resumen]["cantidad_pedida"] = "0";
                    }
                    else
                    {
                        resumen.Rows[fila_resumen]["cantidad_pedida"] = funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion);
                    }
                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado")
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {
                            resumen.Rows[fila_resumen]["cantidad_entregada"] = funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1);
                        }
                    }
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                    if (pedido.Rows[fila]["estado"].ToString() == "Entregado" && "" != resumen.Rows[fila_resumen]["cantidad_entregada"].ToString())
                    {
                        if ("N/A" != funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1))
                        {
                            string dato = resumen.Rows[fila_resumen]["cantidad_entregada"].ToString();
                            cantidad_entregada = double.Parse(resumen.Rows[fila_resumen]["cantidad_entregada"].ToString());
                            resumen.Rows[fila_resumen]["cantidad_entregada"] = cantidad_entregada + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion + 1));
                        }
                    }

                }
            }
        }
        private void cargar_estadistica_produccion(int fila, int columna, string id_producto, DataTable productos_seleccionados, string proveedor, DataTable pedido, int posicion)
        {
            int fila_resumen = 0;
            int fila_producto;
            double cantidad_pedida;
            fila_producto = funciones.buscar_fila_por_id(id_producto, productos_seleccionados);
            if (fila_producto != -1)
            {

                fila_resumen = funciones.buscar_fila_por_id_nombre(id_producto, productos_seleccionados.Rows[fila_producto]["producto"].ToString(), resumen);
                if (fila_resumen == -1)
                {
                    resumen.Rows.Add();
                    fila_resumen = resumen.Rows.Count - 1;
                    resumen.Rows[fila_resumen]["id"] = productos_seleccionados.Rows[fila_producto]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = productos_seleccionados.Rows[fila_producto]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = productos_seleccionados.Rows[fila_producto]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                    resumen.Rows[fila_resumen]["presentacion"] = productos_seleccionados.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                    resumen.Rows[fila_resumen]["proveedor"] = proveedor;
                }
                else
                {
                    cantidad_pedida = double.Parse(resumen.Rows[fila_resumen]["cantidad_pedida"].ToString());
                    resumen.Rows[fila_resumen]["cantidad_pedida"] = cantidad_pedida + double.Parse(funciones.obtener_dato(pedido.Rows[fila][columna].ToString(), posicion));
                }
            }

        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villaMaipu");
        }

        private void consultar_productos_terminados_fabrica_fatay()
        {
            productos_terminados = consultas.consultar_productos_produccion_fabrica_fatay(base_de_datos, "proveedor_villaMaipu");
        }
        private void consultar_pedidos(string sucursal, string fecha_inicio, string fecha_fin)
        {
            pedidos = consultas.consultar_pedidos_segun_rango_de_fecha(sucursal, fecha_inicio, fecha_fin);
        }
        private void consultar_pedidos_segun_sucursal(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {
            lista_pedidos_sucursales.Clear();
            for (int fila = 0; fila <= resumen_sucursales.Rows.Count - 1; fila++)
            {
                lista_pedidos_sucursales.Add(consultas.consultar_pedidos_entregados_segun_rango_de_fecha(resumen_sucursales.Rows[fila]["sucursal"].ToString(), fecha_inicio, fecha_fin));
            }
        }
        private void consultar_orden_de_pedido_segun_sucursal(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {

            pedido_de_insumos = consultas.consultar_pedidos_produccion_segun_rango_de_fecha("Shami Villa Maipu Limpieza", fecha_inicio, fecha_fin);
        }

        private void consultar_sucursal()
        {
            sucursal = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        #endregion

        #region metodos get/set
        public DataTable obtener_estadisticas_de_pedido(string sucursal, string fecha_inicio, string fecha_fin)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos(sucursal, fecha_inicio, fecha_fin);
            calcular_estadisticas();
            return resumen;
        }
        public DataTable obtener_estadisticas_de_pedido_segun_sucursales(DataTable resumen_sucursales, string fecha_inicio, string fecha_fin)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos_segun_sucursal(resumen_sucursales, fecha_inicio, fecha_fin);
            //consultar_orden_de_pedido_segun_sucursal(resumen_sucursales, fecha_inicio, fecha_fin);
            calcular_estadisticas_segun_sucursal();
            return resumen;
        }
        public DataTable get_analisis_produccion(string fecha_inicio, string fecha_fin)
        {
            consultar_sucursal();

            //consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_pedidos_segun_sucursal(sucursal, fecha_inicio, fecha_fin);
            calcular_analisis_de_producccion();
            return resumen;
        }

        public DataTable get_analisis_produccion_fabrica_fatay(string fecha_inicio, string fecha_fin)
        {
            consultar_sucursal();

            //consultar_insumos_fabrica();
            consultar_productos_terminados_fabrica_fatay();
            consultar_pedidos_segun_sucursal(sucursal, fecha_inicio, fecha_fin);
            calcular_analisis_de_producccion();
            return resumen;
        }
        public DataTable get_sucursal()
        {
            consultar_sucursal();
            sucursal.DefaultView.Sort = "sucursal asc";
            sucursal = sucursal.DefaultView.ToTable();
            return sucursal;
        }
        #endregion
    }
}
