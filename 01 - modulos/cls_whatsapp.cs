using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01___modulos
{
    [Serializable]
    public class cls_whatsapp
    {

        #region atributos
        private DataTable usuario;
        private DataTable sucursal;
        private DataTable proveedorBD;
        private DataTable lista_proveedores;
        private string proveedor_seleccionado;
        #endregion
        public string enviar_pedido(DataTable pedido, DataTable usuarioBD, DataTable sucursalBD)
        {
            usuario = usuarioBD;
            sucursal = sucursalBD;
            proveedorBD = sucursalBD;

            int num_pedido = int.Parse(sucursal.Rows[0]["ultimo_pedido_enviado"].ToString()) + 1;
            string salto_de_linea = "%0A"; //
            string body = "";

            body = "Usted recibio en sistema un pedido de: " + salto_de_linea;
            body += "FRANQUICIA: " + sucursal.Rows[0]["franquicia"].ToString() + salto_de_linea;
            body += "SUCURSAL: " + sucursal.Rows[0]["sucursal"].ToString() + salto_de_linea;
            //body += "PEDIDO N°: " + num_pedido.ToString() + salto_de_linea + salto_de_linea;

            //body += "resumen:" + salto_de_linea;
            //body += obtener_cantidad_productos(pedido, bonificados) + salto_de_linea;

            //body += "pedido:" + salto_de_linea;

            /*for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                body += pedido.Rows[fila]["producto"].ToString() + " |" + pedido.Rows[fila]["cantidad"].ToString() + " " + pedido.Rows[fila]["unidad de medida"].ToString() + "|" + salto_de_linea + salto_de_linea;
            }
            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
            {//agregar bonificado especial

                if (bonificados.Rows[fila]["tipo_bonificado"].ToString() == "BONIFICADO")
                {
                    body += bonificados.Rows[fila]["producto"].ToString() + " |" + bonificados.Rows[fila]["cantidad"].ToString() + " " + bonificados.Rows[fila]["unidad de medida"].ToString() + "| BONIFICADO" + salto_de_linea + salto_de_linea;
                }
                else
                {

                    body += bonificados.Rows[fila]["producto"].ToString() + " |" + bonificados.Rows[fila]["cantidad"].ToString() + " " + bonificados.Rows[fila]["unidad de medida"].ToString() + "| BONIFICADO ESPECIAL" + salto_de_linea + salto_de_linea;
                }

            }*/
            return enviar_pedido(body, sucursalBD);

        }

        public string enviar_pedido_proveedor(DataTable pedido, DataTable bonificados)
        {
            int num_pedido = int.Parse(proveedorBD.Rows[0]["ultimo_pedido_enviado"].ToString()) + 1;
            int fila_proveedor = obtener_fila_proveedor();
            string salto_de_linea = "%0A"; //
            string body = "";

            body = "Estimado " + lista_proveedores.Rows[fila_proveedor]["nombre_proveedor"].ToString() + salto_de_linea;
            body += proveedorBD.Rows[0]["nombre_proveedor"].ToString() + " realizo un pedido" + salto_de_linea;
            body += "PEDIDO: " + num_pedido.ToString() + salto_de_linea + salto_de_linea;

            body += "resumen:" + salto_de_linea;
            body += obtener_cantidad_productos(pedido, bonificados) + salto_de_linea;

            body += "pedido:" + salto_de_linea;

            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                body += pedido.Rows[fila]["producto"].ToString() + " |" + pedido.Rows[fila]["cantidad"].ToString() + " " + pedido.Rows[fila]["unidad de medida"].ToString() + "|" + salto_de_linea + salto_de_linea;
            }
            return enviar_pedido_proveedores(body);
        }
        public string cancelar_pedido(string num_pedido, DataTable lista_de_proveedores, DataTable sucursal)
        {
            lista_proveedores = lista_de_proveedores;
            string salto_de_linea = "%0A"; //
            string body = "";

            body = "FRANQUICIA: " + sucursal.Rows[0]["franquicia"].ToString() + salto_de_linea;
            body += "SUCURSAL: " + sucursal.Rows[0]["sucursal"].ToString() + salto_de_linea;
            body += "Cancelo el pedido N°: " + num_pedido + salto_de_linea;

            return enviar_pedido(body);
        }
        public string notificar_nueva_orden_de_compra()
        {
            string salto_de_linea = "%0A"; //
            string body = "";

            body = "Se ha registrado en sistema una nueva orden de compras." + salto_de_linea;

            return enviar_orden(body);

        }

        public string notificar_nueva_orden_de_pedido()
        {
            //string salto_de_linea = "%0A";
            string body = "";

            body = "Se ha registrado en sistema una nueva orden de pedido.";

            return enviar_orden(body);

        }

        public string notificar_nuevo_pedido_de_insumo()
        {
            // string salto_de_linea = "%0A"; 
            string body = "";

            body = "Se ha registrado en sistema un nuevo pedido de insumos.";

            return enviar_orden(body);

        }
        public string notificar_carga_de_produccion_fatay()
        {
            // string salto_de_linea = "%0A"; 
            string body = "";

            body = "Se ha registrado en sistema una nueva produccion de Fabrica Fatay.";

            return enviar_aviso(body);

        }
        private string enviar_pedido(string body, DataTable sucursalBD)
        {
            string telefono = string.Empty;
            if (sucursalBD.Rows[0]["id"].ToString() == "22")
            {
                 telefono = ConfigurationManager.AppSettings["telefono_fabrica_oficina"];
            }
            else
            {
                 telefono = ConfigurationManager.AppSettings["telefono_pedidos"];
            }
            string url = "https://api.whatsapp.com/send?phone=" + telefono + "&text=" + body.Replace(" ", "%20");

            return url;
        }
        private string enviar_pedido_proveedores(string body)
        {


            string telefono = ConfigurationManager.AppSettings["telefono_pedidos"];
            string url = "https://api.whatsapp.com/send?phone=" + telefono + "&text=" + body.Replace(" ", "%20");

            return url;
        }
        private string enviar_orden(string body)
        {

            string telefono = ConfigurationManager.AppSettings["telefono_expedicion"];
            string url = "https://api.whatsapp.com/send?phone=" + telefono + "&text=" + body.Replace(" ", "%20");

            return url;
        }
        private string enviar_aviso(string body)
        {

            string telefono = ConfigurationManager.AppSettings["telefono_tulio"];
            string url = "https://api.whatsapp.com/send?phone=" + telefono + "&text=" + body.Replace(" ", "%20");

            return url;
        }
        private string obtener_cantidad_productos(DataTable pedido, DataTable bonificados)
        {
            string retorno, tipo_actual, tipo_siguiente;
            string salto_de_linea = "%0A";
            int cant;
            List<string> tipo_producto = new List<string>();
            List<string> cantidad = new List<string>();
            retorno = "";

            pedido.DefaultView.Sort = "tipo_producto";
            pedido = pedido.DefaultView.ToTable();



            cant = 0;
            for (int fila = 0; fila <= pedido.Rows.Count - 2; fila++)
            {
                tipo_actual = pedido.Rows[fila]["tipo_producto"].ToString();
                tipo_siguiente = pedido.Rows[fila + 1]["tipo_producto"].ToString();
                cant = cant + int.Parse(pedido.Rows[fila]["cantidad"].ToString());
                if (tipo_actual != tipo_siguiente)
                {
                    tipo_producto.Add(tipo_actual);
                    cantidad.Add(cant.ToString());
                    cant = 0;
                }

                if (fila == pedido.Rows.Count - 2)
                {
                    tipo_actual = pedido.Rows[fila]["tipo_producto"].ToString();
                    tipo_siguiente = pedido.Rows[fila + 1]["tipo_producto"].ToString();

                    if (tipo_actual == tipo_siguiente)
                    {
                        cant = cant + int.Parse(pedido.Rows[fila + 1]["cantidad"].ToString());
                        tipo_producto.Add(tipo_actual);
                        cantidad.Add(cant.ToString());
                    }
                    else
                    {
                        tipo_actual = pedido.Rows[fila + 1]["tipo_producto"].ToString();
                        tipo_producto.Add(tipo_actual);
                        cantidad.Add(pedido.Rows[fila + 1]["cantidad"].ToString());
                    }
                }
            }
            cant = 0;
            /*            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
                        {
                            cant = cant + int.Parse(bonificados.Rows[fila]["cantidad"].ToString());

                        }
                        tipo_actual = "BONIFICADO";
                        tipo_producto.Add(tipo_actual);
                        cantidad.Add(cant.ToString());*/

            for (int fila = 0; fila <= tipo_producto.Count - 1; fila++)
            {
                retorno = retorno + tipo_producto[fila].ToString() + " |" + cantidad[fila].ToString() + "|" + salto_de_linea;
            }
            return retorno;
        }

        private int obtener_fila_proveedor()
        {
            int retorno = 0;
            int fila = 0;
            while (fila <= lista_proveedores.Rows.Count - 1)
            {
                if (proveedor_seleccionado == lista_proveedores.Rows[fila]["nombre_proveedor"].ToString())
                {
                    retorno = fila;
                    fila = lista_proveedores.Rows.Count;
                }
                fila = fila + 1;
            }
            return retorno;
        }
    }
}
