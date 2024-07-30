using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_registro_comida_empleado
    {
        public cls_registro_comida_empleado(DataTable usuario_BD)
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

        #region carga a base de datos
        public void registrar_consumo(string id_sucursal,string id_empleado, string nombre, string apellido, DataTable resumen)
        {
            string columnas;
            string valores;
            string fecha = funciones.get_fecha();
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {
                columnas = string.Empty;
                valores = string.Empty;
                //id_empleado
                columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
                valores = funciones.armar_query_valores(valores, id_empleado, false);
                //id_sucursal
                columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
                valores = funciones.armar_query_valores(valores, id_sucursal, false);
                //nombre
                columnas = funciones.armar_query_columna(columnas, "nombre", false);
                valores = funciones.armar_query_valores(valores, nombre, false);
                //apellido
                columnas = funciones.armar_query_columna(columnas, "apellido", false);
                valores = funciones.armar_query_valores(valores, apellido, false);
                //fecha
                columnas = funciones.armar_query_columna(columnas, "fecha", false);
                valores = funciones.armar_query_valores(valores, fecha, false);
                //id_producto
                columnas = funciones.armar_query_columna(columnas, "id_producto", false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["id"].ToString(), false);
                //producto
                columnas = funciones.armar_query_columna(columnas, "producto", false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["producto"].ToString(), false);
                //cantidad
                columnas = funciones.armar_query_columna(columnas, "cantidad", false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["cantidad"].ToString(), false);
                //costo
                columnas = funciones.armar_query_columna(columnas, "costo", true);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["costo"].ToString(), true);

                consultas.insertar_en_tabla(base_de_datos, "historial_consumo_personal", columnas, valores);
            }
        }
        #endregion

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable lista_productos_comida_empleados;
        #endregion

        #region metodos consulta
        private void consultar_lista_productos_comida_empleados()
        {
            lista_productos_comida_empleados = consultas.consultar_tabla(base_de_datos, "lista_productos_comida_empleados");
        }
        #endregion

        #region metodos get/set
        public DataTable get_lista_productos_comida_empleados()
        {
            consultar_lista_productos_comida_empleados();
            return lista_productos_comida_empleados;
        }
        #endregion
    }
}
