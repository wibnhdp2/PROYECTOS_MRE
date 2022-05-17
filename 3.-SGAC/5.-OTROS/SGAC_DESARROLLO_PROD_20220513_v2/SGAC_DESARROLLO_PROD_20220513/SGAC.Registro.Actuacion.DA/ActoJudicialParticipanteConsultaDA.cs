using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoJudicialParticipanteConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoJudicialParticipanteConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoJudicialParticipanteConsultaDA()
        {
            GC.Collect();
        }
    }
}