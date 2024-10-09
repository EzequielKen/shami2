using _01___modulos;
using _03___sistemas_fabrica;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06___sistemas_gerente
{
    public class cls_resumen_deuda_locales
    {
        public cls_resumen_deuda_locales(DataTable usuario_BD)
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
            cuentas_Por_cobrar = new cls_sistema_cuentas_por_cobrar(usuario_BD);
            calculo_deudas = new cls_calculo_deuda_locales(usuario_BD);
        }

        #region atributos
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        cls_PDF PDF = new cls_PDF();
        cls_sistema_cuentas_por_cobrar cuentas_Por_cobrar;
        cls_calculo_deuda_locales calculo_deudas;
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable locales;
        DataTable entrega_parciales_del_proveedor;
        #endregion

        #region metodos consultas
        private void consultar_locales()
        {
            locales = consultas.consultar_tabla(base_de_datos, "sucursal");
            locales.Columns.Add("deuda", typeof(string));
            string sucursal;
            for (int fila = 0; fila <= locales.Rows.Count - 1; fila++)
            {
                sucursal = locales.Rows[fila]["sucursal"].ToString();
                if (sucursal== "Dalal")
                {
                    string stop="";
                }
                locales.Rows[fila]["deuda"] = calculo_deudas.calcular_deuda_del_mes(sucursal,DateTime.Now.Month.ToString(),DateTime.Now.Year.ToString());
            }
            locales.DefaultView.Sort = "sucursal ASC";
            locales = locales.DefaultView.ToTable();
        }
        private string obtener_cantidad_de_entregas_parciales(string id_proveedor)
        {
            if (id_proveedor == "9")
            {
                string stop = "";
            }
            string retorno = string.Empty;
            consultar_entregas_parciales(id_proveedor);
            int cantidad = entrega_parciales_del_proveedor.Rows.Count;
            if (cantidad > 0)
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
            consultar_locales();
            return locales;
        }
        public string get_deuda_total()
        {
            return funciones.formatCurrency(cuentas_Por_cobrar.deuda_total_del_mes_locales());
        }
        #endregion
    }
}
