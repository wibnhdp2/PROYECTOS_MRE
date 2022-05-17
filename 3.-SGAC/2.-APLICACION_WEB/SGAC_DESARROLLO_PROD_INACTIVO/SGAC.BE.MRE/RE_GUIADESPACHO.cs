using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_GUIADESPACHO : BaseBussinesObject
    {
        public RE_GUIADESPACHO() { }

        public Int64 gude_iGuiaDespachoId { get; set; }//	bigint
        public DateTime gude_dFechaEnvio { get; set; }//	datetime
        public Int16 gude_sTipoEnvioId { get; set; }//	smallint
        public string gude_vNombreEmpresaEnvio { get; set; }//	varchar
        public string gude_vGuiaAerea { get; set; }//	varchar
        public string gude_vNumeroHoja { get; set; }//	varchar
        public string gude_vNumeroGuiaDespacho { get; set; }//	varchar
        public Int16 gude_sEstadoGuia { get; set; }//	smallint
        public string gude_cEstado { get; set; }//	varchar    
        public Int16 gude_sUsuarioCreacion { get; set; }//	smallint
        public string gude_vIPCreacion { get; set; }//	varchar
        public DateTime gude_dFechaCreacion { get; set; }//	datetime
        public Int16 gude_sUsuarioModificacion { get; set; }//	smallint
        public string gude_vIPModificacion { get; set; }//	varchar
        public DateTime gude_dFechaModificacion { get; set; }//	datetime
    }
}
