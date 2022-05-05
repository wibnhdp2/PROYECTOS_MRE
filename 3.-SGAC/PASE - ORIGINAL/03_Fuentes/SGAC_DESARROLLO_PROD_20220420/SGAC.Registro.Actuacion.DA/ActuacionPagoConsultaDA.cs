using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionPagoConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionPagoConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionPagoConsultaDA()
        {
            GC.Collect();
        }

        public DataTable ActuacionPagoObtener(long LonActuacionId)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", LonActuacionId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }

                return DtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
            }
        }
        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 14/02/2017
        // Objetivo: Actualiza Detalle de la actuación y el monto
        //---------------------------------------------------------------------------------------
        public DataTable ActuacionPagoObtenerDetalle(long LonActuacionId)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_OBTENER_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", LonActuacionId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }

                return DtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
            }
        }

        //---------------------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 21/11/2017
        // Objetivo: Obtengo si existe una operación con el mismo numero registrado con anterioridad
        //---------------------------------------------------------------------------------------
        public DataTable verificarOperacion(long? DetalleActuacion, Int16 CodBanco, string vNumOperacion,
            Int16 intTipoPago, Int16 sOficinaConsultar, DateTime dFechaOperacion)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_VERIFICAR_OPERACION_BANCO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pago_iActuacionDetalleId", DetalleActuacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sBancoId", CodBanco));
                        cmd.Parameters.Add(new SqlParameter("@pago_vBancoNumeroOperacion", vNumOperacion));
                        cmd.Parameters.Add(new SqlParameter("@pago_sPagoTipoId", intTipoPago));
                        cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", sOficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pago_dFechaOperacion", dFechaOperacion));
                        
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }

                return DtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
            }
        }
    }
}