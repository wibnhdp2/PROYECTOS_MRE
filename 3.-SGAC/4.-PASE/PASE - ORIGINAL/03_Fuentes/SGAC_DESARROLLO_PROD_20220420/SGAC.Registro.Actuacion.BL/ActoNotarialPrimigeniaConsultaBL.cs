using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.Registro.Actuacion.DA;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.BL
{
    using SGAC.BE.MRE;
    using SGAC.DA.MRE.ACTONOTARIAL;

    public class ActoNotarialPrimigeniaConsultaBL
    {
        public DataTable Consultar(long iActoNotarialPrimigeniaId, long iActoNotarialId)
        {
            RE_ACTONOTARIAL_PRIMIGENIA_DA objDA = new RE_ACTONOTARIAL_PRIMIGENIA_DA();
            return objDA.Consultar(iActoNotarialPrimigeniaId, iActoNotarialId);
        }
    }
}
