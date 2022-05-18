using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_VENTANILLA
    {
        public short vent_sVentanillaId { get; set; }
        public Nullable<short> vent_sOficinaConsularId { get; set; }
        public string vent_vDescripcion { get; set; }
        public Nullable<int> vent_INumeroOrden { get; set; }
        public Nullable<short> vent_sSectorId { get; set; }
        public string vent_cEstado { get; set; }
        public short vent_sUsuarioCreacion { get; set; }
        public string vent_vIPCreacion { get; set; }
        public System.DateTime vent_dFechaCreacion { get; set; }
        public Nullable<short> vent_sUsuarioModificacion { get; set; }
        public string vent_vIPModificacion { get; set; }
        public Nullable<System.DateTime> vent_dFechaModificacion { get; set; }
    }
}
