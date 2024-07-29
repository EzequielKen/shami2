using _01___modulos;
using modulos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace _03___sistemas_fabrica
{
    [Serializable]
    public class cls_historial_de_precios_fabrica
    {
        public cls_historial_de_precios_fabrica(DataTable usuario_BD) 
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
        DataTable usuarioBD;
        cls_consultas_Mysql consultas;
        cls_PDF PDF = new cls_PDF();
        DataTable productos_proveedor;
        DataTable acuerdos_de_precio;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;
        #endregion

        #region metodos consultas
        private void consultar_acuerdos_de_precio(string nombre_proveedor)
        {
            acuerdos_de_precio = consultas.consultar_acuerdo_de_precios_de_proveedor(base_de_datos, "acuerdo_de_precios", nombre_proveedor);
        }
        private void consultar_productos_proveedor(string nombre_proveedor)
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos,nombre_proveedor);
        }
        #endregion

        #region metodos get/set
        public DataTable get_productos_proveedor(string nombre_proveedor)
        {
            consultar_productos_proveedor(nombre_proveedor);
            return productos_proveedor;
        }
        public DataTable get_acuerdo_de_precios(string nombre_proveedor)
        {
            consultar_acuerdos_de_precio(nombre_proveedor);
            return acuerdos_de_precio;
        }
        #endregion

        #region metodos PDF
        public void crear_PDF(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable productos_proveedor, DataTable proveedorBD,string promedio_ganacia)
        {
           PDF.GenerarPDF_reporteria( ruta_archivo,  logo,  fecha_acuerdo,  acuerdo,  productos_proveedor,  proveedorBD,  promedio_ganacia);
        }
        public void crear_lista_de_precios_PDF(string ruta_archivo, byte[] logo, string fecha_acuerdo, string acuerdo, DataTable productos_proveedor, DataTable proveedorBD)
        {
            PDF.GenerarPDF_reporteria_lista_de_precios(ruta_archivo, logo, fecha_acuerdo, acuerdo, productos_proveedor, proveedorBD);
        }
        #endregion
    }
}
