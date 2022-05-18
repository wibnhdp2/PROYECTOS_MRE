using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_UBICACIONGEOGRAFICA
    {
        public string ubge_cCodigo { get; set; }
        public string ubge_cUbi01 { get; set; } 
        public string ubge_cUbi02 { get; set; }        
        public string ubge_cUbi03 { get; set; }        
        public string ubge_vDepartamento { get; set; }        
        public string ubge_vProvincia { get; set; }        
        public string ubge_vDistrito { get; set; }
        public string ubge_cEstado { get; set; }
        public short ubge_sUsuarioCreacion { get; set; }
        public string ubge_vIPCreacion { get; set; }
        public DateTime ubge_dFechaCreacion { get; set; }
        public Nullable<short> ubge_sUsuarioModificacion { get; set; }
        public string ubge_vIPModificacion { get; set; }
        public Nullable<System.DateTime> ubge_dFechaModificacion { get; set; }
        public string ubge_vSiglaPais { get; set; }
    }
}
