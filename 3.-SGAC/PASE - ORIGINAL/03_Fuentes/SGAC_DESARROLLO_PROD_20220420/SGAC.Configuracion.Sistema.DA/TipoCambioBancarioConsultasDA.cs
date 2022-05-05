using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Sistema.DA
{
    public class TipoCambioBancarioConsultasDA
    {
        ~TipoCambioBancarioConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerPorFecha(int intOficinaConsularId,
                                         DateTime datFechaInicio,
                                         DateTime datFechaFin,
                                         int intPaginaActual,
                                         int intPaginaCantidad,
                                         ref int intTotalRegistros,
                                         ref int intTotalPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_BANCARIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFechaInicio", datFechaInicio.ToString("yyyy-MM-dd 00:00:00")));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFechaFin", datFechaFin.ToString("yyyy-MM-dd 23:59:59")));
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

        public DataTable ObtenerPromedioTipoCambio(int intOficinaConsularId,
                                                   DateTime datFechaActual,
                                                   DateTime datFechaAnterior)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TIPOCAMBIO_OBTENER_PROMEDIO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFechaActual", datFechaActual));
                        cmd.Parameters.Add(new SqlParameter("@tiba_dFechaAnterior", datFechaAnterior));

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
