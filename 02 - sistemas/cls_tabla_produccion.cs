using modulos;
using paginaWeb;
using System;
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
        DataTable tabla_produccion;
        DataTable resumen;
        DataTable ventas;
        #endregion

        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto_local", typeof(string));

            resumen.Columns.Add("venta_alta_turno1", typeof(string));
            resumen.Columns.Add("venta_baja_turno1", typeof(string));
            resumen.Columns.Add("venta_alta_turno2", typeof(string));
            resumen.Columns.Add("venta_baja_turno2", typeof(string));

            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("produccion", typeof(string));
            resumen.Columns.Add("objetivo_produccion", typeof(string));
        }
        private void llenar_tabla_resumen()
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
        #endregion

        #region metodos get/set
        public bool get_dia_alto_o_bajo()
        {
            DateTime fecha = DateTime.Now;
            string dia_de_hoy = ObtenerDiaDeLaSemanaEnEspanol(fecha);
            string dias = tabla_produccion.Rows[0]["dias"].ToString();
            bool estado;
            int dia = int.Parse(funciones.obtener_dato(dia_de_hoy,1));
            estado = bool.Parse(funciones.obtener_dato(dias, dia));

          
            return estado;
        }
        public DataTable get_ventas(string id_sucursal)
        {
            consultar_ventas(id_sucursal);
            return ventas;
        }
        public DataTable get_lista_productos(string id_sucursal)
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            consultar_tabla_produccion(id_sucursal);
            llenar_tabla_resumen();
            configurar_tabla_produccion();
            resumen.DefaultView.Sort = "tipo_producto_local asc";
            resumen = resumen.DefaultView.ToTable();
            return resumen;
        }
        #endregion
    }
}
