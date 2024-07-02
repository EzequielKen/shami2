using modulos;
using paginaWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_tabla_produccion
    {
        public cls_tabla_produccion(DataTable usuario_BD)
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
        DataTable historial_tabla_produccion;
        DataTable tabla_produccion;
        DataTable resumen;
        DataTable ventas;
        #endregion

        #region carga base de datos
        public void modificar_registro(string id, string campo, string dato)
        {
            string actualizar = "`"+campo+"` = '"+dato+"'";
            consultas.actualizar_tabla(base_de_datos, "historial_tabla_produccion",actualizar,id);
        }
        public void cargar_registro_tabla_produccion(DataTable sucursal, DataTable empleado, DataTable resumen, string id_tabla_produccion, string turno)
        {
            string fecha = funciones.get_fecha();
            string columnas = string.Empty;
            string valores = string.Empty;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {

                //id_sucursal
                columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
                valores = funciones.armar_query_valores(valores, sucursal.Rows[0]["id"].ToString(), false);
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

                //id_tabla_produccion
                columnas = funciones.armar_query_columna(columnas, "id_tabla_produccion", false);
                valores = funciones.armar_query_valores(valores, id_tabla_produccion, false);

                //fecha
                columnas = funciones.armar_query_columna(columnas, "fecha", false);
                valores = funciones.armar_query_valores(valores, fecha, false);

                //turno
                columnas = funciones.armar_query_columna(columnas, "turno", false);
                valores = funciones.armar_query_valores(valores, turno, false);


                //id_producto
                columnas = funciones.armar_query_columna(columnas, "id_producto", false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["id"].ToString(), false);

                //producto
                columnas = funciones.armar_query_columna(columnas, "producto", false);
                valores = funciones.armar_query_valores(valores, resumen.Rows[fila]["producto"].ToString(), false);

                //stock
                columnas = funciones.armar_query_columna(columnas, "stock", false);
                valores = funciones.armar_query_valores(valores, "N/A", false);

                //produccion
                columnas = funciones.armar_query_columna(columnas, "produccion", true);
                valores = funciones.armar_query_valores(valores, "N/A", true);

                consultas.insertar_en_tabla(base_de_datos, "historial_tabla_produccion", columnas, valores);
                columnas = string.Empty;
                valores = string.Empty;
            }

        }
        #endregion

        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("id_historial", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto_local", typeof(string));

            resumen.Columns.Add("venta_alta_turno1", typeof(string));
            resumen.Columns.Add("venta_baja_turno1", typeof(string));
            resumen.Columns.Add("venta_alta_turno2", typeof(string));
            resumen.Columns.Add("venta_baja_turno2", typeof(string));

            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("produccion", typeof(string));
            resumen.Columns.Add("objetivo_produccion", typeof(string));
            resumen.Columns.Add("crear_nuevo_registro", typeof(bool));
        }
        private void llenar_tabla_resumen(bool crear_nuevo_registro)
        {
            crear_tabla_resumen();
            int ultima_fila;
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;

                resumen.Rows[ultima_fila]["id"] = insumos_fabrica.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = insumos_fabrica.Rows[fila]["producto"].ToString() + " " + insumos_fabrica.Rows[fila]["unidad_tabla_produccion"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto_local"] = insumos_fabrica.Rows[fila]["tipo_producto_local"].ToString();

                resumen.Rows[ultima_fila]["stock"] = "N/A";
                resumen.Rows[ultima_fila]["produccion"] = "N/A";
                resumen.Rows[ultima_fila]["objetivo_produccion"] = "N/A"; 
                resumen.Rows[ultima_fila]["crear_nuevo_registro"] = crear_nuevo_registro; 
            }

            for (int fila = 0; fila <= productos_terminados.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;

                resumen.Rows[ultima_fila]["id"] = productos_terminados.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = productos_terminados.Rows[fila]["producto"].ToString() + " " + productos_terminados.Rows[fila]["unidad_tabla_produccion"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto_local"] = productos_terminados.Rows[fila]["tipo_producto_local"].ToString();

                resumen.Rows[ultima_fila]["stock"] = "N/A";
                resumen.Rows[ultima_fila]["produccion"] = "N/A";
                resumen.Rows[ultima_fila]["objetivo_produccion"] = "N/A";
                resumen.Rows[ultima_fila]["crear_nuevo_registro"] = crear_nuevo_registro;
            }
            string id_producto;
            int fila_producto;
            for (int fila = 0; fila <= historial_tabla_produccion.Rows.Count - 1; fila++)
            {
                id_producto = historial_tabla_produccion.Rows[fila]["id_producto"].ToString();
                fila_producto = funciones.buscar_fila_por_id(id_producto,resumen);
                resumen.Rows[fila_producto]["id_historial"] = historial_tabla_produccion.Rows[fila]["id"].ToString();
                resumen.Rows[fila_producto]["stock"] = historial_tabla_produccion.Rows[fila]["stock"].ToString();
                resumen.Rows[fila_producto]["produccion"] = historial_tabla_produccion.Rows[fila]["produccion"].ToString();
            }

        }
        private void configurar_tabla_produccion()
        {
            int dias_venta_alta = 0;
            int dias_venta_baja = 0;
            string dias = tabla_produccion.Rows[0]["dias"].ToString();
            bool estado;
            for (int dia = 1; dia <= 7; dia++)
            {
                estado = bool.Parse(funciones.obtener_dato(dias, dia));
                if (estado)
                {
                    dias_venta_alta++;
                }
                else
                {
                    dias_venta_baja++;
                }
            }
            int fila;
            double venta_baja, venta_alta, venta_baja_turno1, venta_alta_turno1, venta_baja_turno2, venta_alta_turno2;
            double porcentaje_turno_1 = double.Parse(tabla_produccion.Rows[0]["porcentaje_turno_1"].ToString());
            double porcentaje_turno_2 = double.Parse(tabla_produccion.Rows[0]["porcentaje_turno_2"].ToString());
            string id, producto;
            for (int columna = tabla_produccion.Columns["producto_1"].Ordinal; columna <= tabla_produccion.Columns.Count - 1; columna++)
            {
                if (tabla_produccion.Rows[0][columna].ToString() != "N/A")
                {
                    id = funciones.obtener_dato(tabla_produccion.Rows[0][columna].ToString(), 1);
                    producto = funciones.obtener_dato(tabla_produccion.Rows[0][columna].ToString(), 2);
                    fila = funciones.buscar_fila_por_id_nombre(id, producto, resumen);
                    if (fila != -1)
                    {
                        venta_alta = double.Parse(funciones.obtener_dato(tabla_produccion.Rows[0][columna].ToString(), 3));
                        venta_baja = double.Parse(funciones.obtener_dato(tabla_produccion.Rows[0][columna].ToString(), 4));

                        venta_alta = venta_alta / dias_venta_alta;
                        venta_baja = venta_baja / dias_venta_baja;

                        venta_alta_turno1 = Math.Ceiling((venta_alta * porcentaje_turno_1) / 100);
                        venta_baja_turno1 = Math.Ceiling((venta_baja * porcentaje_turno_1) / 100);

                        venta_alta_turno2 = Math.Ceiling((venta_alta * porcentaje_turno_2) / 100);
                        venta_baja_turno2 = Math.Ceiling((venta_baja * porcentaje_turno_2) / 100);

                        resumen.Rows[fila]["venta_alta_turno1"] = venta_alta_turno1.ToString();
                        resumen.Rows[fila]["venta_baja_turno1"] = venta_baja_turno1.ToString();

                        resumen.Rows[fila]["venta_alta_turno2"] = venta_alta_turno2.ToString();
                        resumen.Rows[fila]["venta_baja_turno2"] = venta_baja_turno2.ToString();

                    }
                }
            }
        }
        static string ObtenerDiaDeLaSemanaEnEspanol(DateTime fecha)
        {
            var diasDeLaSemana = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday, "1-Lunes" },
            { DayOfWeek.Tuesday, "2-Martes" },
            { DayOfWeek.Wednesday, "3-Miercoles" },
            { DayOfWeek.Thursday, "4-Jueves" },
            { DayOfWeek.Friday, "5-Viernes" },
            { DayOfWeek.Saturday, "6-Sabado" },
            { DayOfWeek.Sunday, "7-Domingo" }
        };

            return diasDeLaSemana[fecha.DayOfWeek];
        }
        #endregion
        #region consultas
        private void consultar_ventas(string id_sucursal)
        {
            DateTime fecha_hoy = DateTime.Now;
            DateTime fecha_anterior = fecha_hoy.AddDays(-7);
            ventas = consultas.consultar_registro_venta_local_segun_fecha(id_sucursal, fecha_anterior.ToString("yyyy-MM-dd"), fecha_hoy.ToString("yyyy-MM-dd"));
        }
        private void consultar_tabla_produccion(string id_sucursal)
        {
            tabla_produccion = consultas.consultar_tabla_produccion(id_sucursal);
            tabla_produccion.DefaultView.Sort = "fecha DESC";
            tabla_produccion = tabla_produccion.DefaultView.ToTable();
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_insumos_tabla_produccion();
        }
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_productos_terminado_tabla_produccion();
        }
        private void consultar_historial_tabla_produccion(string id_sucursal, string id_empleado, string id_tabla_produccion, string turno)
        {
            historial_tabla_produccion = consultas.consultar_historial_tabla_produccion(id_sucursal, id_empleado, id_tabla_produccion, turno);
        }
        #endregion

        #region metodos get/set
        public bool get_dia_alto_o_bajo()
        {
            DateTime fecha = DateTime.Now;
            string dia_de_hoy = ObtenerDiaDeLaSemanaEnEspanol(fecha);
            string dias = tabla_produccion.Rows[0]["dias"].ToString();
            bool estado;
            int dia = int.Parse(funciones.obtener_dato(dia_de_hoy, 1));
            estado = bool.Parse(funciones.obtener_dato(dias, dia));


            return estado;
        }
        public DataTable get_ventas(string id_sucursal)
        {
            consultar_ventas(id_sucursal);
            return ventas;
        }
        public DataTable get_lista_productos(DataTable sucursal, DataTable empleado, string turno)
        {
            string id_sucursal = sucursal.Rows[0]["id"].ToString();
            string id_empleado = empleado.Rows[0]["id"].ToString();
            bool crear_nuevo_registro;
            consultar_tabla_produccion(id_sucursal);
            crear_tabla_resumen();
            if (tabla_produccion.Rows.Count > 0)
            {
                string id_tabla_produccion = tabla_produccion.Rows[0]["id"].ToString();
                consultar_historial_tabla_produccion(id_sucursal, id_empleado, id_tabla_produccion, turno);
                if (historial_tabla_produccion.Rows.Count>0)
                {
                    crear_nuevo_registro = false;
                }
                else
                {
                    crear_nuevo_registro =true;
                }
                consultar_insumos_fabrica();
                consultar_productos_terminados();
                llenar_tabla_resumen(crear_nuevo_registro);
                configurar_tabla_produccion();
                resumen.DefaultView.Sort = "tipo_producto_local asc";
                resumen = resumen.DefaultView.ToTable();
            }

            return resumen;
        }
        public string get_id_tabla_produccion(string id_sucursal)
        {
            consultar_tabla_produccion(id_sucursal);
            return tabla_produccion.Rows[0]["id"].ToString();
        }
        #endregion
    }
}
