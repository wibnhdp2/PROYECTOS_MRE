using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOJUDICIALNOTIFICACION : BaseBussinesObject
    {
        public long ajno_iActoJudicialNotificacionId { get; set; }
        public long ajno_iActoJudicialParticipanteId { get; set; }
        public short? ajno_sTipoRecepcionId { get; set; }
        public short ajno_sViaEnvioId { get; set; }
        public string ajno_vEmpresaServicioPostal { get; set; }
        public string ajno_vPersonaNotificacion { get; set; }
        public DateTime ajno_dFechaHoraNotificacion { get; set; }
        public string ajno_vNumeroCedula { get; set; }
        public string ajno_vPersonaRecibeNotificacion { get; set; }
        public Nullable<DateTime> ajno_dFechaHoraRecepcion { get; set; }
        public string ajno_vCuerpoNotificacion { get; set; }
        public string ajno_vObservaciones { get; set; }
        public short ajno_sEstadoId { get; set; }
        public short ajno_sUsuarioCreacion { get; set; }
        public string ajno_vIPCreacion { get; set; }
        public DateTime ajno_dFechaCreacion { get; set; }
        public Nullable<short> ajno_sUsuarioModificacion { get; set; }
        public string ajno_vIPModificacion { get; set; }
        public Nullable<DateTime> ajno_dFechaModificacion { get; set; }
    }
}
