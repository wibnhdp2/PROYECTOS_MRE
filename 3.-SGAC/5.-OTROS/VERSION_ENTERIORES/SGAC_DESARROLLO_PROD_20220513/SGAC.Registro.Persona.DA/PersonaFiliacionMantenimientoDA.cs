using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaFiliacionMantenimientoDA
    {
        ~PersonaFiliacionMantenimientoDA()
        {
            GC.Collect();
        }

        private string StrConnectionName = string.Empty;

        public PersonaFiliacionMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_PERSONAFILIACION Insertar(BE.MRE.RE_PERSONAFILIACION personaFiliacion)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR_FILIACION", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@pefi_iPersonaId", personaFiliacion.pefi_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@pefi_vNombreFiliacion", personaFiliacion.pefi_vNombreFiliacion));
                        if (personaFiliacion.pefi_vLugarNacimiento != null)
                            cmd.Parameters.Add(new SqlParameter("@pefi_vLugarNacimiento", personaFiliacion.pefi_vLugarNacimiento));
                        if (personaFiliacion.pefi_dFechaNacimiento != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@pefi_dFechaNacimiento", personaFiliacion.pefi_dFechaNacimiento));
                        if (personaFiliacion.pefi_sNacionalidad != 0)
                            cmd.Parameters.Add(new SqlParameter("@pefi_sNacionalidad", personaFiliacion.pefi_sNacionalidad));
                        cmd.Parameters.Add(new SqlParameter("@pefi_sTipoFilacionId", personaFiliacion.pefi_sTipoFilacionId));
                        if (personaFiliacion.pefi_vNroDocumento != null)
                            cmd.Parameters.Add(new SqlParameter("@pefi_vNroDocumento", personaFiliacion.pefi_vNroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@pefi_sUsuarioCreacion", personaFiliacion.pefi_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@pefi_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@pefi_sOficinaConsularId", personaFiliacion.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@pefi_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        personaFiliacion.pefi_iPersonaFilacionId = Convert.ToInt64(lReturn.Value);
                        personaFiliacion.Error = false;  
                        
                    }
                }
            }
            catch (SqlException exec)
            {
                personaFiliacion.Error = true;
                personaFiliacion.Message = exec.Message.ToString();
            }

            return personaFiliacion;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------


        public int Insertar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                            int IntOficinaConsularId)
        {
            long LonResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[12];

                prmParameter[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = ObjPersFiliacionBE.pefi_iPersonaId;

                prmParameter[2] = new SqlParameter("@pefi_vNombreFiliacion", SqlDbType.VarChar, 500);
                prmParameter[2].Value = ObjPersFiliacionBE.pefi_vNombreFiliacion;

                prmParameter[3] = new SqlParameter("@pefi_vLugarNacimiento", SqlDbType.VarChar, 500);
                prmParameter[3].Value = ObjPersFiliacionBE.pefi_vLugarNacimiento;

                //prmParameter[4] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.VarChar, 12);
                //prmParameter[4].Value = ObjPersFiliacionBE.pefi_vFechaNacimiento;

                prmParameter[4] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.DateTime);
                prmParameter[4].Value = ObjPersFiliacionBE.pefi_dFechaNacimiento;

                prmParameter[5] = new SqlParameter("@pefi_sNacionalidad", SqlDbType.SmallInt);
                prmParameter[5].Value = ObjPersFiliacionBE.pefi_sNacionalidad;

                prmParameter[6] = new SqlParameter("@pefi_sTipoFilacionId", SqlDbType.SmallInt);
                prmParameter[6].Value = ObjPersFiliacionBE.pefi_sTipoFilacionId;

                prmParameter[7] = new SqlParameter("@pefi_vNroDocumento", SqlDbType.VarChar, 20);
                prmParameter[7].Value = ObjPersFiliacionBE.pefi_vNroDocumento;

                prmParameter[8] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[8].Value = IntOficinaConsularId;

                prmParameter[9] = new SqlParameter("@pefi_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameter[9].Value = ObjPersFiliacionBE.pefi_sUsuarioCreacion;

                prmParameter[10] = new SqlParameter("@pefi_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[10].Value = ObjPersFiliacionBE.pefi_vIPCreacion;

                prmParameter[11] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                prmParameter[11].Value = Util.ObtenerHostName();

                LonResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Actualizar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                              int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[12];

                prmParameter[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                prmParameter[0].Value = ObjPersFiliacionBE.pefi_iPersonaFilacionId;

                prmParameter[1] = new SqlParameter("@pefi_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = ObjPersFiliacionBE.pefi_iPersonaId;

                prmParameter[2] = new SqlParameter("@pefi_vNombreFiliacion", SqlDbType.VarChar, 500);
                prmParameter[2].Value = ObjPersFiliacionBE.pefi_vNombreFiliacion;

                prmParameter[3] = new SqlParameter("@pefi_vLugarNacimiento", SqlDbType.VarChar, 500);
                prmParameter[3].Value = ObjPersFiliacionBE.pefi_vLugarNacimiento;

                //prmParameter[4] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.VarChar, 12);
                //prmParameter[4].Value = ObjPersFiliacionBE.pefi_vFechaNacimiento;

                prmParameter[4] = new SqlParameter("@pefi_vFechaNacimiento", SqlDbType.DateTime);
                prmParameter[4].Value = ObjPersFiliacionBE.pefi_dFechaNacimiento;

                prmParameter[5] = new SqlParameter("@pefi_sNacionalidad", SqlDbType.SmallInt);
                prmParameter[5].Value = ObjPersFiliacionBE.pefi_sNacionalidad;

                prmParameter[6] = new SqlParameter("@pefi_sTipoFilacionId", SqlDbType.SmallInt);
                prmParameter[6].Value = ObjPersFiliacionBE.pefi_sTipoFilacionId;

                prmParameter[7] = new SqlParameter("@pefi_vNroDocumento", SqlDbType.VarChar, 20);
                prmParameter[7].Value = ObjPersFiliacionBE.pefi_vNroDocumento;

                prmParameter[8] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[8].Value = IntOficinaConsularId;

                prmParameter[9] = new SqlParameter("@pefi_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[9].Value = ObjPersFiliacionBE.pefi_sUsuarioModificacion;

                prmParameter[10] = new SqlParameter("@pefi_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[10].Value = ObjPersFiliacionBE.pefi_vIPModificacion;

                prmParameter[11] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                prmParameter[11].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFILIACION_ACTUALIZAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Eliminar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                            int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[5];

                prmParameter[0] = new SqlParameter("@pefi_iPersonaFilacionId", SqlDbType.BigInt);
                prmParameter[0].Value = ObjPersFiliacionBE.pefi_iPersonaFilacionId;

                prmParameter[1] = new SqlParameter("@pefi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameter[1].Value = IntOficinaConsularId;

                prmParameter[2] = new SqlParameter("@pefi_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameter[2].Value = ObjPersFiliacionBE.pefi_sUsuarioModificacion;

                prmParameter[3] = new SqlParameter("@pefi_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameter[3].Value = ObjPersFiliacionBE.pefi_vIPModificacion;

                prmParameter[4] = new SqlParameter("@pefi_vHostName", SqlDbType.VarChar, 20);
                prmParameter[4].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAFILIACION_ELIMINAR",
                                                           prmParameter);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}