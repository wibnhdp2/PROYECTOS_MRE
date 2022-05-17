using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Almacen.DA
{
    public class MovimientoDetalleConsultaDA
    {
        private string strConnectionName = string.Empty;

        public MovimientoDetalleConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~MovimientoDetalleConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------

        public DataTable MovimientoDetalleConsultar(int mode_iMovimientoId)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();


            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@mode_iMovimientoId", SqlDbType.BigInt).Value = mode_iMovimientoId;

                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();

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
            //prmParameter[0] = new SqlParameter("@mode_iMovimientoId", SqlDbType.BigInt);
            //prmParameter[0].Value = mode_iMovimientoId;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_CONSULTAR", prmParameter);
            //dtResult = dsResult.Tables[0];

            //return dtResult;
            #endregion
        }

        //public DataTable MovimientoDetalleLeerRegistro(int mode_iMovimientoDetalleId, int mode_iMovimientoId)
        //{
        //    DataSet dsResult;
        //    DataTable dtResult;

        //    SqlParameter[] prmParameter = new SqlParameter[2];
        //    prmParameter[0] = new SqlParameter("@@mode_iMovimientoDetalleId", SqlDbType.BigInt);
        //    prmParameter[0].Value = mode_iMovimientoDetalleId;
        //    prmParameter[1] = new SqlParameter("@mode_iMovimientoId", SqlDbType.BigInt);
        //    prmParameter[1].Value = mode_iMovimientoId;

        //    dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_MOVIMIENTODETALLE_LEERREGISTRO", prmParameter);
        //    dtResult = dsResult.Tables[0];

        //    return dtResult;
        //}

        #endregion Método CONSULTAR
    }
}