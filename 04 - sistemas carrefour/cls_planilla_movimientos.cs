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

namespace _04___sistemas_carrefour
{
    public class cls_planilla_movimientos
    {
        public cls_planilla_movimientos(DataTable usuario_BD)
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

        DataTable productos_carrefour;
        DataTable movimientos_carrefour;
        DataTable sucural_seleccionada;
        DataTable acuerdo_de_precio_carrefour_activo;
        #endregion

        #region carga a base de datos
        public void registrar_movimiento(DataTable productos_carrefourBD, string id_carrefour_seleccionado, string fecha)
        {
            consultar_sucursal_carrefour_por_id(id_carrefour_seleccionado);
            string tipo_acuerdo = sucural_seleccionada.Rows[0]["acuerdo_de_precio"].ToString();
            string acuerdo = obtener_num_acuerdo_de_precio_carrefour(tipo_acuerdo);

            string dato, id_producto, nombre_producto, Ultimo_inicial, devolucion, reposicion, stock_final, vendio;

            string columnas = "";
            string valores = "";
            //id_sucursal
            columnas = funciones.armar_query_columna(columnas, "id_sucursal", false);
            valores = funciones.armar_query_valores(valores, sucural_seleccionada.Rows[0]["id"].ToString(), false);
            //sucursal
            columnas = funciones.armar_query_columna(columnas, "sucursal", false);
            valores = funciones.armar_query_valores(valores, sucural_seleccionada.Rows[0]["sucursal"].ToString(), false);
            //tipo_acuerdo
            columnas = funciones.armar_query_columna(columnas, "tipo_acuerdo", false);
            valores = funciones.armar_query_valores(valores, tipo_acuerdo, false);
            //acuerdo
            columnas = funciones.armar_query_columna(columnas, "acuerdo", false);
            valores = funciones.armar_query_valores(valores, acuerdo, false);
            //fecha
            columnas = funciones.armar_query_columna(columnas, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha, false);
            //producto_1
            int index = 1;
            for (int fila = 0; fila < productos_carrefourBD.Rows.Count - 1; fila++)
            {
                id_producto = productos_carrefourBD.Rows[fila]["id"].ToString();
                nombre_producto = productos_carrefourBD.Rows[fila]["producto"].ToString();
                Ultimo_inicial = productos_carrefourBD.Rows[fila]["ultimo_stock"].ToString();
                devolucion = productos_carrefourBD.Rows[fila]["devolucion"].ToString();
                reposicion = productos_carrefourBD.Rows[fila]["reposicion"].ToString();
                stock_final = productos_carrefourBD.Rows[fila]["stock_final"].ToString();
                vendio = productos_carrefourBD.Rows[fila]["vendio"].ToString();

                dato = id_producto + "-" + nombre_producto + "-" + Ultimo_inicial + "-" + devolucion + "-" + reposicion + "-" + stock_final + "-" + vendio;
                columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), false);
                valores = funciones.armar_query_valores(valores, dato, false);
                index++;
            }
            int ultima_fila = productos_carrefourBD.Rows.Count - 1;

            id_producto = productos_carrefourBD.Rows[ultima_fila]["id"].ToString();
            nombre_producto = productos_carrefourBD.Rows[ultima_fila]["producto"].ToString();
            Ultimo_inicial = productos_carrefourBD.Rows[ultima_fila]["ultimo_stock"].ToString();
            devolucion = productos_carrefourBD.Rows[ultima_fila]["devolucion"].ToString();
            reposicion = productos_carrefourBD.Rows[ultima_fila]["reposicion"].ToString();
            stock_final = productos_carrefourBD.Rows[ultima_fila]["stock_final"].ToString();
            vendio = productos_carrefourBD.Rows[ultima_fila]["vendio"].ToString();

            dato = id_producto + "-" + nombre_producto + "-" + Ultimo_inicial + "-" + devolucion + "-" + reposicion + "-" + stock_final + "-" + vendio;
            columnas = funciones.armar_query_columna(columnas, "producto_" + index.ToString(), true);
            valores = funciones.armar_query_valores(valores, dato, true);

            consultas.insertar_en_tabla(base_de_datos, "movimientos_carrefour", columnas, valores);
        }
        private string obtener_num_acuerdo_de_precio_carrefour(string tipo_acuerdo)
        {
            consultar_acuerdo_de_precio_carrefour_activo();
            string retorno = "";
            int fila = 0;
            while (fila <= acuerdo_de_precio_carrefour_activo.Rows.Count - 1)
            {
                if (tipo_acuerdo == acuerdo_de_precio_carrefour_activo.Rows[fila]["tipo_de_acuerdo"].ToString())
                {
                    retorno = acuerdo_de_precio_carrefour_activo.Rows[fila]["acuerdo"].ToString();
                    break;
                }
                fila++;
            }
            return retorno;
        }
        private int obtener_fila_acuerdo_de_precio_carrefour(string tipo_acuerdo)
        {
            consultar_acuerdo_de_precio_carrefour_activo();
            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precio_carrefour_activo.Rows.Count - 1)
            {
                if (tipo_acuerdo == acuerdo_de_precio_carrefour_activo.Rows[fila]["tipo_de_acuerdo"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        #endregion

        #region metodos privados
        private void obtener_ultimos_movimientos()
        {
            int fila_ultimo_movimiento = obtener_fila_ultimo_movimiento();
            string id_producto,  stock_final;
            int fila_producto;
            if (fila_ultimo_movimiento == -1)
            {
                for (int fila = 0; fila <= productos_carrefour.Rows.Count - 1; fila++)
                {
                    productos_carrefour.Rows[fila]["ultimo_stock"] = "0";
                    productos_carrefour.Rows[fila]["devolucion"] = "0";
                    productos_carrefour.Rows[fila]["reposicion"] = "0";
                    productos_carrefour.Rows[fila]["stock_final"] = "0";
                    productos_carrefour.Rows[fila]["vendio"] = "0";
                }
            }
            else
            {
                for (int columna = movimientos_carrefour.Columns["producto_1"].Ordinal; columna <= movimientos_carrefour.Columns.Count - 1; columna++)
                {
                    if (movimientos_carrefour.Rows[fila_ultimo_movimiento][columna].ToString() != "N/A")
                    {
                        id_producto = funciones.obtener_dato(movimientos_carrefour.Rows[fila_ultimo_movimiento][columna].ToString(), 1);

                        stock_final = funciones.obtener_dato(movimientos_carrefour.Rows[fila_ultimo_movimiento][columna].ToString(), 6);

                        fila_producto = funciones.buscar_fila_por_id(id_producto, productos_carrefour);
                        productos_carrefour.Rows[fila_producto]["ultimo_stock"] = stock_final;
                        productos_carrefour.Rows[fila_producto]["devolucion"] = "0";
                        productos_carrefour.Rows[fila_producto]["reposicion"] = "0";
                        productos_carrefour.Rows[fila_producto]["stock_final"] = "0";
                        productos_carrefour.Rows[fila_producto]["vendio"] = "0";

                    }
                }
            }
        }
        private int obtener_fila_ultimo_movimiento()
        {
            int retorno = -1;

            if (movimientos_carrefour.Rows.Count > 0)
            {
                movimientos_carrefour.DefaultView.Sort = "fecha ASC";
                movimientos_carrefour = movimientos_carrefour.DefaultView.ToTable();
                retorno = movimientos_carrefour.Rows.Count - 1;
            }

            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_acuerdo_de_precio_carrefour_activo()
        {
            acuerdo_de_precio_carrefour_activo = consultas.consultar_acuerdo_de_precio_activo_carrefour();
        }
        private void consultar_sucursal_carrefour_por_id(string id_carrefour_seleccionado)
        {
            sucural_seleccionada = consultas.consultar_sucursal_carrefour_por_id(id_carrefour_seleccionado);
        }
        private void consultar_movimientos_carrefour(string id_carrefour_seleccionado)
        {
            movimientos_carrefour = consultas.consultar_movimientos_carrefour_segun_id_sucursal(id_carrefour_seleccionado);
        }
        private void consultar_productos_carrefour(string id_carrefour_seleccionado)
        {
            productos_carrefour = consultas.consultar_tabla(base_de_datos, "productos_carrefour");
            productos_carrefour.Columns.Add("ultimo_stock", typeof(string));
            productos_carrefour.Columns.Add("devolucion", typeof(string));
            productos_carrefour.Columns.Add("reposicion", typeof(string));
            productos_carrefour.Columns.Add("stock_final", typeof(string));
            productos_carrefour.Columns.Add("vendio", typeof(string));
            consultar_movimientos_carrefour(id_carrefour_seleccionado);
            obtener_ultimos_movimientos();
        }
        #endregion

        #region metodos get
        public string get_fecha_ultima_visita(string id_carrefour_seleccionado)
        {
            string retorno = "N/A";
            consultar_movimientos_carrefour(id_carrefour_seleccionado);
            int fila_ultimo_movimiento = obtener_fila_ultimo_movimiento();
            if (fila_ultimo_movimiento>-1)
            {
                retorno = movimientos_carrefour.Rows[fila_ultimo_movimiento]["fecha"].ToString();
            }
            return retorno;
        }
        public DataTable get_productos_carrefour(string id_carrefour_seleccionado)
        {
            consultar_productos_carrefour(id_carrefour_seleccionado);
            return productos_carrefour;
        }
        #endregion
    }
}
