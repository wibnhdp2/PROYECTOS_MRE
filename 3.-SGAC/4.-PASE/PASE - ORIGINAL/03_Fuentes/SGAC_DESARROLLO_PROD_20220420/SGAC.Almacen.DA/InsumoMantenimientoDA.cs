using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class InsumoMantenimientoDA
    {
        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public string strError;

        ~InsumoMantenimientoDA()
        {
            GC.Collect();
        }

        #region Método ADICIONAR

        public int InsumoAdicionar(BE.AL_INSUMO objBE, int intMovimientoId, int intRangoInicial, int intRangoFinal)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //int iNumberRowsDet1OK = 0, iNumberRowsDet2OK = 0;

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_IInsumoTipoId", SqlDbType.SmallInt).Value = objBE.insu_sInsumoTipoId;
                        cmd.Parameters.Add("@insu_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.insu_vPrefijoNumeracion;
                        cmd.Parameters.Add("@insu_vCodigoUnicoFabrica", SqlDbType.VarChar, 12).Value = objBE.insu_vCodigoUnicoFabrica;
                        cmd.Parameters.Add("@insu_dFecharegistro", SqlDbType.DateTime).Value = objBE.insu_dFechaRegistro;
                        cmd.Parameters.Add("@insu_sEstadoId", SqlDbType.SmallInt).Value = objBE.insu_sEstadoId;

                        cmd.Parameters.Add("@insu_sUsuarioCreacion", SqlDbType.SmallInt).Value = objBE.insu_sUsuarioCreacion;
                        cmd.Parameters.Add("@insu_IOficinaConsularId", SqlDbType.SmallInt).Value = objBE.AL_MOVIMIENTO.movi_sOficinaConsularIdOrigen;

                        cmd.Parameters.Add("@insu_iMovimientoId", SqlDbType.BigInt).Value = objBE.AL_MOVIMIENTO.movi_iMovimientoId;
                        cmd.Parameters.Add("@insu_IRangoInicial", SqlDbType.Int).Value = intRangoInicial;

                        cmd.Parameters.Add("@insu_IRangoFinal", SqlDbType.Int).Value = intRangoFinal;
                        cmd.Parameters.Add("@insu_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@insu_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.insu_vIPModificacion;



                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();


                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }

        #endregion Método ADICIONAR

        #region Método ACTUALIZAR

        public int InsumoActualizar(BE.AL_INSUMO objBE)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            //int iNumberRowsDet1OK = 0, iNumberRowsDet2OK = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_IInsumoTipoId", SqlDbType.SmallInt).Value = objBE.insu_sInsumoTipoId;
                        cmd.Parameters.Add("@insu_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.insu_vPrefijoNumeracion;
                        cmd.Parameters.Add("@insu_vCodigoUnicoFabrica", SqlDbType.VarChar, 12).Value = objBE.insu_vCodigoUnicoFabrica;
                        cmd.Parameters.Add("@insu_dFecharegistro", SqlDbType.DateTime).Value = objBE.insu_dFechaRegistro;
                        cmd.Parameters.Add("@insu_sEstadoId", SqlDbType.SmallInt).Value = objBE.insu_sEstadoId;

                        cmd.Parameters.Add("@insu_dFechaModificacion", SqlDbType.DateTime).Value = objBE.insu_dFechaModificacion;

                        cmd.Parameters.Add("@insu_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.insu_sUsuarioModificacion;
                     
                        cmd.Parameters.Add("@insu_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@insu_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.insu_vIPModificacion;

                        cmd.Parameters.Add("@insu_iMovimientoId", SqlDbType.BigInt).Value = objBE.insu_iMovimientoId;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();


                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
            
        }

        #endregion Método ACTUALIZAR

        #region Método Dar de Baja

        public int InsumoDarDeBaja(BE.AL_INSUMO objBE)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);           

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_DAR_DE_BAJA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_IInsumoId", SqlDbType.BigInt).Value = objBE.insu_iInsumoId;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = objBE.OficinaConsularId;
                        cmd.Parameters.Add("@insu_vMotivoBaja", SqlDbType.VarChar, 350).Value = objBE.insu_vMotivoBaja;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@insu_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.insu_sUsuarioModificacion;
                        cmd.Parameters.Add("@insu_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();                      

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }

        #endregion Método ACTUALIZAR

        #region Método Recalcular Saldos Insumo
        public int RecalcularSaldosInsumo(Int16 OficinaConsultar, Int16 AnioInicial, Int16 MesInicial, Int16 AnioFinal, Int16 MesFinal, Int16 Usuario, bool SoloReiniciar)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_RECAULCULAR_SALDO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@P_OFCO_SOFICINACONSULARID", SqlDbType.SmallInt).Value = OficinaConsultar;
                        cmd.Parameters.Add("@P_PERIODO_INICIAL", SqlDbType.SmallInt).Value = AnioInicial;
                        cmd.Parameters.Add("@P_MES_INICIAL", SqlDbType.VarChar).Value = MesInicial;
                        cmd.Parameters.Add("@P_PERIODO_FINAL", SqlDbType.SmallInt).Value = AnioFinal;
                        cmd.Parameters.Add("@P_MES_FINAL", SqlDbType.SmallInt).Value = MesFinal;
                        cmd.Parameters.Add("@P_USUARIO", SqlDbType.SmallInt).Value = Usuario;
                        cmd.Parameters.Add("@P_VIPMODIFICACION", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@P_bReiniciar", SqlDbType.Bit).Value = SoloReiniciar;
                        

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }
        #endregion Método Recalcular Saldos Insumo




        public int ReactivarInsumos(Int16 OficinaConsultar, string iRangoInicial, string iRangoFinal, Int16 Usuario, string Observacion)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_INSUMO_REACTIVAR_RANGOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insu_sOficinaConsularId", SqlDbType.SmallInt).Value = OficinaConsultar;
                        cmd.Parameters.Add("@mode_iRangoInicial", SqlDbType.VarChar,20).Value = iRangoInicial;
                        cmd.Parameters.Add("@mode_iRangoFinal", SqlDbType.VarChar,20).Value = iRangoFinal;
                        cmd.Parameters.Add("@sUsuarioModificacion", SqlDbType.SmallInt).Value = Usuario;
                        cmd.Parameters.Add("@vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@vMotivo", SqlDbType.VarChar , 150).Value = Observacion;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }

        }
    }
}