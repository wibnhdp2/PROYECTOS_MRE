using System;
using System.Data;
using SGAC.Almacen.Reportes.DA;

namespace SGAC.Almacen.Reportes.BL
{
    public class ReporteAlmacenConsultasBL
    {
        private ReporteAlmacenConsultasDA objDA;

        public DataSet ObtenerReporteInsumos(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                objDA = new ReporteAlmacenConsultasDA();
                return objDA.ObtenerReporteInsumos(intOficinaConsularId, movi_sBovedaTipoId, movi_sBovedaId,
                movi_sInsumoTipoId, movi_sMovimientoMotivoId, dFechaInicio, dFechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteInsumosDetallado(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                objDA = new ReporteAlmacenConsultasDA();
                return objDA.ObtenerReporteInsumosDetallado(intOficinaConsularId, movi_sBovedaTipoId, movi_sBovedaId,
                movi_sInsumoTipoId, movi_sMovimientoMotivoId, dFechaInicio, dFechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }

        public DataSet ObtenerReporteInsumosRemitidos(int intOficinaConsularId, int movi_sBovedaTipoId, int movi_sBovedaId,
            int movi_sInsumoTipoId, int movi_sMovimientoMotivoId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                objDA = new ReporteAlmacenConsultasDA();
                return objDA.ObtenerReporteInsumosRemitidos(intOficinaConsularId, movi_sBovedaTipoId, movi_sBovedaId,
                movi_sInsumoTipoId, movi_sMovimientoMotivoId, dFechaInicio, dFechaFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}