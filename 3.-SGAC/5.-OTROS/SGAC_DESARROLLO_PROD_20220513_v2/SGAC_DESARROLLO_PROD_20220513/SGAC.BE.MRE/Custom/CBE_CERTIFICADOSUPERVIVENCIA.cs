using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_CERTIFICADOSUPERVIVENCIA:RE_ACTONOTARIAL 
    {
        public CBE_CERTIFICADOSUPERVIVENCIA() {
            this.TITULAR = new CBE_PARTICIPANTE();
            this.SOLICITANTE = new CBE_PARTICIPANTE();
        }
        public CBE_PARTICIPANTE TITULAR { get; set; }
        public CBE_PARTICIPANTE SOLICITANTE { get; set; }


    }
}
