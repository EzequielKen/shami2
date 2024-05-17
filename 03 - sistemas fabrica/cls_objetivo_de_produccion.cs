using _01___modulos;
using _03___sistemas_fabrica;
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

namespace _03___sistemas_fabrica3
{
    public class cls_objetivo_de_produccion
    {
        public cls_objetivo_de_produccion(DataTable usuario_BD)
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
            estadisticas = new cls_estadisticas_de_pedidos(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_estadisticas_de_pedidos estadisticas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable productos_proveedor;
        DataTable insumos_fabrica;
        DataTable promedio_del_mes;
        DataTable resumen;
        #endregion

        #region PDF
        public void crear_PDF_objetivo_produccion(string ruta_archivo, byte[] logo, string tipo_producto, DataTable productos)
        {
            PDF.GenerarPDF_objetivo_de_produccion(ruta_archivo, logo, tipo_producto, productos);
        }
        #endregion

        #region metodos privados
        private void cargar_promedio_pedidos(string nombre_proveedor)
        {
            int mes = obtener_mes(DateTime.Now.Month);
            int año = obtener_año(DateTime.Now.Month, DateTime.Now.Year);
            promedio_del_mes = estadisticas.get_promedio_del_mes(nombre_proveedor, mes.ToString(), año.ToString());
            string id;
            double stock, promedio_mensual, promedio_semanal, incremento, objetivo_produccion, plan_produccion;
            int columna_promedio;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                id = productos_proveedor.Rows[fila]["id"].ToString();
                columna_promedio = buscar_columna_segun_id(id, 0, promedio_del_mes);
                if (columna_promedio != -1)
                {
                    if (double.TryParse(funciones.obtener_dato(promedio_del_mes.Rows[0][columna_promedio].ToString(), 2), out promedio_mensual))
                    {
                        stock = double.Parse(productos_proveedor.Rows[fila]["stock"].ToString());
                        promedio_semanal = Math.Ceiling(promedio_mensual / 4);
                        incremento = Math.Ceiling(promedio_semanal * 1.20);
                        objetivo_produccion = incremento - stock;
                        plan_produccion = Math.Ceiling(objetivo_produccion / 5);

                        productos_proveedor.Rows[fila]["promedio_pedido"] = promedio_semanal.ToString();
                        productos_proveedor.Rows[fila]["incremento"] = incremento.ToString();
                        productos_proveedor.Rows[fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                        productos_proveedor.Rows[fila]["plan_produccion"] = plan_produccion.ToString();

                    }
                    else
                    {
                        productos_proveedor.Rows[fila]["promedio_pedido"] = "N/A";
                        productos_proveedor.Rows[fila]["incremento"] = "N/A";
                        productos_proveedor.Rows[fila]["objetivo_produccion"] = "N/A";
                        productos_proveedor.Rows[fila]["plan_produccion"] = "N/A";
                    }
                }
            }
            if (mes==1 && año==2024)
            {
                mes=2;
            }
            promedio_del_mes = estadisticas.get_promedio_del_mes("insumos_fabrica", mes.ToString(), año.ToString());

            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                id = insumos_fabrica.Rows[fila]["id"].ToString();
               
                columna_promedio = buscar_columna_segun_id(id, 0, promedio_del_mes);
                if (columna_promedio != -1)
                {
                    if (double.TryParse(funciones.obtener_dato(promedio_del_mes.Rows[0][columna_promedio].ToString(), 2), out promedio_mensual))
                    {
                        stock = double.Parse(insumos_fabrica.Rows[fila]["stock"].ToString());
                        promedio_semanal = Math.Ceiling(promedio_mensual / 4);
                        incremento = Math.Ceiling(promedio_semanal * 1.20);
                        objetivo_produccion = incremento - stock;
                        plan_produccion = Math.Ceiling(objetivo_produccion / 5);

                        insumos_fabrica.Rows[fila]["promedio_pedido"] = promedio_semanal.ToString();
                        insumos_fabrica.Rows[fila]["incremento"] = incremento.ToString();
                        insumos_fabrica.Rows[fila]["objetivo_produccion"] = objetivo_produccion.ToString();
                        insumos_fabrica.Rows[fila]["plan_produccion"] = plan_produccion.ToString();

                    }
                    else
                    {
                        insumos_fabrica.Rows[fila]["promedio_pedido"] = "N/A";
                        insumos_fabrica.Rows[fila]["incremento"] = "N/A";
                        insumos_fabrica.Rows[fila]["objetivo_produccion"] = "N/A";
                        insumos_fabrica.Rows[fila]["plan_produccion"] = "N/A";
                    }
                }

            }
        }
        private int buscar_columna_segun_id(string id_producto, int fila_totales_hasta_el_mes, DataTable dt)
        {
            int retorno = -1;
            int columna = dt.Columns["producto_1"].Ordinal;
            while (columna <= dt.Columns.Count - 1)
            {
                if (id_producto == funciones.obtener_dato(dt.Rows[fila_totales_hasta_el_mes][columna].ToString(), 1))
                {
                    retorno = columna;
                    break;
                }
                columna++;
            }
            return retorno;
        }
        private void crear_tabla_resumen()
        {
            resumen = new DataTable();
            resumen.Columns.Add("id", typeof(string));
            resumen.Columns.Add("producto", typeof(string));
            resumen.Columns.Add("tipo_producto", typeof(string));
            resumen.Columns.Add("promedio_pedido", typeof(string));
            resumen.Columns.Add("incremento", typeof(string));
            resumen.Columns.Add("stock", typeof(string));
            resumen.Columns.Add("objetivo_produccion", typeof(string));
            resumen.Columns.Add("plan_produccion", typeof(string));
            resumen.Columns.Add("unidad_de_medida_fabrica", typeof(string));

            int fila_resumen = 0;
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                resumen.Rows.Add();
                resumen.Rows[fila_resumen]["id"] = productos_proveedor.Rows[fila]["id"].ToString();
                resumen.Rows[fila_resumen]["producto"] = productos_proveedor.Rows[fila]["producto"].ToString();
                resumen.Rows[fila_resumen]["tipo_producto"] = productos_proveedor.Rows[fila]["tipo_producto"].ToString();
                resumen.Rows[fila_resumen]["promedio_pedido"] = productos_proveedor.Rows[fila]["promedio_pedido"].ToString();
                resumen.Rows[fila_resumen]["incremento"] = productos_proveedor.Rows[fila]["incremento"].ToString();
                resumen.Rows[fila_resumen]["stock"] = productos_proveedor.Rows[fila]["stock"].ToString();
                resumen.Rows[fila_resumen]["objetivo_produccion"] = productos_proveedor.Rows[fila]["objetivo_produccion"].ToString();
                resumen.Rows[fila_resumen]["plan_produccion"] = productos_proveedor.Rows[fila]["plan_produccion"].ToString();
                resumen.Rows[fila_resumen]["unidad_de_medida_fabrica"] = productos_proveedor.Rows[fila]["unidad_de_medida_fabrica"].ToString();
                fila_resumen++;
            }

            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                if (insumos_fabrica.Rows[fila]["tipo_producto"].ToString() == "3-Panificados")
                {

                    resumen.Rows.Add();
                    resumen.Rows[fila_resumen]["id"] = insumos_fabrica.Rows[fila]["id"].ToString();
                    resumen.Rows[fila_resumen]["producto"] = insumos_fabrica.Rows[fila]["producto"].ToString();
                    resumen.Rows[fila_resumen]["tipo_producto"] = insumos_fabrica.Rows[fila]["tipo_producto"].ToString();
                    resumen.Rows[fila_resumen]["promedio_pedido"] = insumos_fabrica.Rows[fila]["promedio_pedido"].ToString();
                    resumen.Rows[fila_resumen]["incremento"] = insumos_fabrica.Rows[fila]["incremento"].ToString();
                    resumen.Rows[fila_resumen]["stock"] = insumos_fabrica.Rows[fila]["stock"].ToString();
                    resumen.Rows[fila_resumen]["objetivo_produccion"] = insumos_fabrica.Rows[fila]["objetivo_produccion"].ToString();
                    resumen.Rows[fila_resumen]["plan_produccion"] = insumos_fabrica.Rows[fila]["plan_produccion"].ToString();
                    resumen.Rows[fila_resumen]["unidad_de_medida_fabrica"] = insumos_fabrica.Rows[fila]["unidad_de_medida_local"].ToString();
                    fila_resumen++;
                }

            }
        }
        #endregion
        #region metodos consultas
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_productos_proveedor_productos_produccion(base_de_datos, nombre_proveedor);
            productos_proveedor.Columns.Add("incremento", typeof(string));
            productos_proveedor.Columns.Add("objetivo_produccion", typeof(string));
            productos_proveedor.Columns.Add("promedio_pedido", typeof(string));
            productos_proveedor.Columns.Add("plan_produccion", typeof(string));
            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                productos_proveedor.Rows[fila]["incremento"] = "0";
                productos_proveedor.Rows[fila]["objetivo_produccion"] = "0";
                productos_proveedor.Rows[fila]["promedio_pedido"] = "0";
                productos_proveedor.Rows[fila]["plan_produccion"] = "0";
            }

        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
            insumos_fabrica.Columns.Add("incremento", typeof(string));
            insumos_fabrica.Columns.Add("objetivo_produccion", typeof(string));
            insumos_fabrica.Columns.Add("promedio_pedido", typeof(string));
            insumos_fabrica.Columns.Add("plan_produccion", typeof(string));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count - 1; fila++)
            {
                insumos_fabrica.Rows[fila]["incremento"] = "0";
                insumos_fabrica.Rows[fila]["objetivo_produccion"] = "0";
                insumos_fabrica.Rows[fila]["promedio_pedido"] = "0";
                insumos_fabrica.Rows[fila]["plan_produccion"] = "0";
            }

        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            consultar_insumos_fabrica();
            cargar_promedio_pedidos(nombre_proveedor);
            crear_tabla_resumen();

            return resumen;
        }
        #endregion

        #region funciones
        private int obtener_mes(int mes)
        {
            int retorno = mes;
            if (mes == 1)
            {
                retorno = 12;
            }
            else
            {
                retorno = mes - 1;
            }
            return retorno;
        }
        private int obtener_año(int mes, int año)
        {
            int retorno = año;
            if (mes == 1)
            {
                retorno = año - 1;
            }
            return retorno;
        }
        #endregion
    }
}
