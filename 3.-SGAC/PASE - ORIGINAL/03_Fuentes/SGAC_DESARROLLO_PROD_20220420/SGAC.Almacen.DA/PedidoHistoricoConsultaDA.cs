using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Almacen.DA
{
    public class PedidoHistoricoConsultaDA
    {
        private string strConnectionName = string.Empty;

        public PedidoHistoricoConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PedidoHistoricoConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR

        //-----------------------------------------------------------
        //Fecha: 11/11/2019
        //Modificado por: Miguel Márquez Beltrán
        //Motivo: Reemplazar SqlHelper por:
        //          SqlConnection, SqlCommand y SqlDataAdapter.
        //-----------------------------------------------------------

        public DataTable PedidoHistoricoConsultar(int pehi_iPedidoId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDOHISTORICO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@pehi_iPedidoId", SqlDbType.BigInt).Value = pehi_iPedidoId;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        

                    }
                }

                return dtResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            #region Comentada
            //DataSet dsResult;
            //DataTable dtResult;

            //SqlParameter[] prmParameter = new SqlParameter[1];
            //prmParameter[0] = new SqlParameter("@pehi_iPedidoId", SqlDbType.BigInt);
            //prmParameter[0].Value = pehi_iPedidoId;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_PEDIDOHISTORICO_CONSULTAR", prmParameter);
            //dtResult = dsResult.Tables[0];

            //return dtResult;
            #endregion
        }

        #endregion Método CONSULTAR
    }
}