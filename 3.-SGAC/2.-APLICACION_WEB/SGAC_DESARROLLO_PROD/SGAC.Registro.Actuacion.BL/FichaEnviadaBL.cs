using System;
using System.Collections;
using System.Transactions;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using SGAC.DA.MRE;
using System.Data;
using SGAC.Registro.Actuacion.DA;
//-----------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase lógica de la ficha enviada
//-----------------------------------------------
namespace SGAC.Registro.Actuacion.BL
{
    public class FichaEnviadaBL
    {
        public bool isError { get; set; }
        public string ErrMessage { get; set; }

        public void insertar(ArrayList listaFichas)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);
            
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                DA.FichaEnviadaDA objFichaEnviadaDA;
                RE_FICHAENVIADA objBE;

                foreach (RE_FICHAENVIADA objFichaEnviadaBE in listaFichas)
                {
                    objFichaEnviadaDA = new FichaEnviadaDA();
                    objBE = new RE_FICHAENVIADA();

                    objBE = objFichaEnviadaDA.insertar(objFichaEnviadaBE);

                    if (objBE.Error == true)
                    {
                        this.isError = true;
                        this.ErrMessage = objBE.Message;
                        break;
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

        public void anular(RE_FICHAENVIADA objfichaEnviadaBE)
        {
            this.isError = false;
            this.ErrMessage = string.Empty;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);

            DA.FichaEnviadaDA objFichaEnviadaDA = new FichaEnviadaDA();
            RE_FICHAENVIADA objBE = new RE_FICHAENVIADA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                objBE = objFichaEnviadaDA.anular(objfichaEnviadaBE);
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

        public DataTable Consultar(long intFichaEnviadaId, long intGuiaDespachoId, long intFichaRegistralId,
                                          int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DA.FichaEnviadaDA objDA = new DA.FichaEnviadaDA();

            try
            {
                return objDA.Consultar(intFichaEnviadaId, intGuiaDespachoId, intFichaRegistralId,
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
    }
}
