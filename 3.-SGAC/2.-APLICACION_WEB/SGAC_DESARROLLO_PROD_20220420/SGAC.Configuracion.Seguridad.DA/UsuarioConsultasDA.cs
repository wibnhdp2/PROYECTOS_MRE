using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class UsuarioConsultasDA
    {
        ~UsuarioConsultasDA()
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
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_LISTA", cn))
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

        public DataTable ObtenerTodo()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_LISTA_TODO", cn))
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

        public DataTable ObtenerPorId(int IUsuarioId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_POR_ID", cn))
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

        public DataTable ObtenerLista(int intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_LISTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sOficinaConsularId", intOficinaConsularId));

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

        public DataTable ObtenerPorFiltros(int intPaginaActual,
                                           int intPaginaCantidad,
                                           ref int intTotalRegistros,
                                           ref int intTotalPaginas,
                                           int intOficinaConsularId,
                                           string strDocumentoNumero,
                                           string strNombres,
                                           string strApellidoPaterno,
                                           string strApellidoMaterno,
                                           string strEstado)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_POR_FILTRO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vDocumentoNumero", strDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@usua_vNombres", strNombres));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoPaterno", strApellidoPaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidoMaterno", strApellidoMaterno));
                        cmd.Parameters.Add(new SqlParameter("@usua_cEstado", strEstado));

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

        public DataTable ObtenerPorFiltros(string strGrupoId,
                                           string strOficinaConsularId,
                                           string strNombres,
                                           string strApellidos,
                                           string strAlias,
                                           string strActivo,
                                           string strPaginaActual,
                                           int intPaginaCantidad,
                                           ref int intCantTotal,
                                           ref int intCantPaginas)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_CONSULTAR_POR_FILTRO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_vGrupoId", strGrupoId));
                        cmd.Parameters.Add(new SqlParameter("@usro_vOficinaConsularId", strOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vNombres", strNombres));
                        cmd.Parameters.Add(new SqlParameter("@usua_vApellidos", strApellidos));
                        cmd.Parameters.Add(new SqlParameter("@usua_vAlias", strAlias));
                        cmd.Parameters.Add(new SqlParameter("@usua_vActivo", strActivo));

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", strPaginaActual));
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

                        intCantTotal = Convert.ToInt32(lReturn1.Value);
                        intCantPaginas = Convert.ToInt32(lReturn2.Value);
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

        public DataTable Autenticar(int intAplicacionId,
                                    string strAlias,
                                    string strHostName,
                                    string strDireccionIP)
        {
            DataTable objResultado = new DataTable();

            try
            {
                

                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_AUTENTICAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_iAplicacionId", intAplicacionId));
                        cmd.Parameters.Add(new SqlParameter("@usua_vAlias", strAlias));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", strHostName));
                        cmd.Parameters.Add(new SqlParameter("@vDireccionIP", strDireccionIP));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            if (dsObjeto.Tables.Count > 0)
                                objResultado = dsObjeto.Tables[0];
                            else
                                objResultado = null;
                            
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

        public DataSet Autorizar(int intAplicacionId, long intUsuarioId)
        {
            DataSet objResultado = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_AUTORIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@usro_sAplicacionId", intAplicacionId));
                        cmd.Parameters.Add(new SqlParameter("@usua_sUsuarioId", intUsuarioId));                        

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

        public string ValidarUsuarioDocumento(string StrNroDocumento, string StrAlias, int IntOperacion)
        {
            string strMensaje = string.Empty;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_DOCUMENTO_VALIDAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vNroDocumento", StrNroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@vAlias", StrAlias));
                        cmd.Parameters.Add(new SqlParameter("@IOperacion", IntOperacion));

                        SqlParameter lReturn = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 500);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = Convert.ToString(lReturn.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                strMensaje = "ERROR - " + exec.Message;
            }

            return strMensaje;
        }
    }
}
