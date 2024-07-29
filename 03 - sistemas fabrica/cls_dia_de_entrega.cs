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
    public class cls_dia_de_entrega
    {
        public cls_dia_de_entrega(DataTable usuario_BD)
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
        DataTable dias_de_entrega;
        DataTable usuarios;
        #endregion

        #region carga a base de datos
        public void cargar_dias(DataTable resumen)
        {
            for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
            {
                if (int.Parse(resumen.Rows[fila]["id"].ToString()) < 1)
                {
                    //cargar
                    cargar_dia(resumen,fila);
                }
                else
                {
                    //actualizar
                    actualizar_dia(resumen,fila);
                }
            }
        }
        private void actualizar_dia(DataTable resumen, int fila_resumen)
        {
            string id = resumen.Rows[fila_resumen]["id"].ToString();
            string actualizar;
            for (int columna = resumen.Columns["lunes"].Ordinal; columna <resumen.Columns.Count-1; columna++)
            {
                actualizar = "`" + resumen.Columns[columna].ColumnName +"` = '" + resumen.Rows[fila_resumen][columna].ToString() +"'";
                consultas.actualizar_tabla(base_de_datos, "dias_de_entrega",actualizar,id);
                
            }
        }
        private void cargar_dia(DataTable resumen, int fila_resumen)
        {
            string columna = "";
            string valores = "";
            //lunes
            columna = funciones.armar_query_columna(columna,"lunes",false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["lunes"].ToString(),false);
            //martes
            columna = funciones.armar_query_columna(columna, "martes", false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["martes"].ToString(), false);
            //miercoles
            columna = funciones.armar_query_columna(columna, "miercoles", false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["miercoles"].ToString(), false);
            //jueves
            columna = funciones.armar_query_columna(columna, "jueves", false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["jueves"].ToString(), false);
            //viernes
            columna = funciones.armar_query_columna(columna, "viernes", false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["viernes"].ToString(), false);
            //sabado
            columna = funciones.armar_query_columna(columna, "sabado", false);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["sabado"].ToString(), false);
            //domingo
            columna = funciones.armar_query_columna(columna, "domingo", true);
            valores = funciones.armar_query_valores(valores, resumen.Rows[fila_resumen]["domingo"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "dias_de_entrega",columna,valores);
        }
        #endregion

        #region metodos consultas
        private void consultar_usuarios()
        {
            usuarios = consultas.consultar_usuarios_no_admin();
        }
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla_completa(base_de_datos, "sucursal");
        }
        private void consltar_dias_de_entrega()
        {
            dias_de_entrega = consultas.consultar_tabla_completa(base_de_datos, "dias_de_entrega");
        }
        #endregion

        #region metodos get/set
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        public DataTable get_dias_de_entrega()
        {
            consltar_dias_de_entrega();
            
            return dias_de_entrega;
        }
        public DataTable get_usuarios()
        {
            consultar_usuarios();
            usuarios.DefaultView.Sort = "usuario ASC";
            usuarios = usuarios.DefaultView.ToTable();
            return usuarios;
        }
        #endregion
    }
}
