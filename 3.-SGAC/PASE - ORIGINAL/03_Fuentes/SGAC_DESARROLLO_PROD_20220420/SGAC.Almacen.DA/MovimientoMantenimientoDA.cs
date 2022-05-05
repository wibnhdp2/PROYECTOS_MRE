using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class MovimientoMantenimientoDA
    {
        public string strError=string.Empty;

        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~MovimientoMantenimientoDA()
        {
            GC.Collect();
        }

        #region Método ADICIONAR

        public Int16 MovimientoAdicionar(ref BE.AL_MOVIMIENTO objBE, ref Nullable<Int16> iInsumoEstadoId)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@movi_sMovimientoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoTipoId;
                        cmd.Parameters.Add("@movi_sMovimientoMotivoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoMotivoId;
                        cmd.Parameters.Add("@movi_cMovimientoCodigo", SqlDbType.Char, 12).Value = objBE.movi_cMovimientoCodigo;
                        cmd.Parameters.Add("@movi_cPedidoCodigo", SqlDbType.Char, 12).Value = objBE.movi_cPedidoCodigo;
                        cmd.Parameters.Add("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = objBE.movi_sBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@movi_sBodegaOrigenId", SqlDbType.SmallInt).Value = objBE.movi_sBodegaOrigenId;
                        cmd.Parameters.Add("@movi_sOficinaConsularIdDestino", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdDestino;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdDestino", SqlDbType.SmallInt).Value = objBE.movi_sBovedaTipoIdDestino;
                        cmd.Parameters.Add("@movi_sBodegaDestinoId", SqlDbType.SmallInt).Value = objBE.movi_sBodegaDestinoId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sInsumoTipoId;
                        cmd.Parameters.Add("@movi_dFechaRegistro", SqlDbType.DateTime).Value = objBE.movi_dFechaRegistro;
                        cmd.Parameters.Add("@movi_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.movi_vPrefijoNumeracion;
                        cmd.Parameters.Add("@movi_vActaRemision", SqlDbType.VarChar, 12).Value = objBE.movi_vActaRemision;
                        cmd.Parameters.Add("@movi_sEstadoId", SqlDbType.SmallInt).Value = objBE.movi_sEstadoId;
                        cmd.Parameters.Add("@movi_sUsuarioCreacion", SqlDbType.SmallInt).Value = objBE.movi_sUsuarioCreacion;
                        cmd.Parameters.Add("@movi_vIPCreacion", SqlDbType.VarChar, 50).Value = objBE.movi_vIPCreacion;
                        cmd.Parameters.Add("@movi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@movi_dFechaValija", SqlDbType.DateTime).Value = objBE.movi_dFechaValija;

                        SqlParameter lMovimientoIdReturn = cmd.Parameters.Add("@movi_iMovimientoId", SqlDbType.BigInt);
                        lMovimientoIdReturn.Direction = ParameterDirection.Output;

                        SqlParameter lInusmoIdReturn = cmd.Parameters.Add("@insu_sInsumoEstadoId", SqlDbType.SmallInt);
                        lInusmoIdReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lMovimientoIdReturn.Value != null)
                        {
                            if (lMovimientoIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                objBE.movi_iMovimientoId = Convert.ToInt64(lMovimientoIdReturn.Value.ToString());
                            }
                        }

                        if (lInusmoIdReturn.Value != null)
                        {
                            if (lInusmoIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                iInsumoEstadoId = Convert.ToInt16(lInusmoIdReturn.Value.ToString());
                            }
                        }

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


        public void MovimientoDetalleAdicionar(BE.AL_MOVIMIENTODETALLE objBEDETALLE, BE.AL_MOVIMIENTO objBE, Nullable<Int16> iInsumoEstadoId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 1000;
                        cmd.Parameters.Add("@mode_iMovimientoId", SqlDbType.BigInt).Value = objBE.movi_iMovimientoId;
                        cmd.Parameters.Add("@mode_iActuacionId", SqlDbType.BigInt).Value = objBEDETALLE.mode_iActuacionId;
                        cmd.Parameters.Add("@mode_iActuacionDetalleId", SqlDbType.BigInt).Value = objBEDETALLE.mode_iActuacionDetalleId;
                        cmd.Parameters.Add("@mode_vRangoInicial", SqlDbType.VarChar, 10).Value = objBEDETALLE.mode_vRangoInicial;
                        cmd.Parameters.Add("@mode_vRangoFinal", SqlDbType.VarChar, 10).Value = objBEDETALLE.mode_vRangoFinal;
                        cmd.Parameters.Add("@mode_ICantidad", SqlDbType.Int).Value = objBEDETALLE.mode_ICantidad;
                        cmd.Parameters.Add("@mode_vObservacion", SqlDbType.VarChar, 150).Value = objBEDETALLE.mode_vObservacion;
                        cmd.Parameters.Add("@mode_sEstadoId", SqlDbType.SmallInt).Value = objBE.movi_sEstadoId;
                        cmd.Parameters.Add("@mode_sUsuarioCreacion", SqlDbType.SmallInt).Value = objBEDETALLE.mode_sUsuarioCreacion;
                        cmd.Parameters.Add("@mode_vIPCreacion", SqlDbType.VarChar, 50).Value = objBEDETALLE.mode_vIPCreacion;
                        cmd.Parameters.Add("@mode_sOficinaConsularId", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@mode_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Parameters.Add("@insu_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sInsumoTipoId;
                        cmd.Parameters.Add("@insu_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.movi_vPrefijoNumeracion;
                        cmd.Parameters.Add("@insu_dFecharegistro", SqlDbType.DateTime).Value = objBE.movi_dFechaRegistro;
                        cmd.Parameters.Add("@mode_sMovimientoMotivoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoMotivoId;

                        if (iInsumoEstadoId.HasValue)
                            cmd.Parameters.Add("@insu_sInsumoEstadoId", SqlDbType.SmallInt).Value = iInsumoEstadoId.Value;
                        else
                            cmd.Parameters.Add("@insu_sInsumoEstadoId", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                Error = true;
                throw ex;
            }
        }

        #endregion Método ADICIONAR

        #region Método ACTUALIZAR

        public Int16 MovimientoActualizar(ref BE.AL_MOVIMIENTO objBE, ref Nullable<Int16> iInsumoEstadoId)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@movi_sMovimientoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoTipoId;
                        cmd.Parameters.Add("@movi_sMovimientoMotivoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoMotivoId;
                        cmd.Parameters.Add("@movi_cMovimientoCodigo", SqlDbType.Char, 12).Value = objBE.movi_cMovimientoCodigo;
                        cmd.Parameters.Add("@movi_cPedidoCodigo", SqlDbType.Char, 12).Value = objBE.movi_cPedidoCodigo;
                        cmd.Parameters.Add("@movi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = objBE.movi_sBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@movi_sBodegaOrigenId", SqlDbType.SmallInt).Value = objBE.movi_sBodegaOrigenId;
                        cmd.Parameters.Add("@movi_sOficinaConsularIdDestino", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdDestino;
                        cmd.Parameters.Add("@movi_sBovedaTipoIdDestino", SqlDbType.SmallInt).Value = objBE.movi_sBovedaTipoIdDestino;
                        cmd.Parameters.Add("@movi_sBodegaDestinoId", SqlDbType.SmallInt).Value = objBE.movi_sBodegaDestinoId;
                        cmd.Parameters.Add("@movi_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sInsumoTipoId;
                        cmd.Parameters.Add("@movi_dFechaRegistro", SqlDbType.DateTime).Value = objBE.movi_dFechaRegistro;
                        cmd.Parameters.Add("@movi_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.movi_vPrefijoNumeracion;
                        cmd.Parameters.Add("@movi_vActaRemision", SqlDbType.VarChar, 12).Value = objBE.movi_vActaRemision;
                        cmd.Parameters.Add("@movi_sEstadoId", SqlDbType.SmallInt).Value = objBE.movi_sEstadoId;
                        cmd.Parameters.Add("@movi_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.movi_sUsuarioModificacion; ;
                        cmd.Parameters.Add("@movi_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.movi_vIPModificacion;
                        cmd.Parameters.Add("@movi_iMovimientoId", SqlDbType.BigInt).Value = objBE.movi_iMovimientoId;
                        cmd.Parameters.Add("@movi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@movi_dFechaValija", SqlDbType.DateTime).Value = objBE.movi_dFechaValija;

                        SqlParameter lInusmoIdReturn = cmd.Parameters.Add("@insu_sInsumoEstadoId", SqlDbType.SmallInt);
                        lInusmoIdReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lInusmoIdReturn.Value != null)
                        {
                            if (lInusmoIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                iInsumoEstadoId = Convert.ToInt16(lInusmoIdReturn.Value);
                            }
                        }

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

        public void MovimientoDetalleActualizar(BE.AL_MOVIMIENTODETALLE objBEDETALLE, BE.AL_MOVIMIENTO objBE, Nullable<Int16> iInsumoEstadoId, ref bool Error)
        {

            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (objBEDETALLE.mode_sEstadoId != Convert.ToInt16(Enumerador.enmMovimientoEstado.ANULADO))
                            objBEDETALLE.mode_sEstadoId = objBE.movi_sEstadoId;

                        cmd.Parameters.Add("@mode_iMovimientoId", SqlDbType.BigInt).Value = objBE.movi_iMovimientoId;
                        cmd.Parameters.Add("@mode_iActuacionId", SqlDbType.BigInt).Value = objBEDETALLE.mode_iActuacionId;
                        cmd.Parameters.Add("@mode_iActuacionDetalleId", SqlDbType.BigInt).Value = objBEDETALLE.mode_iActuacionDetalleId;
                        cmd.Parameters.Add("@mode_vRangoInicial", SqlDbType.VarChar, 10).Value = objBEDETALLE.mode_vRangoInicial;
                        cmd.Parameters.Add("@mode_vRangoFinal", SqlDbType.VarChar, 10).Value = objBEDETALLE.mode_vRangoFinal;
                        cmd.Parameters.Add("@mode_ICantidad", SqlDbType.Int).Value = objBEDETALLE.mode_ICantidad;
                        cmd.Parameters.Add("@mode_vObservacion", SqlDbType.VarChar, 150).Value = objBEDETALLE.mode_vObservacion;
                        cmd.Parameters.Add("@mode_sEstadoId", SqlDbType.SmallInt).Value = objBE.movi_sEstadoId;
                        cmd.Parameters.Add("@mode_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBEDETALLE.mode_sUsuarioModificacion;
                        cmd.Parameters.Add("@mode_vIPModificacion", SqlDbType.VarChar, 50).Value = objBEDETALLE.mode_vIPModificacion;
                        cmd.Parameters.Add("@mode_sOficinaConsularId", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@mode_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Parameters.Add("@insu_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.movi_sInsumoTipoId;
                        cmd.Parameters.Add("@insu_vPrefijoNumeracion", SqlDbType.VarChar, 10).Value = objBE.movi_vPrefijoNumeracion;
                        cmd.Parameters.Add("@insu_dFecharegistro", SqlDbType.DateTime).Value = objBE.movi_dFechaRegistro;
                        cmd.Parameters.Add("@mode_sMovimientoMotivoId", SqlDbType.SmallInt).Value = objBE.movi_sMovimientoMotivoId;
                        cmd.Parameters.Add("@mode_iMovimientoDetalleId", SqlDbType.BigInt).Value = objBEDETALLE.mode_iMovimientoDetalleId;
                        cmd.Parameters.Add("@insu_sInsumoEstadoId", SqlDbType.SmallInt).Value = iInsumoEstadoId;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();


                        Error = false;

                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                Error = true;
                throw ex;
            }

        }


        #endregion Método ACTUALIZAR

        #region Método ELIMINAR

        public Int16 MovimientoEliminar(BE.AL_MOVIMIENTO objBE)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@movi_iOficinaConsularId", SqlDbType.SmallInt).Value = objBE.movi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@movi_iMovimientoId", SqlDbType.BigInt).Value = objBE.movi_iMovimientoId;
                        cmd.Parameters.Add("@movi_sUsuarioModificacion", SqlDbType.SmallInt).Value = objBE.movi_sUsuarioModificacion;
                        cmd.Parameters.Add("@movi_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.movi_vIPCreacion;
                        cmd.Parameters.Add("@movi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                   
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

        #endregion Método ELIMINAR
    }
}