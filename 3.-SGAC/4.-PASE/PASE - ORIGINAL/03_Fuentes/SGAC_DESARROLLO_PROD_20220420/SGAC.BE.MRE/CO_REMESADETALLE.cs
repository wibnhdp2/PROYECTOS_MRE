using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CO_REMESADETALLE
    {        
        public long rede_iRemesaDetalleId {get; set;}        
        public long rede_iRemesaId {get; set;}        
        public short rede_sTipoId {get; set;}        
        public string rede_cPeriodo {get; set;}        
        public System.DateTime rede_dFechaEnvio {get; set;}
        public double rede_FTipoCambioBancario {get; set;}       
        public double rede_FTipoCambioConsular {get; set;}
        public short rede_sCuentaCorrienteId { get; set; }
        public short rede_sMonedaId {get; set;}
        public double rede_FMonto {get; set;}        
        public string rede_vNroVoucher {get; set;}        
        public string rede_vResponsableEnvio {get; set;}        
        public string rede_vRecurrente {get; set;}        
        public Nullable<short> rede_sTarifarioId {get; set;}        
        public string rede_vObservacion {get; set;}       
        public bool rede_bMesFlag {get; set;}        
        public short rede_sEstadoId {get; set;}        
        public short rede_sUsuarioCreacion {get; set;}       
        public string rede_vIPCreacion {get; set;}        
        public System.DateTime rede_dFechaCreacion {get; set;}        
        public Nullable<short> rede_sUsuarioModificacion {get; set;}        
        public string rede_vIPModificacion {get; set;}
        public Nullable<System.DateTime> rede_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
        public short ClasificacionID { get; set; }
    }
}
