using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_MIGRATORIO
    {
        public CBE_MIGRATORIO()
        {
            this.ACTO = new RE_ACTOMIGRATORIO();
            this.FORMATO = new RE_ACTOMIGRATORIOFORMATO();
            this.HISTORICO = new List<RE_ACTOMIGRATORIOHISTORICO>();
            this.PERSONA = new CBE_PERSONA();
            this.message = string.Empty;
        }

        public RE_ACTOMIGRATORIO ACTO { get; set; }
        public RE_ACTOMIGRATORIOFORMATO FORMATO { get; set; }
        public List<RE_ACTOMIGRATORIOHISTORICO> HISTORICO { get; set; }
        public CBE_PERSONA PERSONA { get; set; }
        public string message { get; set; }
    }
}
