using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brUsuarioRol:brGeneral
    {
        public beUsuarioRol usuarioAutenticar(beUsuarioRol parametros)
        {
            beUsuarioRol obeUsuarioRol = new beUsuarioRol();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daUsuarioRol odaUsuarioRol = new daUsuarioRol();
                    obeUsuarioRol = odaUsuarioRol.usuarioAutenticar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeUsuarioRol);
        }
    }
}
