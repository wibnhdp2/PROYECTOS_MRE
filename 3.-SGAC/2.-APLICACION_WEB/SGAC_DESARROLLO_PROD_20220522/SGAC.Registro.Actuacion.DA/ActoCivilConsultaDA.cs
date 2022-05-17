using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using System.Collections.Generic;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoCivilConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoCivilConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoCivilConsultaDA()
        {
            GC.Collect();
        }

        public RE_REGISTROCIVIL USP_REGISTROCIVIL_OBTENER_ID(Int64 iActuacionDetalleID) {

            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_REGISTROCIVIL_OBTENER_ID]", cnn))
                    {
                        cnn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = iActuacionDetalleID;


                        List<RE_REGISTROCIVIL> l_RE_REGISTROCIVIL = new List<RE_REGISTROCIVIL>();
                        RE_REGISTROCIVIL obj = null;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {

                                while (dr.Read())
                                {
                                    obj = new RE_REGISTROCIVIL();
                                    if (!dr.IsDBNull(0))
                                        obj.reci_iRegistroCivilId = dr.GetInt64(0);
                                    if (!dr.IsDBNull(1))
                                        obj.reci_iActuacionDetalleId = dr.GetInt64(1);
                                    if (!dr.IsDBNull(2))
                                        obj.reci_sTipoActaId = dr.GetInt16(2);
                                    if (!dr.IsDBNull(3))
                                        obj.reci_vNumeroCUI = dr.GetString(3);
                                    if (!dr.IsDBNull(4))
                                        obj.reci_vNumeroActa = dr.GetString(4);
                                    if (!dr.IsDBNull(5))
                                        obj.reci_dFechaRegistro = dr.GetDateTime(5);
                                    if (!dr.IsDBNull(6))
                                        obj.reci_cOficinaRegistralUbigeo = dr.GetString(6);
                                    if (!dr.IsDBNull(7))
                                        obj.reci_IOficinaRegistralCentroPobladoId = dr.GetInt32(7);
                                    if (!dr.IsDBNull(8))
                                        obj.reci_dFechaHoraOcurrenciaActo = dr.GetDateTime(8);
                                    if (!dr.IsDBNull(9))
                                        obj.reci_sOcurrenciaTipoId = dr.GetInt16(9);
                                    if (!dr.IsDBNull(10))
                                        obj.reci_vOcurrenciaLugar = dr.GetString(10);
                                    if (!dr.IsDBNull(11))
                                        obj.reci_cOcurrenciaUbigeo = dr.GetString(11);
                                    if (!dr.IsDBNull(12))
                                        obj.reci_IOcurrenciaCentroPobladoId = dr.GetInt32(12);
                                    if (!dr.IsDBNull(13))
                                        obj.reci_vNumeroExpedienteMatrimonio = dr.GetString(13);
                                    if (!dr.IsDBNull(14))
                                        obj.reci_IAprobacionUsuarioId = dr.GetInt32(14);
                                    if (!dr.IsDBNull(15))
                                        obj.reci_vIPAprobacion = dr.GetString(15);
                                    if (!dr.IsDBNull(16))
                                        obj.reci_dFechaAprobacion = dr.GetDateTime(16);
                                    if (!dr.IsDBNull(17))
                                        obj.reci_bDigitalizadoFlag = dr.GetBoolean(17);
                                    if (!dr.IsDBNull(18))
                                        obj.reci_vCargoCelebrante = dr.GetString(18);
                                    if (!dr.IsDBNull(19))
                                        obj.reci_vLibro = dr.GetString(19);
                                    if (!dr.IsDBNull(20))
                                        obj.reci_bAnotacionFlag = dr.GetBoolean(20);
                                    if (!dr.IsDBNull(21))
                                        obj.reci_vObservaciones = dr.GetString(21);
                                    if (!dr.IsDBNull(22))
                                        obj.reci_cEstado = dr.GetString(22);
                                }
                                return obj;
                            }
                            else
                            {
                                return null;
                            }
                        }

                    }
                }
            }
            catch (Exception ex) {
                throw ex;                 
            }            
        }

        public List<RE_PARTICIPANTE> USP_RE_REGISTROCIVIL_PARTICIPANTE_OBTENER(Int64 sActuaciionDetalleID)
        {

            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_REGISTROCIVIL_PARTICIPANTE_OBTENER]", cnn))
                    {
                        cnn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = sActuaciionDetalleID;


                        List<RE_PARTICIPANTE> l_RE_PARTICIPANTE = new List<RE_PARTICIPANTE>();
                        RE_PARTICIPANTE obj = null;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            if (dr.HasRows)
                            {

                                while (dr.Read())
                                {
 
		 
                                    obj = new RE_PARTICIPANTE();
                                    if (!dr.IsDBNull(0))
                                        obj.iActuacionDetId = dr.GetInt64(0);
                                    if (!dr.IsDBNull(1))
                                        obj.iPersonaId = dr.GetInt64(1);
                                    if (!dr.IsDBNull(2))
                                        obj.vPrimerApellido = dr.GetString(2);
                                    if (!dr.IsDBNull(3))
                                        obj.vSegundoApellido = dr.GetString(3);
                                    if (!dr.IsDBNull(4))
                                        obj.vNombres = dr.GetString(4);
                                    if (!dr.IsDBNull(5))
                                        obj.sTipoParticipanteId = dr.GetInt16(5);
                                    if (!dr.IsDBNull(6))
                                        obj.vTipoParticipante = dr.GetString(6);
                                    if (!dr.IsDBNull(7))
                                        obj.sTipoDocumentoId = dr.GetInt16(7);
                                    if (!dr.IsDBNull(8))
                                        obj.vTipoDocumento = dr.GetString(8);
                                    if (!dr.IsDBNull(9))
                                        obj.vNumeroDocumento = dr.GetString(9);
                                    if (!dr.IsDBNull(10))
                                        obj.vNumeroDocumentoCompleto = dr.GetString(10);
                                    if (!dr.IsDBNull(11))
                                        obj.sTipoDatoId = dr.GetInt16(11);
                                    if (!dr.IsDBNull(12))
                                        obj.sTipoVinculoId = dr.GetInt16(12);
                                    if (!dr.IsDBNull(13))
                                        obj.vNombresCompletos = dr.GetString(13);
                                    if (!dr.IsDBNull(14))
                                        obj.sNacionalidadId = dr.GetInt16(14);
                                    if (!dr.IsDBNull(15))
                                        obj.sGeneroId = dr.GetInt16(15);
                                    if (!dr.IsDBNull(16))
                                        obj.pers_cNacimientoLugar = dr.GetString(16);
                                    if (!dr.IsDBNull(17))
                                        obj.pers_dNacimientoFecha = dr.GetDateTime(17);
                                    if (!dr.IsDBNull(18))
                                        obj.pers_bFallecidoFlag = dr.GetBoolean(18);
                                    if (!dr.IsDBNull(19))
                                        obj.pers_cUbigeoDefuncion = dr.GetString(19);
                                    if (!dr.IsDBNull(20))
                                        obj.pers_dFechaDefuncion = dr.GetDateTime(20);
                                    if (!dr.IsDBNull(21))
                                        obj.vUbigeo = dr.GetString(21);
                                    if (!dr.IsDBNull(22))
                                        obj.vDireccion = dr.GetString(22);
                                    if (!dr.IsDBNull(23))
                                        obj.pers_sEstadoCivilId = dr.GetInt16(23);
                                    if (!dr.IsDBNull(24))
                                        obj.cEstado = dr.GetString(24);
                                    if (!dr.IsDBNull(25))
                                        obj.Item = dr.GetInt64(25);

                                    l_RE_PARTICIPANTE.Add(obj);
                                }
                                return l_RE_PARTICIPANTE;
                            }
                            else
                            {
                                return null;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        /*FIN*/




        /// <summary>
        /// 
        /// </summary>
        /// <param name="LonActuacionDetalleId"></param>
        /// <param name="StrCurrentPage"></param>
        /// <param name="IntPageSize"></param>
        /// <param name="IntTotalCount"></param>
        /// <param name="IntTotalPages"></param>
        /// <returns></returns>
        public DataTable Consultar(long LonActuacionDetalleId,
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
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = Convert.ToInt32(StrCurrentPage);
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = IntPageSize;


                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.Int);
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
            //    SqlParameter[] prmParameter = new SqlParameter[5];

            //    prmParameter[0] = new SqlParameter("@reci_iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonActuacionDetalleId;

            //    prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
            //    prmParameter[1].Value = StrCurrentPage;

            //    prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
            //    prmParameter[2].Value = IntPageSize;

            //    prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
            //    prmParameter[3].Direction = ParameterDirection.Output;

            //    prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
            //    prmParameter[4].Direction = ParameterDirection.Output;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_CONSULTAR",
            //                                         prmParameter);

            //    DtResult = DsResult.Tables[0];

            //    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value);
            //    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable Obtener(long? LonRegCivId = null, long? LonRegCivIdDet = null)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_ID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

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
            //    SqlParameter[] prmParameter = new SqlParameter[2];

            //    prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonRegCivId;

            //    prmParameter[1] = new SqlParameter("@reci_iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[1].Value = LonRegCivIdDet;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_ID",
            //                                         prmParameter);

            //    DtResult = DsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ObtenerPorCUI(string strNumeroCUI)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_NUMCUI", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar, 20).Value = strNumeroCUI;

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

            //    prmParameter[0] = new SqlParameter("@reci_vNumeroCUI", SqlDbType.VarChar, 20);
            //    prmParameter[0].Value = strNumeroCUI;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_NUMCUI",
            //                                         prmParameter);

            //    DtResult = DsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataSet ObtenerDatosCivil(long? LonRegCivId = null, long? LonRegCivIdDet = null)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_ID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt).Value = LonRegCivId;
                        cmd.Parameters.Add("@reci_iActuacionDetalleId", SqlDbType.BigInt).Value = LonRegCivIdDet;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

                    }
                }

                return dsResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet DsResult = new DataSet();
            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[2];

            //    prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonRegCivId;

            //    prmParameter[1] = new SqlParameter("@reci_iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[1].Value = LonRegCivIdDet;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_OBTENER_POR_ID",
            //                                         prmParameter);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DsResult;
            #endregion
        }

        public DataTable Imprimir_Acta(Int64 iActuacionDetalleId)
        {
            using (SqlConnection cn = new SqlConnection(StrConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_REGISTROCIVIL_FORMATO", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.Int).Value = iActuacionDetalleId;
                    da.Fill(ds, "Acta");
                    dt = ds.Tables["Acta"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 11/11/2019
        // Objetivo: Se ha comentado método sin uso.
        //------------------------------------------------------------------------
        #region Comentada
        //public DataTable Consultar_Acta(Int64 iActuacionDetalleId)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[1];

        //        prmParameter[0] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = iActuacionDetalleId;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_REGISTROCIVIL_FORMATO",
        //                                             prmParameter);
        //        DtResult = DsResult.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DtResult;
        //}
        #endregion
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable Consultar_Formatos(long LonActuacionDetalleId, long LonRegistroCivilId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_FORMATO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt).Value = LonRegistroCivilId;
                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;

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
            //    SqlParameter[] prmParameter = new SqlParameter[2];

            //    prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
            //    prmParameter[0].Value = LonRegistroCivilId;

            //    prmParameter[1] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
            //    prmParameter[1].Value = LonActuacionDetalleId;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_FORMATO",
            //                                         prmParameter);

            //    DtResult = DsResult.Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable Consultar_Actas_Titulares(int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages, 
            int intRegistroCivilId = 0, string strNumeroActa = "", string strApPaterno = "", string strApMaterno = "", string strNombres = "", 
            Int16 intTipoActaId=0)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROCIVIL_CONSULTAR_ACTAS_TITULARES_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@reci_iRegistroCivilId", SqlDbType.BigInt).Value = intRegistroCivilId;
                        cmd.Parameters.Add("@reci_vNumeroActa", SqlDbType.VarChar, 10).Value = strNumeroActa;
                        cmd.Parameters.Add("@pers_vApellidoPaterno", SqlDbType.VarChar, 100).Value = strApPaterno;
                        cmd.Parameters.Add("@pers_vApellidoMaterno", SqlDbType.VarChar, 100).Value = strApMaterno;
                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 100).Value = strNombres;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = intCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;


                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@reci_sTipoActaId", SqlDbType.SmallInt).Value = intTipoActaId;

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
            //    SqlParameter[] prmParameter = new SqlParameter[10];

            //    prmParameter[0] = new SqlParameter("@reci_iRegistroCivilId", SqlDbType.BigInt);
            //    prmParameter[0].Value = intRegistroCivilId;

            //    prmParameter[1] = new SqlParameter("@reci_vNumeroActa", SqlDbType.VarChar, 10);
            //    prmParameter[1].Value = strNumeroActa;

            //    prmParameter[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.VarChar, 100);
            //    prmParameter[2].Value = strApPaterno;

            //    prmParameter[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.VarChar, 100);
            //    prmParameter[3].Value = strApMaterno;

            //    prmParameter[4] = new SqlParameter("@pers_vNombres", SqlDbType.VarChar, 100);
            //    prmParameter[4].Value = strNombres;

            //    prmParameter[5] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
            //    prmParameter[5].Value = intCurrentPage;

            //    prmParameter[6] = new SqlParameter("@IPageSize", SqlDbType.Int);
            //    prmParameter[6].Value = IntPageSize;

            //    prmParameter[7] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
            //    prmParameter[7].Direction = ParameterDirection.Output;

            //    prmParameter[8] = new SqlParameter("@ITotalPages", SqlDbType.Int);
            //    prmParameter[8].Direction = ParameterDirection.Output;

            //    prmParameter[9] = new SqlParameter("@reci_sTipoActaId", SqlDbType.SmallInt);
            //    prmParameter[9].Value = intTipoActaId;

            //    DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
            //                                         CommandType.StoredProcedure,
            //                                        "PN_REGISTRO.USP_RE_REGISTROCIVIL_CONSULTAR_ACTAS_TITULARES_MRE",
            //                                         prmParameter);

            //    DtResult = DsResult.Tables[0];
            //    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);
            //    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[8]).Value);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //return DtResult;

            #endregion
        }




        public Int32 ExisteCui(String reci_vNumeroCUI, Int64 peid_iPersonaId, Int16 IOperacion,Int16 Usuario,String IP,Int16 iOficinaConsularID) {
            Int32 Respuesta = 1;
            String strHostName = Util.ObtenerHostName();
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_CUI_EXISTE]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@reci_vNumeroCUI", SqlDbType.VarChar).Value = reci_vNumeroCUI;
                        cmd.Parameters.Add("@peid_iPersonaId", SqlDbType.BigInt).Value = peid_iPersonaId;

                        cmd.Parameters.Add("@sUsuarioModificacion", SqlDbType.SmallInt).Value = Usuario;
                        cmd.Parameters.Add("@vIPModificacion", SqlDbType.VarChar, 50).Value = IP;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = iOficinaConsularID;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = strHostName;

                        

                        cmd.Parameters.Add("@IOperacion", SqlDbType.SmallInt).Value = IOperacion;



                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Respuesta = Convert.ToInt32(lReturn.Value);
                        return Respuesta;

                    }
                }
            }
            catch (Exception ex) {
                return 1;
            }
        }
    }
}