using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    public class cls_crear_insumos
    {
        public cls_crear_insumos(DataTable usuario_BD)
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

        DataTable insumos_fabrica;
        DataTable productos_terminado;
        #endregion

        #region carga a base de datos
        public void crear_insumo(DataTable insumo)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            //producto
            columnas = funciones.armar_query_columna(columnas, "producto", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["producto"].ToString(), false);
            //tipo_producto
            columnas = funciones.armar_query_columna(columnas, "tipo_producto", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["tipo_producto"].ToString(), false);
            //alimento
            columnas = funciones.armar_query_columna(columnas, "alimento", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["alimento"].ToString(), false);
            //bebida
            columnas = funciones.armar_query_columna(columnas, "bebida", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["bebida"].ToString(), false);
            //descartable
            columnas = funciones.armar_query_columna(columnas, "descartable", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["descartable"].ToString(), false);
            //tipo_producto_local
            columnas = funciones.armar_query_columna(columnas, "tipo_producto_local", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["tipo_producto_local"].ToString(), false);
            //unidad_tabla_produccion
            columnas = funciones.armar_query_columna(columnas, "unidad_tabla_produccion", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["unidad_tabla_produccion"].ToString(), false);
            //venta
            columnas = funciones.armar_query_columna(columnas, "venta", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["venta"].ToString(), false);
            //productos_fabrica_fatay
            columnas = funciones.armar_query_columna(columnas, "productos_fabrica_fatay", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["productos_fabrica_fatay"].ToString(), false);
            //productos_caballito
            columnas = funciones.armar_query_columna(columnas, "productos_caballito", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["productos_caballito"].ToString(), false);
            //unidad_de_medida_local
            columnas = funciones.armar_query_columna(columnas, "unidad_de_medida_local", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["unidad_de_medida_local"].ToString(), false);
            //equivalencia
            columnas = funciones.armar_query_columna(columnas, "equivalencia", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["equivalencia"].ToString(), false);
            //unidad_medida
            columnas = funciones.armar_query_columna(columnas, "unidad_medida", false);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["unidad_medida"].ToString(), false);
            //tabla_produccion
            columnas = funciones.armar_query_columna(columnas, "tabla_produccion", true);
            valores = funciones.armar_query_valores(valores, insumo.Rows[0]["tabla_produccion"].ToString(), true);

            consultas.insertar_en_tabla(base_de_datos, "insumos_fabrica", columnas, valores);

            // consultar_insumos_fabrica();

            //  DataTable acuerdo_de_precios = consultas.consultar_tabla(base_de_datos, "acuerdo_de_precios");
            //  int ultima_columna = acuerdo_de_precios.Columns.Count - 1;
            //  string nombre_ultima_columna = acuerdo_de_precios.Columns[ultima_columna].ColumnName;
            //
            //  int ultima_columna_tabla = int.Parse(nombre_ultima_columna.Replace("producto_", ""));
            //  int id_insumo = int.Parse(insumos_fabrica.Rows[insumos_fabrica.Rows.Count - 1]["id"].ToString());
            //  int nueva_columna_tabla = 0;
            //   while (ultima_columna_tabla <= id_insumo)
            //   {
            //       nueva_columna_tabla = ultima_columna_tabla + 1;
            //       string nueva_columna = "producto_" + nueva_columna_tabla.ToString();
            //       consultas.agregar_columna(base_de_datos, "acuerdo_de_precios", nueva_columna, "DOUBLE", "0");
            //       ultima_columna_tabla++;
            //   }
        }

        public void actualizar_dato(string id, string columna, string dato)
        {
            string actualizar = "`" + columna + "` = '" + dato + "'";
            consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id);
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla_completa(base_de_datos, "insumos_fabrica");
        }
        private void consultar_productos_terminados()
        {
            productos_terminado = consultas.consultar_tabla_completa(base_de_datos, "proveedor_villamaipu");
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            insumos_fabrica.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                insumos_fabrica.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            insumos_fabrica.DefaultView.Sort = "orden asc";
            insumos_fabrica = insumos_fabrica.DefaultView.ToTable();
            return insumos_fabrica;
        }
        public DataTable get_categorias()
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            DataTable categorias = new DataTable();
            categorias.Columns.Add("tipo_producto", typeof(string));

            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                if (int.TryParse(funciones.obtener_dato(insumos_fabrica.Rows[fila]["tipo_producto"].ToString(), 1), out int orden))
                {
                    categorias.Rows.Add();
                    categorias.Rows[categorias.Rows.Count - 1]["tipo_producto"] = insumos_fabrica.Rows[fila]["tipo_producto"].ToString();
                }
            }
            for (int fila = 0; fila <= productos_terminado.Rows.Count - 1; fila++)
            {
                if (int.TryParse(funciones.obtener_dato(productos_terminado.Rows[fila]["tipo_producto"].ToString(), 1), out int orden))
                {
                    categorias.Rows.Add();
                    categorias.Rows[categorias.Rows.Count - 1]["tipo_producto"] = productos_terminado.Rows[fila]["tipo_producto"].ToString();
                }
            }
            categorias.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= categorias.Rows.Count - 1; fila++)
            {
                string dato = categorias.Rows[fila]["tipo_producto"].ToString();
                categorias.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(categorias.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            categorias.DefaultView.Sort = "orden asc";
            categorias = categorias.DefaultView.ToTable();
            return categorias;
        }
        public string get_ultimo_num_categoria()
        {
            consultar_insumos_fabrica();
            consultar_productos_terminados();
            DataTable categorias = new DataTable();
            categorias.Columns.Add("tipo_producto", typeof(string));

            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                if (int.TryParse(funciones.obtener_dato(insumos_fabrica.Rows[fila]["tipo_producto"].ToString(), 1), out int orden))
                {
                    categorias.Rows.Add();
                    categorias.Rows[categorias.Rows.Count - 1]["tipo_producto"] = insumos_fabrica.Rows[fila]["tipo_producto"].ToString();
                }
            }
            for (int fila = 0; fila <= productos_terminado.Rows.Count - 1; fila++)
            {
                if (int.TryParse(funciones.obtener_dato(productos_terminado.Rows[fila]["tipo_producto"].ToString(), 1), out int orden))
                {
                    categorias.Rows.Add();
                    categorias.Rows[categorias.Rows.Count - 1]["tipo_producto"] = productos_terminado.Rows[fila]["tipo_producto"].ToString();
                }
            }
            categorias.Columns.Add("orden", typeof(int));
            for (int fila = 0; fila <= categorias.Rows.Count - 1; fila++)
            {
                string dato = categorias.Rows[fila]["tipo_producto"].ToString();
                categorias.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(categorias.Rows[fila]["tipo_producto"].ToString(), 1));
            }
            categorias.DefaultView.Sort = "orden asc";
            categorias = categorias.DefaultView.ToTable();
            return categorias.Rows[categorias.Rows.Count - 1]["orden"].ToString();
        }
        #endregion
    }
}
