using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
 
namespace SGAC.Configuracion.Sistema.DA
{
    public class TarifarioRequisitoConsultasDA
    {
        ~TarifarioRequisitoConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Obtener(int iTarifarioId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIOREQUISITO_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tare_sTarifarioId", iTarifarioId));                      

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }                        
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

    }
}
