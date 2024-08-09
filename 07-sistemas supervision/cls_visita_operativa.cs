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
    public class cls_visita_operativa
    {
        public cls_visita_operativa(DataTable usuario_BD)
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

        DataTable sucursales;
        DataTable sucursal;
        DataTable lista_de_empleado;
        DataTable empleado;
        #endregion

        #region carga a base de datos
        public void actualizar_cargos_empleado(string id_empleado, DataTable empleados)
        {
            int fila_empleado = funciones.buscar_fila_por_id(id_empleado, empleados);
            string dato = configurar_cargo(empleados, fila_empleado);
            actualizar_dato_empleado(id_empleado, "cargo", dato);
        }
        private string configurar_cargo(DataTable empleados, int fila_empleados)
        {
            string retorno = string.Empty;
            int cantidad_cargos = 0;
            string cargos = string.Empty;
            if (empleados.Rows[fila_empleados]["Encargado"].ToString() != "N/A")
            {
                cargos = cargos + "-Encargado";
                cantidad_cargos++;
            }

            if (empleados.Rows[fila_empleados]["Cajero"].ToString() != "N/A")
            {
                cargos = cargos + "-Cajero";
                cantidad_cargos++;
            }

            if (empleados.Rows[fila_empleados]["Shawarmero"].ToString() != "N/A")
            {
                cargos = cargos + "-Shawarmero";
                cantidad_cargos++;
            }

            if (empleados.Rows[fila_empleados]["Atencion al Cliente"].ToString() != "N/A")
            {
                cargos = cargos + "-Atencion al Cliente";
                cantidad_cargos++;
            }

            if (empleados.Rows[fila_empleados]["Cocina"].ToString() != "N/A")
            {
                cargos = cargos + "-Cocina";
                cantidad_cargos++;
            }

            if (empleados.Rows[fila_empleados]["Limpieza"].ToString() != "N/A")
            {
                cargos = cargos + "-Limpieza";
                cantidad_cargos++;
            }
            retorno = cantidad_cargos.ToString() + cargos;
            return retorno;
        }
        public void actualizar_dato_empleado(string id, string campo, string dato)
        {
            string actualizar = "`" + campo + "` = '" + dato + "' ";
            consultas.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id);
        }
        public void desloguear_empleado(string id)
        {
            string actualizar = "`id_sucursal` = 'N/A'";
            consultas.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id);
        }
        public void eliminar_empleado(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id);
        }
        public void registrar_empleado(DataTable empleado)
        {
            string columna = "";
            string valores = "";

            //id_sucursal_origen
            columna = funciones.armar_query_columna(columna, "id_sucursal_origen", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id_sucursal"].ToString(), false);
            //id_sucursal
            columna = funciones.armar_query_columna(columna, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id_sucursal"].ToString(), false);
            //nombre
            columna = funciones.armar_query_columna(columna, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["nombre"].ToString(), false);
            //apellido
            columna = funciones.armar_query_columna(columna, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["apellido"].ToString(), false);
            //dni
            columna = funciones.armar_query_columna(columna, "dni", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["dni"].ToString(), false);
            //contraseña
            columna = funciones.armar_query_columna(columna, "contraseña", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["dni"].ToString(), false);
            //telefono
            columna = funciones.armar_query_columna(columna, "telefono", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["telefono"].ToString(), false);
            //cargo
            columna = funciones.armar_query_columna(columna, "cargo", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["cargo"].ToString(), false);
            //cargo
            columna = funciones.armar_query_columna(columna, "sueldo", true);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["sueldo"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "lista_de_empleado", columna, valores);
        }
        public bool verificar_si_empleado_existe(DataTable empleado)
        {
            bool retorno = false;
            string dni = empleado.Rows[0]["dni"].ToString();
            DataTable empleado_registrado = consultas.login_empleado(dni);
            if (empleado_registrado.Rows.Count > 0)
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
        #region metodos privados
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
        #endregion
        #region metodos consultas
        private void consultar_empleado(string id_empleado)
        {
            empleado = consultas.consultar_empleado(id_empleado);
        }
        private void consultar_sucursal(string sucursal_nombre)
        {
            sucursal = consultas.consultar_sucursal_por_nombre(sucursal_nombre);
        }
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        private void consultar_lista_de_empleado(string id_sucursal)
        {
            lista_de_empleado = consultas.consultar_empleados(id_sucursal);
            lista_de_empleado.Columns.Add("Encargado", typeof(string));
            lista_de_empleado.Columns.Add("Cajero", typeof(string));
            lista_de_empleado.Columns.Add("Shawarmero", typeof(string));
            lista_de_empleado.Columns.Add("Atencion al Cliente", typeof(string));
            lista_de_empleado.Columns.Add("Cocina", typeof(string));
            lista_de_empleado.Columns.Add("Limpieza", typeof(string));
            int iteraciones, posicion;
            string cargo;
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                lista_de_empleado.Rows[fila]["Encargado"] = "N/A";
                lista_de_empleado.Rows[fila]["Cajero"] = "N/A";
                lista_de_empleado.Rows[fila]["Shawarmero"] = "N/A";
                lista_de_empleado.Rows[fila]["Atencion al Cliente"] = "N/A";
                lista_de_empleado.Rows[fila]["Cocina"] = "N/A";
                lista_de_empleado.Rows[fila]["Limpieza"] = "N/A";
                iteraciones = int.Parse(funciones.obtener_dato(lista_de_empleado.Rows[fila]["cargo"].ToString(), 1));
                posicion = 2;
                for (int index = 0; index <= iteraciones; index++)
                {
                    cargo = funciones.obtener_dato(lista_de_empleado.Rows[fila]["cargo"].ToString(), posicion);
                    lista_de_empleado.Rows[fila][cargo] = cargo;
                    posicion++;
                }
            }
        }
        private void consultar_lista_de_empleado_origen(string id_sucursal)
        {
            lista_de_empleado = consultas.consultar_empleados_origen(id_sucursal);
            lista_de_empleado.Columns.Add("Encargado", typeof(string));
            lista_de_empleado.Columns.Add("Cajero", typeof(string));
            lista_de_empleado.Columns.Add("Shawarmero", typeof(string));
            lista_de_empleado.Columns.Add("Atencion al Cliente", typeof(string));
            lista_de_empleado.Columns.Add("Cocina", typeof(string));
            lista_de_empleado.Columns.Add("Limpieza", typeof(string));
            int iteraciones, posicion;
            string cargo;
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                lista_de_empleado.Rows[fila]["Encargado"] = "N/A";
                lista_de_empleado.Rows[fila]["Cajero"] = "N/A";
                lista_de_empleado.Rows[fila]["Shawarmero"] = "N/A";
                lista_de_empleado.Rows[fila]["Atencion al Cliente"] = "N/A";
                lista_de_empleado.Rows[fila]["Cocina"] = "N/A";
                lista_de_empleado.Rows[fila]["Limpieza"] = "N/A";
                iteraciones = int.Parse(funciones.obtener_dato(lista_de_empleado.Rows[fila]["cargo"].ToString(), 1));
                posicion = 2;
                for (int index = 0; index <= iteraciones; index++)
                {
                    cargo = funciones.obtener_dato(lista_de_empleado.Rows[fila]["cargo"].ToString(), posicion);
                    lista_de_empleado.Rows[fila][cargo] = cargo;
                    posicion++;
                }
            }
        }
        #endregion
        #region metodos get/set
        public DataTable get_lista_de_empleado(string id_sucursal, DateTime fecha)
        {
            consultar_lista_de_empleado(id_sucursal);
            //limpiar_lista_empleados(fecha);
            return lista_de_empleado;
        }
        public DataTable get_lista_de_empleado_origen(string id_sucursal, DateTime fecha)
        {
            consultar_lista_de_empleado_origen(id_sucursal);
            //limpiar_lista_empleados(fecha);
            lista_de_empleado.Columns.Add("seleccionado", typeof(string));
            for (int fila = 0; fila <= lista_de_empleado.Rows.Count - 1; fila++)
            {
                lista_de_empleado.Rows[fila]["seleccionado"] = "0";
            }
            return lista_de_empleado;
        }
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            sucursales.DefaultView.Sort = "sucursal ASC";
            sucursales = sucursales.DefaultView.ToTable();
            return sucursales;
        }
        public DataTable get_sucursal(string sucursal_nombre)
        {
            consultar_sucursal(sucursal_nombre);
            return sucursal;
        }
        public DataTable get_empleado(string id_empleado)
        {
            consultar_empleado(id_empleado);
            return empleado;
        }
        #endregion
    }
}
