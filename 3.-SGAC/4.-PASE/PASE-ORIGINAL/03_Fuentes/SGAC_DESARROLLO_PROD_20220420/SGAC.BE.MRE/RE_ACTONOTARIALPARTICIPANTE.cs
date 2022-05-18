using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIALPARTICIPANTE : BaseBussinesObject
    {
        public Int64 anpa_iActoNotarialParticipanteId { get; set; }
        public Int64 anpa_iActoNotarialId { get; set; }
        public Int64 anpa_iPersonaId { get; set; }
        public Int64 anpa_iEmpresaId { get; set; }
        public Int16 anpa_sTipoParticipanteId { get; set; }
        public Nullable<Int64> anpa_iReferenciaParticipanteId { get; set; }
        public Boolean anpa_bFlagFirma { get; set; }
        public Boolean anpa_bFlagHuella { get; set; }
        public string anpa_cEstado { get; set; }
        public Int16 anpa_sUsuarioCreacion { get; set; }
        public string anpa_vIPCreacion { get; set; }
        public DateTime anpa_dFechaCreacion { get; set; }
        public Int16 anpa_sUsuarioModificacion { get; set; }
        public string anpa_vIPModificacion { get; set; }
        public DateTime anpa_dFechaModificacion { get; set; }

        public string anpa_vCampoAuxiliar { get; set; }

        public string anpa_vNumeroEscrituraPublica { get; set; }
        public string anpa_vNumeroPartida { get; set; }
        public DateTime anpa_dFechaSuscripcion { get; set; }
        //--------------------------------------------------------
        public string TipoParticipante { get; set; }
        public Int16 IdiomaId { get; set; }
        public string Idioma { get; set; }
    }
}
