using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SE_USUARIOROL
    {        
        public short usro_sUsuarioRolId {get; set;}        
        public short usro_sUsuarioId {get; set;}        
        public Nullable<short> usro_sGrupoId {get; set;}        
        public Nullable<short> usro_sRolConfiguracionId {get; set;}        
        public short usro_sOficinaConsularId {get; set;}        
        public short usro_sAcceso {get; set;}        
        public string usro_cEstado {get; set;}        
        public short usro_sUsuarioCreacion {get; set;}        
        public string usro_vIPCreacion {get; set;}        
        public System.DateTime usro_dFechaCreacion {get; set;}        
        public Nullable<short> usro_sUsuarioModificacion {get; set;}        
        public string usro_vIPModificacion {get; set;}
        public Nullable<System.DateTime> usro_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
    }
}
