using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTUACIONDETALLE : BaseBussinesObject
    {
        public Int64 acde_iActuacionDetalleId { get; set; }
        public Int64 acde_iActuacionId { get; set; }
        public Int16 acde_sTarifarioId { get; set; }
        public Int16 acde_sItem { get; set; }
        public DateTime acde_dFechaRegistro { get; set; }
        public Boolean acde_bRequisitosFlag { get; set; }
        public Int32 acde_ICorrelativoActuacion { get; set; }
        public Int32 acde_ICorrelativoTarifario { get; set; }
        public Int32 acde_IFuncionarioFirmanteId { get; set; }
        public Int32 acde_IFuncionarioContactoId { get; set; }
        public Int32 acde_IImpresionFuncionarioId { get; set; }
        public string acde_vNotas { get; set; }
        public Int32 acde_IFuncionarioAnulaId { get; set; }
        public string acde_vMotivoAnulacion { get; set; }
        public Int64? acde_iReferenciaId { get; set; }
        public Int16 acde_sEstadoId { get; set; }
        public Int16 acde_sUsuarioCreacion { get; set; }
        public string acde_vIPCreacion { get; set; }
        public DateTime acde_dFechaCreacion { get; set; }
        public Int16? acde_sUsuarioModificacion { get; set; }
        public string acde_vIPModificacion { get; set; }
        public DateTime? acde_dFechaModificacion { get; set; }
        
        //Para la vinculacion
        public Int32 intInsumoTipoId { get; set; }
        public string Insumo { get; set; }
        public Int16 sOficinaConsultaID { get; set; }

    }
}
