using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Persona.DA
{
    public class PersonaFotoConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public PersonaFotoConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PersonaFotoConsultaDA()
        {
            GC.Collect();
        }

        public DataTable PersonaFotoGetFotoFirma(long LonPersonaId, int IntImagenTipo)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                SqlParameter[] prmParameterDir = new SqlParameter[2];

                prmParameterDir[0] = new SqlParameter("@pefo_iPersonaId", SqlDbType.BigInt);
                prmParameterDir[0].Value = LonPersonaId;

                prmParameterDir[1] = new SqlParameter("@pefo_sFotoTipoId", SqlDbType.SmallInt);
                prmParameterDir[1].Value = IntImagenTipo;

                DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_PERSONAFOTO_GETFOTOFIRMA",
                                                    prmParameterDir);

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