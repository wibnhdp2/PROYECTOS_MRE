using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Almacen.Reportes.DA
{
    public class ReporteAlmacenConsultasDA
    {
        private string strConnectionName = string.Empty;

        public ReporteAlmacenConsultasDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ReporteAlmacenConsultasDA()
        {
            GC.Collect();
        }

        public DataSet ObtenerReporteInsumos(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@movi_sBovedaTipoId", SqlDbType.SmallInt);
            prmParameter[1].Value = movi_sBovedaTipoId;
            prmParameter[2] = new SqlParameter("@movi_sBovedaId", SqlDbType.SmallInt);
            prmParameter[2].Value = movi_sBovedaId;
            prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            prmParameter[3].Value = movi_sInsumoTipoId;
            prmParameter[4] = new SqlParameter("@movi_sMovimientoMotivoId", SqlDbType.SmallInt);
            prmParameter[4].Value = movi_sMovimientoMotivoId;
            prmParameter[5] = new SqlParameter("@movi_dFechaInicio", SqlDbType.DateTime);
            prmParameter[5].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[6] = new SqlParameter("@movi_dFechaFin", SqlDbType.DateTime);
            prmParameter[6].Value = dFechaFin;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REPORTES.USP_RP_INSUMOS",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        public DataSet ObtenerReporteInsumosDetallado(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@movi_sBovedaTipoId", SqlDbType.SmallInt);
            prmParameter[1].Value = movi_sBovedaTipoId;
            prmParameter[2] = new SqlParameter("@movi_sBovedaId", SqlDbType.SmallInt);
            prmParameter[2].Value = movi_sBovedaId;
            prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            prmParameter[3].Value = movi_sInsumoTipoId;
            prmParameter[4] = new SqlParameter("@movi_sMovimientoMotivoId", SqlDbType.SmallInt);
            prmParameter[4].Value = movi_sMovimientoMotivoId;
            prmParameter[5] = new SqlParameter("@movi_dFechaInicio", SqlDbType.DateTime);
            prmParameter[5].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[6] = new SqlParameter("@movi_dFechaFin", SqlDbType.DateTime);
            prmParameter[6].Value = dFechaFin;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REPORTES.USP_RP_INSUMOS_DETALLE",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        public DataSet ObtenerReporteInsumosRemitidos(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@movi_sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@movi_sBovedaTipoId", SqlDbType.SmallInt);
            prmParameter[1].Value = movi_sBovedaTipoId;
            prmParameter[2] = new SqlParameter("@movi_sBovedaId", SqlDbType.SmallInt);
            prmParameter[2].Value = movi_sBovedaId;
            prmParameter[3] = new SqlParameter("@movi_sInsumoTipoId", SqlDbType.SmallInt);
            prmParameter[3].Value = movi_sInsumoTipoId;
            prmParameter[4] = new SqlParameter("@movi_sMovimientoMotivoId", SqlDbType.SmallInt);
            prmParameter[4].Value = movi_sMovimientoMotivoId;
            prmParameter[5] = new SqlParameter("@movi_dFechaInicio", SqlDbType.DateTime);
            prmParameter[5].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[6] = new SqlParameter("@movi_dFechaFin", SqlDbType.DateTime);
            prmParameter[6].Value = dFechaFin;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REPORTES.USP_RP_INSUMOS_REMITIDOS",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }
    }
}