using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CL_PERFILATENCION : BaseBussinesObject
    {
        public int peat_IPerfilId { get; set; }
        public Nullable<short> peat_sOficinaConsularId { get; set; }
        public string peat_vDescripcion { get; set; }
        public Nullable<int> peat_INumeroTicket { get; set; }
        public Nullable<TimeSpan> peat_ITiempoAtencion { get; set; }
        public string peat_cEstado { get; set; }
        public short peat_sUsuarioCreacion { get; set; }
        public string peat_vIPCreacion { get; set; }
        public DateTime peat_dFechaCreacion { get; set; }
        public Nullable<short> peat_sUsuarioModificacion { get; set; }
        public string peat_vIPModificacion { get; set; }
        public Nullable<DateTime> peat_dFechaModificacion { get; set; }
    }
}
