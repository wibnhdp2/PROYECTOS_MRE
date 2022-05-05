using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;
using System.Configuration;
using System.Reflection;
using SGAC.Accesorios;

namespace SGAC.DA.MRE
{
    public class RE_PERSONA_DA
    {

        public string strError = string.Empty;

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public string actualizar_persona(RE_PERSONA persona)
        {
            persona.Message = string.Empty;
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


                        cmd.Parameters.Add(new SqlParameter("@pers_bIncapacidadFlag", persona.pers_bIncapacidadFlag));

                        if (!string.IsNullOrEmpty(persona.pers_vDescripcionIncapacidad))
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", persona.pers_vDescripcionIncapacidad));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", string.Empty));
                        }


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

                        if (!string.IsNullOrEmpty(persona.pers_cUbigeoDefuncion))
                        {
                            if (persona.pers_cUbigeoDefuncion.Trim() != string.Empty)
                            {
                                cmd.Parameters.Add(new SqlParameter("@pers_cUbigeoDefuncion", persona.pers_cUbigeoDefuncion));
                            }
                        }

                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", persona.pers_vApellidoCasada));
                        cmd.Parameters.Add(new SqlParameter("@pers_bGenera58A", persona.bGenera58A));
                        cmd.Parameters.Add(new SqlParameter("@pers_sUsuarioModificacion", persona.pers_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@pers_vIPModificacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@pers_sOficinaConsularId", persona.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vHostName", Util.ObtenerHostName()));

                        //--------------------------------------------------------------------
                        //Fecha: 17/04/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar el pais de origen
                        //--------------------------------------------------------------------
                        if (persona.pers_sPaisId != null && persona.pers_sPaisId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", persona.pers_sPaisId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", DBNull.Value));
                        }

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        persona.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                persona.Error = true;
                persona.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                persona.Error = true;
                persona.Message = exec.Message.ToString();
            }

            return persona.Message;
        }
        public Int64 insertar_residencia(RE_RESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@resi_sResidenciaTipoId", residencia.resi_sResidenciaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaDireccion", residencia.resi_vResidenciaDireccion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", residencia.resi_vCodigoPostal));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaTelefono", residencia.resi_vResidenciaTelefono));
                        cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo", residencia.resi_cResidenciaUbigeo));
                        if (residencia.resi_ICentroPobladoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@resi_ICentroPobladoId", residencia.resi_ICentroPobladoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_sUsuarioCreacion", residencia.resi_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", residencia.resi_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@resi_sOficinaConsularId", residencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@resi_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@resi_iResidenciaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        residencia.resi_iResidenciaId = Convert.ToInt64(lReturn.Value);
                        residencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }
            return residencia.resi_iResidenciaId;
        }

        public Int64 actualizar_residencia(RE_RESIDENCIA residencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_RESIDENCIA_ACTUALIZAR_FILIACION]", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@resi_sResidenciaTipoId", residencia.resi_sResidenciaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaDireccion", residencia.resi_vResidenciaDireccion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", residencia.resi_vCodigoPostal));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaTelefono", residencia.resi_vResidenciaTelefono));
                        cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo", residencia.resi_cResidenciaUbigeo));
                        if (residencia.resi_ICentroPobladoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@resi_ICentroPobladoId", residencia.resi_ICentroPobladoId));
                        cmd.Parameters.Add(new SqlParameter("@resi_sUsuarioCreacion", residencia.resi_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", residencia.resi_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@resi_sOficinaConsularId", residencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@resi_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId", residencia.resi_iResidenciaId));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                       
                        residencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                residencia.Error = true;
                residencia.Message = exec.Message.ToString();
            }
            return residencia.resi_iResidenciaId;
        }
        
        public void insertar_residencia_persona(RE_PERSONARESIDENCIA personaResidencia)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONARESIDENCIA_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pere_iPersonaId", personaResidencia.pere_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@pere_iResidenciaId", personaResidencia.pere_iResidenciaId));
                        cmd.Parameters.Add(new SqlParameter("@pere_sUsuarioCreacion", personaResidencia.pere_sUsuarioCreacion));
                        //cmd.Parameters.Add(new SqlParameter("@pere_vIPCreacion", personaResidencia.pere_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pere_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@pere_sOficinaConsularId", personaResidencia.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pere_vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        personaResidencia.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                personaResidencia.Error = true;
                personaResidencia.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                personaResidencia.Error = true;
                personaResidencia.Message = exec.Message.ToString();
            }
        }
        /*Fin*/

        public RE_PERSONA insertar_minirune(RE_PERSONA Participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ADICIONAR_RUNE_RAPIDO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", Participante.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", Participante.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", Participante.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", Participante.pers_sPersonaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", Participante.pers_sNacionalidadId));

                        if(Participante.pers_sProfesionId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", Participante.pers_sProfesionId));

                        if (Participante.pers_sEstadoCivilId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", Participante.pers_sEstadoCivilId));

                        if (Participante.pers_sGeneroId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", Participante.pers_sGeneroId));

                        if (Participante.pers_dNacimientoFecha != null && Participante.pers_dNacimientoFecha != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", Participante.pers_dNacimientoFecha));
                        
                        if (Participante.pers_sIdiomaNatalId >0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sIdiomaNatalId", Participante.pers_sIdiomaNatalId));


                        cmd.Parameters.Add(new SqlParameter("@pers_bIncapacidadFlag", Participante.pers_bIncapacidadFlag));

                        if (!string.IsNullOrEmpty(Participante.pers_vDescripcionIncapacidad))
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", Participante.pers_vDescripcionIncapacidad));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", string.Empty));
                        }

                        cmd.Parameters.Add(new SqlParameter("@rele_sUsuarioCreacion", Participante.pers_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@rele_vIPCreacion", Participante.pers_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", Participante.Identificacion.peid_sDocumentoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", Participante.Identificacion.peid_vDocumentoNumero));
                        //if (Participante.HostName != null && Participante.HostName != string.Empty)
                        //    cmd.Parameters.Add(new SqlParameter("@peid_vHostName", Participante.HostName));
                        //else
                        cmd.Parameters.Add(new SqlParameter("@peid_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@rele_sOficinaConsularId", Participante.OficinaConsultar));

                        if (Participante.pers_sOcupacionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", Participante.pers_sOcupacionId));

                        //--------------------------------------------------------------------
                        //Fecha: 22/03/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar la descripción del Tipo de Documento Otros
                        //--------------------------------------------------------------------
                        if (Participante.Identificacion.peid_vTipodocumento != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vTipoDocumento", Participante.Identificacion.peid_vTipodocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vTipoDocumento", string.Empty));
                        }

                        //--------------------------------------------------------------------
                        //Fecha: 22/03/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar el pais de origen
                        //--------------------------------------------------------------------
                        if (Participante.pers_sPaisId != null && Participante.pers_sPaisId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", Participante.pers_sPaisId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", DBNull.Value));
                        }

                        //--------------------------------------------------------------------
                        //Fecha: 13/12/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar el Apellido de Casada
                        //--------------------------------------------------------------------
                        if (Participante.pers_vApellidoCasada != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", Participante.pers_vApellidoCasada));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", string.Empty));
                        }
                        //--------------------------------------------------------------------
                        if (Participante.Residencias.Count > 0)
                        {
                            if (Participante.Residencias[0].Residencia.resi_vResidenciaDireccion != null)
                                cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaDireccion", Participante.Residencias[0].Residencia.resi_vResidenciaDireccion));
                            else
                                cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaDireccion", string.Empty));
                            
                            if(Participante.Residencias[0].Residencia.resi_vResidenciaTelefono!=null)
                                cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaTelefono", Participante.Residencias[0].Residencia.resi_vResidenciaTelefono));
                            else
                                cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaTelefono", string.Empty));

                            if (Participante.Residencias[0].Residencia.resi_cResidenciaUbigeo != null)
                            {
                                if (Participante.Residencias[0].Residencia.resi_cResidenciaUbigeo.Trim().Length == 6)
                                {
                                    cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", Participante.Residencias[0].Residencia.resi_cResidenciaUbigeo));
                                }
                                else
                                { cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", DBNull.Value)); }
                            }
                            else
                            { cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", DBNull.Value)); }

                            if (Participante.Residencias[0].Residencia.resi_vCodigoPostal != null)
                                cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", Participante.Residencias[0].Residencia.resi_vCodigoPostal));
                            else
                                cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", string.Empty));
                        }                                                                       
                        //---------------------------------------

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Participante.pers_iPersonaId= Convert.ToInt64(lReturn.Value);
                        Participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                Participante.Error = true;
                Participante.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                Participante.Error = true;
                Participante.Message = exec.Message.ToString();
            }
            return Participante;            
        }

        public RE_PERSONA actualizar_minirune(RE_PERSONA Participante)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR_RUNE_RAPIDO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", Participante.pers_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", Participante.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", Participante.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", Participante.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", Participante.pers_sPersonaTipoId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", Participante.pers_sNacionalidadId));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", Participante.Identificacion.peid_sDocumentoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", Participante.Identificacion.peid_vDocumentoNumero));
                        if (Participante.pers_sProfesionId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", Participante.pers_sProfesionId));

                        if (Participante.pers_sEstadoCivilId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", Participante.pers_sEstadoCivilId));

                        if (Participante.pers_sGeneroId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", Participante.pers_sGeneroId));

                        if (Participante.pers_sIdiomaNatalId > 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sIdiomaNatalId", Participante.pers_sIdiomaNatalId));

                        cmd.Parameters.Add(new SqlParameter("@pers_bIncapacidadFlag", Participante.pers_bIncapacidadFlag));

                        if (!string.IsNullOrEmpty(Participante.pers_vDescripcionIncapacidad))
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", Participante.pers_vDescripcionIncapacidad));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vDescripcionIncapacidad", string.Empty));
                        }

                        if (Participante.pers_dNacimientoFecha != null)
                        {
                            if (Participante.pers_dNacimientoFecha != DateTime.MinValue)
                                cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", Participante.pers_dNacimientoFecha));
                        }

                        cmd.Parameters.Add(new SqlParameter("@rele_sUsuarioModificacion", Participante.pers_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@rele_vIPModificacion", Participante.pers_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@peid_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@rele_sOficinaConsularId", Participante.OficinaConsultar));

                        if (Participante.pers_sOcupacionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", Participante.pers_sOcupacionId));

                        //--------------------------------------------------------------------
                        //Fecha: 22/03/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar el pais de origen
                        //--------------------------------------------------------------------
                        if (Participante.pers_sPaisId != null && Participante.pers_sPaisId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", Participante.pers_sPaisId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_spaisid", DBNull.Value));
                        }
                        //--------------------------------------------------------------------
                        //Fecha: 13/12/2017
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Registrar el Apellido de Casada
                        //--------------------------------------------------------------------
                        if (Participante.pers_vApellidoCasada != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", Participante.pers_vApellidoCasada));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoCasada", string.Empty));
                        }

                        if (Participante.Identificacion.peid_vTipodocumento != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vTipoDocumento", Participante.Identificacion.peid_vTipodocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vTipoDocumento", string.Empty));
                        }
                        //--------------------------------------------------------------------
                        //if (Participante.bIndicadorFormaRegistro)
                        //{

                        //}
                        //else
                        //{
                            if (Participante.Residencias.Count > 0)
                            {
                                int indice = Participante.Residencias.Count - 1;
                                if (Participante.Residencias[indice].Residencia.resi_iResidenciaId > 0)
                                    cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId", Participante.Residencias[indice].Residencia.resi_iResidenciaId));

                                if (Participante.Residencias[indice].Residencia.resi_vResidenciaDireccion != null)
                                    cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaDireccion", Participante.Residencias[indice].Residencia.resi_vResidenciaDireccion));
                                else
                                    cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaDireccion", string.Empty));

                                if (Participante.Residencias[indice].Residencia.resi_vResidenciaTelefono != null)
                                    cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaTelefono", Participante.Residencias[indice].Residencia.resi_vResidenciaTelefono));
                                else
                                    cmd.Parameters.Add(new SqlParameter("@pers_vResidenciaTelefono", string.Empty));

                                if (Participante.Residencias[indice].Residencia.resi_cResidenciaUbigeo != null)
                                {
                                    if (Participante.Residencias[indice].Residencia.resi_cResidenciaUbigeo.Trim().Length == 6)
                                    {

                                        cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", Participante.Residencias[indice].Residencia.resi_cResidenciaUbigeo));
                                    }
                                    else
                                    {
                                        cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", DBNull.Value));
                                    }
                                }
                                else
                                { cmd.Parameters.Add(new SqlParameter("@pers_cResidenciaUbigeo", DBNull.Value)); }

                                if (Participante.Residencias[indice].Residencia.resi_vCodigoPostal != null)
                                { cmd.Parameters.Add(new SqlParameter("@resi_vCodigoPostal", Participante.Residencias[indice].Residencia.resi_vCodigoPostal)); }
                            }
                        //}
                        
                       

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Participante.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                Participante.Error = true;
                Participante.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                Participante.Error = true;
                Participante.Message = exec.Message.ToString();
            }
            return Participante;
        }

        public RE_PERSONA actualizar(RE_PERSONA Persona) { return null;  }

        public RE_PERSONA obtener(RE_PERSONA Persona) {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_BUSCAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        if (Persona.pers_iPersonaId !=0 ) cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", Persona.pers_iPersonaId));
                        if (Persona.pers_sEstadoCivilId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", Persona.pers_sEstadoCivilId));
                        if (Persona.pers_sProfesionId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sProfesionId", Persona.pers_sProfesionId));
                        if (Persona.pers_sOcupacionId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", Persona.pers_sOcupacionId));
                        if (Persona.pers_sIdiomaNatalId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sIdiomaNatalId", Persona.pers_sIdiomaNatalId));
                        if (Persona.pers_sGeneroId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", Persona.pers_sGeneroId));
                        if (Persona.pers_sGradoInstruccionId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sGradoInstruccionId", Persona.pers_sGradoInstruccionId));
                        if (Persona.pers_vApellidoPaterno != null) cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", Persona.pers_vApellidoPaterno));
                        if (Persona.pers_vApellidoMaterno != null) cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", Persona.pers_vApellidoMaterno));
                        if (Persona.pers_vNombres != null) cmd.Parameters.Add(new SqlParameter("@pers_vNombres", Persona.pers_vNombres));
                        if (Persona.pers_sPersonaTipoId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", Persona.pers_sPersonaTipoId));
                        if (Persona.pers_vObservaciones != null) cmd.Parameters.Add(new SqlParameter("@pers_vObservaciones", Persona.pers_vObservaciones));
                        if (Persona.pers_sNacionalidadId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", Persona.pers_sNacionalidadId));
                        if (Persona.pers_sColorTezId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sColorTezId", Persona.pers_sColorTezId));
                        if (Persona.pers_sColorOjosId!= 0) cmd.Parameters.Add(new SqlParameter("@pers_sColorOjosId", Persona.pers_sColorOjosId));
                        if (Persona.pers_sColorCabelloId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sColorCabelloId", Persona.pers_sColorCabelloId));
                        if (Persona.pers_sGrupoSanguineoId != 0) cmd.Parameters.Add(new SqlParameter("@pers_sGrupoSanguineoId", Persona.pers_sGrupoSanguineoId));                        
                        if (Persona.pers_IOcurrenciaCentroPobladoId != 0) cmd.Parameters.Add(new SqlParameter("@pers_IOcurrenciaCentroPobladoId", Persona.pers_IOcurrenciaCentroPobladoId));
                        if (Persona.Identificacion.peid_sDocumentoTipoId != 0) cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", Persona.Identificacion.peid_sDocumentoTipoId));
                        if (Persona.Identificacion.peid_vDocumentoNumero != null) cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", Persona.Identificacion.peid_vDocumentoNumero));

                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                #region P E R S O N A
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)Persona.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(Persona, loReader[col], null);
                                        }
                                    }
                                }
                                #endregion

                                #region I D E N T I F I C A C I O N
                                RE_PERSONAIDENTIFICACION lRE_PERSONAIDENTIFICACION = new RE_PERSONAIDENTIFICACION();
                                RE_PERSONAIDENTIFICACION_DA lRE_PERSONAIDENTIFICACION_DA = new RE_PERSONAIDENTIFICACION_DA();
                                lRE_PERSONAIDENTIFICACION.peid_iPersonaId = Persona.pers_iPersonaId;

                                //----------------------------------------------------------------------------
                                //Fecha: 14/03/2022
                                //Autor: Asignar el tipo de documento y el número de documento existente.
                                //----------------------------------------------------------------------------
                                if (Persona.Identificacion.peid_sDocumentoTipoId != 0 && Persona.Identificacion.peid_vDocumentoNumero != null)
                                {
                                    lRE_PERSONAIDENTIFICACION.peid_sDocumentoTipoId = Persona.Identificacion.peid_sDocumentoTipoId;
                                    lRE_PERSONAIDENTIFICACION.peid_vDocumentoNumero = Persona.Identificacion.peid_vDocumentoNumero;
                                }
                                //----------------------------------------------------------------------------

                                Persona.Identificacion = lRE_PERSONAIDENTIFICACION_DA.obtener(lRE_PERSONAIDENTIFICACION);
                                #endregion

                                #region R E S I D E N C I A S
                                RE_PERSONARESIDENCIA lPERSONARESIDENCIA = new RE_PERSONARESIDENCIA();
                                lPERSONARESIDENCIA.pere_iPersonaId = Persona.pers_iPersonaId;

                                RE_PERSONARESIDENCIA_DA lRESIDENCIA_DA = new RE_PERSONARESIDENCIA_DA();
                                Persona.Residencias = lRESIDENCIA_DA.listado(lPERSONARESIDENCIA);

                                if (Persona.Residencias.Count>0)
                                {
                                    var lqResidencia =(from resi in Persona.Residencias
                                                       where resi.pere_iResidenciaId == Persona.Residencias.Max(top => top.pere_iResidenciaId)
                                                       select resi.Residencia);

                                    var result = (from item in lqResidencia
                                                  select new RE_RESIDENCIA{
                                                    resi_iResidenciaId = item.resi_iResidenciaId,
                                                    resi_sResidenciaTipoId = item.resi_sResidenciaTipoId,
                                                    resi_vResidenciaDireccion = item.resi_vResidenciaDireccion,
                                                    resi_vCodigoPostal = item.resi_vCodigoPostal,
                                                    resi_vResidenciaTelefono = item.resi_vResidenciaTelefono,
                                                    resi_cResidenciaUbigeo = item.resi_cResidenciaUbigeo,
                                                    resi_ICentroPobladoId = item.resi_ICentroPobladoId
                                                  }).ToList();

                                    Persona._ResidenciaTop = (RE_RESIDENCIA)result[0];
                                }
                                #endregion
                            }
                        }
                        Persona.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                strError = exec.Message;
                Persona.Error = true;
                Persona.Message = exec.Message.ToString();
            }

            catch (Exception exec)
            {
                strError = exec.Message;
                Persona.Error = true;
                Persona.Message = exec.Message.ToString();
            }
            return Persona;
        }

        public bool InsertarRuneRapido(ref DataTable dtParticipantes, Int16 sOficinaConsularId, string vHostName, Int16 intUsuarioCreacionId, string vIPCreacion, int intFila)
        {
            bool booSeInserto = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ADICIONAR_RUNE_RAPIDO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros

                        if (Convert.ToInt64(dtParticipantes.Rows[intFila]["ajpa_iPersonaId"].ToString()) == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", dtParticipantes.Rows[intFila]["ajpa_vApePaterno"].ToString()));
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", dtParticipantes.Rows[intFila]["ajpa_vApeMaterno"].ToString()));
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", dtParticipantes.Rows[intFila]["ajpa_vNombre"].ToString()));
                            cmd.Parameters.Add(new SqlParameter("@pers_sPersonaTipoId", dtParticipantes.Rows[intFila]["ajpa_sTipoPersonaId"].ToString()));

                            if (dtParticipantes.Columns.Contains("pers_sNacionalidadId"))
                            {
                                if (dtParticipantes.Rows[intFila]["pers_sNacionalidadId"].ToString() != string.Empty)
                                    cmd.Parameters.Add(new SqlParameter("@pers_sNacionalidadId", Convert.ToInt32(dtParticipantes.Rows[intFila]["pers_sNacionalidadId"].ToString())));
                            }

                            cmd.Parameters.Add(new SqlParameter("@rele_sUsuarioCreacion", intUsuarioCreacionId));
                            cmd.Parameters.Add(new SqlParameter("@rele_vIPCreacion", vIPCreacion));
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", dtParticipantes.Rows[intFila]["ajpa_sDocumentoTipoId"].ToString()));
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", dtParticipantes.Rows[intFila]["ajpa_vDocumentoNumero"].ToString()));
                            cmd.Parameters.Add(new SqlParameter("@peid_vHostName", Util.ObtenerHostName()));

                            cmd.Parameters.Add(new SqlParameter("@rele_sOficinaConsularId", sOficinaConsularId));

                            SqlParameter lReturn = cmd.Parameters.Add("@pers_iPersonaId", SqlDbType.BigInt);
                            lReturn.Direction = ParameterDirection.Output;
                            
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();

                            booSeInserto = true;
                            dtParticipantes.Rows[intFila]["ajpa_iPersonaId"] = Convert.ToInt64(lReturn.Value);      // AIGNAMOS EL NUEVO ID 
                        }
                        #endregion
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                booSeInserto = false;
            }

            catch (Exception exec)
            {
                strError = exec.Message;
                booSeInserto = false;
            }

            return booSeInserto;

            
        }
    }
}
