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
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_terminados;
        DataTable insumos;
        DataTable resumen;
        DataTable resumen_carga;
        DataTable lista_de_faltantes;
        #endregion

        #region PDF
        public void crear_pdf(string ruta, byte[] logo, DataTable productosBD,string sucursal)
        {
            llenar_resumen_carga(productosBD);
            PDF.GenerarPDF_resumen_de_faltantes(ruta, logo, resumen_carga,sucursal);
        }
        #endregion

        #region carga a base de datos
        public void desactivar_lista_faltante(string id)
        {
            string actualizar= "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "lista_de_faltantes", actualizar,id); 
        }
        public void cargar_lista_faltante(string id_sucursal, string sucursal, DataTable productosBD)
        {
            llenar_resumen_carga(productosBD);
            string columnas = string.Empty;
            string valores = string.Empty;
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, id_sucursal, false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucursal, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, funciones.get_fecha(), false);
            string id, producto, proveedor,nota, dato = string.Empty;
            int index = 1;
            for (int fila = 0; fila < resumen_carga.Rows.Count - 1; fila++)
            {
                id = resumen_carga.Rows[fila]["id"].ToString();
                producto = resumen_carga.Rows[fila]["producto"].ToString();
                proveedor = resumen_carga.Rows[fila]["proveedor"].ToString();
                nota = resumen_carga.Rows[fila]["nota"].ToString();
                dato = id + "-" + producto + "-" + proveedor +"-"+nota;
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);
                index++;
            }
            int ultima_fila = resumen_carga.Rows.Count - 1;
            if (ultima_fila>=0)
            {
                id = resumen_carga.Rows[ultima_fila]["id"].ToString();
                producto = resumen_carga.Rows[ultima_fila]["producto"].ToString();
                proveedor = resumen_carga.Rows[ultima_fila]["proveedor"].ToString();
                nota = resumen_carga.Rows[ultima_fila]["nota"].ToString();
                dato = id + "-" + producto + "-" + proveedor + "-" + nota;
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
                valores = funciones.armar_query_valores(valores, dato, true);
            }
            else
            {
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
                valores = funciones.armar_query_valores(valores, "N/A", true);
            }
            consultas.insertar_en_tabla(base_de_datos, "lista_de_faltantes", columnas, valores);
        }
        private void crear_tabla_resumen_carga()
        {
            resumen_carga = new DataTable();
            resumen_carga.Columns.Add("id", typeof(string));
            resumen_carga.Columns.Add("producto", typeof(string));
            resumen_carga.Columns.Add("proveedor", typeof(string));
            resumen_carga.Columns.Add("nota", typeof(string));
        }
        private void llenar_resumen_carga(DataTable productosBD)
        {
            crear_tabla_resumen_carga();
            int ultima_fila;
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["faltante"].ToString() != "N/A")
                {
                    resumen_carga.Rows.Add();
                    ultima_fila = resumen_carga.Rows.Count - 1;
                    resumen_carga.Rows[ultima_fila]["id"] = productosBD.Rows[fila]["id"].ToString();
                    resumen_carga.Rows[ultima_fila]["producto"] = productosBD.Rows[fila]["producto"].ToString();
                    resumen_carga.Rows[ultima_fila]["proveedor"] = productosBD.Rows[fila]["proveedor"].ToString();
                    resumen_carga.Rows[ultima_fila]["nota"] = productosBD.Rows[fila]["nota"].ToString();
                }
            }
        }
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
        private void consultar_lista_de_faltantes(string id_sucursal)
        {
            lista_de_faltantes = consultas.consultar_lista_de_faltantes(id_sucursal);
        }
        #endregion

        #region metodos privados
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("nota", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("proveedor", typeof(string));
            resumen.Columns.Add("faltante", typeof(string));
            resumen.Columns.Add("orden_tipo", typeof(int));
            resumen.Columns.Add("id_faltante", typeof(string));
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
                resumen.Rows[ultima_fila]["nota"] = "N/A";
                resumen.Rows[ultima_fila]["orden_tipo"] = int.Parse(funciones.obtener_dato(productos_terminados.Rows[fila]["tipo_producto"].ToString(), 1));
                resumen.Rows[ultima_fila]["id_faltante"] = "N/A";


            }

            for (int fila = 0; fila <= insumos.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                ultima_fila = resumen.Rows.Count - 1;
                resumen.Rows[ultima_fila]["id"] = insumos.Rows[fila]["id"].ToString();
                resumen.Rows[ultima_fila]["producto"] = insumos.Rows[fila]["producto"].ToString();
                resumen.Rows[ultima_fila]["tipo_producto"] = insumos.Rows[fila]["tipo_producto"].ToString();
                resumen.Rows[ultima_fila]["proveedor"] = "proveedor_villaMaipu";
                resumen.Rows[ultima_fila]["faltante"] = "N/A";
                resumen.Rows[ultima_fila]["nota"] = "N/A";
                resumen.Rows[ultima_fila]["orden_tipo"] = int.Parse(funciones.obtener_dato(insumos.Rows[fila]["tipo_producto"].ToString(), 1));
                resumen.Rows[ultima_fila]["id_faltante"] = "N/A";
            }
            resumen.DefaultView.Sort = "orden_tipo ASC";
            resumen = resumen.DefaultView.ToTable();

            if (lista_de_faltantes.Rows.Count > 0)
            {
                string id_producto = string.Empty;
                string proveedor = string.Empty;
                int fila_producto;
                for (int columna = lista_de_faltantes.Columns["producto_1"].Ordinal; columna <= lista_de_faltantes.Columns.Count - 1; columna++)
                {
                    if (lista_de_faltantes.Rows[0][columna].ToString() != "N/A")
                    {
                        id_producto = funciones.obtener_dato(lista_de_faltantes.Rows[0][columna].ToString(), 1);
                        proveedor = funciones.obtener_dato(lista_de_faltantes.Rows[0][columna].ToString(), 3);
                        fila_producto = funciones.buscar_fila_por_id_proveedor(id_producto, proveedor, resumen);
                        resumen.Rows[fila_producto]["faltante"] = "Si";
                        resumen.Rows[fila_producto]["nota"] = funciones.obtener_dato(lista_de_faltantes.Rows[0][columna].ToString(), 5);
                    }
                }
                for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
                {
                        resumen.Rows[fila]["id_faltante"] = lista_de_faltantes.Rows[0]["id"].ToString();
                }
            }

        }
        #endregion

        #region metodos get/set
        public DataTable get_lista_producto(string id_sucursal)
        {
            consultar_productos_terminados();
            consultar_insumos();
            consultar_lista_de_faltantes(id_sucursal);
            lista_de_faltantes.DefaultView.Sort = "fecha DESC";
            lista_de_faltantes = lista_de_faltantes.DefaultView.ToTable();
            llenar_tabla_resumen();
            return resumen;
        }
        #endregion
    }
}
