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

namespace _02___sistemas
{
    public class cls_lista_de_chequeo
    {
        public cls_lista_de_chequeo(DataTable usuario_BD)
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
            login = new cls_sistema_login();
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        cls_sistema_login login;

        DataTable lista_de_chequeo;
        DataTable configuracion_de_chequeo;
        DataTable resumen;
        DataTable historial;
        DataTable historial_resumen;
        DataTable empleado;
        #endregion

        #region PDF
        public void crear_pdf(string ruta_archivo, byte[] logo, DataTable lista)
        {
            DataTable categoria = new DataTable();
            categoria.Columns.Add("id", typeof(string));
            categoria.Columns.Add("categoria", typeof(string)); 
            categoria.Columns.Add("area", typeof(string)); 
            lista.DefaultView.Sort = "categoria ASC, orden ASC";
            lista = lista.DefaultView.ToTable();
            string categoria_dato;
            for (int fila = 0; fila <= lista.Rows.Count - 1; fila++)
            {
                categoria_dato = lista.Rows[fila]["categoria"].ToString();
                if (!funciones.verificar_si_cargo_dato(categoria_dato, "categoria", categoria))
                {
                    categoria.Rows.Add();
                    categoria.Rows[categoria.Rows.Count - 1]["id"] = categoria_dato;
                    categoria.Rows[categoria.Rows.Count - 1]["categoria"] = lista.Rows[fila]["categoria"].ToString(); 
                    categoria.Rows[categoria.Rows.Count - 1]["area"] = lista.Rows[fila]["area"].ToString(); 
                }
            }
            PDF.GenerarPDF_lista_de_chequeo(ruta_archivo, logo, categoria, lista);
        }

        public void crear_pdf_segun_categoria(string ruta_archivo, byte[] logo, DataTable lista,string categoria_dato,string area)
        {
            DataTable categoria = new DataTable();
            categoria.Columns.Add("categoria", typeof(string)); 
            categoria.Columns.Add("area", typeof(string)); 
            lista.DefaultView.Sort = "categoria ASC, orden ASC"; 
            lista = lista.DefaultView.ToTable();
            categoria.Rows.Add();
            categoria.Rows[categoria.Rows.Count - 1]["categoria"] = categoria_dato; 
            categoria.Rows[categoria.Rows.Count - 1]["area"] = area; 
            PDF.GenerarPDF_lista_de_chequeo(ruta_archivo, logo, categoria, lista);
        }
        #endregion

        #region carga a base de datos
        public DataTable cerrar_turno(DataTable empleado, string sucursal)
        {
            login.cerrar_turno(empleado);
            login.login_empleado(sucursal, empleado.Rows[0]["dni"].ToString());
            return login.get_empleado();
        }
        public void registrar_chequeo(DataTable empleado, string actividad, string nota, string turno)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id_sucursal"].ToString(), false);
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id"].ToString(), false);
            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["nombre"].ToString(), false);
            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["apellido"].ToString(), false);
            //cargo
            columnas = funciones.armar_query_columna(columnas, "cargo", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["cargo"].ToString(), false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //actividad
            columnas = funciones.armar_query_columna(columnas, "actividad", false);
            valores = funciones.armar_query_valores(valores, actividad, false);
            //nota
            columnas = funciones.armar_query_columna(columnas, "nota", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //turno_logueado
            columnas = funciones.armar_query_columna(columnas, "turno_logueado", true);
            valores = funciones.armar_query_valores(valores, turno, true);
            consultas.insertar_en_tabla(base_de_datos, "historial_lista_chequeo", columnas, valores);
        }
        public void actualizar_nota(string id, string nota)
        {
            string actualizar = "`nota` = '" + nota + "' ";
            consultas.actualizar_tabla(base_de_datos, "historial_lista_chequeo", actualizar, id);
        }
        #endregion
        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("actividad", typeof(string));
            resumen.Columns.Add("categoria", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
            resumen.Columns.Add("area", typeof(string));
        }
        private void crear_tabla_historial_resumen()
        {
            historial_resumen = new DataTable();
            historial_resumen.Columns.Add("id", typeof(string));
            historial_resumen.Columns.Add("id_historial", typeof(string));
            historial_resumen.Columns.Add("actividad", typeof(string));
            historial_resumen.Columns.Add("nota", typeof(string));
            historial_resumen.Columns.Add("fecha", typeof(string));
            historial_resumen.Columns.Add("cargo", typeof(string));

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
        private void llenar_historial_resumen()
        {
            crear_tabla_historial_resumen();
            string id, actividad;
            int ultima_fila;
            for (int fila = 0; fila <= historial.Rows.Count - 1; fila++)
            {
                id = funciones.obtener_dato(historial.Rows[fila]["actividad"].ToString(), 1);
                actividad = funciones.obtener_dato(historial.Rows[fila]["actividad"].ToString(), 2);

                historial_resumen.Rows.Add();
                ultima_fila = historial_resumen.Rows.Count - 1;

                historial_resumen.Rows[ultima_fila]["id"] = id;
                historial_resumen.Rows[ultima_fila]["id_historial"] = historial.Rows[fila]["id"].ToString();
                historial_resumen.Rows[ultima_fila]["actividad"] = actividad;
                historial_resumen.Rows[ultima_fila]["nota"] = historial.Rows[fila]["nota"].ToString();
                historial_resumen.Rows[ultima_fila]["fecha"] = historial.Rows[fila]["fecha"].ToString();
                historial_resumen.Rows[ultima_fila]["cargo"] = historial.Rows[fila]["cargo"].ToString();
            }
        }
        private string verificar_horario(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 7, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 12:00 PM
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
        private string verificar_horario_turno2(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 05, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "rango 1";
            }
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 00, 0, 0); // 8:00 AM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 04, 59, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2)
            {
                retorno = "rango 2";
            }
          /*  DateTime horaInicio_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 00, 0, 0); // 8:00 AM
            DateTime horaFin_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 04, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango3 && miFecha <= horaFin_rango3)
            {
                retorno = "rango 3";
            }*/
            return retorno;
        }
        public bool verificar_brecha_turno(DateTime miFecha, string turno)
        {
            bool retorno = false;

            // Definir los límites de tiempo para Turno 1
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 7:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 18, 59, 59); // 5:00 PM
            if ((miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1) &&
                turno == "Turno 1")
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
        #region metodos consultas
        private void consultar_empleado(string id_empleado)
        {
            empleado = consultas.consultar_empleado(id_empleado);
        }
        private void consultar_historial(DateTime fecha, string hora_inicio, string hora_fin, string id_empleado, string id_sucursal, string turno)
        {
            historial = consultas.consultar_historial_chequeo_segun_fecha(fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString(), hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
        }
        private void consultar_historial_turno2(DateTime fecha, string id_empleado, string id_sucursal, string turno)
        {
            DateTime fecha_nueva;
            DataTable historial_turno2 = new DataTable();
            int ultima_fila;
            string hora_inicio, hora_fin;
            if ("rango 1" == verificar_horario_turno2(DateTime.Now))
            {
                fecha_nueva = fecha;
                fecha_nueva = fecha_nueva.AddDays(1);
                hora_inicio = "17:00";
                hora_fin = "23:59";
                historial = consultas.consultar_historial_chequeo_segun_fecha(fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString(), hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
                hora_inicio = "00:00";
                hora_fin = "04:59";
                historial_turno2 = consultas.consultar_historial_chequeo_segun_fecha(fecha_nueva.Year.ToString(), fecha_nueva.Month.ToString(), fecha_nueva.Day.ToString(), hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
            }
            else if ("rango 2" == verificar_horario_turno2(DateTime.Now))//
            {
                fecha_nueva = fecha;
                fecha_nueva = fecha_nueva.AddDays(-1);
                hora_inicio = "00:00";
                hora_fin = "04:59";
                historial = consultas.consultar_historial_chequeo_segun_fecha(fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString(), hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
                hora_inicio = "17:00";
                hora_fin = "23:59";
                historial_turno2 = consultas.consultar_historial_chequeo_segun_fecha(fecha_nueva.Year.ToString(), fecha_nueva.Month.ToString(), fecha_nueva.Day.ToString(), hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
            }

            if (historial_turno2.Rows.Count > 0)
            {
                for (int fila = 0; fila <= historial_turno2.Rows.Count - 1; fila++)
                {
                    historial.Rows.Add();
                    ultima_fila = historial.Rows.Count - 1;
                    //activa
                    historial.Rows[ultima_fila]["activa"] = historial_turno2.Rows[fila]["activa"].ToString();
                    //id
                    historial.Rows[ultima_fila]["id"] = historial_turno2.Rows[fila]["id"].ToString();

                    //id_sucursal
                    historial.Rows[ultima_fila]["id_sucursal"] = historial_turno2.Rows[fila]["id_sucursal"].ToString();

                    //id_empleado
                    historial.Rows[ultima_fila]["id_empleado"] = historial_turno2.Rows[fila]["id_empleado"].ToString();

                    //nombre
                    historial.Rows[ultima_fila]["nombre"] = historial_turno2.Rows[fila]["nombre"].ToString();

                    //apellido
                    historial.Rows[ultima_fila]["apellido"] = historial_turno2.Rows[fila]["apellido"].ToString();

                    //fecha
                    historial.Rows[ultima_fila]["fecha"] = historial_turno2.Rows[fila]["fecha"].ToString();

                    //actividad
                    historial.Rows[ultima_fila]["actividad"] = historial_turno2.Rows[fila]["actividad"].ToString();
                    //cargo
                    historial.Rows[ultima_fila]["cargo"] = historial_turno2.Rows[fila]["cargo"].ToString();

                }
            }
        }
        private void consultar_configuracion_de_chequeo(string perfil)
        {
            configuracion_de_chequeo = consultas.consultar_configuracion_chequeo(perfil);
        }
        private void consultar_lista_de_chequeo()
        {
            lista_de_chequeo = consultas.consultar_tabla(base_de_datos, "lista_de_chequeo");
        }
        #endregion

        #region metodos get/set
        public DataTable get_historial(DateTime fecha, string turno, string id_empleado, string id_sucursal)
        {
            string hora_inicio = "07:00";
            string hora_fin = "18:59";
            if (turno == "N/A")
            {
                if ("rango 1" == verificar_horario(fecha))// 
                {
                    consultar_historial(fecha, hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
                }
                else if ("rango 2" == verificar_horario(fecha) || "rango 3" == verificar_horario(fecha))// 
                {

                    consultar_historial_turno2(fecha, id_empleado, id_sucursal, turno);
                }

            }
            else
            {
                if (turno == "Turno 1")// "rango 1" == verificar_horario(fecha)
                {
                    consultar_historial(fecha, hora_inicio, hora_fin, id_empleado, id_sucursal, turno);
                }
                else if (turno == "Turno 2")// "rango 2" == verificar_horario(fecha) ||"rango 3" == verificar_horario(fecha)
                {
                    DateTime nueva_fecha = new DateTime(fecha.Year,
                                                        fecha.Month,
                                                        fecha.Day,
                                                        18, 0, 0);
                        
                    consultar_historial_turno2(nueva_fecha, id_empleado, id_sucursal, turno);
                }

            }

            llenar_historial_resumen();
            return historial_resumen;
        }
        public string get_id_chequeo_activo(string perfil)
        {
            string retorno = "N/A";
            consultar_configuracion_de_chequeo(perfil);
            if (configuracion_de_chequeo.Rows.Count > 0)
            {
                retorno = configuracion_de_chequeo.Rows[0]["id"].ToString();
            }
            return retorno;
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
        public DataTable get_empleado(string id_empleado)
        {
            consultar_empleado(id_empleado);
            return empleado;
        }
        #endregion
    }
}

