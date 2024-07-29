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

    public class cls_orden_de_pedido
    {
        public cls_orden_de_pedido(DataTable usuario_BD)
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
        cls_whatsapp whatsapp = new cls_whatsapp();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos_fabrica;
        #endregion

        #region carga a base de datos
        public string cargar_orden_pedido(DataTable resumen, DataTable tipo_usuario)
        {
            string columna = "";
            string valores = "";
            //solicita
            columna = funciones.armar_query_columna(columna, "solicita", false);
            valores = funciones.armar_query_valores(valores, tipo_usuario.Rows[0]["rol"].ToString(), false);
            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            //producto_1
            string dato, id, producto, cantidad, unidad_medida;
            int index =1;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                producto = resumen.Rows[fila]["producto"].ToString();
                cantidad = resumen.Rows[fila]["cantidad"].ToString();
                unidad_medida = resumen.Rows[fila]["unidad_medida"].ToString();

                dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida + "-No pedido-N/A-N/A";

                columna = funciones.armar_query_columna(columna, "producto_"+index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);

                index ++;
            }
            int ultima_fila = resumen.Rows.Count-1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            producto = resumen.Rows[ultima_fila]["producto"].ToString();
            cantidad = resumen.Rows[ultima_fila]["cantidad"].ToString();
            unidad_medida = resumen.Rows[ultima_fila]["unidad_medida"].ToString();

            dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida + "-No pedido-N/A-N/A";

            columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "orden_de_pedido", columna,valores);
            return whatsapp.notificar_nueva_orden_de_pedido();
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count-1; fila++)
            {
                insumos_fabrica.Rows[fila]["unidad_medida"] = "N/A";
            }
            return insumos_fabrica;
        }
        #endregion
    }
}
