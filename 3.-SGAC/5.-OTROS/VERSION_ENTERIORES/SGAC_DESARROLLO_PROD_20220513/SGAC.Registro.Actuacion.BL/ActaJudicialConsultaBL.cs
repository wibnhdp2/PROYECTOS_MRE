using System;
using System.Data;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActaJudicialConsultaBL
    {
        public DataTable Obtener(Int64 iActoJudicialNotificacionId)
        {
            //ActaJudicialConsultaDA objDA = new ActaJudicialConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTAJUDICIAL();
            try
            {
                return objDA.Obtener(iActoJudicialNotificacionId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
    }
}