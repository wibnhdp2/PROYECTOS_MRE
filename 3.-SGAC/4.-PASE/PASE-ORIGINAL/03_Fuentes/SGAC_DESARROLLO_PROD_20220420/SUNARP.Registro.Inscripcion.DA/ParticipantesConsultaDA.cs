using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class ParticipantesConsultaDA
    {
        private string strConnectionName = string.Empty;

        public ParticipantesConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ParticipantesConsultaDA()
        {
            GC.Collect();
        }
        public DataTable ParticipantesConsulta(Int64 intSolicitudId, Int64 intParticipanteId)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_PARTICIPANTES_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_PART_ISOLICITUDINSCRIPCIONID", intSolicitudId));
                        cmd.Parameters.Add(new SqlParameter("@P_PART_IPARTICIPANTEID", intParticipanteId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }

    }
}
