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
    public class cls_resumen_deuda_proveedores
    {
        public cls_resumen_deuda_proveedores(DataTable usuario_BD)
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
            cuentas_Por_Pagar = new cls_cuentas_por_pagar(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        cls_cuentas_por_pagar cuentas_Por_Pagar;
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable proveedores_de_fabrica;
        DataTable entrega_parciales_del_proveedor;
        #endregion

        #region metodos consultas
        private void consultar_proveedores_de_fabrica()
        {
            proveedores_de_fabrica = consultas.consultar_tabla(base_de_datos, "proveedores_de_fabrica");
            proveedores_de_fabrica.Columns.Add("deuda",typeof(string));
            proveedores_de_fabrica.Columns.Add("entrega_parcial", typeof(string));
            string proveedor;
            for (int fila = 0; fila <= proveedores_de_fabrica.Rows.Count-1; fila++)
            {
                proveedor = proveedores_de_fabrica.Rows[fila]["proveedor"].ToString();
                proveedores_de_fabrica.Rows[fila]["deuda"] = cuentas_Por_Pagar.calcular_deuda_mes(proveedor, DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
                proveedores_de_fabrica.Rows[fila]["entrega_parcial"] =obtener_cantidad_de_entregas_parciales(proveedores_de_fabrica.Rows[fila]["id"].ToString());
            }
            proveedores_de_fabrica.DefaultView.Sort = "proveedor ASC";
            proveedores_de_fabrica = proveedores_de_fabrica.DefaultView.ToTable();
        }
        private string obtener_cantidad_de_entregas_parciales(string id_proveedor)
        {
            if (id_proveedor=="9")
            {
                string stop="";
            }
            string retorno = string.Empty;
            consultar_entregas_parciales(id_proveedor);
            int cantidad = entrega_parciales_del_proveedor.Rows.Count;
            if (cantidad>0)
            {
                retorno = cantidad.ToString();
            }
            return retorno;
        }
        private void consultar_entregas_parciales(string id_proveedor)
        {
            entrega_parciales_del_proveedor = consultas.consultar_cuenta_por_pagar_fabrica(id_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_proveedores_de_fabrica()
        {
            consultar_proveedores_de_fabrica();
            return proveedores_de_fabrica;
        }
        public string get_deuda_total()
        {
            return funciones.formatCurrency(cuentas_Por_Pagar.deuda_total_del_mes("", DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()));
        }
        #endregion
    }
}
