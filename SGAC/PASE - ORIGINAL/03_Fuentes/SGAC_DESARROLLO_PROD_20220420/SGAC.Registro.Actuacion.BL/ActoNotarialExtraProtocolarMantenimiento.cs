using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using SGAC.Accesorios;
using SGAC.BE.MRE.Custom;
using SGAC.BE.MRE;
using SGAC.DA.MRE;
using SGAC.DA.MRE.ACTONOTARIAL;
using SGAC.Registro.Persona.BL;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActoNotarialExtraProtocolarMantenimiento
    {
        public Int32 ActoNotarial_actualizar(RE_ACTONOTARIAL loACTONOTARIAL, List<CBE_PARTICIPANTE> participantes)
        {
            bool lCancel = false;

            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();
            RE_ACTONOTARIAL_DA loACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();

            string strMensajeError = "";
            Int16 intOficinaConsular = 0;
            Int16 intUsuarioCreacion = 0;

            string strHostName = Util.ObtenerHostName();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    #region INSERTANDO PARTICIPANTES

                    foreach (CBE_PARTICIPANTE p in participantes.OrderBy(x=>x.anpa_sTipoParticipanteId).ToList())
                    {
                        RE_PERSONA lPERSONA = new RE_PERSONA();
                        RE_ACTONOTARIALPARTICIPANTE lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();

                        lPERSONA.pers_iPersonaId = p.anpa_iPersonaId;

                        if (!string.IsNullOrEmpty(p.Residencia.resi_vResidenciaDireccion) ||
                                (!string.IsNullOrEmpty(p.Residencia.resi_cResidenciaUbigeo) && p.Residencia.resi_cResidenciaUbigeo.Length == 6))
                        {
                            p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                            {
                                Residencia = p.Residencia
                            });
                        }

                        p.Persona.HostName = strHostName;
                        if (p.anpa_iPersonaId == 0)
                        {
                            lPERSONA = lPERSONA_DA.insertar_minirune(p.Persona);
                        }
                        else if (p.anpa_cEstado != "E" && p.anpa_sTipoParticipanteId != Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.RECURRENTE))
                        {
                            lPERSONA = lPERSONA_DA.actualizar_minirune(p.Persona);
                        }

                        lPERSONA.HostName = strHostName;
                        if ((lPERSONA.Error == true) || (lPERSONA.pers_iPersonaId == 0))
                        {
                            lCancel = true;

                            strMensajeError = lPERSONA_DA.strError;
                            intOficinaConsular = p.OficinaConsultar;
                            intUsuarioCreacion = p.anpa_sUsuarioCreacion;
                        }

                        if (!lCancel)
                        {
                            if (p.anpa_iActoNotarialParticipanteId == 0)
                            {
                                #region Testigo a Ruego

                                if (p.anpa_sTipoParticipanteId == Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.TESTIGO_A_RUEGO))
                                {
                                    foreach (CBE_PARTICIPANTE po in participantes.Where(x => x.anpa_sTipoParticipanteId == (Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.OTORGANTE)) || x.anpa_sTipoParticipanteId == (Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.PADRE)) || x.anpa_sTipoParticipanteId == (Convert.ToInt16(Enumerador.enmNotarialTipoParticipante.MADRE))).ToList())
                                    {
                                        var aux = p.anpa_vCampoAuxiliar.Split(',');

                                        if (aux.Count() > 0)
                                        {
                                            if (aux.ElementAt(1).ToString() == po.Persona.Identificacion.peid_sDocumentoTipoId.ToString() &&
                                                aux.ElementAt(2).ToString() == po.Persona.Identificacion.peid_vDocumentoNumero)
                                            {
                                                p.anpa_iReferenciaParticipanteId = po.anpa_iActoNotarialParticipanteId;
                                                break;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                p.anpa_iPersonaId = lPERSONA.pers_iPersonaId;
                                lACTONOTARIALPARTICIPANTE = lACTONOTARIALPARTICIPANTE_DA.insertar(p);
                            }
                            else
                            {
                                lACTONOTARIALPARTICIPANTE = lACTONOTARIALPARTICIPANTE_DA.actualizar(p);
                            }

                            if (lACTONOTARIALPARTICIPANTE.Error)
                            {
                                lCancel = true;

                                strMensajeError = lACTONOTARIALPARTICIPANTE_DA.strError;
                                intOficinaConsular = p.OficinaConsultar;
                                intUsuarioCreacion = p.anpa_sUsuarioCreacion;

                                break;
                            }
                        } 
                    }

                    #endregion

                    #region ACTUALIZANDO ACTONOTARIAL
                    if (!lCancel)
                    {
                        loACTONOTARIAL = loACTONOTARIAL_DA.actualizar(loACTONOTARIAL);

                        if (loACTONOTARIAL.Error == true)
                        {
                            lCancel = true;

                            strMensajeError = loACTONOTARIAL_DA.strError;
                            intOficinaConsular = loACTONOTARIAL.OficinaConsultar;
                            intUsuarioCreacion = loACTONOTARIAL.acno_sUsuarioCreacion;
                        }
                    }
                    #endregion

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = intOficinaConsular,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = strMensajeError,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = intUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    return (Int32)Enumerador.enmResultadoQuery.ERR;
                }
                finally
                {
                    if (lPERSONA_DA != null)
                    {
                        lPERSONA_DA = null;
                    }
                }
            }

            return (Int32)Enumerador.enmResultadoQuery.OK;
        }

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE insertar(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA objDA = new RE_ACTONOTARIALPARTICIPANTE_DA();

                participante = objDA.insertar(participante);

                return participante;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------
        //Fecha: 17/10/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Actualizar registro del participante
        //-------------------------------------------------
        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE actualizar(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA objDA = new RE_ACTONOTARIALPARTICIPANTE_DA();

            participante = objDA.actualizar(participante);

            return participante;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------

        public Int32 ActoNotarial_actualizar(RE_ACTONOTARIAL loACTONOTARIAL)
        {
            bool lCancel = false;

            RE_ACTONOTARIAL_DA loACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();

            string strMensajeError = "";
            Int16 intOficinaConsular = 0;
            Int16 intUsuarioCreacion = 0;

            string strHostName = Util.ObtenerHostName();

            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                try
                {
                    #region ACTUALIZANDO ACTONOTARIAL
                    if (!lCancel)
                    {
                        loACTONOTARIAL = loACTONOTARIAL_DA.actualizar(loACTONOTARIAL);

                        if (loACTONOTARIAL.Error == true)
                        {
                            lCancel = true;

                            strMensajeError = loACTONOTARIAL_DA.strError;
                            intOficinaConsular = loACTONOTARIAL.OficinaConsultar;
                            intUsuarioCreacion = loACTONOTARIAL.acno_sUsuarioCreacion;
                        }
                    }
                    #endregion

                    if (!lCancel)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        throw new Exception();
                    }

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();

                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = intOficinaConsular,
                        audi_vComentario = ex.Message,
                        audi_vMensaje = strMensajeError,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = intUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    return (Int32)Enumerador.enmResultadoQuery.ERR;
                }
            }

            return (Int32)Enumerador.enmResultadoQuery.OK;
        }


     
//-----------------------------------------------------    
    }
}
