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
    public class cls_remitos_de_carga
    {
        public cls_remitos_de_carga(DataTable usuario_BD)
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

        DataTable cuentas_por_pagar;
        DataTable lista_proveedores;
        #endregion

        #region metodos consultas
        private void consultar_cuentas_por_pagar()
        {
            cuentas_por_pagar = consultas.consultar_cuentas_por_pagar_fabrica_e_insumos(base_de_datos);
        }
        private void consultar_lista_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla_completa(base_de_datos, "lista_proveedores");
        }
        #endregion

        #region metodos privados
        private string obtener_nombre_proveedor(string nombre_en_BD)
        {
            consultar_lista_proveedores();
            string retorno ="";
            int fila= 0;
            while (fila<=lista_proveedores.Rows.Count-1)
            {
                if (nombre_en_BD == lista_proveedores.Rows[fila]["nombre_en_BD"].ToString())
                {
                    retorno = lista_proveedores.Rows[fila]["nombre_proveedor"].ToString();
                    break;
                }
                fila++;
            }
            return retorno;
        }
        #endregion

        #region metodos get/set
        public DataTable get_cuentas_por_pagar()
        {
            consultar_cuentas_por_pagar();
            return cuentas_por_pagar;
        }
        public string get_nombre_proveedor_de_remito(string id_remito)
        {
            consultar_cuentas_por_pagar();
            int fila_remito = funciones.buscar_fila_por_id(id_remito,cuentas_por_pagar);
            return obtener_nombre_proveedor(cuentas_por_pagar.Rows[fila_remito]["proveedor"].ToString());
        }
        #endregion


    }
}
