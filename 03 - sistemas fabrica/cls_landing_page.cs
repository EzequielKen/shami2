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

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_landing_page
    {
        public cls_landing_page(DataTable usuario_BD)
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

        DataTable descripcion_de_cargos;
        DataTable registro_actividad_de_empleado;
        #endregion

        #region carga a base de datos
        public void registrar_actividad(string id_actividad)
        {
            string columna = "";
            string valores = "";


            //id_actividad
            columna = funciones.armar_query_columna(columna, "id_actividad", false);
            valores = funciones.armar_query_valores(valores, id_actividad, false);

            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", true);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), true);

            consultas.insertar_en_tabla(base_de_datos, "registro_actividad_de_empleado", columna, valores);
        }
        #endregion

        #region metodos consultas
        public void consultar_registro_actividad_de_empleado()
        {
            registro_actividad_de_empleado = consultas.consultar_registro_actividad_del_dia(base_de_datos, "registro_actividad_de_empleado");
        }
        private void consultar_descripcion_de_cargos(string rol)
        {
            descripcion_de_cargos = consultas.consultar_descripcion_de_cargos(base_de_datos, rol);
        }
        #endregion

        #region metodos get/set
        public string get_redirect(string id)
        {

            int fila = funciones.buscar_fila_por_id(id, descripcion_de_cargos);
            return descripcion_de_cargos.Rows[fila]["redirect"].ToString();
        }
        public DataTable get_descripcion_de_cargos(string rol)
        {
            consultar_descripcion_de_cargos(rol);
            descripcion_de_cargos.DefaultView.Sort = "prioridad ASC";
            descripcion_de_cargos = descripcion_de_cargos.DefaultView.ToTable();
            return descripcion_de_cargos;
        }
        public bool verificar_si_registro(string id_actividad)
        {
            if (registro_actividad_de_empleado == null)
            {
                consultar_registro_actividad_de_empleado();
            }
            bool retorno = false;
            for (int fila = 0; fila <= registro_actividad_de_empleado.Rows.Count - 1; fila++)
            {
                if (id_actividad == registro_actividad_de_empleado.Rows[fila]["id_actividad"].ToString())
                {
                    retorno = true;
                    break;
                }
            }
            return retorno;
        }
        public bool verificar_correltividad(string id_actividad)
        {
            bool retorno = false;
            if (id_actividad == "N/A")
            {
                retorno = true;
            }
            else if (verificar_si_registro(id_actividad))
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion
    }
}
