using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class CO_MOVIMIENTOCAJACHICA: BaseBussinesObject
    {
        public int moca_iMovimientoCajaChicaId { get; set; }
        public int moca_sTipoMovimientoId { get; set; }
        public short moca_sOficinaConsularId { get; set; }
        public string moca_vNumeroOperacion { get; set; }
        public Nullable<short> moca_sBancoId { get; set; }
        public DateTime moca_dFechaRegistro { get; set; }
        public string moca_vNumeroComprobante { get; set; }
        public Nullable<double> moca_fMontoOperacion { get; set; }
        public double moca_fMonto { get; set; }
        public string moca_cEstado { get; set; }
       
        public short moca_sUsuarioCreacion { get; set; }
        public string moca_vIPCreacion { get; set; }
        public System.DateTime moca_dFechaCreacion { get; set; }
        public Nullable<short> moca_sUsuarioModificacion { get; set; }
        public string moca_vIPModificacion { get; set; }
        public Nullable<System.DateTime> moca_dFechaModificacion { get; set; }


    }
}
