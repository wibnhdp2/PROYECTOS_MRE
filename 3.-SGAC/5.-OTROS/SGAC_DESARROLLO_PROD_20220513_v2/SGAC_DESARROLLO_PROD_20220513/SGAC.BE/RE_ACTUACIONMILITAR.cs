using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    [Serializable]
    public class RE_ACTUACIONMILITAR
    {
        public BE.RE_REGISTROMILITAR REGISTROMILITAR { get; set; }
        public BE.RE_REGISTROCIVIL REGISTROCIVIL { get; set; }
        public BE.RE_PERSONA PERSONA { get; set; }
        public BE.RE_PARTICIPANTE TITULAR { get; set; }
        public BE.RE_RESIDENCIA RESIDENCIA { get; set; }
        public List<BE.RE_PARTICIPANTE> PARTICIPANTE_Container { get; set; }

        
    }
}
