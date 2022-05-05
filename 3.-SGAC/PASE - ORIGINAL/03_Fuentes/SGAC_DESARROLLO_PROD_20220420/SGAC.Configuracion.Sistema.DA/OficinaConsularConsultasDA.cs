using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
 
namespace SGAC.Configuracion.Sistema.DA 
{
    public class OficinaConsularConsultasDA
    {
        ~OficinaConsularConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int IntCategoriaId,
                                   string StrvNombre,
                                   string StrvContinente,
                                   string StrvPais,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string strEstado,
                                   bool bJefatura = false,
                                   bool bElecciones = false)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sCategoriaId", IntCategoriaId));
                        if (StrvNombre != null)
                        {
                            if (StrvNombre.Length == 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_vNombre", DBNull.Value));
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_vNombre", StrvNombre));
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_vNombre", DBNull.Value));
                        }


                        if (StrvContinente != "0")
                        {
                            if (StrvContinente.Length == 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_sContinente", DBNull.Value));
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_sContinente", StrvContinente));
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sContinente", DBNull.Value));
                        }


                        if (StrvPais != "0")
                        {
                            if (StrvPais.Length == 0)
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_sPais", DBNull.Value));
                            }
                            else
                            {
                                cmd.Parameters.Add(new SqlParameter("@ofco_sPais", StrvPais));
                            }
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ofco_sPais", DBNull.Value));
                        }

                        cmd.Parameters.Add(new SqlParameter("@IPaginaActual", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPaginaCantidad", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@ofco_cEstado", strEstado));


                        //-------------------------------------------------
                        //Fecha: 29/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar los parametros:
                        //        @ofco_bJefaturaFlag y @ofco_bElecciones.
                        //-------------------------------------------------
                        cmd.Parameters.Add(new SqlParameter("@ofco_bJefaturaFlag", bJefatura));
                        cmd.Parameters.Add(new SqlParameter("@ofco_bElecciones", bElecciones));
                        //-------------------------------------------------
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
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ObtenerPorId(int iOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_CONSULTAR_POR_ID", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_iOficinaConsularId", iOficinaConsularId));

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

        public DataTable ObtenerDependientes(int iOficinaConsularId,
                                                         string StrCurrentPage,
                                                         int IntPageSize,
                                                         ref int IntTotalCount,
                                                         ref int IntTotalPages)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_OBTENER_DEPENDIENTES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", iOficinaConsularId));
                        
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
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public int Existe(string strNombre, int IntOffConsularId, int IntOperacion)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_EXISTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vNombre", strNombre));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", IntOffConsularId));
                        cmd.Parameters.Add(new SqlParameter("@IOperacion", IntOperacion));

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

        public DataTable ListarFuncionarios(object intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_FUNCIONARIO_ESCALAFON_CONSULTAR_X_OFICINA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (intOficinaConsularId != null)
                            cmd.Parameters.Add(new SqlParameter("@sOficinaConsular", intOficinaConsularId.ToString()));
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

        public DataTable ListaCodigoLocal(string strSiglas, string strCodigoLocal)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_CONSULTAR_CODIGO_LOCAL_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_OFCO_VSIGLAS", strSiglas));
                        cmd.Parameters.Add(new SqlParameter("@P_OFCO_VCODIGOLOCAL", strCodigoLocal));
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

        public DataTable ConsultarMonedas(Int16 IntOficinaConsular)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINAMONEDA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsular", IntOficinaConsular));
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

        public DataTable ConsultarOficinasConsularesCargaInicial()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINACONSULAR_CONSULTAR_CARGA_INICIAL_MRE", cn))
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

        public string OficinaEsActiva(short intOficinaConsularId)
        {
            string strActiva = "N";

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTA_OFICINA_ES_ACTIVA_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_IOFICINACONSULARID", intOficinaConsularId));

                        SqlParameter lReturn = cmd.Parameters.Add("@P_CES_ACTIVA", SqlDbType.Char, 1);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strActiva = lReturn.Value.ToString();
                    }
                }
            }
            catch (SqlException exec)
            {
                strActiva = "N";
                throw exec;
            }

            return strActiva;
        }
//-------------------------------------------------------------
    }
}
