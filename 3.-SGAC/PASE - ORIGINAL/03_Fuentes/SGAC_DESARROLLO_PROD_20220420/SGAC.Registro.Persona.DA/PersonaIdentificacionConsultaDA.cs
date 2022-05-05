using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaIdentificacionConsultaDA
    {
        private string strConnectionName = string.Empty;

        public PersonaIdentificacionConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaIdentificacionConsultaDA()
        {
            GC.Collect();
        }

        public DataTable Consultar(long LonPersonaId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[1];

                prmParameter[0] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameter[0].Value = LonPersonaId;

                DsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_CONSULTAR",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public DataTable Obtener(long LonPersonaId, int IntDocumentoTipoId)
        {
            BE.RE_PERSONAIDENTIFICACION ObjPersIDentBE = new BE.RE_PERSONAIDENTIFICACION();

            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[2];

                prmParameter[0] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameter[0].Value = LonPersonaId;

                prmParameter[1] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameter[1].Value = IntDocumentoTipoId;

                DsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_OBTENER",
                                                    prmParameter);

                DtResult = DsResult.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        public int Existe(int IntTipoDocumentoId, string StrNroDocumento, long LonPersonaID, int IntOperacion)
        {
            int IntResult = 0;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[5];

                prmParameter[0] = new SqlParameter("@Rspta", SqlDbType.Int);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@peid_sDocumentoTipoId", SqlDbType.SmallInt);
                prmParameter[1].Value = IntTipoDocumentoId;

                prmParameter[2] = new SqlParameter("@peid_vDocumentoNumero", SqlDbType.VarChar, 20);
                prmParameter[2].Value = StrNroDocumento;

                prmParameter[3] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameter[3].Value = LonPersonaID;

                prmParameter[4] = new SqlParameter("@IOperacion", SqlDbType.Int);
                prmParameter[4].Value = IntOperacion;

                SqlHelper.ExecuteNonQuery(strConnectionName,
                                          CommandType.StoredProcedure,
                                          "PN_REGISTRO.USP_RE_DOCUMENTO_EXISTE",
                                           prmParameter);

                IntResult = (int)prmParameter[0].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IntResult;
        }

        public int ActivoRune(long LonPersonaId)
        {
            int IntResult = 0;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[2];

                prmParameter[0] = new SqlParameter("@Rspta", SqlDbType.Int);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@peid_iPersonaId", SqlDbType.BigInt);
                prmParameter[1].Value = LonPersonaId;

                SqlHelper.ExecuteNonQuery(strConnectionName,
                                          CommandType.StoredProcedure,
                                          "PN_REGISTRO.USP_RE_PERSONAIDENTIFICACION_ACTIVORUNE",
                                           prmParameter);

                IntResult = (int)prmParameter[0].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IntResult;
        }
    }
}