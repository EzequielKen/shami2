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
    public class cls_analisis_de_produccion
    {
        public cls_analisis_de_produccion(DataTable usuario_BD)
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
            estadisticas = new cls_estadisticas_de_pedidos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_estadisticas_de_pedidos estadisticas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable produccion;
        DataTable analisis_produccion;
        DataTable productos_produccion;
        #endregion

        #region metodos privados
        private void crear_tabla_analisis()
        {
            analisis_produccion = new DataTable();//
            analisis_produccion.Columns.Add("id",typeof(string));
            analisis_produccion.Columns.Add("producto",typeof(string));
            analisis_produccion.Columns.Add("tipo_producto",typeof(string));
            analisis_produccion.Columns.Add("cantidad_producida",typeof(string));
            analisis_produccion.Columns.Add("cantidad_recibida",typeof(string));
            analisis_produccion.Columns.Add("unidad_de_medida_produccion", typeof(string));
        }
        private void llenar_analisis_produccion()
        {
            crear_tabla_analisis();
            for (int fila_produccion = 0; fila_produccion <= produccion.Rows.Count-1; fila_produccion++)
            {
                for (int columna = produccion.Columns["producto_1"].Ordinal; columna <=produccion.Columns.Count-1; columna++)
                {
                    if (funciones.IsNotDBNull(produccion.Rows[fila_produccion][columna]))
                    {
                        if (produccion.Rows[fila_produccion][columna].ToString()!="")
                        {
                        cargar_producto_en_analisis(produccion.Rows[fila_produccion][columna].ToString(), fila_produccion);
                        }
                    }
                }
            }
            string cantidad_producida, cantidad_recibida;
            for (int fila = 0; fila <= analisis_produccion.Rows.Count-1; fila++)
            {
                cantidad_producida= analisis_produccion.Rows[fila]["cantidad_producida"].ToString();
                cantidad_recibida = analisis_produccion.Rows[fila]["cantidad_recibida"].ToString();
                analisis_produccion.Rows[fila]["cantidad_producida"] = cantidad_producida + " " + analisis_produccion.Rows[fila]["unidad_de_medida_produccion"].ToString();
                analisis_produccion.Rows[fila]["cantidad_recibida"] = cantidad_producida + " " + analisis_produccion.Rows[fila]["unidad_de_medida_produccion"].ToString();
            }
        }
        private void cargar_producto_en_analisis(string dato,int fila_produccion)
        {
            double cantidad_producida,nueva_cantidad_producida, cantidad_recibida,nueva_cantidad_recibida;
            string id_producto = funciones.obtener_dato(dato,1);
            int fila_analisis = funciones.buscar_fila_por_id(id_producto,analisis_produccion);
            int fila_producto = funciones.buscar_fila_por_id(id_producto, productos_produccion);
            if (fila_analisis == -1)
            {
                //crear
                analisis_produccion.Rows.Add();
                fila_analisis = analisis_produccion.Rows.Count - 1;
                analisis_produccion.Rows[fila_analisis]["id"] = id_producto;
                analisis_produccion.Rows[fila_analisis]["producto"] = productos_produccion.Rows[fila_producto]["producto"].ToString();
                analisis_produccion.Rows[fila_analisis]["tipo_producto"] = productos_produccion.Rows[fila_producto]["tipo_producto"].ToString();

                analisis_produccion.Rows[fila_analisis]["cantidad_producida"] = funciones.obtener_dato(dato, 3);
                if (produccion.Rows[fila_produccion]["estado"].ToString()== "Recibido" &&
                    funciones.obtener_dato(dato, 4)!= "N/A")
                {
                    analisis_produccion.Rows[fila_analisis]["cantidad_recibida"] = funciones.obtener_dato(dato,4);
                }
                else
                {
                    analisis_produccion.Rows[fila_analisis]["cantidad_recibida"] = "0";
                }

                analisis_produccion.Rows[fila_analisis]["unidad_de_medida_produccion"] = productos_produccion.Rows[fila_producto]["unidad_de_medida_produccion"].ToString();
            }
            else
            {
                //sumar
                cantidad_producida = double.Parse(analisis_produccion.Rows[fila_analisis]["cantidad_producida"].ToString());
                nueva_cantidad_producida = double.Parse(funciones.obtener_dato(dato, 3));
                cantidad_producida = cantidad_producida + nueva_cantidad_producida;
                analisis_produccion.Rows[fila_analisis]["cantidad_producida"] = cantidad_producida.ToString();
                if (produccion.Rows[fila_produccion]["estado"].ToString() == "Recibido" &&
                    funciones.obtener_dato(dato, 4) != "N/A")
                {
                    cantidad_recibida = double.Parse(analisis_produccion.Rows[fila_analisis]["cantidad_recibida"].ToString());
                    nueva_cantidad_recibida = double.Parse(funciones.obtener_dato(dato, 4));
                    cantidad_recibida = cantidad_recibida + nueva_cantidad_recibida;
                    analisis_produccion.Rows[fila_analisis]["cantidad_recibida"] = cantidad_recibida.ToString();
                }
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_produccion_por_fecha(string fecha_inicio, string fecha_fin)
        {
            produccion = consultas.consultar_produccion_segun_rango_de_fecha(fecha_inicio,fecha_fin);
        }
        private void consultar_productos_produccion()
        {
            productos_produccion = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villaMaipu");
        }
        #endregion

        #region metodos get/set
        public DataTable get_analisis_produccion(string fecha_inicio, string fecha_fin)
        {
            consultar_produccion_por_fecha(fecha_inicio,fecha_fin);
            consultar_productos_produccion();
            llenar_analisis_produccion();
            return analisis_produccion;
        }
        #endregion
    }
}
