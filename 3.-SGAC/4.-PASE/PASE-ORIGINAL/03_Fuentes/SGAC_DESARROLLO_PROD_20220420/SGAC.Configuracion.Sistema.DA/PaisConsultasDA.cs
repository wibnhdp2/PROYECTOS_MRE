using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//--------------------------------------------------
//Fecha: 15/03/2017
//Autor: Miguel Márquez Beltrán
//Objetivo: Consultar la tabla PS_SISTEMA.SI_Pais
//--------------------------------------------------

namespace SGAC.Configuracion.Sistema.DA
{
    public class PaisConsultasDA
    {
        ~PaisConsultasDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar_Pais(int intPaisId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalCount,
            ref int IntTotalPages, short sContinenteId = 0, string strNombrePais = "", short sIdiomaId = 0)
        {
            DataTable dtResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_PAIS_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_PAIS_SPAISID", intPaisId));
                        cmd.Parameters.Add(new SqlParameter("@P_PAIS_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", StrCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));
                        cmd.Parameters.Add(new SqlParameter("@P_PAIS_SCONTINENTEID", sContinenteId));
                        cmd.Parameters.Add(new SqlParameter("@P_PAIS_VNOMBRE", strNombrePais));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new SqlParameter("@P_SIDIOMA", sIdiomaId));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);
                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
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

    }
}
