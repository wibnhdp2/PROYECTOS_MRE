using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIALSEGUIMIENTO : BaseBussinesObject
    {
        public Int64 anse_iActoNotarialSeguimientoId { get; set; }
        public Int64 anse_iActoNotarialId { get; set; }        
        public DateTime anse_dFechaRegistro { get; set; }
        public Int16 anse_sEstadoId { get; set; }
        public Int16 anse_sUsuarioCreacion { get; set; }
        public string anse_vIPCreacion { get; set; }
        public DateTime anse_dFechaCreacion { get; set; }
        public Int16 anse_sUsuarioModificacion { get; set; }
        public string anse_vIPModificacion { get; set; }
        public DateTime anse_dFechaModificacion { get; set; }
    }
}
