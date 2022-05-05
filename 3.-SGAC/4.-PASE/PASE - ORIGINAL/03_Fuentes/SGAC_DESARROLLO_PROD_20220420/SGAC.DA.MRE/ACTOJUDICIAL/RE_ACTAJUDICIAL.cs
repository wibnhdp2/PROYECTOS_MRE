using System;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SGAC.DA.MRE.ACTOJUDICIAL
{
    public class RE_ACTAJUDICIAL
    {
        public string strError = "";

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SGAC.BE.MRE.RE_ACTAJUDICIAL Insertar(SGAC.BE.MRE.RE_ACTAJUDICIAL ActaJudicial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        SqlParameter lReturn = cmd.Parameters.Add("@acjd_iActaJudicialId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new SqlParameter("@acjd_iActoJudicialNotificacionId", ActaJudicial.acjd_iActoJudicialNotificacionId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_sTipoActaId", ActaJudicial.acjd_sTipoActaId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_IFuncionarioFirmanteId", ActaJudicial.acjd_IFuncionarioFirmanteId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_dFechaHoraActa", ActaJudicial.acjd_dFechaHoraActa));

                        if (ActaJudicial.acjd_vCuerpoActa != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vCuerpoActa", ActaJudicial.acjd_vCuerpoActa.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vCuerpoActa", DBNull.Value));

                        if (ActaJudicial.acjd_vResultado != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResultado", ActaJudicial.acjd_vResultado.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResultado", DBNull.Value));

                        if (ActaJudicial.acjd_vObservaciones != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vObservaciones", ActaJudicial.acjd_vObservaciones.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vObservaciones", DBNull.Value));

                        if (ActaJudicial.acjd_vResponsable != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResponsable", ActaJudicial.acjd_vResponsable.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResponsable", DBNull.Value));
                        
                        cmd.Parameters.Add(new SqlParameter("@acjd_sEstadoId", ActaJudicial.acjd_sEstadoId));                        
                        cmd.Parameters.Add(new SqlParameter("@acjd_sUsuarioCreacion", ActaJudicial.acjd_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acjd_vIPCreacion", ActaJudicial.acjd_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acjd_dFechaCreacion", ActaJudicial.acjd_dFechaCreacion ));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActaJudicial.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActaJudicial.HostName));
                        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActaJudicial.acjd_iActaJudicialId = Convert.ToInt64(lReturn.Value);
                        ActaJudicial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActaJudicial.Error = true;
                ActaJudicial.Message = exec.Message.ToString();
            }

            return ActaJudicial;
        }

        public SGAC.BE.MRE.RE_ACTAJUDICIAL Actualizar(SGAC.BE.MRE.RE_ACTAJUDICIAL ActaJudicial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        
                        cmd.Parameters.Add(new SqlParameter("@acjd_iActaJudicialId", ActaJudicial.acjd_iActaJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_iActoJudicialNotificacionId", ActaJudicial.acjd_iActoJudicialNotificacionId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_sTipoActaId", ActaJudicial.acjd_sTipoActaId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_IFuncionarioFirmanteId", ActaJudicial.acjd_IFuncionarioFirmanteId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_dFechaHoraActa", ActaJudicial.acjd_dFechaHoraActa));

                        if (ActaJudicial.acjd_vCuerpoActa != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vCuerpoActa", ActaJudicial.acjd_vCuerpoActa.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vCuerpoActa", DBNull.Value));

                        if (ActaJudicial.acjd_vResultado != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResultado", ActaJudicial.acjd_vResultado.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResultado", DBNull.Value));

                        if (ActaJudicial.acjd_vObservaciones != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vObservaciones", ActaJudicial.acjd_vObservaciones.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vObservaciones", DBNull.Value));

                        if (ActaJudicial.acjd_vResponsable != null)
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResponsable", ActaJudicial.acjd_vResponsable.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acjd_vResponsable", DBNull.Value));

                        cmd.Parameters.Add(new SqlParameter("@acjd_sEstadoId", ActaJudicial.acjd_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_sUsuarioModificacion", ActaJudicial.acjd_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acjd_vIPModificacion", ActaJudicial.acjd_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acjd_dFechaModificacion", ActaJudicial.acjd_dFechaModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActaJudicial.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActaJudicial.HostName));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        
                        ActaJudicial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActaJudicial.Error = true;
                ActaJudicial.Message = exec.Message.ToString();
            }

            return ActaJudicial;
        }

        public int Actualizar_Estado(Int64 iActaJudicialId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName,
            string sObservaciones)
        {
            int intResult = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_ACTUALIZAR_ESTADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@acjd_iActaJudicialId",iActaJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_sEstadoId", sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acjd_sUsuarioModificacion",sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acjd_vIPModificacion",vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", vHostName));
                        cmd.Parameters.Add(new SqlParameter("@acjd_vObservaciones", sObservaciones));

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = 1;
                    }
                }
            }
            catch (SqlException exec)
            {
                intResult = 0;
                strError = exec.Message.ToString();
            }

            return intResult;
        }


        //// NO SE HA ENCONTRADO SU INVOCADOR EN BL
        //public int Actualizar_Acto_General(long iActuacionId, long iActuacionDetalleId, long sOficinaConsularId)
        //{
        //    int intResult = 0;
        //    try
        //    {
        //        using (SqlConnection cnx = new SqlConnection(this.conexion()))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_MODIFICAR_ESTADO", cnx))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                #region Creando Parametros

        //                cmd.Parameters.Add(new SqlParameter("@actu_iActuacionId", iActuacionId));
        //                cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", iActuacionDetalleId));
        //                cmd.Parameters.Add(new SqlParameter("@actu_sOficinaConsularId", sOficinaConsularId));
                        
        //                #endregion

        //                cmd.Connection.Open();
        //                cmd.ExecuteNonQuery();

        //                intResult = 1;
        //            }
        //        }
        //    }
        //    catch (SqlException exec)
        //    {
        //        intResult = 0;
        //        strError = exec.Message.ToString();
        //    }

        //    return intResult;
        //}

        public DataTable Obtener(Int64 iActoJudicialNotificacionId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iActoJudicialNotificacionId", iActoJudicialNotificacionId));
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
    }
}
