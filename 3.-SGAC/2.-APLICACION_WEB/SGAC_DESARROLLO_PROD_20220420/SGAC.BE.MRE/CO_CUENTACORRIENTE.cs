using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CO_CUENTACORRIENTE : BaseBussinesObject
    {       
        public short cuco_sCuentaCorrienteId { get; set; }        
        public short cuco_sOficinaConsularId { get; set; }        
        public Nullable<short> cuco_sTipoId { get; set; }        
        public short cuco_sBancoId { get; set; }        
        public short cuco_sMonedaId { get; set; }        
        public string cuco_vNumero { get; set; }
        public string cuco_vRepresentante { get; set; }
        public string cuco_vSucursal { get; set; }        
        public short cuco_sSituacionId { get; set; }        
        public string cuco_vObservacion { get; set; }        
        public string cuco_cEstado { get; set; }        
        public short cuco_sUsuarioCreacion { get; set; }
        public string cuco_vIPCreacion { get; set; }        
        public System.DateTime cuco_dFechaCreacion { get; set; }       
        public Nullable<short> cuco_sUsuarioModificacion { get; set; }
        public string cuco_vIPModificacion { get; set; }
        public Nullable<System.DateTime> cuco_dFechaModificacion { get; set; }        
    }
}
