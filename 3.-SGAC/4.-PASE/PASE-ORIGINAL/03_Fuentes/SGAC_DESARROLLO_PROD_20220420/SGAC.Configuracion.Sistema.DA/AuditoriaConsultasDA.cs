using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Sistema.DA
{
    public class AuditoriaConsultasDA
    {
        ~AuditoriaConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int intPaginaActual,
                                               int intPaginaCantidad,
                                               ref int intTotalRegistros,
                                               ref int intTotalPaginas,
                                               int intOficinaConsularId,
                                               int intUsuarioId,
                                               int intOperacionId,
                                               int intResultadoId,
                                               int intFormularioId,
                                               DateTime datFechaInicio,
                                               DateTime datFechaFin)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_AUDITORIA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add(new SqlParameter("@audi_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@audi_sUsuarioId", intUsuarioId));
                        cmd.Parameters.Add(new SqlParameter("@audi_sOperacionId", intOperacionId));
                        cmd.Parameters.Add(new SqlParameter("@audi_sResultadoId", intResultadoId));
                        cmd.Parameters.Add(new SqlParameter("@audi_sFormularioId", intFormularioId));
                        cmd.Parameters.Add(new SqlParameter("@audi_dFechaInicio", datFechaInicio.ToString("yyyy-MM-dd 00:00:00")));
                        cmd.Parameters.Add(new SqlParameter("@audi_dFechaFin", datFechaFin));                        

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(lReturn1.Value);
                        intTotalPaginas = Convert.ToInt32(lReturn2.Value);
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

