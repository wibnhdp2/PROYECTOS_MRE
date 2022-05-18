using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.Registro.Persona.DA
{
    /// <summary>
    /// Clase: Empresa
    /// Motivo: Manteimiento
    /// Capa: Acceso a Datos
    /// </summary>
    public class EmpresaMantenimientoDA
    {
        public EmpresaMantenimientoDA() {
            this.ErrMessage = "";
        }

        public string ErrMessage { get; set; }

        private string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        #region Método INSERTAR

        public void InsertarEmpresa(ref BE.MRE.RE_EMPRESA ObjEmpBE)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@empr_sTipoEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoDocumentoId;
                        cmd.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vRazonSocial;
                        cmd.Parameters.Add("@empr_vNumeroDocumento", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vNumeroDocumento;
                        cmd.Parameters.Add("@empr_vActividadComercial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vActividadComercial;
                        cmd.Parameters.Add("@empr_vTelefono", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vTelefono;
                        cmd.Parameters.Add("@empr_vCorreo", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vCorreo;
                        cmd.Parameters.Add("@empr_sUsuarioCreacion", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sUsuarioCreacion;
                        cmd.Parameters.Add("@empr_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@empr_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjEmpBE.OficinaConsultar;
                        cmd.Parameters.Add("@empr_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        SqlParameter lReturn = cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ObjEmpBE.empr_iEmpresaId = Convert.ToInt32(lReturn.Value);
                        ObjEmpBE.Error = false;
                        ObjEmpBE.Message = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjEmpBE.Error = true;
                ObjEmpBE.Message = ex.Message.ToString();
            }
        }
        public BE.MRE.RE_REPRESENTANTELEGAL InsertarRepresentanteLegal(BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@rele_iEmpresaId", SqlDbType.BigInt).Value = objRepresentanteLegal.rele_iEmpresaId;
                        cmd.Parameters.Add("@rele_iPersonaId", SqlDbType.BigInt).Value = objRepresentanteLegal.rele_iPersonaId;
                        cmd.Parameters.Add("@rele_cEstado", SqlDbType.Char, 1).Value = "A";
                        cmd.Parameters.Add("@rele_sUsuarioCreacion", SqlDbType.SmallInt).Value = objRepresentanteLegal.rele_sUsuarioCreacion;
                        cmd.Parameters.Add("@rele_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@rele_sOficinaConsularId", SqlDbType.VarChar, 50).Value = objRepresentanteLegal.OficinaConsultar;
                        cmd.Parameters.Add("@rele_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        SqlParameter lReturn = cmd.Parameters.Add("@rele_iRepresentanteLegalId", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objRepresentanteLegal.Error = false;
                        objRepresentanteLegal.Message = string.Empty;
                    }
                }
            }
            catch (SqlException ex)
            {
                objRepresentanteLegal.Error = true;
                objRepresentanteLegal.Message = ex.Message;
            }
            return objRepresentanteLegal;
        }
        public void InsertarEmpresaResidencia(BE.MRE.RE_EMPRESARESIDENCIA objEmpresaResidencia)
        {

        }

        /// <summary>
        /// Método que inserta en la tabla empresa
        /// </summary>
        /// <param name="ObjEmpBE">Entidad de la empresa</param>
        /// <returns>Devuelve el ID generado de la inserción</returns>
        public Int16 Insertar_Empresa(ref BE.RE_EMPRESA ObjEmpBE)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@empr_sTipoEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoDocumentoId;
                        cmd.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vRazonSocial;
                        cmd.Parameters.Add("@empr_vNumeroDocumento", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vNumeroDocumento;
                        cmd.Parameters.Add("@empr_vActividadComercial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vActividadComercial;
                        cmd.Parameters.Add("@empr_vTelefono", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vTelefono;
                        cmd.Parameters.Add("@empr_vCorreo", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vCorreo;
                        cmd.Parameters.Add("@empr_sUsuarioCreacion", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sUsuarioCreacion;
                        cmd.Parameters.Add("@empr_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@empr_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjEmpBE.OficinaConsularId;
                        cmd.Parameters.Add("@empr_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        SqlParameter lReturn = cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ObjEmpBE.empr_iEmpresaId = Convert.ToInt32(lReturn.Value);

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message.ToString();
                throw ex;
            }
        }

        /// <summary>
        /// Método que inserta los representantes legales de la empresa
        /// </summary>
        /// <param name="lngEmpresaId"></param>
        /// <param name="row"></param>
        /// <param name="psUsuarioCreacion"></param>
        /// <param name="psOficinaConsularId"></param>
        /// <returns></returns>
        public void Insertar_Representante_Legal(long lngEmpresaId, DataRow row,
            Int16 psUsuarioCreacion, Int16 psOficinaConsularId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@rele_iEmpresaId", SqlDbType.BigInt).Value = lngEmpresaId;
                        cmd.Parameters.Add("@rele_iPersonaId", SqlDbType.BigInt).Value = Convert.ToInt64(row["sPersonaId"].ToString());
                        cmd.Parameters.Add("@pers_vApellidoPaterno", SqlDbType.VarChar, 100).Value = row["vApellidoPaterno"].ToString();
                        cmd.Parameters.Add("@pers_vApellidoMaterno", SqlDbType.VarChar, 100).Value = row["vApellidoMaterno"].ToString();
                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 100).Value = row["vNombres"].ToString();
                        cmd.Parameters.Add("@pers_sPersonaTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL);
                        cmd.Parameters.Add("@pers_sNacionalidadId", SqlDbType.SmallInt).Value = Convert.ToInt64(row["sNacionalidadId"].ToString());

                        cmd.Parameters.Add("@peid_sDocumentoTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["sDocumentoTipoId"].ToString());
                        cmd.Parameters.Add("@peid_vDocumentoNumero", SqlDbType.VarChar, 20).Value = row["vDocumentoNumero"].ToString();

                        cmd.Parameters.Add("@rele_cEstado", SqlDbType.Char, 1).Value = "A";
                        cmd.Parameters.Add("@rele_sUsuarioCreacion", SqlDbType.SmallInt).Value = psUsuarioCreacion;
                        cmd.Parameters.Add("@rele_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@rele_sOficinaConsularId", SqlDbType.VarChar, 50).Value = psOficinaConsularId;
                        cmd.Parameters.Add("@rele_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        SqlParameter lReturn = cmd.Parameters.Add("@rele_iRepresentanteLegalId", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message.ToString();
                Error = true;
                throw ex;
            }
        }

        /// <summary>
        /// Método para ingresar direcciones
        /// </summary>
        /// <param name="lngEmpresaId"></param>
        /// <param name="row"></param>
        /// <param name="psUsuarioCreacion"></param>
        /// <param name="psOficinaConsularId"></param>
        /// <returns></returns>
        public void Insertar_Direcciones(long lngEmpresaId, DataRow row,
            Int16 psUsuarioCreacion, Int16 psOficinaConsularId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESARESIDENCIA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@emre_iEmpresaId", SqlDbType.BigInt).Value = lngEmpresaId;
                        cmd.Parameters.Add("@emre_cEstado", SqlDbType.Char, 1).Value = "A";
                        cmd.Parameters.Add("@emre_sOficinaConsularId", SqlDbType.SmallInt).Value = psOficinaConsularId;
                        cmd.Parameters.Add("@emre_sUsuarioCreacion", SqlDbType.SmallInt).Value = psUsuarioCreacion;
                        cmd.Parameters.Add("@emre_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@emre_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@resi_sResidenciaTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["sResidenciaTipoId"].ToString());
                        cmd.Parameters.Add("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500).Value = row["vResidenciaDireccion"].ToString();
                        cmd.Parameters.Add("@resi_vCodigoPostal", SqlDbType.VarChar, 10).Value = row["vCodigoPostal"].ToString();
                        cmd.Parameters.Add("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50).Value = row["vResidenciaTelefono"].ToString();
                        cmd.Parameters.Add("@resi_cResidenciaUbigeo", SqlDbType.Char, 6).Value = row["cResidenciaUbigeo"].ToString();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message.ToString();
                Error = true;
                throw ex;
            }
        }

        #endregion Método INSERTAR

        #region Método ACTUALIZAR

        public BE.MRE.RE_EMPRESA ActualizarEmpresa(BE.MRE.RE_EMPRESA ObjEmpBE)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_iEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoDocumentoId;
                        cmd.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vRazonSocial;
                        cmd.Parameters.Add("@empr_vNumeroDocumento", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vNumeroDocumento;
                        cmd.Parameters.Add("@empr_vActividadComercial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vActividadComercial;
                        cmd.Parameters.Add("@empr_vTelefono", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vTelefono;
                        cmd.Parameters.Add("@empr_vCorreo", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vCorreo;
                        cmd.Parameters.Add("@empr_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sUsuarioModificacion;
                        cmd.Parameters.Add("@empr_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@empr_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjEmpBE.OficinaConsultar;
                        cmd.Parameters.Add("@empr_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    ObjEmpBE.Error = false;
                    ObjEmpBE.Message = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ObjEmpBE.Error = true;
                ObjEmpBE.Message = ex.Message;
            }
            return ObjEmpBE;
        }
        public BE.MRE.RE_REPRESENTANTELEGAL ActualizarRepresentanteLegal(BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@rele_iRepresentanteLegalId", SqlDbType.BigInt).Value = objRepresentanteLegal.rele_iRepresentanteLegalId;
                        cmd.Parameters.Add("@rele_iEmpresaId", SqlDbType.BigInt).Value = objRepresentanteLegal.rele_iEmpresaId;
                        cmd.Parameters.Add("@rele_iPersonaId", SqlDbType.BigInt).Value = objRepresentanteLegal.rele_iPersonaId;
                        cmd.Parameters.Add("@rele_cEstado", SqlDbType.Char, 1).Value = objRepresentanteLegal.rele_cEstado;
                        cmd.Parameters.Add("@rele_sUsuarioModificacion", SqlDbType.SmallInt).Value = objRepresentanteLegal.rele_sUsuarioModificacion;
                        cmd.Parameters.Add("@rele_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@rele_sOficinaConsularId", SqlDbType.VarChar, 50).Value = objRepresentanteLegal.OficinaConsultar;
                        cmd.Parameters.Add("@rele_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objRepresentanteLegal.Error = false;
                    }
                }
            }
            catch(SqlException ex)
            {
                objRepresentanteLegal.Error = true;
                objRepresentanteLegal.Message = ex.Message;
            }
            return objRepresentanteLegal;
        }

        /// <summary>
        /// Método que actualiza los datos de empresa
        /// </summary>
        /// <param name="ObjEmpBE"></param>
        /// <returns></returns>
        public Int16 Actualizar_Empresa(BE.RE_EMPRESA ObjEmpBE)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_iEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoEmpresaId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoEmpresaId;
                        cmd.Parameters.Add("@empr_sTipoDocumentoId", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sTipoDocumentoId;
                        cmd.Parameters.Add("@empr_vRazonSocial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vRazonSocial;
                        cmd.Parameters.Add("@empr_vNumeroDocumento", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vNumeroDocumento;
                        cmd.Parameters.Add("@empr_vActividadComercial", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vActividadComercial;
                        cmd.Parameters.Add("@empr_vTelefono", SqlDbType.VarChar, 50).Value = ObjEmpBE.empr_vTelefono;
                        cmd.Parameters.Add("@empr_vCorreo", SqlDbType.VarChar, 200).Value = ObjEmpBE.empr_vCorreo;
                        cmd.Parameters.Add("@empr_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sUsuarioModificacion;
                        cmd.Parameters.Add("@empr_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@empr_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjEmpBE.OficinaConsularId;
                        cmd.Parameters.Add("@empr_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar los representantes legales
        /// </summary>
        /// <param name="lngEmpresaId"></param>
        /// <param name="row"></param>
        /// <param name="psUsuarioCreacion"></param>
        /// <param name="psOficinaConsularId"></param>
        /// <returns></returns>
        public void Actualizar_Representante_Legal(long lngEmpresaId, DataRow row,
            Int16 psUsuarioCreacion, Int16 psOficinaConsularId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REPRESENTANTELEGAL_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@rele_iEmpresaId", SqlDbType.BigInt).Value = lngEmpresaId;
                        cmd.Parameters.Add("@rele_iPersonaId", SqlDbType.BigInt).Value = Convert.ToInt64(row["sPersonaId"].ToString());
                        cmd.Parameters.Add("@pers_vApellidoPaterno", SqlDbType.VarChar, 100).Value = row["vApellidoPaterno"].ToString();
                        cmd.Parameters.Add("@pers_vApellidoMaterno", SqlDbType.VarChar, 100).Value = row["vApellidoMaterno"].ToString();
                        cmd.Parameters.Add("@pers_vNombres", SqlDbType.VarChar, 100).Value = row["vNombres"].ToString();
                        cmd.Parameters.Add("@pers_sPersonaTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL);
                        cmd.Parameters.Add("@pers_sNacionalidadId", SqlDbType.SmallInt).Value = Convert.ToInt64(row["sNacionalidadId"].ToString());

                        cmd.Parameters.Add("@peid_sDocumentoTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["sDocumentoTipoId"].ToString());
                        cmd.Parameters.Add("@peid_vDocumentoNumero", SqlDbType.VarChar, 20).Value = row["vDocumentoNumero"].ToString();

                        cmd.Parameters.Add("@rele_cEstado", SqlDbType.Char, 1).Value = "A";
                        cmd.Parameters.Add("@rele_sUsuarioModificacion", SqlDbType.SmallInt).Value = psUsuarioCreacion;
                        cmd.Parameters.Add("@rele_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@rele_sOficinaConsularId", SqlDbType.VarChar, 50).Value = psOficinaConsularId;
                        cmd.Parameters.Add("@rele_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        SqlParameter lReturn = cmd.Parameters.Add("@rele_iRepresentanteLegalId", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        /// <summary>
        /// Método para actualizar direcciones
        /// </summary>
        /// <param name="lngEmpresaId"></param>
        /// <param name="row"></param>
        /// <param name="psUsuarioCreacion"></param>
        /// <param name="psOficinaConsularId"></param>
        /// <returns></returns>
        public void Actualizar_Direcciones(long lngEmpresaId, DataRow row,
            Int16 psUsuarioCreacion, Int16 psOficinaConsularId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESARESIDENCIA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@emre_iEmpresaId", SqlDbType.BigInt).Value = lngEmpresaId;
                        cmd.Parameters.Add("@emre_cEstado", SqlDbType.Char, 1).Value = "A";
                        cmd.Parameters.Add("@emre_sOficinaConsularId", SqlDbType.SmallInt).Value = psOficinaConsularId;
                        cmd.Parameters.Add("@emre_sUsuarioModificacion", SqlDbType.SmallInt).Value = psUsuarioCreacion;
                        cmd.Parameters.Add("@emre_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@emre_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@resi_sResidenciaTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["sResidenciaTipoId"].ToString());
                        cmd.Parameters.Add("@resi_vResidenciaDireccion", SqlDbType.VarChar, 500).Value = row["vResidenciaDireccion"].ToString();
                        cmd.Parameters.Add("@resi_vCodigoPostal", SqlDbType.VarChar, 10).Value = row["vCodigoPostal"].ToString();
                        cmd.Parameters.Add("@resi_vResidenciaTelefono", SqlDbType.VarChar, 50).Value = row["vResidenciaTelefono"].ToString();
                        cmd.Parameters.Add("@resi_cResidenciaUbigeo", SqlDbType.Char, 6).Value = row["cResidenciaUbigeo"].ToString();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        #endregion Método ACTUALIZAR

        #region Método ELIMINAR

        public Int64 Eliminar(RE_EMPRESA ObjEmpBE)
        {
            SqlTransaction Tran = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    cn.Open();
                    Tran = cn.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_EMPRESA_ANULAR", Tran.Connection);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = Tran;

                    cmd.Parameters.Add("@empr_iEmpresaId", SqlDbType.BigInt).Value = ObjEmpBE.empr_iEmpresaId;
                    cmd.Parameters.Add("@empr_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjEmpBE.empr_sUsuarioCreacion;
                    cmd.Parameters.Add("@empr_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                    cmd.Parameters.Add("@empr_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjEmpBE.OficinaConsularId;
                    cmd.Parameters.Add("@empr_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                    cmd.Parameters.Add("@iEmpresaActuacion", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    ObjEmpBE.empr_iEmpresaId = Convert.ToInt64(cmd.Parameters["@iEmpresaActuacion"].Value);

                    Tran.Commit();

                    return ObjEmpBE.empr_iEmpresaId;
                }
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                throw ex;
            }
        }

        #endregion Método ELIMINAR

    }
}