using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Almacen.DA
{
    public class MovimientoConsultaDA
    {
        private string strConnectionName = string.Empty;

        public MovimientoConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~MovimientoConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR

        //public DataTable LeerRegistro(int movi_iMovimientoId)
        //{
        //    DataSet dsResult;
        //    DataTable dtResult;

        //    SqlParameter[] prmParameter = new SqlParameter[1];
        //    prmParameter[0] = new SqlParameter("@movi_iMovimientoId", SqlDbType.BigInt);
        //    prmParameter[0].Value = movi_iMovimientoId;

        //    dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_LEER", prmParameter);
        //    dtResult = dsResult.Tables[0];

        //    return dtResult;
        //}

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable Consultar(int intOficinaConsularId, DateTime datFechaInicio, DateTime datFechaFin,
                                    int intTipoInsumo, int movi_IEstadoId, string movi_cPedidoCodigo,
                                    int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
                                    int intOficinaConsularOrigenId, int intBovedaTipoOrigenId, int intBovedaOrigenId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;
                        cmd.Parameters.Add("@movi_sEstadoId", SqlDbType.SmallInt).Value = movi_IEstadoId;
                        cmd.Parameters.Add("@movi_cPedidoCodigo", SqlDbType.VarChar, 12).Value = movi_cPedidoCodigo.ToString();
                        cmd.Parameters.Add("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = intOficinaConsularOrigenId;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = intBovedaTipoOrigenId;
                        cmd.Parameters.Add("@movi_sBodegaOrigenId", SqlDbType.SmallInt).Value = intBovedaOrigenId;
                        cmd.Parameters.Add("@movi_dFechaInicio", SqlDbType.DateTime).Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
                        cmd.Parameters.Add("@movi_dFechaFin", SqlDbType.DateTime).Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;

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
                                intTotalRegistros = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lMovimientoIdReturn2.Value.ToString());
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

            //SqlParameter[] prmParameter = new SqlParameter[13];
            //prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //prmParameter[0].Value = intOficinaConsularId;
            //prmParameter[1] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //prmParameter[1].Value = intTipoInsumo;
            //prmParameter[2] = new SqlParameter("@movi_sEstadoId", SqlDbType.SmallInt);
            //prmParameter[2].Value = movi_IEstadoId;
            //prmParameter[3] = new SqlParameter("@movi_cPedidoCodigo", SqlDbType.VarChar);
            //prmParameter[3].Value = movi_cPedidoCodigo.ToString();
            //prmParameter[4] = new SqlParameter("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt);
            //prmParameter[4].Value = intOficinaConsularOrigenId;
            //prmParameter[5] = new SqlParameter("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt);
            //prmParameter[5].Value = intBovedaTipoOrigenId;
            //prmParameter[6] = new SqlParameter("@movi_sBodegaOrigenId", SqlDbType.SmallInt);
            //prmParameter[6].Value = intBovedaOrigenId;
            //prmParameter[7] = new SqlParameter("@movi_dFechaInicio", SqlDbType.DateTime);
            //prmParameter[7].Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            //prmParameter[8] = new SqlParameter("@movi_dFechaFin", SqlDbType.DateTime);
            //prmParameter[8].Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");
            //prmParameter[9] = new SqlParameter("@IPaginaActual", SqlDbType.Int);
            //prmParameter[9].Value = intPaginaActual;
            //prmParameter[10] = new SqlParameter("@IPaginaCantidad", SqlDbType.Int);
            //prmParameter[10].Value = intPaginaCantidad;
            //prmParameter[11] = new SqlParameter("@ITotalRegistros", SqlDbType.Int);
            //prmParameter[11].Direction = ParameterDirection.Output;
            //prmParameter[12] = new SqlParameter("@ITotalPaginas", SqlDbType.Int);
            //prmParameter[12].Direction = ParameterDirection.Output;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_CONSULTAR", prmParameter);
            //dtResult = dsResult.Tables[0];

            //intTotalRegistros = Convert.ToInt32(((SqlParameter)prmParameter[11]).Value);
            //intTotalPaginas = Convert.ToInt32(((SqlParameter)prmParameter[12]).Value);
            #endregion
            
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable MovimientoMotivo(int intMovimientoMotivo, int intOficinaConsularId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_MOTIVO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@para_sParametroId", SqlDbType.SmallInt).Value = intMovimientoMotivo;
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;

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
            //SqlParameter[] prmParameter = new SqlParameter[2];
            //prmParameter[0] = new SqlParameter("@para_sParametroId", SqlDbType.SmallInt);
            //prmParameter[0].Value = intMovimientoMotivo;
            //prmParameter[1] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //prmParameter[1].Value = intOficinaConsularId;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_MOTIVO", prmParameter);
            //dtResult = dsResult.Tables[0];

            //return dtResult;
            #endregion
        }


        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ConsultarStock(int intTipoInsumo, DateTime datFechaInicio, DateTime datFechaFin,
                                        int intOficinaConsularIdOrigen, int intBovedaTipoIdOrigen, int intBodegaOrigenId,
                                        int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
                                        ref string strMensaje)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_STOCK", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;
                        cmd.Parameters.Add("@movi_dFechaInicio", SqlDbType.DateTime).Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
                        cmd.Parameters.Add("@movi_dFechaFin", SqlDbType.DateTime).Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@movi_sBovedaTipoId", SqlDbType.SmallInt).Value = intBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@movi_sBovedaId", SqlDbType.SmallInt).Value = intBodegaOrigenId;
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;

                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn3 = cmd.Parameters.Add("@strMensaje", SqlDbType.VarChar, 150);
                        lMovimientoIdReturn3.Direction = ParameterDirection.Output;


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
                                intTotalRegistros = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }

                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lMovimientoIdReturn2.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn3.Value != null)
                        {
                            if (lMovimientoIdReturn3.Value.ToString().Trim() != string.Empty)
                            {
                                strMensaje = lMovimientoIdReturn3.Value.ToString();
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
            //DataSet dsResult;
            //DataTable dtResult;

            //SqlParameter[] prmParameter = new SqlParameter[11];
            //prmParameter[0] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //prmParameter[0].Value = intTipoInsumo;
            //prmParameter[1] = new SqlParameter("@movi_dFechaInicio", SqlDbType.DateTime);
            //prmParameter[1].Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            //prmParameter[2] = new SqlParameter("@movi_dFechaFin", SqlDbType.DateTime);
            //prmParameter[2].Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");


            //prmParameter[3] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //prmParameter[3].Value = intOficinaConsularIdOrigen;

            //prmParameter[4] = new SqlParameter("@movi_sBovedaTipoId", SqlDbType.SmallInt);
            //prmParameter[4].Value = intBovedaTipoIdOrigen;
            //prmParameter[5] = new SqlParameter("@movi_sBovedaId", SqlDbType.SmallInt);
            //prmParameter[5].Value = intBodegaOrigenId;
            //prmParameter[6] = new SqlParameter("@IPaginaActual", SqlDbType.Int);
            //prmParameter[6].Value = intPaginaActual;

            //prmParameter[7] = new SqlParameter("@IPaginaCantidad", SqlDbType.Int);
            //prmParameter[7].Value = intPaginaCantidad;

            //prmParameter[8] = new SqlParameter("@ITotalRegistros", SqlDbType.Int);
            //prmParameter[8].Direction = ParameterDirection.Output;

            //prmParameter[9] = new SqlParameter("@ITotalPaginas", SqlDbType.Int);
            //prmParameter[9].Direction = ParameterDirection.Output;

            //prmParameter[10] = new SqlParameter("@strMensaje", SqlDbType.VarChar, 200);
            //prmParameter[10].Direction = ParameterDirection.Output;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_STOCK", prmParameter);
            //dtResult = dsResult.Tables[0];

            //intTotalRegistros = Convert.ToInt32(((SqlParameter)prmParameter[8]).Value);
            //intTotalPaginas = Convert.ToInt32(((SqlParameter)prmParameter[9]).Value);
            //strMensaje = (string)prmParameter[10].Value.ToString();

            //return dtResult;
            #endregion
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ConsultarMesAnterior(int intOficinaConsularId, int intTipoInsumo)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_STOCK_MESANTERIOR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;


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
            //DataSet dsResult;
            //DataTable dtResult;

            //SqlParameter[] prmParameter = new SqlParameter[11];
            //prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //prmParameter[0].Value = intOficinaConsularId;
            //prmParameter[1] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //prmParameter[1].Value = intTipoInsumo;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_STOCK_MESANTERIOR", prmParameter);
            //dtResult = dsResult.Tables[0];

            //return dtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------
        public DataTable ValidaRangosDetalle(int intOficinaConsularId, int intMotivoId, int intTipoInsumo,
                                                int intRangoInicial, int intRangoFinal, ref int intResultado, ref string strMensaje,
                                                int intTipoBovedaId, int intBovedaId)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_CONSULTAR_RANGOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@movi_sMovimientoMotivoId", SqlDbType.SmallInt).Value = intMotivoId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;

                        cmd.Parameters.Add("@intRangoInicial", SqlDbType.Int).Value = intRangoInicial;
                        cmd.Parameters.Add("@intRangoFinal", SqlDbType.Int).Value = intRangoFinal;

                        cmd.Parameters.Add("@movi_sTipoBovedaId", SqlDbType.SmallInt).Value = intTipoBovedaId;
                        cmd.Parameters.Add("@movi_sBovedaId", SqlDbType.SmallInt).Value = intBovedaId;


                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@intResultado", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@strMensaje", SqlDbType.VarChar, 200);
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
                                intResultado = Convert.ToInt32(lMovimientoIdReturn1.Value.ToString());
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                strMensaje = lMovimientoIdReturn2.Value.ToString();
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
            //DataSet dsResult;
            //DataTable dtResult;

            //try
            //{
            //    SqlParameter[] prmParameter = new SqlParameter[9];
            //    prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            //    prmParameter[0].Value = intOficinaConsularId;
            //    prmParameter[1] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //    prmParameter[1].Value = intTipoInsumo;
            //    prmParameter[2] = new SqlParameter("@intRangoInicial", SqlDbType.Int);
            //    prmParameter[2].Value = intRangoInicial;
            //    prmParameter[3] = new SqlParameter("@intRangoFinal", SqlDbType.Int);
            //    prmParameter[3].Value = intRangoFinal;
            //    prmParameter[4] = new SqlParameter("@intResultado", SqlDbType.Int);
            //    prmParameter[4].Direction = ParameterDirection.Output;
            //    prmParameter[5] = new SqlParameter("@strMensaje", SqlDbType.VarChar, 200);
            //    prmParameter[5].Direction = ParameterDirection.Output;
            //    prmParameter[6] = new SqlParameter("@movi_sMovimientoMotivoId", SqlDbType.SmallInt);
            //    prmParameter[6].Value = intMotivoId;
            //    prmParameter[7] = new SqlParameter("@movi_sTipoBovedaId", SqlDbType.SmallInt);
            //    prmParameter[7].Value = intTipoBovedaId;
            //    prmParameter[8] = new SqlParameter("@movi_sBovedaId", SqlDbType.SmallInt);
            //    prmParameter[8].Value = intBovedaId;

            //    dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure,
            //        "PN_ALMACEN.USP_AL_MOVIMIENTO_CONSULTAR_RANGOS", prmParameter);

            //    intResultado = (Int32)prmParameter[4].Value;
            //    strMensaje = (string)prmParameter[5].Value.ToString();

            //    dtResult = dsResult.Tables[0];
            //    return dtResult;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 07/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------

        public DataTable ConsultaRangosDisponibles(int intOficinaConsularId, int intBovedaTipoIdOrigen, int intBodegaOrigenId, int intTipoInsumo)
        {
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_RANGOS_DISPONIBLES", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = intBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@movi_sBodegaOrigenId", SqlDbType.SmallInt).Value = intBodegaOrigenId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;

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


            //SqlParameter[] prmParameter = new SqlParameter[4];
            //prmParameter[0] = new SqlParameter("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt);
            //prmParameter[0].Value = intOficinaConsularId;
            //prmParameter[1] = new SqlParameter("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt);
            //prmParameter[1].Value = intBovedaTipoIdOrigen;
            //prmParameter[2] = new SqlParameter("@movi_sBodegaOrigenId", SqlDbType.SmallInt);
            //prmParameter[2].Value = intBodegaOrigenId;
            //prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            //prmParameter[3].Value = intTipoInsumo;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTO_RANGOS_DISPONIBLES", prmParameter);
            //dtResult = dsResult.Tables[0];

            return DtResult;
        }

        #endregion Método CONSULTAR
    }
}