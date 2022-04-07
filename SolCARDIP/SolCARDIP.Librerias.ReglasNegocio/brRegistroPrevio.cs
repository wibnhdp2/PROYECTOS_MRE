using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brRegistroPrevio:brGeneral
    {
        public short adicionar(beRegistroPrevioListas parametros)
        {
            bool exito = false;
            short idRegistroPrevio = -1;
            int MovCarneIdent = -1;
            short SolicitanteId = -1;
            short idActaRecepcionCabecera = -1;
            short idActaRecepcionDetalle = -1;
            short idFinal = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                    idRegistroPrevio = odaRegistroPrevio.adicionar(con, trx, parametros.RegistroPrevio);
                    if (idRegistroPrevio != -1) { idFinal = idRegistroPrevio; exito = true; } else { exito = false; idRegistroPrevio = -1; }
                    if (exito)
                    {
                        daSolicitante odaSolicitante = new daSolicitante();
                        if (parametros.Solicitante.TipoDocumentoIdentidadId > 0)
                        {
                            SolicitanteId = odaSolicitante.validarSolicitante(con, trx, parametros.Solicitante);
                            if (SolicitanteId > 0)
                            {
                                exito = true;
                            }
                            else
                            {
                                SolicitanteId = odaSolicitante.adicionar(con, trx, parametros.Solicitante);
                                if (SolicitanteId > 0) { exito = true; } else { exito = false; idFinal = -1; }
                            }
                            if (SolicitanteId > 0)
                            {
                                parametros.RecepcionCabecera.SolicitanteId = SolicitanteId;
                                daActaRecepcionCabecera odaActaRecepcionCabecera = new daActaRecepcionCabecera();
                                idActaRecepcionCabecera = odaActaRecepcionCabecera.adicionar(con, trx, parametros.RecepcionCabecera);
                                if (idActaRecepcionCabecera != -1) { exito = true; } else { exito = false; idFinal = -1; }
                            }
                            if (exito)
                            {
                                daActaRecepcionDetalle odaActaRecepcionDetalle = new daActaRecepcionDetalle();
                                parametros.RecepcionDetalle.ActaRecepcionCabId = idActaRecepcionCabecera;
                                parametros.RecepcionDetalle.CarneIdentidadId = idRegistroPrevio;
                                idActaRecepcionDetalle = odaActaRecepcionDetalle.adicionar(con, trx, parametros.RecepcionDetalle);
                                if (idActaRecepcionDetalle != -1) { exito = true; } else { exito = false; idFinal = -1; }
                            }
                        }
                        else
                        {
                            exito = true;
                        }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        parametros.MovimientoCarne.CarneIdentidadid = idRegistroPrevio;
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, parametros.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; idFinal = -1; }
                    }
                    if (exito) { trx.Commit(); }
                    else
                    {
                        idFinal = -1;
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    idFinal = -1;
                    grabarLog(exsql);
                }
            }
            return (idFinal);
        }

        public short actualizar(beRegistroPrevioListas parametros)
        {
            bool exito = false;
            int MovCarneIdent = -1;
            short SolicitanteId = -1;
            short idRegistroPrevio = -1;
            short idActaRecepcionCabecera = -1;
            short idActaRecepcionDetalle = -1;
            short idFinal = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                    exito = odaRegistroPrevio.actualizar(con, trx, parametros.RegistroPrevio);
                    if (exito)
                    {
                        idFinal = parametros.RegistroPrevio.RegistroPrevioId;
                        daSolicitante odaSolicitante = new daSolicitante();
                        if (parametros.Solicitante.TipoDocumentoIdentidadId > 0)
                        {
                            SolicitanteId = odaSolicitante.validarSolicitante(con, trx, parametros.Solicitante);
                            if (SolicitanteId > 0)
                            {
                                parametros.RegistroPrevio.SolicitanteId = SolicitanteId;
                                exito = true;
                            }
                            else
                            {
                                SolicitanteId = odaSolicitante.adicionar(con, trx, parametros.Solicitante);
                                if (SolicitanteId > 0) { parametros.RegistroPrevio.SolicitanteId = SolicitanteId; exito = true; } else { exito = false; idFinal = -1; }
                            }
                            if (SolicitanteId > 0)
                            {
                                parametros.RecepcionCabecera.SolicitanteId = SolicitanteId;
                                daActaRecepcionCabecera odaActaRecepcionCabecera = new daActaRecepcionCabecera();
                                idActaRecepcionCabecera = odaActaRecepcionCabecera.adicionar(con, trx, parametros.RecepcionCabecera);
                                if (idActaRecepcionCabecera != -1) { idFinal = idActaRecepcionCabecera; exito = true; } else { exito = false; idFinal = -1; }
                            }
                            if (exito)
                            {
                                daActaRecepcionDetalle odaActaRecepcionDetalle = new daActaRecepcionDetalle();
                                parametros.RecepcionDetalle.ActaRecepcionCabId = idActaRecepcionCabecera;
                                parametros.RecepcionDetalle.CarneIdentidadId = parametros.RegistroPrevio.ConCarneIdentidadId;
                                idActaRecepcionDetalle = odaActaRecepcionDetalle.adicionar(con, trx, parametros.RecepcionDetalle);
                                if (idActaRecepcionDetalle != -1) { exito = true; } else { exito = false; idFinal = -1; }
                            }
                        }
                        else
                        {
                            exito = true;
                        }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, parametros.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; idFinal = -1; }
                    }
                    if (exito) { trx.Commit(); }
                    else
                    {
                        idFinal = -1;
                        trx.Rollback();
                    }
                }
                catch (SqlException exsql)
                {
                    idFinal = -1;
                    grabarLog(exsql);
                }
            }
            return (idFinal);
        }

        public bool agregarSolicitante(beRegistroPrevioListas parametros)
        {
            bool exito = false;
            int MovCarneIdent = -1;
            short SolicitanteId = -1;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daSolicitante odaSolicitante = new daSolicitante();
                    SolicitanteId = odaSolicitante.validarSolicitante(con, trx, parametros.Solicitante);
                    if (SolicitanteId > 0)
                    {
                        exito = true;
                    }
                    else
                    {
                        SolicitanteId = odaSolicitante.adicionar(con, trx, parametros.Solicitante);
                        if (SolicitanteId > 0) { exito = true; } else { exito = false; }
                    }
                    if (exito)
                    {
                        foreach (beRegistroPrevio obeRegistroPrevio in parametros.listaRegistros)
                        {
                            obeRegistroPrevio.SolicitanteId = SolicitanteId;
                            daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                            exito = odaRegistroPrevio.agregarSolicitante(con, trx, parametros.RegistroPrevio);
                        }
                    }
                    if (exito)
                    {
                        daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                        MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, parametros.MovimientoCarne);
                        if (MovCarneIdent != -1) { exito = true; } else { exito = false; }
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
            return (exito);
        }

        public beRegistroPrevioListas consultar(beRegistroPrevio parametros)
        {
            beRegistroPrevioListas obeRegistroPrevioListas = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                    obeRegistroPrevioListas = odaRegistroPrevio.consultar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeRegistroPrevioListas);
        }

        public beRegistroPrevio consultarRegistroEdicion(beRegistroPrevio parametros)
        {
            beRegistroPrevio obeRegistroPrevio = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daRegistroPrevio odaRegistroPrevio = new daRegistroPrevio();
                    obeRegistroPrevio = odaRegistroPrevio.consultarRegistroEdicion(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeRegistroPrevio);
        }
    }
}
