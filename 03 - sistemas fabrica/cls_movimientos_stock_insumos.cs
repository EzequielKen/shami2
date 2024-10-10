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
    public class cls_movimientos_stock_insumos
    {
        public cls_movimientos_stock_insumos(DataTable usuario_BD)
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
            stock_insumos = new cls_stock_insumos(usuario_BD);
        }

        #region atributos
        cls_stock_insumos stock_insumos;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable insumos_fabrica;
        DataTable historial_producto;
        #endregion

        #region carga a base de datos
        public void cargar_historial_stock(string rol_usuario, string id_producto, string tipo_movimiento, string movimiento, string nota,string presentacion)
        {
            stock_insumos.cargar_historial_stock(rol_usuario,id_producto,tipo_movimiento, movimiento,nota,presentacion);        
        }
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
            insumos_fabrica.Columns.Add("orden",typeof(int));
            for (int fila = 0; fila <= insumos_fabrica.Rows.Count-1; fila++)
            {
                insumos_fabrica.Rows[fila]["orden"] = int.Parse(funciones.obtener_dato(insumos_fabrica.Rows[fila]["tipo_producto"].ToString(),1));
            }
            insumos_fabrica.DefaultView.Sort = "orden asc";
            insumos_fabrica = insumos_fabrica.DefaultView.ToTable();
        }
        private void consultar_historial_producto(string id_producto,string presentacion, string mes, string año)
        {
            historial_producto = consultas.consultar_historial_insumo_segun_mes_y_año(base_de_datos, "stock_insumos", id_producto,presentacion, mes, año);
        }
        #endregion

        #region metodos get/set
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            return insumos_fabrica;
        }
        public DataTable get_historial_insumo(string id_producto,string presentacion, string mes, string año)
        {
            consultar_historial_producto(id_producto,presentacion, mes, año);
            historial_producto.DefaultView.Sort = "fecha DESC, id DESC";
            historial_producto = historial_producto.DefaultView.ToTable();
            return historial_producto;
        }
        #endregion
    }
}
