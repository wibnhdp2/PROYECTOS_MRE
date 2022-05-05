using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaIdentificacionMantenimientoDA
    {
        ~PersonaIdentificacionMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public BE.MRE.RE_PERSONAIDENTIFICACION Insertar(BE.MRE.RE_PERSONAIDENTIFICACION personaIdentificacion)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@peid_iPersonaId", personaIdentificacion.peid_iPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", personaIdentificacion.peid_sDocumentoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", personaIdentificacion.peid_vDocumentoNumero));
                        if (personaIdentificacion.peid_dFecVcto != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@peid_dFecVcto", personaIdentificacion.peid_dFecVcto));
                        if (personaIdentificacion.peid_dFecExpedicion != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@peid_dFecExpedicion", personaIdentificacion.peid_dFecExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@peid_vLugarExpedicion", personaIdentificacion.peid_vLugarExpedicion));
                        if (personaIdentificacion.peid_dFecRenovacion != DateTime.MinValue)
                            cmd.Parameters.Add(new SqlParameter("@peid_dFecRenovacion", personaIdentificacion.peid_dFecRenovacion));
                        cmd.Parameters.Add(new SqlParameter("@peid_vLugarRenovacion", personaIdentificacion.peid_vLugarRenovacion));
                        cmd.Parameters.Add(new SqlParameter("@peid_bActivoEnRune", personaIdentificacion.peid_bActivoEnRune));
                        cmd.Parameters.Add(new SqlParameter("@peid_sUsuarioCreacion", personaIdentificacion.peid_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@peid_vIPCreacion", Util.ObtenerDireccionIP()));
                        cmd.Parameters.Add(new SqlParameter("@peid_sOficinaConsularId", personaIdentificacion.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@peid_vHostName", Util.ObtenerHostName()));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@peid_iPersonaIdentificacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        personaIdentificacion.peid_iPersonaIdentificacionId = Convert.ToInt64(lReturn.Value);
                        personaIdentificacion.Error = false;                       
                    }
                }
            }
            catch (SqlException exec)
            {
                personaIdentificacion.Error = true;
                personaIdentificacion.Message = exec.Message.ToString();
            }

            return personaIdentificacion;
        }

        private string StrConnectionName = string.Empty;

        public PersonaIdentificacionMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public int Insertar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                            int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[14];

                prmParameterHeader[0] = new SqlParameter("@peid_iPersonaIdentificacionId", SqlDbType.BigInt);
                prmParameterHeader[0].Direction = ParameterDirection.Output;

                prmParameterHeader[1] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[1].Value = ObjPerIdentBE.peid_iPersonaId;

                prmParameterHeader[2] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[2].Value = ObjPerIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[3] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[3].Value = ObjPerIdentBE.peid_vDocumentoNumero;

                if (ObjPerIdentBE.peid_dFecVcto == null)
                {
                    prmParameterHeader[4] = new SqlParameter("@peid_dFecVcto", SqlDbType.DateTime);
                    prmParameterHeader[4].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[4] = new SqlParameter("@peid_dFecVcto", SqlDbType.DateTime);
                    prmParameterHeader[4].Value = ObjPerIdentBE.peid_dFecVcto;
                }

                if (ObjPerIdentBE.peid_dFecExpedicion == null)
                {
                    prmParameterHeader[5] = new SqlParameter("@peid_dFecExpedicion", SqlDbType.DateTime);
                    prmParameterHeader[5].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[5] = new SqlParameter("@peid_dFecExpedicion", SqlDbType.DateTime);
                    prmParameterHeader[5].Value = ObjPerIdentBE.peid_dFecExpedicion;
                }

                if (ObjPerIdentBE.peid_vLugarExpedicion.Length == 0)
                {
                    prmParameterHeader[6] = new SqlParameter("@peid_vLugarExpedicion", SqlDbType.VarChar, 500);
                    prmParameterHeader[6].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[6] = new SqlParameter("@peid_vLugarExpedicion", SqlDbType.VarChar, 500);
                    prmParameterHeader[6].Value = ObjPerIdentBE.peid_vLugarExpedicion;
                }

                if (ObjPerIdentBE.peid_dFecRenovacion == null)
                {
                    prmParameterHeader[7] = new SqlParameter("@peid_dFecRenovacion", SqlDbType.DateTime);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@peid_dFecRenovacion", SqlDbType.DateTime);
                    prmParameterHeader[7].Value = ObjPerIdentBE.peid_dFecRenovacion;
                }

                if (ObjPerIdentBE.peid_vLugarRenovacion.Length == 0)
                {
                    prmParameterHeader[8] = new SqlParameter("@peid_vLugarRenovacion", SqlDbType.VarChar, 500);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[8] = new SqlParameter("@peid_vLugarRenovacion", SqlDbType.VarChar, 500);
                    prmParameterHeader[8].Value = ObjPerIdentBE.peid_vLugarRenovacion;
                }

                prmParameterHeader[9] = new SqlParameter("@peid_bActivoEnRune", SqlDbType.Bit);
                prmParameterHeader[9].Value = ObjPerIdentBE.peid_bActivoEnRune;

                prmParameterHeader[10] = new SqlParameter("@peid_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[10].Value = IntOficinaConsularId;

                prmParameterHeader[11] = new SqlParameter("@peid_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[11].Value = ObjPerIdentBE.peid_sUsuarioCreacion;

                prmParameterHeader[12] = new SqlParameter("@peid_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[12].Value = ObjPerIdentBE.peid_vIPCreacion;

                prmParameterHeader[13] = new SqlParameter("@peid_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[13].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ADICIONAR",
                                                           prmParameterHeader);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Actualizar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                              int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[15];

                prmParameterHeader[0] = new SqlParameter("@peid_iPersonaIdentificacionId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = ObjPerIdentBE.peid_iPersonaIdentificacionId;

                prmParameterHeader[1] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameterHeader[1].Value = ObjPerIdentBE.peid_iPersonaId;

                prmParameterHeader[2] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameterHeader[2].Value = ObjPerIdentBE.peid_sDocumentoTipoId;

                prmParameterHeader[3] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameterHeader[3].Value = ObjPerIdentBE.peid_vDocumentoNumero;

                if (ObjPerIdentBE.peid_dFecVcto == null)
                {
                    prmParameterHeader[4] = new SqlParameter("@peid_dFecVcto", SqlDbType.DateTime);
                    prmParameterHeader[4].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[4] = new SqlParameter("@peid_dFecVcto", SqlDbType.DateTime);
                    prmParameterHeader[4].Value = ObjPerIdentBE.peid_dFecVcto;
                }

                if (ObjPerIdentBE.peid_dFecExpedicion == null)
                {
                    prmParameterHeader[5] = new SqlParameter("@peid_dFecExpedicion", SqlDbType.DateTime);
                    prmParameterHeader[5].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[5] = new SqlParameter("@peid_dFecExpedicion", SqlDbType.DateTime);
                    prmParameterHeader[5].Value = ObjPerIdentBE.peid_dFecExpedicion;
                }

                if (ObjPerIdentBE.peid_vLugarExpedicion.Length == 0)
                {
                    prmParameterHeader[6] = new SqlParameter("@peid_vLugarExpedicion", SqlDbType.VarChar, 500);
                    prmParameterHeader[6].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[6] = new SqlParameter("@peid_vLugarExpedicion", SqlDbType.VarChar, 500);
                    prmParameterHeader[6].Value = ObjPerIdentBE.peid_vLugarExpedicion;
                }

                if (ObjPerIdentBE.peid_dFecRenovacion == null)
                {
                    prmParameterHeader[7] = new SqlParameter("@peid_dFecRenovacion", SqlDbType.DateTime);
                    prmParameterHeader[7].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[7] = new SqlParameter("@peid_dFecRenovacion", SqlDbType.DateTime);
                    prmParameterHeader[7].Value = ObjPerIdentBE.peid_dFecRenovacion;
                }

                if (ObjPerIdentBE.peid_vLugarRenovacion.Length == 0)
                {
                    prmParameterHeader[8] = new SqlParameter("@peid_vLugarRenovacion", SqlDbType.VarChar, 500);
                    prmParameterHeader[8].Value = DBNull.Value;
                }
                else
                {
                    prmParameterHeader[8] = new SqlParameter("@peid_vLugarRenovacion", SqlDbType.VarChar, 500);
                    prmParameterHeader[8].Value = ObjPerIdentBE.peid_vLugarRenovacion;
                }

                prmParameterHeader[9] = new SqlParameter("@peid_bActivoEnRune", SqlDbType.Bit);
                prmParameterHeader[9].Value = ObjPerIdentBE.peid_bActivoEnRune;

                prmParameterHeader[10] = new SqlParameter("@peid_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[10].Value = IntOficinaConsularId;

                prmParameterHeader[11] = new SqlParameter("@peid_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterHeader[11].Value = ObjPerIdentBE.peid_sUsuarioModificacion;

                prmParameterHeader[12] = new SqlParameter("@peid_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[12].Value = ObjPerIdentBE.peid_vIPModificacion;

                prmParameterHeader[13] = new SqlParameter("@peid_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[13].Value = Util.ObtenerHostName();

                //prmParameterHeader[14] = new SqlParameter("@Rspta", SqlDbType.Int);
                //prmParameterHeader[14].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ACTUALIZAR",
                                                           prmParameterHeader);
                IntResultQuery = 1;
            }
            catch (Exception ex)
            {
                IntResultQuery = -1;
                throw ex;
            }

            return IntResultQuery;
        }

        public int Eliminar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                            int IntOficinaConsularId)
        {
            int IntResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[5];

                prmParameterHeader[0] = new SqlParameter("@peid_iPersonaIdentificacionId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = ObjPerIdentBE.peid_iPersonaIdentificacionId;

                prmParameterHeader[1] = new SqlParameter("@peid_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = IntOficinaConsularId;

                prmParameterHeader[2] = new SqlParameter("@peid_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterHeader[2].Value = ObjPerIdentBE.peid_sUsuarioModificacion;

                prmParameterHeader[3] = new SqlParameter("@peid_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[3].Value = ObjPerIdentBE.peid_vIPModificacion;

                prmParameterHeader[4] = new SqlParameter("@peid_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[4].Value = Util.ObtenerHostName();

                IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
                                                           CommandType.StoredProcedure,
                                                           "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ELIMINAR",
                                                           prmParameterHeader);

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}