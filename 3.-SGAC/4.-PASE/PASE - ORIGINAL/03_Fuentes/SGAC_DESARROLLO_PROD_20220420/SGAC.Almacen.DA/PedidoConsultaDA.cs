using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Almacen.DA
{
    public class PedidoConsultaDA
    {
        private string strConnectionName = string.Empty;

        public PedidoConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PedidoConsultaDA()
        {
            GC.Collect();
        }

        #region Método CONSULTAR

        //public DataTable LeerRegistro(int pedi_iPedidoId)
        //{
        //    DataSet dsResult;
        //    DataTable dtResult;

        //    SqlParameter[] prmParameter = new SqlParameter[1];
        //    prmParameter[0] = new SqlParameter("@pedi_iPedidoId", SqlDbType.BigInt);
        //    prmParameter[0].Value = pedi_iPedidoId;

        //    dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_PEDIDO_LEER", prmParameter);
        //    dtResult = dsResult.Tables[0];

        //    return dtResult;
        //}
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------

        public DataTable Consultar(int pedi_IOficinaConsularIdOrigen, DateTime datFechaInicio, DateTime datFechaFin, string strActaRemision,
                                    int intEstado, int intInsumo, string strCodPedidoC,
                                    int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();
            intTotalRegistros = 0;
            intTotalPaginas = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@pedi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = pedi_IOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@pedi_dFechaInicio", SqlDbType.DateTime).Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
                        cmd.Parameters.Add("@pedi_dFechaFin", SqlDbType.DateTime).Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");
                        cmd.Parameters.Add("@pedi_vActaRemision", SqlDbType.VarChar, 12).Value = strActaRemision.ToString();
                        cmd.Parameters.Add("@pedi_sEstadoId", SqlDbType.SmallInt).Value = intEstado.ToString();
                        cmd.Parameters.Add("@pedi_sInsumoTipoId", SqlDbType.SmallInt).Value = intInsumo.ToString();
                        cmd.Parameters.Add("@pedi_cPedidoCodigo", SqlDbType.VarChar, 12).Value = strCodPedidoC.ToString();
                        cmd.Parameters.Add("@IPaginaActual", SqlDbType.Int).Value = intPaginaActual;
                        cmd.Parameters.Add("@IPaginaCantidad", SqlDbType.Int).Value = intPaginaCantidad;


                        SqlParameter lMovimientoIdReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.Int);
                        lMovimientoIdReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lMovimientoIdReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.Int);
                        lMovimientoIdReturn2.Direction = ParameterDirection.Output;

                        
                        cmd.Connection.Open();

                        SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);

                        sqlDA.Fill(dsResult);

                        if (dsResult.Tables.Count > 0)
                            dtResult = dsResult.Tables[0];
                        else
                            throw new DataException();


                        if (lMovimientoIdReturn1.Value != null)
                        {
                            if (lMovimientoIdReturn1.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalRegistros = Convert.ToInt32(lMovimientoIdReturn1.Value);
                            }
                        }
                        if (lMovimientoIdReturn2.Value != null)
                        {
                            if (lMovimientoIdReturn2.Value.ToString().Trim() != string.Empty)
                            {
                                intTotalPaginas = Convert.ToInt32(lMovimientoIdReturn2.Value);
                            }
                        }

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

            //SqlParameter[] prmParameter = new SqlParameter[11];
            //prmParameter[0] = new SqlParameter("@pedi_sOficinaConsularIdOrigen", SqlDbType.SmallInt);
            //prmParameter[0].Value = pedi_IOficinaConsularIdOrigen;
            //prmParameter[1] = new SqlParameter("@pedi_dFechaInicio", SqlDbType.DateTime);
            //prmParameter[1].Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            //prmParameter[2] = new SqlParameter("@pedi_dFechaFin", SqlDbType.DateTime);
            //prmParameter[2].Value = datFechaFin.ToString("yyyy-MM-dd 23:59:59");

            //prmParameter[3] = new SqlParameter("@pedi_vActaRemision", SqlDbType.VarChar);
            //prmParameter[3].Value = strActaRemision.ToString();
            //prmParameter[4] = new SqlParameter("@pedi_sEstadoId", SqlDbType.SmallInt);
            //prmParameter[4].Value = intEstado.ToString();


            //prmParameter[5] = new SqlParameter("@pedi_sInsumoTipoId", SqlDbType.SmallInt);
            //prmParameter[5].Value = intInsumo.ToString();
            //prmParameter[6] = new SqlParameter("@pedi_cPedidoCodigo", SqlDbType.VarChar);
            //prmParameter[6].Value = strCodPedidoC.ToString();

            //prmParameter[7] = new SqlParameter("@IPaginaActual", SqlDbType.Int);
            //prmParameter[7].Value = intPaginaActual;
            //prmParameter[8] = new SqlParameter("@IPaginaCantidad", SqlDbType.Int);
            //prmParameter[8].Value = intPaginaCantidad;
            //prmParameter[9] = new SqlParameter("@ITotalRegistros", SqlDbType.Int);
            //prmParameter[9].Direction = ParameterDirection.Output;
            //prmParameter[10] = new SqlParameter("@ITotalPaginas", SqlDbType.Int);
            //prmParameter[10].Direction = ParameterDirection.Output;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_PEDIDO_CONSULTAR", prmParameter);
            //dtResult = dsResult.Tables[0];

            //intTotalRegistros = Convert.ToInt32(((SqlParameter)prmParameter[9]).Value);
            //intTotalPaginas = Convert.ToInt32(((SqlParameter)prmParameter[10]).Value);

            //return dtResult;
            #endregion
        }
        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 08/11/2019
        // Motivo: Reemplazar SqlHelper por: 
        //          SqlConnection, SqlCommand y SqlDataReader
        //----------------------------------------------------------

        public DataTable ExistePedidoAtendido(string pedi_vPedidoCodigo)
        {
            DataSet dsResult = new DataSet();
            DataTable dtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDO_EXISTEMOVIMIENTO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add("@pedi_cPedidoCodigo", SqlDbType.Char, 12).Value = pedi_vPedidoCodigo;

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
            //prmParameter[0] = new SqlParameter("@pedi_cPedidoCodigo", SqlDbType.Char);
            //prmParameter[0].Value = pedi_vPedidoCodigo;

            //dsResult = SqlHelper.ExecuteDataset(strConnectionName, CommandType.StoredProcedure, "PN_ALMACEN.USP_AL_PEDIDO_EXISTEMOVIMIENTO", prmParameter);
            //dtResult = dsResult.Tables[0];

            //return dtResult;
            #endregion
        }

        #endregion Método CONSULTAR
    }
}