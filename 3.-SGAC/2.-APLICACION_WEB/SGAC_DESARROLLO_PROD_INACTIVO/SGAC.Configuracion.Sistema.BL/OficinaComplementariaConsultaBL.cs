using System;

namespace SGAC.Configuracion.Sistema.BL
{
    public class OficinaComplementariaConsultaBL
    {
        public System.Data.DataTable Consultar(int ofcm_sTipoId)
        {
            return new SGAC.Configuracion.Sistema.DA.OficinaComplementariaConsultaDA().Consultar(ofcm_sTipoId);
        }
    }
}
