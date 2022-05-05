using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Contabilidad.RemesaDetalle.DA
{
    //public class RemesaDetalleMantenimientoDA : EntityCrud<BE.CO_REMESADETALLE>
    public class RemesaDetalleMantenimientoDA
    {
        ~RemesaDetalleMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(CO_REMESADETALLE pobjBE, ref long LonRemesaDetalleId, ref bool Error, ref string ErrorMensaje)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESADETALLE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@rede_iRemesaId", pobjBE.rede_iRemesaId));
                        cmd.Parameters.Add(new SqlParameter("@rede_sTipoId", pobjBE.rede_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@rede_cPeriodo", pobjBE.rede_cPeriodo));

                        if (pobjBE.rede_dFechaEnvio != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@rede_dFechaEnvio", pobjBE.rede_dFechaEnvio));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_dFechaEnvio", DBNull.Value)); }

                        cmd.Parameters.Add(new SqlParameter("@rede_FTipoCambioBancario", pobjBE.rede_FTipoCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@rede_FTipoCambioConsular", pobjBE.rede_FTipoCambioConsular));
                        cmd.Parameters.Add(new SqlParameter("@rede_sCuentaCorrienteId", pobjBE.rede_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@rede_sMonedaId", pobjBE.rede_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@rede_FMonto", pobjBE.rede_FMonto));

                        if (pobjBE.rede_vNroVoucher != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vNroVoucher", pobjBE.rede_vNroVoucher));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vNroVoucher", DBNull.Value)); }
                            
                        if (pobjBE.rede_vResponsableEnvio != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vResponsableEnvio", pobjBE.rede_vResponsableEnvio));
                        else{ cmd.Parameters.Add(new SqlParameter("@rede_vResponsableEnvio", DBNull.Value)); }

                        if (pobjBE.rede_vRecurrente != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vRecurrente", pobjBE.rede_vRecurrente));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vRecurrente", DBNull.Value)); }

                        if (pobjBE.rede_sTarifarioId != null && pobjBE.rede_sTarifarioId != 0)
                            cmd.Parameters.Add(new SqlParameter("@rede_sTarifarioId", pobjBE.rede_sTarifarioId));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_sTarifarioId", DBNull.Value)); }                        

                        if (pobjBE.rede_vObservacion != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vObservacion", pobjBE.rede_vObservacion));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vObservacion", DBNull.Value)); }                        

                        cmd.Parameters.Add(new SqlParameter("@rede_bMesFlag", pobjBE.rede_bMesFlag));
                        cmd.Parameters.Add(new SqlParameter("@rede_sEstadoId", pobjBE.rede_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@rede_sUsuarioCreacion", pobjBE.rede_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@rede_vIPCreacion", pobjBE.rede_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@rede_vHostName", Util.ObtenerHostName()));

                        cmd.Parameters.Add(new SqlParameter("@rede_sClasificacionId", pobjBE.ClasificacionID));

                        cmd.Parameters.Add(new SqlParameter("@rede_sOficinaConsularId", pobjBE.OficinaConsularId));

                        SqlParameter lReturn = cmd.Parameters.Add("@rede_iRemesaDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output; 

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        LonRemesaDetalleId = Convert.ToInt64(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                ErrorMensaje = ex.Message;
            }
        }

        public void Update(CO_REMESADETALLE pobjBE, ref long LonRemesaDetalleId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESADETALLE_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@rede_iRemesaId", pobjBE.rede_iRemesaId));
                        cmd.Parameters.Add(new SqlParameter("@rede_sTipoId", pobjBE.rede_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@rede_cPeriodo", pobjBE.rede_cPeriodo));
                        
                        if (pobjBE.rede_dFechaEnvio != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@rede_dFechaEnvio", pobjBE.rede_dFechaEnvio));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_dFechaEnvio", DBNull.Value)); }
                        
                        cmd.Parameters.Add(new SqlParameter("@rede_FTipoCambioBancario", pobjBE.rede_FTipoCambioBancario));
                        cmd.Parameters.Add(new SqlParameter("@rede_FTipoCambioConsular", pobjBE.rede_FTipoCambioConsular));
                        
                        if (pobjBE.rede_sCuentaCorrienteId != 0)
                            cmd.Parameters.Add(new SqlParameter("@rede_sCuentaCorrienteId", pobjBE.rede_sCuentaCorrienteId));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_sCuentaCorrienteId", DBNull.Value)); } 

                        cmd.Parameters.Add(new SqlParameter("@rede_sMonedaId", pobjBE.rede_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@rede_FMonto", pobjBE.rede_FMonto));

                        if (pobjBE.rede_vNroVoucher != null)                            
                            cmd.Parameters.Add(new SqlParameter("@rede_vNroVoucher", pobjBE.rede_vNroVoucher));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vNroVoucher", DBNull.Value)); }

                        if (pobjBE.rede_vResponsableEnvio != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vResponsableEnvio", pobjBE.rede_vResponsableEnvio));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vResponsableEnvio", DBNull.Value)); }

                        if (pobjBE.rede_vRecurrente != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vRecurrente", pobjBE.rede_vRecurrente));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_vRecurrente", DBNull.Value)); }

                        if (pobjBE.rede_sTarifarioId != null && pobjBE.rede_sTarifarioId != 0)
                            cmd.Parameters.Add(new SqlParameter("@rede_sTarifarioId", pobjBE.rede_sTarifarioId));
                        else { cmd.Parameters.Add(new SqlParameter("@rede_sTarifarioId", DBNull.Value)); }

                        if (pobjBE.rede_vObservacion != null)
                            cmd.Parameters.Add(new SqlParameter("@rede_vObservacion", pobjBE.rede_vObservacion));
                        else{ cmd.Parameters.Add(new SqlParameter("@rede_vObservacion", DBNull.Value)); }

                        cmd.Parameters.Add(new SqlParameter("@rede_bMesFlag", pobjBE.rede_bMesFlag));
                        cmd.Parameters.Add(new SqlParameter("@rede_sEstadoId", pobjBE.rede_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@rede_sUsuarioModificacion", pobjBE.rede_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@rede_vIPModificacion", pobjBE.rede_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@rede_vHostName", Util.ObtenerHostName()));

                        cmd.Parameters.Add(new SqlParameter("@rede_sClasificacionId", pobjBE.ClasificacionID));

                        cmd.Parameters.Add(new SqlParameter("@rede_sOficinaConsularId", pobjBE.OficinaConsularId));

                        SqlParameter lReturn = cmd.Parameters.Add("@rede_iRemesaDetalleId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output; 

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        LonRemesaDetalleId = Convert.ToInt64(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }            
        }

        public void Delete(CO_REMESADETALLE pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESADETALLE_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                        
                        
                        cmd.Parameters.Add(new SqlParameter("@rede_iRemesaId", pobjBE.rede_iRemesaId));                        
                        cmd.Parameters.Add(new SqlParameter("@rede_sUsuarioModificacion", pobjBE.rede_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@rede_vIPModificacion", pobjBE.rede_vIPModificacion));                        
                        cmd.Parameters.Add(new SqlParameter("@rede_vHostName", Util.ObtenerHostName()));                        

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }           
        }       
    }
}
