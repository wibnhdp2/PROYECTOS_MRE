using System;
using System.Data;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoJudicialNotificacionConsultaBL
    {
        public DataTable Obtener(Int64 iActoJudicialId, Int64 iActoJudicialParticipanteId, string StrCurrentPage, int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
        {
            //ActoJudicialNotificacionConsultaDA objDA = new ActoJudicialNotificacionConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION();
            try
            {
                return objDA.Obtener(iActoJudicialId, iActoJudicialParticipanteId,StrCurrentPage, IntPageSize,
                    ref IntTotalCount, ref IntTotalPages);
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

        public DataTable Obtener_Id_Notificado(Int64 intActoJudicialParticipanteId, Int64 intActoJudicialNotificacionId)
        {
            //ActoJudicialNotificacionConsultaDA objDA = new ActoJudicialNotificacionConsultaDA();
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION();
            try
            {
                return objDA.Obtener_Id_Notificado(intActoJudicialParticipanteId, intActoJudicialNotificacionId);
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
        public void Obtener_Actoparticipante(Int64 iActuacionDetalleId,ref Int64 iActoJudicialId,ref Int64 iActoJudicialParticipanteId)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALNOTIFICACION();
            try
            {
                objDA.Obtener_Actoparticipante(iActuacionDetalleId,ref iActoJudicialId,ref iActoJudicialParticipanteId);
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