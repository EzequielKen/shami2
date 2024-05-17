using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public  class cls_crear_proveedor
    {
        public cls_crear_proveedor(DataTable usuario_BD)
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
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        #endregion

        #region metodos publicos
        public void crear_proveedor(string proveedor, string provincia, string localidad, string direccion, string telefono, string condicion_pago, string CBU_1, string CBU_2, string CBU_3, string CBU_4, string CBU_5)
        { 
            string columnas="";
            string valores="";
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor",false);
            valores = armar_query_valores(valores, proveedor,false);
            //provincia
            columnas = armar_query_columna(columnas, "provincia", false);
            valores = armar_query_valores(valores, provincia, false);
            //localidad
            columnas = armar_query_columna(columnas, "localidad", false);
            valores = armar_query_valores(valores, localidad, false);
            //direccion 
            columnas = armar_query_columna(columnas, "direccion", false);
            valores = armar_query_valores(valores, direccion, false);
            //condicion_pago
            if (condicion_pago != String.Empty)
            {
                columnas = armar_query_columna(columnas, "condicion_pago", false);
                valores = armar_query_valores(valores, condicion_pago, false);
            }
            //CBU_1
            if (CBU_1!=String.Empty)
            {
                columnas = armar_query_columna(columnas, "CBU_1", false);
                valores = armar_query_valores(valores, CBU_1, false);
            }
            //CBU_2
            if (CBU_2 != String.Empty)
            {
                columnas = armar_query_columna(columnas, "CBU_2", false);
                valores = armar_query_valores(valores, CBU_2, false);
            }
            //CBU_3
            if (CBU_3 != String.Empty)
            {
                columnas = armar_query_columna(columnas, "CBU_3", false);
                valores = armar_query_valores(valores, CBU_3, false);
            }
            //CBU_4
            if (CBU_4 != String.Empty)
            {
                columnas = armar_query_columna(columnas, "CBU_4", false);
                valores = armar_query_valores(valores, CBU_4, false);
            }
            //CBU_5
            if (CBU_5 != String.Empty)
            {
                columnas = armar_query_columna(columnas, "CBU_5", false);
                valores = armar_query_valores(valores, CBU_5, false);
            }
            //telefono
            columnas = armar_query_columna(columnas, "telefono", true);
            valores = armar_query_valores(valores, telefono, true);

            consultas.insertar_en_tabla(base_de_datos, "proveedores_de_fabrica",columnas,valores); 
        }
        #endregion

        private string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
        {
            string retorno = "";
            string separador_columna = "`";

            retorno = columnas + separador_columna + columna_valor + separador_columna;

            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        private string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
        {
            string retorno = "";
            string separador_valores = "'";

            retorno = valores + separador_valores + valor_a_insertar + separador_valores;
            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
    }
}
