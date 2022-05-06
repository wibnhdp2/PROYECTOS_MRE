using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;
using System.Web;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaMantenimientoDA
    {
        ~PersonaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        
        public BE.MRE.RE_PERSONA Insertar(BE.MRE.RE_PERSONA persona)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (persona.pers_sEstadoCivilId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", persona.pers_sEstadoCivilId));
                        if (persona.pers_sProfesionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", persona.pers_sProfesionId));
                        if (persona.pers_sOcupacionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", persona.pers_sOcupacionId));
                        if (persona.pers_sIdiomaNatalId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sIdiomaNatalId", persona.pers_sIdiomaNatalId));
                        if (persona.pers_sGeneroId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", persona.pers_sGeneroId));
                        if (persona.pers_sGradoInstruccionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGradoInstruccionId", persona.pers_sGradoInstruccionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", persona.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", persona.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", persona.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@pers_vCorreoElectronico", persona.pers_vCorreoElectronico));
                        if (persona.pers_dNacimientoFecha != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", persona.pers_dNacimientoFecha));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", persona.pers_sPersonaTipoId));
                        if (persona.pers_cNacimientoLugar != null || persona.pers_cNacimientoLugar != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@pers_cNacimientoLugar", persona.pers_cNacimientoLugar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vObservaciones", persona.pers_vObservaciones));
                        if (persona.pers_sNacionalidadId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", persona.pers_sNacionalidadId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vEstatura", persona.pers_vEstatura));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPeso", persona.pers_sPeso));
                        if (persona.pers_sOcurrenciaTipoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcurrenciaTipoId", persona.pers_sOcurrenciaTipoId));
                        if (persona.pers_IOcurrenciaCentroPobladoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_IOcurrenciaCentroPobladoId", persona.pers_IOcurrenciaCentroPobladoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vLugarNacimiento", persona.pers_vLugarNacimiento));
                        if (persona.pers_sColorTezId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorTezId", persona.pers_sColorTezId));
                        if (persona.pers_sColorOjosId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorOjosId", persona.pers_sColorOjosId));
                        if (persona.pers_sColorCabelloId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorCabelloId", persona.pers_sColorCabelloId));
                        if (persona.pers_sGrupoSanguineoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGrupoSanguineoId", persona.pers_sGrupoSanguineoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vSenasParticulares", persona.pers_vSenasParticulares));
                        cmd.Parameters.Add(new SqlParameter("@pers_bFallecidoFlag", persona.pers_bFallecidoFlag));
                        if (persona.pers_dFechaDefuncion != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pers_dFechaDefuncion", persona.pers_dFechaDefuncion));
                        if (persona.pers_cUbigeoDefuncion != null || persona.pers_cUbigeoDefuncion != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@pers_cUbigeoDefuncion", persona.pers_cUbigeoDefuncion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", persona.pers_vApellidoCasada));
                        cmd.Parameters.Add(new SqlParameter("@pers_sOficinaConsularId", persona.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pers_bGenera58A", persona.bGenera58A));
                        cmd.Parameters.Add(new SqlParameter("@pers_sUsuarioCreacion", persona.pers_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vIPCreacion", Util.ObtenerDireccionIP()));
                        //cmd.Parameters.Add(new SqlParameter("@pers_sOficinaConsularId", persona.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        persona.pers_iPersonaId = Convert.ToInt64(lReturn.Value);
                        persona.Error = false; 
                    }
                }
            }
            catch (SqlException exec)
            {
                persona.Error = true;
                persona.Message = exec.Message.ToString();
            }

            return persona;
        }

        public BE.MRE.RE_PERSONA Actualizar(BE.MRE.RE_PERSONA persona)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", persona.pers_iPersonaId));
                        if (persona.pers_sEstadoCivilId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", persona.pers_sEstadoCivilId));
                        if (persona.pers_sProfesionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", persona.pers_sProfesionId));
                        if (persona.pers_sOcupacionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", persona.pers_sOcupacionId));
                        if (persona.pers_sIdiomaNatalId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sIdiomaNatalId", persona.pers_sIdiomaNatalId));
                        if (persona.pers_sGeneroId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", persona.pers_sGeneroId));
                        if (persona.pers_sGradoInstruccionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGradoInstruccionId", persona.pers_sGradoInstruccionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", persona.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", persona.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", persona.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@pers_vCorreoElectronico", persona.pers_vCorreoElectronico));
                        if (persona.pers_dNacimientoFecha != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", persona.pers_dNacimientoFecha));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", persona.pers_sPersonaTipoId));
                        if (persona.pers_cNacimientoLugar != string.Empty && persona.pers_cNacimientoLugar != null)
                            cmd.Parameters.Add(new SqlParameter("@pers_cNacimientoLugar", persona.pers_cNacimientoLugar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vObservaciones", persona.pers_vObservaciones));
                        if (persona.pers_sNacionalidadId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", persona.pers_sNacionalidadId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vEstatura", persona.pers_vEstatura));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPeso", persona.pers_sPeso));
                        if (persona.pers_sOcurrenciaTipoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcurrenciaTipoId", persona.pers_sOcurrenciaTipoId));
                        if (persona.pers_IOcurrenciaCentroPobladoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_IOcurrenciaCentroPobladoId", persona.pers_IOcurrenciaCentroPobladoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vLugarNacimiento", persona.pers_vLugarNacimiento));
                        if (persona.pers_sColorTezId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorTezId", persona.pers_sColorTezId));
                        if (persona.pers_sColorOjosId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorOjosId", persona.pers_sColorOjosId));
                        if (persona.pers_sColorCabelloId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sColorCabelloId", persona.pers_sColorCabelloId));
                        if (persona.pers_sGrupoSanguineoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGrupoSanguineoId", persona.pers_sGrupoSanguineoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vSenasParticulares", persona.pers_vSenasParticulares));
                        cmd.Parameters.Add(new SqlParameter("@pers_bFallecidoFlag", persona.pers_bFallecidoFlag));
                        if (persona.pers_dFechaDefuncion != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pers_dFechaDefuncion", persona.pers_bFallecidoFlag));
                        if (persona.pers_cUbigeoDefuncion != string.Empty || persona.pers_cUbigeoDefuncion != null)
                            cmd.Parameters.Add(new SqlParameter("@pers_cUbigeoDefuncion", persona.pers_cUbigeoDefuncion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", persona.pers_vApellidoCasada));
                        cmd.Parameters.Add(new SqlParameter("@pers_bGenera58A", persona.bGenera58A));
                        cmd.Parameters.Add(new SqlParameter("@pers_sUsuarioModificacion", persona.pers_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vIPModificacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@pers_sOficinaConsularId", persona.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        persona.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                persona.Error = true;
                persona.Message = exec.Message.ToString();
            }

            return persona;
        }

        public BE.MRE.RE_REGISTROUNICO InsertarRU(BE.MRE.RE_REGISTROUNICO registroUnico)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROUNICO_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@reun_iPersonaId", registroUnico.reun_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaNombre", registroUnico.reun_vEmergenciaNombre));
                        if (registroUnico.reun_sEmergenciaRelacionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@reun_sEmergenciaRelacionId", registroUnico.reun_sEmergenciaRelacionId));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaDireccionLocal", registroUnico.reun_vEmergenciaDireccionLocal));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaCodigoPostal", registroUnico.reun_vEmergenciaCodigoPostal));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaTelefono", registroUnico.reun_vEmergenciaTelefono));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaDireccionPeru", registroUnico.reun_vEmergenciaDireccionPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaCorreoElectronico", registroUnico.reun_vEmergenciaCorreoElectronico));
                        cmd.Parameters.Add(new SqlParameter("@reun_cViveExteriorDesde", registroUnico.reun_cViveExteriorDesde));
                        cmd.Parameters.Add(new SqlParameter("@reun_bPiensaRetornarAlPeru", registroUnico.reun_bPiensaRetornarAlPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_cCuandoRetornaAlPeru", registroUnico.reun_cCuandoRetornaAlPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAfiliadoSeguroSocial", registroUnico.reun_bAfiliadoSeguroSocial));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAfiliadoAFP", registroUnico.reun_bAfiliadoAFP));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAportaSeguroSocial", registroUnico.reun_bAportaSeguroSocial));
                        cmd.Parameters.Add(new SqlParameter("@reun_bBeneficiadoExterior", registroUnico.reun_bBeneficiadoExterior));
                        if (registroUnico.reun_sOcupacionPeru != 0)
                            cmd.Parameters.Add(new SqlParameter("@reun_sOcupacionPeru", registroUnico.reun_sOcupacionPeru));
                        if (registroUnico.reun_sOcupacionExtranjero != 0)
                            cmd.Parameters.Add(new SqlParameter("@reun_sOcupacionExtranjero", registroUnico.reun_sOcupacionExtranjero));
                        cmd.Parameters.Add(new SqlParameter("@reun_vNombreConvenio", registroUnico.reun_vNombreConvenio));
                        cmd.Parameters.Add(new SqlParameter("@reun_sUsuarioCreacion", registroUnico.reun_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@reun_vIPCreacion", registroUnico.reun_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@reun_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@reun_sOficinaConsularId", registroUnico.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@reun_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@reun_iRegistroUnicoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        
                        registroUnico.Error = false;
                        registroUnico.reun_iRegistroUnicoId = Convert.ToInt64(lReturn.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                registroUnico.Error = true;
                registroUnico.Message = exec.Message.ToString();
            }

            return registroUnico;
        }

        private string StrConnectionName = string.Empty;     

        public PersonaMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
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
            long LonResultQuery;
            int IntNumberRowsDet1OK = 0, IntNumberRowsDet2OK = 0;
            int IntNumberRowsImagenes = 0, IntNumberRowsFilaciones = 0;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[46];

                #region Insertar Persona

                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Direction = ParameterDirection.Output;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = ObjPersBE.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[2].Value = ObjPersBE.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[3].Value = ObjPersBE.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.NVarChar, 100);
                prmParameterHeader[4].Value = ObjPersBE.pers_vNombres;

                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                prmParameterHeader[5] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[5].Value = ObjPersIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[6] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[6].Value = ObjPersIdentBE.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (ObjPersBE.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = ObjPersBE.pers_vCorreoElectronico;
                }

                if (ObjPersBE.pers_dNacimientoFecha == null)
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    if (ObjPersBE.pers_dNacimientoFecha == DateTime.MinValue)
                    {
                        prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                        prmParameterHeader[8].Value = DBNull.Value;
                    }
                    else
                    {
                        prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                        prmParameterHeader[8].Value = ObjPersBE.pers_dNacimientoFecha;
                    }
                }

                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                if (ObjPersBE.pers_cNacimientoLugar == "0")
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = ObjPersBE.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (ObjPersBE.pers_sGeneroId == 0)
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = ObjPersBE.pers_sGeneroId;
                }

                if (ObjPersBE.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = ObjPersBE.pers_vObservaciones;
                }

                prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                prmParameterHeader[12].Value = ObjPersBE.pers_sNacionalidadId;

                if (ObjPersBE.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = ObjPersBE.pers_sEstadoCivilId;
                }

                if (ObjPersBE.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = ObjPersBE.pers_sGradoInstruccionId;
                }

                if (ObjPersBE.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = ObjPersBE.pers_sOcupacionId;
                }

                if (ObjPersBE.pers_sProfesionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = ObjPersBE.pers_sProfesionId;
                }

                if (ObjPersBE.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = ObjPersBE.pers_vApellidoCasada;
                }

                //*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                if (ObjRegistroUnicoBE.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = ObjRegistroUnicoBE.reun_vEmergenciaNombre;
                }

                if (ObjRegistroUnicoBE.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = ObjRegistroUnicoBE.reun_sEmergenciaRelacionId;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = ObjRegistroUnicoBE.reun_vEmergenciaTelefono;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico;
                }

                if (ObjRegistroUnicoBE.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = ObjRegistroUnicoBE.reun_cViveExteriorDesde;
                }

                prmParameterHeader[26] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[26].Value = ObjRegistroUnicoBE.reun_bPiensaRetornarAlPeru;

                if (ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[28].Value = ObjRegistroUnicoBE.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[29] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[29].Value = ObjRegistroUnicoBE.reun_bAfiliadoAFP;

                prmParameterHeader[30] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = ObjRegistroUnicoBE.reun_bAportaSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[31].Value = ObjRegistroUnicoBE.reun_bBeneficiadoExterior;

                if (ObjRegistroUnicoBE.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = ObjRegistroUnicoBE.reun_sOcupacionPeru;
                }

                if (ObjRegistroUnicoBE.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = ObjRegistroUnicoBE.reun_sOcupacionExtranjero;
                }

                if (ObjRegistroUnicoBE.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = ObjRegistroUnicoBE.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[35] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[35].Value = IntOficinaConsularId;

                prmParameterHeader[36] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[36].Value = bGenera58A;

                prmParameterHeader[37] = new SqlParameter("@pers_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = ObjPersBE.pers_sUsuarioCreacion;

                prmParameterHeader[38] = new SqlParameter("@pers_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[38].Value = ObjPersBE.pers_vIPCreacion;

                prmParameterHeader[39] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[39].Value = Util.ObtenerHostName();

                prmParameterHeader[40] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[40].Value = ObjPersBE.pers_bFallecidoFlag;

                prmParameterHeader[41] = new SqlParameter("@actu_sCiudadItinerante", SqlDbType.SmallInt);
                prmParameterHeader[41].Value = CiudadItinerante;

                prmParameterHeader[42] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                prmParameterHeader[42].Value = ObjPersBE.pers_vSenasParticulares;
                
                prmParameterHeader[43] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (ObjPersBE.pers_sPaisId == 0)
                {
                    prmParameterHeader[43].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[43].Value = ObjPersBE.pers_sPaisId;
                }
                prmParameterHeader[44] = new SqlParameter("@peid_vTipoDocumento", SqlDbType.VarChar);
                if (ObjPersIdentBE.peid_vTipodocumento.Length == 0)
                {
                    prmParameterHeader[44].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[44].Value = ObjPersIdentBE.peid_vTipodocumento;
                }

                prmParameterHeader[45] = new SqlParameter("@pers_bPadresPeruanos", SqlDbType.Bit);
                prmParameterHeader[45].Value = ObjPersBE.pers_bPadresPeruanos;

                LonResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ADICIONAR",
                                                           prmParameterHeader);

                #endregion Insertar Persona

                LonPersonaId = (long)prmParameterHeader[0].Value;

                if (LonPersonaId > 0)
                {
                    ObjPersBE.pers_iPersonaId = LonPersonaId;

                    if (DtDirecciones != null)
                    {
                        #region Direcciones

                        SqlParameter[] prmParameterDirecciones;

                        for (int j = 0; j < DtDirecciones.Rows.Count; j++)
                        {
                            prmParameterDirecciones = new SqlParameter[11];

                            prmParameterDirecciones[0] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                            prmParameterDirecciones[0].Value = Convert.ToInt16(DtDirecciones.Rows[j][4]);

                            prmParameterDirecciones[1] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                            prmParameterDirecciones[1].Value = DtDirecciones.Rows[j][3].ToString();

                            prmParameterDirecciones[2] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                            prmParameterDirecciones[2].Value = DtDirecciones.Rows[j][2].ToString();

                            prmParameterDirecciones[3] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                            prmParameterDirecciones[3].Value = DtDirecciones.Rows[j][10].ToString();

                            prmParameterDirecciones[4] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                            prmParameterDirecciones[4].Value = DtDirecciones.Rows[j][6].ToString();

                            //prmParameterDirecciones[5] = new SqlParameter("@resi_sCentroPobladoId", SqlDbType.SmallInt);
                            //prmParameterDirecciones[5].Value = DBNull.Value;

                            prmParameterDirecciones[6] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                            prmParameterDirecciones[6].Value = Convert.ToInt16(IntOficinaConsularId);

                            prmParameterDirecciones[7] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                            prmParameterDirecciones[7].Value = ObjPersBE.pers_sUsuarioCreacion;

                            prmParameterDirecciones[8] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                            prmParameterDirecciones[8].Value = ObjPersBE.pers_vIPCreacion;

                            prmParameterDirecciones[9] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                            prmParameterDirecciones[9].Value = Util.ObtenerHostName();

                            prmParameterDirecciones[10] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                            prmParameterDirecciones[10].Direction = ParameterDirection.Output;

                            IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                            CommandType.StoredProcedure,
                                                                            "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                                            prmParameterDirecciones);

                            long LonResidenciaId = (long)prmParameterDirecciones[10].Value;

                            if (LonResidenciaId > 0)
                            {
                                SqlParameter[] prmParameterDetail2;

                                prmParameterDetail2 = new SqlParameter[6];

                                prmParameterDetail2[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                                prmParameterDetail2[0].Value = ObjPersBE.pers_iPersonaId;

                                prmParameterDetail2[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                                prmParameterDetail2[1].Value = LonResidenciaId;

                                prmParameterDetail2[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterDetail2[2].Value = IntOficinaConsularId;

                                prmParameterDetail2[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                                prmParameterDetail2[3].Value = ObjPersBE.pers_sUsuarioCreacion;

                                prmParameterDetail2[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                                prmParameterDetail2[4].Value = ObjPersBE.pers_vIPCreacion;

                                prmParameterDetail2[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                                prmParameterDetail2[5].Value = Util.ObtenerHostName();

                                IntNumberRowsDet2OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                CommandType.StoredProcedure,
                                                                                "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                                prmParameterDetail2);
                            }
                        }

                        #endregion Direcciones
                    }

                    if (DtFiliaciones != null)
                    {
                        #region Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)

                        SqlParameter[] prmParameterFilaciones;

                        for (int k = 0; k < DtFiliaciones.Rows.Count; k++)
                        {
                            prmParameterFilaciones = new SqlParameter[14];

                            prmParameterFilaciones[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                            prmParameterFilaciones[0].Direction = ParameterDirection.Output;

                            prmParameterFilaciones[1] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                            prmParameterFilaciones[1].Value = LonPersonaId;

////////////////////////////////

                            prmParameterFilaciones[2] = new SqlParameter("@pefi_iFiliadoId", SqlDbType.BigInt);
                            prmParameterFilaciones[2].Value = DtFiliaciones.Rows[k][15];

                            prmParameterFilaciones[3] = new SqlParameter("@pefi_sDocumentoTipoId", SqlDbType.SmallInt);
                            prmParameterFilaciones[3].Value = DtFiliaciones.Rows[k][14];

////////////////////////////////

                            prmParameterFilaciones[4] = new SqlParameter("@pefi_vNombreFiliacion", SqlDbType.VarChar, 500);
                            prmParameterFilaciones[4].Value = DtFiliaciones.Rows[k][1];

                            prmParameterFilaciones[5] = new SqlParameter("@pefi_vLugarNacimiento", SqlDbType.VarChar, 500);
                            prmParameterFilaciones[5].Value = DtFiliaciones.Rows[k][2];

                            prmParameterFilaciones[6] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.DateTime);
                            prmParameterFilaciones[6].Value = DtFiliaciones.Rows[k][3];

                            prmParameterFilaciones[7] = new SqlParameter("@pefi_sNacionalidad", SqlDbType.SmallInt);
                            prmParameterFilaciones[7].Value = DtFiliaciones.Rows[k][13];

                            prmParameterFilaciones[8] = new SqlParameter("@pefi_sTipoFilacionId", SqlDbType.SmallInt);
                            prmParameterFilaciones[8].Value = DtFiliaciones.Rows[k][6];

                            prmParameterFilaciones[9] = new SqlParameter("@pefi_vNroDocumento", SqlDbType.VarChar, 20);
                            prmParameterFilaciones[9].Value = DtFiliaciones.Rows[k][8];

                            prmParameterFilaciones[10] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                            prmParameterFilaciones[10].Value = IntOficinaConsularId;

                            prmParameterFilaciones[11] = new SqlParameter("@pefi_sUsuarioCreacion", SqlDbType.SmallInt);
                            prmParameterFilaciones[11].Value = ObjPersBE.pers_sUsuarioCreacion;

                            prmParameterFilaciones[12] = new SqlParameter("@pefi_vIPCreacion", SqlDbType.VarChar, 50);
                            prmParameterFilaciones[12].Value = ObjPersBE.pers_vIPCreacion;

                            prmParameterFilaciones[13] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                            prmParameterFilaciones[13].Value = Util.ObtenerHostName();

                            IntNumberRowsFilaciones = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                CommandType.StoredProcedure,
                                                                                "PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR",
                                                                                prmParameterFilaciones);
                        }

                        #endregion Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)
                    }

                    #region Persona Foto (PN_REGISTRO.RE_PERSONAFOTO)

                    if (IntFlagIngresoFoto == 1)
                    {
                        if (DtImagenes != null)
                        {
                            SqlParameter[] prmParameterImagen;

                            for (int z = 0; z < DtImagenes.Rows.Count; z++)
                            {
                                prmParameterImagen = new SqlParameter[8];

                                prmParameterImagen[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                                prmParameterImagen[0].Direction = ParameterDirection.Output;

                                prmParameterImagen[1] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                                prmParameterImagen[1].Value = LonPersonaId;

                                prmParameterImagen[2] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                                prmParameterImagen[2].Value = DtImagenes.Rows[z][2];

                                prmParameterImagen[3] = new SqlParameter("@pefo_GFoto", SqlDbType.Image);
                                prmParameterImagen[3].Value = DtImagenes.Rows[z][3];

                                prmParameterImagen[4] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterImagen[4].Value = IntOficinaConsularId;

                                prmParameterImagen[5] = new SqlParameter("@pefo_sUsuarioCreacion", SqlDbType.SmallInt);
                                prmParameterImagen[5].Value = ObjPersBE.pers_sUsuarioCreacion;

                                prmParameterImagen[6] = new SqlParameter("@pefo_vIPCreacion", SqlDbType.VarChar, 50);
                                prmParameterImagen[6].Value = ObjPersBE.pers_vIPCreacion;

                                prmParameterImagen[7] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                                prmParameterImagen[7].Value = Util.ObtenerHostName();

                                IntNumberRowsImagenes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                CommandType.StoredProcedure,
                                                                                "PN_REGISTRO.USP_RE_PERSONAFOTO_ADICIONAR",
                                                                                prmParameterImagen);
                            }
                        }
                    }

                    #endregion Persona Foto (PN_REGISTRO.RE_PERSONAFOTO)
                }
                return (int)Enumerador.enmResultadoQuery.OK;
            }
           
            catch (SqlException exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;

            }
            catch (Exception exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;
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
            long LonResultQuery;
            int IntNumberRowsDet1OK = 0, IntNumberRowsDet2OK = 0;
            int IntNumberRowsImagenes = 0, IntNumberRowsFilaciones = 0;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[47];

                #region Insertar Persona

                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Direction = ParameterDirection.Output;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = ObjPersBE.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[2].Value = ObjPersBE.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[3].Value = ObjPersBE.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.NVarChar, 100);
                prmParameterHeader[4].Value = ObjPersBE.pers_vNombres;

                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                prmParameterHeader[5] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[5].Value = ObjPersIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[6] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[6].Value = ObjPersIdentBE.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (ObjPersBE.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = ObjPersBE.pers_vCorreoElectronico;
                }

                if (ObjPersBE.pers_dNacimientoFecha == null)
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    if (ObjPersBE.pers_dNacimientoFecha == DateTime.MinValue)
                    {
                        prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                        prmParameterHeader[8].Value = DBNull.Value;
                    }
                    else
                    {
                        prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                        prmParameterHeader[8].Value = ObjPersBE.pers_dNacimientoFecha;
                    }
                }

                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                if (ObjPersBE.pers_cNacimientoLugar == "0")
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = ObjPersBE.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (ObjPersBE.pers_sGeneroId == 0)
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = ObjPersBE.pers_sGeneroId;
                }

                if (ObjPersBE.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = ObjPersBE.pers_vObservaciones;
                }

                prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                prmParameterHeader[12].Value = ObjPersBE.pers_sNacionalidadId;

                if (ObjPersBE.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = ObjPersBE.pers_sEstadoCivilId;
                }

                if (ObjPersBE.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = ObjPersBE.pers_sGradoInstruccionId;
                }

                if (ObjPersBE.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = ObjPersBE.pers_sOcupacionId;
                }

                if (ObjPersBE.pers_sProfesionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = ObjPersBE.pers_sProfesionId;
                }

                if (ObjPersBE.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.VarChar, 100);
                    prmParameterHeader[17].Value = ObjPersBE.pers_vApellidoCasada;
                }

                //*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                if (ObjRegistroUnicoBE.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = ObjRegistroUnicoBE.reun_vEmergenciaNombre;
                }

                if (ObjRegistroUnicoBE.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = ObjRegistroUnicoBE.reun_sEmergenciaRelacionId;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = ObjRegistroUnicoBE.reun_vEmergenciaTelefono;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico;
                }

                if (ObjRegistroUnicoBE.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = ObjRegistroUnicoBE.reun_cViveExteriorDesde;
                }

                prmParameterHeader[26] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[26].Value = ObjRegistroUnicoBE.reun_bPiensaRetornarAlPeru;

                if (ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[28].Value = ObjRegistroUnicoBE.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[29] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[29].Value = ObjRegistroUnicoBE.reun_bAfiliadoAFP;

                prmParameterHeader[30] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = ObjRegistroUnicoBE.reun_bAportaSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[31].Value = ObjRegistroUnicoBE.reun_bBeneficiadoExterior;

                if (ObjRegistroUnicoBE.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = ObjRegistroUnicoBE.reun_sOcupacionPeru;
                }

                if (ObjRegistroUnicoBE.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = ObjRegistroUnicoBE.reun_sOcupacionExtranjero;
                }

                if (ObjRegistroUnicoBE.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = ObjRegistroUnicoBE.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[35] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[35].Value = IntOficinaConsularId;

                prmParameterHeader[36] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[36].Value = bGenera58A;

                prmParameterHeader[37] = new SqlParameter("@pers_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = ObjPersBE.pers_sUsuarioCreacion;

                prmParameterHeader[38] = new SqlParameter("@pers_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[38].Value = ObjPersBE.pers_vIPCreacion;

                prmParameterHeader[39] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[39].Value = Util.ObtenerHostName();

                prmParameterHeader[40] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[40].Value = ObjPersBE.pers_bFallecidoFlag;

                prmParameterHeader[41] = new SqlParameter("@actu_sCiudadItinerante", SqlDbType.SmallInt);
                prmParameterHeader[41].Value = CiudadItinerante;

                prmParameterHeader[42] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                prmParameterHeader[42].Value = ObjPersBE.pers_vSenasParticulares;

                prmParameterHeader[43] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (ObjPersBE.pers_sPaisId == 0)
                {
                    prmParameterHeader[43].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[43].Value = ObjPersBE.pers_sPaisId;
                }
                prmParameterHeader[44] = new SqlParameter("@peid_vTipoDocumento", SqlDbType.VarChar);
                if (ObjPersIdentBE.peid_vTipodocumento.Length == 0)
                {
                    prmParameterHeader[44].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[44].Value = ObjPersIdentBE.peid_vTipodocumento;
                }

                prmParameterHeader[45] = new SqlParameter("@pers_bPadresPeruanos", SqlDbType.Bit);
                prmParameterHeader[45].Value = ObjPersBE.pers_bPadresPeruanos;

                //==== Fecha:11/01/2022, Autor: Pipa
                //==== Motivo: se adiciona la siguiente linea, indica la validacion por reniec 
                //==== tabla RegistroUnico (reun_iRegistroUnicoId) para reutilizar en boton grabar en pestaña Datos del Contacto
                prmParameterHeader[46] = new SqlParameter("@pers_bValidacionReniec", SqlDbType.Bit);
                prmParameterHeader[46].Value = ObjPersIdentBE.pers_bValidacionReniec;

                LonResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ADICIONAR",
                                                           prmParameterHeader);

                #endregion Insertar Persona

                LonPersonaId = (long)prmParameterHeader[0].Value;
                
                if (LonPersonaId > 0)
                {
                    ObjPersBE.pers_iPersonaId = LonPersonaId;
                   // ObjPersBE.REGISTROUNICO.reun_iRegistroUnicoId = (long)prmParameterHeader[46].Value;

                    #region Direcciones

                    SqlParameter[] prmParameterDirecciones;

                    prmParameterDirecciones = new SqlParameter[10];

                    prmParameterDirecciones[0] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                    prmParameterDirecciones[0].Value = objResidencia.resi_sResidenciaTipoId;

                    prmParameterDirecciones[1] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                    prmParameterDirecciones[1].Value = objResidencia.resi_vResidenciaDireccion;

                    prmParameterDirecciones[2] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterDirecciones[2].Value = objResidencia.resi_vCodigoPostal;

                    prmParameterDirecciones[3] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[3].Value = objResidencia.resi_vResidenciaTelefono;

                    prmParameterDirecciones[4] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                    prmParameterDirecciones[4].Value = objResidencia.resi_cResidenciaUbigeo;

                    //prmParameterDirecciones[5] = new SqlParameter("@resi_ICentroPobladoId", SqlDbType.Int);
                    //prmParameterDirecciones[5].Value = objResidencia.resi_ICentroPobladoId;

                    prmParameterDirecciones[5] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterDirecciones[5].Value = ObjPersBE.OficinaConsularId;

                    prmParameterDirecciones[6] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterDirecciones[6].Value = ObjPersBE.pers_sUsuarioCreacion;

                    prmParameterDirecciones[7] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[7].Value = ObjPersBE.pers_vIPCreacion;

                    prmParameterDirecciones[8] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                    prmParameterDirecciones[8].Value = Util.ObtenerHostName();

                    prmParameterDirecciones[9] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                    prmParameterDirecciones[9].Direction = ParameterDirection.Output;

                    IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                                    prmParameterDirecciones);

                    long LonResidenciaId = (long)prmParameterDirecciones[9].Value;

                    if (LonResidenciaId > 0)
                    {
                        SqlParameter[] prmParameterDetail2;

                        prmParameterDetail2 = new SqlParameter[6];

                        prmParameterDetail2[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                        prmParameterDetail2[0].Value = ObjPersBE.pers_iPersonaId;

                        prmParameterDetail2[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                        prmParameterDetail2[1].Value = LonResidenciaId;

                        prmParameterDetail2[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                        prmParameterDetail2[2].Value = IntOficinaConsularId;

                        prmParameterDetail2[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                        prmParameterDetail2[3].Value = ObjPersBE.pers_sUsuarioCreacion;

                        prmParameterDetail2[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                        prmParameterDetail2[4].Value = ObjPersBE.pers_vIPCreacion;

                        prmParameterDetail2[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                        prmParameterDetail2[5].Value = Util.ObtenerHostName();

                        IntNumberRowsDet2OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                        CommandType.StoredProcedure,
                                                                        "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                        prmParameterDetail2);
                    }

                    #endregion Direcciones

                    if (DtFiliaciones != null)
                    {
                        #region Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)

                        SqlParameter[] prmParameterFilaciones;

                        for (int k = 0; k < DtFiliaciones.Rows.Count; k++)
                        {
                            prmParameterFilaciones = new SqlParameter[14];

                            prmParameterFilaciones[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                            prmParameterFilaciones[0].Direction = ParameterDirection.Output;

                            prmParameterFilaciones[1] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                            prmParameterFilaciones[1].Value = LonPersonaId;

                            ////////////////////////////////

                            prmParameterFilaciones[2] = new SqlParameter("@pefi_iFiliadoId", SqlDbType.BigInt);
                            prmParameterFilaciones[2].Value = DtFiliaciones.Rows[k][15];

                            prmParameterFilaciones[3] = new SqlParameter("@pefi_sDocumentoTipoId", SqlDbType.SmallInt);
                            prmParameterFilaciones[3].Value = DtFiliaciones.Rows[k][14];

                            ////////////////////////////////

                            prmParameterFilaciones[4] = new SqlParameter("@pefi_vNombreFiliacion", SqlDbType.VarChar, 500);
                            prmParameterFilaciones[4].Value = DtFiliaciones.Rows[k][1];

                            prmParameterFilaciones[5] = new SqlParameter("@pefi_vLugarNacimiento", SqlDbType.VarChar, 500);
                            prmParameterFilaciones[5].Value = DtFiliaciones.Rows[k][2];

                            prmParameterFilaciones[6] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.DateTime);
                            prmParameterFilaciones[6].Value = DtFiliaciones.Rows[k][3];

                            prmParameterFilaciones[7] = new SqlParameter("@pefi_sNacionalidad", SqlDbType.SmallInt);
                            prmParameterFilaciones[7].Value = DtFiliaciones.Rows[k][13];

                            prmParameterFilaciones[8] = new SqlParameter("@pefi_sTipoFilacionId", SqlDbType.SmallInt);
                            prmParameterFilaciones[8].Value = DtFiliaciones.Rows[k][6];

                            prmParameterFilaciones[9] = new SqlParameter("@pefi_vNroDocumento", SqlDbType.VarChar, 20);
                            prmParameterFilaciones[9].Value = DtFiliaciones.Rows[k][8];

                            prmParameterFilaciones[10] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                            prmParameterFilaciones[10].Value = IntOficinaConsularId;

                            prmParameterFilaciones[11] = new SqlParameter("@pefi_sUsuarioCreacion", SqlDbType.SmallInt);
                            prmParameterFilaciones[11].Value = ObjPersBE.pers_sUsuarioCreacion;

                            prmParameterFilaciones[12] = new SqlParameter("@pefi_vIPCreacion", SqlDbType.VarChar, 50);
                            prmParameterFilaciones[12].Value = ObjPersBE.pers_vIPCreacion;

                            prmParameterFilaciones[13] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                            prmParameterFilaciones[13].Value = Util.ObtenerHostName();

                            IntNumberRowsFilaciones = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                CommandType.StoredProcedure,
                                                                                "PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR",
                                                                                prmParameterFilaciones);
                        }

                        #endregion Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)
                    }

                    #region Persona Foto (PN_REGISTRO.RE_PERSONAFOTO)

                    if (IntFlagIngresoFoto == 1)
                    {
                        if (DtImagenes != null)
                        {
                            SqlParameter[] prmParameterImagen;

                            for (int z = 0; z < DtImagenes.Rows.Count; z++)
                            {
                                prmParameterImagen = new SqlParameter[8];

                                prmParameterImagen[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                                prmParameterImagen[0].Direction = ParameterDirection.Output;

                                prmParameterImagen[1] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                                prmParameterImagen[1].Value = LonPersonaId;

                                prmParameterImagen[2] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                                prmParameterImagen[2].Value = DtImagenes.Rows[z][2];

                                prmParameterImagen[3] = new SqlParameter("@pefo_GFoto", SqlDbType.Image);
                                prmParameterImagen[3].Value = DtImagenes.Rows[z][3];

                                prmParameterImagen[4] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterImagen[4].Value = IntOficinaConsularId;

                                prmParameterImagen[5] = new SqlParameter("@pefo_sUsuarioCreacion", SqlDbType.SmallInt);
                                prmParameterImagen[5].Value = ObjPersBE.pers_sUsuarioCreacion;

                                prmParameterImagen[6] = new SqlParameter("@pefo_vIPCreacion", SqlDbType.VarChar, 50);
                                prmParameterImagen[6].Value = ObjPersBE.pers_vIPCreacion;

                                prmParameterImagen[7] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                                prmParameterImagen[7].Value = Util.ObtenerHostName();

                                IntNumberRowsImagenes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                CommandType.StoredProcedure,
                                                                                "PN_REGISTRO.USP_RE_PERSONAFOTO_ADICIONAR",
                                                                                prmParameterImagen);
                            }
                        }
                    }

                    #endregion Persona Foto (PN_REGISTRO.RE_PERSONAFOTO)
                }
                return (int)Enumerador.enmResultadoQuery.OK;
            }

            catch (SqlException exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;

            }
            catch (Exception exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;
            }

        }
        public Int64 Insertar(BE.MRE.RE_PERSONA ObjPersBE,
                            BE.MRE.RE_PERSONAIDENTIFICACION ObjPersIdentBE,
                            BE.MRE.RE_REGISTROUNICO ObjRegistroUnicoBE,
                            BE.MRE.RE_PERSONARESIDENCIA objPersonaResidencia,
                            BE.MRE.RE_RESIDENCIA objResidencia,
                            int IntOficinaConsularId,
                            bool bGenera58A,
                            ref long LonPersonaId)
        {
            long LonResultQuery;
            int IntNumberRowsDet1OK = 0, IntNumberRowsDet2OK = 0;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[43];

                #region Insertar Persona

                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Direction = ParameterDirection.Output;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = ObjPersBE.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[2].Value =   ObjPersBE.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[3].Value =  ObjPersBE.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.NVarChar, 100);
                prmParameterHeader[4].Value =  ObjPersBE.pers_vNombres;

                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                prmParameterHeader[5] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[5].Value = ObjPersIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[6] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[6].Value = ObjPersIdentBE.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (ObjPersBE.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[7].Value = ObjPersBE.pers_vCorreoElectronico;
                }

                if (ObjPersBE.pers_dNacimientoFecha == null)
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                    prmParameterHeader[8].Value = ObjPersBE.pers_dNacimientoFecha;
                }

                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                if (ObjPersBE.pers_cNacimientoLugar == "00")
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[9].Value = ObjPersBE.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (ObjPersBE.pers_sGeneroId == 0)
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[10].Value = ObjPersBE.pers_sGeneroId;
                }

                if (ObjPersBE.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[11].Value = ObjPersBE.pers_vObservaciones;
                }

                if (ObjPersBE.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                    prmParameterHeader[12].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                    prmParameterHeader[12].Value = ObjPersBE.pers_sNacionalidadId;
                }

                if (ObjPersBE.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[13] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[13].Value = ObjPersBE.pers_sEstadoCivilId;
                }

                if (ObjPersBE.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = ObjPersBE.pers_sGradoInstruccionId;
                }

                if (ObjPersBE.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = ObjPersBE.pers_sOcupacionId;
                }

                if (ObjPersBE.pers_sProfesionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = ObjPersBE.pers_sProfesionId;
                }

                if (ObjPersBE.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[17].Value = ObjPersBE.pers_vApellidoCasada;
                }

                //*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                if (ObjRegistroUnicoBE.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[18].Value = ObjRegistroUnicoBE.reun_vEmergenciaNombre;
                }

                if (ObjRegistroUnicoBE.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[19] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[19].Value = ObjRegistroUnicoBE.reun_sEmergenciaRelacionId;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[20].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[21].Value = ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[22].Value = ObjRegistroUnicoBE.reun_vEmergenciaTelefono;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[23].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[24].Value = ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico;
                }

                if (ObjRegistroUnicoBE.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[25].Value = ObjRegistroUnicoBE.reun_cViveExteriorDesde;
                }

                prmParameterHeader[26] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[26].Value = ObjRegistroUnicoBE.reun_bPiensaRetornarAlPeru;

                if (ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[28].Value = ObjRegistroUnicoBE.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[29] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[29].Value = ObjRegistroUnicoBE.reun_bAfiliadoAFP;

                prmParameterHeader[30] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = ObjRegistroUnicoBE.reun_bAportaSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[31].Value = ObjRegistroUnicoBE.reun_bBeneficiadoExterior;

                if (ObjRegistroUnicoBE.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[32] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[32].Value = ObjRegistroUnicoBE.reun_sOcupacionPeru;
                }

                if (ObjRegistroUnicoBE.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[33] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[33].Value = ObjRegistroUnicoBE.reun_sOcupacionExtranjero;
                }

                if (ObjRegistroUnicoBE.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[34].Value = ObjRegistroUnicoBE.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[35] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[35].Value = IntOficinaConsularId;

                prmParameterHeader[36] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[36].Value = bGenera58A;

                prmParameterHeader[37] = new SqlParameter("@pers_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = ObjPersBE.pers_sUsuarioCreacion;

                prmParameterHeader[38] = new SqlParameter("@pers_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[38].Value = ObjPersBE.pers_vIPCreacion;

                prmParameterHeader[39] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[39].Value = Util.ObtenerHostName();

                prmParameterHeader[40] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[40].Value = ObjPersBE.pers_bFallecidoFlag;

                prmParameterHeader[41] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                prmParameterHeader[41].Value = ObjPersBE.pers_vSenasParticulares;

                prmParameterHeader[42] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (ObjPersBE.pers_sPaisId == 0)
                {
                    prmParameterHeader[42].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[42].Value = ObjPersBE.pers_sPaisId;
                }

                LonResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ADICIONAR",
                                                           prmParameterHeader);

                #endregion Insertar Persona

                LonPersonaId = (long)prmParameterHeader[0].Value;

                if (LonPersonaId > 0)
                {
                    ObjPersBE.pers_iPersonaId = LonPersonaId;

                    #region Direcciones

                    SqlParameter[] prmParameterDirecciones;

                    prmParameterDirecciones = new SqlParameter[10];

                    prmParameterDirecciones[0] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
                    prmParameterDirecciones[0].Value = objResidencia.resi_sResidenciaTipoId;

                    prmParameterDirecciones[1] = new SqlParameter("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500);
                    prmParameterDirecciones[1].Value = objResidencia.resi_vResidenciaDireccion;

                    prmParameterDirecciones[2] = new SqlParameter("@resi_vCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterDirecciones[2].Value = objResidencia.resi_vCodigoPostal;

                    prmParameterDirecciones[3] = new SqlParameter("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[3].Value = objResidencia.resi_vResidenciaTelefono;

                    prmParameterDirecciones[4] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.Char, 6);
                    prmParameterDirecciones[4].Value = objResidencia.resi_cResidenciaUbigeo;

                    //prmParameterDirecciones[5] = new SqlParameter("@resi_ICentroPobladoId", SqlDbType.Int);
                    //prmParameterDirecciones[5].Value = objResidencia.resi_ICentroPobladoId;

                    prmParameterDirecciones[5] = new SqlParameter("@resi_sOficinaConsularId", SqlDbType.SmallInt);
                    prmParameterDirecciones[5].Value = ObjPersBE.OficinaConsularId;

                    prmParameterDirecciones[6] = new SqlParameter("@resi_sUsuarioCreacion", SqlDbType.SmallInt);
                    prmParameterDirecciones[6].Value = ObjPersBE.pers_sUsuarioCreacion;

                    prmParameterDirecciones[7] = new SqlParameter("@resi_vIPCreacion", SqlDbType.VarChar, 50);
                    prmParameterDirecciones[7].Value = ObjPersBE.pers_vIPCreacion;

                    prmParameterDirecciones[8] = new SqlParameter("@resi_vHostName", SqlDbType.VarChar, 20);
                    prmParameterDirecciones[8].Value = Util.ObtenerHostName();

                    prmParameterDirecciones[9] = new SqlParameter("@resi_iResidenciaId", SqlDbType.BigInt);
                    prmParameterDirecciones[9].Direction = ParameterDirection.Output;

                    IntNumberRowsDet1OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                    CommandType.StoredProcedure,
                                                                    "PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR",
                                                                    prmParameterDirecciones);

                    long LonResidenciaId = (long)prmParameterDirecciones[9].Value;

                    if (LonResidenciaId > 0)
                    {
                        SqlParameter[] prmParameterDetail2;

                        prmParameterDetail2 = new SqlParameter[6];

                        prmParameterDetail2[0] = new SqlParameter("@pere_iPersonaId", SqlDbType.BigInt);
                        prmParameterDetail2[0].Value = ObjPersBE.pers_iPersonaId;

                        prmParameterDetail2[1] = new SqlParameter("@pere_iResidenciaId", SqlDbType.BigInt);
                        prmParameterDetail2[1].Value = LonResidenciaId;

                        prmParameterDetail2[2] = new SqlParameter("@pere_sOficinaConsularId", SqlDbType.SmallInt);
                        prmParameterDetail2[2].Value = IntOficinaConsularId;

                        prmParameterDetail2[3] = new SqlParameter("@pere_sUsuarioCreacion", SqlDbType.SmallInt);
                        prmParameterDetail2[3].Value = ObjPersBE.pers_sUsuarioCreacion;

                        prmParameterDetail2[4] = new SqlParameter("@pere_vIPCreacion", SqlDbType.VarChar, 50);
                        prmParameterDetail2[4].Value = ObjPersBE.pers_vIPCreacion;

                        prmParameterDetail2[5] = new SqlParameter("@pere_vHostName", SqlDbType.VarChar, 20);
                        prmParameterDetail2[5].Value = Util.ObtenerHostName();

                        IntNumberRowsDet2OK = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                        CommandType.StoredProcedure,
                                                                        "PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR",
                                                                        prmParameterDetail2);
                    }

                    #endregion Direcciones
                }
                return LonPersonaId;
            }
            catch (SqlException exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;

            }
            catch (Exception exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;
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
            int IntResultQuery;
            int IntNumberRowsImagenes = 0;
            
            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[48];
                #region PERSONA
                prmParameterHeader[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = ObjPersBE.pers_iPersonaId;

                prmParameterHeader[1] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = ObjPersBE.pers_sPersonaTipoId;

                prmParameterHeader[2] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[2].Value =   ObjPersBE.pers_vApellidoPaterno;

                prmParameterHeader[3] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.NVarChar, 100);
                prmParameterHeader[3].Value =   ObjPersBE.pers_vApellidoMaterno;

                prmParameterHeader[4] = new SqlParameter("@pers_vNombres", SqlDbType.NVarChar, 100);
                prmParameterHeader[4].Value =  ObjPersBE.pers_vNombres;

                /****************************************************************************************************/
                /**************Parametros de ingreso para la tabla PN_REGISTRO.PERSONAIDENTIFICACION*****************/
                /****************************************************************************************************/
                prmParameterHeader[6] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[6].Value = ObjPersIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[7] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[7].Value = ObjPersIdentBE.peid_vDocumentoNumero;
                /****************************************************************************************************/

                if (ObjPersBE.pers_vCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[8] = new SqlParameter("@pers_vCorreoElectronico", SqlDbType.VarChar, 150);
                    prmParameterHeader[8].Value = ObjPersBE.pers_vCorreoElectronico;
                }

                prmParameterHeader[9] = new SqlParameter("@pers_dNacimientoFecha", SqlDbType.DateTime);
                if (ObjPersBE.pers_dNacimientoFecha == null || ObjPersBE.pers_dNacimientoFecha==DateTime.MinValue)
                {                    
                    prmParameterHeader[9].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[9].Value = ObjPersBE.pers_dNacimientoFecha;
                }

                /****************************************************************************************************/
                /***************************Parametro del Lugar de nacimiento o Ubigeo*******************************/
                /****************************************************************************************************/
                if (ObjPersBE.pers_cNacimientoLugar == "0")
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[10].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[10] = new SqlParameter("@pers_cNacimientoLugar", SqlDbType.Char, 6);
                    prmParameterHeader[10].Value = ObjPersBE.pers_cNacimientoLugar;
                }
                /****************************************************************************************************/

                if (ObjPersBE.pers_sGeneroId == 0)
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[11].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[11] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
                    prmParameterHeader[11].Value = ObjPersBE.pers_sGeneroId;
                }

                if (ObjPersBE.pers_vObservaciones.Length == 0)
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[12].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[12] = new SqlParameter("@pers_vObservaciones", SqlDbType.VarChar, 1000);
                    prmParameterHeader[12].Value = ObjPersBE.pers_vObservaciones;
                }

                prmParameterHeader[13] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                prmParameterHeader[13].Value = ObjPersBE.pers_sNacionalidadId;

                if (ObjPersBE.pers_sEstadoCivilId == 0)
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[14] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
                    prmParameterHeader[14].Value = ObjPersBE.pers_sEstadoCivilId;
                }

                if (ObjPersBE.pers_sGradoInstruccionId == 0)
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[15] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
                    prmParameterHeader[15].Value = ObjPersBE.pers_sGradoInstruccionId;
                }

                if (ObjPersBE.pers_sOcupacionId == 0)
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[16] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
                    prmParameterHeader[16].Value = ObjPersBE.pers_sOcupacionId;
                }

                if (ObjPersBE.pers_sProfesionId == 0)
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[17].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[17] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
                    prmParameterHeader[17].Value = ObjPersBE.pers_sProfesionId;
                }

                if (ObjPersBE.pers_vApellidoCasada.Length == 0)
                {
                    prmParameterHeader[18] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[18].Value = string.Empty;
                }
                else
                {
                    prmParameterHeader[18] = new SqlParameter("@pers_vApellidoCasada", SqlDbType.NVarChar, 100);
                    prmParameterHeader[18].Value = ObjPersBE.pers_vApellidoCasada;
                }

                /****************************************************************************************************/
                /*************Parametros de ingreso para la tabla PN_REGISTRO.RE_REGISTROUNICO***********************/
                /****************************************************************************************************/
                //=======Fecha 08/11/2020; Autor: PIPA
                //=======Motivo:Se remplaza el seteo de nulls por cadena vacia; 
                //=======por lo q al borrar un dato del campo del formulario no se actualizaba en la base de datos debido al: [SET reun_vEmergenciaNombre = ISNULL(@reun_vEmergenciaNombre,reun_vEmergenciaNombre)]
                prmParameterHeader[19] = new SqlParameter("@reun_iRegistroUnicoId", SqlDbType.BigInt);
                prmParameterHeader[19].Value = ObjRegistroUnicoBE.reun_iRegistroUnicoId;
                if (ObjRegistroUnicoBE.reun_vEmergenciaNombre.Length == 0)
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[20].Value = "";// DBNull.Value;
                }
                else
                {
                    prmParameterHeader[20] = new SqlParameter("@reun_vEmergenciaNombre", SqlDbType.VarChar, 150);
                    prmParameterHeader[20].Value = ObjRegistroUnicoBE.reun_vEmergenciaNombre;
                }

                if (ObjRegistroUnicoBE.reun_sEmergenciaRelacionId == 0)
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[21].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[21] = new SqlParameter("@reun_sEmergenciaRelacionId", SqlDbType.SmallInt);
                    prmParameterHeader[21].Value = ObjRegistroUnicoBE.reun_sEmergenciaRelacionId;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal.Length == 0)
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[22].Value = "";//DBNull.Value;
                }
                else
                {
                    prmParameterHeader[22] = new SqlParameter("@reun_vEmergenciaDireccionLocal", SqlDbType.VarChar, 500);
                    prmParameterHeader[22].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal.Length == 0)
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[23].Value = "";//DBNull.Value;
                }
                else
                {
                    prmParameterHeader[23] = new SqlParameter("@reun_vEmergenciaCodigoPostal", SqlDbType.VarChar, 10);
                    prmParameterHeader[23].Value = ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaTelefono.Length == 0)
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[24].Value = "";//DBNull.Value;
                }
                else
                {
                    prmParameterHeader[24] = new SqlParameter("@reun_vEmergenciaTelefono", SqlDbType.VarChar, 50);
                    prmParameterHeader[24].Value = ObjRegistroUnicoBE.reun_vEmergenciaTelefono;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru.Length == 0)
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[25].Value = "";//DBNull.Value;
                }
                else
                {
                    prmParameterHeader[25] = new SqlParameter("@reun_vEmergenciaDireccionPeru", SqlDbType.VarChar, 500);
                    prmParameterHeader[25].Value = ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru;
                }

                if (ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico.Length == 0)
                {
                    prmParameterHeader[26] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[26].Value = "";//DBNull.Value;
                }
                else
                {
                    prmParameterHeader[26] = new SqlParameter("@reun_vEmergenciaCorreoElectronico", SqlDbType.VarChar, 500);
                    prmParameterHeader[26].Value = ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico;
                }



                if (ObjRegistroUnicoBE.reun_cViveExteriorDesde == "00")
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[27] = new SqlParameter("@reun_cViveExteriorDesde", SqlDbType.Char, 6);
                    prmParameterHeader[27].Value = ObjRegistroUnicoBE.reun_cViveExteriorDesde;
                }

                prmParameterHeader[28] = new SqlParameter("@reun_bPiensaRetornarAlPeru", SqlDbType.Bit);
                prmParameterHeader[28].Value = ObjRegistroUnicoBE.reun_bPiensaRetornarAlPeru;

                if (ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru == "00")
                {
                    prmParameterHeader[29] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[29].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[29] = new SqlParameter("@reun_cCuandoRetornaAlPeru", SqlDbType.Char, 6);
                    prmParameterHeader[29].Value = ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru;
                }

                prmParameterHeader[30] = new SqlParameter("@reun_bAfiliadoSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[30].Value = ObjRegistroUnicoBE.reun_bAfiliadoSeguroSocial;

                prmParameterHeader[31] = new SqlParameter("@reun_bAfiliadoAFP", SqlDbType.Bit);
                prmParameterHeader[31].Value = ObjRegistroUnicoBE.reun_bAfiliadoAFP;

                prmParameterHeader[32] = new SqlParameter("@reun_bAportaSeguroSocial", SqlDbType.Bit);
                prmParameterHeader[32].Value = ObjRegistroUnicoBE.reun_bAportaSeguroSocial;

                prmParameterHeader[33] = new SqlParameter("@reun_bBeneficiadoExterior", SqlDbType.Bit);
                prmParameterHeader[33].Value = ObjRegistroUnicoBE.reun_bBeneficiadoExterior;

                if (ObjRegistroUnicoBE.reun_sOcupacionPeru == 0)
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[34].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[34] = new SqlParameter("@reun_sOcupacionPeru", SqlDbType.SmallInt);
                    prmParameterHeader[34].Value = ObjRegistroUnicoBE.reun_sOcupacionPeru;
                }

                if (ObjRegistroUnicoBE.reun_sOcupacionExtranjero == 0)
                {
                    prmParameterHeader[35] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[35].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[35] = new SqlParameter("@reun_sOcupacionExtranjero", SqlDbType.SmallInt);
                    prmParameterHeader[35].Value = ObjRegistroUnicoBE.reun_sOcupacionExtranjero;
                }

                if (ObjRegistroUnicoBE.reun_vNombreConvenio.Length == 0)
                {
                    prmParameterHeader[36] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[36].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[36] = new SqlParameter("@reun_vNombreConvenio", SqlDbType.VarChar, 400);
                    prmParameterHeader[36].Value = ObjRegistroUnicoBE.reun_vNombreConvenio;
                }

                /****************************************************************************************************/

                prmParameterHeader[37] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[37].Value = IntOficinaConsularId;

                prmParameterHeader[38] = new SqlParameter("@pers_bGenera58A", SqlDbType.Bit);
                prmParameterHeader[38].Value = bGenera58A;

                prmParameterHeader[39] = new SqlParameter("@pers_sUsuarioModificacion", SqlDbType.Int);
                prmParameterHeader[39].Value = ObjPersBE.pers_sUsuarioModificacion;

                prmParameterHeader[40] = new SqlParameter("@pers_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[40].Value = ObjPersBE.pers_vIPModificacion;

                prmParameterHeader[41] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[41].Value = Util.ObtenerHostName();

                prmParameterHeader[42] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.Bit);
                prmParameterHeader[42].Value = ObjPersBE.pers_bFallecidoFlag;

                prmParameterHeader[43] = new SqlParameter("@actu_sCiudadItinerante", SqlDbType.SmallInt);
                prmParameterHeader[43].Value = CiudadItinerante;

                prmParameterHeader[44] = new SqlParameter("@pers_vSenasParticulares", SqlDbType.VarChar, 250);
                if (ObjPersBE.pers_vSenasParticulares == null)
                {
                    prmParameterHeader[44].Value = "";
                }
                else
                {
                    prmParameterHeader[44].Value = ObjPersBE.pers_vSenasParticulares;
                }
                prmParameterHeader[45] = new SqlParameter("@pers_spaisid", SqlDbType.SmallInt);

                if (ObjPersBE.pers_sPaisId == 0)
                {
                    prmParameterHeader[45].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[45].Value = ObjPersBE.pers_sPaisId;
                }
                prmParameterHeader[46] = new SqlParameter("@peid_vTipoDocumento", SqlDbType.VarChar);

                if (ObjPersIdentBE.peid_vTipodocumento.Length == 0)
                {
                    prmParameterHeader[46].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[46].Value = ObjPersIdentBE.peid_vTipodocumento;
                }

                prmParameterHeader[47] = new SqlParameter("@pers_bPadresPeruanos", SqlDbType.Bit);
                prmParameterHeader[47].Value = ObjPersBE.pers_bPadresPeruanos;

                #endregion
                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR",
                                                           prmParameterHeader);

                IntResultQuery = 1;
                if (IntResultQuery > 0)
                {
                    /****************************************************************************************************/
                    /****************Parametros de ingreso para la tabla PN_REGISTRO.RE_PERSONAFOTO**********************/
                    /****************************************************************************************************/
                    #region PN_REGISTRO.RE_PERSONAFOTO
                    if (IntFlagIngresoFoto == 1)
                    {
                        if (DtImagenes != null)
                        {
                            SqlParameter[] prmParameterImagen;

                            for (int z = 0; z < DtImagenes.Rows.Count; z++)
                            {
                                prmParameterImagen = new SqlParameter[8];

                                prmParameterImagen[0] = new SqlParameter("@pefo_iPersonaFotoId", SqlDbType.BigInt);
                                prmParameterImagen[0].Value = DtImagenes.Rows[z][0];

                                prmParameterImagen[1] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                                prmParameterImagen[1].Value = ObjPersBE.pers_iPersonaId;

                                prmParameterImagen[2] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                                prmParameterImagen[2].Value = DtImagenes.Rows[z][2];

                                prmParameterImagen[3] = new SqlParameter("@pefo_GFoto", SqlDbType.Image);
                                prmParameterImagen[3].Value = DtImagenes.Rows[z][3];

                                prmParameterImagen[4] = new SqlParameter("@pefo_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterImagen[4].Value = IntOficinaConsularId;

                                prmParameterImagen[5] = new SqlParameter("@pefo_sUsuarioModificacion", SqlDbType.SmallInt);
                                prmParameterImagen[5].Value = ObjPersBE.pers_sUsuarioModificacion;

                                prmParameterImagen[6] = new SqlParameter("@pefo_vIPModificacion", SqlDbType.VarChar, 50);
                                prmParameterImagen[6].Value = ObjPersBE.pers_vIPModificacion;

                                prmParameterImagen[7] = new SqlParameter("@pefo_vHostName", SqlDbType.VarChar, 20);
                                prmParameterImagen[7].Value = Util.ObtenerHostName();

                                IntNumberRowsImagenes = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                              CommandType.StoredProcedure,
                                                                              "PN_REGISTRO.USP_RE_PERSONAFOTO_ACTUALIZAR",
                                                                              prmParameterImagen);
                            }
                        }
                    }
                    #endregion

                    #region FILIACION
                    if (DtFiliaciones != null) {
                        foreach (DataRow row in DtFiliaciones.Rows)
                        {
                            //SqlParameter[] prm = new SqlParameter[12];
                            if (Convert.ToInt64(row["pefi_iPersonaFilacionId"]) == 0)
                            {
                                #region Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)

                                SqlParameter[] prmParameterFilaciones;
                                prmParameterFilaciones = new SqlParameter[14];

                                prmParameterFilaciones[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                                prmParameterFilaciones[0].Direction = ParameterDirection.Output;

                                prmParameterFilaciones[1] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                                prmParameterFilaciones[1].Value = ObjPersBE.pers_iPersonaId;

                                ////////////////////////////////

                                prmParameterFilaciones[2] = new SqlParameter("@pefi_iFiliadoId", SqlDbType.BigInt);
                                prmParameterFilaciones[2].Value = row[15];

                                prmParameterFilaciones[3] = new SqlParameter("@pefi_sDocumentoTipoId", SqlDbType.SmallInt);
                                prmParameterFilaciones[3].Value = row[14];

                                ////////////////////////////////

                                prmParameterFilaciones[4] = new SqlParameter("@pefi_vNombreFiliacion", SqlDbType.VarChar, 500);
                                prmParameterFilaciones[4].Value = row[1];

                                prmParameterFilaciones[5] = new SqlParameter("@pefi_vLugarNacimiento", SqlDbType.VarChar, 500);
                                prmParameterFilaciones[5].Value = row[2];

                                prmParameterFilaciones[6] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.DateTime);
                                prmParameterFilaciones[6].Value = row[3];

                                prmParameterFilaciones[7] = new SqlParameter("@pefi_sNacionalidad", SqlDbType.SmallInt);
                                prmParameterFilaciones[7].Value = row[13];

                                prmParameterFilaciones[8] = new SqlParameter("@pefi_sTipoFilacionId", SqlDbType.SmallInt);
                                prmParameterFilaciones[8].Value = row[6];

                                prmParameterFilaciones[9] = new SqlParameter("@pefi_vNroDocumento", SqlDbType.VarChar, 20);
                                prmParameterFilaciones[9].Value = row[8];

                                prmParameterFilaciones[10] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                                prmParameterFilaciones[10].Value = IntOficinaConsularId;

                                prmParameterFilaciones[11] = new SqlParameter("@pefi_sUsuarioCreacion", SqlDbType.SmallInt);
                                prmParameterFilaciones[11].Value = ObjPersBE.pers_sUsuarioModificacion;

                                prmParameterFilaciones[12] = new SqlParameter("@pefi_vIPCreacion", SqlDbType.VarChar, 50);
                                prmParameterFilaciones[12].Value = ObjPersBE.pers_vIPModificacion;

                                prmParameterFilaciones[13] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                                prmParameterFilaciones[13].Value = Util.ObtenerHostName();

                                int IntNumberRowsFilaciones = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                                                    CommandType.StoredProcedure,
                                                                                    "PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR",
                                                                                    prmParameterFilaciones);


                                #endregion Registro Filiación (PN_REGISTRO.RE_PERSONA_FILIACION)
                            }
                            else {
                                if (Convert.ToString(row["pefi_cEstado"]) == "E") {
                                    SqlParameter[] prm;
                                    prm = new SqlParameter[5];

                                    prm[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                                    prm[0].Value = row["pefi_iPersonaFilacionId"];

                                    prm[1] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                                    prm[1].Value = IntOficinaConsularId;

                                    prm[2] = new SqlParameter("@pefi_sUsuarioModificacion", SqlDbType.SmallInt);
                                    prm[2].Value = ObjPersBE.pers_sUsuarioModificacion;

                                    prm[3] = new SqlParameter("@pefi_vIPModificacion", SqlDbType.VarChar, 50);
                                    prm[3].Value = ObjPersBE.pers_vIPModificacion;

                                    prm[4] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                                    prm[4].Value = Util.ObtenerHostName();

                                    IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_REGISTRO.USP_RE_PERSONAFILIACION_ELIMINAR",
                                         prm);
                                }
                            }
                        }
                    }
                    #endregion
                }

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;

            }
            catch (Exception exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;
            }
        }

        public int Eliminar(BE.MRE.RE_PERSONA ObjPersBE,
                            long IntRegUnico,
                            int IntOficinaConsularId)
        {
            int IntResultQuery;
            try
            {
                SqlParameter[] prmParameter = new SqlParameter[6];

                prmParameter[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.Int);
                prmParameter[0].Value = ObjPersBE.pers_iPersonaId;

                prmParameter[1] = new SqlParameter("@reun_iRegistroUnicoId", SqlDbType.BigInt);
                prmParameter[1].Value = IntRegUnico;

                prmParameter[2] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[2].Value = IntOficinaConsularId;

                prmParameter[3] = new SqlParameter("@pers_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[3].Value = ObjPersBE.pers_sUsuarioModificacion;

                prmParameter[4] = new SqlParameter("@pers_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[4].Value = ObjPersBE.pers_vIPModificacion;

                prmParameter[5] = new SqlParameter("@pers_vHostName", SqlDbType.VarChar, 20);
                prmParameter[5].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ELIMINAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (SqlException exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;

            }
            catch (Exception exec)
            {

                ObjPersBE.Error = true;
                ObjPersBE.Message = exec.StackTrace.ToString();
                return -1;
            }
        }

        // **************************************************************************
        // FUNCION NO UTILIZADA POR ACTO JUDICIAL SE PASO A SGAC.DA.MRE.RE_PERSONA_DA
        // **************************************************************************
        public bool InsertarRuneRapido(ref DataTable dtParticipantes, Int16 sOficinaConsularId, string vHostName, Int16 intUsuarioCreacionId, string vIPCreacion)
        {
            bool booOk = false;
            long LonResultQueryActuacion = 0;
            int intFila = 0;
            try
            {
                SqlParameter[] prmParameterActuacion = new SqlParameter[12];

                for (intFila = 0; intFila <= dtParticipantes.Rows.Count - 1; intFila++)
                {
                    if (dtParticipantes.Rows[intFila]["ajpa_sTipoPersonaId"].ToString() == "2101")
                    {
                        if (Convert.ToInt64(dtParticipantes.Rows[intFila]["ajpa_iPersonaId"].ToString()) == 0)
                        {
                            prmParameterActuacion[0] = new SqlParameter("@pers_vApellidoPaterno", SqlDbType.VarChar, 100);
                            prmParameterActuacion[0].Value = dtParticipantes.Rows[intFila]["ajpa_vApePaterno"].ToString();

                            prmParameterActuacion[1] = new SqlParameter("@pers_vApellidoMaterno", SqlDbType.VarChar, 100);
                            prmParameterActuacion[1].Value = dtParticipantes.Rows[intFila]["ajpa_vApeMaterno"].ToString();

                            prmParameterActuacion[2] = new SqlParameter("@pers_vNombres", SqlDbType.VarChar, 100);
                            prmParameterActuacion[2].Value = dtParticipantes.Rows[intFila]["ajpa_vNombre"].ToString();

                            prmParameterActuacion[3] = new SqlParameter("@pers_sPersonaTipoId", SqlDbType.SmallInt);
                            prmParameterActuacion[3].Value = dtParticipantes.Rows[intFila]["ajpa_sTipoPersonaId"].ToString();

                            prmParameterActuacion[4] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
                            prmParameterActuacion[4].Value = "2051";

                            prmParameterActuacion[5] = new SqlParameter("@rele_sUsuarioCreacion", SqlDbType.SmallInt);
                            prmParameterActuacion[5].Value = intUsuarioCreacionId;

                            prmParameterActuacion[6] = new SqlParameter("@rele_vIPCreacion", SqlDbType.VarChar, 100);
                            prmParameterActuacion[6].Value = vIPCreacion;

                            prmParameterActuacion[7] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                            prmParameterActuacion[7].Value = dtParticipantes.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString();

                            prmParameterActuacion[8] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                            prmParameterActuacion[8].Value = dtParticipantes.Rows[intFila]["ajpa_vDocumentoNumero"].ToString();

                            prmParameterActuacion[9] = new SqlParameter("@peid_vHostName", SqlDbType.VarChar, 50);
                            prmParameterActuacion[9].Value = vHostName;

                            prmParameterActuacion[10] = new SqlParameter("@rele_sOficinaConsularId", SqlDbType.SmallInt);
                            prmParameterActuacion[10].Value = sOficinaConsularId;

                            prmParameterActuacion[11] = new SqlParameter("@pers_iPersonaId", SqlDbType.SmallInt);
                            prmParameterActuacion[11].Direction = ParameterDirection.Output;

                            LonResultQueryActuacion = SqlHelper.ExecuteNonQuery(StrConnectionName, CommandType.StoredProcedure, "PN_REGISTRO.USP_RE_PERSONA_ADICIONAR_RUNE_RAPIDO", prmParameterActuacion);

                            // ACTUALIZAMOS EL ID DEL PARTICIPANTE EN EL DATATABLE TEMPORAL
                            Int64 iPersonaId = Convert.ToInt64(prmParameterActuacion[11].Value);
                            dtParticipantes.Rows[intFila]["ajpa_iPersonaId"] = Convert.ToInt64(prmParameterActuacion[11].Value);

                            if (iPersonaId != 0)
                            {
                                booOk = true;
                            }
                        }
                    }
                }
                booOk = true;
            }
            catch (Exception ex)
            {
                booOk = false;
                throw ex;
            }
            return booOk;
        }



        // RUNE RÁPIDO
        public Int64 InsertarPersona(BE.MRE.RE_PERSONA objPersona, BE.MRE.RE_PERSONAIDENTIFICACION objIdentificacion, BE.MRE.RE_REGISTROUNICO objRegistroUnico, 
            Int16 intOficinaConsularId, bool bolGenera58A)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", objPersona.pers_sPersonaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", objPersona.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", objPersona.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", objPersona.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", objIdentificacion.peid_sDocumentoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", objIdentificacion.peid_vDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@pers_vCorreoElectronico", objPersona.pers_vCorreoElectronico)); // DBNull.Value
                        cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", objPersona.pers_dNacimientoFecha));
                        cmd.Parameters.Add(new SqlParameter("@pers_cNacimientoLugar", objPersona.pers_cNacimientoLugar));
                        cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", objPersona.pers_sGeneroId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vObservaciones", objPersona.pers_vObservaciones));
                        cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", objPersona.pers_sNacionalidadId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", objPersona.pers_sEstadoCivilId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sGradoInstruccionId", objPersona.pers_sGradoInstruccionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", objPersona.pers_sOcupacionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", objPersona.pers_sProfesionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", objPersona.pers_vApellidoCasada));
                        cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", objPersona.pers_sProfesionId));

                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaNombre", objRegistroUnico.reun_vEmergenciaNombre));
                        cmd.Parameters.Add(new SqlParameter("@reun_sEmergenciaRelacionId", objRegistroUnico.reun_sEmergenciaRelacionId));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaDireccionLocal", objRegistroUnico.reun_vEmergenciaDireccionLocal));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaCodigoPostal", objRegistroUnico.reun_vEmergenciaCodigoPostal));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaTelefono", objRegistroUnico.reun_vEmergenciaTelefono));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaDireccionPeru", objRegistroUnico.reun_vEmergenciaDireccionPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaCorreoElectronico", objRegistroUnico.reun_vEmergenciaCorreoElectronico));
                        cmd.Parameters.Add(new SqlParameter("@reun_cViveExteriorDesde", objRegistroUnico.reun_cViveExteriorDesde));
                        cmd.Parameters.Add(new SqlParameter("@reun_vEmergenciaNombre", objRegistroUnico.reun_vEmergenciaNombre));
                        cmd.Parameters.Add(new SqlParameter("@reun_cViveExteriorDesde", objRegistroUnico.reun_cViveExteriorDesde));
                        cmd.Parameters.Add(new SqlParameter("@reun_bPiensaRetornarAlPeru", objRegistroUnico.reun_bPiensaRetornarAlPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_cCuandoRetornaAlPeru", objRegistroUnico.reun_cCuandoRetornaAlPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAfiliadoSeguroSocial", objRegistroUnico.reun_bAfiliadoSeguroSocial));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAfiliadoAFP", objRegistroUnico.reun_bAfiliadoAFP));
                        cmd.Parameters.Add(new SqlParameter("@reun_bAportaSeguroSocial", objRegistroUnico.reun_bAportaSeguroSocial));
                        cmd.Parameters.Add(new SqlParameter("@reun_bBeneficiadoExterior", objRegistroUnico.reun_bBeneficiadoExterior));
                        cmd.Parameters.Add(new SqlParameter("@reun_sOcupacionPeru", objRegistroUnico.reun_sOcupacionPeru));
                        cmd.Parameters.Add(new SqlParameter("@reun_sOcupacionExtranjero", objRegistroUnico.reun_sOcupacionExtranjero));
                        cmd.Parameters.Add(new SqlParameter("@reun_vNombreConvenio", objRegistroUnico.reun_vNombreConvenio));

                        cmd.Parameters.Add(new SqlParameter("@pers_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@pers_bGenera58A", bolGenera58A));
                        cmd.Parameters.Add(new SqlParameter("@pers_sUsuarioCreacion", objPersona.pers_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vIPCreacion", objPersona.pers_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@pers_bFallecidoFlag", objPersona.pers_bFallecidoFlag));
                        cmd.Parameters.Add(new SqlParameter("@pers_vSenasParticulares", objPersona.pers_vSenasParticulares));

                        if (objPersona.pers_sPaisId == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", DBNull.Value));
                        }
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", objPersona.pers_sPaisId));
                        }

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objPersona.pers_iPersonaId = Convert.ToInt64(lReturn.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                return -1;
            }
            return objPersona.pers_iPersonaId;
        }


        public int ActualizarPorCodigo(Int64 PersonaId,
                            int ConsuladoId,
                            int intUsuarioModifica,
                            bool bFacellido)
        {
            try
            {
                SqlParameter[] prmParameter = new SqlParameter[6];

                prmParameter[0] = new SqlParameter("@pers_iPersonaId", SqlDbType.Int);
                prmParameter[0].Value = PersonaId;

                prmParameter[1] = new SqlParameter("@pers_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[1].Value = ConsuladoId;

                prmParameter[3] = new SqlParameter("@pers_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[3].Value = intUsuarioModifica;

                prmParameter[4] = new SqlParameter("@pers_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[4].Value = Util.ObtenerHostName();

                prmParameter[5] = new SqlParameter("@pers_bFallecidoFlag", SqlDbType.VarChar, 20);
                prmParameter[5].Value = bFacellido;

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR_POR_CODIGO",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch 
            {
                return -1;
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
            try
            {
                SqlParameter[] prmParameter = new SqlParameter[7];

                prmParameter[0] = new SqlParameter("@P_pers_iPersonaId", SqlDbType.Int);
                prmParameter[0].Value = PersonaId;

                prmParameter[1] = new SqlParameter("@P_pena_sPais", SqlDbType.SmallInt);
                prmParameter[1].Value = PaisId;

                prmParameter[2] = new SqlParameter("@P_pena_vNacionalidad", SqlDbType.VarChar, 100);
                prmParameter[2].Value = vNacionalidad;

                prmParameter[3] = new SqlParameter("@P_pena_bVigente", SqlDbType.Bit);
                prmParameter[3].Value = bVigente;

                prmParameter[4] = new SqlParameter("@P_pena_cEstado", SqlDbType.VarChar, 1);
                prmParameter[4].Value = cEstado;

                prmParameter[5] = new SqlParameter("@P_pena_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameter[5].Value = intUsuarioModifica;

                prmParameter[6] = new SqlParameter("@P_pena_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[6].Value = Util.ObtenerHostName();

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.RE_PERSONANACIONALIDAD_ADICIONAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch
            {
                return -1;
            }
        }
        public int EliminarNacionalidad(Int64 PersonaId,
                            Int16 PaisId,
                            Int16 intUsuarioModifica
                            )
        {
            try
            {
                SqlParameter[] prmParameter = new SqlParameter[4];

                prmParameter[0] = new SqlParameter("@P_pers_iPersonaId", SqlDbType.Int);
                prmParameter[0].Value = PersonaId;

                prmParameter[1] = new SqlParameter("@P_pena_sPais", SqlDbType.SmallInt);
                prmParameter[1].Value = PaisId;

                prmParameter[2] = new SqlParameter("@P_pena_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameter[2].Value = intUsuarioModifica;

                prmParameter[3] = new SqlParameter("@P_pena_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[3].Value = Util.ObtenerHostName();

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.[RE_PERSONANACIONALIDAD_ELIMINAR]",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch
            {
                return -1;
            }
        }
    }
}
