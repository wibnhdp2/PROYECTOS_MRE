using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class MaestroOficinasConsultaDA
    {
        private string strConnectionName = string.Empty;

        public MaestroOficinasConsultaDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~MaestroOficinasConsultaDA()
        {
            GC.Collect();
        }
        public DataTable MaestroOficinasConsulta(short intOficinaId, 
            string strCodigoZona, string strCodigoOficina, string strEstado, string strFiltrarOficinas,
            int intPageSize, string StrCurrentPage, string strContar, ref int intTotalPages)
        {
            DataTable objResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_MAESTRO_OFICINAS_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_IOFICINAID", intOficinaId));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOZONA", strCodigoZona));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOOFICINA", strCodigoOficina));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_SESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_FILTRAR_OFICINAS", strFiltrarOficinas));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        intTotalPages = Convert.ToInt32(lReturn1.Value);
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
