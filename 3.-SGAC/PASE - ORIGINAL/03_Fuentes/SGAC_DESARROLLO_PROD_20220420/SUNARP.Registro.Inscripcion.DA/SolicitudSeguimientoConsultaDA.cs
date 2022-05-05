using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SUNARP.Registro.Inscripcion.DA
{
    public class SolicitudSeguimientoConsultaDA
    {
        private string strConnectionName = string.Empty;

        public SolicitudSeguimientoConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        ~SolicitudSeguimientoConsultaDA()
        {
            GC.Collect();
        }

        public DataTable SeguimientoConsulta(Int64 iSolicitudId)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_SOLICITUD_INSCRIPCION_SEGUIMIENTO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_SISE_ISOLICITUDINSCRIPCIONID", iSolicitudId));

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
            catch (Exception ex)
            {
                objResultado = null;
                throw ex;
            }
            return objResultado;
        }
    }
}
