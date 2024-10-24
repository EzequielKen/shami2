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
    public class cls_crear_ticket
    {
        public cls_crear_ticket(DataTable usuario_BD)
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

        #region carga a base de datos
        public void crear_ticket(DataTable ticket)
        {
            string columnas = string.Empty;
            string valores = string.Empty;

            //prioridad
            columnas = funciones.armar_query_columna(columnas, "prioridad", false);
            valores = funciones.armar_query_valores(valores, generar_ultima_posicion(), false);
            //prioridad_area
            columnas = funciones.armar_query_columna(columnas, "prioridad_area", false);
            valores = funciones.armar_query_valores(valores, generar_ultima_posicion_area(ticket.Rows[0]["solicita"].ToString()), false);
            //fecha_solicitud
            columnas = funciones.armar_query_columna(columnas, "fecha_solicitud", false);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["fecha_solicitud"].ToString(), false);
            //fecha_resolucion_solicitada
            columnas = funciones.armar_query_columna(columnas, "fecha_resolucion_solicitada", false);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["fecha_resolucion_solicitada"].ToString(), false);
            //solicita
            columnas = funciones.armar_query_columna(columnas, "solicita", false);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["solicita"].ToString(), false);
            //tipo_ticket
            columnas = funciones.armar_query_columna(columnas, "tipo_ticket", false);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["tipo_ticket"].ToString(), false);
            //asunto
            columnas = funciones.armar_query_columna(columnas, "asunto", false);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["asunto"].ToString(), false);
            //detalle
            columnas = funciones.armar_query_columna(columnas, "detalle", true);
            valores = funciones.armar_query_valores(valores, ticket.Rows[0]["detalle"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "tickets", columnas, valores);
        }

        private string generar_ultima_posicion()
        {
            int retorno = 0;
            consultar_tickets();
            if (tickets.Rows.Count > 0)
            {
                tickets.DefaultView.Sort = "prioridad asc";
                tickets = tickets.DefaultView.ToTable();

                retorno = int.Parse(tickets.Rows[tickets.Rows.Count-1]["prioridad"].ToString()) + 1;
            }
            return retorno.ToString();
        }
        private string generar_ultima_posicion_area(string solicita)
        {
            int retorno = 0;
            consultar_tickets_por_area(solicita);
            if (tickets.Rows.Count > 0)
            {
                tickets.DefaultView.Sort = "prioridad_area asc";
                tickets = tickets.DefaultView.ToTable();

                retorno = int.Parse(tickets.Rows[tickets.Rows.Count - 1]["prioridad_area"].ToString()) + 1;
            }
            return retorno.ToString();
        }
        #endregion

        #region metodos consultas
        private void consultar_tickets()
        {
            tickets = consultas.consultar_tickets_abiertos();
        }
        private void consultar_tickets_por_area(string solicita)
        {
            tickets = consultas.consultar_tickets_abiertos_por_solicitante(solicita);
        }
        #endregion
    }
}
