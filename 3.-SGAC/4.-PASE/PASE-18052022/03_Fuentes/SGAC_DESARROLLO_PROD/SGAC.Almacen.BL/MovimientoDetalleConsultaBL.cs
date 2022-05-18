using System;
using System.Data;
using SGAC.Almacen.DA;

namespace SGAC.Almacen.BL
{
    public class MovimientoDetalleConsultaBL
    {
        public DataTable MovimientoDetalleConsultar(int mode_iMovimientoId)
        {
            DataTable dtlResult = new DataTable();
            MovimientoDetalleConsultaDA xFun = new MovimientoDetalleConsultaDA();

            try
            {
                dtlResult = xFun.MovimientoDetalleConsultar(mode_iMovimientoId);
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

        //public DataTable MovimientoDetalleLeerRegistro(int mode_iMovimientoId, int mode_iMovimientoDetalleId)
        //{
        //    DataTable dtlResult = new DataTable();
        //    MovimientoDetalleConsultaDA xFun = new MovimientoDetalleConsultaDA();

        //    try
        //    {
        //        dtlResult = xFun.MovimientoDetalleLeerRegistro(mode_iMovimientoId, mode_iMovimientoDetalleId);
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
    }
}