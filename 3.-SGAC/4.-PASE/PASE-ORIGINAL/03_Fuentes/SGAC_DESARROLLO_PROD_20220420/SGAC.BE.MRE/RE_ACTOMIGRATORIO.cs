using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTOMIGRATORIO: BaseBussinesObject
    {        
        public long acmi_iActoMigratorioId { get; set; }
        public long acmi_iActuacionDetalleId { get; set; }
        public int acmi_IFuncionarioId { get; set; }
        public short acmi_sTipoDocumentoMigratorioId { get; set; }
        public Nullable<short> acmi_sTipoId { get; set; }
        public Nullable<short> acmi_sSubTipoId { get; set; }
        public string acmi_vNumeroExpediente { get; set; }
        public string acmi_vNumeroLamina { get; set; }
        public DateTime acmi_dFechaExpedicion { get; set; }
        public DateTime acmi_dFechaExpiracion { get; set; }
        public string acmi_vNumeroDocumento { get; set; }
        public string acmi_vNumeroDocumentoAnterior { get; set; }
        public string acmi_vObservaciones { get; set; }
        public short acmi_sEstadoId { get; set; }
        public short acmi_sPaisId { get; set; }
        public short acmi_sUsuarioCreacion { get; set; }
        public string acmi_vIPCreacion { get; set; }
        public DateTime acmi_dFechaCreacion { get; set; }
        public Nullable<short> acmi_sUsuarioModificacion { get; set; }
        public string acmi_vIPModificacion { get; set; }
        public Nullable<DateTime> acmi_dFechaModificacion { get; set; }
        public Int64 acmi_iRecurrenteId { get; set; }
    }
}
