using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Reportes.DA
{
    public class ReportesNotarialesConsultasDA
    {
        private string strConnectionName = string.Empty;

        public ReportesNotarialesConsultasDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public String USP_RP_NOTARIAL_PROTOCOLAR_TESTIMONIO(Int64 acno_iActoNotarialId, Int16 sOficinaConsularID, int intCorrelativo, Int64 intiActoNotarialDetalleId)
        {
            String str_Testimino = String.Empty;
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_NOTARIAL_PROTOCOLAR_TESTIMONIO", cnn)) {
                        cnn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acno_iActoNotarialId", SqlDbType.BigInt).Value = acno_iActoNotarialId;
                        cmd.Parameters.Add("@acno_sOficinaConsularId", SqlDbType.SmallInt).Value = sOficinaConsularID;
                        cmd.Parameters.Add("@acno_sNumeroTestimonio", SqlDbType.SmallInt).Value = intCorrelativo;
                        cmd.Parameters.Add("@ande_iActoNotarialDetalleId", SqlDbType.BigInt).Value = intiActoNotarialDetalleId;

                        using (SqlDataReader dr = cmd.ExecuteReader()) {
                            if (dr.HasRows)
                            {
                                while (dr.Read()) {

                                    if (!dr.IsDBNull(0))
                                        str_Testimino = dr.GetString(0);
                                }

                                return str_Testimino;
                            }
                            else {
                                return str_Testimino; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }        
        }

        public DataTable USP_RP_NOTARIAL_PROTOCOLAR_PARTE(Int64 acno_iActoNotarialId, Int16 sOficinaConsularID, Int16 sNumeroParte, string strNumeroOficio)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_NOTARIAL_PROTOCOLAR_PARTE", cnn))
                    {
                        cnn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acno_iActoNotarialId", SqlDbType.BigInt).Value = acno_iActoNotarialId;
                        cmd.Parameters.Add("@acno_sOficinaConsularId", SqlDbType.SmallInt).Value = sOficinaConsularID;
                        cmd.Parameters.Add("@acno_sNumeroParte", SqlDbType.SmallInt).Value = sNumeroParte;
                        cmd.Parameters.Add("@acno_vNumeroOficio", SqlDbType.VarChar, 20).Value = strNumeroOficio;

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return dt;
        }
    }
}
