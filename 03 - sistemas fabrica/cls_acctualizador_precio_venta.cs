using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_acctualizador_precio_venta
    {
        public cls_acctualizador_precio_venta(DataTable usuario_BD)
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

        DataTable productos;
        DataTable precio_venta;
        DataTable tipo_acuerdo;
        #endregion

        #region metodos consultas
        private void consultar_productos()
        {
            productos = consultas.consultar_tabla(base_de_datos,"productos");
        }
        private void consultar_precio_venta(string proveedor, string tipo_de_acuerdo)
        {
            precio_venta = consultas.consultar_precio_venta_activo(proveedor,tipo_de_acuerdo);
        }
        private void consultar_tipo_acuerdo()
        {
            tipo_acuerdo = consultas.consultar_tabla(base_de_datos,"tipo_acuerdo");
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos(string proveedor, string tipo_de_acuerdo)
        {
            consultar_productos();
            consultar_precio_venta(proveedor, tipo_de_acuerdo);

            return productos;
        }
        public DataTable get_tipo_acuerdo()
        {
            consultar_tipo_acuerdo();
            DataTable resumen = new DataTable();
            resumen.Columns.Add("tipo_de_acuerdo",typeof(string));
            int fila_resumen=0;
            string tipo_de_acuerdo;
            for (int fila = 0; fila <= tipo_acuerdo.Rows.Count-1; fila++)
            {
                tipo_de_acuerdo = tipo_acuerdo.Rows[fila]["proveedor_villaMaipu"].ToString();
                fila_resumen = funciones.buscar_fila_por_dato_en_columna(tipo_de_acuerdo, "tipo_de_acuerdo",resumen);
                if (fila==-1)
                {
                    resumen.Rows.Add();
                    resumen.Rows[resumen.Rows.Count - 1]["tipo_de_acuerdo"]=tipo_de_acuerdo;
                }
            }
            return resumen;
        }
        #endregion
    }
}
