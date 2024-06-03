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
    public class cls_temperatura_de_equipos
    {
        public cls_temperatura_de_equipos(DataTable usuario_BD)
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

        DataTable ubicaciones;
        DataTable equipos;
        DataTable equipos_ordenados;
        DataTable temperaturas_del_dia;
        DataTable temperaturas;
        #endregion
        #region carga a base de datos
        public void registrar_temperatura(string nombre, string id_equipo, string equipo, string temperatura,string nota)
        {
            string columna = "";
            string valores = "";
            //nombre
            columna = funciones.armar_query_columna(columna, "nombre", false);
            valores = funciones.armar_query_valores(valores, nombre, false);
            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //id_equipo
            columna = funciones.armar_query_columna(columna, "id_equipo", false);
            valores = funciones.armar_query_valores(valores, id_equipo, false);
            //equipo
            columna = funciones.armar_query_columna(columna, "equipo", false);
            valores = funciones.armar_query_valores(valores, equipo, false);
            //nota
            columna = funciones.armar_query_columna(columna, "nota", false);
            valores = funciones.armar_query_valores(valores, nota, false);
            //temperatura
            columna = funciones.armar_query_columna(columna, "temperatura", true);
            valores = funciones.armar_query_valores(valores, temperatura, true);
            consultas.insertar_en_tabla(base_de_datos, "temperatura_de_equipos", columna, valores);
        }
        #endregion
        #region metodos privados
        private string verificar_horario(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 8, 0, 0); // 8:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 12, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "rango 1";
            }
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 12, 0, 0); // 8:00 AM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 18, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2)
            {
                retorno = "rango 2";
            }
            DateTime horaInicio_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 18, 0, 0); // 8:00 AM
            DateTime horaFin_rango3 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 0, 0); // 12:00 PM
            if (miFecha >= horaInicio_rango3 && miFecha <= horaFin_rango3)
            {
                retorno = "rango 3";
            }
            return retorno;
        }
        private void crear_tabla_temperatura()
        {
            temperaturas = new DataTable();
            temperaturas.Columns.Add("id", typeof(string));
            temperaturas.Columns.Add("ubicacion", typeof(string));
            temperaturas.Columns.Add("nombre", typeof(string));
            temperaturas.Columns.Add("categoria", typeof(string));
            temperaturas.Columns.Add("observaciones", typeof(string));
            temperaturas.Columns.Add("temperatura", typeof(string));
            temperaturas.Columns.Add("turno_1", typeof(string));
            temperaturas.Columns.Add("turno_2", typeof(string));
            temperaturas.Columns.Add("turno_3", typeof(string));
        }
        public int buscar_fila_temperatura(string id, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                if (id == dt.Rows[fila]["id_equipo"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private void ordenar_temperatura()
        {
            crear_tabla_temperatura();
            int ult_fila, fila_temperatura;
            string rango = "", id_equipo;
            for (int fila = 0; fila <= equipos.Rows.Count - 1; fila++)
            {
                temperaturas.Rows.Add();
                ult_fila = temperaturas.Rows.Count - 1;
                id_equipo = equipos.Rows[fila]["id"].ToString();
                fila_temperatura = buscar_fila_temperatura(id_equipo, temperaturas_del_dia);
                temperaturas.Rows[ult_fila]["id"] = equipos.Rows[fila]["id"].ToString();
                temperaturas.Rows[ult_fila]["ubicacion"] = equipos.Rows[fila]["ubicacion"].ToString();
                temperaturas.Rows[ult_fila]["nombre"] = equipos.Rows[fila]["nombre"].ToString();
                temperaturas.Rows[ult_fila]["categoria"] = equipos.Rows[fila]["categoria"].ToString();
                temperaturas.Rows[ult_fila]["observaciones"] = equipos.Rows[fila]["observaciones"].ToString();
                temperaturas.Rows[ult_fila]["temperatura"] = equipos.Rows[fila]["temperatura"].ToString();
                if (fila_temperatura != -1)
                {
                    rango = verificar_horario(DateTime.Parse(temperaturas_del_dia.Rows[fila_temperatura]["fecha"].ToString()));
                }
                else
                {
                    rango = "";
                }
                if (rango == "rango 1")
                {
                    temperaturas.Rows[ult_fila]["turno_1"] = temperaturas_del_dia.Rows[fila_temperatura]["temperatura"].ToString() + "°C";
                }
                else
                {
                    temperaturas.Rows[ult_fila]["turno_1"] = "N/A";
                }
                if (rango == "rango 2")
                {
                    temperaturas.Rows[ult_fila]["turno_2"] = temperaturas_del_dia.Rows[fila_temperatura]["temperatura"].ToString() + "°C";
                }
                else
                {
                    temperaturas.Rows[ult_fila]["turno_2"] = "N/A";
                }
                if (rango == "rango 3")
                {
                    temperaturas.Rows[ult_fila]["turno_3"] = temperaturas_del_dia.Rows[fila_temperatura]["temperatura"].ToString() + "°C";
                }
                else
                {
                    temperaturas.Rows[ult_fila]["turno_3"] = "N/A";
                }
            }
        }
        private void crear_tabla_equipos()
        {
            equipos_ordenados = new DataTable();
            equipos_ordenados.Columns.Add("id", typeof(string));
            equipos_ordenados.Columns.Add("nombre", typeof(string));
            equipos_ordenados.Columns.Add("categoria", typeof(string));
            equipos_ordenados.Columns.Add("ubicacion", typeof(string));
            equipos_ordenados.Columns.Add("temperatura", typeof(string));
            equipos_ordenados.Columns.Add("observaciones", typeof(string));
            equipos_ordenados.Columns.Add("turno_1", typeof(string));
            equipos_ordenados.Columns.Add("turno_2", typeof(string));
            equipos_ordenados.Columns.Add("turno_3", typeof(string));
        }
        private void ordenar_equipo_segun_ubicacion()
        {
            crear_tabla_equipos();
            string ubicacion;
            int ultima_fila;
            temperaturas.DefaultView.Sort = "categoria ASC";
            temperaturas = temperaturas.DefaultView.ToTable();
            for (int fila = 0; fila <= ubicaciones.Rows.Count - 1; fila++)
            {
                ubicacion = ubicaciones.Rows[fila]["ubicacion"].ToString();
                for (int fila_equipo = 0; fila_equipo <= temperaturas.Rows.Count - 1; fila_equipo++)
                {
                    if (ubicacion == temperaturas.Rows[fila_equipo]["ubicacion"].ToString())
                    {
                        equipos_ordenados.Rows.Add();
                        ultima_fila = equipos_ordenados.Rows.Count - 1;
                        equipos_ordenados.Rows[ultima_fila]["id"] = temperaturas.Rows[fila_equipo]["id"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["nombre"] = temperaturas.Rows[fila_equipo]["nombre"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["categoria"] = temperaturas.Rows[fila_equipo]["categoria"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["ubicacion"] = temperaturas.Rows[fila_equipo]["ubicacion"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["temperatura"] = temperaturas.Rows[fila_equipo]["temperatura"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["observaciones"] = temperaturas.Rows[fila_equipo]["observaciones"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["turno_1"] = temperaturas.Rows[fila_equipo]["turno_1"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["turno_2"] = temperaturas.Rows[fila_equipo]["turno_2"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["turno_3"] = temperaturas.Rows[fila_equipo]["turno_3"].ToString();
                    }
                }
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_temperatura(DateTime fecha)
        {
            temperaturas_del_dia = consultas.consultar_temperatura_segun_fecha(fecha.Year.ToString(), fecha.Month.ToString(), fecha.Day.ToString());
        }
        private void consultar_ubicaciones()
        {
            ubicaciones = consultas.consultar_tabla(base_de_datos, "Ubicaciones_de_equipos");
        }
        private void consultar_equipos(string ubicacion)
        {
            if (ubicacion == "todos")
            {
                equipos = consultas.consultar_tabla(base_de_datos, "equipos");
            }
            else
            {
                equipos = consultas.consultar_equipo_segun_ubicacion(ubicacion);
            }
            equipos.Columns.Add("turno_1", typeof(string));
            equipos.Columns.Add("turno_2", typeof(string));
            equipos.Columns.Add("turno_3", typeof(string));
        }
        #endregion
        #region metodos get/set

        public DataTable get_equipos(string ubicacion, DateTime fecha)
        {
            consultar_equipos(ubicacion);
            consultar_temperatura(fecha);
            ordenar_temperatura();
            if (ubicacion == "todos")
            {
                consultar_ubicaciones();
                ordenar_equipo_segun_ubicacion();
                return equipos_ordenados;
            }
            else
            {
                temperaturas.DefaultView.Sort = "categoria ASC";
                temperaturas = temperaturas.DefaultView.ToTable();
            }
            return temperaturas;
        }
        public DataTable get_ubicaciones()
        {
            consultar_ubicaciones();
            return ubicaciones;
        }
        #endregion
    }
}
