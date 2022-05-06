using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Persona.DA
{
    public class ParticipanteConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ParticipanteConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ParticipanteConsultaDA()
        {
            GC.Collect();
        }

        public long ObtenerIdParticipante(long LonActuacionDetalleId, int IntTipoParticipante, int IntTipoActo)
        {
            long LonResult = 0;

            try
            {
                SqlParameter[] prmParameter = new SqlParameter[4];

                prmParameter[0] = new SqlParameter("@PersonaId", SqlDbType.BigInt);
                prmParameter[0].Direction = ParameterDirection.Output;

                prmParameter[1] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
                prmParameter[1].Value = LonActuacionDetalleId;

                prmParameter[2] = new SqlParameter("@sTipoParticipante", SqlDbType.SmallInt);
                prmParameter[2].Value = IntTipoParticipante;

                prmParameter[3] = new SqlParameter("@sTipoActo", SqlDbType.SmallInt);
                prmParameter[3].Value = IntTipoActo;

                SqlHelper.ExecuteNonQuery(StrConnectionName,
                                          CommandType.StoredProcedure,
                                          "PN_REGISTRO.USP_RE_PARTICIPANTE_OBTENER_POR_ID",
                                           prmParameter);

                LonResult = (long)prmParameter[0].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return LonResult;
        }
    }
}