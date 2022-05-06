using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_AUTORIZACIONVIAJEMENOR : RE_ACTONOTARIAL
    {
        public CBE_AUTORIZACIONVIAJEMENOR() {
            this.MADRE = new CBE_PARTICIPANTE();
            this.PADRE = new CBE_PARTICIPANTE();
            this.MENOR = new CBE_PARTICIPANTE();
            this.ACOMPANANTE = new CBE_PARTICIPANTE();

        } 
        public CBE_PARTICIPANTE MADRE { get; set; }
        public CBE_PARTICIPANTE PADRE { get; set; }
        public CBE_PARTICIPANTE MENOR { get; set; }
        public CBE_PARTICIPANTE ACOMPANANTE { get; set; }
    }
}
