using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brActaRecepcionCabecera:brGeneral
    {
        public short adicionar(beActaRecepcionPrincipal obeActaRecepcionPrincipal)
        {
            short idActaRecepcionCabecera = -1;
            short idActaRecepcionDetalle = -1;
            short SolicitanteId = -1;
            int MovCarneIdent = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daSolicitante odaSolicitante = new daSolicitante();
                    SolicitanteId = odaSolicitante.validarSolicitante(con, trx, obeActaRecepcionPrincipal.Solicitante);
                    if (SolicitanteId > 0)
                    {
                        obeActaRecepcionPrincipal.Solicitante.SolicitanteId = SolicitanteId;
                        //exito = odaSolicitante.actualizar(con, trx, obeActaRecepcionPrincipal.Solicitante);
                    }
                    else
                    {
                        SolicitanteId = odaSolicitante.adicionar(con, trx, obeActaRecepcionPrincipal.Solicitante);
                    }
                    if (SolicitanteId > 0) { exito = true; } else { exito = false; idActaRecepcionCabecera = -1; }
                    if (exito)
                    {
                        daActaRecepcionCabecera odaActaRecepcionCabecera = new daActaRecepcionCabecera();
                        obeActaRecepcionPrincipal.ActaCabecera.SolicitanteId = SolicitanteId;
                        idActaRecepcionCabecera = odaActaRecepcionCabecera.adicionar(con, trx, obeActaRecepcionPrincipal.ActaCabecera);
                        if (idActaRecepcionCabecera != -1) { exito = true; } else { exito = false; idActaRecepcionCabecera = -1; }
                    }
                    if (exito)
                    {
                        for (int i = 0; i <= obeActaRecepcionPrincipal.ActaDetalle.Count - 1; i++)
                        {
                            obeActaRecepcionPrincipal.ActaDetalle[i].ActaRecepcionCabId = idActaRecepcionCabecera;
                            daActaRecepcionDetalle odaActaRecepcionDetalle = new daActaRecepcionDetalle();
                            idActaRecepcionDetalle = odaActaRecepcionDetalle.adicionar(con, trx, obeActaRecepcionPrincipal.ActaDetalle[i]);
                            if (idActaRecepcionDetalle != -1) { exito = true; } else { exito = false; idActaRecepcionCabecera = -1; break; }
                        }
                    }
                    if (exito)
                    {
                        for (int i = 0; i <= obeActaRecepcionPrincipal.ListaMovimientos.Count - 1; i++)
                        {
                            daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                            MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeActaRecepcionPrincipal.ListaMovimientos[i]);
                            if (MovCarneIdent != -1) { exito = true; } else { exito = false; idActaRecepcionCabecera = -1; break; }
                        }
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
            return (idActaRecepcionCabecera);
        }

        public beActaRecepcionPrincipal consultar(beActaRecepcionCabecera parametros)
        {
            beActaRecepcionPrincipal obeActaRecepcionPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daActaRecepcionCabecera odaActaRecepcionCabecera = new daActaRecepcionCabecera();
                    obeActaRecepcionPrincipal = odaActaRecepcionCabecera.consultar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeActaRecepcionPrincipal);
        }

        public beActaRecepcionCabecera obtenerCabecera(beActaRecepcionDetalle parametros)
        {
            beActaRecepcionCabecera obeActaRecepcionCabecera = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daActaRecepcionCabecera odaActaRecepcionCabecera = new daActaRecepcionCabecera();
                    obeActaRecepcionCabecera = odaActaRecepcionCabecera.obtenerCabecera(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeActaRecepcionCabecera);
        }
    }
}
