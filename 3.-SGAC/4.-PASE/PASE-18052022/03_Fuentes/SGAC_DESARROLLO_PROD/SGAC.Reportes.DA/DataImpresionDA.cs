using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace SGAC.Reportes.DA
{
    public class DataImpresionDA
    {
        private string strConnectionName = string.Empty;

        public DataImpresionDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        //string conexion()
        //{
        //    return ConfigurationManager.AppSettings["ConexionSGAC"];
        //}   
        public DataTable ObtenerDataDeImpresion(Int64 iActuacionID, int sTipoDocImpresion)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_XMLIMPRESION_OBTENERDATAIMPRESION]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ximp_iActuacionID", SqlDbType.BigInt).Value = iActuacionID;
                        cmd.Parameters.Add("@ximp_sTipoDocImpresion", SqlDbType.SmallInt).Value = sTipoDocImpresion;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void RegistrarDataImpresion(Int64 ximp_iActuacionID, Int16 ximp_vTipoDocImpresion, Int16 ximp_sUsuarioCreacion, string ximp_vIPCreacion)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_XMLIMPRESION_GENERARDOCUMENTO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ximp_iActuacionID", ximp_iActuacionID));
                        cmd.Parameters.Add(new SqlParameter("@ximp_vTipoDocImpresion", ximp_iActuacionID));
                        cmd.Parameters.Add(new SqlParameter("@ximp_sUsuarioCreacion", ximp_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ximp_vIPCreacion", ximp_vIPCreacion));
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch
            {
                throw;
            }
            
        }
    }
}
