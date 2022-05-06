using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_PROTOCOLAR
    {
        public CBE_PROTOCOLAR() {
            
            this.ACTO = new RE_ACTONOTARIAL();
            this.CUERPO = new RE_ACTONOTARIALCUERPO();
            this.DOCUMENTOS = new List<RE_ACTONOTARIALDOCUMENTO>();
            this.PERSONAS = new List<RE_PERSONA>();
            this.PARTICIPANTES = new List<RE_ACTONOTARIALPARTICIPANTE>();
            this.SEGUIMIENTO = new List<RE_ACTONOTARIALSEGUIMIENTO>();
            this.ACTUACIONDETALLE = new List<RE_ACTUACIONDETALLE>();
            this.PAGO = new List<RE_PAGO>();
        }
        public RE_ACTONOTARIAL ACTO { get; set; }
        public RE_ACTONOTARIALCUERPO CUERPO { get; set; }
        public List<RE_ACTONOTARIALDOCUMENTO> DOCUMENTOS { get; set; }
        public List<RE_PERSONA> PERSONAS { get; set; }
        public List<RE_ACTONOTARIALPARTICIPANTE> PARTICIPANTES { get; set; }
        public List<RE_ACTONOTARIALSEGUIMIENTO> SEGUIMIENTO { get; set; }
        public List<RE_PAGO> PAGO { get; set; }
        public RE_ACTUACION ACTUACION { get; set; }
        public List<RE_ACTUACIONDETALLE> ACTUACIONDETALLE { get; set; }
        public RE_ACTUACIONINSUMODETALLE ACTUACIONINSUMODETALLE { get; set; }
        public RE_EMPRESA EMPRESA { get; set; }
    }
}
