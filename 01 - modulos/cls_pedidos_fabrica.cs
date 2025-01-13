using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using acceso_a_base_de_datos;
using paginaWeb;
namespace modulos
{
    [Serializable]
    public class cls_pedidos_fabrica
    {
        public cls_pedidos_fabrica(DataTable usuarios_BD)
        {
            usuariosBD = usuarios_BD;
            servidor = usuariosBD.Rows[0]["servidor"].ToString();
            puerto = usuariosBD.Rows[0]["puerto"].ToString();
            usuario = usuariosBD.Rows[0]["usuario_BD"].ToString();
            password = usuariosBD.Rows[0]["contraseña_BD"].ToString();
            if ("1" == ConfigurationManager.AppSettings["produccion"])
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos"];
            }
            else
            {
                base_de_datos = ConfigurationManager.AppSettings["base_de_datos_desarrollo"];
            }


            consultas = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);

            crear_pedidos_y_bonificado();

        }
        #region atributos
        private cls_consultas_Mysql consultas;
        private cls_funciones funciones = new cls_funciones();
        private string servidor;
        private string puerto;
        private string usuario;
        private string password;
        private string base_de_datos;


        private string sucursal;
        private DataTable usuariosBD;
        private DataTable sucursales;
        private DataTable pedidos;
        private DataTable bonificados;

        private DataTable lista_proveedores;
        private DataTable lista_proveedores_fabrica;
        private DataTable productos_proveedor;
        private DataTable tipo_acuerdo;
        private DataTable tipo_acuerdo_fabrica;
        private DataTable tipo_acuerdo_proveedor_a_fabrica;
        private DataTable acuerdo_de_precios;
        private DataTable acuerdo_de_precio_activo;
        private DataTable pedidos_sucursales;
        private DataTable pedidos_no_cargados;
        private DataTable pedidos_fabrica;
        private DataTable insumo;

        private string condicion_bonificado;
        private string productos_bonificados;
        private string productos_bonificados_especiales;
        private string precio_bonificado_especial;
        private List<int> productos_bonificados_lista;
        private List<int> productos_bonificados_especiales_lista;

        private string proveedor_seleccionado;
        private string tipo_de_acuerdo;
        private string acuerdo_de_precio;
        private string tipo_de_acuerdo_fabrica;
        private string tipo_de_acuerdo_proveedor_a_fabrica;
        #endregion region

        #region metodos privados de consulta
        private void consultar_lista_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla(base_de_datos, "lista_proveedores");
        }

        private void consultar_productos_proveedor(string proveedor_seleccionado)
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos, proveedor_seleccionado);
        }
        private void consultar_tipo_acuerdo()
        {
            tipo_acuerdo = consultas.consultar_tabla(base_de_datos, "tipo_acuerdo");
          //  tipo_acuerdo_fabrica = consultas.consultar_tabla(base_de_datos, "tipo_acuerdo_fabrica_a_marca");
        }
        private void consultar_acuerdo_de_precio_activo(string nombre_proveedor)
        {
            acuerdo_de_precio_activo = consultas.consultar_acuerdo_de_precio_activo(base_de_datos, nombre_proveedor);
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = consultas.consultar_tabla_completa(base_de_datos, "acuerdo_de_precios");
        }
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        private void consultar_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla(base_de_datos, "lista_proveedores");
        }
        #endregion
        public DataTable consultar_remito_cuentas_por_pagar(string sucursal, string num_pedido, string nombre_remito, string valor_remito, string proveedor)
        {
            return consultas.consultar_remito_cuentas_por_pagar(sucursal, num_pedido, nombre_remito, valor_remito, proveedor);
        }
        private void consultar_pedidos_sucursales(string nombre_sucursal, string nombre_proveedor)
        {
            pedidos_sucursales = consultas.consultar_pedidos_no_cargados_segun_sucursal(base_de_datos, "pedidos", nombre_sucursal, nombre_proveedor);
        }
        private void consultar_pedidos_sucursales_producto_terminado_e_insumo(string nombre_sucursal, string nombre_proveedor)
        {
            pedidos_sucursales = consultas.consultar_pedidos_no_cargados_segun_sucursal_producto_terminado_e_insumo(base_de_datos, "pedidos", nombre_sucursal, nombre_proveedor);
        }
        private void consultar_pedidos_no_cargados(string nombre_proveedor)
        {
            pedidos_no_cargados = consultas.consultar_pedidos_no_cargados(base_de_datos, "pedidos", nombre_proveedor);
        }
        private void consultar_pedidos_no_cargados_producto_terminado_e_insumo(string nombre_proveedor)
        {
            pedidos_no_cargados = consultas.consultar_pedidos_no_cargados_producto_terminado_e_insumo(base_de_datos, "pedidos");
        }

        private void consultar_pedidos_fabrica(string nombre_fabrica)
        {
            pedidos_fabrica = consultas.consultar_pedidos_no_cargados_fabrica(base_de_datos, "pedidos_fabrica_a_proveedor", nombre_fabrica);
        }

        private void consultar_insumnos()
        {
            insumo = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        #region metodos get/set
        public DataTable get_insumos()
        {
            consultar_insumnos();
            return insumo;
        }
        public DataTable get_acuerdo_de_precio_activo(string nombre_proveedor)
        {
            consultar_acuerdo_de_precio_activo(nombre_proveedor);
            return acuerdo_de_precio_activo;
        }
        public DataTable get_acuerdo_de_precios()
        {
            consultar_acuerdo_de_precios();
            return acuerdo_de_precios;
        }
        public DataTable get_pedidos_sucursales(string nombre_sucursal, string nombre_proveedor)
        {
            consultar_pedidos_sucursales(nombre_sucursal, nombre_proveedor);
            return pedidos_sucursales;
        }
        public DataTable get_pedidos_sucursales_producto_terminado_e_insumo(string nombre_sucursal, string nombre_proveedor)
        {
            consultar_pedidos_sucursales_producto_terminado_e_insumo(nombre_sucursal, nombre_proveedor);
            return pedidos_sucursales;
        }
        public DataTable get_pedidos_no_cargados(string nombre_proveedor)
        {
            consultar_pedidos_no_cargados(nombre_proveedor);
            return pedidos_no_cargados;
        }
        public DataTable get_pedidos_no_cargados_producto_terminado_e_insumo(string nombre_proveedor)
        {
            consultar_pedidos_no_cargados_producto_terminado_e_insumo(nombre_proveedor);
            return pedidos_no_cargados;
        }
        public DataTable get_pedidos_fabrica(string nombre_fabrica)
        {
            consultar_pedidos_fabrica(nombre_fabrica);
            return pedidos_fabrica;
        }
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        public DataTable get_proveedores_fabrica()
        {
            consultar_proveedores();
            return lista_proveedores_fabrica;
        }
        public DataTable get_lista_proveedores()
        {
            consultar_lista_proveedores();
            return lista_proveedores;
        }

        public DataTable get_productos_proveedor(string proveedor, string sucursalBD)
        {
            sucursal = sucursalBD;
            int fila_acuerdo_de_precios;
            consultar_lista_proveedores();
            proveedor_seleccionado = proveedor;
            consultar_productos_proveedor(proveedor_seleccionado);
            consultar_tipo_acuerdo();
            consultar_acuerdo_de_precios();

            tipo_de_acuerdo = obtener_tipo_de_acuerdo(proveedor_seleccionado);
            //tipo_de_acuerdo_fabrica = obtener_tipo_de_acuerdo_fabrica(proveedor_seleccionado);
            fila_acuerdo_de_precios = obtener_fila_de_acuerdo(tipo_de_acuerdo, proveedor_seleccionado);


            acuerdo_de_precio = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["acuerdo"].ToString();
            actualizar_precio_productos(fila_acuerdo_de_precios);

            return productos_proveedor;
        }

        public DataTable get_productos_proveedor_fabrica(string proveedor)
        {
            int fila_acuerdo_de_precios;
            consultar_proveedores();
            consultar_productos_proveedor(proveedor);
            consultar_tipo_acuerdo();
            consultar_acuerdo_de_precios();

            tipo_de_acuerdo_proveedor_a_fabrica = obtener_tipo_acuerdo_fabrica_a_marca(proveedor);
            fila_acuerdo_de_precios = obtener_fila_de_acuerdo(tipo_de_acuerdo_proveedor_a_fabrica, proveedor);


            acuerdo_de_precio = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["acuerdo"].ToString();
            actualizar_precio_productos(fila_acuerdo_de_precios);

            return productos_proveedor;
        }


        public DataTable get_pedidos()
        {
            return pedidos;
        }

        public DataTable get_bonificado()
        {
            return bonificados;
        }

        public int get_condicion_bonificado()
        {
            return int.Parse(condicion_bonificado);
        }
        public List<int> get_productos_bonificados_lista()
        {
            return productos_bonificados_lista;
        }
        public List<int> get_productos_bonificados_especiales_lista()
        {
            return productos_bonificados_especiales_lista;
        }
        public string get_precios_bonificados_especial()
        {
            return precio_bonificado_especial;
        }
        #endregion


        #region metodos privados
        private void actualizar_precio_productos(int fila_acuerdo_de_precios)
        {
            string id_producto;

            for (int fila = 0; fila <= productos_proveedor.Rows.Count - 1; fila++)
            {
                id_producto = productos_proveedor.Rows[fila]["id"].ToString();

                if (!DBNull.Value.Equals(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["producto_" + id_producto].ToString()))
                {
                    productos_proveedor.Rows[fila]["precio"] = Math.Round(float.Parse(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["producto_" + id_producto].ToString()), 2);
                }
                else
                {
                    productos_proveedor.Rows[fila]["precio"] = 0;
                }
            }

            setear_conficion_bonificado(fila_acuerdo_de_precios);
        }
        private void setear_conficion_bonificado(int fila_acuerdo_de_precios)
        {
            if (!DBNull.Value.Equals(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["condicion_bonificado"].ToString()))
            {
                condicion_bonificado = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["condicion_bonificado"].ToString();
                setear_productos_bonificados(fila_acuerdo_de_precios);
            }
            else
            {
                condicion_bonificado = "0";
                productos_bonificados = "0";
            }
        }
        private void setear_productos_bonificados(int fila_acuerdo_de_precios)
        {
            if (!DBNull.Value.Equals(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["bonificados"].ToString()))
            {
                productos_bonificados = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["bonificados"].ToString();
                crear_lista_productos_bonificados();
            }
            else
            {
                productos_bonificados = "0";
            }

            if (!DBNull.Value.Equals(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["bonificados_especiales"].ToString()))
            {
                productos_bonificados_especiales = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["bonificados_especiales"].ToString();
                precio_bonificado_especial = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["precio_bonificado"].ToString();
                crear_lista_productos_bonificados_especiales();
            }
            else
            {
                productos_bonificados_especiales = "0";
            }

        }
        private void crear_lista_productos_bonificados()
        {
            string id = "";
            productos_bonificados_lista = new List<int>();
            for (int i = 0; i <= productos_bonificados.Length - 1; i++)
            {
                if (productos_bonificados[i].ToString() != "-")
                {
                    id = id + productos_bonificados[i].ToString();
                }
                else if (productos_bonificados[i].ToString() == "-")
                {
                    productos_bonificados_lista.Add(int.Parse(id));
                    id = "";
                }

            }
            if (id != "")
            {
                productos_bonificados_lista.Add(int.Parse(id));
            }
        }

        private void crear_lista_productos_bonificados_especiales()
        {
            string id = "";
            productos_bonificados_especiales_lista = new List<int>();
            for (int i = 0; i <= productos_bonificados_especiales.Length - 1; i++)
            {
                if (productos_bonificados_especiales[i].ToString() != "-")
                {
                    id = id + productos_bonificados_especiales[i].ToString();
                }
                else if (productos_bonificados_especiales[i].ToString() == "-")
                {
                    productos_bonificados_especiales_lista.Add(int.Parse(id));
                    id = "";
                }

            }
            if (id != "")
            {
                productos_bonificados_especiales_lista.Add(int.Parse(id));
            }
        }
        private int obtener_fila_de_acuerdo(string tipo_de_acuerdo, string proveedor_seleccionado)
        {
            int retorno = 0;
            int fila;

            fila = 0;
            while (fila <= acuerdo_de_precios.Rows.Count - 1)
            {
                if (proveedor_seleccionado == acuerdo_de_precios.Rows[fila]["proveedor"].ToString()
                    && tipo_de_acuerdo == acuerdo_de_precios.Rows[fila]["tipo_de_acuerdo"].ToString()
                    && acuerdo_de_precios.Rows[fila]["activa"].ToString() == "1")
                {
                    retorno = fila;
                    fila = acuerdo_de_precios.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_tipo_de_acuerdo(string proveedor_seleccionado)
        {
            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= tipo_acuerdo.Rows.Count - 1)
            {
                if (sucursal == tipo_acuerdo.Rows[fila]["sucursal"].ToString())
                {
                    retorno = tipo_acuerdo.Rows[fila][proveedor_seleccionado].ToString();
                    fila = tipo_acuerdo.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_tipo_de_acuerdo_fabrica(string proveedor_seleccionado)
        {

            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= tipo_acuerdo_fabrica.Rows.Count - 1)
            {
                if (proveedor_seleccionado == tipo_acuerdo_fabrica.Rows[fila]["proveedor"].ToString())
                {
                    retorno = tipo_acuerdo_fabrica.Rows[fila][sucursal].ToString();
                    fila = tipo_acuerdo_fabrica.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_tipo_acuerdo_proveedor_a_fabrica(string proveedor_seleccionado)
        {
            if (tipo_acuerdo_proveedor_a_fabrica == null)
            {
                consultar_tipo_acuerdo();
            }

            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= tipo_acuerdo_proveedor_a_fabrica.Rows.Count - 1)
            {
                if (proveedor_seleccionado == tipo_acuerdo_proveedor_a_fabrica.Rows[fila]["proveedor"].ToString())
                {
                    retorno = tipo_acuerdo_proveedor_a_fabrica.Rows[fila]["tipo_acuerdo"].ToString();
                    break;
                }
                fila = fila + 1;
            }
            return retorno;
        }


        private string obtener_tipo_acuerdo_fabrica_a_marca(string proveedor_seleccionado)
        {
            if (tipo_acuerdo_fabrica == null)
            {
                consultar_tipo_acuerdo();
            }

            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= tipo_acuerdo_fabrica.Rows.Count - 1)
            {
                if (proveedor_seleccionado == tipo_acuerdo_fabrica.Rows[fila]["proveedor"].ToString())
                {
                    retorno = tipo_acuerdo_fabrica.Rows[fila]["caballito"].ToString();
                    break;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_nombre_proveedor(string provedor_seleccionado)
        {
            string retorno = "";
            int fila;
            fila = 0;

            while (fila <= lista_proveedores.Rows.Count - 1)
            {
                if (provedor_seleccionado == lista_proveedores.Rows[fila]["nombre_proveedor"].ToString())
                {
                    retorno = lista_proveedores.Rows[fila]["nombre_en_BD"].ToString();
                    fila = lista_proveedores.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private string obtener_nombre_proveedor_fabrica(string provedor_seleccionado)
        {
            string retorno = provedor_seleccionado;
            int fila;
            fila = 0;

            while (fila <= lista_proveedores_fabrica.Rows.Count - 1)
            {
                if (provedor_seleccionado == lista_proveedores_fabrica.Rows[fila]["nombre_proveedor"].ToString())
                {
                    retorno = lista_proveedores_fabrica.Rows[fila]["nombre_en_BD"].ToString();
                    break;
                }
                fila = fila + 1;
            }
            return retorno;
        }
        private void crear_pedidos_y_bonificado()
        {
            pedidos = new DataTable();
            pedidos.Columns.Add("id", typeof(string));
            pedidos.Columns.Add("producto", typeof(string));
            pedidos.Columns.Add("cantidad_local", typeof(string));
            pedidos.Columns.Add("precio", typeof(string));
            pedidos.Columns.Add("producto_bonificable", typeof(string));
            pedidos.Columns.Add("tipo_producto", typeof(string));

            bonificados = new DataTable();
            bonificados.Columns.Add("id", typeof(string));
            bonificados.Columns.Add("producto", typeof(string));
            bonificados.Columns.Add("cantidad_local", typeof(string));
            bonificados.Columns.Add("precio", typeof(string));
            bonificados.Columns.Add("tipo_producto", typeof(string));
        }

        #endregion

        #region enviar
        public void actualizar_stock(string proveedor_seleccionado, DataTable pedido)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            string id = "";
            string actualizar = "";
            double stock, nuevo_stock, cantidad_despachada;
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                if (pedido.Rows[fila]["proveedor"].ToString() == proveedor_seleccionado)
                {
                    if (pedido.Rows[fila]["nuevo_stock"].ToString() != "N/A")
                    {
                        stock = double.Parse(pedido.Rows[fila]["stock"].ToString());
                        nuevo_stock = double.Parse(pedido.Rows[fila]["nuevo_stock"].ToString());
                        cantidad_despachada = nuevo_stock - stock;

                        actualizar = "`stock` = '" + pedido.Rows[fila]["nuevo_stock"].ToString() + "'";
                        id = pedido.Rows[fila]["id"].ToString();
                        consultas.actualizar_tabla(base_de_datos, proveedor_seleccionado, actualizar, id);
                        actualizar = "";


                        actualizar = "`stock_expedicion` = '" + pedido.Rows[fila]["nuevo_stock"].ToString() + "'";
                        id = pedido.Rows[fila]["id"].ToString();
                        consultas.actualizar_tabla(base_de_datos, proveedor_seleccionado, actualizar, id);
                        actualizar = "";
                    }
                }
            }


        }
        public void actualizar_stock_nuevo(DataTable productos, string nombre_proveedor)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            string id = "";
            string actualizar = "";
            for (int fila = 0; fila <= productos.Rows.Count - 1; fila++)
            {
                if (productos.Rows[fila]["stock_nuevo"].ToString() != "N/A")
                {
                    actualizar = actualizar + "`stock` = '" + productos.Rows[fila]["stock_nuevo"].ToString() + "'";
                    id = productos.Rows[fila]["id"].ToString();
                    consultas.actualizar_tabla(base_de_datos, nombre_proveedor, actualizar, id);
                    actualizar = "";
                }

            }

        }
        public void actualizar_acuerdo_en_pedidos_no_cargados(string nuevo_acuerdo, string proveedor)
        {
            consultar_pedidos_no_cargados(proveedor);
            string id_pedido = "";
            for (int fila = 0; fila <= pedidos_no_cargados.Rows.Count - 1; fila++)
            {
                if (pedidos_no_cargados.Rows[fila]["tipo_de_acuerdo"].ToString() == "fabrica_a_local")
                {
                    id_pedido = pedidos_no_cargados.Rows[fila]["id"].ToString();
                    consultas.actualizar_tabla(base_de_datos, "pedidos", "`acuerdo_de_precios` = '" + nuevo_acuerdo + "'", id_pedido);

                }
            }
        }
        public void actualizar_estado_pedido(string id_pedido)
        {
            consultas.actualizar_tabla(base_de_datos, "pedidos", "`estado` = 'Pedido en proceso'", id_pedido);
        }
        public void actualizar_stock_fabrica(string proveedor_seleccionado, DataTable pedido)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            string id = "";
            string actualizar = "";
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                actualizar = actualizar + "`stock` = '" + pedido.Rows[fila]["nuevo_stock"].ToString() + "'";
                id = pedido.Rows[fila]["id"].ToString();
                consultas.actualizar_tabla(base_de_datos, proveedor_seleccionado, actualizar, id);
                actualizar = "";
            }


        }
        public void actualizar_pedido(string id_pedido, DataTable pedido, string proveedor, string impuesto)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            int columna = 1;
            string dato = "";
            string actualizar = "`aumento` = '" + impuesto + "'";
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
            actualizar = "`estado` = 'Listo para despachar',";
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                if (pedido.Rows[fila]["proveedor"].ToString() == proveedor && pedido.Rows[fila]["id_pedido"].ToString() == id_pedido)
                {

                    if (proveedor == "insumos_fabrica")
                    {
                        if (pedido.Rows[fila]["estado"].ToString() == "Carga parcial") //
                        {
                            dato = pedido.Rows[fila]["pedido_dato_parcial"].ToString() + "-" + pedido.Rows[fila]["cantidad_entrega"].ToString() + "-" + pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString() + "-" + pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();//
                        }
                        else
                        {
                            dato = pedido.Rows[fila]["pedido_dato_parcial"].ToString() + "-" + pedido.Rows[fila]["cantidad_entrega"].ToString() + "-" + pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString() + "-" + pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();//
                        }
                    }
                    else
                    {
                        string pedido_dato_parcial = funciones.obtener_dato(pedido.Rows[fila]["pedido_dato_parcial"].ToString(), 1) + "-";
                        pedido_dato_parcial += funciones.obtener_dato(pedido.Rows[fila]["pedido_dato_parcial"].ToString(), 2) + "-";
                        pedido_dato_parcial += funciones.obtener_dato(pedido.Rows[fila]["pedido_dato_parcial"].ToString(), 3) + "-";
                        pedido_dato_parcial += funciones.obtener_dato(pedido.Rows[fila]["pedido_dato_parcial"].ToString(), 4);
                        string presentacion_entrega_seleccionada = funciones.obtener_dato(pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString(),1);
                        string presentacion_extraccion_seleccionada = funciones.obtener_dato(pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString(),1);
                        dato = pedido_dato_parcial + "-" + pedido.Rows[fila]["cantidad_entrega"].ToString() + "-" + presentacion_entrega_seleccionada + "-" + presentacion_extraccion_seleccionada + "-" + pedido.Rows[fila]["cantidad_pincho"].ToString();
                    }

                    if (fila == pedido.Rows.Count - 1)
                    {
                        actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "'";
                    }
                    else
                    {
                        if (pedido.Rows[fila + 1]["proveedor"].ToString() == proveedor && pedido.Rows[fila + 1]["id_pedido"].ToString() == id_pedido)
                        {
                            actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "',";

                        }
                        else
                        {
                            actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "'";
                        }
                    }
                    columna = columna + 1;
                }
            }
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
        }
        public void actualizar_pedido_carga_parcial(string id_pedido, DataTable pedido, string proveedor)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            int columna = 1;
            string dato = "";
            string actualizar = "`estado` = 'Carga parcial',";
            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                if (pedido.Rows[fila]["proveedor"].ToString() == proveedor && pedido.Rows[fila]["id_pedido"].ToString() == id_pedido)
                {

                    dato = pedido.Rows[fila]["pedido_dato_parcial"].ToString() + "-" + pedido.Rows[fila]["cantidad_entrega"].ToString() + "-" + pedido.Rows[fila]["presentacion_entrega_seleccionada"].ToString() + "-" + pedido.Rows[fila]["presentacion_extraccion_seleccionada"].ToString();


                    if (fila == pedido.Rows.Count - 1)
                    {
                        actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "'";
                    }
                    else
                    {
                        if (pedido.Rows[fila + 1]["proveedor"].ToString() == proveedor && pedido.Rows[fila + 1]["id_pedido"].ToString() == id_pedido)
                        {
                            actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "',";

                        }
                        else
                        {
                            actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "'";
                        }
                    }
                    columna = columna + 1;
                }
            }
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
        }
        public void actualizar_pedido_fabrica(string id_pedido, DataTable pedido)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            int columna = 1;
            string dato = "";
            string actualizar = "";
            for (int fila = 0; fila < pedido.Rows.Count - 1; fila++)
            {
                dato = pedido.Rows[fila]["pedido_dato"].ToString() + "-" + pedido.Rows[fila]["cantidad_entrega"].ToString() + "-" + pedido.Rows[fila]["kilos"].ToString();
                actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "',";
                columna = columna + 1;
            }
            int fil = pedido.Rows.Count - 1;
            dato = pedido.Rows[fil]["pedido_dato"].ToString() + "-" + pedido.Rows[fil]["cantidad_entrega"].ToString() + "-" + pedido.Rows[fil]["kilos"].ToString();
            actualizar = actualizar + "`producto_" + columna.ToString() + "` = '" + dato + "'";
            consultas.actualizar_tabla(base_de_datos, "pedidos_fabrica_a_proveedor", actualizar, id_pedido);
        }
        public void cargar_venta_en_rendiciones(DataTable productosBD, DataTable pedidos_sucursal, int fila_pedido, string proveedor)
        {
            string columnas = "", valores = "";
            string acuerdo_de_precios = pedidos_sucursal.Rows[fila_pedido]["acuerdo_de_precios"].ToString();
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for (int fila = 0; fila <= productosBD.Rows.Count - 1; fila++)
            {
                if (productosBD.Rows[fila]["nuevo_stock"].ToString() != "N/A" &&
                    productosBD.Rows[fila]["proveedor"].ToString() != proveedor)
                {
                    //id_productto
                    columnas = armar_query_columna(columnas, "id_producto", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["id"].ToString(), false);
                    //detalle
                    columnas = armar_query_columna(columnas, "detalle", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["producto"].ToString(), false);
                    //unidad_de_medida
                    columnas = armar_query_columna(columnas, "unidad_de_medida", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["unidad_entrega"].ToString(), false);
                    //concepto
                    columnas = armar_query_columna(columnas, "concepto", false);
                    valores = armar_query_valores(valores, "Venta", false);
                    //valor

                    columnas = armar_query_columna(columnas, "valor", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["sub_total"].ToString(), false);
                    //stock_inicial
                    columnas = armar_query_columna(columnas, "stock_inicial", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["stock"].ToString(), false);
                    //stock_final
                    columnas = armar_query_columna(columnas, "stock_final", false);
                    valores = armar_query_valores(valores, productosBD.Rows[fila]["nuevo_stock"].ToString(), false);
                    //proveedor
                    columnas = armar_query_columna(columnas, "proveedor", false);
                    valores = armar_query_valores(valores, pedidos_sucursal.Rows[fila_pedido]["proveedor"].ToString(), false);


                    //fecha
                    columnas = armar_query_columna(columnas, "fecha", false);
                    valores = armar_query_valores(valores, fecha, false);

                    //acuerdo_de_precio
                    columnas = armar_query_columna(columnas, "acuerdo_de_precio", true);
                    valores = armar_query_valores(valores, acuerdo_de_precios, true);

                    consultas.insertar_en_tabla(base_de_datos, "rendiciones", columnas, valores);

                    columnas = "";
                    valores = "";
                }

            }


        }
        public void enviar_remito_fabrica(string id_pedido, string sucursal, string num_pedido, string nombre_remito, string valor_remito, string fecha_remito, string proveedor, string nota, string impuesto)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            string columnas = "";
            string valores = "";

            //nota
            columnas = armar_query_columna(columnas, "nota", false);
            valores = armar_query_valores(valores, nota, false);
            //sucursal
            columnas = armar_query_columna(columnas, "sucursal", false);
            valores = armar_query_valores(valores, sucursal, false);
            //num_pedido
            columnas = armar_query_columna(columnas, "num_pedido", false);
            valores = armar_query_valores(valores, num_pedido, false);
            //nombre_remito
            columnas = armar_query_columna(columnas, "nombre_remito", false);
            valores = armar_query_valores(valores, nombre_remito, false);
            //valor_remito
            columnas = armar_query_columna(columnas, "valor_remito", false);
            valores = armar_query_valores(valores, valor_remito, false);
            //aumento
            columnas = armar_query_columna(columnas, "aumento", false);
            valores = armar_query_valores(valores, impuesto, false);
            //fecha_remito
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columnas = armar_query_columna(columnas, "fecha_remito", false);
            valores = armar_query_valores(valores, fecha, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", true);
            valores = armar_query_valores(valores, proveedor, true);

            consultas.insertar_en_tabla(base_de_datos, "cuenta_por_pagar", columnas, valores);
            string actualizar = "`estado` = 'Listo para despachar', `calculado_proveedor` = 'si'";
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);

        }
        public void enviar_remito_proveedor_a_fabrica(string id_pedido, string fabrica, string num_pedido, string nombre_remito, string valor_remito, string fecha_remito, string proveedor)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
            string columnas = "";
            string valores = "";

            //activa
            columnas = armar_query_columna(columnas, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //sucursal
            columnas = armar_query_columna(columnas, "fabrica", false);
            valores = armar_query_valores(valores, fabrica, false);
            //num_pedido
            columnas = armar_query_columna(columnas, "num_pedido", false);
            valores = armar_query_valores(valores, num_pedido, false);
            //nombre_remito
            columnas = armar_query_columna(columnas, "nombre_remito", false);
            valores = armar_query_valores(valores, nombre_remito, false);
            //valor_remito
            columnas = armar_query_columna(columnas, "valor_remito", false);
            valores = armar_query_valores(valores, valor_remito, false);
            //fecha_remito
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columnas = armar_query_columna(columnas, "fecha_remito", false);
            valores = armar_query_valores(valores, fecha, false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", true);
            valores = armar_query_valores(valores, proveedor, true);

            consultas.insertar_en_tabla(base_de_datos, "remitos_proveedores_a_fabrica", columnas, valores);
            string actualizar = "`estado` = 'cargado', `calculado_fabrica` = 'si'";
            consultas.actualizar_tabla(base_de_datos, "pedidos_fabrica_a_proveedor", actualizar, id_pedido);

        }
        public bool enviar_pedido(DataTable pedido_cargado, DataTable bonificados_cargados, DataTable proveedorBD)
        {
            cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);

            string columnas = "";
            string valores = "";
            string precio, id, producto, cantidad, valor_final;
            string fecha;
            int num_pedido = int.Parse(proveedorBD.Rows[0]["ultimo_pedido_enviado"].ToString()) + 1;
            string id_usuario = proveedorBD.Rows[0]["id"].ToString();
            string actualizar = "`ultimo_pedido_enviado` = '" + num_pedido + "'";
            consultas_Mysql.actualizar_tabla("shami", "lista_proveedores", actualizar, id_usuario);
            int producto_index;
            bool retorno = false;

            //activa
            columnas = armar_query_columna(columnas, "activa", false);
            valores = armar_query_valores(valores, "1", false);
            //fecha
            columnas = armar_query_columna(columnas, "fecha", false);
            fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            valores = armar_query_valores(valores, fecha, false);
            //sucursal
            columnas = armar_query_columna(columnas, "fabrica", false);
            valores = armar_query_valores(valores, proveedorBD.Rows[0]["nombre_proveedor"].ToString(), false);
            //num_pedido
            columnas = armar_query_columna(columnas, "num_pedido", false);
            valores = armar_query_valores(valores, num_pedido.ToString(), false);
            //proveedor
            columnas = armar_query_columna(columnas, "proveedor", false);
            valores = armar_query_valores(valores, proveedor_seleccionado, false);
            //acuerdo_de_precios
            columnas = armar_query_columna(columnas, "acuerdo_de_precios", false);
            valores = armar_query_valores(valores, acuerdo_de_precio, false);
            //estado
            columnas = armar_query_columna(columnas, "estado", false);
            valores = armar_query_valores(valores, "local", false);
            //tipo_de_acuerdo
            columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
            valores = armar_query_valores(valores, obtener_tipo_acuerdo_proveedor_a_fabrica(proveedor_seleccionado), false);
            //calculado_local
            columnas = armar_query_columna(columnas, "calculado_fabrica", false);
            valores = armar_query_valores(valores, "no", false);
            //calculado_proveedor
            columnas = armar_query_columna(columnas, "calculado_proveedor", false);
            valores = armar_query_valores(valores, "no", false);
            //tipo_de_acuerdo_fabrica
            columnas = armar_query_columna(columnas, "tipo_de_acuerdo_fabrica", false);
            valores = armar_query_valores(valores, obtener_tipo_acuerdo_proveedor_a_fabrica(proveedor_seleccionado), false);
            //mensaje
            //inicio
            columnas = armar_query_columna(columnas, "inicio", false);
            valores = armar_query_valores(valores, "inicio", false);
            //USAR FUNCIONES COMO OBTENER ACUERDO DE PRECIO PARA  TENER INFO EN TIEMPO REAL Y NO RECIBIRLA POR PARAMETRO.

            producto_index = 1;
            for (int fila = 0; fila <= pedido_cargado.Rows.Count - 1; fila++)
            {
                precio = pedido_cargado.Rows[fila]["precio"].ToString();
                id = pedido_cargado.Rows[fila]["id"].ToString();
                producto = pedido_cargado.Rows[fila]["producto"].ToString();
                cantidad = pedido_cargado.Rows[fila]["cantidad"].ToString();
                valor_final = precio + "-" + id + "-" + producto + "-" + cantidad;

                if (fila == pedido_cargado.Rows.Count - 1 && bonificados_cargados.Rows.Count == 0)
                {
                    columnas = armar_query_columna(columnas, "producto_" + producto_index, true);
                    valores = armar_query_valores(valores, valor_final, true);
                }
                else
                {
                    columnas = armar_query_columna(columnas, "producto_" + producto_index, false);
                    valores = armar_query_valores(valores, valor_final, false);
                }
                producto_index = producto_index + 1;
            }

            for (int fila = 0; fila <= bonificados_cargados.Rows.Count - 1; fila++)
            {

                if (bonificados_cargados.Rows[fila]["tipo_bonificado"].ToString() == "BONIFICADO")
                {
                    precio = "BONIFICADO";
                }
                else
                {
                    precio = "BONIFICADO ESPECIAL";
                }

                id = bonificados_cargados.Rows[fila]["id"].ToString();
                producto = bonificados_cargados.Rows[fila]["producto"].ToString();
                cantidad = bonificados_cargados.Rows[fila]["cantidad"].ToString();
                valor_final = precio + "-" + id + "-" + producto + "-" + cantidad;

                if (fila == bonificados_cargados.Rows.Count - 1)
                {
                    columnas = armar_query_columna(columnas, "producto_" + producto_index, true);
                    valores = armar_query_valores(valores, valor_final, true);
                }
                else
                {
                    columnas = armar_query_columna(columnas, "producto_" + producto_index, false);
                    valores = armar_query_valores(valores, valor_final, false);
                }
                producto_index = producto_index + 1;
            }

            retorno = consultas.insertar_en_tabla(base_de_datos, "pedidos_fabrica_a_proveedor", columnas, valores);
            return retorno;
        }
        public void desactivar_acuerdo_activo(string id)
        {
            consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios", "`activa` = '0'", id);
        }
        public void crear_nuevo_acuerdo_de_precio(DataTable productos, string proveedor, string acuerdo, string tipo_de_acuerdo)
        {
            string columna = "";
            string valor = "";

            columna = armar_query_columna(columna, "activa", false);
            valor = armar_query_valores(valor, "1", false);

            columna = armar_query_columna(columna, "proveedor", false);
            valor = armar_query_valores(valor, proveedor, false);

            columna = armar_query_columna(columna, "acuerdo", false);
            valor = armar_query_valores(valor, acuerdo, false);

            columna = armar_query_columna(columna, "tipo_de_acuerdo", false);
            valor = armar_query_valores(valor, tipo_de_acuerdo, false);

            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            columna = armar_query_columna(columna, "fecha", false);
            valor = armar_query_valores(valor, fecha, false);
            string id;
            for (int fila = 0; fila < productos.Rows.Count - 1; fila++)
            {
                id = productos.Rows[fila]["id"].ToString();
                columna = armar_query_columna(columna, "producto_" + id, false);

                if (productos.Rows[fila]["precio_nuevo"].ToString() == "0")
                {
                    valor = armar_query_valores(valor, productos.Rows[fila]["precio"].ToString(), false);
                }
                else
                {
                    valor = armar_query_valores(valor, productos.Rows[fila]["precio_nuevo"].ToString(), false);
                }
            }
            int ultima_fila = productos.Rows.Count - 1;
            id = productos.Rows[ultima_fila]["id"].ToString();
            columna = armar_query_columna(columna, "producto_" + id, true);
            if (productos.Rows[ultima_fila]["precio_nuevo"].ToString() == "0")
            {
                valor = armar_query_valores(valor, productos.Rows[ultima_fila]["precio"].ToString(), true);
            }
            else
            {
                valor = armar_query_valores(valor, productos.Rows[ultima_fila]["precio_nuevo"].ToString(), true);
            }
            consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columna, valor);
        }
        public void actualizar_precio_proveedor(int fila_acuerdo, DataTable acuerdo_de_precios, DataTable pedido, DataTable pedidos_fabrica)
        {
            if (verificar_si_actualizar_precio(fila_acuerdo, acuerdo_de_precios, pedido))
            {
                cls_consultas_Mysql consultas_Mysql = new cls_consultas_Mysql(servidor, puerto, usuario, password, base_de_datos);
                string columnas = "";
                string valores = "";
                int num_acuerdo_actual, num_acuerdo_nuevo;
                string proveedor = acuerdo_de_precios.Rows[fila_acuerdo]["proveedor"].ToString();
                string tipo_de_acuerdo = acuerdo_de_precios.Rows[fila_acuerdo]["tipo_de_acuerdo"].ToString();
                num_acuerdo_actual = int.Parse(acuerdo_de_precios.Rows[fila_acuerdo]["acuerdo"].ToString());
                num_acuerdo_nuevo = num_acuerdo_actual + 1;
                //activa
                columnas = armar_query_columna(columnas, "activa", false);
                valores = armar_query_valores(valores, "1", false);
                //proveedor
                columnas = armar_query_columna(columnas, "proveedor", false);
                valores = armar_query_valores(valores, proveedor, false);
                //acuerdo
                columnas = armar_query_columna(columnas, "acuerdo", false);
                valores = armar_query_valores(valores, num_acuerdo_nuevo.ToString(), false);
                //tipo_de_acuerdo
                columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
                valores = armar_query_valores(valores, tipo_de_acuerdo, false);
                //condicion_bonificado
                columnas = armar_query_columna(columnas, "condicion_bonificado", false);
                valores = armar_query_valores(valores, acuerdo_de_precios.Rows[fila_acuerdo]["condicion_bonificado"].ToString(), false);
                //inicio
                columnas = armar_query_columna(columnas, "inicio", false);
                valores = armar_query_valores(valores, "inicio", false);

                int id, fila_producto;
                int columna = acuerdo_de_precios.Columns["producto_1"].Ordinal;
                while (columna <= acuerdo_de_precios.Columns.Count - 2)
                {
                    id = columna - 11;
                    if (IsNotDBNull(acuerdo_de_precios.Rows[fila_acuerdo][columna]))
                    {
                        fila_producto = buscar_fila_producto(pedido, id.ToString());
                        if (fila_producto != -1)
                        {
                            if (pedido.Rows[fila_producto]["precio"].ToString() != acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString())
                            {
                                columnas = armar_query_columna(columnas, "producto_" + id.ToString(), false);
                                valores = armar_query_valores(valores, pedido.Rows[fila_producto]["precio"].ToString(), false);
                            }
                            else
                            {
                                columnas = armar_query_columna(columnas, "producto_" + id.ToString(), false);
                                valores = armar_query_valores(valores, acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString(), false);
                            }
                        }
                        else
                        {
                            columnas = armar_query_columna(columnas, "producto_" + id.ToString(), false);
                            valores = armar_query_valores(valores, acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString(), false);
                        }
                    }
                    else
                    {
                        columnas = armar_query_columna(columnas, "producto_" + id.ToString(), false);
                        valores = armar_query_valores(valores, "0", false);
                    }
                    columna++;
                }

                columna = acuerdo_de_precios.Columns.Count - 1;
                id = columna - 11;
                if (IsNotDBNull(acuerdo_de_precios.Rows[fila_acuerdo][columna]))
                {
                    fila_producto = buscar_fila_producto(pedido, id.ToString());
                    if (fila_producto != -1)
                    {
                        if (pedido.Rows[fila_producto]["precio"].ToString() != acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString())
                        {
                            columnas = armar_query_columna(columnas, "producto_" + id.ToString(), true);
                            valores = armar_query_valores(valores, pedido.Rows[fila_producto]["precio"].ToString(), true);
                        }
                        else
                        {
                            columnas = armar_query_columna(columnas, "producto_" + id.ToString(), true);
                            valores = armar_query_valores(valores, acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString(), true);
                        }
                    }
                    else
                    {
                        columnas = armar_query_columna(columnas, "producto_" + id.ToString(), true);
                        valores = armar_query_valores(valores, acuerdo_de_precios.Rows[fila_acuerdo][columna].ToString(), true);
                    }
                }
                else
                {
                    columnas = armar_query_columna(columnas, "producto_" + id.ToString(), true);
                    valores = armar_query_valores(valores, "0", true);
                }

                consultas.insertar_en_tabla(base_de_datos, "acuerdo_de_precios", columnas, valores);



                string id_acuerdo = acuerdo_de_precios.Rows[fila_acuerdo]["id"].ToString();
                string actualizar = "`activa` = '0'";
                consultas.actualizar_tabla(base_de_datos, "acuerdo_de_precios", actualizar, id_acuerdo);
                string id_pedido;
                for (int fila = 0; fila <= pedidos_fabrica.Rows.Count - 1; fila++)
                {
                    if (pedidos_fabrica.Rows[fila]["acuerdo_de_precios"].ToString() == num_acuerdo_actual.ToString() &&
                        pedidos_fabrica.Rows[fila]["proveedor"].ToString() == proveedor &&
                        pedidos_fabrica.Rows[fila]["tipo_de_acuerdo"].ToString() == tipo_de_acuerdo &&
                        pedidos_fabrica.Rows[fila]["estado"].ToString() == "local")
                    {
                        id_pedido = pedidos_fabrica.Rows[fila]["id"].ToString();
                        actualizar = "`acuerdo_de_precios` = '" + num_acuerdo_nuevo.ToString() + "' ";
                        consultas.actualizar_tabla(base_de_datos, "pedidos_fabrica_a_proveedor", actualizar, id_pedido);
                        actualizar = "";
                    }
                }
            }
        }
        public void actualizar_stock_insumo(string dato, string conulma, string id_insumo)
        {
            string actualizar = "`" + conulma + "` = '" + dato + "' ";
            consultas.actualizar_tabla(base_de_datos, "insumos_fabrica", actualizar, id_insumo);
        }

        private int buscar_fila_producto(DataTable pedido, string id)
        {
            int retorno = -1;
            int fila_producto = 0;
            while (fila_producto <= pedido.Rows.Count - 1)
            {
                if (pedido.Rows[fila_producto]["id"].ToString() == id)
                {
                    retorno = fila_producto;
                    break;
                }
                fila_producto++;
            }
            return retorno;
        }
        private bool verificar_si_actualizar_precio(int fila_acuerdo, DataTable acuerdo_de_precios, DataTable pedido)
        {
            bool retorno = false;
            string id;
            int fila = 0;
            while (fila <= pedido.Rows.Count - 1)
            {
                id = pedido.Rows[fila]["id"].ToString();
                if (pedido.Rows[fila]["precio"].ToString() != acuerdo_de_precios.Rows[fila_acuerdo]["producto_" + id].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }

            return retorno;
        }

        private string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
        {
            string retorno = "";
            string separador_columna = "`";

            retorno = columnas + separador_columna + columna_valor + separador_columna;

            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        private string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
        {
            string retorno = "";
            string separador_valores = "'";

            retorno = valores + separador_valores + valor_a_insertar + separador_valores;
            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        private bool IsNotDBNull(object valor)
        {
            bool retorno = false;
            if (!DBNull.Value.Equals(valor))
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion

        #region cancelar pedido
        public void cancelar_pedido(string id_pedido)
        {
            string actualizar = "`estado` = 'Cancelado'";
            consultas.actualizar_tabla(base_de_datos, "pedidos", actualizar, id_pedido);
        }
        #endregion
    }
}
