using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    class CBE_ACTOJUDICIAL
    {
        public CBE_ACTOJUDICIAL()
        {
            this.ACTOJUDICIAL = new BE.RE_ACTOJUDICIAL();
            this.ACTOJUDICIALPARTICIPANTE = new List<BE.RE_ACTOJUDICIALPARTICIPANTE>();
            this.ACTOJUDICIALNOTIFICACION = new List<BE.RE_ACTOJUDICIALNOTIFICACION>();
            this.ACTAJUDICIAL = new List<BE.RE_ACTAJUDICIAL>();
        }

        public BE.RE_ACTOJUDICIAL ACTOJUDICIAL { get; set; }
        public List<BE.RE_ACTOJUDICIALPARTICIPANTE> ACTOJUDICIALPARTICIPANTE { get; set; }
        public List<BE.RE_ACTOJUDICIALNOTIFICACION> ACTOJUDICIALNOTIFICACION { get; set; }
        public List<BE.RE_ACTAJUDICIAL> ACTAJUDICIAL { get; set; }
    }
}
