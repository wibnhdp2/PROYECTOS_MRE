using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.AccesoDatos;

namespace SolCARDIP_REGLINEA.Librerias.ReglasNegocio
{
    public class brGenerales:brGeneral
    {
        public beGenerales obtenerGenerales()
        {
            beGenerales obeGenerales = new beGenerales();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daGenerales odaGenerales = new daGenerales();
                    obeGenerales = odaGenerales.obtenerGenerales(con);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeGenerales);
        }
    }
}
