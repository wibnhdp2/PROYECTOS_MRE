using System.Data;
using SGAC.Contabilidad.Remesa.DA;
using System;

namespace SGAC.Contabilidad.Remesa.BL
{
    public class RemesaConsultasBL
    {
        private RemesaConsultasDA objDA;

        public DataTable Consultar(int intPaginaActual, 
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros, 
                                   ref int intTotalPaginas, 
                                   int intOficinaConsularOrigenId, 
                                   int intOficinaConsularDestinoId,
                                   int intEstadoId,
                                   DateTime? datFechaInicio, 
                                   DateTime? datFechaFin)
        {
            try
            {
                objDA = new RemesaConsultasDA();
                return objDA.Consultar(intPaginaActual, 
                                       intPaginaCantidad, 
                                       ref intTotalRegistros, 
                                       ref intTotalPaginas,
                                       intOficinaConsularOrigenId, 
                                       intOficinaConsularDestinoId,
                                       intEstadoId, 
                                       datFechaInicio, 
                                       datFechaFin);
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

        public DataTable ObtenerPorOficinaConsular(int iOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                objDA = new RemesaConsultasDA();
                return objDA.ObtenerPorOficinaConsular(iOficinaConsularId, dFechaInicio, dFechaFin);
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

        public bool ConsultarAvisoEnvioRemesa(int intOficinaConsularId)
        { 
            try
            {
                objDA = new RemesaConsultasDA();
                return objDA.ConsultarAvisoEnvioRemesa(intOficinaConsularId);
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

        public DataTable ObtenerAlertas(Int16 intOficinaConsularId)
        {
            RemesaConsultasDA objDA;
            try
            {
                objDA = new RemesaConsultasDA();
                DataTable dt = objDA.ObtenerAlertas(intOficinaConsularId);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objDA = null;
            }
        }
        //----------------------------------------------------------------------
        // Fecha: 27/01/2017
        // Autor: Miguel Márquez Beltrán
        // Objetivo: Consultar las alertas pendientes de envio y/o enviadas
        //----------------------------------------------------------------------
        public void ConsultarAlerta(int intOficinaConsularId, ref bool bAlertaPendienteFlag, ref bool bAlertaEnviadosFlag)
        {
            RemesaConsultasDA objDA;
            try
            {
                objDA = new RemesaConsultasDA();
                objDA.ConsultarAlerta(intOficinaConsularId, ref bAlertaPendienteFlag, ref bAlertaEnviadosFlag);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
