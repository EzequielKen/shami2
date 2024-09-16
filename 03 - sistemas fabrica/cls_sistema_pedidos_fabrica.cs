using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_sistema_pedidos_fabrica
    {
        public cls_sistema_pedidos_fabrica(DataTable usuarios_BD)
        {
            usuariosBD = usuarios_BD;
            pedidos = new cls_pedidos_fabrica(usuarios_BD);
            movimientos_stock_producto = new cls_movimientos_stock_producto(usuarios_BD);
            stock_insumos = new cls_stock_insumos(usuarios_BD);
            historial_stock = new cls_movimientos_stock_producto(usuarios_BD);
            sistema_Administracion = new cls_sistema_cuentas_por_cobrar(usuarios_BD);
            stock_producto_terminado = new cls_stock_producto_terminado(usuarios_BD);
        }
        #region atributos
        cls_movimientos_stock_producto movimientos_stock_producto;
        cls_sistema_cuentas_por_cobrar sistema_Administracion;
        cls_stock_insumos stock_insumos;
        cls_stock_producto_terminado stock_producto_terminado;
        cls_movimientos_stock_producto historial_stock;
        cls_funciones funciones = new cls_funciones();
        private DataTable usuariosBD;
        private DataTable sucursalBD;
        private DataTable sucursales;
        private DataTable proveedores_fabrica;
        private DataTable pedidos_sucursal;
        private DataTable pedidos_no_cargados;
        private DataTable pedidos_fabrica;
        private DataTable pedido;
        private DataTable pedido_copia;
        private DataTable resumen_pedido;
        private DataTable productos_proveedor;
        private DataTable acuerdo_de_precios;
        private DataTable acuerdo_de_precio_activo;
        private DataTable resumen_de_pedidos;
        private DataTable producto_produccion_diaria;
        private DataTable insumos;
        private DataTable insumos_copia;
        private List<string> sucursales_que_pidieron;
        private cls_pedidos_fabrica pedidos;
        cls_whatsapp whatsapp;
        cls_PDF PDF = new cls_PDF();
        #endregion

        #region resumen de pedidos
        private bool verificar_si_selecciono(DataTable resumen_sucursales, string sucursal_dato)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= resumen_sucursales.Rows.Count - 1)
            {
                if (sucursal_dato == resumen_sucursales.Rows[fila]["sucursal"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private bool verificar_si_cargo(List<string> sucursales, string sucursal_dato)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= sucursales.Count - 1)
            {
                if (sucursal_dato == sucursales[fila].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int verificar_si_cargo_producto(string id_producto, string nombre_producto)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= resumen_de_pedidos.Rows.Count - 1)
            {
                if (id_producto == resumen_de_pedidos.Rows[fila]["id_producto"].ToString() &&
                    nombre_producto == resumen_de_pedidos.Rows[fila]["producto"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void llenar_sucursales_que_pidieron(DataTable resumen_sucursales)
        {
            sucursales_que_pidieron = new List<string>();
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                if (!verificar_si_cargo(sucursales_que_pidieron, pedidos_no_cargados.Rows[fila]["sucursal"].ToString()) &&
                    verificar_si_selecciono(resumen_sucursales, pedidos_no_cargados.Rows[fila]["sucursal"].ToString()))
                {
                    sucursales_que_pidieron.Add(pedidos_no_cargados.Rows[fila]["sucursal"].ToString());
                }
            }
        }
        private void crear_tabla_resumen_de_pedidos(DataTable resumen_sucursales)
        {

            //consultar_pedidos_no_cargados();
            llenar_sucursales_que_pidieron(resumen_sucursales);
            resumen_de_pedidos = new DataTable();
            string dato;
            resumen_de_pedidos.Columns.Add("stock_resumen", typeof(string));
            resumen_de_pedidos.Columns.Add("tipo producto", typeof(string));
            resumen_de_pedidos.Columns.Add("id_producto", typeof(string));
            resumen_de_pedidos.Columns.Add("total", typeof(string));
            resumen_de_pedidos.Columns.Add("unidad de medida", typeof(string));
            resumen_de_pedidos.Columns.Add("producto", typeof(string));
            for (int fila = 0; fila <= sucursales_que_pidieron.Count - 1; fila++)
            {
                dato = sucursales_que_pidieron[fila].ToString();
                resumen_de_pedidos.Columns.Add(dato, typeof(string));
            }

        }
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
        private void llenar_tabla_resumen_de_pedidos(DataTable resumen_sucursales)
        {
            string proveedor_de_pedido;
            pedidos_no_cargados.DefaultView.Sort = "proveedor ASC";
            pedidos_no_cargados = pedidos_no_cargados.DefaultView.ToTable();
            string proveedor_seleccionado = pedidos_no_cargados.Rows[0]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor_seleccionado);
            int id_producto, fila_producto;
            string sucursal_pedido, nombre_producto;
            int fila_resumen = 0;

            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                sucursal_pedido = pedidos_no_cargados.Rows[fila]["sucursal"].ToString();
                proveedor_de_pedido = pedidos_no_cargados.Rows[fila]["proveedor"].ToString();
                if (proveedor_seleccionado != proveedor_de_pedido)
                {
                    consultar_productos_proveedor(proveedor_de_pedido);
                    proveedor_seleccionado = proveedor_de_pedido;
                }
                if (verificar_si_selecciono(resumen_sucursales, sucursal_pedido))
                {
                    for (int columna = pedidos_no_cargados.Columns["producto_1"].Ordinal; columna <= pedidos_no_cargados.Columns.Count - 1; columna++)
                    {
                        if (IsNotDBNull(pedidos_no_cargados.Rows[fila][columna]))
                        {
                            if (pedidos_no_cargados.Rows[fila][columna].ToString() != string.Empty)
                            {
                                id_producto = int.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 2));
                                fila_producto = buscar_fila_producto(id_producto.ToString(), productos_proveedor);
                                nombre_producto = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                fila_resumen = verificar_si_cargo_producto(id_producto.ToString(), nombre_producto);
                                if (fila_resumen == -1)
                                {
                                    resumen_de_pedidos.Rows.Add();
                                    fila_resumen = resumen_de_pedidos.Rows.Count - 1;
                                    resumen_de_pedidos.Rows[fila_resumen]["id_producto"] = id_producto.ToString();
                                    resumen_de_pedidos.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                    resumen_de_pedidos.Rows[fila_resumen]["tipo producto"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
                                    resumen_de_pedidos.Rows[fila_resumen]["unidad de medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                                    resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                }
                                else
                                {
                                    double cantidad_cargada, cantidad_a_cargar;
                                    if (double.TryParse(resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido].ToString(), out cantidad_cargada))
                                    {
                                        cantidad_a_cargar = double.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4));
                                        resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = Math.Round(cantidad_cargada + cantidad_a_cargar, 2).ToString();

                                    }
                                    else
                                    {
                                        resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                    }
                                }

                            }
                        }
                    }
                }

            }
            double total, cantidad;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                total = 0;
                for (int columna = resumen_de_pedidos.Columns["producto"].Ordinal + 1; columna <= resumen_de_pedidos.Columns.Count - 1; columna++)
                {
                    if (double.TryParse(resumen_de_pedidos.Rows[fila][columna].ToString().Replace(",", "."), out cantidad))
                    {
                        total = total + cantidad;

                    }
                }
                resumen_de_pedidos.Rows[fila]["total"] = total.ToString();
            }

        }
        private void llenar_tabla_resumen_de_pedidos_produccion_diaria(DataTable resumen_sucursales)
        {
            string proveedor_de_pedido;
            pedidos_no_cargados.DefaultView.Sort = "proveedor ASC";
            pedidos_no_cargados = pedidos_no_cargados.DefaultView.ToTable();
            string proveedor_seleccionado = pedidos_no_cargados.Rows[0]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor_seleccionado);
            int id_producto, fila_producto;
            string sucursal_pedido, nombre_producto;
            int fila_resumen = 0;

            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                sucursal_pedido = pedidos_no_cargados.Rows[fila]["sucursal"].ToString();
                proveedor_de_pedido = pedidos_no_cargados.Rows[fila]["proveedor"].ToString();
                if (proveedor_seleccionado != proveedor_de_pedido)
                {
                    consultar_productos_proveedor(proveedor_de_pedido);
                    proveedor_seleccionado = proveedor_de_pedido;
                }
                if (verificar_si_selecciono(resumen_sucursales, sucursal_pedido) && proveedor_de_pedido != "insumos_fabrica")
                {
                    for (int columna = pedidos_no_cargados.Columns["producto_1"].Ordinal; columna < pedidos_no_cargados.Columns.Count - 1; columna++)
                    {
                        if (IsNotDBNull(pedidos_no_cargados.Rows[fila][columna]))
                        {
                            if (pedidos_no_cargados.Rows[fila][columna].ToString() != string.Empty)
                            {
                                id_producto = int.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 2));
                                fila_producto = buscar_fila_producto(id_producto.ToString(), productos_proveedor);
                                nombre_producto = productos_proveedor.Rows[fila_producto]["producto"].ToString();

                                if (productos_proveedor.Rows[fila_producto]["imprimir_resumen_produccion_diaria"].ToString() == "1")
                                {
                                    fila_resumen = verificar_si_cargo_producto(id_producto.ToString(), nombre_producto);
                                    if (fila_resumen == -1)
                                    {
                                        resumen_de_pedidos.Rows.Add();
                                        fila_resumen = resumen_de_pedidos.Rows.Count - 1;
                                        resumen_de_pedidos.Rows[fila_resumen]["id_producto"] = id_producto.ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["tipo producto"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["unidad de medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                    }
                                    else
                                    {
                                        double cantidad_cargada, cantidad_a_cargar;
                                        if (double.TryParse(resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido].ToString(), out cantidad_cargada))
                                        {
                                            cantidad_a_cargar = double.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4));
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = Math.Round(cantidad_cargada + cantidad_a_cargar, 2).ToString();

                                        }
                                        else
                                        {
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }
            double total, cantidad;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                total = 0;
                for (int columna = resumen_de_pedidos.Columns["producto"].Ordinal + 1; columna <= resumen_de_pedidos.Columns.Count - 1; columna++)
                {
                    if (double.TryParse(resumen_de_pedidos.Rows[fila][columna].ToString().Replace(",", "."), out cantidad))
                    {
                        total = total + cantidad;

                    }
                }
                resumen_de_pedidos.Rows[fila]["total"] = total.ToString();
            }

        }
        private void llenar_tabla_resumen_de_pedidos_panificados(DataTable resumen_sucursales)
        {
            string proveedor_de_pedido;
            pedidos_no_cargados.DefaultView.Sort = "proveedor ASC";
            pedidos_no_cargados = pedidos_no_cargados.DefaultView.ToTable();
            string proveedor_seleccionado = pedidos_no_cargados.Rows[0]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor_seleccionado);
            int id_producto, fila_producto;
            string sucursal_pedido, nombre_producto;
            int fila_resumen = 0;

            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                sucursal_pedido = pedidos_no_cargados.Rows[fila]["sucursal"].ToString();
                proveedor_de_pedido = pedidos_no_cargados.Rows[fila]["proveedor"].ToString();
                if (proveedor_seleccionado != proveedor_de_pedido)
                {
                    consultar_productos_proveedor(proveedor_de_pedido);
                    proveedor_seleccionado = proveedor_de_pedido;
                }
                if (verificar_si_selecciono(resumen_sucursales, sucursal_pedido))
                {
                    for (int columna = pedidos_no_cargados.Columns["producto_1"].Ordinal; columna < pedidos_no_cargados.Columns.Count - 1; columna++)
                    {
                        if (IsNotDBNull(pedidos_no_cargados.Rows[fila][columna]))
                        {
                            if (pedidos_no_cargados.Rows[fila][columna].ToString() != string.Empty)
                            {
                                id_producto = int.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 2));
                                fila_producto = buscar_fila_producto(id_producto.ToString(), productos_proveedor);
                                nombre_producto = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                if (productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString() == "3-Panificados")
                                {
                                    fila_resumen = verificar_si_cargo_producto(id_producto.ToString(), nombre_producto);
                                    if (fila_resumen == -1)
                                    {
                                        resumen_de_pedidos.Rows.Add();
                                        fila_resumen = resumen_de_pedidos.Rows.Count - 1;
                                        resumen_de_pedidos.Rows[fila_resumen]["id_producto"] = id_producto.ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["tipo producto"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["unidad de medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                    }
                                    else
                                    {
                                        double cantidad_cargada, cantidad_a_cargar;
                                        if (double.TryParse(resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido].ToString(), out cantidad_cargada))
                                        {
                                            cantidad_a_cargar = double.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4));
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = Math.Round(cantidad_cargada + cantidad_a_cargar, 2).ToString();

                                        }
                                        else
                                        {
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }

            double total, cantidad;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                total = 0;
                for (int columna = resumen_de_pedidos.Columns["producto"].Ordinal + 1; columna <= resumen_de_pedidos.Columns.Count - 1; columna++)
                {
                    if (double.TryParse(resumen_de_pedidos.Rows[fila][columna].ToString().Replace(",", "."), out cantidad))
                    {
                        total = total + cantidad;

                    }
                }
                resumen_de_pedidos.Rows[fila]["total"] = total.ToString();
            }

        }

        private void llenar_tabla_de_pedidos_panificados(DataTable resumen_sucursales)
        {
            string proveedor_de_pedido;
            pedidos_no_cargados.DefaultView.Sort = "proveedor ASC";
            pedidos_no_cargados = pedidos_no_cargados.DefaultView.ToTable();
            string proveedor_seleccionado = pedidos_no_cargados.Rows[0]["proveedor"].ToString();
            consultar_productos_proveedor(proveedor_seleccionado);
            int id_producto, fila_producto;
            string sucursal_pedido, nombre_producto;
            int fila_resumen = 0;

            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                sucursal_pedido = pedidos_no_cargados.Rows[fila]["sucursal"].ToString();
                proveedor_de_pedido = pedidos_no_cargados.Rows[fila]["proveedor"].ToString();
                if (proveedor_seleccionado != proveedor_de_pedido)
                {
                    consultar_productos_proveedor(proveedor_de_pedido);
                    proveedor_seleccionado = proveedor_de_pedido;
                }
                if (verificar_si_selecciono(resumen_sucursales, sucursal_pedido))//&& proveedor_de_pedido != "insumos_fabrica"
                {
                    for (int columna = pedidos_no_cargados.Columns["producto_1"].Ordinal; columna < pedidos_no_cargados.Columns.Count - 1; columna++)
                    {
                        if (IsNotDBNull(pedidos_no_cargados.Rows[fila][columna]))
                        {
                            if (pedidos_no_cargados.Rows[fila][columna].ToString() != string.Empty)
                            {
                                id_producto = int.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 2));
                                fila_producto = buscar_fila_producto(id_producto.ToString(), productos_proveedor);
                                nombre_producto = productos_proveedor.Rows[fila_producto]["producto"].ToString();

                                if (productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString() == "3-Panificados")
                                {
                                    fila_resumen = verificar_si_cargo_producto(id_producto.ToString(), nombre_producto);
                                    if (fila_resumen == -1)
                                    {
                                        resumen_de_pedidos.Rows.Add();
                                        fila_resumen = resumen_de_pedidos.Rows.Count - 1;
                                        resumen_de_pedidos.Rows[fila_resumen]["id_producto"] = id_producto.ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila_producto]["producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["tipo producto"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["unidad de medida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
                                        resumen_de_pedidos.Rows[fila_resumen]["stock_resumen"] = funciones.obtener_dato(productos_proveedor.Rows[fila_producto]["producto_1"].ToString(), 4);
                                        resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                    }
                                    else
                                    {
                                        double cantidad_cargada, cantidad_a_cargar;
                                        if (double.TryParse(resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido].ToString(), out cantidad_cargada))
                                        {
                                            cantidad_a_cargar = double.Parse(obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4));
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = Math.Round(cantidad_cargada + cantidad_a_cargar, 2).ToString();

                                        }
                                        else
                                        {
                                            resumen_de_pedidos.Rows[fila_resumen][sucursal_pedido] = obtener_dato_pedido(pedidos_no_cargados.Rows[fila][columna].ToString(), 4);

                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }

            double total, cantidad, stock;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                total = 0;
                for (int columna = resumen_de_pedidos.Columns["producto"].Ordinal + 1; columna <= resumen_de_pedidos.Columns.Count - 1; columna++)
                {
                    if (double.TryParse(resumen_de_pedidos.Rows[fila][columna].ToString().Replace(",", "."), out cantidad))
                    {
                        total = total + cantidad;

                    }
                }
                stock = double.Parse(resumen_de_pedidos.Rows[fila]["stock_resumen"].ToString());
                total = total - stock;
                if (total < 0)
                {
                    total = 0;
                }
                resumen_de_pedidos.Rows[fila]["total"] = total.ToString();
            }

        }
        #endregion

        #region metodos publicos
        public void crear_pdf_pedido_panificados(DataTable proveedor_BD, DataTable resumen_sucursales, string ruta_archivo, byte[] logo)
        {
            string nombre_proveedor = proveedor_BD.Rows[0]["nombre_en_BD"].ToString();
            //   consultar_productos_proveedor(nombre_proveedor);
            crear_tabla_resumen_de_pedidos(resumen_sucursales);
            llenar_tabla_de_pedidos_panificados(resumen_sucursales);
            resumen_de_pedidos.DefaultView.Sort = "tipo producto ASC";
            resumen_de_pedidos = resumen_de_pedidos.DefaultView.ToTable();
            PDF.GenerarPDF_pedido_de_panificados(ruta_archivo, logo, resumen_de_pedidos);
        }
        public void crear_pdf_resumen_panificados(DataTable proveedor_BD, DataTable resumen_sucursales, string ruta_archivo, byte[] logo)
        {
            string nombre_proveedor = proveedor_BD.Rows[0]["nombre_en_BD"].ToString();
            //consultar_productos_proveedor(nombre_proveedor);
            crear_tabla_resumen_de_pedidos(resumen_sucursales);
            llenar_tabla_resumen_de_pedidos_panificados(resumen_sucursales);
            resumen_de_pedidos.DefaultView.Sort = "tipo producto ASC";
            resumen_de_pedidos = resumen_de_pedidos.DefaultView.ToTable();
            PDF.GenerarPDF_resumen_de_pedidos(ruta_archivo, logo, resumen_de_pedidos);
        }
        public void crear_pdf_resumen_pedido(DataTable proveedor_BD, DataTable resumen_sucursales, string ruta_archivo, byte[] logo)
        {
            string nombre_proveedor = proveedor_BD.Rows[0]["nombre_en_BD"].ToString();
            //     consultar_productos_proveedor(nombre_proveedor);
            crear_tabla_resumen_de_pedidos(resumen_sucursales);
            llenar_tabla_resumen_de_pedidos(resumen_sucursales);
            resumen_de_pedidos.DefaultView.Sort = "tipo producto ASC";
            resumen_de_pedidos = resumen_de_pedidos.DefaultView.ToTable();
            PDF.GenerarPDF_resumen_de_pedidos(ruta_archivo, logo, resumen_de_pedidos);
        }
        public void crear_pdf_resumen_pedido_produccion_diaria(DataTable proveedor_BD, DataTable resumen_sucursales, string ruta_archivo, byte[] logo)
        {
            string nombre_proveedor = proveedor_BD.Rows[0]["nombre_en_BD"].ToString();
            //     consultar_productos_proveedor(nombre_proveedor);
            crear_tabla_resumen_de_pedidos(resumen_sucursales);
            llenar_tabla_resumen_de_pedidos_produccion_diaria(resumen_sucursales);
            resumen_de_pedidos.DefaultView.Sort = "tipo producto ASC";
            resumen_de_pedidos = resumen_de_pedidos.DefaultView.ToTable();
            PDF.GenerarPDF_resumen_de_pedidos(ruta_archivo, logo, resumen_de_pedidos);
        }
        public void actualizar_preciosStock(DataTable productos, string nombre_proveedor, string rol_usuario)
        {
            consultar_acuerdo_de_precio_activo(nombre_proveedor);
            string id_acuerdo_activo, proveedor, acuerdo, tipo_de_acuerdo;
            int nuevo_acuerdo;
            id_acuerdo_activo = acuerdo_de_precio_activo.Rows[0]["id"].ToString();
            proveedor = acuerdo_de_precio_activo.Rows[0]["proveedor"].ToString();
            acuerdo = acuerdo_de_precio_activo.Rows[0]["acuerdo"].ToString();
            nuevo_acuerdo = int.Parse(acuerdo) + 1;
            tipo_de_acuerdo = acuerdo_de_precio_activo.Rows[0]["tipo_de_acuerdo"].ToString();
            pedidos.crear_nuevo_acuerdo_de_precio(productos, proveedor, nuevo_acuerdo.ToString(), tipo_de_acuerdo);
            pedidos.desactivar_acuerdo_activo(id_acuerdo_activo);
            registrar_movimientos_en_historial(productos, rol_usuario);
            pedidos.actualizar_stock_nuevo(productos, nombre_proveedor);
            pedidos.actualizar_acuerdo_en_pedidos_no_cargados(nuevo_acuerdo.ToString(), proveedor);

        }
        private void registrar_movimientos_en_historial(DataTable productos, string rol_usuario)
        {
            string id = "";
            string stock_nuevo;
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                if (productos.Rows[fila]["stock_nuevo"].ToString() != "N/A")
                {
                    stock_nuevo = productos.Rows[fila]["stock_nuevo"].ToString();
                    id = productos.Rows[fila]["id"].ToString();
                    historial_stock.cargar_actualizacion_stock(rol_usuario, id, stock_nuevo, "N/A");
                }
            }
        }

        public void actualizar_estado_de_pedido(DataTable pedidos_no_cargados, DataTable resumen_sucursales)
        {
            for (int fila_resumen = 0; fila_resumen <= resumen_sucursales.Rows.Count - 1; fila_resumen++)
            {
                for (int fila_pedido = 0; fila_pedido <= pedidos_no_cargados.Rows.Count - 1; fila_pedido++)
                {
                    if (resumen_sucursales.Rows[fila_resumen]["sucursal"].ToString() == pedidos_no_cargados.Rows[fila_pedido]["sucursal"].ToString())
                    {
                        pedidos.actualizar_estado_pedido(pedidos_no_cargados.Rows[fila_pedido]["id"].ToString());
                    }
                }
            }
        }
        public void enviar_carga_de_pedido(DataTable pedido_sucursalSESSION, DataTable pedido, DataTable resumen_de_pedidos, string rol_usuario, string impuesto)
        {
            pedidos_sucursal = pedido_sucursalSESSION;
            string id_pedido;
            int fila_pedido;
            string sucursal;
            string num_pedido;
            string nombre_remito = "pedido";
            string valor_remito;
            string fecha_remito = funciones.get_fecha();
            string proveedor;
            string nota_pedido;
            consultar_insumos();
            insumos_copia = pedidos.get_insumos();
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                id_pedido = resumen_de_pedidos.Rows[fila]["id"].ToString();
                nota_pedido = pedido.Rows[fila]["nota"].ToString();
                fila_pedido = buscar_fila_pedido(id_pedido);
                sucursal = resumen_de_pedidos.Rows[fila]["sucursal"].ToString();
                num_pedido = resumen_de_pedidos.Rows[fila]["num_pedido"].ToString();
                valor_remito = resumen_de_pedidos.Rows[fila]["total_pedido"].ToString();
                proveedor = resumen_de_pedidos.Rows[fila]["proveedor"].ToString();

                if (!verificar_si_cargo_remito(sucursal, num_pedido, nombre_remito, valor_remito, proveedor))
                {

                    if (proveedor != "insumos_fabrica")
                    {
                        string nota = sucursal + " PEDIDO: " + num_pedido;
                        cargar_historial_stock(pedido, rol_usuario, nota, proveedor);
                        //pedidos.actualizar_stock(proveedor, pedido);
                    }
                    else if (proveedor == "insumos_fabrica")
                    {
                        restar_stock_insumo(pedido, proveedor);
                    }

                    pedidos.enviar_remito_fabrica(id_pedido, sucursal, num_pedido, nombre_remito, valor_remito, fecha_remito, proveedor, nota_pedido, impuesto);
                    pedidos.actualizar_pedido(id_pedido, pedido, proveedor, impuesto);
                    pedidos.cargar_venta_en_rendiciones(pedido, pedidos_sucursal, fila_pedido, proveedor);
                }
            }
            //  stock_insumos.guardar_registro_entrega_insumo_a_local(rol_usuario, insumos, insumos_copia);
            DateTime fecha = DateTime.Now;
            sucursal = resumen_de_pedidos.Rows[0]["sucursal"].ToString();
            sistema_Administracion.get_deuda_total_mes(sucursal, fecha.Month.ToString(), fecha.Year.ToString());
        }
        private bool verificar_si_cargo_remito(string sucursal, string num_pedido, string nombre_remito, string valor_remito, string proveedor)
        {
            bool retorno = false;
            DataTable dt = pedidos.consultar_remito_cuentas_por_pagar(sucursal, num_pedido, nombre_remito, valor_remito, proveedor);
            if (dt.Rows.Count > 0)
            {
                retorno = true;
            }
            return retorno;
        }
        public void enviar_carga_parcial_de_pedido(DataTable pedido_sucursalSESSION, DataTable pedido, DataTable resumen_de_pedidos, string rol_usuario)
        {
            pedidos_sucursal = pedido_sucursalSESSION;
            string id_pedido;
            string proveedor;
            for (int fila = 0; fila <= resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                id_pedido = resumen_de_pedidos.Rows[fila]["id"].ToString();
                proveedor = resumen_de_pedidos.Rows[fila]["proveedor"].ToString();

                pedidos.actualizar_pedido_carga_parcial(id_pedido, pedido, proveedor);

            }
        }

        private void cargar_historial_stock(DataTable pedido, string rol_usuario, string nota, string proveedor)
        {
            string id;
            double cantidad_despachada;
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                if (pedido.Rows[fila]["proveedor"].ToString() == proveedor)
                {

                    if (pedido.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                    {
                        id = pedido.Rows[fila]["id"].ToString();
                        cantidad_despachada = double.Parse(pedido.Rows[fila]["cantidad_entrega"].ToString());



                        //movimientos_stock_producto.cargar_resta_stock(rol_usuario, id, "despacho", cantidad_despachada.ToString(), nota);
                        if (proveedor == "proveedor_villaMaipu")
                        {
                            if (pedido.Rows[fila]["pincho"].ToString() == "si")
                            {
                                cantidad_despachada = double.Parse(pedido.Rows[fila]["cantidad_pincho"].ToString());
                                stock_producto_terminado.cargar_historial_stock(rol_usuario, id, "despacho", cantidad_despachada.ToString(), nota);
                            }
                            else
                            {
                                stock_producto_terminado.cargar_historial_stock(rol_usuario, id, "despacho", cantidad_despachada.ToString(), nota);
                            }
                        }
                        else
                        {
                            stock_producto_terminado.cargar_historial_stock(rol_usuario, id, "despacho", cantidad_despachada.ToString(), nota);
                        }
                    }
                }


            }
        }
        public void enviar_carga_de_pedido_fabrica(string id_pedido, DataTable pedidos_fabrica_a_proveedorSESSION, DataTable pedido, string total_remito)
        {
            pedidos_fabrica = pedidos_fabrica_a_proveedorSESSION;
            int fila_pedido = buscar_fila_pedido_fabrica(id_pedido);
            int fila_acuerdo = buscar_fila_acuerdo_pedido_fabrica(fila_pedido);
            string fabrica = pedidos_fabrica.Rows[fila_pedido]["fabrica"].ToString();
            string num_pedido = pedidos_fabrica.Rows[fila_pedido]["num_pedido"].ToString();
            string nombre_remito = "pedido";
            string valor_remito = total_remito.ToString();

            string fecha_remito = formatDateBD(pedidos_fabrica.Rows[fila_pedido]["fecha"].ToString());
            string proveedor = pedidos_fabrica.Rows[fila_pedido]["proveedor"].ToString();

            pedidos.actualizar_precio_proveedor(fila_acuerdo, acuerdo_de_precios, pedido, pedidos_fabrica);
            pedidos.actualizar_stock_fabrica(proveedor, pedido);
            pedidos.enviar_remito_proveedor_a_fabrica(id_pedido, fabrica, num_pedido, nombre_remito, total_remito, fecha_remito, proveedor);
            pedidos.actualizar_pedido_fabrica(id_pedido, pedido);
        }

        private void restar_stock_insumo(DataTable pedido, string proveedor)
        {
            double stock_teorico, stock_real, cantidad_entregada, nuevo_stock_teorico;
            string paquete, unidad, tipo_unidad, dato;
            string presentacion_entrega_seleccionada, presentacion_extraccion_seleccionada;
            double unidad_entrega, unidad_extraccion, fraccion_unitaria, fraccion_a_descontar;
            //cantidad_entrega
            int fila_insumo, columna_insumo;
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                if (pedido.Rows[fila]["proveedor"].ToString() == proveedor)
                {
                    if ("0" != pedido.Rows[fila]["cantidad_entrega"].ToString())
                    {
                        fila_insumo = funciones.buscar_fila_por_id(pedido.Rows[fila]["id"].ToString(), insumos);
                        columna_insumo = buscar_columna_presentacion(pedido, fila, fila_insumo);
                        if (columna_insumo != -1)
                        {
                            paquete = funciones.obtener_dato(insumos.Rows[fila_insumo][columna_insumo].ToString(), 1);
                            unidad = funciones.obtener_dato(insumos.Rows[fila_insumo][columna_insumo].ToString(), 2);
                            tipo_unidad = funciones.obtener_dato(insumos.Rows[fila_insumo][columna_insumo].ToString(), 3);
                            //OBTENER STOCK TEORICO
                            stock_teorico = double.Parse(funciones.obtener_dato(insumos.Rows[fila_insumo][columna_insumo].ToString(), 4));
                            //OBTENER STOCK REAL
                            stock_real = double.Parse(funciones.obtener_dato(insumos.Rows[fila_insumo][columna_insumo].ToString(), 5));
                            //OBTENER CANTIDAD CARGADA
                            cantidad_entregada = double.Parse(pedido.Rows[fila]["cantidad_entrega"].ToString());
                            presentacion_entrega_seleccionada = pedido.Rows[fila]["equivalencia"].ToString();
                            presentacion_extraccion_seleccionada = pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();

                            unidad_entrega = double.Parse(funciones.obtener_dato(presentacion_entrega_seleccionada, 2));
                            unidad_extraccion = double.Parse(funciones.obtener_dato(presentacion_extraccion_seleccionada, 2));

                            fraccion_unitaria = unidad_entrega / unidad_extraccion;
                            fraccion_a_descontar = fraccion_unitaria * cantidad_entregada;
                            //RESTAR A STOCK TEORICO Y REAL LA CANTIDAD CARGADA
                            nuevo_stock_teorico = stock_teorico - fraccion_a_descontar;
                            //ARMAR DATO Y ACTUALIZAR COLUMNA DE STOCK DE INSUMO
                            dato = paquete + "-" + unidad + "-" + tipo_unidad + "-" + nuevo_stock_teorico + "-" + nuevo_stock_teorico;
                            insumos.Rows[fila_insumo][columna_insumo] = dato;
                            insumos.Rows[fila_insumo]["nuevo_stock"] = "si";
                        }
                    }
                }
            }
        }
        private int buscar_columna_presentacion(DataTable pedido, int fila, int fila_insumo)
        {
            string paquete, unidad, tipo_unidad, dato;
            int retorno = -1;
            int columna = insumos.Columns["producto_1"].Ordinal;
            while (columna <= insumos.Columns.Count - 1)
            {

                paquete = funciones.obtener_dato(insumos.Rows[fila_insumo][columna].ToString(), 1);
                unidad = funciones.obtener_dato(insumos.Rows[fila_insumo][columna].ToString(), 2);
                tipo_unidad = funciones.obtener_dato(insumos.Rows[fila_insumo][columna].ToString(), 3);

                dato = paquete + "-" + unidad + "-" + tipo_unidad;
                if (pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString() == dato)
                {
                    retorno = columna;
                    break;
                }
                columna++;
            }
            return retorno;
        }
        #endregion

        #region metodos privados
        private string obtener_nombre_proveedor(string proveedor_seleccionado)
        {
            string retorno = proveedor_seleccionado;
            consultar_proveedores_fabrica();
            int fila = 0;
            while (fila <= proveedores_fabrica.Rows.Count - 1)
            {
                if (proveedor_seleccionado == proveedores_fabrica.Rows[fila]["nombre_en_BD"].ToString())
                {
                    retorno = proveedores_fabrica.Rows[fila]["nombre_proveedor"].ToString();
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public void crear_pdf(string ruta, byte[] logo, string id_pedido_fabrica, DataTable pedidos_fabrica_a_proveedor, DataTable pedido, DataTable proveedor_BD) //
        {

            pedidos_fabrica = pedidos_fabrica_a_proveedor;
            int fila_pedido = buscar_fila_pedido_fabrica(id_pedido_fabrica);
            string proveedor_seleccionado = pedidos_fabrica_a_proveedor.Rows[fila_pedido]["proveedor"].ToString();
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor_seleccionado);
            //   PDF.GenerarPDF_orden_de_compra_administrativo(ruta, logo, pedido, proveedor_seleccionado, fila_pedido, pedidos_fabrica_a_proveedor, proveedor_BD);
        }
        public string enviar_pedido(DataTable resumen_pedido, DataTable resumen_bonificado, DataTable lista_proveedores, string proveedor, DataTable proveedorBD)
        {
            pedidos.enviar_pedido(resumen_pedido, resumen_bonificado, proveedorBD);
            //  mail = new cls_mail(usuariosBD, sucursalBD, lista_proveedores, proveedor);
            //  mail.enviar_pedido(resumen_pedido, resumen_bonificado);

            whatsapp = new cls_whatsapp(); //usuariosBD, proveedorBD, lista_proveedores, proveedor
            return whatsapp.enviar_pedido_proveedor(resumen_pedido, resumen_bonificado);
        }
        private void crear_tabla_pedido()
        {
            pedido = new DataTable();
            pedido.Columns.Clear();
            pedido.Columns.Add("id", typeof(string));
            pedido.Columns.Add("producto", typeof(string));
            pedido.Columns.Add("cantidad_pedida", typeof(string));
            pedido.Columns.Add("unidad_pedida", typeof(string));
            pedido.Columns.Add("cantidad_entrega", typeof(string));
            pedido.Columns.Add("unidad_entrega", typeof(List<string>));
            pedido.Columns.Add("presentacion_entrega", typeof(List<string>));
            pedido.Columns.Add("presentacion_extraccion", typeof(List<string>));
            pedido.Columns.Add("equivalencia", typeof(string));
            pedido.Columns.Add("precio", typeof(string));
            pedido.Columns.Add("sub_total", typeof(string));
            pedido.Columns.Add("pedido_dato", typeof(string));
            pedido.Columns.Add("pedido_dato_parcial", typeof(string));
            pedido.Columns.Add("stock", typeof(string));
            pedido.Columns.Add("nuevo_stock", typeof(string));
            pedido.Columns.Add("proveedor", typeof(string));
            pedido.Columns.Add("presentacion_selecccionada", typeof(string));
            pedido.Columns.Add("id_pedido", typeof(string));
            pedido.Columns.Add("estado", typeof(string));
            pedido.Columns.Add("presentacion_entrega_seleccionada", typeof(string));
            pedido.Columns.Add("presentacion_extraccion_seleccionada", typeof(string));
            pedido.Columns.Add("pincho", typeof(string));
            pedido.Columns.Add("cantidad_pincho", typeof(string));

            pedido.Columns.Add("nota", typeof(string));

            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,pedido.Columns.Add("facturacion por", typeof(string));
            //pedido.Columns.Add("kilos", typeof(string));
            //pedido.Columns.Add("factura_por_kilo", typeof(string));


            int fila_pedido = 0;
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {
                pedido.Rows.Add();
                pedido.Rows[fila_pedido]["id"] = resumen_pedido.Rows[fila]["id"].ToString();
                pedido.Rows[fila_pedido]["nota"] = resumen_pedido.Rows[fila]["nota"].ToString();
                pedido.Rows[fila_pedido]["producto"] = resumen_pedido.Rows[fila]["producto"].ToString();
                if (resumen_pedido.Rows[fila]["proveedor"].ToString() == "insumos_fabrica")
                {
                    pedido.Rows[fila_pedido]["cantidad_pedida"] = resumen_pedido.Rows[fila]["pedido"].ToString();
                }
                else
                {
                    pedido.Rows[fila_pedido]["cantidad_pedida"] = resumen_pedido.Rows[fila]["pedido"].ToString() + " x" + resumen_pedido.Rows[fila]["unid.pedida"].ToString();
                }
                pedido.Rows[fila_pedido]["unidad_pedida"] = resumen_pedido.Rows[fila]["unid.pedida"].ToString();
                pedido.Rows[fila_pedido]["cantidad_entrega"] = resumen_pedido.Rows[fila]["entregado"].ToString();
                pedido.Rows[fila_pedido]["unidad_entrega"] = (List<string>)resumen_pedido.Rows[fila]["unid.entregada"];
                pedido.Rows[fila_pedido]["presentacion_entrega"] = (List<string>)resumen_pedido.Rows[fila]["presentacion_entrega"];
                pedido.Rows[fila_pedido]["presentacion_extraccion"] = (List<string>)resumen_pedido.Rows[fila]["presentacion_extraccion"];
                pedido.Rows[fila_pedido]["precio"] = resumen_pedido.Rows[fila]["precio"].ToString();
                pedido.Rows[fila_pedido]["sub_total"] = "0";
                pedido.Rows[fila_pedido]["pedido_dato"] = resumen_pedido.Rows[fila]["pedido_dato"].ToString();
                pedido.Rows[fila_pedido]["pedido_dato_parcial"] = resumen_pedido.Rows[fila]["pedido_dato_parcial"].ToString();
                pedido.Rows[fila_pedido]["stock"] = resumen_pedido.Rows[fila]["stock"].ToString();
                pedido.Rows[fila_pedido]["nuevo_stock"] = "N/A";
                pedido.Rows[fila_pedido]["proveedor"] = resumen_pedido.Rows[fila]["proveedor"].ToString();
                pedido.Rows[fila_pedido]["id_pedido"] = resumen_pedido.Rows[fila]["id_pedido"].ToString();
                pedido.Rows[fila_pedido]["estado"] = resumen_pedido.Rows[fila]["estado"].ToString();
                pedido.Rows[fila_pedido]["presentacion_entrega_seleccionada"] = resumen_pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString();
                pedido.Rows[fila_pedido]["presentacion_extraccion_seleccionada"] = resumen_pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();
                pedido.Rows[fila_pedido]["equivalencia"] = resumen_pedido.Rows[fila]["equivalencia"].ToString();
                pedido.Rows[fila_pedido]["pincho"] = resumen_pedido.Rows[fila]["pincho"].ToString();
                pedido.Rows[fila_pedido]["cantidad_pincho"] = "N/A";


                //pedido.Rows[fila_pedido]["facturacion por"] = resumen_pedido.Rows[fila]["facturacion por"].ToString();
                //pedido.Rows[fila_pedido]["kilos"] = resumen_pedido.Rows[fila]["kilos"].ToString();
                //pedido.Rows[fila_pedido]["factura_por_kilo"] = resumen_pedido.Rows[fila]["factura_por_kilo"].ToString();


                fila_pedido = fila_pedido + 1;
            }
        }

        private void crear_tabla_resumen()
        {
            resumen_pedido = new DataTable();
            resumen_pedido.Columns.Add("id", typeof(string));
            resumen_pedido.Columns.Add("producto", typeof(string));
            resumen_pedido.Columns.Add("pedido", typeof(string));
            resumen_pedido.Columns.Add("unid.pedida", typeof(string));
            resumen_pedido.Columns.Add("entregado", typeof(string));
            resumen_pedido.Columns.Add("unid.entregada", typeof(List<string>));
            resumen_pedido.Columns.Add("presentacion_entrega", typeof(List<string>));
            resumen_pedido.Columns.Add("presentacion_extraccion", typeof(List<string>));
            resumen_pedido.Columns.Add("tipo", typeof(string));
            resumen_pedido.Columns.Add("precio", typeof(string));
            resumen_pedido.Columns.Add("sub.total", typeof(string));
            resumen_pedido.Columns.Add("pedido_dato", typeof(string));
            resumen_pedido.Columns.Add("pedido_dato_parcial", typeof(string));
            resumen_pedido.Columns.Add("stock", typeof(string));
            resumen_pedido.Columns.Add("proveedor", typeof(string));

            resumen_pedido.Columns.Add("id_pedido", typeof(string));
            resumen_pedido.Columns.Add("estado", typeof(string));
            resumen_pedido.Columns.Add("equivalencia", typeof(string));

            resumen_pedido.Columns.Add("presentacion_entrega_seleccionada", typeof(string));
            resumen_pedido.Columns.Add("presentacion_extraccion_seleccionada", typeof(string));

            resumen_pedido.Columns.Add("nota", typeof(string));
            resumen_pedido.Columns.Add("pincho", typeof(string));
            // resumen_pedido.Columns.Add("factura,                     cion por", typeof(string));
            // resumen_pedido.Columns.Add("kilos", typeof(string));
            // resumen_pedido.Columns.Add("factura_por_kilo", typeof(string));

        }
        private string obtener_dato_pedido(string dato, int posicion_dato)
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

        private void cargar_producto(string precio, string id, string producto, string cantidad_pedida, string cantidad_entregada, int fila_acuerdo, string pedido_dato, string proveedor, string id_pedido, string pedido_dato_parcial, string estado, string presentacion_entrega_seleccionada, string presentacion_extraccion_seleccionada, string nota)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;
            int fila_producto = buscar_fila_producto(id, productos_proveedor);
            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["nota"] = nota;
            resumen_pedido.Rows[fila]["producto"] = producto;
            resumen_pedido.Rows[fila]["pedido"] = cantidad_pedida;
            if (cantidad_entregada == "N/A")
            {
                resumen_pedido.Rows[fila]["entregado"] = "0";
            }
            else
            {
                resumen_pedido.Rows[fila]["entregado"] = cantidad_entregada;
            }
            resumen_pedido.Rows[fila]["id_pedido"] = id_pedido;

            //resumen_pedido.Rows[fila]["kilos"] = productos_proveedor.Rows[int.Parse(id) - 1]["kilos"].ToString();
            //resumen_pedido.Rows[fila]["facturacion por"] = productos_proveedor.Rows[int.Parse(id) - 1]["facturacion por"].ToString();
            //resumen_pedido.Rows[fila]["factura_por_kilo"] = productos_proveedor.Rows[int.Parse(id) - 1]["factura_por_kilo"].ToString();



            resumen_pedido.Rows[fila]["tipo"] = productos_proveedor.Rows[fila_producto]["tipo_producto"].ToString();

            resumen_pedido.Rows[fila]["stock"] = productos_proveedor.Rows[fila_producto]["stock"].ToString();

            resumen_pedido.Rows[fila]["proveedor"] = proveedor;

            resumen_pedido.Rows[fila]["precio"] = acuerdo_de_precios.Rows[fila_acuerdo]["producto_" + id].ToString();

            resumen_pedido.Rows[fila]["pedido_dato"] = pedido_dato; //factura_por_kilo 

            resumen_pedido.Rows[fila]["pedido_dato_parcial"] = pedido_dato_parcial; // pedido_dato_parcial 

            resumen_pedido.Rows[fila]["estado"] = estado; // pedido_dato_parcial 

            resumen_pedido.Rows[fila]["unid.entregada"] = obtener_lista_de_presentacion_venta(proveedor, fila_producto);
            resumen_pedido.Rows[fila]["presentacion_entrega"] = obtener_lista_de_presentacion_venta(proveedor, fila_producto);
            resumen_pedido.Rows[fila]["presentacion_extraccion"] = obtener_lista_de_presentaciones(proveedor, fila_producto);


            resumen_pedido.Rows[fila]["unid.pedida"] = productos_proveedor.Rows[fila_producto]["unidad_de_medida_local"].ToString();
            if (proveedor == "insumos_fabrica")
            {

                resumen_pedido.Rows[fila]["equivalencia"] = productos_proveedor.Rows[fila_producto]["equivalencia"].ToString();
            }

            resumen_pedido.Rows[fila]["presentacion_entrega_seleccionada"] = presentacion_entrega_seleccionada; // presentacion_entrega_seleccionada 
            resumen_pedido.Rows[fila]["presentacion_extraccion_seleccionada"] = presentacion_extraccion_seleccionada; // presentacion_extraccion_seleccionada 

            if (proveedor == "proveedor_villaMaipu")
            {
                resumen_pedido.Rows[fila]["pincho"] = productos_proveedor.Rows[fila_producto]["pincho"].ToString();
            }
            else
            {
                resumen_pedido.Rows[fila]["pincho"] = "no";
            }
        }
        private List<string> obtener_lista_de_presentaciones(string proveedor, int fila_producto)
        {
            List<string> presentacion = new List<string>();
            string paquete, unidad, tipo_unidad, dato;
            if (proveedor == "insumos_fabrica")
            {
                for (int columna = productos_proveedor.Columns["producto_1"].Ordinal; columna <= productos_proveedor.Columns.Count - 1; columna++)
                {
                    if (productos_proveedor.Rows[fila_producto][columna].ToString() == "N/A")
                    {
                        break;
                    }
                    else
                    {
                        paquete = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 1);
                        unidad = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 2);
                        tipo_unidad = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 3);
                        if (paquete == "Unidad" &&
                            unidad == "1" &&
                            tipo_unidad == "unid.")
                        {
                            dato = "Unidad";
                        }
                        else
                        {
                            dato = paquete + "-" + unidad + "-" + tipo_unidad;
                        }
                        dato = paquete + "-" + unidad + "-" + tipo_unidad;
                        presentacion.Add(dato);
                    }
                }
            }
            else
            {
                presentacion.Add(productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"].ToString());
            }
            return presentacion;
        }
        private List<string> obtener_lista_de_presentacion_venta(string proveedor, int fila_producto)
        {
            List<string> presentacion = new List<string>();
            string paquete, unidad, tipo_unidad, dato;
            if (proveedor == "insumos_fabrica")
            {
                int columna = productos_proveedor.Columns["unidad_de_medida_local"].Ordinal;
                paquete = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 1);
                unidad = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 2);
                tipo_unidad = funciones.obtener_dato(productos_proveedor.Rows[fila_producto][columna].ToString(), 3);
                if (paquete == "Unidad" &&
                    unidad == "1" &&
                    tipo_unidad == "unid.")
                {
                    dato = "Unidad";
                }
                else
                {
                    dato = paquete + "-" + unidad + "-" + tipo_unidad;
                }
                dato = paquete + "-" + unidad + "-" + tipo_unidad;
                presentacion.Add(dato);
            }
            else
            {
                presentacion.Add(productos_proveedor.Rows[fila_producto]["unidad_de_medida_fabrica"].ToString());
            }
            return presentacion;
        }
        private void cargar_bonificado(string precio, string id, string producto, string cantidad_pedida, string cantidad_entregada, int fila_acuerdo, string pedido_dato)
        {
            resumen_pedido.Rows.Add();
            int fila = resumen_pedido.Rows.Count - 1;

            resumen_pedido.Rows[fila]["id"] = id;
            resumen_pedido.Rows[fila]["producto"] = producto;
            resumen_pedido.Rows[fila]["pedido"] = cantidad_pedida;
            resumen_pedido.Rows[fila]["unid.pedida"] = productos_proveedor.Rows[int.Parse(id) - 1]["unidad_de_medida_local"].ToString();
            if (cantidad_entregada == "N/A")
            {
                resumen_pedido.Rows[fila]["entregado"] = "0";
            }
            else
            {
                resumen_pedido.Rows[fila]["entregado"] = cantidad_entregada;
            }
            resumen_pedido.Rows[fila]["unid.entregada"] = productos_proveedor.Rows[int.Parse(id) - 1]["unidad_de_medida_fabrica"].ToString();
            resumen_pedido.Rows[fila]["stock"] = productos_proveedor.Rows[int.Parse(id) - 1]["stock"].ToString();

            if (precio == "BONIFICADO")
            {
                resumen_pedido.Rows[fila]["precio"] = "0";
                resumen_pedido.Rows[fila]["sub.total"] = "0";
                resumen_pedido.Rows[fila]["tipo"] = "BONIFICADO";
            }
            else
            {

                resumen_pedido.Rows[fila]["precio"] = acuerdo_de_precios.Rows[fila_acuerdo]["precio_bonificado"].ToString();

                resumen_pedido.Rows[fila]["tipo"] = "BONIFICADO ESPECIAL";
            }
            resumen_pedido.Rows[fila]["pedido_dato"] = pedido_dato;

        }

        private void abrir_pedido(DataTable resumen_de_pedidos)
        {
            string precio, id, producto, cantidad_pedida, cantidad_entregada;
            int fila_pedido, fila_acuerdo;
            string id_pedido;
            string proveedor, sucursalBD;
            string pedido_dato, pedido_dato_parcial, estado;
            string tipo_paquete_entrega, unidad_entrega, tipo_unidad_entrega;
            string tipo_paquete_extraccion, unidad_extraccion, tipo_unidad_extraccion;
            string presentacion_entrega_seleccionada, presentacion_extraccion_seleccionada;
            string nota;
            int i = 1;
            crear_tabla_resumen();
            for (int fila_resumen = 0; fila_resumen <= resumen_de_pedidos.Rows.Count - 1; fila_resumen++)
            {
                id_pedido = resumen_de_pedidos.Rows[fila_resumen]["id"].ToString();
                fila_pedido = buscar_fila_pedido(id_pedido);
                fila_acuerdo = buscar_fila_acuerdo(fila_pedido);

                proveedor = pedidos_sucursal.Rows[fila_pedido]["proveedor"].ToString();
                sucursalBD = pedidos_sucursal.Rows[fila_pedido]["sucursal"].ToString();
                estado = pedidos_sucursal.Rows[fila_pedido]["estado"].ToString();
                productos_proveedor = pedidos.get_productos_proveedor(proveedor, sucursalBD);
                i = 1;
                for (int columna = pedidos_sucursal.Columns["producto_1"].Ordinal; columna <= pedidos_sucursal.Columns.Count - 1; columna++)
                {
                    nota = pedidos_sucursal.Rows[fila_pedido]["nota"].ToString();

                    if (IsNotDBNull(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()]))
                    {
                        if (pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                        {
                            pedido_dato = pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString();
                            //extraer precio
                            precio = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                            //extraer id
                            id = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);

                            //extraer producto
                            producto = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                            //extraer cantidad pedida
                            cantidad_pedida = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                            //extraer cantidad entregada
                            cantidad_entregada = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 5).Replace(",", ".");
                            pedido_dato_parcial = precio + "-" + id + "-" + producto + "-" + cantidad_pedida;

                            if (cantidad_entregada != obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 6))
                            {

                                tipo_paquete_entrega = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 6);
                                unidad_entrega = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 7);
                                tipo_unidad_entrega = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 8);

                                presentacion_entrega_seleccionada = tipo_paquete_entrega + "-" + unidad_entrega + "-" + tipo_unidad_entrega;

                                tipo_paquete_extraccion = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 9);
                                unidad_extraccion = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 10);
                                tipo_unidad_extraccion = obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 11);

                                presentacion_extraccion_seleccionada = tipo_paquete_extraccion + "-" + unidad_extraccion + "-" + tipo_unidad_extraccion;


                            }
                            else
                            {
                                presentacion_entrega_seleccionada = "N/A";
                                presentacion_extraccion_seleccionada = "N/A";
                            }
                            //cargar normal
                            cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, fila_acuerdo, pedido_dato, proveedor, id_pedido, pedido_dato_parcial, estado, presentacion_entrega_seleccionada, presentacion_extraccion_seleccionada, nota);
                            //}
                        }
                    }
                    i++;
                }
            }
        }

        private void abrir_pedido_proveedor_a_fabrica(int fila_pedido, int fila_acuerdo)
        {
            string precio, id, producto, cantidad_pedida, cantidad_entregada;
            crear_tabla_resumen();
            string proveedor = pedidos_fabrica.Rows[fila_pedido]["proveedor"].ToString();
            string fabrica = pedidos_fabrica.Rows[fila_pedido]["fabrica"].ToString();
            string estado = pedidos_fabrica.Rows[fila_pedido]["estado"].ToString();
            productos_proveedor = pedidos.get_productos_proveedor_fabrica(proveedor);
            string pedido_dato, pedido_dato_parcial;
            int i = 1;
            for (int columna = pedidos_fabrica.Columns["producto_1"].Ordinal; columna <= pedidos_fabrica.Columns.Count - 1; columna++)
            {
                if (IsNotDBNull(pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()]))
                {
                    if (pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString() != "")
                    {
                        pedido_dato = pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString();
                        //extraer precio
                        precio = obtener_dato_pedido(pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 1);
                        //extraer id
                        id = obtener_dato_pedido(pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 2);
                        //extraer producto
                        producto = obtener_dato_pedido(pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 3);
                        //extraer cantidad pedida
                        cantidad_pedida = obtener_dato_pedido(pedidos_fabrica.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 4);
                        //extraer cantidad entregada
                        cantidad_entregada = "0";//obtener_dato_pedido(pedidos_sucursal.Rows[fila_pedido]["producto_" + i.ToString()].ToString(), 5).Replace(",", ".");
                        pedido_dato_parcial = precio + "-" + id + "-" + producto + "-" + cantidad_pedida;
                        if (precio == "BONIFICADO" || precio == "BONIFICADO ESPECIAL")
                        {
                            //cargar bonificado
                            cargar_bonificado(precio, id, producto, cantidad_pedida, cantidad_entregada, fila_acuerdo, pedido_dato);
                        }
                        else
                        {
                            //cargar normal
                            cargar_producto(precio, id, producto, cantidad_pedida, cantidad_entregada, fila_acuerdo, pedido_dato, pedidos_fabrica.Rows[fila_pedido]["proveedor"].ToString(), "N/A", pedido_dato_parcial, estado, "N/A", "N/A", "N/A");
                        }
                    }
                }
                i++;
            }
        }
        private int buscar_fila_pedido(string id_pedido)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos_sucursal.Rows.Count - 1)
            {
                if (id_pedido == pedidos_sucursal.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_pedido_fabrica(string id_pedido)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= pedidos_fabrica.Rows.Count - 1)
            {
                if (id_pedido == pedidos_fabrica.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_acuerdo(int fila_pedido)
        {
            consultar_acuerdo_de_precios();

            string tipo_de_acuerdo, acuerdo, proveedor;

            tipo_de_acuerdo = pedidos_sucursal.Rows[fila_pedido]["tipo_de_acuerdo_fabrica"].ToString();
            acuerdo = pedidos_sucursal.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
            proveedor = pedidos_sucursal.Rows[fila_pedido]["proveedor"].ToString();

            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios.Rows.Count - 1)
            {
                if (tipo_de_acuerdo == acuerdo_de_precios.Rows[fila]["tipo_de_acuerdo"].ToString() && acuerdo == acuerdo_de_precios.Rows[fila]["acuerdo"].ToString() && proveedor == acuerdo_de_precios.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int buscar_fila_acuerdo_pedido_fabrica(int fila_pedido)
        {
            consultar_acuerdo_de_precios();
            string tipo_de_acuerdo, acuerdo, proveedor;

            tipo_de_acuerdo = pedidos_fabrica.Rows[fila_pedido]["tipo_de_acuerdo"].ToString();
            acuerdo = pedidos_fabrica.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
            proveedor = pedidos_fabrica.Rows[fila_pedido]["proveedor"].ToString();

            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios.Rows.Count - 1)
            {
                if (tipo_de_acuerdo == acuerdo_de_precios.Rows[fila]["tipo_de_acuerdo"].ToString() && acuerdo == acuerdo_de_precios.Rows[fila]["acuerdo"].ToString() && proveedor == acuerdo_de_precios.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion

        #region metodos privados consulta
        private void consultar_insumos()
        {
            insumos = pedidos.get_insumos();
            insumos.Columns.Add("nuevo_stock", typeof(string));
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                insumos.Rows[fila]["nuevo_stock"] = "N/A";
            }
        }
        private void consultar_acuerdo_de_precio_activo(string nombre_proveedor)
        {
            acuerdo_de_precio_activo = pedidos.get_acuerdo_de_precio_activo(nombre_proveedor);

        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = pedidos.get_acuerdo_de_precios();
        }
        private void consultar_pedidos_sucursales(string nombre_sucursal, string nombre_proveedor)
        {
            pedidos_sucursal = pedidos.get_pedidos_sucursales(nombre_sucursal, nombre_proveedor);
        }
        private void consultar_pedidos_sucursales_producto_terminado_e_insumo(string nombre_sucursal, string nombre_proveedor)
        {
            pedidos_sucursal = pedidos.get_pedidos_sucursales_producto_terminado_e_insumo(nombre_sucursal, nombre_proveedor);
        }
        private void consultar_pedidos_no_cargados(string nombre_proveedor)
        {
            pedidos_no_cargados = pedidos.get_pedidos_no_cargados(nombre_proveedor);
        }
        private void consultar_pedidos_no_cargados_producto_terminado_e_insumo(string nombre_proveedor)
        {
            pedidos_no_cargados = pedidos.get_pedidos_no_cargados_producto_terminado_e_insumo(nombre_proveedor);
        }
        private void consultar_pedidos_fabrica(string nombre_fabrica)
        {
            pedidos_fabrica = pedidos.get_pedidos_fabrica(nombre_fabrica);
        }
        private void consultar_sucursales()
        {
            sucursales = pedidos.get_sucursales();
        }
        private void consultar_proveedores_fabrica()
        {
            proveedores_fabrica = pedidos.get_proveedores_fabrica();
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = pedidos.get_productos_proveedor_fabrica(nombre_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            sucursales.DefaultView.Sort = "sucursal ASC";
            sucursales = sucursales.DefaultView.ToTable();
            return sucursales;
        }
        public DataTable get_proveedores_fabrica()
        {
            consultar_proveedores_fabrica();
            return proveedores_fabrica;
        }
        public DataTable get_pedidos_sucursal(string nombre_sucursal, string nombre_proveedor, string id_proveedor)
        {
            if (id_proveedor == "1")
            {
                consultar_pedidos_sucursales_producto_terminado_e_insumo(nombre_sucursal, nombre_proveedor);
            }
            else
            {
                consultar_pedidos_sucursales(nombre_sucursal, nombre_proveedor);
            }
            return pedidos_sucursal;
        }
        public DataTable get_pedidos_no_cargados(string nombre_proveedor, string id_proveedor)
        {
            if (id_proveedor == "1")
            {
                consultar_pedidos_no_cargados_producto_terminado_e_insumo(nombre_proveedor);
            }
            else
            {
                consultar_pedidos_no_cargados(nombre_proveedor);
            }
            return pedidos_no_cargados;
        }
        public DataTable get_pedidos_fabrica(DataTable proveedor_BD)
        {

            consultar_pedidos_fabrica(proveedor_BD.Rows[0]["nombre_proveedor"].ToString());
            return pedidos_fabrica;
        }
        public DataTable get_pedido(DataTable resumen_de_pedidos, DataTable pedido_sucursalSESSION)
        {
            pedidos_sucursal = pedido_sucursalSESSION;

            abrir_pedido(resumen_de_pedidos);
            crear_tabla_pedido();
            return pedido;
        }
        public string get_titulo_pedido(DataTable resumen_de_pedidos)
        {
            string sucursal = resumen_de_pedidos.Rows[0]["sucursal"].ToString();
            string num_pedido = "";
            for (int fila = 0; fila < resumen_de_pedidos.Rows.Count - 1; fila++)
            {
                num_pedido = num_pedido + resumen_de_pedidos.Rows[fila]["num_pedido"].ToString() + ", ";
            }
            int ultima_fila = resumen_de_pedidos.Rows.Count - 1;
            if (ultima_fila == -1)
            {
                num_pedido = resumen_de_pedidos.Rows[0]["num_pedido"].ToString();
            }
            else
            {
                num_pedido = num_pedido + resumen_de_pedidos.Rows[ultima_fila]["num_pedido"].ToString();
            }
            return "Sucursal: " + sucursal + " | Pedido N°: " + num_pedido;
        }
        public string get_proveedor_pedido(string id_pedido, DataTable pedido_sucursalSESSION)
        {
            pedidos_sucursal = pedido_sucursalSESSION;
            int fila_pedido = buscar_fila_pedido(id_pedido);
            return pedidos_sucursal.Rows[fila_pedido]["proveedor"].ToString();
        }
        public DataTable get_pedido_fabrica(string id_pedido, DataTable pedido_fabricaSESSION)
        {
            pedidos_fabrica = pedido_fabricaSESSION;
            int fila_pedido = buscar_fila_pedido_fabrica(id_pedido);
            int fila_acuerdo = buscar_fila_acuerdo_pedido_fabrica(fila_pedido);
            abrir_pedido_proveedor_a_fabrica(fila_pedido, fila_acuerdo);
            crear_tabla_pedido();
            return pedido;
        }
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            return productos_proveedor;
        }


        private string formatDateBD(string valor)
        {
            DateTime fecha = DateTime.Parse(valor);
            return fecha.Year.ToString() + "-" + fecha.Month.ToString() + "-" + fecha.Day.ToString();
        }
        #endregion

        #region cancelar pedido
        public void cancelar_pedido(string id_pedido)
        {
            pedidos.cancelar_pedido(id_pedido);
        }
        #endregion
    }
}
