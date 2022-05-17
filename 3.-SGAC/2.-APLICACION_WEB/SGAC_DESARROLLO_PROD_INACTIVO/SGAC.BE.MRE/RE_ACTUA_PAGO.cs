using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTUA_PAGO : BaseBussinesObject
    {
        public Int64 AP_iAJparticipanteId { get; set; }
        public Int64 AP_iTarifarioId { get; set; }

        public Int64 AP_sPagoTipoId { get; set; }

        public Int64 AP_iPersonaRecurrenteId { get; set; }
        public Int64 AP_iEmpresaRecurrenteId { get; set; }

        public DateTime AP_dFechaOperacion { get; set; }
        public Int16 AP_sMonedaLocalId { get; set; }
        public Double AP_FMontoMonedaLocal { get; set; }
        public Double AP_FMontoSolesConsulares { get; set; }
        public Double AP_FTipCambioBancario { get; set; }
        public Double AP_FTipCambioConsular { get; set; }
        public Int16? AP_sBancoId { get; set; }
        public string AP_vBancoNumeroOperacion { get; set; }
        public Boolean AP_bPagadoFlag { get; set; }
        public string AP_vComentario { get; set; }
        public string AP_vNumeroVoucher { get; set; }

        public string AP_cEstado { get; set; }
        public Int16 AP_sUsuarioCreacion { get; set; }
        public string AP_vIPCreacion { get; set; }
        public DateTime? AP_dFechaCreacion { get; set; }
        public Int16? AP_sUsuarioModificacion { get; set; }
        public string AP_vIPModificacion { get; set; }
        public DateTime? AP_dFechaModificacion { get; set; }
                
        public Boolean Error { get; set; }
        public string Message { get; set; }
    }
}
