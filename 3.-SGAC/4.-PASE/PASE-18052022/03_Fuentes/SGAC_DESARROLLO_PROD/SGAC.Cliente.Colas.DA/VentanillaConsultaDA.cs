using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Cliente.Colas.DA
{
    public class VentanillaConsultaDA
    {
        ~VentanillaConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int intOficinaConsular,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vent_sOficinaConsularId", intOficinaConsular));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }


        public DataTable ImprimeAgenciaRemota(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_AGENCIASREMOTAS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeAfluenciaClientes(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_AFLUENCIACONNACIONALES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeAfluenciaVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_AFLUENCIAVENTANILLA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeAtencionxVentanilla(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_ATENCIONES_POR_VENTANILLA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeAtencionesxUsusario(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_NUMERO_ATENCIONES_RECURRENTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeAtencionesxTipoAtencion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_ATENCIONES_POR_ TIPO_ATENCION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimerRendimientoProcesos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_INDICADORES_RENDIMIENTO_PROCESOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeTiempoEsperaxCliente(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_NUMERO_CLIENTES_ESPERA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeProductividadxOperador(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_PRODUCTIVIDAD_POR_OPERADORES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeTiempoAtencionxTransaccion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_TIEMPO_TRANSACCION_ATENCION_MAX_MIN_PROM", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeEvaluacionAtencionxTipoAtencion(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_EVAL_ATENCION_SERVICIO_SUBSERVICIO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeTicketsAtendidosNoAtendidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_TICKET_ATENDIDOS_Y_NOATENDIDOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeTicketsEmitidos(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_TICKETS_EMITIDOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ImprimeSeguimientoControl(int idOfinaConsular, DateTime fechaInicio, DateTime fechaFin, int iUsuario, string vIp)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_COLAS_SEGUIMIENTO_CONTROL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@COD_OFICINACONSULAR", idOfinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioCreacion", iUsuario));
                        cmd.Parameters.Add(new SqlParameter("@audi_vIPCreacion", vIp));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }


    }
}
