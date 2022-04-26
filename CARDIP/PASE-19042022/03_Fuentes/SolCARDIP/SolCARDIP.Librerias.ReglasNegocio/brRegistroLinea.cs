using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brRegistroLinea:brGeneral
    {
        public int adicionar(beRegistroLinea parametros)
        {
            int idRegistroLinea = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    idRegistroLinea = odaRegistroLinea.adicionar(con, trx, parametros);
                    if (idRegistroLinea != -1) trx.Commit();
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
            return (idRegistroLinea);
        }

        public bool actualizar(beRegistroLinea parametros)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    exito = odaRegistroLinea.actualizar(con, trx, parametros);
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
            return (exito);
        }

        public beRegistroLinea obtenerNumero(beRegistroLinea parametros)
        {
            beRegistroLinea obeRegistroLinea = new beRegistroLinea();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeRegistroLinea = odaRegistroLinea.obtenerNumero(con, parametros);
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (obeRegistroLinea);
        }

        public beCarneIdentidadPrincipal obtenerCarneId(beCarneIdentidad parametros)
        {
            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeCarneIdentidad = odaRegistroLinea.obtenerCarneId(con, parametros);
                    if (obeCarneIdentidad != null)
                    {
                        obeCarneIdentidadPrincipal = odaRegistroLinea.consultarCarne(con, obeCarneIdentidad);
                    }
                }
                catch (SqlException exsql)
                {
                    grabarLog(exsql);
                }
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidadPrincipal obtenerRelacionDependencia(beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeCarneIdentidadPrincipal = odaRegistroLinea.obtenerRelacionDependencia(con, parametrosCarneIdentidad);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidadPrincipal);
        }
        public DataTable consultarRegistroDT(beRegistroLinea parametros)
        {
                DataTable dt = new DataTable();
                daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                using (SqlConnection con = new SqlConnection(CadenaConexion))
            
                try
                {
                    con.Open();
                    dt = odaRegistroLinea.ConsultarRegistroDT(con, parametros);
                }
                catch (Exception excp)
                {
                    grabarLog(excp);
                }
                return dt;
            }
        public beRegistroLinea consultarRegistro(beRegistroLinea parametros)
        {
            beRegistroLinea obeRegistroLinea = new beRegistroLinea();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeRegistroLinea = odaRegistroLinea.consultarRegistro(con, parametros);
                    if (obeRegistroLinea != null)
                    {
                        if(obeRegistroLinea.DpReldepTitular > 0)
                        {
                            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                            obeCarneIdentidad = odaRegistroLinea.consultarCarnexId(con, obeRegistroLinea.DpReldepTitular);
                            if (obeCarneIdentidad != null)
                            {
                                obeRegistroLinea.ConNumeroCarne = obeCarneIdentidad.CarneNumero;
                            }
                        }
                    }
                }
                catch (SqlException exsql)
                {
                    obeRegistroLinea = null;
                    grabarLog(exsql);
                }
            }
            return (obeRegistroLinea);
        }
        public beRegistroLinea consultarRegistroLineaPorIdCarnet(int caid_icarne_identidadid)
        {
            beRegistroLinea obeRegistroLinea = new beRegistroLinea();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeRegistroLinea = odaRegistroLinea.consultarRegistroLineaPorIdCarnet(con, caid_icarne_identidadid);
                    
                }
                catch (SqlException exsql)
                {
                    obeRegistroLinea = null;
                    grabarLog(exsql);
                }
            }
            return (obeRegistroLinea);
        }

        public beCarneIdentidad consultarCarnexId(short carneID)
        {
            beCarneIdentidad obeCarneIdentidad = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    obeCarneIdentidad = odaRegistroLinea.consultarCarnexId(con, carneID);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeCarneIdentidad);
        }

        public bool actualizarEstado(beRegistroLinea parametros)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    exito = odaRegistroLinea.actualizarEstado(con, trx, parametros);
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
            return (exito);
        }
    }
}
