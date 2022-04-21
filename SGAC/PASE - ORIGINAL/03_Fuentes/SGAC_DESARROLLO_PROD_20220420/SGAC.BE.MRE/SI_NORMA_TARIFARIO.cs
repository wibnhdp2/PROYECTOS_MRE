using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE.MRE
{
    public class SI_NORMA_TARIFARIO : BaseBussinesObject
    {
        public SI_NORMA_TARIFARIO() { }

        public long nota_iNormaTarifarioId { get; set; }
        public short nota_sNormaId { get; set; }
        public short nota_sTarifarioId { get; set; }
        public short nota_sPagoTipoId { get; set; }
        public short nota_sOficinaConsularId { get; set; }
        public string nota_cEstado { get; set; }
        public short nota_sUsuarioCreacion { get; set; }
        public string nota_vIPCreacion { get; set; }
        public Nullable<short> nota_sUsuarioModificacion { get; set; }
        public string nota_vIPModificacion { get; set; }
    }
}
