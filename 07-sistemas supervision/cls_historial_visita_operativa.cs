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

namespace _07_sistemas_supervision
{
    public class cls_historial_visita_operativa
    {
        public cls_historial_visita_operativa(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable historial_evaluacion_chequeo;
        DataTable lista_de_chequeo;
        DataTable configuracion_de_chequeo;
        DataTable empleado;
        DataTable resumen;
        #endregion

        #region PDF
        public void crear_pdf_evaluacion(string ruta_archivo, byte[] logo, DataTable lista_de_evaluados, DataTable historial_evaluacion_chequeo, string sucursal, string fecha, string evaluacion_local)
        {
            PDF.GenerarPDF_evaluacion_de_chequeo(ruta_archivo, logo, lista_de_evaluados, historial_evaluacion_chequeo, sucursal, fecha, evaluacion_local);
        }
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
            for (int fila = 0; fila <= historial_evaluacion_chequeo.Rows.Count - 1; fila++)
            {
                fecha_registro = (DateTime)historial_evaluacion_chequeo.Rows[fila]["fecha_logueo"];
                turno_fecha = verificar_horario_turno2(fecha);
                turno_registro = verificar_horario_turno2(fecha_registro);
                if (turno_fecha != turno_registro)
                {
                    id_empleado = historial_evaluacion_chequeo.Rows[fila]["id"].ToString();
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
            resumen.Columns.Add("id_sucursal", typeof(string));
            resumen.Columns.Add("id_empleado", typeof(string));
            resumen.Columns.Add("nombre", typeof(string));
            resumen.Columns.Add("apellido", typeof(string));
            resumen.Columns.Add("cargo", typeof(string));
            resumen.Columns.Add("fecha", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
            resumen.Columns.Add("punto_teorico", typeof(string));
            resumen.Columns.Add("punto_real", typeof(string));
        }
        private void llenar_resumen(DataTable empleado_historial, string perfil)
        {
            crear_tabla_resumen();
            string id_empleado = empleado_historial.Rows[0]["id"].ToString();
            string cargo;
            int ultima_fila;
            for (int fila = 0; fila <= historial_evaluacion_chequeo.Rows.Count - 1; fila++)
            {
                cargo = funciones.obtener_dato(historial_evaluacion_chequeo.Rows[fila]["cargo"].ToString(), 2);
                if (historial_evaluacion_chequeo.Rows[fila]["id_empleado"].ToString() == id_empleado &&
                    cargo == perfil)
                {
                    resumen.Rows.Add();
                    ultima_fila = resumen.Rows.Count - 1;
                    resumen.Rows[ultima_fila]["id"] = historial_evaluacion_chequeo.Rows[fila]["id"].ToString();
                    resumen.Rows[ultima_fila]["id_sucursal"] = historial_evaluacion_chequeo.Rows[fila]["id_sucursal"].ToString();
                    resumen.Rows[ultima_fila]["id_empleado"] = historial_evaluacion_chequeo.Rows[fila]["id_empleado"].ToString();
                    resumen.Rows[ultima_fila]["nombre"] = historial_evaluacion_chequeo.Rows[fila]["nombre"].ToString();
                    resumen.Rows[ultima_fila]["apellido"] = historial_evaluacion_chequeo.Rows[fila]["apellido"].ToString();
                    resumen.Rows[ultima_fila]["cargo"] = historial_evaluacion_chequeo.Rows[fila]["cargo"].ToString();
                    resumen.Rows[ultima_fila]["fecha"] = historial_evaluacion_chequeo.Rows[fila]["fecha"].ToString();
                    resumen.Rows[ultima_fila]["actividad"] = historial_evaluacion_chequeo.Rows[fila]["actividad"].ToString();
                    resumen.Rows[ultima_fila]["nota"] = historial_evaluacion_chequeo.Rows[fila]["nota"].ToString();
                    resumen.Rows[ultima_fila]["punto_teorico"] = historial_evaluacion_chequeo.Rows[fila]["punto_teorico"].ToString();
                    resumen.Rows[ultima_fila]["punto_real"] = historial_evaluacion_chequeo.Rows[fila]["punto_real"].ToString();
                }
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_empleado(string id_empleado)
        {
            empleado = consultas.consultar_empleado(id_empleado);
        }
        private void consultar_lista_de_evaluados(string id_sucursal, DateTime fecha)
        {
            historial_evaluacion_chequeo = consultas.consultar_historial_visita_operativa_segun_fecha(id_sucursal, fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString());
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
        public DataTable get_lista_de_evaluados(string id_sucursal, DateTime fecha)
        {
            consultar_lista_de_evaluados(id_sucursal, fecha);
            historial_evaluacion_chequeo.DefaultView.Sort = "cargo ASC,orden ASC";
            historial_evaluacion_chequeo = historial_evaluacion_chequeo.DefaultView.ToTable();
            //  limpiar_lista_empleados(fecha);
            return historial_evaluacion_chequeo;
        }
        public DataTable get_configuracion_de_chequeo(string perfil, DataTable empleado_historial, DataTable historial)
        {
            consultar_lista_de_chequeo();
            historial_evaluacion_chequeo = historial;
            llenar_resumen(empleado_historial, perfil);
            return resumen;
        }
        public DataTable get_lista_de_chequeo()
        {
            consultar_lista_de_chequeo();
            return lista_de_chequeo;
        }
        public string get_cargos_de_empleado(DataTable empleado_seleccionado, DataTable historial_del_dia)
        {
            DataTable cargos = new DataTable();
            cargos.Columns.Add("cargo", typeof(string));
            string retorno = "";
            string cargo;
            string id_empleado = empleado_seleccionado.Rows[0]["id"].ToString();
            int cant_cargos = 0;
            historial_del_dia.DefaultView.Sort = "id_empleado ASC";
            historial_del_dia = historial_del_dia.DefaultView.ToTable();
            for (int fila = 0; fila <= historial_del_dia.Rows.Count - 1; fila++)
            {
                if (id_empleado == historial_del_dia.Rows[fila]["id_empleado"].ToString())
                {
                    cargo = funciones.obtener_dato(historial_del_dia.Rows[fila]["cargo"].ToString(), 2);
                    if (!funciones.verificar_si_cargo_dato(cargo, "cargo", cargos))
                    {
                        cargos.Rows.Add();
                        cargos.Rows[cargos.Rows.Count - 1]["cargo"] = cargo;
                    }
                }
            }
            cant_cargos = cargos.Rows.Count;
            retorno = cant_cargos.ToString();
            for (int fil = 0; fil <= cargos.Rows.Count - 1; fil++)
            {
                retorno += "-" + cargos.Rows[fil]["cargo"].ToString();
            }
            return retorno;
        }
        public string get_promedio_de_puntaje(string id_empleado, DataTable historial_del_dia)
        {
            double promedio = 0;
            List<double> puntajes = new List<double>();
            double puntos = 0;
            double total_puntos = 0;
            empleado = get_empleado(id_empleado);
            string cargos = get_cargos_de_empleado(empleado, historial_del_dia);
            string cargo;
            int cant_cargos = Convert.ToInt32(funciones.obtener_dato(cargos, 1));
            for (int i = 1; i <= cant_cargos; i++)
            {
                cargo = funciones.obtener_dato(cargos, i + 1);
                puntos = 0;
                for (int fila = 0; fila <= historial_del_dia.Rows.Count - 1; fila++)
                {
                    if (id_empleado == historial_del_dia.Rows[fila]["id_empleado"].ToString())
                    {
                        if (cargo == funciones.obtener_dato(historial_del_dia.Rows[fila]["cargo"].ToString(), 2))
                        {
                            puntos += Convert.ToDouble(historial_del_dia.Rows[fila]["punto_real"].ToString());
                        }
                    }
                }
                puntajes.Add(puntos);
            }
            for (int i = 0; i <= puntajes.Count - 1; i++)
            {
                total_puntos += puntajes[i];
            }
            promedio = total_puntos / double.Parse(puntajes.Count.ToString());
            return promedio.ToString();
        }
        public string get_total_actividades_evaluadas(DataTable actividades_evaluadas)
        {
            double punto_real = 0;
            for (int fila = 0; fila <= actividades_evaluadas.Rows.Count - 1; fila++)
            {
                punto_real += double.Parse(actividades_evaluadas.Rows[fila]["punto_real"].ToString());
            }
            return punto_real.ToString();
        }
        #endregion
    }
}
