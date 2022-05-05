using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActaJudicialConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActaJudicialConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActaJudicialConsultaDA()
        {
            GC.Collect();
        }

        // *************************************
        // SE PASO A SGAC.RE.MRE.RE_ACTAJUDICIAL
        // *************************************
        //public DataTable Obtener(Int64 iActoJudicialNotificacionId)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(StrConnectionName))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTAJUDICIAL_OBTENER", cn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@iActoJudicialNotificacionId", iActoJudicialNotificacionId));
        //                cmd.Connection.Open();
                        
        //                DataSet ds_Objeto = new DataSet();
        //                using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
        //                {
        //                    adap.Fill(ds_Objeto);
        //                }

        //                DtResult = ds_Objeto.Tables[0];  
        //            }
                    
        //        }
        //        //SqlParameter[] prmParameter = new SqlParameter[1];

        //        //prmParameter[0] = new SqlParameter("@iActoJudicialNotificacionId", SqlDbType.BigInt);
        //        //prmParameter[0].Value = iActoJudicialNotificacionId;

        //        //DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //        //                                     CommandType.StoredProcedure,
        //        //                                    "PN_REGISTRO.USP_RE_ACTAJUDICIAL_OBTENER",
        //        //                                     prmParameter);
        //        //DtResult = DsResult.Tables[0];

        //        //if (DtResult.Rows.Count == 0)
        //        //{
        //        //    //DtResult = null;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return DtResult;
        //}
    }
}