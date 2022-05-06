using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SE_ROLCONFIGURACION
    {       
        public short roco_sRolConfiguracionId {get; set;}        
        public short roco_sAplicacionId {get; set;}        
        public short roco_sRolTipoId {get; set;}        
        public string roco_vRolOpcion {get; set;}        
        public string roco_vNombre {get; set;}        
        public string roco_cHorario {get; set;}        
        public string roco_cEstado {get; set;}        
        public short roco_sUsuarioCreacion {get; set;}        
        public string roco_vIPCreacion {get; set;}        
        public System.DateTime roco_dFechaCreacion {get; set;}        
        public Nullable<short> roco_sUsuarioModificacion {get; set;}        
        public string roco_vIPModificacion {get; set;}
        public Nullable<System.DateTime> roco_dFechaModificacion { get; set; }
        public short OficinaConsularId { get; set; }
    }
}
