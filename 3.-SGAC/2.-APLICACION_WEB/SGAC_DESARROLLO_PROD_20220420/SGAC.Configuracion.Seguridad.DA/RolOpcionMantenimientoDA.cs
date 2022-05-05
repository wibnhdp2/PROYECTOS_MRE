using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace SGAC.Configuracion.Seguridad.DA
{
    public class RolOpcionMantenimientoDA
    {
        ~RolOpcionMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(BE.SE_ROLOPCION pobjBE, ref int IntRolOpcionId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLOPCION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roop_sFormularioId", pobjBE.roop_sFormularioId));
                        cmd.Parameters.Add(new SqlParameter("@roop_vAcciones", pobjBE.roop_vAcciones));
                        cmd.Parameters.Add(new SqlParameter("@roop_cEstado", pobjBE.roop_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@roop_sUsuarioCreacion", pobjBE.roop_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@roop_vIPCreacion", pobjBE.roop_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roop_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@roop_sRolOpcionId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntRolOpcionId = Convert.ToInt32(lReturn.Value);

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Update(BE.SE_ROLOPCION pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLOPCION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roop_sRolOpcionId", pobjBE.roop_sRolOpcionId));
                        cmd.Parameters.Add(new SqlParameter("@roop_sFormularioId", pobjBE.roop_sFormularioId));
                        cmd.Parameters.Add(new SqlParameter("@roop_vAcciones", pobjBE.roop_vAcciones));                        
                        cmd.Parameters.Add(new SqlParameter("@roop_sUsuarioModificacion", pobjBE.roop_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roop_vIPModificacion", pobjBE.roop_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roop_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roop_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }

        public void Delete(BE.SE_ROLOPCION pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLOPCION_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roop_sRolOpcionId", pobjBE.roop_sRolOpcionId));                       
                        cmd.Parameters.Add(new SqlParameter("@roop_sUsuarioModificacion", pobjBE.roop_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roop_vIPModificacion", pobjBE.roop_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roop_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roop_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Error = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
        }
    }    
}
