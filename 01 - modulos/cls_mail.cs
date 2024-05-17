using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Data;

namespace modulos
{
    public class cls_mail
    {
        #region atributos
        private DataTable usuario;
        private DataTable sucursal;
        private DataTable lista_proveedores;
        private string proveedor_seleccionado;
        #endregion
        public cls_mail(DataTable usuarioBD, DataTable sucursalBD, DataTable lista_de_proveedores, string proveedor)
        {
            usuario = usuarioBD;
            sucursal = sucursalBD;
            lista_proveedores = lista_de_proveedores;
            proveedor_seleccionado = proveedor;
        }
        public string enviar_pedido(DataTable pedido, DataTable bonificados)
        {
            int num_pedido = int.Parse(sucursal.Rows[0]["ultimo_pedido_enviado"].ToString()) + 1;
            string asunto = "FRANQUICIA: " + sucursal.Rows[0]["franquicia"].ToString() + " SUCURSAL: " + usuario.Rows[0]["sucursal"].ToString() + " PEDIDO: " + num_pedido.ToString();
            string salto_de_linea = "\n";
            string body = "";

            body = "FRANQUICIA: " + sucursal.Rows[0]["franquicia"].ToString() + salto_de_linea;
            body += "SUCURSAL: " + sucursal.Rows[0]["sucursal"].ToString() + salto_de_linea;
            body += "PEDIDO: " + num_pedido.ToString() + salto_de_linea + salto_de_linea;

            body += "resumen:" + salto_de_linea;
            body += obtener_cantidad_productos(pedido, bonificados) + salto_de_linea;

            body += "pedido:" + salto_de_linea;

            for (int fila = 0; fila <= pedido.Rows.Count - 1; fila++)
            {
                body += pedido.Rows[fila]["producto"].ToString() + " |" + pedido.Rows[fila]["cantidad"].ToString() + " " + pedido.Rows[fila]["unidad de medida"].ToString() + "|" + salto_de_linea + salto_de_linea;
            }
            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
            {//agregar bonificado especial
             
                if (bonificados.Rows[fila]["tipo_bonificado"].ToString()== "BONIFICADO")
                {
                    body += bonificados.Rows[fila]["producto"].ToString() + " |" + bonificados.Rows[fila]["cantidad"].ToString() + " " + bonificados.Rows[fila]["unidad de medida"].ToString() + "| BONIFICADO" + salto_de_linea + salto_de_linea;
                }
                else
                {

                    body += bonificados.Rows[fila]["producto"].ToString() + " |" + bonificados.Rows[fila]["cantidad"].ToString() + " " + bonificados.Rows[fila]["unidad de medida"].ToString() + "| BONIFICADO ESPECIAL" + salto_de_linea + salto_de_linea;
                }

            }

            return enviar(asunto, body);
        }
        public string enviar(string asunto, string body)
        {
            int fila_prov = obtener_fila_proveedor();
            string msge = "Error al enviar este correo. por favor verifique los datos o intente mas tarde.";
            string from = usuario.Rows[0]["mail"].ToString();
            string displayName = sucursal.Rows[0]["franquicia"].ToString() + " " + sucursal.Rows[0]["sucursal"].ToString(); 
            
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add(lista_proveedores.Rows[fila_prov]["correo_electronico"].ToString());    //destinatario

                if (!DBNull.Value.Equals(lista_proveedores.Rows[fila_prov]["correo_electronico_copia"]) && lista_proveedores.Rows[fila_prov]["correo_electronico_copia"].ToString()!="")
                {
                    mail.CC.Add(lista_proveedores.Rows[fila_prov]["correo_electronico_copia"].ToString());//correo copia
                }

                if (!DBNull.Value.Equals(lista_proveedores.Rows[fila_prov]["correo_electronico_copia_oculta"])&& lista_proveedores.Rows[fila_prov]["correo_electronico_copia_oculta"].ToString()!="")
                {
                    mail.Bcc.Add(lista_proveedores.Rows[fila_prov]["correo_electronico_copia_oculta"].ToString()); //correo copia oculta
                }

                mail.Subject = asunto;
                mail.Body = body;
                mail.IsBodyHtml = false;

                SmtpClient client = new SmtpClient("smtp.correoseguro.co", 587);
                string con = usuario.Rows[0]["contraseña_mail"].ToString();
                client.Credentials = new NetworkCredential(from, con);
                client.EnableSsl = true;
      

                //client.Send(mail);
                msge = "¡correo enviado exitorsamente!";

            }
            catch (Exception ex)
            {

                msge = ex.Message + ". Porfavor verifica tu conexion a internet.";
            }
            return msge;
        }

        private string obtener_cantidad_productos(DataTable pedido, DataTable bonificados)
        {
            string retorno, tipo_actual, tipo_siguiente;
            string salto_de_linea = "\n";
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
            for (int fila = 0; fila <= bonificados.Rows.Count - 1; fila++)
            {
                cant = cant + int.Parse(bonificados.Rows[fila]["cantidad"].ToString());

            }
            tipo_actual = "BONIFICADO";
            tipo_producto.Add(tipo_actual);
            cantidad.Add(cant.ToString());

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
            while (fila <= lista_proveedores.Rows.Count -1)
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
