using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTUACIONINSUMODETALLE : BaseBussinesObject
    {
        public long aide_iActuacionInsumoDetalleId { get; set; }
        public long aide_iActuacionDetalleId {get; set;}
        public long aide_iInsumoId {get; set;}
        public DateTime aide_dFechaVinculacion {get; set;}
        public short aide_sUsuarioVinculacionId {get; set;}
        public bool aide_bFlagImpresion {get; set;}
        public DateTime aide_dFechaImpresion {get; set;}
        public string aide_cEstado {get; set;}
        public short aide_sUsuarioCreacion {get; set;}
        public string aide_vIPCreacion {get; set;}
        public DateTime aide_dFechaCreacion {get; set;}
        public short aide_sUsuarioModificacion {get; set;}
        public string aide_vIPModificacion {get; set;}
        public DateTime aide_dFechaModificacion {get; set;}
    }
}
