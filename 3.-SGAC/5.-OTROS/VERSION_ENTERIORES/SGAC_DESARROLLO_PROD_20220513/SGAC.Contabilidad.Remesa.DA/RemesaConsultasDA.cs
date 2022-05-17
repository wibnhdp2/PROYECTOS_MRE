using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Contabilidad.Remesa.DA
{
    public class RemesaConsultasDA
    {
        ~RemesaConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas,
                                   int intOficinaConsularOrigenId,
                                   int intOficinaConsularDestinoId,
                                   int intEstadoId,
                                   DateTime? datFechaInicio,
                                   DateTime? datFechaFin)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularOrigenId", intOficinaConsularOrigenId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularDestinoId", intOficinaConsularDestinoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sEstadoId", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_dFechaInicio", datFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@reme_dFechaFin", datFechaFin));

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

        public DataTable ObtenerPorOficinaConsular(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_CONSULTAR_POR_OFICINA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularId", iOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@reme_dFechaInicio", dFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@reme_dFechaFin", dFechaFin));

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

        public bool ConsultarAvisoEnvioRemesa(int intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();
            bool bolEnviar = false;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_CONSULTAR_ENVIOALERTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", intOficinaConsularId));

                        SqlParameter lReturn = cmd.Parameters.Add("@bAlertaFlag", SqlDbType.Bit);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];

                            if (lReturn.Value != null)
                            {
                                if (lReturn.Value.ToString() != string.Empty)
                                {                                   
                                   bolEnviar = Convert.ToBoolean(lReturn.Value);                                    
                                }
                            }                           
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                bolEnviar = true;
                throw exec;
            }

            return bolEnviar;
        }

        //----------------------------------------------------------------------
        // Fecha: 27/01/2017
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Consultar las alertas pendientes de envio y/o enviadas
        //----------------------------------------------------------------------

        public void ConsultarAlerta(int intOficinaConsularId, ref bool bAlertaPendienteFlag, ref bool bAlertaEnviadosFlag)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_CONSULTAR_ALERTA_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", intOficinaConsularId));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@bAlertaPendienteFlag", SqlDbType.Bit);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@bAlertaEnviadosFlag", SqlDbType.Bit);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lReturn1.Value != null)
                        {
                            if (lReturn1.Value.ToString() != string.Empty)
                            {
                                bAlertaPendienteFlag = Convert.ToBoolean(lReturn1.Value);
                            }
                        }
                        if (lReturn2.Value != null)
                        {
                            if (lReturn2.Value.ToString() != string.Empty)
                            {
                                bAlertaEnviadosFlag = Convert.ToBoolean(lReturn2.Value);
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                bAlertaPendienteFlag = false;
                bAlertaEnviadosFlag = false;
                throw exec;
            }

        }

        public DataTable ObtenerAlertas(Int16 intOficinaConsularId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESANOTIFICACION_CONSULTAR_RESUMEN", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@reno_sOficinaConsularId", intOficinaConsularId));
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dt = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                dt = null;
            }
            return dt;
        }
    }
}
