using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoJudicialNotificacionConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoJudicialNotificacionConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoJudicialNotificacionConsultaDA()
        {
            GC.Collect();
        }

    }
}