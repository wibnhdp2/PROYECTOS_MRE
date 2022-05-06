using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE;

namespace SGAC.Auditoria.BL
{
    public class AuditoriaMantenimientoBL
    {
        public Int64 Insertar_Error(SI_AUDITORIA pobjBe)
        {
            return new SGAC.Auditoria.DA.AuditoriaMantenimientoDA().Insertar_Error(pobjBe);
        }
    }
}
