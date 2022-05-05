using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionAnotacionConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionAnotacionConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionAnotacionConsultaDA()
        {
            GC.Collect();
        }

        //public DataTable Obtener(long LonPersonaId, int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@acde_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonPersonaId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = intCurrentPage;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IntPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ANOTACION_OBTENER",
        //                                            prmParameter);

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

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable Obtener(long LonPersonaId, int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANOTACION_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acde_iActuacionDetalleId", SqlDbType.BigInt).Value = LonPersonaId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = intCurrentPage;
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

//------------------------
    }
}