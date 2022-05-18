using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.DA.MRE;
using System.Data;
using SGAC.Registro.Actuacion.DA;
//---------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase lógica de la ficha registral 
//---------------------------------------------------------------
namespace SGAC.Registro.Actuacion.BL
{
    public class FichaRegistralBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }


        public void Anular(RE_FICHAREGISTRAL objBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                FichaRegistralDA objFichaRegistralDA = new FichaRegistralDA();
                RE_FICHAREGISTRAL objFichaRegistralBE = new RE_FICHAREGISTRAL();
                objFichaRegistralBE = objFichaRegistralDA.anular(objBE);

                if (objFichaRegistralBE.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objBE.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }                
            }
        }

        public void Grabar(ArrayList Lista_FichaRegistralBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                FichaRegistralDA objFichaRegistralDA;
                RE_FICHAREGISTRAL objBE;

                foreach (RE_FICHAREGISTRAL objFichaRegistralBE in Lista_FichaRegistralBE)
                {
                    objFichaRegistralDA = new FichaRegistralDA();
                    objBE = new RE_FICHAREGISTRAL();

                    if (objFichaRegistralBE.fire_iFichaRegistralId == 0)
                    {
                        objBE = objFichaRegistralDA.insertar(objFichaRegistralBE);
                    }
                    else
                    {
                        objBE = objFichaRegistralDA.actualizar(objFichaRegistralBE);
                    }
                    if (objBE.Error)
                    {
                        this.isError = true;
                        this.ErrMessage = objBE.Message;
                        break;
                    }
                }
                if (this.isError)
                {
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
        }      


        public DataTable Consultar(long intFichaRegistralId, long intActuacionDetalleId, string strNumeroFicha, string strFechaInicio, string strFechaFin,
                                   int intEstadoId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.Consultar(intFichaRegistralId, intActuacionDetalleId, strNumeroFicha, strFechaInicio, strFechaFin,
                                        intEstadoId, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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

        public DataTable ConsultarTitular(int intOficinaConsularId, long intFichaRegistralId, string strNumeroFicha, int intEstadoId, string strFechaInicio, string strFechaFin,
                                          int intCorrelativoActuacion, string strNumeroGuia, string strDocumentoNumero,
                                          string strApPaterno, string strApMaterno, string strNombres, string strNumeroHoja,bool bSIO,
                                          int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.ConsultarTitular(intOficinaConsularId, intFichaRegistralId, strNumeroFicha, intEstadoId, strFechaInicio, strFechaFin,
                                        intCorrelativoActuacion, strNumeroGuia, strDocumentoNumero,
                                        strApPaterno, strApMaterno, strNombres, strNumeroHoja,bSIO,
                                        ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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

        public DataTable ConsultarFichasPorEnviar(int intOficinaConsularId, long intFichaRegistralId, int intTarifarioId, string strNumeroFicha, int intEstadoId,
                                                        string strFechaInicio, string strFechaFin, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.ConsultarFichasPorEnviar(intOficinaConsularId, intFichaRegistralId, intTarifarioId, strNumeroFicha, intEstadoId,
                                                    strFechaInicio, strFechaFin, ICurrentPag, IPageSize, ref ITotalRecords, ref ITotalPages);
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

        public DataTable Reporte(long intFichaRegistralId)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.Reporte(intFichaRegistralId);
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
        public DataTable ObtenerDocumentosFichaRegistral(long intFichaRegistralId)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.ObtenerDocumentosFichaRegistral(intFichaRegistralId);
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
        public void ActualizarEnvioSIO(ArrayList ListafichaRegistral)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                FichaRegistralDA objFichaRegistralDA;
                RE_FICHAREGISTRAL objFichaRegistralBE;

                foreach (RE_FICHAREGISTRAL objFichaEnviadaBE in ListafichaRegistral)
                {
                    objFichaRegistralDA = new FichaRegistralDA();
                    objFichaRegistralBE = new RE_FICHAREGISTRAL();
                    objFichaRegistralBE = objFichaRegistralDA.ActualizarEnvioSIO(objFichaEnviadaBE);
                    if (objFichaRegistralBE.Error)
                    {
                        this.isError = true;
                        this.ErrMessage = objFichaEnviadaBE.Message;
                        Transaction.Current.Rollback();
                    }
                }
                if (this.isError == true)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
                

                          
            }
            
        }


        public DataTable ConsultarTitularReporte(int intOficinaConsularId, long intFichaRegistralId, string strNumeroFicha, int intEstadoId, string strFechaInicio, string strFechaFin,
                                          int intCorrelativoActuacion, string strNumeroGuia, string strDocumentoNumero,
                                          string strApPaterno, string strApMaterno, string strNombres, string strNumeroHoja,bool bSio)
        {
            DA.FichaRegistralDA objDA = new DA.FichaRegistralDA();

            try
            {
                return objDA.ConsultarTitularReporte(intOficinaConsularId, intFichaRegistralId, strNumeroFicha, intEstadoId, strFechaInicio, strFechaFin,
                                        intCorrelativoActuacion, strNumeroGuia, strDocumentoNumero,
                                        strApPaterno, strApMaterno, strNombres, strNumeroHoja, bSio);
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
