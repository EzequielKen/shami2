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
    public class cls_proveedores_fabrica
    {
        public cls_proveedores_fabrica(DataTable usuario_BD)
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
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable proveedores_de_fabrica;
        DataTable insumos_fabrica;
        DataTable acuerdo_de_precios_fabrica_a_proveedores;
        List<string> id_proveedores_seleccionados;
        #endregion

        #region metodos consultas
        private void consultar_proveedores_fabrica()
        {
            proveedores_de_fabrica = consultas.consultar_tabla(base_de_datos, "proveedores_de_fabrica");
        }
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        private void consultar_acuerdo_de_precios_fabrica_a_proveedores()
        {
            acuerdo_de_precios_fabrica_a_proveedores = consultas.consultar_acuerdo_de_precios_fabrica_a_proveedores_activo();
        }
        #endregion
        private void buscar_proveedores(string id_producto)
        {
            id_proveedores_seleccionados = new List<string>();
            for (int fila = 0; fila <= acuerdo_de_precios_fabrica_a_proveedores.Rows.Count - 1; fila++)
            {
                for (int columna = acuerdo_de_precios_fabrica_a_proveedores.Columns["producto_1"].Ordinal; columna <= acuerdo_de_precios_fabrica_a_proveedores.Columns.Count - 1; columna++)
                {
                    if (acuerdo_de_precios_fabrica_a_proveedores.Rows[fila][columna].ToString() == "N/A")
                    {
                        break;
                    }
                    else if (funciones.obtener_dato(acuerdo_de_precios_fabrica_a_proveedores.Rows[fila][columna].ToString(), 1) == id_producto)
                    {
                        id_proveedores_seleccionados.Add(acuerdo_de_precios_fabrica_a_proveedores.Rows[fila]["id_proveedor"].ToString());
                        break;
                    }
                }
            }
        }
        #region metodos get/set
        public DataTable get_proveedores_de_fabrica()
        {
            consultar_proveedores_fabrica();
            proveedores_de_fabrica.DefaultView.Sort= "proveedor ASC";
            proveedores_de_fabrica = proveedores_de_fabrica.DefaultView.ToTable();
            return proveedores_de_fabrica;
        }
        public DataTable get_insumos_fabrica()
        {
            consultar_insumos_fabrica();
            return insumos_fabrica;
        }
        public List<string> get_proveedores_seleccionados(string id_producto)
        {
            consultar_acuerdo_de_precios_fabrica_a_proveedores();
            buscar_proveedores(id_producto);
            return id_proveedores_seleccionados;
        }
        #endregion
    }
}
