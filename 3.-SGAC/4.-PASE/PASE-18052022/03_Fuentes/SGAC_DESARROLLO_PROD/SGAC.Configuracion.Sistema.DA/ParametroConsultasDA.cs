using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using System.Configuration;
using System.Reflection;
 
namespace SGAC.Configuracion.Sistema.DA 
{
    public class ParametroConsultasDA
    {
        ~ParametroConsultasDA()
        {
            GC.Collect(); 
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
        
        public SI_PARAMETRO Obtener(SI_PARAMETRO parametro){

            try{
                using (SqlConnection cnx = new SqlConnection(this.conexion())){
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_OBTENER", cnx)){
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        if (parametro.para_sParametroId !=0) cmd.Parameters.Add(new SqlParameter("@para_sParametroId", parametro.para_sParametroId));
                        if (parametro.para_vGrupo != null) cmd.Parameters.Add(new SqlParameter("@para_vGrupo", parametro.para_vGrupo));
                        if (parametro.para_vDescripcion != null) cmd.Parameters.Add(new SqlParameter("@para_vDescripcion", parametro.para_vDescripcion));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)parametro.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(parametro, loReader[col], null);
                                        }
                                    }
                                }
                            }
                        }

                        parametro.Error = false;
                    }
                }
            }
            catch (SqlException exec){
                parametro.Error = true;
                parametro.Message = exec.Message.ToString();
            }

            return parametro;
        }

        public DataTable Consultar(string StrGrupo,
                                   string StrEstado,
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
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vGrupo", StrGrupo));
                        cmd.Parameters.Add(new SqlParameter("@cEstado", StrEstado));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
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


        public DataTable ConsultarParametro(   int bPrecarga,
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
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.Parameters.Add(new SqlParameter("@vGrupo", String.Empty));
                        //cmd.Parameters.Add(new SqlParameter("@cEstado", "A"));                        
                        cmd.Parameters.Add(new SqlParameter("@bPrecarga", bPrecarga));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", StrCurrentPage));
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

        public DataTable ConsultarParametroPorValor(string vGrupo,
                                                string vValor,
                                                string vItemInicial = "- SELECCIONAR -", string cEstado = "A",
                                                string vDescripcion = "")
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTAR_POR_VALOR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@para_vGrupo", vGrupo));
                        cmd.Parameters.Add(new SqlParameter("@para_vValor", vValor));
                        cmd.Parameters.Add(new SqlParameter("@para_vItemInicial", vItemInicial));
                        cmd.Parameters.Add(new SqlParameter("@para_cEstado", cEstado));
                        cmd.Parameters.Add(new SqlParameter("@para_vDescripcion", vDescripcion));

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



        public DataTable ConsultarParametroMRE(Int16 intParametroId, string strGrupo, string strDescripcion,
                                               int intPreCarga, string strEstado, int IntPageSize, int IntPageNumber, string strCurrentPage,
                                               string strContar, ref int IntTotalPages,
                                                Int16 intOficinaConsularId, string strTodos)                                                                        
                                          
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_PARA_SPARAMETROID", intParametroId));
                        cmd.Parameters.Add(new SqlParameter("@P_PARA_VGRUPO", strGrupo));
                        cmd.Parameters.Add(new SqlParameter("@P_PARA_VDESCRIPCION", strDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@P_PARA_BPRECARGA", intPreCarga));
                        cmd.Parameters.Add(new SqlParameter("@P_PARA_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", IntPageNumber));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));
                        cmd.Parameters.Add(new SqlParameter("@P_IOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_CTODOS", strTodos));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        IntTotalPages = Convert.ToInt32(lReturn1.Value);                        
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

        public DataTable ConsultarOficinasActivasCargaInicial(int intOficinaConsularId)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PARAMETRO_CONSULTA_OFICINAS_ACTIVAS_CARGA_INICIAL_MRE", cn))
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
        
        //--------------------------    
    }
}
