using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.DA.MRE.ACTONOTARIAL;
using SGAC.BE.MRE;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoNotarial_NormasConsultaBL
    {
        public DataTable ObtenerNormas(BE.MRE.RE_ACTONOTARIAL_NORMA normas)
        {
            RE_ACTONOTARIAL_NORMAS_DA objActoNotarialNormaDA = new RE_ACTONOTARIAL_NORMAS_DA();
            return objActoNotarialNormaDA.ObtenerNormas(normas);
        }
    }
}
