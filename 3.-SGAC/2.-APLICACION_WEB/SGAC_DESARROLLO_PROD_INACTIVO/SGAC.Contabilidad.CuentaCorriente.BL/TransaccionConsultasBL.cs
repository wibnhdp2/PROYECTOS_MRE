using System.Data;
using SGAC.Contabilidad.CuentaCorriente.DA;
using System;

namespace SGAC.Contabilidad.CuentaCorriente.BL
{
    public class TransaccionConsultasBL
    {
        private TransaccionConsultasDA objDA;

        public DataTable Consultar(int intOficinaConsularId, 
                                   int intBancoId, 
                                   string strNumeroCuenta, 
                                   DateTime dFechaInicio, 
                                   DateTime dFechaFin,
                                   int anioPeriodo,
                                   int mesPeriodo,
                                   string busqueda,
                                   string NroOpeacion,
                                   Int16 intCodCuentaCorriente,
                                   int intPaginaActual, 
                                   int intPaginaCantidad, 
                                   ref int intTotalRegistros, 
                                   ref int intTotalPaginas)
        {
            objDA = new TransaccionConsultasDA();
            try
            {
                return objDA.ObtenerPorCuenta(intOficinaConsularId, 
                                              intBancoId, 
                                              strNumeroCuenta, 
                                              dFechaInicio, 
                                              dFechaFin,
                                              anioPeriodo,
                                              mesPeriodo,
                                              busqueda,
                                              NroOpeacion,
                                              intCodCuentaCorriente,
                                              intPaginaActual, 
                                              intPaginaCantidad, 
                                              ref intTotalRegistros, 
                                              ref intTotalPaginas);
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

        public DataTable ObtenerBancoCuenta(int intOficinaConsularId)
        {
            objDA = new TransaccionConsultasDA();
            try
            {
                return objDA.ObtenerBancoCuenta(intOficinaConsularId);
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

        //--------------------------------------------------------------
        //Fecha: 10/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Quitar los parametros de Tipo de Operación y 
        //          Tipo de Transacción. 
        //          DOCUMENTO: OBSERVACIONES_SGAC_06052021. ITEM 2.
        //--------------------------------------------------------------

        public DataTable VerificarRegistroMasivo(string strXMLExcel,
                                   Int16 CuentaCorrienteId)
        {
            objDA = new TransaccionConsultasDA();
            try
            {
                return objDA.VerificarRegistroMasivo(strXMLExcel,
                                              CuentaCorrienteId);
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


        public DataTable ListarConciliacionesPendientes(int intOficinaConsularId,
                                          Int16 intCuentaCorriente,
                                          Int64 TransaccionPadre)
        {
            objDA = new TransaccionConsultasDA();
            try
            {
                return objDA.ListarConciliacionesPendientes(intOficinaConsularId,
                                              intCuentaCorriente,
                                              TransaccionPadre);
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
