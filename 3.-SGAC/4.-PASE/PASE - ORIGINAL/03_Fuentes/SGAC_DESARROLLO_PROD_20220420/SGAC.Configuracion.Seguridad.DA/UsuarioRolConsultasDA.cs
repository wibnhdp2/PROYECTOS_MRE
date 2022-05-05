using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class UsuarioRolConsultasDA
    {
        ~UsuarioRolConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Obtener()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR", cn))
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

        public DataTable ObtenerPorEmpresa(int iEmpresaId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_EMPRESA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@iEmpresaId", iEmpresaId));

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

        public DataSet ObtenerConfiguracion(string strAlias)
        {
            DataSet objResultado = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_EMPRESA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usua_vAlias", strAlias));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto;
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

        public int VerificaIntegridadReferencial(int IntRolId)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_SE_USUARIOROL_VALIDAR_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sRolConfiguracionId", IntRolId));                       

                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Rspta = Convert.ToInt32(lReturn.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                Rspta = -1;
                throw exec;
            }

            return Rspta;
        }

        public DataTable Validar(string StrAlias, int IntOficinaConsular)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_VALIDAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vAlias", StrAlias));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", IntOficinaConsular));

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
