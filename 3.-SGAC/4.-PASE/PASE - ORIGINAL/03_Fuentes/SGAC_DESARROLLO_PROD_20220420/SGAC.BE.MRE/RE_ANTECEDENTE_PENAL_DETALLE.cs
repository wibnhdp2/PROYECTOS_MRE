using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//---------------------------------------------------------------------------------------------------------
// FECHA: 12/09/2019
// AUTOR: MIGUEL MÁRQUEZ BELTRÁN
//---------------------------------------------------------------------------------------------------------
namespace SGAC.BE.MRE
{
    public class RE_ANTECEDENTE_PENAL_DETALLE : BaseBussinesObject
    {
        public Int64 apde_iAntecedentePenalDetalleId { get; set; }
        public Int64 apde_iAntecedentePenalId { get; set; }
        public string apde_vOrganoJurisdiccional { get; set; }
        public string apde_vNumeroExpediente { get; set; }
        public DateTime? apde_dFechaSentencia { get; set; }
        public string apde_vDelito { get; set; }
        public string apde_cDuracion_Anios { get; set; }
        public string apde_cDuracion_Meses { get; set; }
        public string apde_cDuracion_Dias { get; set; }
        public DateTime? apde_dFechaVencimiento { get; set; }
        public string apde_vTipoPena { get; set; }
        public string apde_cEstado { get; set; }
        public short apde_sUsuarioCreacion { get; set; }
        public string apde_vIpCreacion { get; set; }
        public DateTime apde_dFechaCreacion { get; set; }
        public short apde_sUsuarioModificacion { get; set; }
        public string apde_vIpModificacion { get; set; }
        public DateTime apde_dFechaModificacion { get; set; }
    }
}
