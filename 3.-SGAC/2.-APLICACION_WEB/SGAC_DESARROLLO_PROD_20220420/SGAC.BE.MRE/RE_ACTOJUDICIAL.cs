using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOJUDICIAL : BaseBussinesObject
    {
        public long acju_iActoJudicialId { get; set; }
        public long acju_iActuacionId { get; set; }
        public short acju_sTipoNotificacion { get; set; }
        public short acju_sEntidadSolicitanteId { get; set; }
        public DateTime? acju_dFechaRecepcion { get; set; }
        public DateTime? acju_dFechaAudiencia { get; set; }
        //public DateTime acju_dFechaCitacion { get; set; }
        public DateTime acju_dFechaRegistro { get; set; }
        public string acju_vJuzgado { get; set; }
        public string acju_vOrganoJudicial { get; set; }
        public string acju_vNumeroExpediente { get; set; }
        //public string acju_vNumeroHojaRemision { get; set; }
        public string acju_vMateriaDemanda { get; set; }
        public string acju_vNumeroOficio { get; set; }
        public string acju_vObservaciones { get; set; }
        public short acju_sEstadoId { get; set; }
        public short acju_sUsuarioCreacion { get; set; }
        public string acju_vIPCreacion { get; set; }
        public DateTime acju_dFechaCreacion { get; set; }
        public Nullable<short> acju_sUsuarioModificacion { get; set; }
        public string acju_vIPModificacion { get; set; }
        public Nullable<DateTime> acju_dFechaModificacion { get; set; }
    }
}
