using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
   public class RE_REGISTROCIVIL:BaseBussinesObject
    {

        public Int64 reci_iRegistroCivilId { get; set; }
        public Int64 reci_iActuacionDetalleId { get; set; }
        public Int16 reci_sTipoActaId { get; set; }
        public String reci_vNumeroCUI { get; set; }
        public String reci_vNumeroActa { get; set; }
        public DateTime? reci_dFechaRegistro { get; set; }
        public String reci_cOficinaRegistralUbigeo { get; set; }
        public Nullable<Int32> reci_IOficinaRegistralCentroPobladoId { get; set; }
        public DateTime? reci_dFechaHoraOcurrenciaActo { get; set; }
        public Int16 reci_sOcurrenciaTipoId { get; set; }
        public String reci_vOcurrenciaLugar { get; set; }
        public String reci_cOcurrenciaUbigeo { get; set; }
        public Nullable<Int32> reci_IOcurrenciaCentroPobladoId { get; set; }
        public String reci_vNumeroExpedienteMatrimonio { get; set; }
        public Int32 reci_IAprobacionUsuarioId { get; set; }
        public String reci_vIPAprobacion { get; set; }
        public DateTime? reci_dFechaAprobacion { get; set; }
        public Boolean reci_bDigitalizadoFlag { get; set; }
        public String reci_vCargoCelebrante { get; set; }
        public String reci_vLibro { get; set; }
        public Boolean reci_bAnotacionFlag { get; set; }
        public String reci_vObservaciones { get; set; }
        public String reci_cEstado { get; set; }
        public Int16 reci_sUsuarioCreacion { get; set; }
        public String reci_vIPCreacion { get; set; }
        public DateTime? reci_dFechaCreacion { get; set; }
        public Int16 reci_sUsuarioModificacion { get; set; }

        public String reci_vIPModificacion { get; set; }
        public DateTime? reci_dFechaModificacion { get; set; }

        public Int32 acde_iReferenciaId { get; set; }
        public Int32 reci_iRegistroCivilId_iReferenciaId { get; set; }
        public String reci_cConCUI { get; set; }
        public String reci_cReconocimientoAdopcion { get; set; }
        public String reci_cReconstitucionReposicion { get; set; }
        public Nullable<Int32> reci_iNumeroActaAnterior { get; set; }
        public String reci_vTitular { get; set; }
        public Boolean reci_bInscripcionOficio { get; set; }
    }
}
