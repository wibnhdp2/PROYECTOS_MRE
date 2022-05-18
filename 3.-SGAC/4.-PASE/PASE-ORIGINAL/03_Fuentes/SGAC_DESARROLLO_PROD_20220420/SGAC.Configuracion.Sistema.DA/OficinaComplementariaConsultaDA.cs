using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SGAC.Configuracion.Sistema.DA
{
    public class OficinaComplementariaConsultaDA
    {
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int ofcmsTipoId)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACOMPLEMENTARIA_OBTENER", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofcm_sTipoId", ofcmsTipoId));

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
