using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PAGO : BaseBussinesObject
    {
        public Int64 pago_iPagoId { get; set; }
        public Int64 pago_iActuacionDetalleId { get; set; }
        public Int16 pago_sPagoTipoId { get; set; }
        public DateTime pago_dFechaOperacion { get; set; }
        public Int16 pago_sMonedaLocalId { get; set; }
        public Double pago_FMontoMonedaLocal { get; set; }
        public Double pago_FMontoSolesConsulares { get; set; }
        public Double pago_FTipCambioBancario { get; set; }
        public Double pago_FTipCambioConsular { get; set; }
        public Int16? pago_sBancoId { get; set; }
        public string pago_vBancoNumeroOperacion { get; set; }
        public Boolean pago_bPagadoFlag { get; set; }
        public string pago_vComentario { get; set; }
        public string pago_vNumeroVoucher { get; set; }
        public string pago_cEstado { get; set; }
        public Int16 pago_sUsuarioCreacion { get; set; }
        public string pago_vIPCreacion { get; set; }
        public DateTime? pago_dFechaCreacion { get; set; }
        public Int16? pago_sUsuarioModificacion { get; set; }
        public string pago_vIPModificacion { get; set; }
        public DateTime? pago_dFechaModificacion { get; set; }
        public string pago_vSustentoTipoPago { get; set; }
        public Int64 pago_iNormaTarifarioId { get; set; }        
    }
}
