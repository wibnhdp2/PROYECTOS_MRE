using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionConsultaDA()
        {
            GC.Collect();
        }
        public DataTable VerificarRegistroParticipantes(Int64 lngActuacionDetalleId, Int16 sTipoParticipanteId, Int64 iPersona = 0)
        {
            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_VERIFICAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_acde_iActuacionDetalleId", lngActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@P_acpa_sTipoParticipanteId", sTipoParticipanteId));
                        cmd.Parameters.Add(new SqlParameter("@P_acpa_iPersonaId", iPersona));
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            DtResult = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                DtResult = null;
                throw ex;
            }

            return DtResult;
        }
        public DataTable ObtenerParticipantes(Int64 lngActuacionDetalleId)
        {
            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_PARTICIPANTES", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@reci_iActuacionDetalleId", lngActuacionDetalleId));
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            DtResult = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                DtResult = null;
                throw ex;
            }

            return DtResult;  
        }
        public DataTable RecurrenteConsultar(int IntTipo,
                                             string StrNroDoc,
                                             string StrApePat,
                                             string StrApeMat,
                                             string strNombre,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {

            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ITipoQ", IntTipo));
                        cmd.Parameters.Add(new SqlParameter("@vNroDocumento", StrNroDoc));
                        cmd.Parameters.Add(new SqlParameter("@vApellidoPaterno", StrApePat));
                        cmd.Parameters.Add(new SqlParameter("@vApellidoMaterno", StrApeMat));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", strNombre));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", IntCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            DtResult = dsObjeto.Tables[0];
                        }
                        if (lReturn1.Value.ToString().Length > 0)
                        {
                            IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        }
                        if (lReturn2.Value.ToString().Length > 0)
                        {
                            IntTotalPages = Convert.ToInt32(lReturn2.Value);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                DtResult = null;
                throw ex;
            }

            return DtResult;  
        }


        public DataTable EmpresaConsultar(string StrNroDoc,
                                          string StrRazonSocial,
                                          int IntCurrentPage,
                                          int IntPageSize,
                                          ref int IntTotalCount,
                                          ref int IntTotalPages)
        {
            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@empr_vNroDocumento", StrNroDoc));
                        cmd.Parameters.Add(new SqlParameter("@empr_vRazonSocial", StrRazonSocial));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", IntCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            DtResult = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                DtResult = null;
                throw ex;
            }

            return DtResult;            
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ReasignacionConsultar(long LonPersonaId,
                                               string StrNroDoc,
                                               string StrApePat,
                                               string StrApeMat,
                                               string StrCurrentPage,
                                               int IntPageSize,
                                               ref int IntTotalCount,
                                               ref int IntTotalPages)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_REASIGNAR_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iPersonaId", SqlDbType.BigInt).Value = LonPersonaId;


                        if (StrNroDoc.Length == 0)
                        {
                            cmd.Parameters.Add("@vNroDocumento", SqlDbType.VarChar, 20).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@vNroDocumento", SqlDbType.VarChar, 20).Value = StrNroDoc;
                        }

                        if (StrApePat.Length == 0)
                        {
                            cmd.Parameters.Add("@vApellidoPaterno", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@vApellidoPaterno", SqlDbType.VarChar, 100).Value = StrApePat;
                        }


                        if (StrApeMat.Length == 0)
                        {
                            cmd.Parameters.Add("@vApellidoMaterno", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@vApellidoMaterno", SqlDbType.VarChar, 100).Value = StrApeMat;
                        }

                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = StrCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                        if (lMovimientoIdReturn1.Value != null)
                        {
                            if (lMovimientoIdReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalCount = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalPages = Convert.ToInt32(lMovimientoIdReturn2.Value.ToString());
                            }
                        }

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada

            //DataSet DsResult = new DataSet();
            //DataTable DtResult = new DataTable();

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[8];

            //    prmParameter[0] = new SqlParameter("@iPersonaId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonPersonaId;

            //    if (StrNroDoc.Length == 0)
            //    {
            //        prmParameter[1] = new SqlParameter("@vNroDocumento", SqlDbType.VarChar, 20);
            //        prmParameter[1].Value = DBNull.Value;
            //    }
            //    else
            //    {
            //        prmParameter[1] = new SqlParameter("@vNroDocumento", SqlDbType.VarChar, 20);
            //        prmParameter[1].Value = StrNroDoc;
            //    }

            //    if (StrApePat.Length == 0)
            //    {
            //        prmParameter[2] = new SqlParameter("@vApellidoPaterno", SqlDbType.VarChar, 100);
            //        prmParameter[2].Value = DBNull.Value;
            //    }
            //    else
            //    {
            //        prmParameter[2] = new SqlParameter("@vApellidoPaterno", SqlDbType.VarChar, 100);
            //        prmParameter[2].Value = StrApePat;
            //    }

            //    if (StrApeMat.Length == 0)
            //    {
            //        prmParameter[3] = new SqlParameter("@vApellidoMaterno", SqlDbType.VarChar, 100);
            //        prmParameter[3].Value = DBNull.Value;
            //    }
            //    else
            //    {
            //        prmParameter[3] = new SqlParameter("@vApellidoMaterno", SqlDbType.VarChar, 100);
            //        prmParameter[3].Value = StrApeMat;
            //    }

            //    prmParameter[4] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
            //    prmParameter[4].Value = StrCurrentPage;

            //    prmParameter[5] = new SqlParameter("@IPageSize", SqlDbType.Int);
            //    prmParameter[5].Value = IntPageSize;

            //    prmParameter[6] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
            //    prmParameter[6].Direction = ParameterDirection.Output;

            //    prmParameter[7] = new SqlParameter("@ITotalPages", SqlDbType.Int);
            //    prmParameter[7].Direction = ParameterDirection.Output;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_PERSONA_REASIGNAR_CONSULTAR",
            //                                        prmParameter);

            //    DtResult = DsResult.Tables[0];

            //    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[6]).Value);
            //    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);

            //    return DtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    DtResult = null;
            //    DsResult = null;
            //}

            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ActuacionesObtener(long LonPersonaId,
            long LonEmpresaId,
                                            int intSeccionId,
                                            DateTime datFecInicio,
                                            DateTime datFecFin,
                                            string StrCurrentPage,
                                            int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iPersonaId", SqlDbType.BigInt).Value = LonPersonaId;
                        cmd.Parameters.Add("@iEmpresaId", SqlDbType.BigInt).Value = LonEmpresaId;
                        cmd.Parameters.Add("@sSeccionId", SqlDbType.SmallInt).Value = intSeccionId;
                        cmd.Parameters.Add("@dFecIni", SqlDbType.DateTime).Value = datFecInicio;
                        cmd.Parameters.Add("@dFecFin", SqlDbType.DateTime).Value = datFecFin;

                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = Convert.ToInt32(StrCurrentPage);
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                        if (lMovimientoIdReturn1.Value != null)
                        {
                            if (lMovimientoIdReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalCount = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalPages = Convert.ToInt32(lMovimientoIdReturn2.Value.ToString());
                            }
                        }

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet DsResult = new DataSet();
            //DataTable DtResult = new DataTable();

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[9];

            //    prmParameter[0] = new SqlParameter("@iPersonaId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonPersonaId;

            //    prmParameter[1] = new SqlParameter("@iEmpresaId", SqlDbType.BigInt);
            //    prmParameter[1].Value = LonEmpresaId;

            //    prmParameter[2] = new SqlParameter("@sSeccionId", SqlDbType.SmallInt);
            //    prmParameter[2].Value = intSeccionId;

            //    prmParameter[3] = new SqlParameter("@dFecIni", SqlDbType.DateTime);
            //    prmParameter[3].Value = datFecInicio;

            //    prmParameter[4] = new SqlParameter("@dFecFin", SqlDbType.DateTime);
            //    prmParameter[4].Value = datFecFin;

            //    prmParameter[5] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
            //    prmParameter[5].Value = StrCurrentPage;

            //    prmParameter[6] = new SqlParameter("@IPageSize", SqlDbType.Int);
            //    prmParameter[6].Value = IntPageSize;

            //    prmParameter[7] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
            //    prmParameter[7].Direction = ParameterDirection.Output;

            //    prmParameter[8] = new SqlParameter("@ITotalPages", SqlDbType.Int);
            //    prmParameter[8].Direction = ParameterDirection.Output;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER",
            //                                        prmParameter);

            //    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);
            //    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[8]).Value);

            //    DtResult = DsResult.Tables[0];

            //    return DtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    DtResult = null;
            //    DsResult = null;
            //}

            #endregion
        }

        //public DataTable ActuacionesObtenerPorAutoadhesivo(string StrNroDoc,
        //                                    string StrCurrentPage,
        //                                    int IntPageSize,
        //                                    ref int IntTotalCount,
        //                                    ref int IntTotalPages)


        //----------------------------------------------------------
        // Modificado por: JONATAN SILVA CACHAY
        // Fecha: 25/02/2020
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ActuacionesObtenerxRGE(Int16 Consulado,
                                            Int16 Anio,
                                            Int64 RGE
                                            )
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_RGE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iConsuladoId", SqlDbType.SmallInt).Value = Consulado;
                        cmd.Parameters.Add("@sAnio", SqlDbType.SmallInt).Value = Anio;
                        cmd.Parameters.Add("@iRGE", SqlDbType.BigInt).Value = RGE;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        
        public DataTable ActuacionesObtenerPorAutoadhesivo(string StrNroDoc,
                                    int StrCurrentPage,
                                    int IntPageSize,
                                    ref int IntTotalCount,
                                    ref int IntTotalPages)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            IntTotalCount = 0;
            IntTotalPages = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_POR_AUTOADHESIVO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iCodAutoadhesivo", SqlDbType.VarChar, 50).Value = StrNroDoc;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = Convert.ToInt32(StrCurrentPage);
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                        if (lMovimientoIdReturn1.Value != null)
                        {
                            if (lMovimientoIdReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalCount = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                IntTotalPages = Convert.ToInt32(lMovimientoIdReturn2.Value.ToString());
                            }
                        }
                        
                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada

            //DataSet DsResult = new DataSet();
            //DataTable DtResult = new DataTable();

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[9];

            //    prmParameter[0] = new SqlParameter("@iCodAutoadhesivo", SqlDbType.VarChar, 50);
            //    prmParameter[0].Value = StrNroDoc;

            //    prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
            //    prmParameter[1].Value = StrCurrentPage;

            //    prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
            //    prmParameter[2].Value = IntPageSize;

            //    prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
            //    prmParameter[3].Direction = ParameterDirection.Output;

            //    prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
            //    prmParameter[4].Direction = ParameterDirection.Output;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_POR_AUTOADHESIVO",
            //                                        prmParameter);

            //    DtResult = DsResult.Tables[0];

            //    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value is DBNull ? 0 : Convert.ToInt32(((SqlParameter)prmParameter[3]).Value));
            //    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value is DBNull ? 0 : Convert.ToInt32(((SqlParameter)prmParameter[4]).Value));
                
                
            //    return DtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    DtResult = null;
            //    DsResult = null;
            //}
            #endregion
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ActuacionAutoadhesivo(long LonActuacionId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_AUTOADHESIVO_RPT", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionId;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #region Comentada
            //DataSet DsResult = new DataSet();
            //DataTable DtResult = new DataTable();

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[1];

            //    prmParameter[0] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonActuacionId;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACION_AUTOADHESIVO_RPT",
            //                                        prmParameter);

            //    DtResult = DsResult.Tables[0];
            //    return DtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    DtResult = null;
            //    DsResult = null;
            //}
            #endregion
        }

        public DataTable ObtenerDatosAutoadhesivoNotarial(long lngActuacionId, long lngActuacionDetalleId, Int32 iLimiteTarifa = 0)
        {
            DataTable DtResult = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACION_AUTOADHESIVO_RPT_NOTARIAL", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@actu_iActuacionId", lngActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionDetalleId", lngActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@actu_iLimiteTarifa", iLimiteTarifa));
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

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlParameter
        //----------------------------------------------------------

        public int ObtenerSaldoAutoadhesivos(int IntOficinaConsular, int IntBodegaDestinoId, int intTipoInsumo)
        {
            int IntResult = 0;


            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_OBTENER_STOCK_AUTOADHESIVOS", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsular;
                        cmd.Parameters.Add("@movi_sBodegaDestinoId", SqlDbType.SmallInt).Value = IntBodegaDestinoId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@IStock", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lReturn1.Value != null)
                        {
                            if (lReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntResult = Convert.ToInt32(lReturn1.Value.ToString());
                            }
                        }
                        
                    }
                }
            }

            #region Comentada
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[4];

            //    prmParameter[0] = new SqlParameter("@IStock", SqlDbType.Int);
            //    prmParameter[0].Direction = ParameterDirection.Output;

            //    prmParameter[1] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //    prmParameter[1].Value = IntOfficinaConsular;

            //    prmParameter[2] = new SqlParameter("@movi_sBodegaDestinoId", SqlDbType.SmallInt);
            //    prmParameter[2].Value = IntBodegaDestinoId;

            //    prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //    prmParameter[3].Value = intTipoInsumo;

            //    SqlHelper.ExecuteNonQuery(StrConnectionName,
            //                              CommandType.StoredProcedure,
            //                              "PN_ALMACEN.USP_AL_MOVIMIENTO_OBTENER_STOCK_AUTOADHESIVOS",
            //                               prmParameter);

            //    IntResult = (int)prmParameter[0].Value;
            //}
            #endregion
            catch (Exception ex)
            {
                throw ex;
            }
            return IntResult;
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlParameter
        //----------------------------------------------------------
        public int ObtenerSaldoInsumos(int IntOfficinaConsular, int intTipoInsumo)
        {
            int IntResult = 0;

             try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_OBTENER_STOCK_AUTOADHESIVOS_POR_MISION_CONSULAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOfficinaConsular;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@IStock", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lReturn1.Value != null)
                        {
                            if (lReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntResult = Convert.ToInt32(lReturn1.Value.ToString());
                            }
                        }
                        
                    }
                }
            }
             catch (Exception ex)
             {
                 throw ex;
             }
             return IntResult;

            #region Comentada
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[4];

            //    prmParameter[0] = new SqlParameter("@IStock", SqlDbType.Int);
            //    prmParameter[0].Direction = ParameterDirection.Output;

            //    prmParameter[1] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //    prmParameter[1].Value = IntOfficinaConsular;

            //    prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //    prmParameter[3].Value = intTipoInsumo;

            //    SqlHelper.ExecuteNonQuery(StrConnectionName,
            //                              CommandType.StoredProcedure,
            //                              "PN_ALMACEN.USP_AL_MOVIMIENTO_OBTENER_STOCK_AUTOADHESIVOS_POR_MISION_CONSULAR",
            //                               prmParameter);

            //    IntResult = (int)prmParameter[0].Value;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return IntResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlParameter
        //----------------------------------------------------------
        public int ObtenerStockMinimoInsumos(int IntOfficinaConsular, int IntBodegaDestinoId, int intTipoInsumo)
        {
            int IntResult = 0;

            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_MINIMO_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@stck_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOfficinaConsular;
                        cmd.Parameters.Add("@stck_sInsumoId", SqlDbType.Int).Value = intTipoInsumo;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@IStockMinimo", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lReturn1.Value != null)
                        {
                            if (lReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                IntResult = Convert.ToInt32(lReturn1.Value.ToString());
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IntResult;

            #region Comentada

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[4];

            //    prmParameter[0] = new SqlParameter("@IStockMinimo", SqlDbType.Int);
            //    prmParameter[0].Direction = ParameterDirection.Output;

            //    prmParameter[1] = new SqlParameter("@stck_sOficinaConsularId", SqlDbType.SmallInt);
            //    prmParameter[1].Value = IntOfficinaConsular;

            //    prmParameter[2] = new SqlParameter("@stck_sInsumoId", SqlDbType.SmallInt); //3
            //    prmParameter[2].Value = intTipoInsumo; //3

            //    SqlHelper.ExecuteNonQuery(StrConnectionName,
            //                              CommandType.StoredProcedure,
            //                              "PS_SISTEMA.USP_SI_STOCK_MINIMO_OBTENER",
            //                               prmParameter);

            //    IntResult = (int)prmParameter[0].Value;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return IntResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlParameter
        //----------------------------------------------------------
        public DataTable ObtenerDatosPorActuacionDetalle(long lngActuacionDetalleId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_CONSULTAR_ID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = lngActuacionDetalleId;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet dsResult = new DataSet();
            //DataTable dtResult = new DataTable();
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[1];

            //    prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[0].Value = lngActuacionDetalleId;

            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_CONSULTAR_ID",
            //                                        prmParameter);

            //    dtResult = dsResult.Tables[0];
            //    return dtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dtResult = null;
            //    dsResult = null;
            //}
            #endregion
        }

        public DataTable ObtenerDatosPorActuacionDetalleLeftJoin(long lngActuacionDetalleId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_CONSULTAR_ID_LEFTJOIN", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = lngActuacionDetalleId;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataAdapter
        //----------------------------------------------------------
        public DataTable ObtenerSeguimientoActuacion(long lngActuacionDetalleId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_SEGUIMIENTO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = lngActuacionDetalleId;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet dsResult = new DataSet();
            //DataTable dtResult = new DataTable();
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[1];

            //    prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[0].Value = lngActuacionDetalleId;

            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName, CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_SEGUIMIENTO",
            //                                        prmParameter);

            //    dtResult = dsResult.Tables[0];
            //    return dtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dtResult = null;
            //    dsResult = null;
            //}
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataAdapter
        //----------------------------------------------------------
        public DataTable ActuacionDetalleObtener(long LonActuacionIdPrimario, long LonActuacionIdSec)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONES_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iActuacionId1", SqlDbType.BigInt).Value = LonActuacionIdPrimario;
                        cmd.Parameters.Add("@iActuacionId2", SqlDbType.BigInt).Value = LonActuacionIdSec;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet dsResult = null;
            //DataTable dtResult = null;

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[2];

            //    prmParameter[0] = new SqlParameter("@iActuacionId1", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonActuacionIdPrimario;

            //    prmParameter[1] = new SqlParameter("@iActuacionId2", SqlDbType.BigInt);
            //    prmParameter[1].Value = LonActuacionIdSec;

            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONES_OBTENER",
            //                                        prmParameter);
            //    dtResult = dsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return dtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataAdapter
        //----------------------------------------------------------
        public int ObtenerCantidadPorTarifa(Int64 lngPersonaId, Int16 intTarifaId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            int intCantidad = 0;
            
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_TARIFA_VERIFICAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iPersonaId", SqlDbType.BigInt).Value = lngPersonaId;
                        cmd.Parameters.Add("@sTarifaId", SqlDbType.SmallInt).Value = intTarifaId;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            intCantidad = Convert.ToInt32(dsResult.Tables[0].Rows[0][0].ToString());
                        

                    }
                }

                return intCantidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            #region Comentada
            //DataSet dsResult = null;
            
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[2];

            //    prmParameter[0] = new SqlParameter("@iPersonaId", SqlDbType.BigInt);
            //    prmParameter[0].Value = lngPersonaId;

            //    prmParameter[1] = new SqlParameter("@sTarifaId", SqlDbType.SmallInt);
            //    prmParameter[1].Value = intTarifaId;

                
            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_TARIFA_VERIFICAR",
            //                                        prmParameter);
            //    int intCantidad = Convert.ToInt32(dsResult.Tables[0].Rows[0][0].ToString());
            //    //int intCantidad = Convert.ToInt32(SqlHelper.ExecuteScalar(StrConnectionName,
            //    //                                    "PN_REGISTRO.USP_RE_ACTUACIONDETALLE_TARIFA_VERIFICAR",
            //    //                                    prmParameter));
            //    return intCantidad;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlParameter
        //----------------------------------------------------------
        public Int64 ActuacionTramiteExiste(long LonActuacionDetalleId, int IntTipoActo)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            Int64 intActuacionId = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONTRAMITE_EXISTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;
                        cmd.Parameters.Add("@sTipoActo", SqlDbType.SmallInt).Value = IntTipoActo;

                        SqlParameter lMovimientoIdReturn = cmd.Parameters.Add("@iActuacionSeccionId", SqlDbType.BigInt);
                        lMovimientoIdReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lMovimientoIdReturn.Value != null)
                        {
                            if (lMovimientoIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intActuacionId = Convert.ToInt64(lMovimientoIdReturn.Value.ToString());
                            }
                        }
                    }
                }

                return intActuacionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #region Comentada

            //Int64 intActuacionId = 0;

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[3];

            //    prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonActuacionDetalleId;

            //    prmParameter[1] = new SqlParameter("@sTipoActo", SqlDbType.SmallInt);
            //    prmParameter[1].Value = IntTipoActo;

            //    prmParameter[2] = new SqlParameter("@iActuacionSeccionId", SqlDbType.BigInt);
            //    prmParameter[2].Direction = ParameterDirection.Output;

            //    SqlHelper.ExecuteNonQuery(StrConnectionName,
            //                              CommandType.StoredProcedure,
            //                              "PN_REGISTRO.USP_RE_ACTUACIONTRAMITE_EXISTE",
            //                              prmParameter);

            //    if (prmParameter[2].Value != null)
            //    {
            //        if (prmParameter[2].Value.ToString() != string.Empty)
            //            intActuacionId = Convert.ToInt64(prmParameter[2].Value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return intActuacionId;
            #endregion
        }


        public Boolean ExisteDigitalizacion(long acde_iActuacionDetalle,Int32 sSession, ref Boolean bExiste)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_EXISTE_DIGITALIZACION", cnn))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt).Value = acde_iActuacionDetalle;
                        cmd.Parameters.Add("@tari_sSeccionId", SqlDbType.SmallInt).Value = sSession;
                        cmd.Parameters.Add("@bExiste", SqlDbType.Bit, 30);
                        cmd.Parameters["@bExiste"].Direction = ParameterDirection.Output;

                        cnn.Open();
                        cmd.ExecuteNonQuery();
                        cnn.Close();
                        bExiste = Convert.ToBoolean(cmd.Parameters["@bExiste"].Value);
                        return bExiste;
                    }
                }
              
            }
            catch (Exception ex) {
                return false;
            }
        }

        public DataTable ObtenerActuacionDetalle_Actuacion(long iPersonaId, long iactu_ActuacionID) {
            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_ACTUACION", cnn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@actu_iPersonaId", SqlDbType.BigInt).Value = iPersonaId;
                        cmd.Parameters.Add("@actu_iActuacionId", SqlDbType.BigInt).Value = iactu_ActuacionID;
                        cnn.Open();
                        DataTable DT = new DataTable();
                        SqlDataAdapter DA = new SqlDataAdapter(cmd);
                        DA.Fill(DT);
                        cnn.Close();
                        return DT;
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public DataTable ObtenerActuacionDetalle_Actuacion_PorAutoadhesivo(string StrNroDoc)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_ACTUACION_POR_AUTOADHESIVO", cnn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iCodAutoadhesivo", SqlDbType.VarChar).Value = StrNroDoc;
                        cnn.Open();
                        DataTable DT = new DataTable();
                        SqlDataAdapter DA = new SqlDataAdapter(cmd);
                        DA.Fill(DT);
                        cnn.Close();
                        return DT;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataAdapter
        //----------------------------------------------------------
        // OBTENER DATOS DE LA ACTUACIÓN 58 A DE UNA PERSONA
        //JONATAN SILVA C.
        public DataTable ObtenerActuacion58A(long CodPersona)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_ACTUACION_58A", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@actu_iPersonaId", SqlDbType.BigInt).Value = CodPersona;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet dsResult = null;
            //DataTable dtResult = null;

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[1];

            //    prmParameter[0] = new SqlParameter("@actu_iPersonaId", SqlDbType.BigInt);
            //    prmParameter[0].Value = CodPersona;

            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "[PN_REGISTRO].[USP_RE_ACTUACIONDETALLE_OBTENER_ACTUACION_58A]",
            //                                        prmParameter);
            //    dtResult = dsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return dtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataAdapter
        //----------------------------------------------------------
        public DataTable ActuacionesObtenerAnuladasReactivar(Int16 actu_sOficinaConsularId, int acde_ICorrelativoActuacion, Int16 acde_AnioTramite)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_REACTIVAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@P_actu_sOficinaConsularId", SqlDbType.SmallInt).Value = actu_sOficinaConsularId;
                        cmd.Parameters.Add("@P_acde_ICorrelativoActuacion", SqlDbType.Int).Value = acde_ICorrelativoActuacion;
                        cmd.Parameters.Add("@P_acde_AnioTramite", SqlDbType.SmallInt).Value = acde_AnioTramite;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada

            //DataSet dsResult = null;
            //DataTable dtResult = null;

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[3];

            //    prmParameter[0] = new SqlParameter("@P_actu_sOficinaConsularId", SqlDbType.SmallInt);
            //    prmParameter[0].Value = actu_sOficinaConsularId;
            //    prmParameter[1] = new SqlParameter("@P_acde_ICorrelativoActuacion", SqlDbType.Int);
            //    prmParameter[1].Value = acde_ICorrelativoActuacion;
            //    prmParameter[2] = new SqlParameter("@P_acde_AnioTramite", SqlDbType.SmallInt);
            //    prmParameter[2].Value = acde_AnioTramite;

            //    dsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                        CommandType.StoredProcedure,
            //                                        "[PN_REGISTRO].[USP_RE_ACTUACIONDETALLE_OBTENER_REACTIVAR]",
            //                                        prmParameter);
            //    dtResult = dsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return dtResult;
            #endregion
        }

        //-----------------------------------------------
        //Fecha: 13/10/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: consultar la actuación por el ID
        //          para la solicitud de SUNARP.
        //-----------------------------------------------

        public DataTable ConsultarActoNotarialPorIDActuacion(Int64 intActoNotarial)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_NOTARIAL_CONSULTAR_DATOS_INTERCONEXION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@P_acno_iActoNotarialId", SqlDbType.BigInt).Value = intActoNotarial;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
