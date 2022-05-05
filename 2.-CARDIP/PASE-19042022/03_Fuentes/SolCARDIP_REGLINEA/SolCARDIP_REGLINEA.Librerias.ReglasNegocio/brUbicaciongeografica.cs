using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.AccesoDatos;

namespace SolCARDIP_REGLINEA.Librerias.ReglasNegocio
{
    public class brUbicaciongeografica:brGeneral
    {
        public beUbigeoListas obtenerGenerales(beUbicaciongeografica parametros)
        {
            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daUbicaciongeografica odaUbicaciongeografica = new daUbicaciongeografica();
                    obeUbigeoListas = odaUbicaciongeografica.obtenerUbiGeo(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeUbigeoListas);
        }
    }
}
