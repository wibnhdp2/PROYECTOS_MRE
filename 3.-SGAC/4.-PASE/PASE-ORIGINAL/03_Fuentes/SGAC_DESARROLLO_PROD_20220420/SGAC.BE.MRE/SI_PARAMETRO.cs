using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_PARAMETRO:BaseBussinesObject
    {
        public SI_PARAMETRO() { }
        public SI_PARAMETRO(short sParametroId) {
            this.para_sParametroId = sParametroId;
        }

        public short para_sParametroId { get; set; }
        public string para_vGrupo { get; set; }        
        public string para_vDescripcion { get; set; }       
        public string para_vValor { get; set; }        
        public string para_vReferencia { get; set; }        
        public Nullable<byte> para_tOrden { get; set; }        
        public bool para_bVisible { get; set; }       
        public Nullable<System.DateTime> para_dVigenciaInicio { get; set; }       
        public Nullable<System.DateTime> para_dVigenciaFin { get; set; }        
        public bool para_bPrecarga { get; set; }        
        public string para_cEstado { get; set; }        
        public short para_sUsuarioCreacion { get; set; }       
        public string para_vIPCreacion { get; set; }        
        public System.DateTime para_dFechaCreacion { get; set; }        
        public Nullable<short> para_sUsuarioModificacion { get; set; }       
        public string para_vIPModificacion { get; set; }
        public Nullable<System.DateTime> para_dFechaModificacion { get; set; }

        public bool para_bFlagExcepcion { get; set; }
    }
}
