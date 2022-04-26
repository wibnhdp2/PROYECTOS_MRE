using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brCalidadMigratoria:brGeneral
    {
        public short adicionar(beCalidadMigratoria parametrosCalidadMigratoriaPri, beCalidadMigratoria parametrosCalidadMigratoriaSec)
        {
            short CalidadMigratoria = -1;
            short CalidadMigratoriaPri = -1;
            short CalidadMigratoriaSec = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCalidadMigratoria odaCalidadMigratoria = new daCalidadMigratoria();
                    CalidadMigratoria = odaCalidadMigratoria.validarCalidadMigratoria(con, trx, parametrosCalidadMigratoriaPri);
                    if (CalidadMigratoria == 0) { exito = true; } else { exito = false; CalidadMigratoriaPri = -2; }
                    if (exito)
                    {
                        CalidadMigratoriaPri = odaCalidadMigratoria.adicionarPri(con, trx, parametrosCalidadMigratoriaPri);
                        if (CalidadMigratoriaPri != -1) { exito = true; } else { exito = false; CalidadMigratoriaPri = -1; }
                    }
                    //if (exito)
                    //{
                    //    CalidadMigratoria = -1;
                    //    CalidadMigratoria = odaCalidadMigratoria.validarCalidadMigratoria(con, trx, parametrosCalidadMigratoriaSec);
                    //    if (CalidadMigratoria == 0) { exito = true; } else { exito = false; CalidadMigratoriaPri = -3; }
                    //}
                    if (exito)
                    {
                        parametrosCalidadMigratoriaSec.ReferenciaId = CalidadMigratoriaPri;
                        CalidadMigratoriaSec = odaCalidadMigratoria.adicionarCargo(con, trx, parametrosCalidadMigratoriaSec);
                        if (CalidadMigratoriaSec != -1) { exito = true; } else { exito = false; CalidadMigratoriaPri = -1; }
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
            return (CalidadMigratoriaPri);
        }

        public short adicionarCargo(beCalidadMigratoria parametros)
        {
            short cargoId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCalidadMigratoria odaCalidadMigratoria = new daCalidadMigratoria();
                    cargoId = odaCalidadMigratoria.validarCargo(con, trx, parametros);
                    if (cargoId == 0) { exito = true; } else { exito = false; cargoId = -2; }
                    if(exito)
                    {
                        cargoId = odaCalidadMigratoria.adicionarCargo(con, trx, parametros);
                        if (cargoId != -1) { exito = true; } else { exito = false; cargoId = -1; }
                    }
                    if (exito) { trx.Commit(); }
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
            return (cargoId);
        }

        public short actualizarCargo(beCalidadMigratoria parametros)
        {
            short cargoId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daCalidadMigratoria odaCalidadMigratoria = new daCalidadMigratoria();
                    cargoId = odaCalidadMigratoria.validarCargo(con, trx, parametros);
                    if (cargoId == 0) { exito = true; } 
                    else 
                    {
                        if (cargoId != parametros.CalidadMigratoriaid)
                        {
                            exito = false;
                            cargoId = -2;
                        }
                        else
                        {
                            exito = true;
                        }
                    }
                    if (exito)
                    {
                        exito = odaCalidadMigratoria.actualizarCargo(con, trx, parametros);
                        if (exito) { cargoId = 1;   } else { exito = false; cargoId = -1; }
                    }
                    if (exito) { trx.Commit(); }
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
            return (cargoId);
        }
    }
}
