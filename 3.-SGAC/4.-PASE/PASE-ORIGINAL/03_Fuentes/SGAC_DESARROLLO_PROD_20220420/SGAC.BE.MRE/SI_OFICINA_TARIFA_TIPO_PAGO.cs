// -----------------------------------------------------------------------
// <copyright file="SI_OFICINA_TARIFA_TIPO_PAGO.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SGAC.BE.MRE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SI_OFICINA_TARIFA_TIPO_PAGO : BaseBussinesObject
    {
        public SI_OFICINA_TARIFA_TIPO_PAGO() { }

        public long ofpa_iOficinaTipoPagoId { get; set; }
        public short ofpa_sOficinaConsularId { get; set; }
        public short ofpa_sTarifarioId { get; set; }
        public short ofpa_sPagoTipoId { get; set; }
        public string ofpa_cEstado { get; set; }
        public short ofpa_sUsuarioCreacion { get; set; }
        public string ofpa_vIPCreacion { get; set; }
        public Nullable<short> ofpa_sUsuarioModificacion { get; set; }
        public string ofpa_vIPModificacion { get; set; }
    }
}
