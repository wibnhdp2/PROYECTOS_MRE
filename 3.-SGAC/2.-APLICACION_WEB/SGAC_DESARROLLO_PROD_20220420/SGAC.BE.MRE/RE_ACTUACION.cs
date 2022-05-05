using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTUACION : BaseBussinesObject
    {
        public Int64 actu_iActuacionId { get; set; }
        public Int16 actu_sOficinaConsularId { get; set; }
        public Double actu_FCantidad { get; set; }
        public Int64? actu_iPersonaRecurrenteId { get; set; }
        public Int64? actu_iEmpresaRecurrenteId { get; set; }
        public Int32 actu_IFuncionarioId { get; set; }
        public DateTime actu_dFechaRegistro { get; set; }
        public Int16 actu_sEstado { get; set; }
        public Int16 actu_sUsuarioCreacion { get; set; }
        public string actu_vIPCreacion { get; set; }
        public DateTime actu_dFechaCreacion { get; set; }
        public Int16 actu_sUsuarioModificacion { get; set; }
        public string actu_vIPModificacion { get; set; }
        public DateTime actu_dFechaModificacion { get; set; }
        public Int16 actu_sCiudadItinerante { get; set; }
    }
}
