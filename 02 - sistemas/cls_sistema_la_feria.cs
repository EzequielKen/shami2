using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01___modulos;
using modulos;

namespace _02___sistemas
{
    public class cls_sistema_la_feria
    {
        public cls_sistema_la_feria(DataTable usuarios_BD)
        {
            usuariosBD = usuarios_BD;
            servidor = usuariosBD.Rows[0]["servidor"].ToString();
            puerto = usuariosBD.Rows[0]["puerto"].ToString();
            usuario = usuariosBD.Rows[0]["usuario_BD"].ToString();
            password = usuariosBD.Rows[0]["contraseña_BD"].ToString();
            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }

            consultas = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
        }
        #region atributos
        cls_consultas_Mysql consultas;
        DataTable usuariosBD;
        DataTable productos_feria;
        DataTable ventas_feria;
        string servidor, puerto, usuario, password, base_de_datos;
        #endregion
        #region metodos publicos
        public void registrar_ventas(DataTable resumen_pedido, string sucursal)
        {
            string columna, valor;
            for (int fila = 0; fila <= resumen_pedido.Rows.Count - 1; fila++)
            {
                columna = "";
                valor = "";
                //sucursal
                columna = armar_query_columna(columna, "sucursal", false);
                valor =  armar_query_valores(valor, sucursal, false);
                //producto
                columna =  armar_query_columna(columna, "producto", false);
                valor = armar_query_valores(valor, resumen_pedido.Rows[fila]["producto"].ToString(), false);
                //cantidad
                columna =  armar_query_columna(columna, "cantidad", false);
                valor =  armar_query_valores(valor, resumen_pedido.Rows[fila]["cantidad"].ToString(), false);
                //precio
                columna = armar_query_columna(columna, "precio", false);
                valor = armar_query_valores(valor, resumen_pedido.Rows[fila]["precio"].ToString(), false);
                //sub_total
                columna = armar_query_columna(columna, "sub_total", false);
                valor = armar_query_valores(valor, resumen_pedido.Rows[fila]["sub_total"].ToString(), false);
                //fecha
                string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                columna =armar_query_columna(columna, "fecha", true);
                valor =  armar_query_valores(valor, fecha, true);

                consultas.insertar_en_tabla(base_de_datos, "ventas_feria", columna, valor);

            }
        }
        #endregion
        #region metodos consulta
        private void consultar_productos_feria()
        {
            productos_feria = consultas.consultar_tabla(base_de_datos, "productos_feria");
        }
        private void consultar_ventas_ferias(string sucursal)
        {
            ventas_feria = consultas.consultar_ventas_feria(base_de_datos, "ventas_feria", sucursal);
        }
        #endregion

        #region metodos get/set
        public DataTable get_ventas_feria(string sucursal)
        {
            consultar_ventas_ferias(sucursal);
            return ventas_feria;
        }
        public DataTable get_productos_feria()
        {
            consultar_productos_feria();
            return productos_feria;
        }
        #endregion

        #region query
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
        #endregion
    }
}
