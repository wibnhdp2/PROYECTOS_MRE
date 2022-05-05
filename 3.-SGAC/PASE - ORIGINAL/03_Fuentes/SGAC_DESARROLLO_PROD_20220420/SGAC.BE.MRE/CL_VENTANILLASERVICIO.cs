using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_VENTANILLASERVICIO : BaseBussinesObject
    {
        public short vede_sVentanillaId { get; set; }
        public Nullable<short> vent_sOficinaConsularId { get; set; }
        public short vede_sServicioId { get; set; }
        public Nullable<int> vede_IObligatorio { get; set; }
        public string vede_cEstado { get; set; }
        public short vede_sUsuarioCreacion { get; set; }
        public string vede_vIPCreacion { get; set; }
        public System.DateTime vede_dFechaCreacion { get; set; }
        public Nullable<short> vede_sUsuarioModificacion { get; set; }
        public string vede_vIPModificacion { get; set; }
        public Nullable<System.DateTime> vede_dFechaModificacion { get; set; }
    }
}
