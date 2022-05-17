using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PERSONARESIDENCIA : BaseBussinesObject
    {
        public RE_PERSONARESIDENCIA()
        {
            this.Residencia = new RE_RESIDENCIA();
        }

        public Int64 pere_iPersonaId { get; set; }
        public Int64 pere_iResidenciaId { get; set; }
        public String pere_cEstado { get; set; }
        public Int16 pere_sUsuarioCreacion { get; set; }
        public String pere_vIPCreacion { get; set; }
        public DateTime pere_dFechaCreacion { get; set; }
        public Int16 pere_sUsuarioModificacion { get; set; }
        public String pere_vIPModificacion { get; set; }
        public DateTime pere_dFechaModificacion { get; set; }

        //public Int16 OficinaConsularId { get; set; }

        public RE_RESIDENCIA Residencia { get; set; }   
    }
}
