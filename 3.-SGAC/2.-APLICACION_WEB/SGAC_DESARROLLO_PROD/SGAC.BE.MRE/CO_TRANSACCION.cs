using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CO_TRANSACCION
    {       
        public long tran_iTransaccionId {get; set;}       
        public short tran_sCuentaCorrienteId {get; set;}        
        public short tran_sOficinaConsularId {get; set;}       
        public short tran_sMovimientoTipoId {get; set;}        
        public short tran_sTipoId {get; set;}        
        public short tran_sMonedaId {get; set;}       
        public double tran_FMonto {get; set;}        
        public double tran_FSaldo {get; set;}        
        public string tran_vNumeroOperacion {get; set;}        
        public System.DateTime tran_dFechaOperacion {get; set;}        
        public string tran_cPeriodo {get; set;}        
        public string tran_vFuncionarioEnvio {get; set;}       
        public string tran_vObservacion {get; set;}        
        public string tran_cEstado {get; set;}        
        public short tran_sUsuarioCreacion {get; set;}        
        public string tran_vIPCreacion {get; set;}        
        public System.DateTime tran_dFechaCreacion {get; set;}        
        public Nullable<short> tran_sUsuarioModificacion {get; set;}        
        public string tran_vIPModificacion {get; set;}
        public Nullable<System.DateTime> tran_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
        public short tran_sOficinaDepenConsularId { get; set; }
        public double tran_FMontoRREE { get; set; }
        public double tran_FMontoElectoral { get; set; }
        public double tran_FMontoPagadoLima { get; set; }
        public double tran_FMontoMilitar { get; set; }
        public double tran_FRetencion { get; set; }
        public string tran_cPeriodoAnterior { get; set; }
        public Int16 tran_sEstadoDepositoConciliacion { get; set; }
        public Nullable<System.DateTime> tran_dFechaConciliacion { get; set; }
    }
}
