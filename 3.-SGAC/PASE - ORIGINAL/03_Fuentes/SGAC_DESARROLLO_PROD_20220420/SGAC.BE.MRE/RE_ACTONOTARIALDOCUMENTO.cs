using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
	public class RE_ACTONOTARIALDOCUMENTO: BaseBussinesObject
	{
        public RE_ACTONOTARIALDOCUMENTO() {}

        public Int64 ando_iActoNotarialDocumentoId { get; set; }
        public Int64 ando_iActoNotarialId { get; set; }
        public Int16 ando_sTipoDocumentoId { get; set; }
        public Int16 ando_sTipoInformacionId { get; set; }
        public Int16 ando_sSubTipoInformacionId { get; set; }
        public String ando_vDescripcion	{ get; set; }
        public String ando_vDetalleDocumento { get; set; }
        public String ando_vRutaArchivo { get; set; }
        public String ando_cEstado { get; set; }
        public Int16    ando_sUsuarioCreacion { get; set; }
        public String   ando_vIPCreacion { get; set; }
        public DateTime ando_dFechaCreacion { get; set; }
        public Int16    ando_sUsuarioModificacion { get; set; }
        public String   ando_vIPModificacion { get; set; }
        public DateTime ando_dFechaModificacion { get; set; }
        public String ando_vUbicacion { get; set; }   
	}
}
