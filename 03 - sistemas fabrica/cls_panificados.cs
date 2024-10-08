using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]

    public class cls_panificados
    {
        public cls_panificados(DataTable usuario_BD)
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
            stock_insumos = new cls_stock_insumos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_PDF PDF = new cls_PDF();
        cls_funciones funciones = new cls_funciones();
        cls_stock_insumos stock_insumos;
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos_proveedor;
        DataTable tabla_resumen;
        #endregion

        #region carga a base de datos
        public void actualizar_stock(DataTable resumen)
        {
            bool actualizo = false;
            for (int fila = 0; fila <= resumen.Rows.Count - 1; fila++)
            {
                string id_producto, actualizar, dato;
                if (resumen.Rows[fila]["stock_nuevo"].ToString() != "N/A")
                {
                    actualizo = true;
                    dato = resumen.Rows[fila]["dato"].ToString();
                    dato = dato + "-" + resumen.Rows[fila]["stock_nuevo"].ToString() + "-" + resumen.Rows[fila]["stock_nuevo"].ToString();
                    id_producto = resumen.Rows[fila]["id"].ToString();
                    actualizar = "`producto_1` = '" + dato + "'";
                    consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_producto);
                }
            }
            if (actualizo)
            {
             //   stock_insumos.actualizar_stock_insumos();
            }
        }
        #endregion
        #region tabla resumen
        private void crear_tabla_resumen()
        {
            tabla_resumen = new DataTable();
            tabla_resumen.Columns.Add("id", typeof(string));
            tabla_resumen.Columns.Add("producto", typeof(string));
            tabla_resumen.Columns.Add("unidad_de_medida", typeof(string));
            tabla_resumen.Columns.Add("stock_expedicion", typeof(string));
            tabla_resumen.Columns.Add("stock", typeof(string));
            tabla_resumen.Columns.Add("stock_nuevo", typeof(string));
            tabla_resumen.Columns.Add("dato", typeof(string));
        }
        private void llenar_tabla_resumen()
        {
            crear_tabla_resumen();
            string tipo_paquete, unidad, tipo_unidad;
            int index = 0;
            for (int fila = 0; fila <= insumos_proveedor.Rows.Count - 1; fila++)
            {
                if (insumos_proveedor.Rows[fila]["tipo_producto"].ToString() == "3-Panificados")
                {
                    tabla_resumen.Rows.Add();
                    tabla_resumen.Rows[index]["id"] = insumos_proveedor.Rows[fila]["id"].ToString();
                    tabla_resumen.Rows[index]["producto"] = insumos_proveedor.Rows[fila]["producto"].ToString();
                    tabla_resumen.Rows[index]["unidad_de_medida"] = insumos_proveedor.Rows[fila]["unidad_de_medida_local"].ToString();
                    tabla_resumen.Rows[index]["stock"] = funciones.obtener_dato(insumos_proveedor.Rows[fila]["producto_1"].ToString(), 4);
                    tabla_resumen.Rows[index]["stock_nuevo"] = "N/A";
                    tipo_paquete = funciones.obtener_dato(insumos_proveedor.Rows[fila]["producto_1"].ToString(), 1);
                    unidad = funciones.obtener_dato(insumos_proveedor.Rows[fila]["producto_1"].ToString(), 2);
                    tipo_unidad = funciones.obtener_dato(insumos_proveedor.Rows[fila]["producto_1"].ToString(), 3);
                    tabla_resumen.Rows[index]["dato"] = tipo_paquete + "-" + unidad + "-" + tipo_unidad;

                    index++;
                }
            }
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_proveedor()
        {
            insumos_proveedor = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_panificados()
        {
            consultar_insumos_proveedor();
            llenar_tabla_resumen();
            return tabla_resumen;
        }
        #endregion
    }
}
