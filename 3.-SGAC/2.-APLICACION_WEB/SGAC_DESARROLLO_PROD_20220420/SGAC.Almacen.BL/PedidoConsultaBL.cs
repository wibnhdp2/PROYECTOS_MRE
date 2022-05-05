using System;
using System.Data;
using SGAC.Almacen.DA;

namespace SGAC.Almacen.BL
{
    public class PedidoConsultaBL
    {
        public DataTable Consultar(int pedi_iOficinaConsularId, DateTime datFechaInicio, DateTime datFechaFin, string pedi_vActaRemision,
                                    int pedi_IEstadoId, int pedi_IInsumoTipoId, string pedi_cPedidoCodigo,
                                    int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            DataTable dtlResult = new DataTable();
            PedidoConsultaDA xFun = new PedidoConsultaDA();

            try
            {
                dtlResult = xFun.Consultar(pedi_iOficinaConsularId, datFechaInicio, datFechaFin, pedi_vActaRemision, pedi_IEstadoId, pedi_IInsumoTipoId, pedi_cPedidoCodigo,
                                            intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
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

        //public DataTable Leer(int pedi_iPedidoId)
        //{
        //    DataTable dtlResult = new DataTable();
        //    PedidoConsultaDA xFun = new PedidoConsultaDA();

        //    try
        //    {
        //        dtlResult = xFun.LeerRegistro(pedi_iPedidoId);
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

        public DataTable ExistePedidoAtendido(string pedi_vPedidoCodigo)
        {
            DataTable dtlResult = new DataTable();
            PedidoConsultaDA xFun = new PedidoConsultaDA();

            try
            {
                dtlResult = xFun.ExistePedidoAtendido(@pedi_vPedidoCodigo);
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