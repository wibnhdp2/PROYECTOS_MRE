using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using SGAC.DA.MRE;
using SGAC.DA.MRE.ACTONOTARIAL;
using SGAC.DA.MRE.ACTUACION;
using SGAC.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Configuracion.Maestro.BL;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Transactions;

namespace SGAC.Registro.Actuacion.BL
{    
    public class ActoNotarialPrimigeniaMantenimientoBL
    {
        private static string s_Mensaje { get; set; }

        public RE_ACTONOTARIAL_PRIMIGENIA insertar(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            RE_ACTONOTARIAL_PRIMIGENIA_DA objDA = new RE_ACTONOTARIAL_PRIMIGENIA_DA();
            RE_ACTONOTARIAL_PRIMIGENIA BE = new RE_ACTONOTARIAL_PRIMIGENIA();
            BE = objDA.insertar(ActoNotarialPrimigenia);
            return BE;
        }
        public RE_ACTONOTARIAL_PRIMIGENIA actualizar(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            RE_ACTONOTARIAL_PRIMIGENIA_DA objDA = new RE_ACTONOTARIAL_PRIMIGENIA_DA();
            RE_ACTONOTARIAL_PRIMIGENIA BE = new RE_ACTONOTARIAL_PRIMIGENIA();
            BE = objDA.actualizar(ActoNotarialPrimigenia);
            return BE;
        }
        public RE_ACTONOTARIAL_PRIMIGENIA anular(RE_ACTONOTARIAL_PRIMIGENIA ActoNotarialPrimigenia)
        {
            RE_ACTONOTARIAL_PRIMIGENIA_DA objDA = new RE_ACTONOTARIAL_PRIMIGENIA_DA();
            RE_ACTONOTARIAL_PRIMIGENIA BE = new RE_ACTONOTARIAL_PRIMIGENIA();
            BE = objDA.anular(ActoNotarialPrimigenia);
            return BE;
        }
    }
}
