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
    public class cls_planificador_de_horarios
    {
        public cls_planificador_de_horarios(DataTable usuario_BD)
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
        cls_sistema_cuentas_por_pagar cuentas_por_pagar;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable lista_de_empleado;
        DataTable horarios_de_empleados;
        DataTable horarios_de_empleado;
        #endregion

        #region carga a base de datos
        public void modificar_horario_empleado(string id_horario, string hora, string condicion)
        {
            string actualizar = "";
            if (condicion == "entrada")
            {
                actualizar = "`horario_entrada` = '" + hora + "'";
            }
            else if (condicion == "salida")
            {
                actualizar = "`horario_salida` = '" + hora + "'";

            }
            else if (condicion == "franco")
            {
                actualizar = "`franco` = 'Si'";

            }
            else if (condicion == "franco no")
            {
                actualizar = "`franco` = 'N/A'";

            }
            consultas.actualizar_tabla(base_de_datos, "horarios_de_empleados", actualizar, id_horario);
        }
        public void insertar_horario_empleado(DataTable sucursal, DataTable lista_empleado, int fila_empleado, DateTime fecha, string horario_entrada, string horario_salida, string franco)
        {
            string columnas = "";
            string valores = "";
            //fecha_registro
            columnas = funciones.armar_query_columna(columnas, "fecha_registro", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["id"].ToString(), false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(), false);
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, lista_empleado.Rows[fila_empleado]["id"].ToString(), false);
            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, lista_empleado.Rows[fila_empleado]["nombre"].ToString(), false);
            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, lista_empleado.Rows[fila_empleado]["apellido"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha.ToString("yyyy-MM-dd"), false);
            //dia
            columnas = funciones.armar_query_columna(columnas, "dia", false);
            valores = funciones.armar_query_valores(valores, fecha.DayOfWeek.ToString(), false);
            //horario_entrada
            if (horario_entrada != "N/A" || franco=="Si")
            {
                columnas = funciones.armar_query_columna(columnas, "horario_entrada", false);
                valores = funciones.armar_query_valores(valores, horario_entrada, false);
            }
            //horario_salida
            if (horario_salida != "N/A" || franco == "Si")
            {
                columnas = funciones.armar_query_columna(columnas, "horario_salida", false);
                valores = funciones.armar_query_valores(valores, horario_salida, false);
            }
            //franco
            columnas = funciones.armar_query_columna(columnas, "franco", true);
            valores = funciones.armar_query_valores(valores, franco, true);
            consultas.insertar_en_tabla(base_de_datos, "horarios_de_empleados", columnas, valores);
        }

        #endregion

        #region metodos consultas
        private void consultar_horarios_de_empleados(string id_sucursal, DateTime fecha_inicio, DateTime fecha_fin)
        {
            horarios_de_empleados = consultas.consultar_horarios_de_empleados_segun_fecha(id_sucursal, fecha_inicio.ToString("yyyy-MM-dd"), fecha_fin.ToString("yyyy-MM-dd"));
        }
        private void consultar_horarios_de_empleado(string id_sucursal, string id_empleado, DateTime fecha_inicio, DateTime fecha_fin)
        {
            horarios_de_empleado = consultas.consultar_horarios_de_empleado_segun_fecha(id_sucursal, id_empleado, fecha_inicio.ToString("yyyy-MM-dd"), fecha_fin.ToString("yyyy-MM-dd"));
        }
        private void consultar_lista_de_empleado(string id_sucursal)
        {
            lista_de_empleado = consultas.consultar_empleados(id_sucursal);
        }
        #endregion

        #region metodos get/set
        public DataTable get_lista_de_empleado(string id_sucursal)
        {
            consultar_lista_de_empleado(id_sucursal);
            lista_de_empleado.Columns.Add("cargo_original", typeof(string));
            lista_de_empleado.Columns.Add("nombre_completo", typeof(string));
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                lista_de_empleado.Rows[fila]["nombre_completo"] = lista_de_empleado.Rows[fila]["nombre"].ToString() + " " + lista_de_empleado.Rows[fila]["apellido"].ToString();
                lista_de_empleado.Rows[fila]["cargo_original"] = funciones.obtener_dato(lista_de_empleado.Rows[fila]["cargo"].ToString(), 2);
            }
            return lista_de_empleado;
        }
        public DataTable get_horarios_de_empleados(string id_sucursal, DateTime fecha_inicio, DateTime fecha_fin)
        {
            consultar_horarios_de_empleados(id_sucursal, fecha_inicio, fecha_fin);
            return horarios_de_empleados;
        }
        public DataTable get_horarios_de_empleado(string id_sucursal, string id_empleado, DateTime fecha_inicio, DateTime fecha_fin)
        {
            consultar_horarios_de_empleado(id_sucursal, id_empleado, fecha_inicio, fecha_fin);
            return horarios_de_empleado;
        }
        #endregion
    }
}
