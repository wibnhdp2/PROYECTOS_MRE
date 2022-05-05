using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.BE.MRE;
//--------------------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase de acceso a datos de la ficha registral histórica
//--------------------------------------------------------------------

namespace SGAC.Registro.Actuacion.DA
{
    public class FichaRegistralHistoricoDA
    {

        public DataTable Consultar(long intFichaRegistralId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRALHISTORICO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_FIHI_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
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

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
