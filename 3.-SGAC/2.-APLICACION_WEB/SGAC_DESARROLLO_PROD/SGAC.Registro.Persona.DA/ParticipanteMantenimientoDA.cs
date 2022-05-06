using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.DA
{
    public class ParticipanteMantenimientoDA
    {
        public string strError = string.Empty;

        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        //public ParticipanteMantenimientoDA()
        //{
        //    strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        //}

        ~ParticipanteMantenimientoDA()
        {
            GC.Collect();
        }

        public Int16 Insertar(BE.RE_ACTUACIONPARTICIPANTE objParticipante, ref Int64 lngParticipanteId, ref Int64 lngiPersonaId,
            BE.RE_PERSONA objPersona = null, BE.RE_PERSONAIDENTIFICACION objIdentificacion = null,
            BE.RE_RESIDENCIA objResidencia = null, BE.RE_PERSONARESIDENCIA objPerResidencia = null)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = objParticipante.acpa_iActuacionDetalleId;
                        
                        cmd.Parameters.Add("@acpa_sTipoParticipanteId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoParticipanteId;
                        cmd.Parameters.Add("@acpa_sTipoDatoId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoDatoId;

                        if (objParticipante.acpa_sTipoVinculoId != null)
                            cmd.Parameters.Add("@acpa_sTipoVinculoId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoVinculoId;
                        else
                            cmd.Parameters.Add("@acpa_sTipoVinculoId", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Parameters.Add("@acpa_sOficinaConsularId", SqlDbType.SmallInt).Value = objParticipante.OficinaConsularId;
                        cmd.Parameters.Add("@acpa_sUsuarioCreacion", SqlDbType.SmallInt).Value = objParticipante.acpa_sUsuarioCreacion;
                        cmd.Parameters.Add("@acpa_vIPCreacion", SqlDbType.VarChar).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@acpa_vHostName", SqlDbType.VarChar).Value = Util.ObtenerHostName();

                        if (objPersona.pers_sPersonaTipoId != 0)
                            cmd.Parameters.Add("@sTipoPersonaId", SqlDbType.SmallInt).Value = objPersona.pers_sPersonaTipoId;
                        else
                            cmd.Parameters.Add("@sTipoPersonaId", SqlDbType.SmallInt).Value = DBNull.Value;


                        if (objIdentificacion.peid_sDocumentoTipoId != 0)
                            cmd.Parameters.Add("@sTipoDocumentoId", SqlDbType.SmallInt).Value = objIdentificacion.peid_sDocumentoTipoId;
                        else
                            cmd.Parameters.Add("@sTipoDocumentoId", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Parameters.Add("@vNumeroDocumento", SqlDbType.VarChar, 20).Value = objIdentificacion.peid_vDocumentoNumero;


                        if (objPersona.pers_sNacionalidadId != 0)
                            cmd.Parameters.Add("@sNacionalidadId", SqlDbType.SmallInt).Value = objPersona.pers_sNacionalidadId;
                        else
                            cmd.Parameters.Add("@sNacionalidadId", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Parameters.Add("@vNombres", SqlDbType.VarChar, 200).Value = objPersona.pers_vNombres;
                        cmd.Parameters.Add("@vPrimerApellido", SqlDbType.VarChar, 100).Value = objPersona.pers_vApellidoPaterno;
                        cmd.Parameters.Add("@vSegundoApellido", SqlDbType.VarChar, 100).Value = objPersona.pers_vApellidoMaterno;

                        cmd.Parameters.Add("@vDireccion", SqlDbType.VarChar, 100).Value = string.Empty;
                        if (objResidencia.resi_cResidenciaUbigeo != null)
                            if (objResidencia.resi_cResidenciaUbigeo != string.Empty)
                                cmd.Parameters["@vDireccion"].Value = objResidencia.resi_vResidenciaDireccion;


                        cmd.Parameters.Add("@cUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                        if (objResidencia.resi_cResidenciaUbigeo != null)
                            if (objResidencia.resi_cResidenciaUbigeo != string.Empty)
                                cmd.Parameters["@cUbigeo"].Value = objResidencia.resi_cResidenciaUbigeo;


                        if (objResidencia.resi_ICentroPobladoId != 0)
                            cmd.Parameters.Add("@ICentroPobladoId", SqlDbType.Int).Value = objResidencia.resi_ICentroPobladoId;
                        else
                            cmd.Parameters.Add("@ICentroPobladoId", SqlDbType.Int).Value = DBNull.Value;


                        if (objPersona.pers_dNacimientoFecha != DateTime.MinValue)
                            cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = objPersona.pers_dNacimientoFecha;
                        else
                            cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = DBNull.Value;


                        if (objPersona.pers_sGeneroId > 0)
                            cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = objPersona.pers_sGeneroId;
                        else
                            cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = DBNull.Value;


                        if (objPersona.pers_cNacimientoLugar != null && objPersona.pers_cNacimientoLugar != string.Empty)
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = objPersona.pers_cNacimientoLugar;
                        else
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = DBNull.Value;


                        cmd.Parameters.Add("@pers_bFallecidoFlag", SqlDbType.Bit).Value = objPersona.pers_bFallecidoFlag;


                        if (objPersona.pers_dFechaDefuncion != DateTime.MinValue)
                            cmd.Parameters.Add("@pers_dFechaDefuncion", SqlDbType.DateTime).Value = objPersona.pers_dFechaDefuncion;
                        else
                            cmd.Parameters.Add("@pers_dFechaDefuncion", SqlDbType.DateTime).Value = DBNull.Value;


                        if (objPersona.pers_cUbigeoDefuncion != string.Empty)
                            cmd.Parameters.Add("@pers_cUbigeoDefuncion", SqlDbType.Char, 6).Value = objPersona.pers_cUbigeoDefuncion;
                        else
                            cmd.Parameters.Add("@pers_cUbigeoDefuncion", SqlDbType.Char, 6).Value = DBNull.Value;

                        if (objResidencia.resi_iResidenciaId != 0)
                            cmd.Parameters.Add("@acpa_iResidenciaId", SqlDbType.BigInt).Value = objResidencia.resi_iResidenciaId;
                        else
                            cmd.Parameters.Add("@acpa_iResidenciaId", SqlDbType.BigInt).Value = DBNull.Value;

                        if (objPersona.pers_sEstadoCivilId!= 0)
                            cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.Int).Value = objPersona.pers_sEstadoCivilId;
                        else
                            cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.Int).Value = DBNull.Value;


                        cmd.Parameters.Add("@acpa_iPersonaId", SqlDbType.BigInt).Value = objParticipante.acpa_iPersonaId;
                        SqlParameter lPersonaIdReturn = cmd.Parameters["@acpa_iPersonaId"];
                        lPersonaIdReturn.Direction = ParameterDirection.InputOutput;

                        SqlParameter lActuacionParticipanteIdReturn = cmd.Parameters.Add("@acpa_iActuacionParticipanteId", SqlDbType.BigInt);
                        lActuacionParticipanteIdReturn.Direction = ParameterDirection.Output;

                        if (objPersona.pers_sPaisId != 0)
                            cmd.Parameters.Add("@pers_spaisid", SqlDbType.SmallInt).Value = objPersona.pers_sPaisId;
                        else
                            cmd.Parameters.Add("@pers_spaisid", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (lPersonaIdReturn.Value != null)
                        {
                            if (lPersonaIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                lngiPersonaId = Convert.ToInt64(lPersonaIdReturn.Value);                                
                            }
                        }

                        if (lActuacionParticipanteIdReturn.Value != null)
                        {
                            if (lActuacionParticipanteIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                lngParticipanteId = Convert.ToInt64(lActuacionParticipanteIdReturn.Value);
                            }
                        }

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                        
                    }
                }

                return intResult;
            }
            catch(Exception ex)
            {
                strError = ex.Message;
                return Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            }

            
        }
        //------------------------------------------------------------------------------------
        //Autor: Miguel Márquez Beltrán
        //Fecha:04/10/2016
        //Objetivo: Validar que los atributos con relacion FK no tengan espacio en blanco.
        //------------------------------------------------------------------------------------
        public Int16 Actualizar(BE.RE_ACTUACIONPARTICIPANTE objParticipante, BE.RE_PERSONA objPersona = null, BE.RE_PERSONAIDENTIFICACION objIdentificacion = null)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ACTUALIZAR", cn))
                    {

                         cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acpa_iActuacionParticipanteId", SqlDbType.BigInt).Value = objParticipante.acpa_iActuacionParticipanteId;
                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = objParticipante.acpa_iActuacionDetalleId;
                        cmd.Parameters.Add("@acpa_iPersonaId", SqlDbType.BigInt).Value = objParticipante.acpa_iPersonaId;
                        cmd.Parameters.Add("@acpa_sTipoParticipanteId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoParticipanteId;
                        cmd.Parameters.Add("@acpa_sTipoDatoId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoDatoId;
                        cmd.Parameters.Add("@acpa_sTipoVinculoId", SqlDbType.SmallInt).Value = objParticipante.acpa_sTipoVinculoId;
                        cmd.Parameters.Add("@acpa_sUsuarioModificacion", SqlDbType.SmallInt).Value = objParticipante.acpa_sUsuarioModificacion;
                        cmd.Parameters.Add("@acpa_vIPModificacion", SqlDbType.VarChar).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@acpa_vHostName", SqlDbType.VarChar).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@acpa_sOficinaConsularId", SqlDbType.SmallInt).Value = objParticipante.OficinaConsularId;

                        cmd.Parameters.Add("@vDireccion", SqlDbType.VarChar, 100).Value = string.Empty;
                        if (objParticipante.vDirecionParticipante != null)
                            if (objParticipante.vDirecionParticipante.Trim() != string.Empty)
                                cmd.Parameters["@vDireccion"].Value = objParticipante.vDirecionParticipante;

                        cmd.Parameters.Add("@cUbigeo", SqlDbType.Char, 6).Value = DBNull.Value;
                        if (objParticipante.vUbigeo != null)
                            if (objParticipante.vUbigeo.Trim() != string.Empty)
                                cmd.Parameters["@cUbigeo"].Value = objParticipante.vUbigeo;

                        if (objParticipante.vCentroPoblado != null)
                        if (objParticipante.vCentroPoblado > 0)
                            cmd.Parameters.Add("@ICentroPobladoId", SqlDbType.Int).Value = objParticipante.vCentroPoblado;
                        else
                            cmd.Parameters.Add("@ICentroPobladoId", SqlDbType.Int).Value = DBNull.Value;


                        if (objIdentificacion.peid_sDocumentoTipoId != 0)
                            cmd.Parameters.Add("@sTipoDocumentoId", SqlDbType.SmallInt).Value = objIdentificacion.peid_sDocumentoTipoId;
                        else
                            cmd.Parameters.Add("@sTipoDocumentoId", SqlDbType.SmallInt).Value = DBNull.Value;

                        cmd.Parameters.Add("@vNumeroDocumento", SqlDbType.VarChar, 20).Value = objIdentificacion.peid_vDocumentoNumero;

                        if (objPersona.pers_sNacionalidadId != 0)
                            cmd.Parameters.Add("@pers_sNacionalidadID", SqlDbType.SmallInt).Value = objPersona.pers_sNacionalidadId;
                        else
                            cmd.Parameters.Add("@pers_sNacionalidadID", SqlDbType.SmallInt).Value = DBNull.Value;


                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 200).Value = objPersona.pers_vNombres;
                        cmd.Parameters.Add("@pers_vPrimerApellido", SqlDbType.VarChar, 100).Value = objPersona.pers_vApellidoPaterno;
                        cmd.Parameters.Add("@pers_vSegundoApellido", SqlDbType.VarChar, 100).Value = objPersona.pers_vApellidoMaterno;


                        if (objPersona.pers_sPersonaTipoId != 0)
                            cmd.Parameters.Add("@sTipoPersonaId", SqlDbType.SmallInt).Value = objPersona.pers_sPersonaTipoId;
                        else
                            cmd.Parameters.Add("@sTipoPersonaId", SqlDbType.SmallInt).Value = DBNull.Value;



                        if (objPersona.pers_dNacimientoFecha != DateTime.MinValue)
                            cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = objPersona.pers_dNacimientoFecha;
                        else
                            cmd.Parameters.Add("@pers_dNacimientoFecha", SqlDbType.DateTime).Value = DBNull.Value;


                        if (objPersona.pers_sGeneroId > 0)
                            cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = objPersona.pers_sGeneroId;
                        else
                            cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = DBNull.Value;


                        if (objPersona.pers_cNacimientoLugar != null && objPersona.pers_cNacimientoLugar.Trim() != string.Empty)
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = objPersona.pers_cNacimientoLugar;
                        else
                            cmd.Parameters.Add("@pers_cNacimientoLugar", SqlDbType.Char, 6).Value = DBNull.Value;

                        if (objPersona.pers_sEstadoCivilId != 0)
                            cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.Int).Value = objPersona.pers_sEstadoCivilId;
                        else
                            cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.Int).Value = DBNull.Value;

                        if (objPersona.pers_dFechaDefuncion != DateTime.MinValue)
                            cmd.Parameters.Add("@pers_dFechaDefuncion", SqlDbType.DateTime).Value = objPersona.pers_dFechaDefuncion;
                        else
                            cmd.Parameters.Add("@pers_dFechaDefuncion", SqlDbType.DateTime).Value = DBNull.Value;

                        if (objPersona.pers_sPaisId != 0)
                            cmd.Parameters.Add("@pers_spaisid", SqlDbType.SmallInt).Value = objPersona.pers_sPaisId;
                        else
                            cmd.Parameters.Add("@pers_spaisid", SqlDbType.SmallInt).Value = DBNull.Value;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }

                return intResult;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                return Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            }

        }

        public Int16 Eliminar(BE.RE_ACTUACIONPARTICIPANTE objParticipante)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acpa_iActuacionParticipanteId", SqlDbType.BigInt).Value = objParticipante.acpa_iActuacionParticipanteId;
                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = objParticipante.acpa_iActuacionDetalleId;
                        cmd.Parameters.Add("@acpa_cEstado", SqlDbType.Char).Value = Enumerador.enmEstado.DESACTIVO.ToString();
                        cmd.Parameters.Add("@acpa_sUsuarioModificacion", SqlDbType.SmallInt).Value = objParticipante.acpa_sUsuarioModificacion;
                        cmd.Parameters.Add("@acpa_vIPModificacion", SqlDbType.VarChar).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@acpa_vHostName", SqlDbType.VarChar).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@acpa_sOficinaConsularId", SqlDbType.SmallInt).Value = objParticipante.OficinaConsularId;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            }
        }

        public Int16 EliminarParticipanteActuacionDetalle(Int64 iActuacionDetalleId, Int16 iUsuarioId, Int16 iOficinaConsula)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ANULAR_ACTUACION_DETALLE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = iActuacionDetalleId;
                        cmd.Parameters.Add("@acpa_sUsuarioModificacion", SqlDbType.SmallInt).Value = iUsuarioId;
                        cmd.Parameters.Add("@acpa_vIPModificacion", SqlDbType.VarChar).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@acpa_vHostName", SqlDbType.VarChar).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@acpa_sOficinaConsularId", SqlDbType.SmallInt).Value = iOficinaConsula;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            }
        }


        
    }
}