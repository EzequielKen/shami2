using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_recepcion_de_insumos
    {
        public cls_recepcion_de_insumos(DataTable usuario_BD)
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
        cls_whatsapp whatsapp = new cls_whatsapp();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable pedido_de_insumos;
        DataTable insumos_fabricaBD;
        DataTable insumos_fabrica;
        #endregion

        #region carga a base de datos
        public void recibir_insumos(DataTable resumen, string id_pedido_insumo_seleccionado)
        {
            string dato, cantidad_recibida_dato, actualizar,index;
            consultar_insumos_fabricaBD();
            for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
            {
                if (resumen.Rows[fila]["cantidad_recibida_dato"].ToString()!="N/A")
                {
                    index = resumen.Rows[fila]["index"].ToString();
                    cantidad_recibida_dato = resumen.Rows[fila]["cantidad_recibida_dato"].ToString();
                    dato = resumen.Rows[fila]["dato"].ToString();
                    actualizar = "`"+index+"` = '"+dato +"-"+cantidad_recibida_dato +"'";
                    consultas.actualizar_tabla(base_de_datos, "pedido_de_insumos",actualizar,id_pedido_insumo_seleccionado);
                    //descontar de stock de insumos cantidad recibida
                    descontar_stock_de_insumos(dato,cantidad_recibida_dato);
                    actualizar = "`estado` = 'Recibido'";
                    consultas.actualizar_tabla(base_de_datos, "pedido_de_insumos", actualizar, id_pedido_insumo_seleccionado);
                }
            }
        }
        private void descontar_stock_de_insumos(string dato,string cantidad_recibida_dato)
        {
            string id_insumo = funciones.obtener_dato(dato, 1);
            int fila_insumo = funciones.buscar_fila_por_id(id_insumo, insumos_fabricaBD);

            string tipo_paqueteBD, unidadBD, tipo_unidadBD;
            string tipo_paquete, unidad, tipo_unidad;
            string nombre_columna,actualizar,nuevo_dato;
            tipo_paquete = funciones.obtener_dato(dato, 6);
            unidad = funciones.obtener_dato(dato, 7);
            tipo_unidad = funciones.obtener_dato(dato, 8);
            double nuevo_stock, cantidad_stock, cantidad_recibida;
            double nuevo_stock_produccion, stock_produccion;
            for (int columna = insumos_fabricaBD.Columns["producto_1"].Ordinal; columna <= insumos_fabricaBD.Columns.Count-1; columna++)
            {
                if (insumos_fabricaBD.Rows[fila_insumo][columna].ToString()!="N/A")
                {
                    tipo_paqueteBD = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 1);
                    unidadBD = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 2);
                    tipo_unidadBD = funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 3);
                    if (tipo_paquete == tipo_paqueteBD &&
                        unidad == unidadBD &&
                        tipo_unidad == tipo_unidadBD)
                    {
                        cantidad_stock = double.Parse(funciones.obtener_dato(insumos_fabricaBD.Rows[fila_insumo][columna].ToString(), 4));
                        cantidad_recibida = double.Parse(cantidad_recibida_dato);
                        nuevo_stock = cantidad_stock - cantidad_recibida;
                        nuevo_dato = tipo_paqueteBD + "-" + unidadBD + "-" + tipo_unidadBD + "-" + nuevo_stock + "-" + nuevo_stock;
                        nombre_columna = insumos_fabricaBD.Columns[columna].ColumnName;
                        actualizar = "`" + nombre_columna + "` = '" + nuevo_dato+ "'";
                        consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
                        //calcular y sumar stock de produccion
                        stock_produccion = double.Parse(insumos_fabricaBD.Rows[fila_insumo]["stock_produccion"].ToString());
                        nuevo_stock_produccion = stock_produccion + (cantidad_recibida * double.Parse(unidadBD));
                        actualizar = "`stock_produccion` = '" + nuevo_stock_produccion + "'";
                        consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
                        break;
                    }
                }
            }
        }
        #endregion

        #region carga insumos
        private void crear_tabla_insumos()
        {
            insumos_fabrica = new DataTable();
            insumos_fabrica.Columns.Add("id", typeof(string));
            insumos_fabrica.Columns.Add("producto", typeof(string));
            insumos_fabrica.Columns.Add("cantidad_pedida", typeof(string));
            insumos_fabrica.Columns.Add("cantidad_despachada", typeof(string));
            insumos_fabrica.Columns.Add("cantidad_recibida", typeof(string));
            insumos_fabrica.Columns.Add("cantidad_recibida_dato", typeof(string));
            insumos_fabrica.Columns.Add("presentacion_despachada", typeof(string));
            insumos_fabrica.Columns.Add("dato", typeof(string));
            insumos_fabrica.Columns.Add("index", typeof(string));
        }

        private void llenar_tabla_insumos()
        {
            crear_tabla_insumos();
            string cantidad_despachada, tipo_paquete, unidad, tipo_unidad;
            int fila_tabla = 0;
            for (int columna = pedido_de_insumos.Columns["producto_1"].Ordinal; columna <= pedido_de_insumos.Columns.Count - 1; columna++)
            {
                if (pedido_de_insumos.Rows[0][columna].ToString() != "N/A")
                {
                    insumos_fabrica.Rows.Add();
                    insumos_fabrica.Rows[fila_tabla]["id"] = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 1);
                    insumos_fabrica.Rows[fila_tabla]["producto"] = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 2);
                    insumos_fabrica.Rows[fila_tabla]["cantidad_pedida"] = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 3) + " " + funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 4);
                    if (funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 5)=="N/A")
                    {
                        insumos_fabrica.Rows[fila_tabla]["cantidad_despachada"] = "N/A";
                    }
                    else
                    {
                        cantidad_despachada = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 5);
                        tipo_paquete = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 6);
                        unidad = funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 7);
                        tipo_unidad= funciones.obtener_dato(pedido_de_insumos.Rows[0][columna].ToString(), 8);
                        insumos_fabrica.Rows[fila_tabla]["cantidad_despachada"] = cantidad_despachada + " " + tipo_paquete + " " + unidad + " " + tipo_unidad ;
                        insumos_fabrica.Rows[fila_tabla]["presentacion_despachada"] =  tipo_paquete + " " + unidad + " " + tipo_unidad ;
                    }
                    insumos_fabrica.Rows[fila_tabla]["cantidad_recibida"] = "N/A";
                    insumos_fabrica.Rows[fila_tabla]["cantidad_recibida_dato"] = "N/A";
                    insumos_fabrica.Rows[fila_tabla]["dato"] = pedido_de_insumos.Rows[0][columna].ToString();
                    insumos_fabrica.Rows[fila_tabla]["index"] = pedido_de_insumos.Columns[columna].ColumnName.ToString();
                    fila_tabla++;
                }
            }
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabricaBD()
        {
            insumos_fabricaBD = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        private void consultar_pedido_de_insumos(string id_movimiento)
        {
            pedido_de_insumos = consultas.consultar_pedido_de_insumos_por_id(id_movimiento);
        }
        #endregion

        #region metodos get/set
        public DataTable get_pedido(string id_movimiento)
        {
            consultar_pedido_de_insumos(id_movimiento);
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
