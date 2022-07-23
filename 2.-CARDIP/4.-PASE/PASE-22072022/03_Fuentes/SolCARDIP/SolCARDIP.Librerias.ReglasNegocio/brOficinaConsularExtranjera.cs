using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brOficinaConsularExtranjera:brGeneral
    {
        public short adicionar(beOficinaconsularExtranjera parametros)
        {
            short idOficinaconsularExtranjera = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daOficinaConsularExtranjera odaOficinaConsularExtranjera = new daOficinaConsularExtranjera();
                    idOficinaconsularExtranjera = odaOficinaConsularExtranjera.validar(con, trx, parametros);
                    if (idOficinaconsularExtranjera > 0)
                    {
                        idOficinaconsularExtranjera = -2;
                        exito = false;
                    }
                    else
                    {
                        idOficinaconsularExtranjera = odaOficinaConsularExtranjera.adicionar(con, trx, parametros);
                        if (idOficinaconsularExtranjera > 0) { exito = true; } else { idOficinaconsularExtranjera = -1; exito = false; }
                    }
                    if (exito) trx.Commit();
                    else
                    {
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (idOficinaconsularExtranjera);
        }

        public short actualizar(beOficinaconsularExtranjera parametros)
        {
            bool exito = false;
            short idOficinaconsularExtranjera = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daOficinaConsularExtranjera odaOficinaConsularExtranjera = new daOficinaConsularExtranjera();
                    idOficinaconsularExtranjera = odaOficinaConsularExtranjera.validar(con, trx, parametros);
                    if (idOficinaconsularExtranjera > 0)
                    {
                        if (idOficinaconsularExtranjera == parametros.OficinaconsularExtranjeraid)
                        {
                            exito = odaOficinaConsularExtranjera.actualizar(con, trx, parametros);
                            if (!exito) { idOficinaconsularExtranjera = -1; }
                        }
                        else
                        {
                            idOficinaconsularExtranjera = -2;
                            exito = false;
                        }
                    }
                    else
                    {
                        exito = odaOficinaConsularExtranjera.actualizar(con, trx, parametros);
                        idOficinaconsularExtranjera = 1;
                    }
                    if (exito) trx.Commit();
                    else trx.Rollback();
                }
                catch (SqlException ex)
                {
                    grabarLog(ex);
                }
            }
            return (idOficinaconsularExtranjera);
        }

        public List<beOficinaconsularExtranjera> consultar(beOficinaconsularExtranjera parametros)
        {
            List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daOficinaConsularExtranjera odaOficinaConsularExtranjera = new daOficinaConsularExtranjera();
                    lbeOficinaconsularExtranjera = odaOficinaConsularExtranjera.consultar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (lbeOficinaconsularExtranjera);
        } 
    }
}
