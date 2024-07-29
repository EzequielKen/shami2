using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace paginaWeb
{
    [Serializable]
    public class cls_funciones
    {
        public string get_fecha()
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return fecha;
        }
        public bool verificar_fecha(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year == int.Parse(año_dato) && fecha_1.Month == int.Parse(mes_dato))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha_completa(string fecha_1_dato, string dia_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year == int.Parse(año_dato) && fecha_1.Month == int.Parse(mes_dato) &&
                fecha_1.Day == int.Parse(dia_dato))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha_anterior(string fecha_dato, string mes, string año)
        {
            bool retorno = false;
            System.DateTime fecha = DateTime.Parse(fecha_dato);

            if (fecha.Year < int.Parse(año))
            {
                retorno = true;
            }
            else if (fecha.Year.ToString() == año && fecha.Month < int.Parse(mes))
            {
                retorno = true;
            }
            return retorno;
        }
        public int buscar_fila_por_id(string id, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                if (id == dt.Rows[fila]["id"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public int buscar_fila_por_id_nombre(string id, string nombre, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                
                string id_muestra = dt.Rows[fila]["id"].ToString();
                string nombre_muestra = dt.Rows[fila]["producto"].ToString();
                if (id == dt.Rows[fila]["id"].ToString() && nombre == dt.Rows[fila]["producto"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public int buscar_fila_por_id_proveedor(string id, string proveedor, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {

                string id_muestra = dt.Rows[fila]["id"].ToString();
                string nombre_muestra = dt.Rows[fila]["producto"].ToString();
                if (id == dt.Rows[fila]["id"].ToString() && proveedor == dt.Rows[fila]["proveedor"].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public bool verificar_si_cargo(string id, DataTable dt)
        {
            bool retorno = false;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                if (id == dt.Rows[fila]["id"].ToString())
                {
                    retorno = true;
                    break;
                }
                fila++;
            }
            return retorno;
        }
        public bool buscar_alguna_coincidencia(string dato_usuario, string dato_MySQL)
        {
            int posicion_MySQL, posicion_dato, i;
            bool resultado;
            dato_usuario = dato_usuario.ToUpper();
            dato_MySQL = dato_MySQL.ToUpper();
            resultado = false;
            if (dato_usuario.Length < 1)
            {
                resultado = true;
                return resultado;
            }
            try
            {
                posicion_MySQL = 0;
                posicion_dato = 0;
                while (posicion_MySQL <= dato_MySQL.Length - 2 && dato_usuario.Length > 1)
                {
                    i = posicion_MySQL;
                    if (dato_usuario[0] == dato_MySQL[posicion_MySQL] && dato_usuario[0 + 1] == dato_MySQL[posicion_MySQL + 1])
                    {
                        i = posicion_MySQL;
                        while (posicion_dato <= dato_usuario.Length - 1)
                        {
                            if (dato_usuario[posicion_dato] == dato_MySQL[i])
                            {
                                resultado = true;
                                if (posicion_dato == dato_usuario.Length - 1)
                                {
                                    posicion_MySQL = dato_MySQL.Length;
                                }
                            }
                            else
                            {
                                resultado = false;
                                posicion_dato = dato_usuario.Length;
                                posicion_MySQL = dato_MySQL.Length;
                            }
                            posicion_dato = posicion_dato + 1;
                            i = i + 1;
                        }

                    }
                    posicion_MySQL = posicion_MySQL + 1;
                }
            }
            catch (Exception)
            {

                resultado = false;
            }
            return resultado;
        }
        public string obtener_dato(string dato, int posicion_dato)
        {
            string retorno = "";
            int posicion = 0;
            int i = 0;
            while (i <= dato.Length - 1)
            {
                if (dato[i].ToString() == "-" &&
                    dato[i - 1].ToString() != "-")
                {
                    
                    posicion++;
                    if (posicion == posicion_dato)
                    {
                        break;
                    }
                    else
                    {
                        retorno = "";
                    }
                }
                else
                {
                    retorno = retorno + dato[i].ToString();
                }
                i++;
            }
            return retorno;
        }
        public string formatCurrency(object valor)
        {
            if (valor is string)
            {
                string dato = (string)valor;
                double numero;
                if (double.TryParse(dato, out numero))
                {
                    return string.Format("{0:C2}", numero);
                }
                else
                {
                    return string.Format("{0:C2}", valor);
                }
            }
            else
            {
                return string.Format("{0:C2}", valor);
            }
        }
        public bool IsNotDBNull(object valor)
        {
            bool retorno = false;
            if (!DBNull.Value.Equals(valor))
            {
                retorno = true;
            }
            return retorno;
        }
        public string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
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
        public string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
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
        public bool verificar_tipo_producto(string tipo_producto, string tipo_seleccionado)
        {

            bool retorno = false;
            if (tipo_seleccionado == tipo_producto || tipo_seleccionado == "Todos")
            {
                retorno = true;
            }
            return retorno;
        }
        /*private void llenar_dropDownList(DataTable dt)
       {
           dropDown_tipo.Items.Clear();
           int num_item = 1;
           ListItem item;
           dt.DefaultView.Sort = "tipo_producto";
           dt = dt.DefaultView.ToTable();

           //        item = new ListItem("Todos", num_item.ToString());
           //        dropDown_tipo.Items.Add(item);
           //        num_item = num_item + 1;

           tipo_seleccionado = dt.Rows[0]["tipo_producto"].ToString();
           item = new ListItem(dt.Rows[0]["tipo_producto"].ToString(), num_item.ToString());
           dropDown_tipo.Items.Add(item);
           num_item = num_item + 1;
           for (int fila = 1; fila <= dt.Rows.Count - 1; fila++)
           {


               if (dropDown_tipo.Items[num_item - 2].Text != dt.Rows[fila]["tipo_producto"].ToString())
               {

                   item = new ListItem(dt.Rows[fila]["tipo_producto"].ToString(), num_item.ToString());
                   dropDown_tipo.Items.Add(item);
                   num_item = num_item + 1;

               }

           }
       }*/
    }
}