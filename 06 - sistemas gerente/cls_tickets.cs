using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06___sistemas_gerente
{
    public class cls_tickets
    {
        public cls_tickets(DataTable usuario_BD)
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

        DataTable tickets;
        #endregion

        #region cargar a base de datos
        public void actualizar_prioridades(DataTable tickets_ordenados)
        {
            string id;
            string actualizar;
            for (int fila = 0; fila <= tickets_ordenados.Rows.Count-1; fila++)
            {
                id= tickets_ordenados.Rows[fila]["id"].ToString();
                actualizar = "`prioridad` = '"+ tickets_ordenados.Rows[fila]["prioridad"].ToString() + "'";
                consultas.actualizar_tabla(base_de_datos,"tickets",actualizar,id);
            }
        }
        #endregion

        #region  metodos consultas
        private void consultar_tickets(string mes, string año, string tipo_ticket)
        {
            tickets = consultas.consultar_tickets_segun_fecha(mes,año, tipo_ticket);
        }
        #endregion

        #region metodos get/set
        public DataTable get_tickets(string mes, string año, string tipo_ticket)
        {
            consultar_tickets(mes,año,tipo_ticket);
            return tickets;
        }
        #endregion
    }
}
