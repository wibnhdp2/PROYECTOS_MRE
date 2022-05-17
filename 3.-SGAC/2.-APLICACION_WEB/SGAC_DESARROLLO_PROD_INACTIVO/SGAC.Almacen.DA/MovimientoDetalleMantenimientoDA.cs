using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class MovimientoDetalleMantenimientoDA
    {
        private string strConnectionName = string.Empty;

        public string strError=string.Empty;

        public MovimientoDetalleMantenimientoDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~MovimientoDetalleMantenimientoDA()
        {
            GC.Collect();
        }

        #region Método ADICIONAR

        public int MovimientoDetalleAdicionar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            int iResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[12];

                prmParameterHeader[0] = new SqlParameter("@mode_IOficinaConsularId", SqlDbType.Int);
                prmParameterHeader[0].Value = objBE.AL_MOVIMIENTO.movi_sOficinaConsularIdOrigen;
                prmParameterHeader[1] = new SqlParameter("@mode_iMovimientoId", SqlDbType.BigInt);
                prmParameterHeader[1].Value = objBE.AL_MOVIMIENTO.movi_iMovimientoId;
                prmParameterHeader[2] = new SqlParameter("@mode_iActuacionId", SqlDbType.BigInt);
                prmParameterHeader[2].Value = objBE.mode_iActuacionId;
                prmParameterHeader[3] = new SqlParameter("@mode_iActuacionDetalleId", SqlDbType.BigInt);
                prmParameterHeader[3].Value = objBE.mode_iActuacionDetalleId;
                prmParameterHeader[4] = new SqlParameter("@mode_vRangoInicial", SqlDbType.VarChar, 10);
                prmParameterHeader[4].Value = objBE.mode_vRangoInicial;
                prmParameterHeader[5] = new SqlParameter("@mode_vRangoFinal", SqlDbType.VarChar, 10);
                prmParameterHeader[5].Value = objBE.mode_vRangoFinal;
                prmParameterHeader[6] = new SqlParameter("@mode_ICantidad", SqlDbType.Int);
                prmParameterHeader[6].Value = objBE.mode_ICantidad;
                prmParameterHeader[7] = new SqlParameter("@mode_vObservacion", SqlDbType.VarChar, 150);
                prmParameterHeader[7].Value = objBE.mode_vObservacion;
                prmParameterHeader[8] = new SqlParameter("@mode_IEstadoId", SqlDbType.SmallInt);
                prmParameterHeader[8].Value = objBE.mode_sEstadoId;
                prmParameterHeader[9] = new SqlParameter("@mode_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[9].Value = objBE.mode_sUsuarioCreacion;

                prmParameterHeader[10] = new SqlParameter("@mode_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[10].Value = Util.ObtenerHostName();
                prmParameterHeader[11] = new SqlParameter("@mode_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[11].Value = objBE.mode_vIPCreacion;

                iResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_ADICIONAR",
                                         prmParameterHeader);
                return iResultQuery;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
        }

        #endregion Método ADICIONAR

        #region Método ACTUALIZAR

        public int MovimientoDetalleActualizar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            int iResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[14];

                prmParameterHeader[0] = new SqlParameter("@mode_iActuacionId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = objBE.mode_iActuacionId;
                prmParameterHeader[1] = new SqlParameter("@mode_iActuacionDetalleId", SqlDbType.BigInt);
                prmParameterHeader[1].Value = objBE.mode_iActuacionDetalleId;
                prmParameterHeader[2] = new SqlParameter("@mode_vRangoInicial", SqlDbType.VarChar, 10);
                prmParameterHeader[2].Value = objBE.mode_vRangoInicial;
                prmParameterHeader[3] = new SqlParameter("@mode_vRangoFinal", SqlDbType.VarChar, 10);
                prmParameterHeader[3].Value = objBE.mode_vRangoFinal;
                prmParameterHeader[4] = new SqlParameter("@mode_ICantidad", SqlDbType.Int);
                prmParameterHeader[4].Value = objBE.mode_ICantidad;
                prmParameterHeader[5] = new SqlParameter("@mode_vObservacion", SqlDbType.VarChar, 150);
                prmParameterHeader[5].Value = objBE.mode_vObservacion;
                prmParameterHeader[6] = new SqlParameter("@mode_sEstadoId", SqlDbType.SmallInt);
                prmParameterHeader[6].Value = objBE.mode_sEstadoId;
                prmParameterHeader[8] = new SqlParameter("@mode_dFechaModificacion", SqlDbType.DateTime);
                prmParameterHeader[8].Value = objBE.mode_dFechaModificacion;
                prmParameterHeader[9] = new SqlParameter("@mode_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterHeader[9].Value = objBE.mode_sUsuarioModificacion;

                prmParameterHeader[10] = new SqlParameter("@mode_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[10].Value = Util.ObtenerHostName();
                prmParameterHeader[11] = new SqlParameter("@mode_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[11].Value = objBE.mode_vIPCreacion;

                /*PARAMETROS DE CONDICION*/
                prmParameterHeader[12] = new SqlParameter("@mode_iMovimientoId", SqlDbType.BigInt);
                prmParameterHeader[12].Value = objBE.mode_iMovimientoId;
                prmParameterHeader[13] = new SqlParameter("@mode_IOficinaConsularId", SqlDbType.Int);
                prmParameterHeader[13].Value = objBE.AL_MOVIMIENTO.movi_sOficinaConsularIdOrigen;

                iResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_ACTUALIZAR",
                                         prmParameterHeader);

                return iResultQuery;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
        }

        #endregion Método ACTUALIZAR

        #region Método ELIMINAR

        public int MovimientoDetalleEliminar(BE.AL_MOVIMIENTODETALLE objBE)
        {
            int iResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[5];
                prmParameterHeader[0] = new SqlParameter("@mode_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterHeader[0].Value = objBE.mode_sUsuarioModificacion;
                prmParameterHeader[1] = new SqlParameter("@mode_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[1].Value = Util.ObtenerHostName();
                prmParameterHeader[2] = new SqlParameter("@mode_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[2].Value = objBE.mode_vIPModificacion;

                /*PARAMETROS DE CONDICION*/
                prmParameterHeader[3] = new SqlParameter("@mode_iMovimientoId", SqlDbType.BigInt);
                prmParameterHeader[3].Value = objBE.mode_iMovimientoId;
                prmParameterHeader[4] = new SqlParameter("@mode_IOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[4].Value = objBE.AL_MOVIMIENTO.movi_sOficinaConsularIdOrigen;

                iResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_ELIMINAR",
                                         prmParameterHeader);

                return iResultQuery;
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