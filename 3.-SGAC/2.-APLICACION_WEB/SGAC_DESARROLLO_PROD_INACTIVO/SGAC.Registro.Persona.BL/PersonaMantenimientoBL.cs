using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using SGAC.Registro.Persona.DA;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaMantenimientoBL
    {

        private string s_Mensaje { get; set; }

        public Int64 RuneRapido(BE.MRE.RE_PERSONA cbeRune)
        {
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            PersonaMantenimientoDA objPersonaDA = new PersonaMantenimientoDA();
            PersonaIdentificacionMantenimientoDA objPersonaIdentificacionDA = new PersonaIdentificacionMantenimientoDA();
            PersonaFiliacionMantenimientoDA objPersonaFiliacionDA = new PersonaFiliacionMantenimientoDA();
            PersonaResidenciaMantenimientoDA objPersonaResidenciaDA = new PersonaResidenciaMantenimientoDA();

            int intPosicion = 0;

            #region Persona
            if (cbeRune.pers_iPersonaId == 0)
                cbeRune = objPersonaDA.Insertar(cbeRune);
            else
                cbeRune = objPersonaDA.Actualizar(cbeRune);
            #endregion

            if ((cbeRune.Error == false) || (cbeRune.pers_iPersonaId != 0))
            {
                #region RegistroUnico
                if ((lCancel == false) && (cbeRune.REGISTROUNICO != null))
                {
                    if (cbeRune.REGISTROUNICO.reun_iRegistroUnicoId == 0)
                    {
                        cbeRune.REGISTROUNICO.reun_iPersonaId = cbeRune.pers_iPersonaId;
                        cbeRune.REGISTROUNICO.reun_sUsuarioCreacion = cbeRune.pers_sUsuarioCreacion;
                        cbeRune.REGISTROUNICO.OficinaConsultar = cbeRune.OficinaConsultar;

                        cbeRune.REGISTROUNICO = objPersonaDA.InsertarRU(cbeRune.REGISTROUNICO);
                        if ((cbeRune.REGISTROUNICO.Error == true) || (cbeRune.REGISTROUNICO.reun_iRegistroUnicoId == 0))
                        {
                            lCancel = true;
                        }
                    }
                }
                #endregion

                #region PersonaIdentificacion
                if ((lCancel == false) && (cbeRune.Identificacion != null))
                {
                    if (cbeRune.Identificacion.peid_iPersonaIdentificacionId == 0)
                    {
                        cbeRune.Identificacion.peid_iPersonaId = cbeRune.pers_iPersonaId;
                        cbeRune.Identificacion.peid_sUsuarioCreacion = cbeRune.pers_sUsuarioCreacion;
                        cbeRune.Identificacion.OficinaConsultar = cbeRune.OficinaConsultar;

                        cbeRune.Identificacion = objPersonaIdentificacionDA.Insertar(cbeRune.Identificacion);
                        if (cbeRune.Identificacion.Error == true || cbeRune.Identificacion.peid_iPersonaIdentificacionId == 0)
                        {
                            lCancel = true;
                        }
                    }
                }

                #endregion

                #region PersonaResidencia
                if ((lCancel == false && (cbeRune.Residencias.Count != 0)))
                {
                    intPosicion = 0;

                    BE.MRE.RE_RESIDENCIA objResidenciaBase = new BE.MRE.RE_RESIDENCIA();
                    BE.MRE.RE_PERSONARESIDENCIA objPersonaResidenciaBase = new BE.MRE.RE_PERSONARESIDENCIA();

                    foreach (BE.MRE.RE_PERSONARESIDENCIA objPersonaResidencia in cbeRune.Residencias)
                    {
                        // INSERTAR RESIDENCIA
                        if (objPersonaResidencia.Residencia.resi_iResidenciaId == 0)
                        {
                            if (objPersonaResidencia.Residencia.resi_vResidenciaDireccion == null)
                                objPersonaResidencia.Residencia.resi_vResidenciaDireccion = string.Empty;
                            if (objPersonaResidencia.Residencia.resi_vResidenciaTelefono == null)
                                objPersonaResidencia.Residencia.resi_vResidenciaTelefono = string.Empty;

                            objResidenciaBase = objPersonaResidenciaDA.Insertar(objPersonaResidencia.Residencia);

                            if ((objResidenciaBase.Error == true) || (objResidenciaBase.resi_iResidenciaId == 0))
                            {
                                lCancel = true;
                                cbeRune.Residencias[intPosicion].Message = objResidenciaBase.Message;
                                cbeRune.Residencias[intPosicion].Error = objResidenciaBase.Error;
                                break;
                            }
                            else
                            {
                                objPersonaResidenciaBase = new BE.MRE.RE_PERSONARESIDENCIA();
                                objPersonaResidenciaBase.pere_iResidenciaId = objResidenciaBase.resi_iResidenciaId;
                                objPersonaResidenciaBase.pere_sUsuarioCreacion = objResidenciaBase.resi_sUsuarioCreacion;
                                objPersonaResidenciaBase.OficinaConsultar = objResidenciaBase.OficinaConsultar;
                                objPersonaResidenciaBase.pere_iPersonaId = cbeRune.pers_iPersonaId;
                                objPersonaResidenciaBase = objPersonaResidenciaDA.InsertarDR(objPersonaResidenciaBase);
                                if (objPersonaResidenciaBase.Error == true)
                                {
                                    lCancel = true;
                                    cbeRune.Residencias[intPosicion].Message = objPersonaResidenciaBase.Message;
                                    cbeRune.Residencias[intPosicion].Error = objPersonaResidenciaBase.Error;
                                    break;
                                }
                            }
                        }
                        intPosicion++;
                    }
                }
                #endregion

                #region PersonaFiliacion
                if ((lCancel == false) && (cbeRune.FILIACIONES.Count != 0))
                {
                    intPosicion = 0;
                    BE.MRE.RE_PERSONAFILIACION objPersonaFiliacionBase;
                    foreach (BE.MRE.RE_PERSONAFILIACION objPersonaFiliacion in cbeRune.FILIACIONES)
                    {
                        if (objPersonaFiliacion.pefi_iPersonaFilacionId == 0)
                        {
                            objPersonaFiliacion.pefi_iPersonaId = cbeRune.pers_iPersonaId;
                            objPersonaFiliacionBase = objPersonaFiliacionDA.Insertar(objPersonaFiliacion);
                            cbeRune.FILIACIONES[intPosicion].pefi_iPersonaFilacionId = objPersonaFiliacionBase.pefi_iPersonaFilacionId;
                            if ((objPersonaFiliacionBase.Error == true) || (objPersonaFiliacionBase.pefi_iPersonaId == 0))
                            {
                                lCancel = true;
                                cbeRune.FILIACIONES[intPosicion].Message = objPersonaFiliacionBase.Message;
                                cbeRune.FILIACIONES[intPosicion].Error = objPersonaFiliacionBase.Error;
                                break;
                            }
                        }
                        intPosicion++;
                    }
                }
                #endregion
            }
            else
            {
                lCancel = true;
            }

            if (lCancel == true)
                return 0;
            else
                return cbeRune.pers_iPersonaId;
        }
        
        public int Insertar(BE.MRE.RE_PERSONA ObjPersBE,
                            BE.MRE.RE_PERSONAIDENTIFICACION ObjPersIdentBE,
                            BE.MRE.RE_REGISTROUNICO ObjRegistroUnicoBE,
                            int IntFlagIngresoFoto,
                            DataTable DtDirecciones,
                            DataTable DtFiliaciones,
                            DataTable DtImagenes,
                            int IntOficinaConsularId,
                            bool bGenera58A,
                            Int16 CiudadItinerante,
                            ref long LonPersonaId)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            int intResultado = 0;
            Int32 s_Usuario = ObjPersBE.pers_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            #region RUNE RAPIDO - FILIADOS NO EXISTENTES EN RUNE
            if (DtFiliaciones != null)
            {
                foreach (DataRow row in DtFiliaciones.Rows)
                {
                    if ((Convert.ToInt64(row["pefi_iPersonaFilacionId"]) == 0) && (Convert.ToInt64(row["pefi_iFiliadoId"]) == 0))
                    {
                        BE.MRE.RE_PERSONA lPERSONA = new BE.MRE.RE_PERSONA();
                        // I d e n t i f i c a c i ó n
                        lPERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(row["pefi_sDocumentoTipoId"]);
                        lPERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(row["pefi_vNroDocumento"]);
                        // P e r s o n a                        
                        lPERSONA.pers_sPersonaTipoId = (Int16)Enumerador.enmTipoPersona.NATURAL;
                        lPERSONA.pers_sNacionalidadId = Convert.ToInt16(row["pefi_sNacionalidad"]);
                        lPERSONA.pers_vApellidoPaterno = Convert.ToString(row["pers_vApellidoPaterno"]);
                        lPERSONA.pers_vApellidoMaterno = Convert.ToString(row["pers_vApellidoMaterno"]);
                        lPERSONA.pers_vNombres = Convert.ToString(row["pers_vNombres"]);
                        // L o g
                        lPERSONA.HostName = Util.ObtenerHostName();
                        lPERSONA.pers_vIPCreacion = ObjPersBE.pers_vIPModificacion;
                        lPERSONA.pers_sUsuarioCreacion = Convert.ToInt16(ObjPersBE.pers_sUsuarioModificacion);
                        lPERSONA.OficinaConsultar = ObjPersBE.OficinaConsularId;
                        lPERSONA.pers_sUsuarioCreacion = ObjPersBE.pers_sUsuarioCreacion;
                        // U P G R A D E (D A T A T A B L E)
                        Int64 iPersonaId = RuneRapido(lPERSONA);
                        row["pefi_iFiliadoId"] = iPersonaId;
                    }
                }
            }
            #endregion
            ////

            try
            {
                intResultado = objDA.Insertar(ObjPersBE,
                                      ObjPersIdentBE,
                                      ObjRegistroUnicoBE,
                                      IntFlagIngresoFoto,
                                      DtDirecciones,
                                      DtFiliaciones,
                                      DtImagenes,
                                      IntOficinaConsularId,
                                      bGenera58A,
                                      CiudadItinerante,
                                      ref LonPersonaId);

                if (intResultado <= 0) { s_Mensaje = ObjPersBE.Message; }


                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

                return intResultado;
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

        public int Insertar(BE.MRE.RE_PERSONA ObjPersBE,
                            BE.MRE.RE_PERSONAIDENTIFICACION ObjPersIdentBE,
                            BE.MRE.RE_REGISTROUNICO ObjRegistroUnicoBE,
                            int IntFlagIngresoFoto,
                            BE.MRE.RE_RESIDENCIA objResidencia,
                            DataTable DtFiliaciones,
                            DataTable DtImagenes,
                            int IntOficinaConsularId,
                            bool bGenera58A,
                            Int16 CiudadItinerante,
                            ref long LonPersonaId)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            int intResultado = 0;
            Int32 s_Usuario = ObjPersBE.pers_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            #region RUNE RAPIDO - FILIADOS NO EXISTENTES EN RUNE
            if (DtFiliaciones != null)
            {
                foreach (DataRow row in DtFiliaciones.Rows)
                {
                    if ((Convert.ToInt64(row["pefi_iPersonaFilacionId"]) == 0) && (Convert.ToInt64(row["pefi_iFiliadoId"]) == 0))
                    {
                        BE.MRE.RE_PERSONA lPERSONA = new BE.MRE.RE_PERSONA();
                        // I d e n t i f i c a c i ó n
                        lPERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(row["pefi_sDocumentoTipoId"]);
                        lPERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(row["pefi_vNroDocumento"]);
                        // P e r s o n a                        
                        lPERSONA.pers_sPersonaTipoId = (Int16)Enumerador.enmTipoPersona.NATURAL;
                        lPERSONA.pers_sNacionalidadId = Convert.ToInt16(row["pefi_sNacionalidad"]);
                        lPERSONA.pers_vApellidoPaterno = Convert.ToString(row["pers_vApellidoPaterno"]);
                        lPERSONA.pers_vApellidoMaterno = Convert.ToString(row["pers_vApellidoMaterno"]);
                        lPERSONA.pers_vNombres = Convert.ToString(row["pers_vNombres"]);
                        // L o g
                        lPERSONA.HostName = Util.ObtenerHostName();
                        lPERSONA.pers_vIPCreacion = ObjPersBE.pers_vIPModificacion;
                        lPERSONA.pers_sUsuarioCreacion = Convert.ToInt16(ObjPersBE.pers_sUsuarioModificacion);
                        lPERSONA.OficinaConsultar = ObjPersBE.OficinaConsularId;
                        lPERSONA.pers_sUsuarioCreacion = ObjPersBE.pers_sUsuarioCreacion;
                        // U P G R A D E (D A T A T A B L E)
                        Int64 iPersonaId = RuneRapido(lPERSONA);
                        row["pefi_iFiliadoId"] = iPersonaId;
                    }
                }
            }
            #endregion
            ////

            try
            {
                intResultado = objDA.Insertar(ObjPersBE,
                                      ObjPersIdentBE,
                                      ObjRegistroUnicoBE,
                                      IntFlagIngresoFoto,
                                      objResidencia,
                                      DtFiliaciones,
                                      DtImagenes,
                                      IntOficinaConsularId,
                                      bGenera58A,
                                      CiudadItinerante,
                                      ref LonPersonaId);

                if (intResultado <= 0) { s_Mensaje = ObjPersBE.Message; }


                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

                return intResultado;
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

        public int Actualizar(BE.MRE.RE_PERSONA ObjPersBE,
                              BE.MRE.RE_PERSONAIDENTIFICACION ObjPersIdentBE,
                              BE.MRE.RE_REGISTROUNICO ObjRegistroUnicoBE,
                              int IntFlagIngresoFoto,
                              DataTable DtImagenes,
                              DataTable DtFiliaciones,
                              int IntOficinaConsularId,
                              bool bGenera58A,
                              Int16 CiudadItinerante)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            int intResultado = 0;
            Int32 s_Usuario = ObjPersBE.pers_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            ////
            #region RUNE RAPIDO - FILIADOS NO EXISTENTES EN RUNE
            if (DtFiliaciones != null)
            {
                foreach (DataRow row in DtFiliaciones.Rows) 
                { 
                    if ((Convert.ToInt64(row["pefi_iPersonaFilacionId"]) == 0)&&(Convert.ToInt64(row["pefi_iFiliadoId"])==0))
                    {
                        BE.MRE.RE_PERSONA lPERSONA = new BE.MRE.RE_PERSONA();
                        // I d e n t i f i c a c i ó n
                        lPERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(row["pefi_sDocumentoTipoId"]);
                        lPERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(row["pefi_vNroDocumento"]);
                        // P e r s o n a                        
                        lPERSONA.pers_sPersonaTipoId = (Int16)Enumerador.enmTipoPersona.NATURAL;
                        lPERSONA.pers_sNacionalidadId = Convert.ToInt16(row["pefi_sNacionalidad"]);
                        lPERSONA.pers_vApellidoPaterno = Convert.ToString(row["pers_vApellidoPaterno"]);                        
                        lPERSONA.pers_vApellidoMaterno = Convert.ToString(row["pers_vApellidoMaterno"]);
                        lPERSONA.pers_vNombres = Convert.ToString(row["pers_vNombres"]);
                        // L o g
                        lPERSONA.HostName = Util.ObtenerHostName();
                        lPERSONA.pers_vIPCreacion = ObjPersBE.pers_vIPModificacion;
                        lPERSONA.pers_sUsuarioCreacion = Convert.ToInt16(ObjPersBE.pers_sUsuarioModificacion);
                        //lPERSONA.OficinaConsultar = ObjPersBE.OficinaConsularId;
                        lPERSONA.OficinaConsultar = Convert.ToInt16(IntOficinaConsularId);
                        // U P G R A D E (D A T A T A B L E)
                        Int64 iPersonaId = RuneRapido(lPERSONA);
                        row["pefi_iFiliadoId"] = iPersonaId;
                    }
                }
            }
            #endregion
            ////

            try
            {
                #region Validación Filiacion
                if (ObjPersBE.pers_iPersonaId != 0)
                {
                    // discrimar los que encuentre y los demas... eliminarlos
                    PersonaFiliacionConsultaBL objPersonaFiliacionBL = new PersonaFiliacionConsultaBL();
                    PersonaFiliacionMantenimientoBL objFiliacionMantBL = new PersonaFiliacionMantenimientoBL();
                    DataTable dtFiliacionTodos = objPersonaFiliacionBL.Obtener(ObjPersBE.pers_iPersonaId);
                    bool bolEncontro = false;
                    BE.RE_PERSONAFILIACION objFiliacionBE = new BE.RE_PERSONAFILIACION();
                    foreach (DataRow dr in dtFiliacionTodos.Rows)
                    {
                        bolEncontro = false;
                        objFiliacionBE.pefi_iPersonaFilacionId = Convert.ToInt64(dr["pefi_iPersonaFilacionId"]);
                        objFiliacionBE.pefi_sUsuarioModificacion = ObjPersBE.pers_sUsuarioModificacion;
                        objFiliacionBE.pefi_vIPModificacion = ObjPersBE.pers_vIPModificacion;

                        foreach (DataRow drFiliadoActivo in DtFiliaciones.Rows)
                        {
                            if (Convert.ToInt64(drFiliadoActivo["pefi_iPersonaFilacionId"]) != 0)
                            {
                                if (Convert.ToInt64(dr["pefi_iPersonaFilacionId"]) == Convert.ToInt64(drFiliadoActivo["pefi_iPersonaFilacionId"]))
                                {
                                    bolEncontro = true;
                                    break;
                                }
                            }
                        }
                        if (!bolEncontro)
                        {
                            objFiliacionMantBL.Eliminar(objFiliacionBE, IntOficinaConsularId);
                        }
                    }

                }
                #endregion

                intResultado =  objDA.Actualizar(ObjPersBE,
                                        ObjPersIdentBE,
                                        ObjRegistroUnicoBE,
                                        IntFlagIngresoFoto,
                                        DtImagenes,
                                        DtFiliaciones,
                                        IntOficinaConsularId,
                                        bGenera58A,
                                        CiudadItinerante);

                if (intResultado <= 0) { s_Mensaje = ObjPersBE.Message; }

                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

                return intResultado;
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

        public int Eliminar(BE.MRE.RE_PERSONA ObjPersBE,
                            long IntRegUnico,
                            int IntOficinaConsularId)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();
            int intResultado = 0;
            Int32 s_Usuario = ObjPersBE.pers_sUsuarioModificacion;
            Int16 s_OficinaConsular = Convert.ToInt16(IntOficinaConsularId);

            try
            {
                intResultado = objDA.Eliminar(ObjPersBE,
                                      IntRegUnico,
                                      IntOficinaConsularId);

                if (intResultado <= 0) {
                    s_Mensaje = ObjPersBE.Message; 
                }


                if (!string.IsNullOrEmpty(s_Mensaje))
                {
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new SGAC.BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_sOperacionTipoId = (Int16)Enumerador.enmTipoIncidencia.ERROR_DATA_BASE,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = (Int16)s_OficinaConsular,
                        audi_vComentario = "",
                        audi_vMensaje = s_Mensaje,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = (Int16)s_Usuario,
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });

                    s_Mensaje = Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO;
                }

                return intResultado;
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


        // YA NO SE INVOCA DESDE JUDICIAL - SE CAMBIO POR MOTIVO DE CAMBIAR EL HELPER
        public bool InsertarRuneRapido(ref DataTable dtParticipantes, 
                                       Int16 sOficinaConsularId, 
                                       string vHostName, 
                                       Int16 intUsuarioCreacionId, 
                                       string vIPCreacion)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            try
            {
                return objDA.InsertarRuneRapido(ref dtParticipantes, 
                                                sOficinaConsularId, 
                                                vHostName, 
                                                intUsuarioCreacionId, 
                                                vIPCreacion);
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
        
        /*
        public string Actualizar(BE.MRE.Custom.CBE_PERSONA cbePersona, int IntFlagIngresoFoto,
            int intOficinaConsularId, bool bolGenera58A)
        {
            int intResultado = 0;
            bool lCancel = false;
            var loption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(60) };

            PersonaMantenimientoDA objPersonaDA = new PersonaMantenimientoDA();
            PersonaResidenciaMantenimientoDA objResidenciaDA = new PersonaResidenciaMantenimientoDA();
            PersonaFiliacionMantenimientoDA objFiliacionDA = new PersonaFiliacionMantenimientoDA();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, loption))
            {
                #region TITULAR
                intResultado = objPersonaDA.Actualizar(cbePersona.PERSONA, 
                    cbePersona.IDENTIFICACION, cbePersona.REGISTROUNICO,
                    IntFlagIngresoFoto, null, intOficinaConsularId, bolGenera58A);
                #endregion

                #region RESIDENCIAS
                BE.RE_PERSONARESIDENCIA objPersonaResidencia = new RE_PERSONARESIDENCIA();
                foreach (BE.RE_RESIDENCIA objResidencia in cbePersona.RESIDENCIAS)
                {
                    objPersonaResidencia = new RE_PERSONARESIDENCIA();
                    objPersonaResidencia.pere_iResidenciaId = objPersonaResidencia.pere_iResidenciaId;
                    objPersonaResidencia.pere_iPersonaId = cbePersona.PERSONA.pers_iPersonaId;
                    objPersonaResidencia.pere_cEstado = objResidencia.resi_cEstado;
                    objPersonaResidencia.pere_sUsuarioCreacion = objResidencia.resi_sUsuarioCreacion;
                    objPersonaResidencia.pere_vIPCreacion = objResidencia.resi_vIPCreacion;
                    objPersonaResidencia.pere_sUsuarioModificacion = objResidencia.resi_sUsuarioModificacion;
                    objPersonaResidencia.pere_vIPModificacion = objResidencia.resi_vIPModificacion;
                    if (objResidencia.resi_iResidenciaId == 0)
                        intResultado = objResidenciaDA.Insertar(objResidencia, objPersonaResidencia, intOficinaConsularId);
                    else
                        intResultado = objResidenciaDA.Actualizar(objResidencia, objPersonaResidencia, intOficinaConsularId);
                }
                #endregion

                #region FILIACIONES
                Int64 lngPersonaId = 0;

                BE.RE_PERSONA objPersona;
                BE.RE_PERSONAIDENTIFICACION objIdentificacion;
                BE.RE_REGISTROUNICO objRegistroUnico;
                BE.RE_PERSONAFILIACION objFiliacion;
                foreach (BE.MRE.Custom.CBE_FILIACION cbeFiliacion in cbePersona.FILIACIONES)
                {
                    if (cbeFiliacion.pefi_iPersonaId == 0)
                    {
                        objPersona = new RE_PERSONA();
                        objIdentificacion = new RE_PERSONAIDENTIFICACION();
                        objRegistroUnico = new RE_REGISTROUNICO();

                        #region Default Persona
                        objPersona.pers_vNombres = cbeFiliacion.pers_vNombres;
                        objPersona.pers_vApellidoPaterno = cbeFiliacion.pers_vApellidoPaterno;
                        objPersona.pers_vApellidoMaterno = cbeFiliacion.pers_vApellidoMaterno;
                        objPersona.pers_sUsuarioCreacion = cbeFiliacion.pefi_sUsuarioCreacion;
                        objPersona.pers_vIPCreacion = cbeFiliacion.pefi_vIPCreacion;

                        objIdentificacion.peid_sDocumentoTipoId = cbeFiliacion.peid_sDocumentoTipoId;
                        objIdentificacion.peid_vDocumentoNumero = cbeFiliacion.peid_vDocumentoNumero;
                        objIdentificacion.peid_sUsuarioCreacion = cbeFiliacion.pefi_sUsuarioCreacion;
                        objIdentificacion.peid_vIPCreacion = cbeFiliacion.pefi_vIPCreacion;

                        objRegistroUnico.reun_sUsuarioCreacion = cbeFiliacion.pefi_sUsuarioCreacion;
                        objRegistroUnico.reun_vIPCreacion = cbeFiliacion.pefi_vIPCreacion;
                        #endregion

                        lngPersonaId = objPersonaDA.InsertarPersona(objPersona, objIdentificacion, objRegistroUnico, Convert.ToInt16(intOficinaConsularId), bolGenera58A);
                        cbeFiliacion.pefi_iPersonaId = lngPersonaId;
                    }

                    objFiliacion = new RE_PERSONAFILIACION();
                    objFiliacion.pefi_iPersonaId = cbeFiliacion.pefi_iPersonaId;
                    objFiliacion.pefi_vNombreFiliacion = cbeFiliacion.pefi_vNombreFiliacion;
                    objFiliacion.pefi_vLugarNacimiento = cbeFiliacion.pefi_vLugarNacimiento;
                    objFiliacion.pefi_dFechaNacimiento = cbeFiliacion.pefi_dFechaNacimiento;
                    objFiliacion.pefi_sNacionalidad = cbeFiliacion.pefi_sNacionalidad;
                    objFiliacion.pefi_sTipoFilacionId = cbeFiliacion.pefi_sTipoFilacionId;
                    objFiliacion.pefi_vNroDocumento = cbeFiliacion.pefi_vNroDocumento;
                    objFiliacion.pefi_cEstado = cbeFiliacion.pefi_cEstado;                    
                    if (cbeFiliacion.pefi_iPersonaFilacionId == 0)
                    {
                        objFiliacion.pefi_sUsuarioCreacion = cbeFiliacion.pefi_sUsuarioCreacion;
                        objFiliacion.pefi_vIPCreacion = cbeFiliacion.pefi_vIPCreacion;
                        objFiliacionDA.Insertar(objFiliacion, intOficinaConsularId);
                    }
                    else
                    {
                        objFiliacion.pefi_sUsuarioModificacion = cbeFiliacion.pefi_sUsuarioModificacion;
                        objFiliacion.pefi_vIPModificacion = cbeFiliacion.pefi_vIPModificacion;
                        objFiliacionDA.Actualizar(objFiliacion, intOficinaConsularId);
                    }
                }
                #endregion

                if (!lCancel) { scope.Complete(); }
                else
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                }
            }
            return string.Empty;
        }
         * 
         * */

        public int ActualizarPorCodigo(Int64 PersonaId,
                            int ConsuladoId,
                            int intUsuarioModifica,
                            bool bFacellido)
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            try
            {
                return objDA.ActualizarPorCodigo(PersonaId,
                                                ConsuladoId,
                                                intUsuarioModifica,
                                                bFacellido
                                                );
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

        public int RegistrarNacionalidad(Int64 PersonaId,
                            Int16 PaisId,
                            string vNacionalidad,
                            bool bVigente,
                            string cEstado,
                            Int16 intUsuarioModifica
                            )
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            try
            {
                return objDA.RegistrarNacionalidad(PersonaId,
                                                PaisId,
                                                vNacionalidad,
                                                bVigente,
                                                cEstado,
                                                intUsuarioModifica
                                                );
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

        public int EliminarNacionalidad(Int64 PersonaId,
                            Int16 PaisId,
                            Int16 intUsuarioModifica
                            )
        {
            DA.PersonaMantenimientoDA objDA = new DA.PersonaMantenimientoDA();

            try
            {
                return objDA.EliminarNacionalidad(PersonaId,
                                                PaisId,
                                                intUsuarioModifica
                                                );
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
    }
}
