using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE.SC_MAESTRO
{
    public class MA_DOCUMENTO_IDENTIDAD: BaseBussinesObject
    {
        public Int16    doid_sTipoDocumentoIdentidadId	{ get; set; }
        public String   doid_vDescripcionCorta { get; set; }
        public String   doid_vDescripcionLarga { get; set; }
        public String   doid_cEstado { get; set; }
        public Int16    doid_sUsuarioCreacion { get; set; }
        public String   doid_vIPCreacion { get; set; }
        public DateTime doid_dFechaCreacion { get; set; }
        public Int16    doid_sUsuarioModificacion { get; set; }
        public String   doid_vIPModificacion { get; set; }
        public DateTime doid_dFechaModificacion { get; set; }
    }
}
