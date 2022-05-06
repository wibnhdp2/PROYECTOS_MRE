using System;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using System.Data.SqlClient;
using SGAC.BE;

namespace SGAC.DA.MRE.ACTOJUDICIAL
{
    public class RE_ACTOJUDICIAL
    {
        public string strError = "";

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public SGAC.BE.MRE.RE_ACTOJUDICIAL Insertar(SGAC.BE.MRE.RE_ACTOJUDICIAL ActoJudicial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        #region Creando Parametros

                        SqlParameter lReturn = cmd.Parameters.Add("@acju_iActoJudicialId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                                                
                        cmd.Parameters.Add(new SqlParameter("@acju_iActuacionId", ActoJudicial.acju_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acju_sTipoNotificacion", ActoJudicial.acju_sTipoNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_sEntidadSolicitanteId", ActoJudicial.acju_sEntidadSolicitanteId));

                        //------------------------------------------------------------------
                        // Autor: Miguel Márquez Beltrán
                        // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                        //           y la fecha de salida de valija
                        // Fecha: 13/01/2017
                        //------------------------------------------------------------------
                        if (ActoJudicial.acju_dFechaRecepcion != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaRecepcion", ActoJudicial.acju_dFechaRecepcion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaRecepcion", DBNull.Value));
                        }
                        if (ActoJudicial.acju_dFechaAudiencia != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaAudiencia", ActoJudicial.acju_dFechaAudiencia));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaAudiencia", DBNull.Value));
                        }
                        //------------------------------------------------------------------
                        //cmd.Parameters.Add(new SqlParameter("@acju_dFechaCitacion", ActoJudicial.acju_dFechaCitacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_dFechaRegistro", ActoJudicial.acju_dFechaRegistro));

                        if (ActoJudicial.acju_vJuzgado!=null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vJuzgado", ActoJudicial.acju_vJuzgado.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vJuzgado", DBNull.Value));

                        if (ActoJudicial.acju_vOrganoJudicial!=null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vOrganoJudicial", ActoJudicial.acju_vOrganoJudicial.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vOrganoJudicial", DBNull.Value));

                        if (ActoJudicial.acju_vNumeroExpediente!=null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroExpediente", ActoJudicial.acju_vNumeroExpediente.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroExpediente", DBNull.Value));

                        //cmd.Parameters.Add(new SqlParameter("@acju_vNumeroHojaRemision", ActoJudicial.acju_vNumeroHojaRemision));

                        if (ActoJudicial.acju_vMateriaDemanda!=null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vMateriaDemanda", ActoJudicial.acju_vMateriaDemanda.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vMateriaDemanda", DBNull.Value));

                        if (ActoJudicial.acju_vNumeroOficio != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroOficio", ActoJudicial.acju_vNumeroOficio.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroOficio", DBNull.Value));


                        if (ActoJudicial.acju_vObservaciones != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vObservaciones", ActoJudicial.acju_vObservaciones.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vObservaciones", DBNull.Value));


                        cmd.Parameters.Add(new SqlParameter("@acju_sEstadoId", ActoJudicial.acju_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@acju_sUsuarioCreacion", ActoJudicial.acju_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_vIPCreacion", ActoJudicial.acju_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_dFechaCreacion", ActoJudicial.acju_dFechaCreacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoJudicial.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoJudicial.acju_iActoJudicialId = Convert.ToInt64(lReturn.Value);
                        ActoJudicial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoJudicial.Error = true;
                ActoJudicial.Message = exec.Message.ToString();
            }
            return ActoJudicial;
        }
        
        public SGAC.BE.MRE.RE_ACTOJUDICIAL Actualizar(SGAC.BE.MRE.RE_ACTOJUDICIAL ActoJudicial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        
                        cmd.Parameters.Add(new SqlParameter("@acju_iActoJudicialId", ActoJudicial.acju_iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@acju_iActuacionId", ActoJudicial.acju_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acju_sTipoNotificacion", ActoJudicial.acju_sTipoNotificacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_sEntidadSolicitanteId", ActoJudicial.acju_sEntidadSolicitanteId));

                        //------------------------------------------------------------------
                        // Autor: Miguel Márquez Beltrán
                        // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                        //           y la fecha de salida de valija
                        // Fecha: 13/01/2017
                        //------------------------------------------------------------------
                        if (ActoJudicial.acju_dFechaRecepcion != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaRecepcion", ActoJudicial.acju_dFechaRecepcion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaRecepcion", DBNull.Value));
                        }
                        if (ActoJudicial.acju_dFechaAudiencia != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaAudiencia", ActoJudicial.acju_dFechaAudiencia));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acju_dFechaAudiencia", DBNull.Value));
                        }

                        //------------------------------------------------------------------
                        
                        cmd.Parameters.Add(new SqlParameter("@acju_dFechaRegistro", ActoJudicial.acju_dFechaRegistro));

                        if (ActoJudicial.acju_vJuzgado != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vJuzgado", ActoJudicial.acju_vJuzgado.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vJuzgado", DBNull.Value));

                        if (ActoJudicial.acju_vOrganoJudicial != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vOrganoJudicial", ActoJudicial.acju_vOrganoJudicial.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vOrganoJudicial", DBNull.Value));

                        if (ActoJudicial.acju_vNumeroExpediente != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroExpediente", ActoJudicial.acju_vNumeroExpediente.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroExpediente", DBNull.Value));
                        //cmd.Parameters.Add(new SqlParameter("@acju_vNumeroHojaRemision", ActoJudicial.acju_vNumeroHojaRemision));

                        if (ActoJudicial.acju_vMateriaDemanda != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vMateriaDemanda", ActoJudicial.acju_vMateriaDemanda.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vMateriaDemanda", DBNull.Value));


                        if (ActoJudicial.acju_vNumeroOficio != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroOficio", ActoJudicial.acju_vNumeroOficio.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vNumeroOficio", DBNull.Value));


                        if (ActoJudicial.acju_vObservaciones != null)
                            cmd.Parameters.Add(new SqlParameter("@acju_vObservaciones", ActoJudicial.acju_vObservaciones.ToUpper()));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acju_vObservaciones", DBNull.Value));


                        cmd.Parameters.Add(new SqlParameter("@acju_sEstadoId", ActoJudicial.acju_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@acju_sUsuarioModificacion", ActoJudicial.acju_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_vIPModificacion", ActoJudicial.acju_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_dFechaModificacion", ActoJudicial.acju_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoJudicial.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));


                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoJudicial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoJudicial.Error = true;
                ActoJudicial.Message = exec.Message.ToString();
            }
            return ActoJudicial;
        }

        public DataTable Consultar_Expediente(RE_ExpedienteJudicial objEn)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ACTOJUDICIAL_CONSULTAR_EXPEDIENTE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@sOficinaConsularDestinoId", SqlDbType.Int).Value = objEn.sOficinaConsularDestinoId;
                    da.SelectCommand.Parameters.Add("@sTipoParticipanteId", SqlDbType.Int).Value = objEn.sTipoParticipanteId;
                    da.SelectCommand.Parameters.Add("@vNumeroExpediente", SqlDbType.VarChar, 30).Value = objEn.vNumeroExpediente;
                    da.SelectCommand.Parameters.Add("@sEstadoExpedienteId", SqlDbType.Int).Value = objEn.sEstadoExpedienteId;

                    if (objEn.sEstadoActaId == 0)
                        da.SelectCommand.Parameters.Add("@sEstadoActaId", SqlDbType.Int).Value = DBNull.Value;
                    else
                        da.SelectCommand.Parameters.Add("@sEstadoActaId", SqlDbType.Int).Value = objEn.sEstadoActaId;

                    da.SelectCommand.Parameters.Add("@vNumeroHojaRemision", SqlDbType.VarChar, 30).Value = objEn.vNumeroHojaRemision;
                    da.SelectCommand.Parameters.Add("@sTipoPersonaId", SqlDbType.Int).Value = objEn.sTipoPersonaId;
                    da.SelectCommand.Parameters.Add("@vdemandado", SqlDbType.VarChar, 50).Value = objEn.vdemandado;

                    da.SelectCommand.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = objEn.iPaginaActual;
                    da.SelectCommand.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = objEn.iPaginaCantidad;

                    da.SelectCommand.Parameters.Add("@ITotalRegistros", SqlDbType.Int).Direction = ParameterDirection.Output;
                    da.SelectCommand.Parameters.Add("@ITotalPaginas", SqlDbType.Int).Direction = ParameterDirection.Output;

                    if (objEn.dFechaInicio == null)
                    {
                        da.SelectCommand.Parameters.Add("@dfechaInicio", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        da.SelectCommand.Parameters.Add("@dfechaInicio", SqlDbType.DateTime).Value = objEn.dFechaInicio;
                    }

                    if (objEn.dFechaFin == null)
                    {
                        da.SelectCommand.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        da.SelectCommand.Parameters.Add("@dfechaFin", SqlDbType.DateTime).Value = objEn.dFechaFin;
                    }

                    da.Fill(ds, "Expediente");
                    dt = ds.Tables["Expediente"];

                    objEn.iTotalRegistros = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalRegistros"].Value.ToString());
                    objEn.iTotalPaginas = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalPaginas"].Value.ToString());

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable consultar_Exp_Participante(Int64 iActoJudicialId, Int64 iActoJudicialParticipanteId)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ACTOJUDICIALPARTICIPANTE_CONSULTAR", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActoJudicialId", SqlDbType.Int).Value = iActoJudicialId;
                    da.SelectCommand.Parameters.Add("@iActoJudicialParticipanteId", SqlDbType.Int).Value = iActoJudicialParticipanteId;

                    da.Fill(ds, "Participante");
                    dt = ds.Tables["Participante"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Consultar_Expediente_Historico(Int64 iActoJudicialId)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ACTOJUDICIAL_OBTENER_HISTORICO", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@iActoJudicialId", SqlDbType.Int).Value = iActoJudicialId;
     
                    da.Fill(ds, "Participante");
                    dt = ds.Tables["Participante"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable ConsultarExpedientePorPersona(long lngPersonaId, long lngEmpresaId, DateTime datFechaInicio, DateTime datFechaFin,
            int intPaginaActual, int intPaginaCantidad,ref int intTotalRegistros,ref int intTotalPaginas)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@iPersonaId", lngPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@iEmpresaId", lngEmpresaId));
                        cmd.Parameters.Add(new SqlParameter("@dFechaInicio", datFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@dFechaFin", datFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            intTotalRegistros = Convert.ToInt16(lReturn1.Value);
                            intTotalPaginas = Convert.ToInt16(lReturn2.Value);
                        }

                        dt = ds_Objeto.Tables[0];
                        return dt;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                dt = null;
            }
        }

        public BE.RE_ACTOJUDICIAL Obtener(Int64 iActoJudicialId)
        {
            DataTable DtResult = new DataTable();
            BE.RE_ACTOJUDICIAL Entidad = new BE.RE_ACTOJUDICIAL();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acju_iActoJudicialId", iActoJudicialId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];
                    }
                }

                if (DtResult.Rows.Count != 0)
                {
                    Entidad.acju_iActoJudicialId = Convert.ToInt32(DtResult.Rows[0]["acju_iActoJudicialId"].ToString());
                    Entidad.acju_iActuacionId = Convert.ToInt32(DtResult.Rows[0]["acju_iActuacionId"].ToString());
                    Entidad.acju_sTipoNotificacion = Convert.ToInt16(DtResult.Rows[0]["acju_sTipoNotificacion"].ToString());
                    Entidad.acju_sEntidadSolicitanteId = Convert.ToInt16(DtResult.Rows[0]["acju_sEntidadSolicitanteId"].ToString());
                    //------------------------------------------------------------------
                    // Autor: Miguel Márquez Beltrán
                    // Objetivo: No validar la Fecha de Recepción, Fecha de Audiencia
                    //           y la fecha de salida de valija
                    // Fecha: 13/01/2017
                    //------------------------------------------------------------------

                    if (DtResult.Rows[0]["acju_dFechaRecepcion"].ToString().Trim().Length > 0)
                    {
                        Entidad.acju_dFechaRecepcion = Convert.ToDateTime(DtResult.Rows[0]["acju_dFechaRecepcion"].ToString());
                    }
                    else
                    {
                        Entidad.acju_dFechaRecepcion = null;
                    }

                    if (DtResult.Rows[0]["acju_dFechaAudiencia"].ToString().Trim().Length > 0)
                    {
                        Entidad.acju_dFechaAudiencia = Convert.ToDateTime(DtResult.Rows[0]["acju_dFechaAudiencia"].ToString());
                    }
                    else
                    {
                        Entidad.acju_dFechaAudiencia = null;
                    }

                    if (DtResult.Rows[0]["acju_dFechaRegistro"].ToString().Trim().Length > 0)
                    {
                        Entidad.acju_dFechaRegistro = Convert.ToDateTime(DtResult.Rows[0]["acju_dFechaRegistro"].ToString());
                    }
                    else
                    {
                        Entidad.acju_dFechaRegistro = null;
                    }

                    //------------------------------------------------------------------
                    Entidad.acju_vJuzgado = DtResult.Rows[0]["acju_vJuzgado"].ToString();
                    Entidad.acju_vNumeroExpediente = DtResult.Rows[0]["acju_vNumeroExpediente"].ToString();
                    //Entidad.acju_vNumeroHojaRemision = DtResult.Rows[0]["acju_vNumeroHojaRemision"].ToString();
                    Entidad.acju_vMateriaDemanda = DtResult.Rows[0]["acju_vMateriaDemanda"].ToString();
                    Entidad.acju_vNumeroOficio = DtResult.Rows[0]["acju_vNumeroOficio"].ToString();
                    Entidad.acju_vObservaciones = DtResult.Rows[0]["acju_vObservaciones"].ToString();
                    Entidad.acju_sEstadoId = Convert.ToInt16(DtResult.Rows[0]["acju_sEstadoId"].ToString());
                    Entidad.acju_sUsuarioCreacion = Convert.ToInt16(DtResult.Rows[0]["acju_sUsuarioCreacion"].ToString());
                    Entidad.acju_vIPCreacion = DtResult.Rows[0]["acju_vIPCreacion"].ToString();
                    Entidad.acju_dFechaCreacion = Convert.ToDateTime(DtResult.Rows[0]["acju_dFechaCreacion"].ToString());
                    Entidad.acju_vOrganoJudicial = DtResult.Rows[0]["acju_vOrganoJudicial"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Entidad;
        }

        public DataTable Obtenertarifas()
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_CONSULTAR_TARIFARIO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                throw ex;
            }

            return DtResult;
        }

        public DataTable SaberSiCerramos(Int64 ajpa_iActoJudicialId)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_OBTENER_ESTADO_EXPEDIENTE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ajpa_iActoJudicialId", ajpa_iActoJudicialId));
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
                throw ex;
            }

            return DtResult;
        }
        
        public int ActualizarEstado(Int64 iActoJudicialId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
           int intResult = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_ACTUALIZAR_ESTADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@acju_iActoJudicialId",iActoJudicialId));
                        cmd.Parameters.Add(new SqlParameter("@acju_sEstadoId", sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acju_sUsuarioModificacion", sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acju_vIPModificacion", vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", vHostName));

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

        public int ActualizarCorrelativoActoJudicial(long intActuacionDetalleId, short intOficinaConsularId, short intTarifarioId, short intUsuarioId)
        {
            int intResult = 0;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOJUDICIAL_ACTUALIZAR_CORRELATIVO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", intActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sTarifarioId", intTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@acde_sUsuarioModificacion", intUsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@acde_vIPModificacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

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

        //------------------------------------------------
        //Fecha: 04/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Para la impresión
        //------------------------------------------------
        public DataTable Reporte_Expediente(RE_ExpedienteJudicial objEn)
        {
            using (SqlConnection cn = new SqlConnection(this.conexion()))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_ACTOJUDICIAL_CONSULTAR_EXPEDIENTE_MRE", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@sOficinaConsularDestinoId", SqlDbType.Int).Value = objEn.sOficinaConsularDestinoId;
                    da.SelectCommand.Parameters.Add("@sTipoParticipanteId", SqlDbType.Int).Value = objEn.sTipoParticipanteId;
                    da.SelectCommand.Parameters.Add("@vNumeroExpediente", SqlDbType.VarChar, 30).Value = objEn.vNumeroExpediente;
                    da.SelectCommand.Parameters.Add("@sEstadoExpedienteId", SqlDbType.Int).Value = objEn.sEstadoExpedienteId;

                    if (objEn.sEstadoActaId == 0)
                        da.SelectCommand.Parameters.Add("@sEstadoActaId", SqlDbType.Int).Value = DBNull.Value;
                    else
                        da.SelectCommand.Parameters.Add("@sEstadoActaId", SqlDbType.Int).Value = objEn.sEstadoActaId;

                    da.SelectCommand.Parameters.Add("@vNumeroHojaRemision", SqlDbType.VarChar, 30).Value = objEn.vNumeroHojaRemision;
                    da.SelectCommand.Parameters.Add("@sTipoPersonaId", SqlDbType.Int).Value = objEn.sTipoPersonaId;
                    da.SelectCommand.Parameters.Add("@vdemandado", SqlDbType.VarChar, 50).Value = objEn.vdemandado;

                    //da.SelectCommand.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = objEn.iPaginaActual;
                    //da.SelectCommand.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = objEn.iPaginaCantidad;

                    //da.SelectCommand.Parameters.Add("@ITotalRegistros", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //da.SelectCommand.Parameters.Add("@ITotalPaginas", SqlDbType.Int).Direction = ParameterDirection.Output;

                    if (objEn.dFechaInicio == null)
                    {
                        da.SelectCommand.Parameters.Add("@dfechaInicio", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        da.SelectCommand.Parameters.Add("@dfechaInicio", SqlDbType.DateTime).Value = objEn.dFechaInicio;
                    }

                    if (objEn.dFechaFin == null)
                    {
                        da.SelectCommand.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        da.SelectCommand.Parameters.Add("@dfechaFin", SqlDbType.DateTime).Value = objEn.dFechaFin;
                    }

                    da.Fill(ds, "Expediente");
                    dt = ds.Tables["Expediente"];

                    //objEn.iTotalRegistros = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalRegistros"].Value.ToString());
                    //objEn.iTotalPaginas = Convert.ToInt32(da.SelectCommand.Parameters["@ITotalPaginas"].Value.ToString());

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
