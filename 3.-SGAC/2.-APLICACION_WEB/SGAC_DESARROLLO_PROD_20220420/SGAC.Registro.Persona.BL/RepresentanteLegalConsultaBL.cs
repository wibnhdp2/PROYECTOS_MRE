using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SGAC.BE.MRE;
using SGAC.DA.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class RepresentanteLegalConsultaBL
    {
        public RE_REPRESENTANTELEGAL obtener(RE_REPRESENTANTELEGAL representante) {
            RE_REPRESENTANTELEGAL_DA lREPRESENTANTELEGAL_DA = new RE_REPRESENTANTELEGAL_DA();
            return lREPRESENTANTELEGAL_DA.obtener(representante);
        }

        public List<RE_REPRESENTANTELEGAL> listado(RE_EMPRESA empresa) {
            RE_REPRESENTANTELEGAL_DA lREPRESENTANTELEGAL_DA = new RE_REPRESENTANTELEGAL_DA();
            return lREPRESENTANTELEGAL_DA.listado(empresa);
        }
    }
}
