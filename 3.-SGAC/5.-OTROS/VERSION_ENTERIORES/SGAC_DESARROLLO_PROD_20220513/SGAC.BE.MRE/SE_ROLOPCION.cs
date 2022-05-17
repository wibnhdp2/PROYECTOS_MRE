using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SE_ROLOPCION
    {        
        public short roop_sRolOpcionId {get; set;}        
        public short roop_sFormularioId {get; set;}        
        public string roop_vAcciones {get; set;}        
        public string roop_cEstado {get; set;}        
        public short roop_sUsuarioCreacion {get; set;}        
        public string roop_vIPCreacion {get; set;}        
        public System.DateTime roop_dFechaCreacion {get; set;}        
        public Nullable<short> roop_sUsuarioModificacion {get; set;}        
        public string roop_vIPModificacion {get; set;}
        public Nullable<System.DateTime> roop_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
    }
}
