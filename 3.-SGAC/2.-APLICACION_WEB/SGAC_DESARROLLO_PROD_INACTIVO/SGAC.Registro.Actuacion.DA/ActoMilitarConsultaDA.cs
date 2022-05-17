using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoMilitarConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoMilitarConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActoMilitarConsultaDA()
        {
            GC.Collect();
        }

        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        //public DataTable Consultar(long LonActuacionDetalleId,
        //                           string StrCurrentPage,
        //                           int IntPageSize,
        //                           ref int IntTotalCount,
        //                           ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@remi_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionDetalleId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = StrCurrentPage;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IntPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_REGISTROMILITAR_CONSULTAR",
        //                                             prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value);
        //        IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DtResult;
        //}

        public DataTable Consultar(long LonActuacionDetalleId,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@remi_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = StrCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;
                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);

                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        /// <summary>
        /// Consulta la Hoja de Registro de Inscripción Militar
        /// </summary>
        /// <param name="LonActuacionDetalleId"></param>
        /// <returns></returns>
        //public DataTable Consultar_Inscripcion(long LonActuacionDetalleId)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[1];

        //        prmParameter[0] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionDetalleId;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_REGISTROMILITAR_FORMATO_HOJA",
        //                                             prmParameter);

        //        DtResult = DsResult.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DtResult;
        //}

        public DataTable Consultar_Inscripcion(long LonActuacionDetalleId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_FORMATO_HOJA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                    }
                }
                                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }

        /// <summary>
        /// Consulta la Constancia de Inscripción Militar
        /// </summary>
        /// <param name="LonActuacionDetalleId"></param>
        /// <returns></returns>
        //public DataTable Consultar_Constancia(long LonActuacionDetalleId, long LonOficinaConsular)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[2];

        //        prmParameter[0] = new SqlParameter("@acpa_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionDetalleId;

        //        prmParameter[1] = new SqlParameter("@ofco_sOficinaConsularId", SqlDbType.BigInt);
        //        prmParameter[1].Value = LonOficinaConsular;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_REGISTROMILITAR_FORMATO_CONSTANCIA",
        //                                             prmParameter);

        //        DtResult = DsResult.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DtResult;
        //}
        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable Consultar_Constancia(long LonActuacionDetalleId, long LonOficinaConsular)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_FORMATO_CONSTANCIA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acpa_iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionDetalleId;
                        cmd.Parameters.Add("@ofco_sOficinaConsularId", SqlDbType.BigInt).Value = LonOficinaConsular;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                    }
                }
                                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DtResult;
        }


        //public DataSet Obtener(long longActuacionDetalleId)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[1];

        //        prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = longActuacionDetalleId;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                             CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_REGISTROMILITAR_OBTENER_POR_ID",
        //                                             prmParameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DsResult;
        //}
        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataSet Obtener(long longActuacionDetalleId)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_REGISTROMILITAR_OBTENER_POR_ID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = longActuacionDetalleId;
                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DsResult;
        }

    }
}