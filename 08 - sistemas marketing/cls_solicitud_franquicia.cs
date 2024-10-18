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

namespace _08___sistemas_marketing
{
    public class cls_solicitud_franquicia
    {
        public cls_solicitud_franquicia(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable solicitud_franquicia;
        DataTable solicitud;
        #endregion

        #region metodos consultas
        private void consultar_solicitud_franquicia(string mes, string año)
        {
            solicitud_franquicia = consultas.consultar_solicitud_franquicia_segun_fecha(mes,año);
        }
        private void consultar_solicitud(string id)
        {
            solicitud = consultas.consultar_solicitud_franquicia_por_id(id);
        }
        #endregion

        #region metodos get/set
        public DataTable get_solicitud_franquicia(string mes,string año)
        {
            consultar_solicitud_franquicia(mes,año);
            return solicitud_franquicia;
        }
        public DataTable get_solicitud(string id)
        {
            consultar_solicitud(id);
            return solicitud;
        }
        #endregion
    }
}
