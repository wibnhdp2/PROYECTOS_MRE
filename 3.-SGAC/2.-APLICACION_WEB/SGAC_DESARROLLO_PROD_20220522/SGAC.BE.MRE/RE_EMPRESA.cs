using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_EMPRESA:BaseBussinesObject
    {
        public RE_EMPRESA() { }

        public RE_EMPRESA(Int16 TipoDocumentoId, String NumeroDocumento)
        {
            this.empr_sTipoDocumentoId = TipoDocumentoId;
            this.empr_vNumeroDocumento = NumeroDocumento;
        }

        public Int64 empr_iEmpresaId {get; set;}//	bigint
        public Int16 empr_sTipoEmpresaId {get; set;}//	smallint
        public Int16 empr_sTipoDocumentoId {get; set;}//	smallint
        public String empr_vRazonSocial	{get; set;}//	varchar
        public String empr_vNumeroDocumento	{get; set;}//	varchar
        public String empr_vActividadComercial {get; set;}//	varchar
        public String empr_vTelefono {get; set;}//	varchar
        public String empr_vCorreo {get; set;}//	varchar
        public String empr_cEstado {get; set;}//	char
        public Int16  empr_sUsuarioCreacion {get; set;}//		smallint
        public String empr_vIPCreacion {get; set;}//	varchar
        public DateTime empr_dFechaCreacion{get; set;}//	datetime
        public Int16  empr_sUsuarioModificacion	{get; set;}//	smallint
        public string empr_vIPModificacion{get; set;}//	varchar
        public DateTime empr_dFechaModificacion { get; set; }//	datetime
    }
}
