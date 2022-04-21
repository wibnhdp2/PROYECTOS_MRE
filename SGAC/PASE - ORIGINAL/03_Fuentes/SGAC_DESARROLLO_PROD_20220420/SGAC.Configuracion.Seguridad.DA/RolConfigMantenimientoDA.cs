using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SGAC.Configuracion.Seguridad.DA
{
    public class RolConfigMantenimientoDA
    {
        ~RolConfigMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public void Insert(BE.SE_ROLCONFIGURACION pobjBE, ref int IntRolConfiguracionId, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                      

                        cmd.Parameters.Add(new SqlParameter("@roco_sAplicacionId", pobjBE.roco_sAplicacionId));
                        cmd.Parameters.Add(new SqlParameter("@roco_sRolTipoId", pobjBE.roco_sRolTipoId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vRolOpcion", pobjBE.roco_vRolOpcion));
                        cmd.Parameters.Add(new SqlParameter("@roco_vNombre", pobjBE.roco_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@roco_cHorario", pobjBE.roco_cHorario));
                        cmd.Parameters.Add(new SqlParameter("@roco_sUsuarioCreacion", pobjBE.roco_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_vIPCreacion", pobjBE.roco_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vHostName", Util.ObtenerHostName()));

                        SqlParameter lReturn = cmd.Parameters.Add("@roco_sRolConfiguracionId", SqlDbType.SmallInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        IntRolConfiguracionId = Convert.ToInt32(lReturn.Value);

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
        public void Update(BE.SE_ROLCONFIGURACION pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roco_sRolConfiguracionId", pobjBE.roco_sRolConfiguracionId));
                        cmd.Parameters.Add(new SqlParameter("@roco_sAplicacionId", pobjBE.roco_sAplicacionId));
                        cmd.Parameters.Add(new SqlParameter("@roco_sRolTipoId", pobjBE.roco_sRolTipoId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vRolOpcion", pobjBE.roco_vRolOpcion));
                        cmd.Parameters.Add(new SqlParameter("@roco_vNombre", pobjBE.roco_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@roco_cHorario", pobjBE.roco_cHorario));
                        cmd.Parameters.Add(new SqlParameter("@roco_sUsuarioModificacion", pobjBE.roco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_vIPModificacion", pobjBE.roco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@roco_cEstado", pobjBE.roco_cEstado)); 
                        

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

        public void Delete(BE.SE_ROLCONFIGURACION pobjBE, ref bool Error)
        {
            Error = true;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_ROLCONFIGURACION_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@roco_sRolConfiguracionId", pobjBE.roco_sRolConfiguracionId));                       
                        cmd.Parameters.Add(new SqlParameter("@roco_sUsuarioModificacion", pobjBE.roco_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_vIPModificacion", pobjBE.roco_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@roco_sOficinaConsularId", pobjBE.OficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@roco_vHostName", Util.ObtenerHostName())); 

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
