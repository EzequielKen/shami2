using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01___modulos;
using modulos;
using paginaWeb;
namespace _02___sistemas
{
    public class cls_sistema_pedidos
    {
        public cls_sistema_pedidos(DataTable usuarios_BD, DataTable sucursal_BD)
        {
            usuariosBD = usuarios_BD;
            sucursalBD = sucursal_BD;
            pedidos = new cls_pedidos(usuarios_BD, sucursal_BD);

        }
        #region atributos
        cls_pedidos pedidos;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        cls_whatsapp whatsapp;
        DataTable usuariosBD;
        DataTable sucursalBD;
        DataTable lista_proveedores;
        DataTable productos_proveedor;
        DataTable insumos_proveedor;
        List<int> lista_bonificados;
        List<int> lista_bonificados_especiales;
        string precio_especial;
        DataTable resumen;
        #endregion

        #region PDF
        public void crear_pdf_lista_de_precios(string ruta, byte[] logo, DataTable productosPDF)
        {
            PDF.GenerarPDF_lista_de_precios(ruta, logo, productosPDF);
        }
        #endregion


        #region metodos
        public string enviar_pedido(DataTable resumen_pedido,string nota)
        {
            pedidos.enviar_pedido(resumen_pedido, nota);
            //mail = new cls_mail(usuariosBD,sucursalBD, lista_proveedores, proveedor);
            // mail.enviar_pedido(resumen_pedido, resumen_bonificado);

            whatsapp = new cls_whatsapp();
            return whatsapp.enviar_pedido(resumen_pedido, usuariosBD, sucursalBD);
        }
        public string enviar_pedido_automatico(DataTable resumen_pedido, string nota)
        {
           return pedidos.enviar_pedido_automatico(resumen_pedido, nota);

        }
        public List<int> obtener_lista_bonificados()
        {
            lista_bonificados = pedidos.get_productos_bonificados_lista();
            return lista_bonificados;
        }
        public List<int> obtener_lista_bonificados_especiales()
        {
            lista_bonificados_especiales = pedidos.get_productos_bonificados_especiales_lista();
            return lista_bonificados_especiales;
        }
        public string obtener_precio_bonificado_especial()
        {
            precio_especial = pedidos.get_precios_bonificados_especial();
            return precio_especial;

        }
        public DataTable cargar_lista_proveedores()
        {
            obtener_lista_proveedores();
            return lista_proveedores;
        }
        public string obtener_cantidad_bonificables(DataTable resumen_pedido)
        {
            return "cantidad de productos bonificables: " + cantidad_bonificables(resumen_pedido).ToString();
        }
        public string obtener_cantidad_bonificados(DataTable resumen_pedido)
        {
            int cant_bonificables = cantidad_bonificables(resumen_pedido);

            return "cantidad de bonificados: " + cantidad_bonificados(resumen_pedido, cant_bonificables).ToString();
        }
        public bool verificar_modo_bonificado(DataTable resumen_pedido)
        {
            bool retorno = false;
            if (0 < cantidad_bonificados(resumen_pedido, cantidad_bonificables(resumen_pedido)))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_bonificados_cargados(DataTable resumen_pedido, DataTable bonificados)
        {
            bool retorno = false;
            int cant_bonificados = cantidad_bonificados(resumen_pedido, cantidad_bonificables(resumen_pedido));
            int cant_cargada = 0;
            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
            {
                cant_cargada = cant_cargada + int.Parse(bonificados.Rows[fila]["cantidad"].ToString());
            }
            if (cant_cargada == cant_bonificados)
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_si_se_puede_cargar(DataTable resumen_pedido, DataTable bonificados, string cantidad_a_cargar)
        {
            bool retorno = false;
            int cant_bonificados = cantidad_bonificados(resumen_pedido, cantidad_bonificables(resumen_pedido));
            int cant_a_cargar = int.Parse(cantidad_a_cargar);
            int cant_cargada = 0;
            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
            {
                cant_cargada = cant_cargada + int.Parse(bonificados.Rows[fila]["cantidad"].ToString());
            }

            if (cant_a_cargar <= cant_bonificados)
            {
                retorno = true;
            }

            if (cant_cargada == cant_bonificados)
            {
                retorno = false;
            }
            return retorno;
        }
        public string obtener_precio_de_bonificado(string id_producto)
        {
            string retorno = "0";
            lista_bonificados_especiales = obtener_lista_bonificados_especiales();

            for (int i = 0; i < lista_bonificados_especiales.Count - 1; i++)
            {
                if (id_producto == lista_bonificados_especiales[i].ToString())
                {
                    retorno = pedidos.get_precios_bonificados_especial();
                    break;
                }
            }

            return retorno;
        }
        public string obtener_tipo_de_bonificado(string id_producto)
        {
            string retorno = "BONIFICADO ESPECIAL";
            if ("0" == obtener_precio_de_bonificado(id_producto))
            {
                retorno = "BONIFICADO";
            }
            return retorno;
        }
        #endregion

        #region metodos privados
        private int cantidad_bonificables(DataTable resumen_pedido)
        {
            int retorno = 0;
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {

            }
            return retorno;
        }
        private int cantidad_bonificados(DataTable resumen_pedido, int cant_bonificables)
        {
            int retorno = 0;
            int condicion_bonificado = obtener_condicion_bonificado();
            int flag = condicion_bonificado;

            int conteo = 0;
            for (int cant = 1; cant <= cant_bonificables; cant++)
            {
                conteo = conteo + 1;
                if (conteo == flag)
                {
                    retorno++;
                    flag = flag + condicion_bonificado;
                }
            }
            return retorno;
        }
        private void obtener_lista_proveedores()
        {
            lista_proveedores = pedidos.get_lista_proveedores();
        }
        private void obtener_productos_proveedor(string id_sucursal)
        {
            if (id_sucursal == "31")
            {
                productos_proveedor = pedidos.get_productos_proveedor("Fabrica villa maipu", id_sucursal);
            }
            else
            {
                productos_proveedor = pedidos.get_productos_proveedor("Fabrica villa maipu", id_sucursal);
            }
            insumos_proveedor = pedidos.get_productos_proveedor("Shami Insumos", id_sucursal);
        }
        private int obtener_condicion_bonificado()
        {
            return pedidos.get_condicion_bonificado();
        }
        private void crear_lista_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("precio", typeof(string));
            resumen.Columns.Add("unidad_medida_local", typeof(string));
            resumen.Columns.Add("unidad_medida_fabrica", typeof(string));
            resumen.Columns.Add("presentacion", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("multiplicador", typeof(string));
            resumen.Columns.Add("alimento", typeof(string));
            resumen.Columns.Add("bebida", typeof(string));
            resumen.Columns.Add("descartable", typeof(string));
            resumen.Columns.Add("orden_tipo", typeof(int));
        }
        private void llenar_lista_resumen()
        {
            crear_lista_resumen();
            int fila_resumen = 0;
            if (productos_proveedor != null)
            {
                for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
                {
                    if (productos_proveedor.Rows[fila]["activa"].ToString() == "1")
                    {
                        resumen.Rows.Add();
                        resumen.Rows[fila_resumen]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                        resumen.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                        resumen.Rows[fila_resumen]["tipo_producto"] = productos_proveedor.Rows[fila]["tipo_producto"].ToString();
                        resumen.Rows[fila_resumen]["precio"] = productos_proveedor.Rows[fila]["precio"].ToString();
                        resumen.Rows[fila_resumen]["unidad_medida_local"] = productos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();
                        resumen.Rows[fila_resumen]["unidad_medida_fabrica"] = productos_proveedor.Rows[fila]["unidad_de_medida_fabrica"].ToString();
                        resumen.Rows[fila_resumen]["alimento"] = productos_proveedor.Rows[fila]["alimento"].ToString();
                        resumen.Rows[fila_resumen]["bebida"] = productos_proveedor.Rows[fila]["bebida"].ToString();
                        resumen.Rows[fila_resumen]["descartable"] = productos_proveedor.Rows[fila]["descartable"].ToString();
                        resumen.Rows[fila_resumen]["presentacion"] = " x" + productos_proveedor.Rows[fila]["unidad_de_medida_fabrica"].ToString();
                        resumen.Rows[fila_resumen]["proveedor"] = "proveedor_villaMaipu";
                        resumen.Rows[fila_resumen]["multiplicador"] = "1";
                        resumen.Rows[fila_resumen]["orden_tipo"] = funciones.obtener_dato(productos_proveedor.Rows[fila]["tipo_producto"].ToString(), 1);
                        fila_resumen++;
                    }
                }
            }

            string paquete, cantidad_unidades, unidad, dato;
            if (insumos_proveedor != null)
            {
                for (int fila = 0; fila <= insumos_proveedor.Rows.Count - 1; fila++)
                {
                    if (insumos_proveedor.Rows[fila]["activa"].ToString() == "1")
                    {
                        resumen.Rows.Add();
                        resumen.Rows[fila_resumen]["id"] = insumos_proveedor.Rows[fila]["id"].ToString();
                        resumen.Rows[fila_resumen]["producto"] = insumos_proveedor.Rows[fila]["producto"].ToString();
                        resumen.Rows[fila_resumen]["tipo_producto"] = insumos_proveedor.Rows[fila]["tipo_producto"].ToString();
                        resumen.Rows[fila_resumen]["precio"] = insumos_proveedor.Rows[fila]["precio"].ToString();
                        resumen.Rows[fila_resumen]["alimento"] = insumos_proveedor.Rows[fila]["alimento"].ToString();
                        resumen.Rows[fila_resumen]["bebida"] = insumos_proveedor.Rows[fila]["bebida"].ToString();
                        resumen.Rows[fila_resumen]["descartable"] = insumos_proveedor.Rows[fila]["descartable"].ToString();
                        paquete = funciones.obtener_dato(insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString(), 1);
                        cantidad_unidades = funciones.obtener_dato(insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString(), 2);
                        unidad = funciones.obtener_dato(insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString(), 3);
                        if (paquete == "Unidad")
                        {
                            dato = cantidad_unidades + " " + unidad;
                        }
                        else
                        {
                            dato = paquete + " x " + cantidad_unidades + " " + unidad;
                        }
                        resumen.Rows[fila_resumen]["unidad_medida_local"] = dato; //insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();
                        resumen.Rows[fila_resumen]["presentacion"] = ""; //insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();

                        unidad = funciones.obtener_dato(insumos_proveedor.Rows[fila]["producto_1"].ToString(), 3);
                        if (paquete == "Unidad")
                        {
                            dato = cantidad_unidades + " " + unidad;
                        }
                        else
                        {
                            dato = paquete + " x " + cantidad_unidades + " " + unidad;
                        }

                        resumen.Rows[fila_resumen]["unidad_medida_fabrica"] = dato;
                        resumen.Rows[fila_resumen]["proveedor"] = "insumos_fabrica";
                        resumen.Rows[fila_resumen]["multiplicador"] = cantidad_unidades.ToString();
                        resumen.Rows[fila_resumen]["orden_tipo"] = funciones.obtener_dato(insumos_proveedor.Rows[fila]["tipo_producto"].ToString(), 1);
                        fila_resumen++;
                    }
                }
            }
        }
        #endregion
        #region metodos get/set
        public DataTable get_productos_proveedor(string id_sucursal)
        {
            obtener_productos_proveedor(id_sucursal);
            //crear lista resumen
            llenar_lista_resumen();
            //llenar lista con productos e insumos
            //retornar lista resumen
            return resumen;
        }
        #endregion
    }
}
