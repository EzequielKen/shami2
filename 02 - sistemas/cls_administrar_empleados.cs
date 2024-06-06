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
    public class cls_administrar_empleados
    {
        public cls_administrar_empleados(DataTable usuario_BD)
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

        DataTable lista_de_empleado;
        #endregion

        #region carga a base de datos
        public void registrar_empleado(DataTable empleado)
        {
            string columna="";
            string valores="";

            //id_sucursal
            columna = funciones.armar_query_columna(columna,"id_sucursal",false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["id_sucursal"].ToString(),false);
            //nombre
            columna = funciones.armar_query_columna(columna, "nombre", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["nombre"].ToString(), false);
            //apellido
            columna = funciones.armar_query_columna(columna, "apellido", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["apellido"].ToString(), false);
            //dni
            columna = funciones.armar_query_columna(columna, "dni", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["dni"].ToString(), false);
            //telefono
            columna = funciones.armar_query_columna(columna, "telefono", false);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["telefono"].ToString(), false);
            //cargo
            columna = funciones.armar_query_columna(columna, "cargo", true);
            valores = funciones.armar_query_valores(valores, empleado.Rows[0]["cargo"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "lista_de_empleado",columna,valores);
        }
        #endregion
        #region metodos consultas
        private void consultar_lista_de_empleado()
        {
            lista_de_empleado = consultas.consultar_tabla_completa(base_de_datos, "lista_de_empleado");
        }
        #endregion
        #region metodos get/set
        public DataTable get_lista_de_empleado()
        {
            consultar_lista_de_empleado();
            return lista_de_empleado;
        }
        #endregion
    }
}
