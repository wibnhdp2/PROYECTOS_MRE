using System;
using SGAC.Accesorios;
using SGAC.Registro.Persona.DA;

using SGAC.BE.MRE.Custom;
using SGAC.DA.MRE;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class ParticipanteMantenimientoBL
    {
        public string strError = string.Empty;
        private ParticipanteMantenimientoDA objDA;

        public SGAC.BE.MRE.RE_PERSONA insertar_minirune(SGAC.BE.MRE.RE_PERSONA participante) {
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            RE_PERSONA oRE_PERSONA = lPERSONA_DA.insertar_minirune(participante);
            ValidacionError(lPERSONA_DA.strError, participante.OficinaConsultar, participante.pers_sUsuarioCreacion);
            return oRE_PERSONA;
        }
        public SGAC.BE.MRE.RE_PERSONA actualizar_minirune(SGAC.BE.MRE.RE_PERSONA participante)
        {
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            RE_PERSONA oRE_PERSONA = lPERSONA_DA.actualizar_minirune(participante);
            ValidacionError(lPERSONA_DA.strError, participante.OficinaConsultar, participante.pers_sUsuarioCreacion);
            return oRE_PERSONA;
        }   
        public Int64 Insertar(Enumerador.enmTipoActuacionParticipante enmTipoActuacion, 
            BE.RE_PARTICIPANTE objParticipante, ref long LonPersonaId)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            Int64 intParticipanteId = 0;
            switch (enmTipoActuacion)
            {
                case Enumerador.enmTipoActuacionParticipante.JUDICIAL:

                    break;

                case Enumerador.enmTipoActuacionParticipante.NOTARIAL:
                    break;

                default:

                    #region PARTICIPANTE CIVIL y MILITAR

                    objDA = new ParticipanteMantenimientoDA();

                    BE.RE_PERSONA objPersona = null;
                    BE.RE_PERSONAIDENTIFICACION objPersonaIdentificacion = null;
                    BE.RE_PERSONARESIDENCIA objPersonaResidencia = null;
                    BE.RE_RESIDENCIA objResidencia = null;

                    #region Insertar persona

                    objPersona = new BE.RE_PERSONA();
                    objPersonaIdentificacion = new BE.RE_PERSONAIDENTIFICACION();
                    objPersonaResidencia = new BE.RE_PERSONARESIDENCIA();
                    objResidencia = new BE.RE_RESIDENCIA();

                    objPersona.pers_sPersonaTipoId = (int)Enumerador.enmTipoPersona.NATURAL;
                    objPersona.pers_vApellidoPaterno = objParticipante.vPrimerApellido;
                    objPersona.pers_vApellidoMaterno = objParticipante.vSegundoApellido;
                    objPersona.pers_vNombres = objParticipante.vNombres;
                    objPersona.pers_iPersonaId = objParticipante.iPersonaId;

                    objPersonaIdentificacion.peid_sDocumentoTipoId = objParticipante.sTipoDocumentoId;
                    objPersonaIdentificacion.peid_vDocumentoNumero = objParticipante.vNumeroDocumento;

                    objPersona.pers_vCorreoElectronico = string.Empty;
                    objPersona.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                    objPersona.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                    objPersona.pers_sPaisId = objParticipante.pers_sPaisId;

                    objPersona.pers_sGeneroId = Convert.ToInt16(objParticipante.sGeneroId);
                    objPersona.pers_vObservaciones = string.Empty;
                    objPersona.pers_sNacionalidadId = Convert.ToInt16(objParticipante.sNacionalidadId);

                    if (objParticipante.pers_sEstadoCivilId != null)
                    {
                        if (objParticipante.pers_sEstadoCivilId != 0)
                            objPersona.pers_sEstadoCivilId = Convert.ToInt16(objParticipante.pers_sEstadoCivilId);
                        else
                            objPersona.pers_sEstadoCivilId = 0;
                    }
                    else {
                        objPersona.pers_sEstadoCivilId = 0;
                    }

                    objPersona.pers_sEstadoCivilId = Convert.ToInt16(objParticipante.pers_sEstadoCivilId);
                    objPersona.pers_sGradoInstruccionId = 0;
                    objPersona.pers_sOcupacionId = 0;
                    objPersona.pers_sProfesionId = 0;
                    objPersona.pers_vApellidoCasada = string.Empty;
                    objPersona.pers_sUsuarioCreacion = objParticipante.sUsuarioCreacion;
                    objPersona.pers_vIPCreacion = objParticipante.vIPCreacion;

                    //Defuncion
                    objPersona.pers_bFallecidoFlag = objParticipante.pers_bFallecidoFlag;
                    objPersona.pers_dFechaDefuncion = objParticipante.pers_dFechaDefuncion;
                    objPersona.pers_cUbigeoDefuncion = objParticipante.pers_cUbigeoDefuncion;
                    //

                    objResidencia.resi_sResidenciaTipoId = (int)Enumerador.enmTipoResidencia.RESIDENCIA;
                    objResidencia.resi_vResidenciaDireccion = objParticipante.vDireccion;
                    objResidencia.resi_vCodigoPostal = string.Empty;
                    objResidencia.resi_vResidenciaTelefono = string.Empty;
                    objResidencia.resi_cResidenciaUbigeo = objParticipante.vUbigeo;
                    objResidencia.resi_ICentroPobladoId = objParticipante.ICentroPobladoId;
                    objResidencia.OficinaConsularId = objParticipante.sOficinaConsularId;
                    objResidencia.resi_sUsuarioCreacion = objParticipante.sUsuarioCreacion;
                    objResidencia.resi_vIPCreacion = objParticipante.vIPCreacion;
                    objResidencia.resi_iResidenciaId = objParticipante.pere_iResidenciaId;
                    #endregion Insertar persona

                    if (objParticipante.iParticipanteId == 0)
                    {
                        #region Insertar participante

                        BE.RE_ACTUACIONPARTICIPANTE objActParticipante = new BE.RE_ACTUACIONPARTICIPANTE();
                        objActParticipante.acpa_iActuacionParticipanteId = 0;
                        objActParticipante.acpa_iActuacionDetalleId = objParticipante.iActuacionDetId;
                        objActParticipante.acpa_iPersonaId = LonPersonaId;
                        objActParticipante.acpa_sTipoParticipanteId = objParticipante.sTipoParticipanteId;
                        objActParticipante.acpa_sTipoDatoId = objParticipante.sTipoDatoId;
                        objActParticipante.acpa_sTipoVinculoId = objParticipante.sTipoVinculoId;
                        objActParticipante.acpa_cEstado = "A";
                        objActParticipante.acpa_sUsuarioCreacion = objParticipante.sUsuarioCreacion;
                        objActParticipante.OficinaConsularId = objParticipante.sOficinaConsularId;
                        
                        ParticipanteMantenimientoDA objParticipanteDA = new ParticipanteMantenimientoDA();
                        intResult = objParticipanteDA.Insertar(objActParticipante, ref intParticipanteId, ref LonPersonaId, objPersona, objPersonaIdentificacion, objResidencia, objPersonaResidencia);
                        ValidacionError(objParticipanteDA.strError, objActParticipante.OficinaConsularId, objActParticipante.acpa_sUsuarioCreacion);

                        objParticipante.iParticipanteId = intParticipanteId;
                        objParticipante.iPersonaId = LonPersonaId;

                        if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                        {
                            return intParticipanteId;
                        }
                        else
                        {
                            return 0;
                        }


                        #endregion Insertar participante
                    }
                    else
                    {
                        if (objParticipante.cEstado != ((char)Enumerador.enmEstado.DESACTIVO).ToString())
                        {
                            #region Actualizar participante

                            BE.RE_ACTUACIONPARTICIPANTE objActParticipante = new BE.RE_ACTUACIONPARTICIPANTE();
                            objActParticipante.acpa_iActuacionParticipanteId = objParticipante.iParticipanteId;
                            objActParticipante.acpa_iActuacionDetalleId = objParticipante.iActuacionDetId;
                            objActParticipante.acpa_iPersonaId = LonPersonaId;
                            objActParticipante.acpa_sTipoParticipanteId = objParticipante.sTipoParticipanteId;
                            objActParticipante.acpa_sTipoDatoId = objParticipante.sTipoDatoId;
                            objActParticipante.acpa_sTipoVinculoId = objParticipante.sTipoVinculoId;
                            objActParticipante.acpa_cEstado = objParticipante.cEstado;
                            objActParticipante.acpa_sUsuarioModificacion = objParticipante.sUsuarioModificacion;
                            objActParticipante.OficinaConsularId = objParticipante.sOficinaConsularId;

                            objActParticipante.vDirecionParticipante = objParticipante.vDireccion;
                            objActParticipante.vUbigeo = objParticipante.vUbigeo;
                            objActParticipante.vCentroPoblado = objParticipante.ICentroPobladoId;


                            ParticipanteMantenimientoDA objParticipanteDA = new ParticipanteMantenimientoDA();
                            intResult = objParticipanteDA.Actualizar(objActParticipante, objPersona, objPersonaIdentificacion);
                            ValidacionError(objParticipanteDA.strError, objActParticipante.OficinaConsularId, objActParticipante.acpa_sUsuarioCreacion);

                            if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                            {
                                return objParticipante.iParticipanteId;
                            }
                            else
                            {
                                return 0;
                            }

                            #endregion Actualizar participante
                        }
                        else
                        {
                            #region Eliminar participante

                            BE.RE_ACTUACIONPARTICIPANTE objActParticipante = new BE.RE_ACTUACIONPARTICIPANTE();
                            objActParticipante.acpa_iActuacionParticipanteId = objParticipante.iParticipanteId;
                            objActParticipante.acpa_iActuacionDetalleId = objParticipante.iActuacionDetId;
                            objActParticipante.acpa_cEstado = Enumerador.enmEstado.DESACTIVO.ToString();
                            objActParticipante.acpa_sUsuarioModificacion = objParticipante.sUsuarioModificacion;
                            objActParticipante.OficinaConsularId = objParticipante.sOficinaConsularId;

                            ParticipanteMantenimientoDA objParticipanteDA = new ParticipanteMantenimientoDA();
                            intResult = objParticipanteDA.Eliminar(objActParticipante);
                            ValidacionError(objParticipanteDA.strError, objActParticipante.OficinaConsularId, objActParticipante.acpa_sUsuarioCreacion);

                            if (intResult == Convert.ToInt16(Enumerador.enmResultadoQuery.OK))
                            {
                                return objParticipante.iParticipanteId;
                            }
                            else
                            {
                                return 0;
                            }

                            #endregion Eliminar participante
                        }
                    }

                    #endregion PARTICIPANTE CIVIL y MILITAR

            }
            return intParticipanteId;
        }

        public Int64 Actualizar(BE.RE_ACTUACIONPARTICIPANTE objParticipante)
        {

            objDA = new ParticipanteMantenimientoDA();
            int intRet = objDA.Actualizar(objParticipante);
            ValidacionError(objDA.strError, objParticipante.OficinaConsularId, objParticipante.acpa_sUsuarioCreacion);

            return intRet;

        }
        public Int64 ActualizarParticipante(BE.RE_ACTUACIONPARTICIPANTE objParticipante, BE.RE_PERSONA objPersona = null, BE.RE_PERSONAIDENTIFICACION objIdentificacion = null)
        {

            objDA = new ParticipanteMantenimientoDA();
            int intRet = objDA.Actualizar(objParticipante, objPersona, objIdentificacion);
            ValidacionError(objDA.strError, objParticipante.OficinaConsularId, objParticipante.acpa_sUsuarioCreacion);

            return intRet;

        }
        public int Eliminar(BE.RE_ACTUACIONPARTICIPANTE objParticipante)
        {

            objDA = new ParticipanteMantenimientoDA();
            int intRet = objDA.Eliminar(objParticipante);
            ValidacionError(objDA.strError, objParticipante.OficinaConsularId, objParticipante.acpa_sUsuarioCreacion);

            return intRet;
        }

        public int EliminarParticipanteActuacionDetalle(Int64 iActuacionDetalleId,Int16 iUsuarioId,Int16 iOficinaConsula)
        {
            objDA = new ParticipanteMantenimientoDA();
            int intRet = objDA.EliminarParticipanteActuacionDetalle(iActuacionDetalleId, iUsuarioId, iOficinaConsula);
            ValidacionError(objDA.strError, iOficinaConsula, iUsuarioId);

            return intRet;
        }

        public void ValidacionError(string mensaje, Int16 sOficinaConsular, Int16 sUsuario)
        {

            if (!string.IsNullOrEmpty(mensaje))
            {


                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = sOficinaConsular,
                    audi_vComentario = "",
                    audi_vMensaje = mensaje,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = sUsuario,
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });

            }
        }
    }
}
