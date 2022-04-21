using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brSolicitante:brGeneral
    {
        public beSolicitante consultarxIdentificacion(beSolicitante parametros)
        {
            beSolicitante obeSolicitante = new beSolicitante();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daSolicitante odaSolicitante = new daSolicitante();
                    obeSolicitante = odaSolicitante.consultarxIdentificacion(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeSolicitante);
        }

        public short adicionar(beSolicitante parametros)
        {
            short SolicitanteId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daSolicitante odaSolicitante = new daSolicitante();
                    SolicitanteId = odaSolicitante.validarSolicitante(con, trx, parametros);
                    if (SolicitanteId > 0)
                    {
                        SolicitanteId = -2;
                        exito = false; //odaSolicitante.actualizar(con, trx, parametros);
                    }
                    else
                    {
                        SolicitanteId = odaSolicitante.adicionar(con, trx, parametros);
                        if (SolicitanteId > 0) { exito = true; } else { SolicitanteId = -1; exito = false; }
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
            return (SolicitanteId);
        }

        public short actualizar(beSolicitante parametros)
        {
            bool exito = false;
            short SolicitanteId = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daSolicitante odaSolicitante = new daSolicitante();
                    SolicitanteId = odaSolicitante.validarSolicitante(con, trx, parametros);
                    if (SolicitanteId > 0)
                    {
                        if (SolicitanteId == parametros.SolicitanteId)
                        {
                            exito = odaSolicitante.actualizar(con, trx, parametros);
                            if (!exito) { SolicitanteId = -1; }
                        }
                        else
                        {
                            SolicitanteId = -2;
                            exito = false;
                        }
                    }
                    else
                    {
                        exito = odaSolicitante.actualizar(con, trx, parametros);
                        SolicitanteId = 1;
                    }
                    if (exito) trx.Commit();
                    else trx.Rollback();
                }
                catch (SqlException ex)
                {
                    grabarLog(ex);
                }
            }
            return (SolicitanteId);
        }

        public List<beSolicitante> consultar(beSolicitante parametros)
        {
            List<beSolicitante> lbeSolicitante = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daSolicitante odaSolicitante = new daSolicitante();
                    lbeSolicitante = odaSolicitante.consultar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (lbeSolicitante);
        }
    }
}
