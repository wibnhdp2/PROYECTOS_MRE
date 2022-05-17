using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ACTONOTARIALDETALLE : BaseBussinesObject
    {
        public RE_ACTONOTARIALDETALLE() { }

        public Int64 ande_iActoNotarialDetalleId { get; set; }
        public Int64 ande_iActuacionDetalleId { get; set; }
        public Int16 ande_sOficinaConsularId { get; set; }
        public Int16 ande_sTipoFormatoId { get; set; }
        public Int32 ande_IFuncionarioAutorizadorId { get; set; }
        public string ande_vNumeroOficio { get; set; }
        public Int16 ande_sNumeroFoja { get; set; }
        public string ande_vPresentanteNombre { get; set; }
        public Int16 ande_sPresentanteTipoDocumento { get; set; }
        public string ande_vPresentanteNumeroDocumento { get; set; }
        public Int16 ande_sPresentanteGenero { get; set; }
        public DateTime ande_dFechaExtension { get; set; }
        public Int16 ande_sCorrelativo { get; set; }
        public string ande_cEstado { get; set; }
        public Int16 ande_sUsuarioCreacion { get; set; }
        public String ande_vIPCreacion { get; set; }
        public DateTime ande_dFechaCreacion { get; set; }
        public Int16 ande_sUsuarioModificacion { get; set; }
        public String ande_vIPModificacion { get; set; }
        public DateTime ande_dFechaModificacion { get; set; }
        public Int64 ande_iActoNotarialId { get; set; }
    }
}
