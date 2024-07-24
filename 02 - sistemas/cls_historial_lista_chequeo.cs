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
    public class cls_historial_lista_chequeo
    {
        public cls_historial_lista_chequeo(DataTable usuario_BD)
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

        DataTable lista_de_empleado;
        DataTable lista_de_chequeo;
        DataTable configuracion_de_chequeo;
        DataTable empleado;
        DataTable resumen;
        #endregion
        #region carga a base de datos
        public void eliminar_empleado(string id)
        {
            string actualizar = "`id_sucursal` = 'N/A'";
            consultas.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id);
        }
        #endregion
        #region metodos privados
        private void limpiar_lista_empleados(DateTime fecha)
        {
            string turno_fecha, turno_registro, id_empleado;
            DateTime fecha_registro;
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                fecha_registro = (DateTime)lista_de_empleado.Rows[fila]["fecha_logueo"];
                turno_fecha = verificar_horario_turno2(fecha);
                turno_registro = verificar_horario_turno2(fecha_registro);
                if (turno_fecha != turno_registro)
                {
                    id_empleado = lista_de_empleado.Rows[fila]["id"].ToString();
                    eliminar_empleado(id_empleado);
                }
            }
        }
        private string verificar_horario_turno2(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 08, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 16, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "rango 1";
            }
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 8:00 AM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2)
            {
                retorno = "rango 2";
            }
            DateTime horaInicio_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 00, 0, 0); // 8:00 AM
            DateTime horaFin_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 04, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango3 && miFecha <= horaFin_rango3)
            {
                retorno = "rango 3";
            }
            return retorno;
        }
        public string verificar_horario_turno(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 08, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 16, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "Turno 1";
            }
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 8:00 AM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2)
            {
                retorno = "Turno 2";
            }
            DateTime horaInicio_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 00, 0, 0); // 8:00 AM
            DateTime horaFin_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 04, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango3 && miFecha <= horaFin_rango3)
            {
                retorno = "rango 3";
            }
            return retorno;
        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("area", typeof(string));
        }
        private void llenar_resumen()
        {
            crear_tabla_resumen();
            string id, actividad, categoria, area;
            int ultima_fila;
            for (int columna = configuracion_de_chequeo.Columns["producto_1"].Ordinal; columna <= configuracion_de_chequeo.Columns.Count - 1; columna++)
            {
                if (funciones.IsNotDBNull(configuracion_de_chequeo.Rows[0][columna]))
                {

                    if (configuracion_de_chequeo.Rows[0][columna].ToString() != "N/A")
                    {
                        id = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 1);
                        actividad = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 2);
                        categoria = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 3);
                        area = funciones.obtener_dato(configuracion_de_chequeo.Rows[0][columna].ToString(), 4);

                        resumen.Rows.Add();
                        ultima_fila = resumen.Rows.Count - 1;

                        resumen.Rows[ultima_fila]["id"] = id;
                        resumen.Rows[ultima_fila]["actividad"] = actividad;
                        resumen.Rows[ultima_fila]["categoria"] = categoria;
                        resumen.Rows[ultima_fila]["area"] = area;
                    }
                }

            }
        }
        #endregion
        #region metodos consultas
        private void consultar_empleado(string id_empleado)
        {
            empleado = consultas.consultar_empleado(id_empleado);
        }
        private void consultar_lista_de_empleado(string id_sucursal, DateTime fecha)
        {
            lista_de_empleado = consultas.consultar_empleados_segun_fecha(id_sucursal, fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString());
        }
        private void consultar_configuracion_de_chequeo(string perfil)
        {
            configuracion_de_chequeo = consultas.consultar_configuracion_chequeo(perfil);
        }
        private void consultar_lista_de_chequeo()
        {
            lista_de_chequeo = consultas.consultar_tabla_completa(base_de_datos, "lista_de_chequeo");
        }
        #endregion
        #region metodos get/set
        public DataTable get_empleado(string id_empleado)
        {
            consultar_empleado(id_empleado);
            return empleado;
        }
        public DataTable get_lista_de_empleado(string id_sucursal, DateTime fecha)
        {
            consultar_lista_de_empleado(id_sucursal, fecha);
            //  limpiar_lista_empleados(fecha);
            return lista_de_empleado;
        }
        public DataTable get_configuracion_de_chequeo(string perfil)
        {
            consultar_configuracion_de_chequeo(perfil);
            llenar_resumen();
            return resumen;
        }
        public DataTable get_lista_de_chequeo()
        {
            consultar_lista_de_chequeo();
            return lista_de_chequeo;
        }
        #endregion
    }
}
