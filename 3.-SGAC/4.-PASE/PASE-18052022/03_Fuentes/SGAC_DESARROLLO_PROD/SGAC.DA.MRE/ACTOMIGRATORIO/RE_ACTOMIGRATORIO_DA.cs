using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;

namespace SGAC.DA.MRE.ACTOMIGRATORIO
{
    public class RE_ACTOMIGRATORIO_DA
    {
        public string strError = string.Empty;

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO insertar(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActuacionDetalleId", ActoMigratorio.acmi_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_IFuncionarioId", ActoMigratorio.acmi_IFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", ActoMigratorio.acmi_sTipoDocumentoMigratorioId));
                        if (ActoMigratorio.acmi_sTipoId == null) cmd.Parameters.Add(new SqlParameter("@acmi_sTipoId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sTipoId", ActoMigratorio.acmi_sTipoId));
                        if (ActoMigratorio.acmi_sSubTipoId == null) cmd.Parameters.Add(new SqlParameter("@acmi_sSubTipoId", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_sSubTipoId", ActoMigratorio.acmi_sSubTipoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroExpediente", ActoMigratorio.acmi_vNumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroLamina", ActoMigratorio.acmi_vNumeroLamina));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpedicion", ActoMigratorio.acmi_dFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpiracion", ActoMigratorio.acmi_dFechaExpiracion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumento", ActoMigratorio.acmi_vNumeroDocumento));
                        if (ActoMigratorio.acmi_vNumeroDocumentoAnterior == null) cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumentoAnterior", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumentoAnterior", ActoMigratorio.acmi_vNumeroDocumentoAnterior));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vObservaciones", ActoMigratorio.acmi_vObservaciones));

                        cmd.Parameters.Add(new SqlParameter("@acmi_sEstadoId", ActoMigratorio.acmi_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioCreacion", ActoMigratorio.acmi_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPCreacion", ActoMigratorio.acmi_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaCreacion", ActoMigratorio.acmi_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaModificacion", ActoMigratorio.acmi_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@iPersonaRecurrenteId", ActoMigratorio.acmi_iRecurrenteId));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", SGAC.Accesorios.Util.ObtenerHostName()));

                        if (ActoMigratorio.acmi_sPaisId == null || ActoMigratorio.acmi_sPaisId == 0)
                            cmd.Parameters.Add(new SqlParameter("@acmi_sPaisDestinoId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acmi_sPaisDestinoId", ActoMigratorio.acmi_sPaisId));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@acmi_iActoMigratorioId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO Actualizar_Migratorio(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ACTUALIZAR_CONSULTA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros

                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId", ActoMigratorio.acmi_iActoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", ActoMigratorio.acmi_sTipoDocumentoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroLamina", ActoMigratorio.acmi_vNumeroLamina));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumento", ActoMigratorio.acmi_vNumeroDocumento));
                        if (ActoMigratorio.acmi_vNumeroDocumentoAnterior == null) cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumentoAnterior", DBNull.Value));
                        else cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumentoAnterior", ActoMigratorio.acmi_vNumeroDocumentoAnterior));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vHostName", ActoMigratorio.HostName));
                        

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }
        public SGAC.BE.MRE.RE_ACTOMIGRATORIO actualizar(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId", ActoMigratorio.acmi_iActoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActuacionDetalleId", ActoMigratorio.acmi_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_IFuncionarioId", ActoMigratorio.acmi_IFuncionarioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", ActoMigratorio.acmi_sTipoDocumentoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoId", ActoMigratorio.acmi_sTipoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sSubTipoId", ActoMigratorio.acmi_sSubTipoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroExpediente", ActoMigratorio.acmi_vNumeroExpediente));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroLamina", ActoMigratorio.acmi_vNumeroLamina));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpedicion", ActoMigratorio.acmi_dFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_dFechaExpiracion", ActoMigratorio.acmi_dFechaExpiracion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumento", ActoMigratorio.acmi_vNumeroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumentoAnterior", ActoMigratorio.acmi_vNumeroDocumentoAnterior));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vObservaciones", ActoMigratorio.acmi_vObservaciones));


                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vHostName", ActoMigratorio.HostName));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sEstadoId", ActoMigratorio.acmi_sEstadoId));

                        if (ActoMigratorio.acmi_sPaisId == null || ActoMigratorio.acmi_sPaisId == 0)
                            cmd.Parameters.Add(new SqlParameter("@acmi_sPaisDestinoId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@acmi_sPaisDestinoId", ActoMigratorio.acmi_sPaisId));


                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO actualizar_estados(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ACTUALIZAR_ESTADOS", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId", ActoMigratorio.acmi_iActoMigratorioId));
                        
                        cmd.Parameters.Add(new SqlParameter("@acmi_sEstadoId", ActoMigratorio.acmi_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vHostName", ActoMigratorio.HostName));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO actualizar_estado_lamina(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ACTUALIZAR_LAMINA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId", ActoMigratorio.acmi_iActoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroLamina", ActoMigratorio.acmi_vNumeroLamina));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vHostName", ActoMigratorio.HostName));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO actualizar_estado_pasaporte(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIO_ACTUALIZAR_PASSAPORTE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acmi_iActoMigratorioId", ActoMigratorio.acmi_iActoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vNumeroDocumento", ActoMigratorio.acmi_vNumeroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sOficinaConsularId", ActoMigratorio.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vHostName", ActoMigratorio.HostName));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sUsuarioModificacion", ActoMigratorio.acmi_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_vIPModificacion", ActoMigratorio.acmi_vIPModificacion));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorio.acmi_iActoMigratorioId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorio.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorio.Error = true;
                ActoMigratorio.Message = exec.StackTrace.ToString();
            }
            return ActoMigratorio;
        }
        public int Validar_Lamina(int intInsumoLaminaId, string strCodLamina,
            int intOficinaConsularId, int intUsuarioModificacionId, string strLamina,  ref string strMensaje)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_VALIDAR_LAMINA", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@sInsumoTipoId", intInsumoLaminaId));
                        cmd.Parameters.Add(new SqlParameter("@vCodigoAutoadhesivo", strCodLamina));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioModificacion", intUsuarioModificacionId));
                        cmd.Parameters.Add(new SqlParameter("@insu_vCodigoUnicoFabrica", strLamina));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@strMensaje", SqlDbType.VarChar,200);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = Convert.ToString(lReturn.Value);
                    }
                }
            }
            catch(SqlException ex)
            {
                strMensaje = ex.Message.ToString();
                return -1;
            }
            return 1;
        }

        public SGAC.BE.MRE.RE_ACTOMIGRATORIO obtener(SGAC.BE.MRE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            return null;
        }

        public List<SGAC.BE.RE_ACTOMIGRATORIO> paginado(SGAC.BE.RE_ACTOMIGRATORIO ActoMigratorio)
        {
            return null;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public CBE_PERSONA Actualizar_Datos_Persona(CBE_PERSONA oRE_PERSONA)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR_ACTO_MIGRATORIO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pers_iPersonaId", oRE_PERSONA.pers_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sEstadoCivilId", oRE_PERSONA.pers_sEstadoCivilId));
                        if (oRE_PERSONA.pers_sOcupacionId == 0)
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@pers_sOcupacionId", oRE_PERSONA.pers_sOcupacionId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sGeneroId", oRE_PERSONA.pers_sGeneroId));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", oRE_PERSONA.pers_vApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", oRE_PERSONA.pers_vApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@pers_vNombres", oRE_PERSONA.pers_vNombres));
                        cmd.Parameters.Add(new SqlParameter("@pers_dNacimientoFecha", oRE_PERSONA.pers_dNacimientoFecha));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", oRE_PERSONA.IDENTIFICACION.peid_sDocumentoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", oRE_PERSONA.IDENTIFICACION.peid_vDocumentoNumero));

                        cmd.Parameters.Add(new SqlParameter("@peid_dFecExpedicion", oRE_PERSONA.IDENTIFICACION.peid_dFecExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@peid_dFecVcto", oRE_PERSONA.IDENTIFICACION.peid_dFecVcto));
                        cmd.Parameters.Add(new SqlParameter("@peid_vLugarExpedicion", oRE_PERSONA.IDENTIFICACION.peid_vLugarExpedicion));

                        cmd.Parameters.Add(new SqlParameter("@pers_cNacimientoLugar", oRE_PERSONA.pers_cNacimientoLugar));
                        cmd.Parameters.Add(new SqlParameter("@pers_vEstatura", oRE_PERSONA.pers_vEstatura));
                        cmd.Parameters.Add(new SqlParameter("@pers_sColorOjosId", oRE_PERSONA.pers_sColorOjosId));
                        cmd.Parameters.Add(new SqlParameter("@pers_sColorCabelloId", oRE_PERSONA.pers_sColorCabelloId));
                        

                        cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId_peru", oRE_PERSONA.RESIDENCIAS[0].resi_iResidenciaId));

                        if (oRE_PERSONA.RESIDENCIAS[0].resi_cResidenciaUbigeo.Equals("000"))
                            cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo_peru", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo_peru", oRE_PERSONA.RESIDENCIAS[0].resi_cResidenciaUbigeo));

                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaDireccion_peru", oRE_PERSONA.RESIDENCIAS[0].resi_vResidenciaDireccion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaTelefono_peru", oRE_PERSONA.RESIDENCIAS[0].resi_vResidenciaTelefono));


                        cmd.Parameters.Add(new SqlParameter("@resi_iResidenciaId_extranjero", oRE_PERSONA.RESIDENCIAS[1].resi_iResidenciaId));

                        if (oRE_PERSONA.RESIDENCIAS[1].resi_cResidenciaUbigeo.Equals("000"))
                            cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo_extranjero", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@resi_cResidenciaUbigeo_extranjero", oRE_PERSONA.RESIDENCIAS[1].resi_cResidenciaUbigeo));

                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaDireccion_extranjero", oRE_PERSONA.RESIDENCIAS[1].resi_vResidenciaDireccion));
                        cmd.Parameters.Add(new SqlParameter("@resi_vResidenciaTelefono_extranjero", oRE_PERSONA.RESIDENCIAS[1].resi_vResidenciaTelefono));

                        cmd.Parameters.Add(new SqlParameter("@pefi_iPersonaFilacionId_P", oRE_PERSONA.FILIACIONES[0].pefi_iPersonaFilacionId));
                        cmd.Parameters.Add(new SqlParameter("@pefi_sTipoFilacionId_P", oRE_PERSONA.FILIACIONES[0].pefi_sTipoFilacionId));
                        cmd.Parameters.Add(new SqlParameter("@pefi_vNombreFiliacion_P", oRE_PERSONA.FILIACIONES[0].pefi_vNombreFiliacion));


                        cmd.Parameters.Add(new SqlParameter("@pefi_iPersonaFilacionId_M", oRE_PERSONA.FILIACIONES[1].pefi_iPersonaFilacionId));
                        cmd.Parameters.Add(new SqlParameter("@pefi_sTipoFilacionId_M", oRE_PERSONA.FILIACIONES[1].pefi_sTipoFilacionId));
                        cmd.Parameters.Add(new SqlParameter("@pefi_vNombreFiliacion_M", oRE_PERSONA.FILIACIONES[1].pefi_vNombreFiliacion));


                        cmd.Parameters.Add(new SqlParameter("@resi_sUsuarioCreacion", oRE_PERSONA.pers_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sTipoDocumentoMigratorioId", oRE_PERSONA.acmi_sTipoDocumentoMigratorioId));
                        cmd.Parameters.Add(new SqlParameter("@Tipo", oRE_PERSONA.Tipo));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", oRE_PERSONA.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vIPModificacion", Accesorios.Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@acmi_sMigratorioId", oRE_PERSONA.acmi_sMigratorioId));

                        SqlParameter lReturn = cmd.Parameters.Add("@RETURNVALUE", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        oRE_PERSONA.pers_iPersonaId = Convert.ToInt64(lReturn.Value);
                        if (oRE_PERSONA.pers_iPersonaId == 2598)
                        {
                            oRE_PERSONA.Error = true;
                            oRE_PERSONA.Message = "El número de documento ingresado ya se encuentra registrado";
                            oRE_PERSONA.pers_iPersonaId = 0;
                        }
                        oRE_PERSONA.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                oRE_PERSONA.Error = true;
                oRE_PERSONA.Message = exec.StackTrace.ToString();
            }
            return oRE_PERSONA;
        }

        public string ExisteNumeroExpediente(BE.MRE.SI_EXPEDIENTE objExpediente)
        {
            string strMensaje = string.Empty;
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_EXISTE_NUMEROEXPEDIENTE]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@exp_sTipoDocMigId", SqlDbType.SmallInt).Value = objExpediente.exp_sTipoDocMigId;
                        cmd.Parameters.Add("@exp_iNumeroExpediente", SqlDbType.VarChar).Value = objExpediente.exp_INumeroExpediente;
                        cmd.Parameters.Add("@exp_sOficinaConsularId", SqlDbType.SmallInt).Value = objExpediente.exp_sOficinaConsularId;
                        cmd.Parameters.Add("@exp_sPeriodo", SqlDbType.SmallInt).Value = objExpediente.exp_sPeriodo;
                        cmd.Parameters.Add("@exp_sExpedienteId", SqlDbType.SmallInt).Value = objExpediente.exp_sExpedienteId;

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = Convert.ToString(lReturn.Value);

                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                strMensaje = ex.Message;
            }
            return strMensaje;
        }
    }
}
