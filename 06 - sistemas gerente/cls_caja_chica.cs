using _01___modulos;
using modulos;
using paginaWeb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06___sistemas_gerente
{
    public class cls_caja_chica
    {
        public cls_caja_chica(DataTable usuario_BD)
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

        cls_PDF PDF = new cls_PDF();
        DataTable tipo_movimientos_caja_chica;
        DataTable movimientos_caja_chica;
        DataTable movimientos;
        #endregion

        #region carga a base de datos
        #region eliminar movimiento
        public void eliminar_movimiento(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "movimientos_caja_chica", actualizar, id);
            recalcular_total_en_caja();
        }
        #endregion
        #region carga de movimientos
        public void cargar_movimiento(string detalle, string fecha, string concepto, string tipo_movimiento, string inicial, string movimiento, string final)
        {
            string columna = "";
            string valores = "";
            //detalle
            columna = funciones.armar_query_columna(columna, "detalle", false);
            valores = funciones.armar_query_valores(valores, detalle, false);
            //fecha
            columna = funciones.armar_query_columna(columna, "fecha", false);
            valores = funciones.armar_query_valores(valores, fecha, false);
            //concepto
            columna = funciones.armar_query_columna(columna, "concepto", false);
            valores = funciones.armar_query_valores(valores, concepto, false);
            //tipo_movimiento
            columna = funciones.armar_query_columna(columna, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            //inicial
            columna = funciones.armar_query_columna(columna, "inicial", false);
            valores = funciones.armar_query_valores(valores, inicial, false);
            //movimiento
            columna = funciones.armar_query_columna(columna, "movimiento", false);
            valores = funciones.armar_query_valores(valores, movimiento, false);
            //final
            columna = funciones.armar_query_columna(columna, "final", true);
            valores = funciones.armar_query_valores(valores, final, true);
            consultas.insertar_en_tabla(base_de_datos, "movimientos_caja_chica", columna, valores);
        }
        #endregion
        #region conceptos
        public void registrar_concepto(string tipo_movimiento, string concepto)
        {
            string columna = "";
            string valores = "";
            //tipo_movimiento
            columna = funciones.armar_query_columna(columna, "tipo_movimiento", false);
            valores = funciones.armar_query_valores(valores, tipo_movimiento, false);
            //concepto
            columna = funciones.armar_query_columna(columna, "concepto", true);
            valores = funciones.armar_query_valores(valores, concepto, true);
            consultas.insertar_en_tabla(base_de_datos, "tipo_movimientos_caja_chica", columna, valores);
        }
        public void deshabilitar_concepto(string id)
        {
            string actualizar = "`estado` = 'Deshabilitado'";
            consultas.actualizar_tabla(base_de_datos, "tipo_movimientos_caja_chica", actualizar, id);
        }
        public void habilitar_concepto(string id)
        {
            string actualizar = "`estado` = 'Habilitado'";
            consultas.actualizar_tabla(base_de_datos, "tipo_movimientos_caja_chica", actualizar, id);
        }
        public void eliminar_concepto(string id)
        {
            string actualizar = "`activa` = '0'";
            consultas.actualizar_tabla(base_de_datos, "tipo_movimientos_caja_chica", actualizar, id);
        }
        public void modificar_concepto(string id, string concepto, string nuevo_concepto)
        {
            string actualizar,id_movimiento;
            consultar_movmientos(concepto);
            for (int fila = 0; fila <= movimientos.Rows.Count - 1; fila++)
            {
                id_movimiento = movimientos.Rows[fila]["id"].ToString();
                actualizar = "`concepto` = '" + nuevo_concepto + "'";
                consultas.actualizar_tabla(base_de_datos, "movimientos_caja_chica", actualizar, id_movimiento);
            }
            actualizar = "`concepto` = '" + nuevo_concepto + "'";
            consultas.actualizar_tabla(base_de_datos, "tipo_movimientos_caja_chica", actualizar, id);
        }
        #endregion

        #region calculo de deuda
        private void actualizar_movimiento_inicial(string id, string inicial)
        {
            string actualizar = "`inicial` = '" + inicial + "' ";
            consultas.actualizar_tabla(base_de_datos, "movimientos_caja_chica", actualizar, id);
        }
        private void actualizar_movimiento_final(string id, string final)
        {
            string actualizar = "`final` = '" + final + "' ";
            consultas.actualizar_tabla(base_de_datos, "movimientos_caja_chica", actualizar, id);
        }
        #endregion
        #endregion

        #region metodos privados
        public void calcular_total_en_caja()
        {
            consultar_movimientos_caja_chica();
            string id_inicial, id_final;
            double inicial, movimiento;
            double final = 0;
            movimientos_caja_chica.DefaultView.Sort = "fecha ASC, id ASC";
            movimientos_caja_chica = movimientos_caja_chica.DefaultView.ToTable();
            for (int fila = 0; fila <= movimientos_caja_chica.Rows.Count - 1; fila++)
            {

                inicial = double.Parse(movimientos_caja_chica.Rows[fila]["inicial"].ToString());
                movimiento = double.Parse(movimientos_caja_chica.Rows[fila]["movimiento"].ToString());

                if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Ingreso")
                {
                    final = inicial + movimiento;
                }
                else if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Egreso")
                {
                    final = inicial - movimiento;
                }
                if (fila + 1 <= movimientos_caja_chica.Rows.Count - 1)
                {
                    if (final.ToString() != movimientos_caja_chica.Rows[fila + 1]["inicial"].ToString() ||
                        final.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString())
                    {
                        //actualizar final actual e inicial siguiente
                        id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                        actualizar_movimiento_final(id_final, final.ToString());
                        movimientos_caja_chica.Rows[fila]["final"] = final.ToString();

                        id_inicial = movimientos_caja_chica.Rows[fila + 1]["id"].ToString();
                        actualizar_movimiento_inicial(id_inicial, final.ToString());
                        movimientos_caja_chica.Rows[fila + 1]["inicial"] = final.ToString();



                    }
                }
                else if (fila == movimientos_caja_chica.Rows.Count - 1)
                {
                    if (final.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString())
                    {
                        //actualizar final actual e inicial siguiente
                        id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                        actualizar_movimiento_final(id_final, final.ToString());
                        movimientos_caja_chica.Rows[fila]["inicial"] = final.ToString();
                    }
                }
            }
        }
        public void recalcular_total_en_caja()
        {
            consultar_movimientos_caja_chica();
            string id_inicial, id_final;
            double inicial, movimiento;
            double final = 0;
            movimientos_caja_chica.DefaultView.Sort = "fecha ASC, id ASC";
            movimientos_caja_chica = movimientos_caja_chica.DefaultView.ToTable();
            for (int fila = 0; fila <= movimientos_caja_chica.Rows.Count - 1; fila++)
            {
                if (fila == 0)
                {
                    inicial = 0;
                    movimiento = double.Parse(movimientos_caja_chica.Rows[fila]["movimiento"].ToString());
                }
                else
                {
                    inicial = double.Parse(movimientos_caja_chica.Rows[fila - 1]["final"].ToString());
                    movimiento = double.Parse(movimientos_caja_chica.Rows[fila]["movimiento"].ToString());
                }

                if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Ingreso")
                {
                    final = inicial + movimiento;
                }
                else if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Egreso")
                {
                    final = inicial - movimiento;
                }
                if (fila + 1 <= movimientos_caja_chica.Rows.Count - 1)
                {
                    if (fila == 0)
                    {
                        if (inicial.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString() ||
                            final.ToString() != movimientos_caja_chica.Rows[fila]["final"].ToString())
                        {
                            //actualizar final actual e inicial siguiente
                            id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                            actualizar_movimiento_final(id_final, final.ToString());
                            movimientos_caja_chica.Rows[fila]["final"] = final.ToString();

                            id_inicial = movimientos_caja_chica.Rows[fila]["id"].ToString();
                            actualizar_movimiento_inicial(id_inicial, inicial.ToString());
                            movimientos_caja_chica.Rows[fila]["inicial"] = inicial.ToString();
                        }
                    }
                    else
                    {
                        if (final.ToString() != movimientos_caja_chica.Rows[fila + 1]["inicial"].ToString() ||
                            final.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString())
                        {
                            //actualizar final actual e inicial siguiente
                            id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                            actualizar_movimiento_final(id_final, final.ToString());
                            movimientos_caja_chica.Rows[fila]["final"] = final.ToString();

                            id_inicial = movimientos_caja_chica.Rows[fila + 1]["id"].ToString();
                            actualizar_movimiento_inicial(id_inicial, final.ToString());
                            movimientos_caja_chica.Rows[fila + 1]["inicial"] = final.ToString();
                        }
                    }
                }
                else if (fila == movimientos_caja_chica.Rows.Count - 1)
                {
                    if (inicial.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString() ||
                        final.ToString() != movimientos_caja_chica.Rows[fila]["final"].ToString())
                    {
                        //actualizar final actual e inicial siguiente
                        id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                        actualizar_movimiento_final(id_final, final.ToString());
                        movimientos_caja_chica.Rows[fila]["final"] = final.ToString();

                        id_inicial = movimientos_caja_chica.Rows[fila]["id"].ToString();
                        actualizar_movimiento_inicial(id_inicial, inicial.ToString());
                        movimientos_caja_chica.Rows[fila]["inicial"] = inicial.ToString();
                    }
                }
            }
        }
        private string calcular_saldo_inicial(string fecha_dato)
        {
            DateTime fecha = DateTime.Parse(fecha_dato);
            DateTime fecha_movimiento;
            string id_inicial, id_final;
            double inicial, movimiento;
            double final = 0;
            double ultimo_final = 0;
            movimientos_caja_chica.DefaultView.Sort = "fecha ASC, id ASC";
            movimientos_caja_chica = movimientos_caja_chica.DefaultView.ToTable();
            for (int fila = 0; fila <= movimientos_caja_chica.Rows.Count - 1; fila++)
            {
                fecha_movimiento = DateTime.Parse(movimientos_caja_chica.Rows[fila]["fecha"].ToString());
                if (verificar_fecha(fecha, fecha_movimiento))
                {
                    inicial = double.Parse(movimientos_caja_chica.Rows[fila]["inicial"].ToString());
                    movimiento = double.Parse(movimientos_caja_chica.Rows[fila]["movimiento"].ToString());
                    if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Ingreso")
                    {
                        final = inicial + movimiento;
                    }
                    else if (movimientos_caja_chica.Rows[fila]["tipo_movimiento"].ToString() == "Egreso")
                    {
                        final = inicial - movimiento;
                    }

                    if (fila + 1 <= movimientos_caja_chica.Rows.Count - 1)
                    {
                        if (final.ToString() != movimientos_caja_chica.Rows[fila + 1]["inicial"].ToString() ||
                            final.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString())
                        {
                            //actualizar final actual e inicial siguiente
                            id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                            actualizar_movimiento_final(id_final, final.ToString());
                            movimientos_caja_chica.Rows[fila]["final"] = final.ToString();

                            id_inicial = movimientos_caja_chica.Rows[fila + 1]["id"].ToString();
                            actualizar_movimiento_inicial(id_inicial, final.ToString());
                            movimientos_caja_chica.Rows[fila + 1]["inicial"] = final.ToString();



                        }
                    }
                    else if (fila == movimientos_caja_chica.Rows.Count - 1)
                    {
                        if (final.ToString() != movimientos_caja_chica.Rows[fila]["inicial"].ToString())
                        {
                            //actualizar final actual e inicial siguiente
                            id_final = movimientos_caja_chica.Rows[fila]["id"].ToString();
                            actualizar_movimiento_final(id_final, final.ToString());
                            movimientos_caja_chica.Rows[fila]["inicial"] = final.ToString();
                        }
                    }
                    ultimo_final = final;
                }
            }
            return ultimo_final.ToString();
        }
        private bool verificar_fecha(DateTime fecha, DateTime fecha_movimiento)
        {
            bool retorno = false;
            if (fecha_movimiento.Year < fecha.Year)
            {
                retorno = true;
            }
            if (fecha_movimiento.Year == fecha.Year)
            {
                if (fecha_movimiento.Month < fecha.Month)
                {
                    retorno = true;
                }
                else if (fecha_movimiento.Month == fecha.Month)
                {
                    if (fecha_movimiento.Day <= fecha.Day)
                    {
                        retorno = true;
                    }
                }
            }
            return retorno;
        }
        #endregion

        #region metodos consultas
        private void consultar_tipo_movimientos_caja_chica()
        {
            tipo_movimientos_caja_chica = consultas.consultar_tabla(base_de_datos, "tipo_movimientos_caja_chica");
        }
        private void consultar_movimientos_caja_chica()
        {
            movimientos_caja_chica = consultas.consultar_tabla(base_de_datos, "movimientos_caja_chica");
        }
        private void consultar_movmientos(string concepto)
        {
            movimientos = consultas.consultar_movimiento_segun_concepto(concepto);
        }
        #endregion

        #region metodos get/set
        public DataTable get_tipo_movimientos_caja_chica()
        {
            consultar_tipo_movimientos_caja_chica();
            return tipo_movimientos_caja_chica;
        }

        public string get_saldo_en_caja_inicial(string fecha)
        {
            consultar_movimientos_caja_chica();
            return calcular_saldo_inicial(fecha);
        }
        public DataTable get_movimientos_caja_chica()
        {
            consultar_movimientos_caja_chica();
            movimientos_caja_chica.DefaultView.Sort = "fecha DESC, id DESC";
            movimientos_caja_chica = movimientos_caja_chica.DefaultView.ToTable();
            return movimientos_caja_chica;
        }
        public double get_saldo_actual()
        {
            consultar_movimientos_caja_chica();
            movimientos_caja_chica.DefaultView.Sort = "fecha DESC, id DESC";
            movimientos_caja_chica = movimientos_caja_chica.DefaultView.ToTable();
            if (-1 < movimientos_caja_chica.Rows.Count - 1)
            {
                return Math.Round(double.Parse(movimientos_caja_chica.Rows[0]["final"].ToString()), 2);
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
