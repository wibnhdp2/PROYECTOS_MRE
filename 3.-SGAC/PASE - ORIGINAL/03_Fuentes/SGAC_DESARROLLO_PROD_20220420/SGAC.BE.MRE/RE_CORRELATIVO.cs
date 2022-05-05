using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_CORRELATIVO : BaseBussinesObject
    {
        
        public short corr_sCorrelativoId { get; set; }
        public short corr_sOficinaConsularId {get; set;}
        public short corr_sPeriodo {get; set;}
        public short corr_sTarifarioId{get; set;}
        public int corr_ICorrelativo{get; set;}
        public string corr_cEstado{get; set;}
        public short corr_sUsuarioCreacion{get; set;}
        public string corr_vIPCreacion{get; set;}
        public System.DateTime corr_dFechaCreacion{get; set;}
        public short corr_sUsuarioModificacion{get; set;}
        public string corr_vIPModificacion{get; set;}
        public Nullable<System.DateTime> corr_dFechaModificacion { get; set; }
    }
}
