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
    public class cls_desperdicio_merma
    {
        public cls_desperdicio_merma(DataTable usuario_BD)
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
        cls_sistema_cuentas_por_pagar cuentas_por_pagar;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_terminados;
        DataTable insumos;
        #endregion

        #region carga a base de datos
        public void registrar_merma_desperdicio(DataTable sucursal, DataTable producto, int fila_producto, string cantidad,string nota,string proveedor,string categoria)
        {
            string columna="";
            string valor="";
            //id_sucursal
            columna = funciones.armar_query_columna(columna, "id_sucursal",false);
            valor = funciones.armar_query_valores(valor, sucursal.Rows[0]["id"].ToString(), false);
            //sucursal
            columna = funciones.armar_query_columna(columna, "sucursal", false);
            valor = funciones.armar_query_valores(valor, sucursal.Rows[0]["sucursal"].ToString(), false);
            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", false);
            valor = funciones.armar_query_valores(valor, funciones.get_fecha(), false);
            //id_producto
            columna = funciones.armar_query_columna(columna, "id_producto", false);
            valor = funciones.armar_query_valores(valor, producto.Rows[fila_producto]["id"].ToString(), false);
            //producto
            columna = funciones.armar_query_columna(columna, "producto", false);
            valor = funciones.armar_query_valores(valor, producto.Rows[fila_producto]["producto"].ToString(), false);
            //cantidad
            columna = funciones.armar_query_columna(columna, "cantidad", false);
            valor = funciones.armar_query_valores(valor, cantidad, false);
            //nota
            columna = funciones.armar_query_columna(columna, "nota", false);
            valor = funciones.armar_query_valores(valor, nota, false);
            //proveedor
            columna = funciones.armar_query_columna(columna, "proveedor", false);
            valor = funciones.armar_query_valores(valor, proveedor, false);
            //categoria
            columna = funciones.armar_query_columna(columna, "categoria", true);
            valor = funciones.armar_query_valores(valor, categoria, true);

            consultas.insertar_en_tabla(base_de_datos, "desperdicio_merma_local", columna, valor);
        }
        #endregion

        #region metodos consultas
        private void consultar_productos_terminados()
        {
            productos_terminados = consultas.consultar_tabla(base_de_datos, "proveedor_villamaipu");
        }

        private void consultar_insumos()
        {
            insumos = consultas.consultar_insumos_fabrica_venta(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_terminados()
        {
            consultar_productos_terminados();
            productos_terminados.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= productos_terminados.Rows.Count-1; fila++)
            {
                productos_terminados.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(productos_terminados.Rows[fila]["tipo_producto"].ToString(),1));
            }
            return productos_terminados;
        }
        public DataTable get_insumos()
        {
            consultar_insumos();
            insumos.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                insumos.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(insumos.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            return insumos;
        }
        #endregion
    }
}
