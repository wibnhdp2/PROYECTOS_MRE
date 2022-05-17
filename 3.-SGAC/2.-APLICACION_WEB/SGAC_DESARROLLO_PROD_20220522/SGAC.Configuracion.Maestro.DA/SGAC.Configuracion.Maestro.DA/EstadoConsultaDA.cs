using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Maestro.DA
{
    public class EstadoConsultaDA
    {
        ~EstadoConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ConsultaGrupoMRE(ref Int16 intCantidadPaginas, Int16 intEstadoId =0, string strDesCorta="", string strEstado="A", 
            Int16 intPageSize=10000, Int16 intNumeroPagina=1, string strContar="N", string EstadoGrupo="")            
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ESTADO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_ESTA_SESTADOID", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@P_ESTA_VDESCRIPCIONCORTA", strDesCorta));
                        cmd.Parameters.Add(new SqlParameter("@P_ESTA_CESTADO", strEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intNumeroPagina));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));                        
                        cmd.Parameters.Add(new SqlParameter("@P_VGRUPO", EstadoGrupo));

                        cmd.Parameters.Add(new SqlParameter("@P_IPAGECOUNT", intCantidadPaginas)).Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                            obj_Resultado = ds_Objeto.Tables[0];
                            intCantidadPaginas = Convert.ToInt16(adap.SelectCommand.Parameters["@P_IPAGECOUNT"].Value.ToString());
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

        public DataTable ConsultaGrupo(Enumerador.enmEstadoGrupo EstadoGrupo)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ESTADO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@esta_vGrupo", EstadoGrupo));

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

        public DataTable ConsultaGrupo(string EstadoGrupo)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_ESTADO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@esta_vGrupo", EstadoGrupo));

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
