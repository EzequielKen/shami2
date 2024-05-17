using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05___sistemas_fabrica_fatay
{
    public class cls_plantillas_fabrica_fatay
    {
        public cls_plantillas_fabrica_fatay(DataTable usuario_BD)
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
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        cls_PDF PDF = new cls_PDF();
        DataTable productos_proveedor_sin_insumos;
        DataTable productos_proveedor_productos_terminados;
        DataTable insumos_fabrica;
        #endregion

        #region metodos consultas
        private void consultar_insumos_fabrica()
        {
            insumos_fabrica = consultas.consultar_insumos_fabrica_fatay(base_de_datos, "insumos_fabrica_fatay");
        }
        private void consultar_productos_proveedor_sin_insumos(string nombre_proveedor)
        {
            productos_proveedor_sin_insumos = consultas.consultar_tabla_completa(base_de_datos, nombre_proveedor);
        }
        private void consultar_productos_proveedor_productos_terminados(string nombre_proveedor)
        {
            productos_proveedor_productos_terminados = consultas.consultar_productos_proveedor_productos_terminados(base_de_datos, nombre_proveedor);
        }
        #endregion

        #region metodos get/set
        public void crear_PDF_plantilla_de_todos_los_productos(string ruta_archivo, byte[] logo, string nombre_proveedor)
        {
            consultar_productos_proveedor_sin_insumos(nombre_proveedor);
            productos_proveedor_sin_insumos.DefaultView.Sort = "tipo_producto ASC";
            productos_proveedor_sin_insumos = productos_proveedor_sin_insumos.DefaultView.ToTable();
            PDF.GenerarPDF_plantilla_de_stock(ruta_archivo, logo, productos_proveedor_sin_insumos, "TODOS LOS PRODUCTOS");
        }
        public void crear_PDF_plantilla_de_insuoms(string ruta_archivo, byte[] logo)
        {
            consultar_insumos_fabrica();
            insumos_fabrica.DefaultView.Sort = "tipo_producto ASC";
            insumos_fabrica = insumos_fabrica.DefaultView.ToTable();
            PDF.GenerarPDF_plantilla_de_stock(ruta_archivo, logo, insumos_fabrica, "INSUMOS");
        }
        public void crear_PDF_plantilla_de_productos_terminados(string ruta_archivo, byte[] logo, string nombre_proveedor)
        {
            consultar_productos_proveedor_productos_terminados(nombre_proveedor);
            productos_proveedor_productos_terminados.DefaultView.Sort = "tipo_producto ASC";
            productos_proveedor_productos_terminados = productos_proveedor_productos_terminados.DefaultView.ToTable();
            PDF.GenerarPDF_plantilla_de_stock(ruta_archivo, logo, productos_proveedor_productos_terminados, "PRODUCTOS FABRICADOS");
        }
        #endregion
    }
}
