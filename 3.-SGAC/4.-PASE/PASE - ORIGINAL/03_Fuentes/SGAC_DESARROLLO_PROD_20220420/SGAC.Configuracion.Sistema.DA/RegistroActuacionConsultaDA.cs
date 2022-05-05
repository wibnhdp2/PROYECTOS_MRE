using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SGAC.Configuracion.Sistema.DA
{
    public class RegistroActuacionConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public RegistroActuacionConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable obtener(int intOficinaConsularId, int intPeriodo, int intSeccionId,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_CORRELATIVO_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                            cmd.Parameters.Add(new SqlParameter("@corr_sOficinaConsularId", intOficinaConsularId));
                        if (intPeriodo != 0)
                            cmd.Parameters.Add(new SqlParameter("@corr_sPeriodo", intPeriodo));
                        if (intSeccionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", intSeccionId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));
                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.BigInt);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.BigInt);
                        parTotalPagina.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dt = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(parTotalRegistros.Value);
                        intTotalPaginas = Convert.ToInt32(parTotalPagina.Value);
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return dt;
        }

        public DataTable obtenerCorrelativoTarifa(int intOficinaConsularId, int intPeriodo, int intSeccionId,
            int intPaginaActual, int intPaginaCantidad, 
            ref int intTotalRegistros, ref int intTotalPaginas, ref int intCorrelativo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_TARIFARIO_CORRELATIVO_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                            cmd.Parameters.Add(new SqlParameter("@corr_sOficinaConsularId", intOficinaConsularId));
                        if (intPeriodo != 0)
                            cmd.Parameters.Add(new SqlParameter("@corr_sPeriodo", intPeriodo));
                        if (intSeccionId != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", intSeccionId));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", intPaginaActual));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", intPaginaCantidad));

                        SqlParameter parTotalRegistros = cmd.Parameters.Add("@ITotalRegistros", SqlDbType.Int);
                        parTotalRegistros.Direction = ParameterDirection.Output;
                        SqlParameter parTotalPagina = cmd.Parameters.Add("@ITotalPaginas", SqlDbType.Int);
                        parTotalPagina.Direction = ParameterDirection.Output;
                        SqlParameter parCorrelativo = cmd.Parameters.Add("@corr_ICorrelativo", SqlDbType.Int);
                        parCorrelativo.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dt = dsObjeto.Tables[0];
                        }

                        intTotalRegistros = Convert.ToInt32(parTotalRegistros.Value);
                        intTotalPaginas = Convert.ToInt32(parTotalPagina.Value);
                        intCorrelativo = Convert.ToInt32(parCorrelativo.Value);
                    }
                }
            }
            catch (SqlException ex)
            {

            }
            return dt;
        }

        public BE.MRE.RE_CORRELATIVO obtener(BE.MRE.RE_CORRELATIVO objCorrelativo)
        {

            return objCorrelativo;
        }
    }
}
