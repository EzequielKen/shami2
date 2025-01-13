using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using _01___modulos;
using acceso_a_base_de_datos;
using paginaWeb;
namespace modulos
{
    public class cls_pedidos
    {
        public cls_pedidos(DataTable usuarios_BD, DataTable sucursal_BD)
        {
            usuariosBD = usuarios_BD;
            sucursalBD = sucursal_BD;
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

            sucursal = sucursalBD.Rows[0]["sucursal"].ToString();
            crear_pedidos_y_bonificado();

        }
        #region atributos
        private cls_funciones funciones = new cls_funciones();
        private cls_consultas_Mysql consultas;
        private cls_PDF PDF = new cls_PDF();
        private string servidor;
        private string puerto;
        private string usuario;
        private string password;
        private string base_de_datos;


        private string sucursal;
        private DataTable usuariosBD;
        private DataTable sucursalBD;
        private DataTable sucursales;
        private DataTable pedidos;
        private DataTable bonificados;

        private DataTable lista_proveedores;
        private DataTable productos_proveedor;
        private DataTable tipo_acuerdo;
        private DataTable tipo_acuerdo_fabrica;
        private DataTable acuerdo_de_precios;

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
        #endregion region



        #region metodos privados de consulta
        private void consultar_lista_proveedores()
        {
            lista_proveedores = consultas.consultar_tabla(base_de_datos, "lista_proveedores");
        }

        private void consultar_productos_proveedor(string proveedor_seleccionado, string id_sucursal)
        {
            if (proveedor_seleccionado == "insumos_fabrica")
            {
                if (id_sucursal == "17" ||
                    id_sucursal == "18" ||
                    id_sucursal == "22")
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_venta_vip(base_de_datos, proveedor_seleccionado);
                }
                else if (id_sucursal == "10")
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_venta_vip2(base_de_datos, "insumos_fabrica");

                }
                else if (id_sucursal == "31")
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_fatay(base_de_datos, "insumos_fabrica");

                }
                else if (id_sucursal == "28" ||
                         id_sucursal == "19")
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_venta_nivel4(base_de_datos, "insumos_fabrica");

                }
                else if (id_sucursal == "2")
                {
                    productos_proveedor = consultas.consultar_insumos_caballito(base_de_datos, "insumos_fabrica");

                }
                else
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_venta(base_de_datos, proveedor_seleccionado);
                }

            }
            else
            {
                if (id_sucursal == "31")
                {
                    productos_proveedor = consultas.consultar_insumos_fabrica_fatay(base_de_datos, proveedor_seleccionado);

                }
                else
                {
                    productos_proveedor = consultas.consultar_tabla(base_de_datos, proveedor_seleccionado);
                }

            }

        }
        private void consultar_insumos()
        {
            productos_proveedor = consultas.consultar_tabla(base_de_datos, "insumos_fabrica");
        }
        private void consultar_productos_proveedor_sin_insumos(string proveedor_seleccionado)
        {
            productos_proveedor = consultas.consultar_productos_proveedor_sin_insumos(base_de_datos, proveedor_seleccionado);
        }
        private void consultar_tipo_acuerdo()
        {
            tipo_acuerdo = consultas.consultar_tabla(base_de_datos, "tipo_acuerdo");
          //  tipo_acuerdo_fabrica = consultas.consultar_tabla(base_de_datos, "tipo_acuerdo_fabrica_a_marca");
        }
        private void consultar_acuerdo_de_precios()
        {
            acuerdo_de_precios = consultas.consultar_tabla(base_de_datos, "acuerdo_de_precios");
        }
        private void consultar_sucursales()
        {
            sucursales = consultas.consultar_tabla(base_de_datos, "sucursal");
        }
        #endregion

        #region metodos get/set
        public DataTable get_sucursales()
        {
            consultar_sucursales();
            return sucursales;
        }
        public DataTable get_lista_proveedores()
        {
            consultar_lista_proveedores();
            return lista_proveedores;
        }

        public DataTable get_productos_proveedor(string proveedor, string id_sucursal)
        {
            int fila_acuerdo_de_precios;

            consultar_lista_proveedores();
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor);
            consultar_productos_proveedor(proveedor_seleccionado, id_sucursal);

            consultar_tipo_acuerdo();
            consultar_acuerdo_de_precios();

            tipo_de_acuerdo = obtener_tipo_de_acuerdo(proveedor_seleccionado);
           // tipo_de_acuerdo_fabrica = obtener_tipo_de_acuerdo_fabrica(proveedor_seleccionado);
            fila_acuerdo_de_precios = obtener_fila_de_acuerdo(tipo_de_acuerdo, proveedor_seleccionado);


            acuerdo_de_precio = acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["acuerdo"].ToString();
            actualizar_precio_productos(fila_acuerdo_de_precios);
            return productos_proveedor;
        }
        public DataTable get_productos_proveedor_sin_insumos(string proveedor)
        {

            int fila_acuerdo_de_precios;
            consultar_lista_proveedores();
            proveedor_seleccionado = obtener_nombre_proveedor(proveedor);
            consultar_productos_proveedor_sin_insumos(proveedor_seleccionado);
            consultar_tipo_acuerdo();
            consultar_acuerdo_de_precios();

            tipo_de_acuerdo = obtener_tipo_de_acuerdo(proveedor_seleccionado);
            tipo_de_acuerdo_fabrica = obtener_tipo_de_acuerdo_fabrica(proveedor_seleccionado);
            fila_acuerdo_de_precios = obtener_fila_de_acuerdo(tipo_de_acuerdo, proveedor_seleccionado);


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
                    productos_proveedor.Rows[fila]["precio"] = float.Parse(acuerdo_de_precios.Rows[fila_acuerdo_de_precios]["producto_" + id_producto].ToString());
                }
                else
                {
                    productos_proveedor.Rows[fila]["precio"] = 0;
                }
            }

            //setear_conficion_bonificado(fila_acuerdo_de_precios);
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
        private DataTable crear_dataTable_resumen(DataTable pedido_cargado, string proveedor)
        {
            DataTable retorno = new DataTable();
            retorno.Columns.Add("id", typeof(string));
            retorno.Columns.Add("producto", typeof(string));
            retorno.Columns.Add("cantidad", typeof(string));
            retorno.Columns.Add("unidad de medida", typeof(string));
            retorno.Columns.Add("precio", typeof(string));
            retorno.Columns.Add("multiplicador", typeof(string));
            retorno.Columns.Add("tipo_producto", typeof(string));
            retorno.Columns.Add("proveedor", typeof(string));
            int fila_retorno = 0;
            for (int fila = 0; fila <= pedido_cargado.Rows.Count - 1; fila++)
            {
                if (pedido_cargado.Rows[fila]["proveedor"].ToString() == proveedor)
                {
                    retorno.Rows.Add();
                    retorno.Rows[fila_retorno]["id"] = pedido_cargado.Rows[fila]["id"].ToString();
                    retorno.Rows[fila_retorno]["producto"] = pedido_cargado.Rows[fila]["producto"].ToString();
                    retorno.Rows[fila_retorno]["cantidad"] = pedido_cargado.Rows[fila]["cantidad"].ToString();
                    retorno.Rows[fila_retorno]["unidad de medida"] = pedido_cargado.Rows[fila]["unidad de medida"].ToString();
                    retorno.Rows[fila_retorno]["precio"] = pedido_cargado.Rows[fila]["precio"].ToString();
                    retorno.Rows[fila_retorno]["multiplicador"] = pedido_cargado.Rows[fila]["multiplicador"].ToString();
                    retorno.Rows[fila_retorno]["tipo_producto"] = pedido_cargado.Rows[fila]["tipo_producto"].ToString();
                    retorno.Rows[fila_retorno]["proveedor"] = pedido_cargado.Rows[fila]["proveedor"].ToString();
                    fila_retorno++;
                }

            }

            return retorno;
        }
        public string enviar_pedido_automatico(DataTable pedido_cargado, string nota)
        {
            int num_pedido = obtener_ultimo_num_pedido_enviado(sucursalBD) + 1;
            envio_automatico_de_pedido(pedido_cargado, nota);
            return num_pedido.ToString();
        }
        public void enviar_pedido(DataTable pedido_cargado, string nota)
        {
            pedido_cargado.DefaultView.Sort = "proveedor ASC";
            pedido_cargado = pedido_cargado.DefaultView.ToTable();
            string columnas = "";
            string valores = "";
            string precio, id, producto, cantidad, valor_final;
            //            List<string> proveedores = obtener_lista_cantidad_de_proveedores(pedido_cargado);
            //          string proveedor_seleccionado = proveedores[0].ToString();
            string proveedor_seleccionado;
            int num_pedido;
            string id_usuario;
            string actualizar;
            int producto_index;
            DataTable proveedor_villaMaipu = crear_dataTable_resumen(pedido_cargado, "proveedor_villaMaipu");
            if (proveedor_villaMaipu.Rows.Count > 0)
            {
                proveedor_seleccionado = "proveedor_villaMaipu";
                columnas = "";
                valores = "";
                //setear acuerdos de precios
                setear_acuerdos_de_precios("proveedor_villaMaipu");
                num_pedido = obtener_ultimo_num_pedido_enviado(sucursalBD) + 1;
                id_usuario = sucursalBD.Rows[0]["id"].ToString();
                actualizar = "`ultimo_pedido_enviado` = '" + num_pedido + "'";
                consultas.actualizar_tabla("shami", "sucursal", actualizar, id_usuario);

                //activa
                columnas = armar_query_columna(columnas, "nota", false);
                valores = armar_query_valores(valores, nota, false);
                //fecha
                columnas = armar_query_columna(columnas, "fecha", false);
                valores = armar_query_valores(valores, funciones.get_fecha(), false);
                //sucursal
                columnas = armar_query_columna(columnas, "sucursal", false);
                valores = armar_query_valores(valores, sucursalBD.Rows[0]["sucursal"].ToString(), false);
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
                valores = armar_query_valores(valores, tipo_de_acuerdo, false);
                //calculado_local
                columnas = armar_query_columna(columnas, "calculado_local", false);
                valores = armar_query_valores(valores, "no", false);
                //calculado_proveedor
                columnas = armar_query_columna(columnas, "calculado_proveedor", false);
                valores = armar_query_valores(valores, "no", false);
                //tipo_de_acuerdo_fabrica
               //columnas = armar_query_columna(columnas, "tipo_de_acuerdo_fabrica", false);
               //valores = armar_query_valores(valores, tipo_de_acuerdo_fabrica, false);
                //mensaje
                //inicio
                columnas = armar_query_columna(columnas, "inicio", false);
                valores = armar_query_valores(valores, "inicio", false);
                producto_index = 1;
                for (int fila_resumen = 0; fila_resumen <= proveedor_villaMaipu.Rows.Count - 1; fila_resumen++)
                {
                    precio = proveedor_villaMaipu.Rows[fila_resumen]["precio"].ToString();
                    id = proveedor_villaMaipu.Rows[fila_resumen]["id"].ToString();
                    producto = proveedor_villaMaipu.Rows[fila_resumen]["producto"].ToString();
                    cantidad = proveedor_villaMaipu.Rows[fila_resumen]["cantidad"].ToString().Replace(",", ".");
                    valor_final = precio + "-" + id + "-" + producto + "-" + cantidad + "-N/A";

                    if (fila_resumen == proveedor_villaMaipu.Rows.Count - 1)
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
                consultas.insertar_en_tabla(base_de_datos, "pedidos", columnas, valores);
            }

            DataTable insumos_fabrica = crear_dataTable_resumen(pedido_cargado, "insumos_fabrica");
            if (insumos_fabrica.Rows.Count > 0)
            {
                proveedor_seleccionado = "insumos_fabrica";
                columnas = "";
                valores = "";
                //setear acuerdos de precios
                setear_acuerdos_de_precios("insumos_fabrica");
                num_pedido = obtener_ultimo_num_pedido_enviado(sucursalBD) + 1;
                id_usuario = sucursalBD.Rows[0]["id"].ToString();
                actualizar = "`ultimo_pedido_enviado` = '" + num_pedido + "'";
                consultas.actualizar_tabla("shami", "sucursal", actualizar, id_usuario);

                //activa
                columnas = armar_query_columna(columnas, "nota", false);
                valores = armar_query_valores(valores, nota, false);
                //fecha
                columnas = armar_query_columna(columnas, "fecha", false);
                valores = armar_query_valores(valores, funciones.get_fecha(), false);
                //sucursal
                columnas = armar_query_columna(columnas, "sucursal", false);
                valores = armar_query_valores(valores, sucursalBD.Rows[0]["sucursal"].ToString(), false);
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
                valores = armar_query_valores(valores, tipo_de_acuerdo, false);
                //calculado_local
                columnas = armar_query_columna(columnas, "calculado_local", false);
                valores = armar_query_valores(valores, "no", false);
                //calculado_proveedor
                columnas = armar_query_columna(columnas, "calculado_proveedor", false);
                valores = armar_query_valores(valores, "no", false);
                //tipo_de_acuerdo_fabrica
                columnas = armar_query_columna(columnas, "tipo_de_acuerdo_fabrica", false);
                valores = armar_query_valores(valores, tipo_de_acuerdo_fabrica, false);
                //mensaje
                //inicio
                columnas = armar_query_columna(columnas, "inicio", false);
                valores = armar_query_valores(valores, "inicio", false);
                producto_index = 1;
                for (int fila_resumen = 0; fila_resumen <= insumos_fabrica.Rows.Count - 1; fila_resumen++)
                {
                    precio = insumos_fabrica.Rows[fila_resumen]["precio"].ToString();
                    id = insumos_fabrica.Rows[fila_resumen]["id"].ToString();
                    producto = insumos_fabrica.Rows[fila_resumen]["producto"].ToString();
                    cantidad = insumos_fabrica.Rows[fila_resumen]["cantidad"].ToString().Replace(",", ".");
                    valor_final = precio + "-" + id + "-" + producto + "-" + cantidad + "-N/A";

                    if (fila_resumen == insumos_fabrica.Rows.Count - 1)
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
                consultas.insertar_en_tabla(base_de_datos, "pedidos", columnas, valores);
            }

        }
        private void envio_automatico_de_pedido(DataTable pedido_cargado, string nota)
        {
            pedido_cargado.DefaultView.Sort = "proveedor ASC";
            pedido_cargado = pedido_cargado.DefaultView.ToTable();
            string columnas = "";
            string valores = "";
            string precio, id, producto, cantidad, valor_final;
            //            List<string> proveedores = obtener_lista_cantidad_de_proveedores(pedido_cargado);
            //          string proveedor_seleccionado = proveedores[0].ToString();
            string proveedor_seleccionado;
            int num_pedido;
            string id_usuario;
            string actualizar;
            int producto_index;
            DataTable proveedor_villaMaipu = crear_dataTable_resumen(pedido_cargado, "proveedor_villaMaipu");
            if (proveedor_villaMaipu.Rows.Count > 0)
            {
                proveedor_seleccionado = "proveedor_villaMaipu";
                columnas = "";
                valores = "";
                //setear acuerdos de precios
                setear_acuerdos_de_precios("proveedor_villaMaipu");
                num_pedido = obtener_ultimo_num_pedido_enviado(sucursalBD) + 1;
                id_usuario = sucursalBD.Rows[0]["id"].ToString();
                actualizar = "`ultimo_pedido_enviado` = '" + num_pedido + "'";
                consultas.actualizar_tabla("shami", "sucursal", actualizar, id_usuario);

                //activa
                columnas = armar_query_columna(columnas, "nota", false);
                valores = armar_query_valores(valores, nota, false);
                //fecha
                columnas = armar_query_columna(columnas, "fecha", false);
                valores = armar_query_valores(valores, funciones.get_fecha(), false);
                //sucursal
                columnas = armar_query_columna(columnas, "sucursal", false);
                valores = armar_query_valores(valores, sucursalBD.Rows[0]["sucursal"].ToString(), false);
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
                valores = armar_query_valores(valores, "listo para despachar", false);
                //tipo_de_acuerdo
                columnas = armar_query_columna(columnas, "tipo_de_acuerdo", false);
                valores = armar_query_valores(valores, tipo_de_acuerdo, false);
                //calculado_local
                columnas = armar_query_columna(columnas, "calculado_local", false);
                valores = armar_query_valores(valores, "no", false);
                //calculado_proveedor
                columnas = armar_query_columna(columnas, "calculado_proveedor", false);
                valores = armar_query_valores(valores, "si", false);
                //tipo_de_acuerdo_fabrica
                columnas = armar_query_columna(columnas, "tipo_de_acuerdo_fabrica", false);
                valores = armar_query_valores(valores, tipo_de_acuerdo_fabrica, false);
                //mensaje
                //inicio
                columnas = armar_query_columna(columnas, "inicio", false);
                valores = armar_query_valores(valores, "inicio", false);
                producto_index = 1;
                for (int fila_resumen = 0; fila_resumen <= proveedor_villaMaipu.Rows.Count - 1; fila_resumen++)
                {
                    precio = proveedor_villaMaipu.Rows[fila_resumen]["precio"].ToString();
                    id = proveedor_villaMaipu.Rows[fila_resumen]["id"].ToString();
                    producto = proveedor_villaMaipu.Rows[fila_resumen]["producto"].ToString();
                    cantidad = proveedor_villaMaipu.Rows[fila_resumen]["cantidad"].ToString().Replace(",", ".");
                    string unidad_de_medida = proveedor_villaMaipu.Rows[fila_resumen]["unidad de medida"].ToString();
                    valor_final = precio + "-" + id + "-" + producto + "-" + cantidad + "-" + cantidad+"-"+ unidad_de_medida + "-" + unidad_de_medida + "-N/A";

                    if (fila_resumen == proveedor_villaMaipu.Rows.Count - 1)
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
                consultas.insertar_en_tabla(base_de_datos, "pedidos", columnas, valores);
            }

            DataTable insumos_fabrica = crear_dataTable_resumen(pedido_cargado, "insumos_fabrica");
            if (insumos_fabrica.Rows.Count > 0)
            {
                proveedor_seleccionado = "insumos_fabrica";
                columnas = "";
                valores = "";
                //setear acuerdos de precios
                setear_acuerdos_de_precios("insumos_fabrica");
                num_pedido = obtener_ultimo_num_pedido_enviado(sucursalBD) + 1;
                id_usuario = sucursalBD.Rows[0]["id"].ToString();
                actualizar = "`ultimo_pedido_enviado` = '" + num_pedido + "'";
                consultas.actualizar_tabla("shami", "sucursal", actualizar, id_usuario);

                //activa
                columnas = armar_query_columna(columnas, "activa", false);
                valores = armar_query_valores(valores, "1", false);
                //fecha
                columnas = armar_query_columna(columnas, "fecha", false);
                valores = armar_query_valores(valores, funciones.get_fecha(), false);
                //sucursal
                columnas = armar_query_columna(columnas, "sucursal", false);
                valores = armar_query_valores(valores, sucursalBD.Rows[0]["sucursal"].ToString(), false);
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
                valores = armar_query_valores(valores, tipo_de_acuerdo, false);
                //calculado_local
                columnas = armar_query_columna(columnas, "calculado_local", false);
                valores = armar_query_valores(valores, "no", false);
                //calculado_proveedor
                columnas = armar_query_columna(columnas, "calculado_proveedor", false);
                valores = armar_query_valores(valores, "no", false);
                //tipo_de_acuerdo_fabrica
                columnas = armar_query_columna(columnas, "tipo_de_acuerdo_fabrica", false);
                valores = armar_query_valores(valores, tipo_de_acuerdo_fabrica, false);
                //mensaje
                //inicio
                columnas = armar_query_columna(columnas, "inicio", false);
                valores = armar_query_valores(valores, "inicio", false);
                producto_index = 1;
                for (int fila_resumen = 0; fila_resumen <= insumos_fabrica.Rows.Count - 1; fila_resumen++)
                {
                    precio = insumos_fabrica.Rows[fila_resumen]["precio"].ToString();
                    id = insumos_fabrica.Rows[fila_resumen]["id"].ToString();
                    producto = insumos_fabrica.Rows[fila_resumen]["producto"].ToString();
                    cantidad = insumos_fabrica.Rows[fila_resumen]["cantidad"].ToString().Replace(",", ".");
                    valor_final = precio + "-" + id + "-" + producto + "-" + cantidad + "-N/A";

                    if (fila_resumen == insumos_fabrica.Rows.Count - 1)
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
                consultas.insertar_en_tabla(base_de_datos, "pedidos", columnas, valores);
            }

        }
        private void setear_acuerdos_de_precios(string proveedor)
        {
            consultar_acuerdo_de_precios();
            consultar_tipo_acuerdo();
            tipo_de_acuerdo = obtener_tipo_de_acuerdo(proveedor);
            int fila_acuerdo = obtener_fila_de_acuerdo(tipo_de_acuerdo, proveedor);
            acuerdo_de_precio = acuerdo_de_precios.Rows[fila_acuerdo]["acuerdo"].ToString();
           // tipo_de_acuerdo_fabrica = obtener_tipo_de_acuerdo_fabrica(proveedor);
        }
        private int obtener_ultimo_num_pedido_enviado(DataTable sucursalBD)
        {
            int num_pedido = -1;
            consultar_sucursales();
            string id_sucursal = sucursalBD.Rows[0]["id"].ToString();
            int fila_sucursal = funciones.buscar_fila_por_id(id_sucursal, sucursales);
            num_pedido = int.Parse(sucursales.Rows[fila_sucursal]["ultimo_pedido_enviado"].ToString());
            return num_pedido;
        }
        private List<string> obtener_lista_cantidad_de_proveedores(DataTable pedido_cargado)
        {
            List<string> lista = new List<string>();
            pedido_cargado.DefaultView.Sort = "proveedor ASC";
            pedido_cargado = pedido_cargado.DefaultView.ToTable();
            string proveedor_seleccionado = pedido_cargado.Rows[0]["proveedor"].ToString();
            lista.Add(proveedor_seleccionado);
            for (int fila = 0; fila <= pedido_cargado.Rows.Count - 1; fila++)
            {
                if (proveedor_seleccionado != pedido_cargado.Rows[fila]["proveedor"].ToString())
                {
                    proveedor_seleccionado = pedido_cargado.Rows[fila]["proveedor"].ToString();
                    lista.Add(proveedor_seleccionado);
                }
            }
            return lista;
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
        #endregion
    }
}
