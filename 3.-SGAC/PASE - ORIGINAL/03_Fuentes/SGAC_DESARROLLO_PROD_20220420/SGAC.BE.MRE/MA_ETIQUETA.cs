using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class MA_ETIQUETA : BaseBussinesObject
    {
        public Int64 etiq_iEtiquetaId { get; set; }
        public Int16 etiq_sPlantillaId { get; set; }
        public Int16 etiq_tOrden { get; set; }
        public string etiq_vEtiqueta { get; set; }
        public string etiq_cEstado { get; set; }
        public Int16 etiq_sUsuarioCreacion { get; set; }
        public string etiq_vIPCreacion { get; set; }
        public DateTime etiq_dFechaCreacion { get; set; }
        public Int16 etiq_sUsuarioModificacion { get; set; }
        public string etiq_vIPModificacion { get; set; }
        public DateTime etiq_dFechaModificacion { get; set; }
    }
}
