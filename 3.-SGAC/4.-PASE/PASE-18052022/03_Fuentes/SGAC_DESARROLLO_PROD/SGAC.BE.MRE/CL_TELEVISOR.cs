using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_TELEVISOR
    {        
        public short telv_sTelevisorId { get; set; }         
        public Nullable<short> telv_sOficinaConsularId { get; set; }         
        public string telv_vDescripcion { get; set; }         
        public string telv_vMarca { get; set; }        
        public string telv_vModelo { get; set; }        
        public string telv_vSerie { get; set; }         
        public string telv_vCaracteristicas { get; set; }        
        public string telv_cEstado { get; set; }         
        public short telv_sUsuarioCreacion { get; set; }        
        public string telv_vIPCreacion { get; set; }        
        public System.DateTime telv_dFechaCreacion { get; set; }        
        public Nullable<short> telv_sUsuarioModificacion { get; set; }        
        public string telv_vIPModificacion { get; set; }
        public Nullable<System.DateTime> telv_dFechaModificacion { get; set; }         
    }
}
