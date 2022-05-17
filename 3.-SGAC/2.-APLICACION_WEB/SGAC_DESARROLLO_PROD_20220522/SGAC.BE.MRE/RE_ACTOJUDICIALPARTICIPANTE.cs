using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOJUDICIALPARTICIPANTE : BaseBussinesObject
    {
        public long ajpa_iActoJudicialParticipanteId { get; set; }
        public long ajpa_iActoJudicialId { get; set; }
        public long? ajpa_iActuacionDetalleId { get; set; }
        public short ajpa_sTipoParticipanteId { get; set; }
        public short? ajpa_sOficinaConsularDestinoId { get; set; }
        public short ajpa_sTipoPersonaId { get; set; }
        public long? ajpa_iPersonaId { get; set; }
        public long? ajpa_iEmpresaId { get; set; }
        public DateTime? ajpa_dFechaAceptacionExpediente { get; set; }
        public DateTime? ajpa_dFechaLlegadaValija { get; set; }
        public short ajpa_sUsuarioCreacion { get; set; }
        public string ajpa_vIPCreacion { get; set; }
        public DateTime ajpa_dFechaCreacion { get; set; }
        public Nullable<short> ajpa_sUsuarioModificacion { get; set; }
        public string ajpa_vIPModificacion { get; set; }
        public Nullable<DateTime> ajpa_dFechaModificacion { get; set; }
        public short ajpa_sEstadoId { get; set; }
        public string ajpa_vNumeroHojaRemision { get; set; }
        public bool ajpa_bActaFlag { get; set; }
        public bool ajpa_bNotificacionFlag { get; set; }
    }
}
