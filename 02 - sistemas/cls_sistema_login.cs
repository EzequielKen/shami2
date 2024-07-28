using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using modulos;
using paginaWeb;
namespace _02___sistemas
{
    public class cls_sistema_login
    {
        public cls_sistema_login()
        {
            ip = ConfigurationManager.AppSettings["ip"];
            puerto = ConfigurationManager.AppSettings["puerto"];
            user = ConfigurationManager.AppSettings["user"];
            userPasword = ConfigurationManager.AppSettings["userPasword"];

            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }

            consultas_Mysql = new cls_consultas_Mysql(ip, puerto, user, userPasword, base_de_datos);

        }
        #region atributos
        cls_consultas_Mysql consultas_Mysql;
        cls_funciones funciones = new cls_funciones();
        string ip, puerto, user, userPasword, base_de_datos;
        DataTable usuario;
        DataTable empleado;
        DataTable sucursal;
        DataTable tipo_usuario;
        DataTable proveedor;
        #endregion

        #region metodos
        public bool login(string usuario, string contraseña)
        {
            bool retorno = false;

            try
            {
                this.usuario = consultas_Mysql.login(usuario, contraseña);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            if (this.usuario.Rows.Count > 0)
            {
                if ((this.usuario.Rows[0]["usuario"].ToString() == usuario && this.usuario.Rows[0]["contraseña"].ToString() == contraseña))
                {
                    retorno = true;
                    int id = int.Parse(this.usuario.Rows[0]["sucursal"].ToString());
                    consultar_sucursal(id);
                    id = int.Parse(this.usuario.Rows[0]["tipo_usuario"].ToString());
                    consultar_tipo_usuario(id);

                }

            }
            return retorno;
        }
        public bool login_empleado(string usuario, string contraseña)
        {
            bool retorno = false;

            try
            {
                this.empleado = consultas_Mysql.login_empleado(contraseña);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            if (this.empleado.Rows.Count > 0)
            {
                DataTable sucursal_consultada = consultas_Mysql.login_consultar_sucursal(usuario);
                if (sucursal_consultada.Rows.Count > 0)
                {
                    string actualizar;

                    string turno_logueado = verificar_horario_logueo(empleado.Rows[0]["turno_logueado"].ToString());
                    string id_empleado = this.empleado.Rows[0]["id"].ToString();
                    if (turno_logueado != empleado.Rows[0]["turno_logueado"].ToString())
                    {
                        actualizar = "`turno_logueado` = 'N/A'";
                        consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);
                        empleado.Rows[0]["turno_logueado"] = "N/A";
                    }
                    int id_sucursal = int.Parse(sucursal_consultada.Rows[0]["id"].ToString());
                    actualizar = "`id_sucursal` = '" + id_sucursal.ToString() + "'";
                    consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);

                    turno_logueado = verificar_horario_logueo(empleado.Rows[0]["turno_logueado"].ToString());

                    if (empleado.Rows[0]["turno_logueado"].ToString() == "N/A")
                    {
                        actualizar = "`fecha_logueo` = '" + funciones.get_fecha() + "'";
                        consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);

                        actualizar = "`turno_logueado` = '" + turno_logueado + "'";
                        consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);
                    }

                    this.empleado = consultas_Mysql.login_empleado(contraseña);
                    id_sucursal = int.Parse(this.empleado.Rows[0]["id_sucursal"].ToString());
                    consultar_sucursal(id_sucursal);
                    consultar_usuario_segun_sucursal(id_sucursal.ToString());
                    string id_usuario = this.usuario.Rows[0]["id"].ToString();


                    retorno = true;
                }
            }
            return retorno;
        }
        public void actualizar_turno_empleado(string id_empleado,string turno_logueado)
        {
            string actualizar = "`turno_logueado` = '" + turno_logueado + "'";
            consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);
        }
        public void cerrar_turno(DataTable empleado_logueado)
        {
            string actualizar;
            string id_empleado = empleado_logueado.Rows[0]["id"].ToString();

            actualizar = "`turno_logueado` = 'N/A'";
            consultas_Mysql.actualizar_tabla(base_de_datos, "lista_de_empleado", actualizar, id_empleado);
        }
        public void registrar_logueo_empleado(DataTable empleado_logueado)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //id_empleado
            columnas = funciones.armar_query_columna(columnas, "id_empleado", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["id"].ToString(), false);
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["id_sucursal"].ToString(), false);

            //nombre
            columnas = funciones.armar_query_columna(columnas, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["nombre"].ToString(), false);

            //apellido
            columnas = funciones.armar_query_columna(columnas, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["apellido"].ToString(), false);

            //dni
            columnas = funciones.armar_query_columna(columnas, "dni", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["dni"].ToString(), false);

            //telefono
            columnas = funciones.armar_query_columna(columnas, "telefono", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["telefono"].ToString(), false);

            //cargo
            columnas = funciones.armar_query_columna(columnas, "cargo", false);
            valores = funciones.armar_query_valores(valores, empleado_logueado.Rows[0]["cargo"].ToString(), false);

            //fecha_logueo
            columnas = funciones.armar_query_columna(columnas, "fecha_logueo", true);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), true);

            consultas_Mysql.insertar_en_tabla(base_de_datos, "registro_logueo_empleados", columnas, valores);
        }
        #endregion

        #region metodos privados
        private string verificar_horario_logueo(string turno_registrado)
        {
            string retorno = "N/A";
            DateTime miFecha = DateTime.Now;

            // Definir los límites de tiempo para Turno 1
            DateTime horario_normal_inicio = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 7, 0, 0);
            DateTime horario_normal_fin = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 16, 59, 59);

            // Definir los límites de tiempo para brecha horaria 
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 7:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 18, 59, 59); // 5:00 PM

            // Definir los límites de tiempo para Turno 2
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 5:00 PM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 59, 59); // 11:59 PM
            DateTime horaFin_rango2_extendido = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 4, 59, 59).AddDays(1); // 4:59 AM del siguiente día
            DateTime horaInicio_rango1_extendido = miFecha.Date.AddDays(-1).AddHours(17);

            if ((miFecha >= horario_normal_inicio && miFecha <= horario_normal_fin))
            {
                retorno = "Turno 1";
            }
            else if ((miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1) && (turno_registrado == "Turno 1"))
            {
                retorno = "Turno 1";
            }
            else if ((miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2) ||
                (miFecha >= horaInicio_rango1_extendido && miFecha <= horaFin_rango2_extendido))
            {
                retorno = "Turno 2";
            }


            return retorno;
        }

        private string verificar_horario(DateTime miFecha)
        {
            string retorno = "fuera de rango";

            // Definir los límites de tiempo para Turno 1
            DateTime horaInicio_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 7, 0, 0); // 7:00 AM
            DateTime horaFin_rango1 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 5:00 PM
            if (miFecha >= horaInicio_rango1 && miFecha <= horaFin_rango1)
            {
                retorno = "Turno 1";
            }
            // Definir los límites de tiempo para Turno 2
            DateTime horaInicio_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0); // 5:00 PM
            DateTime horaFin_rango2 = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 23, 59, 59); // 11:59 PM
            DateTime horaFin_rango2_extendido = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 4, 59, 59).AddDays(1); // 4:59 AM del siguiente día

            if ((miFecha >= horaInicio_rango2 && miFecha <= horaFin_rango2) ||
                (miFecha >= miFecha.Date.AddDays(-1).AddHours(17) && miFecha <= horaFin_rango2_extendido && retorno != "Turno 1"))
            {
                retorno = "Turno 2";
            }
            return retorno;
        }
        public bool verificar_brecha_horaria(DateTime miFecha)
        {
            bool retorno = false;

            // Definir los límites de tiempo para Turno 1
            DateTime horario_normal_inicio = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 17, 0, 0);
            DateTime horario_normal_fin = new DateTime(miFecha.Year, miFecha.Month, miFecha.Day, 18, 59, 59);

            if ((miFecha >= horario_normal_inicio && miFecha <= horario_normal_fin))
            {
                retorno = true;
            }

            return retorno;
        }
        private bool verificar_fecha(DateTime mi_fecha, DateTime fecha_hoy)
        {
            bool retorno = true;
            if (mi_fecha.Year != fecha_hoy.Year)
            {
                retorno = false;
            }
            else
            {
                if (fecha_hoy.Day != mi_fecha.Day || fecha_hoy.Month != mi_fecha.Month)
                {
                    retorno = false;
                }
            }
            return retorno;
        }
        private void consultar_proveedor_id(string id)
        {
            proveedor = consultas_Mysql.consultar_proveeedor_id(id);
        }
        private void consultar_sucursal(int id)
        {
            sucursal = consultas_Mysql.consultar_sucursal(id);
        }
        private void consultar_tipo_usuario(int id)
        {
            tipo_usuario = consultas_Mysql.consultar_tipo_usuario(id);
        }
        private void consultar_usuario_segun_sucursal(string id_sucursal)
        {
            usuario = consultas_Mysql.consultar_usuario_segun_sucural(id_sucursal);
        }
        #endregion

        #region metodos get
        public DataTable get_proveedor_seleccionado(string id_proveedor)
        {
            consultar_proveedor_id(id_proveedor);
            return proveedor;
        }
        public DataTable get_usuarios()
        {

            return usuario;
        }
        public DataTable get_sucursal()
        {
            
            return sucursal;
        }
        public DataTable get_tipo_usuario()
        {
            return tipo_usuario;
        }
        public DataTable get_empleado()
        {
            return empleado;
        }
        public string get_estado_encargado(DataTable empleado_logueado)
        {
            string retorno = "No";
            string cargo = string.Empty;
            string cargos = empleado_logueado.Rows[0]["cargo"].ToString();
            int iteraciones = int.Parse(funciones.obtener_dato(cargos, 1));
            for (int i = 1; i <= iteraciones; i++)
            {
                cargo = funciones.obtener_dato(cargos, i);
                if (cargo == "Encargado")
                {
                    retorno ="Si";
                    break;
                }
            }
            return retorno;
        }
        #endregion
    }
}
