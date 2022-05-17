using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ASISTENCIABENEFICIARIO : BaseBussinesObject
    {
        public RE_ASISTENCIABENEFICIARIO()
        {
            PERSONA = new RE_PERSONA();
        }

        public long asbe_iAsistenciaBeneficiarioId { get; set; }
	    public long asbe_iAsistenciaId { get; set; }
        public long asbe_iPersonaId { get; set; }
        public Nullable<double> asbe_FMonto { get; set; }
	    public string asbe_cEstado { get; set; }
        public short asbe_sUsuarioCreacion { get; set; }
	    public string asbe_vIPCreacion { get; set; }
	    public DateTime asbe_dFechaCreacion { get; set; }
        public Nullable<short> asbe_sUsuarioModificacion { get; set; }
	    public string asbe_vIPModificacion { get; set; }
        public Nullable<DateTime> asbe_dFechaModificacion { get; set; }

        public RE_PERSONA PERSONA { get; set; } 
    }
}
