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

namespace _04___sistemas_carrefour
{
    public class cls_detalle_de_movimientos
    {
        public cls_detalle_de_movimientos(DataTable usuario_BD)
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

        DataTable sucursales_carrefour;
        DataTable movimientos_carrefour_no_calculados;
        DataTable acuerdo_de_precios_carrefour;
        DataTable productos_carrefour;
        DataTable movimiento_del_mes;
        #endregion

        #region metodos privados
        private void calcular_movimientos_carrefour_no_calculados()
        {
            consultar_movimientos_carrefour_no_calculados();
            consultar_acuerdo_de_precios_carrefour();
            string tipo_de_acuerdo, acuerdo;
            int fila_acuerdo;
            double valor_venta, precio_prodcuto, cant_venta, sub_total;
            string id_producto;
            string id_movimiento, actualizar;
            for (int fila = 0; fila <= movimientos_carrefour_no_calculados.Rows.Count - 1; fila++)
            {
                tipo_de_acuerdo = movimientos_carrefour_no_calculados.Rows[fila]["tipo_acuerdo"].ToString();
                acuerdo = movimientos_carrefour_no_calculados.Rows[fila]["acuerdo"].ToString();
                fila_acuerdo = buscar_fila_acuerdo(tipo_de_acuerdo, acuerdo);
                valor_venta = 0;
                for (int columna = movimientos_carrefour_no_calculados.Columns["producto_1"].Ordinal; columna <= movimientos_carrefour_no_calculados.Columns.Count - 1; columna++)
                {
                    if (movimientos_carrefour_no_calculados.Rows[fila][columna].ToString() != "N/A")
                    {
                        id_producto = funciones.obtener_dato(movimientos_carrefour_no_calculados.Rows[fila][columna].ToString(), 1);

                        precio_prodcuto = double.Parse(acuerdo_de_precios_carrefour.Rows[fila_acuerdo]["producto_" + id_producto].ToString());
                        cant_venta = double.Parse(funciones.obtener_dato(movimientos_carrefour_no_calculados.Rows[fila][columna].ToString(), 7));

                        sub_total = cant_venta * precio_prodcuto;

                        valor_venta = Math.Round(valor_venta + sub_total, 2);
                    }
                }
                id_movimiento = movimientos_carrefour_no_calculados.Rows[fila]["id"].ToString();

                actualizar = "`calculado` = 'Si'";
                consultas.actualizar_tabla(base_de_datos, "movimientos_carrefour", actualizar, id_movimiento);

                actualizar = "`valor_venta` = '" + valor_venta.ToString() + "'";
                consultas.actualizar_tabla(base_de_datos, "movimientos_carrefour", actualizar, id_movimiento);
            }
        }

        private int buscar_fila_acuerdo(string tipo_de_acuerdo, string acuerdo)
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= acuerdo_de_precios_carrefour.Rows.Count - 1)
            {
                if (tipo_de_acuerdo == acuerdo_de_precios_carrefour.Rows[fila]["tipo_de_acuerdo"].ToString() &&
                    acuerdo == acuerdo_de_precios_carrefour.Rows[fila]["acuerdo"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        #endregion

        #region metodos consultas
        private void consultar_movimiento_del_mes(string sucursal, string mes, string año)
        {
            movimiento_del_mes = consultas.consultar_movimiento_segun_mes_y_año(base_de_datos, sucursal, mes, año);
        }
        private void consultar_productos_carrefour()
        {
            productos_carrefour = consultas.consultar_tabla_completa(base_de_datos, "productos_carrefour");
        }
        private void consultar_sucursales_carrefour()
        {
            sucursales_carrefour = consultas.consultar_tabla_completa(base_de_datos, "sucursales_carrefour");
        }
        private void consultar_movimientos_carrefour_no_calculados()
        {
            movimientos_carrefour_no_calculados = consultas.consultar_movimientos_carrefour_no_calculados();
        }
        private void consultar_acuerdo_de_precios_carrefour()
        {
            acuerdo_de_precios_carrefour = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios_carrefour");
        }
        #endregion

        #region detalles de movimientos
        private void cargar_detalle_de_movimientos()
        {
            string id_producto;
            int fila_producto;
            string nombre_columna;
            DateTime fecha;
            productos_carrefour.Columns.Add("total", typeof(string));
            for (int fila = 0; fila <= movimiento_del_mes.Rows.Count - 1; fila++)
            {
                fecha = (DateTime)movimiento_del_mes.Rows[fila]["fecha"];
                nombre_columna = fecha.ToString("dd/MM/yyyy HH:mm");
                productos_carrefour.Columns.Add(nombre_columna, typeof(string));
                for (int columna = movimiento_del_mes.Columns["producto_1"].Ordinal; columna <= movimiento_del_mes.Columns.Count - 1; columna++)
                {
                    if (movimiento_del_mes.Rows[fila][columna].ToString() != "N/A")
                    {
                        id_producto = funciones.obtener_dato(movimiento_del_mes.Rows[fila][columna].ToString(), 1);
                        fila_producto = funciones.buscar_fila_por_id(id_producto, productos_carrefour);

                        productos_carrefour.Rows[fila_producto][nombre_columna] = funciones.obtener_dato(movimiento_del_mes.Rows[fila][columna].ToString(), 7);
                    }
                }
            }

            double total=0;
            for (int fila = 0; fila <= productos_carrefour.Rows.Count-1; fila++)
            {
                total=0;
                for (int columna = productos_carrefour.Columns["total"].Ordinal+1; columna <= productos_carrefour.Columns.Count-1; columna++)
                {
                    total = total + double.Parse(productos_carrefour.Rows[fila][columna].ToString());
                }
                productos_carrefour.Rows[fila]["total"] = total.ToString();
            }
        }
        #endregion

        #region metodos get/set 
        public DataTable get_productos_carrefour(string sucrsal, string mes, string año)
        {
            consultar_productos_carrefour();
            consultar_movimiento_del_mes(sucrsal, mes, año);
            movimiento_del_mes.DefaultView.Sort = "fecha ASC";
            movimiento_del_mes = movimiento_del_mes.DefaultView.ToTable();



            // analisis_de_movimientos();
            cargar_detalle_de_movimientos();
            return productos_carrefour;
        }
        public DataTable get_sucursales_carrefour()
        {
            consultar_sucursales_carrefour();
            return sucursales_carrefour;
        }
        #endregion

        #region metodos publicos
        public void actualizar_movimientos_carrefour_no_calculados()
        {
            calcular_movimientos_carrefour_no_calculados();
        }
        #endregion
    }
}
