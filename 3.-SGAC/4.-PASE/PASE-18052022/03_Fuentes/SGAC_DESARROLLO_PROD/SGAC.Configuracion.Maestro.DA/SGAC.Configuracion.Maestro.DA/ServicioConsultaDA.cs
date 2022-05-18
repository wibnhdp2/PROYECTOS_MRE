using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SGAC.Configuracion.Maestro.DA
{
    public class ServicioConsultaDA
    {
        ~ServicioConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable Consulta(int intTipo)
        {
            DataTable obj_Resultado = new DataTable();

            string strTipo = "";

            if (intTipo == 1) strTipo = "PALH";
            if (intTipo == 2) strTipo = "PAH";
            if (intTipo == 3) strTipo = "OTROS";

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_SERVICIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_vGrupo", strTipo));

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

        public DataTable Consulta()
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_SERVICIO_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@serv_vGrupo", DBNull.Value));

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


        public DataTable ConsultarGeneral(string strConsulta)
        {
            DataTable obj_Resultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sqlScript", strConsulta));

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
