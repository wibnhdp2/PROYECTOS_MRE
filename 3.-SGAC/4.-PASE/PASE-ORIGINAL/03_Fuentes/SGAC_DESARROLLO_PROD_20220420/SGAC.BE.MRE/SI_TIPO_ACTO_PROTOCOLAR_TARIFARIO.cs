using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO : BaseBussinesObject
    {
        public SI_TIPO_ACTO_PROTOCOLAR_TARIFARIO() { }

        public short acta_iTipoActoProtocolarTarifarioId { get; set; }
        public short acta_sTipoActoProtocolarId { get; set; }
        public short  acta_sTarifarioId { get; set; }
        public string acta_cEstado { get; set; }
        public short acta_sUsuarioCreacion { get; set; }
        public string acta_vIPCreacion { get; set; }
        public DateTime acta_dFechaCreacion { get; set; }
        public Nullable<short> acta_sUsuarioModificacion { get; set; }
        public string acta_vIPModificacion { get; set; }
        public Nullable<System.DateTime> acta_dFechaModificacion { get; set; }
    }
}
