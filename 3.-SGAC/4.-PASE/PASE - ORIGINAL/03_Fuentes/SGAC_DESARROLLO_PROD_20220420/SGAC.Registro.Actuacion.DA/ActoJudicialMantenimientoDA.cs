using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoJudicialMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActoJudicialMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoJudicialMantenimientoDA()
        {
            GC.Collect();
        }
    }
}