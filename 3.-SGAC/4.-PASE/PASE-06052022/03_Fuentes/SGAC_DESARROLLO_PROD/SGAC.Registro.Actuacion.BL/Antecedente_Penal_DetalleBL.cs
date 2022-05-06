using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Data;
using SGAC.DA.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class Antecedente_Penal_DetalleBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE insertar(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE objAntecedenteDetalleBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            RE_ANTECEDENTE_PENAL_DETALLE_DA objAntecedentePenalDetalleDA = new RE_ANTECEDENTE_PENAL_DETALLE_DA();
            RE_ANTECEDENTE_PENAL_DETALLE objBE = new RE_ANTECEDENTE_PENAL_DETALLE();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objAntecedentePenalDetalleDA.insertar(objAntecedenteDetalleBE);
                if (objBE.Error)
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
            return objBE;
        }

        public void actualizar(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE objAntecedenteDetalleBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            RE_ANTECEDENTE_PENAL_DETALLE_DA objAntecedentePenalDetalleDA = new RE_ANTECEDENTE_PENAL_DETALLE_DA();
            RE_ANTECEDENTE_PENAL_DETALLE objBE = new RE_ANTECEDENTE_PENAL_DETALLE();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objAntecedentePenalDetalleDA.actualizar(objAntecedenteDetalleBE);
                if (objBE.Error)
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

        public void anular(BE.MRE.RE_ANTECEDENTE_PENAL_DETALLE objAntecedenteDetalleBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            RE_ANTECEDENTE_PENAL_DETALLE_DA objAntecedentePenalDetalleDA = new RE_ANTECEDENTE_PENAL_DETALLE_DA();
            RE_ANTECEDENTE_PENAL_DETALLE objBE = new RE_ANTECEDENTE_PENAL_DETALLE();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objAntecedentePenalDetalleDA.anular(objAntecedenteDetalleBE);
                if (objBE.Error)
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

        public DataTable Consultar(Int64 intAntecedentePenalDetalleId, Int64 intAntecedentePenalId, string strNumeroExpediente,
                                   string strEstado, int IntPageSize, int intPageNumber, string strContar,
                                   ref int ITotalPages, ref int ITotalRecords)
        {
            RE_ANTECEDENTE_PENAL_DETALLE_DA objDA = new RE_ANTECEDENTE_PENAL_DETALLE_DA();

            try
            {
                return objDA.Consultar(intAntecedentePenalDetalleId, intAntecedentePenalId, strNumeroExpediente,
                                        strEstado, IntPageSize, intPageNumber, strContar,
                                        ref ITotalPages, ref ITotalRecords);
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
