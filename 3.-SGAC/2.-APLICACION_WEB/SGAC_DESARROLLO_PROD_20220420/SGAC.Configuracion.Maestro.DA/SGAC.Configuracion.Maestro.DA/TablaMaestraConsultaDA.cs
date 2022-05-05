using System;
using System.Data; 
using System.Configuration;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Maestro.DA 
{
    using System.Reflection;

    public class TablaMaestraConsultaDA
    {
        ~TablaMaestraConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(int IntTablaMaestraId,
                                   string StrCurrentPage,
                                   int IntPageSize,
                                   ref int IntTotalCount,
                                   ref int IntTotalPages,
                                   string strEstado="A")
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_TABLA_MAESTRA_CONSULTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (IntTablaMaestraId == 0)
                            cmd.Parameters.Add(new SqlParameter("@tama_ITablaId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@tama_ITablaId", IntTablaMaestraId));
                        

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_CESTADO", strEstado));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                throw exec;
            }

            return obj_Resultado;
        }
       
        public int Existe(int IntTablaId, int IntOperacion, int IntId, string StrCodigo)
        {
            int Rspta = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_TABLA_MAESTRA_EXISTE_CODIGO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sTablaId", IntTablaId));
                        cmd.Parameters.Add(new SqlParameter("@sOperacion", IntOperacion));
                        cmd.Parameters.Add(new SqlParameter("@sId", IntId));
                        cmd.Parameters.Add(new SqlParameter("@vCodigo", StrCodigo));

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

        public int ObtenerMonedaId(int intOficinaConsularId)
        {
            DataTable obj_Resultado = new DataTable();
            int intMonedId = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_MONEDA_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", intOficinaConsularId));                        

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];

                            if (obj_Resultado.Rows.Count > 0)
                            {
                                intMonedId = Convert.ToInt32(obj_Resultado.Rows[0]["mone_sMonedaId"].ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                intMonedId = -1;
                throw exec;
            }

            return intMonedId;
        }

        public SGAC.BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD OBTENER_DOCUMENTO_IDENTIDAD(SGAC.BE.MRE.SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD documento, Int16 doid_bCui = 0)
        {

            using (SqlConnection cnx = new SqlConnection(conexion()))
            {
                using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_OBTENER", cnx))
                {
                    #region Pasando Parametros
                    if (documento.doid_sTipoDocumentoIdentidadId != 0) cmd.Parameters.Add("@doid_sTipoDocumentoIdentidadId", documento.doid_sTipoDocumentoIdentidadId);
                    if (documento.doid_vDescripcionCorta != null) cmd.Parameters.Add("@doid_vDescripcionCorta", documento.doid_vDescripcionCorta);
                    if (documento.doid_vDescripcionLarga != null) cmd.Parameters.Add("@doid_vDescripcionLarga", documento.doid_vDescripcionLarga);
                    cmd.Parameters.Add("@doid_bCui", doid_bCui);    
                    #endregion

                    cmd.CommandType = CommandType.StoredProcedure;
                    cnx.Open();

                    using (SqlDataReader loReader = cmd.ExecuteReader())
                    {
                        while (loReader.Read())
                        {
                            for (int col = 0; col <= loReader.FieldCount - 1; col++)
                            {
                                if (loReader[col].GetType().ToString() != "System.DBNull")
                                {
                                    PropertyInfo pInfo = (PropertyInfo)documento.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                    if (pInfo != null) pInfo.SetValue(documento, loReader[col], null);
                                }
                            }
                        }
                    }
                }
            }
            return documento;
        }

        public DataTable ConsultarMargenesDocumento(Int16 mado_sOficinaConsular,
                                   Int16 mado_sTipDocImpresion)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_MARGENESDOCUMENTO_BUSCAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_mado_sOficinaConsular", mado_sOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sTipDocImpresion", mado_sTipDocImpresion));

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                throw exec;
            }

            return obj_Resultado;
        }


        public DataTable ObtenerMargenesDocumento(long mado_iCorrelativo)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_MARGENESDOCUMENTO_OBTENER", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_mado_iCorrelativo", mado_iCorrelativo));

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                throw exec;
            }

            return obj_Resultado;
        }

        public DataTable ConsultarDocumentoIdentidad_MRE(Int16 intTipoDocumentoId, string strDescCorta,
                                    string strEstado, Int16 IntPageSize, Int16 intPageActual,
                                    string strContar, ref Int16 IntTotalPages)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_DOID_STIPODOCUMENTOIDENTIDADID", intTipoDocumentoId));
                        cmd.Parameters.Add(new SqlParameter("@P_DOID_VDESCRIPCIONCORTA", strDescCorta));
                        cmd.Parameters.Add(new SqlParameter("@P_DOID_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageActual));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }

                        IntTotalPages = Convert.ToInt16(lReturn1.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                throw exec;
            }

            return obj_Resultado;
        }

        public DataTable ConsultarTablasMaestrasCargaInicial()
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_SC_MAESTRO_CONSULTAR_CARGA_INICIAL_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                obj_Resultado = null;
                throw exec;
            }

            return obj_Resultado;
        }
           
    
    }
}

