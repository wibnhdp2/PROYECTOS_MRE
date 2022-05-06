using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_TIPOCAMBIO_CONSULAR
    {
        public long tico_iTipoCambioConsularId { get; set; }       
        public System.DateTime tico_dFecha { get; set; }
        public System.DateTime tico_dFechaFin { get; set; } 
        public double tico_FValorBancario { get; set; }       
        public double tico_FPorcentaje { get; set; }
        public double tico_FPromedio { get; set; }
        public double tico_FValorConsular { get; set; }       
        public short tico_sOficinaConsularId { get; set; }      
        public short tico_sMonedaId { get; set; }       
        public string tico_cEstado { get; set; }       
        public short tico_sUsuarioCreacion { get; set; }        
        public string tico_vIPCreacion { get; set; }        
        public System.DateTime tico_dFechaCreacion  { get; set; }        
        public Nullable<short> tico_sUsuarioModificacion  { get; set; }       
        public string tico_vIPModificacion  { get; set; }       
        public Nullable<System.DateTime> tico_dFechaModificacion  { get; set; }
        public Int16 DiferenciaHoraria { get; set; }
        public Int16 HorarioVerano { get; set; }
        
    }
}
