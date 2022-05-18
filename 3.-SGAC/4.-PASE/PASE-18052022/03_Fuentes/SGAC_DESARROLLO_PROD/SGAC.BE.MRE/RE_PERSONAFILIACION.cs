using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PERSONAFILIACION : BaseBussinesObject
    {
        public RE_PERSONAFILIACION()
        {
            this.PERSONA = new RE_PERSONA();
        }

        public Int64 pefi_iPersonaFilacionId { get; set; }
        public Int64 pefi_iPersonaId { get; set; }
        public String pefi_vNombreFiliacion { get; set; }
        public String pefi_vLugarNacimiento { get; set; }
        public DateTime pefi_dFechaNacimiento { get; set; }
        public Int16 pefi_sNacionalidad { get; set; }
        public Int16 pefi_sTipoFilacionId { get; set; }
        public String pefi_vNroDocumento { get; set; }
        public String pefi_cEstado { get; set; }
        public Int16 pefi_sUsuarioCreacion { get; set; }
        public String pefi_vIPCreacion { get; set; }
        public DateTime pefi_dFechaCreacion { get; set; }
        public Int16 pefi_sUsuarioModificacion { get; set; }
        public String pefi_vIPModificacion { get; set; }
        public DateTime pefi_dFechaModificacion { get; set; }

        public BE.MRE.RE_PERSONA PERSONA { get; set; }
    }
}
