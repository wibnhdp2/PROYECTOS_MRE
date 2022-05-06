using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_PERSONAIDENTIFICACION:BaseBussinesObject
    {
        public Int64 peid_iPersonaIdentificacionId { get; set; }
        public Int64 peid_iPersonaId { get; set; }
        public Int16 peid_sDocumentoTipoId { get; set; }
        public String peid_vDocumentoNumero { get; set; }
        public DateTime? peid_dFecVcto { get; set; }
        public DateTime? peid_dFecExpedicion { get; set; }
        public String peid_vLugarExpedicion { get; set; }
        public DateTime? peid_dFecRenovacion { get; set; }
        public String peid_vLugarRenovacion { get; set; }
        public bool peid_bActivoEnRune { get; set; }
        public String peid_cEstado { get; set; }
	public bool pers_bValidacionReniec { get; set; }

        public Int16 peid_sUsuarioCreacion { get; set; }
        public String peid_vIPCreacion { get; set; }
        public DateTime? peid_dFechaCreacion { get; set; }
        public Int16 peid_sUsuarioModificacion { get; set; }
        public String peid_vIPModificacion { get; set; }
        public DateTime? peid_dFechaModificacion { get; set; }
        public String pers_vApellidoPaterno { get; set; }
        public String pers_vApellidoMaterno { get; set; }
        public String pers_vNombres { get; set; }
        public Int16 pers_sNacionalidadId { get; set; }
        public string peid_vTipodocumento { get; set; } 
    }
}
