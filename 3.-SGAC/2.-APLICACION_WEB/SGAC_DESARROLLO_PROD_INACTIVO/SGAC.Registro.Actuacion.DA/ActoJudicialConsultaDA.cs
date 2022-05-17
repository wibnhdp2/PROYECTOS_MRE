using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.BE;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoJudicialConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoJudicialConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoJudicialConsultaDA()
        {
            GC.Collect();
        }

    }
}