using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_analisis_de_ventas
    {
        public cls_analisis_de_ventas(DataTable usuario_BD)
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

        DataTable productos_proveedor;
        DataTable insumos_fabrica;
        DataTable resumen_productos;

        DataTable pedidos_del_mes;
        DataTable acuerdo_de_precios;
        DataTable resumen;

        #endregion

        #region metodos privados
        private void crear_tabla_resumen_productos()
        {
            resumen_productos = new DataTable();
            resumen_productos.Columns.Add("id", typeof(string));
            resumen_productos.Columns.Add("producto", typeof(string));
            resumen_productos.Columns.Add("tipo_producto", typeof(string));
            resumen_productos.Columns.Add("orden", typeof(int));
            resumen_productos.Columns.Add("proveedor", typeof(string));
        }
        private void llenar_tabla_resumen_productos()
        {
            crear_tabla_resumen_productos();
            int ultima_fila;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count-1; fila++)
            {
                resumen_productos.Rows.Add();
                ultima_fila = resumen_productos.Rows.Count - 1;
                resumen_productos.Rows[ultima_fila]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                resumen_productos.Rows[ultima_fila]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                resumen_productos.Rows[ultima_fila]["tipo_producto"] = productos_proveedor.Rows[fila]["tipo_producto"].ToString();
                resumen_productos.Rows[ultima_fila]["orden"] = int.Parse(funciones.obtener_dato(productos_proveedor.Rows[fila]["tipo_producto"].ToString(), 1));
                resumen_productos.Rows[ultima_fila]["proveedor"] = "proveedor_villaMaipu";
            }

            for (int fila = 0; fila <= insumos_fabrica.Rows.Count-1; fila++)
            {
                resumen_productos.Rows.Add();
                ultima_fila = resumen_productos.Rows.Count - 1;
                resumen_productos.Rows[ultima_fila]["id"] = insumos_fabrica.Rows[fila]["id"].ToString();
                resumen_productos.Rows[ultima_fila]["producto"] = insumos_fabrica.Rows[fila]["producto"].ToString();
                resumen_productos.Rows[ultima_fila]["tipo_producto"] = insumos_fabrica.Rows[fila]["tipo_producto"].ToString();
                resumen_productos.Rows[ultima_fila]["orden"] = int.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila]["tipo_producto"].ToString(), 1));
                resumen_productos.Rows[ultima_fila]["proveedor"] = "insumos_fabrica";
            }
            resumen_productos.DefaultView.Sort = "orden ASC";
            resumen_productos = resumen_productos.DefaultView.ToTable();
        }   
        #endregion

        #region metodos consultas
        private void consultar_productos_proveedor()
        {
            productos_proveedor = consultas.consultar_productos_proveedor_productos_terminados(base_de_datos, "proveedor_villaMaipu");

        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_insumos_fabrica_venta(base_de_datos, "insumos_fabrica");
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios");
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor()
        {
            consultar_productos_proveedor();
            consultar_insumos_fabrica();
            llenar_tabla_resumen_productos();
            return resumen_productos;
        }
        #endregion
    }
}
