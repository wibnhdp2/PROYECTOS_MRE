using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_FICHAREGISTRAL:BaseBussinesObject
    {
        public RE_FICHAREGISTRAL() { }

        public Int64 fire_iFichaRegistralId { get; set; }//	bigint
        public Int64 fire_iActuacionDetalleId { get; set; }//	bigint
        public string fire_vNumeroFicha { get; set; }//	varchar
        public string fire_vCodigoLocal { get; set; }//	varchar
        public string fire_vCodigoLocalDestino { get; set; }//	varchar
        public Int16 fire_sEstadoId { get; set; }//	smallint
        public DateTime fire_dFechaEstado { get; set; }//	datetime
        public string fire_vObservacion { get; set; }//	varchar
        public string fire_vNumeroGuia { get; set; }//	varchar
        public Int16 fire_sUsuarioCreacion { get; set; }//	smallint
        public string fire_vIPCreacion { get; set; }//	varchar
        public DateTime fire_dFechaCreacion { get; set; }//	datetime
        public Int16 fire_sUsuarioModificacion { get; set; }//	smallint
        public string fire_vIPModificacion { get; set; }//	varchar
        public DateTime fire_dFechaModificacion { get; set; }//	datetime
        public Int64 fihi_iFichaHistoricoId { get; set; }//	bigint
        public byte fire_tTipoRegistro { get; set; }//	tinyint
        public string fire_vTipoRegistro { get; set; }//	varchar
        public DateTime? fire_dFechaEnvio { get; set; }//	datetime
        public string fire_vNumeroOficio { get; set; }//	varchar
    }
}
