using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brUsuario:brGeneral
    {
        public List<beUsuario> listaUsuarios(short Oficina)
        {
            List<beUsuario> lista = new List<beUsuario>();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daUsuario odaUsuario = new daUsuario();
                    lista = odaUsuario.listaUsuarios(con, AbrevSistema, Oficina);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (lista);
        }

        public bool bloqueoActiva(beUsuario parametros_Crea, beUsuario parametros_Bloq, string comentario)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daUsuario odaUsuario = new daUsuario();
                    exito = odaUsuario.bloqueoActiva(con, trx, parametros_Crea, parametros_Bloq, comentario);
                    if (exito) trx.Commit();
                    else trx.Rollback();
                }
                catch (SqlException ex)
                {
                    grabarLog(ex);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (exito);
        }

        public List<beUsuario> obtenerInfoUsuarios(beUsuario parametros)
        {
            List<beUsuario> listaUsuarios = new List<beUsuario>();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daUsuario odaUsuario = new daUsuario();
                    listaUsuarios = odaUsuario.obtenerInfoUsuarios(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (listaUsuarios);
        }

        public short adicionar(beUsuario parametrosUsuario, beUsuarioRol parametrosUsuarioRol)
        {
            short UsuarioId = -1;
            short UsuarioRolId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daUsuario odaUsuario = new daUsuario();
                    UsuarioId = odaUsuario.adicionar(con, trx, parametrosUsuario);
                    if (UsuarioId == -1) { exito = false; UsuarioId = -1; } else { exito = true; }
                    if (exito)
                    {
                        daUsuarioRol odaUsuarioRol = new daUsuarioRol();
                        parametrosUsuarioRol.Usuarioid = UsuarioId;
                        UsuarioRolId = odaUsuarioRol.adicionar(con, trx, parametrosUsuarioRol);
                        if (UsuarioRolId < 0) { exito = false; UsuarioId = -2; } else { exito = true; }
                    }
                    if (exito) trx.Commit();
                    else trx.Rollback();
                }
                catch (SqlException ex)
                {
                    grabarLog(ex);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (UsuarioId);
        }

        public bool actualizar(beUsuario parametrosUsuario, beUsuarioRol parametrosUsuarioRol)
        {
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daUsuario odaUsuario = new daUsuario();
                    exito = odaUsuario.actualizar(con, trx, parametrosUsuario);
                    if (exito)
                    {
                        daUsuarioRol odaUsuarioRol = new daUsuarioRol();
                        exito = odaUsuarioRol.actualizar(con, trx, parametrosUsuarioRol);
                    }
                    if (exito) trx.Commit();
                    else trx.Rollback();
                }
                catch (SqlException ex)
                {
                    grabarLog(ex);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (exito);
        }
    }
}
