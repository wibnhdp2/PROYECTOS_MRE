using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.Registro.Persona.DA;
using SGAC.BE;
using SGAC.Accesorios;
using SGAC.DA.MRE.ACTONOTARIAL;
using SGAC.BE.MRE.Custom;

namespace SGAC.Registro.Persona.BL
{
    public class ParticipanteConsultaBL
    {
        public long ObtenerIdParticipante(long LonActuacionDetalleId, int IntTipoParticipante, int IntTipoActo)
        {
            DA.ParticipanteConsultaDA objDA = new DA.ParticipanteConsultaDA();

            try
            {
                return objDA.ObtenerIdParticipante(LonActuacionDetalleId, IntTipoParticipante, IntTipoActo);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE Obtener_ActoNotarial(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE_DA();
            return lACTONOTARIALPARTICIPANTE.obtener(participante);
        }

        public List<BE.MRE.RE_ACTONOTARIALPARTICIPANTE> Listar_ActoNotarial(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE_DA();
            return lACTONOTARIALPARTICIPANTE.listado(participante);
        }
        public List<CBE_PRESENTANTE> listaPresentante(CBE_PRESENTANTE presentante)
        {
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE_DA();
            return lACTONOTARIALPARTICIPANTE.listaPresentante(presentante);
        }
        public DataTable ObtenerParticipantesExtraprotocolar(Int64 anpa_iActoNotarialId)
        {
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE_DA();
                return lACTONOTARIALPARTICIPANTE.ObtenerParticipantesExtraprotocolar(anpa_iActoNotarialId);
            }
            catch( Exception ex)
            {
                throw ex;
            }
        }
        public DataTable VerificarRegistroParticipantesExtraprotocolar(Int64 anpa_iActoNotarialId, Int16 acno_sTipoActoNotarialId, Int64 actu_iPersonaRecurrenteId, Int16 acno_sSubTipoActoNotarialId)
        {
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE_DA();
                return lACTONOTARIALPARTICIPANTE.VerificarRegistroParticipantesExtraprotocolar(anpa_iActoNotarialId, acno_sTipoActoNotarialId, actu_iPersonaRecurrenteId, acno_sSubTipoActoNotarialId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        //----------------------------------------------
    }
}
