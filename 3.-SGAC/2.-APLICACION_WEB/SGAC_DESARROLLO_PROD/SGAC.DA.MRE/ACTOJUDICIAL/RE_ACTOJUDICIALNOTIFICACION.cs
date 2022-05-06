using System;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SGAC.DA.MRE.ACTOJUDICIAL
{
    public class RE_ACTOJUDICIALNOTIFICACION
    {

        public string strError = string.Empty;

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION Insertar (SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION ActoJudicialNotificaciones)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALNOTIFICACION_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        SqlParameter lReturn = cmd.Parameters.Add("@ajno_iActoJudicialNotificacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialParticipanteId", ActoJudicialNotificaciones.ajno_iActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sTipoRecepcionId", ActoJudicialNotificaciones.ajno_sTipoRecepcionId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sViaEnvioId", ActoJudicialNotificaciones.ajno_sViaEnvioId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vEmpresaServicioPostal", ActoJudicialNotificaciones.ajno_vEmpresaServicioPostal));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vPersonaNotificacion", ActoJudicialNotificaciones.ajno_vPersonaNotificacion));

                        cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraNotificacion", ActoJudicialNotificaciones.ajno_dFechaHoraNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vNumeroCedula", ActoJudicialNotificaciones.ajno_vNumeroCedula));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vPersonaRecibeNotificacion", ActoJudicialNotificaciones.ajno_vPersonaRecibeNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraRecepcion", ActoJudicialNotificaciones.ajno_dFechaHoraRecepcion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vCuerpoNotificacion", ActoJudicialNotificaciones.ajno_vCuerpoNotificacion));

                        cmd.Parameters.Add(new SqlParameter("@ajno_vObservaciones", ActoJudicialNotificaciones.ajno_vObservaciones));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sEstadoId", ActoJudicialNotificaciones.ajno_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sUsuarioCreacion", ActoJudicialNotificaciones.ajno_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vIPCreacion", ActoJudicialNotificaciones.ajno_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_dFechaCreacion", ActoJudicialNotificaciones.ajno_dFechaCreacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoJudicialNotificaciones.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActoJudicialNotificaciones.HostName));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoJudicialNotificaciones.ajno_iActoJudicialNotificacionId = Convert.ToInt64(lReturn.Value);
                        ActoJudicialNotificaciones.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoJudicialNotificaciones.Error = true;
                ActoJudicialNotificaciones.Message = exec.Message.ToString();
            }

            return ActoJudicialNotificaciones;
        }

        public SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION Actualizar(SGAC.BE.MRE.RE_ACTOJUDICIALNOTIFICACION ActoJudicialNotificaciones)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALNOTIFICACION_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialNotificacionId", ActoJudicialNotificaciones.ajno_iActoJudicialNotificacionId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialParticipanteId", ActoJudicialNotificaciones.ajno_iActoJudicialParticipanteId));



                        if (ActoJudicialNotificaciones.ajno_sTipoRecepcionId.HasValue)
                        {
                            if (ActoJudicialNotificaciones.ajno_sTipoRecepcionId.Value > 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@ajno_sTipoRecepcionId", ActoJudicialNotificaciones.ajno_sTipoRecepcionId));
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@ajno_sTipoRecepcionId", DBNull.Value));
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajno_sTipoRecepcionId", DBNull.Value));
                        }

                        if (ActoJudicialNotificaciones.ajno_dFechaHoraNotificacion != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraNotificacion", ActoJudicialNotificaciones.ajno_dFechaHoraNotificacion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraNotificacion", DBNull.Value));
                        }

                        if (ActoJudicialNotificaciones.ajno_dFechaHoraRecepcion != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraRecepcion", ActoJudicialNotificaciones.ajno_dFechaHoraRecepcion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ajno_dFechaHoraRecepcion", DBNull.Value));
                        }
                        
                        cmd.Parameters.Add(new SqlParameter("@ajno_sViaEnvioId", ActoJudicialNotificaciones.ajno_sViaEnvioId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vEmpresaServicioPostal", ActoJudicialNotificaciones.ajno_vEmpresaServicioPostal));

                        cmd.Parameters.Add(new SqlParameter("@ajno_vPersonaNotificacion", ActoJudicialNotificaciones.ajno_vPersonaNotificacion));
                        
                        cmd.Parameters.Add(new SqlParameter("@ajno_vNumeroCedula", ActoJudicialNotificaciones.ajno_vNumeroCedula));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vPersonaRecibeNotificacion", ActoJudicialNotificaciones.ajno_vPersonaRecibeNotificacion));


                        cmd.Parameters.Add(new SqlParameter("@ajno_vCuerpoNotificacion", ActoJudicialNotificaciones.ajno_vCuerpoNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vObservaciones", ActoJudicialNotificaciones.ajno_vObservaciones));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sEstadoId", ActoJudicialNotificaciones.ajno_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_sUsuarioModificacion", ActoJudicialNotificaciones.ajno_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ajno_vIPModificacion", ActoJudicialNotificaciones.ajno_vIPModificacion));

                        cmd.Parameters.Add(new SqlParameter("@ajno_dFechaModificacion", ActoJudicialNotificaciones.ajno_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoJudicialNotificaciones.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActoJudicialNotificaciones.HostName));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                                                
                        ActoJudicialNotificaciones.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoJudicialNotificaciones.Error = true;
                ActoJudicialNotificaciones.Message = exec.Message.ToString();
            }
            return ActoJudicialNotificaciones;
        }
      
        public DataTable Obtener(Int64 iActoJudicialId, Int64 iActoJudicialParticipanteId,string StrCurrentPage, int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALNOTIFICACION_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialId", iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialParticipanteId", iActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

            return DtResult;
        }

        public DataTable Obtener_Id_Notificado(Int64 intActoJudicialParticipanteId, Int64 intActoJudicialNotificacionId)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALNOTIFICACION_OBTENER_ID_NOTIFICADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialParticipanteId", intActoJudicialParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@ajno_iActoJudicialNotificacionId", intActoJudicialNotificacionId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

            return DtResult;
        }

        public void Obtener_Actoparticipante(Int64 iActuacionDetalleId, ref Int64 iActoJudicialId, ref Int64 iActoJudicialParticipanteId)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIALDETALLE_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iActuacionDetalleId", iActuacionDetalleId));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@iActoJudicialId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@iActoJudicialParticipanteId", SqlDbType.BigInt);
                        lReturn2.Direction = ParameterDirection.Output;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        iActoJudicialId = Convert.ToInt32(lReturn1.Value.ToString() == "" ? 0 : lReturn1.Value);
                        iActoJudicialParticipanteId = Convert.ToInt32(lReturn2.Value.ToString() == "" ? 0 : lReturn2.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }

    }
}
