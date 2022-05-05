using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_FICHAREGISTRALPARTICIPANTE:BaseBussinesObject
    {
        public RE_FICHAREGISTRALPARTICIPANTE() {
            this.PERSONA = new RE_PERSONA();
        }

        public Int64 fipa_iFichaRegistralParticipanteId { get; set; }//	bigint
        public Int64 fipa_iFichaRegistralId { get; set; }//	bigint
        public Int32 fipa_sTipoParticipanteId { get; set; }//	smallint
        public Int64 fipa_iPersonaId { get; set; }//	bigint
        public string fipa_cEstadoId { get; set; }//	char
        public Int16 fipa_sUsuarioCreacion { get; set; }//		smallint
        public string fipa_vIPCreacion { get; set; }//	varchar
        public DateTime fipa_dFechaCreacion { get; set; }//	datetime
        public Int16 fipa_sUsuarioModificacion { get; set; }//	smallint
        public string fipa_vIPModificacion { get; set; }//	varchar
        public DateTime fipa_dFechaModificacion { get; set; }//	datetime
        public bool fipa_bConsignaApellidoMaterno { get; set; }
        public RE_PERSONA PERSONA { get; set; }
    }
}
