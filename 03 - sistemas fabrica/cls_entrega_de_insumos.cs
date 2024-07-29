using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_entrega_de_insumos
    {
        public cls_entrega_de_insumos(DataTable usuario_BD)
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
            stock_insumos = new cls_stock_insumos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_whatsapp whatsapp = new cls_whatsapp();
        cls_stock_insumos stock_insumos;
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable pedido_de_insumos;
        DataTable insumos_fabricaBD;
        DataTable insumos_fabricaBD_copia;
        DataTable insumos_fabrica;
        DataTable sub_productos;
        DataTable sub_productos_copia;
        #endregion

        #region carga a base de datos
        public void entregar_insumos(DataTable resumen, string id_pedido_insumo_seleccionado, string rol_usuario)
        {
            //cambiar estado a "Despachado"
            cambiar_estado_a_despachado(id_pedido_insumo_seleccionado);
            //cargar unidades y pesentacion despachadas (cargar index de ubicacion de stock al final)
            cargar_presentacion_despachadas(resumen, id_pedido_insumo_seleccionado, rol_usuario);
        }
        private void cambiar_estado_a_despachado(string id_pedido_insumo_seleccionado)
        {
            string actualizar = "`estado` = 'Despachado'";
            consultas.actualizar_tabla(base_de_datos, "pedido_de_insumos", actualizar, id_pedido_insumo_seleccionado);
        }
        private void cargar_presentacion_despachadas(DataTable resumen, string id_pedido_insumo_seleccionado, string rol_usuario)
        {
            consultar_insumos_fabricaBD();
            consultar_sub_productos();
            consultar_pedido_de_insumos(id_pedido_insumo_seleccionado);
            string id, producto, catidad_pedida, unidad_pedida, dato, cantidad_entrega, presentacion, actualizar;
            int fila = 0;
            int fila_insumo, columna_presentacion;
            for (int columna = pedido_de_insumos.Columns["producto_1"].Ordinal; columna <= pedido_de_insumos.Columns.Count - 1; columna++)
            {
                if (pedido_de_insumos.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 1);
                    fila = funciones.buscar_fila_por_id(id, resumen);
                    if (fila > -1)
                    {
                        id = funciones.obtener_dato(resumen.Rows[fila]["dato"].ToString(), 1);
                        producto = funciones.obtener_dato(resumen.Rows[fila]["dato"].ToString(), 2);
                        catidad_pedida = funciones.obtener_dato(resumen.Rows[fila]["dato"].ToString(), 3);
                        unidad_pedida = funciones.obtener_dato(resumen.Rows[fila]["dato"].ToString(), 4);
                        cantidad_entrega = resumen.Rows[fila]["cantidad_entrega"].ToString();
                        presentacion = resumen.Rows[fila]["presentacion"].ToString();
                        dato = id + "-" + producto + "-" + catidad_pedida + "-" + unidad_pedida + "-" + cantidad_entrega + "-" + presentacion;

                        actualizar = "`" + pedido_de_insumos.Columns[columna].ColumnName + "` = '" + dato + "'";
                        consultas.actualizar_tabla(base_de_datos, "pedido_de_insumos", actualizar, id_pedido_insumo_seleccionado);

                        if ("19-Sub Productos"==resumen.Rows[fila]["tipo_producto"].ToString())//
                        {
                            fila_insumo = funciones.buscar_fila_por_id(id, sub_productos);
                            actualizar_stock_en_sub_producto(fila_insumo,  cantidad_entrega);
                        }
                        else
                        {
                            fila_insumo = funciones.buscar_fila_por_id(id, insumos_fabricaBD);
                            columna_presentacion = buscar_columna_presentacion(fila_insumo, presentacion, insumos_fabricaBD);
                            actualizar_stock_en_tabla(fila_insumo, columna_presentacion, cantidad_entrega);
                        }
                    }
                }
            }
            stock_insumos.guardar_registro_entrega_insumo(rol_usuario, insumos_fabricaBD, insumos_fabricaBD_copia);
            stock_insumos.guardar_registro_entrega_sub_productos(rol_usuario, sub_productos, sub_productos_copia);
        }
        private void actualizar_stock_en_tabla(int fila_insumo, int columna_presentacion, string cantidad_entrega_dato)
        {
            double cantidad_entrega, stock_actual, nuevo_stock;
            string tipo_paquete, unidades, unidad, dato;
            tipo_paquete = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna_presentacion].ToString(), 1);
            unidades = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna_presentacion].ToString(), 2);
            unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna_presentacion].ToString(), 3);
            stock_actual = double.Parse(funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna_presentacion].ToString(), 4));
            cantidad_entrega = double.Parse(cantidad_entrega_dato);
            nuevo_stock = stock_actual - cantidad_entrega;
            dato = tipo_paquete + "-" + unidades + "-" + unidad + "-" + nuevo_stock.ToString() + "-" + nuevo_stock.ToString();
            insumos_fabricaBD.Rows[fila_insumo][columna_presentacion] = dato;
            insumos_fabricaBD.Rows[fila_insumo]["nuevo_stock"] = "si";
        }
        private void actualizar_stock_en_sub_producto(int fila_insumo,string cantidad_entrega_dato)
        {
            double cantidad_entrega, stock_actual, nuevo_stock;
             stock_actual = double.Parse(sub_productos.Rows[fila_insumo]["stock_expedicion"].ToString());
            cantidad_entrega = double.Parse(cantidad_entrega_dato);
            nuevo_stock = stock_actual - cantidad_entrega;
            sub_productos.Rows[fila_insumo]["stock_expedicion"] = nuevo_stock;
            sub_productos.Rows[fila_insumo]["nuevo_stock"] = "si";
        }
        private int buscar_columna_presentacion(int fila_insumo, string presentacion, DataTable dt)
        {
            string tipo_paquete, unidades, unidad;
            string tipo_paquete_dato, unidades_dato, unidad_dato;
            int retorno = -1;
            for (int columna = dt.Columns["producto_1"].Ordinal; columna <= dt.Columns.Count - 1; columna++)
            {
                if (dt.Rows[fila_insumo][columna].ToString() == "N/A")
                {
                    break;
                }
                else
                {
                    tipo_paquete = funciones.obtener_dato(presentacion, 1);
                    unidades = funciones.obtener_dato(presentacion, 2);
                    unidad = funciones.obtener_dato(presentacion, 3);

                    tipo_paquete_dato = funciones.obtener_dato(dt.Rows[fila_insumo][columna].ToString(), 1);
                    unidades_dato = funciones.obtener_dato(dt.Rows[fila_insumo][columna].ToString(), 2);
                    unidad_dato = funciones.obtener_dato(dt.Rows[fila_insumo][columna].ToString(), 3);
                    if (tipo_paquete == tipo_paquete_dato &&
                        unidades == unidades_dato &&
                        unidad == unidad_dato)
                    {
                        retorno = columna;
                        break;
                    }
                }
            }
            return retorno;
        }
        #endregion

        #region carga insumos
        private void crear_tabla_insumos()
        {
            insumos_fabrica = new DataTable();
            insumos_fabrica.Columns.Add("id", typeof(string));
            insumos_fabrica.Columns.Add("producto", typeof(string));
            insumos_fabrica.Columns.Add("pedido", typeof(string));
            insumos_fabrica.Columns.Add("dato", typeof(string));
            insumos_fabrica.Columns.Add("tipo_producto", typeof(string)); 
            insumos_fabrica.Columns.Add("unidad_medida", typeof(string)); 
        }

        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            string id_insumo, producto;
            int fila_insumo = 0;
            int fila_tabla = 0;
            for (int columna = pedido_de_insumos.Columns["producto_1"].Ordinal; columna <= pedido_de_insumos.Columns.Count - 1; columna++)
            {
                if (pedido_de_insumos.Rows[0][columna].ToString() != "N/A")
                {
                    id_insumo = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 2);
                    fila_insumo = funciones.buscar_fila_por_id_nombre(id_insumo, producto, insumos_fabricaBD);
                    insumos_fabrica.Rows.Add();
                    insumos_fabrica.Rows[fila_tabla]["id"] = insumos_fabricaBD.Rows[fila_insumo]["id"].ToString();
                    insumos_fabrica.Rows[fila_tabla]["producto"] = insumos_fabricaBD.Rows[fila_insumo]["producto"].ToString();
                    insumos_fabrica.Rows[fila_tabla]["tipo_producto"] = insumos_fabricaBD.Rows[fila_insumo]["tipo_producto"].ToString();
                    insumos_fabrica.Rows[fila_tabla]["pedido"] = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 3) + " " + funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 4);
                    insumos_fabrica.Rows[fila_tabla]["dato"] = pedido_de_insumos.Rows[0][columna].ToString();
                    if ("19-Sub Productos" == insumos_fabricaBD.Rows[fila_insumo]["tipo_producto"].ToString())//unidad_medida
                    {
                        insumos_fabrica.Rows[fila_tabla]["unidad_medida"] = insumos_fabricaBD.Rows[fila_insumo]["unidad_medida"].ToString();
                    }
                    fila_tabla++;
                }
            }
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabricaBD()
        {
            insumos_fabricaBD = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
            insumos_fabricaBD_copia = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");

            insumos_fabricaBD.Columns.Add("nuevo_stock", typeof(string));
            for (int fila = 0; fila <= insumos_fabricaBD.Rows.Count - 1; fila++)
            {
                insumos_fabricaBD.Rows[fila]["nuevo_stock"] = "N/A";
            }
        }
        private void consultar_pedido_de_insumos(string id_movimiento)
        {
            pedido_de_insumos = consultas.consultar_pedido_de_insumos_por_id(id_movimiento);
        }
        private void consultar_sub_productos()
        {
            sub_productos = consultas.consultar_sub_productos();
            sub_productos_copia = consultas.consultar_sub_productos();
            sub_productos.Columns.Add("nuevo_stock", typeof(string));
            for (int fila = 0; fila <= sub_productos.Rows.Count - 1; fila++)
            {
                sub_productos.Rows[fila]["nuevo_stock"] = "N/A";
            }
        }
        #endregion
        #region metodos privados
        private void combinar_tablas()
        {
            int ultima_fila;
            for (int fila = 0; fila <= sub_productos.Rows.Count - 1; fila++)
            {
                insumos_fabricaBD.Rows.Add();
                ultima_fila = insumos_fabricaBD.Rows.Count - 1;

                insumos_fabricaBD.Rows[ultima_fila]["id"] = sub_productos.Rows[fila]["id"].ToString();
                insumos_fabricaBD.Rows[ultima_fila]["producto"] = sub_productos.Rows[fila]["producto"].ToString();
                insumos_fabricaBD.Rows[ultima_fila]["unidad_medida"] = sub_productos.Rows[fila]["unidad_de_medida_local"].ToString();
                insumos_fabricaBD.Rows[ultima_fila]["tipo_producto"] = sub_productos.Rows[fila]["tipo_producto"].ToString();
            }
        }
        #endregion
        #region metodos get/set
        public DataTable get_insumos_fabrica(string id_movimiento)
        {
            consultar_pedido_de_insumos(id_movimiento);
            consultar_insumos_fabricaBD();
            consultar_sub_productos();
            combinar_tablas();
            llenar_tabla_insumos();
            return insumos_fabrica;
        }
        public List<string> get_presentaciones_de_insumo(string id_insumo)
        {
            consultar_insumos_fabricaBD();
            return cargar_presentaciones(id_insumo);
        }
        #endregion

        #region carga presentaciones
        private List<string> cargar_presentaciones(string id_insumo)
        {
            List<string> retorno = new List<string>();

            int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabricaBD);
            string paquete, unidad, tipo_unidad, dato;
            for (int columna = insumos_fabricaBD.Columns["producto_1"].Ordinal; columna <= insumos_fabricaBD.Columns.Count - 1; columna++)
            {
                if (insumos_fabricaBD.Rows[fila_insumo][columna].ToString() != "N/A")
                {
                    paquete = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 1);
                    unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 2);
                    tipo_unidad = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 3);
                    dato = paquete + "-" + unidad + "-" + tipo_unidad;
                    retorno.Add(dato);
                }
            }

            return retorno;
        }
        #endregion

    }
}
