using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Registro.Actuacion.BL
{
    using SGAC.BE.MRE;
    using SGAC.DA.MRE.ACTONOTARIAL;
    
    public class ActoNotarialCuerpoConsultaBL
    {
        public RE_ACTONOTARIALCUERPO obtener(RE_ACTONOTARIALCUERPO cuerpo) {
            RE_ACTONOTARIALCUERPO_DA lACTONOTARIALCUERPO_DA = new RE_ACTONOTARIALCUERPO_DA();
            return lACTONOTARIALCUERPO_DA.obtener(cuerpo);
        }
    }
}
