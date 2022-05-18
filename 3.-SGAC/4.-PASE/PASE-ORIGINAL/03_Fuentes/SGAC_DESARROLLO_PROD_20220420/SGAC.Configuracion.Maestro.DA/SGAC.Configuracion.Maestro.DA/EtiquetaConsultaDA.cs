using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace SGAC.Configuracion.Maestro.DA
{
    public class EtiquetaConsultaDA
    {
        ~EtiquetaConsultaDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consultar(Int64 intEtiquetaID,
                                   Int16 intPlantillaID,
                                   string strEtiqueta,
                                   string strEstado,
                                   int IntPageSize,
                                   int IntCurrentPage,
                                   string strContar,
                                   ref int IntTotalPages,                      
                                   ref int IntTotalCount)                                  
                                  
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ETIQUETA_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_IETIQUETAID", intEtiquetaID));
                        cmd.Parameters.Add(new SqlParameter("@P_SPLANTILLAID", intPlantillaID));
                        cmd.Parameters.Add(new SqlParameter("@P_VETIQUETA", strEtiqueta));
                        cmd.Parameters.Add(new SqlParameter("@P_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", IntPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", IntCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));
                        
                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                        }

                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);                        
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
