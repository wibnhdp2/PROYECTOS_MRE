using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Cliente.Colas.DA
{
    public class TicketConsultaDA
    {
        ~TicketConsultaDA()
        {
            GC.Collect();
        }

        private string strConnectionName = string.Empty;

        public TicketConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable TicketConsultaVentanilla(int iOficinaConsularId, int iVentanillaNumero, string cTicketFechaEmision)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_TICKET_CONSULTAR_LISTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tick_iOficinaConsularId", iOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tick_iVentanillaNumero", iVentanillaNumero));
                        cmd.Parameters.Add(new SqlParameter("@tick_cFechaEmision", cTicketFechaEmision));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable TicketConsultaPromedio(int iOficinaConsularId, int iVentanillaNumero, string cTicketFechaEmision)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_TICKET_CONSULTAR_PROMEDIOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tick_iOficinaConsularId", iOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tick_cFechaEmision", iVentanillaNumero));
                        cmd.Parameters.Add(new SqlParameter("@tick_iVentanillaNumero", cTicketFechaEmision));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataSet TraerDatos(int iOficinaConsularId, String sFechaEmision)
        {
            DataSet dsObjeto = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_TICKET_CARGARBD", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tick_iOficinaConsularId", iOficinaConsularId));

                        cmd.Connection.Open();
                        
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                dsObjeto = null;
                throw exec;
            }

            return dsObjeto;
        }

        public bool Adicionar(ref List<BE.CL_TICKET> lisTicket)
        {
            bool booOk = false;
            int intResultQuery;
            int A;

            try
            {
                for (A = 0; A <= lisTicket.Count - 1; A++)
                {
                    SqlParameter[] prnParameter = new SqlParameter[21];

                    prnParameter[0] = new SqlParameter("@tick_sTipoServicioId", SqlDbType.SmallInt);
                    prnParameter[0].Value = lisTicket[A].tick_sTipoServicioId;

                    prnParameter[1] = new SqlParameter("@tick_iPersonalId", SqlDbType.Int);
                    prnParameter[1].Value = lisTicket[A].tick_iPersonalId;

                    prnParameter[2] = new SqlParameter("@tick_iNumero", SqlDbType.Int);
                    prnParameter[2].Value = lisTicket[A].tick_iNumero;

                    prnParameter[3] = new SqlParameter("@tick_dFechaHoraGeneracion", SqlDbType.DateTime);
                    prnParameter[3].Value = lisTicket[A].tick_dFechaHoraGeneracion;

                    prnParameter[4] = new SqlParameter("@tick_dAtencionInicio", SqlDbType.DateTime);
                    if (lisTicket[A].tick_dAtencionInicio != null)
                    {
                        prnParameter[4].Value = lisTicket[A].tick_dAtencionInicio;
                    }
                    else
                    {
                        prnParameter[4].Value = null;
                    }

                    prnParameter[5] = new SqlParameter("@tick_dAtencionFinal", SqlDbType.DateTime);
                    if (lisTicket[A].tick_dAtencionFinal != null)
                    {
                        prnParameter[5].Value = lisTicket[A].tick_dAtencionFinal;
                    }
                    {
                        prnParameter[5].Value = null;
                    }

                    prnParameter[6] = new SqlParameter("@tick_sPrioridadId", SqlDbType.SmallInt);
                    prnParameter[6].Value = lisTicket[A].tick_sPrioridadId;


                    prnParameter[7] = new SqlParameter("@tick_sTipoCliente", SqlDbType.SmallInt);
                    prnParameter[7].Value = lisTicket[A].tick_sTipoCliente;

                    prnParameter[8] = new SqlParameter("@tick_sTamanoTicket", SqlDbType.SmallInt);
                    prnParameter[8].Value = lisTicket[A].tick_sTamanoTicket;

                    prnParameter[9] = new SqlParameter("@tick_sTipoEstado", SqlDbType.SmallInt);
                    prnParameter[9].Value = lisTicket[A].tick_sTipoEstado;

                    prnParameter[10] = new SqlParameter("@tick_sTicketeraId", SqlDbType.SmallInt);
                    prnParameter[10].Value = lisTicket[A].tick_sTicketeraId;

                    prnParameter[11] = new SqlParameter("@tick_vLLamada", SqlDbType.VarChar);
                    prnParameter[11].Value = lisTicket[A].tick_vLLamada;

                    prnParameter[12] = new SqlParameter("@tick_sUsuarioAtendio", SqlDbType.SmallInt);
                    prnParameter[12].Value = lisTicket[A].tick_sUsuarioAtendio;

                    prnParameter[13] = new SqlParameter("@tick_cEstado", SqlDbType.VarChar);
                    prnParameter[13].Value = lisTicket[A].tick_cEstado;

                    prnParameter[14] = new SqlParameter("@tick_sUsuarioCreacion", SqlDbType.SmallInt);
                    prnParameter[14].Value = lisTicket[A].tick_sUsuarioCreacion;


                    ////--       @tick_vIPCreacion			varchar(20) ,	DIRECCION IP DE LA PC DONDE SE CREO EL TICKET
                    ////--       @tick_dFechaCreacion		datetime,       FECHA DE CREACION DEL TICKET
                    ////--       @tick_sUsuarioModificacion int,			ID DEL USUARIO QUE MODIFICA DEL TICKET
                    ////--       @tick_vIPModificacion		varchar(20),	DIRECCION IP DEL LA PC DONDE SE MODIFICA EL TICKET
                    ////--       @tick_dFechaModificacion	datetime		FECHA Y HORA DE MODIFICACION DEL TICKET

                    prnParameter[15] = new SqlParameter("@tick_vIPCreacion", SqlDbType.VarChar);
                    prnParameter[15].Value = lisTicket[A].tick_vIPCreacion;

                    prnParameter[16] = new SqlParameter("@tick_dFechaCreacion", SqlDbType.DateTime);
                    if (lisTicket[A].tick_dFechaCreacion != null)
                    {
                        prnParameter[16].Value = lisTicket[A].tick_dFechaCreacion;
                    }
                    else
                    {
                        prnParameter[16].Value = null;
                    }

                    prnParameter[17] = new SqlParameter("@tick_sUsuarioModificacion", SqlDbType.SmallInt);
                    if (lisTicket[A].tick_sUsuarioModificacion != null)
                    {
                        prnParameter[17].Value = lisTicket[A].tick_sUsuarioModificacion;
                    }
                    else
                    {
                        prnParameter[17].Value = null;
                    }


                    prnParameter[18] = new SqlParameter("@tick_vIPModificacion", SqlDbType.VarChar);
                    if (lisTicket[A].tick_vIPModificacion != null)
                    {
                        prnParameter[18].Value = lisTicket[A].tick_vIPModificacion;
                    }
                    else
                    {
                        prnParameter[18].Value = null;
                    }

                    prnParameter[19] = new SqlParameter("@tick_dFechaModificacion", SqlDbType.DateTime);
                    if (lisTicket[A].tick_dFechaModificacion != null)
                    {
                        prnParameter[19].Value = lisTicket[A].tick_dFechaModificacion;
                    }

                    prnParameter[20] = new SqlParameter("@tick_sVentanillaId", SqlDbType.SmallInt);
                    prnParameter[20].Value = lisTicket[A].tick_sVentanillaId;


                    intResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName, CommandType.StoredProcedure, "PN_CLIENTE.USP_CL_TICKET_IMPORTAR", prnParameter);

                    if (intResultQuery != 0)
                    {
                        lisTicket[A].tick_iTicketId = 0; // (int)prnParameter[14].Value;
                    }
                }

                booOk = true;

                return booOk;
            }
            catch
            {
                return booOk;
            }
        }
    }
}
