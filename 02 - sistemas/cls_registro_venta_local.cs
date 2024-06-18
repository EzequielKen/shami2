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
    public class cls_registro_venta_local
    { 
        public cls_registro_venta_local(DataTable usuario_BD)
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

        DataTable registro_venta_local;
        #endregion

        #region carga a base de datos
        public void registrar_venta(DataTable sucursal,DataTable empleado,string turno,string venta,string nota)
        {
            string columnas = string.Empty;
            string valores = string.Empty;

            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["id"].ToString(),false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(), false);
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id"].ToString(), false);
            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["nombre"].ToString(), false);
            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["apellido"].ToString(), false);
            //turno
            columnas = funciones.armar_query_columna(columnas, "turno", false);
            valores = funciones.armar_query_valores(valores, turno, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //venta
            columnas = funciones.armar_query_columna(columnas, "venta", false);
            valores = funciones.armar_query_valores(valores, venta, false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "registro_venta_local", columnas, valores);   
        }
        public void registrar_venta_franquiciado(DataTable sucursal, DateTime fecha, string turno, string venta, string nota)
        {
            string columnas = string.Empty;
            string valores = string.Empty;

            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["id"].ToString(), false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(), false);
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, "N/A", false);
            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, "Franquiciado", false);
            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(), false);
            //turno
            columnas = funciones.armar_query_columna(columnas, "turno", false);
            valores = funciones.armar_query_valores(valores, turno, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha.ToString("yyyy-MM-dd HH:mm:ss"), false);
            //venta
            columnas = funciones.armar_query_columna(columnas, "venta", false);
            valores = funciones.armar_query_valores(valores, venta, false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", true);
            valores = funciones.armar_query_valores(valores, nota, true);

            consultas.insertar_en_tabla(base_de_datos, "registro_venta_local", columnas, valores);
        }
        public void eliminar_registro(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "registro_venta_local",actualizar,id);
        }
        #endregion

        #region metodos consultas
        private void consultar_registro_venta_local(string id_sucursal,DateTime fecha_inicio,DateTime fecha_fin)
        {
            registro_venta_local=consultas.consultar_registro_venta_local_segun_fecha(id_sucursal,fecha_inicio.ToString("yyyy-MM-dd"), fecha_fin.ToString("yyyy-MM-dd"));
        }
        #endregion

        #region metodos get/set
        public DataTable get_registro_venta_local(string id_sucursal, DateTime fecha_inicio, DateTime fecha_fin)
        {
            consultar_registro_venta_local(id_sucursal,fecha_inicio,fecha_fin);
            registro_venta_local.DefaultView.Sort = "fecha desc";
            registro_venta_local = registro_venta_local.DefaultView.ToTable();
            return registro_venta_local;
        }
        #endregion
    }
}
