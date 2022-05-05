using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Cliente.Colas.DA
{
    public class VentanillaServicioConsultaDA
    {
        ~VentanillaServicioConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int vedeSVentanillaId,
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
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vede_sVentanillaId", vedeSVentanillaId));

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
