using SolCARDIP.Librerias.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brMensajeEstado : brGeneral
    {
        public short adicionar(beMensajeEstado parametros,string operacion)
        {
            short id = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daMensajeEstado meest = new daMensajeEstado();
                    id = meest.adicionar(con, trx, parametros, operacion);
                    
                    if (id>0) trx.Commit();
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
            return (id);
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
    }
}
