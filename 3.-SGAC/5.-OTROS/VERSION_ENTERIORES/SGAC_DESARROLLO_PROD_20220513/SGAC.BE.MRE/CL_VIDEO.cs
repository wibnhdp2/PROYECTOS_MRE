using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_VIDEO
    {
            
        public short vide_sVideoId { get; set; }       
        public short vide_sOficinaConsularId { get; set; }      
        public string vide_vDescripcion { get; set; }       
        public Nullable<int> vide_IOrden { get; set; }       
        public string vide_vUrl { get; set; }        
        public string vide_cEstado { get; set; }       
        public short vide_sUsuarioCreacion { get; set; }        
        public string vide_vIPCreacion { get; set; }        
        public System.DateTime vide_dFechaCreacion { get; set; }        
        public Nullable<short> vide_sUsuarioModificacion { get; set; }        
        public string vide_vIPModificacion { get; set; }
        public Nullable<System.DateTime> vide_dFechaModificacion { get; set; }       
    }
}
