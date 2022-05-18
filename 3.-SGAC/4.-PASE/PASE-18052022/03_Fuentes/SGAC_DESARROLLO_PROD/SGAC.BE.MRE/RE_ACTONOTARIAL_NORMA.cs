using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIAL_NORMA : BaseBussinesObject
    {
        public Int64 anra_iActoNotarialNormaId { get; set; }
        public Int64 anra_iActoNotarialId { get; set; }
        public short anra_sNormaId { get; set; }
        public string anra_cEstado { get; set; }
        public Int16 anra_sUsuarioCreacion { get; set; }
        public string anra_vIPCreacion { get; set; }
        public DateTime anra_dFechaCreacion { get; set; }
        public Int16 anra_sUsuarioModificacion { get; set; }
        public string anra_vIPModificacion { get; set; }
        public DateTime anra_dFechaModificacion { get; set; }
    }
}
