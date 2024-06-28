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
    public class cls_administrar_tabla_produccion
    {
        public cls_administrar_tabla_produccion(DataTable usuario_BD)
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

        DataTable insumos_fabrica;
        DataTable productos_terminados;
        DataTable resumen;
        DataTable ventas;
        #endregion

        #region carga a base de datos
        public void cargar_tabla_produccion(DataTable sucursal,string dias,string porcentaje_turno_1, string porcentaje_turno_2, DataTable resumen)
        {
            string columnas= string.Empty;
            string valores = string.Empty;
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["id"].ToString(),false);

            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["sucursal"].ToString(), false);

            //dias
            columnas = funciones.armar_query_columna(columnas, "dias", false);
            valores = funciones.armar_query_valores(valores, dias, false);

            //porcentaje_turno_1
            columnas = funciones.armar_query_columna(columnas, "porcentaje_turno_1", false);
            valores = funciones.armar_query_valores(valores, porcentaje_turno_1, false);

            //porcentaje_turno_2
            columnas = funciones.armar_query_columna(columnas, "porcentaje_turno_2", false);
            valores = funciones.armar_query_valores(valores, porcentaje_turno_2, false);

            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);

            //producto_1
            string id,producto,venta_alta,venta_baja,dato;
            int index=1;
            for (int fila = 0; fila < resumen.Rows.Count-1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                producto = resumen.Rows[fila]["producto"].ToString();
                venta_alta = resumen.Rows[fila]["venta_alta"].ToString();
                venta_baja = resumen.Rows[fila]["venta_baja"].ToString();

                dato = id + "-" + producto + "-" + venta_alta + "-" + venta_baja;

                columnas = funciones.armar_query_columna(columnas,"producto_"+index.ToString(),false);
                valores = funciones.armar_query_valores(valores,dato,false);
                index++;
            }
            int ultima_fila = resumen.Rows.Count-1;

            id = resumen.Rows[ultima_fila]["id"].ToString();
            producto = resumen.Rows[ultima_fila]["producto"].ToString();
            venta_alta = resumen.Rows[ultima_fila]["venta_alta"].ToString();
            venta_baja = resumen.Rows[ultima_fila]["venta_baja"].ToString();

            dato = id + "-" + producto + "-" + venta_alta + "-" + venta_baja;

            columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);

           consultas.insertar_en_tabla(base_de_datos, "tabla_produccion",columnas,valores);
        }
        #endregion
        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id",typeof(string));
            resumen.Columns.Add("producto",typeof(string));
            resumen.Columns.Add("tipo_producto_local", typeof(string));
            resumen.Columns.Add("venta_alta",typeof(string));
            resumen.Columns.Add("venta_baja",typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            int ultima_fila;
            for (int fila = 0; fila <=insumos_fabrica.Rows.Count-1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count-1;

                resumen.Rows[ultima_fila]["id"] = insumos_fabrica.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = insumos_fabrica.Rows[fila]["producto"].ToString() + " " + insumos_fabrica.Rows[fila]["unidad_tabla_produccion"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto_local"] = insumos_fabrica.Rows[fila]["tipo_producto_local"].ToString();

                resumen.Rows[ultima_fila]["venta_alta"] = "0";
                resumen.Rows[ultima_fila]["venta_baja"] = "0";

            }

            for (int fila = 0; fila <= productos_terminados.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;

                resumen.Rows[ultima_fila]["id"] = productos_terminados.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = productos_terminados.Rows[fila]["producto"].ToString() + " " + productos_terminados.Rows[fila]["unidad_tabla_produccion"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto_local"] = productos_terminados.Rows[fila]["tipo_producto_local"].ToString();

                resumen.Rows[ultima_fila]["venta_alta"] = "0";
                resumen.Rows[ultima_fila]["venta_baja"] = "0";

            }
        }
        #endregion
        #region consultas
        private void consultar_ventas(string id_sucursal)
        {
            DateTime fecha_hoy = DateTime.Now;
            DateTime fecha_anterior = fecha_hoy.AddDays(-7);
            ventas = consultas.consultar_registro_venta_local_segun_fecha(id_sucursal,fecha_anterior.ToString("yyyy-MM-dd"),fecha_hoy.ToString("yyyy-MM-dd"));
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_insumos_tabla_produccion();
        }
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_productos_terminado_tabla_produccion();
        }
        #endregion

        #region metodos get/set
        public DataTable get_ventas(string id_sucursal)
        {
            consultar_ventas(id_sucursal);
            return ventas;
        }
        public DataTable get_lista_productos()
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            llenar_tabla_resumen();
            resumen.DefaultView.Sort = "tipo_producto_local asc";
            resumen = resumen.DefaultView.ToTable();
            return resumen;
        }
        #endregion
    }
}
