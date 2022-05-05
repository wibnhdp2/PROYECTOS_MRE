using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIALCUERPO : BaseBussinesObject
    {
        public Int64 ancu_iActoNotarialCuerpoId { get; set; }
        public Int64 ancu_iActoNotarialId { get; set; }
        public string ancu_vCuerpo { get; set; }
        public String ancu_vTextoCentral { get; set; }
        public String ancu_vTextoAdicional { get; set; }
        public String ancu_vTextoNormativo { get; set; }
        public string ancu_vFirmaIlegible { get; set; }
        public Boolean ancu_bFlagExtraprotocolarCuerpo { get; set; }
        public string ancu_cEstado { get; set; }
        public Int16 ancu_sUsuarioCreacion { get; set; }
        public string ancu_vIPCreacion { get; set; }
        public DateTime ancu_dFechaCreacion { get; set; }
        public Int16 ancu_sUsuarioModificacion { get; set; }
        public string ancu_vIPModificacion { get; set; }
        public DateTime ancu_dFechaModificacion { get; set; }
        public String ancu_vDL1049Articulo55C { get; set; }
    }
}
