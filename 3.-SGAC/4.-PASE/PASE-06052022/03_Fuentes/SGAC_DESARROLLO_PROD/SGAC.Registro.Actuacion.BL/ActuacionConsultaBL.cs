using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;
using SGAC.Registro.Persona.BL;

namespace SGAC.Registro.Actuacion.BL
{
    using SGAC.DA.MRE.ACTUACION;

    public class ActuacionConsultaBL
    {
        public DataTable VerificarRegistroParticipantes(Int64 lngActuacionDetalleId, Int16 sTipoParticipanteId, Int64 iPersona = 0)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.VerificarRegistroParticipantes(lngActuacionDetalleId,
                                              sTipoParticipanteId, iPersona);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public DataTable RecurrenteConsultar(int IntTipo,
                                             string StrNroDoc,
                                             string StrApePat,
                                             string StrApeMat,
                                             string strNombre,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.RecurrenteConsultar(IntTipo,
                                              StrNroDoc,
                                              StrApePat,
                                              StrApeMat,
                                              strNombre,
                                              IntCurrentPage,
                                              IntPageSize,
                                              ref IntTotalCount,
                                              ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable EmpresaConsultar(string StrNroDoc,
                                          string StrRazonSocial,
                                          int IntCurrentPage,
                                          int IntPageSize,
                                          ref int IntTotalCount,
                                          ref int IntTotalPages)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.EmpresaConsultar(StrNroDoc,
                                              StrRazonSocial,
                                              IntCurrentPage,
                                              IntPageSize,
                                              ref IntTotalCount,
                                              ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ReasignacionConsultar(long LonPersonaId,
                                               string StrNroDoc,
                                               string StrApePat,
                                               string StrApeMat,
                                               string StrCurrentPage,
                                               int IntPageSize,
                                               ref int IntTotalCount,
                                               ref int IntTotalPages)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ReasignacionConsultar(LonPersonaId,
                                                StrNroDoc,
                                                StrApePat,
                                                StrApeMat,
                                                StrCurrentPage,
                                                IntPageSize,
                                                ref IntTotalCount,
                                                ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ActuacionesObtener(long LonPersonaId, long LonEmpresaId, int intSeccionId,
            DateTime datFecInicio, DateTime datFecFin, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ActuacionesObtener(LonPersonaId, LonEmpresaId, intSeccionId, datFecInicio, datFecFin, StrCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public DataTable ActuacionesObtenerxRGE(Int16 Consulado,
                                            Int16 Anio,
                                            Int64 RGE)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ActuacionesObtenerxRGE(Consulado, Anio, RGE);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        
        //public DataTable ActuacionesObtenerPorAutoadhesivo(string StrNroDoc, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        public DataTable ActuacionesObtenerPorAutoadhesivo(string StrNroDoc, int StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ActuacionesObtenerPorAutoadhesivo(StrNroDoc, StrCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ActuacionAutuadhesivo(long acde_iActuacionDetalleId)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ActuacionAutoadhesivo(acde_iActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerDatosAutoadhesivoNotarial(long lngActuacionId, long lngActuacionDetalleId, Int32 iLimiteTarifa)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerDatosAutoadhesivoNotarial(lngActuacionId, lngActuacionDetalleId, iLimiteTarifa);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public int ObtenerSaldoAutoadhesivos(int IntOfficinaConsular, int IntBodegaDestinoId, int intTipoInsumo)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerSaldoAutoadhesivos(IntOfficinaConsular, IntBodegaDestinoId, intTipoInsumo);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public int ObtenerSaldoInsumos(int IntOfficinaConsular, int intTipoInsumo)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerSaldoInsumos(IntOfficinaConsular, intTipoInsumo);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public int ObtenerStockMinimoInsumos(int IntOfficinaConsular, int IntBodegaDestinoId, int intTipoInsumo)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerStockMinimoInsumos(IntOfficinaConsular, IntBodegaDestinoId, intTipoInsumo);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerDatosPorActuacionDetalle(long lngActuacionDetalleId)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerDatosPorActuacionDetalle(lngActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        public DataTable ObtenerDatosPorActuacionDetalleLeftJoin(long lngActuacionDetalleId)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ObtenerDatosPorActuacionDetalleLeftJoin(lngActuacionDetalleId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        
        public DataTable ObtenerSeguimientoActuacion(long lngActuacionDetalleId)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            return objDA.ObtenerSeguimientoActuacion(lngActuacionDetalleId);
        }

        public DataTable ActuacionDetalleObtener(long LonActuacionIdPrimario, long LonActuacionIdSec)
        {
            DA.ActuacionConsultaDA objDA = new DA.ActuacionConsultaDA();

            try
            {
                return objDA.ActuacionDetalleObtener(LonActuacionIdPrimario, LonActuacionIdSec);
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

        public int ObtenerCantidadPorTarifa(Int64 lngPersonaId, Int16 intTarifaId)
        {
            ActuacionConsultaDA objActuacionDA = new ActuacionConsultaDA();
            return objActuacionDA.ObtenerCantidadPorTarifa(lngPersonaId, intTarifaId);
        }

        public Int64 ActuacionTramiteExiste(long LonActuacionDetalleId, int IntTipoActo)
        {
            DA.ActuacionConsultaDA objDA = new DA.ActuacionConsultaDA();

            try
            {
                return objDA.ActuacionTramiteExiste(LonActuacionDetalleId, IntTipoActo);
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

        public DataTable ActuacionDetalleObtener_Actuacion(long iPersonaID, long iActuacionID)
        {
            ActuacionConsultaDA oActuacionConsultaDA = new ActuacionConsultaDA();
            return oActuacionConsultaDA.ObtenerActuacionDetalle_Actuacion(iPersonaID, iActuacionID);
        }
        
        //public DataTable ActuacionDetalleObtenerEstado(Int64 iActuacionDetalleId)
        //{
        //    ActuacionConsultaDA objDA = new ActuacionConsultaDA();
        //    try
        //    {
        //        return objDA.ActuacionDetalleObtenerEstado(iActuacionDetalleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SGACExcepcion(ex.Message, ex.InnerException);
        //    }
        //    finally
        //    {
        //        objDA = null;
        //    }
        //}

        #region Actos militar y Civil

        public BE.RE_ACTUACIONMILITAR ObtenerActuacionMilitar(Int64 lngActuacionDetalleId, Int64 lngPersonaId)
        {
            try
            {
                BE.RE_ACTUACIONMILITAR objActuacionMilitar = new RE_ACTUACIONMILITAR();

                BE.RE_REGISTROMILITAR objMilitar = new RE_REGISTROMILITAR();
                BE.RE_PERSONA objPersona = new RE_PERSONA();
                BE.RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
                BE.RE_RESIDENCIA objResidencia = new RE_RESIDENCIA();
                List<BE.RE_PARTICIPANTE> lstParticipante = new List<RE_PARTICIPANTE>();

                #region SET DEFAULT PERSONA ...

                objPersona.pers_iPersonaId = -1;	//bigint	1
                objPersona.pers_sEstadoCivilId = -1;	//smallint	2
                objPersona.pers_sProfesionId = -1;	//smallint	47
                objPersona.pers_sOcupacionId = -1;	//smallint	30
                objPersona.pers_sIdiomaNatalId = -1;	//smallint	NULL
                objPersona.pers_sGeneroId = -1;	//smallint	2001
                objPersona.pers_sGradoInstruccionId = -1; //smallint	2152
                objPersona.pers_vApellidoPaterno = string.Empty.ToString();	//varchar	CASTRO
                objPersona.pers_vApellidoMaterno = string.Empty.ToString();	//varchar	ALVA
                objPersona.pers_vNombres = string.Empty.ToString();	//varchar	FERNANDO
                objPersona.pers_vCorreoElectronico = string.Empty.ToString();	//varchar	RRRRR@GMAIL.COM
                objPersona.pers_dNacimientoFecha = DateTime.MinValue;	//datetime	NULL
                objPersona.pers_sPersonaTipoId = -1;	//smallint	2101
                objPersona.pers_cNacimientoLugar = string.Empty.ToString();	//char	220302
                objPersona.pers_vObservaciones = string.Empty.ToString();	//varchar	ACTUALIZADO REGISTRO MILITAR
                objPersona.pers_sNacionalidadId = -1;	//smallint	2051
                objPersona.pers_vEstatura = string.Empty.ToString();	//varchar	NULL
                objPersona.pers_sColorTezId = -1; //smallint	NULL
                objPersona.pers_sColorOjosId = -1;	//smallint	NULL
                objPersona.pers_sColorCabelloId = -1; //smallint	NULL
                objPersona.pers_sGrupoSanguineoId = -1;	//smallint	NULL
                objPersona.pers_vSenasParticulares = string.Empty.ToString();	//varchar	NULL
                objPersona.pers_bFallecidoFlag = false;	//bit	NULL
                objPersona.pers_dFechaDefuncion = DateTime.MinValue;	//datetime	NULL
                objPersona.pers_cUbigeoDefuncion = string.Empty.ToString();	//char	NULL
                objPersona.pers_vApellidoCasada = string.Empty.ToString();//varchar	NULL
                objPersona.pers_cEstado = "A"; //char	A
                //objPersona.pers_sUsuarioCreacion	//smallint	5
                //objPersona.pers_vIPCreacion	//varchar	::1
                //objPersona.pers_dFechaCreacion	//datetime	2015-02-06 23:32:35.900
                //objPersona.pers_sUsuarioModificacion	//smallint	1
                //objPersona.pers_vIPModificacion	//varchar	1.1.
                //objPersona.pers_dFechaModificacion	//datetime	2015-02-06 23:35:59.510
                objPersona.pers_sPeso = 0;	//smallint	NULL
                objPersona.pers_sOcurrenciaTipoId = -1;	//smallint	NULL
                objPersona.pers_IOcurrenciaCentroPobladoId = -1;	//int	NULL
                objPersona.pers_vLugarNacimiento = string.Empty.ToString();	//varchar	NULL

                #endregion SET DEFAULT PERSONA ...

                #region SET DEFAULT IDENTIFICACION ...

                objIdentificacion.peid_iPersonaIdentificacionId = -1; //bigint	90
                objIdentificacion.peid_iPersonaId = -1; //bigint	91
                objIdentificacion.peid_sDocumentoTipoId = -1; //smallint	1
                objIdentificacion.peid_vDocumentoNumero = string.Empty.ToString(); //varchar	55555555
                objIdentificacion.peid_dFecVcto = DateTime.MinValue; //datetime	NULL
                objIdentificacion.peid_dFecExpedicion = DateTime.MinValue; //datetime	NULL
                objIdentificacion.peid_vLugarExpedicion = string.Empty.ToString();//varchar	NULL
                objIdentificacion.peid_dFecRenovacion = DateTime.MinValue; 	//datetime	NULL
                objIdentificacion.peid_vLugarRenovacion = string.Empty.ToString();//varchar	NULL
                objIdentificacion.peid_bActivoEnRune = false;	//bit	1
                objIdentificacion.peid_cEstado = "A";	//char	A
                objIdentificacion.peid_sUsuarioCreacion = -1; //smallint	2
                objIdentificacion.peid_vIPCreacion = string.Empty.ToString(); //varchar	::1
                objIdentificacion.peid_dFechaCreacion = DateTime.MinValue; //datetime	2015-02-08 21:47:13.580
                objIdentificacion.peid_sUsuarioModificacion = -1; //smallint	NULL
                objIdentificacion.peid_vIPModificacion = string.Empty.ToString(); //varchar	NULL
                objIdentificacion.peid_dFechaModificacion = DateTime.MinValue; 	//datetime	NULL

                #endregion SET DEFAULT IDENTIFICACION ...

                #region SET DEFAULT RESIDENCIA

                objResidencia.resi_iResidenciaId = -1; // bigint	1
                objResidencia.resi_sResidenciaTipoId = -1; // smallint	2251
                objResidencia.resi_vResidenciaDireccion = string.Empty.ToString(); // varchar	calle 15 mz q dpt c-1
                objResidencia.resi_vCodigoPostal = string.Empty.ToString(); //varchar	51
                objResidencia.resi_vResidenciaTelefono = string.Empty.ToString(); //varchar	5535532
                objResidencia.resi_cResidenciaUbigeo = string.Empty.ToString(); //char	240106
                objResidencia.resi_cEstado = "A"; //char	A
                objResidencia.resi_sUsuarioCreacion = -1; // smallint	2
                objResidencia.resi_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                objResidencia.resi_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 09:05:30.850
                objResidencia.resi_sUsuarioModificacion = -1; // smallint	NULL
                objResidencia.resi_vIPModificacion = string.Empty.ToString(); //	varchar	NULL
                objResidencia.resi_dFechaModificacion = DateTime.MinValue; // datetime	NULL
                objResidencia.resi_ICentroPobladoId = -1; // int	NULL

                #endregion SET DEFAULT RESIDENCIA

                #region SET DEFAULT MILITAR ...

                objMilitar.remi_iRegistroMilitarId = 0; // bigint	19
                objMilitar.remi_iActuacionDetalleId = 0; // bigint	127
                objMilitar.remi_sCalificacionMilitarId = 0;// smallint	7901
                objMilitar.remi_sInstitucionMilitarId = 0; // smallint	4901
                objMilitar.remi_IFuncionarioId = 0; // int	NULL
                objMilitar.remi_sServicioReservaId = 0; // smallint	7921
                objMilitar.remi_vClase = string.Empty.ToString();//	varchar	66666
                objMilitar.remi_vLibro = string.Empty.ToString();//	varchar	1
                objMilitar.remi_sFolio = 0; // smallint	1
                objMilitar.remi_sNumeroHijos = 0; // smallint	2
                objMilitar.remi_IUsuarioAprobacionId = 0; // int	5
                objMilitar.remi_vIPAprobacion = string.Empty.ToString(); //varchar	::1
                objMilitar.remi_dFechaAprobacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.663
                objMilitar.remi_bDigitalizadoFlag = false; //	bit	1
                objMilitar.remi_sTipoSuscripcion = 0; // smallint	NULL
                objMilitar.remi_vObservaciones = string.Empty.ToString(); //varchar
                objMilitar.remi_cEstado = "A"; //char	A
                objMilitar.remi_sUsuarioCreacion = 0; // smallint	5
                objMilitar.remi_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                objMilitar.remi_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.680
                objMilitar.remi_sUsuarioModificacion = 0; // smallint	NULL
                objMilitar.remi_vIPModificacion = string.Empty.ToString(); // varchar	NULL
                objMilitar.remi_dFechaModificacion = DateTime.MinValue; // datetime	NULL

                #endregion SET DEFAULT MILITAR ...

                PersonaConsultaBL objPersonaConsultaBL = new PersonaConsultaBL();
                DataTable dtPersona = objPersonaConsultaBL.PersonaGetById(lngPersonaId);
                if (dtPersona.Rows.Count > 0)
                {
                    DataRow drPersona = dtPersona.Rows[0];

                    if (drPersona["iPersonaId"] != System.DBNull.Value) { objPersona.pers_iPersonaId = Convert.ToInt64(drPersona["iPersonaId"]); }
                    if (drPersona["sPersonaTipoId"] != System.DBNull.Value) { objPersona.pers_sPersonaTipoId = Convert.ToInt16(drPersona["sPersonaTipoId"]); }
                    if (drPersona["sDocumentoTipoId"] != System.DBNull.Value) { objIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(drPersona["sDocumentoTipoId"]); }
                    if (drPersona["vNroDocumento"] != System.DBNull.Value) { objIdentificacion.peid_vDocumentoNumero = drPersona["vNroDocumento"].ToString(); }
                    if (drPersona["vApellidoPaterno"] != System.DBNull.Value) { objPersona.pers_vApellidoPaterno = drPersona["vApellidoPaterno"].ToString(); }
                    if (drPersona["vApellidoMaterno"] != System.DBNull.Value) { objPersona.pers_vApellidoMaterno = drPersona["vApellidoMaterno"].ToString(); }
                    if (drPersona["vNombres"] != System.DBNull.Value) { objPersona.pers_vNombres = drPersona["vNombres"].ToString(); }
                    if (drPersona["vCorreoElectronico"] != System.DBNull.Value) { objPersona.pers_vCorreoElectronico = drPersona["vCorreoElectronico"].ToString(); }

                    if (drPersona["sGeneroId"] != System.DBNull.Value) { objPersona.pers_sGeneroId = Convert.ToInt16(drPersona["sGeneroId"]); }
                    if (drPersona["sNacionalidadId"] != System.DBNull.Value) { objPersona.pers_sNacionalidadId = Convert.ToInt16(drPersona["sNacionalidadId"]); }
                    if (drPersona["sOcupacionId"] != System.DBNull.Value) { objPersona.pers_sOcupacionId = Convert.ToInt16(drPersona["sOcupacionId"]); }
                    if (drPersona["sEstadoCivilId"] != System.DBNull.Value) { objPersona.pers_sEstadoCivilId = Convert.ToInt16(drPersona["sEstadoCivilId"]); }
                    if (drPersona["sGradoInstruccionId"] != System.DBNull.Value) { objPersona.pers_sGradoInstruccionId = Convert.ToInt16(drPersona["sGradoInstruccionId"]); }
                    if (drPersona["sProfesionId"] != System.DBNull.Value) { objPersona.pers_sProfesionId = Convert.ToInt16(drPersona["sProfesionId"]); }
                    if (drPersona["pers_sColorCabelloId"] != System.DBNull.Value) { objPersona.pers_sColorCabelloId = Convert.ToInt16(drPersona["pers_sColorCabelloId"]); }
                    if (drPersona["pers_sColorOjosId"] != System.DBNull.Value) { objPersona.pers_sColorOjosId = Convert.ToInt16(drPersona["pers_sColorOjosId"]); }
                    if (drPersona["pers_sColorTezId"] != System.DBNull.Value) { objPersona.pers_sColorTezId = Convert.ToInt16(drPersona["pers_sColorTezId"]); }
                    if (drPersona["pers_vSenasParticulares"] != System.DBNull.Value) { objPersona.pers_vSenasParticulares = drPersona["pers_vSenasParticulares"].ToString(); }
                    if (drPersona["pers_sGrupoSanguineoId"] != System.DBNull.Value) { objPersona.pers_sGrupoSanguineoId = Convert.ToInt16(drPersona["pers_sGrupoSanguineoId"]); }
                    if (drPersona["pers_sPeso"] != System.DBNull.Value) { objPersona.pers_sPeso = Convert.ToInt16(drPersona["pers_sPeso"]); }
                    if (drPersona["pers_vEstatura"] != System.DBNull.Value) { objPersona.pers_vEstatura = drPersona["pers_vEstatura"].ToString(); }

                    if (drPersona["pers_sOcurrenciaTipoId"] != System.DBNull.Value) { objPersona.pers_sOcurrenciaTipoId = Convert.ToInt16(drPersona["pers_sOcurrenciaTipoId"]); }
                    if (drPersona["pers_vLugarNacimiento"] != System.DBNull.Value) { objPersona.pers_vLugarNacimiento = Convert.ToString(drPersona["pers_vLugarNacimiento"]); }
                    if (drPersona["cNacimientoLugar"] != System.DBNull.Value) { objPersona.pers_cNacimientoLugar = drPersona["cNacimientoLugar"].ToString(); }
                    if (drPersona["dNacimientoFecha"].ToString() != string.Empty)
                        objPersona.pers_dNacimientoFecha = Convert.ToDateTime(drPersona["dNacimientoFecha"]);
                    else
                        objPersona.pers_dNacimientoFecha = DateTime.MinValue;
                }

                PersonaResidenciaConsultaBL objPersonaResidenciaBL = new PersonaResidenciaConsultaBL();
                DataTable dtResidencia = objPersonaResidenciaBL.Obtener(lngPersonaId);
                if (dtResidencia.Rows.Count > 0)
                {
                    DataRow dr = dtResidencia.Rows[0];
                    if (dr["iResidenciaId"] != System.DBNull.Value) { objResidencia.resi_iResidenciaId = Convert.ToInt64(dr["iResidenciaId"]); }
                    if (dr["vCodigoPostal"] != System.DBNull.Value) { objResidencia.resi_vCodigoPostal = dr["vCodigoPostal"].ToString(); }
                    if (dr["vResidenciaDireccion"] != System.DBNull.Value) { objResidencia.resi_vResidenciaDireccion = dr["vResidenciaDireccion"].ToString(); }
                    if (dr["sResidenciaTipoId"] != System.DBNull.Value) { objResidencia.resi_sResidenciaTipoId = Convert.ToInt16(dr["sResidenciaTipoId"]); }
                    if (dr["cResidenciaUbigeo"] != System.DBNull.Value) { objResidencia.resi_cResidenciaUbigeo = dr["cResidenciaUbigeo"].ToString(); }
                    if (dr["vResidenciaTelefono"] != System.DBNull.Value) { objResidencia.resi_vResidenciaTelefono = dr["vResidenciaTelefono"].ToString(); }
                }

                #region Militar

                ActoMilitarConsultaBL objActoMilitarConsultaBL = new ActoMilitarConsultaBL();
                DataSet ds = objActoMilitarConsultaBL.Obtener(lngActuacionDetalleId);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtRegistroMilitar = ds.Tables[0];
                    if (dtRegistroMilitar.Rows.Count > 0)
                    {
                        DataRow drMilitar = dtRegistroMilitar.Rows[0];
                        if (drMilitar["remi_iRegistroMilitarId"] != System.DBNull.Value) { objMilitar.remi_iRegistroMilitarId = Convert.ToInt64(drMilitar["remi_iRegistroMilitarId"]); }
                        if (drMilitar["remi_iActuacionDetalleId"] != System.DBNull.Value) { objMilitar.remi_iActuacionDetalleId = Convert.ToInt64(drMilitar["remi_iActuacionDetalleId"]); }
                        if (drMilitar["remi_sCalificacionMilitarId"] != System.DBNull.Value) { objMilitar.remi_sCalificacionMilitarId = Convert.ToInt16(drMilitar["remi_sCalificacionMilitarId"]); }
                        if (drMilitar["remi_sInstitucionMilitarId"] != System.DBNull.Value) { objMilitar.remi_sInstitucionMilitarId = Convert.ToInt16(drMilitar["remi_sInstitucionMilitarId"]); }
                        if (drMilitar["remi_sServicioReservaId"] != System.DBNull.Value) { objMilitar.remi_sServicioReservaId = Convert.ToInt16(drMilitar["remi_sServicioReservaId"]); }
                        if (drMilitar["remi_vClase"] != System.DBNull.Value) { objMilitar.remi_vClase = drMilitar["remi_vClase"].ToString(); }
                        if (drMilitar["remi_vLibro"] != System.DBNull.Value) { objMilitar.remi_vLibro = drMilitar["remi_vLibro"].ToString(); }
                        if (drMilitar["remi_sFolio"] != System.DBNull.Value) { objMilitar.remi_sFolio = Convert.ToInt16(drMilitar["remi_sFolio"]); }
                        if (drMilitar["remi_sNumeroHijos"] != System.DBNull.Value) { objMilitar.remi_sNumeroHijos = Convert.ToInt16(drMilitar["remi_sNumeroHijos"]); }
                        if (drMilitar["remi_sTipoSuscripcion"] != System.DBNull.Value) { objMilitar.remi_sTipoSuscripcion = Convert.ToInt16(drMilitar["remi_sTipoSuscripcion"]); }

                        DataTable dtParticipantes = ds.Tables[1];
                        if (dtParticipantes != null)
                        {
                            if (dtParticipantes.Rows.Count > 0)
                            {
                                BE.RE_PARTICIPANTE objParticipante;

                                foreach (DataRow drParticipante in dtParticipantes.Rows)
                                {
                                    objParticipante = new RE_PARTICIPANTE();

                                    #region SET DEFAULT PARTICIPANTE ...

                                    objParticipante.sTipoActuacionId = -1;
                                    objParticipante.iParticipanteId = 0;
                                    objParticipante.iActuacionDetId = 0;
                                    objParticipante.iActoNotarialId = 0;
                                    objParticipante.iActoJudicialId = 0;
                                    objParticipante.sTipoParticipanteId = 0;
                                    objParticipante.sTipoDatoId = 0;
                                    objParticipante.sTipoVinculoId = 0;
                                    objParticipante.dFechaLlegadaValija = DateTime.MinValue;
                                    objParticipante.sOficinaConsularDestinoId = 0;
                                    objParticipante.bolFirma = false;
                                    objParticipante.bolHuella = false;
                                    objParticipante.sTipoPersonaId = -1;
                                    objParticipante.iPersonaId = -1;
                                    objParticipante.iEmpresaId = -1;
                                    objParticipante.sTipoDocumentoId = -1;
                                    objParticipante.vTipoDocumento = string.Empty.ToString();
                                    objParticipante.vNumeroDocumento = string.Empty.ToString();
                                    objParticipante.sNacionalidadId = -1;
                                    objParticipante.vNombres = string.Empty.ToString();
                                    objParticipante.vPrimerApellido = string.Empty.ToString();
                                    objParticipante.vSegundoApellido = string.Empty.ToString();
                                    objParticipante.vDireccion = string.Empty.ToString();
                                    objParticipante.vUbigeo = string.Empty.ToString();
                                    objParticipante.ICentroPobladoId = -1;
                                    objParticipante.cEstado = "A";
                                    objParticipante.sUsuarioCreacion = -1;
                                    objParticipante.vIPCreacion = string.Empty.ToString();
                                    objParticipante.sUsuarioModificacion = -1;
                                    objParticipante.vIPModificacion = string.Empty.ToString();
                                    objParticipante.sOficinaConsularId = -1;
                                    objParticipante.vHostname = string.Empty.ToString();

                                    #endregion SET DEFAULT PARTICIPANTE ...

                                    objParticipante.iActuacionDetId = objMilitar.remi_iActuacionDetalleId;
                                    if (drParticipante["iActuacionParticipanteId"] != System.DBNull.Value) { objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]); }
                                    if (drParticipante["vTipoParticipante"] != System.DBNull.Value) { objParticipante.vTipoParticipante = drParticipante["vTipoParticipante"].ToString(); }
                                    if (drParticipante["sTipoParticipanteId"] != System.DBNull.Value) { objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]); }
                                    if (drParticipante["iPersonaId"] != System.DBNull.Value) { objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]); }
                                    if (drParticipante["vApellidoPaterno"] != System.DBNull.Value) { objParticipante.vPrimerApellido = drParticipante["vApellidoPaterno"].ToString(); }
                                    if (drParticipante["vApellidoMaterno"] != System.DBNull.Value) { objParticipante.vSegundoApellido = drParticipante["vApellidoMaterno"].ToString(); }
                                    if (drParticipante["vNombres"] != System.DBNull.Value) { objParticipante.vNombres = drParticipante["vNombres"].ToString(); }
                                    if (drParticipante["sDocumentoTipoId"] != System.DBNull.Value) { objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]); }
                                    if (drParticipante["vDocumentoTipo"] != System.DBNull.Value) { objParticipante.vTipoDocumento = drParticipante["vDocumentoTipo"].ToString(); }
                                    if (drParticipante["vDocumentoNumero"] != System.DBNull.Value) { objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString(); }
                                    if (drParticipante["pers_sNacionalidadId"] != System.DBNull.Value) { objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["pers_sNacionalidadId"]); }
                                    lstParticipante.Add(objParticipante);
                                }
                            }
                        }
                    }
                }

                #endregion Militar

                objActuacionMilitar.PERSONA = objPersona;
                objActuacionMilitar.RESIDENCIA = objResidencia;
                objActuacionMilitar.REGISTROMILITAR = objMilitar;
                objActuacionMilitar.PERSONA = objPersona;
                objActuacionMilitar.PARTICIPANTE_Container = lstParticipante;

                objActoMilitarConsultaBL = null;
                objPersonaConsultaBL = null;

                return objActuacionMilitar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.RE_ACTUACIONMILITAR ObtenerActuacionCivil(Int64 lngActuacionDetalleId, Int64 lngPersonaId)
        {
            try
            {
                BE.RE_ACTUACIONMILITAR objActuacionCivil = new RE_ACTUACIONMILITAR();

                BE.RE_REGISTROCIVIL objCivil = new RE_REGISTROCIVIL();
                BE.RE_PERSONA objPersona = new RE_PERSONA();
                BE.RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
                BE.RE_RESIDENCIA objResidencia = new RE_RESIDENCIA();
             
                BE.RE_PARTICIPANTE objTitular = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante = new List<RE_PARTICIPANTE>();

                BE.RE_PARTICIPANTE objTitular_Aux = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante_Aux = new List<RE_PARTICIPANTE>();

                PersonaConsultaBL objPersonaConsultaBL = new PersonaConsultaBL();
                DataTable dtPersona = objPersonaConsultaBL.PersonaGetById(lngPersonaId);
                if (dtPersona.Rows.Count > 0)
                {
                    objIdentificacion = new RE_PERSONAIDENTIFICACION();

                    #region SET DEFAULT TITULAR ...

                    objTitular.vNombres = string.Empty;
                    objTitular.vPrimerApellido = string.Empty;
                    objTitular.vSegundoApellido = string.Empty;
                    objTitular.sGeneroId = 0;
                    objTitular.sNacionalidadId = 0;
                    objTitular.pers_dNacimientoFecha = DateTime.MinValue;
                    objTitular.pers_cNacimientoLugar = string.Empty;

                    #endregion SET DEFAULT TITULAR ...

                    #region SET DEFAULT PERSONA ...

                    objPersona.pers_iPersonaId = -1;	//bigint	1
                    objPersona.pers_sEstadoCivilId = -1;	//smallint	2
                    objPersona.pers_sProfesionId = -1;	//smallint	47
                    objPersona.pers_sOcupacionId = -1;	//smallint	30
                    objPersona.pers_sIdiomaNatalId = -1;	//smallint	NULL
                    objPersona.pers_sGeneroId = -1;	//smallint	2001
                    objPersona.pers_sGradoInstruccionId = -1; //smallint	2152
                    objPersona.pers_vApellidoPaterno = string.Empty.ToString();	//varchar	CASTRO
                    objPersona.pers_vApellidoMaterno = string.Empty.ToString();	//varchar	ALVA
                    objPersona.pers_vNombres = string.Empty.ToString();	//varchar	FERNANDO
                    objPersona.pers_vCorreoElectronico = string.Empty.ToString();	//varchar	RRRRR@GMAIL.COM
                    objPersona.pers_dNacimientoFecha = DateTime.MinValue;	//datetime	NULL
                    objPersona.pers_sPersonaTipoId = -1;	//smallint	2101
                    objPersona.pers_cNacimientoLugar = string.Empty.ToString();	//char	220302
                    objPersona.pers_vObservaciones = string.Empty.ToString();	//varchar	ACTUALIZADO REGISTRO MILITAR
                    objPersona.pers_sNacionalidadId = -1;	//smallint	2051
                    objPersona.pers_vEstatura = string.Empty.ToString();	//varchar	NULL
                    objPersona.pers_sColorTezId = -1; //smallint	NULL
                    objPersona.pers_sColorOjosId = -1;	//smallint	NULL
                    objPersona.pers_sColorCabelloId = -1; //smallint	NULL
                    objPersona.pers_sGrupoSanguineoId = -1;	//smallint	NULL
                    objPersona.pers_vSenasParticulares = string.Empty.ToString();	//varchar	NULL
                    objPersona.pers_bFallecidoFlag = false;	//bit	NULL
                    objPersona.pers_dFechaDefuncion = DateTime.MinValue;	//datetime	NULL
                    objPersona.pers_cUbigeoDefuncion = string.Empty.ToString();	//char	NULL
                    objPersona.pers_vApellidoCasada = string.Empty.ToString();//varchar	NULL
                    objPersona.pers_cEstado = "A"; //char	A
                    //objPersona.pers_sUsuarioCreacion	//smallint	5
                    //objPersona.pers_vIPCreacion	//varchar	::1
                    //objPersona.pers_dFechaCreacion	//datetime	2015-02-06 23:32:35.900
                    //objPersona.pers_sUsuarioModificacion	//smallint	1
                    //objPersona.pers_vIPModificacion	//varchar	1.1.
                    //objPersona.pers_dFechaModificacion	//datetime	2015-02-06 23:35:59.510
                    objPersona.pers_sPeso = 0;	//smallint	NULL
                    objPersona.pers_sOcurrenciaTipoId = -1;	//smallint	NULL
                    objPersona.pers_IOcurrenciaCentroPobladoId = -1;	//int	NULL
                    objPersona.pers_vLugarNacimiento = string.Empty.ToString();	//varchar	NULL

                    #endregion SET DEFAULT PERSONA ...

                    #region SET DEFAULT IDENTIFICACION ...

                    objIdentificacion.peid_iPersonaIdentificacionId = -1; //bigint	90
                    objIdentificacion.peid_iPersonaId = -1; //bigint	91
                    objIdentificacion.peid_sDocumentoTipoId = -1; //smallint	1
                    objIdentificacion.peid_vDocumentoNumero = string.Empty.ToString(); //varchar	55555555
                    objIdentificacion.peid_dFecVcto = DateTime.MinValue; //datetime	NULL
                    objIdentificacion.peid_dFecExpedicion = DateTime.MinValue; //datetime	NULL
                    objIdentificacion.peid_vLugarExpedicion = string.Empty.ToString();//varchar	NULL
                    objIdentificacion.peid_dFecRenovacion = DateTime.MinValue; 	//datetime	NULL
                    objIdentificacion.peid_vLugarRenovacion = string.Empty.ToString();//varchar	NULL
                    objIdentificacion.peid_bActivoEnRune = false;	//bit	1
                    objIdentificacion.peid_cEstado = "A";	//char	A
                    objIdentificacion.peid_sUsuarioCreacion = -1; //smallint	2
                    objIdentificacion.peid_vIPCreacion = string.Empty.ToString(); //varchar	::1
                    objIdentificacion.peid_dFechaCreacion = DateTime.MinValue; //datetime	2015-02-08 21:47:13.580
                    objIdentificacion.peid_sUsuarioModificacion = -1; //smallint	NULL
                    objIdentificacion.peid_vIPModificacion = string.Empty.ToString(); //varchar	NULL
                    objIdentificacion.peid_dFechaModificacion = DateTime.MinValue; 	//datetime	NULL

                    #endregion SET DEFAULT IDENTIFICACION ...

                    #region SET DEFAULT RESIDENCIA

                    objResidencia.resi_iResidenciaId = -1; // bigint	1
                    objResidencia.resi_sResidenciaTipoId = -1; // smallint	2251
                    objResidencia.resi_vResidenciaDireccion = string.Empty.ToString(); // varchar	calle 15 mz q dpt c-1
                    objResidencia.resi_vCodigoPostal = string.Empty.ToString(); //varchar	51
                    objResidencia.resi_vResidenciaTelefono = string.Empty.ToString(); //varchar	5535532
                    objResidencia.resi_cResidenciaUbigeo = string.Empty.ToString(); //char	240106
                    objResidencia.resi_cEstado = "A"; //char	A
                    objResidencia.resi_sUsuarioCreacion = -1; // smallint	2
                    objResidencia.resi_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                    objResidencia.resi_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 09:05:30.850
                    objResidencia.resi_sUsuarioModificacion = -1; // smallint	NULL
                    objResidencia.resi_vIPModificacion = string.Empty.ToString(); //	varchar	NULL
                    objResidencia.resi_dFechaModificacion = DateTime.MinValue; // datetime	NULL
                    objResidencia.resi_ICentroPobladoId = -1; // int	NULL

                    #endregion SET DEFAULT RESIDENCIA

                    #region SET DEFAULT CIVIl ...

                    objCivil.reci_iRegistroCivilId = -1;
                    objCivil.reci_iActuacionDetalleId = -1; // bigint	127
                    objCivil.reci_sTipoActaId = -1;// smallint	7901
                    objCivil.reci_vNumeroCUI = string.Empty.ToString();
                    objCivil.reci_vNumeroActa = string.Empty.ToString();
                    objCivil.reci_vLibro = string.Empty.ToString();
                    objCivil.reci_dFechaRegistro = DateTime.MinValue;
                    objCivil.reci_cOficinaRegistralUbigeo = string.Empty.ToString();//	varchar	66666
                    objCivil.reci_IOficinaRegistralCentroPobladoId = -1;
                    objCivil.reci_dFechaHoraOcurrenciaActo = DateTime.MinValue;
                    objCivil.reci_sOcurrenciaTipoId = -1;
                    objCivil.reci_vOcurrenciaLugar = string.Empty.ToString();
                    objCivil.reci_cOcurrenciaUbigeo = string.Empty.ToString();
                    objCivil.reci_IOcurrenciaCentroPobladoId = -1;
                    objCivil.reci_vNumeroExpedienteMatrimonio = string.Empty.ToString();
                    objCivil.reci_IAprobacionUsuarioId = -1; // int	5
                    objCivil.reci_vIPAprobacion = string.Empty.ToString(); //varchar	::1
                    objCivil.reci_dFechaAprobacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.663
                    objCivil.reci_bDigitalizadoFlag = false; //	bit	1
                    objCivil.reci_vObservaciones = string.Empty.ToString(); //varchar
                    objCivil.reci_cEstado = "A"; //char	A
                    objCivil.reci_sUsuarioCreacion = -1; // smallint	5
                    objCivil.reci_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                    objCivil.reci_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.680
                    objCivil.reci_sUsuarioModificacion = -1; // smallint	NULL
                    objCivil.reci_vIPModificacion = string.Empty.ToString(); // varchar	NULL
                    objCivil.reci_dFechaModificacion = DateTime.MinValue; // datetime	NULL

                    #endregion SET DEFAULT CIVIl ...

                    DataRow drPersona = dtPersona.Rows[0];

                    if (drPersona["iPersonaId"] != System.DBNull.Value) { objPersona.pers_iPersonaId = Convert.ToInt64(drPersona["iPersonaId"]); }
                    if (drPersona["sPersonaTipoId"] != System.DBNull.Value) { objPersona.pers_sPersonaTipoId = Convert.ToInt16(drPersona["sPersonaTipoId"]); }
                    if (drPersona["sDocumentoTipoId"] != System.DBNull.Value) { objIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(drPersona["sDocumentoTipoId"]); }
                    if (drPersona["vNroDocumento"] != System.DBNull.Value) { objIdentificacion.peid_vDocumentoNumero = drPersona["vNroDocumento"].ToString(); }
                    if (drPersona["vApellidoPaterno"] != System.DBNull.Value) { objPersona.pers_vApellidoPaterno = drPersona["vApellidoPaterno"].ToString(); }
                    if (drPersona["vApellidoMaterno"] != System.DBNull.Value) { objPersona.pers_vApellidoMaterno = drPersona["vApellidoMaterno"].ToString(); }
                    if (drPersona["vNombres"] != System.DBNull.Value) { objPersona.pers_vNombres = drPersona["vNombres"].ToString(); }
                    if (drPersona["vCorreoElectronico"] != System.DBNull.Value) { objPersona.pers_vCorreoElectronico = drPersona["vCorreoElectronico"].ToString(); }
                    if (drPersona["dNacimientoFecha"].ToString() != string.Empty)
                        objPersona.pers_dNacimientoFecha = Convert.ToDateTime(drPersona["dNacimientoFecha"]);
                    else
                        objPersona.pers_dNacimientoFecha = DateTime.MinValue;
                    if (drPersona["cNacimientoLugar"] != System.DBNull.Value) { objPersona.pers_cNacimientoLugar = drPersona["cNacimientoLugar"].ToString(); }
                    if (drPersona["sGeneroId"] != System.DBNull.Value) { objPersona.pers_sGeneroId = Convert.ToInt16(drPersona["sGeneroId"]); }
                    if (drPersona["sNacionalidadId"] != System.DBNull.Value) { objPersona.pers_sNacionalidadId = Convert.ToInt16(drPersona["sNacionalidadId"]); }
                    if (drPersona["sOcupacionId"] != System.DBNull.Value) { objPersona.pers_sOcupacionId = Convert.ToInt16(drPersona["sOcupacionId"]); }
                    if (drPersona["sEstadoCivilId"] != System.DBNull.Value) { objPersona.pers_sEstadoCivilId = Convert.ToInt16(drPersona["sEstadoCivilId"]); }
                    if (drPersona["sGradoInstruccionId"] != System.DBNull.Value) { objPersona.pers_sGradoInstruccionId = Convert.ToInt16(drPersona["sGradoInstruccionId"]); }
                    if (drPersona["sProfesionId"] != System.DBNull.Value) { objPersona.pers_sProfesionId = Convert.ToInt16(drPersona["sProfesionId"]); }
                    if (drPersona["pers_sColorCabelloId"] != System.DBNull.Value) { objPersona.pers_sColorCabelloId = Convert.ToInt16(drPersona["pers_sColorCabelloId"]); }
                    if (drPersona["pers_sColorOjosId"] != System.DBNull.Value) { objPersona.pers_sColorOjosId = Convert.ToInt16(drPersona["pers_sColorOjosId"]); }
                    if (drPersona["pers_sColorTezId"] != System.DBNull.Value) { objPersona.pers_sColorTezId = Convert.ToInt16(drPersona["pers_sColorTezId"]); }
                    if (drPersona["pers_vSenasParticulares"] != System.DBNull.Value) { objPersona.pers_vSenasParticulares = drPersona["pers_vSenasParticulares"].ToString(); }
                    if (drPersona["pers_sGrupoSanguineoId"] != System.DBNull.Value) { objPersona.pers_sGrupoSanguineoId = Convert.ToInt16(drPersona["pers_sGrupoSanguineoId"]); }
                }

                PersonaResidenciaConsultaBL objPersonaResidenciaBL = new PersonaResidenciaConsultaBL();
                DataTable dtResidencia = objPersonaResidenciaBL.Obtener(lngPersonaId);
                if (dtResidencia.Rows.Count > 0)
                {
                    DataRow dr = dtResidencia.Rows[0];
                    if (dr["iResidenciaId"] != System.DBNull.Value) { objResidencia.resi_iResidenciaId = Convert.ToInt64(dr["iResidenciaId"]); }
                    if (dr["vCodigoPostal"] != System.DBNull.Value) { objResidencia.resi_vCodigoPostal = dr["vCodigoPostal"].ToString(); }
                    if (dr["vResidenciaDireccion"] != System.DBNull.Value) { objResidencia.resi_vResidenciaDireccion = dr["vResidenciaDireccion"].ToString(); }
                    if (dr["sResidenciaTipoId"] != System.DBNull.Value) { objResidencia.resi_sResidenciaTipoId = Convert.ToInt16(dr["sResidenciaTipoId"]); }
                    if (dr["cResidenciaUbigeo"] != System.DBNull.Value) { objResidencia.resi_cResidenciaUbigeo = dr["cResidenciaUbigeo"].ToString(); }
                    if (dr["vResidenciaTelefono"] != System.DBNull.Value) { objResidencia.resi_vResidenciaTelefono = dr["vResidenciaTelefono"].ToString(); }
                }

                #region Civil

                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();
                DataSet ds = objActoCivilConsultaBL.ObtenerDatosCivil(null, lngActuacionDetalleId);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtRegistroCivil = ds.Tables[0];
                    if (dtRegistroCivil.Rows.Count > 0)
                    {
                        DataRow drCivil = dtRegistroCivil.Rows[0];
                        if (drCivil["reci_iRegistroCivilId"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId = Convert.ToInt64(drCivil["reci_iRegistroCivilId"]); }
                        if (drCivil["reci_iActuacionDetalleId"] != System.DBNull.Value) { objCivil.reci_iActuacionDetalleId = Convert.ToInt64(drCivil["reci_iActuacionDetalleId"]); }
                        if (drCivil["reci_sTipoActaId"] != System.DBNull.Value) { objCivil.reci_sTipoActaId = Convert.ToInt16(drCivil["reci_sTipoActaId"]); }
                        if (drCivil["reci_vNumeroCUI"] != System.DBNull.Value) { objCivil.reci_vNumeroCUI = drCivil["reci_vNumeroCUI"].ToString(); }
                        if (drCivil["reci_vNumeroActa"] != System.DBNull.Value) { objCivil.reci_vNumeroActa = Convert.ToString(drCivil["reci_vNumeroActa"]); }
                        if (drCivil["reci_vLibro"] != System.DBNull.Value) { objCivil.reci_vLibro = drCivil["reci_vLibro"].ToString(); }
                        if (drCivil["reci_dFechaRegistro"] != System.DBNull.Value) { objCivil.reci_dFechaRegistro = Convert.ToDateTime(drCivil["reci_dFechaRegistro"]); }
                        if (drCivil["reci_cOficinaRegistralUbigeo"] != System.DBNull.Value) { objCivil.reci_cOficinaRegistralUbigeo = drCivil["reci_cOficinaRegistralUbigeo"].ToString(); }
                        if (drCivil["reci_iOficinaRegistralCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOficinaRegistralCentroPobladoId = Convert.ToInt32(drCivil["reci_iOficinaRegistralCentroPobladoId"]); }
                        if (drCivil["reci_dFechaHoraOcurrenciaActo"] != System.DBNull.Value) { objCivil.reci_dFechaHoraOcurrenciaActo = Convert.ToDateTime(drCivil["reci_dFechaHoraOcurrenciaActo"]); }
                        if (drCivil["reci_sOcurrenciaTipoId"] != System.DBNull.Value) { objCivil.reci_sOcurrenciaTipoId = Convert.ToInt16(drCivil["reci_sOcurrenciaTipoId"]); }
                        if (drCivil["reci_vOcurrenciaLugar"] != System.DBNull.Value) { objCivil.reci_vOcurrenciaLugar = drCivil["reci_vOcurrenciaLugar"].ToString(); }
                        if (drCivil["reci_cOcurrenciaUbigeo"] != System.DBNull.Value) { objCivil.reci_cOcurrenciaUbigeo = drCivil["reci_cOcurrenciaUbigeo"].ToString(); }
                        if (drCivil["reci_IOcurrenciaCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOcurrenciaCentroPobladoId = Convert.ToInt32(drCivil["reci_IOcurrenciaCentroPobladoId"]); }
                        if (drCivil["reci_vNumeroExpedienteMatrimonio"] != System.DBNull.Value) { objCivil.reci_vNumeroExpedienteMatrimonio = drCivil["reci_vNumeroExpedienteMatrimonio"].ToString(); }
                        if (drCivil["reci_IAprobacionUsuarioId"] != System.DBNull.Value) { objCivil.reci_IAprobacionUsuarioId = Convert.ToInt32(drCivil["reci_IAprobacionUsuarioId"]); }
                        if (drCivil["reci_vIPAprobacion"] != System.DBNull.Value) { objCivil.reci_vIPAprobacion = drCivil["reci_vIPAprobacion"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bDigitalizadoFlag"] != System.DBNull.Value) { objCivil.reci_bDigitalizadoFlag = Convert.ToBoolean(drCivil["reci_bDigitalizadoFlag"]); }
                        if (drCivil["reci_vCargoCelebrante"] != System.DBNull.Value) { objCivil.reci_vCargoCelebrante = drCivil["reci_vCargoCelebrante"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bAnotacionFlag"] != System.DBNull.Value) { objCivil.reci_bAnotacionFlag = Convert.ToBoolean(drCivil["reci_bAnotacionFlag"]); }
                        if (drCivil["reci_vObservaciones"] != System.DBNull.Value) { objCivil.reci_vObservaciones = drCivil["reci_vObservaciones"].ToString(); }


                      
                        

                        DataTable dtParticipantes = ds.Tables[1];
                        if (dtParticipantes != null)
                        {
                            if (dtParticipantes.Rows.Count > 0)
                            {
                                BE.RE_PARTICIPANTE objParticipante;

                                foreach (DataRow drParticipante in dtParticipantes.Rows)
                                {
                                    objParticipante = new RE_PARTICIPANTE();

                                    #region SET DEFAULT PARTICIPANTE ...

                                    objParticipante.sTipoActuacionId = -1;
                                    objParticipante.iParticipanteId = 0;
                                    objParticipante.iActuacionDetId = 0;
                                    objParticipante.iActoNotarialId = 0;
                                    objParticipante.iActoJudicialId = 0;
                                    objParticipante.sTipoParticipanteId = 0;
                                    objParticipante.sTipoDatoId = 0;
                                    objParticipante.sTipoVinculoId = 0;
                                    objParticipante.dFechaLlegadaValija = DateTime.MinValue;
                                    objParticipante.sOficinaConsularDestinoId = 0;
                                    objParticipante.bolFirma = false;
                                    objParticipante.bolHuella = false;
                                    objParticipante.sTipoPersonaId = -1;
                                    objParticipante.iPersonaId = -1;
                                    objParticipante.iEmpresaId = -1;
                                    objParticipante.sTipoDocumentoId = -1;
                                    objParticipante.vTipoDocumento = string.Empty.ToString();
                                    objParticipante.vNumeroDocumento = string.Empty.ToString();
                                    objParticipante.sNacionalidadId = -1;
                                    objParticipante.sGeneroId = -1;
                                    objParticipante.pers_dNacimientoFecha = DateTime.MinValue;
                                    objParticipante.pers_cNacimientoLugar = string.Empty;
                                    objParticipante.vNombres = string.Empty.ToString();
                                    objParticipante.vPrimerApellido = string.Empty.ToString();
                                    objParticipante.vSegundoApellido = string.Empty.ToString();
                                    objParticipante.vDireccion = string.Empty.ToString();
                                    objParticipante.vUbigeo = string.Empty.ToString();
                                    objParticipante.ICentroPobladoId = -1;
                                    objParticipante.cEstado = "A";
                                    objParticipante.sUsuarioCreacion = -1;
                                    objParticipante.vIPCreacion = string.Empty.ToString();
                                    objParticipante.sUsuarioModificacion = -1;
                                    objParticipante.vIPModificacion = string.Empty.ToString();
                                    objParticipante.sOficinaConsularId = -1;
                                    objParticipante.vHostname = string.Empty.ToString();
                                    objParticipante.bExisteRune = true;
                                    #endregion SET DEFAULT PARTICIPANTE ...

                                    objParticipante.iActuacionDetId = objCivil.reci_iActuacionDetalleId;
                                    if (drParticipante["iActuacionParticipanteId"] != System.DBNull.Value) { objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]); }
                                    if (drParticipante["sTipoParticipanteId"] != System.DBNull.Value) { objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]); }
                                    if (drParticipante["iPersonaId"] != System.DBNull.Value) { objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]); }
                                    if (drParticipante["vApellidoPaterno"] != System.DBNull.Value) { objParticipante.vPrimerApellido = drParticipante["vApellidoPaterno"].ToString(); }
                                    if (drParticipante["vApellidoMaterno"] != System.DBNull.Value) { objParticipante.vSegundoApellido = drParticipante["vApellidoMaterno"].ToString(); }
                                    if (drParticipante["vNombres"] != System.DBNull.Value) { objParticipante.vNombres = drParticipante["vNombres"].ToString(); }
                                    if (drParticipante["sDocumentoTipoId"] != System.DBNull.Value) { objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]); }
                                    if (drParticipante["vDocumentoTipo"] != System.DBNull.Value) { objParticipante.vTipoDocumento = drParticipante["vDocumentoTipo"].ToString(); }
                                    if (drParticipante["vDocumentoNumero"] != System.DBNull.Value) { objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString(); }
                                    if (drParticipante["vTipoParticipante"] != System.DBNull.Value) { objParticipante.vTipoParticipante = drParticipante["vTipoParticipante"].ToString(); }

                                    //if (drParticipante["vDocumentoCompleto"] != System.DBNull.Value) { objParticipante.vDocumentoCompleto = Convert.ToString(drParticipante["vDocumentoCompleto"]); };
                                    if (drParticipante["sTipoDatoId"] != System.DBNull.Value) { objParticipante.sTipoDatoId = Convert.ToInt16(drParticipante["sTipoDatoId"]); }
                                    if (drParticipante["sTipoVinculoId"] != System.DBNull.Value) { objParticipante.sTipoVinculoId = Convert.ToInt16(drParticipante["sTipoVinculoId"]); }
                                    //if (drParticipante["vNombreCompleto"] != System.DBNull.Value) { objParticipante.vNombreCompleto = Convert.ToString(drParticipante["vNombreCompleto"]); }
                                    if (drParticipante["pers_sNacionalidadId"] != System.DBNull.Value) { objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["pers_sNacionalidadId"]); }
                                    if (drParticipante["pers_sGeneroId"] != System.DBNull.Value) { objParticipante.sGeneroId = Convert.ToInt16(drParticipante["pers_sGeneroId"]); }

                                    if (drParticipante["pers_cNacimientoLugar"] != System.DBNull.Value) { objParticipante.pers_cNacimientoLugar = drParticipante["pers_cNacimientoLugar"].ToString(); objParticipante.bExisteUbicacion = true; }
                                    if (drParticipante["pers_dNacimientoFecha"].ToString() != string.Empty)
                                        objParticipante.pers_dNacimientoFecha = Convert.ToDateTime(drParticipante["pers_dNacimientoFecha"]);
                                    else
                                        objParticipante.pers_dNacimientoFecha = DateTime.MinValue;

                                    if (drParticipante["pers_bFallecidoFlag"] != System.DBNull.Value) { objParticipante.pers_bFallecidoFlag = Convert.ToBoolean(drParticipante["pers_bFallecidoFlag"]); }
                                    if (drParticipante["pers_dFechaDefuncion"] != System.DBNull.Value) { objParticipante.pers_dFechaDefuncion = Convert.ToDateTime(drParticipante["pers_dFechaDefuncion"]); }
                                    if (drParticipante["pers_cUbigeoDefuncion"] != System.DBNull.Value) { objParticipante.pers_cUbigeoDefuncion = drParticipante["pers_cUbigeoDefuncion"].ToString(); }

                                    if (drParticipante["resi_cResidenciaUbigeo"] != System.DBNull.Value) { objParticipante.vUbigeo = drParticipante["resi_cResidenciaUbigeo"].ToString(); objParticipante.bExisteUbicacion = true; }

                                    if (drParticipante["resi_vResidenciaDireccion"] != System.DBNull.Value) { objParticipante.vDireccion = drParticipante["resi_vResidenciaDireccion"].ToString(); }

                                    if (drParticipante["pers_sEstadoCivilId"] != System.DBNull.Value) { objParticipante.pers_sEstadoCivilId = Convert.ToInt32(drParticipante["pers_sEstadoCivilId"]); }

                                    
                                    lstParticipante.Add(objParticipante);

                                    #region Titular

                                    if (objCivil.reci_sTipoActaId > 0)
                                    {
                                        if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.NACIMIENTO))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                objTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                        else if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.DEFUNCION))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteDefuncion.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_bFallecidoFlag = objParticipante.pers_bFallecidoFlag;
                                                objTitular.pers_dFechaDefuncion = objParticipante.pers_dFechaDefuncion;
                                                objTitular.pers_cUbigeoDefuncion = objParticipante.pers_cUbigeoDefuncion;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                //bjTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                    }

                                    #endregion Titular
                                }
                            }
                        }
                    }

                    DataTable dtRegistroCivil_iReferencia = ds.Tables[2];
                    if (dtRegistroCivil_iReferencia.Rows.Count > 0)
                    {
                        DataRow drCivil_iReferenciaID = dtRegistroCivil_iReferencia.Rows[0];
                        if (drCivil_iReferenciaID["acde_iReferenciaId"] != System.DBNull.Value) { objCivil.acde_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["acde_iReferenciaId"].ToString()); }
                        if (drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"].ToString()); }
                          
                         
                    }

                    

                }

                #endregion Civil

                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.RESIDENCIA = objResidencia;
                objActuacionCivil.REGISTROCIVIL = objCivil;
                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.TITULAR = objTitular;
                objActuacionCivil.PARTICIPANTE_Container = lstParticipante;

            

                objActoCivilConsultaBL = null;
                objPersonaConsultaBL = null;

                return objActuacionCivil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerParticipantes(Int64 lngActuacionDetalleId)
        {
            try
            {
                ActuacionConsultaDA obj = new ActuacionConsultaDA();
                DataTable dt = obj.ObtenerParticipantes(lngActuacionDetalleId);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //CREADO POR JONATAN, SOLO OBTENGO DATOS RELACIONADOS A REGISTRO CIVIL 11/01/2018
        public BE.RE_ACTUACIONMILITAR ObtenerDatosActoCivil(Int64 lngActuacionDetalleId, Int64 lngPersonaId, out DataTable dtParticipantesResultado)
        {
            try
            {
                BE.RE_ACTUACIONMILITAR objActuacionCivil = new RE_ACTUACIONMILITAR();

                BE.RE_REGISTROCIVIL objCivil = new RE_REGISTROCIVIL();
                BE.RE_PERSONA objPersona = new RE_PERSONA();
                BE.RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
                BE.RE_RESIDENCIA objResidencia = new RE_RESIDENCIA();

                BE.RE_PARTICIPANTE objTitular = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante = new List<RE_PARTICIPANTE>();

                BE.RE_PARTICIPANTE objTitular_Aux = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante_Aux = new List<RE_PARTICIPANTE>();

                DataTable dtParticipantes = new DataTable();
                #region SET DEFAULT TITULAR ...

                objTitular.vNombres = string.Empty;
                objTitular.vPrimerApellido = string.Empty;
                objTitular.vSegundoApellido = string.Empty;
                objTitular.sGeneroId = 0;
                objTitular.sNacionalidadId = 0;
                objTitular.pers_dNacimientoFecha = DateTime.MinValue;
                objTitular.pers_cNacimientoLugar = string.Empty;

                #endregion SET DEFAULT TITULAR ...

                #region Civil
                #region SET DEFAULT CIVIl ...

                objCivil.reci_iRegistroCivilId = -1;
                objCivil.reci_iActuacionDetalleId = -1; // bigint	127
                objCivil.reci_sTipoActaId = -1;// smallint	7901
                objCivil.reci_vNumeroCUI = string.Empty.ToString();
                objCivil.reci_vNumeroActa = string.Empty.ToString();
                objCivil.reci_vLibro = string.Empty.ToString();
                objCivil.reci_dFechaRegistro = DateTime.MinValue;
                objCivil.reci_cOficinaRegistralUbigeo = string.Empty.ToString();//	varchar	66666
                objCivil.reci_IOficinaRegistralCentroPobladoId = -1;
                objCivil.reci_dFechaHoraOcurrenciaActo = DateTime.MinValue;
                objCivil.reci_sOcurrenciaTipoId = -1;
                objCivil.reci_vOcurrenciaLugar = string.Empty.ToString();
                objCivil.reci_cOcurrenciaUbigeo = string.Empty.ToString();
                objCivil.reci_IOcurrenciaCentroPobladoId = -1;
                objCivil.reci_vNumeroExpedienteMatrimonio = string.Empty.ToString();
                objCivil.reci_IAprobacionUsuarioId = -1; // int	5
                objCivil.reci_vIPAprobacion = string.Empty.ToString(); //varchar	::1
                objCivil.reci_dFechaAprobacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.663
                objCivil.reci_bDigitalizadoFlag = false; //	bit	1
                objCivil.reci_vObservaciones = string.Empty.ToString(); //varchar
                objCivil.reci_cEstado = "A"; //char	A
                objCivil.reci_sUsuarioCreacion = -1; // smallint	5
                objCivil.reci_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                objCivil.reci_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.680
                objCivil.reci_sUsuarioModificacion = -1; // smallint	NULL
                objCivil.reci_vIPModificacion = string.Empty.ToString(); // varchar	NULL
                objCivil.reci_dFechaModificacion = DateTime.MinValue; // datetime	NULL
                objCivil.reci_cConCUI = "S";
                objCivil.reci_cReconocimientoAdopcion = "N";
                objCivil.reci_cReconstitucionReposicion = "N";
                objCivil.reci_iNumeroActaAnterior = null;
                objCivil.reci_vTitular = "";
                objCivil.reci_bInscripcionOficio = false;

                #endregion SET DEFAULT CIVIl ...
                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();
                DataSet ds = objActoCivilConsultaBL.ObtenerDatosCivil(null, lngActuacionDetalleId);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtRegistroCivil = ds.Tables[0];
                    if (dtRegistroCivil.Rows.Count > 0)
                    {
                        DataRow drCivil = dtRegistroCivil.Rows[0];
                        if (drCivil["reci_iRegistroCivilId"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId = Convert.ToInt64(drCivil["reci_iRegistroCivilId"]); }
                        if (drCivil["reci_iActuacionDetalleId"] != System.DBNull.Value) { objCivil.reci_iActuacionDetalleId = Convert.ToInt64(drCivil["reci_iActuacionDetalleId"]); }
                        if (drCivil["reci_sTipoActaId"] != System.DBNull.Value) { objCivil.reci_sTipoActaId = Convert.ToInt16(drCivil["reci_sTipoActaId"]); }
                        if (drCivil["reci_vNumeroCUI"] != System.DBNull.Value) { objCivil.reci_vNumeroCUI = drCivil["reci_vNumeroCUI"].ToString(); }
                        if (drCivil["reci_vNumeroActa"] != System.DBNull.Value) { objCivil.reci_vNumeroActa = Convert.ToString(drCivil["reci_vNumeroActa"]); }
                        if (drCivil["reci_vLibro"] != System.DBNull.Value) { objCivil.reci_vLibro = drCivil["reci_vLibro"].ToString(); }
                        if (drCivil["reci_dFechaRegistro"] != System.DBNull.Value) { objCivil.reci_dFechaRegistro = Convert.ToDateTime(drCivil["reci_dFechaRegistro"]); }
                        if (drCivil["reci_cOficinaRegistralUbigeo"] != System.DBNull.Value) { objCivil.reci_cOficinaRegistralUbigeo = drCivil["reci_cOficinaRegistralUbigeo"].ToString(); }
                        if (drCivil["reci_iOficinaRegistralCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOficinaRegistralCentroPobladoId = Convert.ToInt32(drCivil["reci_iOficinaRegistralCentroPobladoId"]); }
                        if (drCivil["reci_dFechaHoraOcurrenciaActo"] != System.DBNull.Value) { objCivil.reci_dFechaHoraOcurrenciaActo = Convert.ToDateTime(drCivil["reci_dFechaHoraOcurrenciaActo"]); }
                        if (drCivil["reci_sOcurrenciaTipoId"] != System.DBNull.Value) { objCivil.reci_sOcurrenciaTipoId = Convert.ToInt16(drCivil["reci_sOcurrenciaTipoId"]); }
                        if (drCivil["reci_vOcurrenciaLugar"] != System.DBNull.Value) { objCivil.reci_vOcurrenciaLugar = drCivil["reci_vOcurrenciaLugar"].ToString(); }
                        if (drCivil["reci_cOcurrenciaUbigeo"] != System.DBNull.Value) { objCivil.reci_cOcurrenciaUbigeo = drCivil["reci_cOcurrenciaUbigeo"].ToString(); }
                        if (drCivil["reci_IOcurrenciaCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOcurrenciaCentroPobladoId = Convert.ToInt32(drCivil["reci_IOcurrenciaCentroPobladoId"]); }
                        if (drCivil["reci_vNumeroExpedienteMatrimonio"] != System.DBNull.Value) { objCivil.reci_vNumeroExpedienteMatrimonio = drCivil["reci_vNumeroExpedienteMatrimonio"].ToString(); }
                        if (drCivil["reci_IAprobacionUsuarioId"] != System.DBNull.Value) { objCivil.reci_IAprobacionUsuarioId = Convert.ToInt32(drCivil["reci_IAprobacionUsuarioId"]); }
                        if (drCivil["reci_vIPAprobacion"] != System.DBNull.Value) { objCivil.reci_vIPAprobacion = drCivil["reci_vIPAprobacion"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bDigitalizadoFlag"] != System.DBNull.Value) { objCivil.reci_bDigitalizadoFlag = Convert.ToBoolean(drCivil["reci_bDigitalizadoFlag"]); }
                        if (drCivil["reci_vCargoCelebrante"] != System.DBNull.Value) { objCivil.reci_vCargoCelebrante = drCivil["reci_vCargoCelebrante"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bAnotacionFlag"] != System.DBNull.Value) { objCivil.reci_bAnotacionFlag = Convert.ToBoolean(drCivil["reci_bAnotacionFlag"]); }
                        if (drCivil["reci_vObservaciones"] != System.DBNull.Value) { objCivil.reci_vObservaciones = drCivil["reci_vObservaciones"].ToString(); }
                        if (drCivil["reci_cConCUI"] != System.DBNull.Value) { objCivil.reci_cConCUI = drCivil["reci_cConCUI"].ToString(); }
                        if (drCivil["reci_cReconocimientoAdopcion"] != System.DBNull.Value) { objCivil.reci_cReconocimientoAdopcion = drCivil["reci_cReconocimientoAdopcion"].ToString(); }
                        if (drCivil["reci_cReconstitucionReposicion"] != System.DBNull.Value) { objCivil.reci_cReconstitucionReposicion = drCivil["reci_cReconstitucionReposicion"].ToString(); }
                        if (drCivil["reci_iNumeroActaAnterior"] != System.DBNull.Value) { objCivil.reci_iNumeroActaAnterior = Convert.ToInt32(drCivil["reci_iNumeroActaAnterior"].ToString()); }
                        if (drCivil["reci_vTitular"] != System.DBNull.Value) { objCivil.reci_vTitular = drCivil["reci_vTitular"].ToString(); }
                        if (drCivil["reci_bInscripcionOficio"] != System.DBNull.Value) { objCivil.reci_bInscripcionOficio = Convert.ToBoolean(drCivil["reci_bInscripcionOficio"]); }



                        dtParticipantes = ds.Tables[1];
                        
                        if (dtParticipantes != null)
                        {
                            if (dtParticipantes.Rows.Count > 0)
                            {
                                
                                BE.RE_PARTICIPANTE objParticipante;

                                foreach (DataRow drParticipante in dtParticipantes.Rows)
                                {
                                    objParticipante = new RE_PARTICIPANTE();

                                    #region SET DEFAULT PARTICIPANTE ...

                                    objParticipante.sTipoActuacionId = -1;
                                    objParticipante.iParticipanteId = 0;
                                    objParticipante.iActuacionDetId = 0;
                                    objParticipante.iActoNotarialId = 0;
                                    objParticipante.iActoJudicialId = 0;
                                    objParticipante.sTipoParticipanteId = 0;
                                    objParticipante.sTipoDatoId = 0;
                                    objParticipante.sTipoVinculoId = 0;
                                    objParticipante.dFechaLlegadaValija = DateTime.MinValue;
                                    objParticipante.sOficinaConsularDestinoId = 0;
                                    objParticipante.bolFirma = false;
                                    objParticipante.bolHuella = false;
                                    objParticipante.sTipoPersonaId = -1;
                                    objParticipante.iPersonaId = -1;
                                    objParticipante.iEmpresaId = -1;
                                    objParticipante.sTipoDocumentoId = -1;
                                    objParticipante.vTipoDocumento = string.Empty.ToString();
                                    objParticipante.vNumeroDocumento = string.Empty.ToString();
                                    objParticipante.sNacionalidadId = -1;
                                    objParticipante.sGeneroId = -1;
                                    objParticipante.pers_dNacimientoFecha = DateTime.MinValue;
                                    objParticipante.pers_cNacimientoLugar = string.Empty;
                                    objParticipante.vNombres = string.Empty.ToString();
                                    objParticipante.vPrimerApellido = string.Empty.ToString();
                                    objParticipante.vSegundoApellido = string.Empty.ToString();
                                    objParticipante.vDireccion = string.Empty.ToString();
                                    objParticipante.vUbigeo = string.Empty.ToString();
                                    objParticipante.ICentroPobladoId = -1;
                                    objParticipante.cEstado = "A";
                                    objParticipante.sUsuarioCreacion = -1;
                                    objParticipante.vIPCreacion = string.Empty.ToString();
                                    objParticipante.sUsuarioModificacion = -1;
                                    objParticipante.vIPModificacion = string.Empty.ToString();
                                    objParticipante.sOficinaConsularId = -1;
                                    objParticipante.vHostname = string.Empty.ToString();
                                    objParticipante.bExisteRune = true;
                                    #endregion SET DEFAULT PARTICIPANTE ...

                                    objParticipante.iActuacionDetId = objCivil.reci_iActuacionDetalleId;
                                    if (drParticipante["iActuacionParticipanteId"] != System.DBNull.Value) { objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]); }
                                    if (drParticipante["sTipoParticipanteId"] != System.DBNull.Value) { objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]); }
                                    if (drParticipante["iPersonaId"] != System.DBNull.Value) { objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]); }
                                    if (drParticipante["vApellidoPaterno"] != System.DBNull.Value) { objParticipante.vPrimerApellido = drParticipante["vApellidoPaterno"].ToString(); }
                                    if (drParticipante["vApellidoMaterno"] != System.DBNull.Value) { objParticipante.vSegundoApellido = drParticipante["vApellidoMaterno"].ToString(); }
                                    if (drParticipante["vNombres"] != System.DBNull.Value) { objParticipante.vNombres = drParticipante["vNombres"].ToString(); }
                                    if (drParticipante["sDocumentoTipoId"] != System.DBNull.Value) { objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]); }
                                    if (drParticipante["vDocumentoTipo"] != System.DBNull.Value) { objParticipante.vTipoDocumento = drParticipante["vDocumentoTipo"].ToString(); }
                                    if (drParticipante["vDocumentoNumero"] != System.DBNull.Value) { objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString(); }
                                    if (drParticipante["vTipoParticipante"] != System.DBNull.Value) { objParticipante.vTipoParticipante = drParticipante["vTipoParticipante"].ToString(); }

                                    //if (drParticipante["vDocumentoCompleto"] != System.DBNull.Value) { objParticipante.vDocumentoCompleto = Convert.ToString(drParticipante["vDocumentoCompleto"]); };
                                    if (drParticipante["sTipoDatoId"] != System.DBNull.Value) { objParticipante.sTipoDatoId = Convert.ToInt16(drParticipante["sTipoDatoId"]); }
                                    if (drParticipante["sTipoVinculoId"] != System.DBNull.Value) { objParticipante.sTipoVinculoId = Convert.ToInt16(drParticipante["sTipoVinculoId"]); }
                                    //if (drParticipante["vNombreCompleto"] != System.DBNull.Value) { objParticipante.vNombreCompleto = Convert.ToString(drParticipante["vNombreCompleto"]); }
                                    if (drParticipante["pers_sNacionalidadId"] != System.DBNull.Value) { objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["pers_sNacionalidadId"]); }
                                    if (drParticipante["pers_sGeneroId"] != System.DBNull.Value) { objParticipante.sGeneroId = Convert.ToInt16(drParticipante["pers_sGeneroId"]); }

                                    if (drParticipante["pers_cNacimientoLugar"] != System.DBNull.Value) { objParticipante.pers_cNacimientoLugar = drParticipante["pers_cNacimientoLugar"].ToString(); objParticipante.bExisteUbicacion = true; }
                                    if (drParticipante["pers_dNacimientoFecha"].ToString() != string.Empty)
                                        objParticipante.pers_dNacimientoFecha = Convert.ToDateTime(drParticipante["pers_dNacimientoFecha"]);
                                    else
                                        objParticipante.pers_dNacimientoFecha = DateTime.MinValue;

                                    if (drParticipante["pers_bFallecidoFlag"] != System.DBNull.Value) { objParticipante.pers_bFallecidoFlag = Convert.ToBoolean(drParticipante["pers_bFallecidoFlag"]); }
                                    if (drParticipante["pers_dFechaDefuncion"] != System.DBNull.Value) { objParticipante.pers_dFechaDefuncion = Convert.ToDateTime(drParticipante["pers_dFechaDefuncion"]); }
                                    if (drParticipante["pers_cUbigeoDefuncion"] != System.DBNull.Value) { objParticipante.pers_cUbigeoDefuncion = drParticipante["pers_cUbigeoDefuncion"].ToString(); }

                                    if (drParticipante["resi_cResidenciaUbigeo"] != System.DBNull.Value) { objParticipante.vUbigeo = drParticipante["resi_cResidenciaUbigeo"].ToString(); objParticipante.bExisteUbicacion = true; }

                                    if (drParticipante["resi_vResidenciaDireccion"] != System.DBNull.Value) { objParticipante.vDireccion = drParticipante["resi_vResidenciaDireccion"].ToString(); }

                                    if (drParticipante["pers_sEstadoCivilId"] != System.DBNull.Value) { objParticipante.pers_sEstadoCivilId = Convert.ToInt32(drParticipante["pers_sEstadoCivilId"]); }


                                    lstParticipante.Add(objParticipante);

                                    #region Titular

                                    if (objCivil.reci_sTipoActaId > 0)
                                    {
                                        if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.NACIMIENTO))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                objTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                        else if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.DEFUNCION))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteDefuncion.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_bFallecidoFlag = objParticipante.pers_bFallecidoFlag;
                                                objTitular.pers_dFechaDefuncion = objParticipante.pers_dFechaDefuncion;
                                                objTitular.pers_cUbigeoDefuncion = objParticipante.pers_cUbigeoDefuncion;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                //bjTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                    }

                                    #endregion Titular
                                }
                            }
                        }
                    }

                    DataTable dtRegistroCivil_iReferencia = ds.Tables[2];
                    if (dtRegistroCivil_iReferencia.Rows.Count > 0)
                    {
                        DataRow drCivil_iReferenciaID = dtRegistroCivil_iReferencia.Rows[0];
                        if (drCivil_iReferenciaID["acde_iReferenciaId"] != System.DBNull.Value) { objCivil.acde_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["acde_iReferenciaId"].ToString()); }
                        if (drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"].ToString()); }
                    }
                }

                #endregion Civil

                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.RESIDENCIA = objResidencia;
                objActuacionCivil.REGISTROCIVIL = objCivil;
                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.TITULAR = objTitular;
                objActuacionCivil.PARTICIPANTE_Container = lstParticipante;

                objActoCivilConsultaBL = null;
                dtParticipantesResultado = dtParticipantes;
                return objActuacionCivil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BE.RE_ACTUACIONMILITAR ObtenerDatosActoCivil(Int64 lngActuacionDetalleId, Int64 lngPersonaId)
        {
            try
            {
                BE.RE_ACTUACIONMILITAR objActuacionCivil = new RE_ACTUACIONMILITAR();

                BE.RE_REGISTROCIVIL objCivil = new RE_REGISTROCIVIL();
                BE.RE_PERSONA objPersona = new RE_PERSONA();
                BE.RE_PERSONAIDENTIFICACION objIdentificacion = new RE_PERSONAIDENTIFICACION();
                BE.RE_RESIDENCIA objResidencia = new RE_RESIDENCIA();

                BE.RE_PARTICIPANTE objTitular = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante = new List<RE_PARTICIPANTE>();

                BE.RE_PARTICIPANTE objTitular_Aux = new RE_PARTICIPANTE();
                List<BE.RE_PARTICIPANTE> lstParticipante_Aux = new List<RE_PARTICIPANTE>();


                #region SET DEFAULT TITULAR ...

                objTitular.vNombres = string.Empty;
                objTitular.vPrimerApellido = string.Empty;
                objTitular.vSegundoApellido = string.Empty;
                objTitular.sGeneroId = 0;
                objTitular.sNacionalidadId = 0;
                objTitular.pers_dNacimientoFecha = DateTime.MinValue;
                objTitular.pers_cNacimientoLugar = string.Empty;

                #endregion SET DEFAULT TITULAR ...

                #region Civil
                #region SET DEFAULT CIVIl ...

                objCivil.reci_iRegistroCivilId = -1;
                objCivil.reci_iActuacionDetalleId = -1; // bigint	127
                objCivil.reci_sTipoActaId = -1;// smallint	7901
                objCivil.reci_vNumeroCUI = string.Empty.ToString();
                objCivil.reci_vNumeroActa = string.Empty.ToString();
                objCivil.reci_vLibro = string.Empty.ToString();
                objCivil.reci_dFechaRegistro = DateTime.MinValue;
                objCivil.reci_cOficinaRegistralUbigeo = string.Empty.ToString();//	varchar	66666
                objCivil.reci_IOficinaRegistralCentroPobladoId = -1;
                objCivil.reci_dFechaHoraOcurrenciaActo = DateTime.MinValue;
                objCivil.reci_sOcurrenciaTipoId = -1;
                objCivil.reci_vOcurrenciaLugar = string.Empty.ToString();
                objCivil.reci_cOcurrenciaUbigeo = string.Empty.ToString();
                objCivil.reci_IOcurrenciaCentroPobladoId = -1;
                objCivil.reci_vNumeroExpedienteMatrimonio = string.Empty.ToString();
                objCivil.reci_IAprobacionUsuarioId = -1; // int	5
                objCivil.reci_vIPAprobacion = string.Empty.ToString(); //varchar	::1
                objCivil.reci_dFechaAprobacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.663
                objCivil.reci_bDigitalizadoFlag = false; //	bit	1
                objCivil.reci_vObservaciones = string.Empty.ToString(); //varchar
                objCivil.reci_cEstado = "A"; //char	A
                objCivil.reci_sUsuarioCreacion = -1; // smallint	5
                objCivil.reci_vIPCreacion = string.Empty.ToString(); //	varchar	::1
                objCivil.reci_dFechaCreacion = DateTime.MinValue; // datetime	2015-02-07 18:42:41.680
                objCivil.reci_sUsuarioModificacion = -1; // smallint	NULL
                objCivil.reci_vIPModificacion = string.Empty.ToString(); // varchar	NULL
                objCivil.reci_dFechaModificacion = DateTime.MinValue; // datetime	NULL
                objCivil.reci_cConCUI = "S";
                objCivil.reci_cReconocimientoAdopcion = "N";
                objCivil.reci_cReconstitucionReposicion = "N";
                objCivil.reci_iNumeroActaAnterior = null;
                objCivil.reci_vTitular = "";

                #endregion SET DEFAULT CIVIl ...
                ActoCivilConsultaBL objActoCivilConsultaBL = new ActoCivilConsultaBL();
                DataSet ds = objActoCivilConsultaBL.ObtenerDatosCivil(null, lngActuacionDetalleId);
                if (ds.Tables.Count > 0)
                {
                    DataTable dtRegistroCivil = ds.Tables[0];
                    if (dtRegistroCivil.Rows.Count > 0)
                    {
                        DataRow drCivil = dtRegistroCivil.Rows[0];
                        if (drCivil["reci_iRegistroCivilId"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId = Convert.ToInt64(drCivil["reci_iRegistroCivilId"]); }
                        if (drCivil["reci_iActuacionDetalleId"] != System.DBNull.Value) { objCivil.reci_iActuacionDetalleId = Convert.ToInt64(drCivil["reci_iActuacionDetalleId"]); }
                        if (drCivil["reci_sTipoActaId"] != System.DBNull.Value) { objCivil.reci_sTipoActaId = Convert.ToInt16(drCivil["reci_sTipoActaId"]); }
                        if (drCivil["reci_vNumeroCUI"] != System.DBNull.Value) { objCivil.reci_vNumeroCUI = drCivil["reci_vNumeroCUI"].ToString(); }
                        if (drCivil["reci_vNumeroActa"] != System.DBNull.Value) { objCivil.reci_vNumeroActa = Convert.ToString(drCivil["reci_vNumeroActa"]); }
                        if (drCivil["reci_vLibro"] != System.DBNull.Value) { objCivil.reci_vLibro = drCivil["reci_vLibro"].ToString(); }
                        if (drCivil["reci_dFechaRegistro"] != System.DBNull.Value) { objCivil.reci_dFechaRegistro = Convert.ToDateTime(drCivil["reci_dFechaRegistro"]); }
                        if (drCivil["reci_cOficinaRegistralUbigeo"] != System.DBNull.Value) { objCivil.reci_cOficinaRegistralUbigeo = drCivil["reci_cOficinaRegistralUbigeo"].ToString(); }
                        if (drCivil["reci_iOficinaRegistralCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOficinaRegistralCentroPobladoId = Convert.ToInt32(drCivil["reci_iOficinaRegistralCentroPobladoId"]); }
                        if (drCivil["reci_dFechaHoraOcurrenciaActo"] != System.DBNull.Value) { objCivil.reci_dFechaHoraOcurrenciaActo = Convert.ToDateTime(drCivil["reci_dFechaHoraOcurrenciaActo"]); }
                        if (drCivil["reci_sOcurrenciaTipoId"] != System.DBNull.Value) { objCivil.reci_sOcurrenciaTipoId = Convert.ToInt16(drCivil["reci_sOcurrenciaTipoId"]); }
                        if (drCivil["reci_vOcurrenciaLugar"] != System.DBNull.Value) { objCivil.reci_vOcurrenciaLugar = drCivil["reci_vOcurrenciaLugar"].ToString(); }
                        if (drCivil["reci_cOcurrenciaUbigeo"] != System.DBNull.Value) { objCivil.reci_cOcurrenciaUbigeo = drCivil["reci_cOcurrenciaUbigeo"].ToString(); }
                        if (drCivil["reci_IOcurrenciaCentroPobladoId"] != System.DBNull.Value) { objCivil.reci_IOcurrenciaCentroPobladoId = Convert.ToInt32(drCivil["reci_IOcurrenciaCentroPobladoId"]); }
                        if (drCivil["reci_vNumeroExpedienteMatrimonio"] != System.DBNull.Value) { objCivil.reci_vNumeroExpedienteMatrimonio = drCivil["reci_vNumeroExpedienteMatrimonio"].ToString(); }
                        if (drCivil["reci_IAprobacionUsuarioId"] != System.DBNull.Value) { objCivil.reci_IAprobacionUsuarioId = Convert.ToInt32(drCivil["reci_IAprobacionUsuarioId"]); }
                        if (drCivil["reci_vIPAprobacion"] != System.DBNull.Value) { objCivil.reci_vIPAprobacion = drCivil["reci_vIPAprobacion"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bDigitalizadoFlag"] != System.DBNull.Value) { objCivil.reci_bDigitalizadoFlag = Convert.ToBoolean(drCivil["reci_bDigitalizadoFlag"]); }
                        if (drCivil["reci_vCargoCelebrante"] != System.DBNull.Value) { objCivil.reci_vCargoCelebrante = drCivil["reci_vCargoCelebrante"].ToString(); }
                        if (drCivil["reci_dFechaAprobacion"] != System.DBNull.Value) { objCivil.reci_dFechaAprobacion = Convert.ToDateTime(drCivil["reci_dFechaAprobacion"]); }
                        if (drCivil["reci_bAnotacionFlag"] != System.DBNull.Value) { objCivil.reci_bAnotacionFlag = Convert.ToBoolean(drCivil["reci_bAnotacionFlag"]); }
                        if (drCivil["reci_vObservaciones"] != System.DBNull.Value) { objCivil.reci_vObservaciones = drCivil["reci_vObservaciones"].ToString(); }
                        if (drCivil["reci_cConCUI"] != System.DBNull.Value) { objCivil.reci_cConCUI = drCivil["reci_cConCUI"].ToString(); }
                        if (drCivil["reci_cReconocimientoAdopcion"] != System.DBNull.Value) { objCivil.reci_cReconocimientoAdopcion = drCivil["reci_cReconocimientoAdopcion"].ToString(); }
                        if (drCivil["reci_cReconstitucionReposicion"] != System.DBNull.Value) { objCivil.reci_cReconstitucionReposicion = drCivil["reci_cReconstitucionReposicion"].ToString(); }
                        if (drCivil["reci_iNumeroActaAnterior"] != System.DBNull.Value) { objCivil.reci_iNumeroActaAnterior = Convert.ToInt32(drCivil["reci_iNumeroActaAnterior"].ToString()); }
                        if (drCivil["reci_vTitular"] != System.DBNull.Value) { objCivil.reci_vTitular = drCivil["reci_vTitular"].ToString(); }




                        DataTable dtParticipantes = ds.Tables[1];
                        if (dtParticipantes != null)
                        {
                            if (dtParticipantes.Rows.Count > 0)
                            {
                                BE.RE_PARTICIPANTE objParticipante;

                                foreach (DataRow drParticipante in dtParticipantes.Rows)
                                {
                                    objParticipante = new RE_PARTICIPANTE();

                                    #region SET DEFAULT PARTICIPANTE ...

                                    objParticipante.sTipoActuacionId = -1;
                                    objParticipante.iParticipanteId = 0;
                                    objParticipante.iActuacionDetId = 0;
                                    objParticipante.iActoNotarialId = 0;
                                    objParticipante.iActoJudicialId = 0;
                                    objParticipante.sTipoParticipanteId = 0;
                                    objParticipante.sTipoDatoId = 0;
                                    objParticipante.sTipoVinculoId = 0;
                                    objParticipante.dFechaLlegadaValija = DateTime.MinValue;
                                    objParticipante.sOficinaConsularDestinoId = 0;
                                    objParticipante.bolFirma = false;
                                    objParticipante.bolHuella = false;
                                    objParticipante.sTipoPersonaId = -1;
                                    objParticipante.iPersonaId = -1;
                                    objParticipante.iEmpresaId = -1;
                                    objParticipante.sTipoDocumentoId = -1;
                                    objParticipante.vTipoDocumento = string.Empty.ToString();
                                    objParticipante.vNumeroDocumento = string.Empty.ToString();
                                    objParticipante.sNacionalidadId = -1;
                                    objParticipante.sGeneroId = -1;
                                    objParticipante.pers_dNacimientoFecha = DateTime.MinValue;
                                    objParticipante.pers_cNacimientoLugar = string.Empty;
                                    objParticipante.vNombres = string.Empty.ToString();
                                    objParticipante.vPrimerApellido = string.Empty.ToString();
                                    objParticipante.vSegundoApellido = string.Empty.ToString();
                                    objParticipante.vDireccion = string.Empty.ToString();
                                    objParticipante.vUbigeo = string.Empty.ToString();
                                    objParticipante.ICentroPobladoId = -1;
                                    objParticipante.cEstado = "A";
                                    objParticipante.sUsuarioCreacion = -1;
                                    objParticipante.vIPCreacion = string.Empty.ToString();
                                    objParticipante.sUsuarioModificacion = -1;
                                    objParticipante.vIPModificacion = string.Empty.ToString();
                                    objParticipante.sOficinaConsularId = -1;
                                    objParticipante.vHostname = string.Empty.ToString();
                                    objParticipante.bExisteRune = true;
                                    #endregion SET DEFAULT PARTICIPANTE ...

                                    objParticipante.iActuacionDetId = objCivil.reci_iActuacionDetalleId;
                                    if (drParticipante["iActuacionParticipanteId"] != System.DBNull.Value) { objParticipante.iParticipanteId = Convert.ToInt64(drParticipante["iActuacionParticipanteId"]); }
                                    if (drParticipante["sTipoParticipanteId"] != System.DBNull.Value) { objParticipante.sTipoParticipanteId = Convert.ToInt16(drParticipante["sTipoParticipanteId"]); }
                                    if (drParticipante["iPersonaId"] != System.DBNull.Value) { objParticipante.iPersonaId = Convert.ToInt64(drParticipante["iPersonaId"]); }
                                    if (drParticipante["vApellidoPaterno"] != System.DBNull.Value) { objParticipante.vPrimerApellido = drParticipante["vApellidoPaterno"].ToString(); }
                                    if (drParticipante["vApellidoMaterno"] != System.DBNull.Value) { objParticipante.vSegundoApellido = drParticipante["vApellidoMaterno"].ToString(); }
                                    if (drParticipante["vNombres"] != System.DBNull.Value) { objParticipante.vNombres = drParticipante["vNombres"].ToString(); }
                                    if (drParticipante["sDocumentoTipoId"] != System.DBNull.Value) { objParticipante.sTipoDocumentoId = Convert.ToInt16(drParticipante["sDocumentoTipoId"]); }
                                    if (drParticipante["vDocumentoTipo"] != System.DBNull.Value) { objParticipante.vTipoDocumento = drParticipante["vDocumentoTipo"].ToString(); }
                                    if (drParticipante["vDocumentoNumero"] != System.DBNull.Value) { objParticipante.vNumeroDocumento = drParticipante["vDocumentoNumero"].ToString(); }
                                    if (drParticipante["vTipoParticipante"] != System.DBNull.Value) { objParticipante.vTipoParticipante = drParticipante["vTipoParticipante"].ToString(); }

                                    //if (drParticipante["vDocumentoCompleto"] != System.DBNull.Value) { objParticipante.vDocumentoCompleto = Convert.ToString(drParticipante["vDocumentoCompleto"]); };
                                    if (drParticipante["sTipoDatoId"] != System.DBNull.Value) { objParticipante.sTipoDatoId = Convert.ToInt16(drParticipante["sTipoDatoId"]); }
                                    if (drParticipante["sTipoVinculoId"] != System.DBNull.Value) { objParticipante.sTipoVinculoId = Convert.ToInt16(drParticipante["sTipoVinculoId"]); }
                                    //if (drParticipante["vNombreCompleto"] != System.DBNull.Value) { objParticipante.vNombreCompleto = Convert.ToString(drParticipante["vNombreCompleto"]); }
                                    if (drParticipante["pers_sNacionalidadId"] != System.DBNull.Value) { objParticipante.sNacionalidadId = Convert.ToInt16(drParticipante["pers_sNacionalidadId"]); }
                                    if (drParticipante["pers_sGeneroId"] != System.DBNull.Value) { objParticipante.sGeneroId = Convert.ToInt16(drParticipante["pers_sGeneroId"]); }

                                    if (drParticipante["pers_cNacimientoLugar"] != System.DBNull.Value) { objParticipante.pers_cNacimientoLugar = drParticipante["pers_cNacimientoLugar"].ToString(); objParticipante.bExisteUbicacion = true; }
                                    if (drParticipante["pers_dNacimientoFecha"].ToString() != string.Empty)
                                        objParticipante.pers_dNacimientoFecha = Convert.ToDateTime(drParticipante["pers_dNacimientoFecha"]);
                                    else
                                        objParticipante.pers_dNacimientoFecha = DateTime.MinValue;

                                    if (drParticipante["pers_bFallecidoFlag"] != System.DBNull.Value) { objParticipante.pers_bFallecidoFlag = Convert.ToBoolean(drParticipante["pers_bFallecidoFlag"]); }
                                    if (drParticipante["pers_dFechaDefuncion"] != System.DBNull.Value) { objParticipante.pers_dFechaDefuncion = Convert.ToDateTime(drParticipante["pers_dFechaDefuncion"]); }
                                    if (drParticipante["pers_cUbigeoDefuncion"] != System.DBNull.Value) { objParticipante.pers_cUbigeoDefuncion = drParticipante["pers_cUbigeoDefuncion"].ToString(); }

                                    if (drParticipante["resi_cResidenciaUbigeo"] != System.DBNull.Value) { objParticipante.vUbigeo = drParticipante["resi_cResidenciaUbigeo"].ToString(); objParticipante.bExisteUbicacion = true; }

                                    if (drParticipante["resi_vResidenciaDireccion"] != System.DBNull.Value) { objParticipante.vDireccion = drParticipante["resi_vResidenciaDireccion"].ToString(); }

                                    if (drParticipante["pers_sEstadoCivilId"] != System.DBNull.Value) { objParticipante.pers_sEstadoCivilId = Convert.ToInt32(drParticipante["pers_sEstadoCivilId"]); }


                                    lstParticipante.Add(objParticipante);

                                    #region Titular

                                    if (objCivil.reci_sTipoActaId > 0)
                                    {
                                        if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.NACIMIENTO))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteNacimiento.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                objTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                        else if (objCivil.reci_sTipoActaId == Convert.ToInt16(Enumerador.enmTipoActa.DEFUNCION))
                                        {
                                            if (objParticipante.sTipoParticipanteId == (int)Enumerador.enmParticipanteDefuncion.TITULAR)
                                            {
                                                objTitular.vNombres = objParticipante.vNombres;
                                                objTitular.vPrimerApellido = objParticipante.vPrimerApellido;
                                                objTitular.vSegundoApellido = objParticipante.vSegundoApellido;
                                                objTitular.sGeneroId = objParticipante.sGeneroId;
                                                objTitular.pers_bFallecidoFlag = objParticipante.pers_bFallecidoFlag;
                                                objTitular.pers_dFechaDefuncion = objParticipante.pers_dFechaDefuncion;
                                                objTitular.pers_cUbigeoDefuncion = objParticipante.pers_cUbigeoDefuncion;
                                                objTitular.pers_cNacimientoLugar = objParticipante.pers_cNacimientoLugar;
                                                //bjTitular.pers_dNacimientoFecha = objParticipante.pers_dNacimientoFecha;
                                            }
                                        }
                                    }

                                    #endregion Titular
                                }
                            }
                        }
                    }
                   
                    DataTable dtRegistroCivil_iReferencia = ds.Tables[2];
                    if (dtRegistroCivil_iReferencia.Rows.Count > 0)
                    {
                        DataRow drCivil_iReferenciaID = dtRegistroCivil_iReferencia.Rows[0];
                        if (drCivil_iReferenciaID["acde_iReferenciaId"] != System.DBNull.Value) { objCivil.acde_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["acde_iReferenciaId"].ToString()); }
                        if (drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"] != System.DBNull.Value) { objCivil.reci_iRegistroCivilId_iReferenciaId = Convert.ToInt32(drCivil_iReferenciaID["reci_iRegistroCivilId_Referencia"].ToString()); }
                    }
                }

                #endregion Civil

                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.RESIDENCIA = objResidencia;
                objActuacionCivil.REGISTROCIVIL = objCivil;
                objActuacionCivil.PERSONA = objPersona;
                objActuacionCivil.TITULAR = objTitular;
                objActuacionCivil.PARTICIPANTE_Container = lstParticipante;

                objActoCivilConsultaBL = null;

                return objActuacionCivil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public BE.MRE.RE_ACTUACION obtener(BE.MRE.RE_ACTUACION actuacion){
            RE_ACTUACION_DA lACTUACION_DA = new RE_ACTUACION_DA();
            return lACTUACION_DA.obtener(actuacion);
        }

        public Boolean ExisteDigitalizacion(long acde_iActuacionDetalle,Int32 sSession, ref  Boolean bExiste) {
            ActuacionConsultaDA oRE_ACTUACION_DA = new ActuacionConsultaDA();
            return oRE_ACTUACION_DA.ExisteDigitalizacion(acde_iActuacionDetalle,sSession, ref bExiste);
        }

        // OBTENER DATOS DE LA ACTUACIÓN 58 A DE UNA PERSONA
        //JONATAN SILVA C.
        public DataTable ObtenerActuacion58A(long CodPersona)
        {
            DA.ActuacionConsultaDA objDA = new DA.ActuacionConsultaDA();

            try
            {
                return objDA.ObtenerActuacion58A(CodPersona);
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

        public DataTable ActuacionesObtenerAnuladasReactivar(Int16 actu_sOficinaConsularId, int acde_ICorrelativoActuacion, Int16 acde_AnioTramite)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ActuacionesObtenerAnuladasReactivar(actu_sOficinaConsularId, acde_ICorrelativoActuacion, acde_AnioTramite);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        //-----------------------------------------------
        //Fecha: 13/10/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: consultar la actuación por el ID
        //          para la solicitud de SUNARP.
        //-----------------------------------------------
        public DataTable ConsultarActoNotarialPorIDActuacion(Int64 intActoNotarial)
        {
            ActuacionConsultaDA objDA = new ActuacionConsultaDA();
            try
            {
                return objDA.ConsultarActoNotarialPorIDActuacion(intActoNotarial);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
