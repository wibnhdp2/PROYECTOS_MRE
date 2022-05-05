using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_TARIFARIOREQUISITO
    {        
        public short tare_sTarifarioRequisitoId {get; set;}        
        public short tare_sTarifarioId {get; set;}        
        public short tare_sRequisitoId {get; set;}        
        public Nullable<short> tare_sTipoActaId {get; set;}        
        public Nullable<short> tare_sCondicionId {get; set;}        
        public string tare_cEstado {get; set;}        
        public short tare_sUsuarioCreacion {get; set;}        
        public string tare_vIPCreacion {get; set;}        
        public System.DateTime tare_dFechaCreacion {get; set;}        
        public Nullable<short> tare_sUsuarioModificacion {get; set;}        
        public string tare_vIPModificacion {get; set;}
        public Nullable<System.DateTime> tare_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }       
    }
}
