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
    public class cls_sistema_rendiciones
    {
        public cls_sistema_rendiciones(DataTable usuario_BD)
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
        cls_PDF PDF = new cls_PDF();

        DataTable usuarioBD;
        DataTable rendiciones;
        DataTable acuerdos_de_precios;
        DataTable disponibles;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        #endregion
        #region PDF
        public void crear_PDF_rendicion(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable proveedorBD, DataTable resumen, DataTable rendiciones_usuario)
        {
         PDF.GenerarPDF_rendiciones(ruta_archivo,logo,fecha_acuerdo,acuerdo,proveedorBD,resumen,rendiciones_usuario);  
        }
        #endregion
        #region metodos privados
        private int buscar_fila_de_disponible(string nombre_proveedor, string acuerdo_siguiente)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= disponibles.Rows.Count - 1)
            {
                if (nombre_proveedor == disponibles.Rows[fila]["proveedor"].ToString() &&
                    acuerdo_siguiente == disponibles.Rows[fila]["acuerdo_de_precio"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion
        #region carga a base de datos
        public void cargar_nuevo_disponible(string nombre_proveedor, string acuerdo_de_precio, string valor)
        {
            //consultar disponibles
            consultar_disponbibles(nombre_proveedor);
            //buscar si existe disponible
            int acuerdo_siguiente = int.Parse(acuerdo_de_precio) + 1;
            int fila_disponible = buscar_fila_de_disponible(nombre_proveedor, acuerdo_siguiente.ToString());
            //si existe disponible modificar
            if (fila_disponible == -1)
            {
                //si no existe disponible crear
                crear_nuevo_disponible(valor, nombre_proveedor, acuerdo_siguiente.ToString());

            }
            else
            {
                //si existe disponible modificar
                modificar_disponible_existente(valor, disponibles.Rows[fila_disponible]["id"].ToString());
            }
        }
      
        public void cargar_nafta(string nombre_proveedor, string acuerdo_de_precio, string valor)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, "Gasto en nafta", false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Gasto en nafta", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_de_precio, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones", columna, valores);
        }
        public void cargar_compra_cajones(string nombre_proveedor, string acuerdo_de_precio, string valor)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, "Compra cajones", false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Compra cajones", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_de_precio, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones", columna, valores);
        }
        public void cargar_venta_cajones(string nombre_proveedor, string acuerdo_de_precio, string valor)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, "Venta cajones", false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Venta cajones", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_de_precio, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones", columna, valores);
        }
        public void cargar_egreso(string nombre_proveedor, string acuerdo_de_precio, string valor,string detalle)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, detalle, false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Egreso", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_de_precio, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones", columna, valores);
        }
        public void cargar_pago(string nombre_proveedor, string acuerdo_de_precio, string valor, string detalle)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, detalle, false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Pagos", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_de_precio, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones", columna, valores);
        }
        private void crear_nuevo_disponible(string valor, string nombre_proveedor, string acuerdo_siguiente)
        {
            string columna = "";
            string valores = "";
            //activa
            columna = armar_query_columna(columna, "activa", false);
            valores = armar_query_valores(valores,"1",false);
            //id_producto
            columna = armar_query_columna(columna, "id_producto", false);
            valores = armar_query_valores(valores, "N/A", false);
            //detalle
            columna = armar_query_columna(columna, "detalle", false);
            valores = armar_query_valores(valores, "Nuevo disponible", false);
            //concepto
            columna = armar_query_columna(columna, "concepto", false);
            valores = armar_query_valores(valores, "Disponible", false);
            //valor
            columna = armar_query_columna(columna, "valor", false);
            valores = armar_query_valores(valores, valor, false);
            //stock_inicial
            columna = armar_query_columna(columna, "stock_inicial", false);
            valores = armar_query_valores(valores, "N/A", false);
            //stock_final
            columna = armar_query_columna(columna, "stock_final", false);
            valores = armar_query_valores(valores, "N/A", false);
            //proveedor
            columna = armar_query_columna(columna, "proveedor", false);
            valores = armar_query_valores(valores, nombre_proveedor, false);
            //fecha
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valores = armar_query_valores(valores, fecha, false);
            //acuerdo_de_precio 
            columna = armar_query_columna(columna, "acuerdo_de_precio", true);
            valores = armar_query_valores(valores, acuerdo_siguiente, true);

            consultas.insertar_en_tabla(base_de_datos, "rendiciones",columna, valores);
        }
        private void modificar_disponible_existente(string valor, string id_disponible)
        {
            string actualizar = "`valor` = '"+valor+"'";
            consultas.actualizar_tabla(base_de_datos, "rendiciones",actualizar, id_disponible);
        }


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
        #region metodos consultas
        private void consultar_disponbibles(string nombre_proveedor)
        {
            disponibles = consultas.consultar_disponibles(base_de_datos, "rendiciones", nombre_proveedor);
        }
        private void consultar_acuerdos_de_precios()
        {
            acuerdos_de_precios = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios");
        }
        private void conusltar_rendiciones()
        {
            rendiciones = consultas.consultar_tabla(base_de_datos, "rendiciones");
        }
        #endregion

        #region metodos get/set
        public DataTable get_rendiciones()
        {
            conusltar_rendiciones();
            return rendiciones;
        }
        public DataTable get_acuerdos_de_precios()
        {
            consultar_acuerdos_de_precios();
            return acuerdos_de_precios;
        }
        #endregion
    }
}
