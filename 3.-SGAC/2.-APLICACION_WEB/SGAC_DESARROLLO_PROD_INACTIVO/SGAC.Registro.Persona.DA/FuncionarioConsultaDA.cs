using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Registro.Persona.DA
{
    public class FuncionarioConsultaDA
    {
        private string strConnectionName = string.Empty;

        public FuncionarioConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~FuncionarioConsultaDA()
        {
            GC.Collect();
        }

        public DataTable Funcionario_Consultar(int sOficinaConsularId, int IfuncionarioId)
        {
            using (SqlConnection cn = new SqlConnection(strConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_FUNCIONARIO_ESCALAFON_CONSULTAR", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@ocfu_sOficinaConsularId", SqlDbType.SmallInt).Value = sOficinaConsularId;
                    da.SelectCommand.Parameters.Add("@func_IFuncionarioId", SqlDbType.Int).Value = IfuncionarioId;

                    da.Fill(ds, "Funcionario");
                    dt = ds.Tables["Funcionario"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Funcionario_MRE(string vOficinaConsularId)
        {
            using (SqlConnection cn = new SqlConnection(strConnectionName))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();

                try
                {
                    da = new SqlDataAdapter("PN_REGISTRO.USP_RE_OFICINACONSULARFUNCIONARIO_OBTENER", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.Add("@ocfu_sOficinaConsularId", SqlDbType.VarChar).Value = vOficinaConsularId;

                    da.Fill(ds, "Funcionario");
                    dt = ds.Tables["Funcionario"];

                    return dt;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DataTable Funcionario_Listar(string vNroDocumento,
                                            string vPrimerApellido,
                                            string vSegundoApellido,
                                            string vNombre,
                                            string StrCurrentPage,
                                            int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
                                                
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_FUNCIONARIO_ESCALAFON_CONSULTAR_X_FILTRO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@vNroDocumento", vNroDocumento));
                        cmd.Parameters.Add(new SqlParameter("@vPrimerApellido", vPrimerApellido));
                        cmd.Parameters.Add(new SqlParameter("@vSegundoApellido", vSegundoApellido));
                        cmd.Parameters.Add(new SqlParameter("@vNombre", vNombre));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", IntPageSize));

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

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
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