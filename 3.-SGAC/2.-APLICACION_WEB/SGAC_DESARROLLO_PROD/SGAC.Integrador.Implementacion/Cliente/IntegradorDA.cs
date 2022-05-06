using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.BE;

namespace SGAC.Cliente.Colas.DA
{
    public class IntegradorDA
    {
        private readonly string strConnectionName = string.Empty;

        public IntegradorDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataSet DescargaConfiguracion(int iOficinaConsularId)
        {
            DataSet dsResult;

            var prmParameter = new SqlParameter[1];
            prmParameter[0] = new SqlParameter("@tick_iOficinaConsularId", SqlDbType.Int);
            prmParameter[0].Value = iOficinaConsularId;

            dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure,
                "[PN_CLIENTE].[USP_CL_TICKET_CARGARBD]", prmParameter);
            return dsResult;
        }

        private bool InsertarTicket(List<CL_TICKET> LisTicket, int intFila)
        {
            var booOk = false;
            int intResultQuery;

            try
            {
                var prnParameter = new SqlParameter[21];

                prnParameter[0] = new SqlParameter("@tick_sTipoServicioId", SqlDbType.Int);
                prnParameter[0].Value = LisTicket[intFila].tick_sTipoServicioId;

                prnParameter[1] = new SqlParameter("@tick_iPersonalId", SqlDbType.Int);
                prnParameter[1].Value = LisTicket[intFila].tick_iPersonalId;

                prnParameter[2] = new SqlParameter("@tick_iNumero", SqlDbType.Int);
                prnParameter[2].Value = LisTicket[intFila].tick_iNumero;

                prnParameter[3] = new SqlParameter("@tick_dFechaHoraGeneracion", SqlDbType.DateTime);
                if (LisTicket[intFila].tick_dFechaHoraGeneracion != null)
                {
                    prnParameter[3].Value = LisTicket[intFila].tick_dFechaHoraGeneracion;
                }
                else
                {
                    prnParameter[3].Value = System.DBNull.Value;
                }
                
                prnParameter[4] = new SqlParameter("@tick_dAtencionInicio", SqlDbType.DateTime);
                if (LisTicket[intFila].tick_dAtencionInicio != null)
                {
                    prnParameter[4].Value = LisTicket[intFila].tick_dAtencionInicio;
                }
                else
                {
                    prnParameter[4].Value = System.DBNull.Value;
                }

                prnParameter[5] = new SqlParameter("@tick_dAtencionFinal", SqlDbType.DateTime);
                if (LisTicket[intFila].tick_dAtencionFinal != null)
                {
                    prnParameter[5].Value = LisTicket[intFila].tick_dAtencionFinal;
                }
                else
                {
                    prnParameter[5].Value = System.DBNull.Value;
                }

                prnParameter[6] = new SqlParameter("@tick_sPrioridadId", SqlDbType.Int);
                prnParameter[6].Value = LisTicket[intFila].tick_sPrioridadId;

                prnParameter[7] = new SqlParameter("@tick_sTipoCliente", SqlDbType.Int);
                prnParameter[7].Value = LisTicket[intFila].tick_sTipoCliente;

                prnParameter[8] = new SqlParameter("@tick_sTamanoTicket", SqlDbType.Int);
                prnParameter[8].Value = LisTicket[intFila].tick_sTamanoTicket;

                prnParameter[9] = new SqlParameter("@tick_sTipoEstado", SqlDbType.Int);
                prnParameter[9].Value = LisTicket[intFila].tick_sTipoEstado;

                prnParameter[10] = new SqlParameter("@tick_sTicketeraId", SqlDbType.Int);
                prnParameter[10].Value = LisTicket[intFila].tick_sTicketeraId;

                prnParameter[11] = new SqlParameter("@tick_vLLamada", SqlDbType.VarChar);
                prnParameter[11].Value = LisTicket[intFila].tick_vLLamada;

                prnParameter[12] = new SqlParameter("@tick_sUsuarioAtendio", SqlDbType.Int);
                if (LisTicket[intFila].tick_sUsuarioAtendio != 0)
                {
                    prnParameter[12].Value = LisTicket[intFila].tick_sUsuarioAtendio;
                }
                else
                {
                    prnParameter[12].Value = System.DBNull.Value;
                }

                prnParameter[13] = new SqlParameter("@tick_cEstado", SqlDbType.VarChar);
                prnParameter[13].Value = LisTicket[intFila].tick_cEstado;

                prnParameter[14] = new SqlParameter("@tick_sUsuarioCreacion", SqlDbType.Int);
                prnParameter[14].Value = LisTicket[intFila].tick_sUsuarioCreacion;

                prnParameter[15] = new SqlParameter("@tick_vIPCreacion", SqlDbType.VarChar);
                prnParameter[15].Value = LisTicket[intFila].tick_vIPCreacion;

                prnParameter[16] = new SqlParameter("@tick_dFechaCreacion", SqlDbType.DateTime);
                if (LisTicket[intFila].tick_dFechaCreacion != null)
                {
                    prnParameter[16].Value = LisTicket[intFila].tick_dFechaCreacion;
                }
                else
                {
                    prnParameter[16].Value = System.DBNull.Value;
                }

                prnParameter[17] = new SqlParameter("@tick_sUsuarioModificacion", SqlDbType.Int);
                if (LisTicket[intFila].tick_sUsuarioModificacion != 0)
                {
                    prnParameter[17].Value = LisTicket[intFila].tick_sUsuarioModificacion;
                }
                else
                {
                    prnParameter[17].Value = System.DBNull.Value;
                }

                prnParameter[18] = new SqlParameter("@tick_vIPModificacion", SqlDbType.VarChar);
                if (LisTicket[intFila].tick_vIPModificacion != "")
                {
                    prnParameter[18].Value = LisTicket[intFila].tick_vIPModificacion;
                }
                else
                {
                    prnParameter[18].Value = System.DBNull.Value;
                }

                prnParameter[19] = new SqlParameter("@tick_dFechaModificacion", SqlDbType.DateTime);
                if (LisTicket[intFila].tick_dFechaModificacion != null)                
                {
                    prnParameter[19].Value = LisTicket[intFila].tick_dFechaModificacion;
                }
                else
                {
                    prnParameter[19].Value = System.DBNull.Value;
                }

                prnParameter[20] = new SqlParameter("@tick_sVentanillaId", SqlDbType.Int);
                if (LisTicket[intFila].tick_sVentanillaId != 0)
                {
                    prnParameter[20].Value = LisTicket[intFila].tick_sVentanillaId;
                }
                else
                {
                    prnParameter[20].Value = System.DBNull.Value;
                }

                intResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName, CommandType.StoredProcedure,
                    "PN_CLIENTE.USP_CL_TICKET_IMPORTAR", prnParameter);

                if (intResultQuery != 1)
                {
                    // SI NO SE PUEDE AGREGAR UN REGISTRO CANCELAMOS EL PROCESO DE INSERCION
                    booOk = false;
                }
            }
            catch
            {
                booOk = false;
            }   

            return booOk;
        }

        public bool CargaInformacion(List<CL_TICKET> LisTicket, int iOficinaConsularId)
        {
            var booOk = false;
            //int intResultQuery;
            int A;

            var dt = DateTime.UtcNow;
            try
            {
                // TRAEMOS LOS TICKETS PREVIAMENTE SUBIDOS CORRESPONDIENTES A LA OFICINA CONSULAR Y A LA FECHA DE PROCESO
                var dsResult = new DataSet();
                var dtResul = new DataTable();
                var prnParameter2 = new SqlParameter[2];
                var intFila = 0;
                var bolAgregar = false;

                prnParameter2[0] = new SqlParameter("@tick_sOficinaId", SqlDbType.Int);
                prnParameter2[0].Value = iOficinaConsularId;

                prnParameter2[1] = new SqlParameter("tick_dFechaHoraGeneracion", SqlDbType.DateTime);
                prnParameter2[1].Value = LisTicket[0].tick_dFechaHoraGeneracion;

                dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure,
                    "PN_CLIENTE.USP_CL_TICKET_LISTAR_DIA", prnParameter2);
                dtResul = dsResult.Tables["Table"];

                for (A = 0; A <= LisTicket.Count - 1; A++)
                {
                    if (dtResul.Rows.Count != 0)
                    {
                        bolAgregar = true;
                        for (intFila = 0; intFila <= dtResul.Rows.Count - 1; intFila++)
                        {
                            if ((dtResul.Rows[intFila]["tick_iNumero"].ToString() ==
                                 LisTicket[A].tick_iNumero.ToString()) &&
                                (dtResul.Rows[intFila]["tick_sTipoServicioId"].ToString() ==
                                 LisTicket[A].tick_sTipoServicioId.ToString()))
                            {
                                bolAgregar = false;
                                break;
                            }
                            //booOk = InsertarTicket(LisTicket, A);
                        }
                        if (bolAgregar)
                        {
                            booOk = InsertarTicket(LisTicket, A);
                        }
                    }
                    else
                    {
                        booOk = InsertarTicket(LisTicket, A);
                    }
                }
                booOk = true;
            }
            catch
            {
                booOk = false;
            }

            return booOk;
        }

        public string BuscarPersonaRune(int intDocumentoTipo, string strDocumentoNumero)
        {
            var strcadena = "";
            var dsResult = new DataSet();
            var DtResult = new DataTable();

            try
            {
                var prnParameter = new SqlParameter[2];

                prnParameter[0] = new SqlParameter("@sDocumentoTipoId", SqlDbType.SmallInt);
                prnParameter[0].Value = intDocumentoTipo;

                prnParameter[1] = new SqlParameter("@vDocumentoNumero", SqlDbType.VarChar);
                prnParameter[1].Value = strDocumentoNumero;

                dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure,
                    "PN_CLIENTE.USP_CL_TICKET_BUSCARPERSONA", prnParameter);
                DtResult = dsResult.Tables[0];

                if (DtResult.Rows.Count != 0)
                {
                    strcadena = DtResult.Rows[0]["peid_vDocumentoNumero"] + "|" +
                                DtResult.Rows[0]["pers_vNombre"] + "|" +
                                DtResult.Rows[0]["doid_vDescripcionCorta"] + "|" +
                                DtResult.Rows[0]["pers_iPersonaId"];
                }
                else
                {
                    strcadena = "";
                }

                return strcadena;
            }
            catch
            {
                return strcadena;
            }
        }
    }
}