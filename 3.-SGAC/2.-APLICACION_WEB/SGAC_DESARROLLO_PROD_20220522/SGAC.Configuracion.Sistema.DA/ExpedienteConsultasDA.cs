using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SGAC.Configuracion.Sistema.DA
{
    public class ExpedienteConsultasDA
    {
        private string StrConnectionName = string.Empty;

        public ExpedienteConsultasDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable obtener(int intOficinaConsularId, int intPeriodo,
            int intPaginaActual, int intPaginaCantidad, int intTipoDocMigratorio, ref int intTotalRegistros, ref int intTotalPaginas,
            ref string strError, ref string strErrorCompleto) 
        {
            DataTable dtExpedientes = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                            cmd.Parameters.Add(new SqlParameter("@exp_sOficinaConsularId", intOficinaConsularId));
                        if (intPeriodo != 0)
                            cmd.Parameters.Add(new SqlParameter("@exp_sPeriodo", intPeriodo));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));
                        cmd.Parameters.Add(new SqlParameter("@exp_sTipoDocMigId", intTipoDocMigratorio)); 
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtExpedientes = dsObjeto.Tables[0];
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
            return dtExpedientes;
        }


        public BE.MRE.SI_EXPEDIENTE consultar(BE.MRE.SI_EXPEDIENTE Expediente)
        {
            BE.MRE.SI_EXPEDIENTE objExpediente = new BE.MRE.SI_EXPEDIENTE();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_EXPEDIENTE_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@exp_sExpedienteId", Expediente.exp_sExpedienteId));
                        cmd.Parameters.Add(new SqlParameter("@exp_sTipoDocMigId", Expediente.exp_sTipoDocMigId));
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
                                        PropertyInfo pInfo = (PropertyInfo)objExpediente.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(objExpediente, loReader[col], null);
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
                objExpediente.Error = true;
                objExpediente.Message = exec.Message.ToString();
            }
            return objExpediente;
        }
    }
}
