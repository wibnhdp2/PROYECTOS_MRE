using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace SGAC.Configuracion.Seguridad.DA
{

    public class SistemaConsultaDA
    {
        ~SistemaConsultaDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        public DataTable ConsultarSistemasCargaInicial()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_ADM.USP_SE_SISTEMA_CONSULTAR_CARGA_INICIAL_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


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
