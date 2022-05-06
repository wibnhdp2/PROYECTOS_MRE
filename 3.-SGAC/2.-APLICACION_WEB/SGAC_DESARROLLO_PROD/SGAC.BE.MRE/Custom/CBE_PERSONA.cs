using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.Custom
{
    public class CBE_PERSONA : RE_PERSONA
    {
        public CBE_PERSONA()
        {
            this.IDENTIFICACION = new BE.RE_PERSONAIDENTIFICACION();
            this.REGISTROUNICO = new BE.RE_REGISTROUNICO();
            this.RESIDENCIAS = new List<BE.RE_RESIDENCIA>();
            this.FILIACIONES = new List<CBE_FILIACION>();
            this.acmi_sTipoDocumentoMigratorioId = 0;
            this.acmi_sMigratorioId = 0;
            this.Tipo = "";
        }

        public BE.RE_PERSONAIDENTIFICACION IDENTIFICACION { get; set; }
        public BE.RE_REGISTROUNICO REGISTROUNICO { get; set; }
        public List<BE.RE_RESIDENCIA> RESIDENCIAS { get; set; }
        public List<CBE_FILIACION> FILIACIONES { get; set; }
        public Int64 acmi_sTipoDocumentoMigratorioId { get; set; }
        public Int64 acmi_sMigratorioId { get; set; }
        public string Tipo { get; set; }
    }
}
