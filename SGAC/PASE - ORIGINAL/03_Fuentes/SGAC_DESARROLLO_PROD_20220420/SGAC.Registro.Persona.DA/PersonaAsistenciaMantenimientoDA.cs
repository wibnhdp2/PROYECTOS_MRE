using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaAsistenciaMantenimientoDA
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }


        /*
        public bool Insertar(BE.MRE.RE_ASISTENCIA ObjAsisBE, ref string MensajeError)
        {
            bool error = false;

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIA_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros

                        cmd.Parameters.Add("@asis_iPersonaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iPersonaId;
                        cmd.Parameters.Add("@asis_sTipAsistencia", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipAsistencia;
                        cmd.Parameters.Add("@asis_dFecServicio", SqlDbType.DateTime).Value = ObjAsisBE.asis_dFecServicio;
                        cmd.Parameters.Add("@asis_vNroCaso", SqlDbType.VarChar, 10).Value = ObjAsisBE.asis_vNroCaso;
                        cmd.Parameters.Add("@asis_vHoraInicio", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraInicio;
                        cmd.Parameters.Add("@asis_vHoraFin", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraFin;
                        cmd.Parameters.Add("@asis_sTipoServId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoServId;
                        if (ObjAsisBE.asis_sOtrosServiciosId == 0)
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOtrosServiciosId;
                        }
                        cmd.Parameters.Add("@asis_cUbigeo", SqlDbType.Char, 6).Value = ObjAsisBE.asis_cUbigeo;
                        cmd.Parameters.Add("@asis_sOficinaConsularOrigenId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularOrigenId;
                        cmd.Parameters.Add("@asis_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularId;
                        cmd.Parameters.Add("@asis_iFuncionarioId", SqlDbType.Int).Value = ObjAsisBE.asis_IFuncionarioId;
                        cmd.Parameters.Add("@asis_vDirURL", SqlDbType.VarChar, 500).Value = ObjAsisBE.asis_vDirURL;
                        cmd.Parameters.Add("@asis_sMonedaId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sMonedaId;
                        cmd.Parameters.Add("@asis_fMontoServ", SqlDbType.Float).Value = ObjAsisBE.asis_FMontoServ;
                        cmd.Parameters.Add("@asis_vObservaciones", SqlDbType.VarChar, 1000).Value = ObjAsisBE.asis_vObservaciones;
                        cmd.Parameters.Add("@asis_vNombreArchivo", SqlDbType.VarChar, 100).Value = ObjAsisBE.asis_vNombreArchivo;
                        cmd.Parameters.Add("@asis_sEstado", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sEstado;
                        cmd.Parameters.Add("@asis_sUsuarioCreacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioCreacion;
                        cmd.Parameters.Add("@asis_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjAsisBE.asis_vIPCreacion;
                        cmd.Parameters.Add("@asis_sCubrioPAH", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sCubrioPAH == null ? 0 : ObjAsisBE.asis_sCubrioPAH;
                        cmd.Parameters.Add("@asis_sTipoAyudaPah", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoAyudaPah == null ? 0 : ObjAsisBE.asis_sTipoAyudaPah;
                        cmd.Parameters.Add("@asis_sNumeroBeneficiario", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sNumeroBeneficiario;
                        cmd.Parameters.Add("@asis_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        #endregion Creando Parametros

                        cmd.Parameters.Add("@asis_iAsistenciaId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ObjAsisBE.asis_iAsistenciaId = Convert.ToInt64(cmd.Parameters["@asis_iAsistenciaId"].Value.ToString());
                        error = false;
                        MensajeError = "";
                    }

                }
            }
            catch (SqlException exec)
            {
                error = true;
                MensajeError = exec.Message.ToString();
            }
            return error;
        }
        */

        /*
        public bool InsertarBeneficiario(BE.MRE.RE_ASISTENCIA ObjAsisBE, DataTable dtBeneficiario, ref string MensajeError)
        {
            bool error = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    #region Beneficiario

                    foreach (DataRow row in dtBeneficiario.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIABENEFICIARIO_ADICIONAR", cnx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@asbe_iAsistenciaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iAsistenciaId;
                            cmd.Parameters.Add("@asbe_iPersonaId", SqlDbType.BigInt).Value = Convert.ToInt64(row["asbe_iPersonaId"].ToString());
                            cmd.Parameters.Add("@asbe_sGeneroId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["asbe_sGeneroId"].ToString());
                            cmd.Parameters.Add("@asbe_sDocumentoTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["asbe_sDocumentoTipoId"].ToString());
                            cmd.Parameters.Add("@asbe_vDocumentoNumero", SqlDbType.VarChar, 20).Value = row["asbe_vDocumentoNumero"].ToString();
                            cmd.Parameters.Add("@asbe_vApellidoPaterno", SqlDbType.VarChar, 100).Value = row["asbe_vApellidoPaterno"].ToString();
                            cmd.Parameters.Add("@asbe_vApellidoMaterno", SqlDbType.VarChar, 100).Value = row["asbe_vApellidoMaterno"].ToString();
                            cmd.Parameters.Add("@asbe_vNombres", SqlDbType.VarChar, 100).Value = row["asbe_vNombres"].ToString();
                            cmd.Parameters.Add("@asbe_FMonto", SqlDbType.Float).Value = Convert.ToSingle(row["asbe_FMonto"].ToString());
                            cmd.Parameters.Add("@asbe_sUsuarioCreacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioCreacion;
                            cmd.Parameters.Add("@asbe_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjAsisBE.asis_vIPCreacion;
                            cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                            cmd.Parameters.Add("@asbe_sOficinaConsularId", SqlDbType.VarChar, 20).Value = Convert.ToInt16(ObjAsisBE.asis_sOficinaConsularOrigenId);
                                                       

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();

                            error = false;
                            MensajeError = "";
                            cmd.Connection.Close();
                        }
                    }

                    #endregion Beneficiario
                }

            }
            catch (SqlException exec)
            {
                error = true;
                MensajeError = exec.Message.ToString();
            }
            return error;

        }
        */
        public bool Actualizar(BE.MRE.RE_ASISTENCIA ObjAsisBE, ref string MensajeError)
        {
            bool error = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIA_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add("@asis_iPersonaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iPersonaId;
                        cmd.Parameters.Add("@asis_sTipAsistencia", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipAsistencia;
                        cmd.Parameters.Add("@asis_dFecServicio", SqlDbType.DateTime).Value = ObjAsisBE.asis_dFecServicio;
                        cmd.Parameters.Add("@asis_vNroCaso", SqlDbType.VarChar, 10).Value = ObjAsisBE.asis_vNroCaso;
                        cmd.Parameters.Add("@asis_vHoraInicio", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraInicio;
                        cmd.Parameters.Add("@asis_vHoraFin", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraFin;
                        cmd.Parameters.Add("@asis_sTipoServId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoServId;
                        if (ObjAsisBE.asis_sOtrosServiciosId == 0)
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOtrosServiciosId;
                        }
                        cmd.Parameters.Add("@asis_cUbigeo", SqlDbType.Char, 6).Value = ObjAsisBE.asis_cUbigeo;
                        cmd.Parameters.Add("@asis_sOficinaConsularOrigenId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularOrigenId;
                        cmd.Parameters.Add("@asis_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularId;
                        cmd.Parameters.Add("@asis_iFuncionarioId", SqlDbType.Int).Value = ObjAsisBE.asis_IFuncionarioId;
                        cmd.Parameters.Add("@asis_vDirURL", SqlDbType.VarChar, 500).Value = ObjAsisBE.asis_vDirURL;
                        cmd.Parameters.Add("@asis_sMonedaId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sMonedaId;
                        cmd.Parameters.Add("@asis_fMontoServ", SqlDbType.Float).Value = ObjAsisBE.asis_FMontoServ;
                        cmd.Parameters.Add("@asis_vObservaciones", SqlDbType.VarChar, 1000).Value = ObjAsisBE.asis_vObservaciones;
                        cmd.Parameters.Add("@asis_vNombreArchivo", SqlDbType.VarChar, 100).Value = ObjAsisBE.asis_vNombreArchivo;
                        cmd.Parameters.Add("@asis_sEstado", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sEstado;
                        cmd.Parameters.Add("@asis_sUsuarioModificacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioModificacion;
                        cmd.Parameters.Add("@asis_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjAsisBE.asis_vIPModificacion;
                        cmd.Parameters.Add("@asis_sCubrioPAH", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sCubrioPAH == null ? 0 : ObjAsisBE.asis_sCubrioPAH;
                        cmd.Parameters.Add("@asis_sTipoAyudaPah", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoAyudaPah == null ? 0 : ObjAsisBE.asis_sTipoAyudaPah;
                        cmd.Parameters.Add("@asis_sNumeroBeneficiario", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sNumeroBeneficiario;
                        cmd.Parameters.Add("@asis_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@asis_iAsistenciaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iAsistenciaId;

                        if (ObjAsisBE.asis_sCircunscripcionId == null || ObjAsisBE.asis_sCircunscripcionId == 0)
                            cmd.Parameters.Add("@asis_sCircunscripcionId", SqlDbType.SmallInt).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@asis_sCircunscripcionId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sCircunscripcionId;


                        #endregion Creando Parametros

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        error = false;
                        MensajeError = "";


                    }
                }

            }
            catch (SqlException exec)
            {
                error = true;
                MensajeError = exec.StackTrace.ToString();
            }
            return error;

        }
        /*
        public bool ActualizarBeneficiario(BE.MRE.RE_ASISTENCIA ObjAsisBE, DataTable dtBeneficiario, ref string MensajeError)
        {
            bool error = false;
            try
            {   
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    #region Beneficiario
                    foreach (DataRow row in dtBeneficiario.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIABENEFICIARIO_ACTUALIZAR", cnx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@asbe_iAsistenciaBeneficiarioId", SqlDbType.BigInt).Value = Convert.ToInt64(row["asbe_iAsistenciaBeneficiarioId"].ToString());
                            cmd.Parameters.Add("@asbe_iAsistenciaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iAsistenciaId;
                            cmd.Parameters.Add("@asbe_iPersonaId", SqlDbType.BigInt).Value = Convert.ToInt64(row["asbe_iPersonaId"].ToString());
                            if (row["asbe_sGeneroId"].ToString() != string.Empty && row["asbe_sGeneroId"].ToString() != "0")
                                cmd.Parameters.Add("@asbe_sGeneroId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["asbe_sGeneroId"].ToString());
                            cmd.Parameters.Add("@asbe_sDocumentoTipoId", SqlDbType.SmallInt).Value = Convert.ToInt16(row["asbe_sDocumentoTipoId"].ToString());
                            cmd.Parameters.Add("@asbe_vDocumentoNumero", SqlDbType.VarChar, 20).Value = row["asbe_vDocumentoNumero"].ToString();
                            cmd.Parameters.Add("@asbe_vApellidoPaterno", SqlDbType.VarChar, 100).Value = row["asbe_vApellidoPaterno"].ToString();
                            cmd.Parameters.Add("@asbe_vApellidoMaterno", SqlDbType.VarChar, 100).Value = row["asbe_vApellidoMaterno"].ToString();
                            cmd.Parameters.Add("@asbe_vNombres", SqlDbType.VarChar, 100).Value = row["asbe_vNombres"].ToString();
                            cmd.Parameters.Add("@asbe_FMonto", SqlDbType.Float).Value = Convert.ToSingle(row["asbe_FMonto"].ToString());
                            cmd.Parameters.Add("@asbe_sUsuarioModificacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioModificacion;
                            cmd.Parameters.Add("@asbe_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjAsisBE.asis_vIPModificacion;
                            cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                            cmd.Parameters.Add("@asbe_sOficinaConsularId", SqlDbType.VarChar, 20).Value = Convert.ToInt16(ObjAsisBE.asis_sOficinaConsularOrigenId);

                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();

                            error = false;
                            MensajeError = "";
                            cmd.Connection.Close();
                        }
                    }
                    #endregion Beneficiario
                }
            }
            catch (SqlException exec)
            {
                error = true;
                MensajeError = exec.Message.ToString();
            }
            return error;


        }
        */
        public bool Eliminar(BE.MRE.RE_ASISTENCIA ObjAsisBE, ref string MensajeError)
        {
            bool error = false;
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIA_ELIMINAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@asis_iAsistenciaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iAsistenciaId;
                        cmd.Parameters.Add("@asis_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularId;
                        cmd.Parameters.Add("@asis_sUsuarioModificacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioModificacion;
                        cmd.Parameters.Add("@asis_vIPModificacion", SqlDbType.VarChar, 20).Value = ObjAsisBE.asis_vIPModificacion;
                        cmd.Parameters.Add("@asis_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        error = false;
                        MensajeError = "";
                    }
                }

            }
            catch (SqlException exec)
            {
                error = true;
                MensajeError = exec.Message.ToString();
            }
            return error;

        }


        #region Nuevo - RUNE rápido
        public void InsertarAsistencia(ref BE.MRE.RE_ASISTENCIA ObjAsisBE)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIA_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros

                        cmd.Parameters.Add("@asis_iPersonaId", SqlDbType.BigInt).Value = ObjAsisBE.asis_iPersonaId;
                        cmd.Parameters.Add("@asis_sTipAsistencia", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipAsistencia;
                        cmd.Parameters.Add("@asis_dFecServicio", SqlDbType.DateTime).Value = ObjAsisBE.asis_dFecServicio;
                        cmd.Parameters.Add("@asis_vNroCaso", SqlDbType.VarChar, 10).Value = ObjAsisBE.asis_vNroCaso;
                        cmd.Parameters.Add("@asis_vHoraInicio", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraInicio;
                        cmd.Parameters.Add("@asis_vHoraFin", SqlDbType.VarChar, 5).Value = ObjAsisBE.asis_vHoraFin;
                        cmd.Parameters.Add("@asis_sTipoServId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoServId;
                        if (ObjAsisBE.asis_sOtrosServiciosId == 0)
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@asis_sOtrosServiciosId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOtrosServiciosId;
                        }
                        cmd.Parameters.Add("@asis_cUbigeo", SqlDbType.Char, 6).Value = ObjAsisBE.asis_cUbigeo;
                        cmd.Parameters.Add("@asis_sOficinaConsularOrigenId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularOrigenId;
                        cmd.Parameters.Add("@asis_sOficinaConsularId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sOficinaConsularId;
                        cmd.Parameters.Add("@asis_iFuncionarioId", SqlDbType.Int).Value = ObjAsisBE.asis_IFuncionarioId;
                        cmd.Parameters.Add("@asis_vDirURL", SqlDbType.VarChar, 500).Value = ObjAsisBE.asis_vDirURL;
                        cmd.Parameters.Add("@asis_sMonedaId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sMonedaId;
                        cmd.Parameters.Add("@asis_fMontoServ", SqlDbType.Float).Value = ObjAsisBE.asis_FMontoServ;
                        cmd.Parameters.Add("@asis_vObservaciones", SqlDbType.VarChar, 1000).Value = ObjAsisBE.asis_vObservaciones;
                        cmd.Parameters.Add("@asis_vNombreArchivo", SqlDbType.VarChar, 100).Value = ObjAsisBE.asis_vNombreArchivo;
                        cmd.Parameters.Add("@asis_sEstado", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sEstado;
                        cmd.Parameters.Add("@asis_sUsuarioCreacion", SqlDbType.Int).Value = ObjAsisBE.asis_sUsuarioCreacion;
                        cmd.Parameters.Add("@asis_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjAsisBE.asis_vIPCreacion;
                        cmd.Parameters.Add("@asis_sCubrioPAH", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sCubrioPAH == null ? 0 : ObjAsisBE.asis_sCubrioPAH;
                        cmd.Parameters.Add("@asis_sTipoAyudaPah", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sTipoAyudaPah == null ? 0 : ObjAsisBE.asis_sTipoAyudaPah;
                        cmd.Parameters.Add("@asis_sNumeroBeneficiario", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sNumeroBeneficiario;
                        cmd.Parameters.Add("@asis_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        if (ObjAsisBE.asis_sCircunscripcionId == null || ObjAsisBE.asis_sCircunscripcionId == 0)
                            cmd.Parameters.Add("@asis_sCircunscripcionId", SqlDbType.SmallInt).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@asis_sCircunscripcionId", SqlDbType.SmallInt).Value = ObjAsisBE.asis_sCircunscripcionId;

                        #endregion Creando Parametros

                        cmd.Parameters.Add("@asis_iAsistenciaId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ObjAsisBE.asis_iAsistenciaId = Convert.ToInt64(cmd.Parameters["@asis_iAsistenciaId"].Value.ToString());
                        ObjAsisBE.Error = false;
                        ObjAsisBE.Message = "";
                    }

                }
            }
            catch (SqlException exec)
            {
                ObjAsisBE.Error = true;
                ObjAsisBE.Message = exec.Message.ToString();
            }
            //return ObjAsisBE;
        }
        public BE.MRE.RE_ASISTENCIABENEFICIARIO InsertarBeneficiario(BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    #region Beneficiario

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIABENEFICIARIO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@asbe_iAsistenciaId", SqlDbType.BigInt).Value = objBeneficiario.asbe_iAsistenciaId;
                        cmd.Parameters.Add("@asbe_iPersonaId", SqlDbType.BigInt).Value = objBeneficiario.asbe_iPersonaId;
                        cmd.Parameters.Add("@asbe_FMonto", SqlDbType.Float).Value = objBeneficiario.asbe_FMonto;
                        cmd.Parameters.Add("@asbe_sUsuarioCreacion", SqlDbType.Int).Value = objBeneficiario.asbe_sUsuarioCreacion;
                        cmd.Parameters.Add("@asbe_vIPCreacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();                        
                        cmd.Parameters.Add("@asbe_sOficinaConsularId", SqlDbType.VarChar, 20).Value = objBeneficiario.OficinaConsultar;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objBeneficiario.Error = false;
                        objBeneficiario.Message = string.Empty;
                        cmd.Connection.Close();
                    }

                    #endregion Beneficiario
                }

            }
            catch (SqlException exec)
            {
                objBeneficiario.Error = true;
                objBeneficiario.Message = exec.Message.ToString();
            }
            return objBeneficiario;
        }
        public BE.MRE.RE_ASISTENCIABENEFICIARIO ActualizarBeneficiario(BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    #region Beneficiario

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ASISTENCIABENEFICIARIO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@asbe_iAsistenciaBeneficiarioId", SqlDbType.BigInt).Value = objBeneficiario.asbe_iAsistenciaBeneficiarioId;
                        cmd.Parameters.Add("@asbe_iAsistenciaId", SqlDbType.BigInt).Value = objBeneficiario.asbe_iAsistenciaId;
                        cmd.Parameters.Add("@asbe_iPersonaId", SqlDbType.BigInt).Value = objBeneficiario.asbe_iPersonaId;
                        cmd.Parameters.Add("@asbe_FMonto", SqlDbType.Float).Value = objBeneficiario.asbe_FMonto;
                        cmd.Parameters.Add("@asbe_sUsuarioModificacion", SqlDbType.Int).Value = objBeneficiario.asbe_sUsuarioModificacion;
                        cmd.Parameters.Add("@asbe_vIPModificacion", SqlDbType.VarChar, 50).Value = Util.ObtenerDireccionIP();
                        cmd.Parameters.Add("@asbe_sOficinaConsularId", SqlDbType.VarChar, 20).Value = objBeneficiario.OficinaConsultar;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objBeneficiario.Error = false;
                        objBeneficiario.Message = string.Empty;
                        cmd.Connection.Close();
                    }

                    #endregion Beneficiario
                }

            }
            catch (SqlException exec)
            {
                objBeneficiario.Error = true;
                objBeneficiario.Message = exec.Message.ToString();
            }
            return objBeneficiario;
        }
        #endregion
    }
}