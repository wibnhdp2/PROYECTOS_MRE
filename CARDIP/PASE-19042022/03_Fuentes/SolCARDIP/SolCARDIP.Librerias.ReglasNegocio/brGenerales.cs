using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
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
