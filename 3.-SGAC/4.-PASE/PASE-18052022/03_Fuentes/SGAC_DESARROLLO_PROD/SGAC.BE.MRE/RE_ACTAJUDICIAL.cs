using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTAJUDICIAL : BaseBussinesObject
    {
        public long acjd_iActaJudicialId { get; set; }
        public long acjd_iActoJudicialNotificacionId { get; set; }
        public short acjd_sTipoActaId { get; set; }
        public int acjd_IFuncionarioFirmanteId { get; set; }
        public DateTime acjd_dFechaHoraActa { get; set; }
        public string acjd_vCuerpoActa { get; set; }
        public string acjd_vResultado { get; set; }
        public string acjd_vObservaciones { get; set; }
        public short acjd_sEstadoId { get; set; }
        public short acjd_sUsuarioCreacion { get; set; }
        public string acjd_vIPCreacion { get; set; }
        public DateTime acjd_dFechaCreacion { get; set; }
        public short acjd_sUsuarioModificacion { get; set; }
        public string acjd_vIPModificacion { get; set; }
        public DateTime acjd_dFechaModificacion { get; set; }
        public string acjd_vResponsable { get; set; }
    }
}
