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
    public class ActoNotarialMantenimiento
    {

        private static string s_Mensaje { get; set; }
        public long Insertar_ActoNotarialCodeBehind(RE_ACTONOTARIAL actonotarial)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { 
                IsolationLevel = IsolationLevel.ReadCommitted, 
                Timeout = TimeSpan.FromMinutes(2)};

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                //Actuacion
                RE_ACTUACION lActuacion = actonotarial.ACTUACION;
                if (actonotarial.acno_iActoNotarialId == 0)
                {
                    //SI ES EDICION NO SE ACTUALIZA ACTUACION
                    ActuacionMantenimientoBL lActuacionMantenimiento = new ActuacionMantenimientoBL();
                    lActuacion = lActuacionMantenimiento.Insertar(lActuacion);
                }
                else
                {
                    ActuacionConsultaBL lActuacionConsultaBL = new ActuacionConsultaBL();
                    actonotarial.ACTUACION.actu_iActuacionId = actonotarial.acno_iActuacionId;
                    lActuacion = lActuacionConsultaBL.obtener(actonotarial.ACTUACION);
                }

                if ((lActuacion.Error == false) && (lActuacion.actu_iActuacionId != 0))
                {
                    //Acto Notarial
                    RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
                    if (actonotarial.acno_iActoNotarialId == 0)
                    {
                        actonotarial.acno_iActuacionId = lActuacion.actu_iActuacionId;
                        actonotarial = lACTONOTARIAL_DA.insertar(actonotarial);
                    }
                    else
                    {
                        actonotarial = lACTONOTARIAL_DA.actualizar(actonotarial);
                    }

                    if ((actonotarial.Error == true) && (actonotarial.acno_iActoNotarialId == 0))
                    {
                        lCancel = true;
                    }
                }
                else
                {
                    lCancel = true;
                }

                //Cerrando transacción
                if (lCancel == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }


            if (actonotarial.Message != null)
            {
                s_Mensaje = actonotarial.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarial.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarial.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
                return 0;
            }

            return actonotarial.acno_iActoNotarialId;
        }
        //public JResult Insertar_ActoNotarial
        public RE_ACTONOTARIAL Insertar_ActoNotarial(RE_ACTONOTARIAL actonotarial, string strValidarTablaPrimigenia = "N", RE_ACTONOTARIAL_PRIMIGENIA actoNotarialPrimigenia = null)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                RE_ACTUACION lActuacion = actonotarial.ACTUACION;
                if (actonotarial.acno_iActoNotarialId == 0)
                {
                    ActuacionMantenimientoBL lActuacionMantenimiento = new ActuacionMantenimientoBL();
                    lActuacion = lActuacionMantenimiento.Insertar(lActuacion);
                }
                else
                {
                    ActuacionConsultaBL lActuacionConsultaBL = new ActuacionConsultaBL();
                    actonotarial.ACTUACION.actu_iActuacionId = actonotarial.acno_iActuacionId;
                    lActuacion = lActuacionConsultaBL.obtener(actonotarial.ACTUACION);
                }

                if ((lActuacion.Error == false) && (lActuacion.actu_iActuacionId != 0))
                {
                    RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
                    if (actonotarial.acno_iActoNotarialId == 0)
                    {
                        actonotarial.acno_iActuacionId = lActuacion.actu_iActuacionId;
                        actonotarial = lACTONOTARIAL_DA.insertar(actonotarial);

                        if (!actonotarial.Error)
                        {
                            //---------------------------------------------------------
                            //Fecha: 10/11/2021
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Realizar el registro en la Tabla Primigenia 
                            //---------------------------------------------------------
                            if (strValidarTablaPrimigenia == "S")
                            {
                                ActoNotarialPrimigeniaMantenimientoBL actoNotarialPriBL = new ActoNotarialPrimigeniaMantenimientoBL();

                                actoNotarialPrimigenia.anpr_iActoNotarialId = actonotarial.acno_iActoNotarialId;

                                RE_ACTONOTARIAL_PRIMIGENIA ANPBL = new RE_ACTONOTARIAL_PRIMIGENIA();
                                ANPBL = actoNotarialPriBL.insertar(actoNotarialPrimigenia);
                                if (ANPBL.Error)
                                {
                                    lCancel = true;
                                }
                                else
                                {
                                    actonotarial.iActoNotarialPrimigeniaId = ANPBL.anpr_iActoNotarialPrimigeniaId;
                                }
                            }
                            else
                            {
                                actonotarial.iActoNotarialPrimigeniaId = 0;
                            }
                            //---------------------------------------------------------
                            if (!lCancel)
                            {
                                #region Registro Seguimiento Estado

                                RE_ACTONOTARIALSEGUIMIENTO_DA objSeguimientoDA = new RE_ACTONOTARIALSEGUIMIENTO_DA();

                                RE_ACTONOTARIALSEGUIMIENTO ActoNotarialSeguimiento = new RE_ACTONOTARIALSEGUIMIENTO();
                                ActoNotarialSeguimiento.anse_iActoNotarialId = actonotarial.acno_iActoNotarialId;
                                ActoNotarialSeguimiento.anse_sEstadoId = actonotarial.acno_sEstadoId;
                                ActoNotarialSeguimiento.anse_sUsuarioCreacion = actonotarial.acno_sUsuarioCreacion;
                                ActoNotarialSeguimiento.anse_vIPCreacion = Util.ObtenerDireccionIP();
                                ActoNotarialSeguimiento.OficinaConsultar = actonotarial.acno_sOficinaConsularId;
                                ActoNotarialSeguimiento.HostName = Util.ObtenerHostName();

                                ActoNotarialSeguimiento = objSeguimientoDA.insertar(ActoNotarialSeguimiento);
                                if (ActoNotarialSeguimiento.Error)
                                {
                                    lCancel = true;
                                }

                                #endregion
                            }
                            //---------------------------------------------------------
                            
                        }
                    }
                    else
                    {
                        actonotarial = lACTONOTARIAL_DA.actualizar(actonotarial);

                        //---------------------------------------------------------
                        //Fecha: 10/11/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Realizar el registro en la Tabla Primigenia 
                        //---------------------------------------------------------
                        if (strValidarTablaPrimigenia == "S")
                        {
                            ActoNotarialPrimigeniaMantenimientoBL actoNotarialPriBL = new ActoNotarialPrimigeniaMantenimientoBL();
                            actoNotarialPrimigenia = new RE_ACTONOTARIAL_PRIMIGENIA();
                            actoNotarialPrimigenia.anpr_sUsuarioModificacion = actonotarial.acno_sUsuarioModificacion;
                            actoNotarialPrimigenia.anpr_vIPModificacion = actonotarial.acno_vIPModificacion;
                            actoNotarialPrimigenia.anpr_dFechaModificacion = actonotarial.acno_dFechaModificacion;                       
                            actoNotarialPrimigenia.anpr_iActoNotarialId = actonotarial.acno_iActoNotarialId;                       
                            RE_ACTONOTARIAL_PRIMIGENIA ANPBL = new RE_ACTONOTARIAL_PRIMIGENIA();
                            if (actonotarial.acno_sEstadoId == Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.ANULADA))
                            {
                                ANPBL = actoNotarialPriBL.anular(actoNotarialPrimigenia);
                            }
                            else
                            {
                                ANPBL = actoNotarialPriBL.actualizar(actoNotarialPrimigenia);
                            }
                            if (ANPBL.Error)
                            {
                                lCancel = true;
                            }
                        }
                        //---------------------------------------------------------
                    }

                    if ((actonotarial.Error == true) && (actonotarial.acno_iActoNotarialId == 0))
                    {
                        lCancel = true;
                    }
                }
                else
                {
                    lCancel = true;
                }

                if (lCancel == true)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }
           
            if(actonotarial.Error)
            {
                s_Mensaje = actonotarial.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarial.acno_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarial.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }        

           // JResult jsonresult = new JResult(actonotarial.acno_iActoNotarialId, actonotarial.Message);
            return actonotarial;
        }

        public List<CBE_PARTICIPANTE> ListarActoNotarialParticipante_Persona(RE_ACTONOTARIALPARTICIPANTE participante)
        {
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();

            RE_PERSONAIDENTIFICACION_DA lPERSONAIDENTIFICACION_DA = new RE_PERSONAIDENTIFICACION_DA();
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();
            SI_PARAMETRO lPARAMETRO = new SI_PARAMETRO();

            ParametroConsultasBL lParametroConsultasBL = new ParametroConsultasBL();
            TablaMaestraConsultaBL lTablaMaestraConsultaBL = new TablaMaestraConsultaBL();

            List<CBE_PARTICIPANTE> lResult = new List<CBE_PARTICIPANTE>();
            List<RE_ACTONOTARIALPARTICIPANTE> ACTONOTARIALPARTICIPANTE_CONTAINER = lACTONOTARIALPARTICIPANTE_DA.listado(participante);

            foreach (RE_ACTONOTARIALPARTICIPANTE Item in ACTONOTARIALPARTICIPANTE_CONTAINER)
            {
                CBE_PARTICIPANTE p = new CBE_PARTICIPANTE();
                #region To Object
                p.anpa_iActoNotarialParticipanteId = Item.anpa_iActoNotarialParticipanteId;
                p.anpa_iActoNotarialParticipanteId = Item.anpa_iActoNotarialParticipanteId;
                p.anpa_iActoNotarialId = Item.anpa_iActoNotarialId;
                p.anpa_iPersonaId = Item.anpa_iPersonaId;
                p.anpa_iEmpresaId = Item.anpa_iEmpresaId;
                p.anpa_sTipoParticipanteId = Item.anpa_sTipoParticipanteId;
                p.anpa_bFlagFirma = Item.anpa_bFlagFirma;
                p.anpa_bFlagHuella = Item.anpa_bFlagHuella;
                p.anpa_cEstado = Item.anpa_cEstado;
                p.anpa_sUsuarioCreacion = Item.anpa_sUsuarioCreacion;
                p.anpa_vIPCreacion = Item.anpa_vIPCreacion;
                p.anpa_dFechaCreacion = Item.anpa_dFechaCreacion;
                p.anpa_sUsuarioModificacion = Item.anpa_sUsuarioModificacion;
                p.anpa_vIPModificacion = Item.anpa_vIPModificacion;
                p.anpa_dFechaModificacion = Item.anpa_dFechaModificacion;
                p.anpa_iReferenciaParticipanteId = Item.anpa_iReferenciaParticipanteId;
                p.anpa_vNumeroEscrituraPublica = Item.anpa_vNumeroEscrituraPublica;
                p.anpa_vNumeroPartida = Item.anpa_vNumeroPartida;
                #endregion

                p.Persona.pers_iPersonaId = Item.anpa_iPersonaId;
                p.Identificacion.peid_iPersonaId = Item.anpa_iPersonaId;
                p.Persona = lPERSONA_DA.obtener(p.Persona);
                p.Identificacion = lPERSONAIDENTIFICACION_DA.obtener(p.Identificacion);
                //Direccion

                //de Parametros...
                lPARAMETRO = new SI_PARAMETRO();
                lPARAMETRO.para_sParametroId = p.anpa_sTipoParticipanteId;
                p.acpa_sTipoParticipanteId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                lDOCUMENTO_IDENTIDAD = new BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD();
                lDOCUMENTO_IDENTIDAD.doid_sTipoDocumentoIdentidadId = p.Identificacion.peid_sDocumentoTipoId;
                p.peid_sDocumentoTipoId_desc = lTablaMaestraConsultaBL.DOCUMENTO_IDENTIDAD_OBTENER(lDOCUMENTO_IDENTIDAD,1).doid_vDescripcionCorta;

                lPARAMETRO = new SI_PARAMETRO();
                lPARAMETRO.para_sParametroId = p.Persona.pers_sNacionalidadId;
                p.pers_sNacionalidadId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                lPARAMETRO = new SI_PARAMETRO();
                lPARAMETRO.para_sParametroId = p.Persona.pers_sIdiomaNatalId;
                p.pers_sIdiomaNatalId_desc = lParametroConsultasBL.Obtener(lPARAMETRO).para_vDescripcion;

                lResult.Add(p);
            }

            return lResult;
        }

        public void InsertarActoNotarialParticipante_Persona(List<CBE_PARTICIPANTE> participantes,ref String sMensaje)
        {
            bool lCancel = false;

            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();
            RE_PERSONA lPERSONA = null;
            RE_ACTONOTARIALPARTICIPANTE lACTONOTARIALPARTICIPANTE = null;
            
            

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                foreach (CBE_PARTICIPANTE p in participantes)
                {
                    lPERSONA = new RE_PERSONA();
                    lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();
                    if (p.anpa_iPersonaId == 0)
                    {
                        //INSERTANDO PERSONA SI NO EXISTE ...
                        if (!string.IsNullOrEmpty(p.Residencia.resi_vResidenciaDireccion) ||
                            (!string.IsNullOrEmpty(p.Residencia.resi_cResidenciaUbigeo) && p.Residencia.resi_cResidenciaUbigeo.Length == 6))
                        {
                            p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                            {
                                Residencia = p.Residencia
                            });
                        }

                        lPERSONA = lPERSONA_DA.insertar_minirune(p.Persona);
                        if ((lPERSONA.Error == true) || (lPERSONA.pers_iPersonaId == 0))
                        {
                            lCancel = true;
                        }
                    }
                    else
                    {
                        p.Persona.pers_sUsuarioModificacion = p.anpa_sUsuarioCreacion;
                        p.Persona.OficinaConsultar = p.OficinaConsultar;
                       
                        string strTexto = lPERSONA_DA.actualizar_persona(p.Persona);

                        //--------------------------------------------
                        //Fecha: 07/09/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Validar el mensaje del método de
                        //          actualización de la persona.
                        //--------------------------------------------
                        if (strTexto != string.Empty)
                        {
                            lCancel = true;
                        }

                        if (lCancel == false)
                        {
                            if (p.Residencia.resi_iResidenciaId == 0)
                            {
                                if (p.Residencia.resi_cResidenciaUbigeo == null) p.Residencia.resi_cResidenciaUbigeo = string.Empty;
                                if (p.Residencia.resi_cResidenciaUbigeo.Length == 6)
                                {
                                    p.Residencia.resi_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                                    p.Residencia.resi_vIPCreacion = Util.ObtenerDireccionIP();
                                    p.Residencia.OficinaConsultar = p.OficinaConsultar;
                                    p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                                    {
                                        Residencia = p.Residencia
                                    });

                                    Int64 intResidencia = lPERSONA_DA.insertar_residencia(p.Residencia);
                                    if (intResidencia > 0)
                                    {
                                        RE_PERSONARESIDENCIA objPersonaResidenciaBase = new RE_PERSONARESIDENCIA();
                                        objPersonaResidenciaBase.pere_iResidenciaId = intResidencia;
                                        objPersonaResidenciaBase.pere_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                                        objPersonaResidenciaBase.OficinaConsultar = p.OficinaConsultar;
                                        objPersonaResidenciaBase.pere_iPersonaId = p.anpa_iPersonaId;
                                        lPERSONA_DA.insertar_residencia_persona(objPersonaResidenciaBase);
                                    }

                                }
                            }
                            else
                            {
                                if (p.Residencia.resi_cResidenciaUbigeo == null) p.Residencia.resi_cResidenciaUbigeo = string.Empty;
                                if (p.Residencia.resi_cResidenciaUbigeo.Length == 6)
                                {
                                    p.Residencia.resi_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                                    p.Residencia.resi_vIPCreacion = Util.ObtenerDireccionIP();
                                    p.Residencia.OficinaConsultar = p.OficinaConsultar;
                                    p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                                    {
                                        Residencia = p.Residencia
                                    });

                                    Int64 intResidencia = lPERSONA_DA.actualizar_residencia(p.Residencia);
                                }

                            }
                        }

                        lPERSONA = p.Persona;
                    }

                    if (lCancel == false)
                    {
                        //--------------------------------
                        p.anpa_iPersonaId = lPERSONA.pers_iPersonaId;
                        lACTONOTARIALPARTICIPANTE = lACTONOTARIALPARTICIPANTE_DA.insertar(p);
                        if ((lACTONOTARIALPARTICIPANTE.Error == true) ||
                            (lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialParticipanteId == 0))
                        {
                            lCancel = true;
                        }
                        else {
                            foreach (CBE_PARTICIPANTE p_aux in participantes.Where(z => z.anpa_iActoNotarialParticipanteId != lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialParticipanteId))
                            {
                                if (p_aux.anpa_iReferenciaParticipanteId == p.anpa_iActoNotarialParticipanteIdAux)
                                {
                                    p_aux.anpa_iReferenciaParticipanteId = Convert.ToInt64(lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialParticipanteId);

                                    
                                }
                            }
                        }
                    //--------------------------------
                    }
                }

                //-------------------------------------------------------------------------------
                //ACTUALIZA EL ESTADO DE LA ESCRITURA PÚBLICA
                //-------------------------------------------------------------------------------
                if (lCancel == true)
                {
                    actonotarialBE.Error = true;
                }
                else
                {
                    //--------------------------------------------------------------
                    //Fecha: 26/11/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se comentaron las líneas para que no cambie
                    //         el estado al registrar el primer participante.
                    //          Solo debería cambiar el estado a asociada
                    //          cuando esten completos todos los participantes.
                    //--------------------------------------------------------------
                    //actonotarialBE.acno_iActoNotarialId = participantes[0].anpa_iActoNotarialId;
                    //actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.ASOCIADA;
                    //actonotarialBE.acno_sOficinaConsularId = participantes[0].OficinaConsultar;
                    //actonotarialBE.acno_sUsuarioCreacion = participantes[0].anpa_sUsuarioCreacion;
                    //actonotarialBE.acno_vIPCreacion = participantes[0].anpa_vIPCreacion;
                    //actonotarialDA.actualizar_Estado(actonotarialBE);
                    //if (actonotarialBE.Error == true)
                    //{
                    //    lCancel = true;
                    //}
                }
                //-------------------------------------------------------------------------------

                if (lCancel == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

          
            if (actonotarialBE.Error == true)
            {
                s_Mensaje = actonotarialBE.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarialBE.acno_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarialBE.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }

          
          
            if (lPERSONA.Error == true)
            {
                s_Mensaje = lPERSONA.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lPERSONA.OficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lPERSONA.pers_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }

            if (lACTONOTARIALPARTICIPANTE.Error == true)
            {
                s_Mensaje = lACTONOTARIALPARTICIPANTE.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIALPARTICIPANTE.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIALPARTICIPANTE.anpa_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }
            sMensaje = s_Mensaje;
            
        }
        public long InsertarActoNotarialPresentnte(CBE_PRESENTANTE presentante)
        {
            RE_ACTONOTARIALCUERPO_DA lACTONOTARIALCUERPO_DA = new RE_ACTONOTARIALCUERPO_DA();
            return lACTONOTARIALCUERPO_DA.insertarPresentante(presentante);
        }

        public void ActualizarFechaSuscripcion(List<CBE_PARTICIPANTE> participantes, ref String sMensaje)
        {
            bool lCancel = false;

            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();

            RE_ACTONOTARIALPARTICIPANTE lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                foreach (CBE_PARTICIPANTE p in participantes)
                {
                    lACTONOTARIALPARTICIPANTE = lACTONOTARIALPARTICIPANTE_DA.actualizarFechaSuscripcion(p);

                    if ((lACTONOTARIALPARTICIPANTE.Error == true) ||
                        (lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialParticipanteId == 0))
                    {
                        lCancel = true;
                    }
                }

                if (lCancel == true)
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }
            if (lACTONOTARIALPARTICIPANTE.Error == true)
            {
                s_Mensaje = lACTONOTARIALPARTICIPANTE.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIALPARTICIPANTE.OficinaConsultar,
                        audi_vComentario = "Error al actualizar Fecha de Suscripción",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIALPARTICIPANTE.anpa_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            sMensaje = s_Mensaje;
        }


        public void ActualizarActoNotarialParticipante(List<CBE_PARTICIPANTE> participantes,ref String sMensaje)
        {
            bool lCancel = false;

            RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();

            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
          
            RE_PERSONA lPERSONA = null;
            RE_ACTONOTARIALPARTICIPANTE lACTONOTARIALPARTICIPANTE = new RE_ACTONOTARIALPARTICIPANTE();
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {

                foreach (CBE_PARTICIPANTE p in participantes)
                {
                    lPERSONA = new RE_PERSONA();
                    p.Persona.pers_sUsuarioModificacion = p.anpa_sUsuarioCreacion;
                    p.Persona.OficinaConsultar = p.OficinaConsultar;

                                    
                    if (p.Residencia.resi_iResidenciaId == 0)
                    {
                        if (p.Residencia.resi_cResidenciaUbigeo == null) p.Residencia.resi_cResidenciaUbigeo = string.Empty;
                        if (p.Residencia.resi_cResidenciaUbigeo.Length == 6)
                        {
                            p.Residencia.resi_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                            p.Residencia.resi_vIPCreacion = Util.ObtenerDireccionIP();
                            p.Residencia.OficinaConsultar = p.OficinaConsultar;
                            p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                            {
                                Residencia = p.Residencia
                            });

                            Int64 intResidencia = lPERSONA_DA.insertar_residencia(p.Residencia);
                            if (intResidencia > 0)
                            {
                                RE_PERSONARESIDENCIA objPersonaResidenciaBase = new RE_PERSONARESIDENCIA();
                                objPersonaResidenciaBase.pere_iResidenciaId = intResidencia;
                                objPersonaResidenciaBase.pere_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                                objPersonaResidenciaBase.OficinaConsultar = p.OficinaConsultar;
                                objPersonaResidenciaBase.pere_iPersonaId = p.anpa_iPersonaId;
                                lPERSONA_DA.insertar_residencia_persona(objPersonaResidenciaBase);
                            }

                        }
                    }
                    else
                    {
                        if (p.Residencia.resi_cResidenciaUbigeo == null) p.Residencia.resi_cResidenciaUbigeo = string.Empty;
                        if (p.Residencia.resi_cResidenciaUbigeo.Length == 6)
                        {
                            p.Residencia.resi_sUsuarioCreacion = p.anpa_sUsuarioCreacion;
                            p.Residencia.resi_vIPCreacion = Util.ObtenerDireccionIP();
                            p.Residencia.OficinaConsultar = p.OficinaConsultar;
                            p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                            {
                                Residencia = p.Residencia
                            });

                            Int64 intResidencia = lPERSONA_DA.actualizar_residencia(p.Residencia);
                        }

                    }
                    ////INSERTANDO PERSONA SI NO EXISTE ...
                    //if (!string.IsNullOrEmpty(p.Residencia.resi_vResidenciaDireccion) ||
                    //    (!string.IsNullOrEmpty(p.Residencia.resi_cResidenciaUbigeo) && p.Residencia.resi_cResidenciaUbigeo.Length == 6))
                    //{
                    //    p.Persona.Residencias.Add(new RE_PERSONARESIDENCIA()
                    //    {
                    //        Residencia = p.Residencia
                    //    });
                    //}

                    //INSERTANDO PERSONA ACTUALIZANDO SI EXISTE ...                        
                    //lPERSONA = lPERSONA_DA.actualizar_minirune(p.Persona);
                    //if ((lPERSONA.Error == true) || (lPERSONA.pers_iPersonaId == 0))
                    //{
                    //    lCancel = true;
                    //}
                    lPERSONA = lPERSONA_DA.actualizar_minirune(p.Persona);
                    lPERSONA = p.Persona;
                   
                 
                        lACTONOTARIALPARTICIPANTE = lACTONOTARIALPARTICIPANTE_DA.actualizar(p);
                    if ((lACTONOTARIALPARTICIPANTE.Error == true) ||
                        (lACTONOTARIALPARTICIPANTE.anpa_iActoNotarialParticipanteId == 0))
                    {
                        lCancel = true;
                    }
                }

                if (lCancel == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            //sMensaje


            if (actonotarialBE.Error == true)
            {
                s_Mensaje = actonotarialBE.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarialBE.acno_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarialBE.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }



       
            if (lACTONOTARIALPARTICIPANTE.Error == true)
            {
                s_Mensaje = lACTONOTARIALPARTICIPANTE.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIALPARTICIPANTE.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIALPARTICIPANTE.anpa_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }

            sMensaje = s_Mensaje;
        }

        public void InsertarActoNotarialParticipante_Persona(CBE_PROTOCOLAR protocolar)
        {
            bool lCancel = false;
            RE_PERSONA oRE_PERSONA = null;
            RE_ACTONOTARIALPARTICIPANTE lParticipante = null;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                foreach (RE_PERSONA persona in protocolar.PERSONAS.Where(f => f.pers_iPersonaId == 0))
                {
                    oRE_PERSONA = new RE_PERSONA();
                    RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();

                    oRE_PERSONA = lPERSONA_DA.insertar_minirune(persona);
                   if ((oRE_PERSONA.Error == true) || (oRE_PERSONA.pers_iPersonaId == 0))
                    {
                        lCancel = true;
                    }
                }

                if (lCancel == false)
                {
                      lParticipante = new RE_ACTONOTARIALPARTICIPANTE();
                    foreach (RE_ACTONOTARIALPARTICIPANTE participante in protocolar.PARTICIPANTES)
                    {
                        RE_ACTONOTARIALPARTICIPANTE_DA lACTONOTARIALPARTICIPANTE_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();
                        participante.anpa_dFechaCreacion = DateTime.Now;
                        participante.anpa_dFechaModificacion = DateTime.Now;
                        participante.anpa_iEmpresaId = protocolar.EMPRESA.empr_iEmpresaId;
                        lParticipante = lACTONOTARIALPARTICIPANTE_DA.insertar(participante);
                        if ((lParticipante.Error == true) || (lParticipante.anpa_iActoNotarialParticipanteId == 0))
                        {
                            lCancel = true;
                        }
                    }
                }

                if (lCancel == true)
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (oRE_PERSONA.Error)
            {
                s_Mensaje = oRE_PERSONA.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = oRE_PERSONA.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = oRE_PERSONA.pers_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }

            if (lParticipante.Error)
            {
                s_Mensaje = lParticipante.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lParticipante.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lParticipante.anpa_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }
        }

        public void InsertarProtocolar(CBE_PROTOCOLAR protocolar)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIAL_DA actonotarial = new RE_ACTONOTARIAL_DA();
            RE_ACTONOTARIALCUERPO_DA actonotarialcuerpo = new RE_ACTONOTARIALCUERPO_DA();
            RE_ACTONOTARIALDOCUMENTO_DA actonotarialdocumento = new RE_ACTONOTARIALDOCUMENTO_DA();
            RE_ACTONOTARIALPARTICIPANTE_DA actonotarialparticipante = new RE_ACTONOTARIALPARTICIPANTE_DA();
            RE_ACTONOTARIALDOCUMENTO ins = null;
            RE_ACTONOTARIALPARTICIPANTE insPart = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                protocolar.ACTO = actonotarial.insertar(protocolar.ACTO);
                if ((protocolar.ACTO.Error == false) || (protocolar.ACTO.acno_iActoNotarialId != 0))
                {
                    #region Cuerpo ...

                    protocolar.CUERPO.ancu_iActoNotarialId = protocolar.ACTO.acno_iActoNotarialId;
                    protocolar.CUERPO = actonotarialcuerpo.insertar(protocolar.CUERPO);
                    if ((protocolar.CUERPO.Error == true) || (protocolar.CUERPO.ancu_iActoNotarialCuerpoId == 0))
                    {
                        lCancel = true;
                    }

                    #endregion

                    #region Documentos ...

                    if ((lCancel == false) && (protocolar.DOCUMENTOS.Count != 0))
                    {
                        List<RE_ACTONOTARIALDOCUMENTO> DOCUMENTOS = new List<RE_ACTONOTARIALDOCUMENTO>();
                        foreach (RE_ACTONOTARIALDOCUMENTO documento in protocolar.DOCUMENTOS)
                        {
                            documento.ando_iActoNotarialId = protocolar.ACTO.acno_iActoNotarialId;
                            ins = new RE_ACTONOTARIALDOCUMENTO();
                            ins = actonotarialdocumento.insertar(documento);
                            if ((ins.Error == true) || (ins.ando_iActoNotarialDocumentoId == 0))
                            {
                                lCancel = true;
                            }
                            else
                            {
                                DOCUMENTOS.Add(ins);
                            }
                        }
                        protocolar.DOCUMENTOS = DOCUMENTOS;
                    }

                    #endregion

                    #region Participantes ...

                    if ((lCancel == false) && (protocolar.PARTICIPANTES.Count != 0))
                    {
                        List<RE_ACTONOTARIALPARTICIPANTE> PARTICIPANTES = new List<RE_ACTONOTARIALPARTICIPANTE>();
                        foreach (RE_ACTONOTARIALPARTICIPANTE participante in protocolar.PARTICIPANTES)
                        {
                            participante.anpa_iActoNotarialId = protocolar.ACTO.acno_iActoNotarialId;
                            insPart = new RE_ACTONOTARIALPARTICIPANTE();
                            insPart = actonotarialparticipante.insertar(participante);
                            if ((ins.Error == true) || (insPart.anpa_iActoNotarialParticipanteId == 0))
                            {
                                lCancel = true;
                            }
                            else
                            {
                                PARTICIPANTES.Add(insPart);
                            }
                        }

                        protocolar.PARTICIPANTES = PARTICIPANTES;
                    }

                    #endregion
                }
                else
                {
                    lCancel = true;
                }

                //Finanlizando transacción
                if (!lCancel)
                {
                    scope.Complete();
                }
                else
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }


            if (protocolar.ACTO.Error)
            {
                s_Mensaje = protocolar.ACTO.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = protocolar.ACTO.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = protocolar.ACTO.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }


            if (protocolar.CUERPO.Error)
            {
                s_Mensaje = protocolar.CUERPO.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = protocolar.CUERPO.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = protocolar.CUERPO.ancu_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }

            if (ins.Error)
            {
                s_Mensaje = ins.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = ins.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = ins.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }


            if (insPart.Error)
            {
                s_Mensaje = insPart.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = insPart.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = insPart.anpa_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

            }
        }

        public string ActualizarFechaConclusion(RE_ACTONOTARIAL actonotarial)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();

                actonotarial = lACTONOTARIAL_DA.actualizarFechaConclusionFirma(actonotarial);
                if ((actonotarial.Error == true) && (actonotarial.acno_iActoNotarialId == 0))
                {
                    lCancel = true;
                }
                if (lCancel == true)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }
            if (actonotarial.Error)
            {
                s_Mensaje = actonotarial.Message;

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarial.acno_sOficinaConsularId,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarial.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
            return s_Mensaje;
        }

        public JResult Insertar_ActoNotarialCuerpo(CBE_CUERPO actonotarialcuerpo, List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes)
        {
            Insertar_ActoNotarialCuerpoBL(actonotarialcuerpo, lstImagenes);

            JResult jsonresult = new JResult(actonotarialcuerpo.ancu_iActoNotarialCuerpoId, actonotarialcuerpo.Message);
            return jsonresult;
        }

        public void Insertar_ActoNotarialCuerpoServerSide(CBE_CUERPO actonotarialcuerpo, List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes)
        {
            Insertar_ActoNotarialCuerpoBL(actonotarialcuerpo, lstImagenes);
        }

        private void Insertar_ActoNotarialCuerpoBL(CBE_CUERPO actonotarialcuerpo, List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();

            RE_ACTONOTARIALCUERPO lACTONOTARIALCUERPO = new RE_ACTONOTARIALCUERPO();
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();

            RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            RE_ACTONOTARIALCUERPO_DA lACTONOTARIALCUERPO_DA = new RE_ACTONOTARIALCUERPO_DA();

            actonotarialcuerpo.ancu_dFechaCreacion = DateTime.Now;
            actonotarialcuerpo.ancu_dFechaModificacion = DateTime.Now;
            actonotarialcuerpo.ancu_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                if (actonotarialcuerpo.ancu_iActoNotarialCuerpoId == 0)
                {
                    lACTONOTARIALCUERPO = lACTONOTARIALCUERPO_DA.insertar(actonotarialcuerpo);
                    if (lACTONOTARIALCUERPO.Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        
                        lACTONOTARIAL = actonotarialcuerpo.ActoNotarial;
                        lACTONOTARIAL.acno_vNumeroColegiatura = actonotarialcuerpo.ActoNotarial.acno_vNumeroColegiatura;
                        lACTONOTARIAL.acno_vAutorizacionTipo = actonotarialcuerpo.ActoNotarial.acno_vAutorizacionTipo;
                        lACTONOTARIAL.acno_dFechaModificacion = actonotarialcuerpo.ancu_dFechaModificacion;
                        lACTONOTARIAL.acno_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioModificacion;
                        lACTONOTARIAL.acno_vIPModificacion = actonotarialcuerpo.ancu_vIPModificacion;
                        lACTONOTARIAL = lACTONOTARIAL_DA.actualizar(lACTONOTARIAL);
                        if (lACTONOTARIAL.Error == true)
                        {
                            lCancel = true;
                        }
                        else
                        {
                            actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                            actonotarialBE.acno_iActoNotarialReferenciaId = actonotarialcuerpo.ActoNotarial.acno_iActoNotarialReferenciaId;

                            actonotarialBE.acno_vNumeroLibro = actonotarialcuerpo.ActoNotarial.acno_vNumeroLibro;
                            actonotarialBE.acno_vNumeroFojaInicial = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaInicial;
                            actonotarialBE.acno_vNumeroFojaFinal = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaFinal;
                            actonotarialBE.acno_sNumeroHojas = actonotarialcuerpo.ActoNotarial.acno_sNumeroHojas;
                            actonotarialBE.acno_vNumeroEscrituraPublica = actonotarialcuerpo.ActoNotarial.acno_vNumeroEscrituraPublica;
                            actonotarialBE.acno_nCostoEP = actonotarialcuerpo.ActoNotarial.acno_nCostoEP;
                            actonotarialBE.acno_nCostoParte2 = actonotarialcuerpo.ActoNotarial.acno_nCostoParte2;
                            actonotarialBE.acno_nCostoTestimonio = actonotarialcuerpo.ActoNotarial.acno_nCostoTestimonio;

                            actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                            actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                            actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                            actonotarialDA.actualizar_Ep_Foj_Vin(actonotarialBE);
                            if (actonotarialBE.Error == true)
                            {
                                lCancel = true;
                            }
                            else
                            {
                                actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                                actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA;
                                actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                                actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                                actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                                actonotarialDA.actualizar_Estado(actonotarialBE);
                                if (actonotarialBE.Error == true)
                                {
                                    lCancel = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lACTONOTARIALCUERPO = lACTONOTARIALCUERPO_DA.actualizar(actonotarialcuerpo);
                    if (lACTONOTARIALCUERPO.Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        lACTONOTARIAL = actonotarialcuerpo.ActoNotarial;

                        lACTONOTARIAL.acno_vNumeroColegiatura = actonotarialcuerpo.ActoNotarial.acno_vNumeroColegiatura;
                        lACTONOTARIAL.acno_vAutorizacionTipo = actonotarialcuerpo.ActoNotarial.acno_vAutorizacionTipo;
                        lACTONOTARIAL.acno_dFechaModificacion = actonotarialcuerpo.ancu_dFechaModificacion;
                        lACTONOTARIAL.acno_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioModificacion;
                        lACTONOTARIAL.acno_vIPModificacion = actonotarialcuerpo.ancu_vIPModificacion;
                        lACTONOTARIAL = lACTONOTARIAL_DA.actualizar(lACTONOTARIAL);
                        if (lACTONOTARIAL.Error == true)
                        {
                            lCancel = true;
                        }
                        else
                        {
                            actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                            actonotarialBE.acno_iActoNotarialReferenciaId = actonotarialcuerpo.ActoNotarial.acno_iActoNotarialReferenciaId;

                            actonotarialBE.acno_vNumeroLibro = actonotarialcuerpo.ActoNotarial.acno_vNumeroLibro;
                            actonotarialBE.acno_vNumeroFojaInicial = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaInicial;
                            actonotarialBE.acno_vNumeroFojaFinal = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaFinal;
                            actonotarialBE.acno_vNumeroEscrituraPublica = actonotarialcuerpo.ActoNotarial.acno_vNumeroEscrituraPublica;
                            actonotarialBE.acno_nCostoEP = actonotarialcuerpo.ActoNotarial.acno_nCostoEP;
                            actonotarialBE.acno_nCostoParte2 = actonotarialcuerpo.ActoNotarial.acno_nCostoParte2;
                            actonotarialBE.acno_nCostoTestimonio = actonotarialcuerpo.ActoNotarial.acno_nCostoTestimonio;

                            actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                            actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                            actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                            actonotarialDA.actualizar_Ep_Foj_Vin(actonotarialBE);
                            if (actonotarialBE.Error == true)
                            {
                                lCancel = true;
                            }
                        }
                    }
                }

                if (lCancel == true)
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            #region Imagen
            if (lstImagenes != null)
            {
                if (lstImagenes.Count > 0)
                {
                    foreach (BE.MRE.RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                    {
                        ActoNotarialDocumento.OficinaConsultar = actonotarialcuerpo.OficinaConsultar;
                        ActoNotarialDocumento.ando_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                        ActoNotarialDocumento.ando_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                        ActoNotarialDocumento.ando_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                        ActoNotarialDocumento.ando_vIPModificacion = actonotarialcuerpo.ancu_vIPCreacion;
                    }

                    InsertarActoNotarialDocumento(lstImagenes);
                }
            }
            #endregion


            if (lACTONOTARIAL.Error)
            {
                s_Mensaje = lACTONOTARIAL.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIAL.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIAL.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            if (actonotarialBE.Error)
            {
                s_Mensaje = actonotarialBE.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarialBE.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarialBE.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }


            if (lACTONOTARIALCUERPO.Error)
            {
                s_Mensaje = lACTONOTARIALCUERPO.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIALCUERPO.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIALCUERPO.ancu_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
        }
        
        public JResult InsertarActoNotarialDocumento(RE_ACTONOTARIALDOCUMENTO documento)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                lACTONOTARIALDOCUMENTO_DA.insertar(documento);

                if (documento.Error == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (documento.Error)
            {
                s_Mensaje = documento.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = documento.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = documento.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            JResult jsonresult = new JResult(documento.ando_iActoNotarialDocumentoId, documento.Message);
            return jsonresult;
        }

        public JResult ActualizarActoNotarialDocumento(RE_ACTONOTARIALDOCUMENTO documento)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                lACTONOTARIALDOCUMENTO_DA.actualizar(documento);

                if (documento.Error == true)
                {
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (documento.Error)
            {
                s_Mensaje = documento.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = documento.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = documento.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            JResult jsonresult = new JResult(documento.ando_iActoNotarialDocumentoId, documento.Message);
            return jsonresult;
        }

        public JResult InsertarActoNotarialDocumento(List<RE_ACTONOTARIALDOCUMENTO> documento)
        {
            bool lCancel = false;

            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();

            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();

            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            int Resultado = 0;

            RE_ACTONOTARIALDOCUMENTO lACTONOTARIALDOCUMENTO = new RE_ACTONOTARIALDOCUMENTO();
            Int64 i_OldCodigo = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                foreach (RE_ACTONOTARIALDOCUMENTO p in documento)
                {
                    lACTONOTARIALDOCUMENTO = new RE_ACTONOTARIALDOCUMENTO();
                    
                    if (p.ando_sTipoDocumentoId == (Int16)Enumerador.enmTipoAdjunto.FOTO)
                    {
                        #region Mantenimiento imágenes
                        if (p.ando_cEstado == "E")
                        {
                            Resultado = lACTONOTARIALDOCUMENTO_DA.EliminarDocumento(p);
                            if (Resultado == 0)
                            {
                                lCancel = true;
                                break;
                            }
                        }
                        else
                        {
                            if (p.ando_iActoNotarialDocumentoId <= 0)
                            {
                                lACTONOTARIALDOCUMENTO = lACTONOTARIALDOCUMENTO_DA.insertar(p);
                            }
                            else
                            {
                                lACTONOTARIALDOCUMENTO = lACTONOTARIALDOCUMENTO_DA.actualizar(p);
                            }

                            if ((lACTONOTARIALDOCUMENTO.Error == true) || (lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId == 0))
                            {
                                lCancel = true;
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (p.ando_cEstado == "E")
                        {
                            i_OldCodigo = p.ando_iActoNotarialDocumentoId;
                            Resultado = lACTONOTARIALDOCUMENTO_DA.EliminarDocumento(p);

                            if (Resultado == 0)
                            {
                                lCancel = true;
                                lACTONOTARIALDOCUMENTO.Message = "Error: al grabar el archivo";
                                lACTONOTARIALDOCUMENTO.Error = true;
                                lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId = 0;
                            }
                            else
                            {
                                lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId = i_OldCodigo;
                            }

                            break;
                        }
                        else
                        {
                            if (p.ando_iActoNotarialDocumentoId <= 0)
                            {
                                lACTONOTARIALDOCUMENTO = lACTONOTARIALDOCUMENTO_DA.insertar(p);
                            }
                            else
                            {
                                lACTONOTARIALDOCUMENTO = lACTONOTARIALDOCUMENTO_DA.actualizar(p);
                            }

                            if ((lACTONOTARIALDOCUMENTO.Error == true) || (lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId == 0))
                            {
                                lCancel = true;
                                break;
                            }
                        }
                    }                    
                }

                actonotarialBE.acno_iActoNotarialId = documento[0].ando_iActoNotarialId;
                actonotarialBE.acno_iActuacionId = 0;
                actonotarialBE.acno_sTipoActoNotarialId = 0;
                actonotarialBE.acno_sSubTipoActoNotarialId = 0;
                actonotarialBE.acno_sOficinaConsularId = 0;
                actonotarialBE = actonotarialDA.obtener(actonotarialBE);
                if (actonotarialBE.acno_sEstadoId == (int)Enumerador.enmNotarialProtocolarEstado.VINCULADA)
                {
                    //---------------------------------------------------------------------------------------
                    ////ACTUALIZA EL ESTADO DE LA ESCRITURA PÚBLICA
                    //---------------------------------------------------------------------------------------
                    actonotarialBE.acno_iActoNotarialId = documento[0].ando_iActoNotarialId;
                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.DIGITALIZADA;
                    actonotarialBE.acno_sOficinaConsularId = documento[0].OficinaConsultar;
                    actonotarialBE.acno_sUsuarioCreacion = documento[0].ando_sUsuarioCreacion;
                    actonotarialBE.acno_vIPCreacion = documento[0].ando_vIPCreacion;
                    actonotarialDA.actualizar_Estado(actonotarialBE);
                    if (actonotarialBE.Error == true)
                    {
                        lCancel = true;
                    }
                    //---------------------------------------------------------------------------------------
                }

                if (lCancel == true)
                {
                  //  Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            JResult jsonresult = new JResult(lACTONOTARIALDOCUMENTO.ando_iActoNotarialDocumentoId, lACTONOTARIALDOCUMENTO.Message);
            return jsonresult;
        }

        public BE.MRE.RE_ACTONOTARIALDOCUMENTO InsertarActoNotarialDocumento_2(RE_ACTONOTARIALDOCUMENTO documento)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                lACTONOTARIALDOCUMENTO_DA.insertar(documento);

                if (documento.Error == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            return documento;
        }

        public JResult EliminarActoNotarialDocumento(RE_ACTONOTARIALDOCUMENTO documento)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                lACTONOTARIALDOCUMENTO_DA.actualizar(documento);

                if (documento.Error == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (documento.Error)
            {
                s_Mensaje = documento.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = documento.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = documento.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
            JResult jsonresult = new JResult(documento.ando_iActoNotarialDocumentoId, documento.Message);
            return jsonresult;
        }

        public void InsertarActoNotarialPago(CBE_PROTOCOLAR protocolar)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                // GRABA LA CABECERA DE ACTUACIONES //
                if ((protocolar.ACTUACION.actu_iActuacionId == 0) && (protocolar.ACTUACION.actu_iPersonaRecurrenteId != 0))
                {
                    RE_ACTUACION_DA lACTUACION_DA = new RE_ACTUACION_DA();
                    protocolar.ACTUACION = lACTUACION_DA.insertar(protocolar.ACTUACION);

                    if ((protocolar.ACTUACION.Error == true) || (protocolar.ACTUACION.actu_iActuacionId == 0))
                    {
                        lCancel = true;
                    }

                    // GRABA EL DETALLE DE ACTUACIONES //
                    if (lCancel == false)
                    {
                        //RE_ACTUACIONDETALLE lActuacionDetalle = new RE_ACTUACIONDETALLE();                       
                        //RE_ACTUACIONDETALLE_DA lACTUACIONDETALLE_DA = new RE_ACTUACIONDETALLE_DA();
                        //lActuacionDetalle.acde_sEstadoId = 3;
                        //lActuacionDetalle.acde_sUsuarioCreacion = 5;
                        //lActuacionDetalle.acde_vIPCreacion = "::1";
                        //lActuacionDetalle.acde_dFechaCreacion = DateTime.Now;
                        //lActuacionDetalle.acde_sUsuarioModificacion = 5;
                        //lActuacionDetalle.acde_vIPModificacion = "::1";
                        //lActuacionDetalle.acde_dFechaModificacion = DateTime.Now;

                        //protocolar.ACTUACIONDETALLE = lACTUACIONDETALLE_DA.insertar(protocolar.ACTUACIONDETALLE);
                        //if ((lActuacionDetalle.Error == true) || (lActuacionDetalle.acde_iActuacionDetalleId == 0))
                        //{
                        //    lCancel = true;
                        //}

                    }
                }

                if (lCancel == true)
                {
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }


            if (protocolar.ACTUACION.Error) {
                s_Mensaje = protocolar.ACTUACION.Message;
                    if (!string.IsNullOrEmpty(s_Mensaje))
                    {
                        new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                        {
                            audi_vNombreRuta = Util.ObtenerNameForm(),
                            audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                            audi_sTablaId = null,
                            audi_sClavePrimaria = null,
                            audi_sOficinaConsularId = protocolar.ACTUACION.OficinaConsultar,
                            audi_vComentario = "",
                            audi_vMensaje = s_Mensaje,
                            audi_vHostName = Util.ObtenerHostName(),
                            audi_sUsuarioCreacion = protocolar.ACTUACION.actu_sUsuarioCreacion,
                            audi_vIPCreacion = Util.ObtenerDireccionIP()
                        });

                        s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                    }
                
            }
        }

        public bool ActoNotarialActualizarEstado(RE_ACTONOTARIAL actonotarialBE)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            bool lRspta = false;
             
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();

                actonotarialDA.actualizar_Estado(actonotarialBE);
                if (actonotarialBE.Error == true)
                {
                    lCancel = true;
                    lRspta = true;
                }
                else
                {
                    RE_ACTONOTARIALSEGUIMIENTO_DA objDA = new RE_ACTONOTARIALSEGUIMIENTO_DA();

                    RE_ACTONOTARIALSEGUIMIENTO ActoNotarialSeguimiento = new RE_ACTONOTARIALSEGUIMIENTO();
                    ActoNotarialSeguimiento.anse_iActoNotarialId = actonotarialBE.acno_iActoNotarialId;
                    ActoNotarialSeguimiento.anse_sEstadoId = actonotarialBE.acno_sEstadoId;
                    ActoNotarialSeguimiento.anse_sUsuarioCreacion = actonotarialBE.acno_sUsuarioCreacion;
                    ActoNotarialSeguimiento.anse_vIPCreacion = Util.ObtenerDireccionIP();
                    ActoNotarialSeguimiento.OficinaConsultar = actonotarialBE.acno_sOficinaConsularId;
                    ActoNotarialSeguimiento.HostName = Util.ObtenerHostName();

                    ActoNotarialSeguimiento = objDA.insertar(ActoNotarialSeguimiento);
                    if (ActoNotarialSeguimiento.Error)
                    {
                        lCancel = true;
                    }
                }

                if (lCancel == true)
                {                    
                 //   Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            if (actonotarialBE.Error)
            {
                s_Mensaje = actonotarialBE.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarialBE.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarialBE.acno_sUsuarioModificacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            return lRspta;
        }

        public System.Data.DataTable ObtenerListaDocumentos(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();
            return lACTONOTARIALDOCUMENTO_DA.ObtenerDocumentos(documento);
        }

        public int EliminarDocumento(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();
            int intResultado = 0;
            intResultado = lACTONOTARIALDOCUMENTO_DA.EliminarDocumento(documento);

            if (intResultado <= 0) {
                s_Mensaje = documento.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = documento.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = documento.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            return intResultado;
        }

        public BE.MRE.RE_ACTONOTARIALDOCUMENTO Obtener(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();
            return lACTONOTARIALDOCUMENTO_DA.obtener(documento);
        }

        public List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> ListaActoNotarialDocumento(BE.MRE.RE_ACTONOTARIALDOCUMENTO documento)
        {
            RE_ACTONOTARIALDOCUMENTO_DA lACTONOTARIALDOCUMENTO_DA = new RE_ACTONOTARIALDOCUMENTO_DA();
            List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> ListaActoNotarialDocumento = new List<RE_ACTONOTARIALDOCUMENTO>();
            ListaActoNotarialDocumento =  lACTONOTARIALDOCUMENTO_DA.listado(documento);

            if (documento.Error) {
                s_Mensaje = documento.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = documento.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = documento.ando_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
            return ListaActoNotarialDocumento;
        }

        public RE_ACTONOTARIALDETALLE InsertarActoNotarialDetalle(RE_ACTONOTARIALDETALLE objActoNotarialDetalle, Int64 intActuacionId)
        {
            RE_ACTONOTARIALDETALLE_DA objDA = new RE_ACTONOTARIALDETALLE_DA();
            RE_ACTONOTARIALDETALLE ActoNotarialDetalle = objDA.insertar(objActoNotarialDetalle, intActuacionId);
            return ActoNotarialDetalle;
        }

        public RE_ACTONOTARIALDETALLE ActualizarActoNotarialDetalle(RE_ACTONOTARIALDETALLE objActoNotarialDetalle)
        {
            RE_ACTONOTARIALDETALLE_DA objDA = new RE_ACTONOTARIALDETALLE_DA();
            RE_ACTONOTARIALDETALLE ActoNotarialDetalle = objDA.actualizar(objActoNotarialDetalle);
            return ActoNotarialDetalle;
        }

        // Jonatan Silva Cachay
        // Registra el cuerpo de acto Notarial extra protocolar
        // 04/05/2017
        public JResult Insertar_ActoNotarialCuerpoExtraProtocolar(CBE_CUERPO actonotarialcuerpo, List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes)
        {
            Insertar_ActoNotarialCuerpoExtraProtocolarBL(actonotarialcuerpo, lstImagenes);

            JResult jsonresult = new JResult(actonotarialcuerpo.ancu_iActoNotarialCuerpoId, actonotarialcuerpo.Message);
            return jsonresult;
        }

        // Jonatan Silva Cachay
        // Registra el cuerpo de acto Notarial extra protocolar
        // 04/05/2017
        private void Insertar_ActoNotarialCuerpoExtraProtocolarBL(CBE_CUERPO actonotarialcuerpo, List<BE.MRE.RE_ACTONOTARIALDOCUMENTO> lstImagenes)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            RE_ACTONOTARIAL actonotarialBE = new RE_ACTONOTARIAL();
            RE_ACTONOTARIAL_DA actonotarialDA = new RE_ACTONOTARIAL_DA();

            RE_ACTONOTARIALCUERPO lACTONOTARIALCUERPO = new RE_ACTONOTARIALCUERPO();
            RE_ACTONOTARIAL lACTONOTARIAL = new RE_ACTONOTARIAL();

            RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            RE_ACTONOTARIALCUERPO_DA lACTONOTARIALCUERPO_DA = new RE_ACTONOTARIALCUERPO_DA();

            actonotarialcuerpo.ancu_dFechaCreacion = DateTime.Now;
            actonotarialcuerpo.ancu_dFechaModificacion = DateTime.Now;
            actonotarialcuerpo.ancu_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                if (actonotarialcuerpo.ancu_iActoNotarialCuerpoId == 0)
                {
                    lACTONOTARIALCUERPO = lACTONOTARIALCUERPO_DA.insertar(actonotarialcuerpo);
                    if (lACTONOTARIALCUERPO.Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        lACTONOTARIAL = actonotarialcuerpo.ActoNotarial;
                        lACTONOTARIAL.acno_vNumeroColegiatura = actonotarialcuerpo.ActoNotarial.acno_vNumeroColegiatura;
                        lACTONOTARIAL.acno_vAutorizacionTipo = actonotarialcuerpo.ActoNotarial.acno_vAutorizacionTipo;
                        lACTONOTARIAL.acno_dFechaModificacion = actonotarialcuerpo.ancu_dFechaModificacion;
                        lACTONOTARIAL.acno_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioModificacion;
                        lACTONOTARIAL.acno_vIPModificacion = actonotarialcuerpo.ancu_vIPModificacion;

                        //----adicionado  lCuerpo.ActoNotarial.acno_sOficinaConsularId ==>pipa
                        lACTONOTARIAL.acno_sOficinaConsularId = actonotarialcuerpo.ActoNotarial.acno_sOficinaConsularId;
                        //------------
                        lACTONOTARIAL = lACTONOTARIAL_DA.actualizar(lACTONOTARIAL);
                        if (lACTONOTARIAL.Error == true)
                        {
                            lCancel = true;
                        }
                        else
                        {
                            actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                            actonotarialBE.acno_iActoNotarialReferenciaId = actonotarialcuerpo.ActoNotarial.acno_iActoNotarialReferenciaId;

                            actonotarialBE.acno_vNumeroLibro = actonotarialcuerpo.ActoNotarial.acno_vNumeroLibro;
                            actonotarialBE.acno_vNumeroFojaInicial = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaInicial;
                            actonotarialBE.acno_vNumeroFojaFinal = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaFinal;
                            actonotarialBE.acno_vNumeroEscrituraPublica = actonotarialcuerpo.ActoNotarial.acno_vNumeroEscrituraPublica;

                            //----adicionado  actonotarialBE.acno_sOficinaConsularId ==>pipa
                            //actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;//aca se perdía el valor
                            actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.ActoNotarial.acno_sOficinaConsularId;
                            //---------
                            actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                            actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                            actonotarialDA.actualizar_Ep_Foj_Vin(actonotarialBE);
                            if (actonotarialBE.Error == true)
                            {
                                lCancel = true;
                            }
                            else
                            {
                                actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                                if (actonotarialcuerpo.ActoNotarial.acno_bFlagLeidoAprobado)
                                {
                                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.APROBADA;
                                }
                                else
                                {
                                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA;
                                }

                                actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                                actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                                actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                                actonotarialBE.acno_sTamanoLetra = actonotarialcuerpo.ActoNotarial.acno_sTamanoLetra;
                                actonotarialDA.actualizar_Estado(actonotarialBE);
                                if (actonotarialBE.Error == true)
                                {
                                    lCancel = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lACTONOTARIALCUERPO = lACTONOTARIALCUERPO_DA.actualizar(actonotarialcuerpo);
                    if (lACTONOTARIALCUERPO.Error == true)
                    {
                        lCancel = true;
                    }
                    else
                    {
                        //==========verificado por Pipa
                        lACTONOTARIAL = actonotarialcuerpo.ActoNotarial;
                        lACTONOTARIAL.acno_iActoNotarialId = actonotarialcuerpo.ActoNotarial.acno_iActoNotarialId;
                        lACTONOTARIAL.acno_iActuacionId = actonotarialcuerpo.ActoNotarial.acno_iActuacionId;
                        //------------
                        lACTONOTARIAL.acno_vNumeroColegiatura = actonotarialcuerpo.ActoNotarial.acno_vNumeroColegiatura;
                        lACTONOTARIAL.acno_vAutorizacionTipo = actonotarialcuerpo.ActoNotarial.acno_vAutorizacionTipo;
                        lACTONOTARIAL.acno_dFechaModificacion = actonotarialcuerpo.ancu_dFechaModificacion;
                        lACTONOTARIAL.acno_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioModificacion;
                        lACTONOTARIAL.acno_vIPModificacion = actonotarialcuerpo.ancu_vIPModificacion;

                        lACTONOTARIAL = lACTONOTARIAL_DA.actualizar(lACTONOTARIAL);
                        if (lACTONOTARIAL.Error == true)
                        {
                            lCancel = true;
                        }
                        else
                        {
                            actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                            actonotarialBE.acno_iActoNotarialReferenciaId = actonotarialcuerpo.ActoNotarial.acno_iActoNotarialReferenciaId;

                            actonotarialBE.acno_vNumeroLibro = actonotarialcuerpo.ActoNotarial.acno_vNumeroLibro;
                            actonotarialBE.acno_vNumeroFojaInicial = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaInicial;
                            actonotarialBE.acno_vNumeroFojaFinal = actonotarialcuerpo.ActoNotarial.acno_vNumeroFojaFinal;
                            actonotarialBE.acno_vNumeroEscrituraPublica = actonotarialcuerpo.ActoNotarial.acno_vNumeroEscrituraPublica;

                            actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                            actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                            actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                            actonotarialDA.actualizar_Ep_Foj_Vin(actonotarialBE);
                            if (actonotarialBE.Error == true)
                            {
                                lCancel = true;
                            }
                            else
                            {
                                actonotarialBE.acno_iActoNotarialId = actonotarialcuerpo.ancu_iActoNotarialId;
                                if (actonotarialcuerpo.ActoNotarial.acno_bFlagLeidoAprobado)
                                {
                                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.APROBADA;
                                }
                                else
                                {
                                    actonotarialBE.acno_sEstadoId = (int)Enumerador.enmNotarialProtocolarEstado.TRANSCRITA;
                                }

                                actonotarialBE.acno_sOficinaConsularId = actonotarialcuerpo.OficinaConsultar;
                                actonotarialBE.acno_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                                actonotarialBE.acno_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                                actonotarialBE.acno_sTamanoLetra = actonotarialcuerpo.ActoNotarial.acno_sTamanoLetra;
                                actonotarialDA.actualizar_Estado(actonotarialBE);
                                if (actonotarialBE.Error == true)
                                {
                                    lCancel = true;
                                }
                            }
                        }
                    }
                }

                if (lCancel == true)
                {
                    //Transaction.Current.Rollback();
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                }
            }

            #region Imagen
            if (lstImagenes != null)
            {
                if (lstImagenes.Count > 0)
                {
                    foreach (BE.MRE.RE_ACTONOTARIALDOCUMENTO ActoNotarialDocumento in lstImagenes)
                    {
                        ActoNotarialDocumento.OficinaConsultar = actonotarialcuerpo.OficinaConsultar;
                        ActoNotarialDocumento.ando_sUsuarioCreacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                        ActoNotarialDocumento.ando_vIPCreacion = actonotarialcuerpo.ancu_vIPCreacion;
                        ActoNotarialDocumento.ando_sUsuarioModificacion = actonotarialcuerpo.ancu_sUsuarioCreacion;
                        ActoNotarialDocumento.ando_vIPModificacion = actonotarialcuerpo.ancu_vIPCreacion;
                    }

                    InsertarActoNotarialDocumento(lstImagenes);
                }
            }
            #endregion


            if (lACTONOTARIAL.Error)
            {
                s_Mensaje = lACTONOTARIAL.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIAL.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIAL.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }

            if (actonotarialBE.Error)
            {
                s_Mensaje = actonotarialBE.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = actonotarialBE.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = actonotarialBE.acno_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }


            if (lACTONOTARIALCUERPO.Error)
            {
                s_Mensaje = lACTONOTARIALCUERPO.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = lACTONOTARIALCUERPO.OficinaConsultar,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = lACTONOTARIALCUERPO.ancu_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
        }


        //------------------------------------------------
        //Fecha: 18/10/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Anular el participante
        //------------------------------------------------

        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE AnularParticipante(BE.MRE.RE_ACTONOTARIALPARTICIPANTE participante)
        {
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA objDA = new RE_ACTONOTARIALPARTICIPANTE_DA();

                participante = objDA.AnularParticipante(participante);

                return participante;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int64 AnularPresentante(CBE_PRESENTANTE b)
        {
            Int64 result = 0;
            try
            {
                RE_ACTONOTARIALPARTICIPANTE_DA objDA = new RE_ACTONOTARIALPARTICIPANTE_DA();

                result = objDA.AnularPresentante(b);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    //---------------------------------------------    
    //Fecha: 23/12/2021
    //Autor: Miguel Márquez Beltrán
    //Motivo: Registrar el Acto Notarial.
    //---------------------------------------------    

        public void Registro_ActuacionDetalle_Pago_ActoNotarial(SGAC.BE.MRE.RE_ACTUACIONDETALLE objAD, SGAC.BE.MRE.RE_PAGO objPago, SGAC.BE.MRE.RE_ACTONOTARIALDETALLE objANDE, Int16 intsOficinaConsularId)
        {
            var loption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };
            int intResultado = 0;
            Int64 intActuacionId = 0;
            Int64 intActuacionDetalleId= 0;
            SGAC.Registro.Actuacion.DA.ActuacionMantenimientoDA ActuacionDetalle_DA = new SGAC.Registro.Actuacion.DA.ActuacionMantenimientoDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                intActuacionId = objAD.acde_iActuacionId;
                intResultado = ActuacionDetalle_DA.Registro_ActuacionDetalle(objAD, intsOficinaConsularId);
                
                if (objAD.Error == false)
                {
                   intActuacionDetalleId = objAD.acde_iActuacionDetalleId;
                   intResultado = ActuacionDetalle_DA.Registro_Pago(objPago, intsOficinaConsularId, intActuacionDetalleId);

                   if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                   {
                       intResultado = ActuacionDetalle_DA.Registro_ActoNotarialDetalle(objANDE, intsOficinaConsularId, intActuacionId, intActuacionDetalleId);                      
                   }
                   else
                   {
                       objANDE.Error = true;
                       objANDE.Message = objPago.Message;                      
                   }
                }
                else
                {
                    objANDE.Error = true;
                    objANDE.Message = objAD.Message;
                }

                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    scope.Complete();
                }
                else
                {
                    scope.Dispose();
                }
            }
            if (objANDE.Error)
            {
                s_Mensaje = objANDE.Message;
                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = objANDE.OficinaConsultar,
                        audi_vComentario = "Error al registrar el Acto Notarial.",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = objANDE.ande_sUsuarioCreacion,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }
            }
        }

    //-----------------------------------------------        
    }
}
