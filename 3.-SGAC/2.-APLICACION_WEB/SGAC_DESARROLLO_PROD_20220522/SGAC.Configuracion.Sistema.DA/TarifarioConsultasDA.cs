using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using SGAC.Accesorios;
 
namespace SGAC.Configuracion.Sistema.DA
{
    public class TarifarioConsultasDA
    {
        ~TarifarioConsultasDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int IntSeccionId,
                                   string StrDescripcionCorta,
                                   int IntEstado,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   bool bDetalle = false)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", IntSeccionId));
                        if (StrDescripcionCorta.Length == 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vDescripcionCorta", DBNull.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@tari_vDescripcionCorta", StrDescripcionCorta));
                        }
                        cmd.Parameters.Add(new SqlParameter("@tari_sEstadoId", IntEstado));

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@bDetalle", bDetalle));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public DataTable Obtener(Int16 intTarifaId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_OBTENER_HISTORICO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sTarifarioId", intTarifaId));

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

        public int Existe(int IntTarifarioId, string strNumero, string StrLetra, int IntOperacion)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_EXISTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sTarifarioId", IntTarifarioId));
                        if (strNumero != string.Empty)
                            cmd.Parameters.Add(new SqlParameter("@tari_sNumero", strNumero));                        
                        cmd.Parameters.Add(new SqlParameter("@tari_vLetra", StrLetra));
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

        public BE.MRE.SI_TARIFARIO ObtenerTarifaPorId(int intTarifaId)
        {
            BE.MRE.SI_TARIFARIO objTarifario = new BE.MRE.SI_TARIFARIO();
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_CONSULTAR", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@tari_sSeccionId",SqlDbType.SmallInt).Value = 0;
                        cmd.Parameters.Add("@tari_sEstadoId", SqlDbType.SmallInt).Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@tari_sTarifarioId", intTarifaId));
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = 10;
                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                for (int col = 0; col <= loReader.FieldCount - 1; col++)
                                {
                                    if (loReader[col].GetType().ToString() != "System.DBNull")
                                    {
                                        PropertyInfo pInfo = (PropertyInfo)objTarifario.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null) pInfo.SetValue(objTarifario, loReader[col], null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                objTarifario.Error = true;
                objTarifario.Message = ex.Message;
            }            
            return objTarifario;
        }

        public DataTable Obtener(int IntSeccionId, int intNumero, string strLetra,
                                   string StrDescripcion,
                                   int intCurrentPage, int IntPageSize,
                                   ref int IntTotalCount,  ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tari_sSeccionId", IntSeccionId));
                        if (intNumero != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_sNumero", intNumero));
                        if (strLetra.Length != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_vLetra", StrDescripcion));
                        if (StrDescripcion.Length != 0)
                            cmd.Parameters.Add(new SqlParameter("@tari_vDescripcion", StrDescripcion));

                        cmd.Parameters.Add(new SqlParameter("@tari_sEstadoId", (int) Enumerador.enmTarifarioEstado.ACTIVO));
                        cmd.Parameters.Add("@bDetalle", SqlDbType.Bit).Value = true;

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", intCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }


        public DataTable ConsultarTarifarioCargaInicial(Int16 intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_CONSULTAR_CARGA_INICIAL_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@ofco_iOficinaConsularId", intOficinaConsularId));

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

        public DataTable ConsultarTarifarioTotalCargaInicial()
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_TARIFARIO_CONSULTAR_TOTAL_CARGA_INICIAL_MRE", cn))
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

    }
}
