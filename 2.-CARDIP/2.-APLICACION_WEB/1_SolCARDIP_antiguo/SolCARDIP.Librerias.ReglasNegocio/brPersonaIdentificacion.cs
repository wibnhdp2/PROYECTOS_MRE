using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brPersonaIdentificacion:brGeneral
    {
        //public long validarPersonaIdentificacion(bePersonaidentificacion parametrosPersonaIdent)
        //{
        //    long PersonaIdentId = -1;
        //    using (SqlConnection con = new SqlConnection(CadenaConexion))
        //    {
        //        try
        //        {
        //            con.Open();
        //            daPersonaIdentificacion odaPersonaIdentificacion = new daPersonaIdentificacion();
        //            PersonaIdentId = odaPersonaIdentificacion.validarPersonaIdentificacion(con, parametrosPersonaIdent);
        //        }
        //        catch (Exception ex)
        //        {
        //            grabarLog(ex);
        //        }
        //    }
        //    return (PersonaIdentId);
        //}
    }
}
