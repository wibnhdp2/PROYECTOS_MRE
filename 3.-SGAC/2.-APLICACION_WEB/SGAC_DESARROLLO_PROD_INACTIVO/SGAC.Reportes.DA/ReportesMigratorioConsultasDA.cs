using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SGAC.Reportes.DA
{
    public class ReportesMigratorioConsultasDA
    {
        private string strConnectionName = string.Empty;

        public ReportesMigratorioConsultasDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerReportesMigratorio(int i_TipoReporte, string sPasaporte_Inicial, string sPasaporte_Final,
            int iMision_IDV, int iEstado_Pasaporte_IDV, string sAnio, int iMision_PG, string sNumero_Pasaporte,
            bool bExpediente_IMG, int iTipo_Documento, string sNumero_Documento, int iEstado_Pasaporte_PG, string sNumero_Expediente,
            string sApellido_Paterno, string sApellido_Materno, string sNombres, DateTime? dFecInicio, DateTime? dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RP_ACTOMIGRATORIO_REPORTES", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Tipo_Reporte", SqlDbType.BigInt).Value = i_TipoReporte;
                        cmd.Parameters.Add("@Numero_Pasaporte_Inicial", SqlDbType.VarChar).Value = sPasaporte_Inicial;
                        cmd.Parameters.Add("@Numero_Pasaporte_Final", SqlDbType.VarChar).Value = sPasaporte_Final;
                        cmd.Parameters.Add("@Mision_IDV", SqlDbType.Int).Value = iMision_IDV;
                        cmd.Parameters.Add("@Estado_Pasaporte_IDV", SqlDbType.Int).Value = iEstado_Pasaporte_IDV;
                        cmd.Parameters.Add("@Anio", SqlDbType.VarChar).Value = sAnio;
                        cmd.Parameters.Add("@Mision_PG", SqlDbType.Int).Value = iMision_PG;
                        cmd.Parameters.Add("@Numero_Pasaporte", SqlDbType.VarChar).Value = sNumero_Pasaporte;
                        cmd.Parameters.Add("@Expediente_Img", SqlDbType.Bit).Value = bExpediente_IMG;
                        cmd.Parameters.Add("@Tipo_Documento", SqlDbType.Int).Value = iTipo_Documento;
                        cmd.Parameters.Add("@Numero_Documento", SqlDbType.VarChar).Value = sNumero_Documento;
                        cmd.Parameters.Add("@Estado_Pasaporte_PG", SqlDbType.Int).Value = iEstado_Pasaporte_PG;
                        cmd.Parameters.Add("@Numero_Expediente", SqlDbType.VarChar).Value = sNumero_Expediente;
                        cmd.Parameters.Add("@Apellido_Paterno", SqlDbType.VarChar).Value = sApellido_Paterno;
                        cmd.Parameters.Add("@Apellido_Materno", SqlDbType.VarChar).Value = sApellido_Materno;
                        cmd.Parameters.Add("@Nombres", SqlDbType.VarChar).Value = sNombres;
                        if (dFecInicio == null)
                            cmd.Parameters.Add("@Fecha_Inicio", SqlDbType.DateTime).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@Fecha_Inicio", SqlDbType.DateTime).Value = dFecInicio;

                        if (dFecFin == null)
                            cmd.Parameters.Add("@Fecha_Fin", SqlDbType.DateTime).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@Fecha_Fin", SqlDbType.DateTime).Value = dFecFin;
                        
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
