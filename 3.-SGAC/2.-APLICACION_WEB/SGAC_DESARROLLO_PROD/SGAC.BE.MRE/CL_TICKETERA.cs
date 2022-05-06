using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_TICKETERA
    {
        
        public short tira_sTicketeraId { get; set; } 
        public short tira_sOficinaConsularId  { get; set; }        
        public string tira_vNombre { get; set; }         
        public string tira_vMarca { get; set; }        
        public string tira_vModelo { get; set; }        
        public string tira_vSerie { get; set; }         
        public string tira_vCaracteristicas { get; set; } 
        public string tira_cEstado { get; set; }       
        public short tira_sUsuarioCreacion { get; set; }        
        public string tira_vIPCreacion { get; set; }         
        public System.DateTime tira_dFechaCreacion { get; set; }        
        public Nullable<short> tira_sUsuarioModificacion { get; set; }        
        public string tira_vIPModificacion { get; set; }
        public Nullable<System.DateTime> tira_dFechaModificacion { get; set; }         
    }
}
