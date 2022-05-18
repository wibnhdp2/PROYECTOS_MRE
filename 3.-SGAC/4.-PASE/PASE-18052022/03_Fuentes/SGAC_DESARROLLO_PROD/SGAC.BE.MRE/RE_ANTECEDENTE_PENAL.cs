using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class RE_ANTECEDENTE_PENAL : BaseBussinesObject
    {
        public long anpe_iAntecedentePenalId { get; set; }        
        public long anpe_iActuacionDetalleId { get; set; }        
        public DateTime? anpe_dFechaSolicitud { get; set; }
        public int anpe_iFuncionarioId { get; set; }
        public string anpe_vNumeroSolicitud { get; set; }
        public string anpe_vNumeroOficioRpta { get; set; }
        public DateTime? anpe_dFechaRespuesta { get; set; }
        public string anpe_vObservacion { get; set; }
        public string anpe_cEstado { get; set; }
        public short anpe_sUsuarioCreacion { get; set; }
        public string anpe_vIpCreacion { get; set; }
        public DateTime anpe_dFechaCreacion { get; set; }
        public short anpe_sUsuarioModificacion { get; set; }
        public string anpe_vIpModificacion { get; set; }
        public DateTime anpe_dFechaModificacion { get; set; }
        //---------------------------------------------------------------------------------------------------------
        // FECHA: 11/09/2019
        // AUTOR: MIGUEL MÁRQUEZ BELTRÁN
        // MOTIVO: NUEVOS PARAMETROS
        //---------------------------------------------------------------------------------------------------------
        //[anpe_sSolicitaParaId], [anpe_cRegistraAntecedentesPenales],
        //---------------------------------------------------------------------------------------------------------
        public Int16 anpe_sSolicitaParaId { get; set; }
        public string anpe_cRegistraAntecedentesPenales { get; set; }        
        //---------------------------------------------------------------------------------------------------------
    }
}
