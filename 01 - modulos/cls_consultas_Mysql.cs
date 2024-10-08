using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using acceso_a_base_de_datos;
using SkiaSharp;

namespace modulos
{
    [Serializable]
    public class cls_consultas_Mysql
    {
        public cls_consultas_Mysql(string servidor_dato, string puerto_dato, string usuario_dato, string password_dato, string base_de_datos_dato)
        {
            servidor = servidor_dato;
            puerto = puerto_dato;
            usuario = usuario_dato;
            password = password_dato;
            base_de_datos = base_de_datos_dato;
        }
        #region atributos
        private string servidor;
        private string puerto;
        private string usuario;
        private string password;
        private string base_de_datos;
        #endregion

        #region login
        public DataTable login(string usuario_query, string contraseña)
        {
            DataTable usuarios;
            string query = "SELECT * FROM " + base_de_datos + ".usuario where activa=1 and usuario = '" + usuario_query + "' and contraseña ='" + contraseña + "';";
            cls_conexion login = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            try
            {
                usuarios = login.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return usuarios;
        }
        public DataTable login_empleado(string contraseña)
        {
            DataTable usuarios;
            string query = "SELECT * FROM " + base_de_datos + ".lista_de_empleado where activa=1 and contraseña ='" + contraseña + "';";
            cls_conexion login = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            try
            {
                usuarios = login.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return usuarios;
        }
        public DataTable login_consultar_sucursal(string sucursal)
        {
            DataTable usuarios;
            string query = "SELECT * FROM " + base_de_datos + ".sucursal where activa=1 and sucursal ='" + sucursal + "';";
            cls_conexion login = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            try
            {
                usuarios = login.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return usuarios;
        }
        public DataTable login_admin(string usuario_query, string contraseña)
        {
            DataTable usuarios;
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where usuario_admin = '" + usuario_query + "' and contraseña_admin ='" + contraseña + "';";
            cls_conexion login = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            try
            {
                usuarios = login.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return usuarios;
        }
        #endregion


        #region consulta a tablas

        public DataTable consultar_tabla(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_tabla_completa(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla;
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public bool insertar_en_tabla(string base_de_datos, string tabla, string columnas, string valores)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            bool retorno = false;
            string query;
            try
            {
                query = "INSERT INTO " + "`" + base_de_datos + "`" + "." + "`" + tabla + "`" + "(" + columnas + ") VALUES (" + valores + ");";
                retorno = base_datos.CREATE(query);
            }
            catch (Exception ex)
            {
                retorno = false;
                throw ex;
            }

            return retorno;
        }

        public bool actualizar_tabla(string base_de_datos, string tabla, string actualizar, string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            bool retorno = false;
            string query;
            try
            {//`ultimo_pedido_enviado` = '8'
                query = "UPDATE `" + base_de_datos + "`.`" + tabla + "` SET " + actualizar + " WHERE(`id` = '" + id + "');";
                retorno = base_datos.UPDATE(query);
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return retorno;
        }
        #endregion

        #region consulta a tablas segun parametros
        public DataTable consultar_usuarios_no_admin()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".usuario where activa=1 and admin=0";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_venta_para_actualizar(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa = 1 and (venta!=0 or productos_fabrica_fatay!=0 or productos_caballito!=0)";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_producto_terminado_venta(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and venta=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_venta(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and venta=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_venta_vip(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and (venta=1 or venta=2)";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_venta_vip2(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and (venta=1 or venta=2 or venta=3)";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_venta_nivel4(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and (venta=1 or venta=2 or venta=3 or venta=4)";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabrica_fatay(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and productos_fabrica_fatay=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_caballito(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and productos_caballito=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar(string base_de_datos, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar where activa=1 and proveedor='" + proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_fabrica_e_insumos(string base_de_datos)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar where activa=1 and (proveedor='proveedor_villaMaipu' or proveedor='insumos_fabrica')";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_de_pedido(string base_de_datos, string proveedor, string sucursal, string num_pedido)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar where activa=1 and proveedor='" + proveedor + "' and sucursal='" + sucursal + "' and num_pedido='" + num_pedido + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_remito_de_pedido(string base_de_datos, string proveedor, string sucursal, string num_pedido)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".remitos where activa=1 and proveedor='" + proveedor + "' and sucursal='" + sucursal + "' and num_pedido='" + num_pedido + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_fabrica(string base_de_datos, string id_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar_fabrica where activa=1 and id_proveedor='" + id_proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_fabrica_por_id(string base_de_datos, string id_factura)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar_fabrica where activa=1 and id='" + id_factura + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precio_activo(string base_de_datos, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".acuerdo_de_precios where activa=1 and tipo_de_acuerdo='fabrica_a_local' and proveedor='" + proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precio_activo_fabrica_fatay(string base_de_datos, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".acuerdo_de_precios where activa=1 and tipo_de_acuerdo='fabrica_a_fabrica_fatay' and proveedor='" + proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_ventas_feria(string base_de_datos, string tabla, string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where  activa=1 and sucursal='" + sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_cargados(string base_de_datos, string tabla, string nombre_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where (estado='Pedido en proceso' or estado='local') and (activa=1    and proveedor='" + nombre_proveedor + "')";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_cargados_producto_terminado_e_insumo(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where (estado='Pedido en proceso' or estado='local' or  estado='Carga parcial') and ((activa=1 and proveedor='proveedor_villaMaipu') or (activa=1 and proveedor='insumos_fabrica'))";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }

        public DataTable consultar_pedidos_no_cargados_segun_sucursal(string base_de_datos, string tabla, string nombre_sucursal, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and (estado!='Listo para despachar' and estado!='Entregado' and estado!='Cancelado') and sucursal='" + nombre_sucursal + "' and proveedor='" + proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_cargados_segun_sucursal_producto_terminado_e_insumo(string base_de_datos, string tabla, string nombre_sucursal, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and (estado!='Listo para despachar' and estado!='Entregado' and estado!='Cancelado') and sucursal='" + nombre_sucursal + "' and ((activa=1 and proveedor='proveedor_villaMaipu') or (activa=1    and proveedor='insumos_fabrica'))";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_cargados_fabrica(string base_de_datos, string tabla, string nombre_fabrica)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where  activa=1 and estado='local' and fabrica='" + nombre_fabrica + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_calculados(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where  activa=1 and estado='Listo para despachar' and calculado_local='no'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_no_calculados_fabrica(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where  activa=1 and estado='cargado' and calculado_proveedor='no'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precios_segun_parametros(string base_de_datos, string tabla, string proveedor, string acuerdo_de_precios, string tipo_de_acuerdo)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where proveedor='" + proveedor + "' and acuerdo='" + acuerdo_de_precios + "' and tipo_de_acuerdo='" + tipo_de_acuerdo + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precios_de_proveedor(string base_de_datos, string tabla, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where proveedor='" + proveedor + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_disponibles(string base_de_datos, string tabla, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and proveedor='" + proveedor + "' and concepto='Disponible';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_produccion(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where produccion=1 and activa=1;";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_produccion_fabrica_fatay(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where produccion=1 and activa=1 and tipo_producto='2-Empanadas';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_todo_el_historial_produccion(string base_de_datos, string tabla, string fabrica)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and fabrica='" + fabrica + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_produccion_proveedor_cliente(string base_de_datos, string tabla, string fabrica, string proveedor, string receptor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and fabrica='" + fabrica + "' and proveedor='" + proveedor + "' and receptor='" + receptor + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_produccion_proveedor_receptor(string base_de_datos, string tabla, string fabrica, string receptor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1  and receptor='" + receptor + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_despacho_proveedor_receptor(string base_de_datos, string tabla,string receptor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1  and receptor='" + receptor + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_sucursal(int id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".sucursal where  activa=1 and id='" + id.ToString() + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_sucursal_por_nombre(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".sucursal where  activa=1 and sucursal='" + sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_proveeedor_id(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".lista_proveedores where activa=1 and id='" + id + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_tipo_usuario(int id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".tipo_usuario where activa=1 and id='" + id.ToString() + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedido_segun_mes_y_año(string base_de_datos, string tabla, string proveedor, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " WHERE proveedor='" + proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_totales_segun_mes_y_año(string base_de_datos, string tabla, string proveedor, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " WHERE proveedor='" + proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_promedio_segun_mes_y_año(string base_de_datos, string tabla, string proveedor, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " WHERE tipo_registro='promedio_del_mes' and  proveedor='" + proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_totales_completo(string base_de_datos, string tabla, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where proveedor='" + proveedor + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precio_fabrica_proveedor(string base_de_datos, string tabla, string id_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where id_proveedor='" + id_proveedor + "' and activa='1';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_ordenes_de_compra_de_proveedor(string id_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".ordenes_de_compra where  activa=1 and id_proveedor='" + id_proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_ordenes_de_compra_de_proveedor_por_id(string id_orden)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".ordenes_de_compra where  activa=1 and id='" + id_orden + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_ordenes_de_compra_de_proveedor_abiertas(string id_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".ordenes_de_compra where  activa=1 and estado='Abierta' and id_proveedor='" + id_proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precios_fabrica_a_proveedores_de_orden(string acuerdo, string id_proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".acuerdo_de_precios_fabrica_a_proveedores where  acuerdo='" + acuerdo + "' and id_proveedor='" + id_proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_proveedores_de_fabrica_seleccionado(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".proveedores_de_fabrica where ID='" + id + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_proveedor_sin_insumos(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and insumo=0";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_proveedor_productos_terminados(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and producto_terminado=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_proveedor_productos_produccion(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and produccion=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_sucursal_carrefour_por_id(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".sucursales_carrefour where ID='" + id + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precio_activo_carrefour()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".acuerdo_de_precios_carrefour where activa='1'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimientos_carrefour_segun_id_sucursal(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_carrefour where id_sucursal='" + id_sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimientos_carrefour_no_calculados()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_carrefour where calculado='No'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimientos_carrefour(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_carrefour where sucursal='" + sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_imputaciones_carrefour_segun_sucursal(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".imputaciones_carrefour where sucursal='" + sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimientos_seleccionado(string id_movimiento)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_carrefour where id='" + id_movimiento + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimiento_segun_mes_y_año(string base_de_datos, string sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_carrefour WHERE sucursal='" + sucursal + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedido_de_insumos_por_id(string id_movimiento)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".pedido_de_insumos where id='" + id_movimiento + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_producto_segun_mes_y_año(string base_de_datos, string tabla, string id_producto, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " WHERE id_producto='" + id_producto + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_acuerdo_de_precios_fabrica_a_proveedores_activo()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".acuerdo_de_precios_fabrica_a_proveedores where activa='1'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_segun_rango_de_fecha(string sucursal, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".pedidos WHERE activa='1' and estado !='Cancelado' AND sucursal='" + sucursal + "' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_entregados_segun_rango_de_fecha(string sucursal, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".pedidos WHERE  estado !='Cancelado' and activa ='1' AND sucursal='" + sucursal + "' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedidos_produccion_segun_rango_de_fecha(string solicita, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".pedido_de_insumos WHERE activa='1' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_registro_actividad_del_dia(string base_de_datos, string tabla)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1 and DATE(FECHA) = CURDATE()";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_sub_productos()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".proveedor_villaMaipu where activa='1' and tipo_producto='19-Sub Productos'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_produccion_segun_rango_de_fecha(string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".produccion_diaria WHERE  activa ='1' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_detalles_caja_chica(string concepto, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_caja_chica WHERE activa=1 and MONTH(fecha) = '" + mes + "' and year(fecha)='" + año + "' and concepto='" + concepto + "'	;";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimiento_segun_concepto(string concepto)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimientos_caja_chica WHERE  concepto='" + concepto + "'	;";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_equipo_segun_ubicacion(string ubicacion)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".equipos WHERE activa=1 and ubicacion='" + ubicacion + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_temperatura_segun_fecha(string año, string mes, string dia)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".temperatura_de_equipos WHERE activa=1 and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_configuracion_chequeo(string perfil)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".configuracion_de_chequeo WHERE activa=1 and  perfil='" + perfil + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_empleados(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".lista_de_empleado WHERE activa=1 and id_sucursal='" + id_sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_empleados_origen(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".lista_de_empleado WHERE activa=1 and id_sucursal_origen='" + id_sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_empleados_segun_fecha(string id_sucursal, string año, string mes, string dia)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".registro_logueo_empleados WHERE activa=1 and id_sucursal='" + id_sucursal + "' and YEAR(fecha_logueo)='" + año + "' and MONTH(fecha_logueo)='" + mes + "' and DAY(fecha_logueo)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_empleado(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".lista_de_empleado WHERE activa=1 and id='" + id + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_usuario_segun_sucural(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".usuario WHERE activa=1 and sucursal='" + id_sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_chequeo_segun_fecha(string año, string mes, string dia, string hora_inicio, string hora_fin, string id_empleado, string id_sucursal, string turno)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".historial_lista_chequeo WHERE activa=1 and turno_logueado='" + turno + "' and id_sucursal='" + id_sucursal + "' and id_empleado='" + id_empleado + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "' AND TIME(fecha) BETWEEN '" + hora_inicio + "' AND '" + hora_fin + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_evaluacion_chequeo_segun_fecha(string año, string mes, string dia, string id_empleado, string id_sucursal, string cargo)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".historial_evaluacion_chequeo WHERE activa=1 and cargo='" + cargo + "' and id_sucursal='" + id_sucursal + "' and id_empleado='" + id_empleado + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_registro_venta_local_segun_fecha(string id_sucursal, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".registro_venta_local WHERE  activa ='1' and id_sucursal='" + id_sucursal + "' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_deuda_mes_local(string id_sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".deudas_local_a_fabrica WHERE  activa ='1' and id_sucursal='" + id_sucursal + "' AND mes = '" + mes + "' AND año='" + año + "'";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuenta_por_pagar_segun_fecha(string sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar WHERE activa=1 and sucursal='" + sucursal + "' and YEAR(fecha_remito)='" + año + "' and MONTH(fecha_remito)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_remitos_segun_fecha(string sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".remitos WHERE activa=1 and sucursal='" + sucursal + "' and YEAR(fecha_remito)='" + año + "' and MONTH(fecha_remito)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_segun_sucursal(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar WHERE activa=1 and sucursal='" + sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_cuentas_por_pagar_segun_fecha(string sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar WHERE activa=1 and sucursal='" + sucursal + "' and YEAR(fecha_remito)='" + año + "' and MONTH(fecha_remito)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_imputaciones_segun_sucursal(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".imputaciones WHERE activa=1 and sucursal='" + sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_imputaciones_segun_fecha(string sucursal, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".imputaciones WHERE activa=1 and sucursal='" + sucursal + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_deudas_de_sucursal(string sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".deudas_local_a_fabrica WHERE activa=1 and sucursal='" + sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_remitos_todas_las_sucursales(string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar WHERE activa=1  and YEAR(fecha_remito)='" + año + "' and MONTH(fecha_remito)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_tabla_produccion()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".insumos_fabrica WHERE activa=1 and tabla_produccion='1';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_productos_terminado_tabla_produccion()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".proveedor_villaMaipu WHERE activa=1 and tabla_produccion='1';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_tabla_produccion(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".tabla_produccion WHERE  activa ='1' and id_sucursal='" + id_sucursal + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_tabla_produccion(string id_sucursal, string id_empleado, string id_tabla_produccion, string turno)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".historial_tabla_produccion WHERE activa=1 and id_sucursal='" + id_sucursal + "' and id_empleado='" + id_empleado + "'and id_tabla_produccion='" + id_tabla_produccion + "' and turno='" + turno + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_pedido(string base_de_datos, string proveedor, string sucursal, string num_pedido)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".pedidos where activa=1 and proveedor='" + proveedor + "' and sucursal='" + sucursal + "' and num_pedido='" + num_pedido + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_lista_de_faltantes(string id_sucursal)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".lista_de_faltantes where activa=1 and id_sucursal='" + id_sucursal + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_consumo_personal_segun_fecha(string id_sucursal, string dia, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".historial_consumo_personal WHERE activa=1 and id_sucursal='" + id_sucursal + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_horarios_de_empleados_segun_fecha(string id_sucursal, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".horarios_de_empleados WHERE  activa ='1' and id_sucursal='" + id_sucursal + "' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_horarios_de_empleado_segun_fecha(string id_sucursal, string id_empleado, string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".horarios_de_empleados WHERE  activa ='1' and id_sucursal='" + id_sucursal + "' and id_empleado='" + id_empleado + "' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_historial_visita_operativa_segun_fecha(string id_sucursal, string año, string mes, string dia)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".historial_evaluacion_chequeo WHERE activa=1 and id_sucursal='" + id_sucursal + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_observaciones_visita_operativa_segun_fecha(string id_sucursal, string año, string mes, string dia)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".observaciones_visita_operativa WHERE activa=1 and id_sucursal='" + id_sucursal + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_desperdicio_merma_local_segun_fecha(string id_sucursal, string año, string mes, string dia, string categoria)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".desperdicio_merma_local WHERE activa=1 and categoria='" + categoria + "' and id_sucursal='" + id_sucursal + "' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_stock_producto_terminado_segun_producto(string id_producto)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".stock_producto_terminado WHERE activa=1 and id_producto='" + id_producto + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_stock_insumos_segun_producto(string id_producto)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".stock_insumos WHERE activa=1 and id_producto='" + id_producto + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_stock_producto_terminado_fabrica_fatay_segun_producto(string id_producto)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".stock_producto_terminado_fabrica_fatay WHERE activa=1 and id_producto='" + id_producto + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_insumos_fabricas_optimizado()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT activa, id, producto, tipo_producto ,tipo_producto_local,unidad_tabla_produccion,venta,productos_fabrica_fatay,unidad_de_medida_local,equivalencia,precio,stock_produccion,stock,unidad_medida,tabla_produccion FROM " + base_de_datos + ".insumos_fabrica WHERE activa=1;";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_conteo_stock_segun_fecha(string dia, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".conteo_stock WHERE activa=1 and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_todos_los_remitos_cuenta_por_pagar()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar where activa=1 and (proveedor='proveedor_villaMaipu' or proveedor='insumos_fabrica');";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_todas_las_imputaciones_local()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".imputaciones where activa=1 and (proveedor='proveedor_villaMaipu' or proveedor='insumos_fabrica');";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_produccion_semanal_fabrica_fatay_segun_rango_de_fecha(string fecha_inicio, string fecha_fin)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".produccion_diaria WHERE activa='1'and fabrica='Fabrica Fatay Callao' AND (fecha >= '" + fecha_inicio + "' AND fecha < DATE_ADD('" + fecha_fin + "', INTERVAL 1 DAY))";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimiento_mercaderia_interna_segun_fecha(string dia, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".movimiento_mercaderia_interna WHERE activa=1 and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_movimiento_mercaderia_interna_por_id(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM shami.movimiento_mercaderia_interna where activa=1 and id='" + id + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        #endregion

        #region consultas fabrica a proveedor
        public DataTable consultar_proveeedor_de_fabrica_por_id(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".proveedores_de_fabrica where id='" + id + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }

        public DataTable consultar_acuerdo_de_precios_fabrica_a_proveedores_activo(string base_de_datos, string tabla, string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activa=1  and id_proveedor='" + id + "';";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }

        public DataTable consultar_cuenta_por_pagar_fabrica(string id)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar_fabrica where id_proveedor='" + id + "' and estado_entrega='Entrega parcial'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_orden_de_compras_segun_mes(string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".ordenes_de_compra WHERE activa=1  and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_orden_de_compras_incompletas_segun_mes(string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT activa,id,id_proveedor,proveedor,acuerdo_de_precios,estado,fecha,fecha_entrega_estimada,fecha_entrega,nota,tipo_de_pago,condicion_pago,cantidad_a_pagar FROM " + base_de_datos + ".ordenes_de_compra WHERE activa=1  and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_orden_de_compras_abiertas()
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT activa,id,id_proveedor,proveedor,acuerdo_de_precios,estado,fecha,fecha_entrega_estimada,fecha_entrega,nota,tipo_de_pago,condicion_pago,cantidad_a_pagar FROM " + base_de_datos + ".ordenes_de_compra WHERE activa=1  and estado='Abierta';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        #endregion

        #region verificaciones 
        public DataTable consultar_produccion(string fabrica, string proveedor, string receptor, string fecha)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".produccion_diaria where fabrica='" + fabrica + "' and proveedor='" + proveedor + "' and receptor='" + receptor + "' and fecha='" + fecha + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }

        public DataTable consultar_remito_cuentas_por_pagar(string sucursal, string num_pedido, string nombre_remito, string valor_remito, string proveedor)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".cuenta_por_pagar where sucursal='" + sucursal + "' and num_pedido='" + num_pedido + "' and nombre_remito='" + nombre_remito + "' and valor_remito='" + valor_remito + "' and proveedor='" + proveedor + "'";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_descripcion_de_cargos(string base_de_datos, string rol)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".descripcion_de_cargos where rol='" + rol + "'and activa=1";
                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public DataTable consultar_conteo_stock_segun_fecha_e_id(string id_producto,string dia, string mes, string año)
        {
            cls_conexion base_datos = new cls_conexion(servidor, puerto, usuario, password, base_de_datos);
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + ".conteo_stock WHERE activa=1 and id_producto='"+id_producto+"' and YEAR(fecha)='" + año + "' and MONTH(fecha)='" + mes + "' and DAY(fecha)='" + dia + "';";


                retorno = base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        #endregion
    }



}

