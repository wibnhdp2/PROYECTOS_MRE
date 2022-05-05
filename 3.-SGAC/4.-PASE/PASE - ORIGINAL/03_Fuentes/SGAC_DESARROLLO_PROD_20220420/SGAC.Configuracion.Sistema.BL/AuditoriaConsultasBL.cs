using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class AuditoriaConsultasBL
    {
        private AuditoriaConsultasDA objDA;

        public DataTable Consultar(int intPaginaActual, 
                                   int intPaginaCantidad, 
                                   ref int intTotalRegistros, 
                                   ref int intTotalPaginas,
                                   int intOficinaConsularId, 
                                   int intUsuarioId, 
                                   int intOperacionId, 
                                   int intResultadoId, 
                                   int intFormularioId,
                                   DateTime datFechaInicio, 
                                   DateTime datFechaFin)
        {
            try
            {
                objDA = new AuditoriaConsultasDA();
                return objDA.Consultar(intPaginaActual, 
                                       intPaginaCantidad, 
                                       ref intTotalRegistros, 
                                       ref intTotalPaginas,
                                       intOficinaConsularId, 
                                       intUsuarioId, 
                                       intOperacionId, 
                                       intResultadoId, 
                                       intFormularioId,
                                       datFechaInicio, 
                                       datFechaFin);
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