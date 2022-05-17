using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SUNARP.BE;

namespace SUNARP.Registro.Inscripcion.DA
{
    public class MaestroOficinasMantenimientoDA
    {
        public SU_MAESTRO_OFICINAS insertar(SU_MAESTRO_OFICINAS oficina)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_MAESTRO_OFICINAS_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOZONA", oficina.ofic_cCodigoZona));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOOFICINA", oficina.ofic_cCodigoOficina));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_VDESCRIPCION", oficina.ofic_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_SUSUARIO_CREACION", oficina.ofic_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_VIP_CREACION", oficina.ofic_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", oficina.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@P_OFIC_IOFICINAID", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        oficina.ofic_iOficinaId = Convert.ToInt16(lReturn.Value);
                        oficina.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                oficina.Error = true;
                oficina.Message = exec.Message.ToString();
            }
            return oficina;
        }

        public SU_MAESTRO_OFICINAS actualizar(SU_MAESTRO_OFICINAS oficina)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_SUNARP.USP_SU_MAESTRO_OFICINAS_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_IOFICINAID", oficina.ofic_iOficinaId));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOZONA", oficina.ofic_cCodigoZona));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_CCODIGOOFICINA", oficina.ofic_cCodigoOficina));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_VDESCRIPCION", oficina.ofic_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_SESTADO", oficina.ofic_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_SUSUARIO_MODIFICACION", oficina.ofic_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_OFIC_VIP_MODIFICACION", oficina.ofic_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINA_CONSULARID", oficina.OficinaConsultar));
                        #endregion


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        oficina.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                oficina.Error = true;
                oficina.Message = exec.Message.ToString();
            }
            return oficina;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
