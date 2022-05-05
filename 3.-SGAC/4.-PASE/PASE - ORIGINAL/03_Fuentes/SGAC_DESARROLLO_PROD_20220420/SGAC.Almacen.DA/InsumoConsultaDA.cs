using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class InsumoConsultaDA
    {
        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }


        ~InsumoConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR

        public DataTable Consultar(int intOficinaConsularId, int intBodegaTipoId, int intBodegaId,
                                   string insu_vMovimientoCod,
                                   DateTime datFechaInicio,
                                   DateTime datFechaFin,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas,
                                    int intTipoInsumo, string strCodigoFabrica, int intEstado)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@insu_vMovimientoCod", SqlDbType.VarChar, 12).Value = insu_vMovimientoCod;
                        cmd.Parameters.Add("@insu_dFechaInicio", SqlDbType.DateTime).Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
                        cmd.Parameters.Add("@insu_dFechaFin", SqlDbType.DateTime).Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");
                        cmd.Parameters.Add("@insu_sEstadoId", SqlDbType.SmallInt).Value = intEstado;
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;

                        cmd.Parameters.Add("@insu_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;
                        cmd.Parameters.Add("@insu_vCodigoUnicoFabrica", SqlDbType.VarChar, 12).Value = strCodigoFabrica;
                        cmd.Parameters.Add("@insu_sBovedaTipoId", SqlDbType.SmallInt).Value = intBodegaTipoId;
                        cmd.Parameters.Add("@insu_sBodegaId", SqlDbType.SmallInt).Value = intBodegaId;

                        SqlParameter lTotalRegistrosReturn = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        lTotalRegistrosReturn.Direction = ParameterDirection.Output;

                        SqlParameter lTotalPaginasReturn = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        lTotalPaginasReturn.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();


                        if (lTotalRegistrosReturn.Value != null)
                        {
                            if (lTotalRegistrosReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalRegistros = Convert.ToInt32(lTotalRegistrosReturn.Value);
                            }
                        }

                        if (lTotalPaginasReturn.Value != null)
                        {
                            if (lTotalPaginasReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lTotalPaginasReturn.Value);
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
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 02/09/2016
        // Objetivo: Consulta la fecha de registro por el id del insumo
        //------------------------------------------------------------------------

        public DataTable ConsultarFechaRegistro_por_IdInsumo(int intInsumoId)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_ACTUACION_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@INSU_IINSUMOID", SqlDbType.BigInt).Value = intInsumoId;

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

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 04/11/2019
        // Objetivo: Consulta de Boveda
        //------------------------------------------------------------------------

        public DataTable ConsultarBovedas()
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_BOVEDA_CONSULTAR", cn))
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
        }
        
        
        public DataTable ConsultarHistorico(int intNumInsumo, ref string strMovimientoCodigo)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMOHISTORICO_CONSULTAR", cn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add("@inhi_iInsumoId", SqlDbType.BigInt).Value = intNumInsumo;

                        SqlParameter lcMovimientoCodgoReturn = cmd.Parameters.Add("@movi_cMovimientoCodigo", SqlDbType.Char, 12);
                        lcMovimientoCodgoReturn.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();


                        if (lcMovimientoCodgoReturn.Value != null)
                        {
                            if (lcMovimientoCodgoReturn.Value.ToString().Trim() != string.Empty)
                            {
                                strMovimientoCodigo = lcMovimientoCodgoReturn.Value.ToString();
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


        }
        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 17/02/2017
        // Objetivo: Consulta de insumos sin fecha
        //------------------------------------------------------------------------
        public DataTable ConsultarSinFecha(int intOficinaConsularId, int intBodegaTipoId, int intBodegaId,
                                   string insu_vMovimientoCod,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas,
                                    int intTipoInsumo, string strCodigoFabrica, int intEstado)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@insu_vMovimientoCod", SqlDbType.VarChar, 12).Value = insu_vMovimientoCod;
                        cmd.Parameters.Add("@insu_sEstadoId", SqlDbType.SmallInt).Value = intEstado;
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;

                        cmd.Parameters.Add("@insu_sInsumoTipoId", SqlDbType.SmallInt).Value = intTipoInsumo;
                        cmd.Parameters.Add("@insu_vCodigoUnicoFabrica", SqlDbType.VarChar, 12).Value = strCodigoFabrica;
                        cmd.Parameters.Add("@insu_sBovedaTipoId", SqlDbType.SmallInt).Value = intBodegaTipoId;
                        cmd.Parameters.Add("@insu_sBodegaId", SqlDbType.SmallInt).Value = intBodegaId;

                        SqlParameter lTotalRegistrosReturn = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        lTotalRegistrosReturn.Direction = ParameterDirection.Output;

                        SqlParameter lTotalPaginasReturn = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        lTotalPaginasReturn.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();


                        if (lTotalRegistrosReturn.Value != null)
                        {
                            if (lTotalRegistrosReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalRegistros = Convert.ToInt32(lTotalRegistrosReturn.Value);
                            }
                        }

                        if (lTotalPaginasReturn.Value != null)
                        {
                            if (lTotalPaginasReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lTotalPaginasReturn.Value);
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
        }








        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 17/02/2017
        // Objetivo: Consulta de insumos por rangos
        //------------------------------------------------------------------------
        public DataTable ConsultarPorRangos(int intOficinaConsularId, string iRangoInicial, string iRangoFinal,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_CONSULTAR_RANGOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@mode_iRangoInicial", SqlDbType.VarChar,20).Value = iRangoInicial;
                        cmd.Parameters.Add("@mode_iRangoFinal", SqlDbType.VarChar, 20).Value = iRangoFinal;
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;


                        SqlParameter lTotalRegistrosReturn = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        lTotalRegistrosReturn.Direction = ParameterDirection.Output;

                        SqlParameter lTotalPaginasReturn = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        lTotalPaginasReturn.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();


                        if (lTotalRegistrosReturn.Value != null)
                        {
                            if (lTotalRegistrosReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalRegistros = Convert.ToInt32(lTotalRegistrosReturn.Value);
                            }
                        }

                        if (lTotalPaginasReturn.Value != null)
                        {
                            if (lTotalPaginasReturn.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lTotalPaginasReturn.Value);
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
        }


        public DataTable ConsultarUltimoInsumoUsuario(Int16 intConsulado,Int16 intUsuario)
        {

            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_CONSULTAR_PRIMER_INSUMO_DISPONIBLE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@p_sOficinaConsularId", SqlDbType.SmallInt).Value = intConsulado;
                        cmd.Parameters.Add("@p_sUsuarioId", SqlDbType.SmallInt).Value = intUsuario;

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
        #endregion Método CONSULTAR
    }
}