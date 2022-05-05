using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_CUERPO: RE_ACTONOTARIALCUERPO
    {
        /*
        acno_sAutorizacionTipoId	smallint
        acno_vNumeroColegiatura	varchar	no	50
         */
        public CBE_CUERPO() {
            this.ActoNotarial = new RE_ACTONOTARIAL();
            }

        public RE_ACTONOTARIAL ActoNotarial { get; set; }

    }
}
