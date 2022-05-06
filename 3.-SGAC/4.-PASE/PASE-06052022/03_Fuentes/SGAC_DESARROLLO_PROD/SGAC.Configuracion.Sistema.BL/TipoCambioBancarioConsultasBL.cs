using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class TipoCambioBancarioConsultasBL
    {
        private TipoCambioBancarioConsultasDA objDA;

        public DataTable Obtener(int intOficinaConsularId, 
                                 DateTime datFechaInicio, 
                                 DateTime datFechaFin,
                                 int intPaginaActual, 
                                 int intPaginaCantidad, 
                                 ref int intTotalRegistros, 
                                 ref int intTotalPaginas)
        {
            try
            {
                objDA = new TipoCambioBancarioConsultasDA();
                return objDA.ObtenerPorFecha(intOficinaConsularId,
                                             datFechaInicio,
                                             datFechaFin,
                                             intPaginaActual,
                                             intPaginaCantidad,
                                             ref intTotalRegistros,
                                             ref intTotalPaginas);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public DataTable ObtenerPromedioTipoCambio(int intOficinaConsularId, DateTime datFechaActual, DateTime datFechaAnterior)
        {
            try
            {
                objDA = new TipoCambioBancarioConsultasDA();
                return objDA.ObtenerPromedioTipoCambio(intOficinaConsularId, datFechaActual, datFechaAnterior);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }            
        }
    }
}
