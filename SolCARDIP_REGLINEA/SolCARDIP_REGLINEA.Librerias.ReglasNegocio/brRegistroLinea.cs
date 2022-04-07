using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP_REGLINEA.Librerias.AccesoDatos;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;

namespace SolCARDIP_REGLINEA.Librerias.ReglasNegocio
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
                }
                catch (SqlException exsql)
                {
                    obeRegistroLinea = null;
                    grabarLog(exsql);
                }
            }
            return (obeRegistroLinea);
        }
        public List<beRegistroLinea> consultarRegistroTrazabilidad(beRegistroLinea parametros)
        {
            
            List<beRegistroLinea> list = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    list = odaRegistroLinea.consultarRegistroTrazabilidad(con, parametros);
                }
                catch (SqlException exsql)
                {
                    list = null;
                    grabarLog(exsql);
                }
            }
            return (list);
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
        public bool actualizarDetalleEstadoEnviado(beRegistroLinea parametros)
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
                    exito = odaRegistroLinea.actualizarDetalleEstadoEnviado(con, trx, parametros);
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
        public string VerificarInformacionSAM(string numeroPasaporte, string apePaterno, string apeMaterno, string nombres, DateTime fechaNac)
        {
            string resultado = "NO";
            
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroLinea odaRegistroLinea = new daRegistroLinea();
                    resultado = odaRegistroLinea.VerificarInformacionSAM(con, numeroPasaporte, apePaterno, apeMaterno, nombres, fechaNac);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return (resultado);
        }
    }
}
