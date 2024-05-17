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
    public class cls_crear_sub_producto
    {
        public cls_crear_sub_producto(DataTable usuario_BD)
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
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable proveedor_fabrica_seleccionado;
        DataTable insumos;
        #endregion

        #region carga en base de datos
        public void agregar_sub_producto(DataTable resumen, string tipo_receta, string nombre_sub_producto)
        {
            string id, producto, cantidad, unidad_medida, dato;
            string columna = "";
            string valores = "";
            //sub_producto
            columna = funciones.armar_query_columna(columna, "sub_producto", false);
            valores = funciones.armar_query_valores(valores, nombre_sub_producto, false);
            //tipo_sub_producto
            columna = funciones.armar_query_columna(columna, "tipo_sub_producto", false);
            valores = funciones.armar_query_valores(valores, tipo_receta, false);
            //producto_1
            int index = 1;
            for (int fila = 0; fila < resumen.Rows.Count-1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                producto = resumen.Rows[fila]["producto"].ToString();
                cantidad = resumen.Rows[fila]["cantidad"].ToString();
                unidad_medida = resumen.Rows[fila]["unidad_medida"].ToString();
                dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida;
                columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);
                index++;
            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            producto = resumen.Rows[ultima_fila]["producto"].ToString();
            cantidad = resumen.Rows[ultima_fila]["cantidad"].ToString();
            unidad_medida = resumen.Rows[ultima_fila]["unidad_medida"].ToString();
            dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida;
            columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);
            consultas.insertar_en_tabla(base_de_datos, "sub_producto",columna,valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos()
        {
            insumos = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }

        #endregion

        #region metodos get/set
        public DataTable get_insumos()
        {
            consultar_insumos();
            return insumos;
        }
        #endregion

    }
}
