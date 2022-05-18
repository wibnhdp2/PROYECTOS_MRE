using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SGAC.Configuracion.Sistema.DA
{
    public class StockConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public StockConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable obtener(int intOficinaConsularId, int intTipoInsumo,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas,
            ref string strError, ref string strErrorCompleto)
        {
            DataTable dtStockAlmacen = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_ALMACEN_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                            cmd.Parameters.Add(new SqlParameter("@stck_sOficinaConsularId", intOficinaConsularId));
                        if (intTipoInsumo != 0)
                            cmd.Parameters.Add(new SqlParameter("@stck_sInsumoId", intTipoInsumo));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtStockAlmacen = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(parTotalRegistros.Value);
                        intTotalPaginas = Convert.ToInt32(parTotalPagina.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                strError = ex.Message;
                strErrorCompleto = ex.StackTrace;
            }
            return dtStockAlmacen;
        }

        public BE.MRE.SI_STOCK_ALMACEN consultar(BE.MRE.SI_STOCK_ALMACEN Stock)
        {
            BE.MRE.SI_STOCK_ALMACEN objStock = new BE.MRE.SI_STOCK_ALMACEN();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_STOCK_ALMACEN_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@stck_sStockId", Stock.stck_sStockId));
                        cmd.Parameters.Add(new SqlParameter("@stck_sInsumoId", Stock.stck_sInsumoId));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", 1));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", 10));
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)objStock.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(objStock, Convert.ToInt16(loReader[col]), null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objStock.Error = true;
                objStock.Message = exec.Message.ToString();
            }
            return objStock;
        }

    }
}
