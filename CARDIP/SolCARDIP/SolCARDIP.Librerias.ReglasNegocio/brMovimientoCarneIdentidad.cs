using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;
namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brMovimientoCarneIdentidad:brGeneral
    {
        public List<beMovimientoCarneIdentidad> consultarMovimientos(beMovimientoCarneIdentidad parametrosMovimientos)
        {
            List<beMovimientoCarneIdentidad> lbeMovimientoCarneIdentidad = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                    lbeMovimientoCarneIdentidad = odaMovimientoCarneIdentidad.consultarMovimientos(con, parametrosMovimientos);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (lbeMovimientoCarneIdentidad);
        }
    }
}
