using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_PODERFUERAREGISTRO
    {
        public CBE_PODERFUERAREGISTRO() {
            this.PARTICIPANTES = new List<CBE_PARTICIPANTE>();
        }

        public List<CBE_PARTICIPANTE> PARTICIPANTES { get; set; }
    }
}
