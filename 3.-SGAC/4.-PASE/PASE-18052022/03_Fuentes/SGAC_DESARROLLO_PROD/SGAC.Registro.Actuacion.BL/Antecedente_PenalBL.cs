using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Data;
using SGAC.DA.MRE;


namespace SGAC.Registro.Actuacion.BL
{
     public class Antecedente_PenalBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public void Grabar(RE_ANTECEDENTE_PENAL objBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            RE_ANTECEDENTE_PENAL_DA objAntecedenteDA = new RE_ANTECEDENTE_PENAL_DA();
            RE_ANTECEDENTE_PENAL objAntecedente = new RE_ANTECEDENTE_PENAL();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (objBE.anpe_iAntecedentePenalId == 0)
                {
                    objAntecedente = objAntecedenteDA.insertar(objBE);
                }
                else
                {
                    objAntecedente = objAntecedenteDA.actualizar(objBE);
                }
                if (objAntecedente.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objAntecedente.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }            
        }

        public void Anular(RE_ANTECEDENTE_PENAL objBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            RE_ANTECEDENTE_PENAL_DA objAntecedenteDA = new RE_ANTECEDENTE_PENAL_DA();
            RE_ANTECEDENTE_PENAL objAntecedente = new RE_ANTECEDENTE_PENAL();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objAntecedente = objAntecedenteDA.anular(objBE);

                if (objAntecedente.Error)
                {
                    this.isError = true;
                    this.ErrMessage = objAntecedente.Message;
                    Transaction.Current.Rollback();
                }
                else
                {
                    scope.Complete();
                }
            }
        }


        public DataTable Consultar(long intDetalleActuacionid, short intOficinaConsularId, string strFechaInicial="", string strFechaFinal="", string strIsMSIAP_RGE="")
        {
            RE_ANTECEDENTE_PENAL_DA objDA = new RE_ANTECEDENTE_PENAL_DA();

            try
            {
                return objDA.Consultar(intDetalleActuacionid, intOficinaConsularId, strFechaInicial, strFechaFinal, strIsMSIAP_RGE);
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

        public DataTable ReporteCertificadoConsular(long intAntecedentePenalId, Int16 intTipoDocumentoId)
        {
            RE_ANTECEDENTE_PENAL_DA objDA = new RE_ANTECEDENTE_PENAL_DA();

            try
            {
                return objDA.ReporteCertificadoConsular(intAntecedentePenalId, intTipoDocumentoId);
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


        public DataSet ObtenerDataSet(long intDetalleActuacionid, short intOficinaConsularId, string strFechaInicial = "", string strFechaFinal = "", string strIsMSIAP_RGE = "",Int16 Usuario = 0)
        {
            RE_ANTECEDENTE_PENAL_DA objDA = new RE_ANTECEDENTE_PENAL_DA();

            try
            {
                return objDA.ObtenerDataSet(intDetalleActuacionid, intOficinaConsularId, strFechaInicial, strFechaFinal, strIsMSIAP_RGE, Usuario);
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
