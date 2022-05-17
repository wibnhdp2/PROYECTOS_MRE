using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_FICHAENVIADA : BaseBussinesObject
    {
        public RE_FICHAENVIADA() { }

        public Int64 fien_iFichaEnviadaId { get; set; }//	bigint
        public Int64 fien_iGuiaDespachoId { get; set; }//	bigint
        public Int64 fien_iFichaRegistralId { get; set; }//	bigint
        public string fien_cEstado { get; set; }//	char
        public Int16 fien_sUsuarioCreacion { get; set; }//	smallint
        public string fien_vIPCreacion { get; set; }//	varchar
        public DateTime fien_dFechaCreacion { get; set; }//	datetime
        public Int16 fien_sUsuarioModificacion { get; set; }//	smallint
        public string fien_vIPModificacion { get; set; }//	varchar
        public DateTime fien_dFechaModificacion { get; set; }//	datetime
    }
}
