using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
     public class SI_TIPOCAMBIO_BANCARIO
    { 
        public long tiba_iTipoCambioBancarioId { get; set; }        
        public System.DateTime tiba_dFecha { get; set; }
        public System.DateTime tiba_dFechaFin { get; set; }    
        public double tiba_FValorBancario { get; set; }         
        public short tiba_sOficinaConsularId { get; set; }         
        public short tiba_sMonedaId { get; set; }        
        public string tiba_cEstado { get; set; }         
        public short tiba_sUsuarioCreacion { get; set; }         
        public string tiba_vIPCreacion { get; set; }         
        public System.DateTime tiba_dFechaCreacion { get; set; }         
        public Nullable<short> tiba_sUsuarioModificacion { get; set; }        
        public string tiba_vIPModificacion { get; set; }
        public Nullable<System.DateTime> tiba_dFechaModificacion { get; set; }         
    }
}
