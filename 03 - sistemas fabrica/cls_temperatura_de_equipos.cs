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
        #endregion

        #region metodos privados
        private void crear_tabla_equipos()
        {
            equipos_ordenados = new DataTable();
            equipos_ordenados.Columns.Add("id",typeof(string));
            equipos_ordenados.Columns.Add("nombre", typeof(string));
            equipos_ordenados.Columns.Add("categoria", typeof(string));
            equipos_ordenados.Columns.Add("ubicacion", typeof(string));
            equipos_ordenados.Columns.Add("temperatura", typeof(string));
            equipos_ordenados.Columns.Add("observaciones", typeof(string));
        }
        private void ordenar_equipo_segun_ubicacion()
        {
            crear_tabla_equipos();
            string ubicacion;
            int ultima_fila;
            equipos.DefaultView.Sort = "categoria ASC";
            equipos = equipos.DefaultView.ToTable();
            for (int fila = 0; fila <= ubicaciones.Rows.Count-1; fila++)
            {
                ubicacion = ubicaciones.Rows[fila]["ubicacion"].ToString();
                for (int fila_equipo = 0; fila_equipo <= equipos.Rows.Count-1; fila_equipo++)
                {
                    if (ubicacion== equipos.Rows[fila_equipo]["ubicacion"].ToString())
                    {
                        equipos_ordenados.Rows.Add();
                        ultima_fila = equipos_ordenados.Rows.Count - 1;
                        equipos_ordenados.Rows[ultima_fila]["id"] = equipos.Rows[fila_equipo]["id"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["nombre"] = equipos.Rows[fila_equipo]["nombre"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["categoria"] = equipos.Rows[fila_equipo]["categoria"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["ubicacion"] = equipos.Rows[fila_equipo]["ubicacion"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["temperatura"] = equipos.Rows[fila_equipo]["temperatura"].ToString();
                        equipos_ordenados.Rows[ultima_fila]["observaciones"] = equipos.Rows[fila_equipo]["observaciones"].ToString();
                    }
                }
            }
        }
        #endregion
        #region metodos consultas
        private void consultar_ubicaciones()
        {
            ubicaciones = consultas.consultar_tabla(base_de_datos, "Ubicaciones_de_equipos");
        }
        private void consultar_equipos(string ubicacion)
        {
            if (ubicacion=="todos")
            {
                equipos = consultas.consultar_tabla(base_de_datos, "equipos");
            }
            else
            {
                equipos = consultas.consultar_equipo_segun_ubicacion(ubicacion);
            }
        }
        #endregion
        #region metodos get/set
        public DataTable get_equipos(string ubicacion)
        {
            consultar_equipos(ubicacion);
            if (ubicacion == "todos")
            {
                consultar_ubicaciones();
                ordenar_equipo_segun_ubicacion();
                return equipos_ordenados;
            }
            else
            {
                equipos.DefaultView.Sort = "categoria ASC";
                equipos = equipos.DefaultView.ToTable();
                return equipos;
            }
        }
        public DataTable get_ubicaciones()
        {
            consultar_ubicaciones();
            return ubicaciones;
        }
        #endregion
    }
}
