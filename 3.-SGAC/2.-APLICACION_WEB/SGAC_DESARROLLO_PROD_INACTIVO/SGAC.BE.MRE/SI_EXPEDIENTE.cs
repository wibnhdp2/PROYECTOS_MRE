using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_EXPEDIENTE : BaseBussinesObject
    {
        public SI_EXPEDIENTE() { }

        public short exp_sExpedienteId { get; set; }
        public short exp_sOficinaConsularId { get; set; }
        public short exp_sPeriodo { get; set; }
        public short exp_sTipoDocMigId { get; set; }
        public int exp_INumeroExpediente { get; set; }
        public string exp_cEstado { get; set; }
        public short exp_sUsuarioCreacion { get; set; }
        public string exp_vIPCreacion { get; set; }
        public DateTime exp_dFechaCreacion { get; set; }
        public Nullable<short> exp_sUsuarioModificacion { get; set; }
        public string exp_vIPModificacion { get; set; }
        public Nullable<DateTime> exp_dFechaModificacion { get; set; }
    }
}
