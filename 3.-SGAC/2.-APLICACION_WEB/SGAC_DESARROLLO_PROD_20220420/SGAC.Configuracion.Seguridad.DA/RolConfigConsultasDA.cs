using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class RolConfigConsultasDA
    {
        ~RolConfigConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerPorFiltro(int intPaginaActual,
                                          int intPaginaCantidad,
                                          ref int intTotalRegistros,
                                          ref int intTotalPaginas,
                                          int intAplicacionId,
                                          string strNombre,
                                          string strEstado,
                                          string strAuditoria)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_CONSULTAR_POR_FILTRO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roco_sAplicacionId", intAplicacionId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vNombre", strNombre));
                        cmd.Parameters.Add(new SqlParameter("@roco_cEstado", strEstado));                        

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

        public DataTable Listar(int intAplicacionId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_CONSULTAR_LISTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roco_sAplicacionId", intAplicacionId));

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

        public DataTable ObtenerPorUsuario(int IUsuarioId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_CONSULTAR_POR_USUARIO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", IUsuarioId));

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
