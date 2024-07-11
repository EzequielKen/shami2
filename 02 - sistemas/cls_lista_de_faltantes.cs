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
    public class cls_lista_de_faltantes
    {
        public cls_lista_de_faltantes(DataTable usuario_BD)
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

        DataTable productos_terminados;
        DataTable insumos;
        DataTable resumen;
        #endregion

        #region metodos consultas
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_tabla(base_de_datos, "proveedor_villaMaipu");
        }
        private void consultar_insumos()
        {
            insumos = consultas.consultar_insumos_fabrica_venta(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("faltante", typeof(string));
            resumen.Columns.Add("orden_tipo", typeof(int));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            int ultima_fila;
            for (int fila = 0; fila <= productos_terminados.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = productos_terminados.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = productos_terminados.Rows[fila]["producto"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto"] = productos_terminados.Rows[fila]["tipo_producto"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = "proveedor_villaMaipu";
                resumen.Rows[ultima_fila]["faltante"] = "N/A";
                resumen.Rows[ultima_fila]["orden_tipo"] = int.Parse(funciones.obtener_dato(productos_terminados.Rows[fila]["tipo_producto"].ToString(), 1));
            }

            for (int fila = 0; fila <= insumos.Rows.Count-1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = insumos.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = insumos.Rows[fila]["producto"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto"] = insumos.Rows[fila]["tipo_producto"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = "proveedor_villaMaipu";
                resumen.Rows[ultima_fila]["faltante"] = "N/A";
                resumen.Rows[ultima_fila]["orden_tipo"] = int.Parse(funciones.obtener_dato(insumos.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            resumen.DefaultView.Sort = "orden_tipo ASC";
            resumen = resumen.DefaultView.ToTable();
        }
        #endregion

        #region metodos get/set
        public DataTable get_lista_producto()
        {
            consultar_productos_terminados();
            consultar_insumos();
            llenar_tabla_resumen();
            return resumen;
        }
        #endregion
    }
}
