using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        #region reordenamiento
        public void reordenar_tickets(string id, string nuevo_orden, string mes, string año, string tipo_ticket)
        {
            // Consultar los tickets por el área correspondiente
            consultar_tickets(mes, año, tipo_ticket);

            // Buscar la fila y obtener la prioridad actual del ticket que se va a reordenar
            int fila_ticket = funciones.buscar_fila_por_id(id, tickets);
            int prioridad_actual = int.Parse(tickets.Rows[fila_ticket]["prioridad"].ToString());

            // Convertir el nuevo orden a entero
            int nuevo_orden_int = int.Parse(nuevo_orden);

            // Crear una tabla temporal para almacenar las nuevas prioridades
            DataTable tabla_ordenada = new DataTable();
            tabla_ordenada.Columns.Add("id", typeof(string));
            tabla_ordenada.Columns.Add("prioridad", typeof(int));

            int prioridad_area;
            string id_ticket;

            // Caso 1: Mover hacia arriba (nuevo_orden < prioridad_actual)
            if (nuevo_orden_int < prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area >= nuevo_orden_int && prioridad_area < prioridad_actual)
                    {
                        // Incrementar la prioridad de los tickets que están entre el nuevo y el actual
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area + 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }
            // Caso 2: Mover hacia abajo (nuevo_orden > prioridad_actual)
            else if (nuevo_orden_int > prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area <= nuevo_orden_int && prioridad_area > prioridad_actual)
                    {
                        // Decrementar la prioridad de los tickets que están entre el actual y el nuevo
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area - 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }

            // Actualizar la base de datos con las nuevas prioridades
            for (int fila = 0; fila < tabla_ordenada.Rows.Count; fila++)
            {
                string id_ordenado = tabla_ordenada.Rows[fila]["id"].ToString();
                prioridad_area = int.Parse(tabla_ordenada.Rows[fila]["prioridad"].ToString());

                string actualizar = "`prioridad` = '" + prioridad_area + "'";
                consultas.actualizar_tabla(base_de_datos, "tickets", actualizar, id_ordenado);
            }
        }
        public void reordenar_tickets_abiertos(string id, string nuevo_orden, string tipo_ticket)
        {
            // Consultar los tickets por el área correspondiente
            consultar_tickets_abiertos(tipo_ticket);

            // Buscar la fila y obtener la prioridad actual del ticket que se va a reordenar
            int fila_ticket = funciones.buscar_fila_por_id(id, tickets);
            int prioridad_actual = int.Parse(tickets.Rows[fila_ticket]["prioridad"].ToString());

            // Convertir el nuevo orden a entero
            int nuevo_orden_int = int.Parse(nuevo_orden);

            // Crear una tabla temporal para almacenar las nuevas prioridades
            DataTable tabla_ordenada = new DataTable();
            tabla_ordenada.Columns.Add("id", typeof(string));
            tabla_ordenada.Columns.Add("prioridad", typeof(int));

            int prioridad_area;
            string id_ticket;

            // Caso 1: Mover hacia arriba (nuevo_orden < prioridad_actual)
            if (nuevo_orden_int < prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area >= nuevo_orden_int && prioridad_area < prioridad_actual)
                    {
                        // Incrementar la prioridad de los tickets que están entre el nuevo y el actual
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area + 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }
            // Caso 2: Mover hacia abajo (nuevo_orden > prioridad_actual)
            else if (nuevo_orden_int > prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area <= nuevo_orden_int && prioridad_area > prioridad_actual)
                    {
                        // Decrementar la prioridad de los tickets que están entre el actual y el nuevo
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area - 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }

            // Actualizar la base de datos con las nuevas prioridades
            for (int fila = 0; fila < tabla_ordenada.Rows.Count; fila++)
            {
                string id_ordenado = tabla_ordenada.Rows[fila]["id"].ToString();
                prioridad_area = int.Parse(tabla_ordenada.Rows[fila]["prioridad"].ToString());

                string actualizar = "`prioridad` = '" + prioridad_area + "'";
                consultas.actualizar_tabla(base_de_datos, "tickets", actualizar, id_ordenado);
            }
        }
        public void reordenar_tickets_area(string id, string nuevo_orden, string mes, string año, string solicita)
        {
            // Consultar los tickets por el área correspondiente
            consultar_tickets_por_area(mes, año, solicita);

            // Buscar la fila y obtener la prioridad actual del ticket que se va a reordenar
            int fila_ticket = funciones.buscar_fila_por_id(id, tickets);
            int prioridad_actual = int.Parse(tickets.Rows[fila_ticket]["prioridad_area"].ToString());

            // Convertir el nuevo orden a entero
            int nuevo_orden_int = int.Parse(nuevo_orden);

            // Crear una tabla temporal para almacenar las nuevas prioridades
            DataTable tabla_ordenada = new DataTable();
            tabla_ordenada.Columns.Add("id", typeof(string));
            tabla_ordenada.Columns.Add("prioridad_area", typeof(int));

            int prioridad_area;
            string id_ticket;

            // Caso 1: Mover hacia arriba (nuevo_orden < prioridad_actual)
            if (nuevo_orden_int < prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad_area"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area >= nuevo_orden_int && prioridad_area < prioridad_actual)
                    {
                        // Incrementar la prioridad de los tickets que están entre el nuevo y el actual
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area + 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }
            // Caso 2: Mover hacia abajo (nuevo_orden > prioridad_actual)
            else if (nuevo_orden_int > prioridad_actual)
            {
                for (int fila = 0; fila < tickets.Rows.Count; fila++)
                {
                    prioridad_area = int.Parse(tickets.Rows[fila]["prioridad_area"].ToString());
                    id_ticket = tickets.Rows[fila]["id"].ToString();

                    if (prioridad_area <= nuevo_orden_int && prioridad_area > prioridad_actual)
                    {
                        // Decrementar la prioridad de los tickets que están entre el actual y el nuevo
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area - 1);
                    }
                    else if (id_ticket == id)
                    {
                        // Asignar la nueva prioridad al ticket reordenado
                        tabla_ordenada.Rows.Add(id_ticket, nuevo_orden_int);
                    }
                    else
                    {
                        // Mantener la prioridad de los demás tickets
                        tabla_ordenada.Rows.Add(id_ticket, prioridad_area);
                    }
                }
            }

            // Actualizar la base de datos con las nuevas prioridades
            for (int fila = 0; fila < tabla_ordenada.Rows.Count; fila++)
            {
                string id_ordenado = tabla_ordenada.Rows[fila]["id"].ToString();
                prioridad_area = int.Parse(tabla_ordenada.Rows[fila]["prioridad_area"].ToString());

                string actualizar = "`prioridad_area` = '" + prioridad_area + "'";
                consultas.actualizar_tabla(base_de_datos, "tickets", actualizar, id_ordenado);
            }
        }

        #endregion

        #region cargar a base de datos
        public void cerrar_ticket(string id)
        {
            string actualizar = "`fecha_resolucion` = '" + funciones.get_fecha() + "'";
            consultas.actualizar_tabla(base_de_datos, "tickets", actualizar, id);
            actualizar = "`estado` = 'resuelto'";
            consultas.actualizar_tabla(base_de_datos, "tickets", actualizar, id);
        }
        #endregion

        #region  metodos consultas
        private void consultar_tickets(string mes, string año, string tipo_ticket)
        {
            tickets = consultas.consultar_tickets_segun_fecha(mes, año, tipo_ticket);
        }
        private void consultar_tickets_abiertos(string tipo_ticket)
        {
            tickets = consultas.consultar_tickets_abiertos(tipo_ticket);
        }
        private void consultar_todos_tickets_abiertos(string mes, string año)
        {
            tickets = consultas.consultar_todos_tickets_segun_fecha(mes, año);
        }
        private void consultar_tickets_por_area(string mes, string año, string solicita)
        {
            tickets = consultas.consultar_tickets_segun_fecha_area(mes, año, solicita);
        }
        #endregion

        #region metodos get/set
        public DataTable get_tickets(string mes, string año, string tipo_ticket)
        {
            consultar_tickets(mes, año, tipo_ticket);
            tickets.DefaultView.Sort = "prioridad asc";
            tickets = tickets.DefaultView.ToTable();
            return tickets;
        }
        public DataTable get_tickets_abiertos(string tipo_ticket)
        {
            consultar_tickets_abiertos(tipo_ticket);
            tickets.DefaultView.Sort = "prioridad asc";
            tickets = tickets.DefaultView.ToTable();
            return tickets;
        }
        public DataTable get_todos_tickets_abiertos(string mes, string año)
        {
            consultar_todos_tickets_abiertos(mes, año);
            return tickets;
        }
        public DataTable get_tickets_area(string mes, string año, string solicita)
        {
            consultar_tickets_por_area(mes, año, solicita);
            tickets.DefaultView.Sort = "prioridad_area asc";
            tickets = tickets.DefaultView.ToTable();
            return tickets;
        }
        #endregion
    }
}
