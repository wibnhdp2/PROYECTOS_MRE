using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.DA.MRE.ACTONOTARIAL;
using System.Transactions;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoNotarial_NormaMantenimientoBL
    {
        public BE.MRE.RE_ACTONOTARIAL_NORMA insertar(BE.MRE.RE_ACTONOTARIAL_NORMA ActoNotarialNorma)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIAL_NORMAS_DA actoNotarial_DA = new RE_ACTONOTARIAL_NORMAS_DA();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                actoNotarial_DA.insertar(ActoNotarialNorma);

                if (ActoNotarialNorma.Error == true)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            return ActoNotarialNorma;
        }

        public BE.MRE.RE_ACTONOTARIAL_NORMA anular(BE.MRE.RE_ACTONOTARIAL_NORMA ActoNotarialNorma)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIAL_NORMAS_DA actoNotarial_DA = new RE_ACTONOTARIAL_NORMAS_DA();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                actoNotarial_DA.anular(ActoNotarialNorma);

                if (ActoNotarialNorma.Error == true)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            return ActoNotarialNorma;
        }
       

    }
}
