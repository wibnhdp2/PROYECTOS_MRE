using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PERSONAFOTO : BaseBussinesObject
    {        
        public long pefo_iPersonaFotoId {get; set;}        
        public long pefo_iPersonaId {get; set;}        
        public short pefo_sFotoTipoId {get; set;}        
        public byte[] pefo_GFoto {get; set;}        
        public string pefo_cEstado {get; set;}        
        public short pefo_sUsuarioCreacion {get; set;}        
        public string pefo_vIPCreacion {get; set;}        
        public System.DateTime pefo_dFechaCreacion {get; set;}        
        public Nullable<short> pefo_sUsuarioModificacion {get; set;}        
        public string pefo_vIPModificacion {get; set;}
        public Nullable<System.DateTime> pefo_dFechaModificacion { get; set; }        
    }
}
