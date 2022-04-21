using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIAL_PRIMIGENIA : BaseBussinesObject
    {
        public long anpr_iActoNotarialPrimigeniaId { get; set; }
        public long anpr_iActoNotarialId { get; set; }
        public string anpr_cAnioEscritura { get; set; }
        public string anpr_vNumeroEscrituraPublica { get; set; }
        public short anpr_sOficinaConsularId { get; set; }
        public DateTime anpr_dFechaExpedicion { get; set; }
        public string anpr_vTipoActoNotarial { get; set; }
        public string anpr_vNotaria { get; set; }
        public string anpr_cEstado { get; set; }
        public short anpr_sUsuarioCreacion { get; set; }
        public string anpr_vIPCreacion { get; set; }
        public DateTime anpr_dFechaCreacion { get; set; }
        public Nullable<short> anpr_sUsuarioModificacion { get; set; }
        public string anpr_vIPModificacion { get; set; }
        public Nullable<DateTime> anpr_dFechaModificacion { get; set; }
        public long iActoNotarialReferencialId { get; set; }

    }
}
