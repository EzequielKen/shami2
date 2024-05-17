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
    public class cls_pedir_insumos
    {
        public cls_pedir_insumos(DataTable usuario_BD)
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
        DataTable sub_productos;
        #endregion

        #region carga a base de datos
        public string cargar_pedido_de_insumos(DataTable resumen, DataTable tipo_usuario)
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
            int index = 1;
            for (int fila = 0; fila < resumen.Rows.Count - 1; fila++)
            {
                id = resumen.Rows[fila]["id"].ToString();
                producto = resumen.Rows[fila]["producto"].ToString();
                cantidad = resumen.Rows[fila]["cantidad"].ToString();
                unidad_medida = resumen.Rows[fila]["unidad_medida"].ToString();

                dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida + "-N/A";

                columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);

                index++;
            }
            int ultima_fila = resumen.Rows.Count - 1;
            id = resumen.Rows[ultima_fila]["id"].ToString();
            producto = resumen.Rows[ultima_fila]["producto"].ToString();
            cantidad = resumen.Rows[ultima_fila]["cantidad"].ToString();
            unidad_medida = resumen.Rows[ultima_fila]["unidad_medida"].ToString();

            dato = id + "-" + producto + "-" + cantidad + "-" + unidad_medida + "-N/A";

            columna = funciones.armar_query_columna(columna, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "pedido_de_insumos", columna, valores);
            return whatsapp.notificar_nuevo_pedido_de_insumo();
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        private void consultar_sub_productos()
        {
            sub_productos = consultas.consultar_sub_productos();
        }
        #endregion

        #region metodos privados
        private void combinar_tablas()
        {
            int ultima_fila;
            for (int fila = 0; fila <= sub_productos.Rows.Count-1; fila++)
            {
                insumos_fabrica.Rows.Add();
                ultima_fila = insumos_fabrica.Rows.Count-1;

                insumos_fabrica.Rows[ultima_fila]["id"] = sub_productos.Rows[fila]["id"].ToString();
                insumos_fabrica.Rows[ultima_fila]["producto"] = sub_productos.Rows[fila]["producto"].ToString(); 
                insumos_fabrica.Rows[ultima_fila]["unidad_medida"] = sub_productos.Rows[fila]["unidad_de_medida_local"].ToString(); 
                insumos_fabrica.Rows[ultima_fila]["tipo_producto"] = sub_productos.Rows[fila]["tipo_producto"].ToString();
            }
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            consultar_sub_productos();
            combinar_tablas();
            return insumos_fabrica;
        }
        #endregion
    }
}
