using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas
{
    public class cls_actualizar_deuda
    {
        public cls_actualizar_deuda(DataTable usuario_BD)
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
        cls_sistema_cuentas_por_pagar cuentas_por_pagar;
        cls_consultas_Mysql consultas;
        cls_funciones funciones = new cls_funciones();
        DataTable usuarioBD;
        string servidor, puerto, usuario_dato, contraseña_BD, base_de_datos;

        DataTable sucursales;
        DataTable sucursal;
        #endregion

        #region actualizar deuda
        public void actualizar_deuda()
        {
            DateTime fecha_dato = new DateTime(2023, 10, 1);
            DateTime fecha_actual = DateTime.Now;
            consultar_sucursales();
            string id_sucursal, sucursal_nombre, mes, año;
            for (int fila = 0; fila <= sucursales.Rows.Count - 1; fila++)
            {
                llenar_tabla_sucursal(fila);
                cuentas_por_pagar = new cls_sistema_cuentas_por_pagar(usuarioBD, sucursal);
                while (fecha_dato.Year == fecha_actual.Year && fecha_dato.Month == fecha_actual.Month)
                {
                    id_sucursal = sucursal.Rows[0]["id"].ToString();
                    sucursal_nombre = sucursal.Rows[0]["sucursal"].ToString();
                    mes = fecha_dato.Month.ToString();
                    año = fecha_dato.Year.ToString();
                    cuentas_por_pagar.actualizar_deuda_total_mes(id_sucursal,sucursal_nombre,mes,año);
                    fecha_dato.AddMonths(1);
                }
            }
        }
        #endregion

        #region metodos consultas
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla_completa(base_de_datos, "sucursal");
        }
        #endregion

        #region metodos privados
        private void crear_tabla_sucursal()
        {
            sucursal = new DataTable();
            sucursal.Columns.Add("activa", typeof(string));
            sucursal.Columns.Add("id", typeof(string));
            sucursal.Columns.Add("sucursal", typeof(string));
            sucursal.Columns.Add("provincia", typeof(string));
            sucursal.Columns.Add("localidad", typeof(string));
            sucursal.Columns.Add("direccion", typeof(string));
            sucursal.Columns.Add("telefono", typeof(string));
            sucursal.Columns.Add("franquiciado", typeof(string));
            sucursal.Columns.Add("franquicia", typeof(string));
            sucursal.Columns.Add("ultimo_pedido_enviado", typeof(string));
            sucursal.Columns.Add("correo", typeof(string));
            sucursal.Columns.Add("contraseña", typeof(string));
            sucursal.Columns.Add("bloquear_por_deuda", typeof(string));
        }

        private void llenar_tabla_sucursal(int fila)
        {
            crear_tabla_sucursal();
            sucursal.Rows.Add();
            int ultima_fila = sucursal.Rows.Count - 1;

            sucursal.Rows[ultima_fila]["activa"] = sucursales.Rows[fila]["activa"].ToString();
            sucursal.Rows[ultima_fila]["id"] = sucursales.Rows[fila]["id"].ToString();
            sucursal.Rows[ultima_fila]["sucursal"] = sucursales.Rows[fila]["sucursal"].ToString();
            sucursal.Rows[ultima_fila]["provincia"] = sucursales.Rows[fila]["provincia"].ToString();
            sucursal.Rows[ultima_fila]["localidad"] = sucursales.Rows[fila]["localidad"].ToString();
            sucursal.Rows[ultima_fila]["direccion"] = sucursales.Rows[fila]["direccion"].ToString();
            sucursal.Rows[ultima_fila]["telefono"] = sucursales.Rows[fila]["telefono"].ToString();
            sucursal.Rows[ultima_fila]["franquiciado"] = sucursales.Rows[fila]["franquiciado"].ToString();
            sucursal.Rows[ultima_fila]["franquicia"] = sucursales.Rows[fila]["franquicia"].ToString();
            sucursal.Rows[ultima_fila]["ultimo_pedido_enviado"] = sucursales.Rows[fila]["ultimo_pedido_enviado"].ToString();
            sucursal.Rows[ultima_fila]["correo"] = sucursales.Rows[fila]["correo"].ToString();
            sucursal.Rows[ultima_fila]["contraseña"] = sucursales.Rows[fila]["contraseña"].ToString();
            sucursal.Rows[ultima_fila]["bloquear_por_deuda"] = sucursales.Rows[fila]["bloquear_por_deuda"].ToString();
        }

        #endregion
    }
}
