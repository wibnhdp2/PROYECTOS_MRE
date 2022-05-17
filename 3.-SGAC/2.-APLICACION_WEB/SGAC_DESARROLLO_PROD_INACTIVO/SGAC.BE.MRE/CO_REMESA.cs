using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CO_REMESA
    {        
        public long reme_iRemesaId {get; set;}      
        public Nullable<short> reme_sTipoId {get; set;}      
        public short reme_sOficinaConsularOrigenId {get; set;}       
        public Nullable<short> reme_sOficinaConsularDestinoId {get; set;}        
        public string reme_cPeriodo {get; set;}        
        public Nullable<short> reme_sCuentaCorrienteId {get; set;}        
        public string reme_vNumeroEnvio {get; set;}       
        public Nullable<short> reme_sMonedaId {get; set;}
        public Nullable<double> reme_FMonto {get; set;}       
        public Nullable<System.DateTime> reme_dFechaEnvio {get; set;}        
        public string reme_vResponsableEnvio {get; set;}       
        public string reme_vObservacion {get; set;}        
        public short reme_sEstadoId {get; set;}        
        public short reme_sUsuarioCreacion {get; set;}        
        public string reme_vIPCreacion {get; set;}        
        public System.DateTime reme_dFechaCreacion {get; set;}        
        public Nullable<short> reme_sUsuarioModificacion {get; set;}        
        public string reme_vIPModificacion {get; set;}
        public Nullable<System.DateTime> reme_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }  
    }
}
