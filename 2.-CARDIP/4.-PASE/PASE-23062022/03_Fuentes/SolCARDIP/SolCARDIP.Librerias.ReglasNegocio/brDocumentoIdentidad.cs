using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brDocumentoIdentidad:brGeneral
    {
        public short adicionar(beDocumentoIdentidad parametrosDocumentoIdentidad)
        {
            short DocumentoIdentidadId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daDocumentoIdentidad odaDocumentoIdentidad = new daDocumentoIdentidad();
                    DocumentoIdentidadId = odaDocumentoIdentidad.validarDocumentoIdentidad(con, trx, parametrosDocumentoIdentidad);
                    if (DocumentoIdentidadId == 0) { exito = true; } else { exito = false; DocumentoIdentidadId = -2; }
                    if (exito)
                    {
                        DocumentoIdentidadId = odaDocumentoIdentidad.adicionar(con, trx, parametrosDocumentoIdentidad);
                        if (DocumentoIdentidadId != -1) { exito = true; } else { exito = false; DocumentoIdentidadId = -1; }
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
            return (DocumentoIdentidadId);
        }

        public short actualizar(beDocumentoIdentidad parametrosDocumentoIdentidad)
        {
            short DocumentoIdentidadId = -1;
            bool exito = false;
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    daDocumentoIdentidad odaDocumentoIdentidad = new daDocumentoIdentidad();
                    DocumentoIdentidadId = odaDocumentoIdentidad.validarDocumentoIdentidad(con, trx, parametrosDocumentoIdentidad);
                    if (DocumentoIdentidadId == 0) { exito = true; } 
                    else
                    {
                        if (DocumentoIdentidadId != parametrosDocumentoIdentidad.Tipodocumentoidentidadid)
                        {
                            exito = false;
                            DocumentoIdentidadId = -2;
                        }
                        else
                        {
                            exito = true;
                        }
                    }
                    if (exito)
                    {
                        exito = odaDocumentoIdentidad.actualizar(con, trx, parametrosDocumentoIdentidad);
                        if (exito) { DocumentoIdentidadId = 1; } else { exito = false; DocumentoIdentidadId = -1; }
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
            return (DocumentoIdentidadId);
        }
    }
}
