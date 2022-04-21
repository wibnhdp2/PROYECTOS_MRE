using System;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SGAC.Contabilidad.Remesa.DA
{
    public class RemesaMantenimientoDA
    {
        ~RemesaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(CO_REMESA pobjBE, ref long LonRemesaId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularOrigenId", pobjBE.reme_sOficinaConsularOrigenId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularDestinoId", pobjBE.reme_sOficinaConsularDestinoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_cPeriodo", pobjBE.reme_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@reme_FMonto", pobjBE.reme_FMonto));

                        if (pobjBE.reme_dFechaEnvio.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_dFechaEnvio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_dFechaEnvio", pobjBE.reme_dFechaEnvio));
                        }

                        if (pobjBE.reme_vObservacion == null)
                            cmd.Parameters.Add(new SqlParameter("@reme_vObservacion", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@reme_vObservacion", pobjBE.reme_vObservacion));

                        cmd.Parameters.Add(new SqlParameter("@reme_sEstadoId", pobjBE.reme_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sUsuarioCreacion", pobjBE.reme_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vIPCreacion", pobjBE.reme_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularId", pobjBE.OficinaConsularId));

                        SqlParameter lReturn = cmd.Parameters.Add("@reme_iRemesaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        LonRemesaId = Convert.ToInt64(lReturn.Value);

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

        public void Update(CO_REMESA pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_iRemesaId", pobjBE.reme_iRemesaId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sTipoId", pobjBE.reme_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularOrigenId", pobjBE.reme_sOficinaConsularOrigenId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularDestinoId", pobjBE.reme_sOficinaConsularDestinoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_cPeriodo", pobjBE.reme_cPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@reme_sCuentaCorrienteId", pobjBE.reme_sCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@reme_vNumeroEnvio", pobjBE.reme_vNumeroEnvio));
                        cmd.Parameters.Add(new SqlParameter("@reme_sMonedaId", pobjBE.reme_sMonedaId));
                        cmd.Parameters.Add(new SqlParameter("@reme_FMonto", pobjBE.reme_FMonto));

                        if (pobjBE.reme_dFechaEnvio.ToString().Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_dFechaEnvio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_dFechaEnvio", pobjBE.reme_dFechaEnvio));
                        }

                        if (pobjBE.reme_vResponsableEnvio == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_vResponsableEnvio", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_vResponsableEnvio", pobjBE.reme_vResponsableEnvio));
                        }

                        if (pobjBE.reme_vObservacion == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_vObservacion", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@reme_vObservacion", pobjBE.reme_vObservacion));
                        }

                        cmd.Parameters.Add(new SqlParameter("@reme_sEstadoId", pobjBE.reme_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@reme_sUsuarioModificacion", pobjBE.reme_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vIPModificacion", pobjBE.reme_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularId", pobjBE.OficinaConsularId));

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

        public void Eliminar(CO_REMESA pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_iRemesaId", pobjBE.reme_iRemesaId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sUsuarioModificacion", pobjBE.reme_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vIPModificacion", pobjBE.reme_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vHostName", Util.ObtenerHostName()));

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

        public void ActualizarEstado(int intRemesaId,
                                     int intTipoRemesa,
                                     int intEstadoId,
                                     int intUsuarioModificacion,
                                     string strIPModificacion,
                                     int intOficinaConsular,
                                     ref bool Error, ref string strErrorMensaje, ref string strErrorDetalle)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REMESA_CAMBIAR_ESTADO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@reme_iRemesaId", intRemesaId)); 
                        cmd.Parameters.Add(new SqlParameter("@reme_sEstadoId", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@reme_sTipoId", intTipoRemesa));
                        cmd.Parameters.Add(new SqlParameter("@reme_sUsuarioModificacion", intUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vIPModificacion", strIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@reme_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@reme_sOficinaConsularId", intOficinaConsular));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                strErrorMensaje = ex.Message;
                strErrorDetalle = ex.StackTrace;
            }
        }
    }
}
