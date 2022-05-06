using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PLANTILLA_TRADUCCION : BaseBussinesObject
    {
        public Int64 pltr_iPlantillaTraduccionId { get; set; }
        public Int64 pltr_iEtiquetaId { get; set; }
        public Int16 pltr_sIdiomaId { get; set; }
        public string pltr_vTraduccion { get; set; }
        public string pltr_cEstado { get; set; }
        public Int16 pltr_sUsuarioCreacion { get; set; }
        public string pltr_vIPCreacion { get; set; }
        public DateTime pltr_dFechaCreacion { get; set; }
        public Int16 pltr_sUsuarioModificacion { get; set; }
        public string pltr_vIPModificacion { get; set; }
        public DateTime pltr_dFechaModificacion { get; set; }

    }
}
