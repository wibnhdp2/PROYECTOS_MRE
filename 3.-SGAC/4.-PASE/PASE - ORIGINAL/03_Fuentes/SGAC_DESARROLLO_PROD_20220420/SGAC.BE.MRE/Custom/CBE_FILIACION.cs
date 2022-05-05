using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_FILIACION : BE.RE_PERSONAFILIACION
    {
        public Int16 peid_sDocumentoTipoId { get; set; }
        public string peid_vDocumentoNumero { get; set; }
        public string pers_vApellidoPaterno { get; set; }
        public string pers_vApellidoMaterno { get; set; }
        public string pers_vNombres { get; set; }
        public Int16 pers_sGeneroId { get; set; }
    }
}
