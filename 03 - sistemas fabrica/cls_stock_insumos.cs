using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_stock_insumos
    {
        public cls_stock_insumos(DataTable usuario_BD)
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
            historial_stock_insumos = new cls_movimientos_stock_insumos(usuario_BD);
            historial_stock_productos = new cls_movimientos_stock_producto(usuario_BD);
        }
        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        cls_movimientos_stock_insumos historial_stock_insumos;
        cls_movimientos_stock_producto historial_stock_productos;
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable orden_de_pedido;
        DataTable resumen_de_pedido;
        DataTable insumos_fabrica;
        #endregion


        #region conteo stock
        public void guardar_registro(string rol_usuario, DataTable insumos, DataTable insumos_copia)
        {
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    //actualizar stock en insumos
                    actualizar_stock_insumo(fila, insumos);
                }
            }
            guardar_registro_conteo_de_stock(rol_usuario, insumos, insumos_copia, "conteo de stock");
        }
        private void guardar_registro_conteo_de_stock(string rol_usuario, DataTable insumos, DataTable insumos_copia, string tipo_registro)
        {

            string fecha = funciones.get_fecha();
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                for (int columna_insumo = insumos.Columns["producto_1"].Ordinal; columna_insumo <= insumos.Columns.Count - 1; columna_insumo++)
                {
                    if (insumos.Rows[fila][columna_insumo].ToString() == "N/A")
                    {
                        break;
                    }
                    else
                    {
                        historial_stock_insumos.registrar_movimiento_en_historial(fila, columna_insumo, rol_usuario, fecha, insumos, insumos_copia, tipo_registro);
                    }
                }

            }
        }
        #endregion
        #region compra
        public void guardar_registro_compra(string rol_usuario, DataTable insumos, DataTable insumos_copia)
        {
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    //actualizar stock en insumos
                    actualizar_stock_insumo(fila, insumos);
                }
            }
            guardar_registro_conteo_de_stock_compra(rol_usuario, insumos, insumos_copia, "actualizacion por compra");
        }
        private void guardar_registro_conteo_de_stock_compra(string rol_usuario, DataTable insumos, DataTable insumos_copia, string tipo_registro)
        {

            string fecha = funciones.get_fecha();
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                for (int columna_insumo = insumos.Columns["producto_1"].Ordinal; columna_insumo <= insumos.Columns.Count - 1; columna_insumo++)
                {
                    if (insumos.Rows[fila][columna_insumo].ToString() == "N/A")
                    {

                        break;
                    }
                    else
                    {
                        historial_stock_insumos.registrar_movimiento_en_historial(fila, columna_insumo, rol_usuario, fecha, insumos, insumos_copia, tipo_registro);
                    }
                }

            }
        }
        #endregion
        #region entrega de insumos expedicion a produccion
        public void guardar_registro_entrega_insumo(string rol_usuario, DataTable insumos, DataTable insumos_copia)
        {
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    //actualizar stock en insumos
                    actualizar_stock_insumo(fila, insumos);
                }
            }
            guardar_registro_conteo_de_stock_entrega_insumo(rol_usuario, insumos, insumos_copia, "despacho sub producto expredicion a produccion");
        }
        #endregion
        #region entrega de sub productos de expedicion a produccion
        public void guardar_registro_entrega_sub_productos(string rol_usuario, DataTable insumos, DataTable insumos_copia)
        {
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    //actualizar stock en insumos
                    actualizar_stock_sub_producto(fila, insumos);
                }
            }
            guardar_registro_conteo_de_stock_entrega_sub_producto(rol_usuario, insumos, insumos_copia, "despacho insumo expredicion a produccion");
        }
        private void guardar_registro_conteo_de_stock_entrega_insumo(string rol_usuario, DataTable insumos, DataTable insumos_copia, string tipo_registro)
        {

            string fecha = funciones.get_fecha();
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                for (int columna_insumo = insumos.Columns["producto_1"].Ordinal; columna_insumo <= insumos.Columns.Count - 1; columna_insumo++)
                {
                    if (insumos.Rows[fila][columna_insumo].ToString() == "N/A")
                    {
                        break;
                    }
                    else
                    {
                        historial_stock_insumos.registrar_movimiento_en_historial(fila, columna_insumo, rol_usuario, fecha, insumos, insumos_copia, tipo_registro);
                    }
                }

            }
        }
        private void guardar_registro_conteo_de_stock_entrega_sub_producto(string rol_usuario, DataTable insumos, DataTable insumos_copia, string tipo_registro)
        {

            string fecha = funciones.get_fecha();
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    int columna_insumo = insumos.Columns["stock_expedicion"].Ordinal;
                    historial_stock_insumos.registrar_movimiento_en_historial_sub_producto(fila, columna_insumo, rol_usuario, fecha, insumos, insumos_copia, tipo_registro);
                }
            }
        }
        #endregion
        #region entrega de insumos expedicion a local
        public void guardar_registro_entrega_insumo_a_local(string rol_usuario, DataTable insumos, DataTable insumos_copia)
        {
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                if (insumos.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                {
                    //actualizar stock en insumos
                    actualizar_stock_insumo(fila, insumos);

                }
            }
            guardar_registro_conteo_de_stock_entrega_insumo_a_cliente(rol_usuario, insumos, insumos_copia, "despacho insumo expredicion a cliente");
        }
        private void guardar_registro_conteo_de_stock_entrega_insumo_a_cliente(string rol_usuario, DataTable insumos, DataTable insumos_copia, string tipo_registro)
        {

            string fecha = funciones.get_fecha();
            int cantidad = insumos.Rows.Count - 1;
            int columna_insumo = insumos.Columns["producto_1"].Ordinal;
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                columna_insumo = insumos.Columns["producto_1"].Ordinal;
                while (columna_insumo < insumos.Columns.Count &&
                insumos.Rows[fila][columna_insumo].ToString() != "N/A")
                {

                    historial_stock_insumos.registrar_movimiento_en_historial(fila, columna_insumo, rol_usuario, fecha, insumos, insumos_copia, tipo_registro);

                    columna_insumo++;
                }

            }
            columna_insumo = insumos.Columns["producto_1"].Ordinal;

        }
        #endregion
        private void actualizar_stock_insumo(int fila, DataTable insumos)
        {
            string actualizar, id;
            for (int columna = insumos.Columns["producto_1"].Ordinal; columna <= insumos.Columns.Count - 1; columna++)
            {
                if (insumos.Rows[fila][columna].ToString() == "N/A")
                {
                    break;
                }
                else
                {
                    actualizar = "`" + insumos.Columns[columna].ColumnName + "` = '" + insumos.Rows[fila][columna].ToString() + "'";
                    id = insumos.Rows[fila]["id"].ToString();
                    consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id);
                }
            }
        }
        private void actualizar_stock_sub_producto(int fila, DataTable insumos)
        {
            string actualizar, id;
            actualizar = "`stock_expedicion` = '" + insumos.Rows[fila]["stock_expedicion"].ToString() + "'";
            id = insumos.Rows[fila]["id"].ToString();
            consultas.actualizar_tabla(base_de_datos, "proveedor_villaMaipu", actualizar, id);

            actualizar = "`stock` = '" + insumos.Rows[fila]["stock_expedicion"].ToString() + "'";
            consultas.actualizar_tabla(base_de_datos, "proveedor_villaMaipu", actualizar, id);

        }
        #region metodos actualizar stock
        private void actualizar_stock()
        {
            bool calculo;
            string id, actualizar;
            int columa = 0;
            double stock_actual, nuevo_stock, unidad_multiplicadora, cantidad;
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                calculo = false;
                nuevo_stock = 0;
                stock_actual = double.Parse(insumos_fabrica.Rows[fila]["stock"].ToString());
                columa = insumos_fabrica.Columns["producto_1"].Ordinal;

                while (columa <= insumos_fabrica.Columns.Count - 1 &&
                       insumos_fabrica.Rows[fila][columa].ToString() != "N/A")
                {
                    if (double.TryParse(funciones.obtener_dato(insumos_fabrica.Rows[fila][columa].ToString(), 4), out cantidad))
                    {
                        calculo = true;
                        unidad_multiplicadora = double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila][columa].ToString(), 2));
                        // cantidad = double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila][columa].ToString(), 4));

                        nuevo_stock = nuevo_stock + (cantidad * unidad_multiplicadora);

                    }
                    else
                    {
                        //cantidad = double.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila][columa].ToString(), 4));
                        string muestra = insumos_fabrica.Rows[fila][columa].ToString();

                    }
                    columa++;

                }
                if (calculo == true &&
                    stock_actual != nuevo_stock)
                {
                    actualizar = "`stock` = '" + nuevo_stock + "' ";
                    id = insumos_fabrica.Rows[fila]["id"].ToString();
                    consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id);
                }
            }
        }
        #endregion

        #region metodos privados
        private List<string> obtener_presentacion(int fila_producto, DataTable productos)
        {
            List<string> list = new List<string>();
            string dato, tipo_paquete, cant_unidad, unidad, cantidad;

            for (int columna = productos.Columns["producto_1"].Ordinal; columna <= productos.Columns.Count - 1; columna++)
            {
                if (productos.Rows[fila_producto][columna].ToString() == "N/A" ||
                    productos.Rows[fila_producto][columna].ToString() == "0")
                {
                    break;
                }
                else
                {
                    tipo_paquete = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 1);
                    cant_unidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 2);
                    unidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 3);
                    cantidad = funciones.obtener_dato(productos.Rows[fila_producto][columna].ToString(), 4);
                    double cant = double.Parse(cantidad);
                    if (cant < 0)
                    {
                        cantidad = "0";
                    }
                    if (tipo_paquete == "Unidad")
                    {
                        dato = cantidad + " x" + cant_unidad + "" + unidad;
                        string control = productos.Rows[fila_producto][columna].ToString();
                        list.Add(productos.Rows[fila_producto][columna].ToString());
                    }
                    else
                    {
                        dato = cantidad + " " + tipo_paquete + " x" + cant_unidad + " " + unidad;
                        string control = productos.Rows[fila_producto][columna].ToString();
                        list.Add(productos.Rows[fila_producto][columna].ToString());
                    }
                }
            }

            return list;
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            insumos_fabrica.Columns.Add("nuevo_stock", typeof(string));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                insumos_fabrica.Rows[fila]["nuevo_stock"] = "N/A";
            }
            /*insumos_fabrica.Columns.Add("presentaciones", typeof(List<string>)); 
            insumos_fabrica.Columns.Add("stock_presentacion", typeof(string));
            insumos_fabrica.Columns.Add("stock_nuevo", typeof(string)); 
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count-1; fila++)
            {
                insumos_fabrica.Rows[fila]["presentaciones"] = obtener_presentacion(fila, insumos_fabrica);
                insumos_fabrica.Rows[fila]["stock_nuevo"] = "N/A";
            }*/
            return insumos_fabrica;
        }
        #endregion

        #region metodos publicos
        public void actualizar_stock_insumos()
        {
            consultar_insumos_fabrica();
            actualizar_stock();
        }
        #endregion
    }
}