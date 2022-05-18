using System;
using System.Data;
using SGAC.Almacen.DA;

namespace SGAC.Almacen.BL
{
    public class MovimientoConsultaBL
    {
        public DataTable Consultar(int intOficinaConsularId, DateTime datFechaInicio, DateTime datFechaFin, int intTipoInsumo,
                                    int movi_IEstadoId, string movi_cPedidoCodigo, int intPaginaActual, int intPaginaCantidad,
                                    ref int intTotalRegistros, ref int intTotalPaginas,
                                    int intOficinaConsularOrigenId, int intBovedaTipoOrigenId, int intBovedaOrigenId)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.Consultar(intOficinaConsularId, datFechaInicio, datFechaFin, intTipoInsumo,
                                           movi_IEstadoId, movi_cPedidoCodigo, intPaginaActual, intPaginaCantidad,
                                           ref intTotalRegistros, ref intTotalPaginas,
                                           intOficinaConsularOrigenId, intBovedaTipoOrigenId, intBovedaOrigenId);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        //public DataTable Leer(int movi_iMovimientoId)
        //{
        //    DataTable dtlResult = new DataTable();
        //    MovimientoConsultaDA xFun = new MovimientoConsultaDA();

        //    try
        //    {
        //        dtlResult = xFun.LeerRegistro(movi_iMovimientoId);
        //        return dtlResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex.InnerException);
        //    }
        //    finally
        //    {
        //        if (xFun != null)
        //            xFun = null;
        //    }
        //}

        public DataTable MovimientoMotivo(int intMovimientoMotivo, int intOficinaConsularId)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.MovimientoMotivo(intMovimientoMotivo, intOficinaConsularId);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        public DataTable ConsultarStock(int intTipoInsumo, DateTime datFechaInicio, DateTime datFechaFin,
                                        int intOficinaConsularIdOrigen, int intBovedaTipoIdOrigen, int intBodegaOrigenId,
                                        int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
                                        ref string strMensaje)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.ConsultarStock(intTipoInsumo, datFechaInicio, datFechaFin,
                                            intOficinaConsularIdOrigen, intBovedaTipoIdOrigen, intBodegaOrigenId,
                                            intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas, ref strMensaje);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        public DataTable ConsultarMesAnterior(int intOficinaConsularId, int intTipoInsumo)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.ConsultarMesAnterior(intOficinaConsularId, intTipoInsumo);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        public DataTable ValidaRangosDetalle(int intOficinaConsularId, int intMotivoId, int intTipoInsumo,
                                                int intRangoInicial, int intRangoFinal, ref int intResultado, ref string strMensaje,
                                                int intTipoBovedaId, int intBovedaId)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.ValidaRangosDetalle(intOficinaConsularId, intMotivoId, intTipoInsumo,
                                                    intRangoInicial, intRangoFinal, ref intResultado, ref strMensaje,
                                                    intTipoBovedaId, intBovedaId);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }

        public DataTable ConsultaRangosDisponibles(int intOficinaConsularId, int intBovedaTipoIdOrigen, int intBodegaOrigenId, int intTipoInsumo)
        {
            DataTable dtlResult = new DataTable();
            MovimientoConsultaDA xFun = new MovimientoConsultaDA();

            try
            {
                dtlResult = xFun.ConsultaRangosDisponibles(intOficinaConsularId, intBovedaTipoIdOrigen, intBodegaOrigenId, intTipoInsumo);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }
    }
}