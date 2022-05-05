using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaAsistenciaConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public PersonaAsistenciaConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaAsistenciaConsultaDA()
        {
            GC.Collect();
        }

        public DataTable Obtener(long LonPersonaId,
                                 string StrCurrentPage,
                                 int IntPageSize,
                                 int intOficinaId,
                                 string strHotsname,
                                 int intUsuarioCreacion,
                                 string strIPCreacion,
                                 ref int IntTotalCount,
                                 ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[9];

                prmParameter[0] = new SqlParameter("@asis_iPersonaId", SqlDbType.BigInt);
                prmParameter[0].Value = LonPersonaId;

                prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
                prmParameter[1].Value = StrCurrentPage;

                prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
                prmParameter[2].Value = IntPageSize;

                prmParameter[3] = new SqlParameter("@iOficinaId", SqlDbType.Int);
                prmParameter[3].Value = intOficinaId;

                prmParameter[4] = new SqlParameter("@vHostname", SqlDbType.VarChar);
                prmParameter[4].Value = strHotsname;

                prmParameter[5] = new SqlParameter("@sUsuarioCreacion", SqlDbType.Int);
                prmParameter[5].Value = intUsuarioCreacion;

                prmParameter[6] = new SqlParameter("@vIPCreacion", SqlDbType.VarChar);
                prmParameter[6].Value = strIPCreacion;

                prmParameter[7] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
                prmParameter[7].Direction = ParameterDirection.Output;

                prmParameter[8] = new SqlParameter("@ITotalPages", SqlDbType.Int);
                prmParameter[8].Direction = ParameterDirection.Output;

                //SqlHelper.
                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_ASISTENCIA_OBTENER",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];

                if (DtResult.Rows.Count != 0)
                {
                    IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);
                    IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[8]).Value);
                }
                else
                {
                    IntTotalCount = 0;
                    IntTotalPages = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable ListarAsistenciaPAH(DateTime strFchInicio,
                                             DateTime strFchFinal,
                                             string strCodContinente,
                                             Int16? sModalidadPahId,
                                             Int16 sOficinaConsularOrigenId,
                                             Int16? sOficinaConsularId,
                                             Int16? sUsuarioId,
                                             string titular
                                             )
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[10];

                prmParameter[0] = new SqlParameter("@vFchInicio", SqlDbType.DateTime);
                prmParameter[0].Value = strFchInicio;

                prmParameter[1] = new SqlParameter("@vFchFin", SqlDbType.DateTime);
                prmParameter[1].Value = strFchFinal;

                prmParameter[2] = new SqlParameter("@vCodContinente", SqlDbType.Char, 2);
                prmParameter[2].Value = strCodContinente;

                prmParameter[3] = new SqlParameter("@sModalidadPahId", SqlDbType.SmallInt);
                prmParameter[3].Value = sModalidadPahId;

                prmParameter[4] = new SqlParameter("@sOficinaConsularOrigenId ", SqlDbType.SmallInt);
                prmParameter[4].Value = sOficinaConsularOrigenId;

                prmParameter[5] = new SqlParameter("@asis_sOficinaConsularId ", SqlDbType.SmallInt);
                prmParameter[5].Value = sOficinaConsularId;

                prmParameter[6] = new SqlParameter("@asis_sUsuarioId", SqlDbType.SmallInt);
                prmParameter[6].Value = sUsuarioId;

                prmParameter[7] = new SqlParameter("@titular", SqlDbType.VarChar, 100);
                prmParameter[7].Value = titular;

                prmParameter[8] = new SqlParameter("@asis_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[8].Value = Util.ObtenerDireccionIP();

                prmParameter[9] = new SqlParameter("@asis_vHostName", SqlDbType.VarChar, 20);
                prmParameter[9].Value = Util.ObtenerHostName();

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REPORTES.USP_RP_ASISTENCIA_PAH",
                                                    prmParameter);
                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable ListarAsistenciaPALH(DateTime strFchInicio,
                                              DateTime strFchFinal,
                                              string strCodContinente,
                                              Int16? sModalidadPahlId,
                                              Int16 sOficinaConsularOrigenId,
                                              Int16? sOficinaConsularId,
                                              Int16? sUsuarioId,
                                              string titular
                                             )
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[10];

                prmParameter[0] = new SqlParameter("@vFchInicio", SqlDbType.DateTime);
                prmParameter[0].Value = strFchInicio;

                prmParameter[1] = new SqlParameter("@vFchFin", SqlDbType.DateTime);
                prmParameter[1].Value = strFchFinal;

                prmParameter[2] = new SqlParameter("@vCodContinente", SqlDbType.Char, 2);
                prmParameter[2].Value = strCodContinente;

                prmParameter[3] = new SqlParameter("@sModalidadPahlId", SqlDbType.SmallInt);
                prmParameter[3].Value = sModalidadPahlId;

                prmParameter[4] = new SqlParameter("@sOficinaConsularOrigenId ", SqlDbType.SmallInt);
                prmParameter[4].Value = sOficinaConsularOrigenId;

                prmParameter[5] = new SqlParameter("@asis_sOficinaConsularId ", SqlDbType.SmallInt);
                prmParameter[5].Value = sOficinaConsularId;

                prmParameter[6] = new SqlParameter("@asis_sUsuarioId", SqlDbType.SmallInt);
                prmParameter[6].Value = sUsuarioId;

                prmParameter[7] = new SqlParameter("@titular", SqlDbType.VarChar, 100);
                prmParameter[7].Value = titular;

                prmParameter[8] = new SqlParameter("@asis_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameter[8].Value = Util.ObtenerDireccionIP();

                prmParameter[9] = new SqlParameter("@asis_vHostName", SqlDbType.VarChar, 20);
                prmParameter[9].Value = Util.ObtenerHostName();

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REPORTES.USP_RP_ASISTENCIA_PALH",
                                                    prmParameter);
                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable ListarAsistenciaBeneficiario(Int64 IdAsistencia, Int64 personaId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[2];

                prmParameter[0] = new SqlParameter("@asbe_iAsistenciaId", SqlDbType.BigInt);
                prmParameter[0].Value = IdAsistencia;
                prmParameter[1] = new SqlParameter("@personaId", SqlDbType.BigInt);
                prmParameter[1].Value = personaId;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_ASISTENCIABENEFICIARIO_CONSULTAR",
                                                    prmParameter);
                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }
    }
}