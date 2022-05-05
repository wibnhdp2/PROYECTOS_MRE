using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brActaConformidadCabecera:brGeneral
    {
        public short adicionar(beActaConformidadPrincipal obeActaConformidadPrincipal)
        {
            short idActaConformidadCabecera = -1;
            short idActaConformidadDetalle = -1;
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
                    SolicitanteId = odaSolicitante.validarSolicitante(con, trx, obeActaConformidadPrincipal.Solicitante);
                    if (SolicitanteId > 0)
                    {
                        obeActaConformidadPrincipal.Solicitante.SolicitanteId = SolicitanteId;
                        //exito = odaSolicitante.actualizar(con, trx, obeActaConformidadPrincipal.Solicitante);
                    }
                    else
                    {
                        SolicitanteId = odaSolicitante.adicionar(con, trx, obeActaConformidadPrincipal.Solicitante);
                    }
                    if (SolicitanteId > 0) { exito = true; } else { exito = false; idActaConformidadCabecera = -1; }
                    if (exito)
                    {
                        daActaConformidadCabecera odaActaConformidadCabecera = new daActaConformidadCabecera();
                        obeActaConformidadPrincipal.ActaCabecera.SolicitanteId = SolicitanteId;
                        idActaConformidadCabecera = odaActaConformidadCabecera.adicionar(con, trx, obeActaConformidadPrincipal.ActaCabecera);
                        if (idActaConformidadCabecera != -1) { exito = true; } else { exito = false; idActaConformidadCabecera = -1; }
                    }
                    if (exito)
                    {
                        for(int i = 0; i <= obeActaConformidadPrincipal.ActaDetalle.Count - 1; i++)
                        {
                            obeActaConformidadPrincipal.ActaDetalle[i].ActaConformidadCabId = idActaConformidadCabecera;
                            daActaConformidadDetalle odaActaConformidadDetalle = new daActaConformidadDetalle();
                            idActaConformidadDetalle = odaActaConformidadDetalle.adicionar(con, trx, obeActaConformidadPrincipal.ActaDetalle[i]);
                            if (idActaConformidadDetalle != -1) { exito = true; } else { exito = false; idActaConformidadCabecera = -1; break; }
                        }
                    }
                    if (exito)
                    {
                        for (int i = 0; i <= obeActaConformidadPrincipal.ListaMovimientos.Count - 1; i++)
                        {
                            daMovimientoCarneIdentidad odaMovimientoCarneIdentidad = new daMovimientoCarneIdentidad();
                            MovCarneIdent = odaMovimientoCarneIdentidad.adicionar(con, trx, obeActaConformidadPrincipal.ListaMovimientos[i]);
                            if (MovCarneIdent != -1) { exito = true; } else { exito = false; idActaConformidadCabecera = -1; break; }
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
            return (idActaConformidadCabecera);
        }

        public beActaConformidadPrincipal consultar(beActaConformidadCabecera parametros)
        {
            beActaConformidadPrincipal obeActaConformidadPrincipal = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daActaConformidadCabecera odaActaConformidadCabecera = new daActaConformidadCabecera();
                    obeActaConformidadPrincipal = odaActaConformidadCabecera.consultar(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeActaConformidadPrincipal);
        }
    }
}
