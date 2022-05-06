using System;
using System.Data;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoJudicialParticipanteConsultaBL
    {
        public DataTable Obtener(Int64 iActoJudicialId, Int16 sTipoParticipanteId, Int16? IntOficinaConsular)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            try
            { 
                return objDA.Obtener(iActoJudicialId, sTipoParticipanteId, IntOficinaConsular);
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
        
        public bool ActualizarEstadoParticipante(Int64 iActoJudicialParticipanteId, Int16 sEstadoId, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            try
            {
                return objDA.ActualizarEstadoParticipante(iActoJudicialParticipanteId, sEstadoId, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName);
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

        public bool ActualizarEstadoActa(Int64 iActoJudicialParticipanteId, bool sEstado, Int16 sUsuarioModificacion, string vIPModificacion, Int16 sOficinaConsularId, string vHostName)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            try
            {
                return objDA.ActualizarEstadoActa(iActoJudicialParticipanteId, sEstado, sUsuarioModificacion, vIPModificacion, sOficinaConsularId, vHostName);
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

        public bool ParticipanteActasCerradas(Int64 intActoJudicialid)
        {
            SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA objDA = new SGAC.DA.MRE.ACTOJUDICIAL.RE_ACTOJUDICIALPARTICIPANTE_DA();
            try
            {
                return objDA.ParticipanteActasCerradas(intActoJudicialid);
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